using APIFundamentals.Models;

namespace APIFundamentals;

public class CitiesDataStore
{
    public List<CityDto> Cities { get; set; }

    public static CitiesDataStore Current { get; } = new CitiesDataStore();

    public CitiesDataStore()
    {
        Cities = new List<CityDto>()
        {
            new CityDto()
            {
                Id = 1,
                Name = "Berlin",
                Description = "Capital of Germany",
                PointsOfInterest = new List<PointsOfInterestDto>()
                {
                    new PointsOfInterestDto()
                    {
                        Id = 1,
                        Name = "Brandenburg Gate",
                        Description = "Restored 18th Century Gate"
                    },
                    new PointsOfInterestDto()
                    {
                        Id = 2,
                        Name = "Berlin Cathedral",
                        Description = "Cathedral"
                    }
                }
            },
            new CityDto()
            {
                Id = 2,
                Name = "Munich",
                Description = "Metropoliton City",
                PointsOfInterest = new List<PointsOfInterestDto>()
                {
                    new PointsOfInterestDto()
                    {
                        Id = 1,
                        Name = "MarienPlatz",
                        Description = "Plaza"
                    },
                    new PointsOfInterestDto()
                    {
                        Id = 2,
                        Name = "Munich Residenz",
                        Description = "Castle"
                    }
                }
                
            },
            new CityDto()
            {
                Id = 3,
                Name = "Wiesbaden",
                Description = "Capital of Hesse State"
            }
        };
    }
}