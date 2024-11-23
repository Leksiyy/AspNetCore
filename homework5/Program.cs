using homework5;
using homework5.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

app.MapGet("/", async context =>
{
    context.Response.ContentType = "text/html";
    await context.Response.WriteAsync("<h1>Users and services API</h1>");
});

//регистрация
app.MapPost("/register", async (HttpContext request, ApplicationContext context) =>
{
    var query = request.Request.Query;

    var name = query["name"].ToString();
    var email = query["email"].ToString();
    var phone = query["phone"].ToString();
    var serviceIdsRaw = query["serviceIds"].ToString();
    
    if (string.IsNullOrEmpty(name) ||
        string.IsNullOrEmpty(email) ||
        string.IsNullOrEmpty(phone) ||
        string.IsNullOrEmpty(serviceIdsRaw))
    {
        return Results.BadRequest("Invalid data");
    }
    
    var serviceIds = serviceIdsRaw
        .ToString()
        .Split(',')
        .Select(int.Parse)
        .ToList();

    var user = new User
    {
        Name = name,
        Email = email,
        Phone = phone,
        UserServices = new List<UserService>()
    };
    
    var services = await context.Services
        .Where(s => serviceIds.Contains(s.Id))
        .ToListAsync();
    
    if (!services.Any()) return Results.BadRequest("Invalid service IDs");
    
    user.UserServices.AddRange(services.Select(s => new UserService {ServiceId = s.Id}));
    await context.Users.AddAsync(user);
    await context.SaveChangesAsync();
    
    return Results.Ok(user);
});

//просмотр сервисов пользователя
app.MapGet("/users/{userId}/services", async (int userId, ApplicationContext context) =>
{
    var userServices = await context.UserServices
        .Where(s => s.ServiceId == userId)
        .Include(s => s.Service)
        .ToListAsync();

    if (!userServices.Any()) return Results.NotFound("No services found for this user");

    var services = userServices.Select(s => s.Service.Name);
    return Results.Ok(services);
});

//добавить сервисы пользователю
app.MapPost("/users/{userId}/addservice/{**serviceIds}", async (int userId, string serviceIds, ApplicationContext context) =>
{
    int[] serviceIdsArray = serviceIds.Split(',', StringSplitOptions.RemoveEmptyEntries)
        .Select(int.Parse)
        .ToArray();

    var user = context.Users
        .Include(e => e.UserServices)
        .ThenInclude(e => e.Service)
        .FirstOrDefault(u => u.Id == userId);
    
    if (user == null) return Results.NotFound("User does not exist");
    
    var servicesToAdd = await context.Services.Where(e => serviceIdsArray.Contains(e.Id)).ToListAsync();

    if (servicesToAdd.Count != serviceIdsArray.Length) return Results.BadRequest("Invalid service IDs");

    foreach (var service in servicesToAdd)
    {
        if (!context.UserServices.Any(s => s.ServiceId == service.Id && s.UserId == user.Id))
        {
            context.UserServices.Add(new UserService {ServiceId = service.Id, UserId = user.Id});
        }
    }
    
    await context.SaveChangesAsync();
    
    return Results.Ok(new {UserId = userId, Services = serviceIdsArray});
});

//удалить сервисы пользователя
app.MapPost("/users/{userId}/deleteservices/{**serviceIds}", async (int userId, string serviceIds, ApplicationContext context) =>
{
    int[] servicesArray = serviceIds.Split(',', StringSplitOptions.RemoveEmptyEntries)
        .Select(int.Parse)
        .ToArray();
    
    var user = context.Users
        .Include(e => e.UserServices)
        .ThenInclude(e => e.Service)
        .FirstOrDefault(e => e.Id == userId);
    
    if (user == null) return Results.NotFound("User does not exist");
    
    var servicesToRemove = await context.Services.Where(s => servicesArray.Contains(s.Id)).ToListAsync();
    
    if (servicesToRemove.Count != serviceIds.Length) return Results.BadRequest("Invalid service IDs");

    foreach (var service in servicesToRemove)
    {
        var serviceToRemove =
            context.UserServices.FirstOrDefault(s => s.ServiceId == service.Id && s.UserId == user.Id);
        if (!context.UserServices.Any(s => s.ServiceId == service.Id && s.UserId == user.Id) && serviceToRemove is not null)
        {
            context.UserServices.Remove(serviceToRemove);
        }
    }
    
    await context.SaveChangesAsync();
    
    return Results.Ok(new {UserId = userId, Services = servicesToRemove});
});

app.Run();
