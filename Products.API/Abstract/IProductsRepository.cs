using InnoShop.Products.API.Models;

namespace InnoShop.Products.API.Abstract
{
    public interface IProductsRepository
    {
        /// <summary>
        /// Retrieves a product by Id.
        /// </summary>
        /// <param name="userId">Id of a user.</param>
        /// <param name="productId">Id of a product.</param>
        /// <returns>Product if found, otherwise null.</returns>
        Task<Product?> GetProductByIdAsync(int userId, int productId);

        /// <summary>
        /// Retrieves all products by a user.
        /// </summary>
        /// <param name="userId">User Id to retrieve assigned products.</param>
        /// <param name="filter">Filter values to apply to the result list.</param>
        /// <returns>List of filtered products assigned to the user.</returns>
        Task<List<Product>> GetProductsAsync(int? userId, ProductFilter? filter);

        /// <summary>
        /// Adds a new product to the database.
        /// </summary>
        /// <param name="userId">User Id the product is assigned to.</param>
        /// <param name="productDto">Product data to add.</param>
        /// <returns>Created product data.</returns>
        Task<Product> AddProductAsync(int userId, ProductDto productDto);

        /// <summary>
        /// Updates a product.
        /// </summary>
        /// <param name="userId">User Id the product is assigned to.</param>
        /// <param name="productId">Product Id to update.</param>
        /// <param name="productDto">Product data to update.</param>
        /// <returns>Updated product if found, otherwise null.</returns>
        Task<Product?> UpdateProductAsync(int userId, int productId, ProductDto productDto);

        /// <summary>
        /// Deletes a product by Id.
        /// </summary>
        /// <param name="userId">Id of the user the product is assigned to.</param>
        /// <param name="productId">Id of the product to delete.</param>
        /// <returns>Number of affected records.</returns>
        Task<int> DeleteProductAsync(int userId, int productId);

        /// <summary>
        /// Restores a product by Id.
        /// </summary>
        /// <param name="userId">Id of the user the product is assigned to.</param>
        /// <param name="productId">Id of the product to restore.</param>
        /// <returns>Number of affected records.</returns>
        Task<int> RestoreProductAsync(int userId, int productId);

        /// <summary>
        /// Deactivates products by user Id.
        /// </summary>
        /// <param name="userId">Id of the user.</param>
        /// <returns>Number of affected records.</returns>
        Task<int> DeactivateProductsByUserIdAsync(int userId);

        /// <summary>
        /// Activates products by user Id.
        /// </summary>
        /// <param name="userId">Id of the user.</param>
        /// <returns>Number of affected records.</returns>
        Task<int> ActivateProductsByUserIdAsync(int userId);

    }
}
