using System.Security.Claims;
using ASPAuth.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc.Authorization;

// var builder = WebApplication.CreateBuilder(args);
//
// // Add services to the container.
// // builder.Services.AddControllersWithViews();
//
// //builder.Services.AddAuthentication("Cookies");
//
// builder.Services.AddControllersWithViews(
// //     options =>
// // {
// //     options.Filters.Add(new AuthorizeFilter());
// // }
//     );
//
// builder.Services.AddSingleton<UserService>();
//
// builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie();
// builder.Services.AddAuthorization();
//
// var app = builder.Build();
//
// app.UseAuthentication();
// app.UseAuthorization();
//
// app.MapControllerRoute(
//     name: "default",
//     pattern: "{controller=Home}/{action=Index}/{id?}");
//
// app.Run();

// -*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-

// var builder = WebApplication.CreateBuilder();
//  
// // аутентификация с помощью куки
// builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
//     .AddCookie();
//  
// var app = builder.Build();
//  
// app.UseAuthentication();
//  
// app.MapGet("/login", async (HttpContext context) =>
// {
//     var claimsIdentity = new ClaimsIdentity("Undefined");
//     var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
//     // установка аутентификационных куки
//     await context.SignInAsync(claimsPrincipal);
//     return Results.Redirect("/");
// });
//  
// app.MapGet("/logout", async (HttpContext context) =>
// {
//     await context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
//     return "Данные удалены";
// });
// app.Map("/", (HttpContext context) =>
// {
//     var user = context.User.Identity;
//     if (user is not null && user.IsAuthenticated)
//     {
//         return $"Пользователь аутентифицирован. Тип аутентификации: {user.AuthenticationType}";
//     }
//     else
//     {
//         return "Пользователь НЕ аутентифицирован";
//     }
// });
//  
// app.Run();

var builder = WebApplication.CreateBuilder();
 
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie();
 
var app = builder.Build();
 
app.UseAuthentication();
 
app.MapGet("/login/{username}", async (string username, HttpContext context) =>
{
    var claims = new List<Claim> { new(ClaimTypes.Name, username) };
    var claimsIdentity = new ClaimsIdentity(claims, "Cookies");
    var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
    await context.SignInAsync(claimsPrincipal);
    return $"Установлено имя {username}";
});
app.Map("/", (HttpContext context) =>
{
    var user = context.User.Identity;
    if (user is not null && user.IsAuthenticated)
        return $"UserName: {user.Name}";
    else return "Пользователь не аутентифицирован.";
});
app.MapGet("/logout", async (HttpContext context) =>
{
    await context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
    return "Данные удалены";
});
 
app.Run();