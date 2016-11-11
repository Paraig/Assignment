using Assignment.Models;
using Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Interfaces.Models;
using Interfaces.DAL;
using Interfaces.ProcessExcel;
using System.Text;

namespace Assignment.Controllers
{
    public class HomeController : Controller
    {
        IDal _dal;
        IProcessExcel _processExcel;

        public HomeController( IDal dal, IProcessExcel processExcel)
        {
            _dal = dal;
            _processExcel = processExcel;
        }

        public ActionResult Index()
        {
            var model = _dal.LoadTransactionData();

            return View(model);
        }

        [HttpGet]
        public ActionResult UploadFile()
        {
            return View();
        }

        [HttpPost]
        public JsonResult UploadFile(string id)
        {
            int numRows = 0;

            try
            {
                foreach (string file in Request.Files)
                {
                    var fileContent = Request.Files[file];

                    if (fileContent != null && fileContent.ContentLength > 0)
                    {
                        var stream = fileContent.InputStream;
                        var fileName = Path.GetFileName(file);
                        var path = Path.Combine(Server.MapPath("~/App_Data/"), fileName);
                        using (var fileStream = System.IO.File.Create(path))
                        {
                            stream.CopyTo(fileStream);
                        }
                        if (fileContent.FileName.EndsWith(".xlsx"))
                            numRows = _processExcel.ProcessExcelFile(path);

                        System.IO.File.Delete(path);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json("Upload failed");
            }
            var message = new StringBuilder();

            foreach(var transaction in _processExcel.FailedTransactions)
            {
                if(message.Length > 0)
                    message.Append(" | ");
                message.Append("Row number ");
                message.Append(transaction.RowNumber);
                message.Append(" : ");
                message.Append(transaction.ErrorString);
            }

            return Json(String.Format("File uploaded {0} successfully, the following were not processed {1} ", numRows.ToString(), message.ToString()));
        }

        [HttpGet]
        public ActionResult Edit(long id)
        {
            var model = _dal.LoadTransactionData(id);

            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(TransactionData data)
        {
            if(data.IsValid)
            {
                if(_dal.UpdateTransactionData(data) == 1)
                    return Json(new { status = true, msg = "Success", doRedirect = false });

            }
            return Json(new { status = false, msg = String.Format("Failed to update row - {0}", data.ErrorString), doRedirect = false });
        }

        [HttpPost]
        public ActionResult Delete(long id)
        {
            if(_dal.DeleteTransactionData(id) == 1)
            {
                return Json(new { status = true, msg = "Success", doRedirect = true, redirect = "/Home/Index" }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { status = false, msg = "Failed to delete row", doRedirect = false }); ;
        }

    }
}