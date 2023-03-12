using Erp_Apt_Lib.Stocks;
using Erp_Apt_Staff;
using Erp_Lib;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Erp_Apt_Web.Pages.Admin.Stocks
{
    public partial class Index
    {
        [CascadingParameter] public Task<AuthenticationState> AuthenticationStateRef { get; set; }
        [Inject] public IApt_Lib apt_Lib { get; set; }
        [Inject] public NavigationManager MyNav { get; set; }
        [Inject] IJSRuntime JSRuntime { get; set; }
        [Inject] public IReferral_career_Lib referral_Career_Lib { get; set; }
        [Inject] public IStocks_Lib stocks_Lib { get; set; }
        [Inject] public IWhSock_Lib whSock_Lib { get; set; }

        #region 목록
        List<Referral_career_Entity> fnn = new List<Referral_career_Entity>();
        List<WareHouse_Entity> wheL = new List<WareHouse_Entity>();
        WareHouse_Entity wheE { get; set; } = new WareHouse_Entity();
        List<Stock_Code_Entity> sceL = new List<Stock_Code_Entity>();
        Stock_Code_Entity sceE { get; set; } = new Stock_Code_Entity();

        private List<Stock_Code_Entity> ann { get; set; } = new List<Stock_Code_Entity>();
        #endregion

        #region 변수
        public string User_Code { get; set; }
        public string User_Name { get; set; }
        public string Apt_Code { get; set; }
        public string Apt_Name { get; set; }
        public string UserName { get; set; }
        public string ListViews { get; set; } = "B";
        public string PostDuty { get; set; }
        public string DetailsViews { get; set; } = "A";
        public string InsertViews { get; set; } = "A";
        public string InOutViews { get; set; } = "A";
        public string PrivateViews { get; set; } = "A";
        public string EditViews { get; set; } = "A";
        public string StocksViews { get; set; } = "A";
        public string Company_num { get; set; }
        public string FileInsertViews { get; set; } = "A";
        public string FileViews { get; set; } = "A";
        public string InputCompleteViews { get; set; } = "A";
        #endregion

        protected override async Task OnInitializedAsync()
        {
            var authState = await AuthenticationStateRef;
            if (authState.User.Identity.IsAuthenticated)
            {
                //로그인 정보
                Apt_Code = authState.User.Claims.FirstOrDefault(c => c.Type == "Apt_Code")?.Value;
                User_Code = authState.User.Claims.FirstOrDefault(c => c.Type == "User_Code")?.Value;
                Apt_Name = authState.User.Claims.FirstOrDefault(c => c.Type == "Apt_Name")?.Value;
                User_Name = authState.User.Claims.FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name")?.Value;

                

                await DisplayData();
            }
            else
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "로그인되지 않았습니다..");
                MyNav.NavigateTo("/");
            }
        }

        /// <summary>
        /// 상세보기
        /// </summary>
        /// <returns></returns>
        private async Task DisplayData()
        {
            ann = await stocks_Lib.St_List_Apt_Query_new(Apt_Code, "St_Group", "st_Code_2");
        }
    }
}
