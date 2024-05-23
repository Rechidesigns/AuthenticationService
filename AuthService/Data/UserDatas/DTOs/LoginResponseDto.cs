using System.ComponentModel.DataAnnotations;

namespace AuthService.Data.UserDatas.DTOs
{
    public class LoginResponseDto
    {
        public string Token { get; set; }
        public string UserId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public bool IsVerified { get; set; }
        public string RefreshToken { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }

    }
    public class RefreshTokenNewRequestModel
    {
        [Required] public string AccessToken { get; set; }
        [Required] public string RefreshToken { get; set; }
    }
}
