using InnoShop.Products.API.Abstract;
using InnoShop.Products.API.Models;

namespace InnoShop.Products.API.Infrastructure
{
    public class LoginProvider : ILoginProvider
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public LoginProvider(IHttpClientFactory httpClientFactory) => _httpClientFactory = httpClientFactory;

        /// <summary>
        /// Logins user by email and password.
        /// This method just a proxy to users.api auth method,
        /// a token can be issued by a direct call to users.api.
        /// </summary>
        /// <param name="loginInfo">Login credentials.</param>
        /// <returns></returns>
        public async Task<(bool Success, UserLoginResponse? response, string? ErrorMessage)> LoginAsync(UserLoginInfo loginInfo)
        {
            var client = _httpClientFactory.CreateClient("UsersApi");
            var response = await client.PostAsJsonAsync("/auth", loginInfo);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<UserLoginResponse>();
                return (true, result, null);
            }
            else
            {
                var error = await response.Content.ReadAsStringAsync();
                return (false, null, error);
            }
        }
    }
}
