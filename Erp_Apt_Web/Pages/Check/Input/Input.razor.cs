using Erp_Apt_Lib;
using Erp_Apt_Lib.Check;
using Erp_Apt_Staff;
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

namespace Erp_Apt_Web.Pages.Check.Input
{
    public partial class Input
    {
        #region 인스턴스
        [CascadingParameter] public Task<AuthenticationState> AuthenticationStateRef { get; set; }
        [Inject] public IApt_Lib apt_Lib { get; set; }
        [Inject] public NavigationManager MyNav { get; set; }
        [Inject] IJSRuntime JSRuntime { get; set; }
        [Inject] public IReferral_career_Lib referral_Career_Lib { get; set; }
        [Inject] public IPost_Lib Post_Lib { get; set; }
        [Inject] public ICheck_Object_Lib check_Object_Lib { get; set; } //점검 시설물
        [Inject] public ICheck_Cycle_Lib check_Cycle_Lib { get; set; } //점검 주기
        [Inject] public ICheck_Input_Lib check_Input_Lib { get; set; } //점검 등록
        [Inject] public ICheck_Items_Lib check_Items_Lib { get; set; } //점검 내용
        [Inject] public ICheck_List_Lib check_List_Lib { get; set; } //점검 목록
        [Inject] public ICheck_Effect_Lib check_Effect_Lib { get; set; }
        [Inject] public ISw_Files_Lib files_Lib { get; set; } // 파일 첨부
        #endregion

        #region 목록
        Referral_career_Entity cnn { get; set; } = new Referral_career_Entity();
        List<Check_Object_Entity> coe { get; set; } = new List<Check_Object_Entity>();
        List<Check_Cycle_Entity> cce { get; set; } = new List<Check_Cycle_Entity>();

        List<Check_Input_Entity> cine { get; set; } = new List<Check_Input_Entity>();
        Check_Input_Entity ann { get; set; } = new Check_Input_Entity();

        /// <summary>
        /// 점검 사항 목록
        /// </summary>
        List<Check_Items_Entity> cite { get; set; } = new List<Check_Items_Entity>();
        Check_Items_Entity cite_A { get; set; } = new Check_Items_Entity();

        /// <summary>
        /// 점검된 목록
        /// </summary>
        List<Check_List_Entity> clie { get; set; } = new List<Check_List_Entity>();


        /// <summary>
        /// 점검된 내용 속성
        /// </summary>
        Check_List_Entity bnn { get; set; } = new Check_List_Entity();

        Sw_Files_Entity dnn { get; set; } = new Sw_Files_Entity();
        List<Sw_Files_Entity> Files_Entity { get; set; } = new List<Sw_Files_Entity>();
        #endregion

        #region 변수
        public string User_Code { get; set; }
        public string User_Name { get; set; }
        public string Apt_Code { get; set; }
        public string Apt_Name { get; set; }
        public string UserName { get; set; }

        public string FileViews { get; set; } = "A";
        public string FileInsertViews { get; set; } = "A";
        public int Files_Count { get; set; } = 0;
        public int intAid { get; set; } = 0;

        public string InsertViews { get; set; } = "A";
        public string EditViews { get; set; } = "A";

        public string strObject_Code { get; set; }
        public string strObject_Name { get; set; }
        public string strCycle_Code { get; set; }
        public string strCycle_Name { get; set; }
        public int strItems_being { get; set; } = 0;
        public int strList_being { get; set; } = 0;

        public DateTime dbDate { get; set; } = DateTime.Now;
        public string arg { get; set; }

        protected DulPager.DulPagerBase pager = new DulPager.DulPagerBase()
        {
            PageNumber = 1,
            PageIndex = 0,
            PageSize = 10,
            PagerButtonCount = 5
        };

        protected DulPager.DulPagerBase pagerA = new DulPager.DulPagerBase()
        {
            PageNumber = 1,
            PageIndex = 0,
            PageSize = 10,
            PagerButtonCount = 5
        };

        public string strWSortB { get; set; } = "Z";
        public string strWSortC { get; set; } = "Z";
        public string strWSortD { get; set; } = "Z";
        #endregion

        /// <summary>
        /// 로드시 실행
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

                cnn = await referral_Career_Lib.Detail(User_Code);
                //PostDuty = cnn.Post + cnn.Duty;


                coe = await check_Object_Lib.CheckObject_Data_Index();//점검시설물 목록
                cce = await check_Cycle_Lib.CheckCycle_Data_Index(); //점검주기 목록                


            }
            else
            {
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
        private void onObject(ChangeEventArgs args)
        {
            strObject_Code = args.Value.ToString();
            strCycle_Code = "Z";
            //StateHasChanged();
        }

        /// <summary>
        /// 점검 주기선택
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        private async Task OnCheck(ChangeEventArgs args)
        {
            arg = args.Value.ToString();
            if (strObject_Code == null || strObject_Code == "Z")
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "점검대상을 선택하지 않았습니다..");
            }
            else
            {
                strCycle_Code = arg;
                strCycle_Name = await check_Cycle_Lib.CheckCycle_Data_Name(strCycle_Code);
                pager.PageIndex = 0;
                pagerA.PageIndex = 0;

                strItems_being = await check_Items_Lib.CheckItems_View_Data_Count(strObject_Code, arg);
                strList_being = await check_List_Lib.GetCheckList_List_Index_Count(strObject_Code, arg, dbDate.Year.ToString(), dbDate.Month.ToString(), dbDate.Day.ToString(), Apt_Code);

                await DisplayData(arg);
                await DisplayDataZ(arg);
                StateHasChanged();
            }
        }

        /// <summary>
        /// 점검 목록 데이터
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        private async Task DisplayData(string args)
        {
            pager.RecordCount = await check_Items_Lib.CheckItems_Data_Index_Count(strObject_Code, strCycle_Code);
            cite = await check_Items_Lib.CheckItems_Data_Join_Index(pager.PageIndex, strObject_Code, args);
            StateHasChanged();
        }

        /// <summary>
        /// 점검 목록 데이터
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        private async Task DisplayDataZ(string args)
        {
            pagerA.RecordCount = await check_List_Lib.GetCheckList_List_Index_Count_Page(strObject_Code, args, dbDate.Year.ToString(), dbDate.Month.ToString(), dbDate.Day.ToString(), Apt_Code);
            clie = await check_List_Lib.GetCheckList_List_Index_Page(pagerA.PageIndex, strObject_Code, args, dbDate.Year.ToString(), dbDate.Month.ToString(), dbDate.Day.ToString(), Apt_Code);
            StateHasChanged();
        }


        #region 점검 결과 출력 메서드
        public string FuncEffect_A(object objEffectCode, string strCycleCode, string AptCode)
        {
            string strView;
            string strYear = DateTime.Now.Year.ToString();
            string strMonth = DateTime.Now.Month.ToString();
            string strDay = DateTime.Now.Day.ToString();
            string strItemsCode_A = objEffectCode.ToString();
            string strCycleCode_A = strCycleCode;
            ///ViewState["Check_Cycle_Code"].ToString();
            Check_List_Entity ann = check_List_Lib.CheckList_Data_Effect_A(strItemsCode_A, strCycleCode, strYear, strMonth, strDay, AptCode);

            int code = check_List_Lib.CheckList_Data_Effect_A_Count(strItemsCode_A, strCycleCode, strYear, strMonth, strDay, AptCode);

            if (code > 0)
            {
                return strView = check_Effect_Lib.CheckEffect_Data_Name(ann.Check_Effect_Code);

            }
            else
            {
                strView = "미점검";
                return strView;
            }

            //strView = (new Check_Ds.Check_Effect_Ds()).CheckEffect_Name_Ds(objEffectCode.ToString());
        }
        #endregion        


        /// <summary>
        /// 페이징
        /// </summary>
        /// <param name="pageIndex"></param>
        protected async void PageIndexChanged(int pageIndex)
        {
            pager.PageIndex = pageIndex;
            pager.PageNumber = pageIndex + 1;

            await DisplayData(arg);

            StateHasChanged();
        }

        /// <summary>
        /// 페이징
        /// </summary>
        /// <param name="pageIndex"></param>
        protected async void PageIndexChangedA(int pageIndex)
        {
            pagerA.PageIndex = pageIndex;
            pagerA.PageNumber = pageIndex + 1;

            await DisplayDataZ(arg);

            StateHasChanged();
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
                dnn.Parents_Num = intAid.ToString(); // 선택한 ParentId 값 가져오기 
                dnn.Sub_Num = dnn.Parents_Num;
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

                    dnn.Sw_FileName = fileName;
                    dnn.Sw_FileSize = Convert.ToInt32(file.Size);
                    dnn.Parents_Name = "Check";
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
                    await check_List_Lib.Files_Count_Add(intAid, "A"); // 파일 입력된 수 수정
                }
                catch (Exception ex)
                {
                    Logger.LogError("File: {Filename} Error: {Error}",
                        file.Name, ex.Message);
                }
            }

            await DisplayData(strCycle_Code);
            await DisplayDataZ(strCycle_Code);
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
        /// 파일 보기 닫기
        /// </summary>
        private void FilesViewsClose()
        {
            FileViews = "A";
            //StateHasChanged();
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

        #region 첨부파일 보기
        private async Task FiledByViews(int Aid)
        {
            Files_Count = await files_Lib.Sw_Files_Data_Index_Count("Check", Aid.ToString(), Apt_Code);

            if (Files_Count > 0)
            {
                Files_Entity = await files_Lib.Sw_Files_Data_Index("Check", Aid.ToString(), Apt_Code);
            }
        } 
        #endregion

        #region 점검입력 함수

        /// <summary>
        /// 점검내용 입력 열기
        /// </summary>
        /// <param name="items_Entity"></param>
        private async Task ByAid(int Aid)
        {
            cite_A = await check_Items_Lib.CheckItems_Data_Details(Aid);
            strObject_Name = cite_A.Check_Object_Name;
            strCycle_Name = cite_A.Check_Cycle_Name;

            InsertViews = "B";
            //StateHasChanged();
        }

        /// <summary>
        /// 입력 닫기
        /// </summary>
        private void ViewsClose()
        {
            InsertViews = "A";
            //StateHasChanged();
        }

        /// <summary>
        /// 점검내용 입력
        /// </summary>
        /// <returns></returns>
        public async Task btnSave()
        {
            int InputBeing = 0;
            try
            {
                InputBeing = await check_Input_Lib.CheckInput_Data_View_Year_Month_Day_Aid(strObject_Code, strCycle_Code, dbDate.Year.ToString(), dbDate.Month.ToString(), dbDate.Day.ToString(), Apt_Code);
            }
            catch (Exception)
            {
                InputBeing = 0;
            }

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

            if (InputBeing < 1)
            {
                ann.AptCode = Apt_Code;
                ann.Check_Count = 0;
                ann.Check_Cycle_Code = strCycle_Code;
                ann.Check_Day = dbDate.Day.ToString();
                //ann.
                ann.Check_Effect_Code = bnn.Check_Effect_Code;
                ann.Check_Items_Code = cite_A.Check_Items_Code;
                ann.Check_Month = dbDate.Month.ToString();
                ann.Check_Object_Code = strObject_Code;
                ann.Check_Year = dbDate.Year.ToString();
                ann.User_Duty = cnn.Duty;
                ann.User_Name = User_Name;
                ann.User_Post = cnn.Post;
                ann.PostIP = bnn.PostIP;

                InputBeing = await check_Input_Lib.CheckInput_Date_Insert(ann);
            }



            #region 아이피 입력


            if (InputBeing < 1)
            {

                ann.AptCode = Apt_Code;
                ann.Check_Cycle_Code = strCycle_Code;
                ann.Check_Day = dbDate.Day.ToString();
                ann.Check_Month = dbDate.Month.ToString();
                ann.Check_Year = dbDate.Year.ToString();
                ann.Check_Effect_Code = bnn.Check_Effect_Code;
                ann.Check_Items_Code = cite_A.Check_Items_Code;
                ann.Check_Object_Code = strObject_Code;
                ann.Check_Effect_Code = bnn.Check_Effect_Code;
                ann.Check_Year = dbDate.Year.ToString();
                ann.Complete = "A";
                ann.PostIP = bnn.PostIP;
                ann.User_Duty = cnn.Duty;
                ann.User_Name = User_Name;
                ann.User_Post = cnn.Post;

                InputBeing = await check_Input_Lib.CheckInput_Date_Insert(ann);
            }

            #endregion 점검내용 등록
            bnn.AptCode = Apt_Code;
            bnn.Check_Day = dbDate.Day.ToString();
            bnn.Check_Year = dbDate.Year.ToString();
            bnn.Check_Month = dbDate.Month.ToString();
            bnn.Check_Date = dbDate;
            bnn.Check_Cycle_Code = arg;
            bnn.Check_Object_Code = strObject_Code;
            bnn.Check_Items_Code = cite_A.Check_Items_Code;
            bnn.Check_Input_Code = InputBeing.ToString();
            bnn.UserDuty = cnn.Duty;
            bnn.UserPost = cnn.Post;
            bnn.UserName = User_Name;
            bnn.UserID = User_Code;
            bnn.FileSize = 0;
            bnn.Check_Hour = dbDate.Hour.ToString();
            bnn.Check_Input_Code = InputBeing.ToString();

            if (bnn.CheckID < 1)
            {
                await check_List_Lib.CheckList_Date_Insert(bnn); //점검 내용 입력
                await check_Input_Lib.Check_Count_Add(InputBeing, "A");// 점검수 수정 
                //await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "점검 내용이 등록되었습니다...");
            }
            else
            {
                await check_List_Lib.CheckList_Date_Insert(bnn); //점검 내용 입력
                                                                 //await check_Input_Lib.Check_Count_Add(InputBeing, "A");// 점검수 수정 
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "점검 내용이 수정되었습니다...");
            }

            InsertViews = "A";
            ann = new Check_Input_Entity();
            bnn = new Check_List_Entity();
            //clie = await check_List_Lib.GetCheckList_List_Index_new(strObject_Code, strCycle_Code, dbDate.Year.ToString(), dbDate.Month.ToString(), dbDate.Day.ToString(), Apt_Code);

            strList_being = await check_List_Lib.GetCheckList_List_Index_Count(strObject_Code, arg, dbDate.Year.ToString(), dbDate.Month.ToString(), dbDate.Day.ToString(), Apt_Code);

            await DisplayData(arg);
            await DisplayDataZ(arg);
        }

        /// <summary>
        /// 점검 결과 선택하면 실행
        /// </summary>
        /// <param name="args"></param>
        private void onEffect(ChangeEventArgs args)
        {
            string strS = args.Value.ToString();
            bnn.Check_Effect_Code = strS;
            if (strS == "itmCode9")
            {
                bnn.Check_Details = "양호 - 이상 없음.";
            }
            else if (strS == "itmCode8")
            {
                bnn.Check_Details = "요주의 - 해당 시설물의 내구연한이 다 되었습니다.";
            }
            else if (strS == "itmCode7")
            {
                bnn.Check_Details = "요수리 - 해당 시설물 점검 결과 이상이 발견되어 보수 또는 교체할 사항을 반드시 입력하세요.";
            }
            else if (strS == "iemCode6")
            {
                bnn.Check_Details = "이용금지 - 해당 시설물 점검결과 안전에 위험이 있을 것으로 예상되어 사용을 금지합니다.";
            }
            else if (strS == "itmCode11")
            {
                bnn.Check_Details = "적합(O) - 점검결과 규정에 적합함.";
            }
            else if (strS == "itmCode12")
            {
                bnn.Check_Details = "부적합(X) - 점검결과 규정에 부적합 보수 또는 교체를 요함.";
            }
            else
            {
                bnn.Check_Details = "해당 시설물은 해당 사항없음.";
            }
        }

        /// <summary>
        /// 점검 내용 삭제
        /// </summary>
        /// <param name="st"></param>
        private async Task btnRemove(Check_List_Entity st)
        {
            bool isDelete = await JSRuntime.InvokeAsync<bool>("confirm", $"{st.Check_Object_Name}을 정말로 삭제하시겠습니까?");

            if (isDelete)
            {
                //await check_Input_Lib.CheckInput_Date_Delete(st.CheckID);
                await check_List_Lib.CheckList_Date_Remove(st.CheckID);
                await check_Input_Lib.Check_Count_Add(Convert.ToInt32(st.Check_Input_Code), "B");

                await DisplayData(st.Check_Cycle_Code);
                await DisplayDataZ(st.Check_Cycle_Code);
            }
            else
            {
                await JSRuntime.InvokeAsync<object>("alert", "취소되었습니다.");
            }
        }

        /// <summary>
        /// 점검 내용 수정
        /// </summary>
        /// <param name="st"></param>
        private async Task btnEdit(Check_List_Entity st)
        {
            bnn = st;
            dbDate = st.Check_Date;
            cite_A.Check_Items = await check_Items_Lib.CheckItems_Data_Name(st.Check_Items_Code);

            strCycle_Name = await check_Cycle_Lib.CheckCycle_Data_Name(st.Check_Cycle_Code);
            strObject_Name = await check_Object_Lib.CheckObject_Data_Name_Async(st.Check_Object_Code);
            InsertViews = "B";
            await DisplayData(st.Check_Cycle_Code);
            await DisplayDataZ(st.Check_Cycle_Code);

        }



        #endregion

        
    }
}
