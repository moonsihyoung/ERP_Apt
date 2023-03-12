using Erp_Apt_Lib.Apt_Reports;
using Erp_Apt_Lib.ProofReport;
using Erp_Apt_Lib.Up_Files;
using Erp_Entity;
using Erp_Lib;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Erp_Apt_Web.Pages.Reports
{
    public partial class Index
    {
        [CascadingParameter] public Task<AuthenticationState> AuthenticationStateRef { get; set; }
        [Inject] public IProofReport_Lib proofReport_Lib { get; set; }
        [Inject] IJSRuntime JSRuntime { get; set; }
        [Inject] public NavigationManager MyNav { get; set; } // Url 
        [Inject] public IApt_Reports_Lib apt_Reports_Lib { get; set; }
        [Inject] public IReport_Bloom_Lib report_Bloom_Lib { get; set; }
        [Inject] public IReport_Division_Lib report_Division_Lib { get; set; }
        [Inject] public IUpFile_Lib files_Lib { get; set; }
        [Inject] ISido_Lib sido_Lib { get; set; }
        [Inject] IApt_Lib apt_Lib { get; set; }

        List<Sido_Entity> sido { get; set; } = new List<Sido_Entity>();
        List<Apt_Entity> apt { get; set; } = new List<Apt_Entity>();

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

                if (LevelCount >= 5)
                {
                    rbn = await report_Bloom_Lib.GeList("A");
                    rdn = await report_Division_Lib.GeList("A");
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

        /// <summary>
        /// 모달 이동 
        /// </summary>
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await JSRuntime.InvokeVoidAsync("setModalDraggableAndResizable");
            await base.OnAfterRenderAsync(firstRender);
        }

        public string strAptCode { get; set; } = "";
        private async Task DisplayData()
        {
            if (string.IsNullOrWhiteSpace(strAptCode))
            {
                pager.RecordCount = await apt_Reports_Lib.GetList_Count_All();
                ann = await apt_Reports_Lib.GetList_All(pager.PageIndex);
            }
            else
            {
                pager.RecordCount = await apt_Reports_Lib.GetList_Count_Apt(strAptCode);
                ann = await apt_Reports_Lib.GetList_Apt_Code(pager.PageIndex, strAptCode);
            }
        }

        /// <summary>
        /// 보고서 삭제
        /// </summary>
        private async Task btnRemove(Apt_Reports_Entity ar)
        {
            bool isDelete = await JSRuntime.InvokeAsync<bool>("confirm", $"{ar.Report_Title}을 정말로 삭제하시겠습니까?");

            if (isDelete)
            {
                await apt_Reports_Lib.Remove(ar.Aid.ToString());

                int fileCount = await files_Lib.UpFile_Count(ar.Report_Title, ar.Aid.ToString(), ar.Apt_Code);

                if (fileCount > 0)
                {
                    Files_Entity = await files_Lib.UpFile_List(ar.Report_Title, ar.Aid.ToString(), ar.Apt_Code);
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
            Files_Count = await files_Lib.UpFile_Count(bnn.Report_Title, bnn.Aid.ToString(), ar.Apt_Code);
            if (Files_Count > 0)
            {
                Files_Entity = await files_Lib.UpFile_List(bnn.Report_Title, bnn.Aid.ToString(), ar.Apt_Code);
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
            else
            {
                if (bnn.Aid < 1)
                {
                    await apt_Reports_Lib.Add(bnn);
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
        private bool isLoading;
        private async Task File_UpLoad()
        {
            isLoading = true;
            var fileName = "";
            var format = "image/png";
            #region 파일 업로드 관련 추가 코드 영역

            foreach (var file in selectedImage)
            {
                string strFiles = Path.GetExtension(file.Name);
                string _FileName = file.Name;
                strFiles = Path.GetExtension(_FileName);
                string Ydate = DateTime.Now.Year + "_" + DateTime.Now.Month;
                _FileName = Apt_Code + "_" + Ydate + "_" + strFiles;

                if (file.Size < maxFileSize)
                {
                    if (strFiles == ".jpg" || strFiles == ".png" || strFiles == ".gif")
                    {
                        var resizedImageFile = await file.RequestImageFileAsync(format, 1025, 760);
                        var buffer = new byte[resizedImageFile.Size];
                        await resizedImageFile.OpenReadStream(maxFileSize).ReadAsync(buffer);
                        var imageDataUrl = $"data:{format};base64,{Convert.ToBase64String(buffer)}";

                        Stream stream = resizedImageFile.OpenReadStream(maxFileSize);
                        //var path = $"{env.WebRootPath}\\Appeal";
                        var path = $"D:\\Msh\\Msh\\sw_Web\\Files_Up\\A_Files";
                        fileName = Dul.FileUtility.GetFileNameWithNumbering(path, _FileName);
                        path = path + $"\\{fileName}";

                        FileStream fs = File.Create(path);
                        await stream.CopyToAsync(fs);
                        fs.Close();

                        dnn.FileName = fileName;
                        dnn.FileSize = Convert.ToInt32(file.Size);
                        dnn.Cnn_Name = bnn.Report_Title;
                        dnn.Cnn_Code = bnn.Aid.ToString();
                        dnn.Apt_Code = Apt_Code;
                        dnn.Sort = bnn.Report_Title;
                        dnn.Code = bnn.Report_Bloom_Code;

                        //dnn. = "A";

                        await File_up(dnn.Cnn_Name);
                    }
                    else if (strFiles == ".pdf" || strFiles == ".ppt" || strFiles == ".pptx")
                    {
                        Stream stream = file.OpenReadStream();
                        //var path = $"{env.WebRootPath}\\Appeal";
                        var path = $"D:\\Msh\\Msh\\sw_Web\\Files_Up\\A_Files";
                        fileName = Dul.FileUtility.GetFileNameWithNumbering(path, _FileName);
                        path = path + $"\\{fileName}";
                        FileStream fs = File.Create(path);

                        await stream.CopyToAsync(fs);
                        fs.Close();

                        dnn.FileName = fileName;
                        dnn.FileSize = Convert.ToInt32(file.Size);
                        dnn.Cnn_Name = bnn.Report_Title;
                        dnn.Cnn_Code = bnn.Aid.ToString();
                        dnn.Apt_Code = Apt_Code;
                        dnn.Sort = bnn.Report_Title;
                        dnn.Code = bnn.Report_Bloom_Code;
                        //dnn.Del = "A";

                        await File_up(dnn.Cnn_Name);

                    }
                    else if (strFiles == ".zip" || strFiles == ".tar" || strFiles == ".rar" || strFiles == ".hwp")
                    {
                        Stream stream = file.OpenReadStream();
                        //var path = $"{env.WebRootPath}\\Appeal";
                        var path = $"D:\\Msh\\Msh\\sw_Web\\Files_Up\\A_Files";
                        fileName = Dul.FileUtility.GetFileNameWithNumbering(path, _FileName);
                        path = path + $"\\{fileName}";
                        FileStream fs = File.Create(path);

                        await stream.CopyToAsync(fs);
                        fs.Close();

                        dnn.FileName = fileName;
                        dnn.FileSize = Convert.ToInt32(file.Size);
                        dnn.Cnn_Name = bnn.Report_Title;
                        dnn.Cnn_Code = bnn.Aid.ToString();
                        dnn.Sort = bnn.Report_Title;
                        dnn.Code = bnn.Report_Bloom_Code;
                        dnn.Apt_Code = Apt_Code;

                        await File_up(bnn.Report_Title);

                    }
                    else if (strFiles == ".doc" || strFiles == ".docx" || strFiles == ".xls" || strFiles == ".xlsx")
                    {
                        Stream stream = file.OpenReadStream();
                        var path = $"D:\\Msh\\Msh\\sw_Web\\Files_Up\\A_Files";
                        fileName = Dul.FileUtility.GetFileNameWithNumbering(path, _FileName);
                        path = path + $"\\{fileName}";
                        FileStream fs = File.Create(path);

                        await stream.CopyToAsync(fs);
                        fs.Close();

                        dnn.FileName = fileName;
                        dnn.FileSize = Convert.ToInt32(file.Size);
                        dnn.Cnn_Name = bnn.Report_Title;
                        dnn.Cnn_Code = bnn.Aid.ToString();
                        dnn.Apt_Code = Apt_Code;
                        dnn.Sort = bnn.Report_Title;
                        dnn.Code = bnn.Report_Bloom_Code;

                        await File_up(dnn.Cnn_Name);
                    }
                    else
                    {
                        await JSRuntime.InvokeAsync<object>("alert", fileName + " 은 올리 수 없는 파일입니다.");
                    }
                }
                else
                {
                    await JSRuntime.InvokeAsync<object>("alert", fileName + "은 파일 크기가 넘 커서 올리 수 없는 파일입니다. \n 15메가(M) 이하인 파일만 올릴 수 있습니다.");
                }
            }
            #endregion
            isLoading = false;
        }

        /// <summary>
        /// 확정자 찾기오기
        /// </summary>
        private string OnExtension(string filesName)
        {
            return Path.GetExtension(filesName);
        }

        #region Event Handlers
        private long maxFileSize = 1024 * 1024 * 15;
        [Inject] Microsoft.AspNetCore.Hosting.IWebHostEnvironment env { get; set; }

        /// <summary>
        /// 파일 업로드
        /// </summary>
        private IList<string> imageDataUrls = new List<string>();
        IReadOnlyList<IBrowserFile> selectedImage;
        private async Task OnFileChage(InputFileChangeEventArgs e)
        {
            var maxAllowedFiles = 5;
            var format = "image/png";
            selectedImage = e.GetMultipleFiles(maxAllowedFiles);
            foreach (var imageFile in selectedImage)
            {
                string strDew = Path.GetExtension(imageFile.Name);
                if (strDew == ".jpg" || strDew == ".png" || strDew == ".gif")
                {
                    var resizedImageFile = await imageFile.RequestImageFileAsync(format, 300, 300);
                    var buffer = new byte[resizedImageFile.Size];
                    await resizedImageFile.OpenReadStream().ReadAsync(buffer);
                    var imageDataUrl = $"data:{format};base64,{Convert.ToBase64String(buffer)}";
                    imageDataUrls.Add(imageDataUrl);
                }
            }

        }

        /// <summary>
        /// 파일 업로드 메서드
        /// </summary>
        private async Task File_up(string strName)
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

            await files_Lib.Add_UpFile(dnn); //첨부파일 데이터 db 저장           

            Files_Count = await files_Lib.UpFile_Count(strName, bnn.Aid.ToString(), Apt_Code);
            if (Files_Count > 0)
            {
                Files_Entity = await files_Lib.UpFile_List(strName, bnn.Aid.ToString(), Apt_Code);
            }
        }
        #endregion

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
                    string rootFolder = $"D:\\Msh\\Msh\\sw_Web\\Files_Up\\A_Files\\{ _files.FileName}";
                    File.Delete(rootFolder);
                }
                await files_Lib.Remove_UpFile(_files.Aid.ToString());

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

        /// <summary>
        /// 관리자 확인
        /// </summary>
        /// <returns></returns>
        private async Task btnComplete(Apt_Reports_Entity ar)
        {
            if (ar.Result == "A")
            {
                await apt_Reports_Lib.Result("B", bnn.Aid.ToString());
                await DisplayData();
                Views = "A";
            }
            else
            {
                await apt_Reports_Lib.Result("A", bnn.Aid.ToString());
                await DisplayData();
                Views = "A";
            }
        }

        public string SearchViews { get; set; } = "A";
        public string strSido { get; set; }
        public string strSiGunGu { get; set; }
        private void btnSearchOpen()
        {
            strAptCode = "";
            SearchViews = "B";
        }

        private async Task OnSido(ChangeEventArgs a)
        {
            strSido = a.Value.ToString();
            sido = await sido_Lib.GetList(strSido);
        }

        private async Task OnSiGunGu(ChangeEventArgs a)
        {
            strSiGunGu = a.Value.ToString();
            apt = await apt_Lib.GetList_All_Sido_Gun(strSido, strSiGunGu);
        }

        private async Task OnApt(ChangeEventArgs a)
        {
            strAptCode = a.Value.ToString();
            await DisplayData();
            SearchViews = "A";
            strSido = "";
            strSiGunGu = "";
        }

        private async Task btnCloseS()
        {
            SearchViews = "A";
            strAptCode = "";
            await DisplayData();
            strSido = "";
            strSiGunGu = "";
            strSido = "";
            strAptCode = "";
        }

        /// <summary>
        /// 승인하기
        /// </summary>
        private async Task btnResult(Apt_Reports_Entity et)
        {
            if (et.Result == "A")
            {
                await apt_Reports_Lib.Result("B", et.Aid.ToString());
            }
            else
            {
                await apt_Reports_Lib.Result("A", et.Aid.ToString());
            }

            await DisplayData();
        }

        /// <summary>
        /// 이름으로 검색
        /// </summary>
        public string strSearchApt { get; set; }
        private async Task OnSearchApt(ChangeEventArgs a)
        {
            strSearchApt = a.Value.ToString();
            apt = await apt_Lib.SearchList(strSearchApt);
        }
    }
}
