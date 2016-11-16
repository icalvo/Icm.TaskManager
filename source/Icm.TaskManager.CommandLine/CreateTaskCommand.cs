using System;
using Icm.TaskManager.Application;
using NodaTime;
using NodaTime.Text;

namespace Icm.TaskManager.CommandLine
{
    public class CreateTaskCommand : ICommand
    {
        private readonly ITaskApplicationService taskApplicationService;

        public CreateTaskCommand(ITaskApplicationService taskApplicationService)
        {
            this.taskApplicationService = taskApplicationService;
        }

        public bool Matches(string line)
        {
            return line.StartsWith("create");
        }

        public void Process(IObserver<string> observer, string line)
        {
            observer.OnNext("Creating task...");
            var tokens = line.Split(new []{ ' ' }, StringSplitOptions.RemoveEmptyEntries);
            var dueDate = ZonedDateTimePattern.CreateWithInvariantCulture("yyyy-MM-dd", DateTimeZoneProviders.Tzdb).Parse(tokens[1]).Value.ToInstant();
            var description = tokens[1];
            var id = taskApplicationService.CreateSimpleTask(description, dueDate);
            observer.OnNext($"Task {id} created");
        }
    }
}