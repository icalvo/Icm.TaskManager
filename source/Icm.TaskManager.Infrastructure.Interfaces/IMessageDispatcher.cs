using System.Reflection;
using Icm.TaskManager.Domain.Tasks;

namespace Edument.CQRS
{
    public interface IMessageDispatcher
    {
        void AddHandlerFor<TCommand, TAggregate>()
            where TAggregate : Aggregate, new()
            where TCommand : Command;

        void AddSubscriberFor<TEvent>(ISubscribeTo<TEvent> subscriber);

        void ScanAssembly(Assembly ass);

        void ScanInstance(object instance);

        void SendCommand<TCommand>(TCommand c);
    }
}