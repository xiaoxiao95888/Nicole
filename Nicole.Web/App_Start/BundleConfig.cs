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
            //EnquirySetting
            bundles.Add(new ScriptBundle("~/bundles/EnquirySetting").Include(
                "~/Scripts/JS/EnquirySetting.js",
                "~/Scripts/jquery.bootpag.min.js"));
            //MyEnquiry
            bundles.Add(new ScriptBundle("~/bundles/MyEnquiry").Include(
                "~/Scripts/JS/MyEnquiry.js",
                "~/Scripts/jquery.bootpag.min.js",
                "~/Scripts/bootstrap-datetimepicker.min.js"));

            //MyEnquiry
            bundles.Add(new StyleBundle("~/Content/MyEnquiry").Include(
                "~/Content/bootstrap-datetimepicker.min.css"));
            //LeftNavigationSetting
            bundles.Add(new ScriptBundle("~/bundles/LeftNavigationSetting").Include(
              "~/Scripts/JS/LeftNavigationSetting.js"));
            //CustomerCreate
            bundles.Add(new ScriptBundle("~/bundles/CustomerCreate").Include(
                "~/Scripts/JS/CustomerCreate.js",
                "~/Scripts/jquery.bootpag.min.js"));
            //MyCustomer
            bundles.Add(new ScriptBundle("~/bundles/MyCustomer").Include(
                "~/Scripts/JS/MyCustomer.js",
                "~/Scripts/jquery.bootpag.min.js"));

            //ProductSearch
            bundles.Add(new ScriptBundle("~/bundles/ProductSearch").Include(
                "~/Scripts/JS/ProductSearch.js",
                "~/Scripts/jquery.bootpag.min.js"));
            //CustomerManager
            bundles.Add(new ScriptBundle("~/bundles/CustomerManager").Include(
                "~/Scripts/JS/CustomerManager.js",
                "~/Scripts/jquery.bootpag.min.js"));
            //MyOrder
            bundles.Add(new ScriptBundle("~/bundles/MyOrder").Include(
                "~/Scripts/JS/MyOrder.js",
                "~/Scripts/jquery.bootpag.min.js",
                "~/Scripts/bootstrap-datetimepicker.min.js"));
            //MyOrder
            bundles.Add(new StyleBundle("~/Content/MyOrder").Include(
                "~/Content/bootstrap-datetimepicker.min.css"));
            //OrderReview
            bundles.Add(new ScriptBundle("~/bundles/OrderReview").Include(
                "~/Scripts/JS/OrderReview.js",
                "~/Scripts/jquery.bootpag.min.js"));
            //AllOrders
            bundles.Add(new ScriptBundle("~/bundles/AllOrders").Include(
                "~/Scripts/JS/AllOrders.js",
                "~/Scripts/jquery.bootpag.min.js"));
            //AccountReceivable
            bundles.Add(new ScriptBundle("~/bundles/AccountReceivable").Include(
                "~/Scripts/JS/AccountReceivable.js",
                "~/Scripts/jquery.bootpag.min.js",
                "~/Scripts/bootstrap-datetimepicker.min.js"));
            //AccountReceivable
            bundles.Add(new StyleBundle("~/Content/AccountReceivable").Include(
                "~/Content/bootstrap-datetimepicker.min.css"));
            //ApplyExpense
            bundles.Add(new ScriptBundle("~/bundles/ApplyExpense").Include(
                "~/Scripts/JS/ApplyExpense.js",
                "~/Scripts/jquery.bootpag.min.js",
                "~/Scripts/bootstrap-datetimepicker.min.js"));
            //ApplyExpense
            bundles.Add(new StyleBundle("~/Content/ApplyExpense").Include(
                "~/Content/bootstrap-datetimepicker.min.css"));
            //ApplyExpenseAudit
            bundles.Add(new ScriptBundle("~/bundles/ApplyExpenseAudit").Include(
                "~/Scripts/JS/ApplyExpenseAudit.js",
                "~/Scripts/jquery.bootpag.min.js",
                "~/Scripts/bootstrap-datetimepicker.min.js"));
            //ApplyExpenseAudit
            bundles.Add(new StyleBundle("~/Content/ApplyExpenseAudit").Include(
                "~/Content/bootstrap-datetimepicker.min.css"));
            //ReconciliationUpload
            bundles.Add(new ScriptBundle("~/bundles/ReconciliationUpload").Include(
              "~/Scripts/JS/ReconciliationUpload.js"));
            //SampleApply
            bundles.Add(new ScriptBundle("~/bundles/SampleApply").Include(
                "~/Scripts/JS/SampleApply.js",
                "~/Scripts/jquery.bootpag.min.js"));
            //SampleAudit
            bundles.Add(new ScriptBundle("~/bundles/SampleAudit").Include(
                "~/Scripts/JS/SampleAudit.js",
                "~/Scripts/jquery.bootpag.min.js"));
            //SampleRecord
            bundles.Add(new ScriptBundle("~/bundles/SampleRecord").Include(
               "~/Scripts/JS/SampleRecord.js",
               "~/Scripts/jquery.bootpag.min.js"));
        }
    }
}
