using System.ComponentModel.DataAnnotations;

namespace AuthService.Data.Auth
{
    public class VerificationModel
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Verification code is required.")]
        [RegularExpression(@"^\d{6}$", ErrorMessage = "Verification code must be exactly 6 digits.")]
        public string Code { get; set; }

        public DateTime DateCreated { get; set; } = DateTime.UtcNow;

        public DateTime DateUpdated { get; set; } = DateTime.UtcNow;
    }
}
