using System.Web;
using System.Web.Optimization;

namespace IAUNSportsSystem.Web
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));



            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/modified-bootstrap.js"
                      ));


            bundles.Add(new ScriptBundle("~/bundles/common").Include(
                     "~/Scripts/angular.js",
                     "~/Scripts/angular-route.js",
                     "~/Scripts/ngFxBundle.js",
                     "~/Scripts/moment.js",
                     "~/Scripts/moment-jalaali.js",
                     "~/Scripts/ng-file-upload-all.js",
                     "~/Scripts/toaster/toaster.js",
                     "~/Scripts/angular-sanitize.js",
                     "~/Scripts/angular-ui/ui-bootstrap-tpls.js",
                     "~/Scripts/smart-table.js",
                     "~/Scripts/select.js",
                     "~/Scripts/angular-loadingbar/loading-bar.js",
                     "~/Scripts/angular-validation-match.js",
                     "~/Scripts/AppScripts/commonConstants.js",
                     "~/Scripts/AppScripts/Directives/directives.js"
                     ));


            bundles.Add(new ScriptBundle("~/bundles/admin").Include(
                     "~/Scripts/AppScripts/app.js",
                     "~/Scripts/AppScripts/appconfig.js",
                     "~/Scripts/AppScripts/Controllers/Admin/AdminController.js",
                     "~/Scripts/AppScripts/Services/adminService.js",
                     "~/Scripts/ckeditor/angular-ckeditor.js"
                     ));




            bundles.Add(new StyleBundle("~/Content/common").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/bootstrap-rtl/bootstrap-rtl.css",
                      "~/Content/font-awesome.css"
                      ));



            bundles.Add(new ScriptBundle("~/bundles/userPanel").Include(
                    "~/Scripts/AppScripts/userPanelApp.js",
                    "~/Scripts/AppScripts/userPanelAppConfig.js",
                    "~/Scripts/AppScripts/Controllers/UserPanel/UserPanelController.js",
                    "~/Scripts/AppScripts/Services/userPanelService.js",
                    "~/Scripts/angular-scroll.js"
                    ));



            bundles.Add(new StyleBundle("~/Content/admin").Include(
                      "~/Content/admin.css",
                      "~/Content/admin-rtl.css"));

            bundles.Add(new StyleBundle("~/Content/userPanel").Include(
                      "~/Content/user-panel.css"
                      ));

            bundles.Add(new StyleBundle("~/Content/angularParts").Include(
                      "~/Scripts/toaster/toaster.css",
                      "~/Content/select.css",
                      "~/Content/loading-bar.css"));

            bundles.Add(new StyleBundle("~/Content/site").Include(
                     "~/Content/jquery.reject.css",
                     "~/Content/Site.css",
                     "~/Content/animate.css"));

            bundles.Add(new ScriptBundle("~/bundles/account").Include(
                   "~/Scripts/AppScripts/accountApp.js",
                   "~/Scripts/AppScripts/accountAppConfig.js",
                   "~/Scripts/AppScripts/Controllers/Account/AccountController.js",
                   "~/Scripts/AppScripts/Services/accountService.js"
                   ));


            bundles.Add(new ScriptBundle("~/bundles/site").Include(
                  "~/Scripts/jquery.reject.js",
                  "~/Scripts/site.js"
                  ));
        }
    }
}
