using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HPIT.Flat.Data.Entitys
{
    public class PayDetailModel
    {
        [Key]
        [StringLength(36)]
        public string MID { get; set; }

        [StringLength(36)]
        public string FileName { get; set; }

        public string StuName { get; set; }

        public string DormNo { get; set; }

        [StringLength(32)]
        public string StuNo { get; set; }


        public decimal? PayMoney { get; set; }

        public DateTime? CreateTime { get; set; }


        public int PayType { get; set; }

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

        public string AuditStatusStr { get; set; }
        public int AuditStatus { get; set; }

        public string Auditor { get; set; }

        public string PEM { get; set; }

        public string ProjectName { get; set; }

    }
}
