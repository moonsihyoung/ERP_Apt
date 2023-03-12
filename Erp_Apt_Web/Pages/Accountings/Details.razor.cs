using Erp_Apt_Lib;
using Erp_Apt_Lib.Accounting;
using Erp_Apt_Lib.Decision;
using Erp_Apt_Lib.Logs;
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
using System.Net;
using System.Net.NetworkInformation;
using System.Reflection.Emit;
using System.Threading.Tasks;

namespace Erp_Apt_Web.Pages.Accountings
{
    public partial class Details
    {
        #region 인스턴스
        [Parameter] public int Aid { get; set; }
        [CascadingParameter] public Task<AuthenticationState> AuthenticationStateRef { get; set; }
        [Inject] public NavigationManager MyNav { get; set; } // Url 
        [Inject] IJSRuntime JSRuntime { get; set; }
        [Inject] ILogs_Lib logs_Lib { get; set; }
        [Inject] public IDecision_Lib decision_Lib { get; set; } // 결재정보
        [Inject] public IAccount_Lib account_Lib { get; set; }
        [Inject] public IDisbursement_Lib disbursement_Lib { get; set; }
        [Inject] public ISw_Files_Lib files_Lib { get; set; }
        [Inject] public IPost_Lib Post_Lib { get; set; }
        [Inject] public IReferral_career_Lib referral_Career_Lib { get; set; }
        [Inject] public IDisbursementSort_Lib disbursementSort_Lib { get; set; }
        [Inject] public IAccountSort_Lib accountSort_Lib { get; set; }
        [Inject] public IAccountDeals_Lib accountDeals_Lib { get; set; }
        [Inject] public IBankAccount_Lib bankAccount_Lib { get; set; }
        [Inject] public IBankAccountDeals_Lib bankAccountDeals_Lib { get; set; }
        [Inject] public IStaff_Lib staff_Lib { get; set; }


        #endregion

        #region 속성
        List<AccountDealsEntity> ann { get; set; } = new List<AccountDealsEntity>();
        List<AccountDealsEntity> annA { get; set; } = new List<AccountDealsEntity>();
        DisbursementEntity cnn { get; set; } = new DisbursementEntity();
        AccountDealsEntity bnn { get; set; } = new AccountDealsEntity();
        List<Approval_Entity> app { get; set; } = new List<Approval_Entity>();
        //Sw_Files_Entity dnn { get; set; } = new Sw_Files_Entity(); // 첨부 파일 정보
        Sw_Files_Entity finn { get; set; } = new Sw_Files_Entity();
        List<Sw_Files_Entity> Files_Entity { get; set; } = new List<Sw_Files_Entity>();
        List<Post_Entity> pnn { get; set; } = new List<Post_Entity>();
        Referral_career_Entity cnnA { get; set; } = new Referral_career_Entity();
        List<DisbursementSortEnity> dsort { get; set; } = new List<DisbursementSortEnity>();
        List<AccountEntity> acc { get; set; } = new List<AccountEntity>();
        List<AccountEntity> accT { get; set; } = new List<AccountEntity>();
        List<BankAccountEntity> bank { get; set; } = new List<BankAccountEntity>();
        BankAccountEntity bankA { get; set; } = new BankAccountEntity();
        List<BankAccountDealsEntity> bankAList { get; set; } = new List<BankAccountDealsEntity>();
        BankAccountDealsEntity bankAA { get; set; } = new BankAccountDealsEntity();
        List<BankAccountDealsEntity> bankD { get; set; } = new List<BankAccountDealsEntity>();
        AccountDealsEntity accountDealsEntity { get; set; } = new AccountDealsEntity();
        //List<AccountDealsEntity> accountDealsEntities { get; set; } = new List<AccountDealsEntity>();
        List<AccountSortEntity> accA { get; set; } = new List<AccountSortEntity>();
        List<AccountSortEntity> accB { get; set; } = new List<AccountSortEntity>();
        Referral_career_Entity referral { get; set; } = new Referral_career_Entity();
        #endregion

        #region 변수
        public string User_Code { get; set; }
        public string User_Name { get; set; }
        public string Apt_Code { get; set; }
        public string Apt_Name { get; set; }
        public int LevelCount { get; set; } = 0;
        public string PostDuty { get; set; }
        public string strRegion { get; set; }
        public string strSido { get; set; }
        public string Views { get; set; } = "A"; //상세 열기
        
        public string strTitle { get; set; }
        public string CorporateResistration_Num { get; set; } //사업자 번호
        public string strSearchs { get; set; }
        public string strQuery { get; set; }
        public int intNum { get; set; }

        public string FileInputViews { get; set; } = "A";
        public string FileViews { get; set; } = "A";
        public int Files_Count { get; set; } = 0;
        public string InsertViews { get; private set; }

        public string InsertBanks { get; set; } = "A";
        public string InsertBankDeals { get; set; } = "A";
        #endregion

        #region 페이징
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
        /// 로그입력
        /// </summary>
        Logs_Entites logs { get; set; } = new Logs_Entites();
        protected async Task Loks(string Note, string Application, string Logger, string Message)
        {
            logs.Apt_Code = Apt_Code;
            logs.Note = Note;
            logs.Application = Application;
            logs.Logger = User_Code;
            logs.Message = Message;

            await logs_Lib.add(logs); //로그입력
        }
        #endregion



        /// <summary>
        /// 로딩 시 실행
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
                intNum = Aid;
                if (LevelCount < 5)
                {
                    await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "권한이 없습니다.");
                    MyNav.NavigateTo("/");
                }
                else
                {                    
                    dsort = await disbursementSort_Lib.GetList(Apt_Code);
                    cnnA = await referral_Career_Lib.Detail(User_Code);
                    accA = await accountSort_Lib.GetList_Sort("0");
                    accT = await account_Lib.GetList_SortB(54);
                    cnn = await disbursement_Lib.Details(Aid);
                    pnn = await Post_Lib.GetList("A");
                    referral = await referral_Career_Lib.Details(User_Code);
                    PostDuty = referral.Post + referral.Duty;
                    //PostDuty = cnnA.Post + cnnA.Duty;
                    app = await approval_Lib.GetList(Apt_Code, "지출결의서");
                    //referral = await referral_Career_Lib.Details(User_Code);

                    await DisplayData();
                }
            }
            else
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "로그인하지 않았습니다.");
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

        #region 결제 여부(민원)
        Decision_Entity decision { get; set; } = new Decision_Entity();
        [Inject] public IApproval_Lib approval_Lib { get; set; }
        [Inject] public IDbImagesLib dbImagesLib { get; set; }
        public string strUserName { get; set; }
        public string decisionA { get; set; }
        public string strAid { get; set; }

        /// <summary>
        ///결재여부
        /// </summary>
        public string FuncShowOK(string strPostDuty, string Apt_Code, int apNum)
        {
            string strBloomCode = "Disbursement";
            string strUserName = "";
            string nu = apNum.ToString();
            strAid = nu;
            int intA = decision_Lib.Details_Count(Apt_Code, strBloomCode, nu, strPostDuty);
            if (intA > 0)
            {
                decision = decision_Lib.Details(Apt_Code, strBloomCode, strAid, strPostDuty);
                
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
        public byte[] sealImage(string aptcode, string post, string duty, object apNum, string BloomCode, string UserCode)
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
                        int CodeBeing = dbImagesLib.Photos_Count(UserCode);

                        if (CodeBeing > 0)
                        {
                            strResult = dbImagesLib.Photos_image(UserCode);
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
            decision.BloomCode = "Disbursement";
            decision.Parent = strAid;
            if (referral.Duty == "직원" || referral.Duty == "반원" || referral.Duty == "반장" || referral.Duty == "주임" || referral.Duty == "기사" || referral.Duty == "서무")
            {
                decision.PostDuty = "담당자";
            }
            else
            {
                decision.PostDuty = referral.Post + referral.Duty;
            }
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
                //if (vnn.innViw == "B")
                //{
                    await decision_Lib.Add(decision);
                    //await appeal.Edit_Complete(strNum);

                    app = await approval_Lib.GetList(Apt_Code, "지출결의서");
                    await DisplayData();
                    //vnn = await appeal.Detail(vnn.Num.ToString());
                //}
                //else
                //{
                //    await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "지출결의서가 완료되지 않았습니다..");
                //}
            }
            else
            {
                await decision_Lib.Add(decision);
                //await appeal.Edit_Complete(Code.ToString());

                app = await approval_Lib.GetList(Apt_Code, "지출결의서");
                await DisplayData();
            }

        }

        #endregion

        /// <summary>
        /// 데이터 불러오기
        /// </summary>
        public double IntTotalSum_A { get; set; } = 0;
        public double IntTotalSum_B { get; set; } = 0;
        public int be_bankDeals { get; set; } = 0;
        private async Task DisplayData()
        {            
            ann = await accountDeals_Lib.GetList_Apt_DBMC_Provide_A(Aid.ToString(), "자동이체", Apt_Code);
            IntTotalSum_A = await accountDeals_Lib.Sum_Apt_DBMC_Provide_A(Aid.ToString(), "자동이체", Apt_Code);
            annA = await accountDeals_Lib.GetList_Apt_DBMC_Provide_B(Aid.ToString(), "자동이체", Apt_Code);
            IntTotalSum_B = await accountDeals_Lib.Sum_Apt_DBMC_Provide_B(Aid.ToString(), "자동이체", Apt_Code);
            //be_bankDeals = bankAccountDeals_Lib.Being_BankAccount(Apt_Code, Aid.ToString(), )
            bank = await bankAccount_Lib.GetList(Apt_Code);
            intNum = Aid;
        }

        /// <summary>
        /// 지출결의서 입력 모달 열기
        /// </summary>
        private void btnInsert()
        {
            cnn.DraftDate = DateTime.Now;
            cnn.InputDate = DateTime.Now;
            bnn.ExecutionDate = DateTime.Now;
            
            InsertViews = "B";
            strTitle = "지출내역 입력";
        }

        /// <summary>
        /// 대분류 선택 시 중분류 만들기
        /// </summary>        
        private async Task OnSortA(ChangeEventArgs a)
        {
            bnn.AccountSortCodeA = a.Value.ToString();
            bnn.AccountSortCodeB = "";
            bnn.AccountCode = "";
            bnn.SubstitutionAccountCode = "";
            accB = await accountSort_Lib.GetList_Sort(a.Value.ToString());
        }

        private async Task OnSortB(ChangeEventArgs a)
        {
            bnn.AccountSortCodeB = a.Value.ToString();
            bnn.AccountCode = "";
            bnn.SubstitutionAccountCode = "";
            int ac1 = Convert.ToInt32(bnn.AccountSortCodeA);
            int ac2 = Convert.ToInt32(bnn.AccountSortCodeB);
            acc = await account_Lib.GetList_Sort_AB(ac1, ac2);
        }

        /// <summary>
        /// 확정자 찾기오기
        /// </summary>
        private string OnExtension(string filesName)
        {
            return Path.GetExtension(filesName);
        }

        private void btnEdit(DisbursementEntity ar)
        {
            cnn = ar;
            InsertViews = "B";
        }

        /// <summary>
        /// 지출결의서 삭제
        /// </summary>
        private async Task ByRemove(int Aid)
        {
            bool isDelete = await JSRuntime.InvokeAsync<bool>("confirm", $"{Aid} 첨부파일을 정말로 삭제하시겠습니까?");

            if (isDelete)
            {
                await disbursement_Lib.Remove(Aid);
                MyNav.NavigateTo("/Accountings");
            }
            else
            {
                await JSRuntime.InvokeAsync<object>("alert", "취소되었습니다.");
            }
        }

        /// <summary>
        /// 통장 새로 입력 열기
        /// </summary>
        private void btnBlankMove()
        {
            bankA = new BankAccountEntity();

            strTitle = "은행 통장 새로 등록";
            InsertBanks = "B";
        }

        /// <summary>
        /// 지출내역 수정
        /// </summary>
        /// <param name="ar"></param>
        private async Task btnEditA(AccountDealsEntity ar)
        {
            bnn = ar;
            accB = await accountSort_Lib.GetList_Sort(bnn.AccountSortCodeA);

            int ac1 = Convert.ToInt32(bnn.AccountSortCodeA);
            int ac2 = Convert.ToInt32(bnn.AccountSortCodeB);
            acc = await account_Lib.GetList_Sort_AB(ac1, ac2);

            strTitle = "지출결의서 상세 내역 수정";

            InsertViews = "B";
        }

        private string Name(string User_Code)
        {
            string strName = "";
            if (!string.IsNullOrWhiteSpace(User_Code))
            {
                strName = staff_Lib.UsersName(User_Code);
            }
            return strName;
        }

        /// <summary>
        /// 지출내역 삭제
        /// </summary>
        private async Task ByRemoveA(int Aid)
        {
            bool isDelete = await JSRuntime.InvokeAsync<bool>("confirm", $"{Aid} 첨부파일을 정말로 삭제하시겠습니까?");

            if (isDelete)
            {
                await accountDeals_Lib.Remove(Aid);
                await files_Lib.FileRemove(Aid.ToString(), "AccountsDeals", Apt_Code);

                await DisplayData();
            }
            else
            {
                await JSRuntime.InvokeAsync<object>("alert", "취소되었습니다.");
            }
        }

        //이곳 수정요

        /// <summary>
        /// 지출 이전 잔액
        /// </summary>
        public string Ago_Balance(string BankAccountCode, string DisbursementCode, string AptCode)
        {
            string result = "";
            double dbSum = 0;
            double Be = bankAccountDeals_Lib.Being_BankAccount(AptCode, DisbursementCode, BankAccountCode);
            if (Be > 0)
            {
                dbSum = bankAccountDeals_Lib.Ago_Balance_Last(AptCode, BankAccountCode, DisbursementCode);
            }
            else
            {
                dbSum = bankAccountDeals_Lib.Balance_Last(AptCode, DisbursementCode, BankAccountCode);
            }

            result = string.Format("{0:#,##0}", dbSum);
            
            return result;
        }

        /// <summary>
        /// 지출액
        /// </summary>
        public string OutputSum(string BankAccountCode, string DisbursementCode, string AptCode)
        {
            string result = "";
            double dbSum = 0;
            long Be = bankAccountDeals_Lib.Being_BankAccount(AptCode, DisbursementCode, BankAccountCode);
            if (Be > 0)
            {
                dbSum = bankAccountDeals_Lib.OutputSum_Last(AptCode, BankAccountCode, DisbursementCode);
            }
            else
            {
                dbSum = accountDeals_Lib.Sum_Apt_DBMC_BankAccount(DisbursementCode, BankAccountCode, AptCode); ;
            }

            result = string.Format("{0:#,##0}", dbSum);

            return result;
        }

        /// <summary>
        /// 입금액
        /// </summary>
        public string InputSum(string BankAccountCode, string DisbursementCode, string AptCode)
        {
            // 은행 계좌 거래의 합계를 구하는 함수
            double inputSum = 0; // 입력된 거래 금액의 합계
            double being = bankAccountDeals_Lib.Being_BankAccount(AptCode, DisbursementCode, BankAccountCode); // 잔액
            if (being > 0)
            {
                // 잔액이 양수인 경우 마지막 거래 금액을 합산
                inputSum = bankAccountDeals_Lib.InputSum_Last(AptCode, BankAccountCode, DisbursementCode);
            }
            else
            {
                // 잔액이 음수인 경우 모든 거래 금액을 합산
                inputSum = accountDeals_Lib.Sum_Apt_DBMC_inPut_BankAccount(DisbursementCode, BankAccountCode, AptCode);
            }

            return string.Format("{0:#,##0}", inputSum); // 합계를 문자열로 변환하여 반환
        }

        /// <summary>
        /// 지출 이후 잔액
        /// </summary>
        public string After_Balance(string BankAccountCode, string DisbursementCode, string AptCode)
        {
            // 은행 계좌의 잔액을 구하는 함수
            double balance = 0; // 잔액
            double being = bankAccountDeals_Lib.Being_BankAccount(AptCode, DisbursementCode, BankAccountCode); // 잔액
            if (being > 0)
            {
                // 잔액이 양수인 경우 마지막 거래 후의 잔액을 구함
                balance = bankAccountDeals_Lib.After_Balance_Last(AptCode, BankAccountCode, DisbursementCode);
            }
            else
            {
                // 잔액이 음수인 경우 0으로 설정
                balance = 0;
            }

            return string.Format("{0:#,##0}", balance); // 잔액을 문자열로 변환하여 반환
        }

        /// <summary>
        /// 통장내역 존재여부
        /// </summary>
        public int Being_BankAccount(string AptCode, string DisbursementCode, string BankAccountCode)
        {            
            int Be = bankAccountDeals_Lib.Being_BankAccount(AptCode, DisbursementCode, BankAccountCode);

            return Be;
        }

        /// <summary>
        /// 결재완료 여부
        /// </summary>
        public string ApprovalView(string DisbursementCode, string BankAccountCode)
        {
            string strResult = "";
            try
            {
                strResult = bankAccountDeals_Lib.ApprovalView(DisbursementCode, BankAccountCode);
            }
            catch (Exception)
            {
                strResult = "문제 발생";
            }
            return strResult;
        }

        /// <summary>
        /// 지출 내역 모달 닫기
        /// </summary>
        private void btnClose()
        {
            InsertViews = "A";
        }

        /// <summary>
        /// 지출 내역 입력
        /// </summary>
        /// <returns></returns>
        private async Task btnSave()
        {
            #region 아이피 입력
            string myIPAddress = "";
            // 모든 네트워크 인터페이스를 가져옴
            var networkInterfaces = System.Net.NetworkInformation.NetworkInterface.GetAllNetworkInterfaces();
            foreach (var networkInterface in networkInterfaces)
            {
                // 각 네트워크 인터페이스에 할당된 IP 주소들을 가져옴
                var ipProperties = networkInterface.GetIPProperties();
                foreach (var ip in ipProperties.UnicastAddresses)
                {
                    // IPv4 또는 IPv6 주소 중 로컬 링크가 아닌 것만 선택
                    if (ip.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                    {
                        myIPAddress = ip.ToString();
                        break;
                    }
                }
            }
            bnn.PostIp = myIPAddress;
            #endregion

            bnn.User_Code = User_Code;
            bnn.AptCode = Apt_Code;
            bnn.DisbursementCode = Aid.ToString();

            if (string.IsNullOrWhiteSpace(Apt_Code))
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "권한이 없습니다.");
                MyNav.NavigateTo("/");
            }
            else if (string.IsNullOrWhiteSpace(bnn.AccountSortCodeA))
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "대분류를 선택하지 않았습니다.");
            }
            else if (string.IsNullOrWhiteSpace(bnn.AccountSortCodeB))
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "중분류를 선택하지 않았습니다.");
            }
            else if (string.IsNullOrWhiteSpace(bnn.AccountCode))
            {
                await JSRuntime.InvokeAsync<object>("alert", "개정과목을 선택하지 않았습니다.");
            }
            else if (string.IsNullOrWhiteSpace(bnn.BankAccountCode))
            {
                await JSRuntime.InvokeAsync<object>("alert", "통장을 선택하지 않았습니다.");
            }
            else if (string.IsNullOrWhiteSpace(bnn.DisbursementCode))
            {
                await JSRuntime.InvokeAsync<object>("alert", "잘못 접근입니다..");
                MyNav.NavigateTo("/");
            }
            else if (string.IsNullOrWhiteSpace(bnn.ProvidePlace))
            {
                await JSRuntime.InvokeAsync<object>("alert", "지급처를 입력하지 않았습니다.");
            }
            else if (string.IsNullOrWhiteSpace(bnn.ProvideWay))
            {
                await JSRuntime.InvokeAsync<object>("alert", "지급방법을 선택하지 않았습니다.");
            }
            else if (string.IsNullOrWhiteSpace(bnn.InOutDivision))
            {
                await JSRuntime.InvokeAsync<object>("alert", "내외부 지출 선택을 하지 않았습니다.");
            }
            else if (bnn.ExecutionDate < Convert.ToDateTime("2022-01-01"))
            {
                await JSRuntime.InvokeAsync<object>("alert", "집행일을 입력하지 않았습니다.");
            }
            else
            {               
                if (bnn.Aid < 1)
                {
                    await accountDeals_Lib.Add(bnn);
                }
                else
                {
                    await accountDeals_Lib.Edit(bnn);
                }

                double ab = await accountDeals_Lib.Sum_Apt_DBMC(Aid.ToString(), Apt_Code);
                await disbursement_Lib.Sum_InputSum(Aid.ToString(), ab);
                await Loks(bnn.Details, "지출결의서", bnn.User_Code, "지출 거래 내용 입력");
                bnn = new AccountDealsEntity();
                await DisplayData();
            }
        }

        private void btnBankClose()
        {
            InsertBanks = "A";
        }

        /// <summary>
        /// 통장 입력
        /// </summary>
        /// <returns></returns>
        private async Task btnBankSave()
        {
            bankA.AptCode = Apt_Code;
            bankA.User_Code = User_Code;
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
            bankA.PostIp = myIPAddress;
            #endregion

            if (string.IsNullOrWhiteSpace(Apt_Code))
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "권한이 없습니다.");
                MyNav.NavigateTo("/");
            }
            else if (string.IsNullOrWhiteSpace(bankA.BankNumber))
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "계좌번호를 입력하지 않았습니다.");
            }
            else if (string.IsNullOrWhiteSpace(bankA.BankName))
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "은행명을 입력하지 않았습니다.");
            }
            else if (string.IsNullOrWhiteSpace(bankA.BankAccountName))
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "통장명을 입력하지 않았습니다.");
            }
            else if (string.IsNullOrWhiteSpace(bankA.BankAccountSort))
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "통장 용도를 입력하지 않았습니다.");
            }
            else if (string.IsNullOrWhiteSpace(bankA.InputDivision))
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "시재금 여부를 선택하지 않았습니다.");
            }
            else if (string.IsNullOrWhiteSpace(bankA.BankAccountDivision))
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "예금 종류를 선택하지 않았습니다.");
            }
            else
            {
                if (bankA.Aid < 1)
                {
                    await bankAccount_Lib.Add(bankA);
                }
                else
                {
                    await bankAccount_Lib.Edit(bankA);
                }
                await Loks(bnn.Details, "지출결의서", bnn.User_Code, "통장 정보(수정) 입력");
                bankA = new BankAccountEntity();
                bank = await bankAccount_Lib.GetList(Apt_Code);
            }
        }


        #region 파일 첨부 관련 클래스
        /// <summary>
        /// 첨부파일 보기
        /// </summary>
        private async Task onFileViews(string Aidaa, string strName)
        {
            FileViews = "B";
            Files_Count = await files_Lib.Sw_Files_Data_Index_Count(strName, Aid.ToString(), Apt_Code);
            if (Files_Count > 0)
            {
                Files_Entity = await files_Lib.Sw_Files_Data_Index(strName, Aid.ToString(), Apt_Code);
            }
        }

        /// <summary>
        /// 파일 첨부 열기
        /// </summary>
        private void onFileInput(string sAid, string strName )
        {
            FileInsertViews = "B";
            SevName = strName;
            SevAid = sAid;
        }

        /// <summary>
        /// 파일 첨부 닫기
        /// </summary>
        private void btnFileClose()
        {
            FileInsertViews = "A";
        }

        /// <summary>
        /// 파일 보기 닫기
        /// </summary>
        private void FilesViewsClose()
        {
            FileViews = "A";
        }



        #region Event Handlers
       
        [Inject] Microsoft.AspNetCore.Hosting.IWebHostEnvironment env { get; set; }
        public string CompleteViews { get; private set; }
        public string strUserCode { get; private set; }
        public string FileInsertViews { get; set; }
        public string SevName { get; set; }
        public string SevAid { get; set; }
        public string strSortA { get; set; } = "AccountsDeals";
        public string strSortB { get; set; } = "BanksDeals";
        public int F_Counts { get; private set; }

        [Inject] ILogger<Index> Logger { get; set; }
        private List<IBrowserFile> loadedFiles = new();
        private long maxFileSize = 1024 * 1024 * 30;
        private int maxAllowedFiles = 5;
        private bool isLoading;
        private decimal progressPercent;
        //Sw_Files_Entity finn { get; set; } = new Sw_Files_Entity();
        public string fileName { get; set; }

        private async Task LoadFiles(InputFileChangeEventArgs e)
        {
            isLoading = true;
            loadedFiles.Clear();
            progressPercent = 0;

            foreach (var file in e.GetMultipleFiles(maxAllowedFiles))
            {
                if (SevName == "AccountsDeals")
                {
                    finn.Parents_Num = SevAid; // 선택한 ParentId 값 가져오기  
                }
                else
                {
                    finn.Parents_Num = Aid.ToString(); // 선택한 ParentId 값 가져오기  
                }
                finn.Sub_Num = finn.Parents_Num;
                //try
                //{
                    var pathA = $"D:\\Msh\\Home\\Ayoung\\sw_Files_View\\Sw_files";

                string strFiles = Path.GetExtension(file.Name);

                    string _FileName = "Accounts" + Apt_Code + DateTime.Now.ToShortDateString() + strFiles;

                    fileName = Dul.FileUtility.GetFileNameWithNumbering(pathA, _FileName);

                    //fileName = Dul.FileUtility.GetFileNameWithNumbering(pathA, file.Name);
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
                    finn.Parents_Name = SevName;
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

                    if (SevName == "AccountsDeals")
                    {
                        await accountDeals_Lib.FilesCount(SevAid, "A"); //거래 내역 파일 추가 메서드                       
                    }
                    else
                    {
                        await bankAccountDeals_Lib.FilesCount(SevAid, Aid.ToString(), "A"); //통장거래 파일 추가 메서드
                    }
                //}
                //catch (Exception ex)
                //{
                //    Logger.LogError("File: {Filename} Error: {Error}",
                //        file.Name, ex.Message);
                //}
            }
            await DisplayData();

            FileInsertViews = "A";

            //Files_Count = await files_Lib.Sw_Files_Data_Index_Count("Service", dnn.Num.ToString(), Apt_Code);
            //if (Files_Count > 0)
            //{
            //    Files_Entity = await files_Lib.Sw_Files_Data_Index("Service", dnn.Num.ToString(), Apt_Code);
            //}
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
                    string rootFolder = $"D:\\Msh\\Home\\Ayoung\\sw_Files_View\\Sw_files\\{_files.Sw_FileName}";
                    File.Delete(rootFolder);
                }
                await files_Lib.FileRemove(_files.Num.ToString(), _files.Parents_Name, Apt_Code);
                //await UploadRepositoryAsyncReference.DeleteAsync(Id); // 삭제
                //NavigationManagerReference.NavigateTo("/Uploads"); // 리스트 페이지로 이동

                if (_files.Parents_Name == "BanksDeals")
                {
                    await bankAccountDeals_Lib.FilesCount(_files.Parents_Num, Aid.ToString(), "B");
                }
                else
                {
                    await accountDeals_Lib.FilesCount(_files.Parents_Num, "B");
                }

                Files_Count = await files_Lib.Sw_Files_Data_Index_Count(_files.Parents_Name, _files.Parents_Num, Apt_Code);
                if (Files_Count > 0)
                {
                    Files_Entity = await files_Lib.Sw_Files_Data_Index(_files.Parents_Name, _files.Parents_Num, Apt_Code);
                }
            }
            else
            {
                await JSRuntime.InvokeAsync<object>("alert", "취소되었습니다.");
            }
        }

        /// <summary>
        /// 파일 첨부 여부
        /// </summary>
        /// <param name="Aid"></param>
        /// <returns></returns>
        private int Files(string intAid)
        {
            return F_Counts = bankAccountDeals_Lib.FileCount(intAid, Aid.ToString());
        }
       
        /// <summary>
        /// 통장 내역 입력
        /// </summary>
        private async Task btnInsertBanksDeals()
        {
            InsertBankDeals = "B";
            bankAA = new BankAccountDealsEntity();
            bank = await bankAccount_Lib.GetList(Apt_Code);
            strTitle = "통장 내역 입력";
        }
        #endregion

        /// <summary>
        /// 은행 거래 내역 입력시 통장 잔액 구하기
        /// </summary>
        private async Task OnBankSelect(ChangeEventArgs a)
        {
            string av = a.Value.ToString();
            if (!string.IsNullOrWhiteSpace(av))
            {
                int re = await bankAccountDeals_Lib.Being_Aid(Apt_Code, Aid.ToString(), av); //해당 코드 입력 여부 확인
                if (re > 0)
                {
                    var result = await bankAccountDeals_Lib.Details(Apt_Code, Aid.ToString(), av);
                    bankAA= result;
                }
                else
                {
                    bankAA.BankAccountCode = av;
                    string Re = await disbursement_Lib.Ago(Apt_Code, Aid.ToString());
                    bankAA.Ago_Balance = bankAccountDeals_Lib.After_Balance_Last(Apt_Code, av, Re);
                    bankAA.Details = await bankAccount_Lib.BankAccountName(Convert.ToInt32(av));
                }               
            }
        }

        /// <summary>
        /// 지출 구분 선택 실행
        /// </summary>
        private async Task OnDivision(ChangeEventArgs a)
        {
            if (!string.IsNullOrWhiteSpace(a.Value.ToString()))
            {
                bnn.InOutDivision = a.Value.ToString();

                if (bnn.InOutDivision == "B")
                {
                    bnn.ProvidePlace = "시재금 계좌";
                }
            }
            else
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "내외부 구분을 선택하지 않았습니다.");
            }
        }

        private async Task OnInputBank(ChangeEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(e.Value.ToString()))
            {
                bnn.InputBankAccountCode = e.Value.ToString();
            }
            else
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "내부 이체 통장을 선택하지 않았습니다.");
            }
        }

        /// <summary>
        /// 통장 내역을 입력
        /// </summary>
        /// <returns></returns>
        private async Task btnSaveBankDeals()
        {
            bankAA.AptCode = Apt_Code;
            bankAA.User_Code = User_Code;
            bankAA.DisbursementCode = Aid.ToString();

            bankAA.OutputSum = accountDeals_Lib.Sum_Apt_DBMC_BankAccount(bankAA.DisbursementCode, bankAA.BankAccountCode, Apt_Code); // 은행 통장별 지출 합계
            //double dbInputDivisionSum = accountDeals_Lib.Sum_Apt_DBMC_inPut_BankAccount(bankAA.DisbursementCode, bankAA.BankAccountCode, Apt_Code); // 해당 계좌로 입금된 금액
            //double dbAfterSum = bankAA.Ago_Balance - dbOutDivisionSum; //전 잔액에서 지출액을 공제한 잔액
            double dbBeforeSum = bankAA.After_Balance + bankAA.OutputSum - bankAA.Ago_Balance;
            //bankAA.OutputSum = dbOutDivisionSum;
            bankAA.InputSum = dbBeforeSum;

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
            bankAA.PostIp = myIPAddress;
            #endregion

            if (string.IsNullOrWhiteSpace(bankAA.AptCode))
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "로그인하지 않았습니다.");
                MyNav.NavigateTo("/");
            }
            else if (string.IsNullOrWhiteSpace(bankAA.DisbursementCode))
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "지출결의서를 선택하지 않았습니다.");
            }
            else if (string.IsNullOrWhiteSpace(bankAA.BankAccountCode))
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "통장을 선택하지 않았습니다.");
            }
            else if (bankAA.Ago_Balance < 1)
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "지출전 잔액을 입력하지 않았습니다.");
            }
            else if (bankAA.After_Balance < 1)
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "지출후 잔액을 입력하지 않았습니다.");
            }
            else
            {
                #region 시재금 통장 관련 함수
                string InputDivision = await bankAccount_Lib.InputDivision(bankAA.BankAccountCode);//시재금 여부 확인
                if (InputDivision == "B")
                {
                    double dbInputDivisionSum = accountDeals_Lib.Sum_Apt_DBMC_inPut_BankAccount(bankAA.DisbursementCode, bankAA.BankAccountCode, Apt_Code);

                    bankAA.InputSum = dbInputDivisionSum;
                    bankAA.OutputSum = bankAA.Ago_Balance + bankAA.InputSum - bankAA.After_Balance;
                } 
                #endregion

                int be = await bankAccountDeals_Lib.Being_Aid(Apt_Code, bankAA.DisbursementCode, bankAA.BankAccountCode);
                if (be < 1)
                {
                    await bankAccountDeals_Lib.Add(bankAA);
                }
                else
                {
                    await bankAccountDeals_Lib.Edit(bankAA);
                }
                await Loks(bankAA.Details, "지출결의서", bankAA.User_Code, "통장 변경내용 입력");
                bankAA = new BankAccountDealsEntity();
            }
        }

        private void btnCloseC()
        {
            InsertBankDeals = "A";
        }

        private async Task ByApproval(int intAid)
        {            
            decision.PostDuty = referral.Post + referral.Duty;            
            if (decision.PostDuty == "관리소장")
            {                
                await bankAccountDeals_Lib.ApprovalEdit(Aid.ToString(), intAid.ToString());
                await DisplayData();                
            }            
        }

        /// <summary>
        /// 은행 통장 사용 여부 메서드
        /// </summary>
        /// <param name="Aid"></param>
        /// <returns></returns>
        private async Task ByRemoveB(int Aid)
        {
            bool isDelete = await JSRuntime.InvokeAsync<bool>("confirm", $"{Aid} 통장을 정말로 미사용으로 전환 하시겠습니까?");
            if (isDelete)
            {
                await bankAccount_Lib.Remove(Aid);
                bank = await bankAccount_Lib.GetList(Apt_Code);
            }
            else
            {
                await JSRuntime.InvokeAsync<object>("alert", "취소되었습니다.");
            }            
        }

        private void btnPrint()
        {
            MyNav.NavigateTo("http://pv.swtmc.co.kr/Prints/Asp/Accounts?Apt=" + Apt_Code + "&Code=" + Aid);
        }
    }
}
