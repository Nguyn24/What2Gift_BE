using What2Gift.Application.Abstraction.Authentication;
using What2Gift.Application.Abstraction.Data;
using What2Gift.Application.Abstraction.Messaging;
using What2Gift.Domain.Common;
using What2Gift.Domain.Users.Errors;

namespace What2Gift.Application.Authentication.ChangePassword
{
    public class ChangePasswordCommandHandler(IDbContext context, IUserContext userContext,
         IPasswordHasher passwordHasher
         ) : ICommandHandler<ChangePasswordCommand>
    {
        public async Task<Result> Handle(ChangePasswordCommand command, CancellationToken cancellationToken)
        {
            var user = await context.Users.FindAsync(new object[] { userContext.UserId }, cancellationToken);

            // Kiểm tra mật khẩu hiện tại
            if (!passwordHasher.Verify(command.CurrentPassword, user.Password))
            {
                return Result.Failure<ChangePasswordCommand>(UserErrors.InvalidCurrentPassword);
            }

            if (command.CurrentPassword == command.NewPassword)
            {
                return Result.Failure<ChangePasswordCommand>(UserErrors.SamePassword);
            }

            // Cập nhật mật khẩu mới
            user.Password = passwordHasher.Hash(command.NewPassword);

            await context.SaveChangesAsync(cancellationToken);


            return Result.Success();
        }
    }
}
