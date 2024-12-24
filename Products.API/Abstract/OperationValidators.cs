using FluentValidation;
using InnoShop.Products.API.Abstract.Operations;
using InnoShop.Products.API.Models;

namespace InnoShop.Products.API.Abstract
{
    public sealed class AuthUserByLoginInfoValidator : AbstractValidator<AuthUserByLoginInfoQuery>
    {
        public AuthUserByLoginInfoValidator()
        {
            RuleFor(x => x.loginInfo.Email)
                .NotEmpty().WithMessage("Email is required.");

            RuleFor(x => x.loginInfo.Password)
                .NotEmpty().WithMessage("Password is required.");
        }
    }

    public sealed class ProductFilterValidator : AbstractValidator<ProductFilter>
    {
        public ProductFilterValidator()
        {
            RuleFor(x => x.SearchText)
                .MaximumLength(100)
                .WithMessage("SearchText cannot exceed 255 characters.");

            RuleFor(x => x.MinPrice)
                .InclusiveBetween(0.01, 100000)
                .When(x => x.MinPrice.HasValue)
                .WithMessage("MinPrice must be between 0.01 and 100000.");

            RuleFor(x => x.MaxPrice)
                .InclusiveBetween(0.01, 100000)
                .When(x => x.MaxPrice.HasValue)
                .WithMessage("MaxPrice must be between 0.01 and 100000.");

            RuleFor(x => x)
                .Must(x => !x.MinPrice.HasValue || !x.MaxPrice.HasValue || x.MinPrice <= x.MaxPrice)
                .WithMessage("MinPrice must be less than or equal to MaxPrice.");

            RuleFor(x => x)
                .Must(x => !x.CreatedAfter.HasValue || !x.CreatedBefore.HasValue || x.CreatedAfter <= x.CreatedBefore)
                .WithMessage("CreatedAfter must be less than or equal to CreatedBefore.");
        }
    }

    public sealed class ProductDtoValidator : AbstractValidator<ProductDto>
    {
        public ProductDtoValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required.")
                .MaximumLength(255).WithMessage("Title must not exceed 255 characters.");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Description is required.")
                .MaximumLength(3000).WithMessage("Description must not exceed 3000 characters.");

            RuleFor(x => x.Price)
                .InclusiveBetween(0.01, 100000).WithMessage("Price must be between 0.01 and 100,000.");
        }
    }

    public sealed class AddProductCommandValidator : AbstractValidator<AddProductCommand>
    {
        public AddProductCommandValidator()
        {
            RuleFor(x => x.productDto).SetValidator(new ProductDtoValidator());
        }
    }

    public sealed class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
    {
        public UpdateProductCommandValidator()
        {
            RuleFor(x => x.productDto).SetValidator(new ProductDtoValidator());
        }
    }
}
