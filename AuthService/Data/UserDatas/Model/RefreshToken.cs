using AuthService.Core.Shared;

namespace AuthService.Data.UserDatas.Model
{
    public class RefreshToken : BaseEntity
    {
        public string UserId { get; set; }
        public string Token { get; set; }
        public DateTime ExpiryTime { get; set; }
        public bool IsActive { get; set; }

        public ApplicationUser User { get; set; }
    }
}
