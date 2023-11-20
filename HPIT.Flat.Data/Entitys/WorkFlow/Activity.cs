using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HPIT.Flat.Data.Entitys.WorkFlow
{
    public class Activity
    {
        public string ActivityName { get; set; }

        public int ActivityType { get; set; }

        public string RoleName { get; set; }

        public List<Users> ActivityUsers { get; set; } = new List<Users>();

        public int ActivitySort { get; set; }
    }
}
