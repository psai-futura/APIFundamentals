using APIFundamentals.Models;
using APIFundamentals.Services;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace APIFundamentals.Controllers;

[ApiController]
[Authorize]
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
        var (cityEntities, paginationMetaData) = await _cityInfoRepository
            .GetCitiesAsync(name,searchQuery,pageNumber,pageSize);
        
        Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(paginationMetaData));

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

