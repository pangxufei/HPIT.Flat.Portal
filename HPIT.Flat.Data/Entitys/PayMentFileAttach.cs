namespace HPIT.Flat.Data.Entitys
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PayMentFileAttach")]
    public partial class PayMentFileAttach
    {
        [StringLength(36)]
        public string ID { get; set; }

        [StringLength(36)]
        public string FID { get; set; }

        [StringLength(36)]
        public string MID { get; set; }

        public DateTime? CreateTime { get; set; }

        [StringLength(128)]
        public string FileName { get; set; }

        public virtual FileMaterial FileMaterial { get; set; }

        public virtual Payment Payment { get; set; }
    }
}
