using Application.Common.Abstractions.Data;
using Application.Features.Users.Commands.UpdateUserPreferences;
using Application.Identity.Services;
using Domain.Entities.Users;
using Microsoft.EntityFrameworkCore;
using Moq;
using Moq.EntityFrameworkCore;


namespace Application.Unit.Tests.Features.Users;
public class UpdateUserPreferencesTests
{
    [Fact]
    public async Task Handle_ShouldUpdatePreferences()
    {
        # region Arrange
        var userId = Guid.NewGuid();

        var existingPref = new List<UserPreference>
        {
            new() { UserId = userId, Key = "theme", Value = "light" },
            new() { UserId = userId, Key = "language", Value = "en" }
        }.AsQueryable();

        var dbContextMock = new Mock<IApplicationDbContext>();
        dbContextMock.Setup(db => db.Set<UserPreference>()).ReturnsDbSet(existingPref);

        var userMock = new Mock<IUser>();
        userMock.Setup(u => u.IdGuid).Returns(userId);

        var handler = new UpdateUserPreferencesCommandHandler(dbContextMock.Object, userMock.Object);
        var command = new UpdateUserPreferencesCommand
        {
            Preferences = new Dictionary<string, string?>
            {
                { "theme", "dark" },
                { "language", "en" },
                { "newKey",   "value"} //should be added
            }
        };
        # endregion

        // Act
        await handler.Handle(command, CancellationToken.None);

        // Assert
        dbContextMock.Verify(db => db.Set<UserPreference>().UpdateRange(It.IsAny<List<UserPreference>>()), Times.Once);
        dbContextMock.Verify(db => db.Set<UserPreference>().AddRangeAsync(It.IsAny<List<UserPreference>>(), It.IsAny<CancellationToken>()), Times.Once);

        //verify UpdateRange was called with the new one
        dbContextMock.Verify(db => db.Set<UserPreference>().UpdateRange(
                    It.Is<IEnumerable<UserPreference>>(list =>
                        list.Any(p => p.Key == "theme" && p.Value == "dark") &&
                        list.Any(p => p.Key == "language" && p.Value == "en")
                    )), Times.Once());

        //verify AddRangeAsync was called with the new one
        dbContextMock.Verify(db => db.Set<UserPreference>().AddRangeAsync(
            It.Is<IEnumerable<UserPreference>>(list =>
                list.Any(p => p.Key == "newKey" && p.Value == "value")
            ),
            It.IsAny<CancellationToken>()), Times.Once());
    }
}
