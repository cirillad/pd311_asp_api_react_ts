using FluentValidation;

namespace pd311_web_api.BLL.DTOs.Account
{
    public class LoginDto
    {
        public string? UserName { get; set; }
        public string? Password { get; set; }
    }

    public class LoginValidator : AbstractValidator<LoginDto>
    {
        public LoginValidator()
        {
            RuleFor(x => x.UserName)
                .NotEmpty().WithMessage("UserName is required");
            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required")
                .MinimumLength(6).WithMessage("Password minimum length 6 symbols");
        }
    }
}
