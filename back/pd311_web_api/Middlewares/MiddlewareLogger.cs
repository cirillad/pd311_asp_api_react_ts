namespace pd311_web_api.Middlewares
{
    public class MiddlewareLogger
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<MiddlewareLogger> _logger;

        public MiddlewareLogger(RequestDelegate next, ILogger<MiddlewareLogger> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            string? ip = context.Connection.RemoteIpAddress?.ToString();
            _logger.LogInformation($"Request: client ip - {ip}");

            await _next(context);

            
            _logger.LogInformation($"Response: client ip - {ip}");
        }
    }
}
