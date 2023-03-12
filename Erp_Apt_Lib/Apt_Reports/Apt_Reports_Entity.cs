using System;
using System.Collections.Generic;
using System.Text;

namespace Erp_Apt_Lib.Apt_Reports
{
    /// <summary>
    /// 보고서 정보
    /// </summary>
    public class Apt_Reports_Entity
    {
        /// <summary>
        /// 일련번호
        /// </summary>
        public int Aid { get; set; }

        /// <summary>
        /// 직원코드
        /// </summary>
        public string Staff_Code { get; set; }

        /// <summary>
        /// 보고서 코드
        /// </summary>
        public string Report_Bloom_Code { get; set; }

        /// <summary>
        /// 공동주택 코두
        /// </summary>
        public string Apt_Code { get; set; }

        /// <summary>
        /// 보고서 명
        /// </summary>
        public string Report_Title { get; set; }

        /// <summary>
        /// 보고서 내용 및 설명
        /// </summary>
        public string Report_Content { get; set; }

        /// <summary>
        /// 보고서 년도
        /// </summary>
        public string Report_Year { get; set; }

        /// <summary>
        /// 보고서 월
        /// </summary>
        public string Report_Month { get; set; }

        /// <summary>
        /// 보고서 구분 코드
        /// </summary>
        public string Report_Division_Code { get; set; }

        /// <summary>
        /// 입력일
        /// </summary>
        public DateTime PostDate { get; set; }

        /// <summary>
        /// 입력자 아이피
        /// </summary>
        public string PostIP { get; set; }

        public int FilesCount { get; set; }

        /// <summary>
        /// 보고서 결과
        /// </summary>
        public string Result { get; set; }

        /// <summary>
        /// 보고서 본사 승인 여부
        /// </summary>
        public string Complete { get; set; }

        /// <summary>
        /// 분류명
        /// </summary>
        public string Report_Bloom_Name { get; set; }

        /// <summary>
        /// 구분명
        /// </summary>
        public string Report_Division_Name { get; set; }

        /// <summary>
        /// 입력자 명
        /// </summary>
        public string User_Name { get; set; }

        /// <summary>
        /// 공동주택명
        /// </summary>
        public string Apt_Name { get; set; }
    }

    /// <summary>
    /// 보고서 분류
    /// </summary>
    public class Report_Bloom_Entity
    {
        /// <summary>
        /// 보고서 분류 코드
        /// </summary>
        public int Report_Bloom_Code { get; set; }

        /// <summary>
        /// 보고서 분류명
        /// </summary>
        public string Report_Bloom_Name { get; set; }

        /// <summary>
        /// 분류 설명
        /// </summary>
        public string Report_Bloom_Detail { get; set; }

        /// <summary>
        /// 입력일
        /// </summary>
        public DateTime PostDate { get; set; }

        /// <summary>
        /// 사용여부
        /// </summary>
        public string Views { get; set; }
    }

    /// <summary>
    /// 보고서 구분
    /// </summary>
    public class Report_Division_Entity
    {
        /// <summary>
        /// 보고서 구분 코드
        /// </summary>
        public int Report_Division_Code { get; set; }

        /// <summary>
        /// 보고서 구분명
        /// </summary>
        public string Report_Division_Name { get; set; }

        /// <summary>
        /// 보고서 구분 설명
        /// </summary>
        public string Report_Division_Detail { get; set; }

        /// <summary>
        /// 보고서 구분 입력일
        /// </summary>
        public DateTime PostDate { get; set; }

        /// <summary>
        /// 보고서 구분 사용여부
        /// </summary>
        public string Views { get; set; }
    }
}
