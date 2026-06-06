using FluentValidation;

namespace Store.BLL
{
    public class CartItemAddDtoValidator : AbstractValidator<CartItemAddDto>
    {
        public CartItemAddDtoValidator()
        {
            RuleFor(c => c.ProductId)
                .GreaterThan(0).WithMessage("Product is required.").WithErrorCode("ERR-01");

            RuleFor(c => c.Quantity)
                .GreaterThan(0).WithMessage("Quantity must be greater than 0.").WithErrorCode("ERR-02");
        }
    }

    public class CartItemUpdateDtoValidator : AbstractValidator<CartItemUpdateDto>
    {
        public CartItemUpdateDtoValidator()
        {
            RuleFor(c => c.ProductId)
                .GreaterThan(0).WithMessage("Product is required.").WithErrorCode("ERR-01");

            RuleFor(c => c.Quantity)
                .GreaterThan(0).WithMessage("Quantity must be greater than 0.").WithErrorCode("ERR-02");
        }
    }
}
