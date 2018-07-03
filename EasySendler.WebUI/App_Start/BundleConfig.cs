using System.Web;
using System.Web.Optimization;

namespace EasySendler
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*",
                        "~/Scripts/jquery.unobtrusive-ajax.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/additionaljq").Include(
                        "~/Scripts/select2.full.min.js",
                        "~/Scripts/jquery.bootstrap-duallistbox.min.js",
                        "~/Scripts/jquery.knob.min.js",
                        "~/Scripts/underscore.min.js",
                        "~/Scripts/bootstrap-filestyle.min.js",
                        "~/Scripts/site/ConfigureRecipientList.js",
                        "~/Scripts/site/Delivery.js",
                        "~/Scripts/site/Recipients.js",
                        "~/Scripts/site/RecipientLists.js",
                        "~/Scripts/site/MailSettings.js"));

            bundles.Add(new ScriptBundle("~/bundles/tinymce").Include(
                        "~/Scripts/tinymce/tinymce.min.js",
                        "~/Scripts/site/TinyMCE.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js",
                      "~/Scripts/jquery.matchHeight-min.js",
                      "~/Scripts/bootstrap2-toggle.min.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/select2.min.css",
                      "~/Content/bootstrap-duallistbox.min.css",
                      "~/Content/bootstrap2-toggle.min.css",
                      "~/Content/site.css"));
        }
    }
}
