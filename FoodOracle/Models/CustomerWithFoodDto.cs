using FoodOracle.API.Models;

namespace FoodOracle.Models
{
    public class CustomerWithFoodDto
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public List<FoodItemDto> FoodItems { get; set; }
    }
}
