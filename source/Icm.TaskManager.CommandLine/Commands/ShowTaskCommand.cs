using System;
using Icm.TaskManager.Application;

namespace Icm.TaskManager.CommandLine.Commands
{
    internal class ShowTaskCommand : BaseCommand
    {
        private readonly ITaskApplicationService taskApplicationService;

        public ShowTaskCommand(ITaskApplicationService taskApplicationService)
        {
            this.taskApplicationService = taskApplicationService;
        }

        protected override bool Matches(string[] tokens)
        {
            return tokens[0].Equals("show", StringComparison.InvariantCultureIgnoreCase);
        }

        protected override void Process(IObserver<string> output, string[] tokens)
        {
            int id = int.Parse(tokens[1]);
            var dto = taskApplicationService.GetTaskById(id);
            output.ShowDetails(id, dto);
        }
    }
}