using System.Reflection;
using System.Text;
using homework3.Middleware;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

List<Book> books = new List<Book>
{
    new Book(0, "five will survive", "detective"),
    new Book(1, "surgeon", "thriller"),
    new Book(2, "student", "thriller")
};

app.Map("/getbooks", subApp =>
{
    subApp.UseMiddleware<TokenMiddleware>();

    subApp.Run(async context =>
    {
        var query = context.Request.Query;
        string category = query["category"].ToString().ToLower();

        List<Book> sortedBooks = books
            .Where(book => book.Category.ToLower() == category)
            .ToList();

        context.Response.ContentType = "text/html";
        await context.Response.WriteAsync(GenerateHtmlPage(BuildHtmlTable(sortedBooks), "Filtered Books"));
    });
});

app.Map("/allbooks", async context =>
{
    context.Response.ContentType = "text/html";
    await context.Response.WriteAsync(GenerateHtmlPage(BuildHtmlTable(books), "All Books"));
});

app.Map("/", async context =>
{
    context.Response.ContentType = "text/html";
    await context.Response.WriteAsync("<h1>Books API</h1>");
});

static string BuildHtmlTable<T>(IEnumerable<T> collection)
{
    StringBuilder tableHtml = new StringBuilder();
    tableHtml.Append("<table class=\"table\">");

    PropertyInfo[] properties = typeof(T).GetProperties();

    tableHtml.Append("<tr>");
    foreach (PropertyInfo property in properties)
    {
        tableHtml.Append($"<th>{property.Name}</th>");
    }
    tableHtml.Append("</tr>");

    foreach (T item in collection)
    {
        tableHtml.Append("<tr>");
        foreach (PropertyInfo property in properties)
        {
            object value = property.GetValue(item);
            tableHtml.Append($"<td>{value}</td>");
        }
        tableHtml.Append("</tr>");
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

app.Run();
public record Book(int Id, string Name, string Category);

