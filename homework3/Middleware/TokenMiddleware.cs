namespace homework3.Middleware;
public class TokenMiddleware
{
    private readonly RequestDelegate _next;

    public TokenMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var token = context.Request.Query["token"].ToString(); 

        if (token != "12345")
        {
            context.Response.StatusCode = 403;
            await context.Response.WriteAsync("Invalid token");
        }
        else
        {
            await _next(context);
        }
    }
}
