using APIFundamentals.Entities;
using APIFundamentals.Models;
using APIFundamentals.Services;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace APIFundamentals.Controllers
{
    [Route("api/cities/{cityId}/pointsOfInterest")]
    [Authorize]
    [ApiController]
    public class PointOfInterestController : ControllerBase
    {
        private readonly ILogger<PointOfInterestController> _logger;
        private readonly IMailService _mailService;
        private readonly IMapper _mapper;
        private readonly ICityInfoRepository _cityInfoRepository;

        //Constructor Dependency Injection
        public PointOfInterestController(ILogger<PointOfInterestController> logger,
            IMailService mailService,
            ICityInfoRepository cityInfoRepository,
            IMapper mapper)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mailService = mailService ?? throw new ArgumentNullException(nameof(mailService));
            _mapper = mapper?? throw new ArgumentNullException(nameof(mapper));
            _cityInfoRepository = cityInfoRepository ?? throw new ArgumentNullException(nameof(cityInfoRepository));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PointsOfInterestDto>>> GetPointsOfInterest(int cityId)
        {
            try
            {
                var cityName = User.Claims.FirstOrDefault(c => c.Type == "city")?.Value;

                if (!await _cityInfoRepository.CityNameMatchesCityId(cityName, cityId))
                {
                    return Forbid();
                }
                
                if(!await _cityInfoRepository.CityExistsAsync(cityId))
                {
                    _logger.LogInformation($"City with Id {cityId} was not found");
                    return NotFound();
                }
                
                var pointOfInterest = await _cityInfoRepository.GetPointsOfInterestAsync(cityId);

                return Ok(_mapper.Map<IEnumerable<PointsOfInterestDto>>(pointOfInterest));

            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Exception occured for city Id {cityId}",ex);
                return StatusCode(500, "Problem happened while handling your request.");
            }
           
        }

        [HttpGet("{pointOfInterestId}", Name = "GetSinglePointOfInterest")]
         public async Task<ActionResult<PointsOfInterestDto>> GetSinglePointOfInterest(int cityId, int pointOfInterestId)
         {
            if(!await _cityInfoRepository.CityExistsAsync(cityId))
            {
                _logger.LogInformation($"City with Id {cityId} was not found");
                return NotFound();
            }

            var pointOfInterest = await _cityInfoRepository.GetPointOfInterestAsync(cityId, pointOfInterestId);

            if (pointOfInterest == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<PointsOfInterestDto>(pointOfInterest));

         }

        [HttpPost]
        public async Task<ActionResult<PointsOfInterestDto>> CreatePointOfInterest(int cityId,
            PointOfInterestCreateDto pointOfInterest)
        {
            if(!await _cityInfoRepository.CityExistsAsync(cityId))
            {
                _logger.LogInformation($"City with Id {cityId} was not found");
                return NotFound();
            }

            var finalPointOfInterest = _mapper.Map<Entities.PointOfInterest>(pointOfInterest);

            await _cityInfoRepository.AddPointOfInterestAsync(cityId, finalPointOfInterest);

            await _cityInfoRepository.SaveChangesAsync();
            
            var newPointOfInterest = _mapper.Map<Models.PointsOfInterestDto>(finalPointOfInterest);

            return CreatedAtRoute("GetSinglePointOfInterest",
                new
                {
                    cityId = cityId,
                    pointOfInterestId = newPointOfInterest.Id
                }, newPointOfInterest
            );

        }
        
        [HttpPut("{pointOfInterestId}")]
        public async Task<ActionResult> UpdatePointOfInterest(int cityId, int pointOfInterestId,
            PointOfInterestUpdateDto pointOfInterest)
        {
            if(!await _cityInfoRepository.CityExistsAsync(cityId))
            {
                _logger.LogInformation($"City with Id {cityId} was not found");
                return NotFound();
            }
            var pointOfInterestToUpdate = await _cityInfoRepository.GetPointOfInterestAsync(cityId, pointOfInterestId);

            if (pointOfInterestToUpdate == null)
            {
                return NotFound();
            }

            _mapper.Map(pointOfInterest, pointOfInterestToUpdate);

            await _cityInfoRepository.SaveChangesAsync();

            return NoContent();

        }
        
        [HttpPatch("{pointOfInterestId}")]
        
        public async Task<ActionResult> PatchUpdatePointOfInterest(int cityId, int pointOfInterestId,
            JsonPatchDocument<PointOfInterestUpdateDto> patchDocument)
        {
            if(!await _cityInfoRepository.CityExistsAsync(cityId))
            {
                _logger.LogInformation($"City with Id {cityId} was not found");
                return NotFound();
            }
            var pointOfInterestEntity = await _cityInfoRepository.GetPointOfInterestAsync(cityId, pointOfInterestId);

            if (pointOfInterestEntity == null)
            {
                return NotFound();
            }

            var pointOfInterestToPatch = _mapper.Map<PointOfInterestUpdateDto>(pointOfInterestEntity);
            
            patchDocument.ApplyTo(pointOfInterestToPatch, ModelState);
            
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            if (!TryValidateModel(pointOfInterestToPatch))
            {
                return BadRequest(ModelState);
            }

            _mapper.Map(pointOfInterestToPatch, pointOfInterestEntity);
            
            await _cityInfoRepository.SaveChangesAsync();
            
            return NoContent();
        
        }
        
        [HttpDelete("{pointOfInterestId}")]
        
        public async Task<ActionResult> DeletePointOfInterest(int cityId, int pointOfInterestId)
        {
            if(!await _cityInfoRepository.CityExistsAsync(cityId))
            {
                _logger.LogInformation($"City with Id {cityId} was not found");
                return NotFound();
            }
            var pointOfInterestEntity = await _cityInfoRepository.GetPointOfInterestAsync(cityId, pointOfInterestId);

            if (pointOfInterestEntity == null)
            {
                return NotFound();
            }
            
            _cityInfoRepository.DeletePointOfInterest(pointOfInterestEntity);
            
            await _cityInfoRepository.SaveChangesAsync();

            _mailService.SendMail("Deleted Point Of Interest",
                $"Point Of Interest: {pointOfInterestEntity.Name} with Id {pointOfInterestEntity.Id} has been deleted.");
            return NoContent();
        }

    }
}
