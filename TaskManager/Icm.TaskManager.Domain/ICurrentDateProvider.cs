using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Icm.TaskManager.Domain
{
    public interface ICurrentDateProvider
    {
        DateTime Now { get; }
    }
}
