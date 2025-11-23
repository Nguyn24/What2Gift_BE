namespace What2Gift.Apis.Requests;

public class CreateBankAccountRequest
{
    public string BankName { get; set; } = null!;
    public string AccountNumber { get; set; } = null!;
    public string AccountHolderName { get; set; } = null!;
    public string? QrCodeUrl { get; set; }
}

