using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HPIT.Flat.Data.Entitys
{
    public class GeneralSelectItem
    {
        public int ID { get; set; }

        public string Value { get; set; }

        public string Text { get; set; }

        public int parentID { get; set; }
    }
}
