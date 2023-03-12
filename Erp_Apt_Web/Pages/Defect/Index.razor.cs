using Company;
using Erp_Apt_Lib;
using Erp_Apt_Lib.Appeal;
using Erp_Apt_Lib.Print_Images;
using Erp_Apt_Staff;
using Erp_Entity;
using Erp_Lib;
using Facilities;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace Erp_Apt_Web.Pages.Defect
{
    public partial class Index
    {
        #region 속성
        [CascadingParameter]
        public Task<AuthenticationState> AuthenticationStateRef { get; set; }

        [Inject] public IApt_Lib apt_Lib { get; set; }  // 공동주택 정보
        [Inject] public IApt_Sub_Lib apt_Sub_Lib { get; set; }  // 공동주택 상세 정보
        [Inject] public IReferral_career_Lib referral_Career { get; set; }

        [Inject] public ICompany_Lib company_Lib { get; set; }
        [Inject] public ICompany_Sub_Lib company_Sub_Lib { get; set; }
        [Inject] public ICompany_Join_Lib company_Join_Lib { get; set; }
        [Inject] public IContract_Sort_Lib contract_Sort_Lib { get; set; }
        [Inject] public IBloom_Lib bloom_Lib { get; set; }
        [Inject] public IPost_Lib post_Lib { get; set; }
        [Inject] public ISido_Lib sido_Lib { get; set; }
        [Inject] public IDefect_Lib defect_lib { get; set; } // 하자 관리

        [Inject] public NavigationManager MyNav { get; set; } // Url 

        [Inject] public ISw_Files_Lib files_Lib { get; set; } // 파일 첨부

        [Inject] public IAppeal_Bloom_Lib appeal_bloom_Lib { get; set; } //민원분류

        [Inject] IErp_AptPeople_Lib aptPeople_Lib { get; set; } // 입주민 정보 클래스

        [Inject] IBloom_Lib Bloom_Lib { get; set; }

        //[Inject] ILogger<FileUpload1> Logger { get; set; }
        [Inject] IWebHostEnvironment Environment { get; set; }

        [Inject] IJSRuntime JSRuntime { get; set; }

        [Inject] IPrint_Images_Lib print_Images_Lib { get; set; }

        Defect_Entity bnn { get; set; } = new Defect_Entity(); //하자정보
        List<Appeal_Bloom_Entity> abe { get; set; } = new List<Appeal_Bloom_Entity>(); // 민원 분류
        List<Bloom_Entity> boo_b { get; set; } = new List<Bloom_Entity>(); //시설물 분류
        List<Bloom_Entity> boo_c { get; set; } = new List<Bloom_Entity>(); //시설물 분류
        List<Bloom_Entity> boo_d { get; set; } = new List<Bloom_Entity>(); //시설물 장소
        List<Post_Entity> pnn { get; set; } = new List<Post_Entity>(); //부서 분류
        Company_Entity cpe { get; set; } = new Company_Entity(); // 업체 정보
        Company_Sub_Entity cpse { get; set; } = new Company_Sub_Entity(); //업체상세 정보
        Apt_Sub_Entity apt_Sub_Entity { get; set; } = new Apt_Sub_Entity(); //공동주택 상세 정보
        List<Company_Join_Entity> cpje { get; set; } = new List<Company_Join_Entity>(); //업체 및 상세 정보 목록

        List<Contract_Sort_Entity> cse_a { get; set; } = new List<Contract_Sort_Entity>(); // 업체 분류 정보
        List<Contract_Sort_Entity> cse_b { get; set; } = new List<Contract_Sort_Entity>(); //업체 분류 정보

        List<Sido_Entity> sidos { get; set; } = new List<Sido_Entity>(); // 시도 정보

        List<Apt_People_Entity> apt_PoplesA { get; set; } = new List<Apt_People_Entity>(); //입주민 정보
        List<Apt_People_Entity> apt_PoplesH { get; set; } = new List<Apt_People_Entity>(); // 입주민 정보
        List<Apt_People_Entity> apt_PoplesN { get; set; } = new List<Apt_People_Entity>(); // 입주민 정보


        Apt_People_Entity apt_PoplesB { get; set; } = new Apt_People_Entity(); // 입주민 정보
        //Apt_People_Entity apt_PoplesB { get; set; } = new Apt_People_Entity(); // 입주민 정보

        Sw_Files_Entity dnn { get; set; } = new Sw_Files_Entity(); // 첨부 파일 정보
        //List<Post_Entity> pnn { get; set; } = new List<Post_Entity>();

        List<Sw_Files_Entity> Files_Entity { get; set; } = new List<Sw_Files_Entity>();
        List<Defect_Entity> ann = new List<Defect_Entity>();

        public int intNum { get; private set; }

        protected DulPager.DulPagerBase pager = new DulPager.DulPagerBase()
        {
            PageNumber = 1,
            PageIndex = 0,
            PageSize = 15,
            PagerButtonCount = 5
        };

        #region 변수
        public string User_Code { get; set; }
        public string User_Name { get; set; }
        public string Apt_Code { get; set; }
        public string Apt_Name { get; set; }
        public string UserName { get; set; }
        public string Views { get; set; } = "A"; //상세 열기
        public string EditOpen { get; set; } = "A";//수정 열기
        public string InsertViews { get; set; } = "A"; // 하자 입력 열기
        public string Modals { get; set; } = "A"; // 업체정보 입력 열기
        public string lblContent { get; set; } = "";
        public int Files_Count { get; set; } = 0;
        public int intAid { get; set; } = 0;
        public string PostName { get; set; } = "a";
        public string PostCode { get; set; } = "a";
        public string FileViews { get; set; } = "A";
        public string strDong { get; set; }
        public string strHo { get; set; }

        public string Sort_Name { get; set; }
        public string Asort_Name { get; set; }
        public int Period { get; set; }
        public string Private { get; set; } = "B";
        public string CompanyCodeA { get; set; } = "A";
        public string CompanyCodeB { get; set; } = "A";
        public string strSido { get; set; } = "";
        public string strGunGu { get; set; } = "";

        public string InformViews { get; set; } = "A";
        public string FileInputViews { get; set; } = "A";
        public string CompleteViews { get; set; } = "A";
        public int inta { get; set; } = 0;
        public int intb { get; set; } = 0;

        //public string strBloomA { get; set; }
        //public string strBloomB { get; set; }
        //public string strBloomC { get; set; }
        //public string strBloomD { get; set; }
        #endregion 
        #endregion


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

                await DisplayData();
            }
            else
            {
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
        /// 데이터 메서드
        /// </summary>
        /// <returns></returns>
        private async Task DisplayData()
        {
            if (!string.IsNullOrWhiteSpace(strSortSA))
            {
                pager.RecordCount = await defect_lib.GetList_Sort_Count(Apt_Code, strSortSA);
                inta = pager.RecordCount;
                ann = await defect_lib.GetList_Sort_Page(pager.PageIndex, Apt_Code, strSortSA); //하자입력 정보 목록
                intNum = pager.RecordCount - (pager.PageIndex * pager.PageSize);
            }
            else if (!string.IsNullOrEmpty(strDetails))
            {
                pager.RecordCount = await defect_lib.GetList_Details_Count(Apt_Code, strDetails);
                inta = pager.RecordCount;
                ann = await defect_lib.GetList_Details_Page(pager.PageIndex, Apt_Code, strDetails); //하자입력 정보 목록
                intNum = pager.RecordCount - (pager.PageIndex * pager.PageSize);
            }
            else
            {
                pager.RecordCount = await defect_lib.GetListCount(Apt_Code);
                inta = pager.RecordCount;
                ann = await defect_lib.GetList_Page(pager.PageIndex, Apt_Code); //하자입력 정보 목록
                intNum = pager.RecordCount - (pager.PageIndex * pager.PageSize);
            }
        }

        /// <summary>
        /// 페이징
        /// </summary>
        /// <param name="pageIndex"></param>
        protected async void PageIndexChanged(int pageIndex)
        {
            pager.PageIndex = pageIndex;
            pager.PageNumber = pageIndex + 1;

            //i = 0;
            //ann = await defect_lib.GetList_Page(pager.PageIndex, Apt_Code);

            await DisplayData();

            StateHasChanged();
        }


        /// <summary>
        /// 대분류 선택 실행
        /// </summary>
        protected async Task onSort(ChangeEventArgs args)
        {
            Sort_Name = args.Value.ToString();
            abe = new List<Appeal_Bloom_Entity>();
            bnn.Bloom_Code_B = await appeal_bloom_Lib.Sort_Code(Sort_Name);
            bnn.Bloom_Name_B = Sort_Name;
            Period = 0;
            abe = await appeal_bloom_Lib.Asort_List(args.Value.ToString());
        }

        /// <summary>
        /// 소분류 선택 실행
        /// </summary>
        protected async Task onAsort(ChangeEventArgs args)
        {
            Period = 0;
            Asort_Name = args.Value.ToString();
            bnn.Bloom_Code_C = await appeal_bloom_Lib.Asort_Code(Asort_Name);
            bnn.Bloom_Name_C = Asort_Name;
            Period = await appeal_bloom_Lib.Period(Asort_Name);
            bnn.Period = Period;
        }

        /// <summary>
        /// 전용 및 공용 선택 시 실행
        /// </summary>
        /// <param name="args"></param>
        protected void onPrivate(ChangeEventArgs args)
        {
            if (args.Value.ToString() == "공용")
            {
                Private = "B";
            }
            else
            {
                Private = "A";
            }

            bnn.Private = args.Value.ToString();
        }

        /// <summary>
        /// 시설물 대분류 선택 시 실행
        /// </summary>
        protected async Task onBloomA(ChangeEventArgs args)
        {
            bnn.Bloom_Name_A = args.Value.ToString();
            bnn.Bloom_Code_A = await bloom_Lib.B_N_A_Code(bnn.Bloom_Name_A);
            boo_b = new List<Bloom_Entity>();
            boo_c = new List<Bloom_Entity>();
            boo_d = new List<Bloom_Entity>();
            boo_b = await Bloom_Lib.GetList_Apt_bb(Apt_Code, args.Value.ToString());
        }

        /// <summary>
        /// 시설물 중분류 선택 시 실행
        /// </summary>
        protected async Task onBloomB(ChangeEventArgs args)
        {
            bnn.Bloom_Name_B = args.Value.ToString();
            bnn.Bloom_Code_B = await bloom_Lib.B_N_B_Code(bnn.Bloom_Name_A, bnn.Bloom_Name_B);
            boo_c = new List<Bloom_Entity>();
            boo_d = new List<Bloom_Entity>();
            boo_c = await Bloom_Lib.GetList_Apt_bc(Apt_Code, args.Value.ToString());
        }

        /// <summary>
        /// 시설물 소분류 선택 시 실행
        /// </summary>
        protected async Task onBloomC(ChangeEventArgs args)
        {
            bnn.Bloom_Name_C = args.Value.ToString();
            bnn.Bloom_Code_C = await bloom_Lib.B_N_C_Code(bnn.Bloom_Name_A, bnn.Bloom_Name_B, bnn.Bloom_Name_C);
            bnn.Period = await bloom_Lib.Period(bnn.Bloom_Code_C);

            boo_d = new List<Bloom_Entity>();
            boo_d = await Bloom_Lib.GetList_dd(Apt_Code, bnn.Bloom_Name_A);
        }

        /// <summary>
        /// 시설물 소분류 선택 시 실행
        /// </summary>
        protected void onBloomD(ChangeEventArgs args)
        {
            //bnn.Position = args.Value.ToString();
            bnn.Position_Code = args.Value.ToString();//await bloom_Lib.B_N_D_Code(bnn.Bloom_Name_A, args.Value.ToString(), Apt_Code);
        }

        /// <summary>
        /// 하자 정보 입력 열기
        /// </summary>
        /// <returns></returns>
        protected async Task onNewbutton()
        {
            bnn = new Defect_Entity();
            var authState = await AuthenticationStateRef;
            if (authState.User.Identity.IsAuthenticated)
            {
                InsertViews = "B";
                Views = "A";
                bnn.Private = "공용";

                pnn = await post_Lib.GetList("A");//부서 정보 목록
                apt_PoplesH = await aptPeople_Lib.DongList(Apt_Code); //입주민 동정보 목록
                bnn.DefectDate = DateTime.Now;
                bnn.dfApplicant = User_Name;
                bnn.dfApplicant_Code = User_Code;
                bnn.User_Code = User_Name;
                bnn.AptCode = Apt_Code;
                bnn.AptName = Apt_Name;
                bnn.dfPost = await referral_Career.PostName(User_Code);
            }
            else
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "로그아웃 되었습니다..");
                MyNav.NavigateTo("/");
            }
        }

        /// <summary>
        /// 입력폼 닫기
        /// </summary>
        protected void onbtnClose()
        {
            InsertViews = "A";
        }

        /// <summary>
        /// 동 정보
        /// </summary>
        /// <returns></returns>
        protected async Task onDong(ChangeEventArgs args)
        {
            strDong = args.Value.ToString();
            bnn.Dong = strDong;
            apt_PoplesA = await aptPeople_Lib.DongHoList_new(Apt_Code, strDong);
        }

        /// <summary>
        /// 호 정보
        /// </summary>
        /// <returns></returns>
        protected async Task onHo(ChangeEventArgs args)
        {
            strHo = args.Value.ToString();
            bnn.Ho = strHo;
            apt_PoplesH = await aptPeople_Lib.Dong_Ho_Name_List(Apt_Code, bnn.Dong, bnn.Ho);
        }

        /// <summary>
        /// 동호 선택 그 세대 민원인 목록 만들기
        /// </summary>
        private async Task onName(ChangeEventArgs a)
        {
            var pp = await aptPeople_Lib.Dedeils_Name(a.Value.ToString());
            bnn.InnerName = pp.InnerName;
            bnn.Mobile = pp.Hp;
            bnn.Email = pp.Email;
            bnn.Relation = pp.Relation;
        }

        /// <summary>
        /// 업체 등록 닫기
        /// </summary>
        protected void btnClose()
        {
            Modals = "A";
            //EditOpen = "A";
            //StateHasChanged();
        }

        /// <summary>
        /// 하자송부 등록 닫기
        /// </summary>
        protected void btnDefectInputClose()
        {
            //Modals = "A";
            InformViews = "A";
            //StateHasChanged();
        }



        /// <summary>
        /// 하자 입력(주요 사항)
        /// </summary>
        /// <returns></returns>
        public async Task onbtnSave()
        {
            bnn.Position = await bloom_Lib.Position_Name(bnn.Position_Code);
            var authState = await AuthenticationStateRef;
            if (authState.User.Identity.IsAuthenticated)
            {
                if (bnn.Aid > 0)
                {
                    if (bnn.Private == "공용") //공용부분 하자
                    {
                        await defect_lib.Edit_Official(bnn);
                    }
                    else if (bnn.Private == "전용") // 전용부분 하자
                    {
                        await defect_lib.Edit_Private(bnn);
                    }

                    InsertViews = "A";
                }
                else
                {
                    //apt_Sub_Entity = await apt_Sub_Lib.Detail(Apt_Code);
                    bnn.Company_Name = apt_Sub_Entity.Builder;

                    
                    bnn.Company_Code = Apt_Code;
                    if (bnn.Private == "공용") //공용부분 하자
                    {
                        if (bnn.dfPost == "" || bnn.dfPost == null)
                        {
                            await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "하자 접수 부서를 선택하지 않았습니다..");
                        }
                        else if (bnn.User_Code == "" || bnn.User_Code == null)
                        {
                            await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "입력자를 입력하지 않았습니다..");
                        }
                        else if (bnn.dfContent == "" || bnn.dfContent == null)
                        {
                            await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "하자 내요을 입력하지 않았습니다..");
                        }
                        else if (bnn.Bloom_Name_A == "Z")
                        {
                            await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "대분류를 선택하지 않았습니다..");
                        }
                        else if (bnn.Bloom_Name_B == "Z")
                        {
                            await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "중분류를 선택하지 않았습니다..");
                        }
                        else if (bnn.Bloom_Name_C == "Z")
                        {
                            await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "세분류를 선택하지 않았습니다..");
                        }
                        else if (bnn.Position == "Z")
                        {
                            await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "하자 발생 장소를 선택하지 않았습니다..");
                        }
                        else
                        {
                            bnn.dfYear = bnn.DefectDate.Year;
                            bnn.dfMonth = bnn.DefectDate.Month;
                            bnn.dfDay = bnn.DefectDate.Day;
                            bnn.User_Code = User_Name;
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
                            bnn.PostIp = myIPAddress;
                            #endregion

                            //bnn.Position_Code = 

                            bnn.dfTitle = bnn.Bloom_Name_A + "_" + bnn.Bloom_Name_B + "_" + bnn.Bloom_Name_C;

                            await defect_lib.Add_Official(bnn);

                            bnn = new Defect_Entity();
                            InsertViews = "A";
                            await DisplayData();
                            //StateHasChanged();
                        }
                    }
                    else if (bnn.Private == "전용") // 전용부분 하자
                    {
                        if (bnn.dfPost == "" || bnn.dfPost == null)
                        {
                            await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "하자 발생 장소를 입력하지 않았습니다..");
                        }
                        else if (bnn.User_Code == "" || bnn.User_Code == null)
                        {
                            await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "입력자를 입력하지 않았습니다..");
                        }
                        else if (bnn.dfContent == "" || bnn.dfContent == null)
                        {
                            await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "하자 내용을 입력하지 않았습니다..");
                        }
                        else if (bnn.Dong == "Z")
                        {
                            await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "동을 선택하지 않았습니다..");
                        }
                        else if (bnn.Ho == "Z")
                        {
                            await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "호를 선택하지 않았습니다..");
                        }
                        else if (bnn.InnerName == "Z")
                        {
                            await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "민원인을 입력하지 않았습니다..");
                        }
                        else if (bnn.Bloom_Name_B == "Z")
                        {
                            await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "중분류를 선택하지 않았습니다..");
                        }
                        else if (bnn.Bloom_Name_C == "Z")
                        {
                            await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "세분류를 선택하지 않았습니다..");
                        }
                        else if (bnn.Position == "Z")
                        {
                            await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "하자 발생 장소를 선택하지 않았습니다..");
                        }
                        else if (bnn.Dong == "" || bnn.Dong == null)
                        {
                            await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "동을 선택하지 않았습니다..");
                        }
                        else if (bnn.Ho == "" || bnn.Ho == null)
                        {
                            await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "호를 선택하지 않았습니다..");
                        }
                        else if (bnn.Relation == "" || bnn.Relation == null)
                        {
                            await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "관계를 입력하지 않았습니다..");
                        }
                        else
                        {
                            bnn.Bloom_Name_A = "전용 하자";
                            bnn.Bloom_Code_A = "1";

                            bnn.Mobile = apt_PoplesB.Hp;
                            bnn.InnerName = apt_PoplesB.InnerName;
                            bnn.Email = apt_PoplesB.Email;
                            bnn.Dong = strDong;
                            bnn.Ho = strHo;
                            bnn.User_Code = User_Name;

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
                            bnn.PostIp = myIPAddress;
                            bnn.dfYear = bnn.DefectDate.Year;
                            bnn.dfMonth = bnn.DefectDate.Month;
                            bnn.dfDay = bnn.DefectDate.Day;
                            #endregion

                            bnn.dfTitle = bnn.Bloom_Name_A + "_" + bnn.Bloom_Name_B + "_" + bnn.Bloom_Name_C;
                            bnn.Position_Code = "1";
                            await defect_lib.Add_Private(bnn);

                            bnn = new Defect_Entity();
                            InsertViews = "A";
                            await DisplayData();
                        }
                    }
                }


            }
            else
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "로그아웃 되었습니다..");
                MyNav.NavigateTo("/");
            }
        }

        /// <summary>
        /// 업체 입력
        /// </summary>
        public async Task btnCompanySave()
        {
            if (cpe.Adress_GunGu == "" || cpe.Adress_GunGu == null)
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "시군구를 선택하지 않았습니다..");
            }
            else if (cpe.Cor_Name == "" || cpe.Cor_Name == null)
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "업체명을 입력하지 않았습니다..");
            }
            else if (cpe.CorporateResistration_Num == "" || cpe.CorporateResistration_Num == null)
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "사업자 등록 번호를 입력하지 않았습니다..");
            }
            else if (cpe.Telephone == "" || cpe.Telephone == null)
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "대표전화번호를 입력하지 않았습니다..");
            }
            else if (cpse.ChargeMan_Mobile == "" || cpse.ChargeMan_Mobile == null)
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "연락처(담당자 연락처)를 입력하지 않았습니다..");
            }
            else
            {
                try
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
                    cpe.PostIP = myIPAddress;
                    cpe.LevelCount = 3;
                    #endregion
                    cpe.Cor_Code = await sido_Lib.Region_Code(cpe.Adress_GunGu) + await company_Lib.Num_Count();
                    cpe.Intro = cpse.Etc;
                    cpe.User_Code = User_Code;
                    await company_Lib.Add(cpe);

                    cpse.Adress = cpe.Adress_Rest;
                    cpse.Ceo_Mobile = cpe.Mobile;
                    cpse.Ceo_Name = cpe.Ceo_Name;
                    cpse.Telephone = cpe.Telephone;
                    cpse.PostIP = cpe.PostIP;

                    cpse.Company_Code = cpe.Cor_Code;
                    cpse.Sido = cpe.Adress_Sido;
                    cpse.GunGu = cpe.Adress_GunGu;

                    await company_Lib.Add(cpe);
                    await company_Sub_Lib.Add(cpse);

                    bnn.subCompany_Code = cpe.Cor_Code;
                    bnn.subCompany_Name = cpe.Cor_Name;

                    Modals = "A";

                    cpse = new Company_Sub_Entity();
                    cpe = new Company_Entity();
                }
                catch (System.Exception ex)
                {
                    await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "에러 발생.." + ex);
                }

            }
        }

        /// <summary>
        /// 하자 전송여부 입력
        /// </summary>
        public async Task btnDefectInputSave()
        {
            //await defect_lib.Edit_Complete("B", "만족", bnn.Aid);
            //await company_Sub_Lib.Add(cpse);

            if (bnn.subCompany_Name == null || bnn.subCompany_Name == "")
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "하자보수 업체명을 입력하지 않았습니다..");
            }
            else if (bnn.Etc == null || bnn.Etc == "")
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "기타 상세 내역을 입력하지 않았습니다..");
            }
            else
            {
                bnn.subCompany_Code = Apt_Code + "_" + DateTime.Now.ToShortDateString();
                await defect_lib.Edit_dfSatisfaction(bnn);

                bnn = await defect_lib.Details(bnn.Aid);
                await DisplayData();

                InformViews = "A";
                CompanyCodeA = "A";
                CompanyCodeB = "A";
                strSido = "";
                strGunGu = "";
            }            
        }

        /// <summary>
        /// 하자종결 입력
        /// </summary>
        public async Task btnDefectCompleteSave()
        {
            if (bnn.dfSatisfaction == "Z")
            {
                await JSRuntime.InvokeAsync<object>("alert", "만족도를 선택하지 않았습니다.");
            }
            else
            {
                if (bnn.Complete == "A")
                {
                    bnn.Complete = "B";
                }
                else
                {
                    bnn.Complete = "A";
                }

                await defect_lib.Edit_Complete(bnn);

                bnn = await defect_lib.Details(bnn.Aid);

                await DisplayData();

                CompleteViews = "A";
            }
        }


        /// <summary>
        /// 상세
        /// </summary>
        /// <param name="defect"></param>
        protected async Task ByAid(Defect_Entity defect)
        {
            Views = "B";
            InsertViews = "A";

            bnn = defect;

            Files_Count = await files_Lib.Sw_Files_Data_Index_Count("Defect", bnn.Aid.ToString(), Apt_Code);

            if (Files_Count > 0)
            {
                Files_Entity = await files_Lib.Sw_Files_Data_Index("Defect", bnn.Aid.ToString(), Apt_Code);
            }
            else
            {
                Files_Entity = new List<Sw_Files_Entity>();
            }
        }

        /// <summary>
        /// 상세
        /// </summary>
        protected void onViewsClose()
        {
            Views = "A";
        }



        /// <summary>
        /// 수정
        /// </summary>
        /// <param name="defect"></param>
        protected async Task ByEdit(Defect_Entity defect)
        {
            InsertViews = "B";
            bnn = defect;

            //bnn.Bloom_Code_A = await bloom_Lib.B_N_A_Code(bnn.Bloom_Name_A);
            boo_b = await Bloom_Lib.GetList_Apt_bb(Apt_Code, bnn.Bloom_Name_A);
            boo_c = await Bloom_Lib.GetList_Apt_bc(Apt_Code, bnn.Bloom_Name_B);
            boo_d = await Bloom_Lib.GetList_dd(Apt_Code, bnn.Bloom_Name_A);
        }

        /// <summary>
        /// 삭제
        /// </summary>
        protected async Task ByRemove(Defect_Entity defect)
        {
            bool isDelete = await JSRuntime.InvokeAsync<bool>("confirm", $"{defect.Aid}번 글을 정말로 삭제하시겠습니까?");
            if (isDelete)
            {
                await defect_lib.Remove(defect.Aid);
                ann = await defect_lib.GetList(Apt_Code);
            }
            else
            {
                await JSRuntime.InvokeAsync<object>("alert", "삭제되지 않았습니다.");
            }
        }

        /// <summary>
        /// 파일 올리기 모달 폼 열기
        /// </summary>
        protected async Task onFileInputbutton()
        {
            FileInputViews = "B";
            Files_Entity = await files_Lib.Sw_Files_Data_Index("Defect", bnn.Aid.ToString(), Apt_Code);
        }

        /// <summary>
        /// 파일 올리기 모달 폼 열기
        /// </summary>
        protected async Task onFileViews()
        {
            FileViews = "B";
            Files_Count = await files_Lib.Sw_Files_Data_Index_Count("Defect", bnn.Aid.ToString(), Apt_Code);
            if (Files_Count > 0)
            {
                Files_Entity = await files_Lib.Sw_Files_Data_Index("Defect", bnn.Aid.ToString(), Apt_Code);
            }
        }

        [Inject] ILogger<Index> Logger { get; set; }
        private List<IBrowserFile> loadedFiles = new();
        private long maxFileSize = 1024 * 1024 * 300;
        private int maxAllowedFiles = 5;
        private bool isLoading;
        private decimal progressPercent;
        //Sw_Files_Entity finn { get; set; } = new Sw_Files_Entity();
        [Inject] Microsoft.AspNetCore.Hosting.IWebHostEnvironment env { get; set; }
        public string? fileName { get; set; }

        private async Task LoadFiles(InputFileChangeEventArgs e)
        {
            isLoading = true;
            loadedFiles.Clear();
            progressPercent = 0;

            foreach (var file in e.GetMultipleFiles(maxAllowedFiles))
            {
                dnn.Parents_Num = bnn.Aid.ToString(); // 선택한 ParentId 값 가져오기 
                dnn.Sub_Num = dnn.Parents_Num;
                try
                {
                    var pathA = $"{env.WebRootPath}\\UpFiles\\Defect";

                    string strFiles = Path.GetExtension(file.Name);

                    string _FileName = "Defect" + Apt_Code + DateTime.Now.ToShortDateString() + strFiles;

                    fileName = Dul.FileUtility.GetFileNameWithNumbering(pathA, _FileName);
                    
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

                    dnn.Sw_FileName = fileName;
                    dnn.Sw_FileSize = Convert.ToInt32(file.Size);
                    dnn.Parents_Name = "Defect";
                    dnn.AptCode = Apt_Code;
                    dnn.Del = "A";

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
                    await files_Lib.Sw_Files_Date_Insert(dnn); //첨부파일 데이터 db 저장
                    await defect_lib.Edit_ImagesCount(bnn.Aid); // 첨부파일 추가를 db 저장(하자, defect)   
                }
                catch (Exception ex)
                {
                    Logger.LogError("File: {Filename} Error: {Error}",
                        file.Name, ex.Message);
                }
            }
            await Review();
            await DisplayData();
            FileInputViews = "A";
            isLoading = false;
        }


        /// <summary>
        /// 하자 송부 여부 모달폼 열기
        /// </summary>
        protected async Task onInformbutton()
        {
            InformViews = "B";
            cse_a = await contract_Sort_Lib.List("Aa"); //업체 대분류 목록
        }

        /// <summary>
        /// 확정자 찾기오기
        /// </summary>
        /// <param name="filesName"></param>
        /// <returns></returns>
        private string OnExtension(string filesName)
        {
            return Path.GetExtension(filesName);
        }

        private async Task Review()
        {
            FileInputViews = "A";

            bnn = await defect_lib.Details(bnn.Aid);
            await DisplayData();

            Files_Count = await files_Lib.Sw_Files_Data_Index_Count("Defect", bnn.Aid.ToString(), Apt_Code);
            if (Files_Count > 0)
            {
                Files_Entity = await files_Lib.Sw_Files_Data_Index("Defect", bnn.Aid.ToString(), Apt_Code);
            }
        }

        /// <summary>
        /// PDF 파일 확인
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public bool IsPDFHeader(string fileName)
        {
            byte[] buffer = null;
            FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
            BinaryReader br = new BinaryReader(fs);
            long numBytes = new FileInfo(fileName).Length;
            //buffer = br.ReadBytes((int)numBytes);
            buffer = br.ReadBytes(5);
            var enc = new ASCIIEncoding();
            var header = enc.GetString(buffer);
            //%PDF−1.0
            // If you are loading it into a long, this is (0x04034b50).
            if (buffer[0] == 0x25 && buffer[1] == 0x50
                && buffer[2] == 0x44 && buffer[3] == 0x46)
            {
                return header.StartsWith("%PDF-");
            }
            return false;
        }

        

        /// <summary>
        /// 첨부파일 모달 폼 닫기
        /// </summary>
        protected void FilesClose()
        {
            FileInputViews = "A";
            //StateHasChanged();
        }

        /// <summary>
        /// 첨부보기 모달 폼 닫기
        /// </summary>
        protected void FilesViewsClose()
        {
            FileViews = "A";
            //StateHasChanged();
        }

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
                    string rootFolder = $"{env.WebRootPath}\\UpFiles\\Defect\\{_files.Sw_FileName}";
                    File.Delete(rootFolder);
                }
                await files_Lib.FileRemove(_files.Num.ToString(), "Defect", Apt_Code);
                //await UploadRepositoryAsyncReference.DeleteAsync(Id); // 삭제
                //NavigationManagerReference.NavigateTo("/Uploads"); // 리스트 페이지로 이동
                Files_Count = await files_Lib.Sw_Files_Data_Index_Count("Defect", bnn.Aid.ToString(), Apt_Code);
                if (Files_Count > 0)
                {
                    Files_Entity = await files_Lib.Sw_Files_Data_Index("Defect", bnn.Aid.ToString(), Apt_Code);
                }
            }
            else
            {
                await JSRuntime.InvokeAsync<object>("alert", "취소되었습니다.");
            }
        }

        /// <summary>
        /// 업체 등록 모달품 열기
        /// </summary>
        protected async Task btnCompanyInputSave()
        {
            InformViews = "A";
            Modals = "B";
            cse_a = await contract_Sort_Lib.List("Aa"); //업체 대분류 목록
            //StateHasChanged();
        }

        /// <summary>
        /// 대분류 선택 시 실행(소분류 만들기)
        /// </summary>
        protected async Task onSortAA(ChangeEventArgs args)
        {
            cpse.TypeOfBusiness = await contract_Sort_Lib.Name(args.Value.ToString());
            cse_b = await contract_Sort_Lib.List(args.Value.ToString());
            //StateHasChanged();
        }

        /// <summary>
        /// 소분류 선택 시 실행
        /// </summary>
        protected async Task onSortBB(ChangeEventArgs args)
        {
            cpse.BusinessConditions = await contract_Sort_Lib.Name(args.Value.ToString());
            cpse.Company_Sort = args.Value.ToString();
            //lstC = await contract_Sort_Lib.List(args.Value.ToString());
            //StateHasChanged();
        }

        /// <summary>
        /// 사업자 번호 중복 체크
        /// </summary>
        protected async Task OnRepeatCheck()
        {
            cpe.CorporateResistration_Num = cpe.CorporateResistration_Num.Replace("-", "").Replace(" ", "");
            int intR = await company_Lib.CorNum_Being(cpe.CorporateResistration_Num.ToString());

            //string strResult = "";
            //List<SelectListItem> licities = new List<SelectListItem>();

            bool tr = checkCpIdenty(cpe.CorporateResistration_Num);

            //licities.Add(new SelectListItem { Text = "::분류선택::", Value = "0" });
            if (tr == true)
            {
                if (intR > 0)
                {
                    cpe.CorporateResistration_Num = "";
                    await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", cpe.CorporateResistration_Num + "는 이미 입력된 사업자 등록 번호 입니다. 다시 입력해 주세요...");
                }

            }
            else
            {
                cpe.CorporateResistration_Num = "";
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", cpe.CorporateResistration_Num + "는 잘못된 사업자 등록 번호 입니다. 다시 입력해 주세요...");
            }
        }


        /// <summary>
        /// 사업자번호 체크
        /// </summary>
        public bool checkCpIdenty(string cpNum)
        {
            cpNum = cpNum.Replace("-", "");
            if (cpNum.Length != 10)
            {
                return false;
            }
            int sum = 0;
            string checkNo = "137137135";

            // 1
            for (int i = 0; i < checkNo.Length; i++)
            {
                sum += (int)Char.GetNumericValue(cpNum[i]) * (int)Char.GetNumericValue(checkNo[i]);
            }

            // 2
            sum += (int)Char.GetNumericValue(cpNum[8]) * 5 / 10;

            // 3
            sum %= 10;

            // 4
            if (sum != 0)
            {
                sum = 10 - sum;
            }

            // 5
            if (sum != (int)Char.GetNumericValue(cpNum[9]))
            {
                return false;
            }
            else
            {
                return true;
            }
        }


        /// <summary>
        /// 업체 대분류
        /// </summary>
        protected async Task onSortA(ChangeEventArgs args)
        {
            cse_b = await contract_Sort_Lib.List(args.Value.ToString()); //업체 소분류
            CompanyCodeA = await contract_Sort_Lib.Name(args.Value.ToString());
            //StateHasChanged();
        }

        /// <summary>
        /// 업체 소분류
        /// </summary>
        protected async Task onSortB(ChangeEventArgs args)
        {
            CompanyCodeB = args.Value.ToString();
            cpje = await company_Join_Lib.List_Join_A(CompanyCodeB);
            //StateHasChanged();
        }

        /// <summary>
        /// 시도 선택 시군구 실행
        /// </summary>
        public async Task OnSido(ChangeEventArgs args)
        {
            cpe.Adress_Sido = await sido_Lib.SidoName(args.Value.ToString());
            cpse.Sido = cpe.Adress_Sido;
            strSido = cpe.Adress_Sido;
            sidos = await sido_Lib.GetList_Code(args.Value.ToString());

            if (CompanyCodeB != "")
            {
                cpje = await company_Join_Lib.List_Join_E(CompanyCodeB, strSido);

            }
            else
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "업체분류를 선택하지 않았습니다..");
            }

            //StateHasChanged();
        }

        /// <summary>
        /// 시군구 선택 실행
        /// </summary>
        public async Task onGunGu(ChangeEventArgs args)
        {
            cpe.Adress_GunGu = args.Value.ToString();
            cpse.GunGu = cpe.Adress_GunGu;
            strGunGu = cpe.Adress_GunGu;

            if (strSido != "" && CompanyCodeB != "")
            {
                cpje = await company_Join_Lib.List_Join_F(CompanyCodeB, strSido, strGunGu);

            }
            else
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "업체분류를 선택하지 않았습니다..");
            }

            // StateHasChanged();
        }

        /// <summary>
        /// 하자 업체 선택 시 실행
        /// </summary>
        /// <returns></returns>
        public async Task onDefectCompany(ChangeEventArgs args)
        {
            bnn.subCompany_Code = args.Value.ToString();
            bnn.subCompany_Name = await company_Lib.Company_Name_Code(bnn.subCompany_Code);
        }

        /// <summary>
        /// 하자 종결 모달폼 열기
        /// </summary>
        /// <returns></returns>
        public void onDefectComplete()
        {
            CompleteViews = "B";
        }

        /// <summary>
        /// 하자 종결 모달품 닫기
        /// </summary>
        protected void btnDefectCompleteClose()
        {
            CompleteViews = "A";
        }

        /// <summary>
        /// 만족도
        /// </summary>
        /// <param name="args"></param>
        protected void onSortAAA(ChangeEventArgs args)
        {
            bnn.dfSatisfaction = args.Value.ToString();
        }

        /// <summary>
        /// 인쇄로 이동
        /// </summary>
        /// <param name="zx"></param>
        //Print_Images pri { get; set; } = new Print_Images();
        //private int maxAllowedFiles = 3;
       // private List<IBrowserFile> loadedFiles = new();
        public void btnPrint(Defect_Entity zx)
        {
            MyNav.NavigateTo("http://new.wedew.co.kr/Pdf/website?Aid=" + zx.Aid + "&Name=Defect&AptCode=" + Apt_Code, true);
        }

        /// <summary>
        /// 검색 모달 열기
        /// </summary>
        public string strSearchOpen { get; set; } = "A";
        public string strTitle { get; set; }
        private void onSearch()
        {
            strSearchOpen = "B";
            strTitle = "하자 정보 검색 목록 만들기";
        }

        /// <summary>
        /// 검색 모달 닫기
        /// </summary>
        private void btnCloseS()
        {
            strSearchOpen = "A";
        }

        public string strSortSA { get; set; }
        public string strDetails { get; set; }
        private async Task onSortSA(ChangeEventArgs a)
        {
            strSortSA = a.Value.ToString();
            strDetails = null;
            await DisplayData();
        }

        private async Task OnDetailSearch(ChangeEventArgs a)
        {
            strDetails = a.Value.ToString();
            strSortSA = null;
            await DisplayData();
        }
    }
}
