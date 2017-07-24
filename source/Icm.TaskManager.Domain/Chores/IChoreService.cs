using NodaTime;

namespace Icm.ChoreManager.Domain.Chores
{
    public interface IChoreService
    {
        Chore CreateChore(string description, Instant dueDate);

        Chore Finish(Chore chore);
    }
}
