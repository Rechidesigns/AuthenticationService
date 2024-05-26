using Microsoft.AspNetCore.Identity;

namespace AuthService.Data.UserDatas.DTOs
{
    public class RegisterResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public IdentityResult IdentityResult { get; set; }
        public UserDetails UserDetails { get; set; }
    }

    public class UserDetails
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
    }

}
