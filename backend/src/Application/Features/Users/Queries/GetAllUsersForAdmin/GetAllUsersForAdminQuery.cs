using Application.Common.Abstractions.Collections;
using Application.Common.Abstractions.Data;
using Application.Common.Extensions;
using Application.Common.Models;
using Application.Features.Users.Models;
using Domain.Entities.Users;

namespace Application.Features.Users.Queries.GetAllUsersForAdmin;
public class GetAllUsersForAdminQuery : HasTableView, ICommand<PaginatedListWithCount<GetUserModel>>
{

}

public class GetAllUsersForAdminQueryHandler : ICommandHandler<GetAllUsersForAdminQuery, PaginatedListWithCount<GetUserModel>>
{
    private readonly IApplicationDbContext _dbContext;

    public GetAllUsersForAdminQueryHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<PaginatedListWithCount<GetUserModel>> Handle(GetAllUsersForAdminQuery request, CancellationToken cancellationToken)
    {
        // Validate filters and sorting
        List<string> allowedFilters = ["id","userName", "email", "mainRole", "isActive"];
        List<string> allowedSorting = ["id","userName", "email", "mainRole"];
        request.ValidateFiltersAndSorting(allowedFilters, allowedSorting);

        // Build the query
        var baseQuery = _dbContext.Set<ApplicationUser>().AsNoTracking();

        var filteredQuery = baseQuery.ApplyFilters(request.Filters);
        var sortedQuery = filteredQuery.ApplySorting(request.SortBy, request.SortDirection);

        // Project to GetUserModel
        var projectedQuery = sortedQuery.Select(u => new GetUserModel
        {
            Id = u.Id,
            Email = u.Email,
            UserName = u.UserName,
            IsActive = u.IsActive,
            MainRole = u.MainRole,
        });

        // Return paginated result
        var result = projectedQuery.ToPaginatedListWithCountAsync(request.PageNumber, request.PageSize, cancellationToken);
        return result;
    }
}
