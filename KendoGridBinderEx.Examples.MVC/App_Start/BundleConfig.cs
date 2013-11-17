using System.Web.Optimization;

namespace KendoGridBinderEx.Examples.MVC
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            // Kendo JS and CSS
            const string kendoVersion = "2013.2.716";
            bundles.Add(new ScriptBundle("~/bundles/kendo")
                .Include("~/Scripts/kendo/" + kendoVersion + "/kendo.web.min.js")
                .Include("~/Scripts/kendoExtensions.js")
            );
            bundles.Add(new StyleBundle("~/ContentKendo").Include(
                "~/Content/kendo/" + kendoVersion + "/kendo.common.min.css",
                "~/Content/kendo/" + kendoVersion + "/kendo.uniform.min.css"
                )
            );

            // JS
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include("~/Scripts/jquery-{version}.js"));
            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include("~/Scripts/jquery-ui-{version}.js"));
            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                "~/Scripts/jquery.unobtrusive*",
                "~/Scripts/jquery.validate*",
                "~/Scripts/jquery.validation-extra.js",
                "~/Scripts/jquery.validation-extra2.js"
                )
            );

            bundles.Add(new ScriptBundle("~/bundles/common")
                .Include("~/Scripts/common.js")
            );

            bundles.Add(new ScriptBundle("~/bundles/knockout").Include(
                "~/Scripts/knockout-{version}.js",
                "~/Scripts/knockout.mapping-latest.js",
                "~/Scripts/knockout-kendo.js"
                )
            );

            // CSS
            bundles.Add(new StyleBundle("~/Content/css").Include(
                    "~/Content/themes/base/jquery.ui.core.css",
                    "~/Content/themes/base/jquery.ui.accordion.css",
                    "~/Content/themes/base/jquery.ui.autocomplete.css",
                    "~/Content/themes/base/jquery.ui.button.css",
                    "~/Content/themes/base/jquery.ui.datepicker.css",
                    "~/Content/themes/base/jquery.ui.dialog.css",
                    "~/Content/themes/base/jquery.ui.menu.css",
                    "~/Content/themes/base/jquery.ui.progressbar.css",
                    "~/Content/themes/base/jquery.ui.resizable.css",
                    "~/Content/themes/base/jquery.ui.selectable.css",
                    "~/Content/themes/base/jquery.ui.slider.css",
                    "~/Content/themes/base/jquery.ui.spinner.css",
                    "~/Content/themes/base/jquery.ui.tabs.css",
                    "~/Content/themes/base/jquery.ui.theme.css",
                    "~/Content/site.css"
                )
            );

            // Clear all items from the default ignore list to allow minified CSS and JavaScript files to be included in debug mode
            bundles.IgnoreList.Clear();

            // Add back some of the default ignore list rules
            bundles.IgnoreList.Ignore("*.intellisense.js");
            bundles.IgnoreList.Ignore("*-vsdoc.js");
            bundles.IgnoreList.Ignore("*.debug.js", OptimizationMode.WhenEnabled);

            BundleTable.EnableOptimizations = false;
        }
    }
}