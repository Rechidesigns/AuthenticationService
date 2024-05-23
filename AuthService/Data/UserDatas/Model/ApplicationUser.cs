using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace AuthService.Data.UserDatas.Model
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [MaxLength(100)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(100)]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        [MaxLength(100)]
        public string Email { get; set; }

        public bool IsSeller { get; set; }

    }

    public class PersistedLogin
    {
        [Key]
        [Required] public Guid UserId { get; set; }
        [Required] public DateTime DateCreated { get; set; } = DateTime.UtcNow;
        public DateTime? LastUpdatedOn { get; set; }
        [Required, StringLength(256)] public string RefreshToken { get; set; }
        [Required] public DateTime RefreshTokenExpiryTime { get; set; }

    }
}

