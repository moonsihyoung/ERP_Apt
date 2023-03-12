using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Company
{
    /// <summary>
    /// 계약 정보 인터페이스
    /// </summary>
    public interface ICompany_Career_Lib
    {
        /// <summary>
        /// 계약정보 등록
        /// </summary>
        Task<Company_Career_Entity> add(Company_Career_Entity cc);

        /// <summary>
        /// 계약정보 수정
        /// </summary>
        /// <param name="cc"></param>
        /// <returns></returns>
        Task<Company_Career_Entity> edit(Company_Career_Entity cc);

        /// <summary>
        /// 계약 정보 상세보기
        /// </summary>
        Task<Company_Career_Entity> detail(string CC_Code);

        /// <summary>
        /// 가장 최근 계약 정보 상세보기(위탁관리만)
        /// </summary>
        Task<Company_Career_Entity> detail_Apt(string Apt_Code, string ContractSort);

        /// <summary>
        /// 해당 공동주택의 현 위탁계약서 존재 여부 확인
        /// </summary>
        Task<int> detail_Apt_Count(string Apt_Code, string ContractSort, string Contract_start_date, string Contract_end_date);

        /// <summary>
        /// 계약 정보 전체 목록
        /// </summary>
        Task<List<Company_Career_Entity>> getlist_all(int Page, string Division);

        /// <summary>
        /// 계약 정보 전체 수
        /// </summary>
        Task<int> Getcount_all(string Division);

        /// <summary>
        /// 계약 정보 옵션 목록
        /// </summary>
        Task<List<Company_Career_Entity>> getlist_option(int Page, string Feild, string Query);

        /// <summary>
        /// 계약 정보 옵션 목록
        /// </summary>
        Task<List<Company_Career_Entity>> getlist(string Feild, string Query, string Division);

        /// <summary>
        /// 계약 정보 옵션 수
        /// </summary>
        Task<int> Getcount_option(string Feild, string Query);

        /// <summary>
        /// 계약 정보 검색된 목록
        Task<List<Company_Career_Entity>> getlist_search(int Page, string Feild, string Query, string Division);

        /// <summary>
        /// 계약 정보 검색된 수
        /// </summary>
        Task<int> Getcount_search(string Feild, string Query, string Division);

        /// <summary>
        /// 공동주택명으로 계약정보 목록 만들기
        /// </summary>
        Task<List<Company_Career_Entity>> getlist_name_search(string Apt_Name);

        /// <summary>
        /// 식별코드로 계약정보 목록 만들기
        /// </summary>
        Task<List<Company_Career_Entity>> getlist_name(string Apt_Name);

        /// <summary>
        /// 공동주택명으로 찾은 수
        /// </summary>
        Task<int> apt_name_count(string Apt_Name);

        /// <summary>
        /// 해당 년도와 월로 계약 정보 존재 여부
        /// </summary>
        Task<int> be_date(string Apt_Code, string Cor_Code, string Contract_start_date, string Contract_end_date);

        /// <summary>
        /// 해당 공동주택 신원주택 위탁계약 정보 존재 여부 확인
        /// </summary>
        Task<int> BeApt(string Apt_Code, string Cor_Code);

        /// <summary>
        /// 위탁계약 코드 찾아 오기
        /// </summary>
        Task<Company_Career_Entity> BeAptCompany_Code(string Apt_Code, string Cor_Code);

        /// <summary>
        /// 해당 식별코드로 계약 정보 존재 여부
        /// </summary>
        Task<int> be_Code(int Aid);

        /// <summary>
        /// 삭제
        /// </summary>
        Task delete(string Aid, string Division);

        /// <summary>
        /// 계약일자 목록
        /// </summary>
        Task<List<Company_Career_Entity>> ListDrop(string Apt_Code, string ContractSort_Code);

        /// <summary>
        /// 마지막 일련번호
        /// </summary>
        Task<string> LastCount();

        /// <summary>
        /// 날짜 계산
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        int Date_scomp(string start, string end);

        /// <summary>
        /// 첨부파일 추가
        /// </summary>
        Task File_Plus(int Aid);

        /// <summary>
        /// 첨부파일 추가
        /// </summary>
        Task File_Minus(int Aid);
    }
}
