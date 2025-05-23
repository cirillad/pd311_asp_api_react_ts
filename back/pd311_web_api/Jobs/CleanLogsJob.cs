using Quartz;

namespace pd311_web_api.Jobs
{
    public class CleanLogsJob : IJob
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        public CleanLogsJob(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        public Task Execute(IJobExecutionContext context)
        {
            string logsPath = Path.Combine(_webHostEnvironment.ContentRootPath, "logs");
            var logs = Directory.GetFiles(logsPath);

            foreach (var log in logs)
            {
                var file = new FileInfo(log);
                var days = (DateTime.Now - file.CreationTime).Days;

                if(days >= 7)
                {
                    File.Delete(file.FullName);
                }
            }

            return Task.CompletedTask;
        }
    }
}
