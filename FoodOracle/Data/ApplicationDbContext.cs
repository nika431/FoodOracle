using FoodOracle.API.Models;
using FoodOracle.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace FoodOracle.API.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<FoodItem> FoodItems { get; set; }

        public DbSet<Customer> Users { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Customer>()
                .HasMany(c => c.FoodItems)
                .WithOne(f => f.Customer)
                .HasForeignKey(f => f.CustomerId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
