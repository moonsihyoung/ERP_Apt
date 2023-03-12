using System.Collections.Generic;
using System.Threading.Tasks;

namespace Erp_Apt_Lib.Apt_Reports
{
    /// <summary>
    /// 보고서
    /// </summary>
    public interface IApt_Reports_Lib
    {
        /// <summary>
        /// 보고서 입력
        /// </summary>
        /// <param name="_Entity"></param>
        /// <returns></returns>
        Task<Apt_Reports_Entity> Add(Apt_Reports_Entity _Entity);

        /// <summary>
        /// 보고서 삭제 (2022)
        /// </summary>
        Task Delete(string complete, string Aid);

        /// <summary>
        /// 첨부 파일 추가 시 실행
        /// </summary>
        Task Files_Add(int Aid, string Sort);

        /// <summary>
        /// 보고서 상세보기
        /// </summary>
        /// <param name="Aid"></param>
        /// <returns></returns>
        Task<Apt_Reports_Entity> Detail(string Aid);

        /// <summary>
        /// 보고서 수정
        /// </summary>
        /// <param name="_Entity"></param>
        /// <returns></returns>
        Task<Apt_Reports_Entity> Edit(Apt_Reports_Entity _Entity);

        /// <summary>
        /// 보고서 분류별 목록
        /// </summary>
        /// <param name="Report_Bloom_Code"></param>
        /// <returns></returns>
        Task<List<Apt_Reports_Entity>> GeList_Center(int Page, string Report_Bloom_Code);

        /// <summary>
        /// 보고서 분류별 목록 수
        /// </summary>
        /// <param name="Report_Bloom_Code"></param>
        /// <returns></returns>
        Task<int> GeList_Center_Count(string Report_Bloom_Code);

        /// <summary>
        /// 보고서 분류별 단지별 목록
        /// </summary>
        /// <param name="Apt_Code"></param>
        /// <param name="Report_Bloom_Code"></param>
        /// <returns></returns>
        Task<List<Apt_Reports_Entity>> GetList(int Page, string Apt_Code, string Report_Bloom_Code);

        /// <summary>
        /// 보고서 전체 목록
        /// </summary>
        /// <returns></returns>
        Task<List<Apt_Reports_Entity>> GetList_All(int Page);

        /// <summary>
        /// 보고서 단지별 목록
        /// </summary>
        /// <param name="Apt_Code"></param>
        /// <returns></returns>
        Task<List<Apt_Reports_Entity>> GetList_Apt_Code(int Page, string Apt_Code);

        /// <summary>
        /// 보고서 전체 분류별 목록 수
        /// </summary>
        /// <param name="Apt_Code"></param>
        /// <param name="Report_Bloom_Code"></param>
        /// <returns></returns>
        Task<int> GetList_Count(string Apt_Code, string Report_Bloom_Code);

        /// <summary>
        /// 보고서 전체 목록 수
        /// </summary>
        /// <returns></returns>
        Task<int> GetList_Count_All();

        /// <summary>
        /// 보고서 단지별 목록 수
        /// </summary>
        /// <param name="Apt_Code"></param>
        /// <returns></returns>
        Task<int> GetList_Count_Apt(string Apt_Code);

        /// <summary>
        /// 보고서별 입력된 수 
        /// </summary>
        Task<int> Report_Bloom_Count(string Apt_Code, string Report_Bloom_Code);

        /// <summary>
        /// 보고서 단지별 분류별 년도별 목록 수
        /// </summary>
        Task<int> Report_Count(string Apt_Code, string Report_Bloom_Code, string Report_Year);

        /// <summary>
        /// 월간 보고서 입력된 수 
        /// </summary>
        Task<int> Report_Month_Count(string Apt_Code, string Report_Bloom_Code, string Report_Year, string Report_Month);

        /// <summary>
        /// 보고서 결과 정보
        /// </summary>
        Task Result(string result, string Aid);

        /// <summary>
        /// 보고서 삭제
        /// </summary>
        Task Remove(string Aid);
    }

    /// <summary>
    /// 보고서 구분
    /// </summary>
    public interface IReport_Division_Lib
    {
        /// <summary>
        /// 보고서 분류 저장
        /// </summary>
        /// <param name="_Entity"></param>
        /// <returns></returns>
        Task<Report_Division_Entity> add(Report_Division_Entity _Entity);

        /// <summary>
        /// 보고서 분류 수정
        /// </summary>
        /// <param name="_Entity"></param>
        /// <returns></returns>
        Task<Report_Division_Entity> Edit(Report_Division_Entity _Entity);

        /// <summary>
        /// 보고서 분류 목록
        /// </summary>
        /// <param name="Views"></param>
        /// <returns></returns>
        Task<List<Report_Division_Entity>> GeList(string Views);

        /// <summary>
        /// 보고서 분류명 불러오기
        /// </summary>
        /// <param name="Report_Division_Code"></param>
        /// <returns></returns>
        Task<string> ReportDivisin_Name(string Report_Division_Code);

        /// <summary>
        /// 분류명 사용여부
        /// </summary>
        /// <param name="Aid"></param>
        /// <param name="Views"></param>
        /// <returns></returns>
        Task Views(string Aid, string Views);
    }

    /// <summary>
    /// 보고서 분류 
    /// </summary>
    public interface IReport_Bloom_Lib
    {
        /// <summary>
        /// 보고서 구분 저장
        /// </summary>
        /// <param name="_Entity"></param>
        /// <returns></returns>
        Task<Report_Bloom_Entity> add(Report_Bloom_Entity _Entity);

        /// <summary>
        /// 보고서 구분 수정
        /// </summary>
        /// <param name="_Entity"></param>
        /// <returns></returns>
        Task<Report_Bloom_Entity> Edit(Report_Bloom_Entity _Entity);

        /// <summary>
        /// 보고서 구분 목록
        /// </summary>
        /// <param name="Views"></param>
        /// <returns></returns>
        Task<List<Report_Bloom_Entity>> GeList(string Views);

        /// <summary>
        /// 보고서 분류명 불러오기
        /// </summary>
        /// <param name="Report_Bloom_Code"></param>
        /// <returns></returns>
        Task<string> ReportBloom_Name(string Report_Bloom_Code);


        /// <summary>
        /// 보고서 구분명 불러오기
        /// </summary>
        /// <param name="Report_Bloom_Code"></param>
        /// <returns></returns>
        Task<string> Report_Division_Name(string Report_Division_Code);

        /// <summary>
        /// 보고서 구분 사용여부
        /// </summary>
        /// <param name="Aid"></param>
        /// <param name="Views"></param>
        /// <returns></returns>
        Task Views(string Aid, string Views);
    }

}