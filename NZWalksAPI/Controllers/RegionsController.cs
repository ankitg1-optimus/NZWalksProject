using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZWalksAPI.CustomActionFilters;
using NZWalksAPI.Data;
using NZWalksAPI.Models;
using NZWalksAPI.Models.Domain;
using NZWalksAPI.Models.DTO;
using NZWalksAPI.Repositories;

namespace NZWalksAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegionsController : ControllerBase
    {
        private readonly IMapper _mapper;

        public RegionsController(IRegionRepository regionRepository, IMapper mapper)
        {
            _regionRepository = regionRepository;
            _mapper = mapper;
        }

        private IRegionRepository _regionRepository { get; }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            //domain models
            var regionsDomain = await _regionRepository.GetAllAsync();
            //return as dtos
            return Ok(_mapper.Map<List<RegionDTO>>(regionsDomain));
        }

        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id) 
        {
            //var region = this.dbContext.Regions.FirstOrDefault(x=> x.Id==id);
            //GET region Domain Model
            var regionDomain = await _regionRepository.GetByIdAsync(id);
            if (regionDomain == null) 
            {
                return NotFound();
            }
            
            //map and return DTO
            return Ok(_mapper.Map<RegionDTO>(regionDomain));
        }

        [HttpPost]
        [ValidateModel]
        public async Task<IActionResult> CreateRegion([FromBody]RegionDTO regionDTO) {
            try
            {
                    ///map to domain model
                    var regionDomain = _mapper.Map<Region>(regionDTO);
                    //add model
                    regionDomain = await _regionRepository.AddRegionAsync(regionDomain);
                    //map back to DTO
                    var addedRegion = _mapper.Map<RegionDTO>(regionDomain);

                    return CreatedAtAction(nameof(GetById), new { id = regionDomain.Id }, addedRegion);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPut]
        [Route("{id:Guid}")]
        [ValidateModel]
        public async Task<IActionResult> UpdateRegion([FromRoute]Guid id, [FromBody]RegionDTO regionDTO ) {
            try
            {
                    //map DTO to Domain Model
                    var regionDomainModel = _mapper.Map<Region>(regionDTO);
                    regionDomainModel = await _regionRepository.UpdateRegionAsync(id, regionDomainModel);

                    if (regionDomainModel == null)
                    {
                        return NotFound();
                    }
                    return Ok(_mapper.Map<RegionDTO>(regionDomainModel));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        
        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> DeleteRegion([FromRoute]Guid id) {
            try
            {
                var regionDomainModel = await _regionRepository.DeleteRegionAsync(id);
                if (regionDomainModel == null) 
                {
                    return NotFound();
                }
                return Ok(_mapper.Map<RegionDTO>(regionDomainModel));
            }
            catch (Exception) {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

    }
}
