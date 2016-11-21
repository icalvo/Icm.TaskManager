using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reactive.Subjects;
using Icm.TaskManager.Application;
using NodaTime;
using NodaTime.Text;

namespace Icm.TaskManager.CommandLine.Commands
{
    internal class CreateTaskCommand : BaseCommand
    {
        private readonly ITaskApplicationService taskApplicationService;

        public CreateTaskCommand(ITaskApplicationService taskApplicationService)
        {
            this.taskApplicationService = taskApplicationService;
        }

        protected override bool Matches(string[] tokens)
        {
            return tokens[0].Equals("create", StringComparison.InvariantCultureIgnoreCase);
        }


        protected override IEnumerable<string> Validates(string[] tokens)
        {
            DateTime d;
            if (tokens.Length != 3)
            {
                yield return "I need 2 arguments (due date and description) to create a task";
            }

            if (!DateTime.TryParseExact(tokens[1], "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out d))
            {
                yield return "Date is not valid";
            }
        }

        protected override void Process(IObserver<string> output, string[] tokens)
        {
            output.OnNext("Creating task...");

            var dueDate = ZonedDateTimePattern.CreateWithInvariantCulture("yyyy-MM-dd", DateTimeZoneProviders.Tzdb).Parse(tokens[1]).Value.ToInstant();
            var description = tokens[1];
            var id = taskApplicationService.CreateTask(description, dueDate);
            output.OnNext($"Task {id} created");
            var dto = taskApplicationService.GetTaskById(id);
            output.ShowDetails(id, dto);
        }
    }
}