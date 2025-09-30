using FluentValidation;

namespace What2Gift.Application.Users.UpdateUser;

public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
{
    public UpdateUserCommandValidator()
    {
        RuleFor(command => command.FullName)
            .MaximumLength(255).WithMessage("Full Name cannot exceed 255 characters.");

        /*RuleFor(command => command.Email)
            .EmailAddress().WithMessage("Invalid email format.");*/

        RuleFor(command => command.Role)
            .IsInEnum().WithMessage("Invalid Role value.");

        RuleFor(command => command.Status)
            .IsInEnum().WithMessage("Invalid Status value.");
    }
}