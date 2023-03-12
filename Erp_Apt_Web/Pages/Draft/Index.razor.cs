using Erp_Apt_Lib;
using Erp_Apt_Lib.Decision;
using Erp_Apt_Lib.Draft;
using Erp_Apt_Staff;
using Erp_Entity;
using Erp_Lib;
using Facilities;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Erp_Apt_Web.Pages.Draft
{
    public partial class Index
    {
        #region 인스턴스
        [CascadingParameter] public Task<AuthenticationState> AuthenticationStateRef { get; set; }
        [Inject] public NavigationManager MyNav { get; set; } // Url 
        [Inject] IJSRuntime JSRuntime { get; set; }
        [Inject] public IStaff_Lib staff_Lib { get; set; }
        [Inject] public IDraft_Lib draft_Lib { get; set; }
        [Inject] public IDraftDetail_Lib draftDetail_Lib { get; set; }
        [Inject] public IDraftAttach_Lib draftAttach_Lib { get; set; }
        [Inject] public IBloom_Lib bloom_Lib { get; set; }
        [Inject] public IReferral_career_Lib referral_Career_Lib { get; set; }
        [Inject] public IDecision_Lib decision_Lib { get; set; } // 결재정보

        [Inject] public IPost_Lib Post_Lib { get; set; }
        [Inject] public ISw_Files_Lib files_Lib { get; set; }
        #endregion

        #region 속성
        List<DraftEntity> ann { get; set; } = new List<DraftEntity>();
        List<DraftDetailEntity> cnn { get; set; } = new List<DraftDetailEntity>();
        public double dbVat { get; private set; }
        public double dbTotalAccount { get; private set; }
        List<Post_Entity> pnn { get; set; } = new List<Post_Entity>();
        List<Referral_career_Entity> fnn = new List<Referral_career_Entity>();
        DraftEntity bnn { get; set; } = new DraftEntity();
        DraftDetailEntity dnn { get; set; } = new DraftDetailEntity();
        List<Bloom_Entity> bloom_A = new();
        Sw_Files_Entity hnn { get; set; } = new Sw_Files_Entity(); // 첨부 파일 정보
        List<Sw_Files_Entity> Files_Entity { get; set; } = new List<Sw_Files_Entity>();
        Referral_career_Entity cnnA { get; set; } = new Referral_career_Entity();
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
        public string InsertFiles { get; set; } = "A"; // 파일 등록 열기
        public string Titles { get; set; }
        public int intNum { get; set; }
        public string strSearchs { get; set; }
        public string strQuery { get; set; }
        public string DetailsViews { get; set; } = "A";
        public string strEdit { get; set; } = "A";
        public int intDetailsBeing { get; set; } = 0;
        public string strVat { get; set; }
        public string strPrice { get; set; }
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

            await DisplayData();

            StateHasChanged();
        }

        private async Task DisplayData()
        {
            pager.RecordCount = await draft_Lib.GetListCount(Apt_Code);
            ann = await draft_Lib.GetList(pager.PageIndex, Apt_Code);
            pnn = await Post_Lib.GetList("A");//부서 목록
            intNum = pager.RecordCount - (pager.PageIndex * pager.PageSize);
        }
        #endregion

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

                    cnnA = await referral_Career_Lib.Detail(User_Code);
                    PostDuty = cnnA.Post + cnnA.Duty;

                    app = await approval_Lib.GetList(Apt_Code, "기안문서");

                    referral = await referral_Career_Lib.Details(User_Code);
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
        /// 새로등록 열기 시에 실행
        /// </summary>
        private async Task btnNewViews()
        {
            bloom_A = await bloom_Lib.GetList_Apt_ba(Apt_Code); // 작업 대분류
            bnn.ExecutionDate = DateTime.Now.Date;
            bnn.DraftDate = DateTime.Now.Date;
            bnn.Organization = "관리사무소";
            bnn.AptCode = Apt_Code;
            bnn.AptName = Apt_Name;
            bnn.KeepYear = 0;
            dnn.AptCode = Apt_Code;

            InsertViews = "B";
        }

        private void OnSearchs(ChangeEventArgs args)
        {
            strSearchs = args.Value.ToString();
        }

        /// <summary>
        /// 타이틀 검색
        /// </summary>
        private async Task OnClick()
        {
            pager.RecordCount = await draft_Lib.SearchListCount(strSearchs, strQuery, Apt_Code);
            ann = await draft_Lib.SearchList(pager.PageIndex, strSearchs, strQuery, Apt_Code);
        }

        /// <summary>
        /// 합계
        /// </summary>
        private double TotalSum(int Aid)
        {
            return draftDetail_Lib.ToralSum(Aid);
        }

        /// <summary>
        /// 부서 선택 시 실행
        /// </summary>
        protected async Task OnPost(ChangeEventArgs args)
        {
            string Post = args.Value.ToString();
            bnn.Post = Post;
            fnn = await referral_Career_Lib.GetList_Post_Staff_be(Post, Apt_Code);
        }

        /// <summary>
        /// 기안자 선택 시 실행
        /// </summary>
        private async Task OnUser(ChangeEventArgs a)
        {
            bnn.UserCode = a.Value.ToString();
            bnn.UserName = await referral_Career_Lib.UserName(a.Value.ToString());
        }

        /// <summary>
        /// 새로등록 닫기
        /// </summary>
        private void btnClose()
        {
            InsertViews = "A";
            strEdit = "A";
        }

        /// <summary>
        /// 폼목 단가 입력닫기
        /// </summary>
        private void btnCloseC()
        {
            InsertViews = "A";
            DetailsViews = "A";
            Views = "B";
            strEdit = "A";
        }

        /// <summary>
        /// 기안 정보 등록하기
        /// </summary>
        private async Task btnSave()
        {
            bnn.UserCode = User_Code;
            bnn.UserName = User_Name;
            //(((new DraftLib()).LastAid(ann.AptCode, ann.DraftYear)) + 1);
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
            bnn.Vat = "A";
            bnn.OutDraft = "A";
            bnn.DraftYear = bnn.DraftDate.Year;
            bnn.DraftMonth = bnn.DraftDate.Month;
            bnn.DraftDay = bnn.DraftDate.Day;
            string a = (await draft_Lib.LastAid(Apt_Code, bnn.DraftYear)).ToString();
            bnn.DraftNum = "Df" + DateTime.Now.Year.ToString() + "-" + a;

            if (bnn.AptCode == null || bnn.AptCode == "")
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "로그인하지 않았습니다.");
                MyNav.NavigateTo("/");
            }
            else if (bnn.Content == null || bnn.Content == "")
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "기안 상세 내용을 입력하지 않았습니다.");
            }
            else if (bnn.BranchA == null || bnn.BranchA == "")
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "기안 분류를 입력하지 않았습니다.");
            }
            else if (bnn.DraftNum == null || bnn.DraftNum == "")
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "기안번호를 입력하지 않았습니다.");
            }
            else if (bnn.DraftTitle == null || bnn.DraftTitle == "")
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "기안 제목을 입력하지 않았습니다.");
            }
            else if (bnn.Organization == null || bnn.Organization == "")
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "기안 기관을 선택하지 않았습니다.");
            }
            else
            {
                if (bnn.Aid < 1)
                {
                    dnn.ParentAid = await draft_Lib.Add(bnn);
                }
                else
                {
                    await draft_Lib.Edit(bnn);
                }

                bnn = new DraftEntity();
                await DisplayData();
            }
            strEdit = "A";
        }

        /// <summary>
        /// 상세보기 열기
        /// </summary>
        /// <param name="ar"></param>
        /// <returns></returns>
        private async Task ByAid(DraftEntity ar)
        {
            intDetailsBeing = await draftDetail_Lib.GetListCount(ar.Aid);
            bnn = ar;
            cnn = await draftDetail_Lib.GetList(ar.Aid);
            Files_Count = await files_Lib.BeCount(bnn.Aid.ToString(), "Draft");
            Files_Entity = await files_Lib.Sw_Files_Data_Index("Draft", bnn.Aid.ToString(), Apt_Code);
            dbVat = await draftDetail_Lib.VatToralCount(ar.Aid);
            dbTotalAccount = await draftDetail_Lib.ToralCount(ar.Aid);
            Views = "B";
        }

        /// <summary>
        /// 상세보기 모달 닫기
        /// </summary>
        private void btnCloseA()
        {
            Views = "A";
            strEdit = "A";
        }

        /// <summary>
        /// 기안 세부 사항 입력 열기
        /// </summary>
        private void btnDetailsOpen()
        {

            dnn = new DraftDetailEntity();

            dnn.Vat = "A";
            InsertViews = "A";
            DetailsViews = "B";
            Views = "A";
            strEdit = "A";

        }

        /// <summary>
        /// 기안 세부사항 입력 저장
        /// </summary>
        /// <returns></returns>
        private async Task btnDetailsSave()
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
            #endregion

            dnn.AptCode = Apt_Code;
            dnn.UserCode = User_Code;
            if (dnn.ParentAid < 1)
            {
                dnn.ParentAid = bnn.Aid;
            }

            if (dnn.Article == "" || dnn.Article == null)
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "품목을 입력하지 않았습니다.");
            }
            else if (dnn.Goods == "" || dnn.Goods == null)
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "규격을 입력하지 않았습니다.");
            }
            else if (dnn.ParentAid < 1)
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "기안 코드를 입력하지 않았습니다.");
            }
            else if (dnn.Quantity < 1)
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "수량을 입력하지 않았습니다.");
            }
            else
            {
                if (dnn.Aid < 1)
                {
                    await draftDetail_Lib.Add(dnn);
                }
                else
                {
                    await draftDetail_Lib.Edit(dnn);
                }
                await DisplayData();
                dnn = new DraftDetailEntity();
                DetailsViews = "A";
                Views = "B";
                strEdit = "A";
                cnn = await draftDetail_Lib.GetList(bnn.Aid);
            }
        }

        /// <summary>
        /// 기안 세부사항 수정 열기
        /// </summary>
        /// <param name="ar"></param>
        private void btnEditOpen(DraftDetailEntity ar)
        {
            dnn = ar;
            strVat = dnn.Vat;
            strEdit = "B";
            DetailsViews = "B";
            Views = "A";
        }

        /// <summary>
        /// 기안 상세 내용 삭제
        /// </summary>
        /// <param name="ar"></param>
        /// <returns></returns>
        private async Task ByRemove(DraftDetailEntity ar)
        {
            bool isDelete = await JSRuntime.InvokeAsync<bool>("confirm", $"{ar.Aid} 첨부파일을 정말로 삭제하시겠습니까?");

            if (isDelete)
            {
                await draftDetail_Lib.Remove(ar.Aid);
                cnn = await draftDetail_Lib.GetList(bnn.Aid);
            }
            else
            {
                await JSRuntime.InvokeAsync<object>("alert", "취소되었습니다.");
            }
        }

        /// <summary>
        /// 부가세 선택 시 실행
        /// </summary>
        /// <param name="a"></param>
        private void OnVat(ChangeEventArgs a)
        {
            bnn.Vat = a.Value.ToString();
            strVat = a.Value.ToString();
            if (strVat == "B")
            {
                dnn.TotalAcount = dnn.Quantity * dnn.UnitCost;
                dnn.SupplyPrice = dnn.TotalAcount;
                dnn.VatAcount = 0;
            }
            else
            {
                dnn.TotalAcount = dnn.Quantity * dnn.UnitCost * 1.1;
                dnn.SupplyPrice = dnn.Quantity * dnn.UnitCost;
                dnn.VatAcount = dnn.TotalAcount / 10;
            }
            strPrice = string.Format("{0: ###,###.##}", dnn.SupplyPrice);
        }

        /// <summary>
        /// 수량입력시 실행
        /// </summary>
        /// <param name="a"></param>
        private void OnQuantity(ChangeEventArgs a)
        {
            dnn.Quantity = Convert.ToDouble(a.Value);
            if (strVat == "B")
            {
                dnn.TotalAcount = dnn.Quantity * dnn.UnitCost;
                dnn.SupplyPrice = dnn.TotalAcount;
                dnn.VatAcount = 0;
            }
            else
            {
                dnn.TotalAcount = dnn.Quantity * dnn.UnitCost * 1.1;
                dnn.SupplyPrice = dnn.Quantity * dnn.UnitCost;
                dnn.VatAcount = dnn.TotalAcount / 10;
            }
            strPrice = string.Format("{0: ###,###.##}", dnn.SupplyPrice);
        }

        /// <summary>
        /// 단가 입력 실행
        /// </summary>
        /// <param name="a"></param>
        private void OnUniCost(ChangeEventArgs a)
        {
            dnn.UnitCost = Convert.ToDouble(a.Value);
            if (strVat == "B")
            {
                dnn.TotalAcount = dnn.Quantity * dnn.UnitCost;
                dnn.SupplyPrice = dnn.TotalAcount;
                dnn.VatAcount = 0;
            }
            else
            {
                dnn.TotalAcount = dnn.Quantity * dnn.UnitCost * 1.1;
                dnn.SupplyPrice = dnn.Quantity * dnn.UnitCost;
                dnn.VatAcount = dnn.TotalAcount / 10;
            }
            strPrice = string.Format("{0: ###,###.##}", dnn.SupplyPrice);
        }

        /// <summary>
        /// 첨부파일 모달 열기
        /// </summary>
        private void btnFileInsert()
        {
            InsertFiles = "B";
        }

        /// <summary>
        /// 파일 첨부 닫기
        /// </summary>
        private void FilesClose()
        {
            InsertFiles = "A";
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
                    string rootFolder = $"D:\\Msh\\Home\\Ayoung\\sw_Files_View\\Sw_files\\{ _files.Sw_FileName}";
                    File.Delete(rootFolder);
                }
                await files_Lib.FileRemove(_files.Num.ToString(), "Draft", Apt_Code);

                await draft_Lib.FilesCount(bnn.Aid, "B"); //파일 수 줄이기
                //await UploadRepositoryAsyncReference.DeleteAsync(Id); // 삭제
                //NavigationManagerReference.NavigateTo("/Uploads"); // 리스트 페이지로 이동
                Files_Count = await files_Lib.Sw_Files_Data_Index_Count("Draft", bnn.Aid.ToString(), Apt_Code);
                if (Files_Count > 0)
                {
                    Files_Entity = await files_Lib.Sw_Files_Data_Index("Draft", bnn.Aid.ToString(), Apt_Code);
                }
            }
            else
            {
                await JSRuntime.InvokeAsync<object>("alert", "취소되었습니다.");
            }
        }

        #region Event Handlers        
        
        public int Files_Count { get; private set; }


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
                hnn.Parents_Num = bnn.Aid.ToString(); // 선택한 ParentId 값 가져오기 
                hnn.Sub_Num = hnn.Parents_Num;
                try
                {
                    var pathA = $"D:\\Msh\\Home\\Ayoung\\sw_Files_View\\Sw_files";

                    string strFiles = Path.GetExtension(file.Name);

                    string _FileName = "Draft" + Apt_Code + bnn.Aid + strFiles;

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

                    hnn.Sw_FileName = fileName;
                    hnn.Sw_FileSize = Convert.ToInt32(file.Size);
                    hnn.Parents_Name = "Draft";
                    hnn.AptCode = Apt_Code;
                    hnn.Del = "A";

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
                    hnn.PostIP = myIPAddress;

                    #endregion
                    await files_Lib.Sw_Files_Date_Insert(hnn); //첨부파일 데이터 db 저장
                    await draft_Lib.FilesCount(bnn.Aid, "A"); // 첨부파일 추가를 db 저장(문서관리, Document)

                    await DisplayData();
                    InsertFiles = "A";

                    cnn = await draftDetail_Lib.GetList(bnn.Aid);
                    Files_Count = await files_Lib.Sw_Files_Data_Index_Count("Draft", bnn.Aid.ToString(), Apt_Code);
                    if (Files_Count > 0)
                    {
                        Files_Entity = await files_Lib.Sw_Files_Data_Index("cnn = await draftDetail_Lib.GetList(bnn.Aid);", bnn.Aid.ToString(), Apt_Code);
                    }
                }
                catch (Exception ex)
                {

                    Logger.LogError("File: {Filename} Error: {Error}",
                        file.Name, ex.Message);
                }
            }

            

            isLoading = false;
        }

        #endregion

        /// <summary>
        /// 확정자 찾기오기
        /// </summary>
        /// <param name="filesName"></param>
        /// <returns></returns>
        private string OnExtension(string filesName)
        {
            return Path.GetExtension(filesName);
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
        public string strUserCode { get; private set; }

        /// <summary>
        ///결재여부
        /// </summary>
        /// <param name="objPostDate"></param>
        /// <returns></returns>
        public string FuncShowOK(string strPostDuty, string Apt_Code, int apNum)
        {
            string strBloomCode = "Draft";
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
        public byte[] sealImage(string aptcode, string post, string duty, object apNum, string BloomCode, string usercode)
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
                        int CodeBeing = dbImagesLib.Photos_Count(usercode);

                        if (CodeBeing > 0)
                        {
                            strResult = dbImagesLib.Photos_image(usercode);
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
            decision.BloomCode = "Draft";
            decision.Parent = bnn.Aid.ToString();
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

                await decision_Lib.Add(decision);
                await draft_Lib.Draft_Comform(bnn.Aid, "B");
                app = await approval_Lib.GetList(Apt_Code, "기안문서");
                //await DisplayData("", "");
            }
            else
            {
                await decision_Lib.Add(decision);
                app = await approval_Lib.GetList(Apt_Code, "기안문서");
                //await DisplayData("", "");
            }
            StateHasChanged();
        }

        #endregion
    }
}
