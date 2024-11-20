var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton(new List<User> // синглтон потому что нужно создать единственный екземпляр
{
    new User(0, "Alex", 18),
    new User(1, "Tom", 34),
    new User(2, "Kate", 45)
});

builder.Services.AddTransient<IController, Controller>(); // потому что нужно для каждого нового запроса создавать чтобы не засорять память

var app = builder.Build();

app.Run(async (context) =>
{
    var controller = context.RequestServices.GetRequiredService<IController>();
    var path = context.Request.Path.ToString();
    
    switch (path)
    {
        case "/":
            context.Response.ContentType = "text/html";
            await context.Response.WriteAsync("<h1>Hello from Users API made by services and DI</h1>");
            break;

        case "/showusers":
            context.Response.ContentType = "application/json";
            var users = controller.GetAllUsers();
            await context.Response.WriteAsJsonAsync(users);
            break;

        case "/adduser":
            {
                // ?id=3&name=Anna&age=30
                var query = context.Request.Query;
                if (query.ContainsKey("id") && query.ContainsKey("name") && query.ContainsKey("age"))
                {
                    var user = new User(
                        int.Parse(query["id"]),
                        query["name"],
                        int.Parse(query["age"])
                    );
                    controller.AddUser(user);
                    await context.Response.WriteAsync("User added successfully!");
                }
                else
                {
                    context.Response.StatusCode = 400;
                    await context.Response.WriteAsync("Invalid parameters! Please provide id, name, and age.");
                }
                break;
            }

        case "/deleteuser":
            {
                // ?id=1
                var query = context.Request.Query;
                if (query.ContainsKey("id"))
                {
                    var id = int.Parse(query["id"]);
                    controller.DeleteUser(id);
                    await context.Response.WriteAsync($"User with id {id} deleted successfully!");
                }
                else
                {
                    context.Response.StatusCode = 400;
                    await context.Response.WriteAsync("Invalid parameters! Please provide id.");
                }
                break;
            }

        case "/getuser":
            {
                // ?id=1
                var query = context.Request.Query;
                if (query.ContainsKey("id"))
                {
                    var id = int.Parse(query["id"]);
                    var user = controller.GetUser(id);
                    if (user != null)
                    {
                        await context.Response.WriteAsJsonAsync(user);
                    }
                    else
                    {
                        context.Response.StatusCode = 404;
                        await context.Response.WriteAsync($"User with id {id} not found.");
                    }
                }
                else
                {
                    context.Response.StatusCode = 400;
                    await context.Response.WriteAsync("Invalid parameters! Please provide id.");
                }
                break;
            }

        case "/edituser":
            {
                // ?id=1&name=Updated&age=40
                var query = context.Request.Query;
                if (query.ContainsKey("id") && query.ContainsKey("name") && query.ContainsKey("age"))
                {
                    var user = new User(
                        int.Parse(query["id"]),
                        query["name"],
                        int.Parse(query["age"])
                    );
                    controller.EditUser(user);
                    await context.Response.WriteAsync($"User with id {user.Id} updated successfully!");
                }
                else
                {
                    context.Response.StatusCode = 400;
                    await context.Response.WriteAsync("Invalid parameters! Please provide id, name, and age.");
                }
                break;
            }

        default:
            context.Response.StatusCode = 404;
            await context.Response.WriteAsync("Endpoint not found.");
            break;
    }
});

app.Run();

public class User
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Age { get; set; }

    public User(int id, string name, int age)
    {
        Id = id;
        Name = name;
        Age = age;
    }
}

public interface IController
{
    void AddUser(User user);
    void DeleteUser(int id);
    User? GetUser(int id);
    void EditUser(User user);
    List<User> GetAllUsers();
}

public class Controller : IController
{
    private readonly List<User> _users;

    public Controller(List<User> list)
    {
        _users = list;
    }
    
    public void AddUser(User user)
    {
        _users.Add(user);
    }

    public void DeleteUser(int id)
    {
        _users.RemoveAll(x => x.Id == id);
    }

    public User? GetUser(int id)
    {
        return _users.FirstOrDefault(x => x.Id == id);
    }

    public void EditUser(User user)
    {
        var existingUser = _users.FirstOrDefault(x => x.Id == user.Id);
        if (existingUser != null)
        {
            existingUser.Name = user.Name;
            existingUser.Age = user.Age;
        }
    }

    public List<User> GetAllUsers()
    {
        return _users;
    }
}
