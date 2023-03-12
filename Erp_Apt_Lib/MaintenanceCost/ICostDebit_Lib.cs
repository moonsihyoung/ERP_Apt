using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Erp_Apt_Lib.MaintenanceCost
{
    public interface ICostDebit_Lib
    {
        /// <summary>
        /// 신규 입력
        /// </summary>
        Task Add(CostDebit_Entity ann);

        /// <summary>
        /// 전체 목록
        /// </summary>
        Task<List<CostDebit_Entity>> GetList(int Page);

        /// <summary>
        /// 전체 목록 수
        /// </summary>
        Task<int> GetList_Count();

        /// <summary>
        /// 전체 공동주택 목록
        /// </summary>
        Task<List<CostDebit_Entity>> GetList_Apt(int Page, string Apt_Code);

        /// <summary>
        /// 전체 공동주택 목록 수
        /// </summary>
        Task<int> GetList_Apt_Count(string Apt_Code);

        /// <summary>
        /// 전체 공동주택 목록
        /// </summary>
        Task<List<CostDebit_Entity>> GetList_Apt_Month(int Page, string Apt_Code, string Month);

        /// <summary>
        /// 전체 공동주택 목록 수
        /// </summary>
        Task<int> GetList_Apt_Month_Count(string Apt_Code, string Month);

        /// <summary>
        /// 전체 공동주택 목록
        /// </summary>
        Task<List<CostDebit_Entity>> GetList_Apt_Month_dongho(int Page, string Apt_Code, string Month, string dong, string ho);

        /// <summary>
        /// 전체 공동주택 목록 수
        /// </summary>
        Task<int> GetList_Apt_Month_dongho_Count(string Apt_Code, string Month, string dong, string ho);

        /// <summary>
        /// 세대 해당월 상세정보
        /// </summary>
        Task<CostDebit_Entity> GetBy(string Apt_Code, string dong, string ho, string Month);

        /// <summary>
        /// 세대 해당월 상세정보 존재 여부
        /// </summary>
        Task<int> GetBy_be(string Apt_Code, string dong, string ho, string Month);

        /// <summary>
        /// 전체 공동주택 동호 목록
        /// </summary>
        Task<List<CostDebit_Entity>> GetList_Apt_dongho(int Page, string Apt_Code, string dong, string ho);

        /// <summary>
        /// 전체 공동주택 동호 목록 수
        /// </summary>
        Task<int> GetList_Apt_dongho_Count(string Apt_Code, string dong, string ho);
    }
}
