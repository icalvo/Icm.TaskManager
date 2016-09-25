namespace Icm.TaskManager.Infrastructure.Interfaces
{
    public interface IEventHandler<in TEvent>
    {
        System.Threading.Tasks.Task HandleAsync(TEvent ev);
    }
}