using ASPEntityFrameworkCore.Models;
using Microsoft.EntityFrameworkCore;

namespace ASPEntityFrameworkCore.Data;

public class ApplicationContext : DbContext
{
    public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
    {
        Database.EnsureCreated();
    }
    public DbSet<User> Users { get; set; }
}