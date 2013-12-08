using System.ComponentModel.DataAnnotations;

namespace Icm.TaskManager.Web.Models.Account.BindingModels
{
    public class RegisterExternalBindingModel
    {
        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }
    }

}