namespace HPIT.Flat.Data.Entitys
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Department")]
    public partial class Department
    {
        public int? UserID { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID { get; set; }

        [StringLength(20)]
        public string DepartName { get; set; }

        [StringLength(100)]
        public string DepartDesc { get; set; }

        public DateTime? CreateTime { get; set; }

        public int? Status { get; set; }

        public virtual Users Users { get; set; }
    }
}
