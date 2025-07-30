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
        public async Task<ActionResult> SendUserInfo([FromBody] string username)
        {
            var result = await _userService.UserInfo(username);
            return Ok(result);
        }



    }
}
