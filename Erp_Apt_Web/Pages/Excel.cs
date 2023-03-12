using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using OfficeOpenXml;
using Erp_Apt_Lib.Community;

namespace Erp_Apt_App.Pages
{
    [Route("/[controller]")]
    [ApiController]
    public class Excel : ControllerBase
    {
        private ICommunity_Lib _community_Lib;

        public Excel(
            ICommunity_Lib community_Lib)
        {
            _community_Lib = community_Lib;
        }
        [HttpPost]
        public async Task<FileContentResult> GenerateExcel(string Apt_Code, string strStartDate, string strEndDate)
        {
            List<MonthTotalSum_Entity> visits = await _community_Lib.Month_Sum(Apt_Code, strStartDate, strEndDate); //JsonSerializer.Deserialize<Dictionary<string, Community_Entity>>(json);
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            byte[] bytes;
            using (var package = new ExcelPackage())
            {
                var sheet = package.Workbook.Worksheets.Add("VisitSheet");
                sheet.Cells[1, 1].Value = "Dong";
                sheet.Cells[1, 2].Value = "Ho";
                sheet.Cells[1, 3].Value = "Sum";
                int row = 2;
                foreach (var item in visits)
                {
                    sheet.Cells[row, 1].Value = item.Dong;
                    sheet.Cells[row, 2].Value = item.Ho;
                    sheet.Cells[row, 3].Value = item.TotalSum;
                    row++;
                }
                bytes = await package.GetAsByteArrayAsync();
            }
            var file = new FileContentResult(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
            file.FileDownloadName = "Vist.xlsx";
            return file;
        }
    }
}
