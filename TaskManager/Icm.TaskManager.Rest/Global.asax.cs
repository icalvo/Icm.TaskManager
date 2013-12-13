using AutoMapper;
using Icm.TaskManager.Domain.Tasks;
using Icm.TaskManager.Web.Configuration.Routes;
using Icm.TaskManager.Web.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Routing;

namespace Icm.TaskManager.Rest
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
            Mapper.CreateMap<Task, TaskInfoDto>();
        }
    }
}
