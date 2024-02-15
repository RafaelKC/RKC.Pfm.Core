using Microsoft.Extensions.Caching.Distributed;
using RKC.Pfm.Core.Domain.Usuarios;
using RKC.Pfm.Core.Infrastructure.Authentication.Consts;
using RKC.Pfm.Core.Infrastructure.Authentication.Dots;
using RKC.Pfm.Core.Infrastructure.Supabse;
using Supabase.Gotrue;
using Supabase.Gotrue.Exceptions;

namespace RKC.Pfm.Core.Infrastructure.Authentication;

public interface IAuthenticationService
{
    Task<Guid?> RegisterAsync(string email, string password, string userName);
    Task<LoginOutput> Login(LoginInput input);
    Task Logout();
}

public class AuthenticationService: IAuthenticationService
{
    private readonly ISupabseClient _supabseClient;
    private readonly IDistributedCache _distributedCache;

    public AuthenticationService(ISupabseClient supabseClient, IDistributedCache distributedCache)
    {
        _supabseClient = supabseClient;
        _distributedCache = distributedCache;
    }


    public async Task<Guid?> RegisterAsync(string email, string password, string userName)
    {
        var userData = new UserDto
        {
            Email = email,
            Name = userName
        };

        try
        {
            var session = await _supabseClient.Auth.SignUp(email, password, new SignUpOptions
            {
                Data = new Dictionary<string, object>
                {
                    { "user", userData },
                }
            });
            
            return session?.User is not null ? Guid.Parse(session.User.Id) : null;
        }
        catch (GotrueException e)
        {
            return null;
        }
    }

    public async Task<LoginOutput> Login(LoginInput input)
    {
        var session = await _supabseClient.Auth.SignIn(input.email, input.password);
        
        return new LoginOutput
        {
            Success = session.User is not null,
            Token = session.AccessToken,
            RefreshToken = session.RefreshToken
        };
    }

    public async Task Logout()
    {
        var token = _supabseClient.Auth.CurrentSession?.AccessToken;
        var refresh = _supabseClient.Auth.CurrentSession?.RefreshToken;
        
        
        await _supabseClient.Auth.SignOut();
        await _distributedCache.SetStringAsync(AuthenticationConfig.GetTokenCacheKey(token), token, new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(3600)
        });
        await _distributedCache.SetStringAsync(AuthenticationConfig.GetRefreshTokenCacheKey(refresh), refresh, new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(3600)
        });
    }
}