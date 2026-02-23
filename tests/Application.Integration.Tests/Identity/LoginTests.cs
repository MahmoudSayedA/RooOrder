using Application.Common.Models;
using Application.Identity.Commands;

namespace Application.Integration.Tests.Identity;
public class LoginTests(IntegrationTestWebAppFactory factory) : BaseIntegrationTest(factory)
{
    [Fact]
    public async Task LoginCommand_WithValidCredentials_ReturnsLoginResponseModel()
    {
        #region Arrange
        // First, register a new user to ensure we have valid credentials to test with
        var command = new RegisterCommand
        {
            Email = "test@example.com",
            Username = "testuser",
            Password = "@Test1234",
            ConfirmPassword = "@Test1234",
        };
        var _ = await _sender.Send(command);

        var loginCommand = new LoginCommand { Email = "test@example.com", Password = "@Test1234" };
        #endregion

        // Act
        var loginResult = await _sender.Send(loginCommand);

        // Assert
        Assert.NotNull(loginResult);
        Assert.IsType<LoginResponseModel>(loginResult);
        Assert.False(string.IsNullOrEmpty(loginResult.Token));

    }
}
