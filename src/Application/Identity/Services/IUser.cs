using System.Security.Claims;

namespace Application.Identity.Services;

public interface IUser
{
    string? Id { get; }
    public Guid? IdGuid => Guid.TryParse(Id, out var guid) ? guid : null;
    public string? UserName { get; }
    public string? Email { get; }
    public IEnumerable<string> Roles { get; }

    public bool IsAuthenticated { get; }

}
