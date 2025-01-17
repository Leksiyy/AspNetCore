using EmailService;

var builder = WebApplication.CreateBuilder(args);

var emailConfig = builder.Configuration.GetSection("EmailConfiguration").Get<EmailConfiguration>();

builder.Services.AddSingleton(emailConfig);
builder.Services.AddScoped<IEmailSender, EmailSend>();

builder.Services.AddSingleton<MessageCheckService>();
builder.Services.AddHostedService(provider => provider.GetRequiredService<MessageCheckService>());


builder.Services.AddControllersWithViews();

var app = builder.Build();


app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();