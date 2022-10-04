using Microsoft.Extensions.Primitives;

namespace OCRService.Api.Middlewares
{
    public class AuthorizationMiddleware
    {
        private readonly RequestDelegate next;

        public AuthorizationMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            if (context.Request.Headers.TryGetValue("Authorization", out StringValues value) && value == "blabla")
            {
                await next(context);
            }
            else
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            }
        }
    }

    public static class AuthorizationMiddlewareExtensions
    {
        public static IApplicationBuilder UseAuth(this IApplicationBuilder app)
        {
            app.UseMiddleware<AuthorizationMiddleware>();

            return app;
        }
    }
}
