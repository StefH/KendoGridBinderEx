﻿using System.Web.Optimization;

namespace KendoGridBinderEx.Examples.MVC
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            // Kendo JS and CSS
            //const string kendoVersion = "2014.1.318";
            //const string kendoFile = "kendo.web.min.js";

            const string kendoVersion = "2015.2.624";
            const string kendoFile = "kendo.all.min.js";

            bundles.Add(new ScriptBundle("~/bundles/kendo")
                .Include("~/Scripts/kendoExtensions.js")
                .Include("~/Scripts/kendo/" + kendoVersion + "/" + kendoFile)
            );
            bundles.Add(new StyleBundle("~/ContentKendo").Include(
                "~/Content/kendo/" + kendoVersion + "/kendo.common.min.css",
                "~/Content/kendo/" + kendoVersion + "/kendo.uniform.min.css",
                "~/Content/kendo/" + kendoVersion + "/kendo.silver.min.css"
                )
            );

            // JS
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include("~/Scripts/modernizr-{version}.js"));
            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include("~/Scripts/bootstrap.js"));
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

            bundles.Add(new ScriptBundle("~/bundles/angular").Include(
                "~/Scripts/angular.min.js", // core
                "~/Scripts/angular-route.min.js",
                "~/Scripts/angular-kendo.min.js"
                )
            );


            bundles.Add(new ScriptBundle("~/bundles/angularControllers").Include(
               //"~/AngularControllers/*.js"
               )
           );
            // CSS
            bundles.Add(new StyleBundle("~/Content/css").Include(
                    "~/Content/themes/base/all.css",
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