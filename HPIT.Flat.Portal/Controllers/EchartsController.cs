using HPIT.Flat.Data.Adapters;
using HPIT.Flat.Data.Entitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HPIT.Flat.Portal.Controllers
{
    public class EchartsController : Controller
    {
        // GET: Echarts
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public JsonResult EchartSelect(string start,string end)
        {
            DormStatisticDal EC = new DormStatisticDal();
            //调用dal层方法
            var data = EC.GetLiveRateStatistics(Convert.ToDateTime(start), Convert.ToDateTime(end));
            return Json(data);
        }

        [HttpPost]
        public JsonResult Echartlist(int  value)
        {
            List<Dorm> list = DormStatisticDal.getlist(value);
            JsonResult json = new JsonResult();
            json.Data = new { Data = list };
            return json;
        }
    }
}