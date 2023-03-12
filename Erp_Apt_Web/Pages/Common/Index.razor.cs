using Erp_Apt_Lib;
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

namespace Erp_Apt_Web.Pages.Common
{
    public partial class Index
    {
        #region MyRegion
        [CascadingParameter]
        public Task<AuthenticationState> AuthenticationStateRef { get; set; }

        [Inject] public NavigationManager MyNav { get; set; } // Url 
        [Inject] IJSRuntime JSRuntime { get; set; }
        [Inject] public IErp_AptPeople_Lib erp_AptPeople_Lib { get; set; } // 입주자카드 클래스
        [Inject] public IIn_AptPeople_Lib in_AptPeople_Lib { get; set; } // 홈페이지 가입자(로그인 등) 클래스
        [Inject] public ISw_Files_Lib files_Lib { get; set; } // 파일 첨부
        [Inject] public ICar_Infor_Lib car_Infor_Lib { get; set; } // 자동차 관련 클래스

        public List<Apt_People_Entity> ann { get; set; } = new List<Apt_People_Entity>(); // 입주자카드 목록
        public int intNum { get; private set; }
        public List<Apt_People_Entity> dong { get; set; } = new List<Apt_People_Entity>(); // 동 목록
        public List<Apt_People_Entity> ho { get; set; } = new List<Apt_People_Entity>(); // 호 목록
        public List<Apt_People_Entity> s_ho { get; set; } = new List<Apt_People_Entity>(); // 호 목록
        public Apt_People_Entity bnn { get; set; } = new Apt_People_Entity(); // 입주자카드 속성
        public Sw_Files_Entity dnn { get; set; } = new Sw_Files_Entity();
        List<Sw_Files_Entity> Files_Entity { get; set; } = new List<Sw_Files_Entity>(); 
        #endregion

        #region 변수
        public string User_Code { get; set; }
        public string User_Name { get; set; }
        public int LevelCount { get; private set; }
        public string Apt_Code { get; set; }
        public string Apt_Name { get; set; }
        public string Views { get; set; } = "A";
        public string EditViews { get; set; } = "A";
        public string MoveViews { get; set; } = "A";
        public DateTime InDateTime { get; set; }
        public DateTime InnerScn { get; set; }
        public DateTime MoveDate { get; set; }
        public string DetailsViews { get; set; } = "A";
        public string FileInsert { get; set; } = "A";
        public string FileViews { get; set; }
        public int Files_Count { get; set; } = 0;
        public string strDong { get; set; }
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
        /// 방문 시 실행
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
                InDateTime = DateTime.Now.Date;
                InnerScn = DateTime.Now.Date;

                if (LevelCount >= 5)
                {
                    dong = await erp_AptPeople_Lib.DongList(Apt_Code);

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
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "로그인하지 않았습니다.");
                MyNav.NavigateTo("/");
            }
        }

        private async Task DisplayData()
        {
            if (strSort == "A") // 모바일로 검색된 목록
            {
                ann = await erp_AptPeople_Lib.Apt_Mobile_List(Apt_Code, strMobile);                
            }
            else if (strSort == "B")// 이름으로 검색된 목록
            {
                ann = await erp_AptPeople_Lib.Apt_Name_List(Apt_Code, strName);
            }
            else
            {
                pager.RecordCount = await erp_AptPeople_Lib.Apt_List_Count(Apt_Code);
                ann = await erp_AptPeople_Lib.Apt_List_Page(pager.PageIndex, Apt_Code);
                intNum = pager.RecordCount - (pager.PageIndex * pager.PageSize);
            }
        }

        /// <summary>
        /// 검색
        /// </summary>
        public string strMobile { get; set; }
        public string strName { get; set; }
        public string SearchOpen { get; set; }
        public string strTitle { get; set; }
        public string strSort { get; set; }
        private void OnSearch()
        {
            SearchOpen = "B";
            strTitle = "입주자 정보 찾기";
        }

        /// <summary>
        /// 처음으로
        /// </summary>
        /// <returns></returns>
        private async Task Resert()
        {
            strSort = "";
            await DisplayData();
        }

        /// <summary>
        /// 모바일로 검색
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        private async Task OnSearch_Mobile(ChangeEventArgs a)
        {
            strSort = "A";
            strMobile = a.Value.ToString();
            await DisplayData();
        }

        /// <summary>
        /// 이름으로 검색
        /// </summary>
        private async Task OnSearch_Name(ChangeEventArgs a)
        {
            strSort= "B";
            strName = a.Value.ToString();
            await DisplayData();
        }

        /// <summary>
        /// 검색 모달 닫기
        /// </summary>
        private void btnCloseS()
        {
            SearchOpen = "A";
        }

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
        /// 모달 이동 
        /// </summary>
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await JSRuntime.InvokeVoidAsync("setModalDraggableAndResizable");
            await base.OnAfterRenderAsync(firstRender);
        }

        /// <summary>
        /// 입주자카드 모달품 열기
        /// </summary>
        protected void onApt_People_Open()
        {
            Views = "B";
            bnn = new Apt_People_Entity();
        }

        /// <summary>
        /// 동 목록 선택
        /// </summary>
        protected async Task onDong(ChangeEventArgs args)
        {
            ho = await erp_AptPeople_Lib.DongHoList_new(Apt_Code, args.Value.ToString());
            bnn.Dong = args.Value.ToString();
        }

        /// <summary>
        /// 호 목록 선택
        /// </summary>
        protected async Task onHo(ChangeEventArgs args)
        {
            bnn.Ho = args.Value.ToString();
            bnn.Area = await erp_AptPeople_Lib.DongHo_Area(Apt_Code, bnn.Dong, bnn.Ho);
        }

        /// <summary>
        /// 단체 가입 여부
        /// </summary>
        protected void onInnSoceity(ChangeEventArgs args)
        {
            bnn.InnSoceity = args.Value.ToString();
        }

        /// <summary>
        /// 분양여부 선택 실행
        /// </summary>
        protected void OnBunyong(ChangeEventArgs args)
        {
            bnn.Bunyong = args.Value.ToString();
        }

        /// <summary>
        /// 입주형태 선택 실행
        /// </summary>
        protected void OnInter(ChangeEventArgs args)
        {
            bnn.Inter = args.Value.ToString();
        }

        /// <summary>
        /// 입주자 카드 모달 닫기
        /// </summary>
        protected void btnClose()
        {
            Views = "A";
            bnn = new Apt_People_Entity();
        }

        protected void btnMoveClose()
        {
            MoveViews = "A";
            bnn = new Apt_People_Entity();
        }

        /// <summary>
        /// 입주자 카드 입력
        /// </summary>
        protected async Task btnAptPeopleSave()
        {
            bnn.InnerScn1 = InnerScn.ToShortDateString();
            bnn.InDateTime = InDateTime.ToShortDateString();
            bnn.Apt_Code = Apt_Code;
            bnn.Apt_Name = Apt_Name;
            bnn.UserID = User_Code;

            if (bnn.Num > 0)
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
                bnn.ModifyDate = DateTime.Now;
                bnn.ModifyIP = myIPAddress;
                #endregion
                await erp_AptPeople_Lib.Edit(bnn);
            }
            else
            {
                bnn.Relation = "세대주";
                bnn.Owner = bnn.InnerOwner;
                int intCount = await erp_AptPeople_Lib.Dong_Ho_Count(Apt_Code, bnn.Dong, bnn.Ho);
                if (intCount < 1)
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
                    await erp_AptPeople_Lib.Add(bnn);
                }
                else
                {
                    await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "먼저 이사를 입력하고 다시 입력하시기 바랍니다..");
                }
            }

            await DisplayData();
            Views = "A";
        }

        /// <summary>
        /// 이사 입력
        /// </summary>
        protected async Task btnMoveSave()
        {
            if (bnn.MoveA == "A")
            {
                //await erp_AptPeople_Lib.Move("B", MoveDate.ToShortDateString(), bnn.Num);
                
                await erp_AptPeople_Lib.Remove_Ho(bnn.Apt_Code, bnn.Dong, bnn.Ho); // 세대 전체 이사 처리 2023-02-22
                await car_Infor_Lib.Remove_Ho(bnn.Apt_Code, bnn.Dong, bnn.Ho); // 세대 전체 차량 등록 취소 2023-02-22
            }
            else
            {
                await erp_AptPeople_Lib.Move("A", null, bnn.Num);
            }

            bnn = new Apt_People_Entity();
            MoveViews = "A";
            await DisplayData();
        }

        /// <summary>
        /// 상세
        /// </summary>
        protected async Task ByDetails(Apt_People_Entity apt_Pople)
        {
            Views = "A";
            //InsertViews = "A";
            DetailsViews = "B";
            bnn = apt_Pople;
            strDate = Convert.ToDateTime("2000-01-01");
            Files_Count = await files_Lib.Sw_Files_Data_Index_Count("Apt_People", apt_Pople.Num.ToString(), Apt_Code);
            if (Files_Count > 0)
            {
                Files_Entity = await files_Lib.Sw_Files_Data_Index("Apt_People", apt_Pople.Num.ToString(), Apt_Code);
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
            //Views = "A";
        }



        /// <summary>
        /// 수정 열기
        /// </summary>
        /// <param name="defect"></param>
        protected async Task ByEdit(Apt_People_Entity apt_Pople)
        {
            Views = "B";
            ho = await erp_AptPeople_Lib.Dong_HoList(Apt_Code, apt_Pople.Dong);
            bnn = apt_Pople;
        }

        /// <summary>
        /// 이사 모달 열기
        /// </summary>
        protected void ByMove(Apt_People_Entity apt_Pople)
        {
            MoveViews = "B";
            bnn = apt_Pople;
            MoveDate = DateTime.Now;
        }

        /// <summary>
        /// 상세정보 닫기
        /// </summary>
        private void btnDetailsClose()
        {
            DetailsViews = "A";
            //StateHasChanged();
        }

        /// <summary>
        /// 파일 첨부 열기
        /// </summary>
        private void btnFilesOpen()
        {
            FileInsert = "B";
            //StateHasChanged();
        }

        /// <summary>
        /// 첨부파일 보기 열기
        /// </summary>
        private void btnFilesViewsOpen()
        {
            FileViews = "B";
        }


        #region Event Handlers

        [Inject] ILogger<Index> Logger { get; set; }
        private List<IBrowserFile> loadedFiles = new();
        private long maxFileSize = 1024 * 1024 * 30;
        private int maxAllowedFiles = 5;
        private bool isLoading;
        private decimal progressPercent;

        [Inject] Microsoft.AspNetCore.Hosting.IWebHostEnvironment env { get; set; }
        public string fileName { get; set; }

        private async Task LoadFiles(InputFileChangeEventArgs e)
        {
            isLoading = true;
            loadedFiles.Clear();
            progressPercent = 0;

            foreach (var file in e.GetMultipleFiles(maxAllowedFiles))
            {
                dnn.Parents_Num = bnn.Num.ToString(); // 선택한 ParentId 값 가져오기 
                dnn.Sub_Num = dnn.Parents_Num;
                try
                {
                    var pathA = $"{env.WebRootPath}\\UpFiles\\Apt_People";

                    string strFiles = Path.GetExtension(file.Name);

                    string _FileName = "Apt_People" + Apt_Code + DateTime.Now.ToShortDateString() + strFiles;

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
                    dnn.Parents_Name = "Apt_People";
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

                    await erp_AptPeople_Lib.FilesCountAdd(bnn.Num.ToString(), "A"); // 첨부파일 추가를 db 저장(하자, defect)
                }
                catch (Exception ex)
                {
                    Logger.LogError("File: {Filename} Error: {Error}",
                        file.Name, ex.Message);
                }
            }

            await DisplayData();
            FileInsert = "A";

            Files_Count = await files_Lib.Sw_Files_Data_Index_Count("Apt_People", bnn.Num.ToString(), Apt_Code);
            if (Files_Count > 0)
            {
                Files_Entity = await files_Lib.Sw_Files_Data_Index("Apt_People", bnn.Num.ToString(), Apt_Code);
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
                    string rootFolder = $"{env.WebRootPath}\\UpFiles\\Apt_People\\{_files.Sw_FileName}";
                    File.Delete(rootFolder);
                }
                await files_Lib.FileRemove(_files.Num.ToString(), "Apt_People", Apt_Code);
                //await UploadRepositoryAsyncReference.DeleteAsync(Id); // 삭제
                //NavigationManagerReference.NavigateTo("/Uploads"); // 리스트 페이지로 이동

                await erp_AptPeople_Lib.FilesCountAdd(bnn.Num.ToString(), "B"); // 첨부파일 추가를 db 저장(하자, defect)

                Files_Count = await files_Lib.Sw_Files_Data_Index_Count("Apt_People", bnn.Num.ToString(), Apt_Code);
                if (Files_Count > 0)
                {
                    Files_Entity = await files_Lib.Sw_Files_Data_Index("Apt_People", bnn.Num.ToString(), Apt_Code);
                }
                FileViews = "A";
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
            //StateHasChanged();
        }

        /// <summary>
        /// 파일 첨부 입력 닫기
        /// </summary>
        private void FilesClose()
        {
            FileInsert = "A";
        }

        /// <summary>
        /// 찾기 동선택 실행
        /// </summary>
        private async Task onSearchDong(ChangeEventArgs args)
        {
            if (!string.IsNullOrWhiteSpace(args.Value.ToString()))
            {
                strDong = args.Value.ToString();
                s_ho = new List<Apt_People_Entity>();
                s_ho = await erp_AptPeople_Lib.DongHoList_new(Apt_Code, args.Value.ToString());
            }
            else
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "동을 선택하지 않았습니다.");
            }
        }

        /// <summary>
        /// 찾기 호선택 실행
        /// </summary>
        private async Task onSearchHo(ChangeEventArgs args)
        {
            if (!string.IsNullOrWhiteSpace(args.Value.ToString()))
            {
                ann = await erp_AptPeople_Lib.DongHoList(Apt_Code, strDong, args.Value.ToString());
            }
            else
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "호를 선택하지 않았습니다.");
            }
        }

        /// <summary>
        /// 확정자 찾기오기
        /// </summary>
        private string OnExtension(string filesName)
        {
            return Path.GetExtension(filesName);
        }

        /// <summary>
        /// 가족 등록하기
        /// </summary>
        public DateTime strDate { get; set; }
        private async Task btnRelation()
        {
            int be = await erp_AptPeople_Lib.Dong_Ho_Name_Being(Apt_Code, bnn.Dong, bnn.Ho, bnn.InnerName);
            if (be < 1)
            {
                bnn.InnerScn1 = strDate.ToShortDateString();
                await erp_AptPeople_Lib.Add(bnn);
                await DisplayData();
            }
            else
            {
                await JSRuntime.InvokeAsync<object>("alert", "이미 입력된 가족입니다.");
            }
        }

        /// <summary>
        /// 동호 신규 등록 열기
        /// </summary>
        private void onNewFamily()
        {
            bnn = new Apt_People_Entity();
            ViewsNew = "B";
        }

        /// <summary>
        /// 동호 신규 등록
        /// </summary>
        public string ViewsNew { get; set; }
        private async Task btnNewSave()
        {
            bnn.InnerScn1 = InnerScn.ToShortDateString();
            bnn.InDateTime = InDateTime.ToShortDateString();
            bnn.Apt_Code = Apt_Code;
            bnn.Apt_Name = Apt_Name;
            bnn.UserID = User_Code;

            if (string.IsNullOrWhiteSpace(Apt_Code))
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "로그인되지 않았습니다..");
            }
            else if (string.IsNullOrWhiteSpace(bnn.Ho))
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "동호가 선택되지 않았습니다..");
            }
            else if (string.IsNullOrWhiteSpace(bnn.InnerName))
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "이름이 입력되지 않았습니다..");
            }
            else if (string.IsNullOrWhiteSpace(bnn.InDateTime))
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "입주일이 입력되지 않았습니다..");
            }
            else if (string.IsNullOrWhiteSpace(bnn.InnerScn1))
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "생년월일이 입력되지 않았습니다..");
            }
            else
            {
                bnn.Relation = "세대주";
                bnn.Owner = bnn.InnerOwner;
                int intCount = await erp_AptPeople_Lib.Dong_Ho_Count_New(Apt_Code, bnn.Dong, bnn.Ho);
                if (intCount < 1)
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
                    await erp_AptPeople_Lib.Add(bnn);

                    await DisplayData();
                    ViewsNew = "A";
                }
                else
                {
                    await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "이미 입력된 세대입니다...");
                }
            }
        }

        private void btnCloseA()
        {
            ViewsNew = "A";
        }
    }
}
