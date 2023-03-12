using Facilities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Works
{
    /// <summary>
    /// 작업일지 인터페이스
    /// </summary>
    public interface IWorks_Lib
    {
        Task Service_WriteOutViw(Works_Entity dnn);
        Task<List<Bloom_Entity>> List_Bloom_Drop();
        Task<List<Bloom_Entity>> List_Bloom_DropB(string B_N_B_Name);
        Task<List<Bloom_Entity>> List_Bloom_DropC(string B_N_A_Name, string B_N_B_Name);
        Task<int> List_Bloom_D();
        Task<List<Bloom_Entity>> List_Bloom_DropD(string B_N_A_Name, string AptCode);
        Task<List<Bloom_Entity>> List_Bloom_Drop_Place(string B_N_A_Name, string AptCode);
        Task<List<Works_Entity>> ListA(int Page, string AptCode, string B_N_A_Name, string B_N_B_Name, string B_N_C_Name);
        Task<List<Works_Entity>> BoomSearchListC(int Page, string AptCode, string svBloomA, string svBloomB, string svBloomC);
        Task<List<Works_Entity>> FacilityC(string AptCode, string svBloomA, string svBloomB, string svBloomC);
        Task<int> BoomSearchList_CountC(string AptCode, string svBloomA, string svBloomB, string svBloomC);
        Task<List<Works_Entity>> BoomSearchListB(int Page, string AptCode, string svBloomA, string svBloomB);
        Task<int> BoomSearchList_CountB(string AptCode, string svBloomA, string svBloomB);
        Task<List<Works_Entity>> BoomSearchListA(int Page, string AptCode, string svBloomA);
        Task<int> BoomSearchList_CountA(string AptCode, string svBloomA);
        Task<List<Works_Entity>> DateSearchListA(int Page, string AptCode, string StartDate, string EndDate);
        Task<int> DateSearchList_CountA(string AptCode, string StartDate, string EndDate);
        Task<List<Works_Entity>> WordSearchListA(int Page, string AptCode, string Word);
        Task<int> WordSearchList_CountA(string AptCode, string Word);
        Task<List<Bloom_Entity>> Wh_BloomD_List(string AptCode);
        Task<List<Works_Entity>> ListB(int Page, string AptCode, string svYear, string svMonth, string svDay);
        Task<int> GetCountServiceListB(string AptCode, string Year, string Month, string Day);
        Task<List<Works_Entity>> ListB_A(int Page, string AptCode, string svYear, string svMonth);
        Task<List<Works_Entity>> Service_ScwCode_Data_List(string AptCode, string Scw_Code_da);
        Task<int> GetCountServiceListB_A(string AptCode, string Year, string Month);
        Task<List<Works_Entity>> Service_List(int Page, string AptCode);
        Task<List<Works_Entity>> ServiceListComplete(string AptCode);
        Task<int> Service_List_Count(string AptCode);
        Task<int> ServiceListCompleteCount(string AptCode);
        Task<int> GetCountServiceT(string AptCode);
        Task<int> GetCountService(string AptCode);
        Task<int> GetCountServiceListC(string AptCode, string B_N_A_Name, string B_N_B_Name);
        Task<List<Works_Entity>> ListC(int Page, string AptCode, string B_N_A_Name, string B_N_B_Name);
        Task<int> GetCountServiceListC_A(string AptCode, string B_N_A_Name);
        Task<List<Works_Entity>> ListC_A(int Page, string AptCode, string B_N_A_Name);
        Task<int> Service_Last_Num_Data(string AptCode);
        Task Service_Write(Works_Entity Ct);
        Task UpdateWorksEdit(Works_Entity ct);
        Task Bloom_D_Write(Bloom_Entity As);
        Task<Works_Entity> Service_View_Num(int Num);
        Task ServiceComplete(string Num);
        Task ServiceConform(string Num);
        Task<string> CompleteBeing(string Num);
        Task<string> ConformBeing(string Num);
        Task<string> svAgo(string AptCode, string Num);
        Task<int> svAgoBe(string AptCode, string Num);
        Task<string> svNext(string AptCode, string Num);
        Task<int> svNextBe(string AptCode, string Num);
        Task<string> WorkBeing(string Num);
        Task WorksRemove(string id);

        /// <summary>
        /// 단지 시설물 리스트로 작업일지 대분류 검색
        /// </summary>
        Task<List<Works_Entity>> ListWorkA(int Page, string AptCode, string B_N_A_Name);

        /// <summary>
        /// 단지 시설물 리스트로 작업일지 대분류 검색 수
        /// </summary>
        Task<int> ListWorkA_Count(string AptCode, string B_N_A_Name);

        /// <summary>
        /// 단지 시설물 리스트로 작업일지 중분류 검색
        /// </summary>
        Task<List<Works_Entity>> ListWorkB(int Page, string AptCode, string B_N_A_Name, string B_N_B_Name);

        /// <summary>
        /// 단지 시설물 리스트로 작업일지 중분류 검색 수
        /// </summary>
        Task<int> ListWorkB_Count(string AptCode, string B_N_A_Name, string B_N_B_Name);

        /// <summary>
        /// 단지 시설물 리스트로 작업일지 소분류 검색
        /// </summary>
        Task<List<Works_Entity>> ListWorkC(int Page, string AptCode, string B_N_A_Name, string B_N_B_Name, string B_N_C_Name);

        /// <summary>
        /// 단지 시설물 리스트로 작업일지 소분류 검색 수
        /// </summary>
        Task<int> ListWorkC_Count(string AptCode, string B_N_A_Name, string B_N_B_Name, string B_N_C_Name);
    }

    /// <summary>
    /// 작업일지 작업내용 인터페이스
    /// </summary>
    public interface IWorksSub_Lib
    {
        Task Add(WorksSub_Entity at);
        Task<WorksSub_Entity> Edit(WorksSub_Entity at);
        Task<List<WorksSub_Entity>> Getlist(string Service_Code);
        Task<WorksSub_Entity> Details(string Num);
        Task<string> LastNumber();
        Task<int> BeingCount(string AptCode, string Service_Code);
        Task Remove(int Aid);
    }
}