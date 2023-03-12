using System;
using System.Collections.Generic;
using System.Text;

namespace sw_Lib.Labors
{
    /// <summary>
    /// 최저임금 정보
    /// </summary>
    public class wage_Entity
    {
        public int Aid { get; set; }
        public int Year { get; set; }
        public int wage { get; set; }
        public DateTime PostDate { get; set; }
        public string Details { get; set; }
        public string User_Code { get; set; }
    }
}
