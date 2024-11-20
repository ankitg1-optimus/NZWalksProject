using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalksAPI.Data;
using NZWalksAPI.Models;

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
            var regions = this.DbContext.Regions.ToList();
            return Ok(regions);
        }
    }
}
