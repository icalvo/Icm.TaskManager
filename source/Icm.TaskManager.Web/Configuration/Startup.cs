﻿using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(Icm.TaskManager.Web.Configuration.Startup))]

namespace Icm.TaskManager.Web.Configuration
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
        }
    }
}
