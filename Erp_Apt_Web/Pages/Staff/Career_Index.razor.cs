using Company;
using Erp_Apt_Lib.Logs;
using Erp_Apt_Lib;
using Erp_Apt_Staff;
using Erp_Entity;
using Erp_Lib;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.Linq;

namespace Erp_Apt_Web.Pages.Staff
{
    public partial class Career_Index
    {
        #region 인스턴스
        [CascadingParameter] public Task<AuthenticationState> AuthenticationStateRef { get; set; }
        [Inject] public NavigationManager MyNav { get; set; } // Url 
        [Inject] IJSRuntime JSRuntime { get; set; }
        [Inject] public IApt_Lib apt_Lib { get; set; }
        [Inject] public IReferral_career_Lib referral_Career_Lib { get; set; }
        [Inject] public IStaff_Lib staff_Lib { get; set; }
        [Inject] public IStaff_staffSub_Lib staff_StaffSub_Lib { get; set; }
        [Inject] public IStaff_Sub_Lib staff_Sub_Lib { get; set; }
        [Inject] public ISido_Lib sido_Lib { get; set; }
        [Inject] public IPost_Lib post_Lib { get; set; }
        [Inject] public IDuty_Lib duty_Lib { get; set; }
        [Inject] public ICompany_Lib company_Lib { get; set; }
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
        public string strApt_Name { get; set; }
        #endregion

        #region 속성
        List<Staff_Career_Entity> ann { get; set; } = new List<Staff_Career_Entity>();
        List<Staff_Career_Entity> annA { get; set; } = new List<Staff_Career_Entity>();
        public int intNum { get; private set; }
        Staff_Career_Entity rnn { get; set; } = new Staff_Career_Entity();
        Referral_career_Entity rce { get; set; } = new Referral_career_Entity();
        public Staff_Entity bnnA { get; set; } = new Staff_Entity();
        public Staff_Career_Entity staffSub { get; set; } = new Staff_Career_Entity();
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
        /// 페이징
        /// </summary>
        protected async void PageIndexChanged(int pageIndex)
        {
            pager.PageIndex = pageIndex;
            pager.PageNumber = pageIndex + 1;

            await DisplayData(strFeild, strQuery, strSearchApt);

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
                User_Name = authState.User.Claims.FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name")?.Value;
                LevelCount = Convert.ToInt32(authState.User.Claims.FirstOrDefault(c => c.Type == "LevelCount")?.Value);

                #region 로그 파일 만들기
                logs.Note = "직원관리에 들어왔습니다."; logs.Logger = User_Code; logs.Application = "직원관리"; logs.ipAddress = ""; logs.Message = Apt_Code + " " + Apt_Name;
                await logs_Lib.add(logs);
                #endregion

                if (LevelCount >= 5)
                {
                    await DisplayData("", "", "");
                    Post = await post_Lib.GetList("A");
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
        /// 배치정보 목록 불러오기
        /// </summary>
        private async Task DisplayData(string a, string b, string c)
        {
            if (strSort == "B")
            {
                pager.RecordCount = await staff_StaffSub_Lib.GetCareer_Search_Count(a, b, c);
                ann = await staff_StaffSub_Lib.GetCareerList_Search(pager.PageIndex, a, b, c);
            }
            else
            {
                pager.RecordCount = await staff_StaffSub_Lib.GetCareerList_Count();
                ann = await staff_StaffSub_Lib.GetCareerList(pager.PageIndex);
            }
            intNum = pager.RecordCount - (pager.PageIndex * pager.PageSize);
            //StateHasChanged();
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
        /// 직원 정보 입력
        /// </summary>
        public async Task btnStaffSave()
        {
            bnnA.M_Apt_Code = Apt_Code;
            bnnA.Scn = Scn.ToShortDateString();
            bnnA.Scn_Code = bnnA.Scn.Replace("-", "");
            bnnA.Staff_Cd = bnnA.User_ID;
            bnnA.Intro = bnnB.Etc;
            bnnA.JoinDate = DateTime.Now;
            bnnA.LevelCount = 3;
            bnnA.Password_sw = bnnA.User_ID + "pw";
            bnnA.Sido = bnnB.st_Sido;
            bnnA.SiGunGu = bnnB.st_GunGu;
            bnnA.RestAdress = bnnB.st_Adress_Rest;
            bnnA.LevelCount = 3;

            bnnB.d_division = D_Division;
            bnnB.M_Apt_Code = bnnA.M_Apt_Code;
            bnnB.Staff_Cd = bnnA.User_ID;
            bnnB.User_ID = bnnA.User_ID;
            bnnB.Staff_Name = bnnA.User_Name;
            bnnB.Staff_Sub_Cd = bnnA.User_ID;
            bnnB.levelcount = 3;
            bnnB.End_Date = DateTime.Now;

            if (string.IsNullOrWhiteSpace(bnnB.Mobile_Number))
            {
                string cp_Num = bnnB.Mobile_Number.Replace("-", "");

                cp_Num = cp_Num.Insert(2, "-");
                cp_Num = cp_Num.Insert(7, "-");
                bnnB.Mobile_Number = cp_Num;
            }

            if (bnnA.M_Apt_Code == "" || bnnA.M_Apt_Code == null)
            {
                await JSRuntime.InvokeAsync<object>("alert", "로그인되지 않았습니다.");
                MyNav.NavigateTo("/");
            }
            else if (bnnA.Scn == "" || bnnA.Scn == null)
            {
                await JSRuntime.InvokeAsync<object>("alert", "출생일은 입력하지 않았습니다.");
            }
            else if (bnnA.User_ID == "" || bnnA.User_ID == null)
            {
                await JSRuntime.InvokeAsync<object>("alert", "출생일은 입력하지 않았습니다.");
            }
            else if (bnnA.User_Name == "" || bnnA.User_Name == null)
            {
                await JSRuntime.InvokeAsync<object>("alert", "출생일은 입력하지 않았습니다.");
            }
            else if (bnnB.Staff_Name == "" || bnnB.Staff_Name == null)
            {
                await JSRuntime.InvokeAsync<object>("alert", "출생일은 입력하지 않았습니다.");
            }
            else if (bnnB.st_Sido == "Z" || bnnB.st_Sido == null)
            {
                await JSRuntime.InvokeAsync<object>("alert", "출생일은 입력하지 않았습니다.");
            }
            else if (bnnB.st_GunGu == "Z" || bnnB.st_GunGu == null)
            {
                await JSRuntime.InvokeAsync<object>("alert", "출생일은 입력하지 않았습니다.");
            }
            else if (bnnB.Mobile_Number == "" || bnnB.Mobile_Number == null)
            {
                await JSRuntime.InvokeAsync<object>("alert", "출생일은 입력하지 않았습니다.");
            }
            else
            {
                if (bnnA.Aid < 1)
                {
                    bnnA.Scn = Scn.ToShortDateString();
                    await staff_Lib.Add(bnnA);
                    await staff_Sub_Lib.Add(bnnB);
                    #region 로그 파일 만들기
                    //logs.Note = bnnA.User_Name + " " + bnnA.User_ID + " " + bnnA.Scn + " " + bnnA.JoinDate.ToShortDateString() + "입력했습니다."; logs.Logger = User_Code; logs.Application = "직원관리"; logs.ipAddress = ""; logs.Message = Apt_Code + " " + Apt_Name;
                    //await logs_Lib.add(logs);
                    #endregion
                }
                else
                {
                    await staff_Lib.Edit(bnnA);
                    await staff_Sub_Lib.Edit(bnnB);
                }
            }
            await DisplayData("", "", "");
            InsertViews = "A";
            bnnA = new Staff_Entity();
            bnnB = new Staff_Sub_Entity();

            //StateHasChanged();
        }

        /// <summary>
        /// 배치정보 상세
        /// </summary>
        List<Referral_career_Entity> rnn_list { get; set; } = new List<Referral_career_Entity>();
        public DateTime dt { get; set; }
        private async Task ByDetails(Staff_Career_Entity views)
        {
            Views = "B";
            strTitle = views.User_Name + " 상세정보";
            staffSub = views;
            dt = Convert.ToDateTime("2010-01-01");
            rnn_list = await referral_Career_Lib.GetList_Code(views.User_ID);
        }

        private void btnCloseA()
        {
            Views = "A";
        }

        /// <summary>
        /// 퇴사처리 열기
        /// </summary>
        private void ByRemove(Staff_Career_Entity career)
        {
            RemoveViews = "B";
            intAid = career.Aid;//퇴사 처리를 위한 일련 번호 등록
            staffSub = career;
            //StateHasChanged();
        }

        

        /// <summary>
        /// 시도 선택 시 시군구 만들기
        /// </summary>
        public async Task OnSido(ChangeEventArgs a)
        {
            string ar = a.Value.ToString();
            if (ar == "A")
            {
                bnnA.Sido = "서울특별시";
            }
            else if (ar == "B")
            {
                bnnA.Sido = "경기도";
            }
            else if (ar == "C")
            {
                bnnA.Sido = "부산광역시";
            }
            else if (ar == "D")
            {
                bnnA.Sido = "대구광역시";
            }
            else if (ar == "E")
            {
                bnnA.Sido = "인천광역시";
            }
            else if (ar == "F")
            {
                bnnA.Sido = "광주광역시";
            }
            else if (ar == "G")
            {
                bnnA.Sido = "대전광역시";
            }
            else if (ar == "H")
            {
                bnnA.Sido = "울산광역시";
            }
            else if (ar == "I")
            {
                bnnA.Sido = "세종특별자치시";
            }
            else if (ar == "J")
            {
                bnnA.Sido = "충청남도";
            }
            else if (ar == "K")
            {
                bnnA.Sido = "충청북도";
            }
            else if (ar == "L")
            {
                bnnA.Sido = "경상남도";
            }
            else if (ar == "M")
            {
                bnnA.Sido = "경상북도";
            }
            else if (ar == "N")
            {
                bnnA.Sido = "전라남도";
            }
            else if (ar == "O")
            {
                bnnA.Sido = "전라북도";
            }
            else if (ar == "P")
            {
                bnnA.Sido = "강원도";
            }
            else if (ar == "Q")
            {
                bnnA.Sido = "제주특별자치도";
            }
            bnnB.st_Sido = bnnA.Sido;
            //bnnA.JoinDate = DateTime.Now;
            //bnnB.st_Sido = args.Value.ToString();
            sigungu = await sido_Lib.GetList_Code(a.Value.ToString());
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
            //StateHasChanged();
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
            //StateHasChanged();
        }

        /// <summary>
        /// 시군구 선택 실행
        /// </summary>
        public async Task onGunGu(ChangeEventArgs args)
        {
            sido_Entity = await sido_Lib.Details(args.Value.ToString());

            bnnB.st_Sido = sido_Entity.Sido;
            bnnB.st_GunGu = sido_Entity.Region;
            bnnA.User_ID = Apt_Code + await referral_Career_Lib.Count_apt_staff(Apt_Code);

            //StateHasChanged();
        }

        /// <summary>
        /// 부서 선택하면 직책 만들기
        /// </summary>
        public string strPostCode { get; set; }
        private async Task OnPost(ChangeEventArgs args)
        {
            rce.Post = await post_Lib.PostName(args.Value.ToString());
            strPostCode = args.Value.ToString();
            Duty = await duty_Lib.GetList(strPostCode, "A");

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
            await DisplayData("", "", "");

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
        /// 배치 정보 입력 이름 찾기
        /// </summary>
        public string StaffSearchs { get; set; } = "A";
        private void OnStaffSearch()
        {
            StaffSearchs = "B";
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
        private void btnStaffSearchCloses()
        {
            StaffSearchs = "A";
        }

        /// <summary>
        /// 직원정보 입력 닫기
        /// </summary>
        private void btnStaffClose()
        {
            InsertViews = "A";

            //StateHasChanged();
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
            await DisplayData("", "", "");
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
        /// 직원 이름 검색
        /// </summary>        
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
        /// 시도 선택 시 시군구 만들기
        /// </summary>
        public async Task OnSido_Career(ChangeEventArgs args)
        {
            sigungu_Career = await sido_Lib.GetList(args.Value.ToString());
            SidoR = args.Value.ToString();
            //StateHasChanged();
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
        /// 직원정보 수정
        /// </summary>
        public string strSido { get; set; }
        public string strSigungo { get; set; }
        private async Task btnEditStaff(Staff_Career_Entity ar)
        {
            //staffSub = ar;
            bnnA.Aid = ar.Aid;
            bnnA.Staff_Cd = ar.Staff_Cd;
            bnnA.CommentsCount = ar.CommentsCount;
            bnnA.FileUpCount = ar.FileUpCount;
            bnnA.JoinDate = ar.JoinDate;
            bnnA.LevelCount = ar.LevelCount;
            bnnA.M_Apt_Code = ar.M_Apt_Code;
            bnnA.Old_UserID = ar.Old_UserID;
            bnnA.ReadCount = ar.ReadCount;
            bnnA.RestAdress = ar.RestAdress;
            bnnA.Scn = ar.Scn;
            bnnA.Scn_Code = ar.Scn_Code;
            bnnA.Sido = ar.st_Sido;
            bnnA.SiGunGu = ar.st_GunGu;
            bnnA.User_ID = ar.User_ID;
            bnnA.User_Name = ar.User_Name;
            bnnA.VisitCount = ar.VisitCount;
            bnnA.WriteCount = ar.WriteCount;
            bnnA.Intro = ar.Etc;

            bnnB.d_division = ar.d_division;
            bnnB.Email = ar.Email;
            bnnB.End_Date = ar.End_Date;
            bnnB.Etc = ar.Etc;
            bnnB.levelcount = ar.LevelCount;
            bnnB.Mobile_Number = ar.Mobile_Number;
            bnnB.M_Apt_Code = ar.M_Apt_Code;
            bnnB.Staff_Cd = ar.Staff_Cd;
            bnnB.Staff_Name = ar.Staff_Name;
            bnnB.Staff_Sub_Cd = ar.Staff_Sub_Cd;
            bnnB.Start_Date = ar.Start_Date;
            bnnB.st_Adress_Rest = ar.st_Adress_Rest;
            bnnB.st_GunGu = ar.st_GunGu;
            bnnB.st_Sido = ar.st_Sido;
            bnnB.Telephone = ar.Telephone;
            bnnB.User_ID = ar.User_ID;

            try
            {
                Scn = Convert.ToDateTime(ar.Scn);
            }
            catch (Exception)
            {
                Scn = DateTime.Now;
            }

            if (bnnA.Sido == "서울특별시")
            {
                strSido = "A";
            }
            else if (bnnA.Sido == "경기도")
            {
                strSido = "B";
            }
            else if (bnnA.Sido == "부산광역시")
            {
                strSido = "C";
            }
            else if (bnnA.Sido == "대구광역시")
            {
                strSido = "D";
            }
            else if (bnnA.Sido == "인천광역시")
            {
                strSido = "E";
            }
            else if (bnnA.Sido == "광주광역시")
            {
                strSido = "F";
            }
            else if (bnnA.Sido == "대전광역시")
            {
                strSido = "G";
            }
            else if (bnnA.Sido == "울산광역시")
            {
                strSido = "H";
            }
            else if (bnnA.Sido == "세종특별자치시")
            {
                strSido = "I";
            }
            else if (bnnA.Sido == "충청남도")
            {
                strSido = "J";
            }
            else if (bnnA.Sido == "충청북도")
            {
                strSido = "K";
            }
            else if (bnnA.Sido == "경상남도")
            {
                strSido = "L";
            }
            else if (bnnA.Sido == "경상북도")
            {
                strSido = "M";
            }
            else if (bnnA.Sido == "전라남도")
            {
                strSido = "N";
            }
            else if (bnnA.Sido == "전라북도")
            {
                strSido = "O";
            }
            else if (bnnA.Sido == "강원도")
            {
                strSido = "P";
            }
            else if (bnnA.Sido == "제주특별자치도")
            {
                strSido = "Q";
            }

            sigungu = await sido_Lib.GetList(bnnA.Sido);
            strSigungo = await sido_Lib.Region_Code(ar.st_GunGu);

            //await DisplayData();
            InsertViews = "B";
        }

        /// <summary>
        /// 등급 수정 열기
        /// </summary>
        private void btnEditLevel(Staff_Career_Entity ar)
        {
            InsertLevel = "B";
            intLevelCount = ar.LevelCount;
        }

        #region 검색관련
        public string strTitle { get; set; }
        public string strFeild { get; set; }
        public string strQuery { get; set; }

        private void On_Search_Open()
        {
            strTitle = "가입된 직원 정보 검색";
            strFeild = "";
            strQuery = "";
            StaffSearch = "B";
        }

        /// <summary>
        /// 검색
        /// </summary>
        public string SearchSort { get; set; }
        public string SearchQuery { get; set; }
        private async Task OnQuery(ChangeEventArgs a)
        {
            SearchQuery = a.Value.ToString();
            if (!string.IsNullOrWhiteSpace(SearchQuery))
            {
                if (!string.IsNullOrWhiteSpace(SearchSort))
                {
                    strFeild = SearchSort;
                    strQuery = SearchQuery;
                    strSort = "B";
                    await DisplayData(strFeild, strQuery, strSearchApt);
                }
                else
                {
                    await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "분류를 선택하지 않았습니다.");
                }
            }
            else
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "검색 내용을 입력하지 않았습니다.");
            }
        }

        /// <summary>
        /// 직원 정보 찾기 닫기
        /// </summary>
        private void btnStaffSearchClose()
        {
            StaffSearch = "A";
            strQuery = "";
            strFeild = "";
            //await DisplayData();
        }
        #endregion

        /// <summary>
        /// 등급 수정
        /// </summary>
        public int intLevelCount { get; set; } = 0;
        public string InsertLevel { get; set; } = "A";
        private async Task OnLevelSave(ChangeEventArgs a)
        {
            intLevelCount = Convert.ToInt32(a.Value);
            await staff_Lib.LevelUpdate(staffSub.User_ID, intLevelCount);
            staffSub = await staff_StaffSub_Lib.CareerViews(staffSub.User_ID);
            await DisplayData("", "", "");
            InsertLevel = "A";
        }

        /// <summary>
        /// 등급 수정 닫기
        /// </summary>
        private void btnCloseL()
        {
            InsertLevel = "A";
        }

        /// <summary>
        /// 배치정보 수정열기
        /// </summary>
        private async Task btnEditReferral(string User_Code)
        {
            try
            {
                rce = await referral_Career_Lib.Detail(User_Code);
                strPostCode = rce.Post_Code;
                Duty = await duty_Lib.GetList(strPostCode, "A");
            }
            catch (Exception)
            {
                //
            }
            CareerViews = "B";
        }

        List<Apt_Entity> apt { get; set; } = new List<Apt_Entity>();
        private async Task OnAptName(ChangeEventArgs a)
        {
            if (a.Value.ToString() != null)
            {
                strApt_Name = a.Value.ToString();
                apt = await apt_Lib.SearchList(strApt_Name);
                await DisplayData(strFeild, strQuery, strSearchApt);
            }
        }

        public string strSearchApt { get; set; }
        public string strSort { get; set; } = "A";
        private async Task OnSearchApt(ChangeEventArgs a)
        {
            if (!string.IsNullOrWhiteSpace(a.Value.ToString()))
            {
                strFeild = "Apt_Name";
                strQuery = a.Value.ToString();
                strSearchApt = a.Value.ToString();
                strSort = "B";

                await DisplayData(strFeild, strQuery, strSearchApt);
                //pager.RecordCount = await staff_StaffSub_Lib.GetCareer_Search_Count("Apt_Name", strSearchApt, strSearchApt);
                //ann = await staff_StaffSub_Lib.GetCareerList_Search(pager.PageIndex, "Apt_Name", strSearchApt, strSearchApt);
            }
        }
    }
}
