var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();
 
List<Person> people = new List<Person>()
{
    new Person("Tom", 30),
    new Person("Kate", 19)
};
 
app.Run(async (context) =>
{
    var path = context.Request.Path.ToString();
    var response = context.Response;
    var request = context.Request;
 
    switch (path.ToLower())
    {
        case "/": { await response.WriteAsync("Welcome to Users API"); break; }
        case "/adduser":
        {
            string? name = request.Query["name"];
            int age = 0;
            if (!int.TryParse(request.Query["age"], out age))
            {
                await response.WriteAsJsonAsync("Not valid age");
            }
            var user = new Person(name, age);
            people.Add(user);
            await response.WriteAsJsonAsync(user);
            break;
        }
        case "/getuser":
        {
            await response.WriteAsJsonAsync(people.FirstOrDefault(e => e.Id == request.Query["id"]));
            break;
        }
        case "/removeuser":
        {
            var user = people.FirstOrDefault(e => e.Id.ToString() == request.Query["id"]);
            if (user != null)
            {
                people.Remove(user);
            }
            await response.WriteAsJsonAsync(user);
            break;
        }
        case "/allusers": { await response.WriteAsJsonAsync(people); break; }
        default: { await response.WriteAsJsonAsync("Not found"); break; }
    }
});
 
app.Run();
 
class Person
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; }
    public int Age { get; set; }
 
    public Person(string name, int age)
    {
        Name = name;
        Age = age;
    }
}