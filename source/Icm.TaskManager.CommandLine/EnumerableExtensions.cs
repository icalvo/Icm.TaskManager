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
    }
}