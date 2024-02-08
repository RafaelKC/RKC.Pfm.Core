using FirebaseAdmin.Auth;
using RKC.Pfm.Core.Infrastructure.Transients;

namespace RKC.Pfm.Core.Infrastructure;

public interface IAuthenticationService
{
    Task<Guid> RegisterAsync(string email, string password, string userName);
}

public class AuthenticationService: IAuthenticationService, IAutoTransient
{
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
}