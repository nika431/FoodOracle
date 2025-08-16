using FoodOracle.API.Models;
using FoodOracle.Dtos;
using FoodOracle.Models;
using FoodOracle.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace FoodOracle.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly CustomerService _userService;

        public AuthController(CustomerService userService)
        {
            _userService = userService;
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] CustomerRegisterDto dto)
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
        public async Task<IActionResult> Login(CustomeerLoginDto dto)
        {
            try
            {
                var userExists = await _userService.DoesUserExist(dto.Username);
                if (!userExists)
                {
                    return BadRequest(new { error = "User not registered." });
                }

                var result = await _userService.Login(dto.Username, dto.Password);
                if (result == null) return Unauthorized();

                return Ok(new
                {
                    token = result.Token,
                    customerId = result.UserId,
                    username = dto.Username
                });
            }
            catch (InvalidOperationException ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }
    }
}