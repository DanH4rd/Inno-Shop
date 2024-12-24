using MediatR;
using InnoShop.Products.API.Abstract.Operations;
using InnoShop.Products.API.Models;
using InnoShop.Products.API.Abstract;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace InnoShop.Products.API.Presentation
{
    public static partial class EndpointsConfiguration
    {
        public static IEndpointRouteBuilder ConfigureRoutes(this IEndpointRouteBuilder endpoints)
        {
            // welcome
            endpoints.MapGet("/", () => "Welcome to InnoShop Products API!").WithName("Welcome");


            // auth user by login info
            endpoints.MapPost("/auth", async (UserLoginInfo loginInfo,
                                              IValidator<AuthUserByLoginInfoQuery> validator,
                                              IMediator mediator) =>
            {
                var command = new AuthUserByLoginInfoQuery(loginInfo);
                var validationResult = await validator.ValidateAsync(command);
                if (!validationResult.IsValid)
                {
                    return Results.BadRequest(validationResult.Errors);
                }
                var (success, response, errorMessage) = await mediator.Send(new AuthUserByLoginInfoQuery(loginInfo));

                return success ? Results.Ok(response) : Results.BadRequest(new { Error = errorMessage });
            })
            .WithName("LoginUser")
            .WithDescription("Log user in.");


            // get all products by filter
            endpoints.MapGet("/products",
            async (IMediator mediator, [AsParameters] ProductFilter filter, IValidator<ProductFilter> validator) =>
            {
                IResult result;

                var validationResult = await validator.ValidateAsync(filter);
                if (!validationResult.IsValid)
                {
                    result = Results.BadRequest(validationResult.Errors);
                }
                else
                {
                    var list = await mediator.Send(new GetProductsQuery((int?)null, filter));
                    result = Results.Ok(list);
                }

                return result;
            })
           .WithName("GetProducts")
           .WithDescription("Gets filtered products.");


            // get user products by filter
            endpoints.MapGet("/products/my", [Authorize]
            async (IMediator mediator, HttpContext context,
                    [AsParameters] ProductFilter filter, IValidator<ProductFilter> validator) =>
            {
                IResult result;

                var validationResult = await validator.ValidateAsync(filter);
                if (!validationResult.IsValid)
                {
                    result = Results.BadRequest(validationResult.Errors);
                }
                else
                {
                    var userId = int.Parse(context.User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty);
                    var list = await mediator.Send(new GetProductsQuery(userId, filter));
                    result = Results.Ok(list);
                }

                return result;
            })
           .WithName("GetUserProducts")
           .WithDescription("Gets filtered user products.");


            // get product by Id
            endpoints.MapGet("/products/{id:int}", [Authorize]
            async (int id, IMediator mediator, HttpContext context) =>
            {
                var userId = int.Parse(context.User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty);
                var product = await mediator.Send(new GetProductByIdQuery(userId, id));
                return product is not null ? Results.Ok(product) : Results.NotFound();
            })
            .WithName("GetProductById")
            .WithDescription("Gets a single product by Id.");


            // create product
            endpoints.MapPost("/products", [Authorize]
            async (ProductDto productDto, IProductsRepository repo,
                  IValidator<AddProductCommand> validator, HttpContext context,
                  IMediator mediator) =>
            {
                var userId = int.Parse(context.User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty);
                var command = new AddProductCommand(userId, productDto);
                var validationResult = await validator.ValidateAsync(command);
                if (!validationResult.IsValid)
                {
                    return Results.BadRequest(validationResult.Errors);
                }

                var createdProduct = await mediator.Send(command);
                return Results.Created($"/products/{createdProduct.Id}", createdProduct);
            })
            .WithName("CreateProduct")
            .WithDescription("Creates a new product.");


            // update product
            endpoints.MapPut("/products/{id:int}", [Authorize]
            async (int id, ProductDto productDto,
                    IValidator<UpdateProductCommand> validator, IMediator mediator, HttpContext context) =>
            {
                var userId = int.Parse(context.User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty);

                var command = new UpdateProductCommand(userId, id, productDto);
                var validationResult = await validator.ValidateAsync(command);
                if (!validationResult.IsValid)
                {
                    return Results.BadRequest(validationResult.Errors);
                }

                var modifiedProduct = await mediator.Send(command);
                return modifiedProduct != null ? Results.NoContent() : Results.BadRequest("Product could not be updated.");
            })
            .WithName("UpdateProduct")
            .WithDescription("Updates a single product by Id");


            // product soft delete
            endpoints.MapDelete("/products/{id:int}", [Authorize]
            async (int id, IMediator mediator, HttpContext context) =>
            {
                var userId = int.Parse(context.User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty);
                var result = await mediator.Send(new DeleteProductCommand(userId, id));
                return result > 0 ? Results.NoContent() : Results.NotFound($"Product with Id {id} not found.");
            })
            .WithName("DeleteProduct")
            .WithDescription("Deletes a product by Id");


            // restore product
            endpoints.MapPatch("/products/restore/{id:int}", [Authorize]
            async (int id, IMediator mediator, HttpContext context) =>
            {
                var userId = int.Parse(context.User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty);
                var result = await mediator.Send(new RestoreProductCommand(userId, id));
                return result > 0 ? Results.NoContent() : Results.NotFound($"Product with Id {id} not found.");
            })
            .WithName("RestoreProduct")
            .WithDescription("Restores a product by Id");


            // deactivate products by users Id
            // calling service must be authorized by sending the valid X-SVC-AUTH header
            endpoints.MapPatch("/products/deactivatebyuser/{id:int}",
            async (int id, IMediator mediator,
                    [FromHeader(Name = "X-SVC-AUTH")] string authHeader) =>
            {
                var svcAuth = Environment.GetEnvironmentVariable("X_SVC_AUTH_HEADER") ?? Guid.NewGuid().ToString();
                if (!svcAuth.Equals(authHeader))
                {
                    return Results.Unauthorized();
                }

                var result = await mediator.Send(new DeactivateProductsByUserIdCommand(id));
                return result == 0 ? Results.NotFound("Products not found") : Results.NoContent();
            })
           .WithName("DeactivateProductsByUserId")
           .WithDescription("Deactivates products by user Id.");


            // activate products by users Id
            // calling service must be authorized by sending the valid X-SVC-AUTH header
            endpoints.MapPatch("/products/activatebyuser/{id:int}",
            async (int id, IMediator mediator,
                    [FromHeader(Name = "X-SVC-AUTH")] string authHeader) =>
            {
                var svcAuth = Environment.GetEnvironmentVariable("X_SVC_AUTH_HEADER") ?? Guid.NewGuid().ToString();
                if (!svcAuth.Equals(authHeader))
                {
                    return Results.Unauthorized();
                }

                var result = await mediator.Send(new ActivateProductsByUserIdCommand(id));
                return result == 0 ? Results.NotFound("Products not found") : Results.NoContent();
            })
           .WithName("ActivateProductsByUserId")
           .WithDescription("Activates products by user Id.");


            return endpoints;
        }
    }
}

