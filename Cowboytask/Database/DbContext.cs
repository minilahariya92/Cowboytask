using Cowboytask.Database.Models;
using Microsoft.EntityFrameworkCore;

namespace Cowboytask;

public class ApiContext : DbContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseInMemoryDatabase(databaseName: "CowboyDb");
    }

    public DbSet<Cowboy> Cowboy { get; set; }
    public DbSet<Firearm> Firearm { get; set; }
}