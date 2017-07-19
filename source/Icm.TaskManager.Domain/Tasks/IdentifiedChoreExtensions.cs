namespace Icm.TaskManager.Domain.Tasks
{
    public static class IdentifiedChoreExtensions
    {
        public static ChoreMemento ToMemento(this Identified<ChoreId, Chore> identifiedChore)
        {
            return identifiedChore.Value.ToMemento(identifiedChore.Id);
        }
    }
}