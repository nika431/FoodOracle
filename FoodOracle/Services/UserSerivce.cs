using FoodOracle.API.Data;
using FoodOracle.API.Models;
using FoodOracle.Models;
using Microsoft.AspNetCore.Http.Connections;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FoodOracle.Services
{
    public class UserSerivce
    {
        private readonly ApplicationDbContext _context;

        public UserSerivce(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<List<CustomerWithFoodDto>> UserInfo(string username, int userId)
        {
            var customers = await _context.Users
            .Where(u => u.Username == username && u.Id == userId)
            .Select(u => new CustomerWithFoodDto
            {
                Id = u.Id,
                Username = u.Username,
                FoodItems = u.FoodItems

            .Where(f => f.CustomerId == userId)
            .Select(f => new FoodItemDto
            {
                Name = f.Name,
                Quantity = f.Quantity,
                ExpiryDate = f.ExpiryDate
            }).ToList()
            })
            .ToListAsync();

            return customers;

        }
        public async Task<List<CustomerWithFoodDto>> GetAllUsers()
        {
            return await _context.Users
                .Select(u => new CustomerWithFoodDto
                {
                    Id = u.Id,
                    Username = u.Username
                })
                .ToListAsync();
        }

    }
}