using ExcelCake.Intrusive;
using HPIT.Flat.Data.Entitys;
using HPIT.Flat.Data.ExportEntitys;
using HPIT.Flat.Data.ExtEntitys;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HPIT.Flat.Data.Adapter
{
    public class RoomFeedBackDal
    {

        public static RoomFeedBackDal Instance = new RoomFeedBackDal();

        public FlatContext context { get; set; }
        public RoomFeedBackDal()
        {
            this.context = new FlatContext();
        }

        private string GetCellValue(ExcelWorksheet sheet, int row, int col)
        {
            try
            {
                return sheet.Cells[row, col].Value?.ToString();
            }
            catch (Exception ex)
            {
                return "";
            }
        }

        private decimal? GetDecimalCellValue(ExcelWorksheet sheet, int row, int col)
        {
            try
            {
                return Convert.ToDecimal(sheet.Cells[row, col].Value?.ToString());
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public List<RoomModel> ImportAdv(string fileName)
        {
            List<RoomModel> dorms = new List<RoomModel>();
            using (FileStream fs = new FileStream(fileName, FileMode.Open))
            {
                //ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                using (ExcelPackage package = new ExcelPackage(fs))
                {
                    for (int i = 2; i <= package.Workbook.Worksheets.Count - 1; ++i)
                    {
                        ExcelWorksheet sheet = package.Workbook.Worksheets[i];
                        if (sheet.Dimension == null)
                        {
                            continue;
                        }
                        for (int m = 4, n = sheet.Dimension.End.Row; m <= n; m++)
                        {
                            var model = new RoomModel();
                            model.Date = GetCellValue(sheet, m, 1);
                            model.RoomNo = GetCellValue(sheet, m, 2);
                            model.Remark = GetCellValue(sheet, m, 3);
                            model.Result = GetCellValue(sheet, m, 4);
                            model.Type = GetCellValue(sheet,m,5);
                            dorms.Add(model);
                        }
                    }
                }
            }
            return dorms;
        }

        public string Import(string fileName)
        {
            var list = ImportAdv(fileName); // ExcelHelper.GetList<RoomModel>(fileName);
            List<RoomFeedBack> dorms = new List<RoomFeedBack>();
            List<RoomFeedBack> exists = context.RoomFeedBack.ToList();
            foreach (var item in list)
            {
                if (!string.IsNullOrEmpty(item.Date))
                {
                    int month = Convert.ToInt32(item.Date.Split('.')[0]);
                    int day = Convert.ToInt32(item.Date.Split('.')[1]);
                    RoomFeedBack dorm = new RoomFeedBack();
                    dorm.RoomNo = item.RoomNo;
                    dorm.ID = Guid.NewGuid().ToString();
                    dorm.Remark = item.Remark;
                    dorm.Result = item.Result;
                    dorm.Type = item.Type;
                    dorm.HotelDate = new DateTime(DateTime.Now.Year, month, day);
                    if (!string.IsNullOrEmpty(dorm.RoomNo))
                    {
                        dorms.Add(dorm);
                    }
                }
            }
            context.RoomFeedBack.AddRange(dorms);
            int result = context.SaveChanges();
            return result > 0 ? "导入完成!" : "导入失败!";
        }

        public List<RoomDeduct> ImportDeductAdv(string fileName)
        {
            List<RoomDeduct> dorms = new List<RoomDeduct>();
            using (FileStream fs = new FileStream(fileName, FileMode.Open))
            {
                //ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                using (ExcelPackage package = new ExcelPackage(fs))
                {
                    for (int i = 2; i <= package.Workbook.Worksheets.Count - 1; ++i)
                    {
                        ExcelWorksheet sheet = package.Workbook.Worksheets[i];
                        if (sheet.Dimension == null)
                        {
                            continue;
                        }
                        for (int m = 3, n = sheet.Dimension.End.Row; m <= n; m++)
                        {
                            var model = new RoomDeduct();
                            model.Money = GetDecimalCellValue(sheet, m, 7);
                            model.Persons = GetCellValue(sheet, m, 8);
                            dorms.Add(model);
                        }
                    }
                }
            }
            return dorms;
        }

        public string ImportDeduct(string fileName)
        {
            var list = ImportDeductAdv(fileName);
            List<RoomDeduct> reducts = new List<RoomDeduct>();
            foreach (var item in list)
            {
                item.ID = Guid.NewGuid().ToString();
                item.Year = DateTime.Now.Year;
                item.Month = DateTime.Now.Month;
            }
            list = list.Where(r => !string.IsNullOrEmpty(r.Persons)).ToList();
            int year = DateTime.Now.Year;
            int month = DateTime.Now.Month;
            var delete = context.RoomDeduct.Where(r => r.Year == year && r.Month == month).ToList();
            context.RoomDeduct.RemoveRange(delete);
            context.RoomDeduct.AddRange(list);
            int result = context.SaveChanges();
            return result > 0 ? "导入完成!" : "导入失败!";
        }

        public List<PersonDeduct> GetPersonDeducts(int year,int month)
        {
            var list = context.RoomDeduct.Where(r=>r.Year == year && r.Month == month).ToList();
            List<PersonDeduct> personDeducts = new List<PersonDeduct>();
            foreach (var item in list)
            {
                var pp = item.Persons.Split(' ').ToList();
                decimal? avg = Math.Round((item.Money / pp.Count).Value,2);
                foreach (var p in pp)
                {
                    if (p == "")
                    {
                        continue;
                    }
                    var match = personDeducts.FirstOrDefault(r => r.Person == p);
                    if (match == null)
                    {
                        PersonDeduct pd = new PersonDeduct();
                        pd.DeductMoney = avg;
                        pd.Person = p;
                        personDeducts.Add(pd);
                    }
                    else
                    {
                        match.DeductMoney += avg;
                    }
                }
            }
            return personDeducts;
        }

        public List<GuestDateStatistic> TypeStatistic(string type,int year,int month)
        {
            List<GuestDateStatistic> Statistic = new List<GuestDateStatistic>();
            
            string sql = string.Format(@"  select HotelDate as ReportDate ,count(*) as Count from  [FlatDB].[dbo].[RoomFeedBack] where [Type]='{0}' and year(HotelDate) = {1} and month(HotelDate) = {2} group by HotelDate ", type,year,month);
            //string sql = @"select CompanyType name ,COUNT(CompanyType) value from [SurveyDB].[dbo].[Company] group by CompanyType";
            using (var context = new FlatContext())
            {
                Statistic = context.Database.SqlQuery<GuestDateStatistic>(sql).ToList();
            }
            return Statistic;
        }

        public List<GuestTypeStatistic> TypeRemarkStatistic(string type,int year,int month)
        {
            List<GuestTypeStatistic> Statistic = new List<GuestTypeStatistic>();
            string sql = string.Format(@" select Remark,count(*) as Count from  [FlatDB].[dbo].[RoomFeedBack] where [Type]='{0}' and year(HotelDate) = {1} and month(HotelDate) = {2} group by Remark ", type,year,month);
            //string sql = @"select CompanyType name ,COUNT(CompanyType) value from [SurveyDB].[dbo].[Company] group by CompanyType";
            using (var context = new FlatContext())
            {
                Statistic = context.Database.SqlQuery<GuestTypeStatistic>(sql).ToList();
            }
            return Statistic;
        }

        public List<GuestStatistic> GetGuestStatistics(int year,int month)
        {
            List<GuestStatistic> allDateStatistic = new List<GuestStatistic>();
            List<GuestDateStatistic> SatisfyStatistic = TypeStatistic("满意",year,month);
            List<GuestDateStatistic> SuggestStatistic = TypeStatistic("建议", year, month);
            List<GuestDateStatistic> OpinionStatistic = TypeStatistic("意见", year, month);
            foreach (var item in SatisfyStatistic)
            {
                GuestStatistic guestStatistic = new GuestStatistic();
                guestStatistic.ReportDate =item.ReportDate;
                guestStatistic.ReportDateStr = item.ReportDate.Month +"."+ item.ReportDate.Day ;
                guestStatistic.SatisfyCount = item.Count;
                GuestDateStatistic suggestMatch = SuggestStatistic.FirstOrDefault(r => r.ReportDate == item.ReportDate);
                if (suggestMatch != null)
                {
                    guestStatistic.SuggestCount = suggestMatch.Count;
                }
                GuestDateStatistic opinionMatch = OpinionStatistic.FirstOrDefault(r => r.ReportDate == item.ReportDate);
                if (opinionMatch != null)
                {
                    guestStatistic.OpinionCount = opinionMatch.Count;
                }
                guestStatistic.TotalCount = guestStatistic.SatisfyCount + guestStatistic.SuggestCount + guestStatistic.OpinionCount;
                allDateStatistic.Add(guestStatistic);
            }
            return allDateStatistic.OrderBy(r=>r.ReportDate).ToList();
        }
    }
}
