using APIFundamentals.Entities;

namespace APIFundamentals.Services;

public interface ICityInfoRepository
{
    Task<bool> CityExistsAsync(int cityId);
    Task<IEnumerable<City>> GetCitiesAsync();
    Task<(IEnumerable<City>, PaginationMetaData)> GetCitiesAsync(string? name, string? searchQuery, int pageNumber, int pageSize);

    Task<City?> GetCityAsync(int cityId,bool showPointOfInterest);

    Task<IEnumerable<PointOfInterest>> GetPointsOfInterestAsync(int cityId);

    Task<PointOfInterest?> GetPointOfInterestAsync(int cityId, int pointOfInterestId);

    Task AddPointOfInterestAsync(int cityId, PointOfInterest pointOfInterest);

    Task<bool> SaveChangesAsync();
    
    void DeletePointOfInterest(PointOfInterest pointOfInterest);

    Task<bool> CityNameMatchesCityId(string? cityName, int cityId);

}