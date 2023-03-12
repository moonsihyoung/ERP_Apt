using Erp_Apt_Lib.Check;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Erp_Apt_Web.Pages.Check.Input
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

        #endregion

        #region 로드
        public Check_Input_Entity vnn { get; set; } = new Check_Input_Entity();
        List<Check_Object_Entity> bnn = new List<Check_Object_Entity>();
        List<Check_Cycle_Entity> cnn = new List<Check_Cycle_Entity>();
        List<Check_Input_Entity> ann = new List<Check_Input_Entity>();

        public int intNum { get; private set; }

        List<Check_Input_Entity> annO = new List<Check_Input_Entity>();
        List<Check_Input_Entity> annC = new List<Check_Input_Entity>();

        protected DulPager.DulPagerBase pager = new DulPager.DulPagerBase()
        {
            PageNumber = 1,
            PageIndex = 0,
            PageSize = 15,
            PagerButtonCount = 5
        };
        #endregion

        #region 인스턴스
        [CascadingParameter]
        public Task<AuthenticationState> AuthenticationStateRef { get; set; }

        [Inject] public ICheck_Object_Lib check_Object { get; set; }
        [Inject] public ICheck_Cycle_Lib check_Cycle_Lib { get; set; }
        [Inject] public ICheck_Input_Lib check_Input_Lib { get; set; }
        [Inject] public ICheck_List_Lib check_List_Lib { get; set; }
        [Inject] public ICheck_Items_Lib check_Items_Lib { get; set; }
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

                if (LevelCount >= 3)
                {
                    await DisplayData();

                    //ann.PostDate = DateTime.Now;
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
        /// <returns></returns>
        private async Task DisplayData()
        {
            pager.RecordCount = await check_Input_Lib.CheckInput_Data_Index_All_Count(Apt_Code);
            ann = await check_Input_Lib.CheckInput_Data_Index_Page_All_new(pager.PageIndex, Apt_Code);
            intNum = pager.RecordCount - (pager.PageIndex * pager.PageSize);
            //StateHasChanged();
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
        /// 상세보기
        /// </summary>
        /// <param name="input_Entity"></param>
        public void ByAid(int Aid)
        {
            MyNav.NavigateTo("/Check/Input/Views/" + Aid);
        }

        /// <summary>
        /// 삭제하기
        /// </summary>
        /// <param name="Aid"></param>
        public async Task ByRemove(int Aid)
        {
            bool isDelete = await JSRuntime.InvokeAsync<bool>("confirm", Aid + $"을 정말로 삭제하시겠습니까?");

            if (isDelete)
            {
                await check_Input_Lib.CheckInput_Date_Delete(Aid);
                await check_List_Lib.CheckList_Date_Remove_All(Aid);
                await DisplayData();
            }
            else
            {
                await JSRuntime.InvokeAsync<object>("alert", "취소되었습니다.");
            }
        }

        /// <summary>
        /// 입력 새로열기
        /// </summary>
        public void OnInputbutton()
        {
            MyNav.NavigateTo("/Check/Input/Input");

        }

        #region 점검대상 명 출력 메서드
        public string FuncObject(object objObjectCode)
        {

            string strView;
            strView = check_Object.CheckObject_Data_Name(objObjectCode.ToString());
            return strView;
        }
        #endregion


        #region 점검주기 명 출력 메서드
        public string FuncCycle(object objCycleCode)
        {
            string strView;
            strView = check_Cycle_Lib.CheckCycle_Name(objCycleCode.ToString());
            return strView;
        }
        #endregion

        #region 점검수와 점검된 수 출력 메서드
        public string FuncItems(object objObjectCode, object objCycleCode, object objYear, object objMonth, object objDay, string AptCode)
        {
            string strItems = check_Items_Lib.CheckItems_View_Data_Count(objObjectCode.ToString(), objCycleCode.ToString()).ToString();
            //string strView;
            string strObjectCode = objObjectCode.ToString();

            string strCycleCode = objCycleCode.ToString();
            string strMonth = objMonth.ToString();
            int intMonth = Convert.ToInt32(objMonth);
            string strYear = DateTime.Now.Year.ToString();
            if (strCycleCode == "Cyc1")
            {
                strView = check_List_Lib.CheckList_Data_CardView_Year_Month_Day(objObjectCode.ToString(), objCycleCode.ToString(), objYear.ToString(), objMonth.ToString(), objDay.ToString(), AptCode).ToString();
            }
            else if (strCycleCode == "Cyc2")
            {
                #region 요일 추출 메서드
                string strWeek = DateTime.Now.DayOfWeek.ToString();  //이번주 요일
                int intWeek = 0;
                intWeek = 7;
                //}
                string strTotal_Day = DateTime.Now.ToShortDateString();
                #endregion
                strView = check_List_Lib.CheckList_Data_CardView_Week(strObjectCode, strCycleCode, strYear, intMonth, intWeek, strTotal_Day, AptCode).ToString();
                //Label1.Text = strView + " =" + strObjectCode + " =" + strCycleCode + " =" + strYear + " =" + intMonth.ToString() + " =" + intWeek.ToString() + " =" + strTotal_Day + " =" + AptCode;
            }
            else if (strCycleCode == "Cyc3")
            {
                strView = check_List_Lib.CheckList_Data_CardView_Year_Month(strObjectCode, strCycleCode, strYear, strMonth, AptCode).ToString();
            }
            else if (strCycleCode == "Cyc4")
            {
                strView = check_List_Lib.CheckList_Data_CardView_Quarter(strObjectCode, strCycleCode, strYear, intMonth, AptCode).ToString();
            }
            else if (strCycleCode == "Cyc5")
            {
                strView = check_List_Lib.CheckList_Data_CardView_Half(strObjectCode, strCycleCode, strYear, intMonth, AptCode).ToString();
            }
            else if (strCycleCode == "Cyc6")
            {
                strView = check_List_Lib.CheckList_Data_CardView_Year(strObjectCode, strCycleCode, strYear, AptCode).ToString();
            }
            else
            {
                //string strJs = @"
                //    <script>
                //    alert('잘못된 입력값 입니다.');
                //    </script>";
                //this.ClientScript.RegisterClientScriptBlock(this.GetType(), "gogo", strJs);
            }
            return strItems + " - " + strView;
        }
        #endregion

        #region 점검자 명 출력 메서드
        public string FuncName(object objObjectCode, object objCycleCode, object objYear, object objMonth, object objDay, string AptCode)
        {
            string strView;
            Check_List_Entity ann = check_List_Lib.CheckList_Data_Input_A(objObjectCode.ToString(), objCycleCode.ToString(), objYear.ToString(), objMonth.ToString(), objDay.ToString(), AptCode);
            strView = ann.UserName;
            //strView = objObjectCode.ToString() + objCycleCode.ToString() + AptCode;
            return strView;
        }
        #endregion

        #region 점검부서 명 출력 메서드
        public string FuncPost(object objObjectCode, object objCycleCode, object objYear, object objMonth, object objDay, string AptCode)
        {
            string strView;
            Check_List_Entity ann = check_List_Lib.CheckList_Data_Input_A(objObjectCode.ToString(), objCycleCode.ToString(), objYear.ToString(), objMonth.ToString(), objDay.ToString(), AptCode);
            strView = ann.UserPost + ann.UserDuty;
            return strView;
        }
        #endregion
    }
}
