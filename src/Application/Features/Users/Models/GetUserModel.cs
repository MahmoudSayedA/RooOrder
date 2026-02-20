namespace Application.Features.Users.Models;
public class GetUserModel
{
    public Guid Id { get; set; }
    public string? Email { get; set; }
    public string? UserName { get; set; }
    public bool IsActive { get; set; }
    public string? MainRole { get; set; }
}
