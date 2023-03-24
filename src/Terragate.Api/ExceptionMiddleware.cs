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
                _logger.LogError(ex, "{controller} {requestMethod} {requestUrl}",
                    context.GetRouteData().Values["controller"],
                    context.Request.Method, 
                    context.Request.GetDisplayUrl());

                context.Response.ContentType = MediaTypeNames.Text.Plain;
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                await context.Response.WriteAsync(ex.Message);
            }
        }             
    }
}
