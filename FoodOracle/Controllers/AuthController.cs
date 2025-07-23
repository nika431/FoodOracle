using FoodOracle.Dtos;
using FoodOracle.Models;
using FoodOracle.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FoodOracle.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserService _userService;

        public AuthController(UserService userService)
        {
            _userService = userService;
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegisterDto dto)
        {
            try
            {
                var success = await _userService.UserAccountSetup(dto.Username, dto.Password);
                return Ok(new { message = "User registered successfully." });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDto dto)
        {
            try
            {
                // Check if user exists (your service logic might already do this)
                var userExists = await _userService.DoesUserExist(dto.Username);

                if (!userExists)
                {
                    // Username not found = force 400 Bad Request
                    return BadRequest(new { error = "User not registered." });
                }

                var token = await _userService.Login(dto.Username, dto.Password);

                return Ok(new { token });
            }
            catch (InvalidOperationException ex)
            {
                // Wrong password, etc.
                return Unauthorized(new { error = ex.Message });
            }
        }

    }
}
