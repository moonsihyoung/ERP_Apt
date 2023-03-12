using Company;
using Erp_Apt_Lib.Up_Files;
using Erp_Apt_Staff;
using Erp_Entity;
using Erp_Lib;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using sw_Lib.Labors;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace Erp_Apt_Web.Pages.Labor
{
    public partial class Index
    {

        #region 인스턴스
        [Parameter] public string Code { get; set; }
        [CascadingParameter] public Task<AuthenticationState> AuthenticationStateRef { get; set; }
        [Inject] public ILabor_contract_Lib labor { get; set; }
        [Inject] public IApt_Lib apt_Lib { get; set; }
        [Inject] public IStaff_Sub_Lib staff_sub_Lib { get; set; }
        [Inject] public ICompany_Lib company_Lib { get; set; }
        [Inject] public IReferral_career_Lib career_Lib { get; set; }
        [Inject] public ISido_Lib sido { get; set; }
        [Inject] public Iwage_Lib wege { get; set; }
        [Inject] public NavigationManager MyNav { get; set; }
        [Inject] IJSRuntime JSRuntime { get; set; }
        [Inject] public IUpFile_Lib files_Lib { get; set; }
        #endregion

        #region 변수
        public Labor_contract_Entity ann { get; set; } = new Labor_contract_Entity();
        public Referral_career_Entity referral { get; set; }
        public Company_Entity cop { get; set; }
        public Staff_Sub_Entity sse { get; set; }
        public int TotalCount { get; set; } = 0;
        public int Page { get; set; } = 0;
        public string Sum_Pay { get; set; } = "0";
        public string WorkMonthTime { get; set; } = "0";
        public string BreaktimeDivision { get; set; } = "없음";
        public string MonthNightPay { get; set; } = "0";
        public int NightPay { get; set; } = 0;
        public int Count { get; set; }
        public string User_Code { get; set; }
        public string User_Name { get; set; }
        public int LevelCount { get; private set; }
        public string Apt_Code { get; set; }
        public string Apt_Name { get; set; }
        public string UserName { get; set; }
        public string Views { get; set; } = "A";
        public string strCor_Code { get; set; }
        public string FileInputViews { get; set; } = "A";
        public string FileViews { get; set; } = "A";
        public int Files_Count { get; set; } = 0;
        public string InsertViews { get; set; } = "A";
        public int intWage { get; private set; }
        public string strTitle { get; set; }
        #endregion

        #region 속성
        List<Labor_contract_Entity> bnn = new List<Labor_contract_Entity>();
        public int intNum { get; private set; }
        UpFile_Entity dnn { get; set; } = new UpFile_Entity(); // 첨부 파일 정보
        List<UpFile_Entity> Files_Entity { get; set; } = new List<UpFile_Entity>();
        List<Referral_career_Entity> rnn = new List<Referral_career_Entity>();
        List<Sido_Entity> snn = new List<Sido_Entity>();
        #endregion

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

        /// <summary>
        /// 로딩 시 실행
        /// </summary>
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
                Code = Apt_Code;

                if (LevelCount >= 5)
                {
                    await DisplayData();

                    bnn = await labor.Contract_list_Apt(Page, Apt_Code);
                    ann.RetirementAge = 62;
                    ann.WorkContract_Date = DateTime.Now;
                    ann.LaborStartDate = DateTime.Now;
                    ann.LaborEndDate = DateTime.Now;

                    int intA = await labor.Contract_count_Apt(Apt_Code);
                    TotalCount = intA;
                    int intB = (Page * 15);
                    Count = intA - intB;
                }
                else
                {
                    await JSRuntime.InvokeAsync<object>("alert", "권한이 없습니다.");
                    MyNav.NavigateTo("/");
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
            pager.RecordCount = await labor.Contract_count_Apt(Apt_Code);

            bnn = await labor.Contract_list_Apt(pager.PageIndex, Apt_Code);
            intNum = pager.RecordCount - (pager.PageIndex * pager.PageSize);
            //StateHasChanged();
        }

        

        /// <summary>
        /// 업체선택 시 실행
        /// </summary>
        private async Task onCor_Code(ChangeEventArgs args)
        {
            try
            {
                if (args != null)
                {
                    ann.Cor_Code = args.Value.ToString();

                    cop = await company_Lib.detail(ann.Cor_Code);
                    ann.Ceo_Adress = cop.Adress_Sido + " " + cop.Adress_GunGu + " " + cop.Adress_Rest;
                    ann.Ceo_Company = cop.Cor_Name;
                    ann.Ceo_Telephone = cop.Telephone;
                    ann.Ceo_Name = cop.Ceo_Name;
                }
            }
            catch (Exception)
            {
                //
            }
        }

        /// <summary>
        /// 근로형태 선택 시 실행
        /// </summary>
        private void onWorktimeSort(ChangeEventArgs args)
        {
            try
            {
                ann.WorktimeSort = args.Value.ToString();
            }
            catch (Exception)
            {
                ann.WorktimeSort = "매일(월~금)";
            }

            ann.BreaktimeSort = ann.WorktimeSort;

            if (ann.WorktimeSort == "매일(월~금)")
            {
                ann.Worktime = 8;
                ann.WorktimeWeekend = 0;
                ann.StartWorkTime = "09";
                ann.EndWorkTime = "18";
                BreaktimeDivision = ann.WorktimeSort;

                //double dbA = 0;
                dbA = (((((6 * ann.Worktime) + ann.WorktimeWeekend) * 365) / 7) / 12);
                ann.WorkMonthTime = dbA;
                WorkMonthTime = string.Format("{0: ###,###.##}", dbA);
            }
            else if (ann.WorktimeSort == "격일")
            {
                ann.Worktime = 12;
                ann.WorktimeWeekend = 0;
                ann.StartWorkTime = "07";
                ann.EndWorkTime = "익일 07";
                BreaktimeDivision = ann.WorktimeSort;

                //double dbA = 0;
                dbA = (((24 - ann.BreaktimeDaytime - ann.BreaktimeNight) * 365) / 24);
                ann.WorkMonthTime = dbA;
                WorkMonthTime = string.Format("{0: ###,###.##}", dbA);
            }
            else if (ann.WorktimeSort == "3교대")
            {
                ann.Worktime = 8;
                ann.WorktimeWeekend = 0;
                ann.StartWorkTime = "09";
                ann.EndWorkTime = "18";
                BreaktimeDivision = ann.WorktimeSort;

                //double dbA = 0;
                dbA = ((((7 * ann.Worktime) * 365) / 7) / 12);
                ann.WorkMonthTime = dbA;
                WorkMonthTime = string.Format("{0: ###,###.##}", dbA);
            }
            else
            {
                ann.Worktime = 8;
                ann.WorktimeWeekend = 0;
                ann.StartWorkTime = "09";
                ann.EndWorkTime = "18";
                BreaktimeDivision = ann.WorktimeSort;

                //double dbA = 0;
                dbA = (((((6 * ann.Worktime) + ann.WorktimeWeekend) * 365) / 7) / 12);
                WorkMonthTime = string.Format("{0: ###,###.##}", dbA);
            }
        }

        #region 근로시간 관련
        /// <summary>
        /// 주간 휴게시간 입력 시 근로시간 실행
        /// </summary>
        /// <param name="args"></param>
        public void onBreaktimeDaytime(ChangeEventArgs args)
        {
            try
            {
                ann.BreaktimeDaytime = Convert.ToDouble(args.Value);
            }
            catch (Exception)
            {
                ann.BreaktimeDaytime = 0;
            }

            if (ann.WorktimeSort == "매일(월~금)")
            {
                ann.Worktime = 8;

                //double dbA = 0;
                double dbD = ann.Worktime; // - ann.BreaktimeDaytime;
                ann.Worktime = Math.Round(dbD, 3);

                dbA = (((((6 * ann.Worktime) + ann.WorktimeWeekend) * 365) / 7) / 12);
                WorkMonthTime = string.Format("{0: ###,###.##}", dbA);
            }
            else if (ann.WorktimeSort == "격일")
            {
                //double dbA = 0;
                //dbA = (((24 - ann.BreaktimeDaytime - ann.BreaktimeNight) * 365) / 24);
                //WorkMonthTime = string.Format("{0: ###,###.##}", dbA);
                //double dbB = ann.Worktime - ((ann.BreaktimeDaytime + ann.BreaktimeNight) / 2);
                //ann.Worktime = Math.Round(dbB, 3);

                //double dbA = 0;
                double dbC = 0;
                dbA = (((24 - ann.BreaktimeDaytime - ann.BreaktimeNight) / 2 * 365) / 12);
                WorkMonthTime = string.Format("{0: ###,###.##}", dbA);
                dbA = Math.Round(dbA, 2);
                ann.WorkMonthTime = dbA;
                double MonthNightTimeA = (((((8 - ann.BreaktimeNight)) / 2 * 365) / 12) * 0.5);
                MonthNightTimeA = Math.Round(MonthNightTimeA, 6);
                double dbB = ann.BasicsPay + ann.Pay_B + ann.Pay_C + ann.Pay_D;
                dbC = dbB / dbA; //통상임금 구하기

                dbB = dbC * MonthNightTimeA; //야간수당 구하기
                //double intA = eiling(dbB);
                ann.Pay_A = (int)Math.Ceiling(dbB / 10) * 10; //
                NightPay = (int)Math.Ceiling(dbB);
                MonthNightPay = string.Format("{0: ###,###}", dbB);

                int result = ann.BasicsPay + ann.Pay_A + ann.Pay_B + ann.Pay_C + ann.Pay_D;
                Sum_Pay = string.Format("{0: ###,###}", result);

                double dbE = 0;
                if (ann.WorktimeSort == "격일")
                {
                    dbE = 12;
                }
                else
                {
                    dbE = 8;
                }
                double dbD = dbE - ((ann.BreaktimeDaytime + ann.BreaktimeNight) / 2);
                //WorkMonthTime = dbD.ToString();

                ann.Worktime = Math.Round(dbD, 6);
            }
            else if (ann.WorktimeSort == "3교대")
            {
                ann.Worktime = 8;

                //double dbA = 0;


                double dbD = ann.Worktime - ann.BreaktimeDaytime;
                ann.Worktime = Math.Round(dbD, 6);

                dbA = (((((6 * ann.Worktime) + ann.WorktimeWeekend) * 365) / 7) / 12);
                ann.WorkMonthTime = dbA;
                WorkMonthTime = string.Format("{0: ###,###.##}", dbA);
            }
            else
            {
                ann.Worktime = 8;

                //double dbA = 0;


                double dbD = ann.Worktime - ann.BreaktimeDaytime;
                ann.Worktime = Math.Round(dbD, 6);

                dbA = (((((6 * ann.Worktime) + ann.WorktimeWeekend) * 365) / 7) / 12);
                ann.WorkMonthTime = dbA;
                WorkMonthTime = string.Format("{0: ###,###.##}", dbA);
            }
        }

        /// <summary>
        /// 야간 휴게시간 입력 시 근로시간 실행
        /// </summary>
        /// <param name="args"></param>
        public double dbA { get; set; }
        public void onBreaktimeNight(ChangeEventArgs args)
        {
            try
            {
                ann.BreaktimeNight = Convert.ToDouble(args.Value);
            }
            catch (Exception)
            {
                ann.BreaktimeNight = 0;
            }

            if (ann.WorktimeSort == "매일(월~금)")
            {
                ann.Worktime = 8;

                //double dbA = 0;


                double dbD = ann.Worktime - ann.BreaktimeDaytime;
                ann.Worktime = Math.Round(dbD, 6);
                dbA = (((((6 * ann.Worktime) + ann.WorktimeWeekend) * 365) / 7) / 12);
                ann.WorkMonthTime = dbA;
                WorkMonthTime = string.Format("{0: ###,###.##}", dbA);

            }
            else if (ann.WorktimeSort == "격일")
            {
                //double dbA = 0;
                double dbC = 0;
                dbA = (((24 - ann.BreaktimeDaytime - ann.BreaktimeNight) / 2 * 365) / 12);
                WorkMonthTime = string.Format("{0: ###,###.##}", dbA);
                dbA = Math.Round(dbA, 6);
                ann.WorkMonthTime = dbA;
                double MonthNightTimeA = (((((8 - ann.BreaktimeNight)) / 2 * 365) / 12) * 0.5);
                MonthNightTimeA = Math.Round(MonthNightTimeA, 6);
                double dbB = ann.BasicsPay + ann.Pay_B + ann.Pay_C + ann.Pay_D;
                dbC = dbB / dbA; //통상임금 구하기

                dbB = dbC * MonthNightTimeA; //야간수당 구하기
                //int intA = (int)Math.Ceiling(dbB);
                ann.Pay_A = (int)Math.Ceiling(dbB / 10) * 10; //
                NightPay = (int)Math.Ceiling(dbB);
                MonthNightPay = string.Format("{0: ###,###}", dbB);

                int result = ann.BasicsPay + ann.Pay_A + ann.Pay_B + ann.Pay_C + ann.Pay_D;
                Sum_Pay = string.Format("{0: ###,###}", result);

                double dbE = 0;
                if (ann.WorktimeSort == "격일")
                {
                    dbE = 12;
                }
                else
                {
                    dbE = 8;
                }
                double dbD = dbE - ((ann.BreaktimeDaytime + ann.BreaktimeNight) / 2);
                ann.Worktime = Math.Round(dbD, 6);
            }
            else if (ann.WorktimeSort == "3교대")
            {
                ann.Worktime = 8;

                //double dbA = 0;


                double dbD = ann.Worktime - ann.BreaktimeDaytime;
                ann.Worktime = Math.Round(dbD, 6);
                dbA = (((((6 * ann.Worktime) + ann.WorktimeWeekend) * 365) / 7) / 12);
                ann.WorkMonthTime = dbA;
                WorkMonthTime = string.Format("{0: ###,###.##}", dbA);

            }
            else
            {
                ann.Worktime = 8;
                double dbD = ann.Worktime - ann.BreaktimeDaytime;
                ann.Worktime = Math.Round(dbD, 6);
                dbA = (((((6 * ann.Worktime) + ann.WorktimeWeekend) * 365) / 7) / 12);
                ann.WorkMonthTime = dbA;
                WorkMonthTime = string.Format("{0: ###,###.##}", dbA);

            }
        }

        #endregion


        /// <summary>
        /// 근로계약서 저장
        /// </summary>
        private async Task btnSave()
        {
            ann.TotalSum = Convert.ToInt32(Sum_Pay.Replace(",", ""));
            //ann.WorkMonthTime = dbA;  //Convert.ToDouble(WorkMonthTime); 
            intWage = await wege.wage(ann.LaborStartDate.Year);
            int totalsum = (ann.TotalSum - ann.Pay_E - ann.Pay_A);
            double dbWorkMonthTime = ann.WorkMonthTime; //Convert.ToDouble(ViewState["WorkMonthTime"]); //월근로시간 만들기
            double Result = totalsum / dbWorkMonthTime;//ann.WorkMonthTime; //2020년 4월 17일날 수정함.
            int totalsumB = ann.TotalSum - ann.Pay_A;
            //int Year = ann.LaborStartDate.Year;
            double ResultA = intWage;

            double ResultB = 0;
            if (ann.Pay_A > 0)
            {
                double daytimeA = (((((8 - ann.BreaktimeNight) / 2) * 365) / 12) * 0.5); //야간근로시간
                ResultA = Math.Ceiling(ann.Pay_A / daytimeA); //야간 시간당 수당

                ResultB = totalsumB / ann.WorkMonthTime; // 통상임금에 따른 시간급 계산
            }
            else
            {
                ResultA = intWage;
            }

            string so = (DateTime.Now.Year - 1).ToString();
            DateTime dtA = Convert.ToDateTime(so + "-01-01");

            if (ann.Cor_Code == null || ann.Cor_Code == "")
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "소속 업체를 선택하지 않았습니다..");
            }
            else if (intWage < 1)
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "계약 시작 날짜가 잘못 입력되었습니다.");
            }
            else if (ann.WorktimeSort == "" || ann.Worktime < 1)
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "근무시간을 입력하지 않았습니다.");
            }
            else if (ann.WorktimeSort == null || ann.WorktimeSort == "")
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "근무형태를 선택하지 않았습니다.");
            }
            else if (ann.PartTime == null || ann.PartTime == "")
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "촉탁여부를 선택하지 않았습니다.");
            }
            else if (ann.LaborStartDate < dtA)
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "계약 시작 날짜가 잘못 입력되었습니다.");
            }
            else if (ann.LaborEndDate < dtA)
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "계약 종료 날짜가 잘못 입력되었습니다..");
            }
            else if (ann.BreakTimeEtc == null || ann.BreakTimeEtc == "")
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "휴게 시간 상세를 입력하지 않았습니다.");
            }
            else if (ann.BasicsPay < 50000)
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "급여를 입력하지 않았습니다.");
            }
            else
            {
                ann.ContractNotice = "본 계약에 대하여 충분히 설명을 들은 후 그 내용을 이해하고 자의로서";
                ann.ContractApprovalDivision = "A";
                ann.Division = "A";
                ann.Copy_Approval = true;
                ann.Read_Approval1 = true;
                ann.Read_Approval = true;
                ann.Telephone = ann.Mobile;

                if (ann.WorktimeSort == "격일")
                {
                    ann.Holiday = "해당 없음";
                }
                else
                {
                    ann.Holiday = "주휴일(일요일) 및 법정 휴일";
                }

                ann.TotalSum = Convert.ToInt32(Sum_Pay.Replace(",", ""));

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
                ann.PostIP = myIPAddress;
                #endregion

                double paySum = ann.BasicsPay + ann.Pay_B + ann.Pay_C + ann.Pay_D + ann.Pay_E;
                paySum = Math.Round(paySum, 6);
                paySum = paySum / ann.WorkMonthTime;
                if (paySum < intWage)
                {
                    await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "최저 임금 이하 일 수는  없습니다! 입력된 시간급은 " + paySum + "원입니다.");
                    //await Focus(ann.Pay_A.ToString());
                }
                else
                {
                    if (ann.Aid != 0)
                    {
                        if (ann.WorktimeSort == "격일")
                        {
                            if (ResultA >= intWage && ResultA >= ResultB)
                            {
                                if (ann.BreakTimeEtc == null || ann.BreakTimeEtc == "")
                                {
                                    await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "휴게 시간 상세를 입력하지 않았습니다.");
                                }
                                else if (ann.Pay_A < NightPay)
                                {
                                    await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "법정 야간 수당보다 지급되는 야간수당 적을 수는 없습니다.");
                                }
                                else
                                {
                                    await labor.Edit(ann);
                                    await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "수정 되었습니다.");
                                    Views = "C";
                                    await NewReflesh();
                                }
                            }
                            else
                            {
                                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "야간수당 시간급이 통상임금에 따라 산출된 시간급 보다 적을 수는 없습니다. \n 혹은 최저 임금에 미치지 못하도록 근로 계약을 체결할 수 없습니다.");
                            }
                        }
                        else
                        {
                            if (Result >= intWage)
                            {
                                await labor.Edit(ann);
                                //await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "수정 되었습니다.");

                                Views = "C";
                                await NewReflesh();
                            }
                            else
                            {
                                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "야간수당 시간급이 통상임금에 따라 산출된 시간급 보다 적을 수는 없습니다. \n 혹은 최저 임금에 미치지 못하도록 근로 계약을 체결할 수 없습니다.");
                            }
                        }

                    }
                    else
                    {
                        if (ann.WorktimeSort == "격일")
                        {
                            if (ResultA >= intWage && ResultA >= ResultB)
                            {
                                if (ann.BreakTimeEtc == null)
                                {
                                    await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "휴게 시간을 입력하지 않았습니다.");
                                }
                                else if (ann.BreakTimeEtc == "")
                                {
                                    await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "휴게 시간 상세를 입력하지 않았습니다.");
                                }
                                else if (ann.Pay_A < NightPay)
                                {
                                    await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "법정 야간 수당보다 지급되는 야간수당 적을 수는 없습니다.");
                                }
                                else
                                {
                                    await labor.add(ann);
                                    //await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "등록 되었습니다.");

                                    await NewReflesh();
                                }
                            }
                            else
                            {
                                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "야간수당 시간급이 통상임금에 따라 산출된 시간급 보다 적을 수는 없습니다. \n 혹은 최저 임금에 미치지 못하도록 근로 계약을 체결할 수 없습니다.");
                            }
                        }
                        else
                        {
                            if (Result >= intWage)
                            {
                                await labor.add(ann);
                                //await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "등록 되었습니다.");
                                await NewReflesh();
                            }
                            else
                            {
                                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "야간수당 시간급이 통상임금에 따라 산출된 시간급 보다 적을 수는 없습니다. \n 혹은 최저 임금에 미치지 못하도록 근로 계약을 체결할 수 없습니다.");
                            }
                        }
                    }
                }
            }
        }

        private async Task NewReflesh()
        {
            ann = new Labor_contract_Entity();
            Views = "B";
            rnn = new List<Referral_career_Entity>();
            Sum_Pay = "0";
            WorkMonthTime = "0";
            MonthNightPay = "0";
            strCor_Code = "";
            UserName = "";
            ann.PartTime = "불촉탁";
            ann = new Labor_contract_Entity();
            ann.RetirementAge = 62;
            ann.WorkMonthTime = 0;
            ann.WorkContract_Date = DateTime.Now;
            ann.LaborStartDate = DateTime.Now;
            ann.LaborEndDate = DateTime.Now;
            Sum_Pay = "0";
            NightPay = 0;
            await DisplayData();
        }

        /// <summary>
        /// 포커스
        /// </summary>
        public async Task Focus(string elementId)
        {
            await JSRuntime.InvokeVoidAsync("jsfunction.focusElement", elementId);
        }

        /// <summary>
        /// 상세보기 닫기
        /// </summary>
        private void CloseA()
        {
            Views = "A";
        }

        /// <summary>
        ///  근무일 수 계산
        /// </summary>
        protected string Func_span(object objstart, object objend)
        {
            int ddate = 0;
            DateTime date = Convert.ToDateTime(objend);
            if (date.Year == 0001)
            {
                DateTime start = Convert.ToDateTime(objstart);
                //DateTime end = Convert.ToDateTime(objend);
                string start_a = start.ToShortDateString();
                string end_a = DateTime.Now.ToShortDateString();   //end.ToShortDateString();
                ddate = career_Lib.Date_scomp(start_a, end_a);
            }
            else
            {
                DateTime start = Convert.ToDateTime(objstart);
                DateTime end = Convert.ToDateTime(objend);
                string start_a = start.ToShortDateString();
                string end_a = end.ToShortDateString(); //DateTime.Now.ToShortDateString();
                ddate = career_Lib.Date_scomp(start_a, end_a);
            }

            return string.Format("{0: ###,###}", ddate);
        }

        /// <summary>
        ///  승인여부
        /// </summary>
        public string FuncResult(object CodeA)
        {
            string code = CodeA.ToString();
            if (code == "B")
            {
                code = "승인";
            }
            else
            {
                code = "미승인";
            }
            return code;
        }

        /// <summary>
        /// 이름으로 배치정보 찾기
        /// </summary>
        private async Task onNamed()
        {
            try
            {
                rnn = await career_Lib._Career_Name_Apt_Search("User_Name", UserName, Apt_Code);
            }
            catch (Exception)
            {
                //
            }
        }

        /// <summary>
        /// 촉탁여부 확인
        /// </summary>
        private void onPartTime(ChangeEventArgs args)
        {
            ann.PartTime = args.Value.ToString();
        }

        /// <summary>
        /// 이름 선택 시 실행
        /// </summary>
        private async Task onName_s(ChangeEventArgs args)
        {
            try
            {
                string strCode = args.Value.ToString();
                referral = await career_Lib.Detail(strCode);

                ann.UserID = referral.User_ID;
                ann.UserName = referral.User_Name;
                ann.Apt_Name = Apt_Name;
                ann.Apt_Code = Apt_Code;

                sse = await staff_sub_Lib.Detail(ann.UserID);
                ann.Adress = sse.st_Sido + " " + sse.st_GunGu + " " + sse.st_Adress_Rest;
                ann.Mobile = sse.Mobile_Number;
            }
            catch (Exception)
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "배치정보가 없습니다. /n 퇴사처리 처리되었는지 확인하세요.");
            }
        }

        /// <summary>
        /// 근로시간 입력 시 월간시간 실행
        /// </summary>
        public void onWorktime(ChangeEventArgs args)
        {

            if (ann.WorktimeSort == "매일(월~금)")
            {
                try
                {
                    ann.Worktime = Convert.ToDouble(args.Value);
                }
                catch (Exception)
                {
                    ann.Worktime = 0;
                }
                double dbA = 0;
                dbA = (((((6 * ann.Worktime) + ann.WorktimeWeekend) * 365) / 7) / 12);
                ann.WorkMonthTime = dbA;
                WorkMonthTime = string.Format("{0: ###,###.##}", dbA);
            }
        }

        /// <summary>
        /// 토요일 근로시간 입력 실행
        /// </summary>
        public void onWorktimeWeekend(ChangeEventArgs args)
        {
            if (ann.WorktimeSort != "매일(월~금)")
            {
                ann.WorktimeWeekend = 0;
            }

            try
            {
                ann.WorktimeWeekend = Convert.ToDouble(args.Value);
            }
            catch (Exception)
            {
                ann.WorktimeWeekend = 0;
            }
            double dbA = 0;
            dbA = ((6 * ann.Worktime) + ann.WorktimeWeekend);
            dbA = ((dbA * 365) / 7);
            dbA = (dbA / 12);
            ann.WorkMonthTime = dbA;
            WorkMonthTime = string.Format("{0: ###,###.##}", dbA);
        }

        #region 급여관련
        /// <summary>
        /// 기본급 입력시 급여 합계 실행
        /// </summary>
        public void onBasicePay(ChangeEventArgs args)
        {
            try
            {
                ann.BasicsPay = Convert.ToInt32(args.Value);
            }
            catch (Exception)
            {
                ann.BasicsPay = 0;
            }

            if (ann.BreaktimeSort == "격일")
            {
                double dbA = 0;
                double dbC = 0;
                dbA = (((24 - ann.BreaktimeDaytime - ann.BreaktimeNight) / 2 * 365) / 12);
                WorkMonthTime = string.Format("{0: ###,###.##}", dbA);
                dbA = Math.Round(dbA, 6);
                double MonthNightTimeA = (((((8 - ann.BreaktimeNight)) / 2 * 365) / 12) * 0.5);
                MonthNightTimeA = Math.Round(MonthNightTimeA, 6);
                double dbB = ann.BasicsPay + ann.Pay_B + ann.Pay_C + ann.Pay_D;
                dbC = dbB / dbA; //통상임금 구하기

                dbB = dbC * MonthNightTimeA; //야간수당 구하기
                int intA = (int)Math.Ceiling(dbB);

                ann.Pay_A = (int)Math.Ceiling(dbB / 10) * 10;
                NightPay = intA;
                MonthNightPay = string.Format("{0: ###,###}", intA);
            }
            int result = ann.BasicsPay + ann.Pay_A + ann.Pay_B + ann.Pay_C + ann.Pay_D;
            Sum_Pay = string.Format("{0: ###,###}", result);
        }

        /// <summary>
        /// 야간수당 입력시 급여 합계 실행
        /// </summary>
        public void onPay_A(ChangeEventArgs args)
        {
            int intA = 0;
            try
            {
                ann.Pay_A = Convert.ToInt32(args.Value);
            }
            catch (Exception)
            {
                ann.Pay_A = 0;
            }

            if (ann.BreaktimeSort == "격일")
            {
                double dbA = 0;
                double dbC = 0;
                dbA = (((24 - ann.BreaktimeDaytime - ann.BreaktimeNight) / 2 * 365) / 12);
                WorkMonthTime = string.Format("{0: ###,###.##}", dbA);
                dbA = Math.Round(dbA, 6);
                double MonthNightTimeA = (((((8 - ann.BreaktimeNight)) / 2 * 365) / 12) * 0.5);
                MonthNightTimeA = Math.Round(MonthNightTimeA, 6);
                double dbB = ann.BasicsPay + ann.Pay_B + ann.Pay_C + ann.Pay_D;
                dbC = dbB / dbA; //통상임금 구하기

                dbB = dbC * MonthNightTimeA; //야간수당 구하기
                intA = (int)Math.Ceiling(dbB);

                NightPay = intA;

                if (ann.Pay_A >= intA)
                {
                    MonthNightPay = string.Format("{0: ###,###}", intA);
                }
                else
                {

                }


            }
            int result = ann.BasicsPay + ann.Pay_A + ann.Pay_B + ann.Pay_C + ann.Pay_D;
            Sum_Pay = string.Format("{0: ###,###}", result);
        }

        /// <summary>
        /// 직책수당 입력시 합계 실행
        /// </summary>
        public void onPay_B(ChangeEventArgs args)
        {
            try
            {
                ann.Pay_B = Convert.ToInt32(args.Value);
            }
            catch (Exception)
            {
                ann.Pay_B = 0;
            }

            if (ann.BreaktimeSort == "격일")
            {
                double dbA = 0;
                double dbC = 0;
                dbA = (((24 - ann.BreaktimeDaytime - ann.BreaktimeNight) / 2 * 365) / 12);
                WorkMonthTime = string.Format("{0: ###,###.##}", dbA);
                dbA = Math.Round(dbA, 6);
                double MonthNightTimeA = (((((8 - ann.BreaktimeNight)) / 2 * 365) / 12) * 0.5);
                MonthNightTimeA = Math.Round(MonthNightTimeA, 6);
                double dbB = ann.BasicsPay + ann.Pay_B + ann.Pay_C + ann.Pay_D;
                dbC = dbB / dbA; //통상임금 구하기

                dbB = dbC * MonthNightTimeA; //야간수당 구하기
                int intA = (int)Math.Ceiling(dbB);
                ann.Pay_A = (int)Math.Ceiling(dbB / 10) * 10;
                NightPay = intA;
                MonthNightPay = string.Format("{0: ###,###}", intA);
            }
            int result = ann.BasicsPay + ann.Pay_A + ann.Pay_B + ann.Pay_C + ann.Pay_D;
            Sum_Pay = string.Format("{0: ###,###}", result);
        }

        /// <summary>
        /// 기타 수당 입력시 합계 실행
        /// </summary>
        public void onPay_C(ChangeEventArgs args)
        {
            try
            {
                ann.Pay_C = Convert.ToInt32(args.Value);
            }
            catch (Exception)
            {
                ann.Pay_C = 0;
            }

            if (ann.BreaktimeSort == "격일")
            {
                double dbA = 0;
                double dbC = 0;
                dbA = (((24 - ann.BreaktimeDaytime - ann.BreaktimeNight) / 2 * 365) / 12);
                WorkMonthTime = string.Format("{0: ###,###.##}", dbA);
                dbA = Math.Round(dbA, 6);
                double MonthNightTimeA = (((((8 - ann.BreaktimeNight)) / 2 * 365) / 12) * 0.5);
                MonthNightTimeA = Math.Round(MonthNightTimeA, 6);
                double dbB = ann.BasicsPay + ann.Pay_B + ann.Pay_C + ann.Pay_D;
                dbC = dbB / dbA; //통상임금 구하기

                dbB = dbC * MonthNightTimeA; //야간수당 구하기
                int intA = (int)Math.Ceiling(dbB);
                ann.Pay_A = (int)Math.Ceiling(dbB / 10) * 10;
                NightPay = intA;
                MonthNightPay = string.Format("{0: ###,###}", intA);
            }
            int result = ann.BasicsPay + ann.Pay_A + ann.Pay_B + ann.Pay_C + ann.Pay_D;
            Sum_Pay = string.Format("{0: ###,###}", result);
        }

        /// <summary>
        /// 자격수당 입력시 합계 실행
        /// </summary>
        /// <param name="args"></param>
        public void onPay_D(ChangeEventArgs args)
        {
            try
            {
                ann.Pay_D = Convert.ToInt32(args.Value);
            }
            catch (Exception)
            {
                ann.Pay_D = 0;
            }

            if (ann.BreaktimeSort == "격일")
            {
                double dbA = 0;
                double dbC = 0;
                dbA = (((24 - ann.BreaktimeDaytime - ann.BreaktimeNight) / 2 * 365) / 12);
                WorkMonthTime = string.Format("{0: ###,###.##}", dbA);
                dbA = Math.Round(dbA, 6);
                double MonthNightTimeA = (((((8 - ann.BreaktimeNight)) / 2 * 365) / 12) * 0.5);
                MonthNightTimeA = Math.Round(MonthNightTimeA, 6);
                double dbB = ann.BasicsPay + ann.Pay_B + ann.Pay_C + ann.Pay_D;
                dbC = dbB / dbA; //통상임금 구하기

                dbB = dbC * MonthNightTimeA; //야간수당 구하기
                int intA = (int)Math.Ceiling(dbB);
                ann.Pay_A = (int)Math.Ceiling(dbB / 10) * 10;
                NightPay = intA;
                MonthNightPay = string.Format("{0: ###,###}", intA);
            }
            int result = ann.BasicsPay + ann.Pay_A + ann.Pay_B + ann.Pay_C + ann.Pay_D;
            Sum_Pay = string.Format("{0: ###,###}", result);
        }
        #endregion

        public void SelectView(Labor_contract_Entity _labor)
        {
            Views = "C";
            ann = _labor;
            int a = ann.BasicsPay + ann.Pay_A + ann.Pay_B + ann.Pay_C + ann.Pay_D + ann.Pay_E + ann.Pay_F;
            Sum_Pay = string.Format("{0: ###,###.##}", a);
            ann.WorkMonthTime = Math.Round(ann.WorkMonthTime, 2);
            //StateHasChanged(); // 현재 컴포넌트 재로드
        }

        public void btnEditViews(Labor_contract_Entity _labor)
        {
            ann = _labor;

            Sum_Pay = string.Format("{0: ###,###}", ann.TotalSum);
            WorkMonthTime = ann.WorkMonthTime.ToString();
            MonthNightPay = string.Format("{0: ###,###}", ann.Pay_A);
            strCor_Code = ann.Cor_Code;
            UserName = ann.UserName;
            NightPay = ann.Pay_A;

            InsertViews = "B";
        }

        /// <summary>
        /// 근로계약 새로 등록 열기
        /// </summary>
        public async Task btnSaveViews()
        {
            InsertViews = "B";
            intWage = await wege.wage(DateTime.Now.Year);

            ann = new Labor_contract_Entity();
            ann.RetirementAge = 62;
            ann.WorkContract_Date = DateTime.Now;
            ann.LaborStartDate = DateTime.Now;
            ann.LaborEndDate = DateTime.Now;
            //StateHasChanged(); // 현재 컴포넌트 재로드
        }

        /// <summary>
        /// 근로계약 새로 등록 열기
        /// </summary>
        public void btnNewViews()
        {
            InsertViews = "B";
            ann = new Labor_contract_Entity();
            ann.RetirementAge = 62;
            ann.PartTime = "불촉탁";
            ann.WorkContract_Date = DateTime.Now;
            ann.LaborStartDate = DateTime.Now;
            ann.LaborEndDate = DateTime.Now;
            strTitle = "근로계약 새로 등록";
        }


        /// <summary>
        /// 확정자 찾기오기
        /// </summary>
        private string OnExtension(string filesName)
        {
            return Path.GetExtension(filesName);
        }

        /// <summary>
        /// 삭제
        /// </summary>
        private async Task btnRemove(int Aid)
        {
            bool isDelete = await JSRuntime.InvokeAsync<bool>("confirm", $"{Aid}번 글을 정말로 삭제하시겠습니까?");
            if (isDelete)
            {
                await labor.Remove(Aid.ToString());
                bnn = await labor.Contract_list_Apt(Page, Apt_Code);
                Views = "A";
                //StateHasChanged();
            }
        }

        /// <summary>
        /// 인쇄로 이동
        /// </summary>
        /// <param name="zx"></param>
        public void btnPrint(Labor_contract_Entity zx)
        {
            string strUrl = "http://net.wedew.co.kr";

            if (zx.Cor_Code == "sw5")
            {
                MyNav.NavigateTo("http://pt.swtmc.co.kr/Prints/Prints_sw.aspx?Aid=" + zx.Aid + "&Uc=" + zx.UserID + "&Url=" + strUrl);
            }
            else if (zx.Cor_Code == "sw5a")
            {
                MyNav.NavigateTo("http://pt.swtmc.co.kr/Prints/Prints_nG.aspx?Aid=" + zx.Aid + "&Uc=" + zx.UserID + "&Url=" + strUrl);
            }
            else if (zx.Cor_Code == "sw5b")
            {
                MyNav.NavigateTo("http://pt.swtmc.co.kr/Prints/Prints_gat.aspx?Aid=" + zx.Aid + "&Uc=" + zx.UserID + "&Url=" + strUrl);
            }

        }

        /// <summary>
        /// 계약서 첨부 하기 열기
        /// </summary>
        /// <param name="Aid"></param>
        private void btnInputFiles(int Aid)
        {
            FileInputViews = "B";
        }

        /// <summary>
        /// 파일 첨부  닫기
        /// </summary>
        private void FilesClose()
        {
            FileInputViews = "A";
        }

        /// <summary>
        /// 파일 첨부 상세 닫기
        /// </summary>
        private void ViewsClose()
        {
            FileViews = "A";
            Files_Entity = new List<UpFile_Entity>();
        }


        /// <summary>
        /// 첨부파일 삭제
        /// </summary>
        /// <param name="sw_Files"></param>
        private async Task FilesRemove(UpFile_Entity _files)
        {
            bool isDelete = await JSRuntime.InvokeAsync<bool>("confirm", $"{_files.FileName} 첨부파일을 정말로 삭제하시겠습니까?");

            if (isDelete)
            {
                if (!string.IsNullOrEmpty(_files?.FileName))
                {
                    // 첨부 파일 삭제 
                    //await FileStorageManagerInjector.DeleteAsync(_files.Sw_FileName, "");
                    try
                    {
                        string rootFolder = $"D:\\Msh\\Msh\\sw_Web\\Files_Up\\A_Files\\{ _files.FileName}";
                        File.Delete(rootFolder);
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
                await files_Lib.Remove_UpFile(_files.Aid.ToString());

                await labor.Files_Count_Add(ann.Aid, "B"); //파일 수 줄이기
                //await UploadRepositoryAsyncReference.DeleteAsync(Id); // 삭제
                //NavigationManagerReference.NavigateTo("/Uploads"); // 리스트 페이지로 이동
                Files_Count = await files_Lib.UpFile_Count("ContractOfEmployment", ann.Aid.ToString(), Apt_Code);
                if (Files_Count > 0)
                {
                    Files_Entity = await files_Lib.UpFile_List("ContractOfEmployment", ann.Aid.ToString(), Apt_Code);
                }
                else
                {
                    Files_Entity = new List<UpFile_Entity>();
                }
            }
            else
            {
                await JSRuntime.InvokeAsync<object>("alert", "취소되었습니다.");
            }
        }


        #region Event Handlers

        [Inject] ILogger<Index> Logger { get; set; }
        private List<IBrowserFile> loadedFiles = new();
        private long maxFileSize = 1024 * 1024 * 30;
        private int maxAllowedFiles = 5;
        private bool isLoading;
        private decimal progressPercent;
        //Sw_Files_Entity finn { get; set; } = new Sw_Files_Entity();
        [Inject] Microsoft.AspNetCore.Hosting.IWebHostEnvironment env { get; set; }
        public string fileName { get; set; }

        private async Task LoadFiles(InputFileChangeEventArgs e)
        {
            isLoading = true;
            loadedFiles.Clear();
            progressPercent = 0;

            foreach (var file in e.GetMultipleFiles(maxAllowedFiles))
            {
                dnn.Cnn_Code = ann.Aid.ToString(); // 선택한 ParentId 값 가져오기 
                dnn.Code = dnn.Cnn_Code;
                try
                {
                    var pathA = $"D:\\Msh\\Home\\Ayoung\\sw_Files_View\\Sw_files";


                    string strFiles = Path.GetExtension(file.Name);

                    string _FileName = "ContractOfEmploymen" + Apt_Code + DateTime.Now.ToShortDateString() + strFiles;

                    fileName = Dul.FileUtility.GetFileNameWithNumbering(pathA, _FileName);

                    //var trustedFileName = Path.GetRandomFileName();
                    var path = Path.Combine(pathA, fileName);



                    await using FileStream writeStream = new(path, FileMode.Create);
                    using var readStream = file.OpenReadStream(maxFileSize);
                    var bytesRead = 0;
                    var totalRead = 0;
                    var buffer = new byte[1024 * 1024];

                    while ((bytesRead = await readStream.ReadAsync(buffer)) != 0)
                    {
                        totalRead += bytesRead;

                        await writeStream.WriteAsync(buffer, 0, bytesRead);

                        progressPercent = Decimal.Divide(totalRead, file.Size);

                        StateHasChanged();
                    }

                    loadedFiles.Add(file);

                    dnn.Sort = "ContractOfEmployment";
                    dnn.Code = dnn.Sort;
                    dnn.FileName = fileName;
                    dnn.FileSize = Convert.ToInt32(file.Size);
                    dnn.Cnn_Name = "ContractOfEmployment";
                    dnn.Apt_Code = Apt_Code;

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
                    dnn.PostIP = myIPAddress;
                    #endregion

                    await files_Lib.Add_UpFile(dnn); //첨부파일 데이터 db 저장

                    await labor.Files_Count_Add(ann.Aid, "A"); // 첨부파일 추가를 db 저장(문서관리, Document)

                    //FileInputViews = "A";
                }
                catch (Exception ex)
                {
                    Logger.LogError("File: {Filename} Error: {Error}",
                        file.Name, ex.Message);
                }
            }

            await DisplayData();
            FileInputViews = "A";

            Files_Count = await files_Lib.UpFile_Count(dnn.Cnn_Name, ann.Aid.ToString(), Apt_Code);
            if (Files_Count > 0)
            {
                Files_Entity = await files_Lib.UpFile_List(dnn.Cnn_Name, ann.Aid.ToString(), Apt_Code);
            }

            isLoading = false;
        }



        

        
        #endregion

        private async Task OnFileViews()
        {
            FileViews = "B";
            Files_Count = await files_Lib.UpFile_Count("ContractOfEmployment", ann.Aid.ToString(), Apt_Code);
            if (Files_Count > 0)
            {
                Files_Entity = await files_Lib.UpFile_List("ContractOfEmployment", ann.Aid.ToString(), Apt_Code);
            }
        }

        /// <summary>
        /// 승인하기
        /// </summary>
        private async Task OnApproval(Labor_contract_Entity _labor)
        {
            if (LevelCount >= 10)
            {
                if (_labor.Division == "A")
                {
                    await labor.Approval(_labor.Aid.ToString());
                }
                else
                {
                    await labor.Approval(_labor.Aid.ToString());
                }
            }
            else
            {
                await JSRuntime.InvokeAsync<object>("alert", "권한이 없습니다.");
            }
        }

        /// <summary>
        /// 승인
        /// </summary>
        private async Task OnApproval(int Aid)
        {
            if (LevelCount >= 10)
            {
                await labor.Approval(Aid.ToString());
                await DisplayData();
            }
            else
            {
                await JSRuntime.InvokeAsync<object>("alert", "권한이 없습니다.");
            }
        }

        /// <summary>
        /// 승인
        /// </summary>
        private async Task OnApprovalB(int Aid)
        {
            if (LevelCount >= 10)
            {
                await labor.Approval(Aid.ToString());
                await DisplayData();
            }
            else
            {
                await JSRuntime.InvokeAsync<object>("alert", "권한이 없습니다.");
            }
        }

        #region 근로계약 새로 개발

        private void btnCloseV()
        {
            InsertViews = "A";
        }

        #endregion

    }
}
