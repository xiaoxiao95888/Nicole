using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using LinqToExcel;
using Nicole.Library.Services;
using Nicole.Web.MapperHelper;
using Nicole.Web.Models;

namespace Nicole.Web.Controllers.API
{
    public class ReconciliationController : BaseApiController
    {
        private readonly IOrderService _orderService;
        public ReconciliationController(IOrderService orderService)
        {
            _orderService = orderService;
        }
        /// <summary>
        /// 销账
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public object Get(string id)
        {
            var uploadFilePath = ConfigurationManager.AppSettings["UploadFilePath"];
            var fileFullPath = uploadFilePath + id;
            var excel = new ExcelQueryFactory(fileFullPath);
            var result = excel.Worksheet<UploadReconciliationModel>(0).ToArray();
            var orderisd = result.Select(n => n.OrderCode).ToArray().Distinct();
            var orders =
                _orderService.GetOrders()
                    .Where(n => n.IsApproved && n.IsDeleted == false && orderisd.Contains(n.Code))
                    .ToArray();
            foreach (var item in result)
            {
                var order = orders.FirstOrDefault(n => n.Code == item.OrderCode);
                item.State = "未销账";
                if (order != null)
                {
                    item.OrderDate = order.OrderDate;
                    if (string.IsNullOrEmpty(item.PartNumber) == false &&
                        order.Enquiry.Product.PartNumber.Trim().ToUpper() == item.PartNumber.Trim().ToUpper() &&
                        order.Qty == item.Qty)
                    {
                        item.State = "已销账";
                    }
                    else if(string.IsNullOrEmpty(item.PartNumber) == false &&
                        order.Enquiry.Product.PartNumber.Trim().ToUpper() == item.PartNumber.Trim().ToUpper())
                    {
                        item.State = "未完全销账";
                    }
                }
            }
            return result;
        }
        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="filePath"></param>
        ///// <returns></returns>
        //public ActionResult Reconciliation(string filePath)
        //{
        //    var excel = new ExcelQueryFactory(filePath);
        //    var result = excel.Worksheet<UploadReconciliationModel>(0).ToArray();
        //    return View(result);
        //}
    }
}
