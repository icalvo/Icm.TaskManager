using System.ComponentModel.DataAnnotations;

namespace Icm.TaskManager.Web.Models.Account.BindingModels
{
    /// <summary>
    /// Binding model for adding external login
    /// </summary>
    public class AddExternalLoginBindingModel
    {
        [Required]
        [Display(Name = "External access token")]
        public string ExternalAccessToken { get; set; }
    }
}