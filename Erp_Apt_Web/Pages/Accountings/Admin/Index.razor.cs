using Erp_Apt_Lib.Accounting;
using Erp_Apt_Staff;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Erp_Apt_Web.Pages.Accountings.Admin
{
    public partial class Index
    {
        [CascadingParameter] public Task<AuthenticationState> AuthenticationStateRef { get; set; }
        [Inject] public NavigationManager MyNav { get; set; } // Url 
        [Inject] IJSRuntime JSRuntime { get; set; }
        [Inject] public IStaff_Lib staff_Lib { get; set; }
        [Inject] public IAccountSort_Lib accountSort_Lib { get; set; }
        [Inject] public IAccount_Lib account_Lib { get; set; }

        List<AccountEntity> ann { get; set; } = new List<AccountEntity>();

        private int intNum;

        List<AccountSortEntity> SortA { get; set; } = new List<AccountSortEntity>();
        List<AccountSortEntity> SortB { get; set; } = new List<AccountSortEntity>();
        AccountEntity bnn { get; set; } = new AccountEntity();

        public string InsertViews { get; set; } = "A";
        public string User_Code { get; set; }
        public string User_Name { get; set; }
        public string Apt_Code { get; set; }
        public string Apt_Name { get; set; }
        public string UserName { get; set; }
        public int LevelCount { get; set; }
        public string strTitle { get; set; }


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

        private async Task DisplayData()
        {
            pager.RecordCount = await account_Lib.GetListCount();
            ann = await account_Lib.GetList(pager.PageIndex);
            
            intNum = pager.RecordCount - (pager.PageIndex * pager.PageSize);
        }
        #endregion

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
                if (LevelCount > 10)
                {
                    SortA = await accountSort_Lib.GetList_Division("대분류");
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
            bnn = new AccountEntity();
            bnn.User_Code = User_Code;
            InsertViews = "B";
        }

        /// <summary>
        /// 지출결의서 계정과목 분류 등록으로 이도
        /// </summary>
        private void btnSortMove()
        {
            MyNav.NavigateTo("/Accountings/Admin/Index_Sort");
        }

        /// <summary>
        /// 계정과목 수정
        /// </summary>
        private async Task ByEdit(AccountEntity ar)
        {
            strTitle = "지출결의서 계정과목 정보 수정";
            bnn = ar;
            bnn.User_Code = User_Code;
            intAid = bnn.SortA_Code;
            SortB = await accountSort_Lib.GetList_Sort(bnn.SortA_Code.ToString());
            InsertViews = "B";
        }

        /// <summary>
        /// 계정과목 삭제
        /// </summary>
        private async Task ByRemove(AccountEntity ar)
        {
            bool isDelete = await JSRuntime.InvokeAsync<bool>("confirm", $"{ar.Aid} 번 정보을 정말로 삭제하시겠습니까?");

            if (isDelete)
            {
                await account_Lib.Remove(ar.Aid);
            }
            await DisplayData();
        }

        private async Task btnSave()
        {
            if (string.IsNullOrWhiteSpace(bnn.AccountName))
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "계정과목명을 입력하지 않았습니다.");
            }
            else if (string.IsNullOrWhiteSpace(Apt_Code))
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "로그인하지 않았습니다.");
                MyNav.NavigateTo("/");
            }
            //else if (string.IsNullOrWhiteSpace(bnn.AccountType))
            //{
            //    await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "거래 구분을 선택하지 않았습니다.");
            //}
            else if (string.IsNullOrWhiteSpace(bnn.SortA))
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "대분류를 선택하지 않았습니다.");
            }
            else if (string.IsNullOrWhiteSpace(bnn.SortB))
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "중분류를 선택하지 않았습니다.");
            }
            else if (string.IsNullOrWhiteSpace(bnn.Details))
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "상세설명을 입력하지 않았습니다.");
            }
            else
            {
                bnn.User_Code = User_Code;
                bnn.AccountType = "비용";
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
                if (bnn.Aid < 1)
                {
                    await account_Lib.Add(bnn);
                    bnn = new AccountEntity();
                    await DisplayData();
                }
                else
                {
                    await account_Lib.Edit(bnn);
                    bnn = new AccountEntity();
                    await DisplayData();
                }
            }
        }

        /// <summary>
        /// 입력 및 수정 닫기
        /// </summary>
        private void btnClose()
        {
            InsertViews = "A";
        }

        /// <summary>
        /// 대분분 선택
        /// </summary>
        public int intAid { get; set; }
        private async Task OnSort(ChangeEventArgs a)
        {
            if (!string.IsNullOrWhiteSpace(a.Value.ToString()))
            {
                intAid = Convert.ToInt32(a.Value);
                bnn.SortA = await accountSort_Lib.Sort_Name(intAid.ToString());
                bnn.SortA_Code = intAid;
                SortB = await accountSort_Lib.GetList_Sort(intAid.ToString()); 
            }
        }

        private async Task OnSortB(ChangeEventArgs a)
        {
            if (!string.IsNullOrWhiteSpace(a.Value.ToString()))
            {
                bnn.SortB_Code = Convert.ToInt32(a.Value);
                bnn.SortB = await accountSort_Lib.Sort_Name(bnn.SortB_Code.ToString()); 
            }
        }
    }
}
