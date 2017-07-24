using System;

namespace Icm.ChoreManager.CommandLine.StateMachines
{
    public class MealyItem<TState, TInput, TOutput>
    {
        public MealyItem(Func<TInput, bool> inputPredicate, Func<TState, TInput, TOutput> outputFunction, Func<TState, TInput, TState> transitionFunction)
        {
            InputPredicate = inputPredicate;
            OutputFunction = outputFunction;
            TransitionFunction = transitionFunction;
        }

        public Func<TInput, bool> InputPredicate { get; }
        public Func<TState, TInput, TOutput> OutputFunction { get; }
        public Func<TState, TInput, TState> TransitionFunction { get; }
    }
}