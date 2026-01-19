using Application.Dtos.Users;
using FluentValidation;

namespace Application.Validators.Users
{
    public class CreateUserValidator : AbstractValidator<CreateUserDto>
    {
        public CreateUserValidator()
        {
            RuleFor(x => x.Username)
                .NotEmpty().WithMessage("Username is required")
                .MaximumLength(100);

            RuleFor(x => x.BirthDate)
                .LessThan(DateTime.Now).WithMessage("BirthDate must be in the past");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required")
                .MinimumLength(8);

            RuleFor(x => x.RePassword)
                .Equal(x => x.Password).WithMessage("Passwords do not match");

            RuleFor(x => x.Role)
                .Must(r => r == null || r == "Admin" || r == "Editor" || r == "Viewer")
                .WithMessage("Role must be Admin, Editor, Viewer, or null");
        }
    }
}
