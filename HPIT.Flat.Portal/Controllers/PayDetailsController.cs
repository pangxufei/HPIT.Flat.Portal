using ExcelCake.Intrusive;
using HPIT.Data.Core;
using HPIT.Evalute.Data.Model;
using HPIT.Flat.Data.Adapters;
using HPIT.Flat.Data.Entitys;
using HPIT.Flat.Data.ExportEntitys;
using HPIT.Flat.Portal.Common;
using HPIT.Web.Core.Deluxe;
//using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HPIT.Flat.Portal.Controllers
{
    public class PayDetailsController : Controller
    {
        // GET: PayDetails
        public ActionResult Index()
        {

            ViewBag.ProJname = PayDutails.GetProjName().Select(r => new SelectListItem
            {
                Text = r.ProjectName,
                Value = r.ProjectName
            });
            return View();
        }

        public DeluxeJsonResult QueryPageData(SearchModel<Payment> search,  string StuName)
        {
            int total = 0;
            HPITMemberInfo currentUser = DeluxeUser.CurrentMember;
            search.UserName = currentUser.RealName;
            search.RoleName = currentUser.FullName;
            var result = PayDutails.GetPageData(search, out total,  StuName).Select(r => new
            {
                r.MID,
                r.FileName,
                r.StuName,
                r.DormNo,
                r.StuNo,
                r.PayMoney,
                r.CreateTime,
                r.PayType,
                r.PayTypeString,
                r.AuditStatus,
                r.AuditStatusStr,
                r.Auditor,
                r.ProjectName,
                r.PEM,
                
            });
            var totalPages = total % search.PageSize == 0 ? total / search.PageSize : total / search.PageSize + 1;
            return new DeluxeJsonResult(new { Data = result, Total = total, TotalPages = totalPages });
        }


        public ActionResult ProjectNameIndex()
        {
            return View();
        }

        public DeluxeJsonResult GroupByProjectName()
        {
            var result = PaymentDetails.Instance.GroupByProjectName();
            return new DeluxeJsonResult(new { Data = result, Total = result.Count });
        }

        [AllowAnonymous]
        public FileStreamResult ExcelExportGroupByProjectName()
        {
            //指定项目存放的路径
            List<ProjectPayStatisticModel> dataList = PaymentDetails.Instance.GroupByProjectName();
            var temp = dataList.ExportToExcelBytes(); //导出为byte[]
            var path = Server.MapPath("~/Export/");//路径
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            var exportTitle = "导出文件";
            var exportFileName = exportTitle + DateTime.Now.Ticks + ".xlsx";
            var filePath = Path.Combine(path, exportFileName);
            FileInfo file = new FileInfo(filePath);
            System.IO.File.WriteAllBytes(file.FullName, temp);
            //return new FileContentResult(temp, "application/ms-excel");
            return File(new FileStream(filePath, FileMode.Open), "text/plain",exportFileName);
        }

        public ActionResult BatchIndex()
        {
            return View();
        }

        public DeluxeJsonResult GroupByBatch()
        {
            var result = PaymentDetails.Instance.GroupByBatch();
            return new DeluxeJsonResult(new { Data = result, Total = result.Count });
        }

        [AllowAnonymous]
        public FileStreamResult ExcelExportGroupByBatch()
        {
            //指定项目存放的路径
            List<BatchPayStatisticModel> dataList = PaymentDetails.Instance.GroupByBatch();
            var temp = dataList.ExportToExcelBytes(); //导出为byte[]
            var path = Server.MapPath("~/Export/");//路径
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            var exportTitle = "导出文件";
            var exportFileName = exportTitle + DateTime.Now.Ticks + ".xlsx";
            var filePath = Path.Combine(path, exportFileName);
            FileInfo file = new FileInfo(filePath);
            System.IO.File.WriteAllBytes(file.FullName, temp);
            //return new FileContentResult(temp, "application/ms-excel");
            return File(new FileStream(filePath, FileMode.Open), "text/plain", exportFileName);
        }

        public DeluxeJsonResult Audit(string id, int type)
        {
            HPITMemberInfo currentUser = DeluxeUser.CurrentMember;
            AuditLog log = new AuditLog();
            log.AuditName = currentUser.RealName;
            log.AuditState = type;
            log.PayID = id;
            log.RoleName = currentUser.FullName;
            PayDutails dal = new PayDutails();
            var result = dal.Audit(log);
            return new DeluxeJsonResult(result);
        }

        public DeluxeJsonResult BatchAudit(string ids, int type)
        {
            HPITMemberInfo currentUser = DeluxeUser.CurrentMember;
            if (string.IsNullOrEmpty(ids))
            {
                return new DeluxeJsonResult(new { Data="请选择要审批的单子", State = 500});
            }
            List<string> idList = ids.Split(',').ToList();
            PayDutails dal = new PayDutails();
            foreach (string id in idList)
            {
                AuditLog log = new AuditLog();
                log.AuditName = currentUser.RealName;
                log.AuditState = type;
                log.PayID = id;
                log.RoleName = currentUser.FullName;
                var result = dal.Audit(log);
            }
            
            return new DeluxeJsonResult(new { Data = "审批成功", State = 200 });
        }
    }
}