using FluentAssertions;
using InnoShop.Users.API.Models;
using System.Net.Http.Json;

namespace InnoShop.Tests.Users
{
    /// <summary>
    /// User tests definitions
    /// </summary>
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
            var authInfo = await response.Content.ReadFromJsonAsync<InnoShop.Products.API.Models.UserLoginResponse>();

            authInfo?.Token.Should().NotBeEmpty();
            authInfo?.Expiration.Should().BeAfter(DateTime.UtcNow);

            _authAdminToken = authInfo?.Token ?? "";
        }

        [Fact()]
        public async Task GetDefaultUsersResponse()
        {
            var response = await _fixture.UsersClient.GetAsync("/");
            response.EnsureSuccessStatusCode();
        }

        [Fact()]
        public async Task GetAllUsersAsync()
        {
            await LoginAdminAsync();
            var request = new HttpRequestMessage(HttpMethod.Get, "/users");
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _authAdminToken);

            var response = await _fixture.UsersClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var users = await response.Content.ReadFromJsonAsync<List<User>>();
            users.Should().NotBeNull();
            users!.Count.Should().BeGreaterThanOrEqualTo(1); // at lease one admin user should exist
        }

        [Fact()]
        public async Task CreateUserAsync()
        {
            await LoginAdminAsync();
            var request = new HttpRequestMessage(HttpMethod.Post, "/users")
            {
                Content = JsonContent.Create(new UserDto
                {
                    Email = "test@user.com",
                    Name = "Test User",
                    Password = "testpassword",
                    Role = UserRoles.User
                })
            };
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _authAdminToken);
            var response = await _fixture.UsersClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
        }
    }
}
