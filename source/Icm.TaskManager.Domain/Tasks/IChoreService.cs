using NodaTime;

namespace Icm.TaskManager.Domain.Tasks
{
    public interface IChoreService
    {
        Chore CreateTask(string description, Instant dueDate);

        Chore Finish(Chore chore);
    }
}
