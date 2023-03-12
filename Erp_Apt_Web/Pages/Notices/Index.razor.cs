using Erp_Apt_Lib.Decision;
using Erp_Apt_Lib;
using Erp_Apt_Staff;
using Erp_Entity;
using Erp_Lib;
using Facilities;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

namespace Erp_Apt_Web.Pages.Notices
{
    public partial class Index
    {
        #region 인스턴스
        [CascadingParameter] public Task<AuthenticationState> AuthenticationStateRef { get; set; }
        [Inject] public NavigationManager MyNav { get; set; } // Url 
        [Inject] IJSRuntime JSRuntime { get; set; }
        [Inject] public INotice_Lib notice_Lib { get; set; }
        [Inject] public IBloom_Lib bloom_Lib { get; set; }
        [Inject] public IPost_Lib post_Lib { get; set; }
        [Inject] public IReferral_career_Lib referral_Career_Lib { get; set; }
        #endregion

        #region 속성
        List<Notice_Entity> ann { get; set; } = new List<Notice_Entity>();
        List<Notice_Entity> ynn { get; set; } = new List<Notice_Entity>();
        Notice_Entity bnn { get; set; } = new Notice_Entity();
        Sw_Files_Entity dnn { get; set; } = new Sw_Files_Entity(); // 첨부 파일 정보
        List<Sw_Files_Entity> Files_Entity { get; set; } = new List<Sw_Files_Entity>();
        List<Bloom_Entity> fnn { get; set; } = new List<Bloom_Entity>();
        List<Post_Entity> pnn { get; set; } = new List<Post_Entity>();
        public string PostDuty { get; private set; }
        List<Referral_career_Entity> rnn { get; set; } = new List<Referral_career_Entity>();
        [Inject] public ISw_Files_Lib files_Lib { get; set; }
        #endregion

        #region 변수
        public string User_Code { get; set; }
        public string User_Name { get; set; }
        public string Apt_Code { get; set; }
        public string Apt_Name { get; set; }
        public int LevelCount { get; set; } = 0;
        public string Views { get; set; } = "A"; //상세 열기
        public string RemoveViews { get; set; } = "A"; //삭제 열기
        public string InsertViews { get; set; } = "A"; //입력 열기
        public string Titles { get; set; }
        public int intNum { get; set; }
        public string strFeild_a { get; set; }
        public string strQuery_a { get; set; }
        public string strFeild_b { get; set; }
        public string strQuery_b { get; set; }
        public string strDivision { get; set; }

        public string FileInputViews { get; set; } = "A";
        public string FileViews { get; set; } = "A";
        public int Files_Count { get; set; } = 0;
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

            await DisplayData(strFeild_a, strQuery_a, strFeild_b, strQuery_b, strDivision);

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

                if (LevelCount < 3)
                {
                    await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "권한이 없습니다.");
                    MyNav.NavigateTo("/");
                }
                else
                {
                    await DisplayData("", "", "", "", "A");
                    app = await approval_Lib.GetList(Apt_Code, "방송·공고");
                    ynn = await notice_Lib.SearchYear(Apt_Code); //문서 분류 불러오기
                    fnn = await bloom_Lib.GetList_Apt_ba(Apt_Code);
                    pnn = await post_Lib.GetList("A");
                    referral = await referral_Career_Lib.Details(User_Code);
                    if (referral.Post == "회계(경리)")
                    {
                        referral.Post = "회계";
                    }
                    PostDuty = referral.Post + referral.Duty;
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
        /// 데이터 정보 
        /// </summary>
        private async Task DisplayData(string Feild_a, string Query_a, string Feild_b, string Query_b, string Division)
        {
            strDivision = Division;

            if (Division == "A")
            {
                pager.RecordCount = await notice_Lib.GetListCount(Apt_Code);
                ann = await notice_Lib.GetList(pager.PageIndex, Apt_Code);
            }
            else if (Division == "B")
            {
                pager.RecordCount = await notice_Lib.SearchListCount_Two(Apt_Code, Feild_a, Query_a, Feild_b, Query_b);
                ann = await notice_Lib.SearchList_Two(pager.PageIndex, Apt_Code, Feild_a, Query_a, Feild_b, Query_b);
            }
            else
            {
                pager.RecordCount = await notice_Lib.SearchListCount(Apt_Code, Feild_b, Query_b);
                ann = await notice_Lib.SearchList(pager.PageIndex, Apt_Code, Feild_b, Query_b);
            }

            intNum = pager.RecordCount - (pager.PageIndex * pager.PageSize);
            //StateHasChanged();
        }

        /// <summary>
        /// 상세보기
        /// </summary>
        /// <param name="notice"></param>
        public async Task ByAid(Notice_Entity notice)
        {
            bnn = notice;
            Views = "B";
            Files_Count = await files_Lib.BeCount(bnn.Aid.ToString(), "Notice");
            Files_Entity = await files_Lib.Sw_Files_Data_Index("Notice", bnn.Aid.ToString(), Apt_Code);
        }

        /// <summary>
        /// 선택
        /// </summary>
        private void OnSearch_b(ChangeEventArgs a)
        {
            strFeild_b = a.Value.ToString();
            //strQuery_b = a.Value.ToString();
        }

        /// <summary>
        /// 찾기
        /// </summary>
        public void OnSearch_a(ChangeEventArgs a)
        {
            strFeild_a = "AcceptYear";
            strQuery_a = a.Value.ToString();
        }

        /// <summary>
        /// 찾기
        /// </summary>
        public async Task OnSearch()
        {
            if (strQuery_b == "" || strQuery_b == null)
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "검색할 내용을 입력하지 않았습니다.");
            }
            else
            {
                if (strFeild_a == "" || strFeild_a == null || strFeild_a == "Z")
                {
                    strFeild_a = "AcceptYear";
                }
                if (strQuery_a == "" || strQuery_a == null || strQuery_a == "Z")
                {
                    strQuery_a = DateTime.Now.Year.ToString();
                }

                if (strFeild_b == "" || strFeild_b == null || strFeild_b == "Z")
                {
                    strFeild_b = "NoticeTitle";
                }
                await DisplayData(strFeild_a, strQuery_a, strFeild_b, strQuery_b, "B");
            }
        }

        /// <summary>
        /// 입력 닫기
        /// </summary>
        private void btnClose()
        {
            InsertViews = "A";
        }

        /// <summary>
        /// 입력 열기
        /// </summary>
        private void btnInsert()
        {
            bnn = new Notice_Entity();
            bnn.AcceptDate = DateTime.Now.Date;
            InsertViews = "B";
        }

        #region 결제 여부
        [Inject] public IDecision_Lib decision_Lib { get; set; } // 결재정보
        Decision_Entity decision { get; set; } = new Decision_Entity();
        [Inject] public IApproval_Lib approval_Lib { get; set; }
        [Inject] public IDbImagesLib dbImagesLib { get; set; }
        Referral_career_Entity referral { get; set; } = new Referral_career_Entity();
        List<Approval_Entity> app { get; set; } = new List<Approval_Entity>();
        public string strUserName { get; set; }
        public string decisionA { get; set; }
        public string strNum { get; set; }

        /// <summary>
        ///결재여부
        /// </summary>
        /// <param name="objPostDate"></param>
        /// <returns></returns>
        public string FuncShowOK(string strPostDuty, string Apt_Code, int Aid)
        {
            string strBloomCode = "Notice";
            string strUserName = "";
            string nu = Aid.ToString();
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
                        //string User_Code = referral_Career_Lib.User_Code_Bes(aptcode, post, duty);
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
            decision.BloomCode = "Notice";
            decision.Parent = strNum;

            if (referral.Post == "회계(경리)")
            {
                referral.Post = "회계";
            }
            if (referral.Post != "회계")
            {
                if (referral.Duty == "직원" || referral.Duty == "반원" || referral.Duty == "반장" || referral.Duty == "주임" || referral.Duty == "기사")
                {
                    decision.PostDuty = "담당자";
                }
                else
                {
                    decision.PostDuty = referral.Post + referral.Duty;
                }
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

                int a = await decision_Lib.Add(decision);
                int intNum = Convert.ToInt32(strNum);
                await notice_Lib.NoticeApproval(intNum);
                //await decision_Lib.Decision_Comform(strNum, "Notice", "Approval", "Aid");

                app = await approval_Lib.GetList(Apt_Code, "방송·공고");
                await DisplayData("", "", "", "", "A");

            }
            else
            {
                await decision_Lib.Add(decision);

                app = await approval_Lib.GetList(Apt_Code, "방송·공고");
            }
            await DisplayData("", "", "", "", "A");
            //StateHasChanged();
        }

        #endregion


        /// <summary>
        /// 방송 및 공고 입력
        /// </summary>
        /// <returns></returns>
        public async Task btnSave()
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
            bnn.AcceptYear = bnn.AcceptDate.Year;
            if (bnn.Period == "" || bnn.Period == null)
            {
                bnn.Period = "7일간";
            }

            if (bnn.AcceptYear == 0)
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "시행일을 입력하지 않았습니다.");
            }
            else if (bnn.Division == "" || bnn.Division == null)
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "구분을 선택하지 않았습니다.");
            }
            else if (bnn.NoticeContent == "" || bnn.NoticeContent == null)
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "상세 내용을 입력하지 않았습니다.");
            }
            else if (bnn.NoticeSort == "" || bnn.NoticeSort == null)
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "분류를 선택하지 않았습니다.");
            }
            else if (bnn.NoticeTarget == "" || bnn.NoticeTarget == null)
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "대상을 입력하지 않았습니다.");
            }
            else if (bnn.NoticeTitle == "" || bnn.NoticeTitle == null)
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "제목을 입력하지 않았습니다.");
            }
            else if (bnn.Period == "" || bnn.Period == null)
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "시행기간을 입력하지 않았습니다.");
            }
            else if (bnn.Post == "" || bnn.Post == null)
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "부서를 선택하지 않았습니다.");
            }
            else if (bnn.WorkMan == "" || bnn.WorkMan == null)
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "시행자를 선택하지 않았습니다.");
            }
            else
            {
                bnn.AptCode = Apt_Code;

                int a = await notice_Lib.SearchListCount(Apt_Code, "Division", bnn.Division);

                bnn.NoticeNum = bnn.Division + "-" + bnn.AcceptYear + "-" + a;
                bnn.AptName = Apt_Name;
                bnn.OpenPublic = "A";
                bnn.UserCode = User_Code;

                if (bnn.Aid < 1)
                {
                    int v = await notice_Lib.Add(bnn);
                }
                else
                {
                    if (bnn.Approval == "A")
                    {
                        await notice_Lib.Edit(bnn);
                    }
                    else
                    {
                        await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "결재가 완료되어 삭제되지 않았습니다.");
                    }
                }

                await DisplayData("", "", "", "", "A");
                InsertViews = "A";
            }
        }

        /// <summary>
        /// 구분 선택 시 실행
        /// </summary>
        /// <param name="a"></param>
        private void onDivision(ChangeEventArgs a)
        {
            bnn.Division = a.Value.ToString();
        }

        /// <summary>
        /// 실행기간 선택 시 실행
        /// </summary>
        /// <param name="a"></param>
        private void onPeriod(ChangeEventArgs a)
        {
            bnn.Period = a.Value.ToString();
        }

        /// <summary>
        /// 시행자 선택 시 실행
        /// </summary>
        /// <param name="a"></param>
        private void onWorkMan(ChangeEventArgs a)
        {
            bnn.WorkMan = a.Value.ToString();
        }

        /// <summary>
        /// 분류 선택 시 실행
        /// </summary>
        /// <param name="a"></param>
        private void onNoticeSort(ChangeEventArgs a)
        {
            bnn.NoticeSort = a.Value.ToString();
        }



        /// <summary>
        /// 구분 이름 메서드
        /// </summary>
        public string Division_Name(string Code)
        {
            string result = "";

            if (Code == "Pn")
            {
                result = "관리실 공고";
            }
            else if (Code == "Pe")
            {
                result = "선관위 공고";
            }
            else if (Code == "Gn")
            {
                result = "시청 공고";
            }
            else if (Code == "Mn")
            {
                result = "광고 공고";
            }
            else if (Code == "Pr")
            {
                result = "입대의 공고";
            }
            else if (Code == "Nn")
            {
                result = "안내";
            }
            else if (Code == "Bn")
            {
                result = "방송";
            }
            else
            {
                result = "없음";
            }

            return result;
        }

        /// <summary>
        /// 부서 선택 시 직원목록 만들기
        /// </summary>
        /// <returns></returns>
        private async Task onPost(ChangeEventArgs a)
        {
            bnn.Post = a.Value.ToString();
            rnn = await referral_Career_Lib.GetList_Post_Staff_be(a.Value.ToString(), Apt_Code);
        }


        /// <summary>
        /// 상세보기 
        /// </summary>
        private void ViewsClose()
        {
            Views = "A";
        }

        /// <summary>
        /// 첨부파일 삭제
        /// </summary>
        /// <param name="sw_Files"></param>
        private async Task FilesRemove(Sw_Files_Entity _files)
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
                await files_Lib.FileRemove(_files.Num.ToString(), "Notice", Apt_Code);

                await notice_Lib.FilesCount(bnn.Aid, "B"); //파일 수 줄이기
                //await UploadRepositoryAsyncReference.DeleteAsync(Id); // 삭제
                //NavigationManagerReference.NavigateTo("/Uploads"); // 리스트 페이지로 이동
                Files_Count = await files_Lib.Sw_Files_Data_Index_Count("Notice", bnn.Aid.ToString(), Apt_Code);
                if (Files_Count > 0)
                {
                    Files_Entity = await files_Lib.Sw_Files_Data_Index("Notice", bnn.Aid.ToString(), Apt_Code);
                }
            }
            else
            {
                await JSRuntime.InvokeAsync<object>("alert", "취소되었습니다.");
            }
        }


        #region Event Handlers
        
        public string strUserCode { get; private set; }

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
                dnn.Parents_Num = bnn.Aid.ToString(); // 선택한 ParentId 값 가져오기 
                dnn.Sub_Num = dnn.Parents_Num;
                try
                {
                    var pathA = $"D:\\Msh\\Home\\Ayoung\\sw_Files_View\\Sw_files";


                    string strFiles = Path.GetExtension(file.Name);

                    string _FileName = "Notice" + Apt_Code + DateTime.Now.ToShortDateString() + strFiles;

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

                    dnn.Sw_FileName = fileName;
                    dnn.Sw_FileSize = Convert.ToInt32(file.Size);
                    dnn.Parents_Name = "Notice";
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

                    await notice_Lib.FilesCount(bnn.Aid, "A"); // 첨부파일 추가를 db 저장(문서관리, Document)
                }
                catch (Exception ex)
                {
                    Logger.LogError("File: {Filename} Error: {Error}",
                        file.Name, ex.Message);
                }
            }

            await DisplayData("", "", "", "", "A");           

            FileInputViews = "A";

            Files_Count = await files_Lib.Sw_Files_Data_Index_Count("Notice", bnn.Aid.ToString(), Apt_Code);
            if (Files_Count > 0)
            {
                Files_Entity = await files_Lib.Sw_Files_Data_Index("Notice", bnn.Aid.ToString(), Apt_Code);
            }

            isLoading = false;
        }

       

        
        /// <summary>
        /// 확정자 찾기오기
        /// </summary>
        private string OnExtension(string filesName)
        {
            return Path.GetExtension(filesName);
        }
        #endregion

        /// <summary>
        /// 파일 업로드 열기
        /// </summary>
        private void ViewsFileInsert()
        {
            FileInputViews = "B";
        }

        /// <summary>
        /// 파일 업로드 닫기
        /// </summary>
        private void FilesClose()
        {
            FileInputViews = "A";
        }

        /// <summary>
        /// 수정열기
        /// </summary>
        private async Task btnEdit()
        {
            rnn = await referral_Career_Lib.GetList_Post_Staff_be(bnn.Post, Apt_Code);
            Views = "A";
            InsertViews = "B";
        }

        /// <summary>
        /// 문서 삭제
        /// </summary>
        /// <param name="Aid"></param>
        /// <returns></returns>
        public async Task btnRemove(int Aid)
        {
            bool isDelete = await JSRuntime.InvokeAsync<bool>("confirm", $"{Aid} 문서를 정말로 삭제하시겠습니까?");

            if (isDelete)
            {
                await notice_Lib.Remove(Aid);
                await DisplayData("", "", "", "", "A");
                Views = "A";
            }
        }

        private void OnPrint()
        {
            MyNav.NavigateTo("http://pv.swtmc.co.kr/Prints/Asp/NoticeDetails?Aid=" + bnn.Aid + "&AptCode=" + Apt_Code);
        }
    }
}
