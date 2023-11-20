using ExcelCake.Intrusive;
using HPIT.Data.Core;
using HPIT.Data.Core.Tool;
using HPIT.Evalute.Data;
using HPIT.Evalute.Data.Model;
using HPIT.Flat.Data.Entitys;
using HPIT.Flat.Data.ExportEntitys;
using HPIT.Flat.Data.ExtEntitys;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using static HPIT.Flat.Data.Entitys.Enumerations;

namespace HPIT.Flat.Data.Adapters
{
    public class DormDal
    {
        public static DormDal Instance = new DormDal();

        public FlatContext context { get; set; }
        public DormDal()
        {
            this.context = new FlatContext();
        }

        public List<Dorm> GetPageData(SearchModel<Dorm> search, out int count)
        {
            GetPageListParameter<Dorm, string> parameter = new GetPageListParameter<Dorm, string>();
            parameter.isAsc = true;
            parameter.orderByLambda = t => t.DormNo;
            parameter.pageIndex = search.PageIndex;
            parameter.pageSize = search.PageSize;
            parameter.whereLambda = t => t.Status > -1;
            //添加宿舍类型的筛选条件，并进行查询
            if (search.Entity.DormType >= 0)
            {
                Expression<Func<Dorm, bool>> dormTypeWhere = item => item.DormType == search.Entity.DormType;
                parameter.whereLambda = ExpressionExt.ReBuildExpression<Dorm>(parameter.whereLambda, dormTypeWhere);
            }
            DBBaseService baseService = new DBBaseService(FlatContext.Instance);
            List<Dorm> list = baseService.GetSimplePagedData<Dorm, string>(parameter, out count);
            List<DormAssign> assigns = context.DormAssign.ToList();
            foreach (var dorm in list)
            {
                dorm.MatchSize = assigns.Where(r => r.DormNo == dorm.DormNo && (r.Status == (int)LiveStatus.live || r.Status == (int)LiveStatus.none)).Count();
                if (dorm.MatchSize == 0)
                {
                    dorm.Status = (int)DormStatus.none;
                }
                else if (dorm.MatchSize < dorm.DormSize)
                {
                    dorm.Status = (int)DormStatus.part;
                }
                else
                {
                    dorm.Status = (int)DormStatus.use;
                }
                dorm.SizeStr = EnumDescriptionAttribute.GetDescription((DormSize)dorm.DormSize);
                dorm.StatusStr = EnumDescriptionAttribute.GetDescription((DormStatus)dorm.Status);
                dorm.TypeStr = EnumDescriptionAttribute.GetDescription((DormType)dorm.DormType);
                dorm.DormJsonStr = JsonConvert.SerializeObject(dorm);
            }
            return list;
        }

        public int AddDorm(Dorm dorm)
        {
            ///添加地址和公寓的编号判断进行去重
            int num = 0;
            var a = GetDormByNo(dorm.DormNo);
            if (a != null)
            {
                if (a.DormName != dorm.DormName)
                {
                    num = 44;
                }
            }
            else
            {
                dorm.Status = (int)DormStatus.none;
                dorm.DID = Guid.NewGuid().ToString();
                context.Dorm.Add(dorm);
                num = context.SaveChanges();
            }
            return num;

        }

        /// <summary>
        /// 根据学生姓名 和 学号 
        /// 匹配对应的宿舍分配信息
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="userNo"></param>
        /// <returns></returns>
        public AssignRelate GetDormAssignByUser(string userName,string userNo)
        {
            AssignRelate ar = new AssignRelate();
            ar.assign = context.DormAssign.FirstOrDefault(r => r.StuName == userName && r.StuNo == userNo);
            ar.dorm = context.Dorm.FirstOrDefault(r => r.DID == ar.assign.DID);
            PayRequest pay = context.PayRequest.FirstOrDefault(r => r.StuName == userName && r.StuNo == userNo);
            ar.TotalPayMoney = pay == null ? 0 : (decimal)pay.Payment.Where(r=>r.AuditStatus == 3).Sum(r => r.PayMoney);
            return ar;
        }

        /// <summary>
        /// 根据公寓编号
        /// </summary>
        /// <param name="dormNo"></param>
        /// <returns></returns>
        public Dorm GetDormByNo(string dormNo)
        {
            var dorm = context.Dorm.FirstOrDefault(r => r.DormNo == dormNo);
            return dorm;
        }

        public int UpdateDorm(Dorm dorm)
        {
            int result = 0;
            Dorm match = context.Dorm.FirstOrDefault(r => r.DID == dorm.DID);
            if (match != null)
            {
                match.DormNo = dorm.DormNo;
                match.DormSize = dorm.DormSize;
                match.DormType = dorm.DormType;
                match.Phone = dorm.Phone;
                match.Remark = dorm.Remark;
                match.DormName = dorm.DormName;
                match.RentMoney = dorm.RentMoney;
                match.DepositMoney = dorm.DepositMoney;
                context.Entry(match).State = System.Data.Entity.EntityState.Modified;
                result = context.SaveChanges();
            }
            return result;
        }
        /// <summary>
        /// 添加公寓分配
        /// </summary>
        /// <param name="assigns"></param>
        /// <returns></returns>
        /// Differenttypes人员性别与宿舍性别类型不同(男女混寝)
        /// Novacancy房间已住满
        /// succeed操作成功
        public string DormAssign(List<DormAssign> assigns, string DID)
        {
            //返回值
            var bc = "";
            //查询房间已存在人数
            var count = context.DormAssign.Where(p => p.DID == DID);
            //查询房间类型
            var type = context.Dorm.Where(p => p.DID == DID).FirstOrDefault();
            //统计当天要添加得人数
            var num = assigns.Count();
            //统计新增几人
            var js = 0;
            foreach (var item in assigns)
            {
                item.AID = Guid.NewGuid().ToString();
                item.CreateTime = DateTime.Now;
                item.Status = (int)LiveStatus.live;
                item.PeriodType = item.PeriodType + 2;
                //为什么 有外键的要查出来数据 才能在外表里面进行插入。
                //item.Dorm = context.Dorm.Where(r => r.DID == item.DID).FirstOrDefault();
                //判断男女类型对比
                //var gender = EvaluteDal.Instance.StudentSexStuID(item.StuNo);
                if (!Convert.ToBoolean(type.DormType) != item.Gender)
                {
                    bc = "Differenttypes";
                }
                js += 1;
            }
            if (type.DormSize == 0)
            {
                if ((count.Count() + num) > 4)
                {
                    bc = "Novacancy";
                }
            }
            if (type.DormSize == 1)
            {
                if ((count.Count() + num) > 6)
                {
                    bc = "Novacancy";
                }
            }
            if (type.DormSize == 2)
            {
                if ((count.Count() + num) > 8)
                {
                    bc = "Novacancy";
                }
            }
            //不符合条件返回，不能保存到数据库
            if (!string.IsNullOrEmpty(bc))
            {
                return bc;
            }
            //批量添加宿舍分配记录
            context.DormAssign.AddRange(assigns);
            var dorm = context.Dorm.Where(p => p.DID == DID).ToList().FirstOrDefault();
            //drom.DormType = 2;
            //修改入住状态
            if (bc == "Novacancy")
            {
                dorm.Status = 2;
            }
            else
            {
                dorm.Status = 1;
            }
            context.Entry(dorm).State = System.Data.Entity.EntityState.Modified;
            context.SaveChanges();
            return bc;

        }
        /// <summary>
        /// 删除宿舍信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int DormDelete(string id)
        {
            var dorm = context.Dorm.FirstOrDefault(p => p.DID == id);
            var dormA = context.DormAssign.Where(p => p.DID == id);
            foreach (var item in dormA)
            {
                context.DormAssign.Remove(item);
            }
            context.Dorm.Remove(dorm);
            return context.SaveChanges();
        }
        /// <summary>
        /// 查询房间内得人数
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<DormAssign> dormAssignsList(string id)
        {
            var dormA = context.DormAssign.Where(p => p.DID == id && p.Status == (int)LiveStatus.live).ToList();
            return dormA;
        }
        /// <summary>
        /// 查询所有房间
        /// </summary>
        /// <returns></returns>
        public List<Dorm> dormsList()
        {
            var dorm = context.Dorm.ToList();
            return dorm;
        }

        public List<DormAssign> AllAssigns()
        {
            return context.DormAssign.Where(r => r.Status == (int)LiveStatus.live).ToList();
        }
        /// <summary>
        /// 删除该宿舍内得人数
        /// </summary>
        /// <param name="aid"></param>
        /// <returns></returns>
        public int DormAssignDelete(string aid)
        {
            var dromassing = context.DormAssign.Where(p => p.AID == aid).FirstOrDefault();
            context.DormAssign.Remove(dromassing);
            return context.SaveChanges();
        }
        /// <summary>
        /// 转宿操作
        /// </summary>
        /// <param name="did"></param>
        /// <param name="aid"></param>
        /// <returns></returns>
        /// alike存在相同的房间
        /// Differenttypes人员性别与宿舍性别类型不同(男女混寝)
        /// RoomType与原宿舍房间类型不同
        /// succeed操作成功
        public string DormAssingnUpdate(string StuNo, string DormNo)
        {
            var dorm = context.Dorm.Where(p => p.DormNo == DormNo);
            //判断房间是否存在
            if (dorm.ToList().Count() == 0)
            {
                return "alike";
            }
            //当前宿舍类型
            var xdorm = dorm.FirstOrDefault();
            //之前的房间类型
            var adid = context.DormAssign.Where(a => a.StuNo == StuNo);
            var dormList = context.Dorm.Where(p => p.DID == adid.FirstOrDefault().DID).FirstOrDefault();
            //判断类型是否一致
            var sex = 0;
            if (adid.FirstOrDefault().Gender != true)
            {
                sex = 1;
            }
            if (xdorm.DormType == sex)
            {
                return "Differenttypes";
            }
            if (xdorm.DormSize != dormList.DormSize)
            {
                return "RoomType";
            }
            DormAssign dormAssign = new DormAssign()
            {
                StuNo = adid.FirstOrDefault().StuNo,
                DID = dorm.FirstOrDefault().DID,
                StuName = adid.FirstOrDefault().StuName,
                DormNo = dorm.FirstOrDefault().DormNo,
                PEM = adid.FirstOrDefault().PEM,
                PeriodType = Convert.ToInt32(adid.FirstOrDefault().PeriodType),
                AID = Guid.NewGuid().ToString(),
                CreateTime = DateTime.Now,
            };
            context.DormAssign.Add(dormAssign);
            context.DormAssign.Remove(adid.FirstOrDefault());
            context.SaveChanges();
            return "succeed";
        }

        /// <summary>
        /// 导入宿舍数据
        /// </summary>
        /// <param name="fileName"></param>
        public string Import(string fileName)
        {
            var list = ExcelHelper.GetList<DormModel>(fileName);
            List<Dorm> dorms = new List<Dorm>();
            List<Dorm> exists = context.Dorm.ToList();
            foreach (var item in list)
            {
                Dorm dorm = new Dorm();
                dorm.DormNo = item.DormNo;
                dorm.DID = Guid.NewGuid().ToString();
                dorm.DormSize = item.Size;
                dorm.DormType = item.Gender == "男生宿舍" ? 0 : 1;
                dorm.Phone = item.Phone;
                dorm.Remark = item.Remark;
                dorm.DormName = item.Address;
                dorm.Status = 1;
                dorm.DepositMoney = item.DepositMoney;
                dorm.RentMoney = item.RentMoney;
                if (!exists.Any(r => r.DormNo == dorm.DormNo))
                {
                    dorms.Add(dorm);
                }
            }
            context.Dorm.AddRange(dorms);
            int result = context.SaveChanges();
            return result > 0 ? "导入完成!" : "导入失败!";
        }
    }
}
