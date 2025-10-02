using Google.Apis.Auth;
using Microsoft.EntityFrameworkCore;
using What2Gift.Application.Abstraction.Authentication;
using What2Gift.Application.Abstraction.Data;
using What2Gift.Application.Abstraction.Messaging;
using What2Gift.Domain.Common;
using What2Gift.Domain.Users;
using What2Gift.Domain.Users.Errors;

namespace What2Gift.Application.Authentication.Login.LoginWithGoogle;

public sealed class LoginWithGoogleCommandHandler(
    IDbContext context,
    ITokenProvider tokenProvider,
    IPayload payload
) : ICommandHandler<LoginWithGoogleCommand, LoginWithGoogleResponse>
{
    public async Task<Result<LoginWithGoogleResponse>> Handle(LoginWithGoogleCommand command, CancellationToken cancellationToken)
    {
        GoogleJsonWebSignature.Payload googlePayload;
        try
        {
            var settings = new GoogleJsonWebSignature.ValidationSettings()
            {
                Audience = new[] { payload.GoogleClientId }
            };
            googlePayload = await GoogleJsonWebSignature.ValidateAsync(command.IdToken, settings);
        }
        catch (Exception)
        {
            return Result.Failure<LoginWithGoogleResponse>(UserErrors.InvalidGoogleToken);
        }

        // Tìm user theo email
        var email = googlePayload.Email?.Trim().ToLower();
        if (email is null)
            return Result.Failure<LoginWithGoogleResponse>(UserErrors.InvalidGoogleToken);

        var user = await context.Users.Include(u => u.RefreshTokens)
            .SingleOrDefaultAsync(u => u.Email.ToLower() == email, cancellationToken);

        if (user is null)
        {
            user = new User
            {
                Id = Guid.NewGuid(),
                Email = email,
                Username = googlePayload.Name ?? email,
                Role = UserRole.Member,
                Status = UserStatus.Active,
                Password = null,
                IsVerified = true,
                MembershipStatus = MembershipStatus.Inactive,
                AvatarUrl = googlePayload.Picture,
                RefreshTokens = new List<RefreshToken>()
            };

            context.Users.Add(user);
            await context.SaveChangesAsync(cancellationToken);
        }
        else
        {
            if (user.Status == UserStatus.Inactive)
                return Result.Failure<LoginWithGoogleResponse>(UserErrors.InActive);
        }

        // Tạo access token
        string accessToken = tokenProvider.Create(user);

        // Tạo refresh token mới
        var refreshToken = new RefreshToken
        {
            Id = Guid.NewGuid(),
            UserId = user.Id,
            Token = tokenProvider.GenerateRefreshToken(),
            Expires = DateTime.UtcNow.AddDays(1)
        };

        // Xóa refresh token cũ
        context.RefreshTokens.RemoveRange(user.RefreshTokens);
        context.RefreshTokens.Add(refreshToken);

        await context.SaveChangesAsync(cancellationToken);

        return new LoginWithGoogleResponse
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken.Token,
            Name = user.Username,
            Email = user.Email,
            Role = user.Role.ToString()
        };
    }
}
