using AutoMapper;
using FluentValidation.Mvc;
using KendoGridBinderEx.Examples.Business.Unity;
using KendoGridBinderEx.Examples.MVC.Unity;
using StackExchange.Profiling;
using StackExchange.Profiling.EntityFramework6;
using StackExchange.Profiling.SqlFormatters;
using System;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace KendoGridBinderEx.Examples.MVC
{
    public class MvcApplication : HttpApplication
    {
        private readonly bool _miniProfilerEnabled = ApplicationConfig.MiniProfilerEnabled;

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            //GlobalConfiguration.Configuration.EnableCors(new EnableCorsAttribute("*", "*", "*"));

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            ModelBinders.Binders.Add(typeof(DateTime), new MyDateTimeBinder());

            if (_miniProfilerEnabled)
            {
                MiniProfiler.Settings.PopupRenderPosition = RenderPosition.Right;
                MiniProfiler.Settings.SqlFormatter = new SqlServerFormatter();
                MiniProfiler.Settings.ShowControls = false;
                MiniProfilerEF6.Initialize();
            }

            UnityMVCBootstrapper.Initialise(UnityBootstrapper.Container);

            InitAutoMapper(type => UnityBootstrapper.Container.Resolve(type, null));

            FluentValidationModelValidatorProvider.Configure();
        }

        protected void Application_BeginRequest()
        {
            if (_miniProfilerEnabled)
            {
                MiniProfiler.Start();
            }
        }

        protected void Application_EndRequest()
        {
            if (_miniProfilerEnabled)
            {
                MiniProfiler.Stop();
            }
        }

        private static void InitAutoMapper(Func<Type, object> resolver)
        {
            Mapper.Initialize(map =>
            {
                map.ConstructServicesUsing(resolver);
            });

            // Call static method 'InitAutoMapper' on all controllers.
            var assemblies = Assembly.GetExecutingAssembly().GetTypes().Where(t => t.FullName.Contains("KendoGridBinderEx.Examples.MVC.Controllers") && t.Name.EndsWith("Controller")).ToList();
            foreach (var controller in assemblies)
            {
                var methodInfo = controller.GetMethod("InitAutoMapper");

                if (methodInfo != null)
                {
                    methodInfo.Invoke(controller, new object[] { });
                }
            }

            Mapper.AssertConfigurationIsValid();
        }
    }
}