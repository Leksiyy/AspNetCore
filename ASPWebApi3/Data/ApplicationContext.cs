using ASPWebApi3.Models;
using Microsoft.EntityFrameworkCore;

namespace ASPWebApi3.Data;

public class ApplicationContext : DbContext
{
    public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
    {
        
    }

    public DbSet<TodoItem> TodoItems { get; set; } = null!;
}