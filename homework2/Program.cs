using System.Reflection;
using System.Text;
using Microsoft.Data.SqlClient;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

var configuration = app.Services.GetRequiredService<IConfiguration>();
string connectionString = configuration.GetConnectionString("DefaultConnection");

app.MapGet("/", async (context) =>
{
    List<User> users = new List<User>();
    using (SqlConnection connection = new SqlConnection(connectionString))
    {
        await connection.OpenAsync();
        SqlCommand command = new SqlCommand("select Id, Name, Age from Users", connection);
        using (SqlDataReader reader = await command.ExecuteReaderAsync())
        {
            if (reader.HasRows)
            {
                while (await reader.ReadAsync())
                {
                    users.Add(new User(reader.GetInt32(0), reader.GetString(1), reader.GetInt32(2)));
                }
            }
        }
    }

    await context.Response.WriteAsync(GenerateHtmlPage(BuildHtmlTable(users), "All Users from DataBase"));
});

app.MapPost("/add", async (context) =>
{
    var form = await context.Request.ReadFormAsync();
    string name = form["Name"];
    int age = int.Parse(form["Age"]);

    using (SqlConnection connection = new SqlConnection(connectionString))
    {
        await connection.OpenAsync();
        SqlCommand command = new SqlCommand("insert into Users (Name, Age) values (@name, @age)", connection);
        command.Parameters.AddWithValue("@name", name);
        command.Parameters.AddWithValue("@age", age);
        await command.ExecuteNonQueryAsync();
    }

    context.Response.Redirect("/");
});

app.MapPost("/delete", async (context) =>
{
    var form = await context.Request.ReadFormAsync();
    int id = int.Parse(form["Id"]);
    
    using (SqlConnection connection = new SqlConnection(connectionString))
    {
        await connection.OpenAsync();
        SqlCommand command = new SqlCommand("DELETE FROM Users WHERE id = @id", connection);
        command.Parameters.AddWithValue("@id", id);
        await command.ExecuteNonQueryAsync();
    }
    
    context.Response.Redirect("/");
});

app.MapPost("/edit", async (context) =>
{
    var form = await context.Request.ReadFormAsync();
    int editingId = int.Parse(form["id"]);

    List<User> users = new List<User>();
    using (SqlConnection connection = new SqlConnection(connectionString))
    {
        await connection.OpenAsync();
        SqlCommand command = new SqlCommand("select Id, Name, Age from Users", connection);
        using (SqlDataReader reader = await command.ExecuteReaderAsync())
        {
            if (reader.HasRows)
            {
                while (await reader.ReadAsync())
                {
                    users.Add(new User(reader.GetInt32(0), reader.GetString(1), reader.GetInt32(2)));
                }
            }
        }
    }
    await context.Response.WriteAsync(GenerateHtmlPage(BuildHtmlTable(users, editingId), "Edit User"));
});

app.MapPost("/save", async (context) =>
{
    var form = await context.Request.ReadFormAsync();
    int id = int.Parse(form["Id"]);
    string name = form["Name"];
    int age = int.Parse(form["Age"]);

    using (SqlConnection connection = new SqlConnection(connectionString))
    {
        await connection.OpenAsync();
        SqlCommand command = new SqlCommand("UPDATE Users SET Name = @name, Age = @age WHERE Id = @id", connection);
        command.Parameters.AddWithValue("@id", id);
        command.Parameters.AddWithValue("@name", name);
        command.Parameters.AddWithValue("@age", age);
        await command.ExecuteNonQueryAsync();
    }

    context.Response.Redirect("/");
});

app.MapGet("/cancel", async (context) =>
{
    context.Response.Redirect("/");
});

app.MapPost("/search", async (context) =>
{
    var form = await context.Request.ReadFormAsync();

    int? searchId = int.TryParse(form["Id"], out int idValue) ? idValue : (int?)null;
    int? searchAge = int.TryParse(form["Age"], out int ageValue) ? ageValue : (int?)null;
    string searchName = form["Name"].ToString();

    List<User> users = new List<User>();
    using (SqlConnection connection = new SqlConnection(connectionString))
    {
        await connection.OpenAsync();

        var queryBuilder = new StringBuilder("SELECT Id, Name, Age FROM Users WHERE 1=1");
        if (searchId.HasValue)
            queryBuilder.Append(" AND Id = @Id");
        if (!string.IsNullOrWhiteSpace(searchName))
            queryBuilder.Append(" AND Name LIKE @Name");
        if (searchAge.HasValue)
            queryBuilder.Append(" AND Age = @Age");

        SqlCommand command = new SqlCommand(queryBuilder.ToString(), connection);

        if (searchId.HasValue)
            command.Parameters.AddWithValue("@Id", searchId.Value);
        if (!string.IsNullOrWhiteSpace(searchName))
            command.Parameters.AddWithValue("@Name", $"%{searchName}%");
        if (searchAge.HasValue)
            command.Parameters.AddWithValue("@Age", searchAge.Value);

        using (SqlDataReader reader = await command.ExecuteReaderAsync())
        {
            if (reader.HasRows)
            {
                while (await reader.ReadAsync())
                {
                    users.Add(new User(reader.GetInt32(0), reader.GetString(1), reader.GetInt32(2)));
                }
            }
        }
    }

    await context.Response.WriteAsync(GenerateHtmlPage(BuildHtmlTable(users, isSearch: true), "Search User"));
});


app.Run();

static string BuildHtmlTable<T>(IEnumerable<T> collection, int? editingId = null, bool isSearch = false)
{
    StringBuilder tableHtml = new StringBuilder();
    tableHtml.Append("<table class=\"table\">");

    PropertyInfo[] properties = typeof(T).GetProperties();

    // Заголовки столбцов
    tableHtml.Append("<tr>");
    foreach (PropertyInfo property in properties)
    {
        tableHtml.Append($"<th>{property.Name}</th>");
    }
    tableHtml.Append("<th>Options</th>");
    tableHtml.Append("</tr>");

    //Форма для поиска
    tableHtml.Append("<form action=\"/search\" method=\"post\">");
    tableHtml.Append("<tr>");
    tableHtml.Append($"<td><input type=\"number\" name=\"Id\" class=\"form-control\"  /></td>");
    tableHtml.Append($"<td><input type=\"text\" name=\"Name\" class=\"form-control\"  /></td>");
    tableHtml.Append($"<td><input type=\"number\" name=\"Age\" class=\"form-control\"  /></td>");
    tableHtml.Append($"<td><button type=\"submit\" class=\"btn btn-success\">Search</button>");
    tableHtml.Append("</form>");
    tableHtml.Append("<form action=\"/cancel\" method=\"get\" style=\"margin-left: 10px;\">");
    tableHtml.Append($"<button type=\"submit\" class=\"btn btn-secondary\">Cancel</button>");
    tableHtml.Append("</form>");
    tableHtml.Append("</tr>");
    
    // Вывод строк
    foreach (T item in collection)
    {
        int id = (int)item.GetType().GetProperty("Id").GetValue(item);

        if (editingId != null && editingId == id)
        {
            tableHtml.Append("<form action=\"/save\" method=\"post\">");
            tableHtml.Append("<tr>");

            foreach (PropertyInfo property in properties)
            {
                string name = property.Name;
                object value = property.GetValue(item);

                if (name == "Id")
                {
                    tableHtml.Append($"<td><input type=\"hidden\" name=\"Id\" value=\"{value}\" /></td>");
                }
                else
                {
                    tableHtml.Append($"<td><input type=\"text\" name=\"{name}\" value=\"{value}\" class=\"form-control\" /></td>");
                }
            }

            tableHtml.Append($"<td>");
            tableHtml.Append($"<button type=\"submit\" class=\"btn btn-success\">Save</button>");
            tableHtml.Append($"</form>");

            tableHtml.Append($"<form action=\"/cancel\" method=\"get\" style=\"display:inline;\">");
            tableHtml.Append($"<button type=\"submit\" class=\"btn btn-secondary\" style=\"margin-left: 10px;\">Cancel</button>");
            tableHtml.Append($"</form>");
            tableHtml.Append($"</td>");

            tableHtml.Append("</tr>");
        }
        else
        {
            tableHtml.Append("<tr>");
            foreach (PropertyInfo property in properties)
            {
                object value = property.GetValue(item);
                tableHtml.Append($"<td>{value}</td>");
            }

            // edit delete
            tableHtml.Append("<td>");
            tableHtml.Append("<form action=\"/edit\" method=\"post\" style=\"display:inline;\">");
            tableHtml.Append($"<input type=\"hidden\" name=\"id\" value=\"{id}\" />");
            tableHtml.Append("<button type=\"submit\" class=\"btn btn-primary\" style=\"margin-right: 10px;\">Edit</button>");
            tableHtml.Append("</form>");
            tableHtml.Append("<form action=\"/delete\" method=\"post\" style=\"display:inline;\">");
            tableHtml.Append($"<input type=\"hidden\" name=\"id\" value=\"{id}\" />");
            tableHtml.Append("<button type=\"submit\" class=\"btn btn-danger\">Delete</button>");
            tableHtml.Append("</form>");
            tableHtml.Append("</td>");

            tableHtml.Append("</tr>");
        }
    }

    // Строка для добавления
    if (!isSearch)
    {
        tableHtml.Append("<form action=\"/add\" method=\"post\">");
        tableHtml.Append("<tr>");
        tableHtml.Append($"<td></td>");
        tableHtml.Append($"<td><input type=\"text\" name=\"Name\" class=\"form-control\" required /></td>");
        tableHtml.Append($"<td><input type=\"number\" name=\"Age\" class=\"form-control\" required /></td>");
        tableHtml.Append($"<td><button type=\"submit\" class=\"btn btn-success\">Add User</button></td>");
        tableHtml.Append("</tr>");
        tableHtml.Append("</form>");
    }

    tableHtml.Append("</table>");
    return tableHtml.ToString();
}

static string GenerateHtmlPage(string body, string header)
{
    string html = $"""
                   <!DOCTYPE html>
                   <html>
                   <head>
                       <meta charset="utf-8" />
                       <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0-alpha3/dist/css/bootstrap.min.css" rel="stylesheet" 
                       integrity="sha384-KK94CHFLLe+nY2dmCWGMq91rCGa5gtU4mk92HdvYe+M/SXH301p5ILy+dN9+nJOZ" crossorigin="anonymous">
                       <title>{header}</title>
                   </head>
                   <body>
                   <div class="container">
                   <h2 class="d-flex justify-content-center">{header}</h2>
                   <div class="mt-5"></div>
                   {body}
                       <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0-alpha3/dist/js/bootstrap.bundle.min.js" 
                       integrity="sha384-ENjdO4Dr2bkBIFxQpeoTz1HIcje39Wm4jDKdf19U8gI4ddQ3GYNS7NTKfAdVQSZe" crossorigin="anonymous"></script>
                   </div>
                   </body>
                   </html>
                   """;
    return html;
}

record User(int Id, string Name, int Age)
{
    public User(string Name, int Age) : this(0, Name, Age)
    {
        
    }
}