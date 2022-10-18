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
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/cities")]
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

    /// <summary>
    /// Get a city by Id
    /// </summary>
    /// <param name="id">The id of City to get</param>
    /// <param name="showPointsOfInterest">Whether or not to include the points of interest</param>
    /// <returns>An IActionResult</returns>
    /// <response code="200">Returns the requested city</response>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
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

