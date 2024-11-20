using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalksAPI.Data;
using NZWalksAPI.Models;
using NZWalksAPI.Models.DTO;

namespace NZWalksAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegionsController : ControllerBase
    {
        public RegionsController(DataContext DbContext)
        {
            this.DbContext = DbContext;
        }

        public readonly DataContext DbContext;

        [HttpGet]
        public IActionResult GetAll()
        {
            //domain models
            var regionsDomain = this.DbContext.Regions.ToList();
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
            //var region = this.DbContext.Regions.FirstOrDefault(x=> x.Id==id);
            //GET region Domain Model
            var regionDomain = this.DbContext.Regions.Find(id);
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
    }
}
