namespace Icm.TaskManager.Domain.Chores
{
    public static class IdentifiedChoreExtensions
    {
        public static ChoreMemento ToMemento(this Identified<ChoreId, Chore> identifiedChore)
        {
            return identifiedChore.Value.ToMemento(identifiedChore.Id);
        }
    }
}