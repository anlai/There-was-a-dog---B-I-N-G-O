using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Bingo.Web.Controllers;
using Bingo.Web.Models;
using Castle.Windsor;
using Microsoft.Practices.ServiceLocation;
using UCDArch.Data.NHibernate;
using UCDArch.Web.IoC;
using UCDArch.Web.ModelBinder;

namespace Bingo.Web
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );

        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);

#if DEBUG
            HibernatingRhinos.Profiler.Appender.NHibernate.NHibernateProfiler.Initialize();
#endif

            ModelBinders.Binders.DefaultBinder = new UCDArchModelBinder();

            //AutomapperConfig.Configure();

            NHibernateSessionConfiguration.Mappings.UseFluentMappings(typeof(Class1).Assembly);

            IWindsorContainer container = InitializeServiceLocator();
        }

        private static IWindsorContainer InitializeServiceLocator()
        {
            IWindsorContainer container = new WindsorContainer();

            ControllerBuilder.Current.SetControllerFactory(new WindsorControllerFactory(container));

            container.RegisterControllers(typeof(HomeController).Assembly);
            ComponentRegistrar.AddComponentsTo(container);

            ServiceLocator.SetLocatorProvider(() => new WindsorServiceLocator(container));

            return container;
        }
    }
}