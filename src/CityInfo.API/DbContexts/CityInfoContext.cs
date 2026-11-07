using CityInfo.API.Entities;

using Microsoft.EntityFrameworkCore;

namespace CityInfo.API.DbContexts;

/// <summary>
/// Represents the Entity Framework Core database context for the city information API.
/// </summary>
/// <param name="options">The options used to configure the database context.</param>
public class CityInfoContext(DbContextOptions<CityInfoContext> options) : DbContext(options)
{
    /// <summary>
    /// Gets or sets the collection of cities in the database.
    /// </summary>
    public DbSet<City> Cities { get; set; }

    /// <summary>
    /// Gets or sets the collection of points of interest in the database.
    /// </summary>
    public DbSet<PointOfInterest> PointOfInterests { get; set; }

    /// <summary>
    /// Configures the entity model for the database context and seeds initial city and point-of-interest data.
    /// </summary>
    /// <param name="modelBuilder">The model builder used to configure the entity model.</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<City>()
            .HasData(
                new City("Karachi")
                {
                    Id = 1,
                    Description = "The largest city of Pakistan, famous for its beaches, historical buildings, vibrant food culture, and Quaid-e-Azam's Mausoleum.",
                },
                new City("Lahore")
                {
                    Id = 2,
                    Description = "The cultural capital of Pakistan, famous for Badshahi Mosque, Lahore Fort, Shalimar Gardens, and its delicious traditional food.",
                },
                new City("Islamabad")
                {
                    Id = 3,
                    Description = "The capital city of Pakistan, known for its beautiful greenery, modern infrastructure, Faisal Mosque, and Margalla Hills.",
                },
                new City("Peshawar")
                {
                    Id = 4,
                    Description = "One of the oldest cities in South Asia, famous for Qissa Khwani Bazaar, Peshawar Museum, traditional chapli kebabs, and rich Pashtun culture.",
                },
                new City("Quetta")
                {
                    Id = 5,
                    Description = "The capital of Balochistan, surrounded by mountains and known for its beautiful landscapes, fruit orchards, and traditional Balochi culture.",
                }
            );

        modelBuilder.Entity<PointOfInterest>()
            .HasData(
                new PointOfInterest("Mazar-e-Quaid")
                {
                    Id = 1,
                    CityId = 1,
                    Description = "The mausoleum of Muhammad Ali Jinnah, the founder of Pakistan, and one of Karachi's most iconic landmarks."
                },
                new PointOfInterest("Clifton Beach")
                {
                    Id = 2,
                    CityId = 1,
                    Description = "A popular beach along the Arabian Sea, known for its scenic coastline, camel rides, and recreational activities."
                },
                new PointOfInterest("Pakistan Maritime Museum")
                {
                    Id = 3,
                    CityId = 1,
                    Description = "A maritime museum showcasing Pakistan's naval history, ships, aircraft, and naval heritage."
                },
                new PointOfInterest("Badshahi Mosque")
                {
                    Id = 4,
                    CityId = 2,
                    Description = "A magnificent Mughal-era mosque known for its grand architecture and historical significance."
                },
                new PointOfInterest("Faisal Mosque")
                {
                    Id = 5,
                    CityId = 3,
                    Description = "One of Pakistan's largest mosques, famous for its unique modern architecture and scenic location."
                },
                new PointOfInterest("Peshawar")
                {
                    Id = 6,
                    CityId = 4,
                    Description = "A museum showcasing Gandhara art, Buddhist sculptures, and the region's rich archaeological history."
                },
                new PointOfInterest("Hanna Lake")
                {
                    Id = 7,
                    CityId = 5,
                    Description = "A picturesque lake surrounded by mountains, popular for its beautiful scenery and recreational activities."
                }
            );

        base.OnModelCreating(modelBuilder);
    }

    // Alternative method for providing connection string to DbContext
    // protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    // {
    //     optionsBuilder.UseSqlite("connectionString");
    //     base.OnConfiguring(optionsBuilder);
    // }
}