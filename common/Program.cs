using System.Text;
using common.Cacher;
using common.Exceptions;
using common.Filters;
using common.Logs;
using common.Models.Configs;
using common.Repositories;
using common.Services.Implementations;
using common.Services.Interfaces;
using common.Utils;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;

// Add services to the container.
builder.Services.AddControllers(options =>
{
    options.Filters.Add<ValidateModelFilter>(); // Đăng ký filter toàn cục
});

//Add Invalid ModelState response 
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.InvalidModelStateResponseFactory = InvalidModelStateResponse.CreateResponse;
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Đọc cấu hình từ appsettings.json
builder.Services.Configure<CacheConfig>(config.GetSection("CacheSettings"));

// Đăng ký MemoryCache nếu bật
if (config.GetValue<bool>("CacheSettings:EnableMemoryCache"))
{
    builder.Services.AddMemoryCache();
}

// Đăng ký RedisCache nếu bật
if (config.GetValue<bool>("CacheSettings:EnableRedisCache"))
{
    builder.Services.AddStackExchangeRedisCache(options =>
    {
        options.Configuration = "localhost:6379"; // Đổi theo Redis của bạn
        options.InstanceName = "SampleInstance";
    });
}

builder.Services.AddSingleton<IMultipleCacheService, MultipleCacheService>();

//Đăng ký product
builder.Services.AddScoped<ProductRepository>();
builder.Services.AddScoped<IProductService, ProductService>();


// Đọc config từ appsettings.json
var mongoSettings = builder.Configuration.GetSection("MongoDB").Get<MongoDBSettings>();

// Đăng ký MongoDB client vào DI container
builder.Services.AddSingleton<IMongoClient>(new MongoClient(mongoSettings.ConnectionString));
builder.Services.AddSingleton<IMongoDatabase>(sp =>
    sp.GetRequiredService<IMongoClient>().GetDatabase(mongoSettings.DatabaseName));

//Cấu hình IHttpContextAccessor sử dụng để lấy tên người dùng
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IUserService, UserService>();


// ✅ Đăng ký JwtService
builder.Services.AddSingleton<JwtService>();

// ✅ Cấu hình xác thực JWT
builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = config["JwtSettings:Issuer"],
            ValidAudience = config["JwtSettings:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(GeneratorUtils.GenerateKeyFromSecret(config["JwtSettings:Secret"], 32)))
        };
    });

builder.Services.AddAuthorization();


// Đăng ký dịch vụ
//builder.Services.AddScoped<IProductService, ProductService>();
//builder.Services.AddScoped<ProductType>();
//builder.Services.AddScoped<ProductQuery>();
//builder.Services.AddScoped<ISchema, ProductSchema>();
// Sử dụng Newtonsoft.Json cho GraphQL
//builder.Services.AddGraphQL(options => options.EnableMetrics = true).AddNewtonsoftJson();  // Thay vì AddSystemTextJson
//builder.Services.AddGraphQL(options => options.EnableMetrics = false).AddSystemTextJson().AddGraphTypes(ServiceLifetime.Scoped);

var app = builder.Build();

// Thêm middleware GraphQL
//app.UseGraphQL<ISchema>();
//app.UseGraphQLPlayground("/graphql", new PlaygroundOptions { GraphQLEndPoint = "/graphql" });  // Đã kích hoạt lại GraphQL Playground
//app.UseGraphQL("/graphql");


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}

// Đăng ký ExceptionMiddleware
app.UseMiddleware<ExceptionMiddleware>();

// Đăng ký RequestLoggingMiddleware
app.UseMiddleware<RequestLoggingMiddleware>();

// ✅ Kích hoạt xác thực & ủy quyền
app.UseAuthentication();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

// Class đọc cấu hình MongoDB
public class MongoDBSettings
{
    public string ConnectionString { get; set; }
    public string DatabaseName { get; set; }
}