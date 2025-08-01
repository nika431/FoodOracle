using FoodOracle.API.Data;
using FoodOracle.API.Models;
using FoodOracle.Models;
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
        public async Task<List<string>> UserInfo(string username)
        {
            var allUsers = await _context.Users
                .Select(u => u.Username)
                .ToListAsync();

            return allUsers;
        }

    }
}