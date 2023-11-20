using HPIT.Flat.Data.Entitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HPIT.Flat.Data.Adapters
{
    public class FileAttachDal
    {
        public static FileAttachDal Instance = new FileAttachDal();

        public FlatContext context { get; set; }

        public FileAttachDal()
        {
            this.context = new FlatContext();
        }

        public FileMaterial AddFile(string filename,string type="jpg")
        {
            //PayMentFileAttach payMentFileAttach = new PayMentFileAttach();
            //payMentFileAttach.FID = Guid.NewGuid().ToString();
            //payMentFileAttach.FileName = filename;
            //payMentFileAttach.
            FileMaterial fileMaterial = new FileMaterial();
            fileMaterial.FID = Guid.NewGuid().ToString();
            fileMaterial.FileName = filename;
            fileMaterial.FileType = type;
            fileMaterial.CreateTime = DateTime.Now;
            this.context.FileMaterial.Add(fileMaterial);
            this.context.SaveChanges();
            return fileMaterial;
        }
    }
}
