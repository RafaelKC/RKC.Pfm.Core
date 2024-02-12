using RKC.Pfm.Core.Application.Transients;
using RKC.Pfm.Core.Application.Users.Dtos;
using RKC.Pfm.Core.Infrastructure.Authentication;
using RKC.Pfm.Core.Infrastructure.Authentication.Dots;

namespace RKC.Pfm.Core.Application.Users;

public interface IUserService
{
    public Task<Guid?> Create(UserCreateInput input);
}

public class UserService: IUserService, IAutoTransient
{
    private readonly IAuthenticationService _authenticationService;

    public UserService(
        IAuthenticationService authenticationService
        )
    {
        _authenticationService = authenticationService;
    }

    public async Task<Guid?> Create(UserCreateInput input)
    {
        var userId = await _authenticationService.RegisterAsync(input.Email, input.Password, input.Name);
        return userId;
    }
}