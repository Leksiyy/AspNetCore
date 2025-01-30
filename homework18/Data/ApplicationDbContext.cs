using homework18.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace homework18.Data;

public class ApplicationDbContext : IdentityDbContext<User>
{
    public DbSet<ToDo> ToDos { get; set; }
    
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
        //Database.EnsureDeleted();
        
        Database.EnsureCreated();
    }
    
}