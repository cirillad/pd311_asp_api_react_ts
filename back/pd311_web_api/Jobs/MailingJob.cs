using pd311_web_api.BLL.Services.Email;
using Quartz;

namespace pd311_web_api.Jobs
{
    public class MailingJob : IJob
    {
        private readonly IEmailService _emailService;

        public MailingJob(IEmailService emailService)
        {
            _emailService = emailService;
        }

        public Task Execute(IJobExecutionContext context)
        {
            return _emailService.SendMailAsync("dmytro.potapchuk22@gmail.com", "quartz", "Mailing job");
        }
    }
}
