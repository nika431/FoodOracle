using FoodOracle.API.Data;
using FoodOracle.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FoodOracle.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FoodController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public FoodController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<FoodItem>>> GetFood()
        {
            var items = await _context.FoodItems.ToListAsync();
            return Ok(items);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<FoodItem>> GetFoodById(int id)
        {
            var item = await _context.FoodItems.FindAsync(id);

            if (item == null)
            {
                return NotFound();
            }

            return Ok(item);
        }

        [HttpPost]
        public async Task<ActionResult<FoodItem>> AddFood([FromBody] FoodItem item)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.FoodItems.Add(item);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetFoodById), new { id = item.Id }, item);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFood(int id)
        {
            var item = await _context.FoodItems.FindAsync(id);
            if (item == null)
            {
                return NotFound();
            }

            _context.FoodItems.Remove(item);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> EditFood(int id, [FromBody] FoodItem updatedItem)
        {
            if (id != updatedItem.Id)
            {
                return BadRequest("ID mismatch");
            }

            var item = await _context.FoodItems.FindAsync(id);
            if (item == null)
            {
                return NotFound();
            }

            item.Name = updatedItem.Name;
            item.ExpiryDate = updatedItem.ExpiryDate;
            item.Quantity = updatedItem.Quantity;

            _context.Entry(item).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }

            return Ok(item);
        }
    }
}
