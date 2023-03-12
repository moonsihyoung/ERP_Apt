using Erp_Apt_Lib;
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

namespace Erp_Apt_Web.Pages.Admin.Apt_People
{
    public partial class Index
    {

        [CascadingParameter]
        public Task<AuthenticationState> AuthenticationStateRef { get; set; }
        [Inject] public IApt_Lib apt_Lib { get; set; }  // 공동주택 정보
        [Inject] public IApt_Sub_Lib apt_Sub_Lib { get; set; }
        [Inject] public NavigationManager MyNav { get; set; } // Url 
        [Inject] public ISw_Files_Lib files_Lib { get; set; } // 파일 첨부
        [Inject] public IReferral_career_Lib referral_Career_Lib { get; set; }
        [Inject] IErp_AptPeople_Lib aptPeople_Lib { get; set; } // 입주민 정보 클래스
        [Inject] IJSRuntime JSRuntime { get; set; }
        [Inject] Microsoft.AspNetCore.Hosting.IWebHostEnvironment env { get; set; }
        [Inject] public IIn_AptPeople_Lib in_AptPople_Lib { get; set; } // 홈페이지 가입회원 정보 클래스
        [Inject] public ISido_Lib sido_Lib { get; set; }



        In_AptPeople_Entity bnn { get; set; } = new In_AptPeople_Entity(); //홈페이지 가입 입주민 정보
        List<Apt_Entity> apts { get; set; } = new List<Apt_Entity>(); //공동주택 정보
        Apt_Sub_Entity apt_Sub_Entity { get; set; } = new Apt_Sub_Entity(); //공동주택 상세 정보
        List<Apt_People_Entity> annA = new List<Apt_People_Entity>();
        List<Apt_People_Entity> annB = new List<Apt_People_Entity>();
        /*List<Apt_Pople_Entity> apt_PoplesH { get; set; } = new List<Apt_Pople_Entity>(); // 입주민 정*/
        Apt_People_Entity apt_People { get; set; } = new Apt_People_Entity(); // 입주민 정보
        Sw_Files_Entity dnn { get; set; } = new Sw_Files_Entity(); // 첨부 파일 정보
        List<Sw_Files_Entity> Files_Entity { get; set; } = new List<Sw_Files_Entity>();
        List<In_AptPeople_Entity> ann = new List<In_AptPeople_Entity>();
        List<Sido_Entity> sidos { get; set; } = new List<Sido_Entity>(); // 시도 정보
        List<Apt_People_Entity> apt_PoplesA { get; set; } = new List<Apt_People_Entity>(); //입주민 목록 정보
        List<Apt_People_Entity> apt_PoplesB { get; set; } = new List<Apt_People_Entity>(); //입주민 목록 정보

        public string User_Code { get; set; }
        public string User_Name { get; set; }
        public int LevelCount { get; private set; }
        public string Apt_Code { get; set; }
        public string Apt_Name { get; set; }
        public string Dong { get; set; }
        public string Ho { get; set; }
        public string Mobile { get; set; }
        public string lblContent { get; set; } = "";
        public int Files_Count { get; set; } = 0;
        public int intAid { get; set; } = 0;
        public string FileViews { get; set; } = "A";
        public string strDong { get; set; }
        public string strHo { get; set; }
        public string Sort_Name { get; set; }
        public string Asort_Name { get; set; }
        public int Period { get; set; }
        public string Private { get; set; } = "B";
        public string FileInputViews { get; set; } = "A";
        public int inta { get; set; } = 0;
        public string InsertViews { get; private set; } = "A";
        public string Views { get; private set; } = "A";
        public string ListViews { get; set; } = "B";
        public string strTitle { get; set; } = "등록";
        public string strSearchs { get; set; }
        public string strQuery { get; set; }
        public string Division { get; set; } = "A";
        public string Sido { get; set; }
        public string SiGuGo { get; set; }
        public string strUser_Name { get; set; }

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
                    await DisplayData();
                    annA = await aptPeople_Lib.DongList(Apt_Code);
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
            if (Division == "A")
            {
                pager.RecordCount = await in_AptPople_Lib.aptHumanList_Count(Apt_Code);
                ann = await in_AptPople_Lib.aptHumanList(pager.PageIndex, Apt_Code);
            }
            else if (Division == "B")
            {
                pager.RecordCount = await in_AptPople_Lib.DongHoList_Count(Apt_Code, Dong, Ho);
                ann = await in_AptPople_Lib.DongHoList(pager.PageIndex, Apt_Code, Dong, Ho);
            }
            else if (Division == "C")
            {
                pager.RecordCount = await in_AptPople_Lib.aptHuman_Name_List_Count(Apt_Code, strUser_Name);
                ann = await in_AptPople_Lib.aptHuman_Name_List(pager.PageIndex, Apt_Code, strUser_Name);
            }
            else
            {

            }
        }

        /// <summary>
        /// 호 정보 가져오기
        /// </summary>
        private async Task OnDong(ChangeEventArgs a)
        {
            Dong = a.Value.ToString();
            annB = await in_AptPople_Lib.Dong_HoList(Apt_Code, a.Value.ToString());
        }

        private async Task OnHo(ChangeEventArgs a)
        {
            Ho = a.Value.ToString();
            Division = "B";
            await DisplayData();
        }

        /// <summary>
        /// 상세보기 열기
        /// </summary>
        /// <param name="in_Apt"></param>
        private void ByAid(In_AptPeople_Entity in_Apt)
        {
            bnn = in_Apt;
            Views = "B";
        }

        /// <summary>
        /// 새로등록 열기
        /// </summary>
        private async Task onNewbutton()
        {
            InsertViews = "B";
            bnn = new In_AptPeople_Entity();
            apt_PoplesA = await aptPeople_Lib.DongList(Apt_Code);
            bnn.Apt_Code = Apt_Code;
            bnn.Apt_Name = Apt_Name;
        }

        /// <summary>
        /// 상세보기 닫기
        /// </summary>
        private void ViewsClose()
        {
            Views = "A";
        }

        /// <summary>
        /// 수정열기
        /// </summary>
        /// <param name="in_AptPople"></param>
        private async void btnEdit(In_AptPeople_Entity in_AptPople)
        {
            bnn = in_AptPople;

            strDay = bnn.Scn.Day.ToString();
            strDong = bnn.Dong;
            strHo = bnn.Ho;

            var apt = await apt_Lib.Details(bnn.Apt_Code);
            Sido = apt.Apt_Adress_Sido;
            sidos = await sido_Lib.GetList(Sido);
            SiGuGo = apt.Apt_Adress_Gun;
            apts = await apt_Lib.GetList_All_Sido(Sido);

            apt_PoplesB = await aptPeople_Lib.Dong_HoList(bnn.Apt_Code, bnn.Dong);
            apt_PoplesA = await aptPeople_Lib.DongList(bnn.Apt_Code);

            strMonth = bnn.Scn.Month.ToString();
            strTitle = "홈페이 가입 회원 정보 수정";
            strYear = bnn.Scn.Year.ToString();


            InsertViews = "B";
        }

        /// <summary>
        /// 승인하기
        /// </summary>
        private async Task btnApproval(int Aid)
        {
            await in_AptPople_Lib.Approval_being(Aid);
            await DisplayData();
            Views = "A";
            //StateHasChanged();
        }

        private async Task btnSave()
        {
            if (bnn.Aid < 1)
            {
                await in_AptPople_Lib.add(bnn);
            }
            else
            {
                await in_AptPople_Lib.edit(bnn);
                InsertViews = "A";
            }


        }

        /// <summary>
        /// 입력모달 닫기
        /// </summary>
        private void btnClose()
        {
            InsertViews = "A";
        }

        /// <summary>
        /// 시도 선택 실행
        /// </summary>
        protected async Task OnSido(ChangeEventArgs args)
        {
            sidos = await sido_Lib.GetList(args.Value.ToString());
            Sido = args.Value.ToString();
        }

        /// <summary>
        /// 시군구 선택 실행
        /// </summary>
        public async Task onGunGu(ChangeEventArgs args)
        {
            //SiGuGo = await sido_Lib.SidoName(Sido);
            apts = await apt_Lib.GetList_All_Sido_Gun(Sido, args.Value.ToString());
        }

        /// <summary>
        /// 아파트 선택 시 실행
        /// </summary>
        protected async Task onApt(ChangeEventArgs args)
        {
            bnn.Apt_Code = args.Value.ToString();
            Apt_Code = args.Value.ToString();

            bnn.Apt_Name = await apt_Lib.Apt_Name(bnn.Apt_Code);
            Apt_Name = bnn.Apt_Name;
            apt_PoplesA = await aptPeople_Lib.DongList(bnn.Apt_Code); //해당 공동주택 동정보 목록

        }

        /// <summary>
        /// 동 정보
        /// </summary>
        protected async Task onDong(ChangeEventArgs args)
        {
            strDong = args.Value.ToString();
            bnn.Dong = strDong;
            apt_PoplesB = await aptPeople_Lib.Dong_HoList_Ds(bnn.Apt_Code, strDong);

        }

        /// <summary>
        /// 호 정보
        /// </summary>
        List<Apt_People_Entity> HoList = new List<Apt_People_Entity>();
        protected void onHo(ChangeEventArgs args)
        {
            bnn.Ho = args.Value.ToString();
            strHo = bnn.Ho;
            bnn.Mobile = "";
        }

        public string strYear { get; set; }
        public string strMonth { get; set; }
        public string strDay { get; set; }
        private async Task OnDays(ChangeEventArgs a)
        {
            strDay = a.Value.ToString();

            if (!string.IsNullOrWhiteSpace(strDay))
            {
                if (!string.IsNullOrWhiteSpace(strYear) && !string.IsNullOrWhiteSpace(strMonth))
                {
                    bnn.Scn = Convert.ToDateTime(strYear + "-" + strMonth + "-" + strDay);
                }
                else
                {
                    await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "먼저 년도와 월 선택해 주세요..");
                }
            }
            else
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "일자를 선택해 주세요..");
            }
        }

        /// <summary>
        /// 암호 초기화
        /// </summary>
        private async void btnResertPass(int Aid)
        {
            bool isDelete = await JSRuntime.InvokeAsync<bool>("confirm", $"암호를 정말로 초기화 하시겠습니까?");

            if (isDelete)
            {
                await in_AptPople_Lib.PassResert(Aid);
            }
        }

        private async Task OnNameSearch(ChangeEventArgs a)
        {
            strUser_Name = a.Value.ToString();
            Division = "C";
            await DisplayData();
        }
    }
}
