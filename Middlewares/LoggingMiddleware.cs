using System.Diagnostics;

namespace E_CommerceApi.Middlewares
{
    public class LoggingMiddleware
    {
        private readonly ILogger _logger;
        private readonly RequestDelegate _next; 
        public LoggingMiddleware(RequestDelegate next, ILogger<LoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }
        public async Task Invoke(HttpContext context) 
        {
            var watch = new Stopwatch();
            _logger.LogInformation($"Controller : {context.Request.Path} , Request : {context.Request.Method} , At Time : {DateTime.Now}");
            watch.Start();
            await _next(context);
            watch.Stop();
            _logger.LogInformation($" Time Taken : {watch.ElapsedMilliseconds} ms");
        }
    }
}
