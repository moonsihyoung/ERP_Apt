using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Erp_Apt_Lib.Plans
{
	public interface IPlan_Lib
	{
		/// <summary>
		/// 계획 입력
		/// </summary>
		Task Add(Plan_Entity plan);

		/// <summary>
		/// 계획 삭제
		/// </summary>
		Task Delete(int Aid);

		/// <summary>
		/// 계획 수정
		/// </summary>
		Task Update(Plan_Entity plan);

		/// <summary>
		/// 계획상세
		/// </summary>
		Task<Plan_Entity> Details(int Aid);

		/// <summary>
		/// 계획 목록 모두
		/// </summary>
		Task<List<Plan_Entity>> All_List(int Page);

		Task<int> All_List_Count();

		/// <summary>
		/// 단지별 계획목록
		/// </summary>
		Task<List<Plan_Entity>> Apt_List(int Page, string Apt_Code);

        Task<int> Apt_List_Count(string Apt_Code);

        /// <summary>
        /// 단지별 대분류 계획 목록
        /// </summary>
        Task<List<Plan_Entity>> Sort_List(int Page, string Apt_Code, string Sort_Code);

        Task<int> Sort_List_Count(string Apt_Code, string Sort_Code);

        /// <summary>
        /// 단지별 소분류 계획 목록
        /// </summary>
        Task<List<Plan_Entity>> ASort_List(int Page, string Apt_Code, string Sort_Code, string Asort_Code);

        Task<int> Asort_List_Count(string Apt_Code, string Sort_Code, string Asort_Code);
    }

	public interface IPlan_Sort_Lib
	{
		/// <summary>
		/// 계획 분류 입력
		/// </summary>
		Task Add(Plan_Sort_Entity plan);

		/// <summary>
		/// 계획분류 수정
		/// </summary>
		Task Update(Plan_Sort_Entity plan);

		/// <summary>
		/// 계획분류 삭제
		/// </summary>
		Task Delete(int Aid);

		/// <summary>
		/// 계획분류 상세
		/// </summary>
		Task<Plan_Sort_Entity> Details(int Aid);

		/// <summary>
		/// 계획 대분류 전체 목록
		/// </summary>
		Task<List<Plan_Sort_Entity>> SortList();


		/// <summary>
		/// 계획분류 전체 목록
		/// </summary>
		Task<List<Plan_Sort_Entity>> SortList_All();

        /// <summary>
        /// 계획 소분류별 전체 목록
        /// </summary>
        Task<List<Plan_Sort_Entity>> AsortList(string Sort_Code);

		/// <summary>
		/// 공동주택 분류별 목록
		/// </summary>
		Task<List<Plan_Sort_Entity>> Apt_SortList(string Apt_Code);

		/// <summary>
		/// 공동주택 소분류별 목록
		/// </summary>
        Task<List<Plan_Sort_Entity>> Apt_AsortList(string Apt_Code, string Sort_Code);

		/// <summary>
		/// 계획명
		/// </summary>
		Task<string> SortNaneAsync(string Sort_Code);

		/// <summary>
		/// 계획명
		/// </summary>
		string SortNane(string Sort_Code);

        /// <summary>
        /// 분류명
        /// </summary>
        Task<string> AsortNaneAsync(string Asort_Code);

        /// <summary>
        /// 분류명
        /// </summary>
        string AsortNane(string Asort_Code);

		/// <summary>
		/// 마지막 일련번호
		/// </summary>
		Task<int> Last_Aid();

		/// <summary>
		/// 대분류 코드로 상세보기
		/// </summary>
		/// <param name="Sort_Code"></param>
		/// <returns></returns>
		Task<Plan_Sort_Entity> Sort_Details(string Sort_Code);
    }

	public interface IPlan_Man_Lib
	{
		/// <summary>
		/// 작업자 등록
		/// </summary>
		/// <param name="plan"></param>
		/// <returns></returns>
		Task Add(Plan_Man_Entity plan);

		/// <summary>
		/// 작업자 수정
		/// </summary>
		/// <param name="plan"></param>
		/// <returns></returns>
		Task Update(Plan_Man_Entity plan);

		/// <summary>
		/// 작업자 삭제
		/// </summary>
		/// <param name="Aid"></param>
		/// <returns></returns>
		Task Delete(int Aid);

		/// <summary>
		/// 작업자 상세
		/// </summary>
		/// <param name="Aid"></param>
		/// <returns></returns>
		Task<Plan_Man_Entity> Details(int Aid);

		/// <summary>
		/// 작업자 목록
		/// </summary>
		/// <param name="Plan_Code"></param>
		/// <returns></returns>
		Task<List<Plan_Man_Entity>> GetList(string Plan_Code, string Division);
	}
}
