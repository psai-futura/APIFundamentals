using APIFundamentals.Models;
using APIFundamentals.Services;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace APIFundamentals.Controllers;

[ApiController]
[Route("api/cities")]
public class CitiesController : ControllerBase
{
    private readonly ICityInfoRepository _cityInfoRepository;

    private readonly IMapper _mapper;

    const int maxCitiesPageSize = 20;
    

    public CitiesController(ICityInfoRepository cityInfoRepository, IMapper mapper)
    {
        _cityInfoRepository = cityInfoRepository ?? throw new ArgumentNullException(nameof(cityInfoRepository));

       _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    [HttpGet]
    public async Task<ActionResult< IEnumerable<CityWithoutPointOfInterestDto>>> GetCities(string? name,
        string? searchQuery, int pageNumber = 1, int pageSize = 10)
    {
        if (pageSize > maxCitiesPageSize)
        {
            pageSize = maxCitiesPageSize;
        }
        var cityEntities = await _cityInfoRepository.GetCitiesAsync(name,searchQuery,pageNumber,pageSize);

        return Ok(_mapper.Map<IEnumerable<CityWithoutPointOfInterestDto>>(cityEntities));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetCity(int id, bool showPointsOfInterest= false)
    {
        var city = await _cityInfoRepository.GetCityAsync(id, showPointsOfInterest);

        if (city == null)
        {
            return NotFound();
        }
        if (showPointsOfInterest)
        {
            return Ok(_mapper.Map<CityDto>(city));
        }
        return Ok(_mapper.Map<CityWithoutPointOfInterestDto>(city));
    }
}

