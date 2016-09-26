using System;
using System.Collections.Generic;
using System.Web;
using Icm.TaskManager.Domain;
using Icm.TaskManager.Domain.Tasks;
using Icm.TaskManager.Infrastructure.Interfaces;
using Icm.TaskManager.Rest.Configuration;
using Microsoft.Web.Infrastructure.DynamicModuleHelper;
using Ninject;
using Ninject.Web.Common;
using NodaTime;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(NinjectWebCommon), "Stop")]

namespace Icm.TaskManager.Rest.Configuration
{
    public static class NinjectWebCommon
    {
        private static readonly Bootstrapper Bootstrapper = new Bootstrapper();

        /// <summary>
        /// Starts the application
        /// </summary>
        public static void Start()
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            Bootstrapper.Initialize(CreateKernel);
        }

        /// <summary>
        /// Stops the application.
        /// </summary>
        public static void Stop()
        {
            Bootstrapper.ShutDown();
        }

        /// <summary>
        /// Creates the kernel that will manage your application.
        /// </summary>
        /// <returns>The created kernel.</returns>
        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            try
            {
                kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
                kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();

                RegisterServices(kernel);
                return kernel;
            }
            catch
            {
                kernel.Dispose();
                throw;
            }
        }

        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {
            var repo = new FakeTaskRepository(new Dictionary<TaskId, Task>
            {
                { new TaskId(1), Task.Create("Test task 1", Instant.FromUtc(2016, 1, 1, 1, 1), SystemClock.Instance.Now) }
            });
            kernel.Bind<ITaskRepository>().ToConstant(repo).InSingletonScope();
            ////kernel.Bind<ITaskRepository>().To<Infrastructure.TaskRepository>().InRequestScope();
            kernel.Bind<ITaskService>().To<TaskService>().InRequestScope();
            kernel.Bind<ICurrentDateProvider>().To<Infrastructure.NowCurrentDateProvider>().InRequestScope();
        }
    }
}
