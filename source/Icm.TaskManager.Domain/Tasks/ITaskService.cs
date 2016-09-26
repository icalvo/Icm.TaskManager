using NodaTime;

namespace Icm.TaskManager.Domain.Tasks
{
    public interface ITaskService
    {
        Task CreateTask(string description, Instant dueDate);

        Task Finish(Task task);
    }
}
