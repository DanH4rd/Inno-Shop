using FluentValidation;

namespace InnoShop.Users.API.Abstract
{
    public sealed class GetUserByLoginInfoValidator : AbstractValidator<GetUserByLoginInfoQuery>
    {
        public GetUserByLoginInfoValidator()
        {
            RuleFor(x => x.loginInfo.Email)
                .NotEmpty().WithMessage("Email is required.");

            RuleFor(x => x.loginInfo.Password)
                .NotEmpty().WithMessage("Password is required.");
        }
    }

    public sealed class AddUserCommandValidator : AbstractValidator<AddUserCommand>
    {
        public AddUserCommandValidator()
        {
            RuleFor(x => x.userDto.Name)
                .NotEmpty().WithMessage("Name is required.")
                .MaximumLength(255).WithMessage("Name must not exceed 255 characters.");

            RuleFor(x => x.userDto.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Email must be a valid email address.")
                .MaximumLength(255).WithMessage("Email must not exceed 255 characters.");

            RuleFor(x => x.userDto.Password)
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(6).WithMessage("Password must be at least 6 characters long.");

            RuleFor(x => x.userDto.Role)
                .IsInEnum().WithMessage("Role should have valid value.");
        }
    }

    public sealed class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
    {
        public UpdateUserCommandValidator()
        {
            RuleFor(x => x.userDto.Name)
                .MaximumLength(255).WithMessage("Name must not exceed 255 characters.")
                .When(x => !string.IsNullOrWhiteSpace(x.userDto.Name))
                .WithMessage("Name must meet the requirements if provided.");

            RuleFor(x => x.userDto.Email)
                .EmailAddress().WithMessage("Email must be a valid email address.")
                .MaximumLength(255).WithMessage("Email must not exceed 255 characters.")
                .When(x => !string.IsNullOrWhiteSpace(x.userDto.Email))
                .WithMessage("Email must meet the requirements if provided.");

            RuleFor(x => x.userDto.Password)
                .MinimumLength(6).WithMessage("Password must be at least 6 characters long.")
                .When(x => !string.IsNullOrWhiteSpace(x.userDto.Password))
                .WithMessage("Password must meet the requirements if provided.");

            RuleFor(x => x.userDto.Role)
                .IsInEnum().WithMessage("Role should have valid value.");
        }
    }

    public sealed class RequestPasswordRestoreValidator : AbstractValidator<RequestPasswordRestoreCommand>
    {
        public RequestPasswordRestoreValidator()
        {
            RuleFor(x => x.email)
                .EmailAddress().WithMessage("Email must be a valid email address.");
        }
    }

}
