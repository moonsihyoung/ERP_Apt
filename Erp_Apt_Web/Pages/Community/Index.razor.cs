using Erp_Apt_Web.Data;
using Erp_Apt_Lib;
using Erp_Apt_Lib.Community;
using Erp_Entity;
using Erp_Lib;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Http;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Wedew_Lib;
using System.ComponentModel;

namespace Erp_Apt_Web.Pages.Community
{
    public partial class Index
    {
        #region Inject
        [Parameter] public string Code { get; set; }
        [CascadingParameter] public Task<AuthenticationState> AuthenticationStateRef { get; set; }
        [Inject] public NavigationManager MyNav { get; set; }
        [Inject] IJSRuntime JSRuntime { get; set; }
        [Inject] public IErp_AptPeople_Lib Erp_AptPeople_Lib { get; set; }
        [Inject] public ICommunityUsingKind_Lib communityUsingKind { get; set; }
        [Inject] public ICommunityUsingTicket_Lib communityUsingTicket { get; set; }
        [Inject] public ICommunity_Lib community_Lib { get; set; }
        [Inject] public ISw_Files_Lib files_Lib { get; set; }
        [Inject] public Container container { get; set; }
        [Inject] IHttpContextAccessor contextAccessor { get; set; }
        #endregion

        #region 목록
        List<Community_Entity> ann { get; set; } = new List<Community_Entity>();
        List<Community_Entity> annU { get; set; } = new List<Community_Entity>();
        List<Community_Entity> annUA { get; set; } = new List<Community_Entity>();
        List<CommunityUsingKind_Entity> annA { get; set; } = new List<CommunityUsingKind_Entity>();
        List<CommunityUsingTicket_Entity> annB { get; set; } = new List<CommunityUsingTicket_Entity>();
        List<CommunityUsingTicket_Entity> annBB { get; set; } = new List<CommunityUsingTicket_Entity>();

        Community_Entity bnn { get; set; } = new Community_Entity();
        Community_Entity vnn { get; set; } = new Community_Entity();
        CommunityUsingKind_Entity bnnA { get; set; } = new CommunityUsingKind_Entity();
        CommunityUsingTicket_Entity bnnB { get; set; } = new CommunityUsingTicket_Entity();
        List<Apt_People_Entity> Dong { get; set; } = new List<Apt_People_Entity>();
        //List<Apt_People_Entity> DongA { get; set; } = new List<Apt_People_Entity>();
        List<Apt_People_Entity> Ho { get; set; } = new List<Apt_People_Entity>();
        //List<Apt_People_Entity> HoA { get; set; } = new List<Apt_People_Entity>();
        List<Apt_People_Entity> Name { get; set; } = new List<Apt_People_Entity>();
        Apt_People_Entity ape { get; set; } = new Apt_People_Entity();
        #endregion

        #region 변수
        public string User_Code { get; set; }
        public string User_Name { get; set; }
        public string Apt_Code { get; set; }
        public string Apt_Name { get; set; }
        public string UserName { get; set; }
        public int LevelCount { get; set; }
        public string InsertViews { get; set; } = "A";
        public string ViewsA { get; set; } = "A";
        public string ViewsB { get; set; } = "A";
        public string InsertViewsA { get; set; } = "A";
        public string InsertViewsB { get; set; } = "A";

        public string SearchViews { get; set; } = "A";

        public string strTitle { get; set; }
        public int RepeatCount { get; set; } = 0;

        #endregion

        /// <summary>
        /// 페이징
        /// </summary>
        protected DulPager.DulPagerBase pager = new DulPager.DulPagerBase()
        {
            PageNumber = 1,
            PageIndex = 0,
            PageSize = 15,
            PagerButtonCount = 5
        };

        /// <summary>
        /// 페이징
        /// </summary>
        protected async void PageIndexChanged(int pageIndex)
        {
            pager.PageIndex = pageIndex;
            pager.PageNumber = pageIndex + 1;

            await DisplayData();

            StateHasChanged();
        }

        /// <summary>
        /// 로드시 실행
        /// </summary>
        protected override async Task OnInitializedAsync()
        {
            var authState = await AuthenticationStateRef;
            if (authState.User.Identity.IsAuthenticated)
            {
                Apt_Code = authState.User.Claims.FirstOrDefault(c => c.Type == "Apt_Code")?.Value;
                Apt_Name = authState.User.Claims.FirstOrDefault(c => c.Type == "Apt_Name")?.Value;
                User_Code = authState.User.Claims.FirstOrDefault(c => c.Type == "User_Code")?.Value;
                User_Name = authState.User.Claims.FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name")?.Value;
                LevelCount = Convert.ToInt32(authState.User.Claims.FirstOrDefault(c => c.Type == "LevelCount")?.Value);
                Dong = await Erp_AptPeople_Lib.DongList(Apt_Code); //동이름 목록
                annU = await community_Lib.UsingKindName(Apt_Code); //시설명 목록
                annUA = await community_Lib.UsingKindName(Apt_Code);
                await DisplayData();
            }
            else
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "로그인되지 않았습니다..");
                MyNav.NavigateTo("/");
            }
        }


        /// <summary>
        /// 아이피 추출 
        /// </summary>
        public string MyIpAdress { get; set; }
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await JSRuntime.InvokeVoidAsync("setModalDraggableAndResizable");
            await base.OnAfterRenderAsync(firstRender);

            if (firstRender)
            {
                MyIpAdress = contextAccessor.HttpContext.Connection?.RemoteIpAddress.ToString();
                StateHasChanged();
            }
        }


        /// <summary>
        /// 데이터 뷰
        /// </summary>
        #region MyRegion
        public int helCount { get; set; }
        public int gofCount { get; set; }
        public int swMCount { get; set; }
        public int lakCount { get; set; }
        public int swWCount { get; set; }
        public int reMCount { get; set; }
        public int reWCount { get; set; }
        public int GXCount { get; set; }
        public int PTCount { get; set; }
        public int helSum { get; set; }
        public int gofSum { get; set; }
        public int swMSum { get; set; }
        public int swWSum { get; set; }
        public int reMSum { get; set; }
        public int reWSum { get; set; }
        public int PTSum { get; set; }

        public int lakSum { get; set; }
        public int ScreenSum { get; set; }
        public int GxSum { get; set; }

        public int monthSum { get; private set; }
        public string strDate { get; set; }
        public int intYear { get; private set; } = DateTime.Now.Year;
        public int intMonth { get; private set; } = DateTime.Now.Month;
        public int intScreen { get; set; }
        public string strNowDate { get; set; }

        #endregion

        /// <summary>
        /// 데이터 뷰
        /// </summary>
        private async Task DisplayData()
        {
            strNowDate = DateTime.Now.ToShortDateString();

            if (strSort == "A") // 해당공동주택에 모든 정보 
            {
                pager.RecordCount = await community_Lib.GetListCountApt(Apt_Code);
                ann = await community_Lib.GetListApt(pager.PageIndex, Apt_Code);
                annA = await communityUsingKind.GetList_Apt(Apt_Code);
                annB = await communityUsingTicket.GetList_Apt(Apt_Code);

                await TotalInfor();
            }
            else if (strSort == "B") //기간 및 동호로 검색
            {
                pager.RecordCount = await community_Lib.Search_ListCount(Apt_Code, strStartDate, strEndDate, strDoong, strHoo);
                ann = await community_Lib.Search_List(pager.PageIndex, Apt_Code, strStartDate, strEndDate, strDoong, strHoo);
                annA = await communityUsingKind.GetList_Apt(Apt_Code);
                annB = await communityUsingTicket.GetList_Apt(Apt_Code);
                Ho_Sum = await community_Lib.Search_List_All_Sum(Apt_Code, strStartDate, strEndDate, strDoong, strHoo);
                await TotalInfor();

            }
            else if (strSort == "C") //동호로 검색된 
            {
                pager.RecordCount = await community_Lib.Search_List_All_Count(Apt_Code, strDoong, strHoo);
                ann = await community_Lib.Search_List_All(pager.PageIndex, Apt_Code, strDoong, strHoo);
                annA = await communityUsingKind.GetList_Apt(Apt_Code);
                annB = await communityUsingTicket.GetList_Apt(Apt_Code);

                await TotalInfor();
            }
            else if (strSort == "D") //해당 아파트의 기간으로 모든 정보 출력
            {
                pager.RecordCount = await community_Lib.Search_Month_ListCount(Apt_Code, strStartDate, strEndDate);
                ann = await community_Lib.Search_Month_List(pager.PageIndex, Apt_Code, strStartDate, strEndDate);
                annA = await communityUsingKind.GetList_Apt(Apt_Code);
                annB = await communityUsingTicket.GetList_Apt(Apt_Code);
                await TotalInfor();

            }
            else if (strSort == "E") //해당 아파트 기간 시설 횟로 검색
            {
                pager.RecordCount = await community_Lib.RepeatList_Count(Apt_Code, strStartDate, strEndDate, strPlace, strCount);
                ann = await community_Lib.RepeatList(Apt_Code, strStartDate, strEndDate, strPlace, strCount);
                annA = await communityUsingKind.GetList_Apt(Apt_Code);
                annB = await communityUsingTicket.GetList_Apt(Apt_Code);

                await TotalInfor();
            }
            else if (strSort == "F") // 해당 아파트의 기간 및 시설별 종목 검색
            {
                pager.RecordCount = await community_Lib.PlaceList_Count(Apt_Code, strStartDate, strEndDate, strPlace);
                ann = await community_Lib.PlaceList(pager.PageIndex, Apt_Code, strStartDate, strEndDate, strPlace);
                annA = await communityUsingKind.GetList_Apt(Apt_Code);
                annB = await communityUsingTicket.GetList_Apt(Apt_Code);

                await TotalInfor();
            }
            else if (strSort == "G")
            {
                pager.RecordCount = await community_Lib.NameList_Count(Apt_Code, strStartDate, strEndDate, strUser_Name);
                ann = await community_Lib.NameList(pager.PageIndex, Apt_Code, strStartDate, strEndDate, strUser_Name);
                annA = await communityUsingKind.GetList_Apt(Apt_Code);
                annB = await communityUsingTicket.GetList_Apt(Apt_Code);

                await TotalInfor();
            }
            else if (strSort == "H")
            {
                pager.RecordCount = await community_Lib.TicketList_Count(Apt_Code, strStartDate, strEndDate, strKind_Code, strTicket);
                ann = await community_Lib.TicketList(pager.PageIndex, Apt_Code, strStartDate, strEndDate, strKind_Code, strTicket);
                await TotalInfor();
            }
        }


        /// <summary>
        /// 월 통계
        /// </summary>
        private async Task TotalInfor()
        {
            if (string.IsNullOrWhiteSpace(strMonth))
            {
                int dtYear = DateTime.Now.Year;
                int dtMonth = DateTime.Now.Month;
                int lastDay = DateTime.DaysInMonth(intYear, intMonth);
                string dt = dtYear + "-" + dtMonth + "-01";
                string dt1 = dtYear + "-" + dtMonth + "-" + lastDay;

                helCount = await community_Lib.KindCount(Apt_Code, "헬스장", dt, dt1);
                gofCount = await community_Lib.KindCount(Apt_Code, "골프장", dt, dt1);
                swMCount = await community_Lib.KindCount(Apt_Code, "샤워장(남)", dt, dt1);
                swMCount = await community_Lib.KindCount(Apt_Code, "샤워장(남)", dt, dt1);
                swWCount = await community_Lib.KindCount(Apt_Code, "샤워장(여)", dt, dt1);
                reMCount = await community_Lib.KindCount(Apt_Code, "독서실(여)", dt, dt1);
                reWCount = await community_Lib.KindCount(Apt_Code, "독서실(남)", dt, dt1);
                lakCount = await community_Lib.KindCount(Apt_Code, "락커", dt, dt1);
                intScreen = await community_Lib.KindCount(Apt_Code, "스크린골프장", dt, dt1);
                GXCount = await community_Lib.KindCount(Apt_Code, "GX룸", dt, dt1);
                PTCount = await community_Lib.KindCount(Apt_Code, "개인PT", dt, dt1);

                helSum = await community_Lib.KindSum(Apt_Code, "헬스장", dt, dt1);
                gofSum = await community_Lib.KindSum(Apt_Code, "골프장", dt, dt1);
                swMSum = await community_Lib.KindSum(Apt_Code, "샤워장(남)", dt, dt1);
                swWSum = await community_Lib.KindSum(Apt_Code, "샤워장(여)", dt, dt1);
                reMSum = await community_Lib.KindSum(Apt_Code, "독서실(여)", dt, dt1);
                reWSum = await community_Lib.KindSum(Apt_Code, "독서실(남)", dt, dt1);
                lakSum = await community_Lib.KindSum(Apt_Code, "락커", dt, dt1);
                ScreenSum = await community_Lib.KindSum(Apt_Code, "스크린골프장", dt, dt1);
                GxSum = await community_Lib.KindSum(Apt_Code, "GX룸", dt, dt1);
                PTSum = await community_Lib.KindSum(Apt_Code, "개인PT", dt, dt1);

                monthSum = await community_Lib.MonthSum(Apt_Code, dt, dt1);
            }
            else
            {
                intYear = Convert.ToInt32(strYear);
                intMonth = Convert.ToInt32(strMonth);
                int lastDay = DateTime.DaysInMonth(intYear, intMonth);
                string dt = strYear + "-" + strMonth + "-01";
                string dt1 = strYear + "-" + strMonth + "-" + lastDay + " 23:59:59.993";

                helCount = await community_Lib.KindCount(Apt_Code, "헬스장", dt, dt1);
                gofCount = await community_Lib.KindCount(Apt_Code, "골프장", dt, dt1);
                swMCount = await community_Lib.KindCount(Apt_Code, "샤워장(남)", dt, dt1);
                swMCount = await community_Lib.KindCount(Apt_Code, "샤워장(남)", dt, dt1);
                swWCount = await community_Lib.KindCount(Apt_Code, "샤워장(여)", dt, dt1);
                reMCount = await community_Lib.KindCount(Apt_Code, "독서실(여)", dt, dt1);
                reWCount = await community_Lib.KindCount(Apt_Code, "독서실(남)", dt, dt1);
                lakCount = await community_Lib.KindCount(Apt_Code, "락커", dt, dt1);
                intScreen = await community_Lib.KindCount(Apt_Code, "스크린골프장", dt, dt1);
                GXCount = await community_Lib.KindCount(Apt_Code, "GX룸", dt, dt1);
                PTCount = await community_Lib.KindCount(Apt_Code, "개인PT", dt, dt1);

                helSum = await community_Lib.KindSum(Apt_Code, "헬스장", dt, dt1);
                gofSum = await community_Lib.KindSum(Apt_Code, "골프장", dt, dt1);
                swMSum = await community_Lib.KindSum(Apt_Code, "샤워장(남)", dt, dt1);
                swWSum = await community_Lib.KindSum(Apt_Code, "샤워장(여)", dt, dt1);
                reMSum = await community_Lib.KindSum(Apt_Code, "독서실(여)", dt, dt1);
                reWSum = await community_Lib.KindSum(Apt_Code, "독서실(남)", dt, dt1);
                lakSum = await community_Lib.KindSum(Apt_Code, "락커", dt, dt1);
                ScreenSum = await community_Lib.KindSum(Apt_Code, "스크린골프장", dt, dt1);
                GxSum = await community_Lib.KindSum(Apt_Code, "GX룸", dt, dt1);
                PTSum = await community_Lib.KindSum(Apt_Code, "개인PT", dt, dt1);

                monthSum = await community_Lib.MonthSum(Apt_Code, dt, dt1);
            }
        }

        /// <summary>
        /// 리셋
        /// </summary>
        private async Task btnResert()
        {
            strSort = "A";
            strHoo = "";
            strDoong = "";
            strDate = "";
            await DisplayData();
        }

        /// <summary>
        /// 동 선택 시 실행
        /// </summary>
        public string strDong { get; set; }
        private async Task OnDong(ChangeEventArgs a)
        {
            bnn.Dong = a.Value.ToString();
            strDong = bnn.Dong;
            strDoong = strDong;
            Ho = new List<Apt_People_Entity>();
            Ho = await Erp_AptPeople_Lib.DongHoList_new(Apt_Code, bnn.Dong);
        }

        /// <summary>
        /// 호 선택 시 실행
        /// </summary>
        public string strHo { get; set; }
        public int Ho_Sum { get; set; }
        private async Task OnHo(ChangeEventArgs a)
        {
            if (a != null)
            {
                strHo = a.Value.ToString();
                strHoo = strHo;
                bnn.Ho = a.Value.ToString();
                Name = await Erp_AptPeople_Lib.Dong_Ho_Name_List(Apt_Code, bnn.Dong, bnn.Ho);
            }
        }

        /// <summary>
        /// 이용자 정보
        /// </summary>
        public string strName { get; set; }
        private async Task OnName(ChangeEventArgs a)
        {
            if (a != null)
            {
                ape = await Erp_AptPeople_Lib.Dedeils_Name(a.Value.ToString());
                strName = ape.Num.ToString();
                bnn.UserName = ape.InnerName;
                bnn.Mobile = ape.Hp;
            }
        }

        /// <summary>
        /// 이용현황 열기
        /// </summary>
        private void btnInsert()
        {
            if (LevelCount >= 5)
            {
                strTitle = "이용자 등록";
                bnn = new Community_Entity();
                int YearA = DateTime.Now.Year;
                int MonthA = DateTime.Now.Month;

                int re = DateTime.DaysInMonth(YearA, MonthA);

                bnn.UserStartDate = Convert.ToDateTime(YearA + "-" + MonthA + "-01");
                bnn.UserEndDate = Convert.ToDateTime(YearA + "-" + MonthA + "-" + re);
                bnn.ScamDays = (bnn.UserEndDate.Day - bnn.UserStartDate.Day) + 1;
                InsertViews = "B";
            }
        }

        /// <summary>
        /// 이용현황 수정 열기
        /// </summary>
        /// <param name="cn"></param>
        private async Task ByEdit(Community_Entity cn)
        {
            if (LevelCount >= 5)
            {
                try
                {
                    strTitle = "이용자 정보 수정";

                    bnn = cn;
                    strDong = bnn.Dong;
                    Ho = new List<Apt_People_Entity>();
                    Ho = await Erp_AptPeople_Lib.HoList(Apt_Code, bnn.Dong);
                    strHo = bnn.Ho;
                    strScamHours = bnn.UserStartHour;
                    Name = await Erp_AptPeople_Lib.Dong_Ho_Name_List(Apt_Code, bnn.Dong, bnn.Ho);

                    ape = await Erp_AptPeople_Lib.Dedeils_Name(Apt_Code, bnn.UserName, bnn.Dong, bnn.Ho);
                    strName = ape.Num.ToString();

                    strUsingKindCode = bnn.UsingKindCode;
                    //strUsingTicketCode
                    annBB = await communityUsingTicket.GetList_Apt_Kind(Apt_Code, strUsingKindCode);
                    var dr = await communityUsingTicket.Details_Code(bnn.Ticket_Code);
                    strUsingTicketCode = dr.Aid.ToString();
                    InsertViews = "B";
                }
                catch (Exception)
                {
                    await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "문제가 발생하였습니다.. 관리자에게 문의하세요.");
                }
            }
            else
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "권한이 없습니다..");
            }
        }

        /// <summary>
        /// 이용장소 입력 열기
        /// </summary>
        private void btnInsertKind()
        {
            if (LevelCount >= 5)
            {
                strTitle = "이용장소 정보 열기";
                bnnA = new CommunityUsingKind_Entity();
                ViewsA = "B";
            }
        }

        /// <summary>
        /// 이용정보 삭제
        /// </summary>
        private async Task ByRemove(Community_Entity ce)
        {
            if (LevelCount >= 5)
            {
                bool isDelete = await JSRuntime.InvokeAsync<bool>("confirm", $"{ce.Aid} 번 이용정보을 정말로 삭제하시겠습니까?");

                if (isDelete)
                {
                    await community_Lib.Remove(ce.Aid);
                }
                await DisplayData();
                pagerA.RecordCount = await community_Lib.GetListCountApt_NewList(Apt_Code);
                wnn = await community_Lib.GetListApt_NewList(pagerA.PageIndex, Apt_Code);

                pagerB.RecordCount = await community_Lib.GetListCountApt_Golf_NewList(Apt_Code);
                cnList = await community_Lib.GetListApt_Golf_NewList(pagerB.PageIndex, Apt_Code);
            }
        }

        /// <summary>
        /// 이용방법 정보 열기
        /// </summary>
        private void btnInsertTicket()
        {
            if (LevelCount >= 5)
            {
                strTitle = "이용방법 정보";
                ViewsB = "B";
            }
        }

        /// <summary>
        /// 이용현황 입력
        /// </summary>
        //public int strScamHours { get; set; } = 0;
        private async Task btnSave()
        {
            bnn.AptCode = Apt_Code;
            bnn.AptName = Apt_Name;
            //bnn.User_Code = User_Code;
            bnn.Division = "A";
            bnn.Mobile_Use = "A";

            bnn.PostIP = MyIpAdress;

            if (string.IsNullOrWhiteSpace(bnn.AptCode))
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "로그인되지 않았습니다..");
            }
            else if (string.IsNullOrWhiteSpace(bnn.Ho))
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "동호를 선택하지 않았습니다..");
            }
            else if (string.IsNullOrWhiteSpace(bnn.UserCode))
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "이용아이디를 입력하지 않았습니다..");
            }
            else if (string.IsNullOrWhiteSpace(bnn.Ticket))
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "이용방법을 선택하지 않았습니다..");
            }
            else if (bnn.UseCost < 1)
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "이용금액을 입력하지 않았습니다..");
            }
            else if (bnn.ScamDays < 1)
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "이용일수를 입력하지 않았습니다..");
            }
            else if (string.IsNullOrWhiteSpace(bnn.UsingKindName))
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "이용장소를 선택하지 않았습니다..");
            }
            else if (LevelCount <= 5)
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "권한이 없습니다..");
            }
            else if (bnn.UsingKindCode == "Kd4" && strScamHours < 1)
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "스크린 골프 이용시간이 0일 수는 없습니다..");
            }
            else
            {
                bnn.User_Code = User_Code;
                bnn.OrderBy = await community_Lib.OrderBy(bnn.AptCode, bnn.UsingKindCode, bnn.Ticket_Code, bnn.UserStartDate, bnn.UserEndDate) + 1;

                if (bnn.Aid < 1)
                {
                    int being = await community_Lib.BeingCount(bnn.AptCode, bnn.UsingKindCode, bnn.Ticket_Code, bnn.Dong, bnn.Ho, bnn.UserName, bnn.Mobile, bnn.UserStartDate, bnn.UserEndDate);

                    if (bnn.UsingKindCode != "Kd4" && being < 1)
                    {
                        bnn.Approval = "B";
                        await community_Lib.Add(bnn);

                        strScamHours = 0;
                        bnn.ScamDays = 0;
                        strName = "";
                        strHo = "";
                        strUsingKindCode = "";
                        strUsingTicketCode = "";

                        bnn = new Community_Entity();
                        int YearA = DateTime.Now.Year;
                        int MonthA = DateTime.Now.Month;

                        int re = DateTime.DaysInMonth(YearA, MonthA);

                        bnn.UserStartDate = Convert.ToDateTime(YearA + "-" + MonthA + "-01");
                        bnn.UserEndDate = Convert.ToDateTime(YearA + "-" + MonthA + "-" + re);
                        bnn.ScamDays = (bnn.UserEndDate.Day - bnn.UserStartDate.Day) + 1;

                        await DisplayData();
                    }
                    else if (bnn.UsingKindCode == "Kd4")
                    {
                        int be = await community_Lib.HourBeingCount(Apt_Code, bnn.UsingKindCode, bnn.UserStartDate, bnn.UserEndDate, bnn.UserStartHour);
                        if (be < 1)
                        {
                            bnn.Approval = "B";
                            await community_Lib.Add(bnn);
                            strScamHours = 0;
                            //bnn.ScamDays = 0;
                            strName = "";
                            strHo = "";
                            strUsingKindCode = "";
                            strUsingTicketCode = "";

                            bnn = new Community_Entity();
                            int YearA = DateTime.Now.Year;
                            int MonthA = DateTime.Now.Month;

                            int re = DateTime.DaysInMonth(YearA, MonthA);

                            bnn.UserStartDate = Convert.ToDateTime(YearA + "-" + MonthA + "-01");
                            bnn.UserEndDate = Convert.ToDateTime(YearA + "-" + MonthA + "-" + re);
                            bnn.ScamDays = (bnn.UserEndDate.Day - bnn.UserStartDate.Day) + 1;

                            await DisplayData();

                            //StateHasChanged();
                        }
                        else
                        {
                            await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", bnn.UserStartHour + " 시간은 이미 예약되어 있습니다. \n 다시 확인해보세요..");
                        }
                    }
                    else
                    {
                        await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", bnn.UsingKindName + " 이용은 이미 입력되었습니다. \n 다시 확인해보세요..");
                    }
                }
                else
                {
                    await community_Lib.Edit(bnn);
                    strScamHours = 0;
                    bnn.ScamDays = 0;
                    strName = "";
                    strHo = "";
                    await DisplayData();
                }
            }
        }

        /// <summary>
        /// 이용현황 입력 닫기
        /// </summary>
        private void btnClose()
        {
            InsertViews = "A";
        }

        /// <summary>
        /// 이용장소 선택 시 실행
        /// </summary>
        public string strUsingKindCode { get; set; }
        public string strSlectCode { get; set; } = "A";
        private async Task OnKind(ChangeEventArgs a)
        {
            //try
            //{
            strUsingKindCode = a.Value.ToString();
            bnn.UsingKindCode = strUsingKindCode;
            bnnB.Kind_Code = strUsingKindCode;
            bnn.UsingKindName = await communityUsingKind.KindName(bnn.UsingKindCode);
            bnnB.Kind_Name = bnn.UsingKindName;

            if (strUsingKindCode == "Kd4")
            {
                strSlectCode = "B";
                bnn.ScamDays = 1;
            }
            else
            {

                int Year = bnn.UserEndDate.Year;
                int Month = bnn.UserEndDate.Month;
                int ra = DateTime.DaysInMonth(Year, Month);
                DateTime dt1 = Convert.ToDateTime(Year + "-" + Month + "-01");
                DateTime dt2 = Convert.ToDateTime(Year + "-" + Month + "-" + ra);
                strSlectCode = "A";
                //bnn.UserStartDate = dt1;
                //bnn.UserEndDate = dt2;
                bnn.ScamDays = ra;
            }

            annBB = await communityUsingTicket.GetList_Apt_Kind(Apt_Code, strUsingKindCode);
            strUsingTicketCode = "";
        }

        /// <summary>
        /// 이용방법 선택 시 실행
        /// </summary>
        public string strUsingTicketCode { get; set; }
        private async Task OnTicket(ChangeEventArgs a)
        {
            try
            {
                bnnB = await communityUsingTicket.Details(a.Value.ToString());
                strUsingTicketCode = bnnB.Aid.ToString();
                bnn.Ticket = bnnB.Ticket_Name;
                bnn.Ticket_Code = bnnB.Ticket_Code;
                bnn.Dong = strDong;
                bnn.Ho = strHo;
                if (!string.IsNullOrWhiteSpace(bnn.UsingKindCode))
                {
                    bnn.UseCost = bnnB.Ticket_Cost;

                    string dt1 = bnn.UserStartDate.ToShortDateString();
                    string dt2 = bnn.UserEndDate.ToShortDateString();

                    if (!string.IsNullOrWhiteSpace(bnn.UsingKindCode))
                    {
                        int intSame = await community_Lib.DongHoSameCount(Apt_Code, bnn.Dong, bnn.Ho, bnn.UsingKindCode, dt1, dt2);

                        if (bnn.UsingKindCode == "Kd0" || bnn.UsingKindCode == "Kd1")
                        {
                            if (intSame > 0)
                            {
                                double db = (bnnB.Ticket_Cost * 0.8);
                                bnn.UseCost = Convert.ToInt32(db);
                            }
                            else
                            {
                                bnn.UseCost = bnnB.Ticket_Cost;
                            }
                        }
                        else
                        {
                            bnn.UseCost = bnnB.Ticket_Cost;
                        }
                    }
                    else
                    {
                        bnn.UseCost = 0;
                    }
                    bnn.Etc = bnn.Dong + "동 " + bnn.Ho + "호 " + bnn.UserName + "씨가 " + bnn.UsingKindName + "을 " + bnn.Ticket + "으로 " + bnn.UserStartDate + "부터 " + bnn.UserEndDate + "까지 신청함";
                }
                else
                {
                    bnn.UseCost = 0;
                }
            }
            catch (Exception)
            {
                bnn.UseCost = 0;
            }

        }

        /// <summary>
        /// 날짜계산 사용일수
        /// </summary>
        private void OnScamDays(ChangeEventArgs a)
        {
            bnn.UserEndDate = Convert.ToDateTime(a.Value);
            //int ra = DateTime.DaysInMonth(bnn.UserEndDate.Year, bnn.UserEndDate.Month);
            bnn.ScamDays = (bnn.UserEndDate - bnn.UserStartDate).Days + 1;
            //strScamHours = bnn.ScamDays.ToString()
            //double ck = 0.1;

            //DateTime dt = DateTime.Now;
            //DateTime dt1 = Convert.ToDateTime(dt.Year + "-" + (dt.Month + 1) + "- 01");

            //if (bnn.UserStartDate < dt1)
            //{
            //    if (bnn.UsingKindCode != "Kd4")
            //    {
            //        bnn.UserStartDate = dt1;
            //        int lastDay = DateTime.DaysInMonth(intYear, intMonth);
            //        strEndDate = strYear + "-" + strMonth + "-" + lastDay + " 23:59:59.993";
            //        bnn.UserEndDate = Convert.ToDateTime(strEndDate);

            //        await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "현재 월 이용을 등록할 수 없습니다.. \n 다음 달 이용만 등록할 수 있습니다.");
            //    }
            //}

            #region MyRegion
            //if (bnn.ScamDays >= 1)
            //{
            //    if (ra > 0)
            //    {
            //        if (string.IsNullOrWhiteSpace(bnn.UsingKindCode))
            //        {
            //            if (bnn.UseCost > 0)
            //            {
            //                ck = Convert.ToDouble(bnn.UseCost) / ra;
            //                ck = ck * bnn.ScamDays;
            //                ck = Math.Ceiling(ck);
            //                int cb = Convert.ToInt32(ck);
            //                cb = cb / 10;
            //                cb = cb * 10;
            //                bnn.UseCost = cb;
            //            } 
            //        }
            //    }
            //}
            //else
            //{
            //    await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "사용일수가 1보다 작을 수는 없습니다..");
            //} 
            #endregion
        }

        public int strScamHours { get; set; } = 0;
        private async Task OnScamHours(ChangeEventArgs a)
        {
            if (!string.IsNullOrWhiteSpace(a.Value.ToString()))
            {
                int starttime = Convert.ToInt32(a.Value.ToString().Replace("시", ""));
                bnn.UserEndHour = starttime;
                if (bnn.UserStartHour > 5)
                {
                    bnn.UserEndHour = Convert.ToInt32(a.Value.ToString());
                    strScamHours = bnn.UserEndHour - bnn.UserStartHour;
                    if (strScamHours < 1)
                    {
                        await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "사용 시간이 1보다 작을 수는 없습니다..");
                    }
                }
                else
                {
                    await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "먼저 시작시간을 입력해주세요..");
                }
            }
            else
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "먼저 종료시간을 입력해주세요..");
            }
        }

        private async Task OnStartHour(ChangeEventArgs a)
        {
            if (!string.IsNullOrWhiteSpace(a.Value.ToString()))
            {
                int starttime = Convert.ToInt32(a.Value.ToString().Replace("시", ""));
                bnn.UserStartHour = starttime;
                strScamHours = 1;
                if (bnn.UserStartHour > 5)
                {
                    bnn.UserEndHour = bnn.UserStartHour + 1;
                }
                else
                {
                    await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "먼저 시작시간을 선택해주세요..");
                }
            }
            else
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "먼저 시작시간을 선택해주세요..");
            }
        }

        /// <summary>
        /// 이용장소 새로등록 열기
        /// </summary>
        private void btnOpenA()
        {
            if (LevelCount >= 5)
            {
                bnnA = new CommunityUsingKind_Entity();
                strTitle = "이용장소 새로 등록";
                InsertViewsA = "B";
            }
        }

        /// <summary>
        /// 이용방법 새로등록 열기
        /// </summary>
        private void btnOpenB()
        {
            if (LevelCount >= 5)
            {
                bnnB = new CommunityUsingTicket_Entity();
                strTitle = "이용방법 새로 등록";
                InsertViewsB = "B";
            }
        }

        /// <summary>
        /// 이용장소 수정 열기
        /// </summary>
        private void ByEditA(CommunityUsingKind_Entity cuk)
        {
            strTitle = "이용장소 수정";
            bnnA = cuk;
            InsertViewsA = "B";
        }

        #region 이용장소 및 방법
        /// <summary>
        /// 이용장소 미사용 만들기
        /// </summary>
        private async Task ByRemoveA(CommunityUsingKind_Entity cuk)
        {
            bool isDelete = await JSRuntime.InvokeAsync<bool>("confirm", $"{cuk.Aid}을 정말로 삭제하시겠습니까?");
            if (isDelete)
            {
                await communityUsingKind.Remove(cuk.Aid, cuk.Using);
            }
            await DisplayData();
        }

        /// <summary>
        /// 이용장소 등록
        /// </summary>
        private async Task btnSaveA()
        {
            bnnA.AptCode = Apt_Code;
            bnnA.AptName = Apt_Name;
            if (string.IsNullOrWhiteSpace(bnnA.Kind_Name))
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "이용장소를 입력하지 않았습니다..");
            }
            else
            {
                bnnA.User_Code = User_Code;
                if (bnnA.Aid < 1)
                {
                    bnnA.Kind_Code = "Kd" + await communityUsingKind.ListAid();
                    await communityUsingKind.Add(bnnA);
                }
                else
                {
                    await communityUsingKind.Edit(bnnA);
                }
                bnnA = new CommunityUsingKind_Entity();
                await DisplayData();
                InsertViewsA = "A";
            }
        }

        /// <summary>
        /// 이용장소 정보 닫기
        /// </summary>
        private void btnCloseA()
        {
            ViewsA = "A";
        }

        /// <summary>
        /// 이용장소 등록 닫기
        /// </summary>
        private void btnCloseAA()
        {
            InsertViewsA = "A";
        }

        /// <summary>
        /// 이용방법 수정 열기
        /// </summary>
        private void ByEditB(CommunityUsingTicket_Entity cuk)
        {
            strTitle = "이용방법 수정";
            bnnB = cuk;
            strUsingKindCode = bnnB.Kind_Code;
            InsertViewsB = "B";
        }

        /// <summary>
        /// 이용장소 정보 미사용
        /// </summary>
        private async Task ByRemoveB(CommunityUsingTicket_Entity cuk)
        {
            bool isDelete = await JSRuntime.InvokeAsync<bool>("confirm", $"{cuk.Aid}을 정말로 삭제하시겠습니까?");
            if (isDelete)
            {
                await communityUsingTicket.Remove(cuk.Aid, cuk.Using);
            }
            await DisplayData();
        }

        /// <summary>
        /// 이용방법 등록
        /// </summary>
        private async Task btnSaveB()
        {
            bnnB.AptCode = Apt_Code;
            bnnB.AptName = Apt_Name;
            if (string.IsNullOrWhiteSpace(bnnB.Kind_Name))
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "이용장소를 선택하지 않았습니다..");
            }
            else if (string.IsNullOrWhiteSpace(bnnB.Ticket_Name))
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "이용방법을 입력하지 않았습니다..");
            }
            else if (string.IsNullOrWhiteSpace(bnnB.AptCode))
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "로그인되지 않았습니다..");
            }
            else if (bnnB.Ticket_Cost < 1)
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "이용요금이 0일 수는 없습니다..");
            }
            else
            {
                bnnB.User_Code = User_Code;
                if (bnnB.Aid < 1)
                {
                    bnnB.Ticket_Code = "Tc" + await communityUsingTicket.ListAid();

                    await communityUsingTicket.Add(bnnB);
                }
                else
                {
                    await communityUsingTicket.Edit(bnnB);
                }
                bnnB = new CommunityUsingTicket_Entity();
                strUsingKindCode = "";
                await DisplayData();
                InsertViewsB = "A";
            }
        }

        /// <summary>
        /// 이용방법 정보 닫기
        /// </summary>
        private void btnCloseB()
        {
            ViewsB = "A";
        }

        /// <summary>
        /// 이용방법 등록 닫기
        /// </summary>
        private void btnCloseBB()
        {
            InsertViewsB = "A";
        }
        #endregion

        private void ByUp(int Aid)
        {
            strAid = Aid.ToString();
            dnn.Parents_Num = strAid;
            FileInputViews = "B";
        }

        /// <summary>
        /// 첨부파일 보기
        /// </summary>
        /// <param name="Aid"></param>
        private void ByViews(int Aid)
        {
            FileViews = "B";
        }

        /// <summary>
        /// 첨부파일 삭제
        /// </summary>
        Sw_Files_Entity dnn { get; set; } = new Sw_Files_Entity(); // 첨부 파일 정보
        List<Sw_Files_Entity> Files_Entity { get; set; } = new List<Sw_Files_Entity>();
        private async Task FilesRemove(Sw_Files_Entity _files)
        {
            if (LevelCount >= 5)
            {
                bool isDelete = await JSRuntime.InvokeAsync<bool>("confirm", $"{_files.Sw_FileName} 첨부파일을 정말로 삭제하시겠습니까?");

                if (isDelete)
                {
                    if (!string.IsNullOrEmpty(_files?.Sw_FileName))
                    {
                        // 첨부 파일 삭제 
                        //await FileStorageManagerInjector.DeleteAsync(_files.Sw_FileName, "");
                        string rootFolder = $"{env.WebRootPath}\\UpFiles\\Community\\{_files.Sw_FileName}";
                        File.Delete(rootFolder);
                    }
                    await files_Lib.FileRemove(_files.Num.ToString(), "Community", Apt_Code);

                    await community_Lib.FilesCount(bnn.Aid, "B"); //파일 수 줄이기
                                                                  //await UploadRepositoryAsyncReference.DeleteAsync(Id); // 삭제
                                                                  //NavigationManagerReference.NavigateTo("/Uploads"); // 리스트 페이지로 이동
                    Files_Count = await files_Lib.Sw_Files_Data_Index_Count("Community", bnn.Aid.ToString(), Apt_Code);
                    if (Files_Count > 0)
                    {
                        Files_Entity = await files_Lib.Sw_Files_Data_Index("Community", bnn.Aid.ToString(), Apt_Code);
                    }
                }
                else
                {
                    await JSRuntime.InvokeAsync<object>("alert", "취소되었습니다.");
                }
            }
        }

        #region Event Handlers
        private long maxFileSize = 1024 * 1024 * 30;
        [Inject] Microsoft.AspNetCore.Hosting.IWebHostEnvironment env { get; set; }
        
        public string FileInputViews { get; private set; }
        public int Files_Count { get; private set; }
        public string FileViews { get; set; } = "A";
        public string strAid { get; private set; }

        // <summary>
        /// 확정자 찾기오기
        /// </summary>
        private string OnExtension(string filesName)
        {
            return Path.GetExtension(filesName);
        }

        [Inject] ILogger<Index> Logger { get; set; }
        private List<IBrowserFile> loadedFiles = new();
        private int maxAllowedFiles = 5;
        private bool isLoading;
        private decimal progressPercent;
        public string? fileName { get; set; }
        
        private async Task LoadFiles(InputFileChangeEventArgs e)
        {
            isLoading = true;
            loadedFiles.Clear();
            progressPercent = 0;
            //intViews = 1;
            foreach (var file in e.GetMultipleFiles(maxAllowedFiles))
            {
                dnn.Parents_Num = strAid; // 선택한 ParentId 값 가져오기 
                dnn.Sub_Num = dnn.Parents_Num;
                try
                {
                    var pathA = $"{env.WebRootPath}\\UpFiles\\Community";
                    fileName = Dul.FileUtility.GetFileNameWithNumbering(pathA, file.Name);
                    //var trustedFileName = Path.GetRandomFileName();
                    var path = Path.Combine(pathA, fileName);



                    await using FileStream writeStream = new(path, FileMode.Create);
                    using var readStream = file.OpenReadStream(maxFileSize);
                    var bytesRead = 0;
                    var totalRead = 0;
                    var buffer = new byte[1024 * 1024];

                    while ((bytesRead = await readStream.ReadAsync(buffer)) != 0)
                    {
                        totalRead += bytesRead;

                        await writeStream.WriteAsync(buffer, 0, bytesRead);

                        progressPercent = Decimal.Divide(totalRead, file.Size);

                        StateHasChanged();
                    }

                    loadedFiles.Add(file);

                    dnn.Sw_FileName = fileName;
                    dnn.Sw_FileSize = Convert.ToInt32(file.Size);
                    dnn.Parents_Name = "Community";
                    dnn.AptCode = Apt_Code;
                    dnn.Del = "A";

                    #region 아이피 입력
                    string myIPAddress = "";
                    var ipentry = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName());
                    foreach (var ip in ipentry.AddressList)
                    {
                        if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                        {
                            myIPAddress = ip.ToString();
                            break;
                        }
                    }
                    dnn.PostIP = myIPAddress;
                    #endregion
                    await files_Lib.Sw_Files_Date_Insert(dnn); //첨부파일 데이터 db 저장
                }
                catch (Exception ex)
                {
                    Logger.LogError("File: {Filename} Error: {Error}",
                        file.Name, ex.Message);
                }
            }

            FileInputViews = "A";

            Files_Count = await files_Lib.Sw_Files_Data_Index_Count("Community", strAid, Apt_Code);
            if (Files_Count > 0)
            {
                Files_Entity = await files_Lib.Sw_Files_Data_Index("Community", strAid, Apt_Code);
            }

            isLoading = false;
            //intViews = 0;
        }


        #endregion

        /// <summary>
        /// 파일 업로드 열기
        /// </summary>
        private void ViewsFileInsert()
        {
            FileInputViews = "B";
        }

        /// <summary>
        /// 파일 업로드 닫기
        /// </summary>
        private void FilesClose()
        {
            FileInputViews = "A";
        }

        

        /// <summary>
        /// 첨부보기 모달 폼 닫기
        /// </summary>
        protected void FilesViewsClose()
        {
            FileViews = "A";
        }

        /// <summary>
        /// 호 세대 검색
        /// </summary>
        #region 변수
        public string strMonth { get; set; } = DateTime.Now.Month.ToString();
        public string strYear { get; set; } = DateTime.Now.Year.ToString();
        public string strDoong { get; set; }
        public string strHoo { get; set; }
        public string strSort { get; set; } = "A";
        public string strEndDate { get; set; }
        public string strStartDate { get; set; }
        #endregion
        private async Task OnHoo(ChangeEventArgs a)
        {
            //if (!string.IsNullOrWhiteSpace(strMonth))
            //{
            //    strSort = "B";
            //    strHoo = a.Value.ToString();
            //    strDate = strYear + "-" + strMonth + "-1";
            //    await DisplayData();
            //}
            //else
            //{
            strHoo = a.Value.ToString();
            strSort = "C";
            await DisplayData();
            //}
        }

        private async Task OnMonth(ChangeEventArgs a)
        {
            if (string.IsNullOrWhiteSpace(a.Value.ToString()))
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "월을 선택하지 않았습니다..");
            }
            else if (string.IsNullOrWhiteSpace(strYear))
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "년도를 선택하지 않았습니다..");
            }
            else
            {
                strMonth = a.Value.ToString();
                strDate = strYear + "-" + strMonth + "-1";

                intYear = Convert.ToInt32(strYear);
                intMonth = Convert.ToInt32(strMonth);

                int lastDay = DateTime.DaysInMonth(intYear, intMonth);

                strEndDate = strYear + "-" + strMonth + "-" + lastDay + " 23:59:59.993";
                strStartDate = strYear + "-" + strMonth + "-" + "01";
                //container.visits.Clear();
                //container.visits = await community_Lib.Month_Sum(Apt_Code, strStartDate, strEndDate);

                strSort = "D";
                await DisplayData();
            }
        }

        /// <summary>
        /// 시설별 목록
        /// </summary>
        public int strCount { get; set; }
        public string strPlace { get; set; }
        private async Task OnRepeatList(ChangeEventArgs a)
        {
            strSort = "E";
            strCount = Convert.ToInt32(a.Value);
            if (string.IsNullOrWhiteSpace(strYear))
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "년도를 선택하지 않았습니다..");
                strCount = 0;
            }
            else if (string.IsNullOrWhiteSpace(strMonth))
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "월을 선택하지 않았습니다..");
                strCount = 0;
            }
            else if (string.IsNullOrWhiteSpace(strPlace))
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "시설을 선택하지 않았습니다..");
                strCount = 0;
            }
            else if (strCount < 1)
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "중복 횟수를 선택하지 않았습니다..");
            }
            else
            {
                //await community_Lib.
                await DisplayData();
            }
        }

        /// <summary>
        /// 시설별 목록
        /// </summary>
        public string strKind_Code { get; set; }
        private async Task OnPlaceList(ChangeEventArgs a)
        {
            strSort = "F";
            strTicket = "";
            strKind_Code = a.Value.ToString();
            strPlace = await communityUsingKind.KindName(strKind_Code);
            if (string.IsNullOrWhiteSpace(strYear))
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "년도를 선택하지 않았습니다..");
                //strCount = 0;
            }
            else if (string.IsNullOrWhiteSpace(strMonth))
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "월을 선택하지 않았습니다..");
                //strCount = 0;
            }
            else if (string.IsNullOrWhiteSpace(strPlace))
            {
                //await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "시설을 선택하지 않았습니다..");
                //strCount = 0;
            }
            else
            {
                annT = await communityUsingTicket.GetList_Apt_Kind(Apt_Code, strKind_Code);
                await DisplayData();
            }
        }

        public string strTicket { get; set; }
        List<CommunityUsingTicket_Entity> annT { get; set; } = new List<CommunityUsingTicket_Entity>();
        private async Task OnTicketList(ChangeEventArgs e)
        {
            strSort = "H";
            strTicket = e.Value.ToString();
            if (string.IsNullOrWhiteSpace(strYear))
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "년도를 선택하지 않았습니다..");
                //strCount = 0;
            }
            else if (string.IsNullOrWhiteSpace(strMonth))
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "월을 선택하지 않았습니다..");
                //strCount = 0;
            }
            else if (string.IsNullOrWhiteSpace(strPlace))
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "시설장소를 선택하지 않았습니다..");
            }
            else if (string.IsNullOrWhiteSpace(strTicket))
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "이용방법을 선택하지 않았습니다..");
            }
            else
            {
                await DisplayData();
            }
        }

        /// <summary>
        /// 이름으로 검색하기
        /// </summary>
        public string strUser_Name { get; set; }
        private async Task OnNameSearch(ChangeEventArgs a)
        {
            strUser_Name = a.Value.ToString();
            if (string.IsNullOrWhiteSpace(strYear))
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "년도를 선택하지 않았습니다..");
            }
            else if (string.IsNullOrWhiteSpace(strMonth))
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "월을 선택하지 않았습니다..");
            }
            else if (string.IsNullOrWhiteSpace(strUser_Name))
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "이름을 입력하세요..");
            }
            else
            {
                strSort = "G";
                await DisplayData();
            }
        }

        /// <summary>
        /// 엑셀 파일 만들기
        /// </summary>
        private async Task GenerateExcel()
        {
            if (string.IsNullOrWhiteSpace(strStartDate))
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "검색에서 년도와 월을 선택하지 않았습니다..");
            }
            else if (string.IsNullOrWhiteSpace(strEndDate))
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "검색에서 년도와 월을 선택하지 않았습니다..");
            }
            else
            {
                #region MyRegion
                // 첨부 파일 삭제 
                //await FileStorageManagerInjector.DeleteAsync(_files.Sw_FileName, "");
                //string rootFolder = @"D:\Msh\Msh\Net_blazor";
                //string[] txtList = Directory.GetFiles(rootFolder, "#.xlsx");
                //foreach (string file in txtList)
                //{
                //    File.Delete(file);
                //} 
                #endregion
                if (System.IO.Directory.Exists(@"D:\Msh\Msh\Net_blazor"))
                {
                    string[] files = System.IO.Directory.GetFiles(@"D:\Msh\Msh\Net_blazor", Apt_Code + "*.xlsx");
                    if (files.Length > 0)
                    {
                        foreach (string s in files)
                        {
                            string fileName = System.IO.Path.GetFileName(s);
                            string deletefile = @"D:\Msh\Msh\Net_blazor\" + fileName;
                            System.IO.File.Delete(deletefile);
                        }
                    }
                }
                MyNav.NavigateTo("http://net.wedew.co.kr/Excel/GetExcelFiles?AptCode=" + Apt_Code + "&StartDate=" + strStartDate + "&EndDate=" + strEndDate, true);
            }
        }

        /// <summary>
        /// 엑셀 파일 만들기(이용 신청자 명단)
        /// </summary>
        private async Task GenerateExcelView()
        {
            if (string.IsNullOrWhiteSpace(strStartDate))
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "검색에서 년도와 월을 선택하지 않았습니다..");
            }
            else if (string.IsNullOrWhiteSpace(strEndDate))
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "검색에서 년도와 월을 선택하지 않았습니다..");
            }
            else
            {
                #region MyRegion
                // 첨부 파일 삭제 
                //await FileStorageManagerInjector.DeleteAsync(_files.Sw_FileName, "");
                //string rootFolder = @"D:\Msh\Msh\Net_blazor";
                //string[] txtList = Directory.GetFiles(rootFolder, "#.xlsx");
                //foreach (string file in txtList)
                //{
                //    File.Delete(file);
                //} 
                #endregion
                if (System.IO.Directory.Exists(@"D:\Msh\Msh\Net_blazor"))
                {
                    string[] files = System.IO.Directory.GetFiles(@"D:\Msh\Msh\Net_blazor", "*_List.xlsx");
                    if (files.Length > 0)
                    {
                        foreach (string s in files)
                        {
                            string fileName = System.IO.Path.GetFileName(s);
                            string deletefile = @"D:\Msh\Msh\Net_blazor\" + fileName;
                            System.IO.File.Delete(deletefile);
                        }
                    }
                }
                MyNav.NavigateTo("http://net.wedew.co.kr/Excel/GetExcelFilesView?AptCode=" + Apt_Code + "&StartDate=" + strStartDate + "&EndDate=" + strEndDate, true);
            }
        }

        private void btnSearch()
        {
            strTitle = "검색";
            int lastDay = DateTime.DaysInMonth(intYear, intMonth);
            strEndDate = strYear + "-" + strMonth + "-" + lastDay;
            strStartDate = strYear + "-" + strMonth + "-" + "01";
            SearchViews = "B";
            strUser_Name = "";
            strUsingKindCode = "";
            strUsingTicketCode = "";
        }

        private void btnCloseS()
        {
            SearchViews = "A";
        }

        /// <summary>
        /// 월 데이터 로딩 저장
        /// </summary>
        public string strMonthSaveOpen { get; set; } = "A";
        List<Community_Entity> list = new List<Community_Entity>();
        private void OnMonthSave()
        {
            strTitle = "전월 데이터 저장";
            strMonthSaveOpen = "B";
        }

        /// <summary>
        /// 전월 정보 부과하기 
        /// </summary>
        private async Task btnCloseZ()
        {
            strMonthSaveOpen = "A";
            await DisplayData();
        }

        public string strYearAllSave { get; set; } = "";
        public string strMonthAllSave { get; set; } = "";
        public string strPlaceAllSave { get; set; } = "";
        public string strStartDate1 { get; set; }
        public string strEndDate1 { get; set; }
        List<Community_Entity> cen { get; set; } = new List<Community_Entity>();
        private async Task btnMonth_Save()
        {
            if (!string.IsNullOrWhiteSpace(strMonthAllSave))
            {
                if (!string.IsNullOrWhiteSpace(strPlaceAllSave))
                {
                    int yth = Convert.ToInt32(strYearAllSave);
                    int mth = Convert.ToInt32(strMonthAllSave);
                    int lastDay = DateTime.DaysInMonth(yth, mth);
                    int mth1 = mth + 1;
                    int lastDay1 = DateTime.DaysInMonth(yth, mth1);
                    string strStartDate = strYearAllSave + "-" + strMonthAllSave + "-01";
                    string strEndDate = strYearAllSave + "-" + strMonthAllSave + "-" + lastDay;
                    strStartDate1 = yth + "-" + mth1 + "-01";
                    strEndDate1 = yth + "-" + mth1 + "-" + lastDay1;

                    int be = await community_Lib.Search_Month_Place_ListCount(Apt_Code, strStartDate, strEndDate, strPlaceAllSave);
                    int be1 = await community_Lib.Search_Month_Place_ListCount(Apt_Code, strStartDate1, strEndDate1, strPlaceAllSave);
                    if (be1 <= 2 && be >= 1)
                    {
                        list = await community_Lib.Month_Apt_List(Apt_Code, strStartDate, strEndDate, strPlaceAllSave);

                        foreach (var st in list)
                        {
                            bnn.AptName = st.AptName;
                            bnn.AptCode = st.AptCode;
                            bnn.Division = st.Division;
                            bnn.Dong = st.Dong;
                            bnn.Etc = st.Etc;
                            bnn.FilesCount = st.FilesCount;
                            bnn.Ho = st.Ho;
                            bnn.Mobile = st.Mobile;
                            bnn.Relation = st.Relation;
                            if (st.ScamDays == 30 || st.ScamDays == 31 || st.ScamDays == 29 || st.ScamDays == 28)
                            {
                                int sc = DateTime.DaysInMonth(intYear, intMonth);
                                bnn.ScamDays = sc;
                            }
                            else
                            {
                                bnn.ScamDays = st.ScamDays;
                            }

                            bnn.Approval = "B";
                            bnn.Mobile_Use = "A";
                            bnn.Ticket = st.Ticket;
                            bnn.Ticket_Code = st.Ticket_Code;
                            bnn.TotalSum = st.TotalSum;
                            bnn.UseCost = st.UseCost;
                            bnn.UserCode = st.UserCode;
                            bnn.User_Code = User_Code;
                            bnn.UserEndDate = Convert.ToDateTime(strEndDate1);
                            bnn.UserName = st.UserName;
                            bnn.UserStartDate = Convert.ToDateTime(strStartDate1);
                            bnn.UsingKindCode = st.UsingKindCode;
                            bnn.UsingKindName = st.UsingKindName;

                            await community_Lib.Add(bnn);
                        }

                        pager.RecordCount = await community_Lib.PlaceList_Code_Count(Apt_Code, strStartDate1, strEndDate1, strPlaceAllSave);
                        cen = await community_Lib.PlaceList_Code(pager.PageIndex, Apt_Code, strStartDate1, strEndDate1, strPlaceAllSave);
                    }
                    else
                    {
                        await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "이미 저장된 데이타가 있거나 전월 데이터가 없습니다..");
                    }
                }
                else
                {
                    await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "주민운동시설을 선택하지 않았습니다..");
                }
            }
            else
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "검색에서 년도와 월을 선택하지 않았습니다..");
            }
        }

        /// <summary>
        /// 이용정보 삭제
        /// </summary>
        private async Task ByRemoveZ(Community_Entity ce)
        {
            if (LevelCount >= 5)
            {
                bool isDelete = await JSRuntime.InvokeAsync<bool>("confirm", $"{ce.Aid} 번 이용정보을 정말로 삭제하시겠습니까?");

                if (isDelete)
                {
                    await community_Lib.Remove(ce.Aid);
                }
                pagerA.RecordCount = await community_Lib.GetListCountApt_NewList(Apt_Code);
                wnn = await community_Lib.GetListApt_NewList(pagerA.PageIndex, Apt_Code);
            }
        }

        /// <summary>
        /// 미승인 신청자 명단
        /// </summary>
        public string strNewList { get; set; } = "A";
        List<Community_Entity> wnn { get; set; } = new List<Community_Entity>();
        private async Task OnNoneNewList()
        {
            pagerA.RecordCount = await community_Lib.GetListCountApt_NewList(Apt_Code);
            wnn = await community_Lib.GetListApt_NewList(pagerA.PageIndex, Apt_Code);
            strTitle = "모바일 이용신청 목록";
            strNewList = "B";
        }

        private void btnCloseW()
        {
            strNewList = "A";
        }

        /// <summary>
        /// 페이징
        /// </summary>
        protected DulPager.DulPagerBase pagerA = new DulPager.DulPagerBase()
        {
            PageNumber = 1,
            PageIndex = 0,
            PageSize = 15,
            PagerButtonCount = 5
        };

        /// <summary>
        /// 페이징
        /// </summary>
        protected async void PageIndexChangedA(int pageIndex)
        {
            pagerA.PageIndex = pageIndex;
            pagerA.PageNumber = pageIndex + 1;

            await SearchViw();

            StateHasChanged();
        }

        private async Task ByApproval(int Aid)
        {
            try
            {
                await community_Lib.Approval(Aid);
                await DisplayData();
                pagerA.RecordCount = await community_Lib.GetListCountApt_NewList(Apt_Code);
                wnn = await community_Lib.GetListApt_NewList(pagerA.PageIndex, Apt_Code);

                pagerB.RecordCount = await community_Lib.GetListCountApt_Golf_NewList(Apt_Code);
                cnList = await community_Lib.GetListApt_Golf_NewList(pagerB.PageIndex, Apt_Code);
            }
            catch (Exception)
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "삭제에 실패했습니다.");
            }

        }

        public string Views { get; set; } = "A";
        private void onViews(Community_Entity ar)
        {
            vnn = ar;
            strTitle = "주민공동시설 이용 현상 상세";
            Views = "B";
        }

        /// <summary>
        /// 신청순서 수정 열기
        /// </summary>
        public string OderBy_Edit { get; set; } = "A";
        public string strTitleOrderby { get; set; }
        public int strOrderby { get; set; }
        private void btnEditOrderBy()
        {
            strTitleOrderby = "신청순서 수정";
            strOrderby = vnn.OrderBy;
            OderBy_Edit = "B";
        }

        /// <summary>
        /// 신청순서 수정 닫기
        /// </summary>
        private void btnCloseOr()
        {
            OderBy_Edit = "A";
        }

        /// <summary>
        /// 신청순서 수정 저장
        /// </summary>
        /// <returns></returns>
        private async Task btnOrderby()
        {
            await community_Lib.OrderBy_Edit(vnn.Aid, strOrderby);
            vnn = await community_Lib.Details(vnn.Aid);
            await DisplayData();
            OderBy_Edit = "A";
        }



        private void btnCloseV()
        {
            Views = "A";
        }

        private int dateD(DateTime date)
        {
            TimeSpan ts = DateTime.Now - date;
            int intdt = ts.Days;
            return intdt;
        }

        /// <summary>
        /// 스크린 골프 신정자 명단
        /// </summary>
        List<Community_Entity> cnList { get; set; } = new List<Community_Entity>();
        public string Golf_List_Views { get; set; } = "A";
        private async Task OnGolfNewList()
        {
            pagerB.RecordCount = await community_Lib.GetListCountApt_Golf_NewList(Apt_Code);
            cnList = await community_Lib.GetListApt_Golf_NewList(pagerB.PageIndex, Apt_Code);
            strTitle = "스크린 골프 신청자 명단";
            Golf_List_Views = "B";
        }

        /// <summary>
        /// 페이징
        /// </summary>
        protected DulPager.DulPagerBase pagerB = new DulPager.DulPagerBase()
        {
            PageNumber = 1,
            PageIndex = 0,
            PageSize = 15,
            PagerButtonCount = 5
        };

        /// <summary>
        /// 페이징
        /// </summary>
        protected async void PageIndexChangedB(int pageIndex)
        {
            pagerB.PageIndex = pageIndex;
            pagerB.PageNumber = pageIndex + 1;
            await SearchViw();

            StateHasChanged();
        }

        private async Task SearchViw()
        {
            if (SearchSort == "A")
            {
                pagerA.RecordCount = await community_Lib.GetListCountApt_NewList_Sa_Ho(Apt_Code, strDoongA, strHooA);
                wnn = await community_Lib.GetListApt_NewList_Sa_Ho(pagerA.PageIndex, Apt_Code, strDoongA, strHooA);
            }
            else if (SearchSort == "B")
            {
                pagerA.RecordCount = await community_Lib.GetListCountApt_NewList_Sa(Apt_Code, strPlaceA);
                wnn = await community_Lib.GetListApt_NewList_Sa(pagerA.PageIndex, Apt_Code, strPlaceA);
            }
            else if (SearchSort == "C")
            {
                pagerA.RecordCount = await community_Lib.GetListCountApt_NewList_Sb(Apt_Code, strPlaceA, strUsingTicketB);
                wnn = await community_Lib.GetListApt_NewList_Sb(pagerA.PageIndex, Apt_Code, strPlaceA, strUsingTicketB);
            }
            else
            {
                pagerA.RecordCount = await community_Lib.GetListCountApt_NewList(Apt_Code);
                wnn = await community_Lib.GetListApt_NewList(pagerA.PageIndex, Apt_Code);
            }
        }



        private void btnCloseF()
        {
            Golf_List_Views = "A";
        }

        #region 신청자 검색 관련
        public string Search_Views { get; set; } = "A";
        public string strDoongA { get; set; }
        public string strHooA { get; set; }
        public string strPlaceA { get; set; }
        public string SearchSort { get; set; }
        private void OnSearch_Views()
        {
            Search_Views = "B";
            strTitle = "신청자 검색";
            SearchSort = "";
            strDoongA = "";
            strHooA = "";
            strPlaceA = "";
            //Dong = await Erp_AptPeople_Lib.DongList(Apt_Code);
            //annUA = await community_Lib.UsingKindName(Apt_Code);
        }

        private void btnCloseSA()
        {
            Search_Views = "A";
        }

        private async Task OnDongA(ChangeEventArgs a)
        {
            strDoongA = a.Value.ToString();

            Ho = await Erp_AptPeople_Lib.Dong_HoList_Ds(Apt_Code, strDoongA);
            strHooA = "";
        }

        private async Task OnHooA(ChangeEventArgs a)
        {
            if (a.Value != null)
            {
                strHooA = a.Value.ToString();
                SearchSort = "A";
            }
            else
            {
                SearchSort = "";
            }
            await SearchViw();
        }

        List<CommunityUsingTicket_Entity> annBC { get; set; } = new List<CommunityUsingTicket_Entity>();
        private async Task OnPlaceListA(ChangeEventArgs a)
        {
            if (a.Value != null)
            {
                strPlaceA = a.Value.ToString();
                annBC = await communityUsingTicket.GetList_Apt_Kind(Apt_Code, strPlaceA);
                SearchSort = "B";
            }
            else
            {
                SearchSort = "";
            }
            await SearchViw();
        }

        public int ReCount { get; set; } = 0;
        /// <summary>
        /// 이용방법 선택 실행
        /// </summary>
        public string strUsingTicketB { get; set; }
        private async Task OnTicketB(ChangeEventArgs a)
        {
            strUsingTicketB = a.Value.ToString();
            if (!string.IsNullOrWhiteSpace(strUsingTicketB))
            {
                SearchSort = "C";
            }
            else
            {
                SearchSort = "";
            }
            await SearchViw();
        }
        #endregion

        /// <summary>
        /// 신청순서
        /// </summary>
        public string strDivision { get; set; } = "A";
        protected string OrderBy(string Code, int Count)
        {
            string Re = "";
            int Cut = 0;
            if (!string.IsNullOrWhiteSpace(Code) && Count > 0)
            {
                if (Code == "Tc20" && Count > 20) //줌바
                {
                    Cut = Count - 20;
                    Re = "대기 " + Cut;
                    strDivision = "B";
                }
                else if (Code == "Tc20" && Count <= 20) //줌바
                {
                    Re = Count.ToString();
                }
                else if (Code == "Tc28" && Count > 20)//라인댄스 화목
                {
                    Cut = Count - 20;
                    Re = "대기 " + Cut;
                    strDivision = "B";
                }
                else if (Code == "Tc28" && Count <= 20)//라인댄스 화목
                {
                    Re = Count.ToString();
                }
                else if (Code == "Tc29" && Count > 20)//다이어트 수금
                {
                    Cut = Count - 20;
                    Re = "대기 " + Cut;
                    strDivision = "B";
                }
                else if (Code == "Tc29" && Count <= 20) //다이어트 수금
                {
                    Re = Count.ToString();
                }
                else if (Code == "Tc22" && Count > 22) //메트 화목
                {
                    Cut = Count - 22;
                    Re = "대기 " + Cut;
                    strDivision = "B";
                }
                else if (Code == "Tc22" && Count <= 22)//메트 화목
                {
                    Re = Count.ToString();
                }
                else if (Code == "Tc26" && Count > 22)//메트 수금
                {
                    Cut = Count - 22;
                    Re = "대기 " + Cut;
                    strDivision = "B";
                }
                else if (Code == "Tc26" && Count <= 22)//메트 수금
                {
                    Re = Count.ToString();
                }
                else if (Code == "Tc21" && Count > 22)//요가 화목
                {
                    Cut = Count - 22;
                    Re = "대기 " + Cut;
                    strDivision = "B";
                }
                else if (Code == "Tc21" && Count <= 22)//요가 화목
                {
                    Re = Count.ToString();
                }
                else //그외
                {
                    Re = Count.ToString();
                }
            }
            else
            {
                Re = "0";
            }
            return Re;
        }

        /// <summary>
        /// 이용방법 명 변경
        /// </summary>
        protected string Method(string Code, string Name)
        {
            string strCode = "";
            if (Code == "Tc20")
            {
                strCode = "줌바(화목)오전10시";
            }
            else if (Code == "Tc22")
            {
                strCode = "필라(화목)오후9시";
            }
            else if (Code == "Tc22")
            {
                strCode = "필라(수금)오후9시";
            }
            else if (Code == "Tc26")
            {
                strCode = "필라(수금)오후9시";
            }
            else if (Code == "Tc21")
            {
                strCode = "요가(화목)오후8시";
            }
            else if (Code == "Tc28")
            {
                strCode = "라인(화목)오전11시";
            }
            else if (Code == "Tc29")
            {
                strCode = "다이어트(수금)오후8시";
            }
            else
            {
                strCode = Name;
            }

            return strCode;
        }
    }
}
