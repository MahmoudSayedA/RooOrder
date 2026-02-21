
using Application.Common.Abstractions.Data;
using Application.Identity.Services;
using Domain.Entities.Restaurants;

namespace Application.Features.Restaurants.Commands.AddMenuItem
{
    // Create Handler for AddMenuItemCommand
    internal class AddMenuItemHandler(IApplicationDbContext _context, IUser _user) : ICommandHandler<AddMenuItemCommand>
    {
        public async Task Handle(AddMenuItemCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var restaurant = _context.Set<Restaurant>()
                    .FirstOrDefault(r => r.OwnerId == _user.IdGuid)
                    ?? throw new ApplicationException("Restaurant not found for the current user.");


                var menuItem = new MenuItem
                {
                    Name = request.Name,
                    Description = request.Description,
                    Price = request.Price,
                    ImageUrl = request.ImageUrl,
                    CategoryId = request.CategoryId,
                    IsAvailable = request.IsAvailable,
                    IsHidden = request.IsHidden,
                    RestaurantId = restaurant.Id
                };
                await _context.Set<MenuItem>().AddAsync(menuItem);
                await _context.SaveChangesAsync(cancellationToken);

            }
            catch (Exception ex)
            {

                throw new ApplicationException("An error occurred while adding the menu item.", ex);

            }
        }
    }
}
