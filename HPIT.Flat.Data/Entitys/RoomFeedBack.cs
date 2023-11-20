namespace HPIT.Flat.Data.Entitys
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("RoomFeedBack")]
    public partial class RoomFeedBack
    {
        public string ID { get; set; }

        public DateTime HotelDate { get; set; }

        public string RoomNo { get; set; }

        //[StringLength(50)]
        public string Remark { get; set; }

        //[StringLength(50)]
        public string Result { get; set; }

        //[StringLength(50)]
        public string Type { get; set; }
    }
}
