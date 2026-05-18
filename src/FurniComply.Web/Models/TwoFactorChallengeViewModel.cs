using System.ComponentModel.DataAnnotations;

namespace FurniComply.Web.Models;

public class TwoFactorChallengeViewModel
{
    [Required(ErrorMessage = "Verification code is required.")]
    [StringLength(7, MinimumLength = 6, ErrorMessage = "Code must be 6 digits.")]
    [Display(Name = "Verification Code")]
    public string Code { get; set; } = string.Empty;

    public string? ReturnUrl { get; set; }
    public bool RememberMe { get; set; }
    public bool RememberMachine { get; set; }
}
