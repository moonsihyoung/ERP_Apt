using Erp_Apt_Lib;
using Erp_Apt_Lib.Draft;
using Erp_Apt_Lib.Logs;
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
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace Erp_Apt_Web.Pages.CarInfor
{
    public partial class Index
    {
        #region 변수
        public string User_Code { get; set; }
        public string User_Name { get; set; }
        public string Apt_Code { get; set; }
        public string Apt_Name { get; set; }
        public string UserName { get; set; }
        public string strDong { get; set; }
        public int CarCount { get; set; } = 0;
        public string Division { get; set; } = "A";
        public string NumberSearch { get; set; }
        public string FileInsert { get; set; } = "A";
        public string FileViews { get; set; }
        public int Files_Count { get; set; } = 0;
        public string Views { get; set; } = "A";
        public string InsertViews { get; set; } = "A";
        public string PDFViews { get; set; } = "A";
        #endregion

        #region 로드
        public Car_Infor_entity ann { get; set; } = new Car_Infor_entity();
        List<Car_Infor_entity> bnn = new List<Car_Infor_entity>();

        public int intNum { get; private set; }
        public Apt_People_Entity apn { get; set; } = new Apt_People_Entity();
        List<Apt_People_Entity> pnn = new List<Apt_People_Entity>();
        List<Apt_People_Entity> onn = new List<Apt_People_Entity>();
        List<Apt_People_Entity> qnn = new List<Apt_People_Entity>();
        Sw_Files_Entity dnn { get; set; } = new Sw_Files_Entity();
        List<Sw_Files_Entity> Files_Entity { get; set; } = new List<Sw_Files_Entity>();
        Logs_Entites logs { get; set; } = new Logs_Entites();
        #endregion

        #region 인스턴스
        [CascadingParameter]
        public Task<AuthenticationState> AuthenticationStateRef { get; set; }
        [Inject] public ISw_Files_Lib files_Lib { get; set; } // 파일 첨부
        [Inject] public IApt_Lib apt_Lib { get; set; }
        [Inject] public ICar_Infor_Lib car_Infor { get; set; }
        [Inject] public IErp_AptPeople_Lib apt_people { get; set; }
        [Inject] public NavigationManager MyNav { get; set; }
        [Inject] IJSRuntime JSRuntime { get; set; }
        [Inject] public ILogs_Lib logs_Lib { get; set; }
        #endregion

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
            //intNum = pager.RecordCount - (pager.PageIndex * pager.PageSize);
            StateHasChanged();
        }

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

                pnn = await apt_people.DongList(Apt_Code);
                await DisplayData();

                ann.MoveDate = DateTime.Now;
                ann.ResisterDate = DateTime.Now;

                string strNum = await car_Infor.Last_SortAid(Apt_Code);

                int SortAid = Convert.ToInt32(strNum);

                ann.Sort_Aid = (SortAid + 1).ToString();

                #region 로그 파일 만들기
                logs.Note = "차량관리에 들어왔습니다."; logs.Logger = User_Code; logs.Application = "차량관리"; logs.ipAddress = ""; logs.Message = Apt_Code + " " + Apt_Name;
                await logs_Lib.add(logs); 
                #endregion
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

        private async Task DisplayData()
        {
            CarCount = await car_Infor.GetList_Car_Count(Apt_Code);
            pager.RecordCount = CarCount;
            bnn = await car_Infor.GetList_Car_All_A(pager.PageIndex, Apt_Code);

            intNum = pager.RecordCount - (pager.PageIndex * pager.PageSize);
        }

        /// <summary>
        /// 동선택 실행
        /// </summary>
        protected async Task onDong(ChangeEventArgs args)
        {
            ann.Dong = args.Value.ToString();
            onn = new List<Apt_People_Entity>();
            onn = await apt_people.DongHoList_new(Apt_Code, args.Value.ToString());

        }

        protected void onHo(ChangeEventArgs args)
        {
            ann.Ho = args.Value.ToString();
            //ann.Ho = strHo;
        }

        /// <summary>
        /// 이사 등록 열기
        /// </summary>
        /// <param name="args"></param>
        protected void onMove(ChangeEventArgs args)
        {
            ann.Move = args.Value.ToString();
        }

        /// <summary>
        /// 자동차 새로 등록
        /// </summary>
        /// <returns></returns>
        private async Task btnSave()
        {
            if (ann.Dong == "Z")
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "동을 선택하지 않았습니다..");
            }
            else if (ann.Ho == "Z")
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "호를 선택하지 않았습니다..");
            }
            else if (ann.Move == "Z")
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "이사여부를 선택하지 않았습니다..");
            }
            else if (ann.Move == null)
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "이사여부를 선택하지 않았습니다..");
            }
            else
            {
                //ann.PostDate = DateTime.Now;
                if (ann.Aid > 0)
                {
                    await car_Infor.Edit(ann);

                    #region 로그 파일 만들기
                    logs.Note = ann.Dong + "동" + ann.Ho + "호" + ann.Car_Num + " " + ann.Car_Name + "차량을 수정했습니다."; logs.Logger = User_Code; logs.Application = "차량관리"; logs.ipAddress = ""; logs.Message = Apt_Code + " " + Apt_Name;
                    await logs_Lib.add(logs);
                    #endregion

                    Views = "A";
                    onn = new List<Apt_People_Entity>();
                    await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "수정되었습니다...");
                    await DisplayData();
                    ann.Dong = "Z";
                    ann.Ho = "Z";
                    ann.Move = "A";
                    ann.Relation = "본인";
                    ann.MoveDate = DateTime.Now;
                    ann.ResisterDate = DateTime.Now;
                    int SortAid = Convert.ToInt32(await car_Infor.Last_SortAid(Apt_Code));
                    ann.Sort_Aid = (SortAid + 1).ToString();
                    InsertViews = "A";
                    //StateHasChanged(); // 현재 컴포넌트 재로드

                    

                }
                else
                {
                    //if (Division == "A")
                    //{
                    ann.Apt_Code = Apt_Code;
                    await car_Infor.Add(ann);

                    #region 로그 파일 만들기
                    logs.Note = ann.Dong + "동" + ann.Ho + "호" + ann.Car_Num + " " + ann.Car_Name + "차량을 새로 입력했습니다."; logs.Logger = User_Code; logs.Application = "차량관리"; logs.ipAddress = ""; logs.Message = Apt_Code + " " + Apt_Name;
                    await logs_Lib.add(logs);
                    #endregion

                    await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "등록되었습니다...");
                    Views = "A";
                    onn = new List<Apt_People_Entity>();
                    await DisplayData();
                    ann.Dong = "Z";
                    ann.Ho = "Z";
                    ann.Move = "A";
                    ann.Relation = "본인";
                    ann.MoveDate = DateTime.Now;
                    ann.ResisterDate = DateTime.Now;
                    int SortAid = Convert.ToInt32(await car_Infor.Last_SortAid(Apt_Code));
                    ann.Sort_Aid = (SortAid + 1).ToString();
                    InsertViews = "A";

                    // DisplayData
                    //StateHasChanged(); // 현재 컴포넌트 재로드
                    
                    //}
                    //else
                    //{
                    //    await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "새로등록을 클릭하고 진행해 주세요..");
                    //}
                    //bnn = await car_Infor.GetList_Car_All_A(Apt_Code);
                }

            }
        }

        /// <summary>
        /// 동 검색 시 실행
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        protected async Task onDongSearch(ChangeEventArgs args)
        {
            strDong = args.Value.ToString();
            qnn = new List<Apt_People_Entity>();
            //ann.Dong = strDong;
            qnn = await apt_people.DongHoList_new(Apt_Code, args.Value.ToString());
            //StateHasChanged(); // 현재 컴포넌트 재로드
        }

        protected async Task onHoSearch(ChangeEventArgs args)
        {
            string strHo = args.Value.ToString();

            //await DisplayData();
            bnn = await car_Infor.GetList_Ho(Apt_Code, strDong, strHo);
            //StateHasChanged(); // 현재 컴포넌트 재로드
        }

        /// <summary>
        /// 상세보기 및 수정
        /// </summary>
        /// <param name="vnn"></param>
        /// <returns></returns>
        public async Task btnByAid(Car_Infor_entity vnn)
        {
            onn = await apt_people.Dong_HoList(Apt_Code, vnn.Dong);
            ann = vnn;
            ann.MoveDate = DateTime.Now;
            Division = "A";
            Views = "B";
            

            //StateHasChanged(); // 현재 컴포넌트 재로드
        }

        /// <summary>
        /// 상세 팝업 닫기
        /// </summary>
        private void btnViewsClose()
        {
            Views = "A";
            //StateHasChanged(); // 현재 컴포넌트 재로드
        }

        /// <summary>
        /// 새로 등록 열기
        /// </summary>
        /// <returns></returns>
        private async Task OnNewInsert()
        {
            ann = new Car_Infor_entity();
            //pnn = await apt_people.DongList(Apt_Code);
            ann.Dong = "Z";
            ann.Ho = "Z";
            ann.Move = "A";
            ann.Relation = "본인";
            ann.MoveDate = DateTime.Now;
            ann.ResisterDate = DateTime.Now;
            int SortAid = Convert.ToInt32(await car_Infor.Last_SortAid(Apt_Code));
            ann.Sort_Aid = (SortAid + 1).ToString();
            Division = "A";
            InsertViews = "B";
            //StateHasChanged();
        }

        /// <summary>
        /// 차량 뒷넘버로 찾기
        /// </summary>
        /// <returns></returns>
        private async Task OnNumbersSearch()
        {
            bnn = await car_Infor.Search_Car_Infor(Apt_Code, "Car_Num", NumberSearch);
           // StateHasChanged();
        }

        /// <summary>
        /// 파일 첨부 열기
        /// </summary>
        private void btnFilesOpen()
        {
            FileInsert = "B";
           // StateHasChanged();
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
                dnn.Parents_Num = ann.Aid.ToString(); // 선택한 ParentId 값 가져오기 
                dnn.Sub_Num = dnn.Parents_Num;
                try
                {
                    var pathA = $"{env.WebRootPath}\\UpFiles\\Car_Infor";

                    string strFiles = Path.GetExtension(file.Name);

                    string _FileName = "Car_Infor" + Apt_Code + DateTime.Now.ToShortDateString() + strFiles;

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
                    dnn.Parents_Name = "Car_Infor";
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

                    await DisplayData();
                    
                    await files_Lib.Sw_Files_Date_Insert(dnn); //첨부파일 데이터 db 저장

                    await car_Infor.FilesCountAdd(ann.Aid.ToString(), "A"); // 첨부파일 추가를 db 저장(하자, defect)

                    FileInsert = "A";

                    Files_Count = await files_Lib.Sw_Files_Data_Index_Count("Car_Infor", ann.Aid.ToString(), Apt_Code);
                    if (Files_Count > 0)
                    {
                        Files_Entity = await files_Lib.Sw_Files_Data_Index("Car_Infor", ann.Aid.ToString(), Apt_Code);
                    }

                    await DisplayData();
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
                    string rootFolder = $"{env.WebRootPath}\\UpFiles\\Car_Infor\\{_files.Sw_FileName}";
                    File.Delete(rootFolder);
                }
                await files_Lib.FileRemove(_files.Num.ToString(), "Car_Infor", Apt_Code);

                await car_Infor.FilesCountAdd(ann.Aid.ToString(), "B");

                //await UploadRepositoryAsyncReference.DeleteAsync(Id); // 삭제
                //NavigationManagerReference.NavigateTo("/Uploads"); // 리스트 페이지로 이동
                Files_Count = await files_Lib.Sw_Files_Data_Index_Count("Car_Infor", ann.Aid.ToString(), Apt_Code);
                if (Files_Count > 0)
                {
                    Files_Entity = await files_Lib.Sw_Files_Data_Index("Car_Infor", ann.Aid.ToString(), Apt_Code);
                }
                await DisplayData();
                //StateHasChanged();
            }
            else
            {
                await JSRuntime.InvokeAsync<object>("alert", "취소되었습니다.");
            }
        }

        /// <summary>
        /// 첨부파일 보기 닫기
        /// </summary>
        private void FilesViewsClose()
        {
            FileViews = "A";
            PDFViews = "A";
            //StateHasChanged();
        }

        /// <summary>
        /// 파일 첨부 입력 닫기
        /// </summary>
        private void FilesClose()
        {
            FileInsert = "A";

           // StateHasChanged();
        }

        /// <summary>
        /// 첨부파일 보기
        /// </summary>
        private async Task btnFilesViewsOpen()
        {
            FileViews = "B";
            Files_Count = await files_Lib.Sw_Files_Data_Index_Count("Car_Infor", ann.Aid.ToString(), Apt_Code);
            if (Files_Count > 0)
            {
                Files_Entity = await files_Lib.Sw_Files_Data_Index("Car_Infor", ann.Aid.ToString(), Apt_Code);

                Path.GetFileNameWithoutExtension(PDFViews);
            }
            //StateHasChanged();
        }

        /// <summary>
        /// 수정 열기
        /// </summary>
        /// <param name="car_Infor"></param>
        private async Task btnByEdit(Car_Infor_entity car_Infor)
        {
            onn = await apt_people.Dong_HoList(Apt_Code, car_Infor.Dong);
            ann = car_Infor;
            ann.MoveDate = DateTime.Now;
            InsertViews = "B";
            Division = "B";
            //StateHasChanged(); // 현재 컴포넌트 재로드
        }

        /// <summary>
        /// 등록 팝업 닫기
        /// </summary>
        private void btnInsertViewsClose()
        {
            InsertViews = "A";
            //StateHasChanged(); // 현재 컴포넌트 재로드
        }

        private string PdfView(string Pdf)
        {
            return Path.GetExtension(Pdf);
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

        
    }
}
