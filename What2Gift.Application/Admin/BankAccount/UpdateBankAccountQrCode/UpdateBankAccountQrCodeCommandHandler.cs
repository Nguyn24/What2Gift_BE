using Microsoft.EntityFrameworkCore;
using What2Gift.Application.Abstraction.Data;
using What2Gift.Application.Abstraction.Messaging;
using What2Gift.Domain.Common;
using What2Gift.Domain.Finance;

namespace What2Gift.Application.Admin.BankAccount.UpdateBankAccountQrCode;

public class UpdateBankAccountQrCodeCommandHandler(IDbContext context) 
    : ICommandHandler<UpdateBankAccountQrCodeCommand, UpdateBankAccountQrCodeResponse>
{
    public async Task<Result<UpdateBankAccountQrCodeResponse>> Handle(
        UpdateBankAccountQrCodeCommand request, 
        CancellationToken cancellationToken)
    {
        // Get the active bank account (should only be one)
        var bankAccount = await context.BankAccounts
            .FirstOrDefaultAsync(b => b.IsActive, cancellationToken);

        if (bankAccount is null)
        {
            return Result.Failure<UpdateBankAccountQrCodeResponse>(
                Error.NotFound("BankAccount.NotFound", "No active bank account found"));
        }

        bankAccount.QrCodeUrl = request.QrCodeUrl;
        await context.SaveChangesAsync(cancellationToken);

        return Result.Success(new UpdateBankAccountQrCodeResponse
        {
            IsSuccess = true,
            Message = "QR code URL updated successfully",
            QrCodeUrl = bankAccount.QrCodeUrl
        });
    }
}

