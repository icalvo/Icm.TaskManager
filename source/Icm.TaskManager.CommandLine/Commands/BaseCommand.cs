using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading.Tasks;

namespace Icm.TaskManager.CommandLine.Commands
{
    internal abstract class BaseCommand : ICommand
    {
        private string[] tokens;

        public bool Matches(IObserver<string> output, string line)
        {
            tokens = CommandLineTokenizer.Tokenize(line, output).ToArray();
            return Matches(tokens);
        }

        protected abstract bool Matches(string[] tokens);

        protected virtual IEnumerable<string> Validates(string[] tokens)
        {
            return Enumerable.Empty<string>();
        }

        public IObservable<string> Process(string line)
        {
            return Observable.Create<string>(observer =>
                    TaskPoolScheduler.Default.Schedule(() =>
                    {
                        var validation = Validates(tokens).ToArray();
                        if (validation.Any())
                        {
                            foreach (var s in validation)
                            {
                                observer.OnNext(s);
                            }
                        }
                        Process(observer, tokens);
                        observer.OnCompleted();
                    }));
        }

        protected abstract void Process(IObserver<string> output, string[] tokens);
    }
}