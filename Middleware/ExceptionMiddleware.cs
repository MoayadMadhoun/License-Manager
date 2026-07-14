using Serilog;

namespace LicenseManagerMinimalAPI.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
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
                Log.Error(ex, "Unhandled Exception");

                context.Response.StatusCode = 500;

                await context.Response.WriteAsync(
                    "Internal Server Error");
            }
        }
    }
}
