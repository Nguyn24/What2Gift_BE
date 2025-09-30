using Microsoft.Extensions.Configuration;
using What2Gift.Application.Abstraction.Authentication;

namespace What2Gift.Infrastructure.Authentication;

public class Payload(IConfiguration configuration) : IPayload
{
    public string GoogleClientId => configuration["Google:ClientId"] ?? throw new InvalidOperationException("Google:ClientId is not configured");
}