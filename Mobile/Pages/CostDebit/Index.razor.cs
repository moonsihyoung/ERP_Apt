using Erp_Apt_Lib.Community;
using Erp_Apt_Lib.MaintenanceCost;
using Erp_Lib;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Mobile.Pages.CostDebit
{
    public partial class Index
    {
        [CascadingParameter] public Task<AuthenticationState> AuthenticationStateRef { get; set; }
        [Inject] IJSRuntime JSRuntime { get; set; }
        [Inject] IApt_Lib apt_Lib { get; set; }
        [Inject] public NavigationManager MyNav { get; set; } // Url 
        [Inject] ICostDebit_Lib costDebit_Lib { get; set; }
        [Inject] IHttpContextAccessor contextAccessor { get; set; }
        [Inject] public ICommunity_Lib community_Lib { get; set; }

        Community_Entity bnn { get; set; } = new Community_Entity();
        List<Community_Entity> list { get; set; } = new List<Community_Entity>();
        CostDebit_Entity dnn { get; set; } = new CostDebit_Entity();
        CostDebit_Entity cnn { get; set; } = new CostDebit_Entity();

        public string User_Code { get; set; }
        public string User_Name { get; set; }
        public string Apt_Code { get; set; }
        public string Apt_Name { get; set; }
        public string Dong { get; set; }
        public string Ho { get; set; }
        public string ViewsA { get; set; } = "A";
        public string ViewsB { get; set; } = "A";
        public string MonthA { get; set; }
        public string MonthB { get; set; }

        public string MonthC { get; set; }
        public string Year { get; set; }

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
                Dong = authState.User.Claims.FirstOrDefault(c => c.Type == "Dong")?.Value;
                Ho = authState.User.Claims.FirstOrDefault(c => c.Type == "Ho")?.Value;

                int intMonth = 0;
                int intYear = DateTime.Now.Year;

                string dt1 = "";
                string dt2 = "";
                string dt21 = "";
                string dt22 = "";
                int lastDay = 0;

                intMonth = (DateTime.Now.Month) - 1;

                // 전월 만들기
                if (intMonth < 1)
                {
                    intMonth = 12;
                    intYear = intYear - 1;
                }
                if (intMonth < 10)
                {
                    MonthA = intYear.ToString() + "0" + intMonth.ToString();

                    lastDay = DateTime.DaysInMonth(intYear, intMonth);
                    dt1 = intYear.ToString() + "0" + intMonth.ToString() + "01";
                    dt2 = intYear.ToString() + "0" + intMonth.ToString() + lastDay + " 23:59:59.993";

                }
                else
                {
                    MonthA = intYear.ToString() + intMonth.ToString();

                    lastDay = DateTime.DaysInMonth(intYear, intMonth);
                    dt1 = intYear.ToString() + intMonth.ToString() + "01";
                    dt2 = intYear.ToString() + intMonth.ToString() + lastDay + " 23:59:59.993";
                }
                int re1 = await costDebit_Lib.GetBy_be(Apt_Code, Dong, Ho, MonthA); //전월 데이터 존재 여부


                ///전전월 만들기
                int intYearB = 0;
                int intMonthA = 0;


                intMonthA = (intMonth - 1);
                if (intMonthA < 1)
                {
                    intMonthA = 12;
                    intYearB = intYear - 1;
                }
                else
                {
                    intYearB = intYear;
                }
                if (intMonthA < 10)
                {
                    MonthB = intYearB.ToString() + "0" + intMonthA.ToString();

                    lastDay = DateTime.DaysInMonth(intYearB, intMonthA);
                    dt21 = intYearB.ToString() + "0" + intMonthA.ToString() + "01";
                    dt22 = intYearB.ToString() + "0" + intMonthA.ToString() + lastDay + " 23:59:59.993";
                }
                else
                {
                    MonthB = intYearB.ToString() + intMonthA.ToString();

                    lastDay = DateTime.DaysInMonth(intYearB, intMonthA);
                    dt21 = intYearB.ToString() + intMonthA.ToString() + "01";
                    dt22 = intYearB.ToString() + intMonthA.ToString() + lastDay + " 23:59:59.993";
                }
                int re2 = await costDebit_Lib.GetBy_be(Apt_Code, Dong, Ho, MonthB); //전월 데이터 존재 여부

                if (re1 > 0)
                {
                    dnn = await costDebit_Lib.GetBy(Apt_Code, Dong, Ho, MonthA);
                    await datetimeView(dnn.dong, dnn.ho, dnn.Month);
                    Year = dnn.Month.Insert(4, "년");
                    dnn.Month = Year.Insert(7, "월");
                    list = await community_Lib.GetListDongHoDate(Apt_Code, Dong, Ho, dt1, dt2);
                }
                else if (re2 > 0)
                {
                    dnn = await costDebit_Lib.GetBy(Apt_Code, Dong, Ho, MonthB);
                    await datetimeView(dnn.dong, dnn.ho, dnn.Month);
                    Year = dnn.Month.Insert(4, "년");
                    dnn.Month = Year.Insert(7, "월");
                    list = await community_Lib.GetListDongHoDate(Apt_Code, Dong, Ho, dt21, dt22);
                }
                else
                {
                    dnn.Month = "관리비 정보 없음";
                }


            }
            else
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "로그인되지 않았습니다..");
                MyNav.NavigateTo("/");
            }
        }


        public int re1 { get; set; } = 0;
        public int re2 { get; set; } = 0;
        public int re3 { get; set; } = 0;
        public string Mon { get; set; }
        public string strC { get; set; }
        public int intYearA { get; set; } = 0;
        private async Task datetimeView(string Dong, string Ho, string Month)
        {
            Mon = Month.Substring(4);
            int m = Convert.ToInt32(Mon);
            strC = Month.Substring(0, 4);
            int mm = Convert.ToInt32(strC);
            m = m - 1;
            if (m == 0)
            {
                m = 12;
                mm = mm - 1;
                strC = mm.ToString();
                Mon = m.ToString();
            }

            if (m < 10)
            {
                Mon = "0" + m.ToString();
            }
            else
            {
                Mon = m.ToString();
            }


            MonthA = strC + Mon;
            cnn = await costDebit_Lib.GetBy(Apt_Code, Dong, Ho, MonthA);
            re1 = await costDebit_Lib.GetBy_be(Apt_Code, Dong, Ho, MonthA);
        }

        /// <summary>
        /// 추가 정보 보기(관리비)
        /// </summary>
        public string PlusA { get; set; } = "A";
        private void onPlusA()
        {
            if (PlusA == "A")
            {
                PlusA = "B";
            }
            else
            {
                PlusA = "A";
            }
        }

        /// <summary>
        /// 추가 정보 보기(사용료)
        /// </summary>
        public string PlusB { get; set; } = "A";
        private void onPlusB()
        {
            if (PlusB == "A")
            {
                PlusB = "B";
            }
            else
            {
                PlusB = "A";
            }
        }

        /// <summary>
        /// 추가 정보 보기(관리비 차감)
        /// </summary>
        public string PlusC { get; set; } = "A";
        private void onPlusC()
        {
            if (PlusC == "A")
            {
                PlusC = "B";
            }
            else
            {
                PlusC = "A";
            }
        }

        /// <summary>
        /// 추가 정보 보기(주민공동시설 이용료)
        /// </summary>
        public string PlusD { get; set; } = "A";
        private void onPlusD()
        {
            if (PlusD == "A")
            {
                PlusD = "B";
            }
            else
            {
                PlusD = "A";
            }
        }
    }
}
