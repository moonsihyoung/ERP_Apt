using Erp_Apt_Lib;
using Erp_Apt_Lib.Stocks;
using Erp_Apt_Lib.Up_Files;
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

namespace Erp_Apt_Web.Pages.Stocks
{
    public partial class Index
    {
        #region MyRegion
        [CascadingParameter] public Task<AuthenticationState> AuthenticationStateRef { get; set; }
        [Inject] public IApt_Lib apt_Lib { get; set; }
        [Inject] public NavigationManager MyNav { get; set; }
        [Inject] IJSRuntime JSRuntime { get; set; }
        [Inject] public ISw_Files_Lib files_Lib { get; set; } // 파일 첨부
        [Inject] public IBloom_Lib bloom_Lib { get; set; }

        [Inject] public IStocks_Lib stocks_Lib { get; set; }
        [Inject] public IWhSock_Lib whSock_Lib { get; set; }
        #endregion

        #region MyRegion
        public string strSearch { get; private set; }
        public List<Stock_Code_Entity> ann { get; set; } = new List<Stock_Code_Entity>();
        public Stock_Code_Entity bnn { get; set; } = new Stock_Code_Entity();
        public string strGroupName { get; private set; }

        private string strSt_Code;

        public WareHouse_Entity dnn { get; set; } = new WareHouse_Entity();
        public List<WareHouse_Entity> enn { get; set; } = new List<WareHouse_Entity>();
        Sw_Files_Entity fnn { get; set; } = new Sw_Files_Entity();
        List<Bloom_Entity> bloom_A = new List<Bloom_Entity>();

        // UpFile_Entity fnn { get; set; } = new UpFile_Entity(); // 첨부 파일 정보
        List<Sw_Files_Entity> Files_Entity { get; set; } = new List<Sw_Files_Entity>(); 
        #endregion


        #region 변수
        public string User_Code { get; set; }
        public string User_Name { get; set; }
        public string Apt_Code { get; set; }
        public string Apt_Name { get; set; }
        public string UserName { get; set; }
        public string ListViews { get; set; } = "B";
        
        public string DetailsViews { get; set; } = "A";
        public string InsertViews { get; set; } = "A";
        
        public string EditViews { get; set; } = "A";
        public string Views { get; set; } = "A";
        
        public string strWSortA { get; set; }
        public string strWSortB { get; set; }
        public string strWSortC { get; set; }
        public string strWSortD { get; set; }
        
        public string FileInsertViews { get; set; } = "A";
        public string FileViews { get; set; } = "A";
        public string strTitle { get; set; }
        public int Files_Count { get; set; } = 0;
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

            StateHasChanged();
        }


        /// <summary>
        /// 페이징 속성
        /// </summary>
        protected DulPager.DulPagerBase pager1 = new DulPager.DulPagerBase()
        {
            PageNumber = 1,
            PageIndex = 0,
            PageSize = 15,
            PagerButtonCount = 5
        };


        /// <summary>
        /// 페이징
        /// </summary>
        protected async void PageIndexChanged1(int pageIndex)
        {
            pager1.PageIndex = pageIndex;
            pager1.PageNumber = pageIndex + 1;

            await whList_Date(strSt_Code);

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

                bloom_A = await bloom_Lib.GetList_Apt_ba(Apt_Code); // 작업 대분류

                await DisplayData();
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

        /// <summary>
        /// 데이터 불러오기
        /// </summary>
        /// <returns></returns>
        private async Task DisplayData()
        {
            pager.RecordCount = await stocks_Lib.St_List_Apt_New_Count(Apt_Code);
            ann = await stocks_Lib.St_List_Apt_New(pager.PageIndex, Apt_Code);
        }

        protected int Balance(string Code)
        {
            return whSock_Lib.Wh_Balance_(Code, Apt_Code);
        }

        private async Task btnNewViews()
        {
            bnn = new Stock_Code_Entity();
            int r = DateTime.Now.Millisecond;
            bnn.St_Code = "St" + r + await stocks_Lib.St_Count_Data(); 
            strTitle = "자재 정보 등록";
            InsertViews = "B";
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
            bnn.St_UserID = User_Code;
            //bnn.
                        
            if (bnn.Apt_Code == null || bnn.Apt_Code == "")
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "로그인되지 않았습니다..");
            }
            else if (bnn.St_Code == null || bnn.St_Code == "")
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "식별코드가 입력되지 않았습니다..");
            }
            else if (bnn.St_Name == null || bnn.St_Name == "")
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "자재명이 입력되지 않았습니다..");
            }
            else if (bnn.St_Group == null || bnn.St_Group == "")
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "자재 구분이 입력되지 않았습니다..");
            }
            else if (bnn.St_Model == null || bnn.St_Model == "")
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "모델명이 입력되지 않았습니다..");
            }
            else if (bnn.St_Dosage == null || bnn.St_Dosage == "")
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "규격이 입력되지 않았습니다..");
            }
            else if (bnn.St_Section == null || bnn.St_Section == "")
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "분류가 선택되지 않았습니다..");
            }
            else if (bnn.St_Unit == null || bnn.St_Unit == "")
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "단위가 선택되지 않았습니다..");
            }
            else if (string.IsNullOrWhiteSpace(bnn.St_Manual))
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "매뉴얼을 입력하지 않았습니다..");
            }
            else if (string.IsNullOrWhiteSpace(bnn.St_Using))
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "설명을 입력하지 않았습니다..");
            }
            else
            {
                if (bnn.Num < 1)
                {
                    await stocks_Lib.Stock_Code_Write(bnn);
                }
                else
                {
                    await stocks_Lib.St_Modify(bnn);
                }

                //await File_UpLoad(bnn.Num.ToString());

                bnn = new Stock_Code_Entity();

                InsertViews = "A";
                await DisplayData();
            }
            
        }

        private void btnClose()
        {
            InsertViews = "A";
        }

        private async Task ByAid(Stock_Code_Entity ar)
        {
            bnn = ar; //자재 상세정보
            strGroupName = "";
            strSt_Code = ar.St_Code;
            await whList_Date(strSt_Code); //자재 입출고 목록

            Files_Count = await files_Lib.Sw_Files_Data_Index_Count("WareHouse", bnn.Num.ToString(), Apt_Code);
            if (Files_Count > 0)
            {
                Files_Entity = await files_Lib.Sw_Files_Data_Index("WareHouse", bnn.Num.ToString(), Apt_Code);
            }
            strTitle = bnn.St_Name + " 상세정보";
            Views = "B";
        }

        /// <summary>
        /// 자재 입출고 목록
        /// </summary>
        private async Task whList_Date(string Code)
        {
            pager1.RecordCount = await whSock_Lib.GetList_StCodeCount(Apt_Code, Code);
            enn = await whSock_Lib.GetList_StCode(pager1.PageIndex, Apt_Code, Code);//해당 자재 입출고 목록
        }

        public void btnCloseA()
        {
            Views = "A";
        }

        /// <summary>
        /// 구분명 보기
        /// </summary>
        public string St_Group_(string Code)
        {
            string strR = "";
            if (Code == "st_Code_0")
            {
                strR = "소모품";
            }
            else if (Code == "st_Code_1")
            {
                strR = "공기구";
            }
            else if (Code == "st_Code_2")
            {
                strR = "소모자재";
            }
            else if (Code == "st_Code_3")
            {
                strR = "비품";
            }
            else
            {
                strR = "기타";
            }

            return strR;
        }

        /// <summary>
        /// 현재 잔고 불러오기
        /// </summary>
        public string BalanceAgo(string St_Code)
        {
            string strR = "";
            string strYear = (DateTime.Now.Year - 1).ToString();
            int r = whSock_Lib.Wh_BalanceYearLast(Apt_Code, St_Code, strYear);
            if (r > 0)
            {
                strR = string.Format("{0: ###,###}", r);
            }
            else
            {
                strR = "0";
            }

            return strR;
        }

        /// <summary>
        /// 현재 잔고 불러오기
        /// </summary>
        public string BalanceNow(string St_Code)
        {
            string strR = "";
            int r = whSock_Lib.Wh_BalanceNew(Apt_Code, St_Code);
            if (r > 0)
            {
                strR = string.Format("{0: ###,###}", r);
            }
            else
            {
                strR = "0";
            }

            return strR;
        }

        /// <summary>
        /// 전년도 출고 합계
        /// </summary>
        public string OutAgoSum(string St_Code)
        {
            string strR = "";
            string strYear = (DateTime.Now.Year - 1).ToString();
            int r = whSock_Lib.InOutSum(strYear, St_Code, Apt_Code, "A");

            if (r > 0)
            {
                strR = string.Format("{0: ###,###}", r);
            }
            else
            {
                strR = "0";
            }
            return strR;
        }

        /// <summary>
        /// 현재 출고 합계
        /// </summary>
        public string OutNowSum(string St_Code)
        {
            string strR = "";
            string strYear = DateTime.Now.Year.ToString();

            int r = whSock_Lib.InOutSum(strYear, St_Code, Apt_Code, "A");

            if (r > 0)
            {
                strR = string.Format("{0: ###,###}", r);
            }
            else
            {
                strR = "0";
            }
            //strR = ann.NumberFormat((new WhSock_Lib()).InOutSum(strYear, St_Code, AptCode, "A"));
            return strR;
        }
        

        /// <summary>
        /// 전년도 입고 합계
        /// </summary>
        public string IntAgoSum(string St_Code)
        {
            string strR = "";
            string strYear = (DateTime.Now.Year - 1).ToString();

            int r = whSock_Lib.InOutSum(strYear, St_Code, Apt_Code, "B");
            if (r > 0)
            {
                strR = string.Format("{0: ###,###}", r);
            }
            else
            {
                strR = "0";
            }
            //strR = ann.NumberFormat((new WhSock_Lib()).InOutSum(strYear, St_Code, AptCode, "B"));
            return strR;
        }

        /// <summary>
        /// 현재 입고 합계
        /// </summary>
        public string IntNowSum(string St_Code)
        {
            string strR = "";
            string strYear = DateTime.Now.Year.ToString();

            int r = whSock_Lib.InOutSum(strYear, St_Code, Apt_Code, "B");
            if (r > 0)
            {
                strR = string.Format("{0: ###,###}", r);
            }
            else
            {
                strR = "0";
            }
            //strR = ann.NumberFormat((new WhSock_Lib()).InOutSum(strYear, St_Code, AptCode, "B"));
            return strR;
        }

        /// <summary>
        /// 이전년도 구입 총액
        /// </summary>
        public string WhCostAgo(string St_Code)
        {
            string strR = "";
            string strYear = (DateTime.Now.Year - 1).ToString();

            int r = whSock_Lib.wh_Cost_Sum(Apt_Code, St_Code, strYear);
            if (r > 0)
            {
                strR = string.Format("{0: ###,###}", r);
            }
            else
            {
                strR = "0";
            }

            return strR;
        }

        /// <summary>
        /// 당해년도 구입 총액
        /// </summary>
        public string WhCostNow(string St_Code)
        {
            string strR = "";
            string strYear = DateTime.Now.Year.ToString();

            int r = whSock_Lib.wh_Cost_Sum(Apt_Code, St_Code, strYear);
            if (r > 0)
            {
                strR = string.Format("{0: ###,###}", r);
            }
            else
            {
                strR = "0";
            }           

            return strR;
        }

        private void ByEdit(Stock_Code_Entity ar)
        {
            bnn = ar;
            strTitle = "자재 정보 수정";
            InsertViews = "B";
        }

        private async void ByRemove(Stock_Code_Entity ar)
        {
            bool isDelete = await JSRuntime.InvokeAsync<bool>("confirm", $"{ar.St_Name} 을 정말로 삭제하시겠습니까?");

            if (isDelete)
            {                
                await whSock_Lib.Wh_Delete(ar.Num);
            }
            else
            {
                await JSRuntime.InvokeAsync<object>("alert", "취소되었습니다.");
            }
        }
        

        /// <summary>
        /// 확정자 찾기오기
        /// </summary>
        private string OnExtension(string filesName)
        {
            return Path.GetExtension(filesName);
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

        /// <summary>
        /// 파일 첨부
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        private async Task LoadFiles(InputFileChangeEventArgs e)
        {
            isLoading = true;
            loadedFiles.Clear();
            progressPercent = 0;

            foreach (var file in e.GetMultipleFiles(maxAllowedFiles))
            {
                fnn.Parents_Num = bnn.Num.ToString(); // 선택한 ParentId 값 가져오기 
                fnn.Sub_Num = fnn.Parents_Num;
                try
                {
                    var pathA = $"D:\\Msh\\Home\\Ayoung\\sw_Files_View\\Sw_files";


                    string strFiles = Path.GetExtension(file.Name);

                    string _FileName = "WareHouse" + Apt_Code + DateTime.Now.ToShortDateString() + strFiles;

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

                    fnn.Sw_FileName = fileName;
                    fnn.Sw_FileSize = Convert.ToInt32(file.Size);
                    fnn.Parents_Name = "WareHouse";
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
                }
                catch (Exception ex)
                {
                    Logger.LogError("File: {Filename} Error: {Error}",
                        file.Name, ex.Message);
                }
            }

            await DisplayData();
           
            strFileUpOpen = "A";

            Files_Count = await files_Lib.Sw_Files_Data_Index_Count("WareHouse", bnn.Num.ToString(), Apt_Code);
            if (Files_Count > 0)
            {
                Files_Entity = await files_Lib.Sw_Files_Data_Index("WareHouse", bnn.Num.ToString(), Apt_Code);
            }

            isLoading = false;
        }

        
        #endregion

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
                await files_Lib.Sw_Files_Date_Delete(_files.Num);

                Files_Count = await files_Lib.Sw_Files_Data_Index_Count(_files.Parents_Name, bnn.Num.ToString(), Apt_Code);
                if (Files_Count > 0)
                {
                    Files_Entity = await files_Lib.Sw_Files_Data_Index(_files.Parents_Name, bnn.Num.ToString(), Apt_Code);
                }
            }
            else
            {
                await JSRuntime.InvokeAsync<object>("alert", "취소되었습니다.");
            }
        }

        /// <summary>
        /// 검색
        /// </summary>
        private async Task OnSearch(ChangeEventArgs a)
        {
            strSearch = a.Value.ToString();
            ann = await stocks_Lib.stName_Query(Apt_Code, strSearch);
        }

        private void ByUrl(string code)
        {
            MyNav.NavigateTo("/Works/Details/" + code);
        }

        /// <summary>
        /// 파일 첨부 열기
        /// </summary>
        public string strFileUpOpen { get; set; } = "A";
        private void btnFileUpOpen()
        {
            strFileUpOpen = "B";
        }

        

        public string strUserCode { get; private set; }

        /// <summary>
        /// 파일 첨부 닫기
        /// </summary>
        private void FilesClose()
        {
            strFileUpOpen = "A";
        }
    }
}
