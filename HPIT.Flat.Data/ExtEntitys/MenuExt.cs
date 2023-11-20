using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HPIT.Flat.Data.ExtEntitys
{
    public class MenuExt
    {
        public int MenuID { get; set; }

        public string MenuName { get; set; }

        public string MenuType { get; set; }

        public int? Status { get; set; }

        public int? ParentID { get; set; }

        public int? Sort { get; set; }

        public string MenuUrl { get; set; }

        public string MenuCode { get; set; }

        public int  roleID {get;set;}


        public List<MenuExt> Children { get; set; } = new List<MenuExt>();
    }
}
