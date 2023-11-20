using HPIT.Data.Core;
using HPIT.Evalute.Data.Model;
using HPIT.Flat.Data.Entitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace HPIT.Flat.Data.Adapters
{
    public class PaymentStatisticsDal
    {
        private static FlatContext db = new FlatContext();

        /// <summary>
        /// 查询数据
        /// </summary>
        /// <param name="search"></param>
        /// <param name="count"></param>
        /// <param name="Proname"></param> 
        /// <returns></returns>
        public static List<PaymentDetailModel> GetPageData(SearchModel<PayRequest> search, out int count, string Proname)
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
            string sql = string.Format(@"select a.DepositMoney,a.DormNo,a.ProjectName,
                                         a.NeedPayMoney,a.PeriodMonth,a.RequestType,a.RentMoney,a.StuName,a.StuNo,a.PID,
                                         (select sum(PayMoney) from Payment where PID = a.PID and AuditStatus = 3) as TotalPayMoney,
                                         (select Top 1 PayType from Payment where PID = a.PID) as PayType,
                                         c.PEM,c.PRM,c.School,c.JoinTime from PayRequest a
                                         join DormAssign c on a.StuName = c.StuName where a.RequestType = 0 and a.ProjectName='" + Proname + "'");
            list = baseService.GetSqlPagedData<PaymentDetailModel, string>(sql, parameter, out count);
            return list;
        }
        /// <summary>
        /// 获取项目部人数数据
        /// </summary>
        /// <returns></returns>
        public static List<PaymentDetailModel> PayStatis(string batch, HPITMemberInfo user)
        {
            string preixSql = user.FullName == "教质经理" ? "1=1" : "d.PEM = '" + user.RealName + "'";
            string sql = string.Format(string.Format(@"select count(p.StuName) 'PepNum',p.ProjectName, SUM(p.RealPayMoney)'RMoney' from 
                                           PayRequest p 
                                           left join DormAssign d on p.StuNo=d.StuNo where p.RequestType = 0  and d.Batch='{0}' and {1}  group by p.ProjectName",batch,preixSql));
            return db.Database.SqlQuery<PaymentDetailModel>(sql).ToList();
        }
        /// <summary>
        /// 根据项目部获取积累缴费与人数数据
        /// </summary>
        /// <param name="PrName"></param>
        /// <returns></returns>
        public static List<PaymentDetailModel> PayStuNum(string PrName)
        {
            string sql = string.Format(@"select sum(p.PayMoney) as RealPayMoney, r.DormNo from Payment p left join  PayRequest r on r.PID = p.PID where r.RequestType =0 and r.ProjectName='" + PrName+ "' group by r.DormNo");
            return db.Database.SqlQuery<PaymentDetailModel>(sql).ToList();
        }



    }
}
