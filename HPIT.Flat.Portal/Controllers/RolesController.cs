using HPIT.Data.Core;
using HPIT.Evalute.Data.Model;
using HPIT.Flat.Data.Adapter;
using HPIT.Flat.Data.Adapters;
using HPIT.Flat.Data.Entitys;
using HPIT.Flat.Data.ExtEntitys;
using HPIT.Flat.Portal.Common;
using HPIT.Web.Core.Deluxe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HPIT.Flat.Portal.Controllers
{
    public class RolesController : Controller
    {
        // GET: Roles
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        ///查询
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        public DeluxeJsonResult QueryPageData(SearchModel<Roles> search)
        {
            int total = 0;
            HPITMemberInfo currentUser = DeluxeUser.CurrentMember;
            search.UserName = currentUser.RealName;
            search.RoleName = currentUser.FullName;
            //var result = RolesDal.Instance.GetPageData(search, out total);
            var result = RolesDal.Instance.GetPageData(search, out total).Select(r => new
            {
                r.RoleName,
                r.RoleDesc,
                //RoleDest = string.IsNullOrEmpty(r.RoleDesc) ? "" : r.RoleDesc,
                r.StatusStr,
                r.CreateTime,
                r.AlterTime,
                r.RoleID,
            });
            var totalPages = total % search.PageSize == 0 ? total / search.PageSize : total / search.PageSize + 1;
            return new DeluxeJsonResult(new { Data = result, Total = total, TotalPages = totalPages });
        }

        /// <summary>
        /// 添加 修改
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        public DeluxeJsonResult Save(Roles role)
        {
            int result = 0;
            var jie = RolesDal.Instance.GetDormByNo(role.RoleName);
            if (jie==0)
            {
                result = RolesDal.Instance.AddRole(role);
            }
            else
            {
                result = RolesDal.Instance.UpdateRole(role);
            }
            return new DeluxeJsonResult(new { Data = result, State = 200 });
        }

        public DeluxeJsonResult idselect(int id)
        {
            var a = RolesDal.Instance.idselect(id);
            return new DeluxeJsonResult(new { Data = a }); ;
        }

        /// <summary>
        /// 详情视图
        /// </summary>
        /// <returns></returns>
        public DeluxeJsonResult Details(int RoleID)
        {
            var c = RolesDal.Instance.RoleAssignsList(RoleID);
            return new DeluxeJsonResult(new { Data = c });
        }
        //删除
        public DeluxeJsonResult RoleDelete(int ID, int state)
        {
            var num = 0;
            if (state == 1)
            {
                num = RolesDal.Instance.RoleDelete(ID);
            }
            return new DeluxeJsonResult(new { Data = num });
        }

        //授权1
        public DeluxeJsonResult QueryBatchs()
        {
            List<string> batchs = MenuDal.Instance.GetBatchs();
            return new DeluxeJsonResult(new { Data = batchs });
        }
        //授权2
        public DeluxeJsonResult QueryProjectNames()
        {
            List<string> projectNames = MenuDal.Instance.GetBatchssecond();
            return new DeluxeJsonResult(new { Data = projectNames });
        }
        ////授权显示
        public DeluxeJsonResult QueryRoleByBatch( string MenuType)
        {
            List<MenuExt> students = MenuDal.Instance.GetMenuByBatchs( MenuType);
            return new DeluxeJsonResult(new { Data = students });
        }

    }
}