using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RKC.Pfm.Core.Infrastructure.Authentication;
using RKC.Pfm.Core.Infrastructure.Authentication.Dots;

namespace RKC.Pfm.Core.Api.User;

[ApiController]
public class LoginController: ControllerBase
{
    private readonly IAuthenticationService _authenticationService;

    public LoginController(IAuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<ActionResult<LoginOutput>> Login([FromBody] LoginInput input)
    {
        var result = await _authenticationService.Login(input);
        if (result.Success) return result;
        return UnprocessableEntity(result);
    }
    
    [HttpPost("logout/{userId:guid}")]
    [Authorize]
    public async Task<ActionResult> Logout([FromRoute] Guid userId)
    {
        await _authenticationService.Logout(userId);
        return Ok();
    }
    
}