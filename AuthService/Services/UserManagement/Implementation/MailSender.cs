using AuthService.Data.EmailModel;
using AuthService.Services.UserManagement.Interface;
using NETCore.MailKit.Core;
using AuthService.Core.Services;
using IEmailService = AuthService.Services.UserManagement.Interface.IEmailService;

namespace AuthService.Services.UserManagement.Implementation
{
    public class MailSender : IMailSender
    {
        private readonly IEmailService _emailService;

        public MailSender(IEmailService emailService)
        {
            _emailService = emailService;
        }


        public async Task ChangePassword(EmailMessageModel model)
        {
            var emailDto = new MailRequestService();

            emailDto.Subject = "Password Change Alert";

            var temp = await File.ReadAllTextAsync(Path.Combine(Directory.GetCurrentDirectory(), "StaticFiles/ChangePassword.json"));
            var body = temp.Replace("Hi**", model.FirstName );

            emailDto.Body = body;
            emailDto.To = model.Email;

            await _emailService.SendEmailAsync(emailDto);

        }

        public async Task ForgotPassword(EmailMessageModel model)
        {
            var emailDto = new MailRequestService();

            emailDto.Subject = "Password Reset";

            var temp = await File.ReadAllTextAsync(Path.Combine(Directory.GetCurrentDirectory(), "StaticFiles/ForgetPassword.json"));
            var firstBody = temp.Replace("Hi**", model.FirstName);
            var secondBody = firstBody.Replace("otp**", model.Otp);

            emailDto.Body = secondBody;
            emailDto.To = model.Email;

            await _emailService.SendEmailAsync(emailDto);

        }
        public async Task Register(EmailMessageModel model)
        {
            var emailDto = new MailRequestService();

            emailDto.Subject = "HouseMate Registration";

            var temp = await File.ReadAllTextAsync(Path.Combine(Directory.GetCurrentDirectory(), "StaticFiles/RegistrationMail.json"));
            var Body = temp.Replace("Hi**", model.FirstName);

            emailDto.Body = Body;
            emailDto.To = model.Email;

            await _emailService.SendEmailAsync(emailDto);

        }

        public async Task VerifyEmail(EmailMessageModel model)
        {
            var emailDto = new MailRequestService();

            emailDto.Subject = "Verify Email";

            var temp = await File.ReadAllTextAsync(Path.Combine(Directory.GetCurrentDirectory(), "StaticFiles/VerifyEmail.json"));
            var firstBody = temp.Replace("Hi**", model.FirstName);
            var secondBody = firstBody.Replace("otp**", model.Otp);

            emailDto.Body = secondBody;
            emailDto.To = model.Email;

            await _emailService.SendEmailAsync(emailDto);

        }
    }
}
