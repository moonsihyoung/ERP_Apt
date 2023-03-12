using Erp_Apt_Lib;
using Erp_Apt_Lib.Accounting;
using Erp_Apt_Lib.Decision;
using Erp_Apt_Staff;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Erp_Apt_Web.Pages.Accountings
{
    public partial class Index
    {
        #region 인스턴스
        [CascadingParameter] public Task<AuthenticationState> AuthenticationStateRef { get; set; }
        [Inject] public NavigationManager MyNav { get; set; } // Url 
        [Inject] IJSRuntime JSRuntime { get; set; }
        [Inject] public IAccount_Lib account_Lib { get; set; }
        [Inject] public IDisbursement_Lib disbursement_Lib { get; set; }
        [Inject] public ISw_Files_Lib files_Lib { get; set; }
        [Inject] public IDecision_Lib decision_Lib { get; set; } // 결재정보
        [Inject] public IReferral_career_Lib referral_Career_Lib { get; set; }
        [Inject] public IDisbursementSort_Lib disbursementSort_Lib { get; set; }
        [Inject] public IAccountSort_Lib accountSort_Lib { get; set; }
        [Inject] public IAccountDeals_Lib accountDeals_Lib { get; set; }   
        [Inject] public IBankAccount_Lib bankAccount_Lib { get; set; }
        [Inject] public IBankAccountDeals_Lib bankAccountDeals_Lib { get; set; }


        #endregion

        #region 속성
        List<DisbursementEntity> ann { get; set; } = new List<DisbursementEntity>();
        List<DisbursementSortEnity> annA { get; set; } = new List<DisbursementSortEnity>();
        DisbursementEntity bnn { get; set; } = new DisbursementEntity();
        DisbursementSortEnity bnnA { set; get; } = new DisbursementSortEnity();

        List<AccountDealsEntity> dnnL { get; set; } = new List<AccountDealsEntity>();
        AccountDealsEntity dnn { get; set; } = new AccountDealsEntity();
        #endregion

        #region 변수
        public string User_Code { get; set; }
        public string User_Name { get; set; }
        public string Apt_Code { get; set; }
        public string Apt_Name { get; set; }
        public int LevelCount { get; set; } = 0;
        public string SortInsert { get; set; } = "A"; //지출결의서 구분 새로 등록 열기
        public string InsertViews { get; set; } = "A"; //입력 열기
        
        public string strTitle { get; set; }
        public int intNum { get; set; }
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

                if (LevelCount < 5)
                {
                    await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "권한이 없습니다.");
                    MyNav.NavigateTo("/");
                }
                else
                {
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


        /// <summary>
        /// 데이터 불러오기
        /// </summary>
        private async Task DisplayData()
        {
            annA = await disbursementSort_Lib.GetList(Apt_Code);
            pager.RecordCount = await disbursement_Lib.GetListCount(Apt_Code);
            ann = await disbursement_Lib.GetList(pager.PageIndex, Apt_Code); 
        }

        /// <summary>
        /// 지출결의서 입력 모달 열기
        /// </summary>
        private void btnInsert()
        {
            bnn = new DisbursementEntity();
            bnn.DraftDate = DateTime.Now;
            bnn.InputDate = DateTime.Now;
            strTitle = "지출결의서 입력";
            InsertViews = "B";
        }
               
        private void btnEdit(DisbursementEntity ar)
        {
            bnn = ar;
            InsertViews = "B";

        }

        

        /// <summary>
        /// 지출결의서 기본 등록 닫기
        /// </summary>
        private void btnClose()
        {
            InsertViews = "A";
        }

        /// <summary>
        /// 지출결의서 기본 저장
        /// </summary>
        private async Task btnSave()
        {
            if (string.IsNullOrWhiteSpace(bnn.DisbursementName))
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "지출결의서 분류를 선택하지 않았습니다.");
            }
            else if (string.IsNullOrWhiteSpace(bnn.Details))
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "지출결의서 설명을 입력하지 않았습니다.");
            }
            else
            {
                bnn.AptCode = Apt_Code;
                bnn.User_Code = User_Code;
                bnn.User_Name = User_Name;
                bnn.InputYear = bnn.InputDate.Year;
                bnn.InputMonth = bnn.InputDate.Month;
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
                dnn.PostIp = myIPAddress;
                bnn.PostIP = myIPAddress;
                #endregion

                int Re = await disbursement_Lib.Top_Code(bnn.DisbursementName, Apt_Code);

                int result = await disbursement_Lib.Add(bnn);

                if (result > 0)
                {
                    int being;
                    try
                    {
                        being = await disbursement_Lib.Top_Code(bnn.DisbursementName, Apt_Code);
                    }
                    catch (Exception)
                    {
                        being = 0;
                    }

                    if (being > 0)
                    {
                        dnnL = await accountDeals_Lib.GetList_Apt_DBMC(Re.ToString(), Apt_Code);
                        foreach (var st in dnnL)
                        {
                            dnn.AptCode = Apt_Code;
                            dnn.Details = st.Details;
                            dnn.AccountCode = st.AccountCode;
                            dnn.AccountSortCodeA = st.AccountSortCodeA;
                            dnn.AccountSortCodeB = st.AccountSortCodeB;
                            dnn.BankAccountCode = st.BankAccountCode;
                            dnn.CompanyCode = st.CompanyCode;
                            dnn.DisbursementCode = result.ToString();
                            dnn.InputSum = st.InputSum;                            
                            dnn.ProvideWay = st.ProvideWay;
                            dnn.SubstitutionAccountCode = st.SubstitutionAccountCode;
                            dnn.ExecutionDate = bnn.InputDate;
                            dnn.InOutDivision = st.InOutDivision;
                            if (dnn.InOutDivision == "B")
                            {
                                dnn.InputBankAccountCode = st.InputBankAccountCode;
                                dnn.ProvidePlace = st.ProvidePlace;
                            }
                            else
                            {
                                dnn.InputBankAccountCode = "A";
                                dnn.ProvidePlace = st.ProvidePlace;
                            }
                            dnn.User_Code = User_Code;
                            int bing = await accountDeals_Lib.Add(dnn);
                            double ab = await accountDeals_Lib.Sum_Apt_DBMC(being.ToString(), Apt_Code);
                            await disbursement_Lib.Sum_InputSum(being.ToString(), ab);
                        }
                    }
                    bnn = new DisbursementEntity();
                    await DisplayData();
                    InsertViews = "B";
                }                
            }            
        }

        /// <summary>
        /// 지출결의서 삭제
        /// </summary>
        private async Task ByRemove(DisbursementEntity ar)
        {
            bool isDelete = await JSRuntime.InvokeAsync<bool>("confirm", $"{ar.Aid} 첨부파일을 정말로 삭제하시겠습니까?");

            if (isDelete)
            {
                await disbursement_Lib.Remove(ar.Aid);
                await DisplayData();
            }
            else
            {
                await JSRuntime.InvokeAsync<object>("alert", "취소되었습니다.");
            }
        }

        private void btnDetails(int Aid)
        {
            MyNav.NavigateTo("/Accountings/Details/" + Aid);
        }

        private void btnSortInsert()
        {            
            strTitle = "지출결의서 구분 입력";
            SortInsert = "B";
        }

        private void btnCloseA()
        {
            SortInsert = "A";
        }

        /// <summary>
        /// 지출결의서 구분 입력
        /// </summary>
        private async Task btnSaveA()
        {
            bnnA.AptCode = Apt_Code;
            bnnA.User_Code = User_Code;

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
            bnnA.PostIP = myIPAddress;
            
            #endregion

            if (string.IsNullOrWhiteSpace(Apt_Code))
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "로그인하지 않았습니다.");
            }
            else if (string.IsNullOrWhiteSpace(bnnA.DisbursementName))
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "지출결의서 명을 입력하지 않았습니다.");
            }
            else if (string.IsNullOrWhiteSpace(bnnA.Details))
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "지출결의서 명 설명을 입력하지 않았습니다.");
            }
            else
            {
                if (bnnA.Aid < 1)
                {
                    await disbursementSort_Lib.Add(bnnA);
                    bnnA = new DisbursementSortEnity();
                }
                else
                {
                    await disbursementSort_Lib.Edit(bnnA);
                    bnnA = new DisbursementSortEnity();
                }
                annA = await disbursementSort_Lib.GetList(Apt_Code);
            }
        }

        /// <summary>
        /// 지출결의서 구분 수정
        /// </summary>
        private void btnEditA(DisbursementSortEnity ar)
        {
            bnnA = ar;
        }

        /// <summary>
        /// 지출결의서 구분 삭제
        /// </summary>
        private async Task ByRemoveA(DisbursementSortEnity ar)
        {
            bool isDelete = await JSRuntime.InvokeAsync<bool>("confirm", $"{ar.Aid} 첨부파일을 정말로 삭제하시겠습니까?");

            if (isDelete)
            {
                await disbursementSort_Lib.Remove(ar.Aid);
                annA = await disbursementSort_Lib.GetList(Apt_Code);
            }
            else
            {
                await JSRuntime.InvokeAsync<object>("alert", "취소되었습니다.");
            }
        }
    }    
}
