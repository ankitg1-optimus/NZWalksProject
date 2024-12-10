using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NZWalksAPI.Models.DTO;
using NZWalksAPI.Repositories;

namespace NZWalksAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        public AuthController(UserManager<IdentityUser> userManager, ITokenRepository tokenRepository)
        {
            _userManager = userManager;
            _tokenRepository = tokenRepository;
        }

        private readonly UserManager<IdentityUser> _userManager;
        private readonly ITokenRepository _tokenRepository;

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDTO registerRequestDTO)
        {
            var identityUser = new IdentityUser
            {
                UserName = registerRequestDTO.Username,
                Email = registerRequestDTO.Username
            };
            var identityResult = await _userManager.CreateAsync(identityUser, registerRequestDTO.Password);
            if (identityResult.Succeeded)
            {
                if (registerRequestDTO.Roles != null || registerRequestDTO.Roles.Any())
                {
                    identityResult = await _userManager.AddToRolesAsync(identityUser, registerRequestDTO.Roles);
                    if (identityResult.Succeeded)
                    {
                        return Ok("User was registered!");
                    }
                }
            }
            return BadRequest("Something went wrong");
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO loginRequestDTO)
        {
            var user = await _userManager.FindByEmailAsync(loginRequestDTO.Username);
            if (user != null) {
               var checkPassword = await _userManager.CheckPasswordAsync(user, loginRequestDTO.Password);
                if (checkPassword) {
                    //roles
                    var roles = await _userManager.GetRolesAsync(user);
                    if (roles != null)
                    {
                        //create Token
                        var jwtToken = _tokenRepository.CreateJWTToken(user, roles.ToList());
                        var response = new LoginResponseDTO
                        {
                            JwtToken = jwtToken,
                        };
                        return Ok(response);
                    }
                }
            }
            return BadRequest("Invalid Username or Password");

        }
    }
}
