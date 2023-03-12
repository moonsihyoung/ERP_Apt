using Erp_Apt_Lib;
using Erp_Apt_Lib.Check;
using Erp_Apt_Staff;
using Erp_Lib;
using Facilities;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Erp_Apt_Web.Pages.Check.Items
{
    public partial class Index
    {
        [Inject] public IApt_Lib apt_Lib { get; set; }  // 공동주택 정보
        [Inject] public IApt_Sub_Lib apt_Sub_Lib { get; set; }  // 공동주택 상세 정보
        [Inject] public IReferral_career_Lib referral_Career { get; set; } // 공동주택 배치 정보
        [Inject] IBloom_Lib Bloom_Lib { get; set; } // 시설물 분류 정보
        [Inject] IJSRuntime JSRuntime { get; set; } // 자바 스크립트 사용 로드
        [Inject] public ICheck_Items_Lib check_Items_Lib { get; set; } // 시설물 점검 사항 클래스
        [Inject] public ICheck_Object_Lib check_Object_Lib { get; set; } // 시설물 점검 대상 클래스
        [Inject] public ICheck_Cycle_Lib check_Cycle_Lib { get; set; } // 시설물 점검 주기 클래스
        [Inject] public NavigationManager MyNav { get; set; } // Url 


        [CascadingParameter]
        public Task<AuthenticationState> AuthenticationStateRef { get; set; }
        List<Bloom_Entity> boo_b { get; set; } = new List<Bloom_Entity>(); //시설물 분류
        List<Bloom_Entity> boo_c { get; set; } = new List<Bloom_Entity>(); //시설물 분류
        List<Bloom_Entity> boo_d { get; set; } = new List<Bloom_Entity>(); //시설물 장소
        Sw_Files_Entity dnn { get; set; } = new Sw_Files_Entity(); // 첨부 파일 정보
        List<Sw_Files_Entity> Files_Entity { get; set; } = new List<Sw_Files_Entity>(); // 첨부 파일 목록 정보
        List<Check_Items_Entity> ann { get; set; } = new List<Check_Items_Entity>(); // 시설물 점검 사항 목록
        Check_Items_Entity bnn { get; set; } = new Check_Items_Entity(); // 시설물 점검 사항 정보
        List<Check_Object_Entity> onn { get; set; } = new List<Check_Object_Entity>(); // 시설물 점검 대상 목록
        List<Check_Cycle_Entity> cnn { get; set; } = new List<Check_Cycle_Entity>(); // 시설물 점검 주기 목록        

        public string User_Code { get; set; }
        public string User_Name { get; set; }
        public string Apt_Code { get; set; }
        public string Apt_Name { get; set; }
        public string UserName { get; set; }
        public string Views { get; set; } = "A"; //상세 열기
        public string RemoveViews { get; set; } = "A";//삭제 열기
        public string InsertViews { get; set; } = "A"; // 입력 열기
        public string FileInputViews { get; set; } = "A"; // 파일 첨부 열기
        public string FileViews { get; set; } = "A"; // 첨부 파일 보기
        public string Object_Code { get; set; } = "A"; // 점검 대상 코드
        public string Cycle_Code { get; set; } = "A"; // 검상 주기 코드
        public int LevelCount { get; set; } = 0;

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
        /// <returns></returns>
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

                if (LevelCount > 5)
                {
                    bnn.PostDate = DateTime.Now;
                    await DisplayData();
                    cnn = await check_Cycle_Lib.CheckCycle_Data_Index();
                    onn = await check_Object_Lib.CheckObject_Data_Index();
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
        /// 데이터 불러오기
        /// </summary>
        /// <returns></returns>
        private async Task DisplayData()
        {
            if (Cycle_Code == "A" || Object_Code == "A")
            {
                pager.RecordCount = await check_Items_Lib.CheckItems_Data_Count();
                ann = await check_Items_Lib.CheckItems_Index(pager.PageIndex);
            }
            else
            {
                pager.RecordCount = await check_Items_Lib.CheckItems_Data_Index_Count(Object_Code, Cycle_Code);
                ann = await check_Items_Lib.CheckItems_Page_Index(pager.PageIndex, Object_Code, Cycle_Code);
            }
        }

        /// <summary>
        /// 페이징
        /// </summary>
        /// <param name="pageIndex"></param>
        protected async void PageIndexChanged(int pageIndex)
        {
            pager.PageIndex = pageIndex;
            pager.PageNumber = pageIndex + 1;

            //i = 0;
            //ann = await defect_lib.GetList_Page(pager.PageIndex, Apt_Code);

            await DisplayData();

            StateHasChanged();
        }

        /// <summary>
        /// 점검 사항 새로 등록 열기
        /// </summary>
        private void OnInputbutton()
        {
            InsertViews = "B";
            bnn = new Check_Items_Entity();
            bnn.PostDate = DateTime.Now;

            StateHasChanged();
        }

        /// <summary>
        /// 점검사항 수정 열기
        /// </summary>
        /// <param name="_Items_Entity"></param>
        private void ByEdit(Check_Items_Entity _Items_Entity)
        {
            bnn = _Items_Entity;
            InsertViews = "B";
        }

        /// <summary>
        /// 삭제 토글
        /// </summary>
        /// <param name="Aid"></param>
        /// <returns></returns>
        private async Task ByRemove(int Aid)
        {
            bool isDelete = await JSRuntime.InvokeAsync<bool>("confirm", $"{Aid}번 점검 사항을 정말로 삭제하시겠습니까?");
            if (isDelete)
            {
                await check_Items_Lib.CheckItems_Date_Delete(Aid);
                await DisplayData();
                StateHasChanged();
            }
            else
            {
                await JSRuntime.InvokeAsync<object>("alert", "삭제되지 않았습니다.");
            }
            await DisplayData();
            StateHasChanged();
        }

        /// <summary>
        /// 점검 대상 선택 실행
        /// </summary>
        /// <param name="args"></param>
        private void onObject(ChangeEventArgs args)
        {
            bnn.Check_Object_Code = args.Value.ToString();
            StateHasChanged();
        }

        /// <summary>
        /// 점검 주기 선택 실행
        /// </summary>
        /// <param name="args"></param>
        private void onCycle(ChangeEventArgs args)
        {
            bnn.Check_Cycle_Code = args.Value.ToString();
            StateHasChanged();
        }

        /// <summary>
        /// 시설물 점검 사항 입력
        /// </summary>
        /// <returns></returns>
        public async Task btnSave()
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
            //bnn.ModifyDate = DateTime.Now;
            
            bnn.PostIP = myIPAddress;
            bnn.Del = "A";
            #endregion

            if (bnn.CheckItemsID > 0)
            {
                await check_Items_Lib.CheckItems_Data_Modify(bnn);
                await JSRuntime.InvokeAsync<object>("alert", "수정 되었습니다.");
            }
            else
            {
                bnn.Check_Items_Code = "itemCode" + await check_Items_Lib.CheckItems_Last();
                await check_Items_Lib.CheckItems_Date_Insert(bnn);
                await JSRuntime.InvokeAsync<object>("alert", "저장 되었습니다.");
            }
            InsertViews = "A";
            await DisplayData();

            StateHasChanged();
        }

        /// <summary>
        /// 점검사항 입력 닫기
        /// </summary>
        private void btnClose()
        {
            InsertViews = "A";
            StateHasChanged();
        }
    }
}
