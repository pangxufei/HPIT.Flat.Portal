namespace HPIT.Flat.Data.Entitys
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Dictionary")]
    public partial class Dictionary
    {
        public int ID { get; set; }

        [StringLength(20)]
        public string Name { get; set; }

        [StringLength(100)]
        public string Value { get; set; }

        [StringLength(20)]
        public string Type { get; set; }

        public int? Status { get; set; }

        public int? ParentID { get; set; }
    }
}
