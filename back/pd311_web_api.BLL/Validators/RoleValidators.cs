using FluentValidation;
using pd311_web_api.BLL.DTOs.Role;

namespace pd311_web_api.BLL.Validators
{
    public class CreateRoleValidator : AbstractValidator<CreateRoleDto>
    {
        public CreateRoleValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required")
                .MaximumLength(256).WithMessage("Maximum length 256 symbols");
        }
    }

    public class RoleValidator : AbstractValidator<RoleDto>
    {
        public RoleValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required")
                .MaximumLength(256).WithMessage("Maximum length 256 symbols");
        }
    }
}
