using System;
using System.Threading.Tasks;
using Icm.TaskManager.Application;
using NodaTime;

namespace Icm.TaskManager.CommandLine
{
    public interface IChoreApplicationServiceSchedulingAdapter : IChoreApplicationService
    {
        void Complete();
        Task ScheduleExistingAsync();
    }
}