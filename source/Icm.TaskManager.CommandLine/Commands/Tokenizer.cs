using System;
using System.Collections.Generic;
using System.Linq;

namespace Icm.TaskManager.CommandLine.Commands
{
    public static class Tokenizer
    {
        public static IEnumerable<string> Tokenize(string line)
        {
            return
                TokenizeAux(line)
                .GroupBy(result => result.TokenNumber)
                .Select(x => string.Concat(x.Select(y => y.Output)))
                .ToArray();
        }

        private static IEnumerable<CharResult> TokenizeAux(string line)
        {
            return line
                .StateMachine(
                new State(),
                (state, ch) =>
                {
                    switch (ch)
                    {
                        case '\\':
                            return BackslashOutput(state, ch);
                        case '"':
                            return QuoteOutput(state);
                        default:
                            return char.IsWhiteSpace(ch)
                                ? WhitespaceOutput(state, ch)
                                : NormalOutput(state, ch);
                    }
                },
                (state, ch) =>
                {
                    State result;
                    switch (ch)
                    {
                        case '\\':
                            result = BackslashTransition(state);
                            break;
                        case '"':
                            result = QuoteTransition(state);
                            break;
                        default:
                            result = char.IsWhiteSpace(ch)
                                ? WhitespaceTransition(state)
                                : NormalTransition(state);
                            break;
                    }

                    return result.WithPrevChar(ch);
                })
                .Pipe(t => Console.WriteLine(
                    $"[{t.Item1}] => {t.Item2} & {string.Join(", ", t.Item3.Select(x => x.ToString()))}"))
                .SelectMany(x => x.Item3);
        }

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

        private static State WhitespaceTransition(State state)
        {
            if (state.InQuote)
            {
                return state;
            }

            return state
                .WithLastCharWasEscape(false)
                .WithHadQuote(false)
                .WithTokenNumberIf(true, state.TokenNumber + 1);
        }

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

        private class State
        {
            public readonly bool LastCharWasEscape;
            public readonly bool InQuote;
            public readonly bool HadQuote;
            public readonly char PrevChar;
            public readonly int TokenNumber;

            public State()
            {
                LastCharWasEscape = false;
                InQuote = false;
                HadQuote = false;
                PrevChar = '\0';
                TokenNumber = 1;
            }

            private State(bool lastCharWasEscape, bool inQuote, bool hadQuote, char prevChar, int tokenNumber)
            {
                LastCharWasEscape = lastCharWasEscape;
                InQuote = inQuote;
                HadQuote = hadQuote;
                PrevChar = prevChar;
                TokenNumber = tokenNumber;
            }

            public State WithLastCharWasEscape(bool value)
            {
                return new State(
                    value,
                    InQuote,
                    HadQuote,
                    PrevChar,
                    TokenNumber);
            }

            public State WithInQuote(bool value)
            {
                return new State(
                    LastCharWasEscape,
                    value,
                    HadQuote,
                    PrevChar,
                    TokenNumber);
            }


            public State WithHadQuote(bool value)
            {
                return new State(
                    LastCharWasEscape,
                    InQuote,
                    value,
                    PrevChar,
                    TokenNumber);
            }


            public State WithPrevChar(char value)
            {
                return new State(
                    LastCharWasEscape,
                    InQuote,
                    HadQuote,
                    value,
                    TokenNumber);
            }


            public State WithTokenNumberIf(bool condition, int value)
            {
                return condition
                    ? new State(
                        LastCharWasEscape,
                        InQuote,
                        HadQuote,
                        PrevChar,
                        value)
                    : this;
            }

            public override string ToString()
            {
                return
                    $"[{PrevChar}] {TokenNumber:000} {(LastCharWasEscape ? "ESC" : "---")} {(InQuote ? "QUO" : "---")} {(HadQuote ? "HQU" : "---")}";
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