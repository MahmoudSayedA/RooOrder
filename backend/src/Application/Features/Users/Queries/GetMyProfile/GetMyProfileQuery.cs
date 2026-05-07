using Application.Common.Abstractions.Caching;
using Application.Common.Abstractions.Data;
using Application.Common.Exceptions;
using Application.Features.Users.Models;
using Application.Identity.Services;
using Domain.Entities.Users;

namespace Application.Features.Users.Queries.GetMyProfile;
public class GetMyProfileQuery : ICommand<GetProfileModel>
{

}

public class GetMyProfileQueryHandler : ICommandHandler<GetMyProfileQuery, GetProfileModel>
{
    private readonly IUser _user;
    private readonly ICacheService _cacheService;
    private readonly IApplicationDbContext _dbContext;

    public GetMyProfileQueryHandler(IApplicationDbContext dbContext, ICacheService cacheService, IUser user)
    {
        _user = user;
        _dbContext = dbContext;
        _cacheService = cacheService;
    }

    public async Task<GetProfileModel> Handle(GetMyProfileQuery request, CancellationToken cancellationToken)
    {
        // get cached
        var userId = _user.Id;
        var key = $"profile:{userId}";
        var cached = await _cacheService.GetDataAsync<GetProfileModel>(key, cancellationToken);
        if (cached != null)
        {
            return cached;
        }

        // get from db
        var profile = await _dbContext.Set<ApplicationUser>()
            .AsNoTracking()
            .Where(u => u.Id == _user.IdGuid)
            .Include(u => u.Preferences)
            .Select(u => new GetProfileModel
            {
                Id = u.Id,
                Email = u.Email,
                IsActive = u.IsActive,
                MainRole = u.MainRole,
                UserName = u.UserName,
                Preferences = u.Preferences != null ? u.Preferences.ToDictionary(p => p.Key, p => p.Value) : null
            })
            .FirstOrDefaultAsync(cancellationToken)
            ?? throw new NotFoundException("profile not found");

        // set cache
        if (profile != null)
        {
            await _cacheService.SetDataAsync(key, profile, TimeSpan.FromMinutes(30), cancellationToken);
        }
        return profile!;

    }
}
