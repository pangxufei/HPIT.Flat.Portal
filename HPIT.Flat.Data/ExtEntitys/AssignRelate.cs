using HPIT.Flat.Data.Entitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HPIT.Flat.Data.ExtEntitys
{
    public class AssignRelate
    {
        public Dorm dorm { get; set; }

        public DormAssign assign { get; set; }

        public decimal TotalPayMoney { get; set; }
    }
}
