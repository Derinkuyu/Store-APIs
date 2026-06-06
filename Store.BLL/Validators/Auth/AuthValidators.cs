using FluentValidation;
using Store.DAL;

namespace Store.BLL
{
    public class RegisterDtoValidator : AbstractValidator<RegisterDto>
    {
        public RegisterDtoValidator()
        {
            RuleFor(r => r.DisplayName)
                .NotEmpty().WithMessage("Display name is required.").WithErrorCode("ERR-01")
                .MinimumLength(2).WithMessage("Display name must be at least 2 characters.").WithErrorCode("ERR-02")
                .MaximumLength(100).WithMessage("Display name must not exceed 100 characters.").WithErrorCode("ERR-03");

            RuleFor(r => r.Email)
                .NotEmpty().WithMessage("Email is required.").WithErrorCode("ERR-04")
                .EmailAddress().WithMessage("Invalid email format.").WithErrorCode("ERR-05");

            RuleFor(r => r.Password)
                .NotEmpty().WithMessage("Password is required.").WithErrorCode("ERR-06")
                .MinimumLength(6).WithMessage("Password must be at least 6 characters.").WithErrorCode("ERR-07");
        }
    }

    public class LoginDtoValidator : AbstractValidator<LoginDto>
    {
        public LoginDtoValidator()
        {
            RuleFor(l => l.Email)
                .NotEmpty().WithMessage("Email is required.").WithErrorCode("ERR-01")
                .EmailAddress().WithMessage("Invalid email format.").WithErrorCode("ERR-02");

            RuleFor(l => l.Password)
                .NotEmpty().WithMessage("Password is required.").WithErrorCode("ERR-03");
        }
    }
}
