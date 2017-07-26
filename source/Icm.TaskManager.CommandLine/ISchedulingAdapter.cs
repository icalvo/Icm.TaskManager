using System.Threading.Tasks;
using Icm.ChoreManager.Application;

namespace Icm.ChoreManager.CommandLine
{
    public interface IChoreApplicationServiceSchedulingAdapter : IChoreApplicationService
    {
        void Complete();
        Task ScheduleExistingAsync();
    }
}