using System;
using System.Collections.Generic;
using System.Text;

namespace Erp_Apt_Lib.Decision
{
    /// <summary>
    /// 결재 엔터티
    /// </summary>
    public class Decision_Entity
    {
        public int Num { get; set; }
        public string AptCode { get; set; }
        public string Parent { get; set; }
        public string BloomCode { get; set; }
        public string UserName { get; set; }
        public string PostDuty { get; set; }

        public string Comform { get; set; }
        public string User_Code { get; set; }
        public DateTime ModifyDate { get; set; }
        public string ModifyIP { get; set; }
        public DateTime PostDate { get; set; }
        public string PostIP { get; set; }
    }

    /// <summary>
    /// 결재도장 엔터티
    /// </summary>
    public class DbImagesEntity
    {
        public int Aid { get; set; }
        public string FileName { get; set; }
        public string FileType { get; set; }
        public byte[] Photo { get; set; }
        public string AptCode { get; set; }
        public string User_Code { get; set; }
        public DateTime PostDate { get; set; }
        public string PostIP { get; set; }
    }
}
