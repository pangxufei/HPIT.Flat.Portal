using HPIT.Data.Core;
using HPIT.Flat.Data.Entitys;
using HPIT.Flat.Data.ExtEntitys;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static HPIT.Flat.Data.Entitys.Enumerations;

namespace HPIT.Flat.Data.Adapter
{
    public class MenuDal
    {
        public static MenuDal Instance = new MenuDal();
        public static DapperDBHelper DbTool = new DapperDBHelper("SqlDBAddress");
        public FlatContext context { get; set; }

        public MenuDal() {
            this.context = new FlatContext();
        }

        public List<MenuExt> GetMenusByRoleName(string roleName)
        {
            List<MenuExt> meuns = new List<MenuExt>();
            string sql = string.Format(@" select * from ( select m.*,r.RoleName,r.RoleID from RoleMenus rm 
                                          left join Menus m on rm.MenuID = m.MenuID 
                                          left join Roles r on rm.RoleID = r.RoleID) as t where t.RoleName = '{0}'", roleName);
            using (var context = new FlatContext())
            {
                meuns = context.Database.SqlQuery<MenuExt>(sql).ToList();
            }
            List<MenuExt> first = meuns.Where(r => r.MenuType == "一级菜单").ToList();
            List<MenuExt> second = meuns.Where(r => r.MenuType == "二级菜单").ToList();
            foreach (MenuExt menu in first)
            {
                foreach (MenuExt item in second)
                {
                    if (item.ParentID == menu.MenuID)
                    {
                        menu.Children.Add(item);
                    }
                }
            }
            return first;
        }

        public List<MenuExt> GetMenuByBatchs( string MenuType )
        {
            List<MenuExt> meuns = new List<MenuExt>();
            string sql = @"select * from ( select m.*,r.RoleName,r.RoleID from RoleMenus rm 
                                          left join Menus m on rm.MenuID = m.MenuID 
                                          left join Roles r on rm.RoleID = r.RoleID) as t where t.MenuType = @MenuType";
            if (MenuType == "一级菜单")
            {
                meuns = context.Database.SqlQuery<MenuExt>(sql).ToList();
            }
            else {
                meuns = context.Database.SqlQuery<MenuExt>(sql).ToList();
            }
          
            MenuExt stu = new MenuExt();
          
            stu.MenuType = MenuType;
           
            List<MenuExt> result = DbTool.ExcuteQuery<MenuExt>(sql, stu).OrderBy(r => r.MenuName).ToList();
            return result;
        }



        //一级
        public List<string> GetBatchs()
        {
            List<MenuExt> meuns = new List<MenuExt>();
            string sql = string.Format(@" select * from ( select m.*,r.RoleName,r.RoleID from RoleMenus rm 
                                          left join Menus m on rm.MenuID = m.MenuID 
                                          left join Roles r on rm.RoleID = r.RoleID) as t ");
            using (var context = new FlatContext())
            {
                meuns = context.Database.SqlQuery<MenuExt>(sql).ToList();
            }
            List<MenuExt> first = meuns.Where(r => r.MenuType == "一级菜单").ToList();
            List<MenuExt> second = meuns.Where(r => r.MenuType == "二级菜单").ToList();
            foreach (MenuExt menu in first)
            {
                foreach (MenuExt item in second)
                {
                    if (item.ParentID == menu.MenuID)
                    {
                        menu.Children.Add(item);
                    }
                }
            }
            List<string> data = first.Where(r => !string.IsNullOrEmpty(r.MenuName)).Select(r => r.MenuName).Distinct().ToList();
            return data;

        }

        //二级
        public List<string> GetBatchssecond()
        {
            List<MenuExt> meuns = new List<MenuExt>();
            string sql = string.Format(@" select * from ( select m.*,r.RoleName,r.RoleID from RoleMenus rm 
                                          left join Menus m on rm.MenuID = m.MenuID 
                                          left join Roles r on rm.RoleID = r.RoleID) as t ");
            using (var context = new FlatContext())
            {
                meuns = context.Database.SqlQuery<MenuExt>(sql).ToList();
            }
            List<MenuExt> first = meuns.Where(r => r.MenuType == "一级菜单").ToList();
            List<MenuExt> second = meuns.Where(r => r.MenuType == "二级菜单").ToList();
            foreach (MenuExt menu in first)
            {
                foreach (MenuExt item in second)
                {
                    if (item.ParentID == menu.MenuID)
                    {
                        menu.Children.Add(item);
                    }
                }
            }
            List<string> data = second.Where(r => !string.IsNullOrEmpty(r.MenuName)).Select(r => r.MenuName).Distinct().ToList();
            return data;

        }
        //查询

        public List<Menus> GetPageData(SearchModel<Menus> search, out int count)
        {
            GetPageListParameter<Menus, string> parameter = new GetPageListParameter<Menus, string>();
            parameter.isAsc = true;
            parameter.orderByLambda = t => t.MenuName;
            parameter.pageIndex = search.PageIndex;
            parameter.pageSize = search.PageSize;
            parameter.whereLambda = t => t.Status > -1;
            //查询数据
            DBBaseService baseService = new DBBaseService(FlatContext.Instance);
            List<Menus> list = baseService.GetSimplePagedData<Menus, string>(parameter, out count);
            //foreach (var dorm in list)
            //{
            //    dorm.MenuJsonStr = JsonConvert.SerializeObject(dorm);
            //}
            return list;
        }
        public int GetDormByNo(string MenuName)
        {
           
            var role = context.Menus.Where(r => r.MenuName == MenuName).Count();
            return role;
        }

        //添加

        public int AddMenu(Menus menu)
        {
            ///添加角色菜单判断进行去重
            int num = 0;
            var a = GetDormByNo(menu.MenuName);
            Menus match = context.Menus.FirstOrDefault(r => r.MenuID == menu.MenuID);
            if (a!=0)
            {    
                    num = 44;

            }
            else
            {
              
                context.Menus.Add(menu);
                num = context.SaveChanges();
            }
            return num;

        }

        //id查询
        public List<Menus> IdSelect(int id)
        {
            var Menu = context.Menus.Where(r => r.MenuID == id).ToList();
            return Menu;
        }

        //修改
        public int UpdateMenu(Menus menu)
        {
            int result = 0;
            Menus match = context.Menus.FirstOrDefault(r => r.MenuID == menu.MenuID);
            if (match != null)
            {

                match.MenuCode = menu.MenuCode;
                match.MenuName = menu.MenuName;
                match.MenuType = menu.MenuType;
                match.MenuUrl = menu.MenuUrl;
                match.ParentID = menu.ParentID;
                match.Sort = menu.Sort;
                match.Status = menu.Status;

                context.Entry(match).State = System.Data.Entity.EntityState.Modified;
                result = context.SaveChanges();
            }
            return result;
        }

        //详情
        public List<Menus> MenuAssignsList(int id)
        {
            var menu = context.Menus.Where(p => p.MenuID == id).ToList();
            return menu;
        }

        /// <summary>
        /// 删除信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int MenuDelete(int id)
        {
            var menu = context.Menus.FirstOrDefault(p => p.MenuID == id);
            var menuA = context.RoleMenus.Where(p => p.ID == id);
            foreach (var item in menuA)
            {
                context.RoleMenus.Remove(item);
            }
            context.Menus.Remove(menu);
            return context.SaveChanges();
        }

    }
}
