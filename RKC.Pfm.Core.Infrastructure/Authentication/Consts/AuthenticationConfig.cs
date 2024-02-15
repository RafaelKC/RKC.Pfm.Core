namespace RKC.Pfm.Core.Infrastructure.Authentication.Consts;

public static class  AuthenticationConfig
{
    public const string TokenCacheKey = "InvalidToken";
    public const string RefreshTokenCacheKey = "InvalidRefreshToken";

    public static string GetTokenCacheKey(string token)
    {
        return $"{TokenCacheKey}-{token}";
    }
    
    public static string GetRefreshTokenCacheKey(string token)
    {
        return $"{RefreshTokenCacheKey}-{token}";
    }
}