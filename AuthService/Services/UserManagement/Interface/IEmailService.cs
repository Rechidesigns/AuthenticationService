using AuthService.Core.Services;

namespace AuthService.Services.UserManagement.Interface
{
    public interface IEmailService
    {
        Task SendEmailAsync(MailRequestService mailRequest);

    }
}
