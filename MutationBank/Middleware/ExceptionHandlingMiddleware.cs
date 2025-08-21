using MutationBank.Exceptions;
using MutationBank.Models;
using System.Net;
using System.Text.Json;

namespace MutationBank.Middleware
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

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var response = context.Response;
            response.ContentType = "application/json";

            var errorResponse = new ErrorResponse();

            switch (exception)
            {
                case BadRequestException bre:
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    errorResponse = new ErrorResponse
                    {
                        Error = bre.Message
                    };
                    break;

                case NotFoundException nfe:
                    response.StatusCode = (int)HttpStatusCode.NotFound;
                    errorResponse = new ErrorResponse
                    {
                        Error = nfe.Message
                    };
                    break;

                default:
                    response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    errorResponse = new ErrorResponse
                    {
                        Error = "An internal server error occurred"
                    };
                    break;
            }

            var jsonResponse = JsonSerializer.Serialize(errorResponse, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            await response.WriteAsync(jsonResponse);
        }
    }
}
