
namespace What2Gift.Domain.Common.DTO;

public class CreateUserEmailBody : EmailBody
{
    public string VerifyEndpoint { get; set; }
    public string ButtonName { get; set; }
}