using Microsoft.EntityFrameworkCore;
using InnoShop.Users.API.Infrastructure;
using InnoShop.Users.API.Presentation;
using InnoShop.Users.API.Abstract;
using FluentValidation;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddUserDbContext();
builder.Services.AddJwtAuthentication();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));
builder.Services.AddScoped<IUsersRepository, UsersRepository>();
builder.Services.AddValidatorsFromAssemblyContaining<AddUserCommandValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<UpdateUserCommandValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<GetUserByLoginInfoValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<RequestPasswordRestoreValidator>();
builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<CustomExceptionHandler>();
builder.Services.AddSwagger();
builder.Services.AddScoped<IEmailProvider, MockEmailProvider>();
builder.Services.AddScoped<IProductsProvider, ProductsProvider>();

// register http client to communicate to products api
builder.Services.AddHttpClient("ProductsApi", client =>
{
    var productsApiBaseUrl = Environment.GetEnvironmentVariable("PRODUCTS_API_BASEURL")
                ?? throw new ArgumentNullException("Products API base url can not be empty.");
    client.BaseAddress = new Uri(productsApiBaseUrl);
});

// logging config
builder.Logging
    .ClearProviders()
    .AddConsole();

var app = builder.Build();

// apply db changes (migrations) automatically
if (!(Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "").Equals("Testing"))
    using (var scope = app.Services.CreateScope())
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<UsersDbContext>();
        dbContext.Database.Migrate();
    }

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseExceptionHandler(); // Converts unhandled exceptions into Problem Details responses
app.UseStatusCodePages(); // Returns the Problem Details response for (empty) non-successful responses

app.UseAuthentication();
app.UseAuthorization();

app.ConfigureAuthRoutes();
app.ConfigureRoutes();

app.Run();

// define class to make tests available
namespace InnoShop.Users.API
{
    public partial class Program { }
}