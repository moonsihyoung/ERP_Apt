using Erp_Lib;
using Facilities;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Erp_Apt_Web.Pages.Admin.Bloom
{
    public partial class Index
    {
        [CascadingParameter] public Task<AuthenticationState> AuthenticationStateRef { get; set; }
        [Inject] public IApt_Lib apt_Lib { get; set; }
        [Inject] public NavigationManager MyNav { get; set; }
        [Inject] IJSRuntime JSRuntime { get; set; }
        [Inject] public IBloom_Lib bloom_Lib { get; set; }

        public int intNum { get; private set; }
        public string InsertViews { get; set; } = "A";
        public string Views { get; set; } = "A";
        public string strField { get; set; }
        public string strQuery { get; set; }
        public string strTitle { get; set; }
        public string strWSortA { get; set; }
        public string strWSortB { get; set; }


        public List<Bloom_Entity> ann { get; set; } = new List<Bloom_Entity>();
        public List<Bloom_Entity> annA { get; set; } = new List<Bloom_Entity>();
        public List<Bloom_Entity> annB { get; set; } = new List<Bloom_Entity>();
        public Bloom_Entity bnn { get; set; } = new Bloom_Entity();
        public string Apt_Code { get; private set; }
        public string Apt_Name { get; private set; }
        public string User_Code { get; private set; }
        public string User_Name { get; private set; }
        public int LevelCount { get; private set; }
        public string strSelect { get; set; }

        /// <summary>
        /// 로드시 실행
        /// </summary>
        /// <returns></returns>
        protected override async Task OnInitializedAsync()
        {
            var authState = await AuthenticationStateRef;
            if (authState.User.Identity.IsAuthenticated)
            {

                //로그인 정보
                Apt_Code = authState.User.Claims.FirstOrDefault(c => c.Type == "Apt_Code")?.Value;
                Apt_Name = authState.User.Claims.FirstOrDefault(c => c.Type == "Apt_Name")?.Value;
                User_Code = authState.User.Claims.FirstOrDefault(c => c.Type == "User_Code")?.Value;
                User_Name = authState.User.Claims.FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name")?.Value;

                LevelCount = Convert.ToInt32(authState.User.Claims.FirstOrDefault(c => c.Type == "LevelCount")?.Value);

                if (LevelCount > 10)
                {
                    await DisplayData();
                    annA = await bloom_Lib.GetList_Apt_ba(Apt_Code);
                }
                else
                {
                    await JSRuntime.InvokeAsync<object>("alert", "권한이 없습니다.");
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
        private async Task DisplayData()
        {
            pager.RecordCount = await bloom_Lib.GetListCount();
            ann = await bloom_Lib.GetList(pager.PageIndex);

            intNum = pager.RecordCount - (pager.PageIndex * pager.PageSize);
        }

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


        private void onInsert_Open()
        {
            bnn = new Bloom_Entity();
            strTitle = "업무 분류 입력";
            InsertViews = "B";
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
                //await JSRuntime.InvokeAsync<bool>("confirm", $"{Aid}번 계약을 정말로 삭제할 수 없습니까?");
                await bloom_Lib.Remove(Aid.ToString());
            }
            await DisplayData();
        }

        private void ByAid(Bloom_Entity bloom)
        {
            strTitle = "업무분류 상세보기";
            if (bloom == null)
            {
                bnn = bloom;
                return;
            }
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
            bnn.UserCode = User_Code;

            if (string.IsNullOrWhiteSpace(bnn.BloomA))
            {
                bnn.BloomA = await bloom_Lib.BloomNameA(bnn.B_N_A_Name);
            }

            if (string.IsNullOrWhiteSpace(bnn.BloomB))
            {
                bnn.BloomB = await bloom_Lib.BloomNameB(bnn.B_N_A_Name, bnn.B_N_B_Name);
            }

            if (string.IsNullOrWhiteSpace(bnn.Apt_Name))
            {
                bnn.Apt_Name = Apt_Name;
            }

            if (string.IsNullOrWhiteSpace(bnn.AptCode))
            {
                bnn.AptCode = Apt_Code;
            }
            
            #endregion

            bnn.Bloom_Code = strSelect;

            if (string.IsNullOrWhiteSpace(strSelect))
            {
                await JSRuntime.InvokeAsync<object>("alert", "분류를 선택하지 않았습니다.");
            }
            else if (strSelect == "B" && string.IsNullOrWhiteSpace(bnn.B_N_B_Name))
            {
                await JSRuntime.InvokeAsync<object>("alert", "중분류를 선택하지 않았습니다.");
            }
            else if (strSelect == "A" && string.IsNullOrWhiteSpace(bnn.B_N_A_Name))
            {
                await JSRuntime.InvokeAsync<object>("alert", "대분류를 선택하지 않았습니다.");
            }
            else if (strSelect == "C" && string.IsNullOrWhiteSpace(bnn.B_N_C_Name))
            {
                await JSRuntime.InvokeAsync<object>("alert", "소분류를 선택하지 않았습니다.");
            }
            else if (strSelect == "C" && string.IsNullOrWhiteSpace(bnn.B_N_C_Name))
            {
                await JSRuntime.InvokeAsync<object>("alert", "소분류를 선택하지 않았습니다.");
            }            
            else
            {
                if (string.IsNullOrWhiteSpace(bnn.B_N_Code))
                {
                    bnn.B_N_Code = strSelect + await bloom_Lib.LastAid();
                }

                if (bnn.Num < 2)
                {                    
                    await bloom_Lib.Add(bnn);
                }
                else
                {
                    await bloom_Lib.Edit(bnn);
                }
            }
            await DisplayData();
            bnn = new Bloom_Entity();
            strSelect = null;
            InsertViews = "A";
        }

        /// <summary>
        /// 수정
        /// </summary>
        /// <param name="bloom"></param>
        private async Task ByEdit(Bloom_Entity bloom)
        {
            strTitle = "업무분류 수정";
            bnn = bloom;
            strSelect = bnn.Bloom_Code;
            strWSortA = bnn.B_N_A_Name;
            
            annB = await bloom_Lib.GetList_bb(strWSortA);
            strWSortB = bnn.B_N_B_Name;
            
            InsertViews = "B";
        }

        private void btnClose()
        {
            InsertViews = "A";
        }

        /// <summary>
        /// 찾기
        /// </summary>
        private async Task OnSearch(ChangeEventArgs a)
        {
            strQuery = a.Value.ToString();
            if (string.IsNullOrWhiteSpace(strField))
            {
                await JSRuntime.InvokeAsync<object>("alert", "분류를 선택하지 않았습니다.");
            }
            else
            {
                ann = await bloom_Lib.SearchList(strField, strQuery);
            }
        }

        private void Onselect(ChangeEventArgs a)
        {
            strSelect = a.Value.ToString();
        }

        private async Task onWSortA(ChangeEventArgs a)
        {
            strWSortA = a.Value.ToString();
            bnn.B_N_A_Name = strWSortA;
            bnn.Intro = strWSortA;
            bnn.BloomA = await bloom_Lib.B_N_A_Code(strWSortA);
            annB = await bloom_Lib.GetList_bb(strWSortA);
        }

        private async Task onWSortB(ChangeEventArgs a)
        {
            strWSortB = a.Value.ToString();
            bnn.B_N_B_Name = strWSortB;
            bnn.BloomB = await bloom_Lib.B_N_B_Code(strWSortA, strWSortB);
            //annA = await bloom_Lib.GetList_cc(strWSortB);
        }
    }
}
