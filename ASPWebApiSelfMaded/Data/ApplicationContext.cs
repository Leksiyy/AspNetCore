using ASPWebApiSelfMaded.Models;
using Microsoft.EntityFrameworkCore;

namespace ASPWebApiSelfMaded.Data;

public class ApplicationContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
    {
        Database.EnsureCreated();
    }
}