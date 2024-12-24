using InnoShop.Products.API.Models;

namespace InnoShop.Products.API.Abstract
{
    public interface ILoginProvider
    {
        Task<(bool Success, UserLoginResponse? response, string? ErrorMessage)> LoginAsync(UserLoginInfo loginInfo);
    }
}
