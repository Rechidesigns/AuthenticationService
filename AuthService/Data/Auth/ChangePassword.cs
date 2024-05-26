using System.ComponentModel.DataAnnotations;

namespace AuthService.Data.Auth
{
    public class ChangePassword
    {
        [Required]
        [DataType(DataType.Password)]
        public string OldPassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [StringLength(100, MinimumLength = 5, ErrorMessage = "The new password must be at least 5 characters long and not exceed 100 characters.")]
        [RegularExpression(@"^(?=.*\d)(?=.*[a-z])(?=.*[A-Z]).{5,}$", ErrorMessage = "The new password must include at least one uppercase letter, one lowercase letter, and one numeric digit.")]
        public string NewPassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmNewPassword { get; set; }
    }
}
