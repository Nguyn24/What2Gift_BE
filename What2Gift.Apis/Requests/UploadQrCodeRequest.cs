using Microsoft.AspNetCore.Http;

namespace What2Gift.Apis.Requests;

public class UploadQrCodeRequest
{
    public IFormFile QrCodeImage { get; set; } = null!;
}

