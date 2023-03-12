using System;
using System.Collections.Generic;
using System.Text;

namespace Facilities
{
    /// <summary>
    /// 시설물 분류 정보
    /// </summary>
    public class Bloom_Entity
    {
        public int Num { get; set; }
        public string AptCode { get; set; }
        public string Apt_Name { get; set; }
        public string B_N_A_Name { get; set; }
        public string B_N_B_Name { get; set; }
        public string B_N_C_Name { get; set; }
        public string BloomA { get; set; }
        public string BloomB { get; set; }
        public string B_N_Code { get; set; }
        public string Bloom { get; set; }
        public string Bloom_Code { get; set; }
        public int Period { get; set; }
        public string Intro { get; set; }
        public DateTime? ModifyDate { get; set; }
        public string ModifyIP { get; set; }
        public DateTime PostDate { get; set; }
        public string PostIP { get; set; }

        /// <summary>
        /// 삭제여부
        /// </summary>
        public string Views { get; set; }

        /// <summary>
        /// 입력자 코드
        /// </summary>
        public string UserCode { get; set; }
    }
}
