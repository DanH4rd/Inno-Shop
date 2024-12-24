using Microsoft.EntityFrameworkCore;

namespace InnoShop.Products.API.Infrastructure
{
    public static partial class ServiceCollectionExtensions
    {
        public static IServiceCollection AddProductDbContext(this IServiceCollection services)
        {
            // get data from environment variables
            var dbName = Environment.GetEnvironmentVariable("POSTGRES_DB");
            var dbUser = Environment.GetEnvironmentVariable("POSTGRES_USER");
            var dbPassword = Environment.GetEnvironmentVariable("POSTGRES_PASSWORD");

            // compose connection string
            var connectionString = $"Host=postgres;Port=5432;Database={dbName};Username={dbUser};Password={dbPassword}";

            // register DbContext with connection string
            services.AddDbContext<ProductsDbContext>(options =>
                options.UseNpgsql(connectionString));

            return services;
        }
    }
}
