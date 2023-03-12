using Erp_Apt_Lib.Accounting;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Erp_Apt_Web.Pages.Accountings.Admin
{
    public partial class Index_Sort
    {
        [CascadingParameter] public Task<AuthenticationState> AuthenticationStateRef { get; set; }
        [Inject] public NavigationManager MyNav { get; set; } // Url 
        [Inject] IJSRuntime JSRuntime { get; set; }
        [Inject] IAccountSort_Lib accountSort_Lib { get; set; }

        List<AccountSortEntity> ann { get; set; } = new List<AccountSortEntity>();
        List<AccountSortEntity> BigSort { get; set; } = new List<AccountSortEntity>();
        AccountSortEntity bnn { get; set; } = new AccountSortEntity();

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
                    BigSort = await accountSort_Lib.GetList_Division("대분류");
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
        /// 데이터 뷰
        /// </summary>
        /// <returns></returns>
        private async Task DisplayData()
        {
            ann = await accountSort_Lib.GetList();
        }



        private void btnBigInsert()
        {
            bnn = new AccountSortEntity();
            strTitle = "계정 과목 분류 새로 등록";
            bnn.Division = "중분류";
            InsertViews = "B";
        }

        private void ByEdit(AccountSortEntity ar)
        {
            strTitle = "계정 과목 분류 수정 등록";
            bnn = ar;
            intAid = bnn.UpSort;
            if (bnn.Division == "대분류")
            {
                Sort = "A";
            }
            else
            {
                Sort = "B";
            }
            InsertViews = "B";
        }

        private async Task ByRemove(AccountSortEntity ar)
        {
            bool isDelete = await JSRuntime.InvokeAsync<bool>("confirm", $"{ar.Aid} 번 정보을 정말로 삭제하시겠습니까?");

            if (isDelete)
            {
                await accountSort_Lib.Remove(ar.Aid);
            }
            await DisplayData();
        }

        private void btnClose()
        {
            InsertViews = "A";
        }

        /// <summary>
        /// 계정과목 분류 등록
        /// </summary>
        private async Task btnSave()
        {
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
            bnn.PostIp = myIPAddress;
            #endregion
            bnn.User_Code = User_Code;
            if (string.IsNullOrWhiteSpace(User_Code))
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "로그인되지 않았습니다..");
            }
            else if (string.IsNullOrWhiteSpace(bnn.SortName))
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "분류명이 입력되지 않았습니다..");
            }
            else if (string.IsNullOrWhiteSpace(bnn.Division))
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "분류 구분이 입력되지 않았습니다..");
            }
            else if (bnn.Division == "중분류" && bnn.UpSort < 1)
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "중분임에도 상위코드가 입력되지 않았습니다..");
            }
            else
            {
                if (bnn.Aid < 1)
                {
                    await accountSort_Lib.Add(bnn);
                    bnn = new AccountSortEntity();                    
                }
                else
                {
                    await accountSort_Lib.Edit(bnn);
                    bnn = new AccountSortEntity();                    
                }
                await DisplayData();
            }            
        }

        /// <summary>
        /// 분류선택
        /// </summary>
        public string Sort { get; set; } = "B";
        public int intAid { get; set; }
        private void btnSortA()
        {
            Sort = "A";
            bnn.Division = "대분류";
            bnn.UpSort = 0;
        }
        private void btnSortB()
        {
            Sort ="B";
            bnn.Division = "중분류";
        }

        /// <summary>
        /// 대분류 선택 시
        /// </summary>
        private void OnSort(ChangeEventArgs a)
        {
            if (!string.IsNullOrWhiteSpace(a.Value.ToString()))
            {
                intAid = Convert.ToInt32(a.Value);
                bnn.UpSort = intAid; 
            }
        }

        private void btnAccountInsert()
        {
            MyNav.NavigateTo("/Accountings/Admin");
        }
    }
}
