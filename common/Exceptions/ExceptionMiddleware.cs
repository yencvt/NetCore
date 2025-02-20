using System.Text.Json;
using common.Constants;
using common.Models.Base;

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

        public async Task InvokeAsync(HttpContext context)
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
            int statusEx = 500;
            string codeEx = "500";
            string messageEx;

            if (exception is CommonException commonEx)
            {
                statusEx = commonEx.Status;
                codeEx = commonEx.Code;
                messageEx = exception.Message;
            }
            else
            {
                statusEx = exception switch
                {
                    UnauthorizedAccessException => Messages.Unauthorized.Status,
                    _ => Messages.InternalServerError.Status
                };
                messageEx = exception.Message;
            }

            var response = new ResponseBase<object>(codeEx?.ToString() ?? "500", messageEx, null);
           
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = statusEx;

            return context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }
}
