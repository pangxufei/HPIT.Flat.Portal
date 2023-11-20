namespace HPIT.Flat.Data.Entitys
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("DormAssign")]
    public partial class DormAssign
    {
        [Key]
        [StringLength(36)]
        public string AID { get; set; }

        [StringLength(36)]
        public string DID { get; set; }

        [StringLength(20)]
        public string DormNo { get; set; }

        [StringLength(16)]
        public string StuName { get; set; }

        [StringLength(16)]
        public string StuNo { get; set; }

        [StringLength(16)]
        public string PEM { get; set; }

        [StringLength(16)]
        public string PRM { get; set; }

        [StringLength(32)]
        public string School { get; set; }

        public int PeriodType { get; set; }

        public DateTime? UpdateTime { get; set; }

        public DateTime? CreateTime { get; set; }

        public DateTime? JoinTime { get; set; }

        public DateTime? LeaveTime { get; set; }

        public string Phone { get; set; }

        public bool Gender { get; set; }

        public int Status { get; set; }
        [StringLength(200)]
        public string Remark { get; set; }
        [StringLength(2000)]
        public string Direction { get; set; }
        [StringLength(200)]
        public string Batch { get; set; }
        [StringLength(200)]
        public string ProjectName { get; set; }

        [NotMapped]
        public string StatusStr { get; set; }
    }
}
