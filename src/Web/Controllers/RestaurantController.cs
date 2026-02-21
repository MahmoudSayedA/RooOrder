using Application.Features.Restaurants.Commands.AddMenuItem;
using Application.Features.Restaurants.Commands.AddWorkHours;
using Application.Features.Restaurants.Commands.CreateRestaurant;
using Application.Features.Restaurants.Commands.UpdateRestaurant;
using Domain.Constants;
using Microsoft.AspNetCore.Authorization;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RestaurantController(ISender mediator) : ControllerBase
    {
        private readonly ISender _mediator = mediator;

        [HttpPost("add-menu-item")]
        [Authorize(Roles = Roles.RestaurantOwner)]
        public async Task<IActionResult> AddMenuItem([FromBody] AddMenuItemCommand command)
        {
            await _mediator.Send(command);
            return NoContent();
        }

        [HttpPost("create")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> CreateRestaurant([FromBody] CreateRestaurantCommand command)
        {
            await _mediator.Send(command);
            return Ok();
        }

        [HttpPatch("update-menu-item")]
        [Authorize(Roles = Roles.RestaurantOwner)]
        public async Task<IActionResult> UpdateMenuItem([FromBody] UpdateRestaurantCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

        [HttpPost("add-work-hours")]
        [Authorize(Roles = Roles.RestaurantOwner)]
        public async Task<IActionResult> AddWorkHours([FromBody] AddWorkHoursCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

        // list of days of week
        [HttpGet("days-of-week")]
        public IActionResult GetDaysOfWeek()
        {
            var daysOfWeek = Enum.GetValues(typeof(DayOfWeek))
                .Cast<DayOfWeek>()
                .Select(d => new { Id = (int)d, Name = d.ToString() })
                .ToList();
            return Ok(daysOfWeek);
        }




    }
}
