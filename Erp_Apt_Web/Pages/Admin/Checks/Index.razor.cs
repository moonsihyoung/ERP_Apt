using Erp_Apt_Lib.Check;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Erp_Apt_Web.Pages.Admin.Checks
{
    public partial class Index
    {
        #region 변수
        public string User_Code { get; set; }
        public string User_Name { get; set; }
        public string Apt_Code { get; set; }
        public string Apt_Name { get; set; }
        public string UserName { get; set; }
        public int LevelCount { get; set; }
        public string Views { get; set; } = "A";
        public string InsertViews { get; set; } = "A";
        public string strView { get; set; }
        public string strTitle { get; set; }
        public string strCycle { get; set; }

        #endregion

        #region 로드
        List<Check_Card_Entity> ann { get; set; } = new List<Check_Card_Entity>();
        Check_Card_Entity bnn { get; set; } = new Check_Card_Entity();
        List<Check_Cycle_Entity> cyc = new List<Check_Cycle_Entity>();
        List<Check_Card_Entity> ccc { get; set; } = new List<Check_Card_Entity>();
        List<Check_Object_Entity> obj { get; set; } = new List<Check_Object_Entity>();


        public int intNum { get; private set; }

        //List<Check_Input_Entity> annO = new List<Check_Input_Entity>();
        //List<Check_Input_Entity> annC = new List<Check_Input_Entity>();

        protected DulPager.DulPagerBase pager = new DulPager.DulPagerBase()
        {
            PageNumber = 1,
            PageIndex = 0,
            PageSize = 15,
            PagerButtonCount = 5
        };
        #endregion

        #region 인스턴스
        [CascadingParameter] public Task<AuthenticationState> AuthenticationStateRef { get; set; }

        [Inject] public ICheck_Object_Lib check_Object { get; set; }
        [Inject] public ICheck_Cycle_Lib check_Cycle_Lib { get; set; }
        [Inject] public ICheck_Input_Lib check_Input_Lib { get; set; }
        [Inject] public ICheck_List_Lib check_List_Lib { get; set; }
        [Inject] public ICheck_Items_Lib check_Items_Lib { get; set; }
        [Inject] public ICheck_Card_Lib check_Card_Lib { get; set; }
        [Inject] public NavigationManager MyNav { get; set; }
        [Inject] IJSRuntime JSRuntime { get; set; }
        #endregion

        /// <summary>
        /// 처음 로드시에 실행
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

                if (LevelCount >= 10)
                {
                    await DisplayData();
                    obj = await check_Object.CheckObject_Data_Index();
                    cyc = await check_Cycle_Lib.CheckCycle_Data_Index();
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
        /// 모달 이동 
        /// </summary>
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await JSRuntime.InvokeVoidAsync("setModalDraggableAndResizable");
            await base.OnAfterRenderAsync(firstRender);
        }

        /// <summary>
        /// 시설물 대상 목록 정보
        /// </summary>
        private async Task DisplayData()
        {
            pager.RecordCount = await check_Card_Lib.CheckCard_Index_Apt_Count(Apt_Code);
            ann = await check_Card_Lib.CheckCard_Index_Apt(pager.PageIndex, Apt_Code);
            intNum = pager.RecordCount - (pager.PageIndex * pager.PageSize);                
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

        private void ByEdit(Check_Card_Entity ar)
        {
            bnn = ar;
        }

        private async Task ByRemove(int Aid)
        {
            bool isDelete = await JSRuntime.InvokeAsync<bool>("confirm", Aid + $"을 정말로 삭제하시겠습니까?");

            if (isDelete)
            {
                await check_Card_Lib.Remove(Aid);
                await DisplayData();
            }
            else
            {
                await JSRuntime.InvokeAsync<object>("alert", "취소되었습니다.");
            }            
        }

        /// <summary>
        /// 점검표 입력(등록)
        /// </summary>
        private void OnInsert()
        {            
            strTitle = "점검표 입력 및 수정 폼";
            InsertViews = "B";            
        }



        /// <summary>
        /// 등록 닫기
        /// </summary>
        private void btnClose()
        {
            InsertViews = "A";
        }

        /// <summary>
        /// 점검표 등록 및 수정
        /// </summary>
        private async Task btnSave()
        {
            bnn.AptCode = Apt_Code;
            bnn.Category = "A";
            bnn.Sort = "A";

            if (string.IsNullOrWhiteSpace(Apt_Code))
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "로그인하지 않았습니다.");
            }
            else if (string.IsNullOrWhiteSpace(bnn.Check_Card_Name))
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "점검표 명을 입력하지 않았습니다.");
            }
            else if (string.IsNullOrWhiteSpace(bnn.Check_Card_Code))
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "점검표 코드를 입력하지 않았습니다.");
            }
            else if (string.IsNullOrWhiteSpace(bnn.Check_Cycle_Code))
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "점검주기를 입력하지 않았습니다.");
            }
            else if (string.IsNullOrWhiteSpace(bnn.Check_Object_Code))
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "점검대상를 입력하지 않았습니다.");
            }
            else if (string.IsNullOrWhiteSpace(bnn.Check_Card_Details))
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "점검표 상세를 입력하지 않았습니다.");
            }
            else
            {
                bnn.Del = "A";

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

                if (bnn.CheckCardID < 1)
                {
                    await check_Card_Lib.CheckCard_Date_Insert(bnn);
                }
                else
                {
                    await check_Card_Lib.CheckCard_Data_Modify(bnn);
                }
                bnn = new Check_Card_Entity();
                strCycle = "";
                InsertViews = "A";
                await DisplayData();
            }
        }

        /// <summary>
        /// 점검대상 선택 실행
        /// </summary>
        private async Task OnCycle(ChangeEventArgs a)
        {
            bnn.Check_Cycle_Code = a.Value.ToString();
            strCycle = bnn.Check_Cycle_Code;
            int intRe = await check_Items_Lib.CheckItems_View_Data_Count(bnn.Check_Object_Code, bnn.Check_Cycle_Code);
            if (string.IsNullOrWhiteSpace(bnn.Check_Cycle_Code) && string.IsNullOrWhiteSpace(bnn.Check_Object_Code))
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "점검대상 및 점검주기를 선택하지 않았습니다.");
            }
            else
            {
                string objectName = await check_Object.CheckObject_Data_Name_Async(bnn.Check_Object_Code);
                string cycleName = await check_Cycle_Lib.CheckCycle_Data_Name(bnn.Check_Cycle_Code);
                if (intRe > 0)
                {                    
                    bnn.Check_Card_Name = objectName + "-" + cycleName;
                    bnn.Check_Card_Details = bnn.Check_Card_Name;
                    bnn.Check_Items_Code = intRe.ToString();
                    int re = await check_Card_Lib.CheckCard_Data_Count();
                    bnn.Check_Card_Code = "cc" + re;
                }
                else
                {
                    bnn.Check_Object_Code = "";
                    bnn.Check_Card_Code = "";
                    bnn.Check_Cycle_Code = "";
                    bnn.Check_Card_Name = "";
                    strCycle = "";
                    bnn.Check_Card_Details = "";
                    await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", objectName + "-" + cycleName + "의 점검 내용이 없어, 선택할 수 없습니다.");
                }
            }
        }
    }
}
