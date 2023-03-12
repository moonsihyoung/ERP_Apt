using Erp_Apt_Lib.Community;
using Erp_Apt_Lib.Logs;
using Erp_Entity;
using Erp_Lib;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Mobile.Pages.Communitys
{
    public partial class Index
    {
        [CascadingParameter] public Task<AuthenticationState> AuthenticationStateRef { get; set; }
        [Inject] IJSRuntime JSRuntime { get; set; }
        [Inject] IApt_Lib apt_Lib { get; set; }
        [Inject] public NavigationManager MyNav { get; set; } // Url 
        [Inject] public IIn_AptPeople_Lib in_AptPeople { get; set; }
        [Inject] public ISido_Lib sido_Lib { get; set; }
        [Inject] public ICommunity_Lib community_Lib { get; set; }
        [Inject] public IErp_AptPeople_Lib erp_AptPeople_Lib { get; set; }
        [Inject] public ICommunityUsingKind_Lib communityUsingKind { get; set; }
        [Inject] public ICommunityUsingTicket_Lib communityUsingTicket { get; set; }
        [Inject] IHttpContextAccessor contextAccessor { get; set; }

        #region 목록
        List<Community_Entity> ann { get; set; } = new List<Community_Entity>();
        List<Community_Entity> annP { get; set; } = new List<Community_Entity>();
        In_AptPeople_Entity bnnA { get; set; } = new In_AptPeople_Entity();
        Community_Entity bnnB { get; set; } = new Community_Entity();

        Community_Entity bnn { get; set; } = new Community_Entity();

        Apt_People_Entity ape = new Apt_People_Entity();
        List<Apt_Entity> apt { get; set; } = new List<Apt_Entity>();
        List<Sido_Entity> sido { get; set; } = new List<Sido_Entity>();
        List<CommunityUsingKind_Entity> annA { get; set; } = new List<CommunityUsingKind_Entity>();
        List<CommunityUsingTicket_Entity> annB { get; set; } = new List<CommunityUsingTicket_Entity>();
        List<Community_Entity> annU { get; set; } = new List<Community_Entity>();

        List<Apt_People_Entity> DongList = new List<Apt_People_Entity>();
        List<Apt_People_Entity> HoList = new List<Apt_People_Entity>();
        List<Apt_People_Entity> NameList = new List<Apt_People_Entity>();
        #endregion

        #region 변수
        public string User_Code { get; set; }
        public string User_Name { get; set; }
        public string Apt_Code { get; set; }
        public string Apt_Name { get; set; }
        public string Dong { get; set; }
        public string Ho { get; set; }
        public string Mobile { get; set; }
        public int LevelCount { get; set; }
        public string InsertViews { get; set; } = "A";
        public string ViewsA { get; set; } = "A";
        public string ViewsB { get; set; } = "A";
        public string InsertViewsA { get; set; } = "A";
        public string InsertViewsB { get; set; } = "A";
        public string InsertViewsC { get; set; } = "A";
        public string SearchViews { get; set; } = "A";
        public string strTitle { get; set; }
        public int Ho_Sum { get; set; }
        public string MyIPAdress { get; set; }
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
                Dong = authState.User.Claims.FirstOrDefault(c => c.Type == "Dong")?.Value;
                Ho = authState.User.Claims.FirstOrDefault(c => c.Type == "Ho")?.Value;
                Mobile = authState.User.Claims.FirstOrDefault(c => c.Type == "Mobile")?.Value;
                LevelCount = Convert.ToInt32(authState.User.Claims.FirstOrDefault(c => c.Type == "LevelCount")?.Value);
                DongList = await erp_AptPeople_Lib.DongList(Apt_Code); //동이름 목록
                annU = await community_Lib.UsingKindName(Apt_Code); //시설명 목록

                await DisplayData();
            }
            else
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "로그인되지 않았습니다..");
                MyNav.NavigateTo("/");
            }
        }

        protected override void OnAfterRender(bool firstRender)
        {
            if (firstRender)
            {
                MyIPAdress = contextAccessor.HttpContext.Connection?.RemoteIpAddress.ToString();

                StateHasChanged();
            }
        }

        /// <summary>
        /// 데이터 뷰
        /// </summary>
        //public string strDong { get; set; } = "102";
        //public string strHo { get; set; } = "1603";
        private async Task DisplayData()
        {
            //pager.RecordCount = await community_Lib.Search_ListCount(Apt_Code, strStartDate, strEndDate, strDoong, strHoo);
            ann = await community_Lib.GetListDongHo(Apt_Code, Dong, Ho);
            annP = await community_Lib.GetListDongHo_Personal(Apt_Code, Dong, Ho, User_Name);
            annA = await communityUsingKind.GetList_Apt(Apt_Code);
            //annB = await communityUsingTicket.GetList_Apt(Apt_Code);
            //Ho_Sum = await community_Lib.Search_List_All_Sum(Apt_Code, strStartDate, strEndDate, strDoong, strHoo);            
        }

        /// <summary>
        /// 이용신청 열기
        /// </summary>
        public int lastDay { get; set; }
        public string strMonth { get; set; }
        private void OnInsert()
        {
            bnn = new Community_Entity();
            DateTime dt = DateTime.Now;
            int dtYear = dt.Year;
            int dtMonth = dt.Month + 1;

            if (dtMonth == 13)
            {
                dtMonth = 1;
                dtYear = dt.Year + 1;
            }

            DateTime dt1 = Convert.ToDateTime(dtYear + "-" + dtMonth + "-01");
            lastDay = DateTime.DaysInMonth(dtYear, dtMonth);
            DateTime dt2 = Convert.ToDateTime(dtYear + "-" + dtMonth + "-" + lastDay + " 23:59:59.993");

            bnn.UserEndDate = dt2;
            bnn.UserStartDate = dt1;
            strMonth = "";
            strUsingKindCode = "";
            strUsingTicketCode = "";
            strTitle = "주민공동시설 이용신청";
            InsertViews = "B";
        }

        public int dtMonth { get; set; } = 0;
        private async Task OnMonth(int e)
        {
            strMonth = e.ToString();
            dtMonth = e; //Convert.ToInt32(strMonth); //신청한 달
            DateTime dt = DateTime.Now;
            int intMont = dt.Month; // 현재 달
            int dtYear = dt.Year;
            if (intMont == 12 && dtMonth == 1)
            {   //dtMonth = 1;
                dtYear = dt.Year + 1;
            }

            int intRe = dtMonth - intMont;

            if (intMont == dtMonth || intRe == 1 || intRe == 11)
            {
                bnn = new Community_Entity();
                //dt.Month + 1;               

                DateTime dt1 = Convert.ToDateTime(dtYear + "-" + dtMonth + "-01");
                lastDay = DateTime.DaysInMonth(dtYear, dtMonth);
                DateTime dt2 = Convert.ToDateTime(dtYear + "-" + dtMonth + "-" + lastDay + " 23:59:59.993");

                bnn.UserEndDate = dt2;
                bnn.UserStartDate = dt1;

                strMon = "B";
            }
            else
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "현재 달과 다음 달만 신청이 가능합니다.. \n" + strMonth + "월 신청은 가능하지 않습니다.");
            }
        }

        private void btnClose()
        {
            InsertViews = "A";

        }

        [Parameter] public bool isChecked { get; set; } = false;
        private void OnCheck()
        {
            if (isChecked)
            {
                InsertViews = "A";
                InsertViewsA = "B";
                isChecked = !isChecked;
                strTitle = "주민공동시설 등록 신청서 작성";
            }
            else
            {
                InsertViews = "A";
                InsertViewsA = "B";
                isChecked = !isChecked;
                strTitle = "주민공동시설 등록 신청서 작성";
            }
        }

        /// <summary>
        /// 이용신청 저장
        /// </summary>
        private async Task btnSave()
        {
            #region 아이피 입력

            bnn.PostIP = MyIPAdress;
            #endregion
            bnn.AptCode = Apt_Code;
            bnn.AptName = Apt_Name;
            bnn.UserName = Apt_Name;
            bnn.Dong = Dong;
            bnn.Ho = Ho;
            bnn.Mobile = Mobile;
            bnn.Relation = "본인";
            bnn.Approval = "A";
            bnn.Mobile_Use = "B";
            bnn.UserName = User_Name;
            try
            {
                bnn.UserCode = await community_Lib.ByUserCode(bnn.AptCode, bnn.Dong, bnn.Ho, bnn.UserName);
            }
            catch (Exception)
            {
                bnn.UserCode = "0";
            }
            if (string.IsNullOrWhiteSpace(bnn.UserCode))
            {
                bnn.UserCode = "0";
            }
            //try
            //{
            //    //bnn.UserCode = await community_Lib.ByUserCode(Apt_Code, Dong, Ho, bnn.UserName);
            //    if (string.IsNullOrWhiteSpace(bnn.UserCode))
            //    {
            //        await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "지문 등록이 되지 않았습니다. \n 관리사무소를 방문하여 지문등록을 하여 이용신청이 가능합니다.");
            //    }
            //}
            //catch (Exception)
            //{
            //    await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "지문 등록이 되지 않았습니다. \n 관리사무소를 방문하여 지문등록을 하여 이용신청이 가능합니다.");
            //}
            bnn.UserName = User_Name;
            bnn.User_Code = User_Code;
            intScamHour = bnn.UserEndHour - bnn.UserStartHour;

            int noo = dtMonth - DateTime.Now.Month;

            if (string.IsNullOrWhiteSpace(bnn.AptCode))
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "로그인하지 않았습니다.");
            }
            else if (string.IsNullOrWhiteSpace(bnn.Dong))
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "로그인하지 않았습니다.");
            }
            else if (string.IsNullOrWhiteSpace(bnn.Ho))
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "호를 선택하지 않았습니다.");
            }
            else if (bnn.ScamDays < 1)
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "시작일이나 종료일을 선택하지 않았습니다.");
            }
            else if (bnn.UseCost < 1)
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "이용료가 입력되지 않았습니다.");
            }
            else if (string.IsNullOrWhiteSpace(bnn.Ticket))
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "이용방법을 선택하지 않았습니다.");
            }
            else if (string.IsNullOrWhiteSpace(bnn.UsingKindName))
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "이용 시설을 선택하지 않았습니다.");
            }
            else if (string.IsNullOrWhiteSpace(bnn.UserName))
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "로그인하지 않았습니다.");
            }
            else if (string.IsNullOrWhiteSpace(bnn.UserCode))
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "로그인하지 않았습니다.");
            }
            //else if (bnn.UsingKindCode != "Kd4" && DateTime.Now.Day > 25)
            //{
            //    await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "모바일로 이용 신청은 25일까지만 가능합니다.");
            //}
            else if (bnn.UsingKindCode != "Kd4" && noo >= 2)
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "모바일로 이용 신청은 다음달 이용신청만 가능합니다.");
            }
            //else if (bnn.UsingKindCode != "Kd4" && bnn.UserEndDate.Month == DateTime.Now.Month)
            //{
            //    await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "모바일로 이용 신청은 다음달 이용신청만 가능합니다.");
            //}
            else if (bnn.UsingKindCode == "Kd4" && bnn.ScamDays > 1)
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "스크린골프장은 1일 이상을 선택해서는 안됩니다.");
            }
            else if (bnn.UsingKindCode == "Kd4" && bnn.UserStartHour <= 5)
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "스크린골프장 이용 시작시간 5시 이전일 수 없습니다.");
            }
            else if (bnn.UsingKindCode == "Kd4" && bnn.UserEndHour <= 6)
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "스크린골프장 이용 종료시간 6시 이전일 수 없습니다.");
            }
            else if (bnn.UsingKindCode == "Kd4" && intScamHour < 1)
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "스크린골프장 이용시간이 1시 이하일 수 없습니다.");
            }
            else if (bnn.UsingKindCode == "Kd4" && intScamHour < 1)
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "스크린골프장 이용시간이 1시 이상일 수 없습니다.");
            }
            else
            {
                //DateTime dpt = Convert.ToDateTime("2023-01-05 0:0:0.0");
                //bnn.OrderBy = await community_Lib.OrderBy(bnn.AptCode, bnn.UsingKindCode, bnn.Ticket_Code, bnn.UserStartDate, bnn.UserEndDate);
                if (bnn.Aid < 1)
                {
                    bnn.OrderBy = await community_Lib.OrderBy(bnn.AptCode, bnn.UsingKindCode, bnn.Ticket_Code, bnn.UserStartDate, bnn.UserEndDate) + 1;

                    int being = await community_Lib.BeingCount(bnn.AptCode, bnn.UsingKindCode, bnn.Ticket_Code, bnn.Dong, bnn.Ho, bnn.UserName, bnn.Mobile, bnn.UserStartDate, bnn.UserEndDate);

                    if (bnn.UsingKindCode != "Kd4" && being < 1)
                    {
                        int intMM = dtMonth - DateTime.Now.Month;
                        if ((intMM == 1) && (DateTime.Now.Day >= 5))
                        {
                            if (bnn.UsingKindCode != "Kd12")
                            {
                                bnn.Division = "A";
                                await community_Lib.Add(bnn);
                                await Logs();

                                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", bnn.UsingKindName + " 이용 신청이 완료되었습니다. \n 신청된 이용신청서는 앱으로 취소할 수 없으며, \n 다만 질병 등으로 불가피하게 이용이 불가능한 경우, \n 문서로 징빙된 경우에 한하여 취소할 수 있습니다.(연락처 : 02-2611-0967).");
                                await DisplayData();
                                bnn = new Community_Entity();
                                InsertViewsA = "A";
                                InsertViewsC = "A";
                                strMon = "A";
                            }
                            else//GX롬에 대해서는 25일까지만 등록 가능함
                            {
                                //int intdays = DateTime.Now.Day;
                                //if (intdays < 26)
                                //{
                                bnn.Division = "A";
                                await community_Lib.Add(bnn);
                                await Logs();

                                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", bnn.UsingKindName + " 이용 신청이 완료되었습니다. \n 신청된 이용신청서는 앱으로 취소할 수 없으며, \n 다만 질병 등으로 불가피하게 이용이 불가능한 경우, \n 문서로 징빙된 경우에 한하여 취소할 수 있습니다.(연락처 : 02-2611-0967).");
                                await DisplayData();
                                bnn = new Community_Entity();
                                InsertViewsA = "A";
                                InsertViewsC = "A";
                                strMon = "A";
                                //}
                                //else
                                //{
                                //    await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", bnn.UsingKindName + "은 25일까지만 이용 신청이 가능합니다. \n 자세한 사항은 관리사무소에 문의하여 주십시오.(연락처 : 02-2611-0967).");
                                //}
                            }
                        }
                        else if (intMM == 0)
                        {
                            if (bnn.UsingKindCode != "Kd12")
                            {
                                bnn.Division = "A";
                                await community_Lib.Add(bnn);
                                await Logs();

                                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", bnn.UsingKindName + " 이용 신청이 완료되었습니다. \n 신청된 이용신청서는 앱으로 취소할 수 없으며, \n 다만 질병 등으로 불가피하게 이용이 불가능한 경우, \n 문서로 징빙된 경우에 한하여 취소할 수 있습니다.(연락처 : 02-2611-0967).");
                                await DisplayData();
                                bnn = new Community_Entity();
                                InsertViewsA = "A";
                                InsertViewsC = "A";
                                strMon = "A";
                            }
                            else//GX롬에 대해서는 25일까지만 등록 가능함
                            {
                                bnn.Division = "A";
                                await community_Lib.Add(bnn);
                                await Logs();

                                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", bnn.UsingKindName + " 이용 신청이 완료되었습니다. \n 신청된 이용신청서는 앱으로 취소할 수 없으며, \n 다만 질병 등으로 불가피하게 이용이 불가능한 경우, \n 문서로 징빙된 경우에 한하여 취소할 수 있습니다.(연락처 : 02-2611-0967).");
                                await DisplayData();
                                bnn = new Community_Entity();
                                InsertViewsA = "A";
                                InsertViewsC = "A";
                                strMon = "A";
                            }
                        }
                        else
                        {
                            await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "다음달 " + bnn.UsingKindName + " 신청은 매월 05일 00시 00분부터 신청이 가능합합니다. \n 선착순으로 신청받고 있으니 착오 없으시기 바랍니다. \n 불편을 드려 죄송합니다. (문의처 : 02-2611-0967).");
                        }
                    }
                    else if (bnn.UsingKindCode == "Kd4")
                    {
                        int NowDays = DateTime.Now.Day;
                        if ((bnn.UserStartDate.DayOfWeek == DayOfWeek.Monday) && (NowDays <= 7))
                        {
                            await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", bnn.UserStartDate.Day + "일은 첫번째 월요일이므로 휴무입니다. \n 이용 신청을 할 수 없습니다. \n 다시 확인해보세요..");
                        }
                        else
                        {
                            string date = DateTime.Now.ToShortDateString();
                            bnn.UserStartDate = Convert.ToDateTime(bnn.UserStartDate.ToShortDateString());
                            DateTime dta = Convert.ToDateTime(date); //현재의 가장 이른 시간;
                            string dateA = bnn.UserStartDate.ToShortDateString();
                            DateTime dtb = Convert.ToDateTime(dateA);
                            int be = await community_Lib.HourBeingCount(Apt_Code, bnn.UsingKindCode, dtb, dtb, bnn.UserStartHour);
                            if (bnn.UserStartDate == dta)
                            {
                                if (bnn.UserStartHour > DateTime.Now.Hour)
                                {
                                    if (be < 1)
                                    {
                                        bnn.Division = "A";
                                        await community_Lib.Add(bnn);

                                        await Logs();

                                        await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", bnn.UsingKindName + " 이용 신청이 완료되었습니다. \n 신청된 이용신청서는 앱으로 취소할 수 없으며, \n 당일 취소는 가능하지 않습니다. \n 다만 질병 등으로 불가피하게 이용이 불가능한 경우, \n 문서로 징빙된 경우에 한하여 취소할 수 있습니다.(연락처 : 02-2611-0967).");


                                        await DisplayData();

                                        bnn = new Community_Entity();
                                        InsertViewsA = "A";
                                        InsertViewsC = "A";
                                        strMon = "A";
                                    }
                                    else
                                    {
                                        await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", bnn.UserStartHour + "시는 이미 예약되어 있습니다. \n 다시 확인해보세요..");
                                    }
                                }
                                else
                                {
                                    await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", bnn.UserStartHour + "시는 지나간 시간이므로 신청할 수 없습니다. \n 다시 확인해보세요..");
                                }
                            }
                            else if (bnn.UserStartDate > dta)
                            {
                                if (be < 1)
                                {
                                    bnn.Division = "A";
                                    await community_Lib.Add(bnn);

                                    await Logs();

                                    await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", bnn.UsingKindName + " 이용 신청이 완료되었습니다. \n 신청된 이용신청서는 앱으로 취소할 수 없으며, \n 당일 취소는 가능하지 않습니다. \n 다만 질병 등으로 불가피하게 이용이 불가능한 경우, \n 문서로 징빙된 경우에 한하여 취소할 수 있습니다.(연락처 : 02-2611-0967).");

                                    await DisplayData();

                                    bnn = new Community_Entity();
                                    InsertViewsA = "A";
                                    InsertViewsC = "A";
                                    strMon = "A";
                                }
                                else
                                {
                                    await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", bnn.UserStartHour + "시는 이미 예약되어 있습니다. \n 다시 확인해보세요..");
                                }
                            }
                            else
                            {
                                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", bnn.UserStartHour + "시는 지나간 시간이므로 신청할 수 없습니다. \n 다시 확인해보세요..");
                            }

                        }
                    }
                    else
                    {
                        await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", bnn.UsingKindName + "은 이미 신청하였습니다. \n 다시 확인해보세요..");
                    }
                }
                else
                {
                    await community_Lib.Edit(bnn);

                    await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", bnn.UsingKindName + " 이용 신청이 완료되었습니다. \n 신청된 이용신청서는 앱으로 취소할 수 없으며, \n 취소는 질병 등으로 이용이 불가능한 경우 문서로 징빙된 경우에 한하여 가능하며, \n 자세한 설명은 관리사무소로 문의하세요(연락처 : 02-2611-0967).");
                    await DisplayData();
                    bnn = new Community_Entity();
                    InsertViewsA = "A";
                    InsertViewsC = "A";
                    strMon = "A";
                }
            }
        }

        /// <summary>
        /// 로그인
        /// </summary>
        [Inject] ILogs_Lib logs_Lib { get; set; }
        private async Task Logs()
        {
            Logs_Entites dnn = new Logs_Entites();
            dnn.Apt_Code = Apt_Code;
            dnn.Note = User_Name;
            dnn.Application = "주민공동시설 저장";
            dnn.LogEvent = "클릭";
            dnn.Callsite = "";
            dnn.Exception = "";
            dnn.ipAddress = contextAccessor.HttpContext.Connection?.RemoteIpAddress.ToString();
            dnn.Level = "3";
            dnn.Logger = User_Code;
            dnn.Message = bnn.UsingKindName + " " + bnn.Ticket;
            dnn.MessageTemplate = "";
            dnn.Properties = "";
            dnn.TimeStamp = DateTime.Now.ToShortDateString();
            await logs_Lib.add(dnn);
        }

        private void btnCloseA()
        {
            InsertViewsA = "A";
            bnn = new Community_Entity();
            annB = new List<CommunityUsingTicket_Entity>();
            strMon = "A";
        }

        /// <summary>
        /// 이용장소 선택 시 실행
        /// </summary>
        public string strUsingKindCode { get; set; }
        private async Task OnKind(ChangeEventArgs a)
        {
            //try
            //{
            if (!string.IsNullOrWhiteSpace(a.Value.ToString()))
            {
                strUsingKindCode = a.Value.ToString();
                bnn.UsingKindCode = strUsingKindCode;
                //bnnD.Kind_Code = strUsingKindCode;
                bnn.UsingKindName = await communityUsingKind.KindName(bnn.UsingKindCode);
                //bnnD.Kind_Name = bnn.UsingKindName;
                annB = await communityUsingTicket.GetList_Apt_Kind(Apt_Code, strUsingKindCode);
                strUsingTicketCode = "";
                bnn.UseCost = 0;
                strTime = "A";
            }
            else
            {
                annB = new List<CommunityUsingTicket_Entity>();
                bnn.UseCost = 0;
                strTime = "A";
            }
            //}
            //catch (Exception)
            //{
            //    annB = new List<CommunityUsingTicket_Entity>();
            //    strUsingTicketCode = "";
            //    strUsingKindCode = "";
            //    bnn.UseCost = 0;
            //    //await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "이용장소를 선택하세요!");
            //}
        }

        /// <summary>
        /// 이용방법 선택 시 실행
        /// </summary>
        public string strUsingTicketCode { get; set; }
        CommunityUsingKind_Entity bnnC { get; set; } = new CommunityUsingKind_Entity();
        CommunityUsingTicket_Entity bnnD { get; set; } = new CommunityUsingTicket_Entity();
        public string strTime { get; set; } = "A";
        Community_Entity tnn { get; set; } = new Community_Entity();
        public string intCountA { get; set; } = "0";
        public string intCountB { get; set; } = "0";
        private async Task OnTicket(ChangeEventArgs a)
        {
            try
            {
                string date1 = "";
                string date2 = "";
                if (a.Value.ToString() == "24")
                {
                    await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "헬스트레이닝은 선택할 수 없습니다.\n 관리사무소에 문의하세요.");

                    bnn.UseCost = 0;
                    //annB = await communityUsingTicket.GetList_Apt_Kind(Apt_Code, strUsingKindCode);
                    annB = new List<CommunityUsingTicket_Entity>();
                    bnn.ScamDays = 0;
                    strUsingTicketCode = "";
                    strUsingKindCode = "";
                    bnn.Ticket = "";
                    bnn.Ticket_Code = "";
                    bnn.UsingKindName = "";
                    bnn.UsingKindCode = "";
                }
                else if (a.Value.ToString() == "20")
                {
                    await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "헬스트레이닝은 선택할 수 없습니다.\n 관리사무소에 문의하세요.");

                    bnn.UseCost = 0;
                    //annB = await communityUsingTicket.GetList_Apt_Kind(Apt_Code, strUsingKindCode);
                    annB = new List<CommunityUsingTicket_Entity>();
                    bnn.ScamDays = 0;
                    strUsingKindCode = "";
                    strUsingTicketCode = "";
                    bnn.Ticket = "";
                    bnn.Ticket_Code = "";
                    bnn.UsingKindName = "";
                    bnn.UsingKindCode = "";
                }
                else
                {
                    bnnD = await communityUsingTicket.Details(a.Value.ToString());
                    strUsingTicketCode = a.Value.ToString();
                    bnn.Ticket = bnnD.Ticket_Name;
                    bnn.Ticket_Code = bnnD.Ticket_Code;
                    int Year = DateTime.Now.Year;
                    int Month = DateTime.Now.Month;
                    int ra = DateTime.DaysInMonth(Year, Month);

                    date1 = Year.ToString();
                    date2 = date1;
                    date1 = date1 + "-" + Month.ToString();
                    date2 = date1;


                    int ra1 = 0;
                    //Month = Month + 1;

                    if (Month == 12 && dtMonth == 1)
                    {
                        Year = Year + 1;
                        Month = dtMonth;
                    }
                    else
                    {
                        Month = dtMonth;
                    }

                    ra1 = DateTime.DaysInMonth(Year, Month);
                    bnn.ScamDays = ra1;
                    string dt1 = Year + "-" + Month + "-01";
                    string dt2 = Year + "-" + Month + "-" + ra + " 23:59:59.993";

                    if (bnn.UsingKindName == "스크린골프장")
                    {
                        bnn.UserStartDate = DateTime.Now;
                        bnn.UserEndDate = Convert.ToDateTime(bnn.UserStartDate.Year + "-" + bnn.UserStartDate.Month + "-" + bnn.UserStartDate.Day + " 23:59:59.993");


                        date1 = bnn.UserStartDate.Year.ToString();
                        date1 = date1 + "-" + bnn.UserStartDate.Month.ToString();
                        date1 = date1 + "-01" + " 00:00:00.000";
                        date2 = bnn.UserEndDate.Year.ToString() + "-" + bnn.UserEndDate.Month.ToString() + "-" + ra + " 23:59:59.993";
                        DateTime dat1 = Convert.ToDateTime(date1);
                        DateTime dat2 = Convert.ToDateTime(date2);
                        //int intRe = await community_Lib.DongHoSameCount(Apt_Code, Dong, Ho, "Kd1", date1, date2);
                        int intRe = await community_Lib.BeingCount_DongHo(Apt_Code, "Kd1", "Tc5", Dong, Ho, Mobile, dat1, dat2);
                        if (intRe > 0)
                        {
                            bnn.ScamDays = 1;
                            InsertViewsA = "A";
                            InsertViewsC = "B";
                            //strTime = "B";
                            bnn.Etc = "스크린 골프 사용일은 1일만 선택하여야 합니다. \n 2일 이상은 선택할 수 없습니다.";
                        }
                        else
                        {
                            bnn.UsingKindCode = "";
                            strUsingKindCode = "";
                            bnn.UsingKindName = "";
                            await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "골프장을 이용하지 않는 분은 스크린 골프를 사용할 수 없습니다..\n 관리사무소에 문의하세요.");
                        }
                    }
                    else
                    {
                        strTime = "A";
                        bnn.UserStartDate = Convert.ToDateTime(Year + "-" + Month + "-01");
                        bnn.UserEndDate = Convert.ToDateTime(Year + "-" + Month + "-" + ra1);
                        bnn.Etc = "이용 신청과 관련하여 관리사무소에 전달할 메모를 기재해 주세요.";
                    }

                    int reMonth = Month;

                    if (Month == 12 && dtMonth == 1)
                    {
                        Year = Year + 1;
                        reMonth = dtMonth;
                    }
                    else
                    {
                        reMonth = dtMonth;
                    }
                    string dtA1 = "";
                    string dtA2 = "";
                    dtA1 = Year + "-";
                    dtA1 = dtA1 + reMonth + "-01";
                    dtA2 = Year + "-";
                    dtA2 = dtA2 + reMonth + "-";
                    dtA2 = dtA2 + ra1 + " 23:59:59.993";

                    #region 신청자 수 관련
                    DateTime dtaz = Convert.ToDateTime(dtA1);
                    DateTime dtbz = Convert.ToDateTime(dtA2);

                    int intCount = await community_Lib.OrderBy(Apt_Code, bnn.UsingKindCode, bnn.Ticket_Code, dtaz, dtbz);

                    intCountA = intCount.ToString();

                    if (bnn.Ticket_Code == "Tc20" && intCount > 20)
                    {
                        intCountB = (intCount - 20).ToString();
                    }
                    else if (bnn.Ticket_Code == "Tc20" && intCount <= 20)
                    {
                        intCountB = "0";
                    }
                    else if (bnn.Ticket_Code == "Tc28" && intCount > 20)
                    {
                        intCountB = (intCount - 20).ToString();
                    }
                    else if (bnn.Ticket_Code == "Tc28" && intCount <= 20)
                    {
                        intCountB = "0";
                    }
                    else if (bnn.Ticket_Code == "Tc29" && intCount > 20)
                    {
                        intCountB = (intCount - 20).ToString();
                    }
                    else if (bnn.Ticket_Code == "Tc29" && intCount <= 20)
                    {
                        intCountB = "0";
                    }
                    else if (bnn.Ticket_Code == "Tc22" && intCount > 22)
                    {
                        intCountB = (intCount - 22).ToString();
                    }
                    else if (bnn.Ticket_Code == "Tc22" && intCount <= 22)
                    {
                        intCountB = "0";
                    }
                    else if (bnn.Ticket_Code == "Tc26" && intCount > 22)
                    {
                        intCountB = (intCount - 22).ToString();
                    }
                    else if (bnn.Ticket_Code == "Tc26" && intCount <= 22)
                    {
                        intCountB = "0";
                    }
                    else if (bnn.Ticket_Code == "Tc21" && intCount > 22)
                    {
                        intCountB = (intCount - 22).ToString();
                    }
                    else if (bnn.Ticket_Code == "Tc21" && intCount <= 22)
                    {
                        intCountB = "0";
                    }
                    else
                    {
                        intCountB = "0";
                    }
                    #endregion

                    if (!string.IsNullOrWhiteSpace(bnn.UsingKindCode))
                    {
                        int intSame = await community_Lib.DongHoSameCount(Apt_Code, Dong, Ho, bnn.UsingKindCode, dtA1, dtA2);

                        if (bnn.UsingKindCode == "Kd0" || bnn.UsingKindCode == "Kd1")
                        {
                            if (intSame > 0)
                            {
                                double db = (bnnD.Ticket_Cost * 0.8);
                                bnn.UseCost = Convert.ToInt32(db);
                            }
                            else
                            {
                                bnn.UseCost = bnnD.Ticket_Cost;
                            }
                        }
                        else
                        {
                            bnn.UseCost = bnnD.Ticket_Cost;
                        }
                    }
                    else
                    {
                        bnn.UseCost = 0;
                    }
                }
            }
            catch (Exception)
            {
                strUsingTicketCode = "";
                bnn.UseCost = 0;
            }
        }

        /// <summary>
        /// 날짜계산 사용일수
        /// </summary>
        private async Task OnScamDays(ChangeEventArgs a)
        {
            bnn.UserEndDate = Convert.ToDateTime(a.Value);
            //int ra = DateTime.DaysInMonth(bnn.UserEndDate.Year, bnn.UserEndDate.Month);
            bnn.ScamDays = (bnn.UserEndDate - bnn.UserStartDate).Days + 1;

            DateTime dt = DateTime.Now;
            int intMonth = dt.Month + 1;
            int intYear = dt.Year;

            if (intMonth == 12)
            {
                intYear = intYear + 1;
                intMonth = 1;

                int lastDay = DateTime.DaysInMonth(dt.Year, dt.Month);

                DateTime dt1 = Convert.ToDateTime(intYear + "-" + intMonth + "- 01");
                DateTime dt2 = Convert.ToDateTime(intYear + "-" + intMonth + "-" + lastDay + " 23:59:59.993");

                if (bnn.UserStartDate < dt1)
                {
                    if (bnn.UsingKindCode != "Kd4" && bnn.UserStartDate.Month == DateTime.Now.Month)
                    {
                        bnn.UserStartDate = dt1;
                        bnn.UserEndDate = dt2;
                        bnn.UseCost = 0;
                        await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "현재 월 이용신청을 가능하지 않습니다.. \n 다음 달 이용신청만 가능합니다.");
                    }
                }
                else if (bnn.UserEndDate < dt2)
                {
                    if (bnn.UsingKindCode != "Kd4" && bnn.UserEndDate.Month == DateTime.Now.Month)
                    {
                        bnn.UserStartDate = dt1;
                        bnn.UserEndDate = dt2;
                        bnn.UseCost = 0;
                        await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "현재 월 이용신청을 가능하지 않습니다.. \n 다음 달 이용신청만 가능합니다.");
                    }
                }
                else
                {
                    if (bnn.ScamDays >= 1)
                    {
                        bnn.UseCost = bnnD.Ticket_Cost;
                    }
                    else
                    {
                        await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "사용일수가 1보다 작을 수는 없습니다..");
                    }
                }
            }
            else if (intMonth >= 13)
            {
                intYear = intYear + 1;
                intMonth = 1;

                int lastDay = DateTime.DaysInMonth(dt.Year, dt.Month);

                DateTime dt1 = Convert.ToDateTime(intYear + "-" + intMonth + "- 01");
                DateTime dt2 = Convert.ToDateTime(intYear + "-" + intMonth + "-" + lastDay + " 23:59:59.993");
                bnn.UserStartDate = dt1;
                bnn.UserEndDate = dt2;

                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "12월 달에 2월분 이용신청을 할 수는 없습니다..");
            }
        }

        /// <summary>
        /// 이용시작 시간
        /// </summary>
        public string strUseTimeA { get; set; }
        public string strUseTimeB { get; set; }
        public string strMon { get; set; } = "A";
        private void OnUseTimeA(ChangeEventArgs a)
        {
            strUseTimeA = a.Value.ToString();
            bnn.UserStartHour = Convert.ToInt32(a.Value);
            bnn.UserEndHour = bnn.UserStartHour + 1;
            strUseTimeB = bnn.UserEndHour.ToString();
            bnn.Etc = "이용 시작시간 " + strUseTimeA + "시부터 ~";
        }



        /// <summary>
        /// 이용종료 시간
        /// </summary>
        public int intScamHour { get; set; } = 0;
        private async Task OnUseTimeB(ChangeEventArgs a)
        {
            if (!string.IsNullOrWhiteSpace(strUseTimeA))
            {
                strUseTimeB = a.Value.ToString();
                bnn.UserEndHour = Convert.ToInt32(a.Value);
                intScamHour = bnn.UserEndHour - bnn.UserStartHour;
                if (intScamHour < 1)
                {
                    strUseTimeA = "";
                    strUseTimeB = "";
                    bnn.UserStartHour = 0;
                    bnn.UserEndHour = 0;
                    intScamHour = 0;
                    await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "시작 시간과 종료 시간을 입력하지 않았습니다.");
                }
                else if (intScamHour > 1)
                {
                    strUseTimeA = "";
                    strUseTimeB = "";
                    bnn.UserStartHour = 0;
                    bnn.UserEndHour = 0;
                    intScamHour = 0;
                    await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "1시간 이상을 입력할 수 없습니다.");
                }
                else
                {
                    //intScamHour = bnn.UserEndHour - bnn.UserStartHour;
                    bnn.Etc = bnn.Etc + "이용 종료시간 " + a.Value.ToString() + "시까지 입니다. \n 1인 이상이 함께 스크린 골프을 이용하려면 이용자 각자 신청하여야 합니다. \n 이용 여부를 이곳에 입력해 주세요. ";
                }
            }
        }

        /// <summary>
        /// 해당 시간에 예약 사실 확인
        /// </summary>
        public int intHourA { get; set; } = 0;
        private int OnHour(int hour)
        {
            try
            {
                string dateA1 = bnn.UserStartDate.ToShortDateString();
                string dateB2 = bnn.UserEndDate.ToShortDateString() + " 23:59:59.993";
                intHourA = community_Lib.GolfSceenInfor(Apt_Code, dateA1, dateB2, bnn.UsingKindCode, hour);
                return intHourA;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        /// <summary>
        /// 시간 선택 시 실행
        /// </summary>
        public int intHour { get; set; } = 0;
        private async Task onSaving(int hour)
        {
            if (hour < 23)
            {
                bnn.UserEndHour = hour + 1;
                bnn.UserStartHour = hour;
                intHour = hour;
            }
            else
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "동절기에는 선택할 수 없습니다..");
            }
        }

        /// <summary>
        /// 스크린 골프 시간선택
        /// </summary>
        private void btnCloseC()
        {
            InsertViewsC = "A";
            intHour = 0;
            intHourA = 0;
        }

        /// <summary>
        /// 스크린 골프
        /// </summary>
        private void OnStartDate(ChangeEventArgs m)
        {
            DateTime dt = Convert.ToDateTime(m.Value);
            bnn.UserStartDate = dt;
            bnn.UserEndDate = dt;
        }

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
                if (Code == "Tc20" && Count > 20)
                {
                    Cut = Count - 20;
                    Re = "대기 " + Cut;
                    strDivision = "B";
                }
                else if (Code == "Tc20" && Count <= 20)
                {
                    Re = Count.ToString();
                }
                else if (Code == "Tc28" && Count > 20)
                {
                    Cut = Count - 20;
                    Re = "대기 " + Cut;
                    strDivision = "B";
                }
                else if (Code == "Tc28" && Count <= 20)
                {
                    Re = Count.ToString();
                }
                else if (Code == "Tc29" && Count > 20)
                {
                    Cut = Count - 20;
                    Re = "대기 " + Cut;
                    strDivision = "B";
                }
                else if (Code == "Tc29" && Count <= 20)
                {
                    Re = Count.ToString();
                }
                else if (Code == "Tc22" && Count > 22)
                {
                    Cut = Count - 22;
                    Re = "대기 " + Cut;
                    strDivision = "B";
                }
                else if (Code == "Tc22" && Count <= 22)
                {
                    Re = Count.ToString();
                }
                else if (Code == "Tc26" && Count > 22)
                {
                    Cut = Count - 22;
                    Re = "대기 " + Cut;
                    strDivision = "B";
                }
                else if (Code == "Tc26" && Count <= 22)
                {
                    Re = Count.ToString();
                }
                else if (Code == "Tc21" && Count > 22)
                {
                    Cut = Count - 22;
                    Re = "대기 " + Cut;
                    strDivision = "B";
                }
                else if (Code == "Tc21" && Count <= 22)
                {
                    Re = Count.ToString();
                }
                else
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

        protected string Method(string Code, string Name)
        {
            string strCode = "";
            if (Code == "Tc20")
            {
                strCode = "줌바 (화목) 오전10시";
            }
            else if (Code == "Tc22")
            {
                strCode = "필라 (화목) 오후9시";
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

