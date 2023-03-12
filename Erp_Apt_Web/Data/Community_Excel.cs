using Erp_Apt_Lib.Community;
using OfficeOpenXml;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Erp_Apt_App.Data
{
    public class Community_Excel
    {
        public static async Task<byte[]> Community_MonthExcel(List<MonthTotalSum_Entity> nitys)
        {
            var package = new ExcelPackage();
            var workSheet = package.Workbook.Worksheets.Add("Gallagher");

            List<string> list = new List<string>()
            {
                "A1", "B1", "C1"
            };

            workSheet.Cells[list[0]].Value = "Dong";
            workSheet.Cells[list[1]].Value = "Ho";
            workSheet.Cells[list[2]].Value = "TotalSum";

            int i = 1;
            foreach (var it in nitys)
            {
                workSheet.Cells[i + 1, 1].Value = it.Dong;
                workSheet.Cells[i + 1, 2].Value = it.Ho;
                workSheet.Cells[i + 1, 3].Value = it.TotalSum;
                i++;
            }

            //for (int i = 0; i < nitys.Count; i++)
            //{
            //    workSheet.Cells[i + 1, 1].Value = nitys[1].Dong;
            //    workSheet.Cells[i + 1, 2].Value = nitys[2].Ho;
            //    workSheet.Cells[i + 1, 3].Value = nitys[3].TotalSum;
            //}

            return await package.GetAsByteArrayAsync();
        }

        public static async Task<byte[]> Community_MonthExcel_View(List<Community_Entity> nitys)
        {
            var package = new ExcelPackage();
            var workSheet = package.Workbook.Worksheets.Add("Gallagher");

            List<string> list = new List<string>()
            {
                "A1", "B1", "C1", "D1", "E1", "F1", "G1", "H1"
            };

            workSheet.Cells[list[0]].Value = "지문코드";
            workSheet.Cells[list[1]].Value = "이름";
            workSheet.Cells[list[2]].Value = "동";
            workSheet.Cells[list[3]].Value = "호";
            workSheet.Cells[list[4]].Value = "핸드폰";
            workSheet.Cells[list[5]].Value = "이용시설";
            workSheet.Cells[list[6]].Value = "이용방법";
            workSheet.Cells[list[7]].Value = "이용료";


            int i = 1;
            foreach (var it in nitys)
            {
                workSheet.Cells[i + 1, 1].Value = it.UserCode;
                workSheet.Cells[i + 1, 2].Value = it.UserName;
                workSheet.Cells[i + 1, 3].Value = it.Dong;
                workSheet.Cells[i + 1, 4].Value = it.Ho;
                workSheet.Cells[i + 1, 5].Value = it.Mobile;
                workSheet.Cells[i + 1, 6].Value = it.UsingKindName;
                workSheet.Cells[i + 1, 7].Value = it.Ticket;
                workSheet.Cells[i + 1, 8].Value = it.UseCost;
                i++;
            }

            //for (int i = 0; i < nitys.Count; i++)
            //{
            //    workSheet.Cells[i + 1, 1].Value = nitys[1].Dong;
            //    workSheet.Cells[i + 1, 2].Value = nitys[2].Ho;
            //    workSheet.Cells[i + 1, 3].Value = nitys[3].TotalSum;
            //}

            return await package.GetAsByteArrayAsync();
        }
    }
}
