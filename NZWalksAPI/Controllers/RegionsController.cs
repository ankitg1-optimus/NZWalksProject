using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        public RegionsController(IRegionRepository regionRepository)
        {
            RegionRepository = regionRepository;
        }

        public IRegionRepository RegionRepository { get; }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            //domain models
            var regionsDomain = await RegionRepository.GetAllAsync();
            //map Domain models to DTOs
            var regionsDTO = new List<RegionDTO>();

            foreach (var regionDomain in regionsDomain) { 
                regionsDTO.Add(new RegionDTO
                {
                    //Id = regionDomain.Id,
                    Code = regionDomain.Code,
                    Name = regionDomain.Name,
                    RegionImageUrl = regionDomain.RegionImageUrl
                });
            }
            //return DTOs
            return Ok(regionsDTO);
        }

        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id) 
        {
            //var region = this.dbContext.Regions.FirstOrDefault(x=> x.Id==id);
            //GET region Domain Model
            var regionDomain = await RegionRepository.GetByIdAsync(id);
            if (regionDomain == null) 
            {
                return NotFound();
            }
            //Map domain model to dto
            var regionDTO = new RegionDTO
            {
                //Id = regionDomain.Id,
                Code = regionDomain.Code,
                Name = regionDomain.Name,
                RegionImageUrl = regionDomain.RegionImageUrl
            };
            //return DTO
            return Ok(regionDTO);
        }

        [HttpPost]
        public async Task<IActionResult> CreateRegion([FromBody]RegionDTO regionDTO) {
            try
            {
                var regionDomain = new Region
                {
                    Code = regionDTO.Code,
                    Name = regionDTO.Name,
                    RegionImageUrl = regionDTO.RegionImageUrl
                };

                regionDomain = await RegionRepository.AddRegionAsync(regionDomain);

                var addedRegion = new RegionDTO
                {
                    Code = regionDomain.Code,
                    Name = regionDomain.Name,
                    RegionImageUrl = regionDomain.RegionImageUrl
                };
                return CreatedAtAction(nameof(GetById), new {id = regionDomain.Id}, addedRegion);
            }
            catch (Exception) {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> UpdateRegion([FromRoute]Guid id, [FromBody]RegionDTO regionDTO ) {
            try
            {
                //map DTO to Domain Model
                var regionDomainModel = new Region
                {
                    Code = regionDTO.Code,
                    Name = regionDTO.Name,
                    RegionImageUrl = regionDTO.RegionImageUrl
                };
                regionDomainModel = await RegionRepository.UpdateRegionAsync(id, regionDomainModel);

                if (regionDomainModel == null) {
                    return NotFound();
                }
                var updatedModel = new RegionDTO
                {
                    Code = regionDomainModel.Code,
                    Name = regionDomainModel.Name,
                    RegionImageUrl = regionDomainModel.RegionImageUrl
                };
                return Ok(updatedModel);
            }
            catch (Exception) {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        
        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> DeleteRegion([FromRoute]Guid id) {
            try
            {
                var regionDomainModel = await RegionRepository.DeleteRegionAsync(id);
                if (regionDomainModel == null) 
                {
                    return NotFound();
                }

                var deletedModel = new RegionDTO
                {
                    Code = regionDomainModel.Code,
                    Name = regionDomainModel.Name,
                    RegionImageUrl = regionDomainModel.RegionImageUrl
                };
                return Ok(deletedModel);
            }
            catch (Exception) {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

    }
}
