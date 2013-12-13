using Microsoft.Web.Infrastructure.DynamicModuleHelper;
using Ninject.Web;

[assembly: WebActivator.PreApplicationStartMethod(typeof(Icm.TaskManager.Web.Configuration.NinjectWeb), "Start")]

namespace Icm.TaskManager.Web.Configuration
{

    public static class NinjectWeb 
    {
        /// <summary>
        /// Starts the application
        /// </summary>
        public static void Start() 
        {
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
        }
    }
}
