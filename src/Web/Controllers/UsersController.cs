using Application.Features.Users.Commands.UpdateUserPreferences;
using Application.Features.Users.Models;
using Application.Features.Users.Queries.GetAllUsersForAdmin;
using Application.Features.Users.Queries.GetMyProfile;
using Application.Identity.Services;
using Domain.Constants;
using Microsoft.AspNetCore.Authorization;

namespace Web.Controllers;


[Route("api/[controller]")]
[ApiController]
public class UsersController: ControllerBase
{
    private readonly ISender _mediator;
    private readonly IUser _user;

    public UsersController(ISender mediator, IUser user)
    {
        _mediator = mediator;
        _user = user;
    }

    [HttpPost("get-all")]
    [Authorize(Roles = Roles.Admin)]
    public async Task<IActionResult> GetAllUsers(GetAllUsersForAdminQuery query, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(query, cancellationToken);
        return Ok(result);
    }

    [HttpGet("my-profile")]
    [Authorize]
    public async Task<IActionResult> GetMyProfile(CancellationToken cancellationToken)
    {
        var query = new GetMyProfileQuery();
        var profile = await _mediator.Send(query, cancellationToken);
        return Ok(profile);
    }


    [HttpPut("update-preferences")]
    [Authorize]
    public async Task<IActionResult> UpdatePreferences([FromBody] UpdateUserPreferencesCommand command, CancellationToken cancellationToken)
    {
        await _mediator.Send(command, cancellationToken);
        return NoContent();
    }

}
