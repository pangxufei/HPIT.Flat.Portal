using HPIT.Data.Core;
using HPIT.Flat.Data.Adapter;
using HPIT.Flat.Data.Entitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static HPIT.Flat.Data.Entitys.Enumerations;

namespace HPIT.Flat.Data.Adapters
{
    public class PayDutails
    {
        public static PayDutails Instance = new PayDutails();
        public FlatContext context { get; set; }
        public PayDutails()
        {
            this.context = new FlatContext();
        }
        //根据名字查询     
        public static List<PayDetailModel> GetPageData(SearchModel<Payment> search, out int count, string StuName)
        {
            GetPageListParameter<PayDetailModel, string> parameter = new GetPageListParameter<PayDetailModel, string>();
            parameter.isAsc = true;
            parameter.orderByLambda = t => t.StuName;
            parameter.pageIndex = search.PageIndex;
            parameter.pageSize = search.PageSize;
            parameter.whereLambda = t => !string.IsNullOrEmpty(t.FileName);
            List<PayDetailModel> list = new List<PayDetailModel>();
            //查询数据
            string sqlwhere = " where 1=1 ";
            DBBaseService baseService = new DBBaseService(FlatContext.Instance);
            string sql = string.Format(@"select * from (select a.MID,a.Auditor,d.ProjectName,
                                       (select top 1 PEM from dbo.DormAssign ss where ss.DormNo = d.DormNo) as PEM, c.FileName,
                                       a.StuName,d.DormNo,d.StuNo,a.PayMoney,a.CreateTime,a.PayType,a.AuditStatus 
                                       from Payment a
                                       left join PayRequest d on d.PID = a.PID
                                       left join PayMentFileAttach c  on a.MID = c.MID) t ");
            if (!string.IsNullOrEmpty(search.UserName))
            {
                sqlwhere += string.Format(@" and t.Auditor = '{0}'",search.UserName);
            }
            sql = sql + sqlwhere;
            list = baseService.GetSqlPagedData<PayDetailModel, string>(sql, parameter, out count);
            foreach (var detail in list)
            {
                detail.AuditStatusStr = EnumDescriptionAttribute.GetDescription((PayRequestStatus)detail.AuditStatus);
            }
            return list;

        }
        public static List<PayRequest> GetProjName()
        {
            FlatContext flat = new FlatContext();
            var count = flat.PayRequest.ToList().OrderBy(p => p.ProjectName).ToList();
            return count;
        }


        public int Audit(AuditLog log)
        {
            log.AuditTime = DateTime.Now;
            FlatContext context = new FlatContext();
            //通过工作流审批 获取到下一个审批人
            string nextAuditName = "";
            FlatWorkFlow surveyWorkFlow = new FlatWorkFlow();
            int finalLogStatus = 0;
            //审批通过
            if (log.AuditState == 1)
            {
                nextAuditName = surveyWorkFlow.Pass(log.AuditName, log.RoleName);
                log.AuditState = (int)PayRequestStatus.pass;
                finalLogStatus = log.AuditState;
            }
            //拒绝
            else
            {
                nextAuditName = surveyWorkFlow.Reject(log.AuditName, log.RoleName);
                log.AuditState = (int)PayRequestStatus.reject;
                finalLogStatus = log.AuditState;
                if (nextAuditName == "人事经理")
                {
                    Payment current = this.context.Payment.FirstOrDefault(r=>r.MID == log.PayID);
                    //如果没找到则为空，如果不为空，则设置成为当前单子的pem;
                    nextAuditName = current == null ? "" : current.Auditor;
                }

                if (nextAuditName == "学生")
                {
                    nextAuditName = "";
                }
            }
            if (nextAuditName == "已完成")
            {
                log.AuditState = (int)PayRequestStatus.complete;
                finalLogStatus = log.AuditState;
            }
            //添加一条log
            context.AuditLog.Add(log);
            context.Database.ExecuteSqlCommand(
                  string.Format(@"UPDATE dbo.Payment SET Auditor = '{0}', AuditStatus={1} where MID='{2}'", nextAuditName, finalLogStatus, log.PayID));
            return context.SaveChanges();
        }
    }
}
