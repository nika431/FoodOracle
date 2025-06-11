using FoodOracle.API.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace FoodOracle.API.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<FoodItem> FoodItems { get; set; }

    }
}
