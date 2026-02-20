namespace Application.Features.Users.Models;
public class GetProfileModel
{
    public Guid Id { get; set; }
    public string? Email { get; set; }
    public string? UserName { get; set; }
    public bool IsActive { get; set; }
    public string? MainRole { get; set; }
    public Dictionary<string, string?>? Preferences { get; set; }
    // TODO: add favorites and cart

}
