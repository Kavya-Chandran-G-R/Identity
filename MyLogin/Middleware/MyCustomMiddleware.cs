using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace MyLogin.Middleware
{
    public class MyCustomMiddleware : IMiddleware
    {

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            await next(context);

            if (context.Response.StatusCode == 404 && !context.Response.HasStarted)
            {
                context.Response.Redirect("/Error/Error404"); // Replace "/error" with your desired error page URL
            }
        }
    }
}
