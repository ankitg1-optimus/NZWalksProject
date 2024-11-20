using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalksAPI.Data;
using NZWalksAPI.Models;
using NZWalksAPI.Models.Domain;
using NZWalksAPI.Models.DTO;

namespace NZWalksAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegionsController : ControllerBase
    {
        public RegionsController(DataContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public readonly DataContext dbContext;

        [HttpGet]
        public IActionResult GetAll()
        {
            //domain models
            var regionsDomain = this.dbContext.Regions.ToList();
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
        public IActionResult GetById([FromRoute] Guid id) 
        {
            //var region = this.dbContext.Regions.FirstOrDefault(x=> x.Id==id);
            //GET region Domain Model
            var regionDomain = this.dbContext.Regions.Find(id);
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
        public IActionResult CreateRegion([FromBody]RegionDTO regionDTO) {
            try
            {
                var regionDomain = new Region
                {
                    Code = regionDTO.Code,
                    Name = regionDTO.Name,
                    RegionImageUrl = regionDTO.RegionImageUrl
                };

                dbContext.Regions.Add(regionDomain);
                dbContext.SaveChanges();

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
        public IActionResult UpdateRegion([FromRoute]Guid id, [FromBody]RegionDTO regionDTO ) {
            try
            {
                var regionDomainModel = dbContext.Regions.Find(id);
                if (regionDomainModel == null) 
                {
                    return NotFound();
                }

                regionDomainModel.Code = regionDTO.Code;
                regionDomainModel.Name = regionDTO.Name;
                regionDomainModel.RegionImageUrl = regionDTO.RegionImageUrl;
                dbContext.SaveChanges();

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
        public IActionResult DeleteRegion([FromRoute]Guid id) {
            try
            {
                var regionDomainModel = dbContext.Regions.Find(id);
                if (regionDomainModel == null) 
                {
                    return NotFound();
                }

                dbContext.Regions.Remove(regionDomainModel);
                dbContext.SaveChanges();

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
