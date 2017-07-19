using System;

namespace Icm.TaskManager.Domain.Tasks
{
    public static class MonadicExtensions
    {
        public static TOut Match<TIn, TOut>(
            this TIn? obj,
            Func<TIn, TOut> func)
            where TIn : struct
        {
            return obj.HasValue ? func(obj.Value) : default(TOut);
        }

        public static TOut Match<TIn1, TIn2, TOut>(
            TIn1? obj1,
            TIn2? obj2,
            Func<TIn1, TIn2, TOut> func)
            where TIn1 : struct
            where TIn2 : struct
        {
            return obj1.HasValue && obj2.HasValue ? func(obj1.Value, obj2.Value) : default(TOut);
        }

        public static TOut Match<TIn1, TIn2, TOut>(
            TIn1? obj1,
            TIn2 obj2,
            Func<TIn1, TIn2, TOut> func)
            where TIn1 : struct
            where TIn2 : class
        {
            return obj1.HasValue && obj2 != null ? func(obj1.Value, obj2) : default(TOut);
        }

        public static TOut Match<TIn, TOut>(
            this TIn obj,
            Func<TIn, TOut> func)
            where TIn : class
        {
            return obj != null ? func(obj) : default(TOut);
        }

        public static TOut Match<TIn, TOut>(
            this TIn? obj,
            Func<TIn, TOut> func,
            Func<TOut> ifNothing)
            where TIn : struct
        {
            return obj.HasValue ? func(obj.Value) : ifNothing();
        }

        public static TOut Match<TIn, TOut>(
            this TIn obj,
            Func<TIn, TOut> func,
            Func<TOut> ifNothing)
            where TIn : class
        {
            return obj != null ? func(obj) : ifNothing();
        }

        public static TOut Match<TIn, TOut>(
            this TIn? obj,
            Func<TIn, TOut> func,
            TOut ifNothing)
            where TIn : struct
        {
            return obj.HasValue ? func(obj.Value) : ifNothing;
        }

        public static TOut Match<TIn, TOut>(
            this TIn obj,
            Func<TIn, TOut> func,
            TOut ifNothing)
            where TIn : class
        {
            return obj != null ? func(obj) : ifNothing;
        }
    }
}