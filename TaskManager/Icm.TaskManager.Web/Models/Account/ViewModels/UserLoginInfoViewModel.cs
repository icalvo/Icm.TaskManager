using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Icm.TaskManager.Web.Models.Account.ViewModels
{
    /// <summary>
    /// View model for one login information
    /// </summary>
    public class UserLoginInfoViewModel
    {
        public string LoginProvider { get; set; }

        public string ProviderKey { get; set; }
    }
}