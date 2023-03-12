using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Erp_Apt_Lib.Community
{
    public interface ICommunity_Lib
    {
        /// <summary>
        /// 커뮤니티 이용정보 입력
        /// </summary>
        Task<int> Add(Community_Entity cn);

        /// <summary>
        /// 커뮤니티 이용정보 수정
        /// </summary>
        Task Edit(Community_Entity cn);

        /// <summary>
        /// 커뮤니티 이용정보 전체 관리자 목록
        /// </summary>
        /// <param name="Page"></param>
        /// <returns></returns>
        Task<List<Community_Entity>> GetList(int Page);

        /// <summary>
        /// 입력된 전체 수
        /// </summary>
        Task<int> GetListCount();

        Task<List<Community_Entity>> GetListApt(int Page, string AptCode);
        Task<int> GetListCountApt(string AptCode);

        /// <summary>
        /// 미승인 신청자 명단 목록
        /// </summary>        
        Task<List<Community_Entity>> GetListApt_NewList(int Page, string AptCode);

        /// <summary>
        /// 미승인 신청자 명단 목록 수
        /// </summary>
        /// <param name="AptCode"></param>
        /// <returns></returns>
        Task<int> GetListCountApt_NewList(string AptCode);

        /// <summary>
        /// 스크린골프 신청자 명단
        /// </summary>
        Task<List<Community_Entity>> GetListApt_Golf_NewList(int Page, string AptCode); //GetListApt_NewList

        /// <summary>
        /// 스크린 골프 신청자 명단 수
        /// </summary>
        Task<int> GetListCountApt_Golf_NewList(string AptCode);

        Task<List<Community_Entity>> GetListSearch(int Page, string AptCode, string Field, string Query);
        Task<int> GetListCountSearch(string AptCode, string Field, string Query);
        Task<List<Community_Entity>> GetListSearchDongHo(int Page, string AptCode, string Dong, string Ho, string StartDate, string EndDate);

        /// <summary>
        /// 세대 정보 불러오기 2022
        /// </summary>
        Task<List<Community_Entity>> GetListDongHo(string AptCode, string Dong, string Ho);

        /// <summary>
        /// 이용자 이용정보 불러오기 2022
        /// </summary>
        Task<List<Community_Entity>> GetListDongHo_Personal(string AptCode, string Dong, string Ho, string Name);

        /// <summary>
        /// 해당 세대 월 이용 수
        /// </summary>
        Task<int> GetListCountSearchDongHo(string AptCode, string Dong, string Ho, string StartDate, string EndDate);

        /// <summary>
        /// 동호 해당월 사용내역
        /// </summary>
        Task<List<Community_Entity>> GetListDongHoDate(string AptCode, string Dong, string Ho, string StartDate, string EndDate);


        Task<Community_Entity> Details(int Aid);
        
        Task Remove(int Aid);

        /// <summary>
        /// 신청자 정보 검색 목록(호)
        /// </summary>
        Task<List<Community_Entity>> GetListApt_NewList_Sa_Ho(int Page, string AptCode, string Dong, string Ho);

        /// <summary>
        /// 신청자 정보 검색 목록 수(호)
        /// </summary>
        Task<int> GetListCountApt_NewList_Sa_Ho(string AptCode, string Dong, string Ho);

        /// <summary>
        /// 신청자 정보 검색 목록(이용장소)
        /// </summary>
        Task<List<Community_Entity>> GetListApt_NewList_Sa(int Page, string AptCode, string UsingKindCode);

        /// <summary>
        /// 신청자 정보 검색 목록 수(이용장소)
        /// </summary>
        Task<int> GetListCountApt_NewList_Sa(string AptCode, string UsingKindCode);

        /// <summary>
        /// 신청자 정보 검색 목록(이용방법)
        /// </summary>
        Task<List<Community_Entity>> GetListApt_NewList_Sb(int Page, string AptCode, string UsingKindCode, string Ticket_Code);

        /// <summary>
        /// 신청자 정보 검색 목록 수(이용방법)
        /// </summary>
        Task<int> GetListCountApt_NewList_Sb(string AptCode, string UsingKindCode, string Ticket_Code);

        Task<int> KindCount(string AptCode, string Query, string StartDate, string EndDate);
        Task<int> KindSum(string AptCode, string Query, string StartDate, string EndDate);

        /// <summary>
        /// 월 총액
        /// </summary>
        Task<int> MonthSum(string AptCode, string StartDate, string EndDate);

        /// <summary>
        /// 파일 추가 삭제
        /// </summary>
        Task FilesCount(int Aid, string Sort);

        /// <summary>
        /// 동호로 해당 검색 목록
        /// </summary>
        Task<List<Community_Entity>> Search_List(int Page, string AptCode, string StartDate, string EndDate, string Dong, string Ho);

        /// <summary>
        /// 동호로 검색 목록
        /// </summary>
        Task<List<Community_Entity>> Search_List_All(int Page, string AptCode, string Dong, string Ho);

        /// <summary>
        /// 해당동호로 검색된 수
        /// </summary>
        Task<int> Search_ListCount(string AptCode, string StartDate, string EndDate, string Dong, string Ho);

        /// <summary>
        /// 해당 월에 동호로 검색된 수
        /// </summary>
        Task<int> Search_List_All_Count(string AptCode, string Dong, string Ho);

        // <summary>
        /// 동호로 해당월 검색 목록
        /// </summary>
        Task<List<Community_Entity>> Search_Month_List(int Page, string AptCode, string EndDate, string StartDate);

        /// <summary>
        /// 해당 월로 검색된 수
        /// </summary>
        Task<int> Search_Month_ListCount(string AptCode, string EndDate, string StartDate);

        /// <summary>
        /// 해당 월로 검색된 수
        /// </summary>
        Task<int> Search_Month_Place_ListCount(string AptCode, string EndDate, string StartDate, string Place_Code);

        /// <summary>
        /// 같은 세대 중복 신청 정보 목록
        /// </summary>
        Task<List<Community_Entity>> RepeatList(string AptCode, string StartDate, string EndDate, string UsingKindName, int Count);

        /// <summary>
        /// 같은 세대 중복 신청 정보 목록 수
        /// </summary>
       Task<int> RepeatList_Count(string AptCode, string StartDate, string EndDate, string UsingKindName, int Count);

        /// <summary>
        /// 시설별 신청 정보 목록
        /// </summary>
        Task<List<Community_Entity>> PlaceList(int Page, string AptCode, string StartDate, string EndDate, string UsingKindName);

        /// <summary>
        /// 시설별 신청 정보 목록 수
        /// </summary>
        Task<int> PlaceList_Count(string AptCode, string StartDate, string EndDate, string UsingKindName);

        /// <summary>
        /// 시설별 신청 정보 목록(Code)
        /// </summary>
        Task<List<Community_Entity>> PlaceList_Code(int Page, string AptCode, string StartDate, string EndDate, string UsingKindCode);

        /// <summary>
        /// 시설별 신청 정보 목록 수(Code)
        /// </summary>
        Task<int> PlaceList_Code_Count(string AptCode, string StartDate, string EndDate, string UsingKindCode);

        /// <summary>
        /// 커뮤니티 시설명 목록
        /// </summary>
        Task<List<Community_Entity>> UsingKindName(string Apt_Code);

        /// <summary>
        /// 동호로 해당월 검색 목록
        /// </summary>
        Task<List<Community_Entity>> Month_Apt_List(string AptCode, string StartDate, string EndDate, string Place_Code);

        /// <summary>
        /// 해당 월에 동호로 검색된 합계
        /// </summary>
        Task<int> Search_List_All_Sum(string AptCode, string StartDate, string EndDate, string Dong, string Ho);

        /// <summary>
        /// 월 결산 목록
        /// </summary>
        Task<List<MonthTotalSum_Entity>> Month_Sum(string Apt_Code, string StartDate, string EndDate);

        /// <summary>
        /// 승인하기
        /// </summary>
        Task Approval(int Aid);

        /// <summary>
        /// 지문 식별코드 불러오기 2022
        /// </summary>
        Task<string> ByUserCode(string AptCode, string Dong, string Ho, string UserName);

        /// <summary>
        /// 선청인 해당 월 시설 이용 정보 존재 여부 2022
        /// </summary>
        Task<int> BeingCount(string AptCode, string KindCode, string Ticket_Code, string Dong, string Ho, string UserName, string Mobile, DateTime StartDate, DateTime EndDate);

        /// <summary>
        /// 해당월 이용 여부 확인
        /// </summary>
        Task<int> BeingCount_DongHo(string AptCode, string KindCode, string Ticket_Code, string Dong, string Ho, string Mobile, DateTime StartDate, DateTime EndDate);

        /// <summary>
        /// 해당 시간 존재 여부 2022
        /// </summary>
        Task<int> HourBeingCount(string AptCode, string KindCode, DateTime StartDate, DateTime EndDate, int StartHour);

        /// <summary>
        /// 해당 세대 시설이용 월로 등록여부 확인
        /// </summary>
        Task<int> DongHoSameCount(string AptCode, string Dong, string Ho, string UsingKindCode, string StartDate, string EndDate);
        
        /// <summary>
        /// 월 신청자 정보 목록
        /// </summary>
        Task<List<Community_Entity>> Month_Input_List(string Apt_Code, string StartDate, string EndDate);

        /// <summary>
        /// 이름으로 검색된 정보 목록 수
        /// </summary>
        Task<int> NameList_Count(string AptCode, string StartDate, string EndDate, string UserName);

        /// <summary>
        /// 이름으로 검색된 정보 목록
        /// </summary>
        Task<List<Community_Entity>> NameList(int Page, string AptCode, string StartDate, string EndDate, string UserName);


        /// <summary>
        /// 스크린 골프 신청자 정보 목록 수
        /// </summary>
        int GolfSceenInfor(string AptCode, string StartDate, string EndDate, string UsingKindCode, int Hour);

        /// <summary>
        /// 관리자 모두 보기
        /// </summary>
        Task<List<Community_Entity>> All_List(int Page);

        /// <summary>
        /// 관리자 모두 보기 목록 수
        /// </summary>
        Task<int> All_List_Count();

        /// <summary>
        /// 신청순서 값 불러오기
        /// </summary>
        Task<int> OrderBy(string Apt_Code, string UsingKindCode, string Ticket_Code, DateTime StartDate, DateTime EndDate);

        // <summary>
        /// 이용방법 목록(공동주택식별코드, 시작일, 종료일, 시설 코드, 이용방법 코드) 20221226
        /// </summary>
        Task<List<Community_Entity>> TicketList(int Page, string AptCode, string StartDate, string EndDate, string UsingKindCode, string Ticket_Code);

        /// <summary>
        /// 이용방법 목록 수(공동주택식별코드, 시작일, 종료일, 시설 코드, 이용방법 코드) 20221226
        /// </summary>
        Task<int> TicketList_Count(string AptCode, string StartDate, string EndDate, string UsingKindCode, string Ticket_Code);

        /// <summary>
        /// 신청순서 변경 20221226
        /// </summary>
        Task OrderBy_Edit(int Aid, int OrderBy);
    }

    public interface ICommunityUsingKind_Lib
    {
        Task<int> Add(CommunityUsingKind_Entity cu);
        Task Edit(CommunityUsingKind_Entity cu);
        Task<List<CommunityUsingKind_Entity>> GetList(int Page);
        Task<int> GetListCount();
        Task<List<CommunityUsingKind_Entity>> GetList_Apt(string AptCode);
        Task<string> KindName(string Kind_Code);
        Task Remove(int Aid, string Using);

        /// <summary>
        /// 마지막 일련번호
        /// </summary>
        Task<string> ListAid();
    }

    public interface ICommunityUsingTicket_Lib
    {
        Task<int> Add(CommunityUsingTicket_Entity cu);
        Task Edit(CommunityUsingTicket_Entity cu);
        Task<List<CommunityUsingTicket_Entity>> GetList(int Page);
        Task<int> GetListCount();
        Task<List<CommunityUsingTicket_Entity>> GetList_Apt(string AptCode);

        /// <summary>
        /// 이용장소의 이용방법 목록
        /// </summary>
        Task<List<CommunityUsingTicket_Entity>> GetList_Apt_Kind(string AptCode, string Kind_Code);

        /// <summary>
        /// 이용방법 상세보기
        /// </summary>
        Task<CommunityUsingTicket_Entity> Details(string Aid);

        /// <summary>
        /// 이용방법 코드로 상세보기
        /// </summary>
        Task<CommunityUsingTicket_Entity> Details_Code(string Ticket_Code);

        Task<string> TicketName(string Ticket_Code);
        Task Remove(int Aid, string Using);

        /// <summary>
        /// 마지막 일련번호
        /// </summary>
        /// <returns></returns>
        Task<string> ListAid();
    }
}
