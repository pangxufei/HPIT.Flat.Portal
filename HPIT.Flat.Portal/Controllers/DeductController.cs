using HPIT.Flat.Data.Adapters;
using HPIT.Web.Core.Deluxe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HPIT.Flat.Portal.Controllers
{
    public class DeductController : Controller
    {
        // GET: Deduct
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 扣款操作
        /// </summary>
        /// <param name="requestID"></param>
        /// <param name="remark"></param>
        /// <param name="deductMoney"></param>
        /// <returns></returns>
        public DeluxeJsonResult DoDeductMoney(string requestID,string remark,string deductMoney)
        {
            int result = PayRequestDal.Instance.DoDeductMoney(requestID,Convert.ToDecimal(deductMoney),remark);
            if (result > 0)
            {
                return new DeluxeJsonResult(new { Data = "扣款成功", State = 200 });
            }
            else
            {
                return new DeluxeJsonResult(new { Data = "扣款失败", State = 500 });
            }
            
        }

        public DeluxeJsonResult DoRefundMoney(string requestID, string remark, string refundMoney)
        {
            int result = PayRequestDal.Instance.DoRefundMoney(requestID, Convert.ToDecimal(refundMoney), remark);
            if (result > 0)
            {
                return new DeluxeJsonResult(new { Data = "退款成功", State = 200 });
            }
            else
            {
                return new DeluxeJsonResult(new { Data = "退款失败", State = 500 });
            }

        }
    }
}