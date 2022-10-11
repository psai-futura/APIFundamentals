using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APIFundamentals.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APIFundamentals.Controllers
{
    [Route("api/cities/{cityId}/pointsOfInterest")]
    [ApiController]
    public class PointOfInterestController : ControllerBase
    {
        [HttpGet]
        public ActionResult<IEnumerable<PointsOfInterestDto>> GetPointsOfInterest(int cityId)
        {
            var city = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == cityId);

            if (city == null)
                return NotFound();

            return Ok(city.PointsOfInterest);
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

    }
}
