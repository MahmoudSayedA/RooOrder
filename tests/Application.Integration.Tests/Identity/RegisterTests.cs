using Application.Identity.Commands;

namespace Application.Integration.Tests.Identity;
public class RegisterTests(IntegrationTestWebAppFactory factory) : BaseIntegrationTest(factory)
{
    [Fact]
    public async Task RegisterCommand_WithValidData_ReturnsSuccessMessage()
    {
        // Arrange
        var command = new RegisterCommand
        {
            Email = "test@example.com",
            Username = "testuser",
            Password = "@Test1234",
            ConfirmPassword = "@Test1234",
        };

        // Act
        var result = await _sender.Send(command);

        // Assert
        Assert.NotNull(result);

        var userId = Guid.Parse(result);
        var user = await _dbContext.Users.FindAsync(userId);
        Assert.NotNull(user);
        Assert.Equal(command.Email, user.Email);
        Assert.Equal(command.Username, user.UserName);
        Assert.True(user.IsActive);

    }
}