using System.ComponentModel.DataAnnotations;

namespace FurniComply.Web.Models;

public class RequestAccessViewModel
{
    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
    [Display(Name = "Work Email")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Full name is required.")]
    [StringLength(200, ErrorMessage = "Name cannot exceed 200 characters.")]
    [Display(Name = "Full Name")]
    public string FullName { get; set; } = string.Empty;

    [StringLength(1000, ErrorMessage = "Reason cannot exceed 1000 characters.")]
    [Display(Name = "Reason for Access (optional)")]
    public string? Reason { get; set; }

    [StringLength(100)]
    [Display(Name = "Preferred Role (optional)")]
    public string? PreferredRole { get; set; }
}
