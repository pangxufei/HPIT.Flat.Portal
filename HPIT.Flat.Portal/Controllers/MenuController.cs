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
    public class MenuController : Controller
    {
        // GET: Menu
        public ActionResult Index()
        {
            return View();
        }
        //查询
        public DeluxeJsonResult QueryPageData(SearchModel<Menus> search)
        {
            int total = 0;
            HPITMemberInfo currentUser = DeluxeUser.CurrentMember;
            search.UserName = currentUser.RealName;
            search.RoleName = currentUser.FullName;
            //search.查询
            var result = MenuDal.Instance.GetPageData(search, out total).Select(r => new
            {
                r.MenuID,
                r.Status,
                r.MenuName,
                r.MenuType,
                r.ParentID,
                r.Sort,
                r.MenuCode,
                r.MenuUrl,
            });
            var totalPages = total % search.PageSize == 0 ? total / search.PageSize : total / search.PageSize + 1;
            return new DeluxeJsonResult(new { Data = result, Total = total, TotalPages = totalPages });
        }
        /// <summary>
        /// id修改
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public DeluxeJsonResult IdSelect(int id)
        {
            var a = MenuDal.Instance.IdSelect(id);
            return new DeluxeJsonResult(new { Data = a }); ;
        }


        /// <summary>
        /// 添加  修改
        /// </summary>
        /// <param name="menu"></param>
        /// <returns></returns>
        public DeluxeJsonResult Save(Menus menu)
        {
            int result = 0;
            var jie = MenuDal.Instance.GetDormByNo(menu.MenuName);
            if (jie ==0)
            {
                result = MenuDal.Instance.AddMenu(menu);
            }
            else
            {
                result = MenuDal.Instance.UpdateMenu(menu);
            }
            return new DeluxeJsonResult(new { Data = result, State = 200 });
        }

        /// <summary>
        /// 详情视图
        /// </summary>
        /// <returns></returns>
        public DeluxeJsonResult Details(int MenuID)
        {
            var c = MenuDal.Instance.MenuAssignsList(MenuID);
            return new DeluxeJsonResult(new { Data = c });
        }
        //删除
        public DeluxeJsonResult MenuDelete(int ID, int state)
        {
            var num = 0;
            if (state == 0)
            {
                num = MenuDal.Instance.MenuDelete(ID);
            }
            return new DeluxeJsonResult(new { Data = num });
        }
    }
}