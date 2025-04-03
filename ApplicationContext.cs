using Microsoft.EntityFrameworkCore;
using WeatherArchive.Models;

namespace WeatherArchive;

public class ApplicationContext : DbContext
{
    public ApplicationContext(DbContextOptions<ApplicationContext> options)
        : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<WeatherRecord>().HasIndex(p => new { p.Date, p.Time }).IsUnique();
    }

    public DbSet<WeatherRecord> WeatherRecords { get; set; }
}
