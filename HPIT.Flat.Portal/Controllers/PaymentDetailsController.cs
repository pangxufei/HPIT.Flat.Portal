using HPIT.Data.Core;
using HPIT.Evalute.Data.Model;
using HPIT.Flat.Data.Adapters;
using HPIT.Flat.Data.Entitys;
using HPIT.Flat.Portal.Common;
using HPIT.Web.Core.Deluxe;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ExcelCake.Intrusive;
using System.IO;
using System.Threading;

namespace HPIT.Flat.Portal.Controllers
{
    public class PaymentDetailsController : Controller
    {
        // GET: PaymentDetails
        public ActionResult Index()
        {

            ViewBag.ProJname = PaymentDetails.GetProjName().Select(r => new SelectListItem
            {
                Text = r.ProjectName,
                Value = r.ProjectName
            });
            return View();
        }
        /// <summary>
        /// 根据输入部门名称、学生名称查询数据
        /// </summary>
        /// <param name="search"></param>
        /// <param name="ProJName">部门名称</param>
        /// <param name="StuName">学生名称</param>
        /// <returns></returns>
        public DeluxeJsonResult QueryPageData(SearchModel<PayRequest> search)
        {
            int total = 0;
            HPITMemberInfo currentUser = DeluxeUser.CurrentMember;
            search.UserName = currentUser.RealName;
            search.RoleName = currentUser.FullName;
            var result = PaymentDetails.GetPageData(search, out total).Select(r => new
            {
                r.PID,
                r.DormNo,
                r.StuName,
                r.StuNo,
                r.NeedPayMoney,
                r.CurrentPayMoney,
                r.RealPayMoney,
                r.Remark,
                r.StatusStr,
                r.DepositMoney,
                r.RentMoney,
                r.ProjectName,
                r.PeriodMonth,
                r.UpdateTime,
                r.PayType,
                JoinTime =  ((DateTime)r.JoinTime).ToString("yyyy-MM-dd"),
                LeaveTime = r.LeaveTime == null ? "" : ((DateTime)r.LeaveTime).ToString("yyyy-MM-dd"),
                r.PayTypeString,
                r.PEM,
                r.PRM,
                r.School,
                r.TotalPayMoney,
                r.Phone,
                r.Sex,
                r.Batch,
                DormSize = string.Format(r.DormSize+"人间"),
                OweMoney = r.NeedPayMoney - r.TotalPayMoney,
                r.DeductMoney,
                r.DeductRemark,
                r.RefundMoney,
                r.RefundRemark
            });
            var totalPages = total % search.PageSize == 0 ? total / search.PageSize : total / search.PageSize + 1;
            return new DeluxeJsonResult(new { Data = result, Total = total, TotalPages = totalPages });
        }

        /// <summary>
        /// 根据选择条件 导出数据到桌面
        /// </summary>
        /// <param name="search"></param>
        /// <param name="ProJName">部门名称</param>
        /// <param name="StuName">学生名称</param>
        /// <param name="name">导出文本的名称</param>
        /// <returns></returns>
        [AllowAnonymous]
        public FileStreamResult ExcelExport(string ProJName, string StuName, string Filname)
        {
            //指定项目存放的路径
            //var rowNum = 2; // rowNum 1 is head
            ////把项目名加到指定存放的路径
            //FileInfo file = new FileInfo(Path.Combine(sWebRootFolder, sFileName));
            //using (ExcelPackage package = new ExcelPackage(file))
            //{
            //    //添加worksheet的名字
            //    ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("公寓缴费详情");
            //    //添加表头名字
            //    worksheet.Cells[1, 1].Value = "公寓编号";
            //    worksheet.Cells[1, 2].Value = "学生姓名";
            //    worksheet.Cells[1, 3].Value = "学生学号";
            //    worksheet.Cells[1, 4].Value = "项目部";
            //    worksheet.Cells[1, 5].Value = "应缴费金额";
            //    worksheet.Cells[1, 6].Value = "押金";
            //    worksheet.Cells[1, 7].Value = "月租金";
            //    worksheet.Cells[1, 8].Value = "培训周期";
            //    worksheet.Cells[1, 9].Value = "累计缴费金额";
            //    worksheet.Cells[1, 10].Value = "离宿日期";
            //    worksheet.Cells[1, 11].Value = "缴费类型";
            //    worksheet.Cells[1, 12].Value = "入住日期";
            //    worksheet.Cells[1, 13].Value = "人事经理";
            //    worksheet.Cells[1, 14].Value = "项目经理";
            //    worksheet.Cells[1, 15].Value = "学校";
            //    //添加值
            //    PaymentDetails details = new PaymentDetails();
            //    var count = PaymentDetails.GetList(ProJName, StuName);
            //    var Homework = from text in count select text;
            //    //循环数据库输出数据
            //    // Homework = Homework.OrderBy(s => s.S_Number);
            //    foreach (var message in Homework)
            //    {
            //        worksheet.Cells["A" + rowNum].Value = message.DormNo;
            //        worksheet.Cells["B" + rowNum].Value = message.StuName;
            //        worksheet.Cells["C" + rowNum].Value = message.StuNo;
            //        worksheet.Cells["D" + rowNum].Value = message.ProjectName;
            //        worksheet.Cells["E" + rowNum].Value = message.NeedPayMoney;
            //        worksheet.Cells["F" + rowNum].Value = message.DepositMoney;
            //        worksheet.Cells["G" + rowNum].Value = message.RentMoney;
            //        worksheet.Cells["H" + rowNum].Value = message.PeriodMonth;
            //        worksheet.Cells["I" + rowNum].Value = message.TotalPayMoney;
            //        worksheet.Cells["J" + rowNum].Value = message.UpdateTime.ToString();
            //        worksheet.Cells["K" + rowNum].Value = message.PayTypeString;
            //        worksheet.Cells["L" + rowNum].Value = message.JoinTime.ToString();
            //        worksheet.Cells["M" + rowNum].Value = message.PEM;
            //        worksheet.Cells["N" + rowNum].Value = message.PRM;
            //        worksheet.Cells["O" + rowNum].Value = message.School;
            //        rowNum++;
            //    }
            //    package.Save();
            //}
            List<PaymentDetailModel> dataList = PaymentDetails.GetList(ProJName, StuName);
            var temp = dataList.ExportToExcelBytes(); //导出为byte[]
            var path =  Server.MapPath("~/Export/");//路径
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            var exportTitle = "导出文件";
            var exportFileName = exportTitle + DateTime.Now.Ticks + ".xlsx";
            var filePath = Path.Combine(path, exportFileName);
            FileInfo file = new FileInfo(filePath);
            System.IO.File.WriteAllBytes(file.FullName, temp);
            Thread.Sleep(2000);
            //return new FileContentResult(temp, "application/ms-excel");
            return File(new FileStream(filePath, FileMode.Open), "text/plain", 
                exportFileName);
        }
    }
}