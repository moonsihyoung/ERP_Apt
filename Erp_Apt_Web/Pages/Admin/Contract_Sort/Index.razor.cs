using Company;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Erp_Apt_Web.Pages.Admin.Contract_Sort
{
    public partial class Index
    {
        [CascadingParameter] public Task<AuthenticationState> AuthenticationStateRef { get; set; }
        [Inject] public IContract_Sort_Lib contract_Sort_Lib { get; set; }
        [Inject] IJSRuntime JSRuntime { get; set; }
        [Inject] public NavigationManager MyNav { get; set; } // Url 


        List<Contract_Sort_Entity> ann { get; set; } = new List<Contract_Sort_Entity>();
        Contract_Sort_Entity bnn { get; set; } = new Contract_Sort_Entity();
        List<Contract_Sort_Entity> lstB { get; set; } = new List<Contract_Sort_Entity>();
        List<Contract_Sort_Entity> lstC { get; set; } = new List<Contract_Sort_Entity>();



        #region 변수
        public string User_Code { get; set; }
        public string User_Name { get; set; }
        public string Apt_Code { get; set; }
        public string Apt_Name { get; set; }
        public int LevelCount { get; set; } = 0;
        public string Views { get; set; } = "A"; //상세 열기
        public string RemoveViews { get; set; } = "A";//삭제 열기
        public string InsertViews { get; set; } = "A"; // 입력 열기

        #endregion

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
        /// 방문 시 실행
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

                if (LevelCount > 10)
                {
                    await DisplayData();
                }
                else
                {
                    await JSRuntime.InvokeAsync<object>("alert", "권한이 없습니다.");
                    MyNav.NavigateTo("/");
                }
            }
            else
            {
                await JSRuntime.InvokeAsync<object>("alert", "로그인되지 않았습니다.");
                MyNav.NavigateTo("/");
            }
        }

        /// <summary>
        /// 배치정보 목록 불러오기
        /// </summary>
        private async Task DisplayData()
        {
            pager.RecordCount = await contract_Sort_Lib.GetListsCount();
            ann = await contract_Sort_Lib.GetLists_Page(pager.PageIndex);
            //StateHasChanged();
        }

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
        /// 분류정보 등록 열기
        /// </summary>
        private void OnCompnayInput()
        {
            InsertViews = "B";
            //StateHasChanged();
        }

        /// <summary>
        /// 상세보기
        /// </summary>
        /// <param name="entity"></param>
        private void ByDetails(Contract_Sort_Entity entity)
        {
            Views = "B";
            bnn = entity;
            //StateHasChanged();
        }

        /// <summary>
        /// 수정열기
        /// </summary>
        /// <param name="entity"></param>
        private void ByEdit(Contract_Sort_Entity entity)
        {
            InsertViews = "B";
            bnn = entity;
        }

        /// <summary>
        /// 삭제하기
        /// </summary>
        /// <param name="entity"></param>
        private async Task ByRemove(int Aid)
        {
            bool isDelete = await JSRuntime.InvokeAsync<bool>("confirm", $"{Aid}번 계약을 정말로 삭제하시겠습니까?");

            if (isDelete)
            {
                await JSRuntime.InvokeAsync<bool>("confirm", $"{Aid}번 계약을 정말로 삭제할 수 없습니까?");
                //contract_Sort_Lib.Remove(Aid);
            }

            await DisplayData();
        }
    }
}
