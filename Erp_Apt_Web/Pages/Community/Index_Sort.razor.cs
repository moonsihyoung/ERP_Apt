using Erp_Apt_Lib.Community;
using Erp_Lib;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Erp_Apt_Web.Pages.Community
{
    public partial class Index_Sort
    {
        #region Inject
        [Parameter] public string Code { get; set; }
        [CascadingParameter] public Task<AuthenticationState> AuthenticationStateRef { get; set; }
        [Inject] public NavigationManager MyNav { get; set; }
        [Inject] IJSRuntime JSRuntime { get; set; }
        [Inject] public IErp_AptPeople_Lib Erp_AptPeople_Lib { get; set; }
        [Inject] public ICommunityUsingKind_Lib communityUsingKind { get; set; }
        [Inject] public ICommunityUsingTicket_Lib communityUsingTicket { get; set; }
        #endregion

        #region 목록
        List<CommunityUsingKind_Entity> ann = new List<CommunityUsingKind_Entity>();
        CommunityUsingKind_Entity bnn = new CommunityUsingKind_Entity();
        List<CommunityUsingTicket_Entity> annA = new List<CommunityUsingTicket_Entity>();
        CommunityUsingTicket_Entity bnnA = new CommunityUsingTicket_Entity();
        #endregion

        #region 변수
        public string User_Code { get; set; }
        public string User_Name { get; set; }
        public string Apt_Code { get; set; }
        public string Apt_Name { get; set; }
        public string UserName { get; set; }
        public int LevelCount { get; set; }
        public string InsertViewsA { get; set; } = "A";
        public string InsertViewsB { get; set; } = "A";
        public string strTitle { get; set; }
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
            //StateHasChanged();
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
        /// <returns></returns>
        protected override async Task OnInitializedAsync()
        {
            var authState = await AuthenticationStateRef;
            if (authState.User.Identity.IsAuthenticated)
            {
                //var result = await ProtectedSessionStore.GetAsync<int>("count");
                //var resultA = await ProtectedLocalStore.GetAsync<int>("count");
                //로그인 정보
                Apt_Code = authState.User.Claims.FirstOrDefault(c => c.Type == "Apt_Code")?.Value;
                Apt_Name = authState.User.Claims.FirstOrDefault(c => c.Type == "Apt_Name")?.Value;
                User_Code = authState.User.Claims.FirstOrDefault(c => c.Type == "User_Code")?.Value;
                User_Name = authState.User.Claims.FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name")?.Value;
                LevelCount = Convert.ToInt32(authState.User.Claims.FirstOrDefault(c => c.Type == "LevelCount")?.Value);

                await DisplayData();
            }
            else
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "로그인되지 않았습니다..");
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
        /// 데이터 뷰
        /// </summary>
        private async Task DisplayData()
        {
            ann = await communityUsingKind.GetList_Apt(Apt_Code);
        }

        private void btnInsertKind()
        {
            strTitle = "커뮤니티 종류 입력";
            bnn = new CommunityUsingKind_Entity();
            InsertViewsA = "B";
            InsertViewsB = "A";
        }

        private void btnInsertTicket()
        {
            strTitle = "커뮤니티 분류 입력";
            bnnA = new CommunityUsingTicket_Entity();
            InsertViewsB = "B";
            InsertViewsA = "A";
        }

        private async Task btnSaveA()
        {
            await communityUsingKind.Add(bnn);
        }

        private async Task btnSaveB()
        {
            await communityUsingTicket.Add(bnnA);
        }

        private void btnCloseA()
        {
            InsertViewsA = "A";
        }

        private void btnCloseB()
        {
            InsertViewsB = "A";
        }
    }
}
