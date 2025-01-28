using homework17.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace homework17.Data;

public class ApplicationContext : IdentityDbContext<Student>
{
    public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
    {
        //Database.EnsureDeleted();
        //Database.EnsureCreated();
        
    }
    
    public DbSet<Subject> Subjects { get; set; }
    
    public DbSet<Grade> Grades { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        
        builder.Entity<Subject>(entity =>
        {
            entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
        });
        
        builder.Entity<Grade>(entity =>
        {
            entity.HasOne(g => g.Student)
                .WithMany(s => s.Grades)
                .HasForeignKey(g => g.StudentId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(g => g.Subject)
                .WithMany(s => s.Grades)
                .HasForeignKey(g => g.SubjectId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
