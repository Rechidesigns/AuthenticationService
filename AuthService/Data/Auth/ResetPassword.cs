using System.ComponentModel.DataAnnotations;

namespace AuthService.Data.Auth
{
    public class ResetPassword
    {
        [Required(ErrorMessage = "New password is required.")]
        [DataType(DataType.Password)]
        [StringLength(100, MinimumLength = 5, ErrorMessage = "The new password must be at least 5 characters long and not exceed 100 characters.")]
        [RegularExpression(@"^(?=.*\d)(?=.*[a-z])(?=.*[A-Z]).{5,}$", ErrorMessage = "The new password must include at least one uppercase letter, one lowercase letter, and one numeric digit.")]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string Email { get; set; }
    }
    public class OTPVerificationDTO
    {
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "OTP is required.")]
        [RegularExpression(@"^\d{6}$", ErrorMessage = "OTP must be exactly 6 digits.")]
        public string OTP { get; set; }
    }
}
