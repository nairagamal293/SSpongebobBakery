using Microsoft.AspNetCore.Mvc;
using SpongPopBakery.DTOs;
using SpongPopBakery.Services;
using SpongPopBakery.Utilities;

namespace SpongPopBakery.Controllers
{
    // Controllers/AccountController.cs
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IConfiguration _config;

        public AccountController(IUserService userService, IConfiguration config)
        {
            _userService = userService;
            _config = config;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserRegisterDto userRegisterDto)
        {
            try
            {
                var user = await _userService.Register(userRegisterDto);
                var token = _userService.GenerateJwtToken(user);
                return Ok(new UserDto
                {
                    Username = user.Username,
                    Email = user.Email,
                    Token = token
                });
            }
            catch (AppException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserLoginDto userLoginDto)
        {
            try
            {
                var user = await _userService.Login(userLoginDto);
                var token = _userService.GenerateJwtToken(user);
                return Ok(new UserDto
                {
                    Username = user.Username,
                    Email = user.Email,
                    Token = token
                });
            }
            catch (AppException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
