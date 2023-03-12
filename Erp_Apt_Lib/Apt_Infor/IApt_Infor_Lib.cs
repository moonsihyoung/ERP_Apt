using System.Collections.Generic;
using System.Threading.Tasks;

namespace Plan_Lib.Apt_Infor
{
    public interface IApt_Infor_Lib
    {
        Task<List<AptInfor_All>> aptinforTest();
        Task<List<AptInfor_All>> aptinforList();
        Task Add();

        /// <summary>
        /// 공동주택 정보 수정하기
        /// </summary>
        Task Update();


        /// <summary>
        /// 기본정보 수정
        /// </summary>
        Task<Apt_Basic_Entity> Edd_Basis(Apt_Basic_Entity enn);

        /// <summary>
        /// 상세정보 수정
        /// </summary>
        Task<Apt_Details_Entity> Edd_Detail(Apt_Details_Entity enn);

        /// <summary>
        /// 기본정보 목록
        /// </summary>
        Task<List<Apt_Basic_Entity>> GetList_Basis(int Page);

        /// <summary>
        /// 기본정보 목록 수
        /// </summary>
        Task<int> GetList_Basis_Count();

        /// <summary>
        /// 상세정보 목록
        /// </summary>
        Task<List<Apt_Details_Entity>> GetList_Detail(int Page);

        /// <summary>
        /// 상세정보 목록 수
        /// </summary>
        Task<int> GetList_Detail_Count();

        /// <summary>
        /// 공동주택 기본정보 상세
        /// </summary>
        Task<Apt_Basic_Entity> GetBy_Basis(string kaptCode);

        /// <summary>
        /// 공동주택 상세정보 상세
        /// </summary>
        Task<Apt_Details_Entity> GetBy_Detail(string kaptCode);

        /// <summary>
        /// 기본정보 시도 목록
        /// </summary>
        Task<List<Apt_Basic_Entity>> SearchList_Juso_Basis();

        /// <summary>
        /// 기본정보 시도로 찾은 시군구 목록
        /// </summary>
        Task<List<Apt_Basic_Entity>> SearchList_sodo_Basis(string sido);

        /// <summary>
        /// 기본정보 시도 및 시군구로 찾은 시군구 목록
        /// </summary>
        Task<List<Apt_Basic_Entity>> SearchList_sigungo_Basis(string sido, string sigungo);

        /// <summary>
        /// 기본정보 시도 및 시군구로 찾은 시군구 목록
        /// </summary>
        Task<List<Apt_Basic_Entity>> SearchList_sigungo_maeul_Basis(string sido, string sigungo, string maeul);
    }
}
