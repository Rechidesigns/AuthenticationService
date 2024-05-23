using System.ComponentModel.DataAnnotations;

namespace AuthService.Data.UserDatas.DTOs
{
    public class LoginDto
    {
        [Required(ErrorMessage = "Email address is required")]
        public string Email { get; set; }


        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }

        public bool RememberMe { get; set; } = false;
    }
}
