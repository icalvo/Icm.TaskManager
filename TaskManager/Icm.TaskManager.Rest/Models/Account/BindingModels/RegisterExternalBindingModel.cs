using System.ComponentModel.DataAnnotations;

namespace Icm.TaskManager.Rest.Models.Account.BindingModels
{
    public class RegisterExternalBindingModel
    {
        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }
    }

}