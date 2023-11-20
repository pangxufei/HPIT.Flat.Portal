using HPIT.Data.Core;
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

namespace HPIT.Flat.Portal.Controllers
{
    public class RetirementController : Controller
    {
        // GET: Retirement
        public ActionResult RetireIndex()
        {
            return View("RetireIndex");
        }

        [HttpPost]
        public DeluxeJsonResult DormRetire(string name, string no, string remark = "")
        {
            var result = RetirementDAL.Instance.Retire(name, no, remark);
            DeluxeJsonResult json = new DeluxeJsonResult();
            json.Data = new { Data = result, Status = result > 0 ? 200 : 300 };
            return json;
        }

        public DeluxeJsonResult QueryPageData(SearchModel<DormAssign> search)
        {
            int total = 0;
            HPITMemberInfo currentUser = DeluxeUser.CurrentMember;
            search.UserName = currentUser.RealName;
            search.RoleName = currentUser.FullName;
            var result = RetirementDAL.Instance.GetPageData(search, out total);
            var totalPages = total % search.PageSize == 0 ? total / search.PageSize : total / search.PageSize + 1;
            return new DeluxeJsonResult(new { Data = result, Total = total, TotalPages = totalPages });
        }

        public JsonResult update(string Remark, string DormNo)
        {

            Dorm dorm = new Dorm()
            {
                Remark = Remark,
                DormNo = DormNo

            };
            int result = RetirementDAL.save(dorm);
            return Json(result);
        }
        /// <summary>
        /// 退宿列表
        /// </summary>
        /// <returns></returns>
        public ActionResult RetirementIndex()
        {
            return View();
        }
        public JsonResult ReDorm()
        {
            List<Dorm> list = RetirementDAL.dormlist();
            JsonResult json = new JsonResult();
            json.Data = new { Data = list };
            return json;
        }
    }
}