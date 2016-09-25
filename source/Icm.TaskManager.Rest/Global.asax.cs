using AutoMapper;
using Icm.TaskManager.Domain.Tasks;
using Icm.TaskManager.Rest.Configuration.Routes;
using Icm.TaskManager.Rest.DTOs;
using System.Web;
using System.Web.Http;

namespace Icm.TaskManager.Rest
{
    public class WebApiApplication : HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
            Mapper.CreateMap<Task, TaskInfoDto>();
        }
    }
}
