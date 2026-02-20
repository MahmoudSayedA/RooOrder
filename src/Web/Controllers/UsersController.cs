using Application.Features.Users.Models;
using Application.Features.Users.Queries.GetAllUsersForAdmin;
using Application.Identity.Services;
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
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetAllUsers(GetAllUsersForAdminQuery query, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(query, cancellationToken);
        return Ok(result);
    }

    //[HttpGet("my-info")]
    //[Authorize]
    //public async Task<IActionResult> GetCurrentUser(CancellationToken cancellationToken)
    //{
        
    //}


}
