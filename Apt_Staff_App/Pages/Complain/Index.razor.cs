using Erp_Apt_Lib;
using Erp_Apt_Lib.Appeal;
using Erp_Apt_Lib.Decision;
using Erp_Apt_Staff;
using Erp_Entity;
using Erp_Lib;
using Facilities;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using Works;

namespace Apt_Staff_App.Pages.Complain
{
    public partial class Index
    {
        #region Inject
        [Parameter] public string Code { get; set; }
        [CascadingParameter] public Task<AuthenticationState> AuthenticationStateRef { get; set; }
        [Inject] public IApt_Lib apt_Lib { get; set; }
        [Inject] public IAppeal_Lib appeal { get; set; }
        [Inject] public NavigationManager MyNav { get; set; }
        [Inject] IJSRuntime JSRuntime { get; set; }
        [Inject] public IReferral_career_Lib referral_Career_Lib { get; set; }
        [Inject] public IPost_Lib Post_Lib { get; set; }
        [Inject] public IErp_AptPeople_Lib Erp_AptPeople_Lib { get; set; }
        [Inject] public IAppeal_Bloom_Lib appeal_Bloom_Lib { get; set; }
        [Inject] public IBloom_Lib bloom_Lib { get; set; }
        [Inject] public IWorks_Lib works_Lib { get; set; }

        [Inject] public ISw_Files_Lib files_Lib { get; set; } // 파일 첨부
        [Inject] public IsubAppeal_Lib subAppeal { get; set; } //민원처리
        [Inject] public IsubWorker_Lib subWorker { get; set; } // 민원처리자
        [Inject] public IDecision_Lib decision_Lib { get; set; } // 결재정보
                                                                 //[Inject] public IWorksSub_Lib worksSub_Lib { get; set; }
        #endregion

        #region 목록
        List<Appeal_Entity> ann = new List<Appeal_Entity>();
        Referral_career_Entity cnn { get; set; } = new Referral_career_Entity();
        Appeal_Entity bnn { get; set; } = new Appeal_Entity();
        Appeal_Entity vnn { get; set; } = new Appeal_Entity();
        List<Post_Entity> pnn { get; set; } = new List<Post_Entity>();
        List<Referral_career_Entity> dnn = new List<Referral_career_Entity>();
        List<Apt_People_Entity> Dong = new List<Apt_People_Entity>();
        List<Approval_Entity> app { get; set; } = new List<Approval_Entity>();
        Referral_career_Entity referral { get; set; } = new Referral_career_Entity();
        List<Referral_career_Entity> wnn { get; set; } = new List<Referral_career_Entity>();
        List<subAppeal_Entity> snn { get; set; } = new List<subAppeal_Entity>();
        subAppeal_Entity sub { get; set; } = new subAppeal_Entity();

        Sw_Files_Entity fnn { get; set; } = new Sw_Files_Entity();

        public string strDong_No { get; private set; }

        List<Apt_People_Entity> Ho = new List<Apt_People_Entity>();
        Apt_People_Entity apt_Pople { get; set; } = new Apt_People_Entity();
        List<Apt_People_Entity> apt_Pople_List { get; set; } = new List<Apt_People_Entity>();
        List<Appeal_Bloom_Entity> snnA = new List<Appeal_Bloom_Entity>();
        List<Appeal_Bloom_Entity> snnB = new List<Appeal_Bloom_Entity>();
        Appeal_Bloom_Entity abe { get; set; } = new Appeal_Bloom_Entity();
        List<Bloom_Entity> bloom_A = new List<Bloom_Entity>();
        List<Bloom_Entity> bloom_B = new List<Bloom_Entity>();
        List<Bloom_Entity> bloom_C = new List<Bloom_Entity>();
        List<Bloom_Entity> bloom_D = new List<Bloom_Entity>();
        Works_Entity works_s { get; set; } = new Works_Entity();
        List<Sw_Files_Entity> Files_Entity { get; set; } = new List<Sw_Files_Entity>();

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
        public string RemoveViews { get; set; } = "A";
        public string DetailsInsert { get; set; } = "A";
        public string strPost { get; set; }
        public DateTime apDate { get; set; }
        public string PrivateViews { get; set; } = "A";
        public string strSortA { get; set; } = "Z";
        public string strSortB { get; set; } = "Z";
        public string strWSortA { get; set; } = "Z";
        public string strSortSort { get; private set; }
        public string strWSortB { get; set; } = "Z";
        public string strWSortC { get; set; } = "Z";
        public string strWSortD { get; set; } = "Z";
        public int intNum { get; set; }

        public string strKeyword { get; set; }
        public string strBicSort { get; set; }
        public string strSmallSort { get; set; }
        public string strStartDate { get; set; } = DateTime.Now.ToShortDateString();
        public string strEndDate { get; set; } = DateTime.Now.ToShortDateString();
        public string strDong { get; set; }
        public string strHo { get; set; }

        public int Files_Count { get; set; } = 0;
        public string FileViews { get; set; } = "A";
        public string FileInsertViews { get; set; } = "A";

        //public string DetailsViews { get; set; } = "A";
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

                //로그인 정보
                Apt_Code = authState.User.Claims.FirstOrDefault(c => c.Type == "Apt_Code")?.Value;
                User_Code = authState.User.Claims.FirstOrDefault(c => c.Type == "User_Code")?.Value;
                Apt_Name = authState.User.Claims.FirstOrDefault(c => c.Type == "Apt_Name")?.Value;
                User_Name = authState.User.Claims.FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name")?.Value;

                //try
                //{
                cnn = await referral_Career_Lib.Detail(User_Code);
                PostDuty = cnn.Post + cnn.Duty;
                apDate = DateTime.Now.Date;
                pnn = await Post_Lib.GetList("A");//부서 목록
                Dong = await Erp_AptPeople_Lib.DongList(Apt_Code); //동이름 목록
                snnA = await appeal_Bloom_Lib.Sort_Name_List();//민원 대분류 목록
                                                               //snnB = await appeal_Bloom_Lib.
                bloom_A = await bloom_Lib.GetList_Apt_ba(Apt_Code); // 작업 대분류
                strStartDate = DateTime.Now.ToShortDateString();
                strEndDate = DateTime.Now.ToShortDateString();

                app = await approval_Lib.GetList(Apt_Code, "민원일지");
                pnn = await post_Lib.GetList("A");
                referral = await referral_Career_Lib.Details(User_Code);
                PostDuty = referral.Post + referral.Duty;
                //}
                //catch (Exception)
                //{
                //    MyNav.NavigateTo("/");
                //}

                await DisplayData(null);
            }
            else
            {
                AlertBodyA = "로그인되지 않았습니다";
                AlertBodyB = "로그인하지 않고는 사용이 가능하지 않습니다. 먼저 로그인을 해 주세요.";
                AlertViews = "B";
                //await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "로그인되지 않았습니다..");
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
        private async Task DisplayData(string sort)
        {
            //try
            //{
            if (sort == null || sort == "")
            {
                // 해당 공동주택의 민원 목록
                pager.RecordCount = await appeal.getlist_apt_count(Apt_Code);
                ann = await appeal.getlist_apt(pager.PageIndex, Apt_Code);
            }
            else
            {
                if (strSortSort == "A")
                {
                    //해당 공동주택 분류로 검색된 민원 목록
                    pager.RecordCount = await appeal.getlistSortA_Count(Apt_Code, sort);
                    ann = await appeal.getlistSortA(pager.PageIndex, Apt_Code, sort);
                }
                else if (strSortSort == "B")
                {
                    //해당 공동주택 동과 호로 검색된 민원 목록
                    pager.RecordCount = await appeal.getlistDongHo_count(Apt_Code, strDong_No, sort);
                    ann = await appeal.getlistDongHo(pager.PageIndex, Apt_Code, strDong_No, sort);
                }
                else if (strSortSort == "C")
                {
                    //해당 공동주택 날짜로 검색된 민원 목록
                    pager.RecordCount = await appeal.getlist_apt_New_Count(Apt_Code, dtB, sort);
                    ann = await appeal.getlist_apt_New(pager.PageIndex, Apt_Code, dtB, sort);
                }
                else if (strSortSort == "D")
                {
                    // 해당 공동주택에서 키워드로 검색된 민원 목록
                    pager.RecordCount = await appeal.getlist_Search_Count(Apt_Code, sort);
                    ann = await appeal.getlist_Search(pager.PageIndex, Apt_Code, sort);
                }
            }

            intNum = pager.RecordCount - (pager.PageIndex * pager.PageSize);
            //}
            //catch (Exception)
            //{
            //    MyNav.NavigateTo("/");
            //}
        }

        private async void OnKeyWord(ChangeEventArgs a)
        {
            string strKW = a.Value.ToString();
            if (strKW != null)
            {
                strSortSort = "D";
                await DisplayData(strKW);
            }
        }

        /// <summary>
        /// 시작시간
        /// </summary>
        private void OnStartDate(ChangeEventArgs a)
        {
            dtB = a.Value.ToString();
            strStartDate = a.Value.ToString();
        }

        /// <summary>
        /// 종료시간
        /// </summary>
        /// <param name="a"></param>
        private async void OnEndDate(ChangeEventArgs a)
        {
            string dtA = a.Value.ToString();
            strEndDate = a.Value.ToString();
            strSortSort = "C";
            await DisplayData(dtA);
        }

        /// <summary>
        /// 상세보기로 이동
        /// </summary>
        public string strNum { get; set; }
        public async Task ByAid(Appeal_Entity ar)
        {
            try
            {
                vnn = ar;
                DetailsViews = "B";
                snn = await subAppeal.GetList(vnn.Num.ToString());
                strNum = vnn.Num.ToString();
                Files_Count = await files_Lib.Sw_Files_Data_Index_Count("Appeal", vnn.Num.ToString(), Apt_Code);
            }
            catch (Exception)
            {
                MyNav.NavigateTo("/");
            }

        }

        /// <summary>
        /// 수정
        /// </summary>
        public async Task ByEdit(Appeal_Entity appeal_Entity)
        {
            InsertViews = "B";
            DetailsViews = "A";
            Ho = await Erp_AptPeople_Lib.Dong_HoList(Apt_Code, appeal_Entity.apDongNo);
            dnn = await referral_Career_Lib.GetList_Post_Staff_be(appeal_Entity.apPost, Apt_Code);
            abe = await appeal_Bloom_Lib.Details_Code(appeal_Entity.Bloom_Code);
            snnB = await appeal_Bloom_Lib.Asort_Name_List(abe.Sort);
            strSortA = abe.Sort;
            bnn = appeal_Entity;

            //StateHasChanged();
        }

        /// <summary>
        /// 민원 접수 삭제
        /// </summary>
        protected async Task ByRemove(Appeal_Entity appeal_Entity)
        {
            bool isDelete = await JSRuntime.InvokeAsync<bool>("confirm", $"{appeal_Entity.apTitle} 민원을 정말로 삭제하시겠습니까?");

            if (isDelete)
            {
                await appeal.Remove(appeal_Entity.Num.ToString());
            }

            await DisplayData(sort_sort);
            //StateHasChanged();
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
        };

        /// <summary>
        /// 페이징
        /// </summary>
        protected async void PageIndexChanged(int pageIndex)
        {
            pager.PageIndex = pageIndex;
            pager.PageNumber = pageIndex + 1;

            await DisplayData(sort_sort);

            StateHasChanged();
        }

        /// <summary>
        /// 민원 접수 입력 열기
        /// </summary>
        protected void onAppealInsert_Open()
        {
            InsertViews = "B";
            bnn.Private = "전용";
        }

        /// <summary>
        /// 민원 접수 입력 닫기
        /// </summary>
        private void btnClose()
        {
            InsertViews = "A";
        }

        /// <summary>
        /// 민원 접수 등록
        /// </summary>
        /// <returns></returns>
        public async Task btnAppealSave()
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
            bnn.PostIP = myIPAddress;
            #endregion
            bnn.PostDate = DateTime.Now;
            bnn.AptCode = Apt_Code;
            bnn.AptName = Apt_Name;
            bnn.apYear = apDate.Year.ToString();
            bnn.apMonth = apDate.Month.ToString();
            bnn.apDay = apDate.Day.ToString();
            bnn.apClock = bnn.PostDate.Hour.ToString();
            bnn.apMinute = bnn.PostDate.Minute.ToString();
            bnn.Bloom_Code = strSortA;
            //bnn.ComAlias = Apt_Code;
            //bnn.ComTitle = Apt_Name;

            works_s.AptCode = Apt_Code;
            works_s.Apt_Name = Apt_Name;
            //works_s.ComAlias = Apt_Code;
            works_s.PostDate = DateTime.Now;
            works_s.PostIP = bnn.PostIP;
            //works_s.subClock = works_s.PostDate.Hour.ToString();
            works_s.svPost = bnn.apPost;
            works_s.svReceiver = bnn.apReciever;
            works_s.svDay = apDate.Day.ToString();
            works_s.svMinute = bnn.PostDate.Minute.ToString();
            works_s.svMonth = apDate.Month.ToString();
            works_s.svYear = apDate.Year.ToString();
            works_s.svDirect = "관리소장";
            works_s.svBloomCode = "";
            works_s.svBloomA = strWSortA;
            works_s.svBloomB = strWSortB;
            works_s.svBloomC = strWSortC;
            works_s.svBloom = strWSortD;
            works_s.svContent = "민원일지에서 " + bnn.apDongNo + "동 " + bnn.apHoNo + "호 요청으로 " + " 입력 됨. ☞ " + bnn.apContent;
            works_s.UserIDM = User_Code;
            if (bnn.AptCode == null)
            {
                AlertBodyA = "로그인되지 않았습니다";
                AlertBodyB = "로그인하지 않고는 사용이 가능하지 않습니다. 먼저 로그인을 해 주세요.";
                AlertViews = "B";
            }
            else if (bnn.apReciever == null)
            {
                AlertBodyA = "접수자를 선택하지 않았습니다.";
                AlertViews = "B";
            }
            else if (bnn.apTitle == null)
            {
                AlertBodyA = "분류를 선택하지 않았습니다.";
                AlertViews = "B";
            }
            else if (bnn.apHoNo == null)
            {
                AlertBodyA = "세대 호를 선택하지 않았습니다.";
                AlertViews = "B";
            }
            else if (bnn.apName == null)
            {
                AlertBodyA = "민원인 명을 입력하지 않았습니다.";
                AlertViews = "B";
            }
            else if (bnn.apMonth == null)
            {
                AlertBodyA = "민원접수일을 입력하지 않았습니다.";
                AlertViews = "B";
            }
            else if (bnn.apContent == null)
            {
                AlertBodyA = "민원 내용을 입력하지 않았습니다.";
                AlertViews = "B";
            }
            else if (bnn.apHp == null)
            {
                AlertBodyA = "민원인 연락처를 입력하지 않았습니다.";
                AlertViews = "B";
            }
            else
            {

                if (bnn.Num < 1)
                {
                    await appeal.add(bnn);

                    if (bnn.Private == "공용")
                    {
                        if (works_s.svBloomC == null)
                        {
                            AlertBodyA = "작업분류를 선택하지 않았습니다.";
                            AlertViews = "B";
                        }
                        else
                        {
                            await works_Lib.Service_Write(works_s);
                        }
                    }
                }
                else
                {
                    await appeal.Edit(bnn);
                }
                bnn = new Appeal_Entity();
                works_s = new Works_Entity();
                strSortA = "";
                strSortB = "";
                strWSortA = "";
                strWSortB = "";
                strWSortC = "";
                strWSortD = "";
                bnn.apPost = "";
                bnn.apReciever = "";
                InsertViews = "A";
                PrivateViews = "A";
                snnB = new List<Appeal_Bloom_Entity>();
                Ho = new List<Apt_People_Entity>();
                await DisplayData(sort_sort);//민원목록 재 로드
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "민원 신청 내용을 등록습니다..");

            }
        }

        /// <summary>
        /// 부서 선택 시 실행
        /// </summary>
        protected async Task OnPost(ChangeEventArgs args)
        {
            string Post = args.Value.ToString();
            bnn.apPost = Post;
            dnn = await referral_Career_Lib.GetList_Post_Staff_be(Post, Apt_Code);
        }

        /// <summary>
        /// 동 선택 실행
        /// </summary>
        protected async Task OnDong(ChangeEventArgs args)
        {
            bnn.apDongNo = args.Value.ToString();
            strDong_No = bnn.apDongNo;
            Ho = await Erp_AptPeople_Lib.Dong_HoList(Apt_Code, args.Value.ToString());
        }

        /// <summary>
        /// 호 선택 실행
        /// </summary>
        protected async Task OnHo(ChangeEventArgs args)
        {
            bnn.apHoNo = args.Value.ToString();
            try
            {
                apt_Pople_List = await Erp_AptPeople_Lib.Dong_Ho_Name_List(Apt_Code, bnn.apDongNo, bnn.apHoNo);
            }
            catch (Exception)
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "해당 되는 정보가 없습니다..");
            }
            strSortSort = "B";
            await DisplayData(args.Value.ToString());
        }

        /// <summary>
        /// 민원선택하기
        /// </summary>
        private async Task OnName(ChangeEventArgs a)
        {
            var pp = await Erp_AptPeople_Lib.Dedeils_Name(a.Value.ToString());
            bnn.apName = pp.InnerName;
            bnn.apHp = pp.Hp;
        }

        /// <summary>
        /// 전용이나 공용 선택 실행
        /// </summary>
        protected void onPrivate(ChangeEventArgs args)
        {
            if (args.Value.ToString() == "전용")
            {
                PrivateViews = "A";
                bnn.Private = args.Value.ToString();
            }
            else
            {
                PrivateViews = "B";
                bnn.Private = args.Value.ToString();
            }
        }

        /// <summary>
        /// 민원대분류 선택 시 실행
        /// </summary>
        protected async Task onSortA(ChangeEventArgs args)
        {
            strSortA = args.Value.ToString();
            strBicSort = strSortA;
            snnB = await appeal_Bloom_Lib.Asort_Name_List(args.Value.ToString());
        }

        /// <summary>
        /// 민원중분류 선택 시 실행
        /// </summary>
        protected async Task onSortB(ChangeEventArgs args)
        {
            abe = await appeal_Bloom_Lib.Details_Code(args.Value.ToString());
            bnn.apTitle = abe.Sort + "_" + abe.Asort;
            strSortB = abe.Asort;
            strSmallSort = abe.Bloom_Code;
            bnn.Bloom_Code = abe.Bloom_Code;
            strSortSort = "A";
            await DisplayData(bnn.apTitle);
        }

        /// <summary>
        /// 작업대분류 선택 시 실행
        /// </summary>
        protected async Task onWSortA(ChangeEventArgs args)
        {
            strWSortA = args.Value.ToString();
            bloom_B = await bloom_Lib.GetList_Apt_bb(Apt_Code, strWSortA);
        }

        /// <summary>
        /// 작업중분류 선택 시 실행
        /// </summary>
        protected async Task onWSortB(ChangeEventArgs args)
        {
            strSortSort = "A";
            strWSortB = args.Value.ToString();
            bloom_C = await bloom_Lib.GetList_Apt_bc(Apt_Code, strWSortB);
        }

        /// <summary>
        /// 작업중분류 선택 시 실행
        /// </summary>
        protected async Task onWSortC(ChangeEventArgs args)
        {
            strWSortC = args.Value.ToString();
            bloom_D = await bloom_Lib.GetList_Apt_c(Apt_Code, strWSortA);
        }

        /// <summary>
        /// 작업세분류 선택 시 실행
        /// </summary>
        protected void onWSortD(ChangeEventArgs args)
        {
            strWSortD = args.Value.ToString();
        }

        private void OnReciever(ChangeEventArgs args)
        {
            bnn.apReciever = args.Value.ToString();
        }

        [Inject] public IDecision_Lib decusion_Lib { get; set; }
        public string sort_sort { get; private set; }
        public DateTime dt { get; private set; }
        public string dtB { get; private set; }

        /// <summary>
        /// 결재 여부 확인
        /// </summary>
        public string DecisionResult(string AptCode, string Parent, string BloomCode, string PostDuty, string User_Code, string Approval)
        {
            string strR = "";

            try
            {
                int be = decusion_Lib.Decisions_Being_Count(AptCode, Parent, BloomCode, PostDuty, User_Code); //로그인한 자가 해당 문서에 결재했다면 1 그렇지 않다면 0
                int Be_PostDust = decusion_Lib.PostDutyBeCount(AptCode, BloomCode, PostDuty); //로그인한 자가 해당 문서에 결재라인에 있다면 1 그렇지 않다면 0

                if (Be_PostDust > 0)
                {
                    if (be > 0)
                    {
                        strR = "결재";
                    }
                    else
                    {
                        strR = "미결재";
                    }
                }
                else
                {
                    if (Approval == "B")
                    {
                        strR = "결재";
                    }
                    else
                    {
                        strR = "미결재";
                    }
                }
            }
            catch (Exception)
            {
                strR = "미결재";
            }

            return strR;
        }

        /// <summary>
        /// 민원처리 입력 열기
        /// </summary>
        public int intAid { get; set; }
        [Inject] public IPost_Lib post_Lib { get; set; }
        private void btnDetailOpen()
        {
            DetailsInsert = "B";
            intAid = vnn.Num;
            sub.subDate = DateTime.Now;
            PostCode = "";
        }

        /// <summary>
        /// 민원처리 수정 열기
        /// </summary>
        /// <param name="ar"></param>
        private void btnSubEdit(subAppeal_Entity ar)
        {
            sub = ar;
            DetailsInsert = "B";
        }

        /// <summary>
        /// 민원 상세 닫기
        /// </summary>
        private void btnDetailClose()
        {
            DetailsViews = "A";
        }

        /// <summary>
        /// 부서 선택 실행
        /// </summary>
        public string PostName { get; set; } = "a";
        public string PostCode { get; set; } = "a";
        public string PostCodeA { get; set; }
        public string Worker { get; set; } = "";
        public async Task onPost(ChangeEventArgs args)
        {
            wnn = new List<Referral_career_Entity>();
            PostCode = args.Value.ToString();

            PostName = await post_Lib.PostName(PostCode);
            sub.subPost = PostName;
            wnn = await referral_Career_Lib.GetList_Post_Staff_be(PostName, Apt_Code);
        }

        /// <summary>
        /// 작업자 만들기
        /// </summary>
        public void onCareer(ChangeEventArgs args)
        {
            if (Worker == "")
            {
                Worker = PostName + "▶" + args.Value.ToString();
            }
            else
            {
                Worker = Worker + ", " + PostName + "▶" + args.Value.ToString();
            }
        }

        /// <summary>
        /// 민원처리 내용 입력
        /// </summary>
        private async Task btnSubSave()
        {
            sub.subWorker = Worker;
            //sub.subPost = PostName;
            sub.apNum = vnn.Num.ToString();
            sub.AppealViw = "A";
            sub.AptCode = Apt_Code;
            sub.AptName = Apt_Name;
            sub.Complete = "A";
            sub.innView = "A";
            sub.outMobile = "A";
            sub.outName = "A";
            sub.outViw = "A";

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
            sub.PostIP = myIPAddress;
            #endregion
            sub.subYear = sub.subDate.Year;
            sub.subMonth = sub.subDate.Month;
            sub.subDay = sub.subDate.Day;
            sub.subDuty = "A";

            if (sub.subPost == "" || sub.subPost == null)
            {
                await JSRuntime.InvokeAsync<object>("alert", "작업부서가 선택되지 않았습니다.");
            }
            else if (sub.subWorker == "" || sub.subWorker == null)
            {
                await JSRuntime.InvokeAsync<object>("alert", "작업자가 입력되지 않았습니다.");
            }
            else if (sub.subContent == "" || sub.subContent == null)
            {
                await JSRuntime.InvokeAsync<object>("alert", "작업내용이 입력되지 않았습니다.");
            }
            else
            {
                if (sub.subAid < 1)
                {
                    await subAppeal.Add(sub);
                }
                else
                {
                    await subAppeal.Edit(sub);
                }
            }

            sub = new subAppeal_Entity();
            snn = await subAppeal.GetList(vnn.Num.ToString());

            PostName = "";
            PostCode = "a";
            Worker = "";

            DetailsInsert = "A";
        }


        /// <summary>
        /// 작업처리 내용 삭제
        /// </summary>
        private async Task btnSubRemove(int Aid)
        {
            bool isDelete = await JSRuntime.InvokeAsync<bool>("confirm", $"{Aid}번 글을 정말로 삭제하시겠습니까?");
            if (isDelete)
            {
                await subAppeal.Remove(Aid.ToString()); // 처리내용 삭제
                snn = await subAppeal.GetList(vnn.Num.ToString());
            }
            else
            {
                AlertViews = "B";
                AlertBodyA = "삭제되지 않았습니다. 관리자에게 문의하세요.";
            }
        }

        /// <summary>
        /// 민원처리 등록 닫기
        /// </summary>
        private void btnSubClose()
        {
            DetailsInsert = "A";
        }

        /// <summary>
        /// 파일 첨부 열기
        /// </summary>
        protected void FiledBy(int Num)
        {
            FileInsertViews = "B";
            //StateHasChanged();
        }
        /// <summary>
        /// 파일 보기 닫기
        /// </summary>
        private void FilesViewsClose()
        {
            FileViews = "A";
            // StateHasChanged();
        }

        #region 결제 여부(민원)
        Decision_Entity decision { get; set; } = new Decision_Entity();
        [Inject] public IApproval_Lib approval_Lib { get; set; }
        [Inject] public IDbImagesLib dbImagesLib { get; set; }
        public string strUserName { get; set; }
        public string decisionA { get; set; }

        /// <summary>
        ///결재여부
        /// </summary>
        /// <param name="objPostDate"></param>
        /// <returns></returns>
        public string FuncShowOK(string strPostDuty, string Apt_Code, int apNum)
        {
            string strBloomCode = "Appeal";
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
        public byte[] sealImage(string aptcode, string post, string duty, object apNum, string BloomCode, string UserCode)
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
                        
                        int CodeBeing = dbImagesLib.Photos_Count(UserCode);

                        if (CodeBeing > 0)
                        {
                            strResult = dbImagesLib.Photos_image(UserCode);
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
            decision.BloomCode = "Appeal";
            decision.Parent = strNum;
            if (referral.Duty == "직원" || referral.Duty == "반원" || referral.Duty == "반장" || referral.Duty == "주임" || referral.Duty == "기사" || referral.Duty == "서무")
            {
                decision.PostDuty = "담당자";
            }
            else
            {
                decision.PostDuty = referral.Post + referral.Duty;
            }
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
                if (vnn.innViw == "B")
                {
                    await decision_Lib.Add(decision);
                    await appeal.Edit_Complete(strNum);

                    app = await approval_Lib.GetList(Apt_Code, "민원일지");
                    await DisplayData(null);
                    vnn = await appeal.Detail(vnn.Num.ToString());
                }
                else
                {
                    await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "민원이 완료되지 않았습니다..");
                }
            }
            else
            {
                await decision_Lib.Add(decision);
                //await appeal.Edit_Complete(Code.ToString());

                app = await approval_Lib.GetList(Apt_Code, "민원일지");
                await DisplayData(null);
            }

        }

        #endregion

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
        public string? fileName { get; set; }
        //public int intViews { get; set; } = 0;
        private async Task LoadFiles(InputFileChangeEventArgs e)
        {
            isLoading = true;
            loadedFiles.Clear();
            progressPercent = 0;
            //intViews = 1;
            foreach (var file in e.GetMultipleFiles(maxAllowedFiles))
            {
                fnn.Parents_Num = vnn.Num.ToString(); // 선택한 ParentId 값 가져오기 
                fnn.Sub_Num = fnn.Parents_Num;
                try
                {
                    var pathA = $"D:\\Msh\\Home\\Ayoung\\sw_Files_View\\Sw_files";

                    string strFiles = Path.GetExtension(file.Name);

                    string _FileName = "Appeal" + Apt_Code + DateTime.Now.ToShortDateString() + strFiles;

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

                    fnn.Sw_FileName = fileName;
                    fnn.Sw_FileSize = Convert.ToInt32(file.Size);
                    fnn.Parents_Name = "Appeal";
                    fnn.AptCode = Apt_Code;
                    fnn.Del = "A";

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
                    fnn.PostIP = myIPAddress;
                    #endregion
                    await files_Lib.Sw_Files_Date_Insert(fnn); //첨부파일 데이터 db 저장
                }
                catch (Exception ex)
                {
                    Logger.LogError("File: {Filename} Error: {Error}",
                        file.Name, ex.Message);
                }
            }

            FileInsertViews = "A";

            Files_Count = await files_Lib.Sw_Files_Data_Index_Count("Appeal", vnn.Num.ToString(), Apt_Code);
            if (Files_Count > 0)
            {
                Files_Entity = await files_Lib.Sw_Files_Data_Index("Appeal", vnn.Num.ToString(), Apt_Code);
            }

            isLoading = false;
        }

        #region 파일첨부 관련
        #region Event Handlers
        public string CompleteViews { get; private set; }
        public string strUserCode { get; private set; }
        //private List<IBrowserFile> loadedFiles = new();



        #endregion

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
                    string rootFolder = $"D:\\Msh\\Home\\Ayoung\\sw_Files_View\\Sw_files\\{_files.Sw_FileName}";
                    File.Delete(rootFolder);
                }
                await files_Lib.FileRemove(_files.Num.ToString(), "Appeal", Apt_Code);
                Files_Count = await files_Lib.Sw_Files_Data_Index_Count("Appeal", vnn.Num.ToString(), Apt_Code);
                if (Files_Count > 0)
                {
                    Files_Entity = await files_Lib.Sw_Files_Data_Index("Appeal", vnn.Num.ToString(), Apt_Code);
                }
            }
            else
            {
                await JSRuntime.InvokeAsync<object>("alert", "취소되었습니다.");
            }
        }

        /// <summary>
        /// 첨부된 사진 보기
        /// </summary>
        protected async Task FileViewsBy(int Num)
        {
            FileViews = "B";
            Files_Count = await files_Lib.Sw_Files_Data_Index_Count("Appeal", vnn.Num.ToString(), Apt_Code);
            if (Files_Count > 0)
            {
                Files_Entity = await files_Lib.Sw_Files_Data_Index("Appeal", vnn.Num.ToString(), Apt_Code);
            }

            //StateHasChanged();
        }

        /// <summary>
        /// 파일 입력 닫기
        /// </summary>
        protected void FilesClose()
        {
            FileInsertViews = "A";
            //StateHasChanged();
        }
        #endregion

        #region 민원처리 완료 관련
        /// <summary>
        /// 만족도 선택 
        /// </summary>
        public string Satisfaction { get; set; }
        private void onSortAAA(ChangeEventArgs args)
        {
            Satisfaction = args.Value.ToString();
            //StateHasChanged();
        }

        /// <summary>
        /// 민원완료 입력
        /// </summary>
        /// <returns></returns>
        public async Task btnAppealCompleteSave()
        {
            if (vnn.innViw == "C" || vnn.innViw == "A")
            {
                await appeal.apSatisfaction(vnn.Num.ToString(), Satisfaction);
                await appeal.Edit_WorkComplete(vnn.Num.ToString());
            }
            else
            {
                await appeal.apSatisfaction(vnn.Num.ToString(), null);
                await appeal.Edit_WorkComplete(vnn.Num.ToString());
            }
            await DisplayData(strSortSort);
            vnn = await appeal.Detail(vnn.Num.ToString());
            CompleteViews = "A";
            //StateHasChanged();
        }

        /// <summary>
        /// 민원완료 닫기
        /// </summary>
        private void btnAppealCompleteClose()
        {
            CompleteViews = "A";
            //StateHasChanged();
        }

        /// <summary>
        /// 민원완료 열기
        /// </summary>
        private void CompleteBy(int Num)
        {
            vnn.Num = Num;
            CompleteViews = "B";
            //StateHasChanged();
        }
        #endregion



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
