using Acacia_Back_End.Core.ViewModels;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using MimeKit.Text;

namespace Acacia_Back_End.Infrastructure.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _config;

        public EmailService(IConfiguration config)
        {
            _config = config;
        }
        public void SendEmail(EmailVM request)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(_config["MailSettings:Mail"]));
            email.To.Add(MailboxAddress.Parse(request.To));
            email.Subject = request.Subject;
            email.Body = new TextPart(TextFormat.Html) { Text = request.Body };


            using var smtp = new SmtpClient();
            smtp.Connect(_config["MailSettings:Host"], int.Parse(_config["MailSettings:Port"]), SecureSocketOptions.StartTls);
            smtp.Authenticate(_config["MailSettings:Mail"], _config["MailSettings:Password"]);
            smtp.Send(email);
            smtp.Disconnect(true);
        }
    }
}
