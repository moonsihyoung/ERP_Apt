using System.Collections.Generic;
using System.Threading.Tasks;

namespace Facilities
{
    public interface IFacility_Lib
    {
        /// <summary>
        /// 시설물 정보 입력
        /// </summary>
        /// <param name="fac"></param>
        /// <returns></returns>
        Task<int> Add(Facility_Entity fac);

        /// <summary>
        /// 시설물 정보 수정
        /// </summary>
        /// <param name="fac"></param>
        Task Edit(Facility_Entity fac);

        /// <summary>
        /// 공동주택코드로 목록만들기
        /// </summary>
        /// <param name="Apt_Code"></param>
        /// <returns></returns>
        Task<List<Facility_Entity>> GetList_Apt(int Page, string Apt_Code);

        /// <summary>
        /// 공동주택식별코드로 목록 수 구하기
        /// </summary>
        /// <param name="Apt_Code"></param>
        /// <returns></returns>
        Task<int> GetList_Apt_Count(string Apt_Code);

        /// <summary>
        /// 해당 공동주택에 대분류로 검색된 목록
        /// </summary>
        /// <param name="Apt_Code"></param>
        /// <returns></returns>
        Task<List<Facility_Entity>> GetList_Apt_SortA(int Page, string Apt_Code, string Sort_A_Code);

        /// <summary>
        /// 공동주택식별코드로 목록 수 구하기
        /// </summary>
        /// <param name="Apt_Code"></param>
        /// <returns></returns>
        Task<int> GetList_Apt_SortA_Count(string Apt_Code, string Sort_A_Code);

        /// <summary>
        /// 해당 공동주택에 중분류로 검색된 목록
        /// </summary>
        /// <param name="Apt_Code"></param>
        /// <returns></returns>
        Task<List<Facility_Entity>> GetList_Apt_SortB(int Page, string Apt_Code, string Sort_A_Code, string Sort_B_Code);

        /// <summary>
        /// 공동주택식별코드로 목록 수 구하기
        /// </summary>
        /// <param name="Apt_Code"></param>
        /// <returns></returns>
        Task<int> GetList_Apt_SortB_Count(string Apt_Code, string Sort_A_Code, string Sort_B_Code);

        /// <summary>
        /// 해당 공동주택에 소분류로 검색된 목록
        /// </summary>
        /// <param name="Apt_Code"></param>
        /// <returns></returns>
        Task<List<Facility_Entity>> GetList_Apt_SortC(int Page, string Apt_Code, string Sort_A_Code, string Sort_B_Code, string Sort_C_Code);


        /// <summary>
        /// 공동주택식별코드로 목록 수 구하기
        /// </summary>
        /// <param name="Apt_Code"></param>
        /// <returns></returns>
       Task<int> GetList_Apt_SortC_Count(string Apt_Code, string Sort_A_Code, string Sort_B_Code, string Sort_C_Code);

        /// <summary>
        /// 시설물 정보 상세
        /// </summary>
        Task<Facility_Entity> Detail(string Apt_Code, string Aid);

        /// <summary>
        /// 해당 시설물 정보 삭제
        /// </summary>
        /// <param name="Aid"></param>
        Task Remove(string Aid);
    }
}
