using Microsoft.EntityFrameworkCore;

namespace TemporalTableSample.Models;

public class EFDataContext : DbContext
{
    public DbSet<Person> People { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("server=.;initial catalog=TemporalTable;Trusted_Connection=True;");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Person>().ToTable(_ =>
        {
            _.IsTemporal();
        });
    }
}