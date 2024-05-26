using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using AuthService.Core.Services;
using AuthService.Services.UserManagement.Interface;

namespace AuthService.Services.UserManagement.Implementation
{
    public class EmailClientService : IEmailService
    {
        private readonly MailClientService _mailClientService;


        public EmailClientService(IOptions<MailClientService> mailClientService)
        {
            _mailClientService = mailClientService.Value;
        }


        public async Task SendEmailAsync(MailRequestService mailRequest)
        {
            var email = new MimeMessage();
            email.Sender = MailboxAddress.Parse(_mailClientService.Email);
            email.To.Add(MailboxAddress.Parse(mailRequest.To));
            email.Subject = mailRequest.Subject;
            var builder = new BodyBuilder();
            if (mailRequest.Attachments != null)
            {
                byte[] fileBytes;
                foreach (var file in mailRequest.Attachments)
                {
                    if (file.Length > 0)
                    {
                        using (var ms = new MemoryStream())
                        {
                            file.CopyTo(ms);
                            fileBytes = ms.ToArray();
                        }
                        builder.Attachments.Add(file.FileName, fileBytes, ContentType.Parse(file.ContentType));
                    }
                }
            }
            builder.HtmlBody = mailRequest.Body;
            email.Body = builder.ToMessageBody();
            using var smtp = new SmtpClient();
            smtp.Connect(_mailClientService.Host, _mailClientService.Port, SecureSocketOptions.StartTls);
            smtp.Authenticate(_mailClientService.Email, _mailClientService.Password);
            await smtp.SendAsync(email);
            smtp.Disconnect(true);
        }
    }
}
