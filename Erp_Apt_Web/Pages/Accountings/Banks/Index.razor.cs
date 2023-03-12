using Erp_Apt_Lib.Accounting;
using Erp_Apt_Staff;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Erp_Apt_Web.Pages.Accountings.Banks
{
    public partial class Index
    {
        [CascadingParameter] public Task<AuthenticationState> AuthenticationStateRef { get; set; }
        [Inject] public NavigationManager MyNav { get; set; } // Url 
        [Inject] IJSRuntime JSRuntime { get; set; }
        [Inject] public IStaff_Lib staff_Lib { get; set; }
        [Inject] public IBankAccount_Lib bankAccount_Lib { get; set; }
        [Inject] public IAccount_Lib account_Lib { get; set; }

        public List<BankAccountEntity> ann { get; set; } = new List<BankAccountEntity>();

        //private int intNum;

        List<AccountSortEntity> SortA { get; set; } = new List<AccountSortEntity>();
        List<AccountSortEntity> SortB { get; set; } = new List<AccountSortEntity>();
        BankAccountEntity bnn { get; set; } = new BankAccountEntity();

        public string InsertViews { get; set; } = "A";
        public string User_Code { get; set; }
        public string User_Name { get; set; }
        public string Apt_Code { get; set; }
        public string Apt_Name { get; set; }
        public string UserName { get; set; }
        public int LevelCount { get; set; }
        public string strTitle { get; set; }


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
                if (LevelCount >= 10)
                {
                    //SortA = await accountSort_Lib.GetList_Division("대분류");
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
        /// 모달 이동 
        /// </summary>
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await JSRuntime.InvokeVoidAsync("setModalDraggableAndResizable");
            await base.OnAfterRenderAsync(firstRender);
        }

        /// <summary>
        /// 계정과목 등록 열기
        /// </summary>
        private void btnInsert()
        {
            strTitle = "지출결의서 계정과목 등록";
            bnn = new BankAccountEntity();
            bnn.User_Code = User_Code;
            InsertViews = "B";
        }

        private async Task DisplayData()
        {            
            ann = await bankAccount_Lib.GetList(Apt_Code);
        }

        private void ByEdit(BankAccountEntity ar)
        {
            bnn = ar;
            InsertViews = "B";
        }

        private async Task ByRemove(BankAccountEntity ar)
        {
            bool isDelete = await JSRuntime.InvokeAsync<bool>("confirm", $"{ar.Aid} 번 정보을 정말로 삭제하시겠습니까?");

            if (isDelete)
            {
                await bankAccount_Lib.Remove(ar.Aid);
            }
            await DisplayData();
        }
    }
}
