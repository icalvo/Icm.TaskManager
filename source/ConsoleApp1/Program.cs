using System;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace ConsoleApp1
{


    class Program
    {
        static void Main(string[] args)
        {
            var bufferBlock = new BroadcastBlock<long>(i => i);

            var parent1 = new TransformBlock<long, string>(i => "A" + i);
            var parent2 = new TransformBlock<long, string>(i => "B" + i);

            var subject = new TransformBlock<string, string>(s => "PROC " + s);


            var child1 = new ActionBlock<string>(s => Console.WriteLine("1: " + s));
            var child2 = new ActionBlock<string>(s => Console.WriteLine("2: " + s));


            bufferBlock.LinkTo(parent1, new DataflowLinkOptions {PropagateCompletion = true});
            bufferBlock.LinkTo(parent2, new DataflowLinkOptions { PropagateCompletion = true });
            parent1.LinkTo(subject, new DataflowLinkOptions { PropagateCompletion = true });
            parent2.LinkTo(subject, new DataflowLinkOptions { PropagateCompletion = true });
            subject.LinkTo(child1, new DataflowLinkOptions { PropagateCompletion = true });
            subject.LinkTo(child2, new DataflowLinkOptions { PropagateCompletion = true });

            Observable.Interval(TimeSpan.FromSeconds(1))
                .Take(20)
                .Subscribe(o =>
                {
                    bufferBlock.Post(o);
                },
                () => bufferBlock.Complete());

            Task.WhenAll(child1.Completion, child2.Completion).Wait();

            Console.WriteLine("FINISH");
            Console.ReadLine();
        }
    }
}