using HPIT.Flat.Data.Entitys;
using HPIT.Flat.Data.ExtEntitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HPIT.Flat.Data.Adapters
{
    public class DormStatisticDal
    {
        public static DormStatisticDal Instance = new DormStatisticDal();

        public FlatContext context { get; set; }
        public DormStatisticDal()
        {
            this.context = new FlatContext();
        }

        /// <summary>
        /// 入住率统计
        /// 开始时间 结束时间
        /// </summary>
        /// <returns></returns>
        public LiveRateStatistic GetLiveRateStatistics(DateTime start ,DateTime end)
        {
            LiveRateStatistic model = new LiveRateStatistic();
            model.TotalBeds = TotalBeds();
            var usedList = new List<DormAssign>();
            if (start != null && start.Year > 2000)
            {
                usedList = context.DormAssign.Where(r => r.CreateTime >= start).ToList();
            }
            if (end !=null && end.Year > 2000)
            {
                usedList = usedList.Where(r => r.CreateTime <= end).ToList();
            }
            model.UsedBeds = usedList.Count();
            model.UnUsedBeds = model.TotalBeds - model.UsedBeds;
            return model;
        }
        /// <summary>
        /// 获取公寓的总床位
        /// </summary>
        /// <returns></returns>
        public int TotalBeds()
        {
            int total = 0;
            foreach (var dorm in context.Dorm.ToList())
            {
                total += (int)dorm.DormSize;
            }
            return total;
        }

        public static List<Dorm> getlist(int value)
        {
            FlatContext flat = new FlatContext();
            string sql =string.Format(@"select *from Dorm where Dorm.Status='{0}'",value);
            List<Dorm> list = flat.Database.SqlQuery<Dorm>(sql).ToList();
            return list;
        }

    }
}
