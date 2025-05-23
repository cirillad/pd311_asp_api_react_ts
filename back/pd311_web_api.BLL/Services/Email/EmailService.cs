using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Mail;

namespace pd311_web_api.BLL.Services.Email
{
    public class EmailService : IEmailService
    {
        private readonly string email;
        private readonly string password;
        private readonly string host;
        private readonly int port;
        private readonly SmtpClient _smtpClient;

        public EmailService(IConfiguration configuration)
        {
            email = configuration["SmtpSettings:Email"] ?? "";
            password = configuration["SmtpSettings:Password"] ?? "";
            host = configuration["SmtpSettings:Host"] ?? "";
            port = int.Parse(configuration["SmtpSettings:Port"] ?? "0");

            _smtpClient = new SmtpClient(host, port);
            _smtpClient.EnableSsl = true;
            _smtpClient.Credentials = new NetworkCredential(email, password);
        }

        public async Task SendMailAsync(string to, string subject, string body, bool isHtml = false)
        {
            var message = new MailMessage();
            message.From = new MailAddress(email);
            message.To.Add(to);
            message.Subject = subject;
            message.Body = body;
            message.IsBodyHtml = isHtml;
            await SendMailAsync(message);
        }

        public async Task SendMailAsync(MailMessage message)
        {
            await _smtpClient.SendMailAsync(message);
        }
    }
}
