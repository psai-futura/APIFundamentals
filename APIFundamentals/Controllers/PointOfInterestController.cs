using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using APIFundamentals.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace APIFundamentals.Controllers
{
    [Route("api/cities/{cityId}/pointsOfInterest")]
    [ApiController]
    public class PointOfInterestController : ControllerBase
    {
        private readonly ILogger<PointOfInterestController> _logger;
        public PointOfInterestController(ILogger<PointOfInterestController> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        
        
        
        // public bool ValidateCity(int cityId, ref CityDto? cityData)
        // {
        //     //cityData = null;
        //     var city = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == cityId);
        //     
        //     if(city == null)
        //         return false;
        //     else
        //     {
        //         cityData = city;
        //         return true;
        //     }
        // }
        
        [HttpGet]
        public ActionResult<IEnumerable<PointsOfInterestDto>> GetPointsOfInterest(int cityId)
        {
            try
            {
               // throw new Exception("Errrrror");
                var city = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == cityId);
                if (city == null)
                {
                    _logger.LogInformation($"City Id {cityId} not found!");
                    return NotFound();
                }

                return Ok(city.PointsOfInterest);

            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Exception occured for city Id {cityId}",ex);
                return StatusCode(500, "Problem happened while handling your request.");
            }
           
        }

        [HttpGet("{pointOfInterestId}", Name = "GetSinglePointOfInterest")]
        public ActionResult<PointsOfInterestDto> GetSinglePointOfInterest(int cityId, int pointOfInterestId)
        {
        
            var city = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == cityId);
            
            if (city == null)
                return NotFound();

            var pointOfInterest = city.PointsOfInterest.FirstOrDefault(p => p.Id == pointOfInterestId);
            
            if (pointOfInterest == null)
            {
                return NotFound();
            }
            
            return Ok(pointOfInterest);
        
        }

        [HttpPost]
        public ActionResult<PointsOfInterestDto> CreatePointOfInterest(int cityId,
            PointOfInterestCreateDto pointOfInterest)
        {
            
            var city = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == cityId);

            if (city == null)
            {
                return NotFound();
            }

            var maxInterestId = city.NumberOfPointsOfInterest;

            // var maxInterestId = CitiesDataStore.Current.Cities.
            //                         SelectMany(c => c.PointsOfInterest).Max(p => p.Id);

            var newPointOfInterest = new PointsOfInterestDto()
            {
                Id = ++maxInterestId,
                Name = pointOfInterest.Name,
                Description = pointOfInterest.Description
            };
            
            city.PointsOfInterest.Add(newPointOfInterest);

            return CreatedAtRoute("GetSinglePointOfInterest",
                new
                {
                    cityId = cityId,
                    pointOfInterestId = newPointOfInterest.Id
                }, newPointOfInterest
            );

        }

        [HttpPut("{pointOfInterestId}")]
        public ActionResult UpdatePointOfInterest(int cityId, int pointOfInterestId,
            PointOfInterestUpdateDto pointOfInterest)
        {
            var city = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == cityId);
            
            if (city == null)
                return NotFound();

            var pointOfInterestForCity = city.PointsOfInterest.FirstOrDefault(p => p.Id == pointOfInterestId);

            if (pointOfInterestForCity == null)
            {
                return NotFound();
            }

            pointOfInterestForCity.Name = pointOfInterest.Name;
            pointOfInterestForCity.Description = pointOfInterest.Description;

            return NoContent();

        }

        [HttpPatch("{pointOfInterestId}")]

        public ActionResult PatchUpdatePointOfInterest(int cityId, int pointOfInterestId,
            JsonPatchDocument<PointOfInterestUpdateDto> patchDocument)
        {
            var city = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == cityId);
            
            if (city == null)
                return NotFound();

            var pointOfInterestForCity = city.PointsOfInterest.FirstOrDefault(p => p.Id == pointOfInterestId);

            if (pointOfInterestForCity == null)
            {
                return NotFound();
            }

            var pointOfInterestToPatch = new PointOfInterestUpdateDto()
                                        {
                                            Name = pointOfInterestForCity.Name,
                                            Description = pointOfInterestForCity.Description
                                        };
            patchDocument.ApplyTo(pointOfInterestToPatch, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!TryValidateModel(pointOfInterestToPatch))
            {
                return BadRequest(ModelState);
            }
            pointOfInterestForCity.Name = pointOfInterestToPatch.Name;
            pointOfInterestForCity.Description = pointOfInterestToPatch.Description;

            return NoContent();

        }

        [HttpDelete("{pointOfInterestId}")]

        public ActionResult DeletePointOfInterest(int cityId, int pointOfInterestId)
        {
            var city = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == cityId);
            
            if (city == null)
                return NotFound();

            var pointOfInterestForCity = city.PointsOfInterest.FirstOrDefault(p => p.Id == pointOfInterestId);

            if (pointOfInterestForCity == null)
            {
                return NotFound();
            }

            city.PointsOfInterest.Remove(pointOfInterestForCity);
            return NoContent();
        }

    }
}
