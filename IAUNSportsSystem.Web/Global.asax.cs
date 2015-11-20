using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Data.Entity.Infrastructure.Interception;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using IAUNSportsSystem.DataLayer;
using IAUNSportsSystem.Web.DependencyResolution;
using IAUNSportsSystem.Web.Infrastructure;
using IAUNSportsSystem.Web.WebTasks;
using Iris.Web.IrisMembership;
using StructureMap;
using StructureMap.Web.Pipeline;

namespace IAUNSportsSystem.Web
{
    public static class IaunSportsSystemApp
    {
        public static string GetSiteRootUrl()
        {
            return ConfigurationManager.AppSettings["SiteRootUrl"];
        }
    }

    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {

            try
            {
                AreaRegistration.RegisterAllAreas();
                FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
                RouteConfig.RegisterRoutes(RouteTable.Routes);
                BundleConfig.RegisterBundles(BundleTable.Bundles);

                ScheduledTasksRegistry.Init();


                //Database.SetInitializer<SportsSystemDbContext>(null);
                Database.SetInitializer(new MigrateDatabaseToLatestVersion<SportsSystemDbContext, IAUNSportsSystem.DataLayer.Migrations.Configuration>());

                DbInterception.Add(new YeKeInterceptor());



                ViewEngines.Engines.Clear();
                ViewEngines.Engines.Add(new RazorViewEngine());

                MvcHandler.DisableMvcResponseHeader = true;
            }
            catch
            {
                HttpRuntime.UnloadAppDomain(); // سبب ری استارت برنامه و آغاز مجدد آن با درخواست بعدی می‌شود
                throw;
            }

        }

        protected void Application_AuthenticateRequest(Object sender, EventArgs e)
        {
            if (ShouldIgnoreRequest()) return;

            var context = DependencyResolver.Current.GetService<HttpContextBase>();

            var principalService = IoC.Container.GetInstance<IPrincipalService>();

            // Set the HttpContext's User to our IPrincipal
            context.User = principalService.GetCurrent();
        }

        protected void Application_PreSendRequestHeaders(object sender, EventArgs e)
        {
            var app = sender as HttpApplication;
            if (app == null || app.Context == null)
                return;
            app.Context.Response.Headers.Remove("Server");
        }

        private void Application_EndRequest(object sender, EventArgs e)
        {
            HttpContextLifecycle.DisposeAndClearAll();
        }

        protected void Application_End()
        {
            ScheduledTasksRegistry.End();
            //نکته مهم این روش نیاز به سرویس پینگ سایت برای زنده نگه داشتن آن است
            ScheduledTasksRegistry.WakeUp(IaunSportsSystemApp.GetSiteRootUrl());
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }


        private bool ShouldIgnoreRequest()
        {
            string[] reservedPath =
                            {
                                "/__browserLink",
                                "/img",
                                "/fonts",
                                "/Scripts",
                                "/Content"
                            };

            var rawUrl = Context.Request.RawUrl;
            if (reservedPath.Any(path => rawUrl.StartsWith(path, StringComparison.OrdinalIgnoreCase)))
            {
                return true;
            }

            return BundleTable.Bundles.Select(bundle => bundle.Path.TrimStart('~'))
                      .Any(bundlePath => rawUrl.StartsWith(bundlePath, StringComparison.OrdinalIgnoreCase));
        }


    }
}
