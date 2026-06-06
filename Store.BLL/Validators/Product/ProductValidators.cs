using FluentValidation;

namespace Store.BLL
{
    public class ProductCreateDtoValidator : AbstractValidator<ProductCreateDto>
    {
        public ProductCreateDtoValidator()
        {
            RuleFor(p => p.Name)
                .NotEmpty().WithMessage("Product name is required.").WithErrorCode("ERR-01")
                .MinimumLength(2).WithMessage("Product name must be at least 2 characters.").WithErrorCode("ERR-02")
                .MaximumLength(200).WithMessage("Product name must not exceed 200 characters.").WithErrorCode("ERR-03");

            RuleFor(p => p.Price)
                .GreaterThan(0).WithMessage("Price must be greater than 0.").WithErrorCode("ERR-04");

            RuleFor(p => p.Stock)
                .GreaterThanOrEqualTo(0).WithMessage("Stock cannot be negative.").WithErrorCode("ERR-05");

            RuleFor(p => p.CategoryId)
                .GreaterThan(0).WithMessage("Category is required.").WithErrorCode("ERR-06");
        }
    }

    public class ProductEditDtoValidator : AbstractValidator<ProductEditDto>
    {
        public ProductEditDtoValidator()
        {
            RuleFor(p => p.Name)
                .NotEmpty().WithMessage("Product name is required.").WithErrorCode("ERR-01")
                .MinimumLength(2).WithMessage("Product name must be at least 2 characters.").WithErrorCode("ERR-02")
                .MaximumLength(200).WithMessage("Product name must not exceed 200 characters.").WithErrorCode("ERR-03");

            RuleFor(p => p.Price)
                .GreaterThan(0).WithMessage("Price must be greater than 0.").WithErrorCode("ERR-04");

            RuleFor(p => p.Stock)
                .GreaterThanOrEqualTo(0).WithMessage("Stock cannot be negative.").WithErrorCode("ERR-05");

            RuleFor(p => p.CategoryId)
                .GreaterThan(0).WithMessage("Category is required.").WithErrorCode("ERR-06");
        }
    }
}
