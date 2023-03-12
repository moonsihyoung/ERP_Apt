using Erp_Apt_Lib.MonthlyUsage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Mobile.Pages.Common_Usage
{
    public partial class Index
    {
        [Parameter] public string Code { get; set; }
        [CascadingParameter] public Task<AuthenticationState> AuthenticationStateRef { get; set; }
        [Inject] public NavigationManager MyNav { get; set; }
        [Inject] IJSRuntime JSRuntime { get; set; }
        [Inject] IHttpContextAccessor contextAccessor { get; set; }
        [Inject] IMonthlyUsage_Lib monthlyUsage_Lib { get; set; }
        [Inject] IUsageDetails_Lib usageDetails_Lib { get; set; }

        List<MonthlyUsage_Entity> ann { get; set; } = new List<MonthlyUsage_Entity>();
        List<UsageDetails_Entity> lst { get; set; } = new List<UsageDetails_Entity>();
        MonthlyUsage_Entity bnn { get; set; } = new MonthlyUsage_Entity();
        MonthlyUsage_Entity dnn { get; set; } = new MonthlyUsage_Entity();
        UsageDetails_Entity vnn { get; set; } = new UsageDetails_Entity();
        List<UsageDetails_Entity> vnnA { get; set; } = new List<UsageDetails_Entity>();
        List<UsageDetails_Entity> vnnB { get; set; } = new List<UsageDetails_Entity>();
        List<UsageDetails_Entity> vnnC { get; set; } = new List<UsageDetails_Entity>();
        List<UsageDetails_Entity> vnnD { get; set; } = new List<UsageDetails_Entity>();

        public string strTitle { get; private set; }

        public string User_Code { get; set; }
        public string User_Name { get; set; }
        public string Dong { get; private set; }
        public string Ho { get; private set; }
        public int LevelCount { get; private set; }
        public string Apt_Code { get; set; }
        public string Apt_Name { get; set; }

        public string InsertViews { get; set; } = "A";
        public string InsertDetails { get; set; } = "A";
        public string SortViews { get; set; } = "A";
        public string Views { get; set; } = "A";
        public string strSort { get; private set; }


        /// <summary>
        /// 페이징
        /// </summary>
        protected DulPager.DulPagerBase pager = new DulPager.DulPagerBase()
        {
            PageNumber = 1,
            PageIndex = 0,
            PageSize = 15,
            PagerButtonCount = 3
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


                await DisplayData();

            }
            else
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "로그인되지 않았습니다..");
                MyNav.NavigateTo("/");
            }
        }

        /// <summary>
        /// 입력된 데이터 로드
        /// </summary>
        private async Task DisplayData()
        {
            if (strSort == "B")
            {

            }
            else
            {
                pager.RecordCount = await monthlyUsage_Lib.GetList_Count(Apt_Code);
                ann = await monthlyUsage_Lib.GetList(pager.PageIndex, Apt_Code);
            }
        }


        /// <summary>
        /// 상세보기
        /// </summary>
        public int intYear { get; set; } = 0;
        public int intMonth { get; set; } = 0;
        public int re1 { get; set; } = 0;
        private async Task SelectView(MonthlyUsage_Entity e)
        {
            bnn = e;
            lst = await usageDetails_Lib.GetList(Apt_Code, bnn.intYear, bnn.intMonth);
            if (bnn.intMonth == 1)
            {
                intMonth = 12;
                intYear = bnn.intYear - 1;
            }
            else
            {
                intMonth = bnn.intMonth - 1;
                intYear = bnn.intYear;
            }

            dnn = await monthlyUsage_Lib.GetById(bnn.Apt_Code, intYear, intMonth);
            //vnn = await usageDetails_Lib.GetById(Apt_Code, intYear, intMonth);

            strTitle = bnn.intYear + "년 " + bnn.intMonth + "월 공용부 사용료 정보";
            re1 = await usageDetails_Lib.GetById_Count(Apt_Code, intYear, intMonth);

            Electric_Views = "A";
            Water_Views = "A";
            Hoting_Views = "A";
            Heating_View = "A";
            Views = "B";
        }




        private async Task onMonthAsync(ChangeEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(e.Value.ToString()))
            {
                bnn.intMonth = Convert.ToInt32(e.Value);
                if (bnn.intYear < 2010 || bnn.intYear == 0)
                {
                    await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "년도가 선택되지 않았습니다..");
                }
                else if (bnn.intMonth <= 0)
                {
                    await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "해당 월이 선택되지 않았습니다..");
                }
                else
                {
                    bnn.UseDate = Convert.ToDateTime(bnn.intYear + "-" + bnn.intMonth + "-01");
                }
            }
        }



        public string strTitleA { get; set; }


        private void btnCloseA()
        {
            Views = "A";
        }

        private void btnCloseB()
        {
            InsertDetails = "A";
        }



        /// <summary>
        /// 전기료 상세 목록
        /// </summary>
        public string Electric_Views { get; set; } = "A";
        public string Water_Views { get; set; } = "A";
        public string Hoting_Views { get; set; } = "A";
        public string Heating_View { get; set; } = "A";
        private async Task DetailListViewA(MonthlyUsage_Entity ar)
        {
            if (Electric_Views == "A")
            {
                vnnA = await usageDetails_Lib.GetList_sort(Apt_Code, ar.intYear, ar.intMonth, "전기료");
                Electric_Views = "B";
                Water_Views = "A";
                Hoting_Views = "A";
                Heating_View = "A";
            }
            else
            {
                Electric_Views = "A";
                Water_Views = "A";
                Hoting_Views = "A";
                Heating_View = "A";
            }
        }

        /// <summary>
        /// 수도료 상세 목록
        /// </summary>
        private async Task DetailListViewB(MonthlyUsage_Entity ar)
        {
            if (Water_Views == "A")
            {
                vnnB = await usageDetails_Lib.GetList_sort(Apt_Code, ar.intYear, ar.intMonth, "수도료");
                Electric_Views = "A";
                Water_Views = "B";
                Hoting_Views = "A";
                Heating_View = "A";
            }
            else
            {
                Electric_Views = "A";
                Water_Views = "A";
                Hoting_Views = "A";
                Heating_View = "A";
            }
        }

        /// <summary>
        /// 급탕료 상세 목록
        /// </summary>
        private async Task DetailListViewC(MonthlyUsage_Entity ar)
        {
            if (Hoting_Views == "A")
            {
                vnnC = await usageDetails_Lib.GetList_sort(Apt_Code, ar.intYear, ar.intMonth, "급탕료");
                Electric_Views = "A";
                Water_Views = "A";
                Hoting_Views = "B";
                Heating_View = "A";
            }
            else
            {
                Electric_Views = "A";
                Water_Views = "A";
                Hoting_Views = "A";
                Heating_View = "A";
            }
        }

        /// <summary>
        /// 난방료 상세 목록
        /// </summary>
        private async Task DetailListViewD(MonthlyUsage_Entity ar)
        {
            if (Heating_View == "A")
            {
                vnnD = await usageDetails_Lib.GetList_sort(Apt_Code, ar.intYear, ar.intMonth, "난방료");
                Electric_Views = "A";
                Water_Views = "A";
                Hoting_Views = "A";
                Heating_View = "B";
            }
            else
            {
                Electric_Views = "A";
                Water_Views = "A";
                Hoting_Views = "A";
                Heating_View = "A";
            }
        }
    }
}
