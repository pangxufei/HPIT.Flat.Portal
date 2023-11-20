using HPIT.Data.Core;
using HPIT.Data.Core.Tool;
using HPIT.Flat.Data.Entitys;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using static HPIT.Flat.Data.Entitys.Enumerations;

namespace HPIT.Flat.Data.Adapters
{
    public class RetirementDAL
    {

        public static RetirementDAL Instance = new RetirementDAL();

        public FlatContext context { get; set; }
        public RetirementDAL()
        {
            this.context = new FlatContext();
        }
        /// <summary>
        /// 全表查询
        /// </summary>
        /// <returns></returns>
        public static List<RetiremenModel> selectlist(string DormNo="")
        {
            FlatContext db = new FlatContext();
            List<RetiremenModel> list = (from cc in db.Dorm
                                         join aa in db.DormAssign
                                         on cc.DID equals aa.DID
                                         
                                         select new RetiremenModel
                                         {
                                             Dorm = cc,
                                             DormAssign = aa,
                                             
                                         }).Where(p=>p.Dorm.DormNo.Contains(DormNo)).ToList();
            return list;

        }

        /// <summary>
        /// 查询住宿信息列表
        /// </summary>
        /// <param name="search"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public List<DormAssign> GetPageData(SearchModel<DormAssign> search, out int count)
        {
            GetPageListParameter<DormAssign, string> parameter = new GetPageListParameter<DormAssign, string>();
            parameter.isAsc = true;
            parameter.orderByLambda = t => t.DormNo;
            parameter.pageIndex = search.PageIndex;
            parameter.pageSize = search.PageSize;
            parameter.whereLambda = t => t.Status > -1;
            //查询数据
            if (!string.IsNullOrEmpty(search.Entity.ProjectName))
            {
                Expression<Func<DormAssign, bool>> projectNameWhere = item => item.ProjectName == search.Entity.ProjectName;
                parameter.whereLambda = ExpressionExt.ReBuildExpression<DormAssign>(parameter.whereLambda, projectNameWhere);
            }
            if (search.Entity.Status > 0)
            {
                Expression<Func<DormAssign, bool>> statusWhere = item => item.Status == search.Entity.Status;
                parameter.whereLambda = ExpressionExt.ReBuildExpression<DormAssign>(parameter.whereLambda, statusWhere);
            }
            DBBaseService baseService = new DBBaseService(FlatContext.Instance);
            List<DormAssign> list = baseService.GetSimplePagedData<DormAssign, string>(parameter, out count);
            foreach (var dorm in list)
            {
                dorm.StatusStr = EnumDescriptionAttribute.GetDescription((LiveStatus)dorm.Status);
            }
            return list;
        }

        public static Dorm Get(string id)
        {
            FlatContext db = new FlatContext();
            var data = db.Dorm.Where(r => r.DID == id);
            if (data.Count() > 0 || data != null)
            {
                return data.First();
            }
            return null;
        }

        public static DormAssign Getdorm(string id)
        {
            FlatContext db = new FlatContext();
            var data = db.DormAssign.Where(r => r.StuName == id);
            if (data.Count() > 0 || data != null)
            {
                return data.First();
            }
            return null;
        }

        //public static bool Retirementupdate(Dorm model, DormAssign mdeol1)
        //{
        //    FlatContext db = new FlatContext();

        //    var dormassign = Getdorm(mdeol1.StuName);

        //    if (dormassign != null)
        //    {
        //        dormassign.LeaveTime = mdeol1.LeaveTime;
        //        dormassign.Phone = mdeol1.Phone;

        //        dormassign.Status = mdeol1.Status;
        //        dormassign.StuName = mdeol1.StuName;
        //        db.Entry(dormassign).State = System.Data.Entity.EntityState.Modified;

        //        if (db.SaveChanges() > 0)
        //        {
        //            sava(model);
        //        }


        //    }
        //    return false ;

        //}

        
        public static int save(Dorm model)
        {
            FlatContext db = new FlatContext();
            string sql = string.Format(@"update Dorm set Remark ='" + model.Remark + "',Status=0 where DormNo='" + model.DormNo + "'");
            int result = (int)db.Database.ExecuteSqlCommand(sql);
            return result;
        }

        public int Retire(string name,string no,string remark)
        {
            FlatContext db = new FlatContext();
            string sql = string.Format(@"update DormAssign set Remark ='{0}',Status=2,LeaveTime = '{1}' where StuName='{2}' and StuNo='{3}'",remark,DateTime.Now,name,no);
            int result = (int)db.Database.ExecuteSqlCommand(sql);
            return result;
        }

        /// <summary>
        /// dorm全表查询
        /// </summary>
        /// <returns></returns>
        public static List<Dorm> dormlist()
        {
            FlatContext model = new FlatContext();
            List<Dorm> list = model.Dorm.ToList();
            return list;
        }

    }
}
