using FirebaseAdmin.Auth;
using RKC.Pfm.Core.Infrastructure.Authentication.Dots;

namespace RKC.Pfm.Core.Infrastructure.Authentication;

public interface IAuthenticationService
{
    Task<Guid> RegisterAsync(string email, string password, string userName);
    Task<LoginOutput> Login(LoginInput input);
    Task Logout(Guid userId);
}

public class AuthenticationService: IAuthenticationService
{
    private readonly IJwtProvider _jwtProvider;

    public AuthenticationService(IJwtProvider jwtProvider)
    {
        _jwtProvider = jwtProvider;
    }

    public async Task<Guid> RegisterAsync(string email, string password, string userName)
    {
        var createUserArgs = new UserRecordArgs
        {
            Email = email,
            Password = password,
            DisplayName = userName,
            Uid = Guid.NewGuid().ToString()
        };

       var user = await FirebaseAuth.DefaultInstance.CreateUserAsync(createUserArgs);
       return Guid.Parse(user.Uid);
    }

    public async Task<LoginOutput> Login(LoginInput input)
    {
        var authToken = await _jwtProvider.GetForCredentialsAsync(input.email, input.password);
        return new LoginOutput
        {
            Success = !string.IsNullOrWhiteSpace(authToken.IdToken),
            Token = authToken.IdToken,
            RefreshToken = authToken.RefreshToken
        };
    }

    public async Task Logout(Guid userId)
    {
        await FirebaseAuth.DefaultInstance.RevokeRefreshTokensAsync(userId.ToString());
    }
}