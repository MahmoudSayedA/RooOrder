using Application.Common.Abstractions.Data;
using Application.Common.Exceptions;
using Application.Identity.Services;
using Domain.Entities.Restaurants;

namespace Application.Features.Restaurants.Commands.UpdateRestaurant
{
    // Create Handler for the command

    public class UpdateRestaurantCommandHandler(IApplicationDbContext _dbContext,
        IUser _user) : ICommandHandler<UpdateRestaurantCommand, int>
    {

        public async Task<int> Handle(UpdateRestaurantCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var restaurant = await _dbContext.Set<Restaurant>()
               .Where(x => x.Id == request.Id && x.OwnerId == _user.IdGuid)
               .FirstOrDefaultAsync(cancellationToken)
               ?? throw new NotFoundException("Restaurant not found.");

                restaurant.Name = request.Name ?? restaurant.Name;
                restaurant.Description = request.Description ?? restaurant.Description;
                restaurant.Address = request.Address ?? restaurant.Address;
                restaurant.Region = request.Region ?? restaurant.Region;
                restaurant.City = request.City ?? restaurant.City;
                restaurant.IsActive = request.IsActive;

                _dbContext.Set<Restaurant>().Update(restaurant);
                await _dbContext.SaveChangesAsync(cancellationToken);
                return restaurant.Id;
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.Message);
            }
        }
    }

}
