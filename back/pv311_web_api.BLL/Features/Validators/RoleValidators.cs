using FluentValidation;
using pv311_web_api.BLL.DTOs.Role;

namespace pv311_web_api.BLL.Features.Validators
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

    public class UpdateRoleValidator : AbstractValidator<UpdateRoleDto>
    {
        public UpdateRoleValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required")
                .MaximumLength(256).WithMessage("Maximum length 256 symbols");

            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Id is required");
        }
    }
}
