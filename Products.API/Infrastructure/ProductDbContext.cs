using Microsoft.EntityFrameworkCore;
using InnoShop.Products.API.Models;

namespace InnoShop.Products.API.Infrastructure
{
    /// <summary>
    /// Class describes InnoShop Products data context.
    /// </summary>
    public class ProductsDbContext : DbContext
    {
        public required DbSet<Product> Products { get; set; }

        public ProductsDbContext(DbContextOptions<ProductsDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // set Id as primary key
            modelBuilder.Entity<Product>().HasKey(p => p.Id);

            modelBuilder.Entity<Product>().HasIndex(p => p.UserId).HasDatabaseName("IX_Product_User");

            modelBuilder.Entity<Product>().HasIndex(p => p.Title).HasDatabaseName("IX_Product_Title");

            modelBuilder.Entity<Product>().HasIndex(p => p.Price).HasDatabaseName("IX_Product_Price");

            modelBuilder.Entity<Product>().HasIndex(p => p.IsAvailable).HasDatabaseName("IX_Product_Availability");

            modelBuilder.Entity<Product>().HasIndex(p => p.IsUserActive).HasDatabaseName("IX_Product_UserActive");

            modelBuilder.Entity<Product>().HasIndex(p => p.IsDeleted).HasDatabaseName("IX_Product_Deleted");

            modelBuilder.Entity<Product>().HasIndex(p => p.CreatedDate).HasDatabaseName("IX_Product_Created");

            modelBuilder.Entity<Product>().HasIndex(p => p.ModifiedDate).HasDatabaseName("IX_Product_Modified");
        }
    }
}
