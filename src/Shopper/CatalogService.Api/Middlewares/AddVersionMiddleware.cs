namespace CatalogService.Api.Middlewares
{


    public static class AddVersionMiddlewareExtensions
    {
        public static IApplicationBuilder UseVersion(this IApplicationBuilder app)
        {
            return app.UseMiddleware<AddVersionMiddleware>();
        }
    }

    public class AddVersionMiddleware : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            context.Response.OnStarting(() => {
                context.Response.Headers.Add("X-Version", "1.0");
                return Task.FromResult(0);
            });

            await next(context);
        }
    }
}
