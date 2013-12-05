using System.Web;
using System.Web.Optimization;

namespace SS
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.IgnoreList.Clear();

            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryNew").Include(
                        "~/Scripts/jquery-1.9.1.js",
                        "~/Scripts/jquery-ui-1.10.3.custom.js"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrapjs").Include(
                        "~/Scripts/bootstrap.min.js",
                        "~/Scripts/bootstrap-switch.js"));

            bundles.Add(new StyleBundle("~/Content/bootstrapcss").Include(
                        "~/Content/bootstrap.min.css",
                        "~/Content/bootstrap.css",
                        "~/Content/bootstrap-responsive.min.css",
                        "~/Content/bootstrap-switch.css"));
        }
    }
}