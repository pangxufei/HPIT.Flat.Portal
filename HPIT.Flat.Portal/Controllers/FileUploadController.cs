using HPIT.Data.Core;
using HPIT.Flat.Data.Adapter;
using HPIT.Flat.Data.Adapters;
using HPIT.Flat.Data.Entitys;
using HPIT.Flat.Portal.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HPIT.Flat.Portal.Controllers
{
    public class FileUploadController : Controller
    {
        [AllowAnonymous]
        public ActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public JsonResult ProcessUploadFiles(IEnumerable<HttpPostedFileBase> filenames)
        {
            FileMaterial fileid = null;
            foreach (var file in filenames)
            {
                var filename = Path.GetFileName(file.FileName);
                var path = Path.Combine(Server.MapPath("~/Upload"), filename);
                file.SaveAs(path);
                fileid = FileAttachDal.Instance.AddFile(filename);
            }
            JsonResult result = new JsonResult();
            result.Data = fileid;
            return result;
        }

        [AllowAnonymous]
        [HttpPost]
        public JsonResult ProcessImportFiles(IEnumerable<HttpPostedFileBase> filenames)
        {
            string msg = "";
            string path = "";
            foreach (var file in filenames)
            {
                var filename = Path.GetFileName(file.FileName);
                path = Path.Combine(Server.MapPath("~/Upload"), filename);
                file.SaveAs(path);
            }
            if (!string.IsNullOrEmpty(path))
            {
                msg = DormDal.Instance.Import(path);
            }
            JsonResult result = new JsonResult();
            result.Data = msg;
            return result;
        }


        [AllowAnonymous]
        [HttpPost]
        public JsonResult ProcessImportRoomFeedBackFiles(IEnumerable<HttpPostedFileBase> filenames)
        {
            string msg = "";
            string path = "";
            foreach (var file in filenames)
            {
                var filename = Path.GetFileName(file.FileName);
                path = Path.Combine(Server.MapPath("~/Upload"), filename);
                file.SaveAs(path);
            }
            if (!string.IsNullOrEmpty(path))
            {
                msg = RoomFeedBackDal.Instance.Import(path);
            }
            JsonResult result = new JsonResult();
            result.Data = msg;
            return result;
        }


        /// <summary>
        /// 提成
        /// </summary>
        /// <param name="filenames"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        public JsonResult ProcessImportRoomDeductFiles(IEnumerable<HttpPostedFileBase> filenames)
        {
            string msg = "";
            string path = "";
            foreach (var file in filenames)
            {
                var filename = Path.GetFileName(file.FileName);
                path = Path.Combine(Server.MapPath("~/Upload"), filename);
                file.SaveAs(path);
            }
            if (!string.IsNullOrEmpty(path))
            {
                msg = RoomFeedBackDal.Instance.ImportDeduct(path);
            }
            JsonResult result = new JsonResult();
            result.Data = msg;
            return result;
        }

        [AllowAnonymous]
        public ActionResult FileInputIndex()
        {
            return View();
        }

        [AllowAnonymous]
        public FileStreamResult download(string filename)
        {
            string filePath = Server.MapPath("~/Upload/"+filename);//路径
            //return File(filePath, "text/plain", filename); //filename是客户端保存的名字
            return File(new FileStream(filePath, FileMode.Open), "text/plain",
            filename);
        }

        [AllowAnonymous]
        public JsonResult GetFileList(SearchModel<FileModel> search)
        {
            int total = 0;
            //获取需要显示文件的目录 
            string filePath = Server.MapPath("~/Upload/");//路径
            DirectoryInfo directoryInfo = new DirectoryInfo(filePath);
            //获取文件夹下的所有文件
            FileInfo[] allfiles = directoryInfo.GetFiles();
            //生成新的文件类型的数据集合 linq to collection
            var data = from file in allfiles
                       select new
                       {
                           filename = file.Name,
                           path = file.DirectoryName,
                           time = file.LastWriteTime,
                           fullname = file.FullName
                       };
            //总条数
            total = allfiles.Length;
            var totalPages = total % search.PageSize == 0 ? total / search.PageSize : total / search.PageSize + 1;
            JsonResult result = new JsonResult();
            //原生分页代码 skip take
            var pagedata = data.Skip((search.PageIndex-1) * search.PageSize).Take(search.PageSize); 
            //组织新的数据 Data , Total, TotalPages 
            result.Data =new { Data = pagedata, Total = total, TotalPages = totalPages };
            result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return result;
        }


        [AllowAnonymous]
        public ActionResult FileListIndex()
        {
            return View();
        }
    }
}