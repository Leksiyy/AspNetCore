var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

// app.Map("/", () => "Index page");
// app.Map("/about", () => "About page");
// app.Map("/contact", () => "Contact page");


//app.Map("/users/{**info}", (string info) => $"Info: {info}.");

//app.Map("{controller=Home}/{action=Index}/{id?}", (string controller, string action, string id) => $"Controller: {controller}\nAction: {action}\nId: {id}");

// app.Map("/routes", async (IEnumerable<EndpointDataSource> endpoints, HttpContext context) =>
// {
//     await context.Response.WriteAsync(String.Join("\n", endpoints.SelectMany(e => e.Endpoints)));
// });

app.Map("{number:int}", async context =>
{
    context.Response.WriteAsync("Routed to the int endpoint");
}).Add(e => ((RouteEndpointBuilder)e).Order = 1);

app.Map("{number:double}", async context =>
{
    context.Response.WriteAsync("Routed to the double endpoint");
}).Add(e => ((RouteEndpointBuilder)e).Order = 2);

app.Run(); 