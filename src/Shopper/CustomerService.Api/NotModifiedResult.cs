namespace CustomerService.Api
{
    public class NotModifiedResult : IResult
    {
        public Task ExecuteAsync(HttpContext httpContext)
        {
            httpContext.Response.StatusCode = 304;

            return Task.CompletedTask;
        }
    }

    public static class ResultsExtensions
    {
        public static IResult NotModified(this IResultExtensions resultExtensions)
        {
            ArgumentNullException.ThrowIfNull(resultExtensions);

            return new NotModifiedResult();
        }
    }
}
