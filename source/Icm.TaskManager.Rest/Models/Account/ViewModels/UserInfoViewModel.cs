namespace Icm.TaskManager.Web.Models.Account.ViewModels
{
    /// <summary>
    /// View model for showing user information
    /// </summary>
    public class UserInfoViewModel
    {
        public string UserName { get; set; }

        public bool HasRegistered { get; set; }

        public string LoginProvider { get; set; }
    }
}