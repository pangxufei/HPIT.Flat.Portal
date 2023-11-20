using ExcelCake.Intrusive;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HPIT.Flat.Data.ExtEntitys
{
    [ExportEntity(EnumColor.AliceBlue, "小程序提成统计")]
    public class PersonDeduct : ExcelBase
    {
        [Export("小程序提成", 2)]
        public decimal? DeductMoney { get; set; }

        [Export("提成人", 1)]
        public string Person { get; set; }
    }
}
