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
using KendoGridBinderEx.Examples.MVC.AutoMapper;

namespace KendoGridBinderEx.Examples.MVC
{
    public class MvcApplication : HttpApplication
    {
        private readonly bool _miniProfilerEnabled = ApplicationConfig.MiniProfilerEnabled;

        protected void Application_Start()
        {
            System.Linq.Dynamic.Core.ExtensibilityPoint.QueryOptimizer = ExpressionOptimizer.visit;
            QueryInterceptor.Core.ExtensibilityPoint.QueryOptimizer = ExpressionOptimizer.visit;

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

            AutoMapperConfig.InitAutoMapper(type => UnityBootstrapper.Container.Resolve(type, null));

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
    }
}