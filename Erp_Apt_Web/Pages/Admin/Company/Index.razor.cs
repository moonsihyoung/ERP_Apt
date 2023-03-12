using Erp_Apt_Lib.Appeal;
using Erp_Apt_Lib;
using Erp_Lib;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Threading.Tasks;
using Company;
using Erp_Apt_Staff;
using System.Collections.Generic;
using Erp_Entity;
using System;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.Hosting;
using Erp_Apt_Lib.Community;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections;
using System.Runtime.InteropServices;

namespace Erp_Apt_Web.Pages.Admin.Company
{
    public partial class Index
    {
        [CascadingParameter] public Task<AuthenticationState> AuthenticationStateRef { get; set; }
        [Inject] public IApt_Lib apt_Lib { get; set; }
        [Inject] public NavigationManager MyNav { get; set; }
        [Inject] IJSRuntime JSRuntime { get; set; }
        [Inject] public ISw_Files_Lib files_Lib { get; set; } // 파일 첨부        
        [Inject] Microsoft.AspNetCore.Hosting.IWebHostEnvironment env { get; set; }

        [Inject] public IApt_Sub_Lib apt_Sub_Lib { get; set; }
        [Inject] public ISido_Lib sido_Lib { get; set; }
        [Inject] public IStaff_Lib staff_Lib { get; set; }
        [Inject] public IStaff_Sub_Lib staff_Sub_Lib { get; set; }
        [Inject] public IReferral_career_Lib referral_Career_Lib { get; set; }
        [Inject] public ICompany_Lib company_Lib { get; set; }
        [Inject] public ICompany_Apt_Career_Lib company_Apt_Career_Lib { get; set; }
        [Inject] public IContract_Sort_Lib contract_Sort_Lib { get; set; }

        public List<Company_Career_Entity> ann { get; set; } = new List<Company_Career_Entity>();
        public Company_Career_Entity bnn { get; set; } = new Company_Career_Entity();
        public List<Apt_Entity> apt { get; set; } = new List<Apt_Entity>();
        public List<Company_Entity> cop { get; set; } = new List<Company_Entity>();
        public List<Staff_Entity> stf { get; set; } = new List<Staff_Entity>();


        #region 변수
        public string Views { get; set; } = "A";
        public string InsertViews { get; set; } = "A";
        public string Apt_Code { get; set; }
        public string User_Code { get; set; }
        public string Apt_Name { get; set; }
        public string User_Name { get; set; }
        public int LevelCount { get; set; }
        public string strTitle { get; set; }
        public string strApt { get; set; }
        public string strCor_Code { get; set; } = "sw5";
        public string strSort { get; set; } = "A";

        //private ElementReference myref;
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

        /// <summary>
        /// 방문 시 실행
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
                User_Name = authState.User.Claims.FirstOrDefault(c => c.Type == "User_Name")?.Value;
                User_Name = authState.User.Claims.FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name")?.Value;
                LevelCount = Convert.ToInt32(authState.User.Claims.FirstOrDefault(c => c.Type == "LevelCount")?.Value);

                if (LevelCount > 5)
                {                   
                    await DisplayData();
                }
                else
                {
                    await JSRuntime.InvokeAsync<object>("alert", "권한이 없습니다.");
                    MyNav.NavigateTo("/");
                }
            }
            else
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "로그인되지 않았습니다. 먼저 로그인하세요.");
                //MyNav.NavigateTo("Logs/Index");
            }
        }

        /// <summary>
        /// 관리자명
        /// </summary>
        /// <param name="Apt_Code"></param>
        /// <returns></returns>
        private string Staff_Name(string Apt_Code)
        {
            return staff_Lib.Staff_Name(Apt_Code);
        }

        /// <summary>
        /// 계약정보 목록
        /// </summary>
        /// <returns></returns>
        private async Task DisplayData()
        {
            if (strSort == "A")
            {
                pager.RecordCount = await company_Apt_Career_Lib.Getcount_apt_all("sw5");
                ann = await company_Apt_Career_Lib.getlist_apt_all(pager.PageIndex, "sw5");
            }
            else
            {
                pager.RecordCount = await company_Apt_Career_Lib.getlist_count("Cb45", "sw5");
                ann = await company_Apt_Career_Lib.getlist(pager.PageIndex, "Cb45", "sw5");
            }
        }
        #endregion

        /// <summary>
        /// 계약정보 수정
        /// </summary>
        /// <param name="ar"></param>
        private void ByEdit(Company_Career_Entity ar)
        {
            bnn = ar;
            if (bnn.Tender == "::입찰선택::")
            {
                bnn.Tender = "";
            }

            if (bnn.Bid == "::낙찰선택::")
            {
                bnn.Bid = "";
            }

            strTitle = "위탁관리 정보 수정";

            strDate = bnn.Contract_end_date.ToShortDateString();
            InsertViews = "B";
        }

        /// <summary>
        /// 계약정보 삭제
        /// </summary>
        /// <param name="ar"></param>
        /// <returns></returns>
        private async Task ByRemove(Company_Career_Entity ar)
        {
            if (LevelCount >= 10)
            {
                bool isDelete = await JSRuntime.InvokeAsync<bool>("confirm", $"{ar.Aid} 번 계약정보을 정말로 삭제하시겠습니까?");

                if (isDelete)
                {
                    await company_Apt_Career_Lib.delete(ar.Aid.ToString(), "B");
                }
                await DisplayData();
            }
        }

        /// <summary>
        /// 계약정보 상세보기
        /// </summary>
        /// <param name="ar"></param>
        private void ByDetails(Company_Career_Entity ar)
        {
            bnn = ar;
            strTitle = "위탁계약 상세정보";
            Views = "B";
        }

        /// <summary>
        /// 상세보기 닫기
        /// </summary>
        private void btnCloseV()
        {
            Views = "A";
        }

        protected string SortName(string Code)
        {
            if (Code != null)
            {
                return contract_Sort_Lib._Name(Code);
            }
            else
            {
                return "없음";
            }
        }

        /// <summary>
        /// 계약정보 입력
        /// </summary>
        private void btnOpen()
        {
            strTitle = "위탁계약 새로 입력";
            bnn.Contract_end_date = DateTime.Now;
            bnn.Contract_start_date = DateTime.Now;
            strDate = DateTime.Now.ToShortDateString();

            InsertViews = "B";
        }

        /// <summary>
        /// 계약 전체 정보
        /// </summary>
        private async Task btnAllList()
        {
            strSort = "B";
            await DisplayData();
        }

        private async Task OnAptList(ChangeEventArgs a)
        {
            apt = await apt_Lib.SearchList(a.Value.ToString());
        }

        /// <summary>
        /// 위탁계약 저장
        /// </summary>
        private async Task btnSave()
        {
            if (string.IsNullOrWhiteSpace(bnn.Apt_Code))
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "공동주택 코드를 입력하지 않았습니다..");
            }
            else if (string.IsNullOrWhiteSpace(bnn.Apt_Name))
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "공동주택 명을 입력하지 않았습니다..");
            }
            else if (string.IsNullOrWhiteSpace(bnn.Company_Name))
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "업체명을 입력하지 않았습니다..");
            }
            else if (string.IsNullOrWhiteSpace(bnn.Cor_Code))
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "업체 코드를 입력하지 않았습니다..");
            }
            else if (bnn.Contract_Sum < 1000)
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "계약금액을 입력하지 않았습니다..");
            }
            else if (string.IsNullOrWhiteSpace(bnn.Tender))
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "입찰방법을 입력하지 않았습니다..");
            }
            else if (string.IsNullOrWhiteSpace(bnn.Bid))
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "낙찰방법을 입력하지 않았습니다..");
            }
            else if (bnn.Contract_end_date <= DateTime.Now)
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "계약 종료일을 입력하지 않았습니다..");
            }
            else
            {
                if (bnn.Aid < 1)
                {
                    bnn.ContractMainAgent = "입주자대표회의";
                    bnn.Contract_date = DateTime.Now;
                    bnn.Intro = "기재하지 않음";

                    await company_Apt_Career_Lib.add(bnn);
                }
                else
                {
                    await company_Apt_Career_Lib.edit(bnn);
                }

                bnn = new Company_Career_Entity();
                await DisplayData();
            }                   
        }

        /// <summary>
        /// 위탁 계약 입력 모달 닫기
        /// </summary>
        private void btnClose()
        {
            InsertViews = "A";
        }

        /// <summary>
        /// 아파트 검색
        /// </summary>
        private void OnApt(Apt_Entity apt_)
        {
            bnn.CC_Code = apt_.Apt_Code + "_" + strCor_Code;
            bnn.Apt_Code = apt_.Apt_Code;
            bnn.Apt_Name = apt_.Apt_Name;
            bnn.Cor_Code = strCor_Code;
            bnn.Company_Name = "신원티엠씨(TMC)";
            bnn.ContractMainAgent = "입주자대표회의";
            bnn.ContractSort = "Cb45";            
        }

        /// <summary>
        ///  계약일 수 계산
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
                ddate = referral_Career_Lib.Date_scomp(start_a, end_a);
            }
            else
            {
                DateTime start = Convert.ToDateTime(objstart);
                DateTime end = Convert.ToDateTime(objend);
                string start_a = start.ToShortDateString();
                string end_a = end.ToShortDateString(); //DateTime.Now.ToShortDateString();
                ddate = referral_Career_Lib.Date_scomp(start_a, end_a);
            }

            return string.Format("{0: ###,###}", ddate);
        }

        /// <summary>
        /// 종료일 선택 실행
        /// </summary>
        public string strDate { get; set; }
        private void OnDate(ChangeEventArgs a)
        {
            bnn.Contract_end_date = Convert.ToDateTime(a.Value);
            strDate = bnn.Contract_end_date.ToShortDateString();
            bnn.Contract_Period = Convert.ToInt32(Func_span(bnn.Contract_start_date, bnn.Contract_end_date).Replace(",",""));
        }
    }
}
