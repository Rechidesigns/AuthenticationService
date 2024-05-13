using Microsoft.AspNetCore.Identity;

namespace AuthService.Data.Model
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string email { get; set; }
        public byte[] PasswordHash { get; set; }
        public bool IsSeller { get; set; }

    }
}


//id PK 
//date_created          datetime
//updated_at            datetime