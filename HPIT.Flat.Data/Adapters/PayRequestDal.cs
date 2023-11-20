using HPIT.Data.Core;
using HPIT.Flat.Data.Entitys;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static HPIT.Flat.Data.Entitys.Enumerations;

namespace HPIT.Flat.Data.Adapters
{
    public class PayRequestDal
    {

        public static PayRequestDal Instance = new PayRequestDal();
        public FlatContext context { get; set; }
        public PayRequestDal()
        {
            this.context = new FlatContext();
        }
        /// <summary>
        /// 缴费列表
        /// </summary>
        /// <param name="search"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public List<PayRequest> GetPageData(SearchModel<PayRequest> search, out int count)
        {
            GetPageListParameter<PayRequest, string> parameter = new GetPageListParameter<PayRequest, string>();
            parameter.isAsc = true;
            parameter.orderByLambda = t => t.DormNo;
            parameter.pageIndex = search.PageIndex;
            parameter.pageSize = search.PageSize;
            parameter.whereLambda = t => !string.IsNullOrEmpty(t.PID) && t.StuName == search.UserName && t.RequestType == (int)RequestType.pay;
            //查询数据
            DBBaseService baseService = new DBBaseService(FlatContext.Instance);
            List<PayRequest> list = baseService.GetSimplePagedData<PayRequest, string>(parameter, out count);
            foreach (var request in list)
            {
                request.StatusStr = EnumDescriptionAttribute.GetDescription((RequestStatus)request.Status);
                request.RealPayMoney = request.Payment.Where(r=>r.AuditStatus == (int)PayRequestStatus.complete).Sum(r => r.PayMoney);
            }
            return list;
        }

        /// <summary>
        /// 退款管理列表
        /// </summary>
        /// <param name="search"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public List<PayRequest> GetRefundPageData(SearchModel<PayRequest> search, out int count)
        {
            GetPageListParameter<PayRequest, string> parameter = new GetPageListParameter<PayRequest, string>();
            parameter.isAsc = true;
            parameter.orderByLambda = t => t.DormNo;
            parameter.pageIndex = search.PageIndex;
            parameter.pageSize = search.PageSize;
            if (search.RoleName == "人事经理")
            {
                parameter.whereLambda = t => !string.IsNullOrEmpty(t.PID) && t.RequestType == (int)RequestType.refund;
            }
            else if (search.RoleName == "教质经理")
            {
                parameter.whereLambda = t => !string.IsNullOrEmpty(t.PID) && t.RequestType == (int)RequestType.refund;
            }
            else
            {
                parameter.whereLambda = t => !string.IsNullOrEmpty(t.PID) && t.StuName == search.UserName && t.RequestType == (int)RequestType.refund;
            }
           
            //查询数据
            DBBaseService baseService = new DBBaseService(FlatContext.Instance);
            List<PayRequest> list = baseService.GetSimplePagedData<PayRequest, string>(parameter, out count);
            foreach (var request in list)
            {
                request.StatusStr = EnumDescriptionAttribute.GetDescription((PayRequestStatus)request.Status);
                request.RealPayMoney = request.Payment.Sum(r => r.PayMoney);
            }
            return list;
        }

        /// <summary>
        /// 判断当前用户是否已经发出过付款请求
        /// </summary>
        /// <param name="name"></param>
        /// <param name="no"></param>
        /// <returns></returns>
        public bool CheckHaveRequest(string name,string no,int type)
        {
            var match = context.PayRequest.FirstOrDefault(r => r.StuName == name && r.StuNo == no && r.RequestType == type);
            if (match == null)
            {
                return false;
            }
            return true;
        }

        public int DoDeductMoney(string requestID,decimal deductMoney, string remark)
        {
            FlatContext db = new FlatContext();
            string sql = string.Format(@"update PayRequest set DeductRemark ='{0}',DeductMoney={1}  where PID='{2}'",remark,deductMoney,requestID);
            int result = db.Database.ExecuteSqlCommand(sql);
            return result;
        }

        public int DoRefundMoney(string requestID, decimal refundMoney, string remark)
        {
            FlatContext db = new FlatContext();
            string sql = string.Format(@"update PayRequest set RefundRemark ='{0}',RefundMoney={1}  where PID='{2}'", remark, refundMoney, requestID);
            int result = db.Database.ExecuteSqlCommand(sql);
            return result;
        }

        public List<Payment> QueryPayDetailByRequestID(string requestID)
        {
            List<Payment> result = context.Payment.AsNoTracking().Where(r => r.PID == requestID).ToList();
            return result;
        }

        public int Save(PayRequest request,string pem)
        {
            int result = 0;
            //如果为空的话，则添加，如果不为空的话，则进行更新，添加一条付款记录
            //1.付款请求
            if (string.IsNullOrEmpty(request.PID))
            {
                request.PID = Guid.NewGuid().ToString();
                request.RealPayMoney = request.CurrentPayMoney;
                request.Status = (int)PayRequestStatus.audit;
                context.PayRequest.Add(request);
            }
            //2.付款请求下 有多次付款 每一次付款需要对应一个票据。 
            Payment payment = new Payment();
            payment.PID = request.PID;
            payment.Auditor = pem;
            payment.MID = Guid.NewGuid().ToString();
            payment.PayMoney = request.CurrentPayMoney;
            payment.StuName = request.StuName;
            payment.CreateTime = DateTime.Now;
            context.Payment.Add(payment);
            //3.添加票据信息，票据信息 和 票据文件需要一块查看。
            PayMentFileAttach payMentFileAttach = new PayMentFileAttach();
            if (request.FileInfo != null)
            {
                payMentFileAttach.FileName = request.FileInfo.FileName;
                payMentFileAttach.FID = request.FileInfo.FID;
                payMentFileAttach.ID = Guid.NewGuid().ToString();
                payMentFileAttach.MID = payment.MID;
                context.PayMentFileAttach.Add(payMentFileAttach);
            }
            try
            {
                //4.最后保存该数据内容。 这之间应该是事务的关系 ，这个问题需要处理一下。
                result = context.SaveChanges();
            }
            catch (DbEntityValidationException dx)
            {
                LogHelper.Default.WriteError(dx.Message);
            }
            return result;
        }

        /// <summary>
        /// 退款操作
        /// </summary>
        /// <param name="request"></param>
        /// <param name="pem"></param>
        /// <returns></returns>
        public int Refund(PayRequest request, string pem)
        {
            int result = 0;
            //如果为空的话，则添加，如果不为空的话，则进行更新，添加一条付款记录
            //1.付款请求
            if (string.IsNullOrEmpty(request.PID))
            {
                request.PID = Guid.NewGuid().ToString();
                request.RealPayMoney = request.CurrentPayMoney;
                request.Status = (int)PayRequestStatus.audit;
                context.PayRequest.Add(request);
            }
            //2.付款请求下有多次付款 每一次付款需要对应一个票据。 
            Payment payment = new Payment();
            payment.PID = request.PID;
            payment.Auditor = pem;
            payment.MID = Guid.NewGuid().ToString();
            payment.PayMoney = request.CurrentPayMoney;
            payment.StuName = request.StuName;
            payment.CreateTime = DateTime.Now;
            context.Payment.Add(payment);
            //3.添加票据信息，票据信息 和 票据文件需要一块查看。
            PayMentFileAttach payMentFileAttach = new PayMentFileAttach();
            if (request.FileInfo != null)
            {
                payMentFileAttach.FileName = request.FileInfo.FileName;
                payMentFileAttach.FID = request.FileInfo.FID;
                payMentFileAttach.ID = Guid.NewGuid().ToString();
                payMentFileAttach.MID = payment.MID;
                context.PayMentFileAttach.Add(payMentFileAttach);
            }
            try
            {
                //4.最后保存该数据内容。 这之间应该是事务的关系 ，这个问题需要处理一下。
                result = context.SaveChanges();
            }
            catch (DbEntityValidationException dx)
            {
                LogHelper.Default.WriteError(dx.Message);
            }
            return result;
        }
    }
}
