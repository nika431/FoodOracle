using FoodOracle.Models;
using FoodOracle.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FoodOracle.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UserSerivce _userService;

        public UserController(UserSerivce userService)
        {
            _userService = userService;
        }
        [HttpPost]
        public async Task<ActionResult<CustomerWithFoodDto>> SendUserInfo([FromBody] UserInfoRequeest request)
        {
            var result = await _userService.UserInfo(request.Username, request.UserId);
            return Ok(result);
            
        }
        [HttpPost("all")]
        public async Task<ActionResult<List<CustomerWithFoodDto>>> GetAllUsers()
        {
            var users = await _userService.GetAllUsers();
            if (users == null || !users.Any())
            {
                return NotFound("No users found.");
            }
            return Ok(users);
        }
    }
}