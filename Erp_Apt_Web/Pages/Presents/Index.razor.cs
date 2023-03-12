using Erp_Apt_Lib.Logs;
using Erp_Apt_Staff;
using Erp_Lib;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Erp_Apt_Web.Pages.Presents
{
    public partial class Index
    {
        #region 인스턴스
        [CascadingParameter] public Task<AuthenticationState> AuthenticationStateRef { get; set; }
        [Inject] public NavigationManager MyNav { get; set; } // Url 
        [Inject] IJSRuntime JSRuntime { get; set; }
        [Inject] ILogs_Lib logs_Lib { get; set; }
        [Inject] public IApt_Lib apt_Lib { get; set; }
        [Inject] public IApt_Sub_Lib apt_Sub_Lib { get; set; }
        [Inject] public IStaff_Lib staff_Lib { get; set; }        
        #endregion

        #region 속성
        List<Logs_Entites> ann { get; set; } = new List<Logs_Entites>();
        Staff_Entity snn { get; set; } = new Staff_Entity();
        #endregion

        #region 변수
        public string Views { get; set; } = "A";
        public string InsertViews { get; set; } = "A";
        public string Apt_Code { get; set; }
        public string User_Code { get; set; }
        public string Apt_Name { get; set; }
        public string User_Name { get; set; }
        public int LevelCount { get; set; }
        public string strTitle { get; set; }
        
        //private ElementReference myref;
        #endregion

        #region 페이징
        /// <summary>
        /// 페이징 속성
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
        #endregion

        /// <summary>
        /// 로딩 시 실행
        /// </summary>
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
                LevelCount = Convert.ToInt32(authState.User.Claims.FirstOrDefault(c => c.Type == "LevelCount")?.Value);


                if (LevelCount < 5)
                {
                    await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "권한이 없습니다.");
                    MyNav.NavigateTo("/");
                }
                else
                {
                    await DisplayData();
                }

            }
            else
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "로그인하지 않았습니다.");
                MyNav.NavigateTo("/");
            }
        }

        /// <summary>
        /// 모달 이동 
        /// </summary>
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await JSRuntime.InvokeVoidAsync("setModalDraggableAndResizable");
            await base.OnAfterRenderAsync(firstRender);
        }

        /// <summary>
        /// 데이터 정보 
        /// </summary>
        private async Task DisplayData()
        {
            pager.RecordCount = await logs_Lib.GetList_Apt_Count(Apt_Code);
            ann = await logs_Lib.GetList_Apt(pager.PageIndex, Apt_Code);
        }
    }
}
