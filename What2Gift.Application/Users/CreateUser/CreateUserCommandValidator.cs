using FluentValidation;
using What2Gift.Application.Users.CreateUser;

namespace BloodDonation.Application.Users.CreateUser;

public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserCommandValidator()
    {
        RuleFor(command => command.Name).NotNull().NotEmpty();
        RuleFor(command => command.Email).NotNull().NotEmpty();
        RuleFor(command => command.Role).IsInEnum().NotNull().NotEmpty();
    }
}