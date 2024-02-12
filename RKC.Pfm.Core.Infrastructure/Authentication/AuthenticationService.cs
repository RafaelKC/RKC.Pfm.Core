using RKC.Pfm.Core.Domain.Usuarios;
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

    public AuthenticationService(ISupabseClient supabseClient)
    {
        _supabseClient = supabseClient;
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
        await _supabseClient.Auth.SignOut();
    }
}