using System;
using System.Collections.Generic;
using System.Linq;

namespace Icm.TaskManager.CommandLine.StateMachines
{
    public static class StateMachineExtensions
    {
        /// <summary>
        /// Makes a single transition (single input).
        /// </summary>
        /// <typeparam name="TInput"></typeparam>
        /// <typeparam name="TState"></typeparam>
        /// <typeparam name="TOutput"></typeparam>
        /// <param name="input"></param>
        /// <param name="state"></param>
        /// <param name="coalesced"></param>
        /// <returns></returns>
        public static StateMachineResult<TInput, TState, TOutput> Mealy<TInput, TState, TOutput>(
            this TInput input,
            TState state,
            Func<TState, TInput, (TState, TOutput)> coalesced)
        {
            var result = coalesced(state, input);
            var output = result.Item2;
            state = result.Item1;
            return new StateMachineResult<TInput, TState, TOutput>(input, state, output);
        }

        /// <summary>
        /// Makes a single transition (single input).
        /// </summary>
        /// <typeparam name="TInput"></typeparam>
        /// <typeparam name="TState"></typeparam>
        /// <typeparam name="TOutput"></typeparam>
        /// <param name="input"></param>
        /// <param name="state"></param>
        /// <param name="outputfunc"></param>
        /// <param name="transitionfunc"></param>
        /// <returns></returns>
        public static StateMachineResult<TInput, TState, TOutput> Mealy<TInput, TState, TOutput>(
            this TInput input,
            TState state,
            Func<TState, TInput, TOutput> outputfunc,
            Func<TState, TInput, TState> transitionfunc)
        {
            var output = outputfunc(state, input);
            state = transitionfunc(state, input);
            return new StateMachineResult<TInput, TState, TOutput>(input, state, output);
        }

        /// <summary>
        /// Makes all transitions for the given inputs, starting with a provided state.
        /// </summary>
        /// <typeparam name="TInput"></typeparam>
        /// <typeparam name="TState"></typeparam>
        /// <typeparam name="TOutput"></typeparam>
        /// <param name="source"></param>
        /// <param name="initial"></param>
        /// <param name="coalesced"></param>
        /// <returns></returns>
        public static IEnumerable<StateMachineResult<TInput, TState, TOutput>> RunStateMachine<TInput, TState, TOutput>(
            this IEnumerable<TInput> source,
            TState initial,
            Func<TState, TInput, (TState, TOutput)> coalesced)
        {
            return source.Scan(
                new StateMachineResult<TInput, TState, TOutput>(default(TInput), initial, default(TOutput)),
                (smr, i) => Mealy(i, smr.State, coalesced));
        }

        public static IEnumerable<StateMachineResult<TInput, TState, TOutput>> RunStateMachine<TInput, TState, TOutput>(
            this IEnumerable<TInput> source,
            TState initial,
            Func<TState, TInput, TOutput> outputfunc,
            Func<TState, TInput, TState> transitionfunc)
        {
            return source.Scan(
                new StateMachineResult<TInput, TState, TOutput>(default(TInput), initial, default(TOutput)),
                (smr, i) => Mealy(i, smr.State, outputfunc, transitionfunc));
        }

        public static IEnumerable<StateMachineResult<TInput, TState, TOutput>> RunStateMachine<TInput, TState, TOutput>(
            this IEnumerable<TInput> source,
            TState initial,
            IEnumerable<MealyItem<TState, TInput, TOutput>> spec)
        {
            return source.RunStateMachine(
                initial,
                (state, input) => spec.First(x => x.InputPredicate(input)).OutputFunction(state, input),
                (state, input) => spec.First(x => x.InputPredicate(input)).TransitionFunction(state, input));
        }

        public static IEnumerable<StateMachineResult<TInput, TState, TOutput>> RunStateMachine<TInput, TState, TOutput>(
            this IEnumerable<TInput> source,
            IEnumerable<MealyItem<TState, TInput, TOutput>> spec)
            where TState : new()
        {
            return source.RunStateMachine(
                new TState(),
                (state, input) => spec.First(x => x.InputPredicate(input)).OutputFunction(state, input),
                (state, input) => spec.First(x => x.InputPredicate(input)).TransitionFunction(state, input));
        }
    }
}