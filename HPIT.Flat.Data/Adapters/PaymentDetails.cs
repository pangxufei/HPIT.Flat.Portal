using HPIT.Data.Core;
using HPIT.Evalute.Data;
using HPIT.Evalute.Data.Model;
using HPIT.Flat.Data.Entitys;
using HPIT.Flat.Data.ExportEntitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static HPIT.Flat.Data.Entitys.Enumerations;

namespace HPIT.Flat.Data.Adapters
{
    public class proname
    {
        public string ProjectName { get; set; }
    }
    public class PaymentDetails
    {

        public static PaymentDetails Instance = new PaymentDetails();
        public FlatContext context { get; set; }
        public PaymentDetails()
        {
            this.context = new FlatContext();
        }
        /// <summary>
        /// 根据部门、姓名查询
        /// </summary>
        /// <param name="search"></param>
        /// <param name="count"></param>
        /// <param name="ProjectName">部门名称</param>
        /// <param name="StuName">学生姓名</param>
        /// <returns></returns>
        public static List<PaymentDetailModel> GetPageData(SearchModel<PayRequest> search, out int count)
        {
            GetPageListParameter<PaymentDetailModel, string> parameter = new GetPageListParameter<PaymentDetailModel, string>();
            parameter.isAsc = true;
            parameter.orderByLambda = t => t.DormNo;
            parameter.pageIndex = search.PageIndex;
            parameter.pageSize = search.PageSize;
            parameter.whereLambda = t => !string.IsNullOrEmpty(t.PID);
            List<PaymentDetailModel> list = new List<PaymentDetailModel>();
            //查询数据
            DBBaseService baseService = new DBBaseService(FlatContext.Instance);
            string sql = string.Format(@"select * from (select t.*,Case 
                                         When t.TotalPayMoney =0 then 1
                                         When t.NeedPayMoney>t.TotalPayMoney then 2
		 　                              When t.NeedPayMoney=t.TotalPayMoney then 3
                                         end as paystatus,
                                         Case 
                                         When t.Gender =1 then '男'
                                         When t.Gender =0 then '女'
                                         end as Sex
                                         from 
                                                   (select a.PID, a.DepositMoney,a.DormNo,a.ProjectName,a.DeductMoney,a.DeductRemark,a.RefundMoney,a.RefundRemark,
                                                   a.NeedPayMoney,a.PeriodMonth,a.RequestType,a.RentMoney,a.StuName,a.StuNo,
                                                   (select isnull(sum(PayMoney),0) from Payment where PID = a.PID and AuditStatus = 3) as TotalPayMoney,
                                                   (select Top 1 PayType from Payment where PID = a.PID) as PayType,
                                                   c.PEM,c.PRM,c.School,c.JoinTime,c.LeaveTime,
                                                   c.Batch,c.Direction,d.DormSize,d.DormType,c.Gender,c.Phone from PayRequest a
                                                   left join DormAssign c on a.StuName = c.StuName 
                                                   left join Dorm d on d.DID = c.DID
                                                   where a.RequestType = 0 ) 
                                                   t ) k where 1=1 ");
            string sqlWhere = "";
            string ProjectName = search.Entity.ProjectName;
            string StuName = search.Entity.StuName;
            int paystatus = search.Entity.PayStatus;
            if (!string.IsNullOrEmpty(ProjectName) || !string.IsNullOrEmpty(StuName))
            {
                if (!string.IsNullOrEmpty(ProjectName))
                {
                    sqlWhere += string.Format(" and k.projectName = '{0}'",ProjectName);
                }
                if (!string.IsNullOrEmpty(StuName))
                {
                    sqlWhere += string.Format(" and k.stuName = '{0}'", StuName);
                }
            }
            if (paystatus > 0)
            {
                sqlWhere += string.Format(" and k.paystatus = {0}", paystatus);
            }
            list = baseService.GetSqlPagedData<PaymentDetailModel, string>(sql+sqlWhere, parameter, out count);
            return list;

        }
        /// <summary>
        /// 获取下拉框项目部Value
        /// </summary>
        /// <returns></returns>
        public static List<proname> GetProjName()
        {
            FlatContext flat = new FlatContext();
            string sql = string.Format(" select  distinct p.ProjectName from PayRequest p ");
            return flat.Database.SqlQuery<proname>(sql).ToList();

        }
        /// <summary>
        /// 导出数据
        /// </summary>
        /// <param name="ProjectName">项目部门</param>
        /// <param name="StuName">学生姓名</param>
        /// <returns></returns>
        public static List<PaymentDetailModel> GetList(string ProjectName, string StuName)
        {

            FlatContext context = new FlatContext();
            string sql = string.Format(@"select * from (select t.*,Case 
                                         When t.TotalPayMoney =0 then 1
                                         When t.NeedPayMoney>t.TotalPayMoney then 2
		 　                              When t.NeedPayMoney=t.TotalPayMoney then 3
                                         end as paystatus,
                                         Case 
                                         When t.Gender =1 then '男'
                                         When t.Gender =0 then '女'
                                         end as Sex
                                         from 
                                                   (select a.PID, a.DepositMoney,a.DormNo,a.ProjectName,a.DeductMoney,a.DeductRemark,,a.RefundMoney,a.RefundRemark,
                                                   a.NeedPayMoney,a.PeriodMonth,a.RequestType,a.RentMoney,a.StuName,a.StuNo,
                                                   (select isnull(sum(PayMoney),0) from Payment where PID = a.PID and AuditStatus = 3) as TotalPayMoney,
                                                   (select Top 1 PayType from Payment where PID = a.PID) as PayType,
                                                   c.PEM,c.PRM,c.School,c.JoinTime,c.LeaveTime,
                                                   c.Batch,c.Direction,d.DormSize,d.DormType,c.Gender,c.Phone
                                                   from PayRequest a
                                                   left join DormAssign c on a.StuName = c.StuName 
                                                   left join Dorm d on d.DID = c.DID
                                                   where a.RequestType = 0 ) 
                                                                           t ) k where 1=1 ");
            List<PaymentDetailModel> list = new List<PaymentDetailModel>();
            //查询数据
            DBBaseService baseService = new DBBaseService(FlatContext.Instance);
            string sqlWhere = "";
            if (!string.IsNullOrEmpty(ProjectName) || !string.IsNullOrEmpty(StuName))
            {
                if (!string.IsNullOrEmpty(ProjectName))
                {
                    sqlWhere += string.Format(" and a.projectName = '{0}'", ProjectName);
                }
                if (!string.IsNullOrEmpty(StuName))
                {
                    sqlWhere += string.Format(" and a.stuName = '{0}'", StuName);
                }
            }
            list = context.Database.SqlQuery<PaymentDetailModel>(sql).ToList();
            foreach (PaymentDetailModel item in list)
            {
                item.OweMoney = (decimal)item.NeedPayMoney - (decimal)item.TotalPayMoney;
            }
            return list;

        }


        public List<ProjectPayStatisticModel> GroupByProjectName()
        {
            FlatContext context = new FlatContext();
            string sql = string.Format(@"select t.Batch,t.ProjectName,t.PEM,t.PRM ,sum(NeedPayMoney) NeedPayMoney,sum(TotalPayMoney) TotalPayMoney from  (select a.PID, a.DepositMoney,a.DormNo,a.ProjectName,a.DeductMoney,a.DeductRemark,
                                                   a.NeedPayMoney,a.PeriodMonth,a.RequestType,a.RentMoney,a.StuName,a.StuNo,
                                                   (select isnull(sum(PayMoney),0) from Payment where PID = a.PID and AuditStatus = 3) as TotalPayMoney,
                                                   (select Top 1 PayType from Payment where PID = a.PID) as PayType,
                                                   c.PEM,c.PRM,c.School,c.JoinTime,c.LeaveTime,
                                                   c.Batch,c.Direction,d.DormSize,d.DormType,c.Gender,c.Phone
                                                   from PayRequest a
                                                   left join DormAssign c on a.StuName = c.StuName 
                                                   left join Dorm d on d.DID = c.DID
                                                   where a.RequestType = 0 ) t group by t.ProjectName,t.PEM,t.PRM,t.Batch");
            List<ProjectPayStatisticModel> list = new List<ProjectPayStatisticModel>();
            DBBaseService baseService = new DBBaseService(FlatContext.Instance);
            list = context.Database.SqlQuery<ProjectPayStatisticModel>(sql).ToList();
            List<ProjectGroupBy> baseList = EvaluteDal.Instance.GroupByProjectName();
            List<ProjectPayStatisticModel> Livelist = LiveProjectNameGroupBy();
            List<ProjectPayStatisticModel> payList = PayMoneyProjectNameGroupBy();
            foreach (ProjectPayStatisticModel item in list)
            {
                var baseMatch = baseList.FirstOrDefault(r=>r.ProjectName.ToLower() == item.ProjectName.ToLower());
                if (baseMatch != null)
                {
                    item.TotalStudents = baseMatch.TotalStudents;
                }
                var payMatch = payList.FirstOrDefault(r => r.ProjectName.ToLower() == item.ProjectName.ToLower());
                if (payMatch != null)
                {
                    item.NeedPayMoney = payMatch.NeedPayMoney;
                }
                var liveMatch = Livelist.FirstOrDefault(r => r.ProjectName.ToLower() == item.ProjectName.ToLower());
                if (liveMatch != null)
                {
                    item.LiveStudents = liveMatch.LiveStudents;
                    if (item.TotalStudents > 0)
                    {
                        item.LiveRate =(float)Math.Round((float)item.LiveStudents / (float)item.TotalStudents,2);
                    }
                    
                }
                item.OweMoney = (decimal)item.NeedPayMoney - (decimal)item.TotalPayMoney;
            }
            return list;
        }

        private List<ProjectPayStatisticModel> LiveProjectNameGroupBy()
        {
            string sql = @"select a.ProjectName,count(*) LiveStudents  from DormAssign a where a.Status = 1 group by a.ProjectName";
            List<ProjectPayStatisticModel> list = new List<ProjectPayStatisticModel>();
            DBBaseService baseService = new DBBaseService(FlatContext.Instance);
            list = context.Database.SqlQuery<ProjectPayStatisticModel>(sql).ToList();
            return list;
        }

        private List<ProjectPayStatisticModel> PayMoneyProjectNameGroupBy()
        {
            string sql = @"select t.ProjectName , sum(t.NeedPayMoney) as NeedPayMoney  from (select a.ProjectName,a.StuName,(d.DepositMoney+d.RentMoney*a.PeriodType) as NeedPayMoney  from DormAssign a 
                           left join Dorm d on a.DID = d.DID where a.Status = 1 ) t group by t.ProjectName";
            List<ProjectPayStatisticModel> list = new List<ProjectPayStatisticModel>();
            DBBaseService baseService = new DBBaseService(FlatContext.Instance);
            list = context.Database.SqlQuery<ProjectPayStatisticModel>(sql).ToList();
            return list;
        }

        public List<BatchPayStatisticModel> GroupByBatch()
        {
            FlatContext context = new FlatContext();
            string sql = string.Format(@"select t.Batch ,sum(NeedPayMoney) NeedPayMoney,sum(TotalPayMoney) TotalPayMoney from  (select a.PID, a.DepositMoney,a.DormNo,a.ProjectName,a.DeductMoney,a.DeductRemark,
                                                   a.NeedPayMoney,a.PeriodMonth,a.RequestType,a.RentMoney,a.StuName,a.StuNo,
                                                   (select isnull(sum(PayMoney),0) from Payment where PID = a.PID and AuditStatus = 3) as TotalPayMoney,
                                                   (select Top 1 PayType from Payment where PID = a.PID) as PayType,
                                                   c.PEM,c.PRM,c.School,c.JoinTime,c.LeaveTime,
                                                   c.Batch,c.Direction,d.DormSize,d.DormType,c.Gender,c.Phone
                                                   from PayRequest a
                                                   left join DormAssign c on a.StuName = c.StuName 
                                                   left join Dorm d on d.DID = c.DID
                                                   where a.RequestType = 0 ) t group by t.Batch");
            List<BatchPayStatisticModel> list = new List<BatchPayStatisticModel>();
            DBBaseService baseService = new DBBaseService(FlatContext.Instance);
            list = context.Database.SqlQuery<BatchPayStatisticModel>(sql).ToList();
            List<BatchGroupBy> baseList = EvaluteDal.Instance.GroupByBatch();
            List<BatchPayStatisticModel> Livelist = LiveBatchGroupBy();
            List<BatchPayStatisticModel> payList = PayMoneyBatchGroupBy();
            foreach (BatchPayStatisticModel item in list)
            {
                var baseMatch = baseList.FirstOrDefault(r => r.Batch == item.Batch);
                if (baseMatch != null)
                {
                    item.TotalStudents = baseMatch.TotalStudents;
                }
                var payMatch = payList.FirstOrDefault(r => r.Batch == item.Batch);
                if (payMatch != null)
                {
                    item.NeedPayMoney = payMatch.NeedPayMoney;
                }
                var liveMatch = Livelist.FirstOrDefault(r => r.Batch == item.Batch);
                if (liveMatch != null)
                {
                    item.LiveStudents = liveMatch.LiveStudents;
                    if (item.TotalStudents > 0)
                    {
                        item.LiveRate = (float)Math.Round((float)item.LiveStudents / (float)item.TotalStudents,2);
                    }
                }
                item.OweMoney = (decimal)item.NeedPayMoney - (decimal)item.TotalPayMoney;
            }

            return list;
        }

        private List<BatchPayStatisticModel> LiveBatchGroupBy()
        {
            string sql = @"select a.Batch,count(*) LiveStudents  from DormAssign a where a.Status = 1 group by a.Batch";
            List<BatchPayStatisticModel> list = new List<BatchPayStatisticModel>();
            DBBaseService baseService = new DBBaseService(FlatContext.Instance);
            list = context.Database.SqlQuery<BatchPayStatisticModel>(sql).ToList();
            return list;
        }

        private List<BatchPayStatisticModel> PayMoneyBatchGroupBy()
        {
            string sql = @"select t.Batch , sum(t.NeedPayMoney) as NeedPayMoney  from (select a.Batch,a.StuName,(d.DepositMoney+d.RentMoney*a.PeriodType) as NeedPayMoney  from DormAssign a 
                           left join Dorm d on a.DID = d.DID where a.Status = 1 ) t group by t.Batch";
            List<BatchPayStatisticModel> list = new List<BatchPayStatisticModel>();
            DBBaseService baseService = new DBBaseService(FlatContext.Instance);
            list = context.Database.SqlQuery<BatchPayStatisticModel>(sql).ToList();
            return list;
        }
    }
}
