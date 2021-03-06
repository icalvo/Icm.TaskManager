﻿using System;
using System.Collections.Generic;
using Icm.TaskManager.Domain.Tasks;
using NodaTime;

namespace Icm.TaskManager.Domain.Tests.Fakes
{
    public class FakeTaskRepository : MemoryRepoKeyRepository<TaskId, Task>, ITaskRepository
    {
        public FakeTaskRepository()
            : base(lastKey => new TaskId(lastKey + 1))
        {
        }

        public IEnumerable<(Instant Time, TimeKind Kind)> GetActiveReminders()
        {
            throw new NotImplementedException();
        }
    }
}
