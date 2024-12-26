using InnoShop.Products.API.Infrastructure;
using InnoShop.Users.API.Infrastructure;
using InnoShop.Users.API.Models;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace InnoShop.Tests
{
    public class DBFixture : IDisposable
    {
        public WebApplicationFactory<InnoShop.Users.API.Program> UsersFactory { get; private set; } = null!;
        public WebApplicationFactory<InnoShop.Products.API.Program> ProductsFactory { get; private set; } = null!;
        public HttpClient UsersClient { get; private set; } = null!;
        public HttpClient ProductsClient { get; private set; } = null!;
        private string _usersDbName;
        private string _productsDbName;

        public DBFixture()
        {
            // set environment variables
            Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Testing");
            Environment.SetEnvironmentVariable("JWT_SECRET_KEY", "my_super_secret_key_32_bytes_long");
            Environment.SetEnvironmentVariable("POSTGRES_USERS_DB", "innoshop_users");
            Environment.SetEnvironmentVariable("POSTGRES_PRODUCTS_DB", "innoshop_products");
            Environment.SetEnvironmentVariable("POSTGRES_USER", "innoshop_user");
            Environment.SetEnvironmentVariable("POSTGRES_PASSWORD", "innoshop_password");
            Environment.SetEnvironmentVariable("USERS_API_BASEURL", "http://localhost:8080");
            Environment.SetEnvironmentVariable("PRODUCTS_API_BASEURL", "http://localhost:8082");
            Environment.SetEnvironmentVariable("X_SVC_AUTH_HEADER", "8291d15b-9fa3-43a2-94ec-2a686daad548");


            // unique db names
            _usersDbName = $"UsersDb_{Guid.NewGuid()}";
            _productsDbName = $"ProductsDb_{Guid.NewGuid()}";

            // create fabrics for each service
            UsersFactory = CreateFactory<UsersDbContext, InnoShop.Users.API.Program>(_usersDbName);
            ProductsFactory = CreateFactory<ProductsDbContext, InnoShop.Products.API.Program>(_productsDbName);

            // create http clients
            UsersClient = UsersFactory.CreateClient();
            ProductsClient = ProductsFactory.CreateClient();
        }

        private WebApplicationFactory<TProgram> CreateFactory<TContext, TProgram>(string databaseName)
            where TContext : DbContext
            where TProgram : class
        {
            return new WebApplicationFactory<TProgram>()
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureServices(services =>
                    {
                        // register InMemory Database instead of a real one
                        services.AddDbContext<TContext>(options =>
                            options.UseInMemoryDatabase(databaseName));

                        // init db
                        using var scope = services.BuildServiceProvider().CreateScope();
                        var context = scope.ServiceProvider.GetRequiredService<TContext>();
                        context.Database.EnsureCreated(); // create db

                        // add initial user
                        if (context is UsersDbContext)
                        {
                            (context as UsersDbContext)!.Users.Add(new User
                            {
                                Name = "Admin",
                                Email = "admin@admin.com",
                                PasswordHash = UserDto.CalcMD5Hash("admin"),
                                EmailConfirmationToken = Guid.NewGuid().ToString(),
                                CreatedDate = DateTime.UtcNow,
                                IsActive = true,
                                IsEmailConfirmed = true,
                                LastLogin = null,
                                Role = UserRoles.Admin
                            });

                            context.SaveChanges();
                        }
                    });
                });
        }

        public void Dispose()
        {
            // remove users db
            using (var scope = UsersFactory.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<UsersDbContext>();
                context.Database.EnsureDeleted();
            }

            // remove products db
            using (var scope = ProductsFactory.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<ProductsDbContext>();
                context.Database.EnsureDeleted();
            }

            // release fabric resources
            UsersFactory.Dispose();
            ProductsFactory.Dispose();
        }
    }
}
