using System;

namespace Icm.TaskManager.CommandLine.Commands
{
    internal class UnknownCommand : BaseCommand
    {
        protected override bool Matches(string[] tokens)
        {
            return false;
        }

        protected override void Process(IObserver<string> output, string[] tokens)
        {
            output.OnNext("Unknown command");
        }
    }
}