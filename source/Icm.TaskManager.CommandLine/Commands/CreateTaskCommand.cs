using System;
using Icm.TaskManager.Application;
using NodaTime;
using NodaTime.Text;

namespace Icm.TaskManager.CommandLine.Commands
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

        public void Process(IObserver<string> output, string line)
        {
            output.OnNext("Creating task...");
            var tokens = line.Split(new []{ ' ' }, StringSplitOptions.RemoveEmptyEntries);
            var dueDate = ZonedDateTimePattern.CreateWithInvariantCulture("yyyy-MM-dd", DateTimeZoneProviders.Tzdb).Parse(tokens[1]).Value.ToInstant();
            var description = tokens[1];
            var id = taskApplicationService.CreateTask(description, dueDate);
            output.OnNext($"Task {id} created");
            var dto = taskApplicationService.GetTaskById(id);

            output.OnNext($"  Description: {dto.Description}");
            output.OnNext($"  Due date: {dto.DueDate}");
        }
    }
}