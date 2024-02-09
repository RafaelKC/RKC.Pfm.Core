using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RKC.Pfm.Core.Application.Users;
using RKC.Pfm.Core.Application.Users.Dtos;
using RKC.Pfm.Core.Domain.Usuarios;

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

    [HttpGet("{userId:guid}")]
    [Authorize]
    public async Task<ActionResult<UserDto>> Get([FromRoute] Guid userId)
    {
        var user = await _userService.Get(userId);
        if (user is not null) return user;
        return NotFound();
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Create([FromBody] UserCreateInput input)
    {
        var userId = await _userService.Create(input);
        if (userId is not null) return Ok();
        return UnprocessableEntity();
    }
}