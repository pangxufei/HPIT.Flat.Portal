namespace HPIT.Flat.Data.Entitys
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Menus
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Menus()
        {
            RoleMenus = new HashSet<RoleMenus>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int MenuID { get; set; }

        [StringLength(20)]
        public string MenuName { get; set; }

        [StringLength(20)]
        public string MenuType { get; set; }

        public int? Status { get; set; }

        public int? ParentID { get; set; }

        public int? Sort { get; set; }
        public string MenuCode { get; set; }
        public string MenuUrl { get; set; }
        [NotMapped]
        public string MenuJsonStr { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RoleMenus> RoleMenus { get; set; }
    }
}
