using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using System;
using System.Threading.Tasks;
using Erp_Apt_Lib.Decision;
using System.Collections.Generic;
using System.Linq;
using Erp_Lib;
using Erp_Entity;

namespace Erp_Apt_Web.Pages.Admin.Approval
{
    public partial class Index
    {
        [Parameter] public string Code { get; set; }
        [CascadingParameter] public Task<AuthenticationState> AuthenticationStateRef { get; set; }
        [Inject] public NavigationManager MyNav { get; set; }
        [Inject] IJSRuntime JSRuntime { get; set; }
        [Inject] IApproval_Lib approval_Lib { get; set; }
        [Inject] public IPost_Lib post_Lib { get; set; }
        [Inject] public IDuty_Lib duty_Lib { get; set; }


        public string User_Code { get; set; }
        public string User_Name { get; set; }
        public string Apt_Code { get; set; }
        public string Apt_Name { get; set; }
        public string UserName { get; set; }
        public int LevelCount { get; set; }

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

                if (LevelCount >= 5)
                {
                    Post = await post_Lib.GetList("A");
                    await DisplayData();
                }
                else
                {
                    await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "접근 권한이 없습니다..");
                    MyNav.NavigateTo("/");
                }
            }
            else
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "로그인되지 않았습니다..");
                MyNav.NavigateTo("/");
            }
        }

        public List<Post_Entity> Post { get; set; } = new List<Post_Entity>();
        public List<Duty_Entity> Duty { get; set; } = new List<Duty_Entity>();
        List<Approval_Entity> ann { get; set; } = new List<Approval_Entity>();
        public string strBloom { get; set; }
        private async Task DisplayData()
        {
            if (!string.IsNullOrWhiteSpace(strBloom))
            {
                ann = await approval_Lib.GetList(Apt_Code, strBloom);
            }
            else
            {
                pager.RecordCount = await approval_Lib.GetListCount(Apt_Code);
                ann = await approval_Lib.GetBloomList(pager.PageIndex, Apt_Code);
            }
        }

        public string InsertViews { get; set; } = "A";
        Approval_Entity bnn { get; set; } = new Approval_Entity();
        public void btnInsertOpen()
        {
            InsertViews = "B";
            strTitle = "결재 정보 입력";
            bnn = new Approval_Entity();
        }

        public async Task btnResert()
        {
            strBloom = null;
            await DisplayData();
        }

        public async Task btnRemove(Approval_Entity ar)
        {
            bool isDelete = await JSRuntime.InvokeAsync<bool>("confirm", $"{ar.Bloom}을 정말로 삭제하시겠습니까?");
            if (isDelete)
            {
                await approval_Lib.Remove(ar.Num);
                await DisplayData();
            }
        }

        //public string EditViews { get; set; } = "A";
        public string strTitle { get; set; }
        public async Task ByEdits(Approval_Entity ar)
        {
            bnn = ar;
            Post_Code = await post_Lib.PostCode(bnn.Post);
            Duty = await duty_Lib.GetList(Post_Code, "A");
            InsertViews = "B";
            strTitle = "결재 정보 수정";
        }

        /// <summary>
        /// 검색
        /// </summary>
        private async Task OnBloom(ChangeEventArgs a)
        {
            strBloom = a.Value.ToString();
            bnn.Bloom = strBloom;
            if (strBloom == "작업일지")
            {
                bnn.Bloom_Code = "Service";
            }
            else if (strBloom == "민원일지")
            {
                bnn.Bloom_Code = "Appeal";
            }
            else if (strBloom == "문서관리")
            {
                bnn.Bloom_Code = "Document";
            }
            else if (strBloom == "시설물점검일지")
            {
                bnn.Bloom_Code = "Check";
            }
            else if (strBloom == "지출결의서")
            {
                bnn.Bloom_Code = "Disbursement";
            }
            else if (strBloom == "기안문서")
            {
                bnn.Bloom_Code = "Draft";
            }
            else if (strBloom == "방송·공고")
            {
                bnn.Bloom_Code = "Notice";
            }
            else if (strBloom == "하자관리")
            {
                bnn.Bloom_Code = "Defect";
            }
            else if (strBloom == "주민공동시설")
            {
                bnn.Bloom_Code = "Community";
            }

            ann = await approval_Lib.GetList(Apt_Code, strBloom);
        }

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
            bnn.PostIP = myIPAddress;
            bnn.ModifyIP = myIPAddress;
            bnn.ModifyDate = DateTime.Now;
            #endregion
            bnn.AptCode = Apt_Code;
            bnn.AptName = Apt_Name;
            bnn.PostDuty = bnn.Post + bnn.Duty;
            //bnn.Step = await approval_Lib.GetListCount(Apt_Code);
            //bnn.Step = bnn.Step + 1;

            if (bnn.Num < 1)
            {
                await approval_Lib.Add(bnn);
            }
            else
            {
                await approval_Lib.Edit(bnn);
            }
            await DisplayData();
            bnn = new();
        }

        /// <summary>
        /// 부서 선택하면 직책 만들기
        /// </summary>
        public string Post_Code { get; set; }
        private async Task OnPost(ChangeEventArgs args)
        {
            Post_Code = args.Value.ToString();
            bnn.Post = await post_Lib.PostName(args.Value.ToString());
            Duty = await duty_Lib.GetList(args.Value.ToString(), "A");
        }

        private void btnClose()
        {
            InsertViews = "A";
        }
    }
}
