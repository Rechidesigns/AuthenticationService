using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace AuthService.Data.UserDatas.Model
{
    public class ApplicationUser : IdentityUser
    {

        [MaxLength(100)]
        public required string FirstName { get; set; }

        [MaxLength(100)]
        public required string LastName { get; set; }

        public bool IsSeller { get; set; } = false;

        public ICollection<RefreshToken> RefreshTokens { get; set; }


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

