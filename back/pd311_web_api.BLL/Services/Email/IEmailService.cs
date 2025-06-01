using System.Net.Mail;

namespace pd311_web_api.BLL.Services.Email
{
    public interface IEmailService
    {
        Task SendMailAsync(string to, string subject, string body, bool isHtml = false);
        Task SendMailAsync(MailMessage message);
    }
}
