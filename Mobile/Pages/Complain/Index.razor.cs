using Erp_Apt_Lib.Appeal;
using Erp_Apt_Lib;
using Erp_Lib;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Mobile.Pages.Complain
{
    public partial class Index
    {
        [CascadingParameter] public Task<AuthenticationState> AuthenticationStateRef { get; set; }
        [Inject] public IApt_Lib apt_Lib { get; set; }
        [Inject] public IAppeal_Lib appeal { get; set; }
        [Inject] public NavigationManager MyNav { get; set; }
        [Inject] IJSRuntime JSRuntime { get; set; }
        [Inject] public ISw_Files_Lib files_Lib { get; set; } // 파일 첨부
        [Inject] public IAppeal_Bloom_Lib appeal_bloom_Lib { get; set; } //민원분류
        [Inject] IErp_AptPeople_Lib aptPeople_Lib { get; set; } // 입주민 정보 클래스
        [Inject] Microsoft.AspNetCore.Hosting.IWebHostEnvironment env { get; set; }
        [Inject] IsubAppeal_Lib subAppeal { get; set; }

        public string User_Code { get; set; }
        public string User_Name { get; set; }
        public string Apt_Code { get; set; }
        public string Apt_Name { get; set; }
        public string Dong { get; set; }
        public string Ho { get; set; }
        public string Mobile { get; set; }
        public string Views { get; set; } = "A";
        public int ImagesCount { get; set; } = 0;
        public string ListViews { get; set; } = "B";
        public string InsertViews { get; set; }
        public string FilesViews { get; set; }
        public string FilesInsertViews { get; set; }
        public DateTime ApDate { get; set; } = DateTime.Now;
        public string Looding { get; set; } = "A";


        List<Appeal_Entity> ann { get; set; } = new List<Appeal_Entity>(); //민원 목록
        Appeal_Entity bnn { get; set; } = new Appeal_Entity(); //민원 상세

        Sw_Files_Entity dnn { get; set; } = new Sw_Files_Entity(); // 첨부 파일
        List<Sw_Files_Entity> Files { get; set; } = new List<Sw_Files_Entity>(); // 첨부파일 상세
        public string Sort_Name { get; set; }
        public int Period { get; set; }
        public List<Appeal_Bloom_Entity> abe { get; set; } = new List<Appeal_Bloom_Entity>();

        

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
                Dong = authState.User.Claims.FirstOrDefault(c => c.Type == "Dong")?.Value;
                Ho = authState.User.Claims.FirstOrDefault(c => c.Type == "Ho")?.Value;
                Mobile = authState.User.Claims.FirstOrDefault(c => c.Type == "Mobile")?.Value;

                ListViews = "B";

                await DisplayData();
            }
            else
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "로그인되지 않았습니다. 먼저 로그인하세요.");
                MyNav.NavigateTo("/");
            }
        }

        /// <summary>
        /// 민원목록
        /// </summary>
        private async Task DisplayData()
        {
            pager.RecordCount = await appeal.getlist_count(Apt_Code, Dong, Ho);
            ann = await appeal.getlist(pager.PageIndex, Apt_Code, Dong, Ho); // 해당 세대 민원 목록
        }

        /// <summary>
        /// 민원 입력 열기
        /// </summary>
        private void onNewbutton()
        {
            InsertViews = "B";
            ListViews = "A";
        }

        /// <summary>
        /// 대분류 선택 실행
        /// </summary>
        protected async Task onSort(ChangeEventArgs args)
        {
            Sort_Name = args.Value.ToString();
            abe = new List<Appeal_Bloom_Entity>();
            abe = await appeal_bloom_Lib.Asort_List(args.Value.ToString());
        }

        /// <summary>
        /// 소분류 선택 실행
        /// </summary>
        protected async Task onAsort(ChangeEventArgs args)
        {
            bnn.apTitle = Sort_Name + "_" + args.Value.ToString();
            bnn.Bloom_Code = await appeal_bloom_Lib.Asort_Code(args.Value.ToString());
        }


        /// <summary>
        /// 민원내용 입력
        /// </summary>
        /// <returns></returns>
        public async Task btnSave()
        {
            bnn.apYear = ApDate.Year.ToString();
            bnn.apMonth = ApDate.Month.ToString();
            bnn.apDay = ApDate.Day.ToString();
            bnn.apMinute = ApDate.Minute.ToString();
            bnn.subClock = ApDate.Hour.ToString();
            bnn.apDongNo = Dong;
            bnn.apHoNo = Ho;
            bnn.apHp = Mobile;
            bnn.apName = User_Name;
            bnn.AptName = Apt_Name;
            bnn.AptCode = Apt_Code;
            bnn.ComAlias = Apt_Code;
            bnn.ComTitle = Apt_Name;
            bnn.apPost = "인터넷";
            bnn.apReciever = "인터넷";

            if (bnn.apDongNo == null)
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "로그인되지 않았습니다..");
            }
            else if (bnn.apContent == null)
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "민원 내용을 입력하지 않았습니다..");
            }
            else if (bnn.Bloom_Code == null)
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "민원 분류를 선택하지 않았습니다..");
            }
            else if (bnn.apYear == null)
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "민원 접수일을 입력하지 않았습니다..");
            }
            else
            {
                //string Asort = await 
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

                bnn = await appeal.add(bnn);
                await DisplayData();
                InsertViews = "A";
                Views = "B";
            }

        }

        /// <summary>
        /// 민원입력 닫기
        /// </summary>
        protected void WorkClose()
        {
            //Views = "B";
            InsertViews = "A";
            ListViews = "B";
        }

        /// <summary>
        /// 민원 상세 정보
        /// </summary>
        List<subAppeal_Entity> gnn { get; set; } = new List<subAppeal_Entity>();
        protected async Task ByAid(Appeal_Entity entity)
        {
            ListViews = "A";
            Views = "B";

            ImagesCount = await files_Lib.Sw_View_Data_Count("Appeal", entity.Num.ToString());

            bnn = entity;

            gnn = await subAppeal.GetList(bnn.Num.ToString());
        }

        /// <summary>
        /// 민원 수정 열기
        /// </summary>
        protected void ByEdit(Appeal_Entity entity)
        {
            ListViews = "A";
            InsertViews = "B";
            bnn = entity;
        }

        /// <summary>
        /// 민원 삭제
        /// </summary>
        protected async Task ByRemove(Appeal_Entity entity)
        {
            bool isDelete = await JSRuntime.InvokeAsync<bool>("confirm", $"민원을 정말로 삭제하시겠습니까?");
            if (isDelete)
            {
                await appeal.Remove(entity.Num.ToString());

                ann = await appeal.getlistDongHo(pager.PageIndex, Apt_Code, Dong, Ho);
            }
            else
            {
                await JSRuntime.InvokeAsync<object>("alert", "취소되었습니다.");
            }
        }



       

        /// <summary>
        /// 상세보기 닫기
        /// </summary>
        protected void onViewsClose()
        {
            Views = "A";
            ListViews = "B";
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
                    string rootFolder = $"D:\\Msh\\Home\\Ayoung\\sw_Files_View\\Sw_files\\{ _files.Sw_FileName}";
                    File.Delete(rootFolder);

                }
                await files_Lib.FileRemove(_files.Num.ToString(), "Appeal", Apt_Code);

                Files = await files_Lib.Sw_Files_Data_Index("Appeal", bnn.Num.ToString(), Apt_Code);
            }
            else
            {
                await JSRuntime.InvokeAsync<object>("alert", "취소되었습니다.");
            }
        }

        #region 첨부 파일 관련
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
                finn.Parents_Num = bnn.Num.ToString(); // 선택한 ParentId 값 가져오기 
                finn.Sub_Num = finn.Parents_Num;
                try
                {
                    var pathA = $"D:\\Msh\\Home\\Ayoung\\sw_Files_View\\Sw_files";
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

                    finn.Sw_FileName = fileName;
                    finn.Sw_FileSize = Convert.ToInt32(file.Size);
                    finn.Parents_Name = "Appeal";
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

            FilesInsertViews = "A";

            ImagesCount = await files_Lib.Sw_Files_Data_Index_Count("Appeal", bnn.Num.ToString(), Apt_Code);
            if (ImagesCount > 0)
            {
                Files = await files_Lib.Sw_Files_Data_Index("Appeal", bnn.Num.ToString(), Apt_Code);
            }

            isLoading = false;
        }

        // <summary>
        /// 파일 첨부 닫기
        /// </summary>
        private void FilesClose()
        {
            FilesInsertViews = "A";
        }

        /// <summary>
        /// 파일 보기 닫기
        /// </summary>
        private void FilesViewsClose()
        {
            FilesViews = "A";
            //StateHasChanged();
        }
        #endregion

        /// <summary>
        /// 첨부파일 보기 모달 열기
        /// </summary>
        protected async Task onFileViews()
        {
            FilesViews = "B";
            ImagesCount = await files_Lib.Sw_Files_Data_Index_Count("Appeal", bnn.Num.ToString(), Apt_Code);
            if (ImagesCount > 0)
            {
                Files = await files_Lib.Sw_Files_Data_Index("Appeal", bnn.Num.ToString(), Apt_Code);
            }
        }

        /// <summary>
        /// 파일 올리기 모달 폼 열기
        /// </summary>
        protected async Task onFileInputbutton()
        {
            FilesInsertViews = "B";

            ImagesCount = await files_Lib.Sw_Files_Data_Index_Count("Appeal", bnn.Num.ToString(), Apt_Code);
            if (ImagesCount > 0)
            {
                Files = await files_Lib.Sw_Files_Data_Index("Appeal", bnn.Num.ToString(), Apt_Code);
            }
        }
    }
}
