namespace HPIT.Flat.Data.Entitys
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class UserRoles
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID { get; set; }

        public int? RoleID { get; set; }

        public int? UserID { get; set; }

        public virtual Roles Roles { get; set; }

        public virtual Users Users { get; set; }
    }
}
