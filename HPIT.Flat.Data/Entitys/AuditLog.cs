using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HPIT.Flat.Data.Entitys
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    [Table("AuditLog")]
    public class AuditLog
    {
        [Key]
        public int ID { get; set; }

        public string AuditName { get; set; }

        public DateTime AuditTime { get; set; }

        public string  PayID { get; set; }

        public int AuditState { get; set; }

        public string RoleName { get; set; }

        public int UserID { get; set; }
    }
}
