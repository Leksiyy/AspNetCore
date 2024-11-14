var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();



app.Run(async (context) =>
{
    context.Response.ContentType = "text/html; charset=utf-8";

    if (context.Request.Path == "/postuser")
    {
        var form = context.Request.Form;
        
        var name = form["name"];
        var email = form["email"];
        var phone = form["phone"];

        await context.Response.WriteAsync($"<div>" +
                                          $"<p>Name: {name}</p>" +
                                          $"<p>Email: {email}</p>" +
                                          $"<p>Phone number: {phone}</p>" +
                                          $"</div>");
    }
    else
    {
        await context.Response.SendFileAsync("html/index.html");
    }
});

app.Run();
