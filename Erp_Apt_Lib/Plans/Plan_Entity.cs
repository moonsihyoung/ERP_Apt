using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace Erp_Apt_Lib.Plans
{
    public class Plan_Entity
    {
        /// <summary>
        /// 일련번호
        /// </summary>
        public int Aid { get; set; }

        /// <summary>
        /// 공동주택코드
        /// </summary>
        public string Apt_Code { get; set; }

        /// <summary>
        /// 입력명
        /// </summary>
        public string User_Name { get; set; }

        /// <summary>
        /// 입력자 아이디
        /// </summary>
        public string User_Code { get; set; }

        /// <summary>
        /// 계획명
        /// </summary>
        public string Plan_Name { get; set; }

        /// <summary>
        /// 계획코드
        /// </summary>
        public string Plan_Code { get; set; }

        /// <summary>
        /// 집행년도
        /// </summary>
        public int Year { get; set; }

        /// <summary>
        /// 집행월
        /// </summary>
        public int Month { get; set; }

        /// <summary>
        /// 시작일
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// 종료일
        /// </summary>
        public DateTime EndDate { get; set; }

        /// <summary>
        /// 분류명
        /// </summary>
        public string Sort { get; set; }

        /// <summary>
        /// 분류명 코드
        /// </summary>
        public string Sort_Code { get; set; }

        /// <summary>
        /// 세분류
        /// </summary>
        public string Asort { get; set; }

        /// <summary>
        /// 세분류 코드
        /// </summary>
        public string Asort_Code { get; set; }

        /// <summary>
        /// 계획 설명
        /// </summary>
        public string Plan_Details { get; set; }

        /// <summary>
        /// 법정 계획 여부
        /// </summary>
        public string Law_Division { get; set; }

        /// <summary>
        /// 대분류
        /// </summary>
        public string BloomA { get; set; }

        /// <summary>
        /// 중분류
        /// </summary>
        public string BloomB { get; set; }

        /// <summary>
        /// 소분류
        /// </summary>
        public string BloomC { get; set; }

        /// <summary>
        /// 대분류 코드
        /// </summary>
        public string BloomA_Code { get; set; }        

        /// <summary>
        /// 중분류 코드
        /// </summary>
        public string BloomB_Code { get; set; }

        /// <summary>
        /// 소분류 코드
        /// </summary>
        public string BloomC_Code { get; set; }

        /// <summary>
        /// 부서
        /// </summary>
        public string Post { get; set; }

        /// <summary>
        /// 직책
        /// </summary>
        public string Duty { get; set; }

        /// <summary>
        /// 관리자
        /// </summary>
        public string Menager { get; set; }

        /// <summary>
        /// 부서
        /// </summary>
        public string W_Post { get; set; }

        /// <summary>
        /// 직책
        /// </summary>
        public string W_Duty { get; set; }

        /// <summary>
        /// 작업자
        /// </summary>
        public string Worker { get; set; }

        /// <summary>
        /// 기타
        /// </summary>
        public string Etc { get; set; }

        /// <summary>
        /// 입력일
        /// </summary>
        public DateTime PostDate { get; set; }

        /// <summary>
        /// 입력자 아이피
        /// </summary>
        public string PostIP { get; set; }

        /// <summary>
        /// 사용여부
        /// </summary>
        public string Using { get; set; }
    }

    /// <summary>
    /// 계획분류 
    /// </summary>
    public class Plan_Sort_Entity
    {
        /// <summary>
        /// 식별코드
        /// </summary>
        public int Aid { get; set; }

        /// <summary>
        /// 공동주택 코드
        /// </summary>
        public string Apt_Code { get; set; }

        /// <summary>
        /// 대분류명
        /// </summary>
        public string Sort_Name { get; set; }

        /// <summary>
        /// 대분류 코드
        /// </summary>
        public string Sort_Code { get; set; }        

        /// <summary>
        /// 세분류 코드
        /// </summary>
        public string  Asort_Code { get; set; }

        /// <summary>
        /// 법정여부
        /// </summary>
        public string Law_Division { get; set; }

        /// <summary>
        /// 설명
        /// </summary>
        public string Details { get; set; }

        /// <summary>
        /// 입력자 아이디
        /// </summary>
        public string User_Code { get; set; }

        /// <summary>
        /// 입력일
        /// </summary>
        public DateTime PostDate { get; set; }

        /// <summary>
        /// 입력자 아이피
        /// </summary>
        public string PostIP { get; set; }

        /// <summary>
        /// 구분
        /// </summary>
        public string Division { get; set; }

        /// <summary>
        /// 사용여부
        /// </summary>
        /// 
        public string Using { get; set; }
    }

    public class Plan_Man_Entity
    {
        /// <summary>
        /// 일련번호
        /// </summary>
        public int Aid { get; set; }

        /// <summary>
        /// 공주택코드
        /// </summary>
        public string Apt_Code { get; set; }

        /// <summary>
        /// 계획코드
        /// </summary>
        public string Plan_Code { get; set; }

        /// <summary>
        /// 부서
        /// </summary>
        public string Post { get; set; }

        /// <summary>
        /// 직책
        /// </summary>
        public string Duty { get; set; }

        /// <summary>
        /// 담당자 아이디
        /// </summary>
        public string User_Code { get; set; }

        /// <summary>
        /// 담당자 명
        /// </summary>
        public string User_Name { get; set; }

        /// <summary>
        /// 입력일
        /// </summary>
        public string PostDate { get; set; }

        /// <summary>
        /// 입력자 아이피
        /// </summary>
        public string PostIP { get; set; }

        /// <summary>
        /// 사용여부
        /// </summary>
        public string Using { get; set; }

        /// <summary>
        /// 단계 구분
        /// </summary>
        public string? Division { get; set; }
    }
}
