using APIFundamentals.Entities;

namespace APIFundamentals.Services;

public interface ICityInfoRepository
{
    Task<bool> CityExistsAsync(int cityId);
    Task<IEnumerable<City>> GetCitiesAsync();

    Task<City?> GetCityAsync(int cityId,bool showPointOfInterest);

    Task<IEnumerable<PointOfInterest>> GetPointsOfInterestAsync(int cityId);

    Task<PointOfInterest?> GetPointOfInterestAsync(int cityId, int pointOfInterestId);

    Task AddPointOfInterestAsync(int cityId, PointOfInterest pointOfInterest);

    Task<bool> SaveChangesAsync();
    
    void DeletePointOfInterest(PointOfInterest pointOfInterest);

}