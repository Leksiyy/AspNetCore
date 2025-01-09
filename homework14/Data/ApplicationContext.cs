using homework14.Models;
using homework14.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace homework14.Data;

public class ApplicationContext : DbContext
{
    public DbSet<Currency> Currencies { get; set; }

    public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
    {

    }
}