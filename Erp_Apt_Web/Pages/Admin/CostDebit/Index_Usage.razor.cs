using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http;
using Microsoft.JSInterop;
using System.Threading.Tasks;
using Erp_Apt_Lib.MaintenanceCost;
using System.Collections.Generic;
using System.Linq;
using System;
using Erp_Apt_Lib.MonthlyUsage;
using Erp_Apt_Lib;
using System.Runtime.InteropServices;

namespace Erp_Apt_Web.Pages.Admin.CostDebit
{
    public partial class Index_Usage
    {
        [Parameter] public string Code { get; set; }
        [CascadingParameter] public Task<AuthenticationState> AuthenticationStateRef { get; set; }
        [Inject] public NavigationManager MyNav { get; set; }
        [Inject] IJSRuntime JSRuntime { get; set; }
        [Inject] IHttpContextAccessor contextAccessor { get; set; }
        [Inject] IMonthlyUsage_Lib monthlyUsage_Lib { get; set; }
        [Inject] IUsageDetails_Lib usageDetails_Lib { get; set; }

        List<MonthlyUsage_Entity> ann { get; set; } = new List<MonthlyUsage_Entity>();
        List<UsageDetails_Entity> lst { get; set; } = new List<UsageDetails_Entity>();
        MonthlyUsage_Entity bnn { get; set; } = new MonthlyUsage_Entity();
        MonthlyUsage_Entity dnn { get; set; } = new MonthlyUsage_Entity();
        UsageDetails_Entity vnn { get; set; } = new UsageDetails_Entity();
        List<UsageDetails_Entity> vnnA { get; set; } = new List<UsageDetails_Entity>();
        List<UsageDetails_Entity> vnnB { get; set; } = new List<UsageDetails_Entity>();
        List<UsageDetails_Entity> vnnC { get; set; } = new List<UsageDetails_Entity>();
        List<UsageDetails_Entity> vnnD { get; set; } = new List<UsageDetails_Entity>();

        public string strTitle { get; private set; }

        public string User_Code { get; set; }
        public string User_Name { get; set; }
        public int LevelCount { get; private set; }
        public string Apt_Code { get; set; }
        public string Apt_Name { get; set; }

        public string InsertViews { get; set; } = "A";
        public string InsertDetails { get; set; } = "A";
        public string SortViews { get; set; } = "A";
        public string Views { get; set; } = "A";
        public string strSort { get; private set; }


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

                if (LevelCount > 5)
                {
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
        /// 입력된 데이터 로드
        /// </summary>
        private async Task DisplayData()
        {
            if (strSort == "B")
            {

            }
            else
            {
                pager.RecordCount = await monthlyUsage_Lib.GetList_Count(Apt_Code);
                ann = await monthlyUsage_Lib.GetList(pager.PageIndex, Apt_Code);
            }
        }

        /// <summary>
        /// 아이피 추출 
        /// </summary>
        public string MyIpAdress { get; set; }
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await JSRuntime.InvokeVoidAsync("setModalDraggableAndResizable");
            await base.OnAfterRenderAsync(firstRender);

            if (firstRender)
            {
                MyIpAdress = contextAccessor.HttpContext.Connection?.RemoteIpAddress.ToString();
                StateHasChanged();
            }
        }

        /// <summary>
        /// 상세보기
        /// </summary>
        private async Task SelectView(MonthlyUsage_Entity e)
        {
            bnn = e;
            lst = await usageDetails_Lib.GetList(Apt_Code, bnn.intYear, bnn.intMonth);
            strTitle = Apt_Name + " " + bnn.intYear + "년 " + bnn.intMonth + " 월 사용료 정보";

            Electric_Views = "A";
            Water_Views = "A";
            Hoting_Views = "A";
            Heating_View = "A";
            Views = "B";
        }

        /// <summary>
        /// 수정 열기
        /// </summary>
        private void btnEdit(MonthlyUsage_Entity ar)
        {
            bnn = ar;
            strTitle = bnn.Apt_Name + " " + bnn.intYear + "년 " + bnn.intMonth + "월 사용량 정보 수정";
            InsertViews = "B";
        }

        /// <summary>
        /// 삭제
        /// </summary>
        private async Task btnRemove(int Aid)
        {
            bool isDelete = await JSRuntime.InvokeAsync<bool>("confirm", $"{Aid} 문서를 정말로 삭제하시겠습니까?");

            if (isDelete)
            {
                await monthlyUsage_Lib.Remove(Aid);
                await DisplayData();
                ///Views = "A";
            }
        }

        private async Task btnOpenA()
        {
            bnn = new MonthlyUsage_Entity();
            bnn.intYear= DateTime.Now.Year;
            bnn.intMonth= DateTime.Now.Month -1;

            bnn.UseDate = Convert.ToDateTime(bnn.intYear + "-" + bnn.intMonth + "-01");

            int intRe = 0;
            try
            {
                intRe = await monthlyUsage_Lib.GetById_Being(Apt_Code);
            }
            catch (Exception)
            {
                intRe= 0;
            }
            if (intRe > 0)
            {
                dnn = await monthlyUsage_Lib.GetDetails(intRe);
            }

            bnn.codeHeatNm = dnn.codeHeatNm;
            bnn.ElectricContractMethod = dnn.ElectricContractMethod;
            bnn.ElectricInpose = dnn.ElectricInpose;            

            strTitle = "사용량 정보 입력";
            InsertViews = "B";
        }

        /// <summary>
        /// 입력 열기
        /// </summary>
        private void btnOpenB()
        {
            InsertViews = "B";
        }

        /// <summary>
        /// 저장
        /// </summary>
        private async Task btnSave()
        {
            bnn.PostIp = MyIpAdress;
            bnn.UserCode = User_Code;
            bnn.Apt_Code = Apt_Code;
            bnn.Apt_Name = Apt_Name;

            if (string.IsNullOrWhiteSpace(bnn.Apt_Code))
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "공동주택 식별코드가 없습니다..");
            }
            else if (bnn.intYear < 2010 || bnn.intYear == 0)
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "년도가 선택되지 않았습니다..");
            }
            else if (bnn.intMonth <= 0)
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "해당 월이 선택되지 않았습니다..");
            }
            else if (string.IsNullOrWhiteSpace(bnn.ElectricContractMethod))
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "전기 계약 방식이 선택되지 않았습니다..");
            }
            else if (string.IsNullOrWhiteSpace(bnn.ElectricInpose))
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "전기 부과방식이 선택되지 않았습니다..");
            }
            else if (string.IsNullOrWhiteSpace(bnn.codeHeatNm))
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "난방방식이 선택되지 않았습니다..");
            }
            else if (bnn.ElectricPerUsage < 10)
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "세대 전기 사용량이 입력되지 않았습니다..");
            }            
            else if (bnn.ElectricAllUsage < 10)
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "전기 전체 사용량이 입력되지 않았습니다..");
            }
            else if (bnn.WaterAllUsage < 10)
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "수도 전체 사용량이 입력되지 않았습니다..");
            }
            else
            {
                if (bnn.Aid < 1)
                {
                    await monthlyUsage_Lib.Add(bnn);
                }
                else
                {
                    await monthlyUsage_Lib.Edit(bnn);
                }

                bnn = new MonthlyUsage_Entity();
                await DisplayData();
                InsertViews = "A";
            }
        }
        
        private async Task onMonthAsync(ChangeEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(e.Value.ToString()))
            {
                bnn.intMonth = Convert.ToInt32(e.Value);
                if (bnn.intYear < 2010 || bnn.intYear == 0)
                {
                    await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "년도가 선택되지 않았습니다..");
                }
                else if (bnn.intMonth <= 0)
                {
                    await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "해당 월이 선택되지 않았습니다..");
                }
                else
                {
                    bnn.UseDate = Convert.ToDateTime(bnn.intYear + "-" + bnn.intMonth + "-01");
                }             
            }
        }

        /// <summary>
        /// 세대 사용량 입력 시에 공동사용량 계산
        /// </summary>
        private async Task onUsage(ChangeEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(e.Value.ToString()))
            {
                bnn.ElectricPerUsage = Convert.ToInt32(e.Value);
                if (bnn.ElectricAllUsage > 0)
                {
                    bnn.ElectricComUsage = bnn.ElectricAllUsage - bnn.ElectricPerUsage;
                }
                else
                {
                    await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "전기 전체 사용량이 입력되지 않았습니다..");
                }
            }
        }

        /// <summary>
        /// 세대 사용료 입력 시 공용 사용료 자동 입력
        /// </summary>
        private async Task onUsageFee(ChangeEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(e.Value.ToString()))
            {
                bnn.ElectricPerFee = Convert.ToInt32(e.Value);
                if (bnn.ElectricAllFee > 0)
                {
                    bnn.ElectricComFee = bnn.ElectricAllFee - bnn.ElectricPerFee;
                }
                else
                {
                    await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "전기 전체 사용료이 입력되지 않았습니다..");
                }
            }
        }

        private async Task onWatering(ChangeEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(e.Value.ToString()))
            {
                bnn.WaterPerUsage = Convert.ToInt32(e.Value);
                if (bnn.WaterAllUsage > 0)
                {
                    bnn.WaterComUsage = bnn.WaterAllUsage - bnn.WaterPerUsage;
                }
                else
                {
                    await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "전기 전체 사용량을 입력되지 않았습니다..");
                }
            }
        }

        /// <summary>
        /// 세대 수도료 입력시 공용 수도료 자동 입력
        /// </summary>
        private async Task onWateringFee(ChangeEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(e.Value.ToString()))
            {
                bnn.WaterPerFee = Convert.ToInt32(e.Value);
                if (bnn.WaterAllFee > 0)
                {
                    bnn.WaterComFee = bnn.WaterAllFee - bnn.WaterPerFee;
                }
                else
                {
                    await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "전기 전체 사용료을 입력되지 않았습니다..");
                }
            }
        }

        /// <summary>
        /// 급탕 세대 사용량 입력 급탕 공동 사용량 자동 입력
        /// </summary>
        private async Task onHotWaterUsage(ChangeEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(e.Value.ToString()))
            {
                bnn.HeatingPerUsage = Convert.ToInt32(e.Value);
                if (bnn.HotWaterAllUsage > 0)
                {
                    bnn.HeatingComUsage = bnn.HeatingAllUsage - bnn.HeatingPerUsage;
                }
                else
                {
                    await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "급탕 전체 사용량을 입력되지 않았습니다..");
                }
            }
        }

        /// <summary>
        /// 급탕 세대 사용료 입력 급탕 공동 사용료 자동 입력
        /// </summary>
        private async Task onHotWaterUsageFee(ChangeEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(e.Value.ToString()))
            {
                bnn.HotWaterAllFee = Convert.ToInt32(e.Value);
                if (bnn.HotWaterAllFee > 0)
                {
                    bnn.HotWaterComFee = bnn.HotWaterAllFee - bnn.HotWaterPerFee;
                }
                else
                {
                    await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "전기 전체 사용료을 입력되지 않았습니다..");
                }
            }            
        }

        /// <summary>
        /// 난방 세대 사용량 입력 시 공동 사용량 자동 입력
        /// </summary>
        private async Task onHeatingUsage(ChangeEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(e.Value.ToString()))
            {
                bnn.HeatingPerUsage = Convert.ToInt32(e.Value);
                if (bnn.HeatingAllUsage > 0)
                {
                    bnn.HeatingComUsage = bnn.HeatingAllUsage - bnn.HeatingPerUsage;
                }
                else
                {
                    await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "난바 전체 사용량을 입력되지 않았습니다..");
                }
            }
        }

        /// <summary>
        /// 난방 세대 사용료 입력 시 공동 사용료 자동 입력
        /// </summary>
        private async Task onHeatingUsageFee(ChangeEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(e.Value.ToString()))
            {
                bnn.HeatingPerFee = Convert.ToInt32(e.Value);
                if (bnn.HeatingAllFee > 0)
                {
                    bnn.HeatingComFee = bnn.HeatingAllFee - bnn.HeatingPerFee;
                }
                else
                {
                    await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "난바 전체 사용량을 입력되지 않았습니다..");
                }
            }
        }

        public string strTitleA { get; set; }

        private void btnClose()
        {
            InsertViews= "A";
        }
        private void btnCloseA()
        {
            Views = "A";
        }

        private void btnCloseB()
        {
            InsertDetails = "A";
        }

        private async Task btnOpen()
        {
            if (bnn.Aid > 0)
            {
                vnn.intYear = bnn.intYear;
                vnn.intMonth = bnn.intMonth;
                vnn.UsageDate = bnn.UseDate;
                vnn.PostIp = MyIpAdress;
                vnn.UserCode = User_Code;
                vnn.Apt_Code = Apt_Code;
                vnn.Apt_Name = Apt_Name;
                strTitleA = "사용량 상세 정보 입력";
                InsertDetails= "B";
            }
            else
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "식별코드가 없습니다..");
            }
        }

        /// <summary>
        /// 각 사용량 상세 정보 목록
        /// </summary>
        private async Task btnSaveB()
        {
            if (string.IsNullOrWhiteSpace(vnn.Apt_Code))
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "공동주택 식별코드가 없습니다..");
            }
            else if (vnn.intYear < 1)
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "년도가 없습니다..");
            }
            else if (vnn.intMonth < 1)
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "해당 월이 없습니다..");
            }
            else if (string.IsNullOrWhiteSpace(vnn.Division))
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "구분이 선택되지 않았습니다..");
            }
            else if (string.IsNullOrWhiteSpace(vnn.UseName))
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "상세 구분이 입력되지 않았습니다..");
            }
            else if (vnn.CostSum < 1)
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "사용료를 입력하지 않았습니다..");
            }
            else if (vnn.Usage < 1)
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "사용량을 입력하지 않습니다..");
            }
            else
            {
                if (vnn.Aid < 1)
                {
                    await usageDetails_Lib.Add(vnn);
                }
                else
                {
                    await usageDetails_Lib.Edit(vnn);
                }
                vnn = new UsageDetails_Entity();
                InsertDetails = "A";
                await DisplayData();
            }           
        }

        /// <summary>
        /// 전기료 상세 목록
        /// </summary>
        public string Electric_Views { get; set; } = "A";
        public string Water_Views { get; set; } = "A";
        public string Hoting_Views { get; set; } = "A";
        public string Heating_View { get; set; } = "A";
        private async Task DetailListViewA(MonthlyUsage_Entity ar)
        {
            if (Electric_Views == "A")
            {
                vnnA = await usageDetails_Lib.GetList_sort(Apt_Code, ar.intYear, ar.intMonth, "전기료");
                Electric_Views = "B";
                Water_Views = "A";
                Hoting_Views = "A";
                Heating_View = "A";
            }
            else
            {
                Electric_Views = "A";
                Water_Views = "A";
                Hoting_Views = "A";
                Heating_View = "A";
            }
        }

        /// <summary>
        /// 수도료 상세 목록
        /// </summary>
        private async Task DetailListViewB(MonthlyUsage_Entity ar)
        {
            if (Water_Views == "A")
            {
                vnnB = await usageDetails_Lib.GetList_sort(Apt_Code, ar.intYear, ar.intMonth, "수도료");
                Electric_Views = "A";
                Water_Views = "B";
                Hoting_Views = "A";
                Heating_View = "A";
            }
            else
            {
                Electric_Views = "A";
                Water_Views = "A";
                Hoting_Views = "A";
                Heating_View = "A";
            }
        }

        /// <summary>
        /// 급탕료 상세 목록
        /// </summary>
        private async Task DetailListViewC(MonthlyUsage_Entity ar)
        {
            if (Hoting_Views == "A")
            {
                vnnC = await usageDetails_Lib.GetList_sort(Apt_Code, ar.intYear, ar.intMonth, "급탕료");
                Electric_Views = "A";
                Water_Views = "A";
                Hoting_Views = "B";
                Heating_View = "A";
            }
            else
            {
                Electric_Views = "A";
                Water_Views = "A";
                Hoting_Views = "A";
                Heating_View = "A";
            }
        }

        /// <summary>
        /// 난방료 상세 목록
        /// </summary>
        private async Task DetailListViewD(MonthlyUsage_Entity ar)
        {
            if (Heating_View == "A")
            {
                vnnD = await usageDetails_Lib.GetList_sort(Apt_Code, ar.intYear, ar.intMonth, "난방료");
                Electric_Views = "A";
                Water_Views = "A";
                Hoting_Views = "A";
                Heating_View = "B";
            }
            else
            {
                Electric_Views = "A";
                Water_Views = "A";
                Hoting_Views = "A";
                Heating_View = "A";
            }
        }
    }
}
