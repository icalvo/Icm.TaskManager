using System.Collections.Generic;

namespace Icm.TaskManager.Web.Models.Account.ViewModels
{
    /// <summary>
    /// View model for user profile information
    /// </summary>
    public class ManageInfoViewModel
    {
        public string LocalLoginProvider { get; set; }

        public string UserName { get; set; }

        public IEnumerable<UserLoginInfoViewModel> Logins { get; set; }

        public IEnumerable<ExternalLoginViewModel> ExternalLoginProviders { get; set; }
    }
}