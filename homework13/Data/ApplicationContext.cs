using homework13.Models;
using Microsoft.EntityFrameworkCore;

namespace ASPEntityFrameworkCore.Data;

public class ApplicationContext : DbContext
{
    public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
    {
    }
    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .HasIndex(u => u.UserName)
            .IsUnique();
        
        base.OnModelCreating(modelBuilder);
    }
}