namespace Icm.TaskManager.Domain.Tasks
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
}