using Company;
using Erp_Apt_Lib;
using Erp_Apt_Lib.Decision;
using Erp_Apt_Lib.Stocks;
using Erp_Apt_Staff;
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
using System.Threading.Tasks;
using Works;

namespace Erp_Apt_Web.Pages.Works
{
    public partial class Details
    {
        #region 인스턴스
        [Parameter] public int Num { get; set; }
        [CascadingParameter] public Task<AuthenticationState> AuthenticationStateRef { get; set; }
        [Inject] public IApt_Lib apt_Lib { get; set; }
        [Inject] public NavigationManager MyNav { get; set; }
        [Inject] IJSRuntime JSRuntime { get; set; }
        [Inject] public ISw_Files_Lib files_Lib { get; set; } // 파일 첨부
        [Inject] public IReferral_career_Lib referral_Career_Lib { get; set; }
        [Inject] public IPost_Lib Post_Lib { get; set; }
        [Inject] public IBloom_Lib bloom_Lib { get; set; }
        [Inject] public IWorks_Lib works_Lib { get; set; }
        [Inject] public IWorksSub_Lib worksSub_Lib { get; set; }
        [Inject] public ICompany_Lib company_Lib { get; set; }
        [Inject] public IStocks_Lib stocks_Lib { get; set; }
        [Inject] public IWhSock_Lib whSock_Lib { get; set; }
        [Inject] public IDecision_Lib decision_Lib { get; set; } // 결재정보
        #endregion

        #region 목록
        //public int intNum { get; private set; }

        List<WorksSub_Entity> bnn = new List<WorksSub_Entity>();
        Referral_career_Entity cnnA { get; set; } = new Referral_career_Entity();
        Works_Entity dnn { get; set; } = new Works_Entity();
        WorksSub_Entity ennA { get; set; } = new WorksSub_Entity();
        List<WorksSub_Entity> enn { get; set; } = new List<WorksSub_Entity>();
        public int svAgoBe { get; private set; }
        public int svNextBe { get; private set; }
        List<Post_Entity> pnn { get; set; } = new List<Post_Entity>();
        List<Referral_career_Entity> fnn = new List<Referral_career_Entity>();
        List<WareHouse_Entity> wheL = new List<WareHouse_Entity>();
        WareHouse_Entity wheE { get; set; } = new WareHouse_Entity();
        List<Stock_Code_Entity> sceL = new List<Stock_Code_Entity>();
        Stock_Code_Entity sceE { get; set; } = new Stock_Code_Entity();
        
        List<Bloom_Entity> bloom_A = new List<Bloom_Entity>();
        List<Bloom_Entity> bloom_B = new List<Bloom_Entity>();
        List<Bloom_Entity> bloom_C = new List<Bloom_Entity>();
        List<Bloom_Entity> bloom_D = new List<Bloom_Entity>();
        private int Files_Count;
        Works_Entity works_s { get; set; } = new Works_Entity();
        Sw_Files_Entity sfnn { get; set; } = new Sw_Files_Entity();
        List<Sw_Files_Entity> Files_Entity { get; set; } = new List<Sw_Files_Entity>();
        List<SC_WH_Join_Entity> scJoin { get; set; } = new List<SC_WH_Join_Entity>();
        Sw_Files_Entity finn { get; set; } = new Sw_Files_Entity();
        #endregion

        #region 변수
        public string User_Code { get; set; }
        public string User_Name { get; set; }
        public string Apt_Code { get; set; }
        public string Apt_Name { get; set; }
        public string UserName { get; set; }
        public string ListViews { get; set; } = "B";
        public string PostDuty { get; set; }
        public string DetailsViews { get; set; } = "A";
        public string InsertViews { get; set; } = "A";
        public string InOutViews { get; set; } = "A";
        public string strPost { get; set; }
        public DateTime apDate { get; set; }
        public string PrivateViews { get; set; } = "A";
        public string EditViews { get; set; } = "A";
        public string StocksViews { get; set; } = "A";
        public string Company_num { get; set; }
        public string strWSortA { get; set; }
        public string strWSortB { get; set; }
        public string strWSortC { get; set; }
        public string strWSortD { get; set; }
        public DateTime svDate { get; set; } = DateTime.Now.Date;
        public string FileInsertViews { get; set; } = "A";
        public string FileViews { get; set; } = "A";

        public string InputCompleteViews { get; set; } = "A";
        #endregion

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

                cnnA = await referral_Career_Lib.Detail(User_Code);
                PostDuty = cnnA.Post + cnnA.Duty;

                app = await approval_Lib.GetList(Apt_Code, "작업일지");

                referral = await referral_Career_Lib.Details(User_Code);

                apDate = DateTime.Now.Date;
                pnn = await Post_Lib.GetList("A");//부서 목록

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

        #region 결제 여부(작업)
        Decision_Entity decision { get; set; } = new Decision_Entity();
        Referral_career_Entity referral { get; set; } = new Referral_career_Entity();
        List<Approval_Entity> app { get; set; } = new List<Approval_Entity>();
        [Inject] public IApproval_Lib approval_Lib { get; set; }
        [Inject] public IDbImagesLib dbImagesLib { get; set; }
        public string strUserName { get; set; }
        public string decisionA { get; set; }
        public string strNum { get; set; }

        /// <summary>
        ///결재여부
        /// </summary>
        /// <param name="objPostDate"></param>
        /// <returns></returns>
        public string FuncShowOK(string strPostDuty, string Apt_Code, int apNum)
        {
            string strBloomCode = "Service";
            string strUserName = "";
            string nu = apNum.ToString();
            strNum = nu;
            int intA = decision_Lib.Details_Count(Apt_Code, strBloomCode, nu, strPostDuty);
            if (intA > 0)
            {
                decision = decision_Lib.Details(Apt_Code, strBloomCode, strNum, strPostDuty);
                //PostDuty = decision.PostDuty;
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
        public byte[] sealImage(string aptcode, string post, string duty, object apNum, string BloomCode, string User_Code)
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
                        //string User_Code = referral_Career_Lib.User_Code_Bes(aptcode, post, duty, );
                        int CodeBeing = dbImagesLib.Photos_Count(User_Code);

                        if (CodeBeing > 0)
                        {
                            strResult = dbImagesLib.Photos_image(User_Code);
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
            decision.BloomCode = "Service";
            decision.Parent = Num.ToString();
            if (referral.Duty == "직원" || referral.Duty == "반원" || referral.Duty == "반장" || referral.Duty == "주임" || referral.Duty == "기사" || referral.Duty == "서무")
            {
                decision.PostDuty = "담당자";
            }
            else
            {
                decision.PostDuty = referral.Post + referral.Duty;
            }
            decision.PostDuty = referral.Post + referral.Duty;
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
                if (dnn.Complete == "B")
                {
                    await decision_Lib.Add(decision);
                    await works_Lib.ServiceConform(Num.ToString());

                    app = await approval_Lib.GetList(Apt_Code, "작업일지");
                    await DisplayData();
                }
                else
                {
                    await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "작업이 완료되지 않았습니다..");
                }
            }
            else
            {
                await decision_Lib.Add(decision);
                //await appeal.Edit_Complete(Code.ToString());

                app = await approval_Lib.GetList(Apt_Code, "작업일지");
                await DisplayData();
            }
            StateHasChanged();
        }

        #endregion

        /// <summary>
        /// 상세보기
        /// </summary>
        /// <returns></returns>
        private async Task DisplayData()
        {
            dnn = await works_Lib.Service_View_Num(Num);
            enn = await worksSub_Lib.Getlist(Num.ToString());

            svAgoBe = await works_Lib.svAgoBe(Apt_Code, Num.ToString());
            svNextBe = await works_Lib.svNextBe(Apt_Code, Num.ToString());

            Files_Entity = await files_Lib.Sw_Files_Data_Index("Service", Num.ToString(), Apt_Code);

            scJoin = await whSock_Lib.Group_List_ABC(Apt_Code, "Service", Num.ToString());
            
        }

        // <summary>
        /// 부서 선택 시 실행
        /// </summary>
        protected async Task OnPost(ChangeEventArgs args)
        {
            string Post = args.Value.ToString();
            ennA.WorkPost = Post;
            fnn = await referral_Career_Lib.GetList_Post_Staff_be(Post, Apt_Code);
        }

        /// <summary>
        /// 작업자 만들기
        /// </summary>
        private void OnReciever(ChangeEventArgs a)
        {
            if (ennA.WorkerName == null || ennA.WorkerName == "")
            {
                ennA.WorkerName = ennA.WorkPost + "▶" + a.Value.ToString();
            }
            else
            {
                ennA.WorkerName = ennA.WorkerName + "; " + ennA.WorkPost + "▶" + a.Value.ToString();
            }
        }

        /// <summary>
        /// 작업 일지 새로 등록
        /// </summary>
        private void onOpen()
        {
            InsertViews = "B";
            InOutViews = "Z";
            ennA.WorkDate = DateTime.Now;
            ennA.Out_In_Viw = "내부작업";
        }

        /// <summary>
        /// 지시사항 수정 열기
        /// </summary>
        private async Task onEditOpen()
        {
            EditViews = "B";
            fnn = await referral_Career_Lib.GetList_Post_Staff_be(dnn.svPost, Apt_Code);
            strWSortA = dnn.svBloomA;
            bloom_B = await bloom_Lib.GetList_bb(strWSortA);
            strWSortB = dnn.svBloomB;
            bloom_C = await bloom_Lib.GetList_cc(strWSortB);
            strWSortC = dnn.svBloomC;
            bloom_D = await bloom_Lib.GetList_dd(Apt_Code, strWSortA);
            strWSortD = dnn.svBloom;
            dnn.ModifyDate = DateTime.Now;
            
        }

       /// <summary>
       /// 지시사항 수정 닫기
       /// </summary>
        private void btnWorkClose()
        {
            EditViews = "A";
        }
        
        /// <summary>
        /// 지시사항 수정
        /// </summary>
        /// <returns></returns>
        private async Task btnWorkSave()
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
            //dnn.PostIP = myIPAddress;
            dnn.ModifyIP = myIPAddress;
            dnn.ModifyDate = DateTime.Now;
            #endregion
            await works_Lib.UpdateWorksEdit(dnn);

            EditViews = "A";
        }


        #region 작업분류 관련
        /// <summary>
        /// 대분류 선택
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        private async Task onWSortA(ChangeEventArgs a)
        {
            strWSortA = a.Value.ToString();
            bloom_B = await bloom_Lib.GetList_bb(strWSortA);
            dnn.svBloomA = strWSortA;

            strWSortB = "Z";
            strWSortC = "Z";
            strWSortD = "Z";
        }

        /// <summary>
        /// 중분류 선택
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        private async Task onWSortB(ChangeEventArgs a)
        {
            string strA = a.Value.ToString();
            //strWSortB = "Z";
            strWSortC = "Z";
            strWSortD = "Z";
            bloom_C = await bloom_Lib.GetList_cc(strA);
            dnn.svBloomB = strA;
        }

        /// <summary>
        /// 세분류 선택
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        private async Task onWSortC(ChangeEventArgs a)
        {
            string strA = a.Value.ToString();
            //strWSortC = "Z";
            strWSortD = "Z";
            bloom_D = await bloom_Lib.GetList_dd(Apt_Code, strWSortA);
            dnn.svBloomC = strA;
        }

        /// <summary>
        /// 장소 선택
        /// </summary>
        /// <param name="a"></param>
        private void onWSortD(ChangeEventArgs a)
        {
            string strA = a.Value.ToString();
            strWSortD = "Z";
            dnn.svBloom = strA;
        }
        #endregion

        /// <summary>
        /// 작업내역 입력 닫기
        /// </summary>
        private void btnClose()
        {
            InsertViews = "A";
        }

        /// <summary>
        /// 작업내역 입력
        /// </summary>
        /// <returns></returns>
        private async Task btnSave()
        {
            ennA.AptCode = Apt_Code;
            ennA.subDay = ennA.WorkDate.Day.ToString();
            ennA.subMonth = ennA.WorkDate.Month.ToString();
            ennA.subYear = ennA.WorkDate.Year.ToString();
            ennA.UserID = User_Code;
            ennA.Service_Code = Num.ToString();
            ennA.Net_Group = "Service";
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
            ennA.PostIP = myIPAddress;
            ennA.ModifyIP = myIPAddress;
            ennA.ModifyDate = DateTime.Now;
            #endregion

            if (ennA.WorkContent == null || ennA.WorkContent == "")
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "작업 내용을 입력하지 않았습니다..");
            }
            else if (ennA.Out_In_Viw == null || ennA.Out_In_Viw == "" || ennA.Out_In_Viw == "Z")
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "내부작업인지 외부작인지 선택하지 않았습니다..");
            }
            else if (ennA.WorkerName == null || ennA.WorkerName == "")
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "작업자를 입력하지 않았습니다..");
            }
            else if (ennA.WorkPost == null || ennA.WorkPost == "" || ennA.WorkPost == "Z")
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "작업부서를 선택하지 않았습니다..");
            }
            else if (ennA.Out_In_Viw == null || ennA.Out_In_Viw == "" || ennA.Out_In_Viw == "Z")
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "내부작업인지 외부작인지 선택하지 않았습니다..");
            }
            else if (ennA.Service_Code == null || ennA.Service_Code == "")
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "작업코드가 없습니다. 저장할 수 없으니 다시 로그인하여 주십시요..");
            }
            else
            {
                if (ennA.Aid < 1)
                {
                    await worksSub_Lib.Add(ennA);
                }
                else
                {
                    await worksSub_Lib.Edit(ennA);
                }

                ennA = new WorksSub_Entity();
                InsertViews = "A";
                await DisplayData();
            }


            //if (ennA.Aid < 1)
            //{
            //    await worksSub_Lib.Add(ennA);
            //}
            //else
            //{
            //    await worksSub_Lib.Edit(ennA);
            //}
            
        }

        /// <summary>
        /// 내부 혹은 외부 작선 선택 실행
        /// </summary>
        private void onInOutCom(ChangeEventArgs a)
        {
            InOutViews = a.Value.ToString();
            ennA.Out_In_Viw = a.Value.ToString();
        }

        /// <summary>
        /// 외부업체 찾기
        /// </summary>
        private async Task OnCompanyInfor(ChangeEventArgs a)
        {
            var companyInfor = await company_Lib.CorNum_Detail(a.Value.ToString());
            ennA.OutCorName = companyInfor.Cor_Name;
            ennA.OutCorMobile = companyInfor.Mobile;
            ennA.OutCorCharger = companyInfor.Ceo_Name;
            ennA.Scw_Code = companyInfor.Cor_Code;
        }

        /// <summary>
        /// 업무 이행 내역 수정 열기
        /// </summary>
        private async Task onUpdate(WorksSub_Entity wse)
        {           
            fnn = await referral_Career_Lib.GetList_Post_Staff_be(wse.WorkPost, Apt_Code);
            ennA = wse;
            
            InsertViews = "B";
        }

        /// <summary>
        /// 업무일지 이행 내역 삭제
        /// </summary>
        private async Task onRemove(int Aid)
        {
            bool isDelete = await JSRuntime.InvokeAsync<bool>("confirm", $"{Aid}번 업무이행 내역을 정말로 삭제하시겠습니까?");
            if (isDelete)
            {
                //svDate = DateTime.Now.Date;
                await worksSub_Lib.Remove(Aid);
                await DisplayData();
            }
        }

        #region 자재관련 정보 클래스
        /// <summary>
        /// 자재 입출력 입력열기
        /// </summary>
        private void onStock()
        {
            StocksViews = "B";
        }

        /// <summary>
        /// 자재 입출고 입력
        /// </summary>
        public DateTime Wh_Date { get; set; }
        private async Task btnStocksSave()
        {
            wheE.AptCode = Apt_Code;
            wheE.Parents = Num.ToString();
            wheE.P_Group = "Service";
            wheE.Wh_UserID = User_Code;
            wheE.Wh_Year = Wh_Date.Year.ToString();
            wheE.Wh_Month = Wh_Date.Month.ToString();
            wheE.Wh_Day = Wh_Date.Day.ToString();
            wheE.Wh_Code = "wh" + await whSock_Lib.Wh_Count_Data();
            wheE.Wh_Use = wheE.Etc;
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
            wheE.PostIP = myIPAddress;
            wheE.ModifyIP = myIPAddress;
            wheE.ModifyDate = DateTime.Now;
            #endregion

            if (wheE.Wh_Quantity < 0)
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "입출고 수량을 입력하지 않았습니다..");
            }
            else if (wheE.St_Code == null || wheE.St_Code == "")
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "자재코드가 선택되지 않습니다...");
            }
            else
            {
                if (wheE.Num < 1)
                {
                    await whSock_Lib.Warehouse_Write(wheE);
                }
                else
                {
                    await whSock_Lib.Warehouse_Update(wheE);
                }
                scJoin = await whSock_Lib.Group_List_ABC(Apt_Code, "Service", Num.ToString());
                wheE = new WareHouse_Entity();
                StocksViews = "A";
            }

        }

        /// <summary>
        /// 자재명 찾기 (유사한 자재명 목록)
        /// </summary>
        private async Task OnStocksName(ChangeEventArgs a)
        {
            sceL = await stocks_Lib.stName_Query(Apt_Code, a.Value.ToString());
        }

        /// <summary>
        /// 선택된 자재입출고 상세정보 불러오기
        /// </summary>
        public string strSt_Name { get; set; }
        public int strWh_Balance { get; set; } = 0;
        private async Task OnSelectName(ChangeEventArgs a)
        {
            try
            {
                var wh = await whSock_Lib.Wh_View_StCode(Apt_Code, a.Value.ToString()); //자재 코드로 입출고 내역 불러오기
                strWh_Balance = wh.Wh_Balance;  // wheE.Wh_Balance = wheE.Wh_Balance ?? 0; //잔고 값이 없으면 0으로 초기화
            }
            catch (Exception)
            {
                strWh_Balance = 0;
            }
            
            var strStocks = await stocks_Lib.St_View_Code(a.Value.ToString());
            wheE.St_Code = strStocks.St_Code;
            wheE.St_Group = strStocks.St_Group;
            wheE.Wh_Unit = strStocks.St_Unit;
            strSt_Name = strStocks.St_Name;//자재명 불러오기
        }

        /// <summary>
        /// 입출고 수량 입력 실행
        /// </summary>
        private async Task OnQuantity(ChangeEventArgs a)
        {
            wheE.Wh_Quantity = Convert.ToInt32(a.Value);
            if (wheE.Wh_Section == "B")
            {
                wheE.Wh_Balance = strWh_Balance - wheE.Wh_Quantity;
                wheE.Wh_Cost = 0;
            }
            else if (wheE.Wh_Section == "A")
            {
                wheE.Wh_Balance = strWh_Balance + wheE.Wh_Quantity;
            }
            else
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "알 수 없는 문제가 발생하였습니다. \n 관리자에게 문의 하세요..");
            }
        }

        /// <summary>
        /// 입출고 내역 선택 시 실행
        /// </summary>
        private void OnSection(ChangeEventArgs a)
        {
            wheE.Wh_Section = a.Value.ToString();
            wheE.Wh_Quantity = 0;
            wheE.Wh_Cost = 0;

        }

        /// <summary>
        /// 자재관리 입력 폼 닫기
        /// </summary>
        private void btnStocksClose()
        {
            StocksViews = "A";
        }

        /// <summary>
        /// 자재관리 수정 열기
        /// </summary>
        private async Task onStocksEdit(SC_WH_Join_Entity ae)
        {
            wheE.Num = ae.Num;
            wheE.AptCode = ae.AptCode;
            wheE.Etc = ae.Etc;
            wheE.Parents = ae.Parents;
            wheE.P_Group = ae.P_Group;
            wheE.St_Code = ae.St_Code;
            wheE.St_Group = ae.St_Group;
            wheE.Wh_Balance = ae.Wh_Balance;
            wheE.Wh_Code = ae.Wh_Code;
            wheE.Wh_Cost = ae.Wh_Cost;
            wheE.Wh_Day = ae.Wh_Day;
            wheE.Wh_Month = ae.Wh_Month;
            wheE.Wh_Place = ae.Wh_Place;
            wheE.Wh_Quantity = ae.Wh_Quantity;
            wheE.Wh_Section = ae.Wh_Section;
            wheE.Wh_Unit = ae.Wh_Unit;
            wheE.Wh_Use = ae.Wh_Use;
            wheE.Wh_UserID = ae.Wh_UserID;
            wheE.Wh_Year = ae.Wh_Year;

            if (ae.St_Section == "A")
            {
                strWh_Balance = ae.Wh_Balance - ae.Wh_Quantity;
            }
            else if (ae.St_Section == "B")
            {
                strWh_Balance = ae.Wh_Balance + ae.Wh_Quantity;
            }

            sceL = await stocks_Lib.stName_Query(Apt_Code, ae.St_Name);
            strSt_Name = ae.St_Name;

            StocksViews = "B";
        }

        /// <summary>
        /// 자재관리 정보 삭제
        /// </summary>
        private async Task onStocksRemove(int Num)
        {
            bool isDelete = await JSRuntime.InvokeAsync<bool>("confirm", $"{Num}번 자재관리정보을 정말로 삭제하시겠습니까?");
            if (isDelete)
            {
                //svDate = DateTime.Now.Date;
                await whSock_Lib.Wh_Delete(Num);
                await DisplayData();
            }

        } 
        #endregion

        #region 파일 첨부 관련 클래스
        /// <summary>
        /// 첨부파일 보기
        /// </summary>
        private async Task onFileViews()
        {
            FileViews = "B";
            Files_Count = await files_Lib.Sw_Files_Data_Index_Count("Service", Num.ToString(), Apt_Code);
            if (Files_Count > 0)
            {
                Files_Entity = await files_Lib.Sw_Files_Data_Index("Service", Num.ToString(), Apt_Code);
            }
        }

        /// <summary>
        /// 파일 첨부 열기
        /// </summary>
        private void onFileInput()
        {
            FileInsertViews = "B";
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
            //StateHasChanged();
        }

        #region Event Handlers
        private long maxFileSize = 1024 * 1024 * 30;
        [Inject] Microsoft.AspNetCore.Hosting.IWebHostEnvironment env { get; set; }
        public string CompleteViews { get; private set; }
        public string strUserCode { get; private set; }

        /// <summary>
        /// 저장하기 버튼 클릭 이벤트 처리기
        /// </summary>
        protected async void btnFileSave()
        {
            finn.Parents_Num = Num.ToString(); // 선택한 ParentId 값 가져오기 
            finn.Sub_Num = finn.Parents_Num;
            var fileName = "";

            var format = "image/png";

            #region 파일 업로드 관련 추가 코드 영역

            foreach (var file in selectedImage)
            {
                var resizedImageFile = await file.RequestImageFileAsync(format, 1025, 760);
                var buffer = new byte[resizedImageFile.Size];
                await resizedImageFile.OpenReadStream(maxFileSize).ReadAsync(buffer);
                var imageDataUrl = $"data:{format};base64,{Convert.ToBase64String(buffer)}";

                Stream stream = resizedImageFile.OpenReadStream(maxFileSize);

                var path = $"D:\\Msh\\Home\\Ayoung\\sw_Files_View\\Sw_files";

                string _FileName = file.Name;
                string _FileNameA = Path.GetExtension(_FileName);
                string _FileNameB = "Service_" + Apt_Code;
                _FileName = _FileNameB + _FileNameA;

                fileName = Dul.FileUtility.GetFileNameWithNumbering(path, _FileName);


                path = path + $"\\{fileName}";

                FileStream fs = File.Create(path);
                await stream.CopyToAsync(fs);
                fs.Close();

                finn.Sw_FileName = fileName;
                finn.Sw_FileSize = Convert.ToInt32(file.Size);
                finn.Parents_Name = "Service";
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

                //await defect_lib.Edit_ImagesCount(bnn.Aid); // 첨부파일 추가를 db 저장(하자, defect)


                //dnn = new Sw_Files_Entity();
            }


            FileInsertViews = "A";

            Files_Count = await files_Lib.Sw_Files_Data_Index_Count("Service", Num.ToString(), Apt_Code);
            if (Files_Count > 0)
            {
                Files_Entity = await files_Lib.Sw_Files_Data_Index("Service", Num.ToString(), Apt_Code);
            }

            StateHasChanged();
            #endregion           
        }

        #endregion

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
                var resizedImageFile = await imageFile.RequestImageFileAsync(format, 300, 300);
                var buffer = new byte[resizedImageFile.Size];
                await resizedImageFile.OpenReadStream().ReadAsync(buffer);
                var imageDataUrl = $"data:{format};base64,{Convert.ToBase64String(buffer)}";
                imageDataUrls.Add(imageDataUrl);
            }
            StateHasChanged();
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
                    string rootFolder = $"D:\\Msh\\Home\\Ayoung\\sw_Files_View\\Sw_files\\{_files.Sw_FileName}";
                    File.Delete(rootFolder);
                }
                await files_Lib.FileRemove(_files.Num.ToString(), "Service", Apt_Code);
                //await UploadRepositoryAsyncReference.DeleteAsync(Id); // 삭제
                //NavigationManagerReference.NavigateTo("/Uploads"); // 리스트 페이지로 이동
                Files_Count = await files_Lib.Sw_Files_Data_Index_Count("Service", Num.ToString(), Apt_Code);
                if (Files_Count > 0)
                {
                    Files_Entity = await files_Lib.Sw_Files_Data_Index("Service", Num.ToString(), Apt_Code);
                }
            }
            else
            {
                await JSRuntime.InvokeAsync<object>("alert", "취소되었습니다.");
            }
        }
        #endregion

        /// <summary>
        /// 작업완료 등록 열기
        /// </summary>
        private void onComplete()
        {
            InputCompleteViews = "B";
        }

        /// <summary>
        /// 작업완료 등록 닫기
        /// </summary>
        private void btnWorkCompleteClose()
        {
            InputCompleteViews = "A";
        }

        /// <summary>
        /// 작업완료 여부 입력
        /// </summary>
        private async Task btnWorkCompleteSave(int Num )
        {
            await works_Lib.ServiceComplete(Num.ToString());
            await DisplayData();
        }

        /// <summary>
        /// 목록으로 이동
        /// </summary>
        /// <param name="Aid"></param>
        private void btnList()
        {
            MyNav.NavigateTo("/Works");
        }

        /// <summary>
        /// 이전 정보로 이동
        /// </summary>
        /// <param name="Aid"></param>
        private async Task btnAgo(int Aid)
        {
            string result = await works_Lib.svAgo(Apt_Code, Aid.ToString());
            Num = Convert.ToInt32(result);
            await DisplayData();
        }

        /// <summary>
        /// 다음 정보로 이동
        /// </summary>
        /// <param name="Aid"></param>
        private async Task btnNext(int Aid)
        {
            string result = await works_Lib.svNext(Apt_Code, Aid.ToString());
            Num = Convert.ToInt32(result);
            await DisplayData();
        }
    }
}
