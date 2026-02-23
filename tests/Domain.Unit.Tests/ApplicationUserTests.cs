using Domain.Entities.Users;

namespace Domain.Unit.Tests;
public class ApplicationUserTests
{

    [Fact]
    public void ApplicationUser_MainRole_ReturnsCorrectRole()
    {
        // Arrange
        var admin = new Admin();
        var customer = new Customer();
        var restaurantOwner = new RestaurantOwner();
        // Act & Assert
        Assert.Equal(Constants.Roles.Admin, admin.MainRole);
        Assert.Equal(Constants.Roles.Customer, customer.MainRole);
        Assert.Equal(Constants.Roles.RestaurantOwner, restaurantOwner.MainRole);
    }
}
