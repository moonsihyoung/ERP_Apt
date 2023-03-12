using Erp_Apt_Lib;
using Erp_Apt_Staff;
using Facilities;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Erp_Apt_Web.Pages.Facilities.Facility_Place
{
    public partial class Index
    {
        #region 인스턴스
        [CascadingParameter] public Task<AuthenticationState> AuthenticationStateRef { get; set; }
        [Inject] public NavigationManager MyNav { get; set; } // Url 
        [Inject] IJSRuntime JSRuntime { get; set; }
        [Inject] public IStaff_Lib staff_Lib { get; set; }

        [Inject] public IBloom_Lib bloom_Lib { get; set; }
        //[Inject] public IDraftDetail_Lib draftDetail_Lib { get; set; }
        //[Inject] public IDraftAttach_Lib draftAttach_Lib { get; set; }
        //[Inject] public IBloom_Lib bloom_Lib { get; set; }
        [Inject] public IReferral_career_Lib referral_Career_Lib { get; set; }
        //[Inject] public IDecision_Lib decision_Lib { get; set; } // 결재정보

        //[Inject] public IPost_Lib Post_Lib { get; set; }
        [Inject] public ISw_Files_Lib files_Lib { get; set; }
        #endregion

        #region 속성
        List<Referral_career_Entity> fnn = new List<Referral_career_Entity>();
        
        List<Bloom_Entity> ann = new List<Bloom_Entity>();
        Bloom_Entity bnn = new Bloom_Entity();
        Sw_Files_Entity hnn { get; set; } = new Sw_Files_Entity(); // 첨부 파일 정보
        List<Sw_Files_Entity> Files_Entity { get; set; } = new List<Sw_Files_Entity>();
        Referral_career_Entity cnnA { get; set; } = new Referral_career_Entity();

        List<Bloom_Entity> bloom_A = new List<Bloom_Entity>();
        List<Bloom_Entity> bloom_B = new List<Bloom_Entity>();
        List<Bloom_Entity> bloom_C = new List<Bloom_Entity>();
        #endregion

        #region 변수
        public string User_Code { get; set; }
        public string User_Name { get; set; }
        public string Apt_Code { get; set; }
        public string Apt_Name { get; set; }
        public int LevelCount { get; set; } = 0;
        public string Views { get; set; } = "A"; //상세 열기
        public string RemoveViews { get; set; } = "A"; //삭제 열기
        public string InsertViews { get; set; } = "A"; //입력 열기
        public string InsertFiles { get; set; } = "A"; // 파일 등록 열기
        public string strTitle { get; set; }
        public int intNum { get; set; }
        public string strSortA { get; set; }
        public string strSortB { get; set; }
        public string strSortC { get; set; }
        public string DetailsViews { get; set; } = "A";
        public string strEdit { get; set; } = "A";
        
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
                    bloom_A = await bloom_Lib.GetList_Apt_ba(Apt_Code); // 작업 대분류
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
        /// 배치정보 목록 불러오기
        /// </summary>
        private async Task DisplayData()
        {
            if (string.IsNullOrWhiteSpace(strBloomA))
            {
                pager.RecordCount = await bloom_Lib.GetList_Apt_Count(Apt_Code);
                ann = await bloom_Lib.GetList_Apt(pager.PageIndex, Apt_Code);
                intNum = pager.RecordCount - (pager.PageIndex * pager.PageSize);
            }
            else
            {
                pager.RecordCount = await bloom_Lib.GetList_Apt_Bloom_Count(Apt_Code, strBloomA);
                ann = await bloom_Lib.GetList_Apt_Bloom(pager.PageIndex, Apt_Code, strBloomA);
                intNum = pager.RecordCount - (pager.PageIndex * pager.PageSize);
            }
        }
        /// <summary>
        /// 새로등록 열기 
        /// </summary>
        private void OnOpen()
        {
            strTitle = "작업장소 정보 새로 등록";
            InsertViews = "B";
        }

        /// <summary>
        /// 대분류로 목록 만들기
        /// </summary>
        public string strBloomA { get; set; }
        private async Task onSortAA(ChangeEventArgs a)
        {
            strBloomA = a.Value.ToString();
            await DisplayData();
            //ann = await bloom_Lib.GetList_Apt_Bloom(Apt_Code, a.Value.ToString());
        }

        /// <summary>
        /// 대분류 선택
        /// </summary>
        private async Task onSortA(ChangeEventArgs a)
        {
            strSortA = a.Value.ToString();
            bloom_B = await bloom_Lib.GetList_bb(strSortA);

            bnn.B_N_A_Name = strSortA;
            strSortB = "Z";
            strSortC = "Z";
        }

        /// <summary>
        /// 중분류 선택
        /// </summary>
        private async Task onSortB(ChangeEventArgs a)
        {
            string strA = a.Value.ToString();
            //strWSortB = "Z";
            strSortC = "Z";
            bloom_C = await bloom_Lib.GetList_cc(strA);
            bnn.B_N_B_Name = strA;
        }

        /// <summary>
        /// 세분류 선택
        /// </summary>
        private void onSortC(ChangeEventArgs a)
        {
            string strA = a.Value.ToString();
           
            bnn.B_N_C_Name = strA;
        }

        private void ByAid(Bloom_Entity ar)
        {
            bnn = ar;
        }

        private void btnClose()
        {
            InsertViews = "A";
        }

        private async Task btnSave()
        {
            bnn.AptCode = Apt_Code;
            bnn.Apt_Name = Apt_Name;
            bnn.Bloom_Code = "D";

            if (bnn.Intro == null || bnn.Intro == "")
            {
                bnn.Intro = bnn.B_N_A_Name;
            }

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
            #endregion

            int bv = await bloom_Lib.Be(bnn.AptCode, bnn.B_N_A_Name, bnn.Bloom);

            if (bnn.AptCode == null || bnn.AptCode == "")
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "로그인하지 않았습니다.");
            }
            else if (bnn.B_N_A_Name == null || bnn.B_N_A_Name == "")
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "대분류를 선택하지 않았습니다.");
            }
            else if (bnn.B_N_B_Name == null || bnn.B_N_B_Name == "")
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "중분류를 선택하지 않았습니다.");
            }
            else if (bnn.B_N_C_Name == null || bnn.B_N_C_Name == "")
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "소분류하지 않았습니다.");
            }
            else if (bnn.Bloom == null || bnn.Bloom == "")
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "작업장소를 입력하지 않았습니다.");
            }
            else if (bnn.Intro == null || bnn.Intro == "")
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "설명을 입력하지 않았습니다.");
            }
            else
            {
                if (bnn.Num < 1)
                {
                    if (bv < 1)
                    {
                        bnn.B_N_Code = "B_N_Code_A" + await bloom_Lib.LastAid();
                        await bloom_Lib.Add(bnn);

                        await DisplayData();

                        //ann = await bloom_Lib.GetList_Apt_Bloom(Apt_Code, bnn.B_N_A_Name);
                    }
                    //InsertViews = "A";
                }
                else
                {
                    await bloom_Lib.Update(bnn);
                    InsertViews = "A";
                }
                
            }
        }

        private async Task ByEdit(Bloom_Entity ar)
        {
            bnn = ar;

            strSortA = bnn.B_N_A_Name;
            bloom_B = await bloom_Lib.GetList_bb(strSortA);
            strSortB = bnn.B_N_B_Name;
            bloom_C = await bloom_Lib.GetList_cc(strSortB);
            strSortC = bnn.B_N_C_Name;
            strTitle = "작업 장소 수정";

            InsertViews = "B";
        }
    }
}
