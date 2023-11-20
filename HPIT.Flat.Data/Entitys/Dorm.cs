namespace HPIT.Flat.Data.Entitys
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Dorm")]
    public partial class Dorm
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Dorm()
        {
            DormAssign = new HashSet<DormAssign>();
            DormFileAttach = new HashSet<DormFileAttach>();
        }

        [Key]
        [StringLength(36)]
        public string DID { get; set; }

        [StringLength(20)]
        public string DormNo { get; set; }

        [StringLength(200)]
        public string DormName { get; set; }

        public int? DormType { get; set; }

        [NotMapped]
        public string TypeStr { get; set; }

        [StringLength(15)]
        public string Phone { get; set; }

        [NotMapped]
        public string SizeStr { get; set; }

        /// <summary>
        /// 现在已经住了多少人
        /// </summary>
        [NotMapped]
        public int MatchSize { get; set; }

        public int? DormSize { get; set; }

        [StringLength(2000)]
        public string Remark { get; set; }

        [NotMapped]
        public string StatusStr { get; set; }

        //押金
        public decimal DepositMoney { get; set; }

        //租金
        public decimal RentMoney { get; set; }

        public int? Status { get; set; }

        [NotMapped]
        public string DormJsonStr { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DormAssign> DormAssign { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DormFileAttach> DormFileAttach { get; set; }
    }
}
