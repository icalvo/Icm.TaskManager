using System;

namespace Icm.TaskManager.CommandLine.StateMachines
{
    public static class MealyItems
    {
        public static MealyItem<TState, TInput, TOutput> Create<TState, TInput, TOutput>(Func<TInput, bool> inputPredicate, Func<TState, TInput, TOutput> outputFunction, Func<TState, TInput, TState> transitionFunction)
        {
            return new MealyItem<TState, TInput, TOutput>(
                inputPredicate,
                outputFunction,
                transitionFunction);
        }

        public static MealyItem<TState, TInput, TOutput> Create<TState, TInput, TOutput>(TInput input, Func<TState, TInput, TOutput> outputFunction, Func<TState, TInput, TState> transitionFunction)
        {
            return new MealyItem<TState, TInput, TOutput>(
                i => i.Equals(input),
                outputFunction,
                transitionFunction);
        }
    }
}