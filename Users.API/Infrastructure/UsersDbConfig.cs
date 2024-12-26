using Microsoft.EntityFrameworkCore;

namespace InnoShop.Users.API.Infrastructure
{
    public static partial class ServiceCollectionExtensions
    {
        public static IServiceCollection AddUserDbContext(this IServiceCollection services)
        {
            // don't register db context for testing env, it will be registered from tests project
            if (!(Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "").Equals("Testing"))
            {
                // get data from environment variables
                var dbName = Environment.GetEnvironmentVariable("POSTGRES_DB");
                var dbUser = Environment.GetEnvironmentVariable("POSTGRES_USER");
                var dbPassword = Environment.GetEnvironmentVariable("POSTGRES_PASSWORD");

                // compose connection string
                var connectionString = $"Host=postgres;Port=5432;Database={dbName};Username={dbUser};Password={dbPassword}";

                // register DbContext with connection string
                services.AddDbContext<UsersDbContext>(options =>
                    options.UseNpgsql(connectionString));
            }

            return services;
        }
    }
}
