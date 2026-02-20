using Application.Common.Abstractions.Data;
using Application.Identity.Services;
using Domain.Entities.Restaurants;

namespace Application.Features.Restaurants.Commands.UpdateRestaurant
{
    public class UpdateRestaurantCommand : ICommand<int>
    {
        public required int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Address { get; set; }
        public string? Region { get; set; }
        public string? City { get; set; }
        public bool IsActive { get; set; } = true;
    }


    internal class UpdateRestaurantCommandValidator : AbstractValidator<UpdateRestaurantCommand>
    {

        public UpdateRestaurantCommandValidator(IApplicationDbContext _dbContext, IUser _user)
        {

            RuleFor(x => x.Id).NotEmpty().WithMessage("Id is required.")
                .MustAsync(async (id, cancellation) =>
                {
                    return await _dbContext.Set<Restaurant>().AnyAsync(r => r.Id == id, cancellation);
                }).WithMessage("Restaurant with the specified Id does not exist.");
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required.");
            RuleFor(x => x.Description).NotEmpty().WithMessage("Description is required.");
            RuleFor(x => x.Address).NotEmpty().WithMessage("Address is required.");
            RuleFor(x => x.Region).NotEmpty().WithMessage("Region is required.");
            RuleFor(x => x.City).NotEmpty().WithMessage("City is required.");

            //check if user is authenticated and using _user.id compare with restaurant owner id in database using restaurant id from command and if not match return error message "You are not authorized to update this restaurant."
            RuleFor(x => x.Id).MustAsync(async (id, cancellation) =>
            {

                return await _dbContext.Set<Restaurant>().AnyAsync(
                    X => X.Id == id && X.OwnerId == Guid.Parse(_user.Id), cancellation);
            }).WithMessage("You are not authorized to update this restaurant.");

            // check if user is restaurant owner and 


        }
    }
}
