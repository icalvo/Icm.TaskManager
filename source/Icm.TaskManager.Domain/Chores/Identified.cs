namespace Icm.TaskManager.Domain.Chores
{
    public class Identified<TKey, TValue>
    {
        public Identified(TKey id, TValue value)
        {
            Id = id;
            Value = value;
        }

        public TKey Id { get; }

        public TValue Value { get; }
    }

    namespace Icm.TaskManager.Domain.Tasks
    {
        public static class Identified
        {
            public static Identified<TKey, TValue> Create<TKey, TValue>(TKey key, TValue value)
            {
                return new Identified<TKey, TValue>(key, value);
            }
        }
    }
}