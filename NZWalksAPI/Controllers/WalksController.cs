﻿using AutoMapper;
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
        public async Task<IActionResult> Create([FromBody] AddUpdateWalkDTO addWalkDTO)
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
        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetById([FromRoute]Guid id)
        {
            var result = await _walkRepository.GetByIdAsync(id);
            if(result== null) return NotFound();
            return Ok(_mapper.Map<WalkDTO>(result));
        }

        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Update([FromRoute]Guid id, AddUpdateWalkDTO walkDTO)
        {
            var updatedModel = await _walkRepository.UpdateAsync(id, _mapper.Map<Walk>(walkDTO));
            if(updatedModel== null) return NotFound();
            return Ok(_mapper.Map<WalkDTO>(updatedModel));
        }
        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var deletedModel = await _walkRepository.DeleteAsync(id);
            return Ok(_mapper.Map<WalkDTO>(deletedModel));
        }
}
}