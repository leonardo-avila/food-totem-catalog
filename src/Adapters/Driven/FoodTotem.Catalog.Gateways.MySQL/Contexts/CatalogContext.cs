using System.Diagnostics.CodeAnalysis;
using FoodTotem.Catalog.Domain.Models;
using FoodTotem.Catalog.Gateways.MySQL.Mappings;
using Microsoft.EntityFrameworkCore;

namespace FoodTotem.Catalog.Gateways.MySQL.Contexts
{
    [ExcludeFromCodeCoverage]
    public class CatalogContext : DbContext
    {
        public CatalogContext(DbContextOptions<CatalogContext> options) : base(options) 
        {
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            ChangeTracker.AutoDetectChangesEnabled = false;
        }

        public DbSet<Food> Foods { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("catalog");

            modelBuilder.ApplyConfiguration(new FoodMap());

            base.OnModelCreating(modelBuilder);
        }
    }
}