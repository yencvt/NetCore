using common.Entities;
using common.Services.Interfaces;
using MongoDB.Driver;

namespace common.Services.Implementations
{
    public class ProductService : IProductService
    {
        private readonly IMongoCollection<ProductEntity> _products;
        private readonly IUserService _userService;

        public ProductService(IMongoDatabase database, IUserService userService)
        {
            _products = database.GetCollection<ProductEntity>("products");
            _userService = userService;
        }

        public async Task<List<ProductEntity>> GetAllProducts()
        {
            return await _products.Find(_ => true).ToListAsync();
        }

        public async Task<ProductEntity> GetProductById(string id)
        {
            return await _products.Find(p => p.Id == id).FirstOrDefaultAsync();
        }

        public async Task CreateProduct(ProductEntity product)
        {
            product.CreatedBy = _userService.GetCurrentUserId();
            product.UpdatedBy = product.CreatedBy;
            product.CreatedAt = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            product.UpdatedAt = product.CreatedAt;

            await _products.InsertOneAsync(product);
        }

        public async Task UpdateProduct(string id, ProductEntity product)
        {
            product.UpdatedBy = _userService.GetCurrentUserId();
            product.UpdatedAt = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            await _products.ReplaceOneAsync(p => p.Id == id, product);
        }

        public async Task DeleteProduct(string id)
        {
            await _products.DeleteOneAsync(p => p.Id == id);
        }
    }
}
