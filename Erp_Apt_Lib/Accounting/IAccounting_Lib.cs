using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Erp_Apt_Lib.Accounting
{
    /// <summary>
    /// 지출결의서 종류
    /// </summary>
    public interface IDisbursementSort_Lib
    {

        /// <summary>
        /// 지출결의서 종류 입력
        /// </summary>
        /// <param name="ds"></param>
        /// <returns></returns>
        Task<int> Add(DisbursementSortEnity ds);

        /// <summary>
        /// 지출결의서 종류 수정
        /// </summary>
        /// <param name="ds"></param>
        Task Edit(DisbursementSortEnity ds);

        /// <summary>
        /// 공동주택별 지출결의서 종류 목록
        /// </summary>
        /// <param name="AptCode"></param>
        /// <returns></returns>
        Task<List<DisbursementSortEnity>> GetList(string AptCode);

        /// <summary>
        /// 공동주택별 지출결의서 종류 목록 수
        /// </summary>
        /// <param name="AptCode"></param>
        /// <returns></returns>
        Task<int> GetListCount(string AptCode);

        /// <summary>
        /// 공동주택별 지출결의서 종류 목록 수
        /// </summary>
        /// <param name="AptCode"></param>
        /// <returns></returns>
        Task<string> dbsortName(int Aid);

        /// <summary>
        /// 공동주택별 지출결의서 종류 목록 수
        /// </summary>
        string dbsort_Name(string Aid);

        /// <summary>
        /// 공동주택별 지출결의서 종류 목록
        /// </summary>
        /// <param name="AptCode"></param>
        /// <returns></returns>
        Task<List<DisbursementSortEnity>> GetList_Name(string AptCode);

        /// <summary>
        /// 지출결의서 종류 상세정보
        /// </summary>
        /// <param name="Aid"></param>
        /// <returns></returns>
        Task<DisbursementSortEnity> Details(int Aid);

        /// <summary>
        /// 삭제
        /// </summary>
        /// <param name="Aid"></param>
        Task Remove(int Aid);


    }

    /// <summary>
    /// 지출결의서 정보
    /// </summary>
    public interface IDisbursement_Lib
    {
        /// <summary>
        /// 지출결의서 입력하기
        /// </summary>
        /// <param name="de"></param>
        /// <returns></returns>
        Task<int> Add(DisbursementEntity de);

        /// <summary>
        /// 지출결의서 수정
        /// </summary>
        /// <param name="de"></param>
        Task Edit(DisbursementEntity de);

        /// <summary>
        /// 결재 완료
        /// </summary>
        /// <param name="Aid"></param>
        Task ApprovalEdit(int Aid);

        /// <summary>
        /// 결재여부 확인
        /// </summary>
        /// <param name="Aid"></param>
        /// <returns></returns>
        Task<string> Approval(int Aid);

        /// <summary>
        /// 지출결의서 목록(공동주택 단위별)
        /// </summary>
        Task<List<DisbursementEntity>> GetList(int Page, string AptCode);

        /// <summary>
        /// 지출결의서 목록 수
        /// </summary>
        Task<int> GetListCount(string AptCode);

        /// <summary>
        /// 지출결의서 상세
        /// </summary>
        /// <param name="Aid"></param>
        /// <returns></returns>
        Task<DisbursementEntity> Details(int Aid);

        /// <summary>
        /// 삭제
        /// </summary>
        /// <param name="Aid"></param>
        Task Remove(int Aid);

        /// <summary>
        /// 지출결의서 찾기
        /// </summary>
        /// <param name="Feild"></param>
        /// <param name="Query"></param>
        /// <returns></returns>
        Task<List<DisbursementEntity>> Search(string Feild, string Query);

        /// <summary>
        /// 지출결의서 년월 찾기
        /// </summary>
        /// <param name="InputYear"></param>
        /// <param name="InputMonth"></param>
        /// <returns></returns>
        Task<List<DisbursementEntity>> SearchDate(string InputYear, string InputMonth);

        /// <summary>
        /// 합계 금액 입력
        /// </summary>
        /// <param name="DisbursementCode"></param>
        /// <param name="InputSum"></param>
        Task Sum_InputSum(string DisbursementCode, double InputSum);

        /// <summary>
        /// 앞 지출결의서 정보
        /// </summary>
        /// <param name="AptCode"></param>
        /// <param name="Num"></param>
        /// <returns></returns>
        Task<string> Ago(string AptCode, string Aid);

        /// <summary>
        /// 앞 지출결의서  존재여부
        /// </summary>
        /// <param name="AptCode"></param>
        /// <param name="Num"></param>
        /// <returns></returns>
        Task<int> AgoBe(string AptCode, string Aid);

        /// <summary>
        /// 뒤 지출결의서
        /// </summary>
        Task<string> Next(string AptCode, string Aid);

        /// <summary>
        /// 뒤 지출결의서 존재 여부
        /// </summary>
        /// <param name="AptCode"></param>
        /// <param name="Num"></param>
        /// <returns></returns>
        Task<int> NextBe(string AptCode, string Aid);

        /// <summary>
        /// 지출결의서 종류 중 최근 정보 번호 구하기
        /// </summary>
        Task<int> Top_Code(string DisbursementName, string AptCode);
    }

    /// <summary>
    /// 계정과목 정보
    /// </summary>
    public interface IAccount_Lib
    {
        /// <summary>
        /// 계정과목 입력
        /// </summary>
        /// <param name="at"></param>
        /// <returns></returns>
        Task<int> Add(AccountEntity at);

        /// <summary>
        /// 수정
        /// </summary>
        /// <param name="at"></param>
        Task Edit(AccountEntity at);

        /// <summary>
        /// 계정과목 모록
        /// </summary>
        /// <returns></returns>
        Task<List<AccountEntity>> GetList(int Page);

        /// <summary>
        /// 계정과목 목록 수
        /// </summary>
        /// <returns></returns>
        Task<int> GetListCount();

        /// <summary>
        /// 계정과목 목록(사용)
        /// </summary>
        Task<List<AccountEntity>> GetListUsing(int Page);

        /// <summary>
        /// 계정과목 목록 수(사용)
        /// </summary>
        Task<int> GetListCountUsing();

        /// <summary>
        /// 대분류로 계정과목 검색하기
        /// </summary>
        /// <param name="SortA_Code"></param>
        /// <returns></returns>
        Task<List<AccountEntity>> GetList_SortA(int SortA_Code);

        /// <summary>
        /// 계정과목 대분류 검색된 목록 수
        /// </summary>
        /// <returns></returns>
        Task<int> GetList_SortA_Count(int SortA_Code);

        /// <summary>
        /// 중분류로 검색하기
        /// </summary>
        /// <param name="SortA_Code"></param>
        /// <returns></returns>
        Task<List<AccountEntity>> GetList_SortB(int SortB_Code);

        /// <summary>
        ///  중분류로 검색된 목록 수
        /// </summary>
        /// <returns></returns>
        Task<int> GetList_SortB_Count(int SortB_Code);

        /// <summary>
        /// 대분류 그리고 중분류로 계정과목 목록
        /// </summary>
        /// <param name="SortA_Code"></param>
        /// <param name="SortB_Code"></param>
        /// <returns></returns>
        Task<List<AccountEntity>> GetList_Sort_AB(int SortA_Code, int SortB_Code);

        /// <summary>
        /// 계정과목 상세보기
        /// </summary>
        /// <param name="Aid"></param>
        /// <returns></returns>
        Task<AccountEntity> Details(int Aid);

        /// <summary>
        /// 계정과목 명 불러오기
        /// </summary>
        /// <param name="Aid"></param>
        /// <returns></returns>
        Task<string> AccountName(int Aid);

        /// <summary>
        /// 계정과목 명 불러오기
        /// </summary>
        string Account_Name(string Aid);

        /// <summary>
        /// 계정과목 삭제
        /// </summary>
        /// <param name="Aid"></param>
        Task Remove(int Aid);


    }

    /// <summary>
    /// 계정과목 분류 정보 
    /// </summary>
    public interface IAccountSort_Lib
    {
        /// <summary>
        /// 계정 분류 입력
        /// </summary>
        /// <param name="ase"></param>
        /// <returns></returns>
        Task<int> Add(AccountSortEntity ase);

        /// <summary>
        /// 계정 분류 수정
        /// </summary>
        /// <param name="ase"></param>
        Task Edit(AccountSortEntity ase);

        /// <summary>
        /// 계정 분류 전체 목록
        /// </summary>
        /// <returns></returns>
        Task<List<AccountSortEntity>> GetList();

        /// <summary>
        /// 계정분류 구분별 목록
        /// </summary>
        Task<List<AccountSortEntity>> GetList_Division(string Division);

        /// <summary>
        /// 대분류로 중분류 목록 구하기
        /// </summary>
        /// <param name="Division"></param>
        /// <returns></returns>
        Task<List<AccountSortEntity>> GetList_Sort(string SortA);

        /// <summary>
        /// 계정분류 상세
        /// </summary>
        /// <param name="Aid"></param>
        /// <returns></returns>
       Task<AccountSortEntity> Details(int Aid);

        /// <summary>
        /// 분류명 불러오기
        /// </summary>
        /// <param name="Aid"></param>
        /// <returns></returns>
        Task<string> Sort_Name(string Aid);

        /// <summary>
        /// 분류명 불러오기
        /// </summary>
        string SortName(string Aid);

        /// <summary>
        /// 분류 삭제
        /// </summary>
        /// <param name="Aid"></param>
        Task Remove(int Aid);
    }

    /// <summary>
    /// 계정(지출) 거래 정보
    /// </summary>
    public interface IAccountDeals_Lib
    {
        /// <summary>
        /// 지출내역 상세 입력
        /// </summary>
        /// <param name="ade"></param>
        /// <returns></returns>
        Task<int> Add(AccountDealsEntity ade);

        /// <summary>
        /// 지출내역 상세 수정
        ///  </summary>
        /// <param name="ade"></param>
        /// <returns></returns>
        Task Edit(AccountDealsEntity ade);

        /// <summary>
        /// 지출결의서별 지출내역 상세정보
        /// </summary>
        Task<AccountDealsEntity> DealsDetails(int Aid, string AptCode);

        /// <summary>
        /// 지출결의서별 지출내역
        /// </summary>
        /// <param name="DisbursementCode"></param>
        /// <param name="AptCode"></param>
        /// <returns></returns>
        Task<List<AccountDealsEntity>> GetList_Apt_DBMC(string DisbursementCode, string AptCode);

        /// <summary>
        /// 입력된 수
        /// </summary>
        Task<int> GetList_Apt_DBMC_Count(string DisbursementCode, string AptCode);

        /// <summary>
        /// 지출결의서 별 총액
        /// </summary>
        Task<double> Sum_Apt_DBMC(string DisbursementCode, string AptCode);

        /// <summary>
        /// 지출결의서별 지급방법별 지출내역(현금 지출)
        /// </summary>
        /// <param name="DisbursementCode"></param>
        /// <param name="AptCode"></param>
        /// <returns></returns>
        Task<List<AccountDealsEntity>> GetList_Apt_DBMC_Provide_A(string DisbursementCode, string ProvideWay, string AptCode);

        /// <summary>
        /// 지출결의서별 지급방법별 지출내역(현금 지출) 수
        /// </summary>
        Task<int> GetList_Apt_DBMC_Provide_A_Count(string DisbursementCode, string ProvideWay, string AptCode);

        /// <summary>
        /// 지출결의서별 지급방법별 지출내역(자동이체)
        /// </summary>
        Task<List<AccountDealsEntity>> GetList_Apt_DBMC_Provide_B(string DisbursementCode, string ProvideWay, string AptCode);

        /// <summary>
        /// 지출결의서별 지급방법별 지출내역(현금 지출) 수
        /// </summary>
        Task<int> GetList_Apt_DBMC_Provide_B_Count(string DisbursementCode, string ProvideWay, string AptCode);

        /// <summary>
        /// 지출결의서별 지급방법별 지출내역 총액A(자동이체 제외하고 모두)
        /// </summary>
        Task<double> Sum_Apt_DBMC_Provide_A(string DisbursementCode, string ProvideWay, string AptCode);

        /// <summary>
        /// 지출결의서별 지급방법별 지출내역 총액B(자동이체 만)
        /// </summary>
        /// <param name="DisbursementCode"></param>
        /// <param name="AptCode"></param>
        /// <returns></returns>
        Task<double> Sum_Apt_DBMC_Provide_B(string DisbursementCode, string ProvideWay, string AptCode);

        /// <summary>
        /// 지출결의서별 은행통장별 지출내역 총액
        /// </summary>
        double Sum_Apt_DBMC_BankAccount(string DisbursementCode, string BankAccountCode, string AptCode);

        /// <summary>
        /// 지출결의서별 은행통장별 지출내역 총액(내부 계좌 이체 입금)
        /// </summary>
        double Sum_Apt_DBMC_inPut_BankAccount(string DisbursementCode, string BankAccountCode, string AptCode);

        /// <summary>
        /// 지출내역 삭제
        /// </summary>
        Task Remove(int Aid);

        /// <summary>
        /// 지출내역 삭제
        /// </summary>
        /// <param name="Aid"></param>
        Task RemoveAll(string DisbursementCode, string AptCode);

        /// <summary>
        /// 내부 이체 계좌 합계 구하기
        /// </summary>
        Task<double> InputBankAccountSum(string AptCode, string DisbursementCode, string BankAccountCode);

        /// <summary>
        /// 첨부파일 추가 또는 삭제
        /// </summary>
        Task FilesCount(string Aid, string Division);
    }

    /// <summary>
    /// 은행통장 정보
    /// </summary>
    public interface IBankAccount_Lib
    {
        /// <summary>
        /// 통장정보 입력
        /// </summary>
        Task<int> Add(BankAccountEntity ba);

        /// <summary>
        /// 통장정보 수정
        /// </summary>
        /// <param name="da"></param>
        Task Edit(BankAccountEntity da);

        /// <summary>
        /// 통장 목록만들기
        /// </summary>
        /// <param name="AptCode"></param>
        /// <returns></returns>
        Task<List<BankAccountEntity>> GetList(string AptCode);

        /// <summary>
        /// 상세정보
        /// </summary>
        /// <param name="Aid"></param>
        /// <returns></returns>
        Task<BankAccountEntity> Details(int Aid);

        /// <summary>
        /// 통장 삭제
        /// </summary>
        /// <param name="Aid"></param>
        Task Remove(int Aid);

        /// <summary>
        /// 통장 명
        /// </summary>
        /// <param name="Aid"></param>
        /// <returns></returns>
        Task<string> BankAccountName(int Aid);

        /// <summary>
        /// 통장 명(동기식)
        /// </summary>
        string BankAccount_Name(int Aid);

        /// <summary>
        /// 시재금 여부
        /// </summary>
        /// <param name="Aid"></param>
        /// <returns></returns>
        Task<string> InputDivision(string Aid);
    }

    /// <summary>
    /// 통장 거래 정보
    /// </summary>
    public interface IBankAccountDeals_Lib
    {
        /// <summary>
        /// 통장 내역 입력
        /// </summary>
        /// <param name="bade"></param>
        /// <returns></returns>
        Task<int> Add(BankAccountDealsEntity bade);

        /// <summary>
        /// 통장 내역 수정(전금액, 후금액, 지출합계, 상세, 아이피, 사용자코드)
        /// </summary>
        /// <param name="bade"></param>
        Task Edit(BankAccountDealsEntity bade);

        /// <summary>
        /// 통장 내역 보고
        /// </summary>
        /// <param name="AptCode"></param>
        /// <returns></returns>
        Task<List<BankAccountDealsEntity>> GetList(string AptCode);

        /// <summary>
        /// 상세보기(지출결의서 식별코드)
        /// </summary>
        Task<BankAccountDealsEntity> Details(string AptCode, string DisbursementCode, string BankAccountCode);

        /// <summary>
        /// 해당 지출결의서 코드로 존재 여부 확인
        /// </summary>
        /// <param name="AptCode"></param>
        /// <param name="DisbursementCode"></param>
        /// <returns></returns>
        Task<int> Being(string AptCode, string DisbursementCode);

        /// <summary>
        /// 해당 지출결의서 코드로 존재 여부 확인
        /// </summary>
        int Being_BankAccount(string AptCode, string DisbursementCode, string BankAccountCode);

        /// <summary>
        /// 지출결의서 별 통장거래 내역  불러오기
        /// </summary>
        /// <param name="DisbursementCode"></param>
        /// <returns></returns>
        Task<BankAccountDealsEntity> DetailsDBS(string DisbursementCode);

        /// <summary>
        /// 통장별 마지막 지급 후 잔액 구하기
        /// </summary>
        /// <param name="AptCode"></param>
        /// <param name="BankAccountCode"></param>
        /// <returns></returns>
        double Balance_Last(string AptCode, string DisbursementCode, string BankAccountCode);

        /// <summary>
        /// 통장별 마지막 지급 후 잔액 구하기(하나 전)
        /// </summary>
        Task<double> Balance_Last_Result(string AptCode, string Aid, string BankAccountCode);

        /// <summary>
        /// 통장 내역 존재 여부
        /// </summary>
        Task<int> Being_Last(string AptCode, string DisbursementCode, string BankAccountCode);

        /// <summary>
        /// 통장 내역 존재 여부(일련번호
        /// </summary>
        Task<int> Being_Aid(string AptCode, string DisbursementCode, string BankAccountCode);

        /// <summary>
        /// 해당 지출결의서에 통장별 지급 후 잔액 구하기
        /// </summary>
        /// <param name="AptCode"></param>
        /// <param name="BankAccountCode"></param>
        /// <returns></returns>
        double After_Balance_Last(string AptCode, string BankAccountCode, string DisbursementCode);

        /// <summary>
        /// 해당 지출결의서에 통장별 지급 전 잔액 구하기
        /// </summary>
        /// <param name="AptCode"></param>
        /// <param name="BankAccountCode"></param>
        /// <returns></returns>
        double Ago_Balance_Last(string AptCode, string BankAccountCode, string DisbursementCode);

        /// <summary>
        /// 해당 지출결의서에 통장별 지출액 구하기
        /// </summary>
        /// <param name="AptCode"></param>
        /// <param name="BankAccountCode"></param>
        /// <returns></returns>
        double InputSum_Last(string AptCode, string BankAccountCode, string DisbursementCode);

        /// <summary>
        /// 해당 지출결의서에 통장별 입금액 구하기
        /// </summary>
        /// <param name="AptCode"></param>
        /// <param name="BankAccountCode"></param>
        /// <returns></returns>
        double OutputSum_Last(string AptCode, string BankAccountCode, string DisbursementCode);

        /// <summary>
        /// 해당 지출결의서에 통장내역의 입력일 구하기
        /// </summary>
        /// <param name="AptCode"></param>
        /// <param name="BankAccountCode"></param>
        /// <returns></returns>
        DateTime InputDate(string AptCode, string BankAccountCode, string DisbursementCode);

        /// <summary>
        /// 삭제
        /// </summary>
        /// <param name="Aid"></param>
        Task Remove(int Aid);

        /// <summary>
        /// 삭제
        /// </summary>
        /// <param name="Aid"></param>
        Task Delete(string DisbursementCode, string AptCode);

        /// <summary>
        /// 결재 입력
        /// </summary>
        /// <param name="Aid"></param>
        Task ApprovalEdit(string DisbursementCode, string BankAccountCode);

        /// <summary>
        /// 결재여부 확인
        /// </summary>
        /// <param name="DisbursementCode"></param>
        /// <param name="BankAccountCode"></param>
        /// <returns></returns>
        string ApprovalView(string DisbursementCode, string BankAccountCode);

        /// <summary>
        /// 첨부파일 추가 또는 삭제
        /// </summary>
        Task FilesCount(string Aid, string strAid, string Division);

        /// <summary>
        /// 입력된 파일 수 불러오기
        /// </summary>
        int FileCount(string Aid, string Dv_Aid);

    }
}
