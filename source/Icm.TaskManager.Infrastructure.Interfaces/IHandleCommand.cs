using System.Collections.Generic;

namespace Icm.TaskManager.Infrastructure.Interfaces
{
    public interface IHandleCommand<in TCommand>
        where TCommand : Command
    {
        IEnumerable<object> Handle(TCommand c);
    }
}
