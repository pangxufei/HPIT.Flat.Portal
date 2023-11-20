using ExcelCake.Intrusive;
using HPIT.Data.Core;
using HPIT.Evalute.Data;
using HPIT.Evalute.Data.Model;
using HPIT.Flat.Data.Adapter;
using HPIT.Flat.Data.Adapters;
using HPIT.Flat.Data.Entitys;
using HPIT.Flat.Data.ExtEntitys;
using HPIT.Flat.Portal.Common;
using HPIT.Flat.Portal.Models;
using HPIT.Web.Core.Deluxe;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HPIT.Flat.Portal.Controllers
{
    public class RoomFeedBackController : Controller
    {
        [AllowAnonymous]
        // GET: RoomFeedBack
        public ActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        public DeluxeJsonResult DateStatistic(int year,int month)
        {
            var result = RoomFeedBackDal.Instance.GetGuestStatistics(year, month);
            return new DeluxeJsonResult(new { Data = result, state = result });
        }

        [AllowAnonymous]
        public DeluxeJsonResult TypeRemarkStatistic(string type,int year,int month)
        {
            var result = RoomFeedBackDal.Instance.TypeRemarkStatistic(type,year,month);
            return new DeluxeJsonResult(new { Data = result, state = result });
        }

        [AllowAnonymous]
        public FileStreamResult ExcelExport(int year ,int month)
        {
            Dictionary<string, IEnumerable<ExcelBase>> excelSheets = new Dictionary<string, IEnumerable<ExcelBase>>();
            List<GuestStatistic> first = RoomFeedBackDal.Instance.GetGuestStatistics(year,month);
            List<GuestTypeStatistic> second = RoomFeedBackDal.Instance.TypeRemarkStatistic("建议",year,month);
            List<GuestTypeStatistic> third = RoomFeedBackDal.Instance.TypeRemarkStatistic("意见",year,month);
            excelSheets.Add("sheet1", first);
            excelSheets.Add("建议统计", second);
            excelSheets.Add("意见统计", third);

            var temp = excelSheets.ExportMultiToBytes(); //导出为byte[]

            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Export");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            var exportTitle = "导出文件";
            var filePath = Path.Combine(path, exportTitle + DateTime.Now.Ticks + ".xlsx");
            FileInfo file = new FileInfo(filePath);
            System.IO.File.WriteAllBytes(file.FullName, temp);
            //return new FileContentResult(temp, "application/ms-excel");
            return File(new FileStream(filePath, FileMode.Open), "text/plain", "宾客入住建议统计"+DateTime.Now.ToShortDateString()+".xlsx");
        }
    }
}