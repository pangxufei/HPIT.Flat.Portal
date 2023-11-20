using HPIT.Data.Core;
using HPIT.Evalute.Data;
using HPIT.Evalute.Data.Model;
using HPIT.Flat.Data.Adapters;
using HPIT.Flat.Data.Entitys;
using HPIT.Flat.Portal.Common;
using HPIT.Flat.Portal.Models;
using HPIT.Web.Core.Deluxe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HPIT.Flat.Portal.Controllers
{
    public class DormController : Controller
    {
        // GET: Dorm
        public ActionResult Index()
        {
            return View();
        }

        public DeluxeJsonResult QueryPageData(SearchModel<Dorm> search)
        {
            int total = 0;
            HPITMemberInfo currentUser = DeluxeUser.CurrentMember;
            search.UserName = currentUser.RealName;
            search.RoleName = currentUser.FullName;
            search.SexType = currentUser.SexType;
            //search.
            var result = DormDal.Instance.GetPageData(search, out total);
            var totalPages = total % search.PageSize == 0 ? total / search.PageSize : total / search.PageSize + 1;
            return new DeluxeJsonResult(new { Data = result, Total = total, TotalPages = totalPages });
        }

        public DeluxeJsonResult QueryBatchs()
        {
            List<string> batchs = EvaluteDal.Instance.GetBatchs();
            return new DeluxeJsonResult(new { Data = batchs });
        }

        public DeluxeJsonResult QueryProjectNames()
        {
            List<string> projectNames = EvaluteDal.Instance.GetProjectNames();
            return new DeluxeJsonResult(new { Data = projectNames });
        }

        public DeluxeJsonResult Save(Dorm dorm)
        {
            int result = 0;
            if (string.IsNullOrEmpty(dorm.DID))
            {
                result = DormDal.Instance.AddDorm(dorm);
            }
            else
            {
                result = DormDal.Instance.UpdateDorm(dorm);
            }
            return new DeluxeJsonResult(new { Data = result, State = 200 });
        }

        public DeluxeJsonResult DormAssign(AssignModel assign, string DID)
        {
            var result = "";
            result = DormDal.Instance.DormAssign(assign.DormAssigns, DID);
            return new DeluxeJsonResult(new { Data = result, State = 200 });
        }

        public DeluxeJsonResult QueryStudentByBatch(string batch, string direction, string projectName, string sex)
        {
            List<EvalStudent> students = EvaluteDal.Instance.GetStudentsByBatchs(batch, direction, projectName, sex);
            List<DormAssign> assigns = DormDal.Instance.AllAssigns();
            List<EvalStudent> exceptStudents = new List<EvalStudent>();
            foreach (EvalStudent stu in students)
            {
                if (!assigns.Any(r => r.StuName == stu.Name))
                {
                    exceptStudents.Add(stu);
                }
            }
            return new DeluxeJsonResult(new { Data = exceptStudents });
        }
        public DeluxeJsonResult DormDelete(string ID, string state)
        {
            var num = 0;
            num = DormDal.Instance.DormDelete(ID);
            return new DeluxeJsonResult(new { Data = num });
        }
        /// <summary>
        /// 宿舍人员详情
        /// </summary>
        /// <returns></returns>
        public DeluxeJsonResult Details(string DId)
        {
            var c = DormDal.Instance.dormAssignsList(DId);
            var list = DormDal.Instance.dormsList();
            return new DeluxeJsonResult(new { Data = c, List = list });
        }
        /// <summary>
        /// 公寓宿舍列表
        /// </summary>
        /// <returns></returns>
        public DeluxeJsonResult DormSelectList()
        {
            var list = DormDal.Instance.dormsList();
            return new DeluxeJsonResult(list);
        }
        /// <summary>
        /// 移除宿舍内人员信息
        /// </summary>
        /// <param name="AID"></param>
        /// <returns></returns>
        [HttpPost]
        public DeluxeJsonResult DormAssignDelete(string AID)
        {
            var num = 0;
            if (DormDal.Instance.DormAssignDelete(AID) > 0)
            {
                num = 1;
            }
            return new DeluxeJsonResult(new { Data = num, State = 200 });
        }
        /// <summary>
        /// 修改宿舍详情信息
        /// </summary>
        /// <param name="StuNo"></param>
        /// <param name="DormNo"></param>
        /// <returns></returns>
        public DeluxeJsonResult DormAssingnUpdate(string StuNo, string DormNo)
        {
            var num = DormDal.Instance.DormAssingnUpdate(StuNo, DormNo);
            return new DeluxeJsonResult(new { state = num });
        }


        public ActionResult RoomFeedBack()
        {
            return View();
        }

    }
}