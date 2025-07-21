using FoodOracle.API.Data;
using FoodOracle.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FoodOracle.Dtos;
using FoodOracle.Models;

namespace FoodOracle.Services
{
    public class FoodService
    {
        private readonly ApplicationDbContext _context;

        public FoodService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<PagedResult<FoodItem>> GetFoodAsync(string? searchQuery, string? sortBy, int pageNumber = 1, int pageSize = 5)
        {
            var query = _context.FoodItems.AsQueryable();

            if (!string.IsNullOrEmpty(searchQuery))
            {
                query = query.Where(f => f.Name.ToLower().Contains(searchQuery.ToLower()));
            }

            if (!string.IsNullOrEmpty(sortBy))
            {
                query = sortBy.ToLower() switch
                {
                    "date" => query.OrderBy(item => item.ExpiryDate),
                    "name" => query.OrderBy(item => item.Name),
                    _ => query
                };
            }

            var totalCount = await query.CountAsync();

            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<FoodItem>
            {
                Items = items,
                TotalCount = totalCount
            };
        }
        public async Task<FoodItem?> GetFoodByIdAsync(int id)
        {
            return await _context.FoodItems.FindAsync(id);
        }
        public async Task<FoodItem> AddFoodAsync(FoodItem item)
        {
            _context.FoodItems.Add(item);
            await _context.SaveChangesAsync();
            return item;
        }
        public async Task DeleteFoodAsync(FoodItem item)
        {
            _context.FoodItems.Remove(item);
            await _context.SaveChangesAsync();
        }
        public void UpdateFood(FoodItem item)
        {
            _context.Entry(item).State = EntityState.Modified;
        }

        public async Task<bool> SaveChangesAsync()
        {
            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                return false;
            }
        }
    }
}

