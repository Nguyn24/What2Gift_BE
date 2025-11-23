using What2Gift.Application.Abstraction.Messaging;
using What2Gift.Domain.Common;

namespace What2Gift.Application.Admin.BankAccount.CreateBankAccount;

public class CreateBankAccountCommand : ICommand<CreateBankAccountResponse>
{
    public string BankName { get; init; } = null!;
    public string AccountNumber { get; init; } = null!;
    public string AccountHolderName { get; init; } = null!;
    public string? QrCodeUrl { get; init; }
}

public class CreateBankAccountResponse
{
    public Guid BankAccountId { get; init; }
    public string BankName { get; init; } = null!;
    public string AccountNumber { get; init; } = null!;
    public string AccountHolderName { get; init; } = null!;
    public string? QrCodeUrl { get; init; }
}

