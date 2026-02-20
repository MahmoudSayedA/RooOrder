using Application.Common.Abstractions.Data;
using Application.Common.Exceptions;
using Application.Identity.Services;
using Domain.Entities.Restaurants;

namespace Application.Features.Restaurants.Commands.AddWorkHours
{
    //Create AddWorkHoursCommandHandler

    internal class AddWorkHoursCommandHandler(IApplicationDbContext _dbContext,
        IUser _user) : ICommandHandler<AddWorkHoursCommand, int>
    {

        public async Task<int> Handle(AddWorkHoursCommand request, CancellationToken cancellationToken)
        {
            // using transaction to ensure data integrity

            var transaction = await _dbContext.BeginTransactionAsync(cancellationToken);

            try
            {
                var restaurant = await _dbContext.Set<Restaurant>()
               .Include(r => r.WorkingHours)
               .Where(r => r.Id == request.RestaurantId && r.OwnerId == _user.IdGuid)
               .FirstAsync(cancellationToken)
                ?? throw new NotFoundException("Restaurant not found.");

                foreach (var workHour in request.WorkHours)
                {
                    var existingWorkHour = restaurant.WorkingHours!.FirstOrDefault(wh => wh.DayOfWeek == workHour.DayOfWeek);
                    if (existingWorkHour != null)
                    {
                        existingWorkHour.OpeningTime = workHour.OpeningTime!.Value;
                        existingWorkHour.ClosingTime = workHour.ClosingTime!.Value;
                        existingWorkHour.IsClosed = workHour.IsClosed;
                    }
                    else
                    {
                        restaurant.WorkingHours!.Add(new RestaurantWorkingHours
                        {
                            RestaurantId = restaurant.Id,
                            DayOfWeek = workHour.DayOfWeek,
                            OpeningTime = workHour.OpeningTime!.Value,
                            ClosingTime = workHour.ClosingTime!.Value,
                            IsClosed = workHour.IsClosed
                        });
                    }
                }
                await _dbContext.SaveChangesAsync(cancellationToken);

                await transaction.CommitAsync(cancellationToken);


                return restaurant.Id;


            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync(cancellationToken);

                throw new ApplicationException("An error occurred while adding work hours.", ex);

            }
        }



    }
}
