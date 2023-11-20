using HPIT.Data.Core;
using HPIT.Evalute.Data;
using HPIT.Evalute.Data.Model;
using HPIT.Flat.Data;
using HPIT.Flat.Data.Adapters;
using HPIT.Flat.Data.Entitys;
using HPIT.Flat.Portal.Common;
using HPIT.Web.Core.Deluxe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HPIT.Flat.Portal.Controllers
{
    public class PaymentStatisticsController : Controller
    {

        // GET: PaymentStatisticsDal
        public ActionResult PaymentStatistics()
        {
            ViewBag.Batchs = EvaluteDal.Instance.GetBatchs().Select(r=> new GeneralSelectItem() { Text = r,Value = r});
            return View();
        }
        /// <summary>
        /// 获取查询数据
        /// </summary>
        /// <param name="search"></param>
        /// <param name="ProJName">项目部名称</param>
        /// <returns></returns>
        public DeluxeJsonResult QueryPageData(SearchModel<PayRequest> search, string ProJName)
        {
            int total = 0;
            var result = PaymentStatisticsDal.GetPageData(search, out total, ProJName).Select(r => new
            {
                r.DormNo,
                r.StuName,
                r.StuNo,
                r.NeedPayMoney,
                r.CurrentPayMoney,
                r.RealPayMoney,
                r.Remark,
                r.StatusStr,
                r.PID,
                r.DepositMoney,
                r.RentMoney,
                r.ProjectName,
                r.PeriodMonth,
                r.UpdateTime,
                r.PayType,
                r.JoinTime,
                r.PayTypeString,
                r.TotalPayMoney
            });
            var totalPages = total % search.PageSize == 0 ? total / search.PageSize : total / search.PageSize + 1;
            return new DeluxeJsonResult(new { Data = result, Total = total, TotalPages = totalPages });
        }
        /// <summary>
        /// 获取项目部/人数
        /// </summary>
        /// <returns></returns>
        public JsonResult GetPayStatis(string batch)
        {
            HPITMemberInfo currentUser = DeluxeUser.CurrentMember;
            var coun = PaymentStatisticsDal.PayStatis(batch,currentUser);
            return Json(coun, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 根据项目部获取积累缴费人数
        /// </summary>
        /// <param name="ProName"></param>
        /// <returns></returns>
        public JsonResult GetPayProNum(string ProName)
        {
            var coun = PaymentStatisticsDal.PayStuNum(ProName);
            return Json(coun, JsonRequestBehavior.AllowGet);
        }

    }
}