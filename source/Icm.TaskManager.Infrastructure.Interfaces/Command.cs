using System;

namespace Icm.TaskManager.Infrastructure.Interfaces
{
    public abstract class Command
    {
        public readonly Guid Id = Guid.NewGuid();
    }
}