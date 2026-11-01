using CityInfo.API.Entities;

using Microsoft.EntityFrameworkCore;

namespace CityInfo.API.DbContexts;

public class CityInfoContext(DbContextOptions<CityInfoContext> options) : DbContext(options)
{
    public DbSet<City> Cities { get; set; }
    public DbSet<PointOfInterest> PointOfInterests { get; set; }

    // Alternative method for providing connection string to DbContext
    // protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    // {
    //     optionsBuilder.UseSqlite("connectionString");
    //     base.OnConfiguring(optionsBuilder);
    // }
}