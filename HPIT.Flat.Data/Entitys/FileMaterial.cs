namespace HPIT.Flat.Data.Entitys
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("FileMaterial")]
    public partial class FileMaterial
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public FileMaterial()
        {
            DormFileAttach = new HashSet<DormFileAttach>();
            PayMentFileAttach = new HashSet<PayMentFileAttach>();
        }

        [Key]
        [StringLength(36)]
        public string FID { get; set; }

        [StringLength(128)]
        public string FileName { get; set; }

        [StringLength(8)]
        public string FileType { get; set; }

        public int? FileLength { get; set; }

        public int? Status { get; set; }

        public DateTime? CreateTime { get; set; }

        [StringLength(128)]
        public string FilePath { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DormFileAttach> DormFileAttach { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PayMentFileAttach> PayMentFileAttach { get; set; }
    }
}
