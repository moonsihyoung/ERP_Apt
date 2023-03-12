using Erp_Apt_Lib.MaintenanceCost;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.JSInterop;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Threading.Tasks;
using Erp_Apt_Lib.MonthlyUsage;

namespace Erp_Apt_Web.Pages.Admin.CostDebit
{
    public partial class Index_Usage_Details
    {
        [Parameter] public string Code { get; set; }
        [CascadingParameter] public Task<AuthenticationState> AuthenticationStateRef { get; set; }
        [Inject] public NavigationManager MyNav { get; set; }
        [Inject] IJSRuntime JSRuntime { get; set; }
        [Inject] IHttpContextAccessor contextAccessor { get; set; }
        [Inject] IUsageDetails_Lib usageDetails_Lib { get; set; }
        [Inject] IMonthlyUsage_Lib monthlyUsage_Lib { get; set; }

        List<MonthlyUsage_Entity> ann { get; set; } = new List<MonthlyUsage_Entity>();
        UsageDetails_Entity bnn { get; set; } = new UsageDetails_Entity();


        public string User_Code { get; set; }
        public string User_Name { get; set; }
        public int LevelCount { get; private set; }
        public string Apt_Code { get; set; }
        public string Apt_Name { get; set; }

        public string InsertViews { get; set; } = "A";
        public string SortViews { get; set; } = "A";
        public string DetailsViews { get; set; } = "A";
        public string strSort { get; private set; }


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

                if (LevelCount > 5)
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

        public int Year { get; set; }
        public int Month { get; set; }
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
    }
}
