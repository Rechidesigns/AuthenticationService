using AuthService.Core.Shared;

namespace AuthService.Data.EmailModel
{
    public class SentEmailOtp : BaseEntity
    {
        public string Email { get; set; }
        public string OTP { get; set; }
        public string Purpose { get; set; }
        public string Subject { get; set; }
        public string HtmlContent { get; set; }
    }
}