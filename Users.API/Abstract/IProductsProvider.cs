namespace InnoShop.Users.API.Abstract
{
    /// <summary>
    /// Describes operations with products service
    /// </summary>
    public interface IProductsProvider
    {
        /// <summary>
        /// Updates products catalog availability according to user availability.
        /// </summary>
        /// <param name="userId">User Id.</param>
        /// <param name="availability">User availability.</param>
        /// <returns></returns>
        Task<(bool Success, string? ErrorMessage)> AdjustUserAvailabilityAsync(int userId, bool availability);
    }
}
