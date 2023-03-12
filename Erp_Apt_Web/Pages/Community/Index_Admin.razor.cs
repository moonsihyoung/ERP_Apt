using Erp_Apt_Lib.Community;
using Erp_Entity;
using Erp_Lib;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http;
using Microsoft.JSInterop;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.Linq;
using System.Xml.Linq;
using Erp_Apt_Lib;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections;
using System.ComponentModel;

namespace Erp_Apt_Web.Pages.Community
{
    public partial class Index_Admin
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
        //[Inject] public ISw_Files_Lib files_Lib { get; set; }
        //[Inject] public Container container { get; set; }
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
        public string strUsingTicketCode { get; private set; }
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

            pagerA.RecordCount = await community_Lib.GetListCountApt_NewList(Apt_Code);
            wnn = await community_Lib.GetListApt_NewList(pagerA.PageIndex, Apt_Code);

            //await DisplayData();

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
                //annU = await community_Lib.UsingKindName(Apt_Code); //시설명 목록
                //annUA = await community_Lib.UsingKindName(Apt_Code);
                if (LevelCount >= 10)
                {
                    await DisplayData();
                }
                else
                {
                    await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "권한이 없습니다..");
                    MyNav.NavigateTo("/");
                }
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
            }            
            else if (strSort == "D") //해당 아파트의 기간으로 모든 정보 출력
            {
                pager.RecordCount = await community_Lib.Search_Month_ListCount(Apt_Code, strStartDate, strEndDate);
                ann = await community_Lib.Search_Month_List(pager.PageIndex, Apt_Code, strStartDate, strEndDate);
            }
            else if (strSort == "F") // 해당 아파트의 기간 및 시설별 종목 검색
            {
                pager.RecordCount = await community_Lib.PlaceList_Count(Apt_Code, strStartDate, strEndDate, strPlace);
                ann = await community_Lib.PlaceList(pager.PageIndex, Apt_Code, strStartDate, strEndDate, strPlace);
            }
            else
            {
                pager.RecordCount = await community_Lib.All_List_Count();
                ann = await community_Lib.All_List(pager.PageIndex);
            }
        }


        private int dateD(DateTime date)
        {
            TimeSpan ts = DateTime.Now - date;
            int intdt = ts.Days;
            return intdt;
        }

        public string Views { get; set; } = "A";
        //Community_Entity vnn { get; set; } = new Community_Entity();
        private void onViews(Community_Entity ar)
        {
            vnn = ar;
            strTitle = "주민공동시설 이용 현상 상세";
            Views = "B";
        }

        /// <summary>
        /// 이용현황 수정 열기
        /// </summary>
        /// <param name="cn"></param>
        private async Task ByEdit(Community_Entity cn)
        {
            if (LevelCount >= 5)
            {
                strTitle = "이용자 정보 수정";

                bnn = cn;
                strDong = bnn.Dong;
                Ho = new List<Apt_People_Entity>();
                Ho = await Erp_AptPeople_Lib.HoList(Apt_Code, bnn.Dong);
                strHo = bnn.Ho;

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
        }

        /// <summary>
        /// 이용정보 삭제
        /// </summary>
        List<Community_Entity> wnn { get; set; } = new List<Community_Entity>();
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

                //pagerB.RecordCount = await community_Lib.GetListCountApt_Golf_NewList(Apt_Code);
                //cnList = await community_Lib.GetListApt_Golf_NewList(pagerB.PageIndex, Apt_Code);
            }
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
        //public string strHoo { get; set; }
        public int Ho_Sum { get; set; }
        private async Task OnHo(ChangeEventArgs a)
        {
            strHo = a.Value.ToString();
            strHoo = strHo;
            bnn.Ho = a.Value.ToString();
            Name = await Erp_AptPeople_Lib.Dong_Ho_Name_List(Apt_Code, bnn.Dong, bnn.Ho);

        }

        /// <summary>
        /// 이용자 정보
        /// </summary>
        public string strName { get; set; }
        public string strUsingKindCode { get; private set; }
        public string strPlace { get; private set; }
        public string strNowDate { get; private set; }

        private async Task OnName(ChangeEventArgs a)
        {
            ape = await Erp_AptPeople_Lib.Dedeils_Name(a.Value.ToString());
            strName = ape.Num.ToString();
            bnn.UserName = ape.InnerName;
            bnn.Mobile = ape.Hp;
        }

        private void btnClose()
        {
            InsertViews = "A";
        }

        // <summary>
        /// 이용장소 선택 시 실행
        /// </summary>
        //public string strUsingKindCode { get; set; }
        public string strSlectCode { get; set; } = "A";
        private async Task OnKind(ChangeEventArgs a)
        {
            try
            {
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
                    DateTime dt = DateTime.Now;
                    int Year = dt.Year;
                    int Month = dt.Month;
                    Month = Month + 1;
                    int ra = DateTime.DaysInMonth(Year, Month);
                    if (Month > 12)
                    {
                        Month = 1;
                        Year = Year + 1;
                    }

                    DateTime dt1 = Convert.ToDateTime(Year + "-" + Month + "-01");
                    DateTime dt2 = Convert.ToDateTime(Year + "-" + Month + "-" + ra);
                    strSlectCode = "A";
                    bnn.UserStartDate = dt1;
                    bnn.UserEndDate = dt2;
                    bnn.ScamDays = ra;
                }

                annBB = await communityUsingTicket.GetList_Apt_Kind(Apt_Code, strUsingKindCode);
                strUsingTicketCode = "";
                //bnn.UseCost = 0;
            }
            catch (Exception)
            {
                strUsingTicketCode = "";
                //bnn.UseCost = 0;
                //await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "이용장소를 선택하세요!");
            }
        }

        /// <summary>
        /// 이용방법 선택 시 실행
        /// </summary>
        //public string strUsingTicketCode { get; set; }
        public int intYear { get; private set; } = DateTime.Now.Year;        public int intMonth { get; private set; } = DateTime.Now.Month;
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

                    if (strUsingKindCode == "Kd4")
                    {
                        bnn.UserStartDate = DateTime.Now;
                        bnn.UserEndDate = DateTime.Now;
                        //bnn.UseCost;
                    }
                    else
                    {
                        intMonth = DateTime.Now.Month;
                        intYear = DateTime.Now.Year;
                        intMonth = intMonth + 1;

                        string dt1 = intYear + "-" + intMonth + "01";
                        string dt2 = intYear + "-" + intMonth + "31";

                        if (string.IsNullOrWhiteSpace(bnn.UsingKindCode))
                        {
                            int intSame = await community_Lib.DongHoSameCount(Apt_Code, bnn.Dong, bnn.Ho, bnn.UsingKindCode, dt1, dt2);

                            if (bnn.UsingKindCode == "Kd0" && bnn.UsingKindCode == "Kd1")
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

                        if (intMonth > 12)
                        {
                            intMonth = 1;
                            intYear = intYear + 1;
                        }
                        int ra = DateTime.DaysInMonth(intYear, intMonth);

                        bnn.UserEndDate = Convert.ToDateTime(intYear + "-" + intMonth + "-" + ra);
                        bnn.UserStartDate = Convert.ToDateTime(intYear + "-" + intMonth + "-01");
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

        private void btnCloseV()
        {
            Views = "A";
        }

        /// <summary>
        /// 이용현황 입력
        /// </summary>
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
                            bnn.ScamDays = 0;
                            strName = "";
                            strHo = "";
                            await DisplayData();
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
    }
}
