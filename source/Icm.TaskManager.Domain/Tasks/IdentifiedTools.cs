namespace Icm.TaskManager.Domain.Tasks
{
    public static class IdentifiedTools
    {
        public static Identified<TKey, TValue> Identified<TKey, TValue>(TKey key, TValue value)
        {
            return new Identified<TKey, TValue>(key, value);
        }
    }
}