using System.ComponentModel.DataAnnotations;
using FoodOracle.API.Models;

namespace FoodOracle.Models
{
    public class Customer
    {
        public int Id { get; set; }

        [Required]
        public string Username { get; set; } = string.Empty;

        [Required]
        public byte[] PasswordHash { get; set; }

        [Required]
        public byte[] PasswordSalt { get; set; }
        public ICollection<FoodItem> FoodItems { get; set; } = new List<FoodItem>();
    }
}
