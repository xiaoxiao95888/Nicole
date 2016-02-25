using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LinqToExcel;
using Nicole.Web.Models;

namespace Nicole.Web.Controllers
{
    public class FinanceController : Controller
    {
        // GET: Finance
        public ActionResult AccountReceivable()
        {
            return View();
        }
        public ActionResult ApplyExpense()
        {
            return View();
        }
        public ActionResult ApplyExpenseAudit()
        {
            return View();
        }

       
        [HttpGet]
        public ActionResult ReconciliationUpload()
        {
            return View();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileid">fileid</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ReconciliationUpload(Guid fileid)
        {
            var model = new List<UploadReconciliationModel>();
            var uploadFilePath = ConfigurationManager.AppSettings["UploadFilePath"];
            if (!Directory.Exists(uploadFilePath))
            {
                Directory.CreateDirectory(uploadFilePath);
            }
            var fileNamePrefix = fileid.ToString();
            var file = System.Web.HttpContext.Current.Request.Files[0];
            if (file.ContentLength > 0)
            {
                var fileName = Path.GetFileName(file.FileName);
                //var fileExtension = Path.GetExtension(fileName);
                //var newFileName = fileNamePrefix + fileExtension;
                //var fileFullPath = uploadFilePath + newFileName;
                var fileFullPath = uploadFilePath + fileNamePrefix;
                try
                {
                    file.SaveAs(fileFullPath);
                    //return RedirectToAction("Reconciliation", new { filePath = uploadFilePath });
                    var excel = new ExcelQueryFactory(fileFullPath);
                    model = excel.Worksheet<UploadReconciliationModel>(0).ToList();
                }
                catch (Exception ex)
                {
                    System.IO.File.Delete(fileFullPath);
                }
            }
            return View(model.ToArray());
        }
    }
}