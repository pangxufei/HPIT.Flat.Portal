using ExcelCake.Intrusive;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HPIT.Flat.Data.ExportEntitys
{

    [ExportEntity(EnumColor.LightGray, "批次缴费信息统计表")]
    [ImportEntity(titleRowIndex: 1, headRowIndex: 2, dataRowIndex: 4)]
    public class BatchPayStatisticModel : ExcelBase
    {
        [Export("批次", 1)]
        public string Batch { get; set; }

        [Export("总人数", 1)]
        public int TotalStudents { get; set; }

        [Export("入住人数", 1)]
        public int LiveStudents { get; set; }

        [Export("入住率", 1)]
        public float LiveRate { get; set; }

        [Export("应缴费金额", 7)]
        public decimal? NeedPayMoney { get; set; }

        [Export("累积缴费金额", 7)]
        public decimal? TotalPayMoney { get; set; }

        [Export("欠费金额", 7)]
        public decimal OweMoney { get; set; }

        [Export("备注", 7)]
        public string Remark { get; set; }
    }
}
