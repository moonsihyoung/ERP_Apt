using Erp_Apt_Lib;
using Erp_Apt_Lib.Check;
using Erp_Apt_Lib.Decision;
using Erp_Apt_Staff;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Http;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace Erp_Apt_Web.Pages.Check.Input
{
    public partial class Views
    {
        #region 변수
        public string User_Code { get; set; }
        public string User_Name { get; set; }
        public string Apt_Code { get; set; }
        public string Apt_Name { get; set; }
        public string UserName { get; set; }
        public int LevelCount { get; set; }
        public string View { get; set; } = "A";
        public string InsertViews { get; set; } = "A";
        public string strView { get; set; }
        public string FileViews { get; private set; }
        public int Files_Count { get; set; } = 0;
        public string PostDuty { get; private set; }

        public int AgoBe { get; set; }

        public int NextBe { get; set; }

        #endregion

        #region 로드
        List<Check_Object_Entity> bnn = new List<Check_Object_Entity>();
        List<Check_Cycle_Entity> cnn = new List<Check_Cycle_Entity>();
        List<Check_List_Entity> ann = new List<Check_List_Entity>();
        List<Sw_Files_Entity> Files_Entity { get; set; } = new List<Sw_Files_Entity>();
        Check_Input_Entity dnn { get; set; } = new Check_Input_Entity();
        Referral_career_Entity referral { get; set; } = new Referral_career_Entity();
        List<Approval_Entity> app { get; set; } = new List<Approval_Entity>();

        Sw_Files_Entity fnn { get; set; } = new Sw_Files_Entity();
        
        #endregion

        #region 인스턴스
        [Parameter] public int Code { get; set; }
        [CascadingParameter]
        public Task<AuthenticationState> AuthenticationStateRef { get; set; }

        [Inject] public ICheck_Object_Lib check_Object { get; set; }
        [Inject] public ICheck_Cycle_Lib check_Cycle_Lib { get; set; }
        [Inject] public ICheck_Input_Lib check_Input_Lib { get; set; }
        [Inject] public ICheck_List_Lib check_List_Lib { get; set; }
        [Inject] public ICheck_Items_Lib check_Items_Lib { get; set; }
        [Inject] public NavigationManager MyNav { get; set; }
        [Inject] IJSRuntime JSRuntime { get; set; }
        [Inject] public ISw_Files_Lib files_Lib { get; set; } // 파일 첨부
        [Inject] public IDecision_Lib decision_Lib { get; set; } // 결재정보
        [Inject] public IDbImagesLib dbImagesLib { get; set; } // 결재정보
        [Inject] public IReferral_career_Lib referral_Career_Lib { get; set; }
        [Inject] IHttpContextAccessor contextAccessor { get; set; }
        #endregion

        /// <summary>
        /// 처음 로드시에 실행
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
                LevelCount = Convert.ToInt32(authState.User.Claims.FirstOrDefault(c => c.Type == "LevelCount")?.Value);
                app = await approval_Lib.GetList(Apt_Code, "시설물점검일지");
                referral = await referral_Career_Lib.Details(User_Code);
                PostDuty = referral.Post + referral.Duty;
                await DisplayDate(Code);
            }
            else
            {
                await JSRuntime.InvokeAsync<object>("alert", "로그인되지 않았습니다.");
                MyNav.NavigateTo("/");
            }
        }

        /// <summary>
        /// 아이피 추출 
        /// </summary>
        public string MyIpAdress { get; set; }
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await JSRuntime.InvokeVoidAsync("setModalDraggableAndResizable");
            await base.OnAfterRenderAsync(firstRender);

            if (firstRender)
            {
                MyIpAdress = contextAccessor.HttpContext.Connection?.RemoteIpAddress.ToString();
                StateHasChanged();
            }
        }

        /// <summary>
        /// 데이터 보여주기
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        private async Task DisplayDate(int code)
        {
            dnn = await check_Input_Lib.CheckInput_Data_View(code, Apt_Code); // 데이터 불러오기 데이터
            ann = await check_List_Lib.GetCheckList_List_Index_new(dnn.Check_Object_Code, dnn.Check_Cycle_Code, dnn.Check_Year, dnn.Check_Month, dnn.Check_Day, Apt_Code);
            AgoBe = await check_Input_Lib.CheckInput_CountAgo(Apt_Code, code); //이전페이지 존재여부
            NextBe = await check_Input_Lib.CheckInput_CountNext(Apt_Code, code); //다음페이지 존재여부
        }

        /// <summary>
        /// 첨부파일 보기
        /// </summary>
        /// <param name="Aid"></param>
        /// <returns></returns>
        private async Task PhotoView(int Aid)
        {
            View = "B";
            //FileViews = "B";
            Files_Count = await files_Lib.Sw_Files_Data_Index_Count("Check", Aid.ToString(), Apt_Code);
            if (Files_Count > 0)
            {
                Files_Entity = await files_Lib.Sw_Files_Data_Index("Check", Aid.ToString(), Apt_Code);
            }

            //StateHasChanged();
        }

        #region 결제 여부(민원)
        Decision_Entity decision { get; set; } = new Decision_Entity();
        [Inject] public IApproval_Lib approval_Lib { get; set; }
        //[Inject] public IDbImagesLib dbImagesLib { get; set; }
        public string strUserName { get; set; }
        public string decisionA { get; set; }
        public string strNum { get; set; }
        public string strUserCode { get; private set; }

        /// <summary>
        ///결재여부
        /// </summary>
        /// <param name="objPostDate"></param>
        /// <returns></returns>
        public string FuncShowOK(string strPostDuty, string Apt_Code, int Aid)
        {
            string strBloomCode = "Check";
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
            decision.BloomCode = "Check";
            decision.Parent = strNum;
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

                await decision_Lib.Decision_Comform(strNum, "Check_Input", "Complete", "CheckInputID");

                app = await approval_Lib.GetList(Apt_Code, "시설물점검일지");

            }
            else
            {
                await decision_Lib.Add(decision);

                app = await approval_Lib.GetList(Apt_Code, "시설물점검일지");
            }
            //StateHasChanged();
        }

        #endregion

        private void FilesViewsClose()
        {
            View = "A";
            //StateHasChanged();
        }

        /// <summary>
        /// 이전 페이지로 이동
        /// </summary>
        /// <returns></returns>
        private async Task OnAgo()
        {
            int Ago = await check_Input_Lib.CheckInput_AgoA(Apt_Code, Code);
            //StateHasChanged();
            MyNav.NavigateTo("/Check/Input/Views/" + Ago);
            //Code = Ago;
            await DisplayDate(Ago);
           
        }

        /// <summary>
        /// 다음 페이지 이동
        /// </summary>
        /// <returns></returns>
        private async Task OnNext()
        {
            int NextBe = await check_Input_Lib.CheckInput_CountNext(Apt_Code, Code);
            if (NextBe > 0)
            {
                int Next = await check_Input_Lib.CheckInput_NextA(Apt_Code, Code);
                //Code = Next;
                //StateHasChanged();
                MyNav.NavigateTo("/Check/Input/Views/" + Next);
                await DisplayDate(Next);

            }
            
        }

        /// <summary>
        /// 목록으로 이동
        /// </summary>
        private void OnReset()
        {
            MyNav.NavigateTo("/Check/Input");
            
        }


        #region 파일 첨부 관련 함수 모음


        /// <summary>
        /// 파일 첨부 열기
        /// </summary>
        protected void FiledBy(int Num)
        {
            intAid = Num;
            FileInsertViews = "B";
            //StateHasChanged();
        }

        /// <summary>
        /// 첨부된 사진 보기
        /// </summary>
        protected async Task FileViewsBy(int Num)
        {
            FileViews = "B";
            Files_Count = await files_Lib.Sw_Files_Data_Index_Count("Check", Num.ToString(), Apt_Code);
            if (Files_Count > 0)
            {
                Files_Entity = await files_Lib.Sw_Files_Data_Index("Check", Num.ToString(), Apt_Code);
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

        #region Event Handlers
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
                fnn.Parents_Num = intAid.ToString(); // 선택한 ParentId 값 가져오기 
                fnn.Sub_Num = fnn.Parents_Num;
                try
                {
                    var pathA = $"{env.WebRootPath}\\UpFiles\\Check";
                    fileName = Dul.FileUtility.GetFileNameWithNumbering(pathA, file.Name);
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
                    fnn.Parents_Name = "Check";
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
                    await check_List_Lib.Files_Count_Add(intAid, "A"); // 파일 입력된 수 수정  
                }
                catch (Exception ex)
                {
                    Logger.LogError("File: {Filename} Error: {Error}",
                        file.Name, ex.Message);
                }
            }

            await DisplayDate(Code);
            FileInsertViews = "A";

            Files_Count = await files_Lib.Sw_Files_Data_Index_Count("Check", intAid.ToString(), Apt_Code);
            if (Files_Count > 0)
            {
                Files_Entity = await files_Lib.Sw_Files_Data_Index("Check", intAid.ToString(), Apt_Code);
            }

            isLoading = false;
        }

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
                    //await FileStorageManagerInjector.DeleteAsync(_files.Sw_FileName, "");
                    string rootFolder = $"{env.WebRootPath}\\UpFiles\\Check\\{_files.Sw_FileName}";
                    File.Delete(rootFolder);
                }
                await files_Lib.FileRemove(_files.Num.ToString(), "Check", Apt_Code);

                await check_List_Lib.Files_Count_Add(Convert.ToInt32(_files.Parents_Num), "B"); // 파일 입력된 수 수정
                //await UploadRepositoryAsyncReference.DeleteAsync(Id); // 삭제
                //NavigationManagerReference.NavigateTo("/Uploads"); // 리스트 페이지로 이동
                Files_Count = await files_Lib.Sw_Files_Data_Index_Count("Check", _files.Parents_Num, Apt_Code);
                if (Files_Count > 0)
                {
                    Files_Entity = await files_Lib.Sw_Files_Data_Index("Check", _files.Parents_Num, Apt_Code);
                }
            }
            else
            {
                await JSRuntime.InvokeAsync<object>("alert", "취소되었습니다.");
            }
        }
        #endregion


        public int strFiles_Count { get; set; }
        public int intAid { get; private set; }
        public string FileInsertViews { get; private set; }

        private async Task OnFileView()
        {
            View = "B";
            //FileViews = "B";
            strFiles_Count = await files_Lib.Sw_Files_Data_Index_Count("Check", Code.ToString(), Apt_Code);
            if (strFiles_Count > 0)
            {
                Files_Entity = await files_Lib.Sw_Files_Data_Index("Check", Code.ToString(), Apt_Code);
            }
        }
    }
}
