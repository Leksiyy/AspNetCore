var builder = WebApplication.CreateBuilder(args);

builder.Services.AddTransient<ITime, TimerService>();

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

public interface ITime
{
    string GetTime(bool inculdeSeconds);
}

class TimerService : ITime
{
    public string GetTime(bool inculdeSeconds)
    {
        if (inculdeSeconds)
        {
            return $"{DateTime.Now.ToString("hh:mm:ss")}";
        }
        else
        {
            return $"{DateTime.Now.ToString("hh:mm")}";
        }
    }
}