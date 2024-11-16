using homework1;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

List<Person> persons = new List<Person>();

app.Run(async (context) =>
{
    context.Response.ContentType = "text/html; charset=utf-8";

    if (context.Request.Path == "/")
    {
        await context.Response.WriteAsync("<h1 style=\"color: red\">Hello from this page</h1>\n" +
                                          $"<a href=\"/postuser\">invite</a>");
    }
    else if (context.Request.Path == "/postuser" && context.Request.Method == "GET")
    {
        await context.Response.WriteAsync(@"<h2>User form</h2>
            <form method=""post"" action=""/posted"">
            <p>Name: <input name=""name"" /></p>
            <p>Email: <input name=""email"" /></p>
            <p>Phone: <input name=""phone"" /></p>
            <input type=""submit"" value=""Send"" />
            </form>");
    }
    else if (context.Request.Path == "/posted" && context.Request.Method == "POST")
    {
        var form = context.Request.Form;

        var name = form["name"];
        var email = form["email"];
        var phone = form["phone"];

        persons.Add(new Person{Email = email, Name = name, Phone = phone});

        await context.Response.WriteAsync("<h1 style=\"color: green\">Thank you for registration</h1>");
    }
    else
    {
        await context.Response.WriteAsync("There is nothing here.");
    }
});

app.Run();