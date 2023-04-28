using FHSaverMVC.Infrastructure;

namespace FHSaverMVC.Middlewares
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        public GlobalExceptionMiddleware(RequestDelegate next) 
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (CustomException ex)
            {
                context.Response.ContentType = "text/html";
                context.Response.StatusCode = ex.StatusCode;
                await context.Response.WriteAsync("OOps! Some error ocurred!\n" + ex.ErrorMessage);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                context.Response.ContentType = "text/html";
                context.Response.StatusCode = 500;
                await context.Response.WriteAsync("OOps! Some error ocurred!\n" + ex.Message);
            }
        }

    }
}
