using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalksAPI.Models.Domain;
using NZWalksAPI.Models.DTO;
using NZWalksAPI.Repositories;

namespace NZWalksAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WalksController : ControllerBase
    {
        public WalksController(IMapper mapper, IWalkRepository walkRepository)
        {
            _mapper = mapper;
            _walkRepository = walkRepository;
        }

        private readonly IMapper _mapper;
        private readonly IWalkRepository _walkRepository;

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] WalkDTO addWalkDTO)
        {
            //map Dto to domain model
            var walkDomain = _mapper.Map<Walk>(addWalkDTO);
            var createdModel = await _walkRepository.CreateAsync(walkDomain);
            return Ok(_mapper.Map<WalkDTO>(createdModel));
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            //map Dto to domain model
            var walks = await _walkRepository.GetAllAsync();
            return Ok(_mapper.Map<List<WalkDTO>>(walks));
            
        }
    }
}
