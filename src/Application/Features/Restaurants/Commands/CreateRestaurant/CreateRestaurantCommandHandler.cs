
using Application.Common.Abstractions.Data;
using Domain.Entities.Restaurants;

namespace Application.Features.Restaurants.Commands.CreateRestaurant;

public partial class CreateRestaurantCommandHandler(IApplicationDbContext dbContext) : ICommandHandler<CreateRestaurantCommand>
{
    public async Task Handle(CreateRestaurantCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var restaurant = new Restaurant
            {
                Name = request.Name,
                Description = request.Description,
                Address = request.Address,
                Region = request.Region,
                City = request.City,
                IsActive = request.IsActive,
                OwnerId = Guid.Parse(request.Id)

            };

            await dbContext.Set<Restaurant>().AddAsync(restaurant);
            await dbContext.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            throw new Exception($"An error occurred while creating the restaurant: {ex.Message}", ex);
        }




    }


}