using System.Web.Optimization;

namespace Nicole.Web
{
    public class BundleConfig
    {
        // 有关绑定的详细信息，请访问 http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // 使用要用于开发和学习的 Modernizr 的开发版本。然后，当你做好
            // 生产准备时，请使用 http://modernizr.com 上的生成工具来仅选择所需的测试。
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                "~/Content/bootstrap.css",
                "~/Content/site.css",
                "~/Content/dashboard.css"));

            bundles.Add(new ScriptBundle("~/bundles/knockout").Include(
                "~/Scripts/knockout-3.2.0.js",
                "~/Scripts/knockout.mapping-latest.js"));

            bundles.Add(new ScriptBundle("~/bundles/Main").Include(
                "~/Scripts/JS/Main.js"));

            //moment
            bundles.Add(new ScriptBundle("~/bundles/moment").Include(
                "~/Scripts/moment-with-locales.min.js"));
            //Home
            bundles.Add(new ScriptBundle("~/bundles/Home").Include(
           "~/Scripts/JS/Home.js"));
            //ProductSetting
            bundles.Add(new ScriptBundle("~/bundles/ProductSetting").Include(
          "~/Scripts/JS/ProductSetting.js",
          "~/Scripts/jquery.bootpag.min.js"));
            //StandardCostSetting
            bundles.Add(new ScriptBundle("~/bundles/StandardCostSetting").Include(
          "~/Scripts/JS/StandardCostSetting.js",
          "~/Scripts/jquery.bootpag.min.js",
          "~/Scripts/bootstrap-multiselect.js",
           "~/Scripts/bootstrap-multiselect-collapsible-groups.js",
           "~/Scripts/bootstrap-datetimepicker.min.js"));
            //StandardCostSetting
            bundles.Add(new StyleBundle("~/Content/StandardCostSetting").Include(
                "~/Content/bootstrap-datetimepicker.min.css"));
            //EnquiryManager
            bundles.Add(new ScriptBundle("~/bundles/EnquiryManager").Include(
        "~/Scripts/JS/EnquiryManager.js",
        "~/Scripts/jquery.bootpag.min.js"));
        }
    }
}
