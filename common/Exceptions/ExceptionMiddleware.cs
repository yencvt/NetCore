using System;
using System.Net;
using System.Text.Json;

namespace common.Exceptions
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Có lỗi xảy ra!");
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var statusCode = exception switch
            {
                //NotFoundException => (int)HttpStatusCode.NotFound, // 404
                //BadRequestException => (int)HttpStatusCode.BadRequest, // 400
                UnauthorizedAccessException => (int)HttpStatusCode.Unauthorized, // 401
                _ => (int)HttpStatusCode.InternalServerError // 500
            };

            var response = new
            {
                status = statusCode,
                message = exception.Message,
                errorType = exception.GetType().Name
            };

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = statusCode;

            return context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }
}

