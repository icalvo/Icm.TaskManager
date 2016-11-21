using System;
using System.Reactive.Linq;

namespace Icm.TaskManager.CommandLine.Commands
{
    internal class QuitCommand : ICommand
    {
        public bool Matches(IObserver<string> output, string line)
        {
            return line.StartsWith("quit");
        }

        public IObservable<string> Process(string line)
        {
            return Observable.Empty<string>();
        }
    }
}