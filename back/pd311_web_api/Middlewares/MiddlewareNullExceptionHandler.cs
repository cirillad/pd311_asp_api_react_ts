using pd311_web_api.BLL.Services;

namespace pd311_web_api.Middlewares
{
    public class MiddlewareNullExceptionHandler
    {
        private readonly RequestDelegate _next;

        public MiddlewareNullExceptionHandler(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (ArgumentNullException ex)
            {
                string error = ex.InnerException == null ? ex.Message : ex.InnerException.Message;
                var response = new ServiceResponse(message: "Обробка ArgumentNullException");

                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsJsonAsync(response);
            }
        }
    }
}
