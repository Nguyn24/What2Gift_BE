using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Configuration;
using What2Gift.Application.Abstraction.Services;

namespace What2Gift.Infrastructure.Services;

public class VnPayService : IVnPayService
{
    private readonly IConfiguration _configuration;
    private readonly string _vnp_TmnCode;
    private readonly string _vnp_HashSecret;
    private readonly string _vnp_Url;
    private readonly string _vnp_ReturnUrl;

    public VnPayService(IConfiguration configuration)
    {
        _configuration = configuration;
        _vnp_TmnCode = _configuration["VnPay:TmnCode"] ?? "NMVI1Y1Z";
        _vnp_HashSecret = _configuration["VnPay:HashSecret"] ?? "HSGDTE9AA0X0FJE177SMIKLEDFF6O2AF";
        _vnp_Url = _configuration["VnPay:Url"] ?? "https://sandbox.vnpayment.vn/paymentv2/vpcpay.html";
        _vnp_ReturnUrl = _configuration["VnPay:ReturnUrl"] ?? "https://corvus-fe.vercel.app/payment/callback";
    }

    public string CreatePaymentUrl(Guid userId, Guid membershipPlanId, decimal amount, string returnUrl, string? clientIp = null)
    {
        var vnp_Params = new Dictionary<string, string>
        {
            ["vnp_Version"] = "2.1.0",
            ["vnp_Command"] = "pay",
            ["vnp_TmnCode"] = _vnp_TmnCode,
            ["vnp_Amount"] = ((long)(amount * 100)).ToString(), // Convert to VND (multiply by 100)
            ["vnp_CreateDate"] = DateTime.Now.ToString("yyyyMMddHHmmss"),
            ["vnp_CurrCode"] = "VND",
            ["vnp_IpAddr"] = clientIp ?? "127.0.0.1",
            ["vnp_Locale"] = "vn",
            ["vnp_OrderInfo"] = $"Membership Plan Payment - User: {userId} - Plan: {membershipPlanId}",
            ["vnp_OrderType"] = "other",
            ["vnp_ReturnUrl"] = returnUrl,
            ["vnp_TxnRef"] = $"{userId}_{membershipPlanId}_{DateTime.Now:yyyyMMddHHmmss}"
        };

        // Sort parameters
        var sortedParams = vnp_Params.OrderBy(x => x.Key).ToList();
        
        // Create query string
        var queryString = string.Join("&", sortedParams.Select(x => $"{x.Key}={Uri.EscapeDataString(x.Value)}"));
        
        // Create secure hash
        var secureHash = CreateSecureHash(queryString);
        queryString += $"&vnp_SecureHash={secureHash}";

        return $"{_vnp_Url}?{queryString}";
    }

    public VnPayResponse ProcessPaymentCallback(Dictionary<string, string> queryParams)
    {
        var vnp_Params = new Dictionary<string, string>();
        
        foreach (var item in queryParams)
        {
            if (!string.IsNullOrEmpty(item.Key) && item.Key.StartsWith("vnp_"))
            {
                vnp_Params[item.Key] = item.Value;
            }
        }

        var vnp_SecureHash = vnp_Params.GetValueOrDefault("vnp_SecureHash", "");
        vnp_Params.Remove("vnp_SecureHash");

        // Sort parameters
        var sortedParams = vnp_Params.OrderBy(x => x.Key).ToList();
        var queryString = string.Join("&", sortedParams.Select(x => $"{x.Key}={Uri.EscapeDataString(x.Value)}"));
        
        // Verify secure hash
        var secureHash = CreateSecureHash(queryString);
        var isValidSignature = secureHash.Equals(vnp_SecureHash, StringComparison.OrdinalIgnoreCase);

        return new VnPayResponse
        {
            IsSuccess = isValidSignature && vnp_Params.GetValueOrDefault("vnp_ResponseCode") == "00",
            TransactionId = vnp_Params.GetValueOrDefault("vnp_TxnRef", ""),
            Amount = decimal.Parse(vnp_Params.GetValueOrDefault("vnp_Amount", "0")) / 100, // Convert back from VND
            OrderInfo = vnp_Params.GetValueOrDefault("vnp_OrderInfo", ""),
            ResponseCode = vnp_Params.GetValueOrDefault("vnp_ResponseCode", ""),
            TransactionNo = vnp_Params.GetValueOrDefault("vnp_TransactionNo", ""),
            BankCode = vnp_Params.GetValueOrDefault("vnp_BankCode", ""),
            PayDate = vnp_Params.GetValueOrDefault("vnp_PayDate", ""),
            RawData = vnp_Params
        };
    }

    private string CreateSecureHash(string input)
    {
        using var hmacsha512 = new HMACSHA512(Encoding.UTF8.GetBytes(_vnp_HashSecret));
        var hashBytes = hmacsha512.ComputeHash(Encoding.UTF8.GetBytes(input));
        return Convert.ToHexString(hashBytes).ToLower();
    }
}

