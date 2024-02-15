using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RKC.Pfm.Core.Application.Users;
using RKC.Pfm.Core.Application.Users.Dtos;
using RKC.Pfm.Core.Infrastructure.Authentication.Dots;

namespace RKC.Pfm.Core.Api.User;

[ApiController]
[Route("users")]
public class UserController: ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Create([FromBody] UserCreateInput input)
    {
        var userId = await _userService.Create(input);
        if (userId.HasValue) return Ok();
        return UnprocessableEntity();
    }
}