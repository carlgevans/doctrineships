[assembly: WebActivator.PreApplicationStartMethod(typeof(DoctrineShips.Web.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivator.ApplicationShutdownMethodAttribute(typeof(DoctrineShips.Web.App_Start.NinjectWebCommon), "Stop")]

namespace DoctrineShips.Web.App_Start
{
    using System;
    using System.Web;
    using DoctrineShips.Data;
    using DoctrineShips.Repository;
    using DoctrineShips.Service;
    using DoctrineShips.Validation;
    using EveData;
    using GenericRepository;
    using GenericRepository.Providers.EntityFramework;
    using Microsoft.Web.Infrastructure.DynamicModuleHelper;
    using Ninject;
    using Ninject.Web.Common;
    using Tools;
    
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
            kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
            kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();
            kernel.Bind<IDoctrineShipsServices>().To<DoctrineShipsServices>();
            kernel.Bind<IUnitOfWork>().To<UnitOfWork>().InRequestScope();
            kernel.Bind<IEveDataSource>().To<EveDataSourceCached>();
            kernel.Bind<IDbContext>().To<DoctrineShipsContext>();
            kernel.Bind<IDoctrineShipsRepository>().To<DoctrineShipsRepository>();
            kernel.Bind<IDoctrineShipsValidation>().To<DoctrineShipsValidation>();
            kernel.Bind<ISystemLogger>().To<SystemLogger>();
            kernel.Bind<ISystemLoggerStore>().To<DoctrineShipsRepository>();
            
            RegisterServices(kernel);
            return kernel;
        }

        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {
        }        
    }
}
