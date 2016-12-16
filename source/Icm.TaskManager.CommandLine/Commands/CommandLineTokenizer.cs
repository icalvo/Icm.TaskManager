using System;
using System.Collections.Generic;
using System.Linq;
using Icm.TaskManager.CommandLine.StateMachines;
using JetBrains.Annotations;

namespace Icm.TaskManager.CommandLine.Commands
{
    public static class CommandLineTokenizer
    {
        [NotNull]
        public static IEnumerable<string> Tokenize(string line)
        {
            if (string.IsNullOrEmpty(line))
            {
                return Enumerable.Empty<string>();
            }

            var stateMachineSpec = new[]
            {
                MealyItem('\\',              BackslashOutput,  BackslashTransition),
                MealyItem('"',               QuoteOutput,      QuoteTransition),
                MealyItem(char.IsWhiteSpace, WhitespaceOutput, WhitespaceTransition),
                MealyItem(Any<char>(),       NormalOutput,     NormalTransition),
            };

            return
                line.RunStateMachine(stateMachineSpec)
                //.Pipe(t => output.OnNext(
                //    $"[{t.Input}] => {t.State} & {t.Output.JoinStr(", ")}"))
                .SelectMany(x => x.Output)
                .GroupBy(result => result.TokenNumber)
                .Select(x => string.Concat(x.Select(y => y.Output)))
                .ToArray();
        }

        [NotNull]
        private static IEnumerable<CharResult> NormalOutput(State state, char ch)
        {
            if (state.LastCharWasEscape)
            {
                // Add pending escape char
                yield return new CharResult('\\', state.TokenNumber);
            }

            // Copy character from input, no special meaning
            yield return new CharResult(ch, state.TokenNumber);
        }

        private static State NormalTransition(State state)
        {
            return state.WithLastCharWasEscape(false);
        }

        [NotNull]
        private static IEnumerable<CharResult> WhitespaceOutput(State state, char ch)
        {
            if (state.InQuote)
            {
                foreach (var result in NormalOutput(state, ch))
                {
                    yield return result;
                }
            }
            else
            {
                if (state.LastCharWasEscape)
                {
                    yield return new CharResult('\\', state.TokenNumber);
                }
            }
        }

        [NotNull]
        private static State WhitespaceTransition(State state)
        {
            if (state.InQuote)
            {
                return state;
            }

            return state
                .WithLastCharWasEscape(false)
                .WithTokenNumberIf(true, state.TokenNumber + 1);
        }

        [NotNull]
        private static IEnumerable<CharResult> QuoteOutput(State state)
        {
            if (state.LastCharWasEscape)
            {
                // Backslash-escaped quote, keep it
                yield return new CharResult('"', state.TokenNumber);
            }
            else
            {
                if (state.InQuote && state.PrevChar == '"')
                {
                    // Doubled quote within a quoted range is like escaping
                    yield return new CharResult(' ', state.TokenNumber);
                }
            }
        }

        private static State QuoteTransition(State state)
        {
            return state
                .WithInQuote(state.LastCharWasEscape == state.InQuote)
                .WithLastCharWasEscape(false);
        }

        private static IEnumerable<CharResult> BackslashOutput(State state, char ch)
        {
            if (state.LastCharWasEscape)
            {
                yield return new CharResult(ch, state.TokenNumber);
            }
        }

        private static State BackslashTransition(State state)
        {
            return state.WithLastCharWasEscape(!state.LastCharWasEscape);
        }

        private static MealyItem<State, char, IEnumerable<CharResult>> MealyItem(
            char input,
            Func<State, IEnumerable<CharResult>> output,
            Func<State, State> trans)
        {
            return MealyItems.Create<State, char, IEnumerable<CharResult>>(
                input,
                (st, ch) => output(st),
                (st, ch) => trans(st));
        }

        private static MealyItem<State, char, IEnumerable<CharResult>> MealyItem(
            char input,
            Func<State, char, IEnumerable<CharResult>> output,
            Func<State, State> trans)
        {
            return MealyItems.Create(input, output, (st, ch) => trans(st));
        }

        private static MealyItem<State, char, IEnumerable<CharResult>> MealyItem(
            Func<char, bool> input,
            Func<State, char, IEnumerable<CharResult>> output,
            Func<State, State> trans)
        {
            return MealyItems.Create(input, output, (st, ch) => trans(st));
        }

        private static Func<T, bool> Any<T>()
        {
            return _ => true;
        }

        private class State
        {
            public readonly bool LastCharWasEscape;
            public readonly bool InQuote;
            public readonly char PrevChar;
            public readonly int TokenNumber;

            public State()
            {
                LastCharWasEscape = false;
                InQuote = false;
                PrevChar = '\0';
                TokenNumber = 1;
            }

            private State(bool lastCharWasEscape, bool inQuote, char prevChar, int tokenNumber)
            {
                LastCharWasEscape = lastCharWasEscape;
                InQuote = inQuote;
                PrevChar = prevChar;
                TokenNumber = tokenNumber;
            }

            public State WithLastCharWasEscape(bool value)
            {
                return new State(
                    value,
                    InQuote,
                    PrevChar,
                    TokenNumber);
            }

            public State WithInQuote(bool value)
            {
                return new State(
                    LastCharWasEscape,
                    value,
                    PrevChar,
                    TokenNumber);
            }

            public State WithTokenNumberIf(bool condition, int value)
            {
                return condition
                    ? new State(
                        LastCharWasEscape,
                        InQuote,
                        PrevChar,
                        value)
                    : this;
            }

            public override string ToString()
            {
                return
                    $"[{PrevChar}] {TokenNumber:000} {(LastCharWasEscape ? "ESC" : "---")} {(InQuote ? "QUO" : "---")}";
            }
        }

        private class CharResult
        {
            public char Output { get; }
            public int TokenNumber { get; }

            public CharResult(char output, int tokenNumber)
            {
                Output = output;
                TokenNumber = tokenNumber;
            }

            public override string ToString()
            {
                return $"Char: {Output} TokenNumber: {TokenNumber}";
            }
        }
    }
}