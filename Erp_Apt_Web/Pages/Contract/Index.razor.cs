using Company;
using Erp_Apt_Lib;
using Erp_Apt_Lib.Check;
using Erp_Apt_Staff;
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

namespace Erp_Apt_Web.Pages.Contract
{
    public partial class Index
    {
        #region 인스턴스
        [CascadingParameter] public Task<AuthenticationState> AuthenticationStateRef { get; set; }
        [Inject] public NavigationManager MyNav { get; set; } // Url 
        [Inject] IJSRuntime JSRuntime { get; set; }
        [Inject] public IStaff_Lib staff_Lib { get; set; }
        [Inject] public ISido_Lib sido_Lib { get; set; }
        [Inject] public ICompany_Lib company_Lib { get; set; }
        [Inject] public ICompany_Sub_Lib company_Sub_Lib { get; set; }
        [Inject] public ICompany_Join_Lib company_Join_Lib { get; set; }
        [Inject] public ICompany_Apt_Career_Lib company_Career_Lib { get; set; }
        [Inject] public IApt_Lib apt_Lib { get; set; }
        [Inject] public IContract_Sort_Lib contract_Sort_Lib { get; set; }

        [Inject] public ISw_Files_Lib files_Lib { get; set; } // 파일 첨부
        #endregion

        #region 변수
        public string User_Code { get; set; }
        public string User_Name { get; set; }
        public string Apt_Code { get; set; }
        public string Apt_Name { get; set; }
        public int LevelCount { get; set; } = 0;
        public string Views { get; set; } = "A"; //상세 열기
        public string RemoveViews { get; set; } = "A";//삭제 열기
        public string InsertViews { get; set; } = "A"; // 계약입력 열기
        public string ComInsertViews { get; set; } = "A"; //업체입력 열기
        public string SortA { get; set; }
        public string CorporateResistration_Num { get; set; }
        
        public string Sido { get; set; }
        private ElementReference myref;
        
        public string FileInsertViews { get; set; } = "A";
        public int Files_Count { get; set; } = 0;
        public string FileViews { get; set; } = "A";
        #endregion

        #region 속성
        Staff_Career_Entity rnn { get; set; } = new Staff_Career_Entity();
        List<Company_Career_Entity> ann { get; set; } = new List<Company_Career_Entity>();
        public int intNum { get; private set; }
        Company_Career_Entity cce { get; set; } = new Company_Career_Entity();
        Company_Entity company_Entity { get; set; } = new Company_Entity();
        Company_Sub_Entity company_Sub_Entity { get; set; } = new Company_Sub_Entity();
        Company_Entity lstB { get; set; } = new Company_Entity();
        Company_Sub_Entity LstC { get; set; } = new Company_Sub_Entity();
        List<Contract_Sort_Entity> listA { get; set; } = new List<Contract_Sort_Entity>();
        List<Contract_Sort_Entity> listB { get; set; } = new List<Contract_Sort_Entity>();
        Company_Join_Entity cje { get; set; } = new Company_Join_Entity();
        Contract_Sort_Entity Sort_Entity { get; set; } = new Contract_Sort_Entity();
        List<Sido_Entity> sidos { get; set; } = new List<Sido_Entity>();
        List<Sw_Files_Entity> Files_Entity { get; set; } = new List<Sw_Files_Entity>();
        Sw_Files_Entity dnn { get; set; } = new Sw_Files_Entity();
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

        /// <summary>
        /// 배치정보 목록 불러오기
        /// </summary>
        private async Task DisplayData()
        {
            pager.RecordCount = await company_Career_Lib.Getcount_option("Apt_Code", Apt_Code);
            ann = await company_Career_Lib.getlist_option(pager.PageIndex, "Apt_Code", Apt_Code);
            intNum = pager.RecordCount - (pager.PageIndex * pager.PageSize);
            //StateHasChanged();
        }

        /// <summary>
        /// 계약정보 입력 열기
        /// </summary>
        private async Task btnInsertContract()
        {
            InsertViews = "B";
            cce.Contract_end_date = DateTime.Now;
            cce.Contract_start_date = DateTime.Now;
            cce.Contract_Sum = 1000;
            listA = await contract_Sort_Lib.List_All("A");//계약 대분류 만들기
            //StateHasChanged();
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
        /// 상세보기
        /// </summary>
        /// <param name="entity"></param>
        private void ByDetails(Company_Career_Entity entity)
        {
            Views = "B";
            cce = entity;
            //StateHasChanged();
        }

        /// <summary>
        /// 계약 수정
        /// </summary>
        /// <param name="entity"></param>
        private void ByEdit(Company_Career_Entity entity)
        {
            InsertViews = "B";
            cce = entity;
            //StateHasChanged();
        }

        /// <summary>
        /// 계약 삭제
        /// </summary>
        private async Task ByRemove(int Aid)
        {
            bool isDelete = await JSRuntime.InvokeAsync<bool>("confirm", $"{Aid}번 계약을 정말로 삭제하시겠습니까?");

            if (isDelete)
            {
                await company_Career_Lib.delete(Aid.ToString(), "D");
            }

            await DisplayData();

            StateHasChanged();
        }

        /// <summary>
        /// 계약 대분류 선택 시
        /// </summary>
        private async Task onSortA(ChangeEventArgs args)
        {
            listB = await contract_Sort_Lib.List_Code("B", args.Value.ToString());
            //StateHasChanged();
        }

        /// <summary>
        /// 사업자 번호 검색
        /// </summary>
        /// <returns></returns>
        public string cdn { get; private set; }
        private async Task OnRepeatCheck(ChangeEventArgs a)
        {
            cdn = a.Value.ToString();
            cdn = cdn.Replace(" ", "").Replace("-", "").Replace("_", "");
            int intRe = await company_Lib.CorNum_Being(cdn);
            if (intRe > 0)
            {
                company_Entity = await company_Lib.CorNum_Detail(cdn);
                company_Sub_Entity = await company_Sub_Lib.Detail(company_Entity.Cor_Code);

                cce.Company_Name = company_Entity.Cor_Name;
                cce.Cor_Code = company_Entity.Cor_Code;
            }
            else
            {
                await JSRuntime.InvokeAsync<object>("alert", "등록되지 않은 사업자 번호 입니다. \n업체등록으로 이동합니다.");
                lstB.CorporateResistration_Num = cdn;
                InsertViews = "A";
                ComInsertViews = "B";
            }
        }

        /// <summary>
        /// 계약정보 등록
        /// </summary>
        /// <returns></returns>
        public async Task btnContractSave()
        {
            DateTime date = Convert.ToDateTime("2011-01-01");
            if (cce.ContractMainAgent == "" || cce.ContractMainAgent == null)
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "계약주체를 선택하지 않았습니다..");
            }
            else if (cce.ContractSort == "" || cce.ContractSort == null)
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "계약분류를 선택하지 않았습니다..");
            }
            else if (cce.Bid == "" || cce.Bid == null)
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "낙찰방법을 선택하지 않았습니다..");
            }
            else if (cce.Tender == "" || cce.Tender == null)
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "입찰방법을 선택하지 않았습니다..");
            }
            else if (cce.Contract_Sum < 50000)
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "계약금액을 입력하지 않았습니다..");
            }
            else if (cce.Contract_start_date < date )
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "계약시작일을 입력하지 않았습니다..");
            }
            else if (cce.Contract_end_date < date)
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "계약종료일을 입력하지 않았습니다..");
            }
            else if (cce.Company_Name == "" || cce.Company_Name == null)
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "업체명을 입력하지 않았습니다..");
            }
            else
            {
                cce.Apt_Code = Apt_Code;
                cce.Apt_Name = Apt_Name;
                cce.Contract_date = cce.Contract_start_date;
                cce.Division = "A";
                string Period = Func_span(cce.Contract_start_date, cce.Contract_end_date).Replace(",", "");
                cce.Contract_Period = Convert.ToInt32(Period);
                cce.CC_Code = Apt_Code + "_" + DateTimeOffset.Now.ToUnixTimeSeconds();

                if (cce.Aid < 1)
                {
                    await company_Career_Lib.add(cce);
                }
                else
                {
                    await company_Career_Lib.edit(cce);
                }

                cce = new Company_Career_Entity();

                InsertViews = "A";

                await DisplayData();
                //StateHasChanged();
            }
            
        }

        /// <summary>
        /// 계약정보 닫기
        /// </summary>
        private void btnContractClose()
        {
            InsertViews = "A";
            //StateHasChanged();
        }

        /// <summary>
        ///  계약일 수 계산
        /// </summary>
        protected string Func_span(object objstart, object objend)
        {
            int ddate = 0;
            DateTime date = Convert.ToDateTime(objend);
            if (date.Year == 0001)
            {
                DateTime start = Convert.ToDateTime(objstart);
                //DateTime end = Convert.ToDateTime(objend);
                string start_a = start.ToShortDateString();
                string end_a = DateTime.Now.ToShortDateString();   //end.ToShortDateString();
                ddate = company_Career_Lib.Date_scomp(start_a, end_a);
            }
            else
            {
                DateTime start = Convert.ToDateTime(objstart);
                DateTime end = Convert.ToDateTime(objend);
                string start_a = start.ToShortDateString();
                string end_a = end.ToShortDateString(); //DateTime.Now.ToShortDateString();
                ddate = company_Career_Lib.Date_scomp(start_a, end_a);
            }

            return string.Format("{0: ###,###}", ddate);
        }

        /// <summary>
        /// 업체 등록 열기
        /// </summary>
        private async Task btnInsertCompany()
        {
            listA = await contract_Sort_Lib.List_All("A");//계약 대분류 만들기
            ComInsertViews = "B";
            LstC.CraditRating = "B";
            //StateHasChanged();
        }

        /// <summary>
        /// 사업자 번호 중복 체크
        /// </summary>
        protected async Task OnRepeatCheckA()
        {
            if (lstB.CorporateResistration_Num == "" || lstB.CorporateResistration_Num == null)
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "사업자 등록 번호를 입력해 주세요...");
                await JSRuntime.InvokeVoidAsync("SetFocusToElement", myref);

            }
            else
            {
                lstB.CorporateResistration_Num = lstB.CorporateResistration_Num.Replace("-", "").Replace(" ", "");
                int intR = await company_Lib.CorNum_Being(lstB.CorporateResistration_Num.ToString());

                //string strResult = "";
                //List<SelectListItem> licities = new List<SelectListItem>();

                bool tr = checkCpIdenty(lstB.CorporateResistration_Num);

                //licities.Add(new SelectListItem { Text = "::분류선택::", Value = "0" });
                if (tr == true)
                {
                    if (intR > 0)
                    {
                        await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", lstB.CorporateResistration_Num + "는 이미 입력된 사업자 등록 번호 입니다. 다시 입력해 주세요...");
                        lstB.CorporateResistration_Num = "";
                    }
                }
                else
                {
                    await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", lstB.CorporateResistration_Num + "는 잘못된 사업자 등록 번호 입니다. 다시 입력해 주세요...");
                    lstB.CorporateResistration_Num = "";
                }
            }
        }


        /// <summary>
        /// 사업자번호 체크
        /// </summary>
        public bool checkCpIdenty(string cpNum)
        {
            cpNum = cpNum.Replace("-", "");
            if (cpNum.Length != 10)
            {
                return false;
            }
            int sum = 0;
            string checkNo = "137137135";

            // 1
            for (int i = 0; i < checkNo.Length; i++)
            {
                sum += (int)Char.GetNumericValue(cpNum[i]) * (int)Char.GetNumericValue(checkNo[i]);
            }

            // 2
            sum += (int)Char.GetNumericValue(cpNum[8]) * 5 / 10;

            // 3
            sum %= 10;

            // 4
            if (sum != 0)
            {
                sum = 10 - sum;
            }

            // 5
            if (sum != (int)Char.GetNumericValue(cpNum[9]))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// 시도 선택 시군구 실행
        /// </summary>
        public async Task OnSido(ChangeEventArgs args)
        {
            lstB.Adress_Sido = await sido_Lib.SidoName(args.Value.ToString());
            LstC.Sido = lstB.Adress_Sido;
            sidos = await sido_Lib.GetList_Code(args.Value.ToString());
            //StateHasChanged();
        }

        /// <summary>
        /// 시군구 선택 실행
        /// </summary>
        /// <param name="args"></param>
        string strCoding = null;
        public async void onGunGu(ChangeEventArgs args)
        {
            lstB.Adress_GunGu = args.Value.ToString();
            LstC.GunGu = lstB.Adress_GunGu;
            strCoding = await sido_Lib.Region_Code(args.Value.ToString());
            //StateHasChanged();
        }

        /// <summary>
        /// 업체 정보 등록
        /// </summary>
        /// <returns></returns>
        public async Task btnCompanySave()
        {
            lstB.Cor_Code = strCoding + await company_Lib.Num_Count();
            lstB.User_Code = User_Code;
            
            if (LstC.Company_Sort == "" || LstC.Company_Sort == null)
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "업체분류를 선택하지 않았습니다..");
            }
            else if (lstB.Cor_Code == "" || lstB.Cor_Code == null)
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "입체코드를 입력하지 않았습니다..");
            }
            else if (lstB.Cor_Name == "" || lstB.Cor_Name == null)
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "입체명을 입력하지 않았습니다..");
            }
            else if (lstB.Adress_Sido == "" || lstB.Adress_Sido == null)
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "시도를 선택하지 않았습니다..");
            }
            else if (lstB.Adress_GunGu == "" || lstB.Adress_GunGu == null)
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "시군구를 입력하지 않았습니다..");
            }
            else if (lstB.CorporateResistration_Num == "" || lstB.CorporateResistration_Num == null)
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "사업자 등록 번호를 입력하지 않았습니다..");
            }
            else if (lstB.Cor_Name == "" || lstB.Cor_Name == null)
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "대표자를 입력하지 않았습니다..");
            }
            else if (LstC.ChargeMan_Mobile == "" || LstC.ChargeMan_Mobile == null)
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "담당자 휴대전화 연락처를 입력하지 않았습니다..");
            }
            else if (LstC.TypeOfBusiness == "" || LstC.TypeOfBusiness == null)
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "업종을 입력하지 않았습니다..");
            }
            else if (LstC.BusinessConditions == "" || LstC.BusinessConditions == null)
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "업태를 입력하지 않았습니다..");
            }
            else if (LstC.ChargeMan == "" || LstC.ChargeMan == null)
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "담당자를 입력하지 않았습니다..");
            }
            else
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
                lstB.PostIP = myIPAddress;
                lstB.LevelCount = 3;
                #endregion
                lstB.Intro = LstC.Etc;
                lstB.LevelCount = 3;
                lstB.Intro = LstC.Etc;
                await company_Lib.Add(lstB);

                try
                {
                    LstC.Adress = lstB.Adress_Rest;
                    LstC.Ceo_Mobile = lstB.Mobile;
                    LstC.Ceo_Name = lstB.Ceo_Name;
                    LstC.Telephone = lstB.Telephone;
                    LstC.PostIP = lstB.PostIP;

                    LstC.Company_Code = lstB.Cor_Code;
                    LstC.Sido = lstB.Adress_Sido;
                    LstC.GunGu = lstB.Adress_GunGu;
                    
                    await company_Sub_Lib.Add(LstC);

                    Sort_Entity = await contract_Sort_Lib.Detail(LstC.Company_Sort);

                    cce.ContractSort = Sort_Entity.ContractSort_Code;
                    SortA = Sort_Entity.Up_Code;
                    cce.Company_Name = lstB.Cor_Name;
                    cce.CC_Code = lstB.Cor_Code;
                    CorporateResistration_Num = lstB.CorporateResistration_Num;
                    await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", lstB.Cor_Name + "의 등록이 완료되었습니다...");
                   

                    ComInsertViews = "A";
                    InsertViews = "B";

                    
                    //StateHasChanged();
                }
                catch (Exception)
                {
                    await company_Lib.Delete(lstB.Cor_Code);
                    await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", lstB.Cor_Name + "의 등록에 실패했습니다...");
                } 
            }            
        }

        /// <summary>
        /// 업체 등록
        /// </summary>
        private void btnClose()
        {
            ComInsertViews = "A";
            //StateHasChanged();
        }

        /// <summary>
        /// 파일 첨부 버튼
        /// </summary>
        private void btnFilesInsert()
        {
            FileInsertViews = "B";
        }

        /// <summary>
        /// 파일 첨부 열기
        /// </summary>
        protected void FiledBy(int Num)
        {
            FileInsertViews = "B";
            //StateHasChanged();
        }

        /// <summary>
        /// 첨부된 사진 보기
        /// </summary>
        protected async Task FileViewsBy(int Aid)
        {
            FileViews = "B";
            Files_Count = await files_Lib.Sw_Files_Data_Index_Count("Contract", Aid.ToString(), Apt_Code);
            if (Files_Count > 0)
            {
                Files_Entity = await files_Lib.Sw_Files_Data_Index("Contract", Aid.ToString(), Apt_Code);
            }
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

        /// <summary>
        /// 파일 입력 닫기
        /// </summary>
        protected void FilesClose()
        {
            FileInsertViews = "A";
        }

        /// <summary>
        /// 상세보기 닫기 버튼
        /// </summary>
        private void btnViewsClose()
        {
            Views = "A";
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
                dnn.Parents_Num = cce.Aid.ToString(); // 선택한 ParentId 값 가져오기 
                dnn.Sub_Num = dnn.Parents_Num;
                try
                {
                    var pathA = $"{env.WebRootPath}\\UpFiles\\Contract";

                    string strFiles = Path.GetExtension(file.Name);

                    string _FileName = "Contract" + Apt_Code + DateTime.Now.ToShortDateString() + strFiles;

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
                    dnn.Parents_Name = "Contract";
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
                }
                catch (Exception ex)
                {
                    Logger.LogError("File: {Filename} Error: {Error}",
                        file.Name, ex.Message);
                }
            }

            await DisplayData();
            FileInsertViews = "A";

            Files_Count = await files_Lib.Sw_Files_Data_Index_Count("Contract", cce.Aid.ToString(), Apt_Code);
            if (Files_Count > 0)
            {
                Files_Entity = await files_Lib.Sw_Files_Data_Index("Contract", cce.Aid.ToString(), Apt_Code);
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
                    string rootFolder = $"{env.WebRootPath}\\UpFiles\\Contract\\{ _files.Sw_FileName}";
                    File.Delete(rootFolder);
                }
                await files_Lib.FileRemove(_files.Num.ToString(), "Contract", Apt_Code);
                int ab = Convert.ToInt32(_files.Parents_Num.ToString());
                await company_Career_Lib.File_Minus(ab); // 첨부파일 추가를 db 저장(문서관리, Document)

                Files_Count = await files_Lib.Sw_Files_Data_Index_Count("Contract", cce.Aid.ToString(), Apt_Code);
                if (Files_Count > 0)
                {
                    Files_Entity = await files_Lib.Sw_Files_Data_Index("Contract", cce.Aid.ToString(), Apt_Code);
                }
            }
            else
            {
                await JSRuntime.InvokeAsync<object>("alert", "취소되었습니다.");
            }
        }

        /// <summary>
        /// 파일보기 닫기
        /// </summary>
        private void FilesViewsClose()
        {
            FileViews = "A";
        }
    }
}
