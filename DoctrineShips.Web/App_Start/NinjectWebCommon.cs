[assembly: WebActivator.PreApplicationStartMethod(typeof(DoctrineShips.Web.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivator.ApplicationShutdownMethodAttribute(typeof(DoctrineShips.Web.App_Start.NinjectWebCommon), "Stop")]

namespace DoctrineShips.Web.App_Start
{
    using System;
    using System.Web;
    using System.Web.Configuration;
    using DoctrineShips.Data;
    using DoctrineShips.Repository;
    using DoctrineShips.Service;
    using DoctrineShips.Service.Entities;
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
            // Fetch application settings and instantiate a DoctrineShipsSettings object.
            DoctrineShipsSettings doctrineShipsSettings = new DoctrineShipsSettings(
                WebConfigurationManager.AppSettings["TaskKey"],
                WebConfigurationManager.AppSettings["SecondKey"],
                WebConfigurationManager.AppSettings["WebsiteDomain"],
                WebConfigurationManager.AppSettings["TwitterConsumerKey"],
                WebConfigurationManager.AppSettings["TwitterConsumerSecret"],
                WebConfigurationManager.AppSettings["TwitterAccessToken"],
                WebConfigurationManager.AppSettings["TwitterAccessTokenSecret"],
                WebConfigurationManager.AppSettings["Brand"]
            );

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
            kernel.Bind<IDoctrineShipsSettings>().ToConstant(doctrineShipsSettings);
            
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
