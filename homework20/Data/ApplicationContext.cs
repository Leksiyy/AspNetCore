using homework20.Models;
using Microsoft.EntityFrameworkCore;

namespace homework20.Data;

public class ApplicationDbContext : DbContext
{
    public DbSet<UserProfile> UserProfiles { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
    
}

