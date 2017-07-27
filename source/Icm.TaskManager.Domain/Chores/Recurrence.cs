using NodaTime;

namespace Icm.ChoreManager.Domain.Chores
{
    public abstract class Recurrence
    {
        protected Recurrence(Duration repeatInterval)
        {
            RepeatInterval = repeatInterval;
        }

        public Duration RepeatInterval { get; }

        public abstract RecurrenceKind Kind { get; }

        public abstract Chore CreateRecurringChore(Chore chore, Instant now);

        public static Recurrence FromMemento(RecurrenceMemento memento)
        {
            switch (memento.Kind)
            {
                case RecurrenceKind.DueDate:
                    return new DueDateRecurrence(memento.Interval);
                case RecurrenceKind.FinishDate:
                    return new FinishDateRecurrence(memento.Interval);
                default:
                    return null;
            }
        }
    }

    public enum RecurrenceKind
    {
        DueDate,
        FinishDate
    }
}
