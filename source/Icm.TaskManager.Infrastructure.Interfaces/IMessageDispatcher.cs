using System.Reflection;

namespace Icm.TaskManager.Infrastructure.Interfaces
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