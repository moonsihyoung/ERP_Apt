using System;

namespace Erp_Apt_Lib.Accounting
{

    /// <summary>
    /// 지출결의서 종류 속성
    /// </summary>
    public class DisbursementSortEnity
    {
        /// <summary>
        /// 식별코드
        /// </summary>
        public int Aid { get; set; }

        /// <summary>
        /// 공동주택 식별코드
        /// </summary>
        public string AptCode { get; set; }

        /// <summary>
        /// 지출명
        /// </summary>
        public string DisbursementName { get; set; }

        /// <summary>
        /// 지출명 상세정보
        /// </summary>
        public string Details { get; set; }

        /// <summary>
        /// 입력일
        /// </summary>
        public DateTime PostDate { get; set; }

        /// <summary>
        /// 입력자 아이피
        /// </summary>
        public string PostIP { get; set; }

        /// <summary>
        /// 입력자 아이디
        /// </summary>
        public string User_Code { get; set; }

        /// <summary>
        /// 사용여부
        /// </summary>

        public string del { get; set; }
    }

    /// <summary>
    /// 지출결의서 속성
    /// </summary>
    public class DisbursementEntity
    {
        public int Aid { get; set; }

        /// <summary>
        /// 공동주택 식별코드
        /// </summary>
        public string AptCode { get; set; }
        /// <summary>
        /// 지출명
        /// </summary>
        public string DisbursementName { get; set; }

        /// <summary>
        /// 기안 일자
        /// </summary>
        public DateTime DraftDate { get; set; }

        /// <summary>
        /// 집행일자
        /// </summary>
        public DateTime InputDate { get; set; }

        /// <summary>
        /// 집행 년도
        /// </summary>
        public int InputYear { get; set; }

        /// <summary>
        /// 집행 월
        /// </summary>
        public int InputMonth { get; set; }

        /// <summary>
        /// 지출 총액
        /// </summary>
        public double InputSum { get; set; }

        /// <summary>
        /// 기안자
        /// </summary>
        public string User_Code { get; set; }

        /// <summary>
        /// 기안자 명
        /// </summary>
        public string User_Name { get; set; }

        /// <summary>
        /// 기타 설명
        /// </summary>
        public string Details { get; set; }

        /// <summary>
        /// 결재여부
        /// </summary>
        public string Approval { get; set; }

        /// <summary>
        /// 입력일
        /// </summary>
        public DateTime PostDate { get; set; }

        /// <summary>
        /// 입력자 아이피
        /// </summary>
        public string PostIP { get; set; }
    }

    /// <summary>
    /// 계정과목 정보
    /// </summary>
    public class AccountEntity
    {
        /// <summary>
        /// 식별코드
        /// </summary>
        public int Aid { get; set; }

        /// <summary>
        /// 거래 구분
        /// </summary>
        public string AccountType { get; set; }

        /// <summary>
        /// 대분류명
        /// </summary>
        public string SortA { get; set; }

        /// <summary>
        /// 대분류 코드
        /// </summary>
        public int SortA_Code { get; set; }

        /// <summary>
        /// 중분류명
        /// </summary>
        public string SortB { get; set; }

        /// <summary>
        /// 중분류 코드
        /// </summary>
        public int SortB_Code { get; set; }

        /// <summary>
        /// 계정과목명
        /// </summary>
        public string AccountName { get; set; }

        /// <summary>
        /// 계정과목 설명
        /// </summary>
        public string Details { get; set; }

        /// <summary>
        /// 입력일
        /// </summary>
        public DateTime PostDate { get; set; }

        /// <summary>
        /// 입력자 아이피
        /// </summary>
        public string PostIp { get; set; }

        /// <summary>
        /// 입력자 아이디
        /// </summary>
        public string User_Code { get; set; }

        /// <summary>
        /// 사용여부
        /// </summary>

        public string del { get; set; }
    }

    /// <summary>
    /// 계정 분류 정보 속성
    /// </summary>
    public class AccountSortEntity
    {
        /// <summary>
        /// 식별코드
        /// </summary>
        public int Aid { get; set; }

        /// <summary>
        /// 구분
        /// </summary>
        public string Division { get; set; }

        /// <summary>
        /// 분류명
        /// </summary>
        public string SortName { get; set; }

        /// <summary>
        /// 상위분류 식별코드
        /// </summary>
        public int UpSort { get; set; }

        /// <summary>
        /// 분류명 설명
        /// </summary>
        public string Details { get; set; }

        /// <summary>
        /// 입력일
        /// </summary>
        public DateTime PostDate { get; set; }

        /// <summary>
        /// 입력자 아이피
        /// </summary>
        public string PostIp { get; set; }
       
        
        /// <summary>
        /// 입력자 아이디
        /// </summary>
        public string User_Code { get; set; }

        /// <summary>
        /// 사용여부
        /// </summary>
        public string del { get; set; }

    }

    /// <summary>
    /// 지출내역 상세 속성
    /// </summary>
    public class AccountDealsEntity
    {
        /// <summary>
        /// 식별코드
        /// </summary>
        public int Aid { get; set; }

        /// <summary>
        /// 공동주택 식별코드
        /// </summary>
        public string AptCode { get; set; }

        /// <summary>
        /// 계정과목 대분류
        /// </summary>
        public string AccountSortCodeA { get; set; }

        /// <summary>
        /// 계정과목 중분류
        /// </summary>
        public string AccountSortCodeB { get; set; }

        /// <summary>
        /// 계정과목 식별코드
        /// </summary>
        public string AccountCode { get; set; }

        /// <summary>
        /// 대체계정과목 식별코드
        /// </summary>
        public string SubstitutionAccountCode { get; set; }

        /// <summary>
        /// 지출결의서 식별코드
        /// </summary>
        public string DisbursementCode { get; set; }

        /// <summary>
        /// 통장 식별코드
        /// </summary>
        public string BankAccountCode { get; set; }

        /// <summary>
        /// 지급처 식별코드
        /// </summary>
        public string CompanyCode { get; set; }

        /// <summary>
        /// 지급방법
        /// </summary>
        public string ProvideWay { get; set; }

        /// <summary>
        /// 지출 금액
        /// </summary>
        public double InputSum { get; set; }

        /// <summary>
        /// 지출처
        /// </summary>
        public string ProvidePlace { get; set; }


        /// <summary>
        /// 지급일
        /// </summary>
        public DateTime ExecutionDate { get; set; }

        /// <summary>
        /// 내외부 지급 여부(A : 외부지급, B : 내부지급)
        /// </summary>
        public string InOutDivision { get; set; }

        /// <summary>
        /// 내부 통장 코드
        /// </summary>
        public string InputBankAccountCode { get; set; }

        /// <summary>
        /// 지출 상세
        /// </summary>
        public string Details { get; set; }

        /// <summary>
        /// 입력일
        /// </summary>
        public DateTime PostDate { get; set; }

        /// <summary>
        /// 입력자 아이피
        /// </summary>
        public string PostIp { get; set; }

        /// <summary>
        /// 집행자
        /// </summary>
        public string User_Code { get; set; }

        /// <summary>
        /// 첨부파일 수
        /// </summary>
        public int Files_Count { get; set; }

    }

    /// <summary>
    /// 통장 정보 속성
    /// </summary>
    public class BankAccountEntity
    {
        /// <summary>
        /// 식별코드
        /// </summary>
        public int Aid { get; set; }

        /// <summary>
        /// 공동주택 코드
        /// </summary>
        public string AptCode { get; set; }

        /// <summary>
        /// 은행명
        /// </summary>
        public string BankName { get; set; }

        /// <summary>
        /// 통장명
        /// </summary>
        public string BankAccountName { get; set; }

        /// <summary>
        /// 계좌번호
        /// </summary>
        public string BankNumber { get; set; }

        /// <summary>
        /// 통장 종류(장충, 일반관리비, 잡수입 등)
        /// </summary>
        public string BankAccountSort { get; set; }

        /// <summary>
        /// 통장구분(적금, 자유예금 등)
        /// </summary>
        public string BankAccountDivision { get; set; }

        /// <summary>
        /// 시재금 여부
        /// </summary>
        public string InputDivision { get; set; }

        /// <summary>
        /// 통장 설명
        /// </summary>
        public string Details { get; set; }


        /// <summary>
        /// 입력일
        /// </summary>
        public DateTime PostDate { get; set; }

        /// <summary>
        /// 입력자 아이피
        /// </summary>
        public string PostIp { get; set; }

        /// <summary>
        /// 입력자 아이디
        /// </summary>
        public string User_Code { get; set; }

        public string del { get; set; }
    }

    /// <summary>
    /// 통장거래 내역 속성
    /// </summary>
    public class BankAccountDealsEntity
    {
        /// <summary>
        /// 식별코드
        /// </summary>
        public int Aid { get; set; }
        /// <summary>
        /// 공동주택 식별코드
        /// </summary>
        public string AptCode { get; set; }
        /// <summary>
        /// 지출결의서 식별코드
        /// </summary>
        public string DisbursementCode { get; set; }

        /// <summary>
        /// 통장 식별코드
        /// </summary>
        public string BankAccountCode { get; set; }

        /// <summary>
        /// 이전 잔고
        /// </summary>
        public double Ago_Balance { get; set; }

        /// <summary>
        /// 이후 잔고
        /// </summary>
        public double After_Balance { get; set; }

        /// <summary>
        /// 지출 총액
        /// </summary>
        public double InputSum { get; set; }

        /// <summary>
        /// 입금 총액
        /// </summary>
        public double OutputSum { get; set; }

        /// <summary>
        /// 상세정보
        /// </summary>
        public string Details { get; set; }

        /// <summary>
        /// 입력일
        /// </summary>
        public DateTime PostDate { get; set; }

        /// <summary>
        /// 입력자 아이피
        /// </summary>
        public string PostIp { get; set; }

        /// <summary>
        /// 입력자 아이디
        /// </summary>
        public string User_Code { get; set; }

        /// <summary>
        /// 첨부파일 수
        /// </summary>
        public int Files_Count { get; set; }
    }
}
