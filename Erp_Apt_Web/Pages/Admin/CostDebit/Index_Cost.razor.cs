using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Erp_Apt_Lib.Plans;
using System.Collections.Generic;
using Erp_Apt_Lib.MaintenanceCost;
using System;
using Erp_Lib;
using Facilities;
using System.Drawing;
using System.Linq;
using Wedew_Lib;
using System.Net;
using Erp_Apt_Lib.Community;
using Erp_Entity;
using System.Drawing.Printing;

namespace Erp_Apt_Web.Pages.Admin.CostDebit
{
    public partial class Index_Cost
    {
        [Parameter] public string Code { get; set; }
        [CascadingParameter] public Task<AuthenticationState> AuthenticationStateRef { get; set; }
        [Inject] public NavigationManager MyNav { get; set; }
        [Inject] IJSRuntime JSRuntime { get; set; }
        [Inject] IHttpContextAccessor contextAccessor { get; set; }
        [Inject] ICostDebit_Lib costDebit_Lib { get; set; }
        [Inject] ICommunity_Lib community_Lib { get; set; }
        [Inject] IErp_AptPeople_Lib erp_AptPeople_Lib { get; set; }

        List<CostDebit_Entity> ann { get; set; } = new List<CostDebit_Entity>();
        CostDebit_Entity bnn { get; set; } = new CostDebit_Entity();
        CostDebit_Entity cnn { get; set; } = new CostDebit_Entity();
        List<Community_Entity> lst { get; set; }
        List<Apt_People_Entity> dnn { get; set; } = new List<Apt_People_Entity>();
        List<Apt_People_Entity> fnn { get; set; } = new List<Apt_People_Entity>();


        public string User_Code { get; set; }
        public string User_Name { get; set; }
        public int LevelCount { get; private set; }
        public string Apt_Code { get; set; }
        public string Apt_Name { get; set; }
        public string strTitle { get; set; }
        public string InsertViews { get; set; } = "A";
        public string SortViews { get; set; } = "A";
        public string Views { get; set; } = "A";
        public int lastDay { get; private set; }


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

        private async Task DisplayData()
        {
            if (strSort == "B")
            {
                pager.RecordCount = await costDebit_Lib.GetList_Apt_dongho_Count(Apt_Code, strDong, strHo);
                ann = await costDebit_Lib.GetList_Apt_dongho(pager.PageIndex, Apt_Code, strDong, strHo);
            }
            else
            {
                pager.RecordCount = await costDebit_Lib.GetList_Apt_Count(Apt_Code);
                ann = await costDebit_Lib.GetList_Apt(pager.PageIndex, Apt_Code);
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
        /// 주민공동시설 운영비
        /// </summary>
        public string strPlus { get; set; } = "A";
        private void onPlus()
        {
            if (strPlus == "A")
            {
                strPlus = "B";
            }
            else
            {
                strPlus = "A";
            }
        }

        public int re1 { get; set; } = 0;
        public int re2 { get; set; } = 0;
        public int re3 { get; set; } = 0;
        public string Mon { get; set; }
        public string strC { get; set; }
        private async Task datetimeView(string Dong, string Ho, string Month)
        {
            Mon = Month.Substring(4);
            int m = Convert.ToInt32(Mon);
            strC = Month.Substring(0, 4);
            int mm = Convert.ToInt32(strC);
            m = m - 1;
            if (m < 1)
            {
                m = 12;
                mm = m - 1;
               Mon = mm.ToString();
            }

            if (m < 10) 
            {
                Mon = "0" + m.ToString();
            }
            else
            {
                Mon= m.ToString();
            }

            
            MonthA = strC + Mon;
            re1= await costDebit_Lib.GetBy_be(Apt_Code, Dong, Ho, MonthA);
        }

        /// <summary>
        /// 해당월
        /// </summary>
        private string strMonth(string month)
        {
            string Year;
            Year = month.Insert(4, "년");
            month = Year.Insert(7, "월");
            return month;
        }

        /// <summary>
        /// 면적
        /// </summary>
        /// <param name="area"></param>
        /// <returns></returns>
        private string strArea(double area)
        {
            string strR = Math.Round(area, 2).ToString();
            return strR;
        }

        /// <summary>
        /// 상세보기
        /// </summary>
        private async Task SelectView(CostDebit_Entity e)
        {
            bnn = e;
            await datetimeView(bnn.dong, bnn.ho, bnn.Month);
            if (re1 > 0)
            {
                cnn = await costDebit_Lib.GetBy(Apt_Code, bnn.dong, bnn.ho, MonthA);
            }
            strTitle = strMonth(bnn.Month) + " " + bnn.dong + "동 " + bnn.ho + "호 세대 관리비 정보";
            Views = "B";

            int r1 = Convert.ToInt32(strC);
            int r2 = Convert.ToInt32(Mon);
            //r1 = r1+1; 
            //r2 = r2+1;
            lastDay = DateTime.DaysInMonth(r1, r2);
            string date1 = r1 + "-" + r2 + "-01";
            string date2 = r1 + "-" + r2 + "-" + lastDay.ToString() + " 23:59:59.993";
            lst = await community_Lib.GetListDongHoDate(bnn.Apt_Code, bnn.dong, bnn.ho, date1, date2);
        }

        /// <summary>
        /// 상세보기 모달 닫기
        /// </summary>
        private void btnClose()
        {
            Views= "A";
        }

        /// <summary>
        /// 초기화
        /// </summary>
        private async Task btnOpenA()
        {
            strSort= "A";
            await DisplayData();
        }

        /// <summary>
        /// 찾기
        /// </summary>
        public string strSort { get; set; } = "A";
        public string strSearch { get; set; } = "A";
        public string MonthA { get; private set; }
        public string MonthB { get; private set; }
        public string MonthC { get; private set; }

        /// <summary>
        /// 검색 열기
        /// </summary>
        public string strDong { get; set; }
        public string strHo { get; set; }
        private async Task btnOpenB()
        {
            strSearch = "B";
            dnn = await erp_AptPeople_Lib.DongList(Apt_Code);
        }

        /// <summary>
        /// 동 선택
        /// </summary>
        private async Task OnDong(ChangeEventArgs e)
        {
             strDong = e.Value.ToString();
            fnn = await erp_AptPeople_Lib.DongHoList_new(Apt_Code, strDong);
        }

        private async Task OnHo(ChangeEventArgs e)
        {
            strHo= e.Value.ToString();
            if (!string.IsNullOrWhiteSpace(strHo))
            {
                strSort = "B";
                await DisplayData();
            }
            else
            {
                strSort = "A";
                await DisplayData();
            }
        }

        /// <summary>
        /// 검색 닫기
        /// </summary>
        private void btnCloseS()
        {
            strSearch= "A";
            //strSort = "A";
        }
    }
}
