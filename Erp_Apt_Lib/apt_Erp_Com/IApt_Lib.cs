using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using Microsoft.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Erp_Entity;
using Microsoft.Extensions.Configuration;



namespace Erp_Lib
{
    public interface IApt_Lib
    {
        Task<int> Add(Apt_Entity apt);
        Task Edit(Apt_Entity apt);
        Task<Apt_Entity> Detail(string Aid);
        Task<Apt_Entity> Details(string AptCode);
        Task<int> Be_Count(string Apt_Code);
        Task<List<Apt_Entity>> GetList(int Page);
        Task<List<Apt_Entity>> GetList_All();
        Task<int> GetList_All_Count();
        Task<List<Apt_Entity>> GetList_All_Sido(string Apt_Adress_Sido);
        Task<int> GetList_All_Sido_Count(string Apt_Adress_Sido);
        Task<List<Apt_Entity>> GetList_All_Sido_Gun(string Apt_Adress_Sido, string Apt_Adress_Gun);
        Task<int> GetList_All_Sido_Gun_Count(string Apt_Adress_Sido, string Apt_Adress_Gun);
        Task<List<Apt_Entity>> GetList_new();
        Task<List<Apt_Entity>> GetList_Sido(int Page, string Apt_Adress_Sido);
        Task<List<Apt_Entity>> GetList_Sido_Gun(int Page, string Apt_Adress_Sido, string Apt_Adress_Gun);
        Task<List<Apt_Entity>> SearchList(string AptName);
        Task<int> Aid_Count(string Apt_Adress_Gun);
        Task<int> Cn_Check(string CorporateResistration_Num);
        Task Delete(string AId);
        Task<string> List_Number();
        //bool checkCpIdenty(string cpNum);
        Task<string> Apt_Name(string Apt_Code);

        /// <summary>
        /// 해당 공동주택 사용검사일
        /// </summary>
        Task<DateTime> Apt_BuildDate(string Apt_Code);
    }

    public interface IApt_Sub_Lib
    {
        Task<Apt_Sub_Entity> Add(Apt_Sub_Entity ast);
        Task Edit(Apt_Sub_Entity ast);
        Task<Apt_Sub_Entity> Detail(string Apt_Code);
        Task<int> being(string Apt_Code);

        /// <summary>
        /// 삭제
        /// </summary>
        /// <param name="Aid"></param>
        /// <returns></returns>
        Task Remove(int Aid);
    }

    public interface ISido_Lib
    {
        Task<Sido_Entity> Add(Sido_Entity sido);
        Task Edit(Sido_Entity sido);
        Task<List<Sido_Entity>> GetList(string Sido);
        Task<List<Sido_Entity>> GetList_Code(string Sido);
        Task<string> GunGoName(string code);
        Task<string> SidoName(string code);
        Task<string> Region_Code(string Region);
        Task<Sido_Entity> Details(string Code);
        Task<int> Apt_Count(string Apt_Code);

        /// <summary>
        /// 시도명으로 상세정보 불러오기
        /// </summary>
        Task<Sido_Entity> Details_Name(string Name);
    }

    public interface IErp_Files_Lib
    {
        Task<Erp_Files_Entity> Add(Erp_Files_Entity _Entity);
        Task<List<Erp_Files_Entity>> GeList(string Parents_Num, string Parents_Name);
        Task<int> Be_Count(string Parents_Num, string Parents_Name);
        void Delete(string Parents_Num, string Parents_Name);
    }

    public interface IErp_AptPeople_Lib
    {
        Task<Apt_People_Entity> Add(Apt_People_Entity at);
        Task<Apt_People_Entity> Edit(Apt_People_Entity at);
        Task Move(string MoveA, string MoveDate, int Num);

        /// <summary>
        /// 동호로 이사 정보 입력
        /// </summary>
        Task Remove_Ho(string Apt_Code, string Dong, string Ho);


        Task<List<Apt_People_Entity>> DongList(string Apt_Code);
        Task<List<Apt_People_Entity>> HoList(string Apt_Code, string Dong);

        /// <summary>
        /// 공동주택별 입주민 중복 제거 정보 목록
        /// </summary>
        Task<List<Apt_People_Entity>> Dong_HoList_Ds(string Apt_Code, string Dong);

        Task<int> Dong_HoList_Count(string Apt_Code, string Dong);
        Task<int> Dong_Ho_Count(string Apt_Code, string Dong, string Ho);

        /// <summary>
        /// 동호 만들기 검색된 갯수
        /// </summary>
        Task<int> Dong_Ho_Count_New(string Apt_Code, string Dong, string Ho);

        /// <summary>
        /// 동으로 검색된 호 목록
        /// </summary>
        Task<List<Apt_People_Entity>> DongHoList_new(string Apt_Code, string Dong);

        Task<List<Apt_People_Entity>> Apt_List(string Apt_Code);

        /// <summary>
        /// 모바일로 검색된 정보
        /// </summary>
        Task<List<Apt_People_Entity>> Apt_Mobile_List(string Apt_Code, string Mobile);

        /// <summary>
        /// 이름으로 검색된 정보
        /// </summary>
        Task<List<Apt_People_Entity>> Apt_Name_List(string Apt_Code, string Name);

        Task<List<Apt_People_Entity>> Apt_List_Page(int Page, string Apt_Code);
        Task<Apt_People_Entity> Dedeils_Mobile(string Mobile, string Dong, string Ho);
        Task<int> Dedeils_Mobile_Count(string Mobile, string Dong, string Ho, string InnerName);
        Task<int> Apt_List_Count(string Apt_Code);
        Task<List<Apt_People_Entity>> Dong_HoList(string Apt_Code, string Dong);
        Task<List<Apt_People_Entity>> Dong_Ho_Name_List(string Apt_Code, string Dong, string Ho);
        Task<List<Apt_People_Entity>> Dong_HoList_Page(int Page, string Apt_Code, string Dong);
        Task<Apt_People_Entity> DongHo_Name(string Apt_Code, string Dong, string Ho);
        /// <summary>
        /// 해당 세대 상세 정보
        /// </summary>
        Task<int> Dong_Ho_Name_Being(string Apt_Code, string Dong, string Ho, string Name);

        /// <summary>
        /// 해당 세대 이름으로 검색된 상세 정보
        /// </summary>
        Task<Apt_People_Entity> Dedeils_Name(string Apt_Code, string Name, string Dong, string Ho);

        Task<double> DongHo_Area(string Apt_Code, string Dong, string Ho);
        Task<List<Apt_People_Entity>> DongHoList(string Apt_Code, string Dong, string Ho);
        Task FilesCountAdd(string Aid, string Division);
        Task<Apt_People_Entity> Dedeils_Name(string Num);
    }

    public interface IPostDuty_Lib
    {
        Task<PostDuty_Entity> Add(PostDuty_Entity pd);
        Task Edit(PostDuty_Entity pd);
        Task Remove(string Num);
        Task<List<PostDuty_Entity>> GetList_Post_P(string PD_Code, string Post_Duty_D_Code);
        Task<string> Post_Duty_Code(string PD_Code, string Post_Duty_Name);
        Task<List<PostDuty_Entity>> Duty_List(string Post_Code, string PD_Code);
        Task<PostDuty_Entity> Detail(string Num);
        Task<List<PostDuty_Entity>> PostDuty_AllList();
        Task<int> Be_Last();
        Task<List<PostDuty_Entity>> PostList(string PD_Code, string Post_Code);

    }

    public interface IPost_Lib
    {
        Task Add(Post_Entity post);
        Task Edit(Post_Entity post);
        Task<int> Count();
        Task<List<Post_Entity>> GetList(string Division);
        Task<List<Post_Entity>> GetListAll();
        Task<string> PostName(string PostCode);
        Task<string> PostCode(string PostName);
        Task Remove(string PostCode);
    }

    public interface IDuty_Lib
    {
        Task Add(Duty_Entity ann);

        /// <summary>
        /// 직책 수정
        /// </summary>
        Task Edit(Duty_Entity ann);

        Task<List<Duty_Entity>> GetList(string PostCode, string Division);

        Task<List<Duty_Entity>> GetListAll(int Page);

        /// <summary>
        /// 직책명 불러오기
        /// </summary>
        Task<int> GetListAll_Count();

        Task<string> DutyName(string DutyCode);
        Task<List<Duty_Entity>> GetList_B(string PostCode);
        
        /// <summary>
        /// 직책코드 불러오기
        /// </summary>
        Task<int> DutyCode(string Post_Code, string Duty);

        /// <summary>
        /// 직책 정보 삭제
        /// </summary>
        Task Remove(int Aid);
    }

    public interface IDecusion_PostDuty_Lib
    {
        Task Add(Decision_PostDuty_Entity dnn);
        Task Edit(Decision_PostDuty_Entity dp);
        Task<Decision_PostDuty_Entity> Details(int Num);
        Task<List<Decision_PostDuty_Entity>> List(string AptCode, string Bloom);
        Task<int> ListCount(string AptCode, string Bloom);
        Task<int> PostDutyBeCount(string AptCode, string Bloom, string PostDuty);
        Task<List<Decision_PostDuty_Entity>> GetList(int Page, string AptCode);
        Task<int> GetListCount(string AptCode);

        /// <summary>
        /// 삭제
        /// </summary>
        Task Remove(int Num);
    }

    /// <summary>
    /// /// <summary>
    /// 입주민 가입 승인 정보
    /// </summary>
    /// </summary>
    public interface Ibs_apt_career
    {
        Task<bs_apt_career_Entity> add(bs_apt_career_Entity _Career);
        Task edit_end(DateTime bs_end, int Aid, string bs_division);
        Task edit_end_A(int Aid, string bs_division);
        Task<List<bs_apt_career_Entity>> GetList_all(int Page, string Apt_Code);
        Task<int> GetList_all_Count(string Apt_Code);
        Task<List<bs_apt_career_Entity>> GetList_ok(int Page, string Apt_Code, string bs_division);
        Task<int> GetList_ok_Count(string Apt_Code, string bs_division);
        Task<List<Join_apt_People_a_Career_Entity>> Join_GetList_People_a_Career(int Page, string Apt_Code, string bs_division);
        Task<int> Join_GetList_People_a_Career_Count(string Apt_Code, string bs_division);
        Task<Join_apt_People_a_Career_Entity> Join_detail_UserCode(string Apt_Code, string User_Code);
        Task<int> be(string bs_code);
        Task<int> be_apt(string Apt_Code);
        Task<int> be_dong(string Apt_Code, string bs_code, string Dong, string Ho);
        Task<int> be_dongHo(string Apt_Code, string Dong, string Ho);
        Task<int> Log_views(string User_ID, string Password_sw);
        Task<string> Log_Name(string bs_Code);
        Task<bs_apt_career_Entity> detail(string bs_Code, string bs_division);
        Task<int> Be_Count(string bs_Code, string bs_division);
    }

    /// <summary>
    /// 입주민 홈페이지 가입정보 등
    /// </summary>
    public interface IIn_AptPeople_Lib 
    {
        Task<In_AptPeople_Entity> add(In_AptPeople_Entity ap);
        Task<In_AptPeople_Entity> edit(In_AptPeople_Entity ap);
        Task Eidt_pass(string Pass_Word, string User_Code);
        Task Edit_Level(int LevelCount, string User_Code);
        Task VisitCount_Add(string User_Code);
        Task WriteCount_Add(string User_Code);
        Task ReadCount_Add(string User_Code);
        Task CommentsCount_Add(string User_Code);
        Task FileUp_Add(string User_Code);
        Task<List<In_AptPeople_Entity>> GetList(string Apt_Code);
        Task<In_AptPeople_Entity> Detail(string User_Code);
        Task<List<In_AptPeople_Entity>> aptHumanList(int Page, string Apt_Code);
        Task<int> aptHumanList_Count(string Apt_Code);
        Task<List<In_AptPeople_Entity>> DongHoList(int Page, string Apt_Code, string Dong, string Ho);
        Task<int> DongHoList_Count(string Apt_Code, string Dong, string Ho);
        Task<int> Be_UserCode(string User_Code);
        Task<int> Be_DongHo(string User_Code, string Dong, string Ho);
        Task<int> Log_views(string User_ID, string Password);
        Task<int> Log_views_M(string Mobile, string MobileB, string Password);
        Task<In_AptPeople_Entity> Detail_M(string Mobile);
        Task Approval_being(int Aid);
        Task<int> Mobile_Being(string Mobile);
        Task Approval_Remove(string Mobile);
        Task Approval_Remove_ReSet(string Mobile);

        /// <summary>
        /// 암호 초기화 변경
        /// </summary>
        Task PassResert(int Aid);

        /// <summary>
        /// 가입한 회원 이름으로 검색된 목록(단지별)
        /// </summary>
        Task<List<In_AptPeople_Entity>> aptHuman_Name_List(int Page, string Apt_Code, string Name);

        /// <summary>
        /// 가입한 회원 이름 검색된 수(단지별)
        /// </summary>
       Task<int> aptHuman_Name_List_Count(string Apt_Code, string Name);

        /// <summary>
        /// 호 중복 제거한 호 리스트
        /// </summary>
        /// <param name="Apt_Code"></param>
        /// <param name="Dong"></param>
        /// <returns></returns>
        Task<List<Apt_People_Entity>> Dong_HoList(string Apt_Code, string Dong);
    }
}
