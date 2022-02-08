using Microsoft.EntityFrameworkCore;
using System;

namespace GroceriesStoreSystem.Models
{
    public class MarketContext : DbContext
    {
        public MarketContext(DbContextOptions<MarketContext> options)
            : base(options)
        {
        }

        public DbSet<Groceries> Groceries { get; set; }
        public DbSet<ProductCategory> ProductCategory { get; set; }
        public DbSet<Brands> Brands { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=MarketApp;MultipleActiveResultSets=true;User ID=sa;Password=1qaz2wsx3edc;");
            }
        }
    }
}
