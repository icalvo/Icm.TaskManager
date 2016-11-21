using System;
using System.Collections.Generic;

namespace Icm.TaskManager.CommandLine.Commands
{
    internal interface ICommand
    {
        bool Matches(IObserver<string> output, string line);
        IObservable<string> Process(string line);
    }
}