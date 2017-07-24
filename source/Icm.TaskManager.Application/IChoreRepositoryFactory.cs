using Icm.ChoreManager.Domain.Chores;

namespace Icm.ChoreManager.Application
{
    public interface IChoreRepositoryFactory
    {
        IChoreRepository Build();
    }
}