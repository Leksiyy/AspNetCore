using homework10.Models;
using Microsoft.EntityFrameworkCore;

namespace homework10.Data;

public class ApplicationContext : DbContext
{
    public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
    {
        Database.EnsureCreated();
    }
    public DbSet<Book> Books { get; set; }
    public DbSet<Comment> Comments { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<Comment>()
            .HasOne(c => c.Book)
            .WithMany(b => b.Comments)
            .HasForeignKey(c => c.BookId)
            .OnDelete(DeleteBehavior.Cascade);
    }

}