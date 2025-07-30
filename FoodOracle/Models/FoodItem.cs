using FoodOracle.Models;

namespace FoodOracle.API.Models
{
    public class FoodItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime ExpiryDate { get; set; }
        public int Quantity { get; set; }
    }
}
