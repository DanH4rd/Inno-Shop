using FluentValidation;
using InnoShop.Users.API.Abstract;
using InnoShop.Users.API.Models;
using MediatR;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace InnoShop.Users.API.Presentation
{
    public static partial class EndpointsConfiguration
    {
        private static string GenerateToken(User user)
        {
            var keyValue = Environment.GetEnvironmentVariable("JWT_SECRET_KEY") ?? "";
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(keyValue));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
               new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
               new Claim(JwtRegisteredClaimNames.Email, user.Email),
               new Claim(JwtRegisteredClaimNames.Name, user.Name),
               new Claim(ClaimTypes.Role, user.Role.ToString())
            };

            var token = new JwtSecurityToken(
                issuer: "InnoShop",
                audience: "InnoShop",
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(30),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public static IEndpointRouteBuilder ConfigureAuthRoutes(this IEndpointRouteBuilder endpoints)
        {
            // auth user by login info
            endpoints.MapPost("/auth", async (UserLoginInfo loginInfo,
                                              IValidator<GetUserByLoginInfoQuery> validator,
                                              IMediator mediator) =>
            {
                var command = new GetUserByLoginInfoQuery(loginInfo);
                var validationResult = await validator.ValidateAsync(command);
                if (!validationResult.IsValid)
                {
                    return Results.BadRequest(validationResult.Errors);
                }

                var user = await mediator.Send(new GetUserByLoginInfoQuery(loginInfo));

                if (user == null)
                {
                    return Results.NotFound("User not found.");
                }

                var token = GenerateToken(user);

                return Results.Ok(new { Token = token, Expiration = DateTime.UtcNow.AddMinutes(30) });

            })
            .WithName("GetUserByLoginInfo")
            .WithDescription("Log user in.");

            return endpoints;
        }
    }
}
