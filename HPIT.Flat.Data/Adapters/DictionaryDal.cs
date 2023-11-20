using HPIT.Data.Core;
using HPIT.Flat.Data.Entitys;
using System.Collections.Generic;
using System.Linq;

namespace HPIT.Flat.Data.Adapters
{
    public class DictionaryDal
    {
        public static DictionaryDal Instance = new DictionaryDal();

        public FlatContext context { get; set; }

        public DictionaryDal()
        {
            this.context = new FlatContext();
        }
        /// <summary>
        /// 查询列表
        /// </summary>
        /// <returns></returns>
        public static List<Dictionary> GetByList(string Type="")
        {
            //查询数据
            List<Dictionary> list = new List<Dictionary>();
            DBBaseService baseService = new DBBaseService(FlatContext.Instance);
            string sql = "";
            if (Type=="")
            {
                 sql = string.Format("select * from Dictionary ");
            }
            else
            {
                 sql = string.Format("select * from Dictionary where Type='{0}'" ,Type);
            }
            
            using (var context = new FlatContext())
            {
                list = context.Database.SqlQuery<Dictionary>(sql).ToList();
            }
            return list;
        }
        /// <summary>
        /// 根据name查询
        /// </summary>
        /// <param name="Name"></param>
        /// <returns>添加去重时使用</returns>
        public Dictionary GetDictName(string Name)
        {
            var dict = context.Dictionary.FirstOrDefault(r => r.Name == Name);
            return dict;
        }
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="Dict"></param>
        /// <returns></returns>
        public int Ins(Dictionary Dict)
        {
            var a = GetDictName(Dict.Name);
            if (a == null)
            {
                Dictionary model = new Dictionary()
                {
                    ID = Dict.ID,
                    Name = Dict.Name,
                    Type = Dict.Type,
                    ParentID = Dict.ParentID,
                    Status = Dict.Status,
                    Value = Dict.Value
                };
                context.Dictionary.Add(model);
                return context.SaveChanges();
            }
            else
            {
                return -1;
            }
        }
        /// <summary>
        /// 根据id查询
        /// </summary>
        /// <param name="ID"></param>
        /// <returns>修改渲数据时使用</returns>
        public Dictionary GetDictID(int ID)
        {
            var dict = context.Dictionary.FirstOrDefault(r => r.ID == ID);
            return dict;
        }
        /// <summary>
        /// 更新/修改
        /// </summary>
        /// <param name="Dict"></param>
        /// <returns></returns>
        public int Upd(Dictionary Dict)
        {

            Dictionary model = new Dictionary()
            {
                ID = Dict.ID,
                Name = Dict.Name,
                Type = Dict.Type,
                ParentID = Dict.ParentID,
                Status = Dict.Status,
                Value = Dict.Value
            };
            context.Entry(model).State = System.Data.Entity.EntityState.Modified;
            return context.SaveChanges();
        }
        /// <summary>
        /// 启用/禁用
        /// </summary>
        /// <param name="id"></param>
        /// <param name="Status"></param>
        /// <returns></returns>
        public int UpdStatus(int id, int Status = 0)
        {
            string sql = "";
            if (Status == 0)
            {
                sql = "update Dictionary set Status=1 where ID=" + id;
            }
            else
            {
                sql = "update Dictionary set Status=0 where ID=" + id;
            }
            int result = (int)context.Database.ExecuteSqlCommand(sql);
            return result;
        }
        /// <summary>
        /// 根据ID删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int Del(int id)
        {
            var num = context.Dictionary.FirstOrDefault(p => p.ID == id);
            context.Dictionary.Remove(num);
            return context.SaveChanges();
        }



    }
}
