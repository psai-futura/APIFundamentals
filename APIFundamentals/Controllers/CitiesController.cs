using APIFundamentals.Models;
using Microsoft.AspNetCore.Mvc;

namespace APIFundamentals.Controllers;

[ApiController]
[Route("api/cities")]
public class CitiesController : ControllerBase
{

    [HttpGet]
    public ActionResult< IEnumerable<CityDto>> GetCities()
    {
        return Ok(CitiesDataStore.Current.Cities);
       // return new JsonResult(CitiesDataStore.Current.Cities);
    }

    [HttpGet("{id}")]
    public ActionResult<CityDto> GetCity(int id)
    {
        var cityToReturn = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == id);

        if (cityToReturn == null)
            return NotFound();

        return Ok(cityToReturn);
    }
}

