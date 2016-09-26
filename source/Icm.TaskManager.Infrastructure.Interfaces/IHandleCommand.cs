using System.Collections;
using System.Collections.Generic;
using Icm.TaskManager.Domain.Tasks;

namespace Edument.CQRS
{
    public interface IHandleCommand<in TCommand>
        where TCommand : Command
    {
        IEnumerable<object> Handle(TCommand c);
    }
}
