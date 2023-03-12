using Erp_Apt_Lib.Decision;
using Erp_Apt_Lib.Documents;
using Erp_Apt_Lib;
using Erp_Apt_Staff;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Erp_Apt_Web.Pages.Documents
{
    public partial class Index
    {
        #region 인스턴스
        [CascadingParameter] public Task<AuthenticationState> AuthenticationStateRef { get; set; }
        [Inject] public NavigationManager MyNav { get; set; } // Url 
        [Inject] IJSRuntime JSRuntime { get; set; }
        [Inject] public IStaff_Lib staff_Lib { get; set; }
        [Inject] public IDocument_Lib documents_Lib { get; set; }
        [Inject] public IDocument_Sort_Lib document_Sort_Lib { get; set; }
        [Inject] public ISw_Files_Lib files_Lib { get; set; }
        [Inject] public IDecision_Lib decision_Lib { get; set; } // 결재정보
        [Inject] public IReferral_career_Lib referral_Career_Lib { get; set; }

        #endregion

        #region 속성
        List<Document_Sort_Entity> fnn { get; set; } = new List<Document_Sort_Entity>();
        List<Document_Entity> ann { get; set; } = new List<Document_Entity>();
        Document_Entity bnn { get; set; } = new Document_Entity();
        Sw_Files_Entity dnn { get; set; } = new Sw_Files_Entity(); // 첨부 파일 정보
        List<Sw_Files_Entity> Files_Entity { get; set; } = new List<Sw_Files_Entity>();
        Referral_career_Entity cnnA { get; set; } = new Referral_career_Entity();
        #endregion

        #region 변수
        public string User_Code { get; set; }
        public string User_Name { get; set; }
        public string Apt_Code { get; set; }
        public string Apt_Name { get; set; }
        public int LevelCount { get; set; } = 0;
        public string strRegion { get; set; }
        public string strSido { get; set; }
        public string Views { get; set; } = "A"; //상세 열기
        public string RemoveViews { get; set; } = "A"; //삭제 열기
        public string InsertViews { get; set; } = "A"; //입력 열기
        public string Titles { get; set; }
        public string CorporateResistration_Num { get; set; } //사업자 번호
        public string strSearchs { get; set; }
        public string strQuery { get; set; }
        public int intNum { get; set; }

        public string FileInputViews { get; set; } = "A";
        public string FileViews { get; set; } = "A";
        public int Files_Count { get; set; } = 0;
        public string PostDuty { get; set; }
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

            await DisplayData(strSearchs, strQuery);

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
                    await DisplayData("", "");

                    cnnA = await referral_Career_Lib.Detail(User_Code);

                    if (cnnA.Post == "회계(경리)")
                    {
                        cnnA.Post = "회계";
                    }
                    PostDuty = cnnA.Post + cnnA.Duty;


                    app = await approval_Lib.GetList(Apt_Code, "문서관리");

                    referral = await referral_Career_Lib.Details(User_Code);
                    fnn = await document_Sort_Lib.GetList(Apt_Code); //문서 분류 불러오기
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
        private async Task DisplayData(string Feild, string Query)
        {
            if (Feild == "" || Feild == null || Query == "" || Query == null)
            {
                pager.RecordCount = await documents_Lib.GetList_Count(Apt_Code);
                ann = await documents_Lib.GetList_Page(pager.PageIndex, Apt_Code);
            }
            else
            {
                pager.RecordCount = await documents_Lib.SearchList_Count(Feild, Query, Apt_Code);
                ann = await documents_Lib.SearchList_Page(pager.PageIndex, Feild, Query, Apt_Code);
            }

            intNum = pager.RecordCount - (pager.PageIndex * pager.PageSize);
            //StateHasChanged();
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
            string strBloomCode = "Document";
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
                        //string User_Code = referral_Career_Lib.User_Code_Bes(aptcode, post, duty);
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
            decision.BloomCode = "Document";
            decision.Parent = bnn.Aid.ToString();
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

                await decision_Lib.Add(decision);
                await documents_Lib.Document_Comform(bnn.Aid, "B");
                app = await approval_Lib.GetList(Apt_Code, "문서관리");
                await DisplayData("", "");
            }
            else
            {
                await decision_Lib.Add(decision);
                app = await approval_Lib.GetList(Apt_Code, "문서관리");
                await DisplayData("", "");
            }
            //StateHasChanged();
        }

        #endregion

        /// <summary>
        /// 구분
        /// </summary>
        /// <param name="Code"></param>
        /// <returns></returns>
        public string Division_Name(string Code)
        {
            string result = "";
            try
            {
                if (Code == "A")
                {
                    result = "관리소 발신";
                }
                else if (Code == "B")
                {
                    result = "관리소 수신";
                }
                else if (Code == "C")
                {
                    result = "입대의 발신";
                }
                else if (Code == "D")
                {
                    result = "입대의 수신";
                }
                else if (Code == "E")
                {
                    result = "선관위 발신";
                }
                else if (Code == "F")
                {
                    result = "선관위 수신";
                }
                else if (Code == "G")
                {
                    result = "기타 발신";
                }
                else if (Code == "H")
                {
                    result = "기타 수신";
                }
                else
                {
                    result = "잘못됨";
                }

                return result;
            }
            catch (Exception)
            {
                return "잘못됨";
            }

        }

        /// <summary>
        /// 분류명
        /// </summary>
        public string Sort_Name(string Sort_Code)
        {
            string a = "";
            try
            {
                a = document_Sort_Lib.SortName(Sort_Code);
            }
            catch (Exception)
            {
                a = "잘못됨";
            }
            return a;
        }

        private async Task OnDivision(ChangeEventArgs a)
        {
            bnn.Division = a.Value.ToString();
            int intCount = await documents_Lib.Division_Count(Apt_Code, bnn.Division, (bnn.AcceptDate.Year.ToString() + "-01-01"), (bnn.AcceptDate.Year + 1 + "-01-01"));
            bnn.Doc_Code = bnn.Division + "-" + bnn.AcceptDate.Year.ToString() + "-" + intCount.ToString();
        }

        /// <summary>
        /// 검색
        /// </summary>
        private void OnSearchs(ChangeEventArgs args)
        {
            strSearchs = args.Value.ToString();
        }

        /// <summary>
        /// 검색 후 정보 불러오기
        /// </summary>
        /// <returns></returns>
        private async Task OnClick()
        {
            if (strSearchs == "Z" || strSearchs == "")
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "로그인하지 않았습니다.");
            }
            else if (strQuery == "" || strQuery == null)
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "로그인하지 않았습니다.");
            }
            else
            {
                await DisplayData(strSearchs, strQuery);
            }
        }

        /// <summary>
        /// 새로 등록 열기
        /// </summary>
        private void btnInsert()
        {
            bnn = new Document_Entity();
            InsertViews = "B";
            bnn.AcceptDate = DateTime.Now;
        }

        /// <summary>
        /// 상세보기
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        protected async Task ByAid(Document_Entity entity)
        {
            isLoading = true;
            Views = "B";
            bnn = entity;
            Files_Count = await files_Lib.BeCount(bnn.Aid.ToString(), "Document");

            Files_Entity = await files_Lib.Sw_Files_Data_Index("Document", bnn.Aid.ToString(), Apt_Code);
            isLoading = false;
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
                await files_Lib.FileRemove(_files.Num.ToString(), "Document", Apt_Code);

                await documents_Lib.FilesCount(bnn.Aid, "B"); //파일 수 줄이기
                //await UploadRepositoryAsyncReference.DeleteAsync(Id); // 삭제
                //NavigationManagerReference.NavigateTo("/Uploads"); // 리스트 페이지로 이동
                Files_Count = await files_Lib.Sw_Files_Data_Index_Count("Document", bnn.Aid.ToString(), Apt_Code);
                if (Files_Count > 0)
                {
                    Files_Entity = await files_Lib.Sw_Files_Data_Index("Document", bnn.Aid.ToString(), Apt_Code);
                }
            }
            else
            {
                await JSRuntime.InvokeAsync<object>("alert", "취소되었습니다.");
            }
        }

        #region Event Handlers
        //private long maxFileSize = 1024 * 1024 * 30;
        [Inject] Microsoft.AspNetCore.Hosting.IWebHostEnvironment env { get; set; }
        public string strUserCode { get; private set; }

        


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
                finn.Parents_Num = bnn.Aid.ToString(); // 선택한 ParentId 값 가져오기 
                finn.Sub_Num = finn.Parents_Num;
                try
                {
                    var pathA = $"D:\\Msh\\Home\\Ayoung\\sw_Files_View\\Sw_files";

                    string strFiles = Path.GetExtension(file.Name);

                    string _FileName = "Document" + Apt_Code + bnn.Aid + strFiles;


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

                    finn.Sw_FileName = fileName;
                    finn.Sw_FileSize = Convert.ToInt32(file.Size);
                    finn.Parents_Name = "Document";
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
                    await documents_Lib.FilesCount(bnn.Aid, "A"); // 첨부파일 추가를 db 저장(문서관리, Document)
                }
                catch (Exception ex)
                {
                    Logger.LogError("File: {Filename} Error: {Error}",
                        file.Name, ex.Message);
                }
            }

            FileInputViews = "A";

            Files_Count = await files_Lib.Sw_Files_Data_Index_Count("Document", bnn.Aid.ToString(), Apt_Code);
            if (Files_Count > 0)
            {
                Files_Entity = await files_Lib.Sw_Files_Data_Index("Document", bnn.Aid.ToString(), Apt_Code);
            }

            isLoading = false;
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
        /// 문서 접수 등록
        /// </summary>
        private async Task btnSave()
        {
            bnn.Apt_Code = Apt_Code;
            bnn.Apt_Name = Apt_Name;
            bnn.WorkMan = User_Code;


            if (string.IsNullOrWhiteSpace(bnn.Apt_Code))
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "로그인되지 않았습니다..");
            }
            else if (string.IsNullOrWhiteSpace(bnn.Division))
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "문서 구분이 선택되지 않았습니다..");
            }
            else if (string.IsNullOrWhiteSpace(bnn.Sort_Code))
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "문서 분류가 선택되지 않았습니다..");
            }
            else if (string.IsNullOrWhiteSpace(bnn.Doc_Code))
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "문서번호가 등록되지 않았습니다..");
            }
            else if (string.IsNullOrWhiteSpace(bnn.Details))
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "문서 주요 내용이 입력되지 않았습니다..");
            }
            else if (string.IsNullOrWhiteSpace(bnn.Title))
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "문서 제목이 입력되지 않았습니다..");
            }
            else if (string.IsNullOrWhiteSpace(bnn.Organization))
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "문서 수발신처가 입력되지 않았습니다..");
            }
            else
            {
                if (bnn.Aid < 1)
                {

                    int a = await documents_Lib.Add(bnn);
                }
                else
                {
                    await documents_Lib.Edit(bnn);
                }
                InsertViews = "A";

                await DisplayData("", "");
            }
        }

        /// <summary>
        /// 문서 새로등록 닫기
        /// </summary>
        private void btnClose()
        {
            InsertViews = "A";
        }

        /// <summary>
        /// 문서 등록 수정
        /// </summary>
        public void ByEdit(Document_Entity entity)
        {
            bnn = entity;
            InsertViews = "B";
        }
    }
}
