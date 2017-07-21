using NodaTime;

namespace Icm.TaskManager.Domain.Chores
{
    public interface IChoreService
    {
        Chore CreateTask(string description, Instant dueDate);

        Chore Finish(Chore chore);
    }
}
