using Quartz;

namespace pd311_web_api.Jobs
{
    public class HelloJob : IJob
    {
        public Task Execute(IJobExecutionContext context)
        {
            Console.WriteLine($"Hello, it's first job - {DateTime.Now}");
            return Task.CompletedTask;
        }
    }
}
