using Application.Common.Abstractions.Data;
using Application.Identity.Services;
using Domain.Entities.Users;

namespace Application.Features.Users.Commands.UpdateUserPreferences;
public class UpdateUserPreferencesCommand : ICommand
{
    public Dictionary<string, string?> Preferences { get; set; } = null!;
}

public class UpdateUserPreferencesCommandHandler : ICommandHandler<UpdateUserPreferencesCommand>
{
    private readonly IApplicationDbContext _dbContext;
    private readonly IUser _user;
    public UpdateUserPreferencesCommandHandler(IApplicationDbContext dbContext, IUser user)
    {
        _dbContext = dbContext;
        _user = user;
    }
    public async Task Handle(UpdateUserPreferencesCommand request, CancellationToken cancellationToken)
    {
        var userId = _user.IdGuid;
        // get existing preferences
        var existingPreferences = await _dbContext.Set<UserPreference>()
            .AsTracking()
            .Where(p => p.UserId == userId)
            .ToListAsync(cancellationToken);

        // update or add preferences
        List<UserPreference> preferencesToAdd = new List<UserPreference>();
        foreach (var kvp in request.Preferences)
        {
            var existing = existingPreferences.FirstOrDefault(p => p.Key == kvp.Key);
            if (existing != null)
            {
                existing.Value = kvp.Value;
            }
            else
            {
                var newPreference = new UserPreference
                {
                    UserId = (Guid)userId!,
                    Key = kvp.Key,
                    Value = kvp.Value
                };
                preferencesToAdd.Add(newPreference);
            }
        }

        _dbContext.Set<UserPreference>().UpdateRange(existingPreferences);
        await _dbContext.Set<UserPreference>().AddRangeAsync(preferencesToAdd, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

    }
}