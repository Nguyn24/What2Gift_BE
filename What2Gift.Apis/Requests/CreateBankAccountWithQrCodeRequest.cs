using Microsoft.AspNetCore.Http;

namespace What2Gift.Apis.Requests;

public class CreateBankAccountWithQrCodeRequest
{
    public string BankName { get; set; } = null!;
    public string AccountNumber { get; set; } = null!;
    public string AccountHolderName { get; set; } = null!;
    public IFormFile? QrCodeImage { get; set; }
}

