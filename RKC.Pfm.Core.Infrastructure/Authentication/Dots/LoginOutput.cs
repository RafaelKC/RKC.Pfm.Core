namespace RKC.Pfm.Core.Infrastructure.Authentication.Dots;

public class LoginOutput
{
    public bool Success { get; set; }
    public string Token { get; set; }
    public string RefreshToken { get; set; }
}