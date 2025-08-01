using System.ComponentModel.DataAnnotations;
using FoodOracle.Models;

namespace FoodOracle.Models
{
    public class AddFoodItemDto
    {
        public string Name { get; set; }
        public DateTime ExpiryDate { get; set; }
        public int Quantity { get; set; }
    }

}
