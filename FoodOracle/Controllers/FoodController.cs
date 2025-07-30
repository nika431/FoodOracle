using FoodOracle.API.Models;
using FoodOracle.Models;
using FoodOracle.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FoodOracle.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class FoodController : ControllerBase
    {
        private readonly FoodService _foodService;

        public FoodController(FoodService foodService)
        {
            _foodService = foodService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<FoodItem>>> GetFood(
              [FromQuery] string? searchQuery, 
              [FromQuery] string? sortBy,
              [FromQuery] int pageNumber = 1,
              [FromQuery] int pageSize = 5)
        {
            pageNumber = pageNumber < 1 ? 1 : pageNumber;
            pageSize = pageSize < 1 ? 5 : pageSize;
            pageSize = pageSize > 100 ? 100 : pageSize;

            var foods = await _foodService.GetFoodAsync(searchQuery, sortBy, pageNumber, pageSize);
            return Ok(foods);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<FoodItem>> GetFoodById(int id)
        {
            var item = await _foodService.GetFoodByIdAsync(id);

            if (item == null) { return NotFound(); } 

            return Ok(item); 
        }

        [HttpPost]
        public async Task<ActionResult<FoodItem>> AddFood([FromBody] FoodItem item)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createdItem = await _foodService.AddFoodAsync(item);

            return CreatedAtAction(nameof(GetFoodById), new { id = createdItem.Id }, createdItem);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFood(int id)
        {
            var item = await _foodService.GetFoodByIdAsync(id);

            if (item == null) { return NotFound(); }

            await _foodService.DeleteFoodAsync(item);

            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> EditFood(int id, [FromBody] FoodItem updatedItem)
        {
            if (id != updatedItem.Id)
            {
                return BadRequest("ID mismatch");
            }

            var item = await _foodService.GetFoodByIdAsync(id);
            if (item == null)
            {
                return NotFound();
            }

            item.Name = updatedItem.Name;
            item.ExpiryDate = updatedItem.ExpiryDate;
            item.Quantity = updatedItem.Quantity;

            _foodService.UpdateFood(item);

            var saved = await _foodService.SaveChangesAsync();
            if (!saved)
            {
                return StatusCode(409, "Concurrency conflict detected");
            }

            return Ok(item);
        }

    }
}
