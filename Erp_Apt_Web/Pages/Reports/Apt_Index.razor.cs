using Erp_Apt_Lib.Apt_Reports;
using Erp_Apt_Lib.ProofReport;
using Erp_Apt_Lib.Up_Files;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using System.Net.NetworkInformation;

namespace Erp_Apt_Web.Pages.Reports
{
    public partial class Apt_Index
    {

        [CascadingParameter] public Task<AuthenticationState> AuthenticationStateRef { get; set; }
        [Inject] public IProofReport_Lib proofReport_Lib { get; set; }
        [Inject] IJSRuntime JSRuntime { get; set; }
        [Inject] public NavigationManager MyNav { get; set; } // Url 
        [Inject] public IApt_Reports_Lib apt_Reports_Lib { get; set; }
        [Inject] public IReport_Bloom_Lib report_Bloom_Lib { get; set; }
        [Inject] public IReport_Division_Lib report_Division_Lib { get; set; }
        [Inject] public IUpFile_Lib files_Lib { get; set; }

        public List<Apt_Reports_Entity> ann { get; set; } = new List<Apt_Reports_Entity>();
        public List<Report_Bloom_Entity> rbn { get; set; } = new List<Report_Bloom_Entity>();
        public List<Report_Division_Entity> rdn { get; set; } = new List<Report_Division_Entity>();
        public Apt_Reports_Entity bnn { get; set; } = new Apt_Reports_Entity();
        UpFile_Entity dnn { get; set; } = new UpFile_Entity(); // 첨부 파일 정보
        List<UpFile_Entity> Files_Entity { get; set; } = new List<UpFile_Entity>();

        public string InsertViews { get; set; } = "A";
        public string Views { get; set; } = "A";
        public string Apt_Code { get; private set; }
        public string User_Code { get; private set; }
        public string Apt_Name { get; private set; }
        public string User_Name { get; private set; }
        public int LevelCount { get; private set; }
        public string strTitle { get; set; }
        public DateTime strdate { get; set; }
        public int Files_Count { get; set; } = 0;


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

                if (LevelCount > 5)
                {
                    rbn = await report_Bloom_Lib.GeList("A");
                    rdn = await report_Division_Lib.GeList("A");
                    await onComplete();
                    await DisplayData();
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

        private async Task DisplayData()
        {
            pager.RecordCount = await apt_Reports_Lib.GetList_Count_Apt(Apt_Code);
            ann = await apt_Reports_Lib.GetList_Apt_Code(pager.PageIndex, Apt_Code);
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
        /// 삭제
        /// </summary>
        /// <param name="ar"></param>
        /// <returns></returns>
        private async Task btnRemove(Apt_Reports_Entity ar)
        {
            bool isDelete = await JSRuntime.InvokeAsync<bool>("confirm", $"{ar.Report_Title}을 정말로 삭제하시겠습니까?");

            if (isDelete)
            {
                await apt_Reports_Lib.Remove(ar.Aid.ToString());

                int fileCount = await files_Lib.UpFile_Count(ar.Report_Title, ar.Aid.ToString(), Apt_Code);

                if (fileCount > 0)
                {
                    Files_Entity = await files_Lib.UpFile_List(ar.Report_Title, ar.Aid.ToString(), Apt_Code);
                    //dnn = await files_Lib.GetDetails(ar.Aid);
                    foreach (var st in Files_Entity)
                    {
                        if (!string.IsNullOrEmpty(st?.FileName))
                        {
                            string rootFolder = $"D:\\Msh\\Msh\\sw_Web\\Files_Up\\A_Files\\{st.FileName}";
                            File.Delete(rootFolder);
                        }
                        //await files_Lib.Remove_UpFile(dnn.Aid.ToString());
                    }

                }
                await DisplayData();
            }
            else
            {
                await JSRuntime.InvokeAsync<object>("alert", "취소되었습니다.");
            }
        }

        private void btnOpen()
        {
            strdate = DateTime.Now;
            bnn.Report_Year = strdate.Year.ToString();
            bnn.Report_Month = strdate.Month.ToString();
            InsertViews = "B";
            strTitle = "보고서 입력";
        }

        private async Task ByDetails(Apt_Reports_Entity ar)
        {
            bnn = await apt_Reports_Lib.Detail(ar.Aid.ToString());
            Files_Count = await files_Lib.UpFile_Count(bnn.Report_Title, bnn.Aid.ToString(), Apt_Code);
            if (Files_Count > 0)
            {
                Files_Entity = await files_Lib.UpFile_List(bnn.Report_Title, bnn.Aid.ToString(), Apt_Code);
            }
            strTitle = "보고 내용 상세";
            Views = "B";
        }

        private void ByEdits(Apt_Reports_Entity ar)
        {
            bnn = ar;
            InsertViews = "B";
        }

        private void btnClose()
        {
            InsertViews = "A";
        }

        private void OnBloom(ChangeEventArgs a)
        {
            bnn.Report_Bloom_Code = a.Value.ToString();
        }




        /// <summary>
        /// 저장
        /// </summary>
        public int intAid { get; set; }
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
            bnn.PostIP = myIPAddress;
            #endregion
            bnn.Apt_Code = Apt_Code;
            bnn.Staff_Code = User_Code;
            bnn.Complete = "A";

            bnn.Report_Title = await report_Bloom_Lib.ReportBloom_Name(bnn.Report_Bloom_Code);
            bnn.Report_Bloom_Name = bnn.Report_Title;

            if (bnn.Apt_Code == null || bnn.Apt_Code == "")
            {
                await JSRuntime.InvokeAsync<object>("alert", "로그인되지 않았습니다.");
            }
            else if (bnn.Report_Bloom_Code == null | bnn.Report_Bloom_Code == "")
            {
                await JSRuntime.InvokeAsync<object>("alert", "분류를 선택하지 않았습니다.");
            }
            else if (bnn.Report_Division_Code == null | bnn.Report_Division_Code == "")
            {
                await JSRuntime.InvokeAsync<object>("alert", "구분을 선택하지 않았습니다.");
            }
            else if (bnn.Report_Year == null | bnn.Report_Year == "")
            {
                await JSRuntime.InvokeAsync<object>("alert", "보고 년도를 선택하지 않았습니다.");
            }
            else if (bnn.Report_Month == null | bnn.Report_Month == "")
            {
                await JSRuntime.InvokeAsync<object>("alert", "보고 월을 선택하지 않았습니다.");
            }
            else if (bnn.Report_Title == null | bnn.Report_Title == "")
            {
                await JSRuntime.InvokeAsync<object>("alert", "보고서 이름을 선택하지 않았습니다.");
            }
            else if (bnn.Report_Content == null | bnn.Report_Content == "")
            {
                await JSRuntime.InvokeAsync<object>("alert", "보고서 설명을 입력하지 않았습니다.");
            }
            //else if (selectedImage == null)
            //{
            //    await JSRuntime.InvokeAsync<object>("alert", "첨부파일을 입력하지 않았습니다.");
            //}
            else
            {
                if (bnn.Aid < 1)
                {
                    var re = await apt_Reports_Lib.Add(bnn);
                    intAid = re.Aid;
                    await File_UpLoad();
                }
                else
                {
                    await apt_Reports_Lib.Edit(bnn);
                    await File_UpLoad();
                }
            }
            bnn = new Apt_Reports_Entity();
            await DisplayData();
            InsertViews = "A";
        }

        private async Task File_UpLoad()
        {
            isLoading = true;
            loadedFiles.Clear();
            progressPercent = 0;
            //intViews = 1;
            foreach (var file in selectedImage)
            {
                try
                {
                    var pathA = $"D:\\Msh\\Msh\\sw_Web\\Files_Up\\A_Files";

                    string strFiles = Path.GetExtension(file.Name);

                    string _FileName = bnn.Report_Title + Apt_Code + DateTime.Now.ToShortDateString() + strFiles;

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

                    dnn.FileName = fileName;
                    dnn.FileSize = Convert.ToInt32(file.Size);
                    dnn.Cnn_Name = bnn.Report_Title;
                    dnn.Cnn_Code = bnn.Aid.ToString();
                    dnn.Apt_Code = Apt_Code;
                    dnn.Sort = bnn.Report_Title;
                    dnn.Code = bnn.Report_Bloom_Code;

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

                    await files_Lib.Add_UpFile(dnn); //첨부파일 데이터 db 저장 
                    await apt_Reports_Lib.Files_Add(intAid, "A");
                }
                catch (Exception ex)
                {
                    Logger.LogError("File: {Filename} Error: {Error}",
                        file.Name, ex.Message);
                }
            }
            //FileInsertViews = "A";

            Files_Count = await files_Lib.UpFile_Count(dnn.Cnn_Name, bnn.Aid.ToString(), Apt_Code);
            if (Files_Count > 0)
            {
                Files_Entity = await files_Lib.UpFile_List(dnn.Cnn_Name, bnn.Aid.ToString(), Apt_Code);
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


        [Inject] ILogger<Index> Logger { get; set; }
        IReadOnlyList<IBrowserFile> selectedImage;
        private List<IBrowserFile> loadedFiles = new();
        private long maxFileSize = 1024 * 1024 * 30;
        private int maxAllowedFiles = 5;
        private bool isLoading;
        private decimal progressPercent;

      

        public string? fileName { get; set; }
        //public int intViews { get; set; } = 0;
        private void LoadFiles(InputFileChangeEventArgs e)
        {
            selectedImage = e.GetMultipleFiles(maxAllowedFiles);            
        }


        


        /// <summary>
        /// 첨부파일 삭제
        /// </summary>
        /// <param name="sw_Files"></param>
        private async Task FilesRemove(UpFile_Entity _files)
        {
            bool isDelete = await JSRuntime.InvokeAsync<bool>("confirm", $"{_files.FileName} 첨부파일을 정말로 삭제하시겠습니까?");

            if (isDelete)
            {
                if (!string.IsNullOrEmpty(_files?.FileName))
                {
                    // 첨부 파일 삭제 
                    //await FileStorageManagerInjector.DeleteAsync(_files.Sw_FileName, "");
                    string rootFolder = $"D:\\Msh\\Msh\\sw_Web\\Files_Up\\A_Files\\{_files.FileName}";
                    File.Delete(rootFolder);
                }
                await files_Lib.Remove_UpFile(_files.Aid.ToString());

                int intRe = Convert.ToInt32(_files.Cnn_Code);
                await apt_Reports_Lib.Files_Add(intRe, "B"); //부모 db에서 첨부된 파일 수 빼기
                                                             //_files.Cnn_Code, "B");


                Files_Count = await files_Lib.UpFile_Count(_files.Cnn_Name, bnn.Aid.ToString(), Apt_Code);
                if (Files_Count > 0)
                {
                    Files_Entity = await files_Lib.UpFile_List(_files.Cnn_Name, bnn.Aid.ToString(), Apt_Code);
                }
            }
            else
            {
                await JSRuntime.InvokeAsync<object>("alert", "취소되었습니다.");
            }
        }

        private void btnCloseV()
        {
            Views = "A";
        }

        public int strA { get; set; } = 0;
        public int strB { get; set; } = 0;
        public int strC { get; set; } = 0;
        public int strD { get; set; } = 0;
        public int strE { get; set; } = 0;
        public int strF { get; set; } = 0;
        public int strG { get; set; } = 0;
        public int strAA { get; set; } = 0;
        public int strBB { get; set; } = 0;
        public int strCC { get; set; } = 0;
        public int strDD { get; set; } = 0;
        public int strEE { get; set; } = 0;
        public int strFF { get; set; } = 0;
        public int strGG { get; set; } = 0;

        private async Task onComplete()
        {
            DateTime dt = DateTime.Now;
            strA = await apt_Reports_Lib.Report_Month_Count(Apt_Code, "1", dt.Year.ToString(), dt.Year.ToString()); //재무제표
            strB = await apt_Reports_Lib.Report_Count(Apt_Code, "2", dt.Year.ToString()); //예산서
            strC = await apt_Reports_Lib.Report_Count(Apt_Code, "3", dt.Year.ToString()); //사업계획서
            strD = await apt_Reports_Lib.Report_Count(Apt_Code, "4", dt.Year.ToString()); //장기수선계획서
            strE = await apt_Reports_Lib.Report_Month_Count(Apt_Code, "5", dt.Year.ToString(), dt.Year.ToString()); //회의록
            strF = await apt_Reports_Lib.Report_Month_Count(Apt_Code, "6", dt.Year.ToString(), dt.Year.ToString()); //부과내역서
            strG = await apt_Reports_Lib.Report_Month_Count(Apt_Code, "9", dt.Year.ToString(), dt.Year.ToString()); //안전교육내용

            strAA = await apt_Reports_Lib.Report_Count(Apt_Code, "1", dt.Year.ToString()); //재무제표
            //strBB = await apt_Reports_Lib.Report_Bloom_Count(Apt_Code, "2"); //예산서
            //strCC = await apt_Reports_Lib.Report_Bloom_Count(Apt_Code, "3"); //사업계획
            //strDD = await apt_Reports_Lib.Report_Bloom_Count(Apt_Code, "4"); //장기수선
            strEE = await apt_Reports_Lib.Report_Count(Apt_Code, "5", dt.Year.ToString()); //회의록
            strFF = await apt_Reports_Lib.Report_Count(Apt_Code, "6", dt.Year.ToString()); //부과내역
            strGG = await apt_Reports_Lib.Report_Count(Apt_Code, "7", dt.Year.ToString()); //안전교육
        }
    }
}
