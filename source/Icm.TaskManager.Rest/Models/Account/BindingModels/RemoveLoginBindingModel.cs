using System.ComponentModel.DataAnnotations;

namespace Icm.TaskManager.Rest.Models.Account.BindingModels
{
    /// <summary>
    /// Binding model for removing a login
    /// </summary>
    public class RemoveLoginBindingModel
    {
        [Required]
        [Display(Name = "Login provider")]
        public string LoginProvider { get; set; }

        [Required]
        [Display(Name = "Provider key")]
        public string ProviderKey { get; set; }
    }
}