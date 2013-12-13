using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Optimization;

namespace Icm.TaskManager.Web.Configuration
{
    /// <summary>
    /// General bundle configuration
    /// </summary>
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/knockout").Include(
                "~/Scripts/knockout-{version}.js",
                "~/Scripts/knockout.validation.js",
                "~/Scripts/knockout.mapping-latest.js"));

            bundles.Add(new ScriptBundle("~/bundles/app").Include(
                "~/Scripts/app/ajaxPrefilters.js",
                "~/Scripts/app/app.bindings.js",
                "~/Scripts/app/app.datamodel.js",
                "~/Scripts/app/app.viewmodel.js",
                "~/Scripts/app/home.viewmodel.js",
                "~/Scripts/app/account/login.viewmodel.js",
                "~/Scripts/app/account/register.viewmodel.js",
                "~/Scripts/app/account/registerExternal.viewmodel.js",
                "~/Scripts/app/account/manage.viewmodel.js",
                "~/Scripts/app/account/userInfo.viewmodel.js",
                "~/Scripts/app/task/task.viewmodel.js",
                "~/Scripts/app/task/taskList.viewmodel.js",
                "~/Scripts/app/_run.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/respond").Include(
                "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                 "~/Content/Site.css"));
        }
    }
}
