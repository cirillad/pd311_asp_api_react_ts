using pd311_web_api.BLL.Services;

namespace pd311_web_api.Middlewares
{
    public class MiddlewareExceptionHandler
    {
        private readonly RequestDelegate _next;

        public MiddlewareExceptionHandler(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                string error = ex.InnerException == null ? ex.Message : ex.InnerException.Message;
                var response = new ServiceResponse(message: error);

                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsJsonAsync(response);
            }
        }
    }
}
