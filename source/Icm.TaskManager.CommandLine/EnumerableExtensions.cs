using System;
using System.Collections.Generic;

namespace Icm.TaskManager.CommandLine
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<T> Pipe<T>(this IEnumerable<T> source, Action<T> action)
        {
            foreach (var item in source)
            {
                action(item);
                yield return item;
            }
        }

        public static void Execute<T>(this IEnumerable<T> source)
        {
            foreach (var item in source)
            {
            }
        }

        public static void Execute<T>(this IEnumerable<T> source, Action<T> action)
        {
            foreach (var item in source)
            {
                action(item);
            }
        }

        public static IEnumerable<TOutput> Scan<TInput, TOutput>(
            this IEnumerable<TInput> source,
            TOutput seed,
            Func<TOutput, TInput, TOutput> aggregation)
        {
            var current = seed;
            foreach (var input in source)
            {
                current = aggregation(current, input);
                yield return current;
            }
        }


        public static IEnumerable<Tuple<TInput, TState, TOutput>> StateMachine<TInput, TState, TOutput>(
            this IEnumerable<TInput> source,
            TState initial,
            Func<TState, TInput, Tuple<TOutput, TState>> transition)
        {
            var state = initial;
            foreach (var input in source)
            {
                var result = transition(state, input);
                var output = result.Item1;
                state = result.Item2;
                yield return Tuple.Create(input, state, output);
            }
        }

        public static IEnumerable<Tuple<TInput, TState, TOutput>> StateMachine<TInput, TState, TOutput>(
            this IEnumerable<TInput> source,
            TState initial,
            Func<TState, TInput, TOutput> outputfunc,
            Func<TState, TInput, TState> transition)
        {
            return source.StateMachine(
                initial,
                (state, input) => Tuple.Create(
                    outputfunc(state, input),
                    transition(state, input)));
        }
    }
}