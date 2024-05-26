using AuthService.Data.EmailModel;

namespace AuthService.Services.UserManagement.Interface
{
    public interface IMailSender
    {
        Task ForgotPassword(EmailMessageModel model);
        Task ChangePassword(EmailMessageModel model);
        Task Register(EmailMessageModel model);
        Task VerifyEmail(EmailMessageModel model);
    }
}
