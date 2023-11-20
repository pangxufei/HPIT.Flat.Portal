using ExcelCake.Intrusive;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HPIT.Flat.Data.ExtEntitys
{
    public class GuestDateStatistic
    {
        public DateTime ReportDate { get; set; }

        public string Type { get; set; }

        public int Count { get; set; }
    }


    [ExportEntity(EnumColor.LightGray, "宾客入住统计信息")]
    public class GuestStatistic : ExcelBase
    {

        public DateTime ReportDate { get; set; }

        [Export("日期", 1)]
        public string ReportDateStr { get; set; }

        [Export("满意", 2)]
        public int SatisfyCount { get; set; }

        [Export("建议", 3)]
        public int SuggestCount { get; set; }

        [Export("意见", 4)]
        public int OpinionCount { get; set; }

        [Export("总计", 5)]
        public int TotalCount { get; set; }
    
    }

    [ExportEntity(EnumColor.LightGray, "宾客入住建议统计")]
    public class GuestTypeStatistic : ExcelBase
    {

        public string Type { get; set; }

        [Export("建议内容", 1)]
        public string Remark { get; set; }

        [Export("次数", 2)]
        public int Count { get; set; }
    }
}
