using System;

namespace Icm.ChoreManager.Domain.Chores
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

        public static bool IsValid(string arg)
        {
            return int.TryParse(arg, out int _);
        }

        public static ChoreId Parse(string arg)
        {
            return int.Parse(arg);
        }

        public override bool Equals(object obj)
        {
            return obj is ChoreId && Equals((ChoreId) obj);
        }

        private bool Equals(ChoreId other)
        {
            return Value == other.Value;
        }

        public override int GetHashCode()
        {
            return Value;
        }
    }
}