using System;
using System.Collections.Generic;
using System.Text;

namespace sw_Lib.Labors
{
    public class Labor_contract_Entity
    {
        /// <summary>
        /// 근로계약서 식별코드
        /// </summary>
        public int Aid { get; set; }
        /// <summary>
        /// 공동주택 식별코드
        /// </summary>
        public string Apt_Code { get; set; }
        /// <summary>
        /// 공동주택명
        /// </summary>
        public string Apt_Name { get; set; }
        /// <summary>
        /// 근로자 아이디
        /// </summary>
        public string UserID { get; set; }
        /// <summary>
        /// 근로자명
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 근로자 주소
        /// </summary>
        public string Adress { get; set; }
        /// <summary>
        /// 휴대폰번호
        /// </summary>
        public string Mobile { get; set; }
        /// <summary>
        /// 전화번호
        /// </summary>
        public string Telephone { get; set; }

        /// <summary>
        /// 촉탁여부
        /// </summary>
        public string PartTime { get; set; }

        /// <summary>
        /// 근로계약 시작일
        /// </summary>
        public DateTime LaborStartDate { get; set; }
        /// <summary>
        /// 근로계약 종료일
        /// </summary>
        public DateTime LaborEndDate { get; set; }
        /// <summary>
        /// 근무장소
        /// </summary>
        public string WorkPlace { get; set; }

        /// <summary>
        /// 근로내용
        /// </summary>
        public string WorkDetail { get; set; }

        /// <summary>
        /// 기본급
        /// </summary>
        public int BasicsPay { get; set; }
        /// <summary>
        /// 야간수당
        /// </summary>
        public int Pay_A { get; set; }
        /// <summary>
        /// 직책수당
        /// </summary>
        public int Pay_B { get; set; }
        /// <summary>
        /// 기타수당
        /// </summary>
        public int Pay_C { get; set; }
        /// <summary>
        /// 자격수당
        /// </summary>
        public int Pay_D { get; set; }
        /// <summary>
        /// 그외 수당
        /// </summary>
        public int Pay_E { get; set; }
        public int Pay_F { get; set; }
        /// <summary>
        /// 급여 합계
        /// </summary>
        public int TotalSum { get; set; }

        /// <summary>
        /// 기타 및 그외수당 설명
        /// </summary>
        public string Pay_Etc { get; set; }

        /// <summary>
        /// 근로시간 구분
        /// </summary>
        public string WorktimeSort { get; set; }
        /// <summary>
        /// 근로시간
        /// </summary>
        public double Worktime { get; set; }
        /// <summary>
        /// 토요일 근로시간
        /// </summary>
        public double WorktimeWeekend { get; set; }
        /// <summary>
        /// 월간 근로시간
        /// </summary>
        public double WorkMonthTime { get; set; }

        /// <summary>
        /// 근로시간 기타
        /// </summary>
        public string WorkTimeEtc { get; set; }

        /// <summary>
        /// 근로 시작 시간
        /// </summary>
        public string StartWorkTime { get; set; }

        /// <summary>
        /// 근로종료 시간
        /// </summary>
        public string EndWorkTime { get; set; }

        /// <summary>
        /// 휴게시간 구분
        /// </summary>
        public string BreaktimeSort { get; set; }
        /// <summary>
        /// 주간 휴게시간
        /// </summary>
        public double BreaktimeDaytime { get; set; }

        /// <summary>
        /// 휴게시간 기타
        /// </summary>    
        public string BreakTimeEtc { get; set; }

        /// <summary>
        /// 야간 휴게시간
        /// </summary>
        public double BreaktimeNight { get; set; }

        /// <summary>
        /// 법정 휴일 
        /// </summary>
        public string Holiday { get; set; }

        /// <summary>
        /// 정년 나이
        /// </summary>
        public int RetirementAge { get; set; }
        /// <summary>
        /// 계약내용 설명 들은 후 승인여부
        /// </summary>
        public Boolean Read_Approval { get; set; }

        /// <summary>
        /// 계약 사본을 받았습니다. 승인여부
        /// </summary>
        /// 
        public Boolean Copy_Approval { get; set; }

        /// <summary>
        /// 계약 사본을 받았습니다. 승인여부
        /// </summary>
        public Boolean Read_Approval1 { get; set; }

        /// <summary>
        /// 기타
        /// </summary>
        public string Contract_Etc { get; set; }
        /// <summary>
        /// 계약서 작성일
        /// </summary>
        public DateTime WorkContract_Date { get; set; }
        /// <summary>
        /// 회사 식별코드
        /// </summary>
        public string Cor_Code { get; set; }
        /// <summary>
        /// 회사 주소
        /// </summary>
        public string Ceo_Adress { get; set; }
        /// <summary>
        /// 회사 전화번호
        /// </summary>
        public string Ceo_Telephone { get; set; }
        /// <summary>
        /// 회사명
        /// </summary>
        public string Ceo_Company { get; set; }

        /// <summary>
        /// 사업주명
        /// </summary>
        public string Ceo_Name { get; set; }

        /// <summary>
        /// 승인과정 (A이면 미 승인, B이면 승인, C이면 승인중)
        /// </summary>
        public string ContractApprovalDivision { get; set; }

        /// <summary>
        /// 감단직 확인 서명 부분 동기화
        /// </summary>
        public string ContractNotice { get; set; }

        /// <summary>
        /// 입력일
        /// </summary>
        public DateTime PostDate { get; set; }
        /// <summary>
        /// 입력자 아이피
        /// </summary>
        public string PostIP { get; set; }

        /// <summary>
        /// A : 현재 근로 중, B : 근로계약 만료 
        /// </summary>
        public string Division { get; set; }

        /// <summary>
        /// 첨부된 파일 수
        /// </summary>
        public int Files_Count { get; set; }
    }
}
