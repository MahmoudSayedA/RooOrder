using Application.Common.Abstractions.Data;
using Application.Identity.Services;
using Domain.Entities.Restaurants;

namespace Application.Features.Restaurants.Commands.AddMenuItem
{
    // Create validator for AddMenuCommand

    internal class AddMenuItemCommandValidator : AbstractValidator<AddMenuItemCommand>
    {
        public AddMenuItemCommandValidator(IUser _user, IApplicationDbContext _dbContext)
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required.");
            RuleFor(x => x.Price).GreaterThan(0).WithMessage("Price must be greater than 0.");
            RuleFor(x => x.Description).NotEmpty().MaximumLength(500).WithMessage("Description cannot be longer than 500 characters.");
            RuleFor(x => x.CategoryId).GreaterThan(0).When(x => x.CategoryId.HasValue).WithMessage("CategoryId must be greater than 0.");

            // Check if user owner of restaurant  has assignment restaurant 

            RuleFor(x => x).MustAsync(async (command, cancellation) =>
            {
                return !await _dbContext.Set<Restaurant>().AnyAsync(r => r.OwnerId == _user.IdGuid);
            }).WithMessage("User must have a restaurant assignment to add a menu item.");

            // Check if category exists in restaurant
            RuleFor(x => x).MustAsync(async (command, cancellation) =>
            {

                if (!command.CategoryId.HasValue)
                    return true; // If no category is provided, skip this validation

                return await _dbContext.Set<Category>().AnyAsync(c => c.Id == command.CategoryId);
            }).WithMessage("Category does not exist.");

        }

    }


}
