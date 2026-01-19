using Application.Dtos.Users;
using FluentValidation;

namespace Application.Validators.Users
{
    public class UpdateUserValidator : AbstractValidator<UpdateUserDto>
    {
        public UpdateUserValidator()
        {
            RuleFor(x => x.UserName)
                .MaximumLength(50)
                .When(x => !string.IsNullOrEmpty(x.UserName));

            RuleFor(x => x.BirthDate)
                .LessThan(DateTime.Now)
                .When(x => x.BirthDate.HasValue);

            RuleFor(x => x.Role)
                .Must(r => r == null || r == "Admin" || r == "Editor" || r == "Viewer")
                .WithMessage("Role must be Admin, Editor, Viewer, or null");
        }
    }
}
