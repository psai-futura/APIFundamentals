using APIFundamentals.Entities;
using Microsoft.EntityFrameworkCore;

namespace APIFundamentals.DBContexts;

public class CityInfoContext : DbContext
{

    public DbSet<City> Cities { get; set; } = null!;

    public DbSet<PointOfInterest> PointsOfInterest { get; set; } = null!;

    public CityInfoContext(DbContextOptions<CityInfoContext> options): base(options)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<City>().HasData(
            new City("Amsterdam")
            {
                Id =1,
                Description = "Capital City of Netherlands"
            },
            new City("Paris")
            {
                Id =2,
                Description = "Capital city of France"
            },
            new City("Berlin")
            {
                Id =3,
                Description = "Capital City of Germany"
            }
            );
        
        modelBuilder.Entity<PointOfInterest>().HasData(
            new PointOfInterest("Anne Frank House")
            {
                Id =1,
                CityId=1,
                Description = "WW2 teenage diarist's house museum"
            },
            new PointOfInterest("Vondelpark")
            {
                Id =2,
                CityId=1,
                Description = "Huge park with open air theatre"
            },
            new PointOfInterest("Eiffel Tower")
            {
                Id =3,
                CityId=2,
                Description = "Huge Tower"
            },
            new PointOfInterest("Lovre Museum")
            {
                Id =4,
                CityId=2,
                Description = "Museum with Mona Lisa painting"
            },
            new PointOfInterest("Brandenburg Gate")
            {
                Id =5,
                CityId=3,
                Description = "Restored 18th Century Gate"
            },
            new PointOfInterest("Berlin Cathedral")
            {
                Id =6,
                CityId=3,
                Description = "Cathedral"
            }
        );
        base.OnModelCreating(modelBuilder);
    }
}