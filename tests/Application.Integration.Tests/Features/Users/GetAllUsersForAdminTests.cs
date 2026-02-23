using Application.Common.Exceptions;
using Application.Common.Models;
using Application.Features.Users.Models;
using Application.Features.Users.Queries.GetAllUsersForAdmin;

namespace Application.Integration.Tests.Features.Users;
public class GetAllUsersForAdminTests(IntegrationTestWebAppFactory factory) : BaseIntegrationTest(factory)
{

    [Fact]
    public async Task GetAllUsersForAdminQuery_ReturnsPaginatedListWithCount()
    {
        // Arrange
        var query = new GetAllUsersForAdminQuery
        {
            PageNumber = 1,
            PageSize = 10,
            SortBy = "userName",
            SortDirection = "asc"
        };

        // Act
        var result = await _sender.Send(query);

        // Assert
        Assert.NotNull(result);
        Assert.IsType<PaginatedListWithCount<GetUserModel>>(result);
        Assert.True(result.Items.Count <= query.PageSize);
    }

    [Fact]
    public async Task GetAllUsersForAdminQuery_WithInvalidFilter_ThrowsException()
    {
        // Arrange
        var query = new GetAllUsersForAdminQuery
        {
            PageNumber = 1,
            PageSize = 10,
            SortBy = "userName",
            SortDirection = "asc",
            Filters = new Dictionary<string, string> { { "invalidFilter", "value" } }
        };
        // Act & Assert
        await Assert.ThrowsAsync<ValidationException>(() => _sender.Send(query));
    }

    [Fact]
    public async Task GetAllUsersForAdminQuery_WithInvalidSorting_ThrowsException()
    {
        // Arrange
        var query = new GetAllUsersForAdminQuery
        {
            PageNumber = 1,
            PageSize = 10,
            SortBy = "invalidSort",
            SortDirection = "asc"
        };
        // Act & Assert
        await Assert.ThrowsAsync<ValidationException>(() => _sender.Send(query));
    }


}