using MediatR;
using InnoShop.Products.API.Models;

namespace InnoShop.Products.API.Abstract.Operations
{
    #region Oueries

    public record AuthUserByLoginInfoQuery(UserLoginInfo loginInfo) : IRequest<(bool success, UserLoginResponse? response, string? errorMessage)>;

    public record GetProductsQuery(int? userId, ProductFilter? filter) : IRequest<IEnumerable<Product>>;

    public record GetProductByIdQuery(int userId, int productId) : IRequest<Product?>;

    #endregion

    #region Commands

    public record AddProductCommand(int userId, ProductDto productDto) : IRequest<Product>;

    public record UpdateProductCommand(int userId, int productId, ProductDto productDto) : IRequest<Product?>;

    public record DeleteProductCommand(int userId, int productId) : IRequest<int>;

    public record RestoreProductCommand(int userId, int productId) : IRequest<int>;

    public record DeactivateProductsByUserIdCommand(int userId) : IRequest<int>;

    public record ActivateProductsByUserIdCommand(int userId) : IRequest<int>;

    #endregion
}
