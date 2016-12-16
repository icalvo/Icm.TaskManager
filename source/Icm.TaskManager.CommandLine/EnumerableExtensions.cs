using System;
using System.Collections.Generic;
using System.Linq;

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

        public static string JoinStr<T>(this IEnumerable<T> source, string separator)
        {
            return string.Join(separator, source.Select(x => x.ToString()));
        }

        public static string JoinStr<T>(this IEnumerable<T> source, string separator, Func<T, string> toString)
        {
            return string.Join(separator, source.Select(toString));
        }

        public static bool VerbIs(this string[] tokens, string verb)
        {
            return string.Equals(verb, tokens.ElementAtOrDefault(0), StringComparison.InvariantCultureIgnoreCase);
        }

    }
}