using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HPIT.Flat.Data.ExtEntitys
{
    public class LiveRateStatistic
    {

        /// <summary>
        /// 所有的床位
        /// </summary>
        public int TotalBeds { get; set; }

        /// <summary>
        /// 使用中的床位
        /// </summary>
        public int UsedBeds { get; set; }

        /// <summary>
        /// 没有使用的床位
        /// </summary>
        public int UnUsedBeds { get; set; }


        /// <summary>
        /// 专业方向
        /// </summary>
        public string Direction { get; set; }


        /// <summary>
        /// 项目部
        /// </summary>
        public string ProjectName { get; set; }
    }
}
