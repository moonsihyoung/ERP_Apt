using Company;
using Erp_Apt_Staff;
using Erp_Entity;
using Erp_Lib;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Erp_Apt_Web.Pages.Staff
{
    public partial class Placement_Index
    {
        #region 인스턴스
        [CascadingParameter] public Task<AuthenticationState> AuthenticationStateRef { get; set; }
        [Inject] public NavigationManager MyNav { get; set; } // Url 
        [Inject] IJSRuntime JSRuntime { get; set; }
        [Inject] public IApt_Lib apt_Lib { get; set; }
        [Inject] public IApt_Sub_Lib apt_Sub_Lib { get; set; }
        [Inject] public ISido_Lib sido_Lib { get; set; }
        [Inject] public IStaff_Lib staff_Lib { get; set; }
        [Inject] public IStaff_Sub_Lib staff_Sub_Lib { get; set; }
        [Inject] public IReferral_career_Lib referral_Career_Lib { get; set; }
        [Inject] public ICompany_Lib company_Lib { get; set; }
        [Inject] public ICompany_Apt_Career_Lib company_Career_Lib { get; set; }
        [Inject] public IPost_Lib post_Lib { get; set; }
        [Inject] public IDuty_Lib duty_Lib { get; set; }
        #endregion

        #region 속성
        Apt_Entity apt { get; set; } = new Apt_Entity();
        Apt_Sub_Entity aptsub { get; set; } = new Apt_Sub_Entity();
        Staff_Entity snn { get; set; } = new Staff_Entity();
        Staff_Sub_Entity bnn { get; set; } = new Staff_Sub_Entity();
        Referral_career_Entity cnn { get; set; } = new Referral_career_Entity();
        Company_Career_Entity ccn { get; set; } = new Company_Career_Entity();

        List<Apt_Entity> aptA { get; set; } = new List<Apt_Entity>();
        List<Staff_Entity> snnA { get; set; } = new List<Staff_Entity>();
        List<Referral_career_Entity> cnnA { get; set; } = new List<Referral_career_Entity>();
        List<Referral_career_Entity> cnnB { get; set; } = new List<Referral_career_Entity>();
        List<Post_Entity> pnnA { get; set; } = new List<Post_Entity>();
        List<Duty_Entity> dnnA { get; set; } = new List<Duty_Entity>();
        List<Sido_Entity> sidos { get; set; } = new List<Sido_Entity>();

        #endregion

        #region 변수
        public string Views { get; set; } = "A";
        public string InsertViews { get; set; } = "A";
        public string Apt_Code { get; set; }
        public string User_Code { get; set; }
        public string Apt_Name { get; set; }
        public string User_Name { get; set; }
        public int LevelCount { get; set; }
        public string strTitle { get; set; }
        public string ResignViews { get; set; } = "A";
        //private ElementReference myref;
        public string strField { get; set; }
        public string strQuery { get; set; }
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


                if (LevelCount < 5)
                {
                    await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "권한이 없습니다.");
                    MyNav.NavigateTo("/");
                }
                else
                {
                    pnnA = await post_Lib.GetListAll();
                    dnnA = await duty_Lib.GetList_B("1");
                    await DisplayData();
                }

            }
            else
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "로그인하지 않았습니다.");
                MyNav.NavigateTo("/");
            }
        }

        /// <summary>
        /// 데이터 정보 
        /// </summary>
        private async Task DisplayData()
        {
            pager.RecordCount = await referral_Career_Lib.GetList_Sojang_CountA();
            cnnA = await referral_Career_Lib.GetList_SojangA(pager.PageIndex);
        }

        /// <summary>
        /// 상세
        /// </summary>
        public async Task ByAid(Referral_career_Entity career)
        {
            cnn = career;
            snn = await staff_Lib.View(cnn.User_ID);
            bnn = await staff_Sub_Lib.View(cnn.User_ID);
            apt = await apt_Lib.Details(cnn.Apt_Code);
            cnnB = await referral_Career_Lib._Career_Name_Search("User_ID", cnn.User_ID);
            Views = "B";
            strTitle = "배치 관리자 상세정보";
        }

        /// <summary>
        /// 수정
        /// </summary>
        public void ByEdit(Referral_career_Entity career)
        {
            strTitle = "배치 정보 수정";
            cnn = career;
            InsertViews = "B";
            //bnn = await apt_Sub_Lib.Detail(apt.Apt_Code);
        }

        /// <summary>
        /// 삭제
        /// </summary>
        public async Task ByRemove(int Aid)
        {
            bool isDelete = await JSRuntime.InvokeAsync<bool>("confirm", $"{Aid} 첨부파일을 정말로 삭제하시겠습니까?");
            if (isDelete)
            {
                await referral_Career_Lib.delete(Aid.ToString());
                //await apt_Sub_Lib.Remove(Aid);
                await DisplayData();
            }
        }

        /// <summary>
        /// 새로 등록 열기
        /// </summary>
        private void onOpen()
        {
            strTitle = "배치정보 등록";
            cnn = new Referral_career_Entity();
            cnn.Career_Start_Date = DateTime.Now.Date;
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
        /// 근무기간
        /// </summary>
        private string Func_span(object objstart, object objend, string Division)
        {
            int ddate = 0;
            DateTime date = Convert.ToDateTime(objend);
            if (Division == "A")
            {
                DateTime start = Convert.ToDateTime(objstart);
                //DateTime end = Convert.ToDateTime(objend);
                string start_a = start.ToShortDateString();
                string end_a = DateTime.Now.ToShortDateString();   //end.ToShortDateString();
                ddate = referral_Career_Lib.Date_scom(start_a, end_a);
            }
            else
            {
                DateTime start = Convert.ToDateTime(objstart);
                DateTime end = Convert.ToDateTime(objend);
                string start_a = start.ToShortDateString();
                string end_a = end.ToShortDateString();
                if (end_a == null)
                {
                    end_a = start_a;
                }
                //DateTime.Now.ToShortDateString();
                ddate = referral_Career_Lib.Date_scom(start_a, end_a);
            }

            return string.Format("{0: ###,###}", ddate);
        }

        /// <summary>
        /// 업체명 불러오기
        /// </summary>
        private string Company_Name(string Code)
        {
            return company_Lib.Company_Name_Code_A(Code);
        }

        /// <summary>
        /// 배치할 단지명 찾기
        /// </summary>
        private async Task Onsearch(ChangeEventArgs a)
        {
            aptA = await apt_Lib.SearchList(a.Value.ToString());
        }

        /// <summary>
        /// 공동주택 선택 실행
        /// </summary>
        private async Task OnApt(ChangeEventArgs a)
        {
            cnn.Apt_Code = a.Value.ToString();
            cnn.Apt_Name = await apt_Lib.Apt_Name(cnn.Apt_Code);  
        }

        /// <summary>
        /// 배치할 관리자 이름으로 검색
        /// </summary>
        private async Task onStaffName(ChangeEventArgs a)
        {
            snnA = await staff_Lib.Staff_Name_SearchA(a.Value.ToString());
        }

        /// <summary>
        /// 배치 관리자 실행
        /// </summary>
        private async Task OnUserName(ChangeEventArgs a)
        {
            cnn.User_ID = a.Value.ToString();
            cnn.User_Name = await staff_Lib.Name(cnn.User_ID);
        }

        /// <summary>
        /// 배치정보 입력
        /// </summary>
        /// <returns></returns>
        private async Task btnSave()
        {
            int BeingApt = await referral_Career_Lib.be_apt(cnn.User_ID, cnn.Apt_Code); //현재 배치 여부 확인(배치되어 있으면 먼저 퇴사 처리되어야 배치 가능)

            if (BeingApt < 1)
            {
                int BeContect = await company_Career_Lib.BeApt(cnn.Apt_Code, "C63088");// 위탁계약 존재 여부확인 및 계약 기간 종료 여부 확인
                if (BeContect >= 1)
                {
                    int intRC = await referral_Career_Lib.being(cnn.User_ID); // 선택된 관리자 다른 단지에 배치되어 있는지 확인
                    if (intRC > 0)
                    {
                        cnn.Staff_Cd = cnn.User_ID;                        
                        cnn.Post_Code = "A";

                        ccn = await company_Career_Lib.BeAptCompany_Code(cnn.Apt_Code, "C63088");
                        cnn.Cor_Code = ccn.Cor_Code;
                        cnn.ContractSort_Code = ccn.ContractSort;
                        cnn.CC_Code = ccn.CC_Code;

                        if (cnn.Aid < 1)
                        {
                            await referral_Career_Lib.Add_rc(cnn); // 새로 등록
                            await DisplayData();
                        }
                        else
                        {
                            await referral_Career_Lib.Edit_rc_A(cnn); // 수정
                        }

                        cnn = new Referral_career_Entity();
                        InsertViews = "A";
                    }
                    else
                    {
                        await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", cnn.User_Name + "이 다른 공동주택에 배치된 상태입니다. \n 퇴사를 먼저 입력하신 후에 다시 시도해 보시기 바랍니다.");
                    }
                }
                else
                {
                    await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", cnn.Apt_Name + "와 맺은 위탁 계약 내용을 찾을 수 없습니다. \n 위탁계약 기간이 종료되었는지 확인하시거나, \n 위탁계약 내용을 입력하시고 다시 시도해 보시기 바랍니다.");
                }
            }
            else
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", cnn.User_Name + "은 아직 다른 공동 주택에 배치되어 있습니다.");
            }
        }

        /// <summary>
        /// 퇴사 열기
        /// </summary>
        /// <param name="Aid"></param>
        private void ByResignation(Referral_career_Entity RC)
        {
            cnn = RC;
            cnn.Career_End_Date = DateTime.Now.Date;
            ResignViews = "B";
            strTitle = "퇴사 처리 입력";
        }

        /// <summary>
        /// 상세보기 닫기
        /// </summary>
        private void btnCloseA()
        {
            Views = "A";
        }

        /// <summary>
        /// 배치정보 찾기
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        private async Task onQruery(ChangeEventArgs a)
        {
            if (a.Value.ToString() == "Z" || a.Value == null || a.Value.ToString() == "")
            {
                await DisplayData();
            }
            else
            {
                cnnA = await referral_Career_Lib._Career_Feild_Search(strField, a.Value.ToString());
            }
        }

        /// <summary>
        /// 퇴사입력 닫기
        /// </summary>
        private void btnCloseB()
        {
            ResignViews = "A";
        }

        /// <summary>
        /// 퇴사 입력
        /// </summary>
        /// <returns></returns>
        private async Task btnSaveB()
        {
            if (cnn.Division == "A")
            {
                cnn.Division = "B";
                await referral_Career_Lib.Resign(cnn.Career_Start_Date, cnn.Division, cnn.Etc, cnn.Aid);
            }
            else
            {
                cnn.Division = "A";
                await referral_Career_Lib.Resign(cnn.Career_Start_Date, cnn.Division, cnn.Etc, cnn.Aid);
            }
        }
    }
}
