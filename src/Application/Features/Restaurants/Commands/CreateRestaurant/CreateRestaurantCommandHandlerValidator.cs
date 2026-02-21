
using Application.Common.Abstractions.Data;
using Application.Identity.Services;
using Domain.Entities.Users;

namespace Application.Features.Restaurants.Commands.CreateRestaurant;

public partial class CreateRestaurantCommandHandler
{
    // Create a validator for the command

    internal class CreateRestaurantCommandHandlerValidator : AbstractValidator<CreateRestaurantCommand>
    {

        public CreateRestaurantCommandHandlerValidator(IApplicationDbContext _dbContext, IUser _user)
        {

            RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required.");
            RuleFor(x => x.Description).NotEmpty().WithMessage("Description is required.");
            RuleFor(x => x.Address).NotEmpty().WithMessage("Address is required.");
            RuleFor(x => x.Region).NotEmpty().WithMessage("Region is required.");
            RuleFor(x => x.City).NotEmpty().WithMessage("City is required.");


            //check if id is find in RestaurantOwner 
            RuleFor(x => x.Id).MustAsync(async (id, cancellation) =>
            {
                return await _dbContext.Set<RestaurantOwner>().AnyAsync(r => r.Id == Guid.Parse(id), cancellation);
            }).WithMessage("Restaurant owner with the specified Id does not exist.");

        }
    }


}