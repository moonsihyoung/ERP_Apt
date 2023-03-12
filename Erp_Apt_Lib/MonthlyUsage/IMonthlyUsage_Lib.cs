using System.Collections.Generic;
using System.Threading.Tasks;

namespace Erp_Apt_Lib.MonthlyUsage
{
    /// <summary>
    /// 해당 공동주택 월 사용량 정보
    /// </summary>
    public interface IMonthlyUsage_Lib
    {
        /// <summary>
        /// 사용량 입력
        /// </summary>
        Task Add(MonthlyUsage_Entity ar);

        /// <summary>
        /// 사용량 수정
        /// </summary>
        Task Edit(MonthlyUsage_Entity ar);

        /// <summary>
        /// 사용량 상세보기
        /// </summary>
        Task<MonthlyUsage_Entity> GetById(string Apt_Code, int intYear, int intMonth);

        /// <summary>
        /// 해당월 사용량 존재 여부
        /// </summary>
        Task<int> GetById_Count(string Apt_Code, int intYear, int intMonth);

        /// <summary>
        /// 해당월 사용량 존재 여부
        /// </summary>
        Task<int> GetById_Being(string Apt_Code);

        /// <summary>
        /// 사용량 상세보기
        /// </summary>
        Task<MonthlyUsage_Entity> GetDetails(int Aid);

        /// <summary>
        /// 모든 사용량 정보
        /// </summary>
        Task<List<MonthlyUsage_Entity>> GetListAll(int Page);

        /// <summary>
        /// 모든 사용량 정보 수
        /// </summary>
        Task<int> GetListAll_Count();

        /// <summary>
        /// 해당 공동주택 모든 사용량 정보
        /// </summary>
        Task<List<MonthlyUsage_Entity>> GetList(int Page, string Apt_Code);

        /// <summary>
        /// 해당 공동주택 모든 사용량 정보 수
        /// </summary>
        Task<int> GetList_Count(string Apt_Code);

        /// <summary>
        /// 삭제
        /// </summary>
        Task Remove(int Aid);
    }

    /// <summary>
    /// 해당 공동주택 월 상세정보
    /// </summary>
    public interface IUsageDetails_Lib
    {
        /// <summary>
        /// 상세 사용량 정보 입력
        /// </summary>
        Task Add(UsageDetails_Entity ar);

        /// <summary>
        /// 상세 사용량 정보 수정
        /// </summary>
        Task Edit(UsageDetails_Entity ar);

        /// <summary>
        /// 해당 상세 정보
        /// </summary>
        Task<UsageDetails_Entity> GetById(string Apt_Code, int intYear, int intMonth);

        /// <summary>
        /// 해당 공동주택 사용량 상세 정보 존재 여부 
        /// </summary>
        Task<int> GetById_Count(string Apt_Code, int intYear, int intMonth);

        /// <summary>
        /// 상세정보 모두 목록
        /// </summary>
        Task<List<UsageDetails_Entity>> GetListAll(int Page);

        /// <summary>
        /// 상세정보 모두 목록 수
        /// </summary>
        Task<int> GetListAll_Count();

        /// <summary>
        /// 해당 공동주택 월 사용량 상세정보 목록
        /// </summary>
        Task<List<UsageDetails_Entity>> GetList(string Apt_Code, int Year, int Month);

        /// <summary>
        /// 해당 공동주택 월 사용량 상세정보 목록
        /// </summary>
        Task<List<UsageDetails_Entity>> GetList_sort(string Apt_Code, int Year, int Month, string Division);

        /// <summary>
        /// 해당 공동주택 월 사용량 상세정보 목록 존재 여부
        /// </summary>
        Task<int> GetList_Count(string Apt_Code, int Year, int Month);
    }
}
