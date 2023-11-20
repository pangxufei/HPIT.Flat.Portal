using HPIT.Data.Core;
using HPIT.Evalute.Data;
using HPIT.Evalute.Data.Model;
using HPIT.Flat.Data.Adapters;
using HPIT.Flat.Data.Entitys;
using HPIT.Flat.Portal.Common;
using HPIT.Web.Core.Deluxe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static HPIT.Flat.Data.Entitys.Enumerations;

namespace HPIT.Flat.Portal.Controllers
{
    public class RefundController : Controller
    {
        // GET: Refund
        public ActionResult Index()
        {
            return View();
        }
        public DeluxeJsonResult QueryPageData(SearchModel<PayRequest> search)
        {
            int total = 0;
            HPITMemberInfo currentUser = DeluxeUser.CurrentMember;
            search.UserName = currentUser.RealName;
            search.RoleName = currentUser.FullName;
            var result = PayRequestDal.Instance.GetRefundPageData(search, out total).Select(r => new
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
                r.RefundMoney,
                CurrentUser = DeluxeUser.CurrentMember
            });
            var totalPages = total % search.PageSize == 0 ? total / search.PageSize : total / search.PageSize + 1;
            return new DeluxeJsonResult(new { Data = result, Total = total, TotalPages = totalPages });
        }

        public DeluxeJsonResult DormChange(string DormNo)
        {
            Dorm dorm = DormDal.Instance.GetDormByNo(DormNo);
            return new DeluxeJsonResult(new { Data = dorm });
        }

        public DeluxeJsonResult QueryPayDetailByRequestID(string requestID)
        {
            var payDetails = PayRequestDal.Instance.QueryPayDetailByRequestID(requestID).ToList().Select(r => new {
                r.Remark,
                r.PayMoney,
                r.StuName,
                r.CreateTime,
                r.AuditStatus,
                AuditStr = EnumDescriptionAttribute.GetDescription((PayRequestStatus)r.AuditStatus)
            });
            return new DeluxeJsonResult(new { Data = payDetails });
        }

        public DeluxeJsonResult PayRequestSave(PayRequest request)
        {
            HPITMemberInfo currentUser = DeluxeUser.CurrentMember;
            request.Operator = currentUser.RealName;
            request.StuName = currentUser.RealName;
            request.StuNo = currentUser.StuNo;
            request.RequestType = (int)RequestType.refund;
            EvalStudent evalStudent = EvaluteDal.Instance.GetMatchStudent(currentUser.RealName);
            bool isExist = PayRequestDal.Instance.CheckHaveRequest(request.StuName, request.StuNo,1);
            if (isExist && string.IsNullOrEmpty(request.PID))
            {
                return new DeluxeJsonResult(new { Data = "您已经发起付款请求，如果付款，点解列表中的付款进行操作！！！", State = 201 });
            }
            request.ProjectName = currentUser.ProjectName;
            request.PeriodMonth = (int)currentUser.Period + 2;
            request.NeedPayMoney = request.RentMoney * (currentUser.Period + 2) + request.DepositMoney;
            request.UpdateTime = DateTime.Now;
            request.CreateTime = DateTime.Now;
            int result = PayRequestDal.Instance.Refund(request, evalStudent.PEM);
            return new DeluxeJsonResult(new { Data = result, State = 200 });
        }
    }
}