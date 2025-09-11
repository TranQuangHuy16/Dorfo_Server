using Dorfo.Application.Exceptions;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json;

namespace Dorfo.API.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
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
                _logger.LogError(ex, "Unhandled exception");
                await HandleExceptionAsync(context, ex);
            }
        }

        private static async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            context.Response.ContentType = "application/json";

            int statusCode;
            object errorResponse;

            if (ex is AppException appEx)
            {
                statusCode = MapErrorCodeToHttpStatus(appEx.ErrorCode);
                errorResponse = new { errorCode = appEx.ErrorCode, message = appEx.Message };
            }
            else
            {
                statusCode = (int)HttpStatusCode.InternalServerError;
                errorResponse = new { errorCode = 500, message = "Internal server error" };
            }

            context.Response.StatusCode = statusCode;
            var json = JsonSerializer.Serialize(errorResponse);
            await context.Response.WriteAsync(json);
        }

        private static int MapErrorCodeToHttpStatus(int errorCode)
        {
            // Nếu ErrorCode = HTTP status code 3 chữ số thì chỉ cần map trực tiếp
            return Enum.IsDefined(typeof(HttpStatusCode), errorCode)
                ? errorCode
                : (int)HttpStatusCode.InternalServerError;
        }
    }
}
