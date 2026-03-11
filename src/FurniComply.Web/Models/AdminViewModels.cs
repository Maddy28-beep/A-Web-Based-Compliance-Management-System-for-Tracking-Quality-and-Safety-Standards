using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FurniComply.Web.Models;

public record UserSummary(string Id, string Email, IReadOnlyList<string> Roles);
public record RoleSummary(string Id, string Name, int UserCount, bool IsProtected);

public class UserRolesViewModel
{
    public string Id { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public List<RoleSelection> Roles { get; set; } = new();
}

public class RoleSelection
{
    public string Name { get; set; } = string.Empty;
    public bool Selected { get; set; }
}

public class CreateUserViewModel
{
    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    public string FullName { get; set; } = string.Empty;

    /// <summary>
    /// When true, the user is for assignment/notification only—they won't log in.
    /// Password is auto-generated; they'll receive email when assigned to CAPA tasks.
    /// </summary>
    public bool AssignmentOnly { get; set; }

    [Required(ErrorMessage = "Password is required.")]
    [MinLength(12, ErrorMessage = "Password must be at least 12 characters.")]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^a-zA-Z0-9]).{12,}$",
        ErrorMessage = "Password must be at least 12 characters and include uppercase, lowercase, a digit, and a special character.")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;

    [Required(ErrorMessage = "Confirm password is required.")]
    [Compare(nameof(Password), ErrorMessage = "Passwords do not match.")]
    [DataType(DataType.Password)]
    public string ConfirmPassword { get; set; } = string.Empty;

    public List<RoleSelection> Roles { get; set; } = new();
    public List<string> SelectedRoles { get; set; } = new();
}
