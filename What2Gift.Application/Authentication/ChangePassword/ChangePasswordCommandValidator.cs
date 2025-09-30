﻿using FluentValidation;

namespace What2Gift.Application.Authentication.ChangePassword
{
    public class ChangePasswordCommandValidator : AbstractValidator<ChangePasswordCommand>
    {
        public ChangePasswordCommandValidator()
        {
            RuleFor(command => command.CurrentPassword).NotNull().NotEmpty();
            RuleFor(command => command.NewPassword).NotNull().NotEmpty();
        }
    }
}
