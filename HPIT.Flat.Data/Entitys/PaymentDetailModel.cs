using ExcelCake.Intrusive;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HPIT.Flat.Data.Entitys
{
    [ExportEntity(EnumColor.LightGray, "学生缴费信息表")]
    [ImportEntity(titleRowIndex: 1, headRowIndex: 2, dataRowIndex: 4)]
    public class PaymentDetailModel : ExcelBase
    {
        // [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PaymentDetailModel()
        {
            Payment = new HashSet<Payment>();
        }

        public string PID { get; set; }

        [Export("批次", 1)]
        public string Batch { get; set; }

        [Export("学校", 1)]
        public string School { get; set; }

        [Export("项目部", 1)]
        public string ProjectName { get; set; }

        [Export("PEM", 1)]
        public string PEM { get; set; }

        [Export("PRM", 1)]
        public string PRM { get; set; }

        [Export("宿舍编号", 2)]
        public string DormNo { get; set; }

        public int? RequestType { get; set; }

        public decimal? RealPayMoney { get; set; }

        [Export("学生学号", 4)]
        public string StuNo { get; set; }

        [Export("学生姓名", 5)]
        public string StuName { get; set; }

        [Export("性别", 5)]
        public string Sex { get; set; }

        [Export("性别", 5)]
        public string Phone { get; set; }

        [Export("住宿标准", 6, suffix: "人间")]
        public int DormSize { get; set; }

        [Export("入住日期", 6)]
        public DateTime? JoinTime { get; set; }

        [Export("离宿日期", 6)]
        public DateTime? LeaveTime { get; set; }

        [Export("押金", 6)]
        public decimal DepositMoney { get; set; }

        [Export("租金", 6)]
        public decimal RentMoney { get; set; }

        [Export("住宿月份", 6)]
        public int PeriodMonth { get; set; }

        [Export("应缴费金额", 7)]
        public decimal? NeedPayMoney { get; set; }

        [Export("累积缴费金额", 7)]
        public decimal? TotalPayMoney { get; set; }

        [Export("支付方式", 7)]
        public string PayTypeString
        {
            get
            {
                if (PayType == 1)
                {
                    return "微信";
                }
                else if (PayType == 2)
                {
                    return "支付宝";
                }
                else
                {
                    return "其他";
                }
            }
        }

        [Export("欠费金额", 7)]
        public decimal OweMoney { get; set; }

        [Export("扣款金额", 7)]
        public decimal DeductMoney { get; set; }

        [Export("扣款原因", 7)]
        public string DeductRemark { get; set; }

        [Export("退款金额", 7)]
        public decimal RefundMoney { get; set; }

        [Export("退款备注", 7)]
        public string RefundRemark { get; set; }

        public decimal? CurrentPayMoney { get; set; }

        public string StatusStr { get; set; }

        public int? Status { get; set; }

        public string Operator { get; set; }

        public DateTime? CreateTime { get; set; }

        public DateTime? UpdateTime { get; set; }

        public string Remark { get; set; }

        //  [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Payment> Payment { get; set; }

        public int PepNum { get; set; }
        public Decimal RMoney { get; set; }

        public int PayType { get; set; }


    }
}
