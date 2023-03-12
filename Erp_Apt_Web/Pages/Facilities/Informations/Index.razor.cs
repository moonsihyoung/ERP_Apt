using Erp_Apt_Lib;
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
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Works;

namespace Erp_Apt_Web.Pages.Facilities.Informations
{
    public partial class Index
    {
        #region MyRegion
        [CascadingParameter] public Task<AuthenticationState> AuthenticationStateRef { get; set; }
        [Inject] public IApt_Lib apt_Lib { get; set; }
        [Inject] public IApt_Sub_Lib apt_Sub_Lib { get; set; }
        [Inject] public NavigationManager MyNav { get; set; }
        [Inject] IJSRuntime JSRuntime { get; set; }
        [Inject] public ISw_Files_Lib files_Lib { get; set; } // 파일 첨부
        [Inject] public IBloom_Lib bloom_Lib { get; set; }

        [Inject] public IFacility_Lib facility_Lib { get; set; }
        [Inject] public IWorks_Lib works_Lib { get; set; }
        #endregion

        #region MyRegion
        public string strSearch { get; private set; }
        public List<Facility_Entity> ann { get; set; } = new List<Facility_Entity>();
        public Facility_Entity bnn { get; set; } = new Facility_Entity();
        List<Works_Entity> wnn = new List<Works_Entity>();


        public Apt_Sub_Entity dnn { get; set; } = new Apt_Sub_Entity();
        public List<Bloom_Entity> bnnA { get; set; } = new List<Bloom_Entity>();
        public List<Bloom_Entity> bnnB { get; set; } = new List<Bloom_Entity>();
        public List<Bloom_Entity> bnnC { get; set; } = new List<Bloom_Entity>();

        public List<Bloom_Entity> bnnAA { get; set; } = new List<Bloom_Entity>();
        public List<Bloom_Entity> bnnBB { get; set; } = new List<Bloom_Entity>();
        public List<Bloom_Entity> bnnCC { get; set; } = new List<Bloom_Entity>();
        public List<Bloom_Entity> bnnDD { get; set; } = new List<Bloom_Entity>();

        Sw_Files_Entity fnn { get; set; } = new Sw_Files_Entity();
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
        public string strSort { get; private set; }
        public string strWSortA { get; set; }
        public string strWSortB { get; set; }
        public string strWSortC { get; set; }

        public string strWSortAw { get; set; }
        public string strWSortBw { get; set; }
        public string strWSortCw { get; set; }

        public string strWSortAA { get; set; }
        public string strWSortBB { get; set; }
        public string strWSortCC { get; set; }
        public string strWSortDD { get; set; }

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

            if (strSort == "A" || strSort == "B" || strSort == "C")
            {
                await ListData();                
            }
            else
            {
                await DisplayData();
            }
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

            await WorkList(strWSortAw, strWSortBw, strWSortCw);

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

                bnnA = await bloom_Lib.GetList_Apt_ba(Apt_Code); // 작업 대분류
                bnnAA = await bloom_Lib.GetList_Apt_ba(Apt_Code); // 작업 대분류

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
            pager.RecordCount = await facility_Lib.GetList_Apt_Count(Apt_Code);
            ann = await facility_Lib.GetList_Apt(pager.PageIndex, Apt_Code);

            dnn = await apt_Sub_Lib.Detail(Apt_Code);
        }

        private async Task ListData()
        {
            if (strSort == "A")
            {
                pager.RecordCount = await facility_Lib.GetList_Apt_SortA_Count(Apt_Code, strWSortA);
                ann = await facility_Lib.GetList_Apt_SortA(pager.PageIndex, Apt_Code, strWSortA);
            }
            else if (strSort == "B")
            {
                pager.RecordCount = await facility_Lib.GetList_Apt_SortB_Count(Apt_Code, strWSortA, strWSortB);
                ann = await facility_Lib.GetList_Apt_SortB(pager.PageIndex, Apt_Code, strWSortA, strWSortB);
            }
            else if (strSort == "C")
            {
                pager.RecordCount = await facility_Lib.GetList_Apt_SortC_Count(Apt_Code, strWSortA, strWSortB, strWSortC);
                ann = await facility_Lib.GetList_Apt_SortC(pager.PageIndex, Apt_Code, strWSortA, strWSortB, strWSortC);
            }
        }

        private async Task btnNewViews()
        {
            bnn = new Facility_Entity();
            try
            {
                bnn.Installation_Date = await apt_Lib.Apt_BuildDate(Apt_Code);
            }
            catch (Exception)
            {

                bnn.Installation_Date = DateTime.Now;
            }
            //int r = DateTime.Now.Millisecond;
            strTitle = "시설물 새로 등록";
            InsertViews = "B";
        }

        private async Task onWSortA(ChangeEventArgs a)
        {
            strSort = "A";
            strWSortA = a.Value.ToString();
            strWSortB = "";
            strWSortC = "";           

            bnnB = await bloom_Lib.GetList_bb(strWSortA);

            await ListData();
        }
        private async Task onWSortB(ChangeEventArgs a)
        {
            strSort = "B";
            strWSortB = a.Value.ToString();
            strWSortC = "";
            
            bnnC = await bloom_Lib.GetList_cc(strWSortB);
            await ListData();
        }
        private async Task onWSortC(ChangeEventArgs a)
        {
            strSort = "C";
            strWSortC = a.Value.ToString();
            
            await ListData();
        }
        private async Task onWSortAA(ChangeEventArgs a)
        {
            //strSort = "A";
            strWSortAA = a.Value.ToString();
            bnn.Sort_A_Name = strWSortAA;
            bnn.Sort_A_Code = await bloom_Lib.BloomNameA(strWSortAA);
            bnnBB = await bloom_Lib.GetList_bb(strWSortAA);
            strWSortBB = "";
            strWSortCC = "";
            strWSortDD = "";
            //await ListData(strWSortA, strWSortB, strWSortC, "A");
        }
        private async Task onWSortBB(ChangeEventArgs a)
        {
            //strSort = "B";
            strWSortBB = a.Value.ToString();
            bnn.Sort_B_Name = strWSortBB;
            bnn.Sort_B_Code = await bloom_Lib.BloomNameB(strWSortAA, strWSortBB);
            strWSortCC = "";
            strWSortDD = "";
            bnnCC = await bloom_Lib.GetList_cc(strWSortBB);
        }
        private async Task onWSortCC(ChangeEventArgs a)
        {
            //strSort = "C";
            strWSortCC = a.Value.ToString();
            bnn.Sort_C_Name = strWSortCC;
            bnn.Sort_C_Code = await bloom_Lib.BloomNameC(strWSortAA, strWSortBB, strWSortCC);

            strWSortDD = "";
            bnnDD = await bloom_Lib.GetList_dd(Apt_Code, strWSortAA);
        }
        private void onWSortDD(ChangeEventArgs a)
        {
            //strSort = "C";
            strWSortDD = a.Value.ToString();
            bnn.Position = strWSortDD;
        }

        private async Task ByAid(Facility_Entity ar)
        {
            bnn = ar;
            strWSortAw = ar.Sort_A_Name;
            strWSortBw = ar.Sort_B_Name;
            strWSortCw = ar.Sort_C_Name;
            await WorkList(ar.Sort_A_Name, ar.Sort_B_Name, ar.Sort_C_Name);
            await FileCount();
            strTitle = bnn.Facility_Name + " 시설물 이력 정보";
            Views = "B";
        }

        /// <summary>
        /// 해당 시설물 작업일지 목록
        /// </summary>
        private async Task WorkList(string a, string b, string c)
        {
            pager1.RecordCount = await works_Lib.ListWorkC_Count(Apt_Code, a, b, c);
            wnn = await works_Lib.ListWorkC(pager1.PageIndex, Apt_Code, a, b, c);
        }

        private void ByEdit(Facility_Entity ar)
        {
            bnn = ar;

            strWSortAA = bnn.Sort_A_Name;
            strWSortBB = bnn.Sort_B_Name;
            strWSortCC = bnn.Sort_C_Name;
            strWSortDD = bnn.Position;
            strTitle = "시설물 정보 수정";
            InsertViews = "B";
        }

        private async Task ByRemove(Facility_Entity ar)
        {
            bool isDelete = await JSRuntime.InvokeAsync<bool>("confirm", $"{ar.Aid}번 작업일지를 정말로 삭제하시겠습니까?");
            if (isDelete)
            {
                await works_Lib.WorksRemove(ar.Aid.ToString());
                await DisplayData();
            }
        }

        /// <summary>
        /// 새로입력 혹은 수정 닫기
        /// </summary>
        private void btnClose()
        {
            InsertViews = "A";
        }

        /// <summary>
        /// 시설물 정보 입력
        /// </summary>
        /// <returns></returns>
        private async Task btnSave()
        {
            bnn.Apt_Code = Apt_Code;
            bnn.Apt_Name = Apt_Name;

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

            if (bnn.Apt_Code == null || bnn.Apt_Code == "")
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "로그인되지 않았습니다..");
            }
            else if (bnn.Sort_A_Name == null || bnn.Sort_A_Name == "")
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "대분류가 선택되지 않았습니다..");
            }
            else if (bnn.Sort_B_Name == null || bnn.Sort_B_Name == "")
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "중분류가 선택되지 않았습니다..");
            }
            else if (bnn.Sort_C_Name == null || bnn.Sort_C_Name == "")
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "소분류가 선택되지 않았습니다..");
            }
            else if (bnn.Position == null || bnn.Position == "")
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "장소가 선택되지 않았습니다..");
            }
            else if (bnn.Quantity < 1)
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "수량이 입력되지 않았습니다..");
            }
            else if (bnn.Manufacture == null || bnn.Manufacture == "")
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "제조사가 입력되지 않았습니다..");
            }
            else if (bnn.Explanation == null || bnn.Explanation == "")
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "설명이 입력되지 않았습니다..");
            }
            else if (bnn.Facility_Name == null || bnn.Facility_Name == "")
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "시설물 이름이 입력되지 않았습니다..");
            }
            else
            {
                if (bnn.Aid < 1)
                {
                    await facility_Lib.Add(bnn);
                }
                else
                {
                    await facility_Lib.Edit(bnn);
                }
            }
            bnn = new Facility_Entity();
            await DisplayData();
            InsertViews = "A";
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

        private async Task LoadFiles(InputFileChangeEventArgs e)
        {
            isLoading = true;
            loadedFiles.Clear();
            progressPercent = 0;

            foreach (var file in e.GetMultipleFiles(maxAllowedFiles))
            {
                fnn.Parents_Num = bnn.Aid.ToString(); // 선택한 ParentId 값 가져오기 
                fnn.Sub_Num = fnn.Parents_Num;
                try
                {
                    var pathA = $"D:\\Msh\\Home\\Ayoung\\sw_Files_View\\Sw_files";


                    string strFiles = Path.GetExtension(file.Name);

                    string _FileName = "Institution" + Apt_Code + bnn.Aid + strFiles;

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
                    fnn.Parents_Name = "Institution";
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
            FileInsertViews = "A";

            await FileCount();

            isLoading = false;
        }



        private async Task FileCount()
        {
            Files_Count = await files_Lib.Sw_Files_Data_Index_Count("Institution", bnn.Aid.ToString(), Apt_Code);
            if (Files_Count > 0)
            {
                Files_Entity = await files_Lib.Sw_Files_Data_Index("Institution", bnn.Aid.ToString(), Apt_Code);
            }
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

                Files_Count = await files_Lib.Sw_Files_Data_Index_Count(_files.Parents_Name, bnn.Aid.ToString(), Apt_Code);
                if (Files_Count > 0)
                {
                    Files_Entity = await files_Lib.Sw_Files_Data_Index(_files.Parents_Name, bnn.Aid.ToString(), Apt_Code);
                }
            }
            else
            {
                await JSRuntime.InvokeAsync<object>("alert", "취소되었습니다.");
            }
        }

        public void btnCloseA()
        {
            Views = "A";
        }

        /// <summary>
        /// 상세보기로 이동
        /// </summary>
        /// <param name="Aid"></param>
        public void ByViews(int Aid)
        {
            MyNav.NavigateTo("/Works/Details/" + Aid);
        }

        /// <summary>
        /// 파일 업로드
        /// </summary>
        private void btnFileSave()
        {
            FileInsertViews = "B";
        }

        /// <summary>
        /// 파일 열기 닫기
        /// </summary>
        public void FilesClose()
        {
            FileInsertViews = "A";
        }
    }
}
