using FluentValidation;

namespace Store.BLL
{
    public class CategoryCreateDtoValidator : AbstractValidator<CategoryCreateDto>
    {
        public CategoryCreateDtoValidator()
        {
            RuleFor(c => c.Name)
                .NotEmpty()
                .WithMessage("Category name is required.")
                .WithErrorCode("ERR-01")

                .MinimumLength(2)
                .WithMessage("Category name must be at least 2 characters.")
                .WithErrorCode("ERR-02")

                .MaximumLength(100)
                .WithMessage("Category name must not exceed 100 characters.")
                .WithErrorCode("ERR-03");

            RuleFor(c => c.Description)
                .MaximumLength(500)
                .WithMessage("Description must not exceed 500 characters.")
                .WithErrorCode("ERR-04")
                .When(c => c.Description is not null);
        }
    }

    public class CategoryEditDtoValidator : AbstractValidator<CategoryEditDto>
    {
        public CategoryEditDtoValidator()
        {
            RuleFor(c => c.Name)
                .NotEmpty()
                .WithMessage("Category name is required.")
                .WithErrorCode("ERR-01")

                .MinimumLength(2)
                .WithMessage("Category name must be at least 2 characters.")
                .WithErrorCode("ERR-02")

                .MaximumLength(100)
                .WithMessage("Category name must not exceed 100 characters.")
                .WithErrorCode("ERR-03");

            RuleFor(c => c.Description)
                .MaximumLength(500)
                .WithMessage("Description must not exceed 500 characters.")
                .WithErrorCode("ERR-04")
                .When(c => c.Description is not null);
        }
    }
}
