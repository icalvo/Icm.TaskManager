using System.Collections.Generic;
using Icm.TaskManager.Infrastructure.Interfaces;
using NodaTime;

namespace Icm.TaskManager.Domain.Tasks
{
    public class TaskAggregate : Aggregate,
        IHandleCommand<CreateTask>,
        IHandleCommand<FinishTask>,
        IHandleCommand<ChangeTaskNotes>
    {
        private IClock clock;

        IEnumerable<object> IHandleCommand<CreateTask>.Handle(CreateTask c)
        {
            yield return new TaskCreatedEvent(c.Id, c.Description, c.DueDate, clock.Now);
        }

        IEnumerable<object> IHandleCommand<FinishTask>.Handle(FinishTask c)
        {
            throw new System.NotImplementedException();
        }

        IEnumerable<object> IHandleCommand<ChangeTaskNotes>.Handle(ChangeTaskNotes c)
        {
            throw new System.NotImplementedException();
        }
    }
}