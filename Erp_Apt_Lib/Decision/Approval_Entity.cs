using System;

namespace Erp_Apt_Lib.Decision
{
    /// <summary>
    /// 결재 구조 엔터티
    /// </summary>
    public class Approval_Entity
    {
        public int Num { get; set; }
        public string AptCode { get; set; }
        public string AptName { get; set; }
        public string Bloom { get; set; }
        public string Bloom_Code { get; set; }
        public string PostDuty { get; set; }
        public string Post { get; set; }
        public string Duty { get; set; }
        public int Step { get; set; }
        public string Intro { get; set; }
        public DateTime ModifyDate { get; set; }
        public string ModifyIP { get; set; }
        public DateTime PostDate { get; set; }
        public string PostIP { get; set; }
    }

}
