namespace Icm.TaskManager.Rest.Models.Account.ViewModels
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