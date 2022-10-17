using APIFundamentals.DBContexts;
using APIFundamentals.Entities;
using APIFundamentals.Models;
using Microsoft.EntityFrameworkCore;
using SQLitePCL;

namespace APIFundamentals.Services;

public class CityInfoRepository : ICityInfoRepository
{
    private readonly CityInfoContext _context;

    public CityInfoRepository(CityInfoContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<bool> CityExistsAsync(int cityId)
    {
        return await _context.Cities.AnyAsync(c => c.Id == cityId);
    }

    public async Task<IEnumerable<City>> GetCitiesAsync()
    {
        return await _context.Cities.OrderBy(c => c.Name).ToListAsync();
    }


    public async Task<IEnumerable<City>> GetCitiesAsync(string? name, string? searchQuery,int pageNumber, int pageSize)
    {
        //Getting all cities
        var citiesList = _context.Cities as IQueryable<City>;

        if (!string.IsNullOrWhiteSpace(name))
        {
            name = name.Trim();
            citiesList = citiesList.Where(c => c.Name == name);
        }
        
        if (!string.IsNullOrWhiteSpace(searchQuery))
        {
            searchQuery = searchQuery.Trim();
            citiesList = citiesList.Where(c => c.Name.Contains(searchQuery) 
                                               || c.Description != null && c.Description.Contains(searchQuery) );

        }

        return await citiesList.OrderBy(c => c.Name)
            .Skip(pageSize * (pageNumber - 1))
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<City?> GetCityAsync(int cityId, bool showPointOfInterest)
    {
        if (showPointOfInterest)
        {
            return await _context.Cities.Include(c => c.PointsOfInterest)
                .Where(c => c.Id == cityId)
                .FirstOrDefaultAsync();

        }
        return await _context.Cities.Where(c => c.Id == cityId)
            .FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<PointOfInterest>> GetPointsOfInterestAsync(int cityId)
    {
        return await _context.PointsOfInterest.Where(c => c.CityId == cityId).ToListAsync();
    }

    public async Task<PointOfInterest?> GetPointOfInterestAsync(int cityId, int pointOfInterestId)
    {
        return await _context.PointsOfInterest
            .Where(p => p.CityId == cityId && p.Id == pointOfInterestId)
            .FirstOrDefaultAsync();
    }

    public async Task AddPointOfInterestAsync(int cityId, PointOfInterest pointOfInterest)
    {
        var city = await GetCityAsync(cityId, false);
        if (city != null)
        {
            city.PointsOfInterest.Add(pointOfInterest);
        }

    }

    public async Task<bool> SaveChangesAsync()
    {
        return (await _context.SaveChangesAsync() >= 0);
    }

    public void DeletePointOfInterest(PointOfInterest pointOfInterest)
    {
        _context.PointsOfInterest.Remove(pointOfInterest);
    }
}