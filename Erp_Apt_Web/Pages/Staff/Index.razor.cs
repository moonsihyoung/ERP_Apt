using Erp_Apt_Lib;
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
using Erp_Apt_Lib.Logs;

namespace Erp_Apt_Web.Pages.Staff
{
    public partial class Index
    {
        #region 인스턴스
        [CascadingParameter] public Task<AuthenticationState> AuthenticationStateRef { get; set; }
        [Inject] public NavigationManager MyNav { get; set; } // Url 
        [Inject] IJSRuntime JSRuntime { get; set; }
        [Inject] public IReferral_career_Lib referral_Career_Lib { get; set; }
        [Inject] public IStaff_Lib staff_Lib { get; set; }
        [Inject] public IStaff_staffSub_Lib staff_StaffSub_Lib { get; set; }
        [Inject] public IStaff_Sub_Lib staff_Sub_Lib { get; set; }
        [Inject] public ISido_Lib sido_Lib { get; set; } 
        [Inject] public IPost_Lib post_Lib { get; set; }
        [Inject] public IDuty_Lib duty_Lib { get; set; }
        [Inject] public ICompany_Apt_Career_Lib company_Career_Lib { get; set; }
        [Inject] public ILogs_Lib logs_Lib { get; set; }
        
        #endregion

        #region 변수
        public string User_Code { get; set; }
        public string User_Name { get; set; }
        public string Apt_Code { get; set; }
        public string Apt_Name { get; set; }
        public int LevelCount { get; set; } = 0;
        public string Views { get; set; } = "A"; //상세 열기
        public string RemoveViews { get; set; } = "A";//삭제 열기
        public string InsertViews { get; set; } = "A"; // 입력 열기
        public string CareerViews { get; set; } = "A";
        public string FileViews { get; set; }
        public int Files_Count { get; set; } = 0;
        public int intIdBe { get; set; } = 0;
        public string D_Division { get; set; }
        public string Password_sw { get; set; }
        public string SiGunGu { get; set; }
        public string StaffName { get; set; }
        public string StaffCode { get; set; }
        public DateTime Scn { get; set; }
        public DateTime Career_End_Date { get; set; } = DateTime.Now;
        public int intAid { get; set; } = 0;
        public string Sido { get; set; }
        public string SidoR { get; set; }
        public string StaffSearch { get; set; } = "A";
        public string CompanySearch { get; set; } = "A";
        public string Company_name { get; set; }
        #endregion

        #region 속성
        List<Staff_Career_Entity> ann { get; set; } = new List<Staff_Career_Entity>();
        public int intNum { get; private set; }
        Staff_Career_Entity rnn { get; set; } = new Staff_Career_Entity();
        Referral_career_Entity rce { get; set; } = new Referral_career_Entity();
        public Staff_Entity bnnA { get; set; } = new Staff_Entity();
        public Staff_StaffSub_Entity staffSub { get; set; } = new Staff_StaffSub_Entity();
        public List<Staff_Sub_Entity> staffA { get; set; } = new List<Staff_Sub_Entity>();
        public Staff_Sub_Entity bnnB { get; set; } = new Staff_Sub_Entity();
        public Sw_Files_Entity dnn { get; set; } = new Sw_Files_Entity();
        public List<Sw_Files_Entity> Files_Entity { get; set; } = new List<Sw_Files_Entity>();
        public List<Sido_Entity> sigungu { get; set; } = new List<Sido_Entity>();
        public List<Sido_Entity> sigungu_Career { get; set; } = new List<Sido_Entity>();
        public Sido_Entity sido_Entity { get; set; } = new Sido_Entity();
        public List<Post_Entity> Post { get; set; } = new List<Post_Entity>();
        public List<Duty_Entity> Duty { get; set; } = new List<Duty_Entity>();
        public Logs_Entites logs { get; set; } = new Logs_Entites();
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
                User_Name = authState.User.Claims.FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name")?.Value;
                LevelCount = Convert.ToInt32(authState.User.Claims.FirstOrDefault(c => c.Type == "LevelCount")?.Value);

                #region 로그 파일 만들기
                logs.Note = "직원관리에 들어왔습니다."; logs.Logger = User_Code; logs.Application = "직원관리"; logs.ipAddress = ""; logs.Message = Apt_Code + " " + Apt_Name;
                await logs_Lib.add(logs);
                #endregion

                if (LevelCount >= 5)
                {
                    await DisplayData("", "");
                    Post = await post_Lib.GetList("A");
                }
                else
                {
                    await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "권한이 없습니다.");
                    MyNav.NavigateTo("/");
                }
            }
            else
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "로그인되지 않았습니다.");
                MyNav.NavigateTo("/");
            }
        }

        /// <summary>
        /// 배치정보 목록 불러오기
        /// </summary>
        private async Task DisplayData(string Code, string Division)
        {
            if (Code == "" || Code == null)
            {
                pager.RecordCount = await referral_Career_Lib.Staff_Career_Join_Count(Apt_Code);
                ann = await referral_Career_Lib.Staff_Career_Join(pager.PageIndex, Apt_Code);
            }
            else
            {
                pager.RecordCount = await referral_Career_Lib.GetListStaffCarrerSearchCount(Apt_Code, Code, Division);
                ann = await referral_Career_Lib.GetListStaffCarrerSearch(pager.PageIndex, Apt_Code, Code, Division);
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
        /// 페이징
        /// </summary>
        protected async void PageIndexChanged(int pageIndex)
        {
            pager.PageIndex = pageIndex;
            pager.PageNumber = pageIndex + 1;

            await DisplayData(strSortSearch, strDivision);

            StateHasChanged();
        }

        /// <summary>
        /// 직원 정보 입력
        /// </summary>
        /// <returns></returns>
        public async Task btnStaffSave()
        {
            bnnA.M_Apt_Code = Apt_Code;
            bnnA.Scn = Scn.ToShortDateString();
            bnnA.Scn_Code = bnnA.Scn.Replace("-", "");
            bnnA.User_ID = Apt_Code + await staff_Lib.Apt_Number_Count(Apt_Code);

            bnnA.Staff_Cd = bnnA.User_ID;
            if (bnnB.Etc == null || bnnB.Etc == "")
            {
                bnnB.Etc = "관리자가 입력했으므로 입력하지 않았습니다.";
                bnnA.Intro = bnnB.Etc;
            }
            else
            {
                bnnA.Intro = bnnB.Etc;
            }
            bnnA.JoinDate = DateTime.Now;
            bnnA.LevelCount = 3;
            bnnA.Password_sw = bnnA.User_ID + "pw";
            bnnA.Sido = bnnB.st_Sido;
            bnnA.SiGunGu = bnnB.st_GunGu;
            bnnA.RestAdress = bnnB.st_Adress_Rest;
            bnnA.LevelCount = 3;

            if (D_Division == null || D_Division == "")
            {
                bnnB.d_division = "기타";
            }
            else
            {
                bnnB.d_division = D_Division;
            }
            bnnB.M_Apt_Code = bnnA.M_Apt_Code;
            bnnB.Staff_Cd = bnnA.User_ID;
            bnnB.User_ID = bnnA.User_ID;
            bnnB.Staff_Name = bnnA.User_Name;
            bnnB.Staff_Sub_Cd = bnnA.User_ID;
            bnnB.levelcount = 3;
            bnnB.End_Date = DateTime.Now;            

            if (string.IsNullOrWhiteSpace(bnnA.M_Apt_Code))
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "단지 코드를 입력하지 않았습니다.");
                MyNav.NavigateTo("/");
            }
            else if (string.IsNullOrWhiteSpace(bnnA.Scn))
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "출생일을 입력하지 않았습니다.");
            }
            else if (string.IsNullOrWhiteSpace(bnnA.User_ID))
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "아이디를 입력하지 않았습니다.");
            }            
            else if (string.IsNullOrWhiteSpace(bnnA.User_Name))
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "직원명을 입력하지 않았습니다.");
            }
            else if (string.IsNullOrWhiteSpace(bnnB.Staff_Name))
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "직원명을 입력하지 않았습니다.");
            }
            else if (string.IsNullOrWhiteSpace(bnnB.st_Sido))
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "시도를 선택하지 않았습니다.");
            }
            else if (string.IsNullOrWhiteSpace(bnnB.st_GunGu))
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "시군구를 입력하지 않았습니다.");
            }
            else if (string.IsNullOrWhiteSpace(bnnB.Mobile_Number))
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "핸드폰 전화번호를 입력하지 않았습니다.");
            }
            else if (string.IsNullOrWhiteSpace(bnnB.d_division))
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "구분을 선택하지 않았습니다.");
            }
            else if (string.IsNullOrWhiteSpace(bnnB.Etc))
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "소개를 설명하지 않았습니다.");
            }
            else
            {
                try
                {
                    bnnB.Mobile_Number = bnnB.Mobile_Number.Replace("-", "").Replace(" ", "");
                    string cp_Num = bnnB.Mobile_Number;
                    cp_Num = cp_Num.Insert(3, "-");
                    cp_Num = cp_Num.Insert(8, "-");
                    bnnB.Mobile_Number = cp_Num;
                }
                catch (Exception)
                {
                    await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "핸드폰 전화번호를 입력하지 않았습니다.");
                }

                int su = await staff_StaffSub_Lib.GetListInsertCount(bnnA.User_Name, bnnB.Mobile_Number);
                if (su < 1)
                {
                    if (bnnA.Aid < 1)
                    {
                        bnnA.Scn = Scn.ToShortDateString();
                        try
                        {
                            await staff_Lib.Add(bnnA);
                            try
                            {
                                await staff_Sub_Lib.Add(bnnB);
                                bnnA = new Staff_Entity();
                                bnnB = new Staff_Sub_Entity();
                            }
                            catch (Exception)
                            {
                                await staff_Lib.Remove(bnnA.User_ID);
                                await JSRuntime.InvokeAsync<object>("alert", "직원 기본 정보 입력에 실패했습니다.");
                            }                            
                        }
                        catch (Exception)
                        {
                            await JSRuntime.InvokeAsync<object>("alert", "작원 상세 정보 입력에 실패했습니다.");
                        }
                        
                        #region 로그 파일 만들기
                        //logs.Note = bnnA.User_Name + " " + bnnA.User_ID + " " + bnnA.Scn + " " + bnnA.JoinDate.ToShortDateString() + "입력했습니다."; logs.Logger = User_Code; logs.Application = "직원관리"; logs.ipAddress = ""; logs.Message = Apt_Code + " " + Apt_Name;
                        //await logs_Lib.add(logs);
                        #endregion
                    }
                    else
                    {
                        await staff_Lib.Edit(bnnA);
                        await staff_Sub_Lib.Edit(bnnB);
                        bnnA = new Staff_Entity();
                        bnnB = new Staff_Sub_Entity();
                    }

                    InsertViews = "A";
                }
                else
                {
                    await JSRuntime.InvokeAsync<object>("alert", "해당 성명과 핸드폰 번호로 입력된 정보가 있어서 입력되지 않았습니다.");
                }
            }
        }

        /// <summary>
        /// 배치정보 상세
        /// </summary>
        /// <param name="career_Entity"></param>
        private void ByDetails(Staff_Career_Entity career_Entity)
        {
            Views = "B";
            rnn = career_Entity;
            //StateHasChanged();
        }        

        /// <summary>
        /// 퇴사처리 열기
        /// </summary>
        private void ByRemove(Staff_Career_Entity career)
        {
            RemoveViews = "B";
            intAid = career.Aid;//퇴사 처리를 위한 일련 번호 등록
            rnn = career;
            //StateHasChanged();
        }

        /// <summary>
        /// 직원 새로 등록
        /// </summary>
        private void onInsertViews()
        {
            InsertViews = "B";
            bnnA.JoinDate = DateTime.Now;
            Scn = DateTime.Now;
            bnnB.Start_Date = DateTime.Now;
            //StateHasChanged();
        }

        /// <summary>
        /// 시도 선택 시 시군구 만들기
        /// </summary>
        public async Task OnSido(ChangeEventArgs args)
        {
            //bnnA.JoinDate = DateTime.Now;
            //bnnB.st_Sido = args.Value.ToString();
            sigungu = await sido_Lib.GetList_Code(args.Value.ToString());
            //StateHasChanged();
        }

        /// <summary>
        /// 아이디 중복 확인
        /// </summary>
        public async Task OnSearchID()
        {

            if (bnnA.Password_sw == Password_sw)
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "암호가 일치합니다..");
                intIdBe = 0;
            }
            else
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "암호가 일치하지 않습니다..");
                intIdBe = 1;
            }
        }

        /// <summary>
        /// 자격증 등록 
        /// </summary>
        /// <param name="args"></param>
        public void OnD_Division(ChangeEventArgs args)
        {
            if (D_Division != null)
            {
                D_Division = D_Division + ", " + args.Value.ToString();
            }
            else
            {
                D_Division = args.Value.ToString();
            }

            bnnB.d_division = D_Division;
        }

        /// <summary>
        /// 시군구 선택 실행
        /// </summary>
        public async Task onGunGu(ChangeEventArgs args)
        {
            sido_Entity = await sido_Lib.Details(args.Value.ToString());

            bnnB.st_Sido = sido_Entity.Sido;
            bnnB.st_GunGu = sido_Entity.Region;            
        }

        /// <summary>
        /// 직원정보 입력 닫기
        /// </summary>
        private void btnStaffClose()
        {
            InsertViews = "A";
        }

        /// <summary>
        /// 직원 정보 상세 닫기
        /// </summary>
        private void btnDetailsClose()
        {
            Views = "A";

            //StateHasChanged();
        }        

        /// <summary>
        /// 퇴직 처리
        /// </summary>
        /// <returns></returns>
        private async Task btnResignationSave()
        {
            if (rnn.Division == "A")
            {
                await referral_Career_Lib.Edit_Resign(Career_End_Date, "B", intAid);
            }
            else
            {
                await referral_Career_Lib.Edit_Resign(Career_End_Date, "A", intAid);
            }

            await DisplayData(strSortSearch, strDivision);
            RemoveViews = "A";
            //StateHasChanged();
        }

        /// <summary>
        /// 퇴사처리 닫기
        /// </summary>
        private void btnResignationClose()
        {
            RemoveViews = "A";
            //StateHasChanged();
        }

        /// <summary>
        /// 시도 선택 시 시군구 만들기
        /// </summary>
        public async Task OnSido_Career(ChangeEventArgs args)
        {
            sigungu_Career = await sido_Lib.GetList(args.Value.ToString());
            SidoR = args.Value.ToString();
            //StateHasChanged();
        }

        /// <summary>
        /// 직원 이름 검색
        /// </summary>
        /// <returns></returns>
        private async Task btnStaffNameSearch()
        {
            if (SidoR == "" || SidoR == null)
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "시도를 선택하지 않았습니다..");
            }
            else if (SiGunGu == "" || SiGunGu == null)
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "시군구를 선택하지 않았습니다..");
            }
            else if (StaffName == "" || StaffName == null)
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "이름을 입력하지 않았습니다..");
            }
            else
            {
                staffA = await staff_Sub_Lib.GetList_Name(SidoR, SiGunGu, StaffName);                
                SiGunGu = "";
                StaffName = "";
            }
        }

        /// <summary>
        /// 배치정보 등록 열기
        /// </summary>
        private void OnInsertCareerViews()
        {
            
            rce.Career_Start_Date = DateTime.Now;
            //Duty = await duty_Lib.GetList()
            CareerViews = "B";
           // StateHasChanged();
        }

        /// <summary>
        /// 부서 선택하면 직책 만들기
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        private async Task OnPost(ChangeEventArgs args)
        {
            rce.Post = await post_Lib.PostName(args.Value.ToString());
            Duty = await duty_Lib.GetList(args.Value.ToString(), "A");
            //StateHasChanged();
        }

        /// <summary>
        /// 배치정보 등록
        /// </summary>
        /// <returns></returns>
        public async Task btnStaff_CareerSave()
        {
            rce.Apt_Code = Apt_Code;
            rce.Apt_Name = Apt_Name;
            
            if (rce.Apt_Code == "" || rce.Apt_Code == null)
            {
                await JSRuntime.InvokeAsync<object>("alert", "로그인되지 않았습니다.");
                MyNav.NavigateTo("/");
            }
            else if (rce.Post == "" || rce.Post == null)
            {
                await JSRuntime.InvokeAsync<object>("alert", "부서를 선택하지 않았습니다..");
            }
            else if (rce.Duty == "" || rce.Duty == null)
            {
                await JSRuntime.InvokeAsync<object>("alert", "부서를 선택하지 않았습니다..");
            }
            else if (rce.Post_Code == "" || rce.Post_Code == null)
            {
                await JSRuntime.InvokeAsync<object>("alert", "구분을 선택하지 않았습니다..");
            }
            else if (rce.User_Name == "" || rce.User_Name == null)
            {
                await JSRuntime.InvokeAsync<object>("alert", "이름을 입력하지 않았습니다..");
            }
            else
            {
                int intCCE = await company_Career_Lib.detail_Apt_Count(Apt_Code, "Cb45", DateTime.Now.ToShortDateString(), DateTime.Now.ToShortDateString());

                if (intCCE > 0)
                {
                    Company_Career_Entity carc = await company_Career_Lib.detail_Apt(Apt_Code, "Cb45");

                    rce.CC_Code = carc.CC_Code;
                    rce.ContractSort_Code = carc.ContractSort;
                    rce.Cor_Code = carc.Cor_Code;
                }
                else
                {
                    rce.Cor_Code = Apt_Code;
                    rce.ContractSort_Code = "Cb104";
                    rce.Cor_Code = Apt_Code;                    
                }

                rce.Staff_Cd = rce.User_ID;
                
                if (rce.Aid < 1)
                {
                    await referral_Career_Lib.Add_rc(rce); //배치정보 저장
                }
                else
                {
                    await referral_Career_Lib.Edit_rc_A(rce); //배치정보 수정
                }
            }

            rce = new Referral_career_Entity();
            await DisplayData(strSortSearch, strDivision);
            
            CareerViews = "A";
            
           // StateHasChanged();
        }

        /// <summary>
        /// 배치정보 등록 닫기
        /// </summary>
        private void btnStaff_CareerClose()
        {
            CareerViews = "A";
            //StateHasChanged();
        }

        /// <summary>
        /// 배치정보 수정 열기
        /// </summary>
        public string strPostCode { get; set; }
        public string strSortSearch { get; private set; }
        public string strDivision { get; private set; }
        public string strS { get; private set; }

        private async Task CareerByEdit(Staff_Career_Entity cay)
        {
            strPostCode = await post_Lib.PostCode(cay.Post);
            Duty = await duty_Lib.GetList(strPostCode, "A");
            rce.Aid = cay.Aid;
            rce.Apt_Code = cay.Apt_Code;
            rce.Apt_Name = cay.Apt_Name;
            rce.Career_End_Date = cay.Career_End_Date;
            rce.Career_Start_Date = cay.Career_Start_Date;
            rce.CC_Code = cay.CC_Code;
            rce.ContractSort_Code = cay.ContractSort_Code;
            rce.Cor_Code = cay.Cor_Code;
            rce.Division = cay.Division;
            rce.Duty = cay.Duty;
            rce.Etc = cay.Etc;
            rce.Post = cay.Post;
            rce.Post_Code = cay.Post_Code;
            rce.Staff_Cd = cay.Staff_Cd;
            rce.User_ID = cay.User_ID;
            rce.User_Name = cay.User_Name;
            CareerViews = "B";
            //StateHasChanged();
        }

        /// <summary>
        /// 배치 정보 입력 이름 찾기
        /// </summary>
        private void OnStaffSearch()
        {
            StaffSearch = "B";
            //StateHasChanged();
        }

        /// <summary>
        /// 배치정보 선택 실행
        /// </summary>
        /// <param name="entity"></param>
        private void BySelect(Staff_Sub_Entity entity)
        {
            rce.User_ID = entity.User_ID;
            rce.User_Name = entity.Staff_Name;
            staffA = new List<Staff_Sub_Entity>();
            StaffSearch = "A";
        }

        /// <summary>
        /// 직원 정보 찾기 닫기
        /// </summary>
        private void btnStaffSearchClose()
        {
            StaffSearch = "A";
        }

        /// <summary>
        /// 구분 검색 목록 만들기
        /// </summary>
        private async Task OnSortSearch(ChangeEventArgs a)
        {
            strS = a.Value.ToString();
            if (strS == "관리직원")
            {
                strSortSearch = "A";
                strDivision = "A";
            }
            else if (strS == "경비직원" )
            {
                strSortSearch = "F";
                strDivision = "A";
            }
            else if (strS == "미화직원")
            {
                strSortSearch = "G";
                strDivision = "A";
            }
            else if (strS == "입대의")
            {
                strSortSearch = "C";
                strDivision = "A";
            }
            else if (strS == "선관위")
            {
                strSortSearch = "D";
                strDivision = "A";
            }
            else if (strS == "자생단체임원")
            {
                strSortSearch = "E";
                strDivision = "A";
            }
            else if (strS == "기타")
            {
                strSortSearch = "H";
                strDivision = "A";
            }
            else if (strS == "퇴직 관리직원")
            {
                strSortSearch = "A";
                strDivision = "B";
            }
            else if (strS == "퇴직 경비직원")
            {
                strSortSearch = "F";
                strDivision = "B";
            }
            else if (strS == "퇴직 미화직원")
            {
                strSortSearch = "G";
                strDivision = "B";
            }
            else if (strS == "퇴임 입대의")
            {
                strSortSearch = "C";
                strDivision = "B";
            }
            else if (strS == "퇴임 선관위")
            {
                strSortSearch = "D";
                strDivision = "B";
            }
            else if (strS == "퇴임 자생단체임원")
            {
                strSortSearch = "E";
                strDivision = "B";
            }
            else if (strS == "퇴직 기타")
            {
                strSortSearch = "H";
                strDivision = "B";
            }
            else
            {
                strSortSearch = "";
                strDivision = "";
            }
            await DisplayData(strSortSearch, strDivision);
        }
    }
}
