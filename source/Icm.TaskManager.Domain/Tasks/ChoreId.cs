namespace Icm.TaskManager.Domain.Tasks
{
    public struct ChoreId
    {
        public int Value { get; }

        public ChoreId(int id)
        {
            Value = id;
        }

        public static implicit operator int(ChoreId id)
        {
            return id.Value;
        }

        public static implicit operator ChoreId(int id)
        {
            return new ChoreId(id);
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}