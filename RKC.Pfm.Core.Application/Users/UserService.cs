using FirebaseAdmin.Auth;
using RKC.Pfm.Core.Application.Transients;
using RKC.Pfm.Core.Application.Users.Dtos;
using RKC.Pfm.Core.Domain.Usuarios;
using RKC.Pfm.Core.Infrastructure.Authentication;

namespace RKC.Pfm.Core.Application.Users;

public interface IUserService
{
    public Task<UserDto?> Get(Guid id);
    public Task<Guid?> Create(UserCreateInput input);
}

public class UserService: IUserService, IAutoTransient
{
    private readonly IAuthenticationService _authenticationService;

    public UserService(IAuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
    }

    public async Task<UserDto?> Get(Guid id)
    {
        try
        {
            var userRecord = await FirebaseAuth.DefaultInstance.GetUserAsync(id.ToString());
            return userRecord != null ? new UserDto(userRecord) : null;
        }
        catch (FirebaseAuthException e)
        {
            return null;
        }
    }

    public async Task<Guid?> Create(UserCreateInput input)
    {
        try
        {
            var alreadyExistentUser = await FirebaseAuth.DefaultInstance.GetUserByEmailAsync(input.Email);
            if (alreadyExistentUser is not null) return null;
        }
        catch (FirebaseAuthException e)
        {
            var userId = await _authenticationService.RegisterAsync(input.Email, input.Password, input.Name);
            return userId;
        }
    
        return null;
    }
}