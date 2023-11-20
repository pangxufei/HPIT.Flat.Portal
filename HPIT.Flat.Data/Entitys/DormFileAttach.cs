namespace HPIT.Flat.Data.Entitys
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("DormFileAttach")]
    public partial class DormFileAttach
    {
        [StringLength(36)]
        public string ID { get; set; }

        [StringLength(36)]
        public string FID { get; set; }

        [StringLength(36)]
        public string DID { get; set; }

        public DateTime? CreateTime { get; set; }

        [StringLength(36)]
        public string FileName { get; set; }

        public virtual Dorm Dorm { get; set; }

        public virtual FileMaterial FileMaterial { get; set; }
    }
}
