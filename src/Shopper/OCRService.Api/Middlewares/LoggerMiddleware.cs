namespace OCRService.Api.Middlewares
{
    // Logger Middleware (warstwa pośrednia)
    public class LoggerMiddleware
    {
        private readonly RequestDelegate _next;

        public LoggerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            Console.WriteLine($"{context.Request.Method} {context.Request.Path}");

            await _next(context);

            Console.WriteLine($"{context.Response.StatusCode}");
        }
    }

    public static class LoggerMiddlewareExtensions
    {
        public static IApplicationBuilder UseLogger(this IApplicationBuilder app)
        {
            return app.UseMiddleware<LoggerMiddleware>();
        }
    }
}
