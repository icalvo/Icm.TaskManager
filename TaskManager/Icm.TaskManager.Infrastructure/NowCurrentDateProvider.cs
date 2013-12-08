using Icm.TaskManager.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icm.TaskManager.Infrastructure
{
    public class NowCurrentDateProvider : ICurrentDateProvider
    {
        public DateTime Now
        {
            get 
            {
                return DateTime.Now;
            }
        }
    }
}
