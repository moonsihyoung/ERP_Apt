using Erp_Apt_Lib.apt_Erp_Com;
using Erp_Apt_Lib.Logs;
using Erp_Apt_Lib;
using Erp_Entity;
using Erp_Lib;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Erp_Apt_Web.Pages.Bike
{
    public partial class Index
    {
        #region 변수
        public string User_Code { get; set; }
        public string User_Name { get; set; }
        public string Apt_Code { get; set; }
        public string Apt_Name { get; set; }
        public string UserName { get; set; }
        public string strDong { get; set; }
        public int CarCount { get; set; } = 0;
        public string Division { get; set; } = "A";
        public string? NumberSearch { get; set; }
        public string FileInsert { get; set; } = "A";
        public string? FileViews { get; set; }
        public int Files_Count { get; set; } = 0;
        public string Views { get; set; } = "A";
        public string InsertViews { get; set; } = "A";
        public string PDFViews { get; set; } = "A";
        #endregion

        #region 로드
        public Bike_Entity ann { get; set; } = new Bike_Entity();
        List<Bike_Entity> bnn = new List<Bike_Entity>();

        public int intNum { get; private set; }
        public Apt_People_Entity apn { get; set; } = new Apt_People_Entity();
        List<Apt_People_Entity> pnn = new List<Apt_People_Entity>();
        List<Apt_People_Entity> onn = new List<Apt_People_Entity>();
        List<Apt_People_Entity> qnn = new List<Apt_People_Entity>();
        List<Apt_People_Entity> dnn = new List<Apt_People_Entity>();
        Logs_Entites logs { get; set; } = new Logs_Entites();
        #endregion

        #region 인스턴스
        [CascadingParameter]
        public Task<AuthenticationState> AuthenticationStateRef { get; set; }
        [Inject] public ISw_Files_Lib files_Lib { get; set; } // 파일 첨부
        [Inject] public IApt_Lib apt_Lib { get; set; }
        [Inject] public IBike_Lib bike_Lib { get; set; }
        [Inject] public IErp_AptPeople_Lib apt_people { get; set; }
        [Inject] public NavigationManager MyNav { get; set; }
        [Inject] IJSRuntime JSRuntime { get; set; }
        [Inject] public ILogs_Lib logs_Lib { get; set; }
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

                pnn = await apt_people.DongList(Apt_Code);
                await DisplayData();

                #region 로그 파일 만들기
                logs.Note = "자전거 관리에 들어왔습니다."; logs.Logger = User_Code; logs.Application = "자전거 관리"; logs.ipAddress = ""; logs.Message = Apt_Code + " " + Apt_Name;
                await logs_Lib.add(logs);
                #endregion
            }
            else
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "로그인되지 않았습니다..");
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


        private async Task DisplayData()
        {
            CarCount = await bike_Lib.GetList_Apt_Count(Apt_Code);
            pager.RecordCount = CarCount;
            bnn = await bike_Lib.GetList_Apt(pager.PageIndex, Apt_Code);
        }

        /// <summary>
        /// 동선택 실행
        /// </summary>
        protected async Task onDong(ChangeEventArgs args)
        {
            ann.Dong = args.Value.ToString();
            onn = new List<Apt_People_Entity>();
            onn = await apt_people.DongHoList_new(Apt_Code, args.Value.ToString());
            ann.Ho = "";
        }

        /// <summary>
        /// 호 선택 시 실행
        /// </summary>
        public int intBeing { get; set; } = 0;
        protected async Task onHo(ChangeEventArgs args)
        {
            ann.Ho = args.Value.ToString();
            intBeing = await apt_people.Dong_Ho_Count(Apt_Code, ann.Dong, ann.Ho);
            if (intBeing > 0)
            {
                dnn = await apt_people.DongHoList(Apt_Code, ann.Dong, ann.Ho);
            }
            else
            {
                ann.Ho = "";
                if (string.IsNullOrWhiteSpace(ann.Dong))
                {
                    await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "동을 선택하지 않았습니다..");
                }
                else if (string.IsNullOrWhiteSpace(ann.Ho))
                {
                    await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "호를 선택하지 않았습니다..");
                }
                else
                {
                    await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", ann.Dong + "동" + ann.Ho + "호에 등록된 입주민이 없습니다..");
                }
            }
        }

        /// <summary>
        /// 입주자 선택 시 실행
        /// </summary>
        /// <param name="ar"></param>
        private void btnBySelect(Apt_People_Entity ar)
        {
            ann.Mobile = ar.Hp;
            ann.Name = ar.InnerName;
        }

        /// <summary>
        /// 이사 등록 열기
        /// </summary>
        /// <param name="args"></param>
        protected void onMove(ChangeEventArgs args)
        {
            ann.del = args.Value.ToString();
        }

        /// <summary>
        /// 자동차 새로 등록
        /// </summary>
        /// <returns></returns>
        private async Task btnSave()
        {
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
            ann.PostIp = myIPAddress;
            ann.Apt_Code = Apt_Code;
            ann.Apt_Name = Apt_Name;

            #endregion
            if (string.IsNullOrWhiteSpace(ann.Dong))
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "동을 선택하지 않았습니다..");
            }
            else if (string.IsNullOrWhiteSpace(ann.Ho))
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "호를 선택하지 않았습니다..");
            }
            else if (string.IsNullOrWhiteSpace(ann.Apt_Code))
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "로그인되지 않았습니다..");
            }
            else if (string.IsNullOrWhiteSpace(ann.Name))
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "소유자명을 입력하지 않았습니다..");
            }
            else
            {
                if (ann.Aid > 0)
                {
                    await bike_Lib.Edit(ann);

                    #region 로그 파일 만들기
                    logs.Note = ann.Dong + "동" + ann.Ho + "호" + ann.Name + "자전거를 수정했습니다."; logs.Logger = User_Code; logs.Application = "자전거 관리"; logs.ipAddress = ""; logs.Message = Apt_Code + " " + Apt_Name;
                    await logs_Lib.add(logs);
                    #endregion

                    Views = "A";
                    onn = new List<Apt_People_Entity>();
                    await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "수정되었습니다...");
                    await DisplayData();
                    ann = new Bike_Entity();
                    InsertViews = "A";
                    dnn = new List<Apt_People_Entity>();
                }
                else
                {
                    ann.Apt_Code = Apt_Code;
                    await bike_Lib.Add(ann);

                    #region 로그 파일 만들기
                    logs.Note = ann.Dong + "동" + ann.Ho + "호" + ann.Name + "자전거를 새로 입력했습니다."; logs.Logger = User_Code; logs.Application = "자전거 관리"; logs.ipAddress = ""; logs.Message = Apt_Code + " " + Apt_Name;
                    await logs_Lib.add(logs);
                    #endregion

                    await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "등록되었습니다...");
                    Views = "A";
                    onn = new List<Apt_People_Entity>();
                    await DisplayData();
                    ann = new Bike_Entity();
                    InsertViews = "A";
                }

            }
        }

        /// <summary>
        /// 동 검색 시 실행
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        protected async Task onDongSearch(ChangeEventArgs args)
        {
            strDong = args.Value.ToString();
            qnn = new List<Apt_People_Entity>();
            qnn = await apt_people.DongHoList_new(Apt_Code, args.Value.ToString());
        }

        protected async Task onHoSearch(ChangeEventArgs args)
        {
            string strHo = args.Value.ToString();

            bnn = await bike_Lib.SearchList(Apt_Code, strDong, strHo);
        }

        /// <summary>
        /// 상세보기 및 수정
        /// </summary>
        public async Task btnByAid(Bike_Entity vnn)
        {
            onn = await apt_people.Dong_HoList(Apt_Code, vnn.Dong);
            ann = vnn;
            ann.MoveDate = DateTime.Now;
            Division = "A";
            Views = "B";
        }

        /// <summary>
        /// 상세 팝업 닫기
        /// </summary>
        private void btnViewsClose()
        {
            Views = "A";
        }

        /// <summary>
        /// 새로 등록 열기
        /// </summary>
        private void onOpen()
        {
            ann = new Bike_Entity();
            ann.PostDate = DateTime.Now.Date;
            InsertViews = "B";
        }

        /// <summary>
        /// 수정 열기
        /// </summary>
        /// <param name="car_Infor"></param>
        private async Task btnByEdit(Bike_Entity bike)
        {
            onn = await apt_people.Dong_HoList(Apt_Code, bike.Dong);
            ann = bike;
            await DisplayData();
            InsertViews = "B";
            Views = "A";
        }

        /// <summary>
        /// 등록 팝업 닫기
        /// </summary>
        private void btnInsertViewsClose()
        {
            ann = new Bike_Entity();
            dnn = new List<Apt_People_Entity>();
            InsertViews = "A";
        }

        /// <summary>
        /// 이사 등록
        /// </summary>
        private async Task btnMove(int Aid)
        {
            await bike_Lib.Remove(Aid, DateTime.Now);
            ann = await bike_Lib.Details(Aid);
            await DisplayData();

        }
    }
}
