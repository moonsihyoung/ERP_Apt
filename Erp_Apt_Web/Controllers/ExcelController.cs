using Erp_Apt_App.Data;
using Erp_Apt_Lib.Community;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Erp_Apt_Web.Controllers
{
    [Route("Excel")]
    public class ExcelController : Controller
    {
        public int sss { get; set; } = 1;
        private ICommunity_Lib _community_Lib;

        public ExcelController(
            ICommunity_Lib community_Lib)
        {
            _community_Lib = community_Lib;
        }

        [Route("GetExcelFiles")]
        public async Task<IActionResult> GetExcelFiles(string AptCode, string StartDate, string EndDate)
        {
            //int Year = DateTime.Now.Year;
            //int month = DateTime.Now.Month;
            List<MonthTotalSum_Entity> c_data = await _community_Lib.Month_Sum(AptCode, StartDate, EndDate);
            byte[] data = await Community_Excel.Community_MonthExcel(c_data);
            string strFileName = AptCode + "_" + StartDate + ".xlsx";
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "", strFileName);
            System.IO.File.WriteAllBytes(filePath, data);

            byte[] bytes = System.IO.File.ReadAllBytes(filePath);

            return File(bytes, "application/octet-steam", strFileName);
        }

        [Route("GetExcelFilesView")]
        public async Task<IActionResult> GetExcelFilesView(string AptCode, string StartDate, string EndDate)
        {            
            List<Community_Entity> c_data = await _community_Lib.Month_Input_List(AptCode, StartDate, EndDate);
            byte[] data = await Community_Excel.Community_MonthExcel_View(c_data);
            string strFileName = AptCode + "_" + StartDate + "_Lilst.xlsx";
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "", strFileName);
            System.IO.File.WriteAllBytes(filePath, data);
            byte[] bytes = System.IO.File.ReadAllBytes(filePath);
            return File(bytes, "application/octet-steam", strFileName);
        }
    }
}
