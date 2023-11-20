using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HPIT.Flat.Data.Entitys
{
    [Table("RoomDeduct")]
    public class RoomDeduct
    {
        public string ID { get; set; }

        public int Year { get; set; }

        public int Month { get; set; }

        public string HotelDate { get; set; }

        public string RoomNo { get; set; }

        //[StringLength(50)]
        public string Remark { get; set; }

        //[StringLength(50)]
        public decimal? Money { get; set; }

        //[StringLength(50)]
        public string Persons { get; set; }

        public DateTime? CreateDate { get; set; }
    }
}
