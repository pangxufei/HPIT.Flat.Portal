using ExcelCake.Intrusive;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HPIT.Flat.Data.ExportEntitys
{

    [ExportEntity(EnumColor.LightGray, "宿舍信息")]
    [ImportEntity(titleRowIndex: 1, headRowIndex: 2, dataRowIndex: 3)]
    public class DormModel : ExcelBase
    {
        [Import("宿舍编号")]
        public string DormNo { get; set; }

        [Import("宿舍类型")]
        public string Gender { get; set; }

        [Import("宿舍规格")]
        public int Size { get; set; }

        [Import("宿舍地址")]
        public string Address { get; set; }

        [Import("联系电话")]
        public string Phone { get; set; }

        [Import("押金")]
        public decimal DepositMoney { get; set; }

        [Import("租金")]
        public decimal RentMoney { get; set; }

        [Import("备注")]
        public string Remark { get; set; }
    }
}
