namespace RKC.Pfm.Core.Infrastructure.Authentication.Dots;

public class AuthToken
{
    public string IdToken { get; set; }
    public string Email { get; set; }
    public string RefreshToken { get; set; }
    public long ExpiresIn { get; set; }
    public string LocalId { get; set; }
    public bool Registered { get; set; }
}