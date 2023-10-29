namespace MyLogin.Middleware
{
    public class DisableBackButtonAfterLogoutMiddleware: IMiddleware
    {
        
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            context.Response.Headers.Add("Cache-Control", "no-cache, no-store, max-age=0, must-revalidate");
            context.Response.Headers.Add("Pragma", "no-cache");
            context.Response.Headers.Add("Expires", "Sat, 01 Jan 2000 00:00:00 GMT");

            await next(context);
        }
    }
}
