using FluentAssertions;
using System.Net.Http.Json;
using InnoShop.Products.API.Models;
using InnoShop.Products.API.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace InnoShop.Tests.Products
{
    public class IntegrationTests : TestBase
    {
        private string _authAdminToken;

        public IntegrationTests(DBFixture fixture) : base(fixture)
        {
            _authAdminToken = string.Empty;
        }

        private async Task LoginAdminAsync()
        {
            if (!string.IsNullOrEmpty(_authAdminToken)) return;

            var loginInfo = new UserLoginInfo { Email = "admin@admin.com", Password = "admin" };
            var response = await _fixture.UsersClient.PostAsJsonAsync("/auth", loginInfo);
            response.EnsureSuccessStatusCode();

            var authInfo = await response.Content.ReadFromJsonAsync<UserLoginResponse>();

            authInfo?.Token.Should().NotBeEmpty();
            authInfo?.Expiration.Should().BeAfter(DateTime.UtcNow);

            _authAdminToken = authInfo?.Token ?? "";
        }

        [Fact()]
        public async Task GetDefaultProductsResponse()
        {
            var response = await _fixture.ProductsClient.GetAsync("/");
            response.EnsureSuccessStatusCode();
        }

        [Fact()]
        public async Task CreateProductAndGetAllProductsAsync()
        {
            await LoginAdminAsync();

            // create a product
            var request = new HttpRequestMessage(HttpMethod.Post, "/products")
            {
                Content = JsonContent.Create(new ProductDto
                {
                    Title = "Test product",
                    Description = "Test product description",
                    IsAvailable = true,
                    Price = 123
                })
            };
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _authAdminToken);
            var response = await _fixture.ProductsClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            // get products
            var respProd = await _fixture.ProductsClient.GetAsync("/products");
            respProd.EnsureSuccessStatusCode();

            var products = await respProd.Content.ReadFromJsonAsync<List<Product>>();
            products.Should().NotBeNull();
            products!.Count.Should().BeGreaterThanOrEqualTo(1); // at lease one product should exist
        }
    }
}
