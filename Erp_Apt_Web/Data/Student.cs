using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
//using Microsoft.JSInterop;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;

namespace Erp_Apt_App.Data
{
    //[Inject] IJSRuntime JSRuntime { get; set; }
    public class Student
    {
        public void GenerateExcel(IJSRuntime iJSRuntime)
        {
            byte[] fileContents;
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (var package = new ExcelPackage())
            {
                var workSheet = package.Workbook.Worksheets.Add("Sheet1");

                #region Header Row
                workSheet.Cells[1, 1].Value = "Student Name";
                workSheet.Cells[1, 1].Style.Font.Size = 12;
                workSheet.Cells[1, 1].Style.Font.Bold = true;
                workSheet.Cells[1, 1].Style.Border.Top.Style = ExcelBorderStyle.Hair;

                workSheet.Cells[1, 2].Value = "Student Roll";
                workSheet.Cells[1, 2].Style.Font.Size = 12;
                workSheet.Cells[1, 2].Style.Font.Bold = true;
                workSheet.Cells[1, 2].Style.Border.Top.Style = ExcelBorderStyle.Hair;
                #endregion

                #region Body 1st Row
                workSheet.Cells[2, 1].Value = "Shakib";
                workSheet.Cells[2, 1].Style.Font.Size = 12;
                workSheet.Cells[2, 1].Style.Border.Top.Style = ExcelBorderStyle.Hair;

                workSheet.Cells[2, 2].Value = "1001";
                workSheet.Cells[2, 2].Style.Font.Size = 12;
                workSheet.Cells[2, 2].Style.Border.Top.Style = ExcelBorderStyle.Hair;
                #endregion

                #region Body 21st Row
                workSheet.Cells[3, 1].Value = "Rohit";
                workSheet.Cells[3, 1].Style.Font.Size = 12;
                workSheet.Cells[3, 1].Style.Border.Top.Style = ExcelBorderStyle.Hair;

                workSheet.Cells[3, 2].Value = "1002";
                workSheet.Cells[3, 2].Style.Font.Size = 12;
                workSheet.Cells[3, 2].Style.Border.Top.Style = ExcelBorderStyle.Hair;
                #endregion

                fileContents = package.GetAsByteArray();
            }

            
            iJSRuntime.InvokeAsync<Student>(
                    "saveAsFile",
                    "Student List.xlsx",
                    Convert.ToBase64String(fileContents)
                    );
        }
    }
}
