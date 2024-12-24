using InnoShop.Products.API.Abstract;
using InnoShop.Products.API.Models;
using Microsoft.EntityFrameworkCore;

namespace InnoShop.Products.API.Infrastructure
{
    public class ProductsRepository : IProductsRepository
    {
        private readonly ProductsDbContext _context;

        public ProductsRepository(ProductsDbContext context)
        {
            _context = context;
        }

        public async Task<Product> AddProductAsync(int userId, ProductDto productDto)
        {
            var product = new Product()
            {
                Title = productDto.Title,
                Description = productDto.Description,
                Price = productDto.Price,
                IsAvailable = productDto.IsAvailable,
                IsDeleted = false,
                IsUserActive = true,
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = null,
                UserId = userId
            };
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return product;
        }

        public async Task<List<Product>> GetProductsAsync(int? userId, ProductFilter? filter)
        {
            var query = _context.Products.Where(p => !p.IsDeleted);

            if (userId.HasValue)
            {
                query = query.Where(p => p.UserId == userId.Value);
            }
            else
            {
                query = query.Where(p => p.IsUserActive && p.IsAvailable);
            }

            // apply filter
            if (filter is not null)
            {
                if (filter.MinPrice.HasValue)
                {
                    query = query.Where(p => p.Price >= filter.MinPrice.Value);
                }

                if (filter.MaxPrice.HasValue)
                {
                    query = query.Where(p => p.Price <= filter.MaxPrice.Value);
                }

                if (filter.IsAvailable.HasValue)
                {
                    query = query.Where(p => p.IsAvailable == filter.IsAvailable.Value);
                }

                if (!string.IsNullOrWhiteSpace(filter.SearchText))
                {
                    query = query.Where(p => p.Title.ToLower().Contains(filter.SearchText.ToLower())
                                          || p.Description.ToLower().Contains(filter.SearchText.ToLower()));
                }

                if (filter.CreatedAfter.HasValue)
                {
                    query = query.Where(p => p.CreatedDate >= filter.CreatedAfter.Value.ToUniversalTime());
                }

                if (filter.CreatedBefore.HasValue)
                {
                    // exclude time for the upper date 
                    query = query.Where(p => p.CreatedDate.Date <= filter.CreatedBefore.Value.Date.ToUniversalTime());
                }
            }

            var result = await query.OrderByDescending(p => p.CreatedDate)
                                    .AsNoTracking()
                                    .ToListAsync();
            return result;
        }

        public async Task<Product?> GetProductByIdAsync(int userId, int productId)
        {
            var result = await _context.Products
                                .AsNoTracking()
                                .FirstOrDefaultAsync(p => p.UserId == userId && p.Id == productId && !p.IsDeleted);
            return result;
        }

        public async Task<Product?> UpdateProductAsync(int userId, int productId, ProductDto productDto)
        {
            var product = await _context.Products
                                        .FirstOrDefaultAsync(p => p.UserId == userId && p.Id == productId && !p.IsDeleted);
            if (product == null) return null;

            product.Title = productDto.Title;
            product.Description = productDto.Description;
            product.Price = productDto.Price;
            product.IsAvailable = productDto.IsAvailable;
            product.ModifiedDate = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return product;
        }

        private async Task<int> SetProductDeletedState(int userId, int productId, bool isDeleted)
        {
            var product = await _context.Products
                                        .FirstOrDefaultAsync(p => p.Id == productId && p.UserId == userId);
            if (product == null) return 0;

            product.IsDeleted = isDeleted;
            return await _context.SaveChangesAsync();
        }

        public async Task<int> DeleteProductAsync(int userId, int productId)
        {
            return await SetProductDeletedState(userId, productId, true);
        }

        public async Task<int> RestoreProductAsync(int userId, int productId)
        {
            return await SetProductDeletedState(userId, productId, false);
        }

        private async Task<int> SetProductAvailabilityByUserId(int userId, bool availability)
        {
            // for more effective work it's possible to use
            // _context.Database.ExecuteSqlRawAsync() to run raw query against the db
            // or EFCore.BulkExtensions package
            // which will do update operations at the db level without loading product objects to the app memory
            var products = _context.Products.Where(p => p.UserId == userId);
            await products.ForEachAsync(p => p.IsUserActive = availability);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> DeactivateProductsByUserIdAsync(int userId)
        {
            return await SetProductAvailabilityByUserId(userId, false);
        }

        public async Task<int> ActivateProductsByUserIdAsync(int userId)
        {
            return await SetProductAvailabilityByUserId(userId, true);
        }
    }
}
