using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Erp_Apt_Lib.Appeal
{
    public interface IAppeal_Bloom_Lib
    {
        Task<Appeal_Bloom_Entity> Add(Appeal_Bloom_Entity ad);
        Task<Appeal_Bloom_Entity> Edit(Appeal_Bloom_Entity ad);
        Task<Appeal_Bloom_Entity> Edit_Use(Appeal_Bloom_Entity ad);
        Task<List<Appeal_Bloom_Entity>> GetList();
        Task<List<Appeal_Bloom_Entity>> GetList_Page(int Page);
        Task<Appeal_Bloom_Entity> Details(int Num);
        Task<int> GetList_Count();
        Task<string> LastNumber();
        Task<List<Appeal_Bloom_Entity>> Sort_Name_List();
        Task<List<Appeal_Bloom_Entity>> Asort_Name_List(string Sort);
        Task<List<Appeal_Bloom_Entity>> Asort_List(string Sort);
        Task<string> ASortName(string Bloom_Code);
        Task<string> SortName(string Bloom_Code);
        Task<int> Period(string Asort);
        Task<string> Sort_Code(string SortName);
        Task<string> Asort_Code(string AsortName);
        Task<Appeal_Bloom_Entity> Details_Code(string Bloom_Code);
    }

    public interface IAppeal_Lib
    {
        Task<Appeal_Entity> add(Appeal_Entity appeal);
        Task Remove(string Num);
        Task<List<Appeal_Entity>> getlist(int Page, string AptCode, string apDongNo, string apHoNo);

        /// <summary>
        /// 입주민 민원 내역 수
        /// </summary>
        Task<int> getlist_count(string AptCode, string apDongNo, string apHoNo);
        /// <summary>
        /// 동호로 검색된 민원정보
        /// </summary>
        Task<List<Appeal_Entity>> getlistDongHo(int Page, string AptCode, string apDongNo, string apHoNo);
        Task<int> getlistDongHo_count(string AptCode, string apDongNo, string apHoNo);
        Task<List<Appeal_Entity>> getlist_apt(int Page, string AptCode);
        Task<List<Appeal_Entity>> getlist_Mobile_apt(string AptCode);
        Task<List<Appeal_Entity>> getlistPage(string AptCode);
        Task<List<Appeal_Entity>> AppealListComplete(string AptCode);
        Task<int> AppealListCompleteCount(string AptCode);

        /// <summary>
        /// 키워드로 검색된 민원 목록
        /// </summary>
        Task<List<Appeal_Entity>> getlist_Search(int Page, string AptCode, string Word);

        /// <summary>
        /// 키워드로 검색된 민원 목록 수
        /// </summary>
        Task<int> getlist_Search_Count(string AptCode, string Word);

        /// <summary>
        /// 날짜로 검색된 민원 정보
        /// </summary>
        Task<List<Appeal_Entity>> getlist_apt_New(int Page, string AptCode, string StartDate, string EndDate);

        /// <summary>
        /// 날짜로 검색된 민원 정보 수
        /// </summary>
        Task<int> getlist_apt_New_Count(string AptCode, string StartDate, string EndDate);
        Task<Appeal_Entity> ViewsSv(string Num, string AptCode);
        Task<string> apOk(string Num);
        Task<List<Appeal_Entity>> getlistSort(string AptCode, string Bloom_Code);
        Task<int> getlistSort_Count(string AptCode, string Bloom_Code);
        
        /// <summary>
        /// 소분류로 검색된 민원 정보 불러오기
        /// </summary>
        Task<List<Appeal_Entity>> getlistSortA(int Page, string AptCode, string apTitle);

        Task<int> getlistSortA_Count(string AptCode, string apTitle);
        Task<int> getlist_apt_count(string AptCode);
        Task File_Up(string ComFileName, int ComFileSize, string ComFileName2, int ComFileSize2, string ComFileName3, int ComFileSize3, string ComFileName4, int ComFileSize4, string ComFileName5, int ComFileSize5, int Num);
        Task File_Up_set(string ComFileName_Field, string ComFileName_Query, string ComFileSize_Field, int ComFileSize_Query, int Num);
        Task<Appeal_Entity> Detail(string Num);
        Task<Appeal_Entity> Edit_Insert(Appeal_Entity appeal);
        Task<Appeal_Entity> Edit(Appeal_Entity appeal);
        Task<Appeal_Entity> Edit_JobWork(Appeal_Entity appeal);
        Task Edit_Complete(string Num);
        Task<string> Complete(string Num);
        Task apOkComplete(string Num);

        /// <summary>
        /// 만족도 입력
        /// </summary>
        Task apSatisfaction(string Num, string Ok);

        /// <summary>
        /// 민원 완료 여부 입력
        /// </summary>
        Task Edit_WorkComplete(string Num);
        Task Modify_WorkComplete(string Num, string innViw);
        Task<string> innView(string Num);
        Task<Appeal_Entity> File_View(string Num);
        Task<List<Sw_Files_Entity>> GetList_UpFile_Appeal(string Apt_Code, string Parents_Name, string Parents_Num);
        Task<int> GetList_UpFile_Appeal_Count(string Apt_Code, string Parents_Name, string Parents_Num);
        Task<string> apAgo(string AptCode, string Num);
        Task<int> apAgoBe(string AptCode, string Num);
        Task<string> apNext(string AptCode, string Num);
        Task<int> apNextBe(string AptCode, string Num);
    }

    public interface IsubAppeal_Lib
    {
        Task Add(subAppeal_Entity sub);
        Task Edit(subAppeal_Entity sub);
        Task<List<subAppeal_Entity>> GetList(string apNum);
        Task<subAppeal_Entity> Detail(string subAid);
        Task<int> BeCount(string apNum);
        Task<string> apNum(string subAid);
        Task Remove(string subAid);
        Task EditComplete(string subAid, string Complete);

    }

    public interface IsubWorker_Lib
    {
        Task Add(subWorker_Entity sub);
        Task<List<subWorker_Entity>> GetList(string subAid);
        Task Remove(string workAid);

    }
}
