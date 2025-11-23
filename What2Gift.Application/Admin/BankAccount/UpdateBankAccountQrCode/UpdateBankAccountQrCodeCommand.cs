using What2Gift.Application.Abstraction.Messaging;
using What2Gift.Domain.Common;

namespace What2Gift.Application.Admin.BankAccount.UpdateBankAccountQrCode;

public class UpdateBankAccountQrCodeCommand : ICommand<UpdateBankAccountQrCodeResponse>
{
    public string QrCodeUrl { get; init; } = null!;
}

public class UpdateBankAccountQrCodeResponse
{
    public bool IsSuccess { get; init; }
    public string Message { get; init; } = null!;
    public string QrCodeUrl { get; init; } = null!;
}

