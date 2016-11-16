using System;
using System.Reactive.Linq;

namespace Icm.TaskManager.CommandLine
{
    internal class QuitCommand : ICommand
    {
        public bool Matches(string line)
        {
            return line.StartsWith("quit");
        }

        public void Process(IObserver<string> observer, string line)
        {
        }
    }
}