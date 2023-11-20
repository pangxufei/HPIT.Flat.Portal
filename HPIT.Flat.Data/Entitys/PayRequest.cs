namespace HPIT.Flat.Data.Entitys
{
    using HPIT.Evalute.Data.Model;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PayRequest")]
    public partial class PayRequest
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PayRequest()
        {
            Payment = new HashSet<Payment>();
        }

        [Key]
        [StringLength(36)]
        public string PID { get; set; }

        [StringLength(20)]
        public string DormNo { get; set; }

        public int? RequestType { get; set; }

        [StringLength(16)]
        public string StuNo { get; set; }

        [StringLength(32)]
        public string StuName { get; set; }

        public decimal? NeedPayMoney { get; set; }

        public decimal? CurrentPayMoney { get; set; }

        public decimal? RealPayMoney { get; set; }

        [NotMapped]
        public string StatusStr { get; set; }
        public int? Status { get; set; }

        [StringLength(36)]
        public string Operator { get; set; }

        public DateTime? CreateTime { get; set; }

        public DateTime? JoinTime { get; set; }
        public DateTime? UpdateTime { get; set; }

        [StringLength(2000)]
        public string Remark { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Payment> Payment { get; set; }

        public decimal DepositMoney { get; set; }

        public int PeriodMonth { get; set; }

        public string ProjectName { get; set; }

        public decimal RentMoney { get; set; }

        public decimal RefundMoney { get; set; }

        public decimal DeductMoney { get; set; }

        public string DeductRemark { get; set; }

        public string RefundRemark { get; set; }

        [NotMapped]

        public FileMaterial FileInfo { get; set; }

        [NotMapped]
        public int PayStatus { get; set; }
    }
}
