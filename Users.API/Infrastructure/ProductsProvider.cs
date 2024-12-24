using System.Net;
using InnoShop.Users.API.Abstract;

namespace InnoShop.Users.API.Infrastructure
{
    public class ProductsProvider : IProductsProvider
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public ProductsProvider(IHttpClientFactory httpClientFactory) => _httpClientFactory = httpClientFactory;

        public async Task<(bool Success, string? ErrorMessage)> AdjustUserAvailabilityAsync(int userId, bool availability)
        {
            var client = _httpClientFactory.CreateClient("ProductsApi");

            var url = availability ? $"/products/activatebyuser/{userId}" : $"/products/deactivatebyuser/{userId}";

            var request = new HttpRequestMessage(HttpMethod.Patch, url);
            request.Headers.Add("X-SVC-AUTH", Environment.GetEnvironmentVariable("X_SVC_AUTH_HEADER") ?? Guid.NewGuid().ToString());

            var response = await client.SendAsync(request);

            return response.StatusCode is HttpStatusCode.NoContent or HttpStatusCode.NotFound
                   ? (true, null)
                   : (false, await response.Content.ReadAsStringAsync());
        }
    }
}
