using Quartz;

namespace pv311_web_api.Jobs
{
    public class ConsoleLogJob : IJob
    {
        public Task Execute(IJobExecutionContext context)
        {
            Console.WriteLine($"Log job work - {DateTime.Now}");
            return Task.CompletedTask;
        }
    }
}
