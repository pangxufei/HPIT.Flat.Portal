using HPIT.Data.Core;
using HPIT.Flat.Data.Entitys;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using static HPIT.Flat.Data.Entitys.Enumerations;

namespace HPIT.Flat.Data.Adapters
{
  public  class RolesDal
    {
        public static RolesDal Instance = new RolesDal();
        public FlatContext context { get; set; }
        public RolesDal()
        {
            this.context = new FlatContext();
        }
        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="search"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public List<Roles> GetPageData(SearchModel<Roles> search, out int count)
        {
            GetPageListParameter<Roles, string> parameter = new GetPageListParameter<Roles, string>();
            parameter.isAsc = true;
            parameter.orderByLambda = t => t.RoleName;
            parameter.pageIndex = search.PageIndex;
            parameter.pageSize = search.PageSize;
            parameter.whereLambda = t => t.Status > -1;
            //查询数据
            DBBaseService baseService = new DBBaseService(FlatContext.Instance);
            List<Roles> list = baseService.GetSimplePagedData<Roles, string>(parameter, out count);
            foreach (var role in list)
            {
                role.StatusStr = EnumDescriptionAttribute.GetDescription((RoleStatus)role.Status);
                //role.RoleJsonStr = JsonConvert.SerializeObject(role);
            }

            return list;
        }

        /// <summary>
        /// 根据主键
        /// </summary>
        /// <param name="RoleID"></param>
        /// <returns></returns>
        public int GetDormByNo(string RoleName)
        {
            var role = context.Roles.Where(r => r.RoleName == RoleName).Count();
            return role;
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        public int AddRole(Roles role)
        {
            ///添加地址和公寓的编号判断进行去重
            int num = 0;
            var a = GetDormByNo(role.RoleName);
            if (a != 0)
            {
                    num = 44;
                
            }
            else
            {
                //role.Status = (int)RoleStatus.stope;                
                role.CreateTime = DateTime.Now;
                //role.RoleID = Convert.ToInt32(Guid.NewGuid().ToString());
                context.Roles.Add(role);
                num = context.SaveChanges();
            }
            return num;

        }

        public List<Roles> idselect(int id) {
             var role= context.Roles.Where(r => r.RoleID == id).ToList();
            return role;
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="dorm"></param>
        /// <returns></returns>
        public int UpdateRole(Roles role)
        {
            int result = 0;
            Roles match = context.Roles.FirstOrDefault(r => r.RoleID == role.RoleID);
            if (match != null)
            {
                match.RoleID = role.RoleID;
                match.RoleDesc = role.RoleDesc;                
                match.RoleName = role.RoleName;
                match.AlterTime = role.AlterTime;
                match.Status = role.Status;
                //match.CreateTime = role.CreateTime;
                context.Entry(match).State = System.Data.Entity.EntityState.Modified;
                result = context.SaveChanges();
            }
            return result;
        }

        //详情
        public List<Roles> RoleAssignsList(int id)
        {
            var role = context.Roles.Where(p => p.RoleID == id).ToList();
            return role;
        }

        /// <summary>
        /// 删除信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int RoleDelete(int id)
        {
            var role = context.Roles.FirstOrDefault(p => p.RoleID == id);
            //var roleA = context.Roles.Where(p => p.RoleID == id);
            //foreach (var item in roleA)
            //{
            //    context.Roles.Remove(item);
            //}
            context.Roles.Remove(role);
            return context.SaveChanges();
        }

    }
}
