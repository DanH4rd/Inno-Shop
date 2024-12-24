using Microsoft.AspNetCore.Authorization;
using MediatR;
using InnoShop.Users.API.Abstract;
using InnoShop.Users.API.Models;
using FluentValidation;
using System.Security.Claims;

namespace InnoShop.Users.API.Presentation
{
    public static partial class EndpointsConfiguration
    {
        public static IEndpointRouteBuilder ConfigureRoutes(this IEndpointRouteBuilder endpoints)
        {
            // root welcome
            endpoints.MapGet("/", () => "Welcome to InnoShop Users API!").WithName("Welcome");


            // get all users
            endpoints.MapGet("/users", [Authorize(Roles = "Admin")]
            async (IMediator mediator) => await mediator.Send(new GetUsersQuery()))
           .WithName("GetAllUsers")
           .WithDescription("Gets all users list");


            // get user by Id
            endpoints.MapGet("/users/{id:int}", [Authorize]
            async (int id, IMediator mediator, HttpContext context) =>
            {
                var userId = int.Parse(context.User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty);
                var userRole = Enum.Parse<UserRoles>(context.User.FindFirstValue(ClaimTypes.Role) ?? string.Empty);
                // users with non-admin role can get only their data
                if (userRole != UserRoles.Admin && id != userId)
                {
                    return Results.NotFound();
                }

                var user = await mediator.Send(new GetUserByIdQuery(id));
                return user is not null ? Results.Ok(user) : Results.NotFound();
            })
            .WithName("GetUserById")
            .WithDescription("Gets a single user by Id.");

            // create user
            endpoints.MapPost("/users", [Authorize(Roles = "Admin")]
            async (UserDto userDto, IValidator<AddUserCommand> validator, IMediator mediator) =>
            {
                var command = new AddUserCommand(userDto);
                var validationResult = await validator.ValidateAsync(command);
                if (!validationResult.IsValid)
                {
                    return Results.BadRequest(validationResult.Errors);
                }

                var createdUser = await mediator.Send(command);
                await mediator.Publish(new UserModifiedNotification(createdUser)); // send email
                return Results.Created($"/users/{createdUser.Id}", createdUser);
            })
            .WithName("CreateUser")
            .WithDescription("Creates a new user.");


            // update user
            endpoints.MapPut("/users/{id:int}", [Authorize]
            async (int id, UserDto userDto,
                   IValidator<UpdateUserCommand> validator,
                   IMediator mediator, HttpContext context) =>
            {
                var userId = int.Parse(context.User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty);
                var userRole = Enum.Parse<UserRoles>(context.User.FindFirstValue(ClaimTypes.Role) ?? string.Empty);
                // users with non-admin role can update only their data
                if (userRole != UserRoles.Admin && id != userId)
                {
                    return Results.NotFound();
                }

                var command = new UpdateUserCommand(id, userDto);
                var validationResult = await validator.ValidateAsync(command);
                if (!validationResult.IsValid)
                {
                    return Results.BadRequest(validationResult.Errors);
                }

                var modifiedUser = await mediator.Send(command);

                // check if we need to confirm email
                if (modifiedUser != null && !modifiedUser.IsEmailConfirmed)
                {
                    await mediator.Publish(new UserModifiedNotification(modifiedUser)); // send email
                }

                return modifiedUser != null ? Results.NoContent() : Results.BadRequest("User could not be updated.");
            })
            .WithName("UpdateUser")
            .WithDescription("Updates a single user by Id");


            // user soft delete (deactivate)
            endpoints.MapDelete("/users/{id:int}", [Authorize(Roles = "Admin")]
            async (int id, IMediator mediator) =>
            {
                var result = await mediator.Send(new DeleteUserByIdCommand(id));
                if (result > 0)
                {
                    await mediator.Publish(new UserActiveStatusChangedNotification(id, false)); // deactivate products
                    return Results.NoContent();
                }
                return Results.NotFound($"User with Id {id} not found.");
            })
            .WithName("DeleteUser")
            .WithDescription("Deletes a user by Id");


            // user restore
            endpoints.MapPatch("/users/restore/{id:int}", [Authorize(Roles = "Admin")]
            async (int id, IMediator mediator) =>
            {
                var result = await mediator.Send(new RestoreUserByIdCommand(id));
                if (result > 0)
                {
                    await mediator.Publish(new UserActiveStatusChangedNotification(id, true)); // activate products
                    return Results.NoContent();
                }
                return Results.NotFound($"User with Id {id} not found.");
            })
            .WithName("RestoreUser")
            .WithDescription("Restores a user by Id");


            // confirm email
            endpoints.MapPatch("/users/emailconfirm/{token}",
            async (string token, IMediator mediator) =>
            {
                var result = await mediator.Send(new ConfirmUserEmailCommand(token));
                return result > 0 ? Results.NoContent() : Results.NotFound($"Email cannot be confirmed.");
            })
            .WithName("ConfirmEmail")
            .WithDescription("Confirms email by a confirmation token.");


            // password restore request
            endpoints.MapPatch("/users/auth/restorereq/{email}",
            async (string email, IMediator mediator) =>
            {
                var token = await mediator.Send(new RequestPasswordRestoreCommand(email));

                if (!string.IsNullOrWhiteSpace(token))
                {
                    await mediator.Publish(new UserInfoRequestedNotification(email,
                        "InnoShop: password restore request",
                        string.Format("/users/auth/restore/{0}", token))); // send email with restore link
                    return Results.NoContent();
                }

                return Results.NotFound($"Email not found.");
            })
            .WithName("RequestPasswordRestore")
            .WithDescription("Sends a password restore link.");


            // password restore 
            endpoints.MapPatch("/users/auth/restore/{token}",
            async (string token, IMediator mediator) =>
            {
                var (email, passw) = await mediator.Send(new RestorePasswordCommand(token));

                if (!string.IsNullOrWhiteSpace(email))
                {
                    await mediator.Publish(new UserInfoRequestedNotification(email, "InnoShop: new password", passw)); // send email with the new password
                    return Results.NoContent();
                }

                return Results.NotFound($"User not found.");
            })
            .WithName("RestorePassword")
            .WithDescription("Sends new password to email.");

            return endpoints;
        }
    }
}
