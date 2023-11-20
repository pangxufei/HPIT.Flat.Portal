using ExcelCake.Intrusive;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HPIT.Flat.Data.ExportEntitys
{
    [ExportEntity(EnumColor.LightGray, "宾客信息")]
    [ImportEntity(titleRowIndex: 1, headRowIndex: 2, dataRowIndex: 3)]
    public class RoomModel : ExcelBase
    {
        [Import("日期")]
        public string Date { get; set; }

        [Import("房号")]
        public string RoomNo { get; set; }

        [Import("反馈信息")]
        public string Remark { get; set; }

        [Import("处理结果")]
        public string Result { get; set; }

        [Import("问题类型")]
        public string Type { get; set; }
    }

}
