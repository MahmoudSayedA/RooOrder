using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities.Users;
public class UserPreference
{
    public required Guid UserId { get; set; }
    public required string Key { get; set; }
    public string? Value { get; set; }
    public ApplicationUser User { get; set; } = null!;
}
