using Icm.TaskManager.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NodaTime;

namespace Icm.TaskManager.Infrastructure
{
    public class NowCurrentDateProvider : ICurrentDateProvider
    {
        public Instant Now
        {
            get 
            {
                return SystemClock.Instance.Now;
            }
        }
    }
}
