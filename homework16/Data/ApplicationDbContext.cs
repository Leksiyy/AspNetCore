using homework16.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace homework16.Data;

public class ApplicationDbContext : IdentityDbContext
{
    public DbSet<Articles> Articles { get; set; }
    
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
}