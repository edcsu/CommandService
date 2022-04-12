using Microsoft.EntityFrameworkCore;
using System.Data.SqlClient;
using System.Net;
using System.Net.Mime;
using System.Text.Json;

namespace CommandService.Core
{
    public class GlobalErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration _config;
        private readonly ILogger<GlobalErrorHandlerMiddleware> _iLogger;

        public GlobalErrorHandlerMiddleware(RequestDelegate next, IConfiguration config, ILogger<GlobalErrorHandlerMiddleware> iLogger)
        {
            _next = next;
            _config = config;
            _iLogger = iLogger;
        }

        public async Task Invoke(HttpContext context /* other scoped dependencies */)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _iLogger.LogError(ex, "An error has been thrown in the HTTP Pipeline");
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {

            var showDetailedErrors = _config.GetValue<bool>("ShowDetailedErrors");
            _iLogger.LogError(exception, "global.error");
            var code = HttpStatusCode.InternalServerError;

            var message = "Unknown error, please contact administrator";
            switch (exception)
            {
                case UnauthorizedAccessException _:
                    code = HttpStatusCode.Unauthorized;
                    break;

                case DbUpdateException _:
                case SqlException _:
                    code = HttpStatusCode.BadRequest;
                    break;

                case NullReferenceException _:
                    code = HttpStatusCode.InternalServerError;
                    message = exception.Message;
                    break;

            }

            var options = new JsonSerializerOptions { WriteIndented = true };
            var result = JsonSerializer.Serialize(new { code, message }, options);

            context.Response.ContentType = MediaTypeNames.Application.Json;
            context.Response.StatusCode = (int)code;
            await context.Response.WriteAsync(result);
        }
    }
}
