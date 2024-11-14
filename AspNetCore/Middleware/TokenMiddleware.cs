namespace AspNetCore.Middleware;

public class TokenMiddleware
{
    private string token;
    private readonly RequestDelegate _next;

    public TokenMiddleware(RequestDelegate next, string token)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var token = context.Request.Query["token"];
        if (token != this.token)
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