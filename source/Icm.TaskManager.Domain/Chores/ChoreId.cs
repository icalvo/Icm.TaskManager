using System;

namespace Icm.ChoreManager.Domain.Chores
{
    public struct ChoreId
    {
        public Guid Value { get; }

        public ChoreId(Guid id)
        {
            Value = id;
        }

        public static implicit operator Guid(ChoreId id)
        {
            return id.Value;
        }

        public static implicit operator ChoreId(Guid id)
        {
            return new ChoreId(id);
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}