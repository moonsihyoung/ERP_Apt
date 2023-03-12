using System.Collections.Generic;
using System.Threading.Tasks;

namespace Facilities
{
    public interface IBloom_Lib
    {
        Task Add(Bloom_Entity bn);
        Task Edit(Bloom_Entity bn);
        Task Update(Bloom_Entity bn);
        Task<List<Bloom_Entity>> GetList(int Page);
        Task<int> GetListCount();
        Task<List<Bloom_Entity>> GetList_Apt(int Page, string AptCode);

        /// <summary>
        /// 시설물 분류 전체 목록
        /// </summary>
        Task<int> GetList_Apt_Count(string AptCode);

        Task<List<Bloom_Entity>> GetList_Apt_a(string AptCode);
        Task<List<Bloom_Entity>> GetList_Apt_b(string AptCode);
        Task<List<Bloom_Entity>> GetList_Apt_ba(string AptCode);
        Task<int> GetList_Apt_ba_Count(string AptCode);
        Task<List<Bloom_Entity>> GetList_Apt_bb(string AptCode, string B_N_A_Name);
        Task<int> GetList_Apt_bb_Count(string AptCode, string B_N_A_Name);
        Task<List<Bloom_Entity>> GetList_Apt_bc(string AptCode, string B_N_B_Name);
        Task<int> GetList_Apt_bc_Count(string AptCode, string B_N_B_Name);
        Task<List<Bloom_Entity>> GetList_Apt_c(string AptCode, string B_N_A_Name);
        Task<List<Bloom_Entity>> GetListBloomPlece(string AptCode);
        Task<int> GetList_Apt_c_Count(string AptCode, string B_N_A_Name);
        Task<string> LastAid();
        Task<string> BloomNameA(string B_N_A_Name);
        Task<string> BloomNameB(string B_N_A_Name, string B_N_B_Name);
        Task<string> BloomNameC(string B_N_A_Name, string B_N_B_Name, string B_N_C_Name);
        Task<int> Be(string AptCode, string B_N_A_Name, string Bloom);
        Task Remove(string Num);
        Task<List<Bloom_Entity>> GetList_bb(string B_N_A_Name);
        Task<List<Bloom_Entity>> GetList_cc(string B_N_B_Name);
        Task<List<Bloom_Entity>> GetList_dd(string AptCode, string B_N_A_Name);

        Task<string> B_N_A_Code(string B_N_A_Name);
        Task<string> B_N_B_Code(string B_N_A_Name, string B_N_B_Name);
        Task<string> B_N_C_Code(string B_N_A_Name, string B_N_B_Name, string B_N_C_Name);
        Task<string> B_N_D_Code(string B_N_A_Name, string Bloom, string AptCode);

        // <summary>
        /// 분류명 불러오기
        /// </summary>
        Task<string> Sort_Name(string Num, string Bloom_Code);

        Task<int> Period(string Num);

        /// <summary>
        /// 일련번호로 작업장소 불러오기
        /// </summary>
        Task<string> Position_Name(string Code);

        /// <summary>
        /// 시설물 분류 해당 공동주택 전체 목록(장소)
        /// </summary>
        Task<List<Bloom_Entity>> GetList_Apt_Bloom(int Page, string AptCode, string B_N_A_Name);

        /// <summary>
        /// 시설물 분류 해당 공동주택 전체 목록(장소) 수
        /// </summary>
        Task<int> GetList_Apt_Bloom_Count(string AptCode, string B_N_A_Name);

        /// <summary>
        /// 찾기
        /// </summary>
        /// <param name="Field"></param>
        /// <param name="Query"></param>
        /// <returns></returns>
        Task<List<Bloom_Entity>> SearchList(string Field, string Query);
    }
}
