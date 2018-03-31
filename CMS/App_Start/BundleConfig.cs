using System.Web;
using System.Web.Optimization;

namespace CMS
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            // Enable Cdn
            bundles.UseCdn = true;
            // var bootstrapCssCdnPath = "https://maxcdn.bootstrapcdn.com/bootstrap/4.0.0-beta/css/bootstrap.min.css";
            // var bootstrapJsCdnPath = "https://maxcdn.bootstrapcdn.com/bootstrap/4.0.0-beta/js/bootstrap.min.js";

            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js",
                        "~/Scripts/umd/popper.js",
                        "~/Scripts/jquery.unobtrusive-ajax.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            // Bootsrtap Scripts Bundel
            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                       "~/Scripts/respond.js"));

            // Site Scripts Bundels
            bundles.Add(new ScriptBundle("~/Scripts/js").Include(
                      "~/Scripts/wow.min.js",
                      "~/Scripts/nanobar.min.js",
                      "~/Scripts/script.js"));

            // Dashboard Scripts Bundels
            bundles.Add(new ScriptBundle("~/admin/scripts").Include(
                    "~/Scripts/jquery.easing.min.js",
                    "~/Scripts/Chart.min.js",
                    "~/Scripts/jquery.dataTables.js",
                    "~/Scripts/dataTables.bootstrap4.js",
                    "~/Scripts/admin.min.js",
                    "~/Scripts/admin-datatables.min.js",
                    "~/Scripts/admin-charts.min.js"));

            // Bootstrap Style Bundel
            bundles.Add(new StyleBundle("~/Content/bootstrap").Include(
                      "~/Content/bootstrap.css"));

            // Site Style Bundels
            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/normalize.css",
                      "~/Content/animate.css",
                      "~/Content/font-awesome.min.css",
                      "~/Content/site.css"));

            // Dashboard Style Bundels
            bundles.Add(new StyleBundle("~/Admin/css").Include(
                  "~/Content/font-awesome.min.css",
                  "~/Content/dataTables.bootstrap4.css",
                  "~/Content/admin.css"));

            // Enable Optimization and Compress files
            BundleTable.EnableOptimizations = true;
        }
    }
}
