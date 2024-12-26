using Microsoft.EntityFrameworkCore;
using FluentValidation;
using InnoShop.Products.API.Abstract;
using InnoShop.Products.API.Infrastructure;
using InnoShop.Products.API.Presentation;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddProductDbContext();

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));
builder.Services.AddScoped<IProductsRepository, ProductsRepository>();
builder.Services.AddJwtAuthentication();

builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<CustomExceptionHandler>();
builder.Services.AddValidatorsFromAssemblyContaining<AuthUserByLoginInfoValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<ProductFilterValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<AddProductCommandValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<UpdateProductCommandValidator>();

// register http client to communicate to users api
builder.Services.AddHttpClient("UsersApi", client =>
{
    var usersApiBaseUrl = Environment.GetEnvironmentVariable("USERS_API_BASEURL")
                ?? throw new ArgumentNullException("Users API base url can not be empty.");
    client.BaseAddress = new Uri(usersApiBaseUrl);
});

builder.Services.AddScoped<ILoginProvider, LoginProvider>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwagger();

// logging config
builder.Logging
    .ClearProviders()
    .AddConsole();

var app = builder.Build();

// apply db changes (migrations) automatically
if (!(Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "").Equals("Testing"))
    using (var scope = app.Services.CreateScope())
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<ProductsDbContext>();
        dbContext.Database.Migrate();
    }

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseExceptionHandler(); // Converts unhandled exceptions into Problem Details responses
app.UseStatusCodePages(); // Returns the Problem Details response for (empty) non-successful responses

app.UseAuthentication();
app.UseAuthorization();

app.ConfigureRoutes();

app.Run();

// define class to make tests available
namespace InnoShop.Products.API
{
    public partial class Program { }
}