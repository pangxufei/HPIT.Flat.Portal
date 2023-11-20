using HPIT.Flat.Data.Adapters;
using HPIT.Flat.Data.Entitys;
using System.Collections.Generic;
using System.Web.Mvc;

namespace HPIT.Flat.Portal.Controllers
{
    public class DictionaryController : Controller
    {
        // GET: Dictionary
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult Sel(string Type="")
        {
            List<Dictionary> list = DictionaryDal.GetByList(Type);
            JsonResult json = new JsonResult();
            json.Data = new { Data = list };
            json.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return json;
        }
        [HttpPost]
        public JsonResult GetByID(int ID)
        {
            DictionaryDal dal = new DictionaryDal();
            Dictionary model = dal.GetDictID(ID);
            return Json(model);
        }
        [HttpPost]
        public JsonResult Upd(Dictionary Dict)
        {
            DictionaryDal dal = new DictionaryDal();
            if (dal.Upd(Dict) > 0)
            {
                return Json(1);
            }
            else
            {
                return Json(-1);
            }

        }
        [HttpPost]
        public JsonResult UpdStatus(int id, int Status = 0)
        {
            DictionaryDal dal = new DictionaryDal();
            if (dal.UpdStatus(id, Status) > 0)
            {
                return Json(1);
            }
            else
            {
                return Json(-1);
            }
        }
        [HttpPost]
        public JsonResult Ins(Dictionary Dict)
        {
            DictionaryDal dal = new DictionaryDal();
            if (dal.Ins(Dict) > 0)
            {
                return Json(1);
            }
            else
            {
                return Json(-1);

            }

        }
        [HttpPost]
        public JsonResult Del(int id)
        {
            DictionaryDal dal = new DictionaryDal();
            if (dal.Del(id) > 0)
            {
                return Json(1);
            }
            else
            {
                return Json(-1);
            }

        }
    }
}