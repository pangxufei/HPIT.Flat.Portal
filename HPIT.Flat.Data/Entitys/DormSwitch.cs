namespace HPIT.Flat.Data.Entitys
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("DormSwitch")]
    public partial class DormSwitch
    {
        [StringLength(36)]
        public string ID { get; set; }

        [StringLength(36)]
        public string StuName { get; set; }

        [StringLength(16)]
        public string StuNo { get; set; }

        [StringLength(20)]
        public string TargetDormNo { get; set; }

        [StringLength(20)]
        public string SourceDormNo { get; set; }

        [StringLength(500)]
        public string Remark { get; set; }

        [StringLength(100)]
        public string Operator { get; set; }

        public DateTime? CreateTime { get; set; }

        [StringLength(36)]
        public string SecondStuName { get; set; }

        [StringLength(20)]
        public string SecondStuNo { get; set; }
    }
}
