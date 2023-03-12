using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Erp_Apt_Staff
{
    public interface IStaff_Lib
    {
        Task<int> Add(Staff_Entity st);

        Task Add1(int Aid, string st);
        Task Edit(Staff_Entity st);
        Task<List<Staff_Entity>> Getlist();
        Task<List<Staff_Entity>> GetlistApt(string AptCode);
        Task<Staff_Entity> View(string UserID);
        Task<int> Be(string UserID);
        Task<int> Login(string User_ID, string Password_sw);
        Task<string> Edit_Pass(string User_ID, string Password_sw_Old, string Password_sw_New);
        Task Remove(string UserID);
        Task<string> Name(string User_ID);
        string UsersName(string User_ID);
        Task<int> List_Number();
        Task<int> Being_Staff(string UserName, string Scn, string Sido);
        Task<Staff_StaffSub_Entity> Be_Staff(string UserName, string Mobile, string Mobile_A);
        Task<int> Be_Staff_Count(string UserName, string Mobile, string Mobile_A);
        Task<int> Staff_LevelCount(string UserId);
        Task<List<Staff_Entity>> Staff_Name_Search(string User_Name, string Sido, string SiGunGu);
        Task<List<Staff_Entity>> Staff_Name_SearchA(string User_Name);

        /// <summary>
        /// 직원 마지막 일련번호
        /// </summary>
        Task<int> Apt_Number_Count(string AptCode);

        /// <summary>
        /// 유저 생년월일 불러오기
        /// </summary>
        string Users_bath(string User_ID);

        /// <summary>
        /// 방문수 증가
        /// </summary>
        /// <param name="User_Code"></param>
        Task VisitCount_Add(string User_Code);

        /// <summary>
        /// 글수 증가
        /// </summary>
        /// <param name="User_Code"></param>
       Task WriteCount_Add(string User_Code);

        /// <summary>
        /// 읽은 수 증가
        /// </summary>
        /// <param name="User_Code"></param>
        Task ReadCount_Add(string User_Code);

        /// <summary>
        /// 댓글수 증가
        /// </summary>
        /// <param name="User_Code"></param>
        Task CommentsCount_Add(string User_Code);

        /// <summary>
        /// 파일 업로드 수 증가
        /// </summary>
        /// <param name="User_Code"></param>
        Task FileUpCount_Add(string User_Code);

        /// <summary>
        /// 등급 수정
        /// </summary>
        Task LevelUpdate(string User_ID, int LevelCount);

        /// <summary>
        /// 유저 이름 불러오기
        /// </summary>
       string Staff_Name(string Apt_Code);
    }

    public interface IStaff_Sub_Lib
    {
        Task<int> Add(Staff_Sub_Entity sse);
        Task Edit(Staff_Sub_Entity sse);
        Task<Staff_Sub_Entity> View(string User_ID);
        Task<int> be(string User_ID);
        Task Remove(string User_ID);
        Task<string> SealView(string User_ID);
        Task<Staff_Sub_Entity> Detail(string User_ID);
        Task<List<Staff_Sub_Entity>> GetList_Name(string Sido, String Gungo, string Name);

    }

    public interface IStaff_staffSub_Lib
    {
        /// <summary>
        /// 회원정보 상세
        /// </summary>
        Task<Staff_StaffSub_Entity> Views(string User_ID);

        /// <summary>
        /// 배치정보 상세
        /// </summary>
        Task<Staff_Career_Entity> CareerViews(string User_ID);

        Task<List<Staff_StaffSub_Entity>> GetlistApt(string M_Apt_Code);

        /// <summary>
        /// 회원 정보 목록
        /// </summary>
        Task<List<Staff_StaffSub_Entity>> GetList(int Page);        

        /// <summary>
        /// 회원 정보 목록 수
        /// </summary>
        Task<int> GetList_Count();

        /// <summary>
        /// 회원 정보 목록
        /// </summary>
        Task<List<Staff_Career_Entity>> GetCareerList(int Page);

        /// <summary>
        /// 회원 정보 목록 수
        /// </summary>
        Task<int> GetCareerList_Count();

        /// <summary>
        /// 회원 정보 목록
        /// </summary>
        Task<List<Staff_StaffSub_Entity>> GetList_Search(string Feild, string Query);

        /// <summary>
        /// 배치정보 목록
        /// </summary>
        Task<List<Staff_Career_Entity>> GetCareerList_Search(int Page, string Feild, string Query, string AptName);

        Task<int> GetCareer_Search_Count(string Feild, string Query, string AptName);

        /// <summary>
        /// 회원 정보 존재여부 확인
        /// </summary>
        Task<int> GetListInsertCount(string Name, string Mobile);
    }

    public interface IReferral_career_Lib
    {
        Task<List<Referral_career_Entity>> GetList_Post_Staff(string Post, string Apt_Code);
        Task<List<Referral_career_Entity>> GetList_Post_Staff_be(string Post, string Apt_Code);
        Task<int> GetList_Post_Staff_beCount(string Post, string Apt_Code);
        Task<List<Referral_career_Entity>> GetList_Sojang();
        Task<int> GetList_Sojang_Count();
        Task<List<Referral_career_Entity>> GetList_SojangA(int Page);
        Task<int> GetList_Sojang_CountA();
        Task<List<Referral_career_Entity>> GetList_Sojang_Seach(string Query);
        Task<int> GetList_Sojang_Search_Count(string Query);
        Task<Referral_career_Entity> Details(string User_ID);

        /// <summary>
        /// 아이디로 배치정보 불러오기
        /// </summary>
        Referral_career_Entity Detailss(string User_ID);
        string User_Code_Bes(string AptCode, string Post, string Duty, string User_Code);
        Task<List<Referral_career_Entity>> GetList_Apt_Staff(string Apt_Code);
        Task<List<Referral_career_Entity>> GetList_Apt_Staff_Page(int Page, string Apt_Code);
        Task<List<Referral_career_Entity>> GetList_Apt_Staff_be(string Apt_Code);
        Task<int> GetList_Apt_Staff_beCount(string Apt_Code);
        Task<List<Referral_career_Entity>> GetList_Post(string Apt_Code);
        Task<List<Referral_career_Entity>> GetList_Duty(string Apt_Code);
        Task<Referral_career_Entity> Add_rc(Referral_career_Entity rc);
        Task<Referral_career_Entity> Edit_rc(Referral_career_Entity rc);
        Task<Referral_career_Entity> Edit_rc_A(Referral_career_Entity rc);
        Task Resign(DateTime Career_End_Date, string Division, string Etc, int Aid);
        Task Edit_Resign(DateTime Career_End_Date, string Division, int Aid);
        Task<List<Referral_career_Entity>> GetList(int Page, string Feild, string Query);
        Task<int> GetCount(string Feild, string Query);
        Task<string> PostName(string User_ID);
        Task<string> DutyName(string User_ID);
        Task<string> AptName(string User_ID);
        Task<List<Referral_career_Entity>> GetList_Code(string User_ID);
        Task<int> Getcount(string Feild, string Query);
        Task<List<Referral_career_Entity>> _Career_Search(int Page, string Feild, string Query);
        int Date_scom(string start, string end);
        int Date_scomp(string start, string end);
        Task<int> be(string User_ID);
        Task<int> being(string User_ID);
        Task<int> be_apt(string User_ID, string Apt_Code);
        Task<int> be_not(string User_ID, string Career_Start_Date);
        Task<Referral_career_Entity> Detail(string User_ID);
        Task<Referral_career_Entity> Detail_A(string User_ID, string Aid);
        Task<int> Count_apt_staff(string Apt_Code);
        Task delete(string Aid);
        Task<List<Referral_career_Entity>> Getlist_apt(string Apt_Code, string Division, string Post_Code);
        Task<List<Referral_career_Entity>> _Career_Name_Search(string Feild, string Query);
        Task<List<Referral_career_Entity>> _Career_Name_Apt_Search(string Feild, string Query, string Apt_Code);
        Task<List<Referral_career_Entity>> _Career_PostCode_Apt_Search(string Post_Code, string Post_CodeA, string Apt_Code);
        Task<string> User_Code_Be(string AptCode, string Post, string Duty);
        Task Remove(string User_ID);
        Task<List<Staff_Career_Entity>> StaffCareer_Join(string Apt_Code, string Division);
        Task<List<Staff_Career_Entity>> Staff_Career_Join(int Page, string Apt_Code);
        Task<int> Staff_Career_Join_Count(string Apt_Code);
        Task<List<Referral_career_Entity>> _Career_Feild_Search(string Feild, string Query);

        /// <summary>
        /// 이름 찾기 
        /// </summary>
        Task<string> UserName(string User_ID);

        /// <summary>
        /// 단지별 관리소장 목록
        /// </summary>
        /// <param name="Post"></param>
        /// <returns></returns>
        Task<List<Referral_career_Entity>> GetList_Apt_Sojang(string Apt_Code);

        /// <summary>
        /// 해당 공동주택 근무자 상세정보
        /// </summary>
        Task<Staff_Career_Entity> Details_Staff_Career_Join(string User_Code);

        /// <summary>
        /// 배치 및 퇴직 정보 수 구하기(new)
        /// </summary>
        Task<int> GetListStaffCarrerSearchCount(string Apt_Code, string Post_Code, string Division);

        /// <summary>
        /// 배치정보 찾기(new)
        /// </summary>
        Task<List<Staff_Career_Entity>> GetListStaffCarrerSearch(int Page, string Apt_Code, string Post_Code, string Division);



    }

    /// <summary>
    /// 직원정보 불러오기
    /// </summary>
    public interface IStaff_Career_Lib
    {
        Task<List<Staff_Career_Entity>> StaffCareer_Join(string Apt_Code, string Division);
        /// <summary>
        /// 직원아이디로 배치 및 회원 정보 불러오기
        /// </summary>
        Task<Staff_Career_Entity> Details_Staff_Career(string UserID);

        /// <summary>
        /// 해당 공동주택 근무자 목록(Division = A면 근무자, B면 비근무자) 
        /// </summary>
        Task<List<Staff_Career_Entity>> StaffCareer_Join_Users(string User_Code);
    }

    public interface IService_Worker_Lib
    {
        Task<Service_Worker_Entity> Add(Service_Worker_Entity _Entity);
        Task<List<Service_Worker_Entity>> GetList(string AptCode, string Service_Code);
        Task Delete(string Num);
        Task<int> Worker_Count();

    }

    public interface IPresent_Lib
    {
        Task Add(Present_Entity pe);
        Task<List<Present_Entity>> GetLists(string YearDate, string MonthDate, string DayDate, string User_Code);
    }
 }
