namespace HPIT.Flat.Data.Entitys
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Roles
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Roles()
        {
            RoleMenus = new HashSet<RoleMenus>();
            UserRoles = new HashSet<UserRoles>();
        }

        [Key]
        //[DatabaseGenerated(DatabaseGeneratedOption.None)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RoleID { get; set; }

        [StringLength(20)]
        public string RoleName { get; set; }

        [StringLength(100)]
        public string RoleDesc { get; set; }

        public int? Status { get; set; }

        [NotMapped]
        public string StatusStr { get; set; }

        public DateTime? CreateTime { get; set; }

        public DateTime? AlterTime { get; set; }

        [NotMapped]
        public string RoleJsonStr { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RoleMenus> RoleMenus { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UserRoles> UserRoles { get; set; }
    }
}
