using Microsoft.AspNetCore.Http.Extensions;
using System.Net;
using System.Net.Mime;

namespace Terragate.Api
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _logger = logger;
            _next = next;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{unhandledExceptionMethod} {unhandledExceptionUrl}", 
                    context.Request.Method, 
                    context.Request.GetDisplayUrl());

                await HandleExceptionAsync(context, ex);
            }
        }
        
        private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = MediaTypeNames.Application.Json;
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            
            await context.Response.WriteAsJsonAsync(new 
            {
                context.Response.StatusCode,
                exception.Message
            });
        }
    }
}
