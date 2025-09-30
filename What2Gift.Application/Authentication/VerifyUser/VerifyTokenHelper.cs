namespace What2Gift.Application.Authentication.VerifyUser;

public static class VerifyTokenHelper
{
    public static string Encode(string email)
    {
        return Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(email));
    }

    public static string Decode(string token)
    {
        return System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(token));
    }
}
