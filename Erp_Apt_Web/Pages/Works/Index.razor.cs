using Company;
using Erp_Apt_Lib;
using Erp_Apt_Lib.Decision;
using Erp_Apt_Lib.Stocks;
using Erp_Apt_Staff;
using Erp_Entity;
using Erp_Lib;
using Facilities;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using Wedew_Lib;
using Works;

namespace Erp_Apt_Web.Pages.Works
{
    public partial class Index
    {
        #region Inject
        [Parameter] public string Code { get; set; }
        [CascadingParameter] public Task<AuthenticationState> AuthenticationStateRef { get; set; }
        [Inject] public IApt_Lib apt_Lib { get; set; }
        [Inject] public NavigationManager MyNav { get; set; }
        [Inject] IJSRuntime JSRuntime { get; set; }
        [Inject] public IReferral_career_Lib referral_Career_Lib { get; set; }
        [Inject] public IPost_Lib Post_Lib { get; set; }
        [Inject] public IErp_AptPeople_Lib Erp_AptPeople_Lib { get; set; }
        [Inject] public IBloom_Lib bloom_Lib { get; set; }
        [Inject] public IWorks_Lib works_Lib { get; set; }
        [Inject] public IWorksSub_Lib worksSub_Lib { get; set; }
        [Inject] public IStocks_Lib stocks_Lib { get; set; }
        [Inject] public IWhSock_Lib whSock_Lib { get; set; }
        [Inject] public ISw_Files_Lib files_Lib { get; set; } // 파일 첨부
        [Inject] public ICompany_Lib company_Lib { get; set; }
        [Inject] public IDecision_Lib decision_Lib { get; set; } // 결재정보

        #endregion

        #region 목록
        List<Works_Entity> ann = new List<Works_Entity>();

        public int intNum { get; private set; }

        List<WorksSub_Entity> bnn = new List<WorksSub_Entity>();
        Referral_career_Entity cnnA { get; set; } = new Referral_career_Entity();
        Works_Entity dnn { get; set; } = new Works_Entity();
        WorksSub_Entity ennA { get; set; } = new WorksSub_Entity();
        List<Post_Entity> pnn { get; set; } = new List<Post_Entity>();
        List<Referral_career_Entity> fnn = new List<Referral_career_Entity>();
        List<Referral_career_Entity> fnnD = new List<Referral_career_Entity>();

        List<Bloom_Entity> bloom_A = new List<Bloom_Entity>();
        List<Bloom_Entity> bloom_B = new List<Bloom_Entity>();
        List<Bloom_Entity> bloom_C = new List<Bloom_Entity>();
        List<Bloom_Entity> bloom_D = new List<Bloom_Entity>();
        Works_Entity works_s { get; set; } = new Works_Entity();
        List<WareHouse_Entity> wheL = new List<WareHouse_Entity>();
        WareHouse_Entity wheE { get; set; } = new WareHouse_Entity();
        List<Stock_Code_Entity> sceL = new List<Stock_Code_Entity>();
        Stock_Code_Entity sceE { get; set; } = new Stock_Code_Entity();

        #endregion

        #region 변수
        public string User_Code { get; set; }
        public string User_Name { get; set; }
        public string Apt_Code { get; set; }
        public string Apt_Name { get; set; }
        public string UserName { get; set; }
        public string ListViews { get; set; } = "B";
        public string PostDuty { get; set; }
        public string DetailsViews { get; set; } = "A";
        public string InsertViews { get; set; } = "A";
        public string InOutViews { get; private set; }
        public string RemoveViews { get; set; } = "A";
        public string strPost { get; set; }
        public DateTime apDate { get; set; }
        public string PrivateViews { get; set; } = "A";
        public string EditViews { get; set; } = "A";

        public string strWSortA { get; set; }
        public string strWSortB { get; set; }
        public string strWSortC { get; set; }
        public string strWSortD { get; set; }
        public DateTime svDate { get; set; } = DateTime.Now.Date;
        public string Sort { get; private set; }
        public string Views { get; set; } = "A";
        #endregion

        /// <summary>
        /// 로드시 실행
        /// </summary>
        /// <returns></returns>
        protected override async Task OnInitializedAsync()
        {
            var authState = await AuthenticationStateRef;
            if (authState.User.Identity.IsAuthenticated)
            {
                //var result = await ProtectedSessionStore.GetAsync<int>("count");
                //var resultA = await ProtectedLocalStore.GetAsync<int>("count");
                //로그인 정보
                Apt_Code = authState.User.Claims.FirstOrDefault(c => c.Type == "Apt_Code")?.Value;
                Apt_Name = authState.User.Claims.FirstOrDefault(c => c.Type == "Apt_Name")?.Value;
                User_Code = authState.User.Claims.FirstOrDefault(c => c.Type == "User_Code")?.Value;
                User_Name = authState.User.Claims.FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name")?.Value;

                cnnA = await referral_Career_Lib.Detail(User_Code);
                PostDuty = cnnA.Post + cnnA.Duty;

                app = await approval_Lib.GetList(Apt_Code, "작업일지");
                referral = await referral_Career_Lib.Details(User_Code);

                apDate = DateTime.Now.Date;
                pnn = await Post_Lib.GetList("A");//부서 목록

                bloom_A = await bloom_Lib.GetList_Apt_ba(Apt_Code); // 작업 대분류

                await DisplayData("", "", "", "");
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


        /// <summary>
        /// 데이터 뷰
        /// </summary>
        private async Task DisplayData(string Sort_A, string Sort_B, string Sort_C, string Sort_D)
        {
            if (Sort == "A")
            {
                pager.RecordCount = await works_Lib.WordSearchList_CountA(Apt_Code, Sort_A);
                ann = await works_Lib.WordSearchListA(pager.PageIndex, Apt_Code, Sort_A);
            }
            else if (Sort == "B")
            {
                pager.RecordCount = await works_Lib.DateSearchList_CountA(Apt_Code, Sort_A, Sort_B);
                ann = await works_Lib.DateSearchListA(pager.PageIndex, Apt_Code, Sort_A, Sort_B);
            }
            else if (Sort == "C_A")
            {
                pager.RecordCount = await works_Lib.BoomSearchList_CountA(Apt_Code, Sort_A);
                ann = await works_Lib.BoomSearchListA(pager.PageIndex, Apt_Code, Sort_A);
            }
            else if (Sort == "C_B")
            {
                pager.RecordCount = await works_Lib.BoomSearchList_CountB(Apt_Code, Sort_A, Sort_B);
                ann = await works_Lib.BoomSearchListB(pager.PageIndex, Apt_Code, Sort_A, Sort_B);
            }
            else if (Sort == "C_C")
            {
                pager.RecordCount = await works_Lib.BoomSearchList_CountC(Apt_Code, Sort_A, Sort_B, Sort_C);
                ann = await works_Lib.BoomSearchListC(pager.PageIndex, Apt_Code, Sort_A, Sort_B, Sort_C);
            }
            else
            {
                pager.RecordCount = await works_Lib.Service_List_Count(Apt_Code);
                ann = await works_Lib.Service_List(pager.PageIndex, Apt_Code);
            }
            intNum = pager.RecordCount - (pager.PageIndex * pager.PageSize);
        }

        /// <summary>
        /// 페이징
        /// </summary>
        protected DulPager.DulPagerBase pager = new DulPager.DulPagerBase()
        {
            PageNumber = 1,
            PageIndex = 0,
            PageSize = 15,
            PagerButtonCount = 5
            //StateHasChanged();
        };

        /// <summary>
        /// 페이징
        /// </summary>
        protected async void PageIndexChanged(int pageIndex)
        {
            pager.PageIndex = pageIndex;
            pager.PageNumber = pageIndex + 1;

            if (Sort == "A")
            {
                await DisplayData(KeyWord, "", "", "");
            }
            else if (Sort == "B")
            {
                await DisplayData(StartDate, EndDate, "", "");
            }
            else if (Sort == "C_A")
            {
                await DisplayData(strWSortA, "", "", "");
            }
            else if (Sort == "C_B")
            {
                await DisplayData(strWSortA, strWSortB, "", "");
            }
            else if (Sort == "C_C")
            {
                await DisplayData(strWSortA, strWSortB, strWSortC, "");
            }
            else
            {
                await DisplayData("", "", "", "");
            }


            StateHasChanged();
        }

        /// <summary>
        /// 상세보기 이동
        /// </summary>
        /// <param name="Num"></param>
        private void ByAid(int Num)
        {
            MyNav.NavigateTo("/Works/Details/" + Num);
        }

        /// <summary>
        /// 수정 열기
        /// </summary>
        /// <param name="works"></param>
        private async Task ByEdit(Works_Entity works)
        {
            pnn = await Post_Lib.GetList("A");
            fnn = await referral_Career_Lib.GetList_Post_Staff_be(works.svPost, Apt_Code);
            dnn = works;

            svDate = Convert.ToDateTime(works.svYear + "-" + works.svMonth + "-" + works.svDay);
            strWSortA = dnn.svBloomA;
            bloom_B = await bloom_Lib.GetList_bb(strWSortA);
            strWSortB = dnn.svBloomB;
            bloom_C = await bloom_Lib.GetList_cc(strWSortB);
            strWSortC = dnn.svBloomC;
            bloom_D = await bloom_Lib.GetList_dd(Apt_Code, strWSortA);
            strWSortD = dnn.svBloom;
            InsertViews = "B";
        }

        private async Task ByRemove(Works_Entity works)
        {
            bool isDelete = await JSRuntime.InvokeAsync<bool>("confirm", $"{works.Num}번 작업일지를 정말로 삭제하시겠습니까?");
            if (isDelete)
            {
                svDate = DateTime.Now.Date;
                await works_Lib.WorksRemove(works.Num.ToString());
                await DisplayData("", "", "", "");
            }
        }

        /// <summary>
        /// 업무일지 지시사항 입력 열기
        /// </summary>
        private async Task onInsert_Open()
        {
            svDate = DateTime.Now.Date;
            pnn = await Post_Lib.GetList("A");
            InsertViews = "B";
            dnn.svDirect = "관리소장";
        }


        /// <summary>
        /// 부서 선택 시 실행
        /// </summary>
        protected async Task OnPost(ChangeEventArgs args)
        {
            string Post = args.Value.ToString();
            dnn.svPost = Post;
            fnn = await referral_Career_Lib.GetList_Post_Staff_be(Post, Apt_Code);
        }

        #region 작업분류 관련
        /// <summary>
        /// 대분류 선택
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        private async Task onWSortA(ChangeEventArgs a)
        {
            strWSortA = a.Value.ToString();
            bloom_B = await bloom_Lib.GetList_bb(strWSortA);
            dnn.svBloomA = strWSortA;

            strWSortB = "";
            strWSortC = "";
            strWSortD = "";
        }

        /// <summary>
        /// 중분류 선택
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        private async Task onWSortB(ChangeEventArgs a)
        {
            string strA = a.Value.ToString();
            //strWSortB = "Z";
            strWSortC = "";
            strWSortD = "";
            bloom_C = await bloom_Lib.GetList_cc(strA);
            dnn.svBloomB = strA;
        }

        /// <summary>
        /// 세분류 선택
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        private async Task onWSortC(ChangeEventArgs a)
        {
            string strA = a.Value.ToString();
            //strWSortC = "Z";
            strWSortD = "Z";
            bloom_D = await bloom_Lib.GetList_dd(Apt_Code, strWSortA);
            dnn.svBloomC = strA;
        }

        /// <summary>
        /// 장소 선택
        /// </summary>
        /// <param name="a"></param>
        private void onWSortD(ChangeEventArgs a)
        {
            string strA = a.Value.ToString();
            strWSortD = "Z";
            dnn.svBloom = strA;
        }
        #endregion

        /// <summary>
        /// 업무일지 입력 혹은 수정
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
            dnn.PostIP = myIPAddress;
            dnn.ModifyIP = myIPAddress;
            #endregion

            dnn.ModifyDate = DateTime.Now;
            dnn.PostDate = DateTime.Now;
            dnn.AptCode = Apt_Code;
            dnn.Apt_Name = Apt_Name;
            dnn.ComAlias = Apt_Name;
            dnn.UserIDM = User_Code;
            dnn.svYear = svDate.Year.ToString();
            dnn.svMonth = svDate.Month.ToString();
            dnn.svDay = svDate.Day.ToString();
            dnn.svClock = "1";
            dnn.svMinute = "1";

            if (string.IsNullOrEmpty(dnn.AptCode))
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "로그인되지 않았습니다..");
            }
            else if (dnn.svPost == null || dnn.svPost == "")
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "접수부서를 선택하지 않았습니다..");
            }
            else if (dnn.svReceiver == null || dnn.svReceiver == "")
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "접수자를 선택하지 않았습니다..");
            }
            else if (dnn.svDirect == null || dnn.svDirect == "")
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "지시자를 선택하지 않았습니다..");
            }
            else if (dnn.svBloomA == null || dnn.svBloomA == "")
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "작업분류가 선택되지 않았습니다..");
            }
            else if (dnn.svBloomB == null || dnn.svBloomB == "")
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "작업분류가 선택되지 않았습니다..");
            }
            else if (dnn.svBloomC == null || dnn.svBloomC == "")
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "작업분류가 선택되지 않았습니다..");
            }
            else if (dnn.svContent == null || dnn.svContent == "")
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "지시 내용을 입력하지 않았습니다..");
            }
            else
            {
                if (dnn.Num < 1)
                {
                    await works_Lib.Service_Write(dnn);
                }
                else
                {

                    await works_Lib.UpdateWorksEdit(dnn);
                }

                dnn = new Works_Entity();
                InsertViews = "A";
                await DisplayData("", "", "", "");
            }
        }

        private void OnReciever(ChangeEventArgs a)
        {
            dnn.svReceiver = a.Value.ToString();
        }

        private async Task OnDetailPost(ChangeEventArgs a)
        {
            string Post = a.Value.ToString();
            ennA.WorkPost = Post;
            fnnD = await referral_Career_Lib.GetList_Post_Staff_be(Post, Apt_Code);
        }

        /// <summary>
        /// 작업내역 작업자 만들기
        /// </summary>
        /// <param name="a"></param>
        private void OnDetailReciever(ChangeEventArgs a)
        {
            if (string.IsNullOrWhiteSpace(ennA.WorkerName))
            {
                ennA.WorkerName = ennA.WorkPost + "->" + a.Value.ToString();
            }
            else
            {
                ennA.WorkerName = ennA.WorkerName + ", " + ennA.WorkPost + "->" + a.Value.ToString();
            }
        }

        /// <summary>
        /// 업무일지 입력 모달 닫기
        /// </summary>
        private void btnClose()
        {
            InsertViews = "A";
        }

        /// <summary>
        /// 작업내용 수정
        /// </summary>
        private async Task onUpdateD(WorksSub_Entity ar)
        {
            fnn = await referral_Career_Lib.GetList_Post_Staff_be(ar.WorkPost, Apt_Code);
            ennA = ar;

            DetailsViews = "B";
        }

        /// <summary>
        /// 작업내용 삭제
        /// </summary>
        private async Task onRemoveD(int Aid)
        {
            bool isDelete = await JSRuntime.InvokeAsync<bool>("confirm", $"{Aid}번 업무이행 내역을 정말로 삭제하시겠습니까?");
            if (isDelete)
            {
                //svDate = DateTime.Now.Date;
                await worksSub_Lib.Remove(Aid);
                wnn = await worksSub_Lib.Getlist(vnn.Num.ToString());
            }
        }

        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string KeyWord { get; set; }

        /// <summary>
        /// 키월드 검색
        /// </summary>
        private async Task OnKeyWord(ChangeEventArgs a)
        {
            KeyWord = a.Value.ToString();
            Sort = "A";
            if (KeyWord != string.Empty)
            {
                await DisplayData(KeyWord, "", "", "");
            }
            else
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "단어를 입력하지 않았습니다..");
            }
        }

        /// <summary>
        /// 날짜로 검색
        /// </summary>
        /// <returns></returns>
        private async Task onBySearch()
        {
            Sort = "B";
            if (string.IsNullOrEmpty(StartDate))
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "시작일을 입력하지 않았습니다..");
            }
            else if (string.IsNullOrEmpty(EndDate))
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "종료일 입력하지 않았습니다..");
            }
            else
            {
                await DisplayData(StartDate, EndDate, "", "");
            }
        }

        /// <summary>
        /// 대분류 선택
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        private async Task onSearchA(ChangeEventArgs a)
        {
            Sort = "C_A";
            strWSortA = a.Value.ToString();
            bloom_B = await bloom_Lib.GetList_bb(strWSortA);
            strWSortB = "";
            strWSortC = "";
            if (!string.IsNullOrEmpty(strWSortA))
            {
                await DisplayData(strWSortA, "", "", "");
            }
            else
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "대분류를 선택하지 않았습니다..");
            }
        }

        /// <summary>
        /// 중분류 선택
        /// </summary>
        private async Task onSearchB(ChangeEventArgs a)
        {
            Sort = "C_B";
            strWSortB = a.Value.ToString();
            bloom_C = await bloom_Lib.GetList_cc(strWSortB);
            strWSortC = "";
            if (!string.IsNullOrEmpty(strWSortB))
            {
                await DisplayData(strWSortA, strWSortB, "", "");
            }
            else
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "중분류를 선택하지 않았습니다..");
            }
        }

        /// <summary>
        /// 소분류 선택
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        private async Task onSearchC(ChangeEventArgs a)
        {
            Sort = "C_C";
            strWSortC = a.Value.ToString();
            if (!string.IsNullOrEmpty(strWSortC))
            {
                await DisplayData(strWSortA, strWSortB, strWSortC, "");
            }
            else
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "소분류를 선택하지 않았습니다..");
            }
        }

        /// <summary>
        /// 시작일 선택
        /// </summary>
        /// <param name="a"></param>
        private void OnStart(ChangeEventArgs a)
        {
            StartDate = a.Value.ToString();
        }

        /// <summary>
        /// 종료일 선택
        /// </summary>
        /// <param name="a"></param>
        private async Task OnEnd(ChangeEventArgs a)
        {
            EndDate = a.Value.ToString();
            Sort = "B";
            if (string.IsNullOrEmpty(StartDate))
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "시작일을 입력하지 않았습니다..");
            }
            else if (string.IsNullOrEmpty(EndDate))
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "종료일 입력하지 않았습니다..");
            }
            else
            {
                await DisplayData(StartDate, EndDate, "", "");
            }
        }

        public string strTitle { get; set; }
        Works_Entity vnn { get; set; } = new Works_Entity();
        List<WorksSub_Entity> wnn { get; set; } = new List<WorksSub_Entity>();
        List<Sw_Files_Entity> Files_Entity { get; set; } = new List<Sw_Files_Entity>();
        List<SC_WH_Join_Entity> scJoin { get; set; } = new List<SC_WH_Join_Entity>();

        private async Task ByDetails(Works_Entity ar)
        {
            vnn = ar;
            await Wh_Stock();
            Views = "B";
        }

        private async Task Wh_Stock()
        {
            wnn = await worksSub_Lib.Getlist(vnn.Num.ToString());
            strTitle = "작업일지 상세정보";
            strNum = vnn.Num.ToString();
            Files_Entity = await files_Lib.Sw_Files_Data_Index("Service", vnn.Num.ToString(), Apt_Code);
            scJoin = await whSock_Lib.Group_List_ABC(Apt_Code, "Service", vnn.Num.ToString());
        }

        private void btnCloseV()
        {
            Views = "A";
        }

        /// <summary>
        /// 첨부파일 보기
        /// </summary>
        public string FileViews { get; set; } = "A";
        public int Files_Count { get; set; }
        public string FileInsertViews { get; private set; }

        //List<Sw_Files_Entity> Files_Entity { get; set; } = new List<Sw_Files_Entity>();
        private async Task onFileViews()
        {
            FileViews = "B";
            Files_Count = await files_Lib.Sw_Files_Data_Index_Count("Service", vnn.Num.ToString(), Apt_Code);
            if (Files_Count > 0)
            {
                Files_Entity = await files_Lib.Sw_Files_Data_Index("Service", vnn.Num.ToString(), Apt_Code);
            }
        }

        

        /// <summary>
        /// 작업 일지 새로 등록
        /// </summary>
        private void onOpen()
        {
            DetailsViews = "B";
            InOutViews = "Z";
            ennA.WorkDate = DateTime.Now;
            ennA.Out_In_Viw = "내부작업";
        }

        /// <summary>
        /// 지시사항 수정 열기
        /// </summary>
        private async Task onEditOpen(Works_Entity ar)
        {
            EditViews = "B";
            dnn = ar;
            fnn = await referral_Career_Lib.GetList_Post_Staff_be(vnn.svPost, Apt_Code);

            strWSortA = vnn.svBloomA;
            bloom_B = await bloom_Lib.GetList_bb(strWSortA);
            strWSortB = vnn.svBloomB;
            bloom_C = await bloom_Lib.GetList_cc(strWSortB);
            strWSortC = dnn.svBloomC;
            bloom_D = await bloom_Lib.GetList_dd(Apt_Code, strWSortA);
            strWSortD = vnn.svBloom;
            dnn.ModifyDate = DateTime.Now;

            dnn.svBloomA = strWSortA;
            dnn.svBloomB = strWSortB;
            dnn.svBloomC = strWSortC;
            dnn.svBloom = strWSortD;
        }

        /// <summary>
        /// 지시사항 수정 닫기
        /// </summary>
        private void btnWorkClose()
        {
            EditViews = "A";
        }

        /// <summary>
        /// 지시사항 수정
        /// </summary>
        /// <returns></returns>
        private async Task btnWorkSave()
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
            //dnn.PostIP = myIPAddress;
            dnn.ModifyIP = myIPAddress;
            dnn.ModifyDate = DateTime.Now;
            #endregion
            if (string.IsNullOrEmpty(dnn.AptCode))
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "로그인되지 않았습니다..");
            }
            else if (dnn.svPost == null || dnn.svPost == "")
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "접수부서를 선택하지 않았습니다..");
            }
            else if (dnn.svReceiver == null || dnn.svReceiver == "")
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "접수자를 선택하지 않았습니다..");
            }
            else if (dnn.svDirect == null || dnn.svDirect == "")
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "지시자를 선택하지 않았습니다..");
            }
            else if (dnn.svBloomA == null || dnn.svBloomA == "")
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "작업분류가 선택되지 않았습니다..");
            }
            else if (dnn.svBloomB == null || dnn.svBloomB == "")
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "작업분류가 선택되지 않았습니다..");
            }
            else if (dnn.svBloomC == null || dnn.svBloomC == "")
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "작업분류가 선택되지 않았습니다..");
            }
            else if (dnn.svContent == null || dnn.svContent == "")
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "지시 내용을 입력하지 않았습니다..");
            }
            else
            {
                if (dnn.Num > 0)
                {
                    await works_Lib.UpdateWorksEdit(dnn);
                }

                EditViews = "A";
            }
        }

        #region 자재관련 정보 클래스
        /// <summary>
        /// 자재 입출력 입력열기
        /// </summary>
        private void onStock()
        {
            StocksViews = "B";
        }

        /// <summary>
        /// 자재 입출고 입력
        /// </summary>
        public DateTime Wh_Date { get; set; }
        private async Task btnStocksSave()
        {
            wheE.AptCode = Apt_Code;
            wheE.Parents = vnn.Num.ToString();
            wheE.P_Group = "Service";
            wheE.Wh_UserID = User_Code;
            wheE.Wh_Year = Wh_Date.Year.ToString();
            wheE.Wh_Month = Wh_Date.Month.ToString();
            wheE.Wh_Day = Wh_Date.Day.ToString();
            wheE.Wh_Code = "wh" + await whSock_Lib.Wh_Count_Data();
            wheE.Wh_Use = wheE.Etc;
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
            wheE.PostIP = myIPAddress;
            wheE.ModifyIP = myIPAddress;
            wheE.ModifyDate = DateTime.Now;
            #endregion

            if (wheE.Wh_Quantity < 0)
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "입출고 수량을 입력하지 않았습니다..");
            }
            else if (wheE.St_Code == null || wheE.St_Code == "")
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "자재코드가 선택되지 않습니다...");
            }
            else
            {
                if (wheE.Num < 1)
                {
                    await whSock_Lib.Warehouse_Write(wheE);
                }
                else
                {
                    await whSock_Lib.Warehouse_Update(wheE);
                }
                scJoin = await whSock_Lib.Group_List_ABC(Apt_Code, "Service", dnn.Num.ToString());

                await Wh_Stock();
                wheE = new WareHouse_Entity();

                StocksViews = "A";
            }

        }

        /// <summary>
        /// 자재명 찾기 (유사한 자재명 목록)
        /// </summary>
        private async Task OnStocksName(ChangeEventArgs a)
        {
            sceL = await stocks_Lib.stName_Query(Apt_Code, a.Value.ToString());
        }

        /// <summary>
        /// 선택된 자재입출고 상세정보 불러오기
        /// </summary>
        public string strSt_Name { get; set; }
        public string StocksViews { get; private set; }
        public int strWh_Balance { get; set; } = 0;
        public string InputCompleteViews { get; private set; }

        private async Task OnSelectName(ChangeEventArgs a)
        {
            try
            {
                var wh = await whSock_Lib.Wh_View_StCode(Apt_Code, a.Value.ToString()); //자재 코드로 입출고 내역 불러오기
                strWh_Balance = wh.Wh_Balance;  // wheE.Wh_Balance = wheE.Wh_Balance ?? 0; //잔고 값이 없으면 0으로 초기화
            }
            catch (Exception)
            {
                strWh_Balance = 0;
            }

            var strStocks = await stocks_Lib.St_View_Code(a.Value.ToString());
            wheE.St_Code = strStocks.St_Code;
            wheE.St_Group = strStocks.St_Group;
            wheE.Wh_Unit = strStocks.St_Unit;
            strSt_Name = strStocks.St_Name;//자재명 불러오기
        }

        /// <summary>
        /// 입출고 수량 입력 실행
        /// </summary>
        private async Task OnQuantity(ChangeEventArgs a)
        {
            wheE.Wh_Quantity = Convert.ToInt32(a.Value);
            if (wheE.Wh_Section == "B")
            {
                wheE.Wh_Balance = strWh_Balance - wheE.Wh_Quantity;
                wheE.Wh_Cost = 0;
            }
            else if (wheE.Wh_Section == "A")
            {
                wheE.Wh_Balance = strWh_Balance + wheE.Wh_Quantity;
            }
            else
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "알 수 없는 문제가 발생하였습니다. \n 관리자에게 문의 하세요..");
            }
        }

        /// <summary>
        /// 입출고 내역 선택 시 실행
        /// </summary>
        private void OnSection(ChangeEventArgs a)
        {
            wheE.Wh_Section = a.Value.ToString();
            wheE.Wh_Quantity = 0;
            wheE.Wh_Cost = 0;

        }

        /// <summary>
        /// 자재관리 입력 폼 닫기
        /// </summary>
        private void btnStocksClose()
        {
            StocksViews = "A";
        }

        /// <summary>
        /// 자재관리 수정 열기
        /// </summary>
        private async Task onStocksEdit(SC_WH_Join_Entity ae)
        {
            wheE.Num = ae.Num;
            wheE.AptCode = ae.AptCode;
            wheE.Etc = ae.Etc;
            wheE.Parents = ae.Parents;
            wheE.P_Group = ae.P_Group;
            wheE.St_Code = ae.St_Code;
            wheE.St_Group = ae.St_Group;
            wheE.Wh_Balance = ae.Wh_Balance;
            wheE.Wh_Code = ae.Wh_Code;
            wheE.Wh_Cost = ae.Wh_Cost;
            wheE.Wh_Day = ae.Wh_Day;
            wheE.Wh_Month = ae.Wh_Month;
            wheE.Wh_Place = ae.Wh_Place;
            wheE.Wh_Quantity = ae.Wh_Quantity;
            wheE.Wh_Section = ae.Wh_Section;
            wheE.Wh_Unit = ae.Wh_Unit;
            wheE.Wh_Use = ae.Wh_Use;
            wheE.Wh_UserID = ae.Wh_UserID;
            wheE.Wh_Year = ae.Wh_Year;


            if (ae.St_Section == "A")
            {
                strWh_Balance = ae.Wh_Balance - ae.Wh_Quantity;
            }
            else if (ae.St_Section == "B")
            {
                strWh_Balance = ae.Wh_Balance + ae.Wh_Quantity;
            }

            sceL = await stocks_Lib.stName_Query(Apt_Code, ae.St_Name);
            strSt_Name = ae.St_Name;

            StocksViews = "B";
        }

        /// <summary>
        /// 자재관리 정보 삭제
        /// </summary>
        private async Task onStocksRemove(int Num)
        {
            bool isDelete = await JSRuntime.InvokeAsync<bool>("confirm", $"{Num}번 자재관리정보을 정말로 삭제하시겠습니까?");
            if (isDelete)
            {
                //svDate = DateTime.Now.Date;
                await whSock_Lib.Wh_Delete(Num);
                //await DisplayData("", "", "", "");
                await Wh_Stock();
            }

        }
        #endregion

        /// <summary>
        /// 작업완료 등록 열기
        /// </summary>
        private void onComplete()
        {
            InputCompleteViews = "B";
        }

        /// <summary>
        /// 작업완료 등록 닫기
        /// </summary>
        private void btnWorkCompleteClose()
        {
            InputCompleteViews = "A";
        }

        /// <summary>
        /// 작업완료 여부 입력
        /// </summary>
        private async Task btnWorkCompleteSave(int Num)
        {
            await works_Lib.ServiceComplete(Num.ToString());
            vnn = await works_Lib.Service_View_Num(vnn.Num);
            await DisplayData("", "", "", "");
        }


        /// <summary>
        /// 내부 혹은 외부 작선 선택 실행
        /// </summary>
        private void onInOutCom(ChangeEventArgs a)
        {
            InOutViews = a.Value.ToString();
            ennA.Out_In_Viw = a.Value.ToString();
        }

        /// <summary>
        /// 외부업체 찾기
        /// </summary>
        public string Company_num { get; set; }
        private async Task OnCompanyInfor(ChangeEventArgs a)
        {
            Company_num = a.Value.ToString();
            var companyInfor = await company_Lib.CorNum_Detail(a.Value.ToString());
            ennA.OutCorName = companyInfor.Cor_Name;
            ennA.OutCorMobile = companyInfor.Mobile;
            ennA.OutCorCharger = companyInfor.Ceo_Name;
            ennA.Scw_Code = companyInfor.Cor_Code;
        }

        /// <summary>
        /// 업무 이행 내역 수정 열기
        /// </summary>
        private async Task onUpdate(WorksSub_Entity wse)
        {
            fnn = await referral_Career_Lib.GetList_Post_Staff_be(wse.WorkPost, Apt_Code);
            ennA = wse;

            InsertViews = "B";
        }

        /// <summary>
        /// 업무일지 이행 내역 삭제
        /// </summary>
        private async Task onRemove(int Aid)
        {
            bool isDelete = await JSRuntime.InvokeAsync<bool>("confirm", $"{Aid}번 업무이행 내역을 정말로 삭제하시겠습니까?");
            if (isDelete)
            {
                //svDate = DateTime.Now.Date;
                await worksSub_Lib.Remove(Aid);
                await DisplayData("", "", "", "");
            }
        }


        #region 파일 첨부 관련 클래스
        

        /// <summary>
        /// 파일 첨부 열기
        /// </summary>
        private void onFileInput()
        {
            FileInsertViews = "B";
        }

        /// <summary>
        /// 파일 첨부 닫기
        /// </summary>
        private void btnFileClose()
        {
            FileInsertViews = "A";
        }

        /// <summary>
        /// 파일 보기 닫기
        /// </summary>
        private void FilesViewsClose()
        {
            FileViews = "A";
            //StateHasChanged();
        }

        /// <summary>
        /// 확정자 찾기오기
        /// </summary>
        private string OnExtension(string filesName)
        {
            return Path.GetExtension(filesName);
        }

        [Inject] ILogger<Index> Logger { get; set; }
        private List<IBrowserFile> loadedFiles = new();
        private long maxFileSize = 1024 * 1024 * 30;
        private int maxAllowedFiles = 5;
        private bool isLoading;
        private decimal progressPercent;
        Sw_Files_Entity finn { get; set; } = new Sw_Files_Entity();
        public string fileName { get; set; }

        private async Task LoadFiles(InputFileChangeEventArgs e)
        {
            isLoading = true;
            loadedFiles.Clear();
            progressPercent = 0;

            foreach (var file in e.GetMultipleFiles(maxAllowedFiles))
            {
                finn.Parents_Num = vnn.Num.ToString(); // 선택한 ParentId 값 가져오기 
                finn.Sub_Num = finn.Parents_Num;
                try
                {
                    var pathA = $"D:\\Msh\\Home\\Ayoung\\sw_Files_View\\Sw_files";
                    // 파일의 확장자를 가져옴
                    string strFiles = Path.GetExtension(file.Name);
                    // 파일 이름에 Works + 아파트 코드 + 날짜 + 확장자 형식으로 생성
                    string _FileName = "Works" + Apt_Code + DateTime.Now.ToShortDateString() + strFiles;
                    // 중복되지 않는 파일 이름을 생성
                    fileName = Dul.FileUtility.GetFileNameWithNumbering(pathA, _FileName);

                    //fileName = Dul.FileUtility.GetFileNameWithNumbering(pathA, file.Name);
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

                        //StateHasChanged();
                    }

                    loadedFiles.Add(file);

                    finn.Sw_FileName = fileName;
                    finn.Sw_FileSize = Convert.ToInt32(file.Size);
                    finn.Parents_Name = "Service";
                    finn.AptCode = Apt_Code;
                    finn.Del = "A";

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
                    finn.PostIP = myIPAddress;
                    #endregion
                    await files_Lib.Sw_Files_Date_Insert(finn); //첨부파일 데이터 db 저장
                }
                catch (Exception ex)
                {
                    Logger.LogError("File: {Filename} Error: {Error}",
                        file.Name, ex.Message);
                }
            }

            FileInsertViews = "A";

            Files_Count = await files_Lib.Sw_Files_Data_Index_Count("Service", vnn.Num.ToString(), Apt_Code);
            if (Files_Count > 0)
            {
                Files_Entity = await files_Lib.Sw_Files_Data_Index("Service", vnn.Num.ToString(), Apt_Code);
            }

            isLoading = false;
        }


        public string CompleteViews { get; private set; }
        public string strUserCode { get; private set; }

        

        /// <summary>
        /// 첨부 파일 삭제
        /// </summary>
        protected async Task ByFileRemove(Sw_Files_Entity _files)
        {

            bool isDelete = await JSRuntime.InvokeAsync<bool>("confirm", $"{_files.Sw_FileName} 첨부파일을 정말로 삭제하시겠습니까?");

            if (isDelete)
            {
                if (!string.IsNullOrEmpty(_files?.Sw_FileName))
                {
                    // 첨부 파일 삭제 
                    //await FileStorageManagerInjector.DeleteAsync(_files.Sw_FileName, "");
                    string rootFolder = $"D:\\Msh\\Home\\Ayoung\\sw_Files_View\\Sw_files\\{_files.Sw_FileName}";
                    File.Delete(rootFolder);
                }
                await files_Lib.FileRemove(_files.Num.ToString(), "Service", Apt_Code);
                //await UploadRepositoryAsyncReference.DeleteAsync(Id); // 삭제
                //NavigationManagerReference.NavigateTo("/Uploads"); // 리스트 페이지로 이동
                Files_Count = await files_Lib.Sw_Files_Data_Index_Count("Service", dnn.Num.ToString(), Apt_Code);
                if (Files_Count > 0)
                {
                    Files_Entity = await files_Lib.Sw_Files_Data_Index("Service", vnn.Num.ToString(), Apt_Code);
                }
            }
            else
            {
                await JSRuntime.InvokeAsync<object>("alert", "취소되었습니다.");
            }
        }
        #endregion

        /// <summary>
        /// 작업내용 입력 모달 닫기
        /// </summary>
        private void btnDetailClose()
        {
            DetailsViews = "A";
        }

        /// <summary>
        /// 작업 내용 입력 저장
        /// </summary>
        private async Task btnDetailSave()
        {
            ennA.AptCode = Apt_Code;
            ennA.subDay = ennA.WorkDate.Day.ToString();
            ennA.subMonth = ennA.WorkDate.Month.ToString();
            ennA.subYear = ennA.WorkDate.Year.ToString();
            ennA.UserID = User_Code;
            ennA.Service_Code = vnn.Num.ToString();
            ennA.Net_Group = "Service";
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
            ennA.PostIP = myIPAddress;
            ennA.ModifyIP = myIPAddress;
            ennA.ModifyDate = DateTime.Now;
            #endregion

            if (ennA.WorkContent == null || ennA.WorkContent == "")
            {
                AlertViews = "B";
                AlertBodyA = "작업내용을 입력하지 않았습니다. \n 속상해요.";
                //await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "작업 내용을 입력하지 않았습니다..");
            }
            else if (ennA.Out_In_Viw == null || ennA.Out_In_Viw == "" || ennA.Out_In_Viw == "Z")
            {
                AlertViews = "B";
                AlertBodyA = "내부작업인지 외부작인지 선택하지 않았습니다... \n 속상해요.";
                //await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "내부작업인지 외부작인지 선택하지 않았습니다..");
            }
            else if (ennA.WorkPost == null || ennA.WorkPost == "" || ennA.WorkPost == "Z")
            {
                AlertViews = "B";
                AlertBodyA = "어찌 이런 일이....";
                AlertBodyB = "작업부서를 입력하지 않았다니!";
                AlertBodyC = "빨리 입력해 주세용";
                //await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "작업부서를 선택하지 않았습니다..");
            }
            else if (ennA.WorkerName == null || ennA.WorkerName == "")
            {
                AlertViews = "B";
                AlertBodyA = "작업자를 입력하지 않았습니다... 속상해요.";
                //await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "작업자를 입력하지 않았습니다..");
            }
            
            else if (ennA.Out_In_Viw == null || ennA.Out_In_Viw == "" || ennA.Out_In_Viw == "Z")
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "내부작업인지 외부작인지 선택하지 않았습니다..");
            }
            else if (ennA.Service_Code == null || ennA.Service_Code == "")
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "작업코드가 없습니다. 저장할 수 없으니 다시 로그인하여 주십시요..");
            }
            else
            {
                if (ennA.Aid < 1)
                {
                    await worksSub_Lib.Add(ennA);
                }
                else
                {
                    await worksSub_Lib.Edit(ennA);
                }

                ennA = new WorksSub_Entity();
                DetailsViews = "A";
                wnn = await worksSub_Lib.Getlist(vnn.Num.ToString());
            }
        }

        #region 결제 여부(작업)
        Decision_Entity decision { get; set; } = new Decision_Entity();
        Referral_career_Entity referral { get; set; } = new Referral_career_Entity();
        List<Approval_Entity> app { get; set; } = new List<Approval_Entity>();
        [Inject] public IApproval_Lib approval_Lib { get; set; }
        [Inject] public IDbImagesLib dbImagesLib { get; set; }
        public string strUserName { get; set; }
        public string decisionA { get; set; }
        public string strNum { get; set; }

        /// <summary>
        ///결재여부
        /// </summary>
        /// <param name="objPostDate"></param>
        /// <returns></returns>
        public string FuncShowOK(string strPostDuty, string Apt_Code, int apNum)
        {
            string strBloomCode = "Service";
            string strUserName = "";
            string nu = apNum.ToString();
            strNum = nu;
            int intA = decision_Lib.Details_Count(Apt_Code, strBloomCode, nu, strPostDuty);
            if (intA > 0)
            {
                decision = decision_Lib.Details(Apt_Code, strBloomCode, strNum, strPostDuty);
                //PostDuty = decision.PostDuty;
                strUserName = decision.UserName;
                strUserCode = decision.User_Code;
            }
            else
            {
                strPostDuty = "";
                strUserName = "";
            }


            if (intA < 1)
            {
                decisionA = "결재하기";
                return "결재하기";
            }
            else
            {
                decisionA = strUserName;
                return strUserName;
            }
        }

        /// <summary>
        /// 결재도장 불러오기
        /// </summary>
        public byte[] sealImage(string aptcode, string post, string duty, object apNum, string BloomCode, string User_Code)
        {
            string strNum = apNum.ToString();
            string strPostDuty = "";
            if (post == "회계(경리)")
            {
                post = "회계";
            }
            if (duty == "담당자")
            {
                strPostDuty = duty;
            }
            else
            {
                strPostDuty = post + duty;
            }

            byte[] strResult;
            int Being = decision_Lib.Details_Count(aptcode, BloomCode, strNum, strPostDuty);

            if (Being > 0)
            {
                Decision_Entity ann = decision_Lib.Details(aptcode, BloomCode, apNum.ToString(), strPostDuty);

                if (ann.User_Code != null && ann.User_Code != "A")
                {
                    if (strPostDuty != "담당자")
                    {
                        if (post == "회계")
                        {
                            post = "회계(경리)";
                        }
                        //string sstPostDuty = post + duty;

                        //decision = decision_Lib.Details(Apt_Code, BloomCode, strNum, strPostDuty);
                        //string User_Code = referral_Career_Lib.User_Code_Bes(aptcode, post, duty, );
                        int CodeBeing = dbImagesLib.Photos_Count(User_Code);

                        if (CodeBeing > 0)
                        {
                            strResult = dbImagesLib.Photos_image(User_Code);
                        }
                        else
                        {
                            strResult = dbImagesLib.Photos_image("confirmation");
                        }
                    }
                    else
                    {
                        strResult = dbImagesLib.Photos_image("confirmation");
                    }
                }
                else if (ann.User_Code == "A")
                {
                    strResult = dbImagesLib.Photos_image("confirmation");
                }
                else
                {
                    strResult = dbImagesLib.Photos_image("inn");
                }
            }
            else
            {
                strResult = dbImagesLib.Photos_image("inn");
            }

            return strResult;
        }

        /// <summary>
        /// 결재 입력
        /// </summary>
        public async Task btnDecision()
        {
            decision.AptCode = Apt_Code;
            decision.BloomCode = "Service";
            decision.Parent = vnn.Num.ToString();
            if (referral.Duty == "직원" || referral.Duty == "반원" || referral.Duty == "반장" || referral.Duty == "주임" || referral.Duty == "기사" || referral.Duty == "서무")
            {
                decision.PostDuty = "담당자";
            }
            else
            {
                decision.PostDuty = referral.Post + referral.Duty;
            }
            decision.PostDuty = referral.Post + referral.Duty;
            decision.User_Code = User_Code;
            decision.UserName = User_Name;

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
            decision.PostIP = myIPAddress;
            #endregion

            if (decision.PostDuty == "관리소장")
            {
                if (vnn.Complete == "B")
                {
                    await decision_Lib.Add(decision);
                    await works_Lib.ServiceConform(vnn.Num.ToString());

                    app = await approval_Lib.GetList(Apt_Code, "작업일지");
                    await DisplayData("", "", "", "");
                    vnn = await works_Lib.Service_View_Num(vnn.Num);
                }
                else
                {
                    await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "작업이 완료되지 않았습니다..");
                }
            }
            else
            {
                await decision_Lib.Add(decision);
                //await appeal.Edit_Complete(Code.ToString());

                app = await approval_Lib.GetList(Apt_Code, "작업일지");
                await DisplayData("", "", "", "");
            }
            StateHasChanged();
        }

        #endregion

        /// <summary>
        /// 검색
        /// </summary>
        public string SearchViews { get; set; } = "A";
        private void onSearch_Open()
        {
            SearchViews = "B";
            strTitle = "작업일지 검색";
        }

        private void btnCloseS()
        {
            SearchViews = "A";
        }

        private void OnInput()
        {
            AlertViews = "B";
            AlertBodyA= "문제가 발생하였습니다. 관리자에게 문의하세요.";
        }

        public string? AlertBodyA { get; set; }
        public string? AlertBodyB { get; set; }
        public string? AlertBodyC { get; set; }
        public string? AlertBodyD { get; set; }

        public string AlertViews { get; set; } = "A";

        private void btnCloseZ()
        {
            AlertViews = "A";
            AlertBodyA = ""; AlertBodyB = ""; AlertBodyC = ""; AlertBodyD = "";
        }
    }
}
