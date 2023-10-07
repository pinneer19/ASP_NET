using Serilog;
using Serilog.Events;

namespace Web_153502_Logvinovich.Middleware
{
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;

        public RequestLoggingMiddleware(RequestDelegate next)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
        }

        public async Task InvokeAsync(HttpContext context)
        {
            await _next(context);

            //context.Response.Body.Seek(0, SeekOrigin.Begin);
            var responseStatusCode = context.Response.StatusCode;

            if (responseStatusCode >= 200 && responseStatusCode < 300)
            {
                return;
            }

            var requestPath = context.Request.Path;
            Log.Write(LogEventLevel.Information, $"---> request {requestPath} returns {responseStatusCode}");
            
        }
    }
}
