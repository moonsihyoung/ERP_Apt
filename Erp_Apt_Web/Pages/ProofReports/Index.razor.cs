using Erp_Apt_Lib.ProofReport;
using Erp_Apt_Staff;
using Erp_Entity;
using Erp_Lib;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Erp_Apt_Web.Pages.ProofReports
{
    public partial class Index
    {
        [CascadingParameter] public Task<AuthenticationState> AuthenticationStateRef { get; set; }
        [Inject] public IProofReport_Lib proofReport_Lib { get; set; }
        [Inject] IJSRuntime JSRuntime { get; set; }
        [Inject] public NavigationManager MyNav { get; set; } // Url 
        [Inject] public IStaff_Career_Lib staff_Career_Lib { get; set; }
        [Inject] public IPost_Lib post_Lib { get; set; }
        [Inject] public IDuty_Lib duty_Lib { get; set; }

        List<ProofRepot_Entity> ann { get; set; } = new List<ProofRepot_Entity>();
        ProofRepot_Entity bnn { get; set; } = new ProofRepot_Entity();
        public List<Post_Entity> Post { get; set; } = new List<Post_Entity>();
        public List<Duty_Entity> Duty { get; set; } = new List<Duty_Entity>();


        #region 변수
        public string User_Code { get; set; }
        public string User_Name { get; set; }
        public string Apt_Code { get; set; }
        public string AptCode { get; set; } = "";
        public string Apt_Name { get; set; }
        public int LevelCount { get; set; } = 0;
        public string Views { get; set; } = "A"; //상세 열기
        public string RemoveViews { get; set; } = "A";//삭제 열기
        public string InsertViews { get; set; } = "A"; // 입력 열기
        public string strTitle { get; set; }
        public int inta { get; set; } = 0;
        public int intb { get; set; } = 0;

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

                if (LevelCount > 5)
                {
                    Post = await post_Lib.GetList("A");
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
            if (Apt_Code == null || Apt_Code == "")
            {
                pager.RecordCount = await proofReport_Lib.GetProofs_All_Count();
                ann = await proofReport_Lib.GetProofs_All(pager.PageIndex);
            }
            else
            {
                pager.RecordCount = await proofReport_Lib.GetProofs_Apt_Count(Apt_Code);
                ann = await proofReport_Lib.GetProofs_Apt(pager.PageIndex, Apt_Code);
            }       
        }        

        /// <summary>
        /// 제증명 요청 정보 찾기
        /// </summary>
        public string strPost { get; set; }
        public string strPost_Code { get; set; }
        public string strDuty { get; set; }
        public string strDuty_Code { get; set; }
        public string strEndDate { get; set; }
        public string Date_Type { get; set; } = "A";
        private async Task OnUserID(ChangeEventArgs a)
        {
            var Users = await staff_Career_Lib.Details_Staff_Career(a.Value.ToString());

            bnn.UserName = Users.User_Name;
            bnn.UserCode = Users.User_ID;
            bnn.Mobile = Users.Mobile_Number;
            bnn.Scn = Users.Scn;
            bnn.Resignation = Users.Division;
            bnn.StartDate = Users.Career_Start_Date;
            bnn.EndDate = Users.Career_End_Date;
            if (bnn.Resignation == "A")
            {
                Date_Type = "B";
                bnn.EndDate = bnn.StartDate;
                bnn.Resignation = "A";
                strEndDate = "근무중";
            }
            else
            {
                Date_Type = "A";
                bnn.Resignation = "B";
                bnn.EndDate = Users.Career_End_Date;
                strEndDate = "";
            }
            //Duty = await duty_Lib.GetList(Users.Post_Code, "A");
            //strDuty_Code = await duty_Lib.
            bnn.PostDuty = Users.Post + Users.Duty;
            strPost = Users.Post;
            strDuty = Users.Duty;
            
            //strDuty =  (await duty_Lib.DutyCode(Users.Post_Code, Users.Duty)).ToString();
            
            bnn.Adress = Users.st_Sido + " " + Users.st_GunGu + " " + Users.st_Adress_Rest;
        }

        #region MyRegion
        /// <summary>
        /// 부서 선택하면 직책 만들기
        /// </summary>
        //public string strPost_Name { get; set; }
        //public string strDuty_Name { get; set; }
        //private async Task onPost(ChangeEventArgs args)
        //{
        //    strPost_Code = args.Value.ToString();
        //    strPost = await post_Lib.PostName(args.Value.ToString());
        //    Duty = await duty_Lib.GetList(args.Value.ToString(), "A");
        //} 

        //private void onDuty(ChangeEventArgs args)
        //{
        //    bnn.PostDuty = 
        //}
        #endregion

        private async Task btnSave()
        {           

            if (bnn.UserCode == null || bnn.UserCode == "")
            {
                await JSRuntime.InvokeAsync<object>("alert", "로그인되지 않았습니다.");
            }
            else if (bnn.CompanyCode == null || bnn.CompanyCode == "")
            {
                await JSRuntime.InvokeAsync<object>("alert", "발급처를 입력하지 않았습니다.");
            }
            else if (bnn.Adress == null || bnn.Adress == "")
            {
                await JSRuntime.InvokeAsync<object>("alert", "주소가 입력되지 않았습니다. \n상단 자기 이름 클릭하여 수정하거나 입력하여 주세요.");
            }
            else if (bnn.Mobile == null || bnn.Mobile == "")
            {
                await JSRuntime.InvokeAsync<object>("alert", "모바일 전화 번호가 없습니다. \n상단 자기 이름 클릭하여 수정하거나 입력하여 주세요.");
            }
            else if (bnn.Scn == null || bnn.Scn == "")
            {
                await JSRuntime.InvokeAsync<object>("alert", "생년월일이 입력되지 않았습니다. \n상단 자기 이름 클릭하여 수정하거나 입력하여 주세요.");
            }
            else if (bnn.StartDate < Convert.ToDateTime("2010-01-01"))
            {
                await JSRuntime.InvokeAsync<object>("alert", "배치일이 입력되지 않았습니다. \n 배치정보를 확인하세요.");
            }            
            else if (bnn.UseDate < Convert.ToDateTime("2020-01-01"))
            {
                await JSRuntime.InvokeAsync<object>("alert", "사용일이 입력되지 않았습니다.");
            }
            else if (bnn.UserPlace == null || bnn.UserPlace == "")
            {
                await JSRuntime.InvokeAsync<object>("alert", "사용처가 입력되지 않았습니다.");
            }
            else if (bnn.ProofName == null || bnn.ProofName == "")
            {
                await JSRuntime.InvokeAsync<object>("alert", "증명서 이름을 선택하지 않았습니다.");
            }
            else
            {
                if (bnn.ProofReportId < 1)
                {
                    bnn.AptName = Apt_Name;
                    bnn.AptCode = Apt_Code;
                    if (bnn.CompanyCode == "sw5")
                    {
                        bnn.CompanyName = "(주)신원티엠씨";
                    }
                    else if (bnn.CompanyCode == "sw5a")
                    {
                        bnn.CompanyName = "(주)남광개발";
                    }
                    else if (bnn.CompanyCode == "sw5b")
                    {
                        bnn.CompanyName = "(주)지엔텍";
                    }
                    else if (bnn.CompanyCode == "sw5c")
                    {
                        bnn.CompanyName = "삼일환경";
                    }

                    if (bnn.CompanyCode == "sw5")
                    {
                        bnn.CeoName = "김윤만 윤효순";
                    }
                    else if (bnn.CompanyCode == "sw5a")
                    {
                        bnn.CeoName = "남길석";
                    }
                    else if (bnn.CompanyCode == "sw5b")
                    {
                        bnn.CeoName = "남영호";
                    }
                    else
                    {
                        bnn.CeoName = "남길석";
                    }
                    bnn.Telephone = "031-469-1283";
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
                    bnn.PostIp = myIPAddress;
                    #endregion
                 
                    await proofReport_Lib.Add(bnn);
                }
                else
                {
                    await proofReport_Lib.Edit(bnn);
                }

                bnn = new ProofRepot_Entity();
                await DisplayData();
            }
        }

        /// <summary>
        /// 상세보기
        /// </summary>
        ///public string strEndDate { get; set; }
        private void ByDetails(ProofRepot_Entity proof)
        {
            bnn = proof;
            try
            {
                if (bnn.EndDate <= DateTime.Now)
                {
                    strEndDate = bnn.EndDate.ToShortDateString();
                }
                else
                {
                    strEndDate = "현재 근무 중";
                }
            }
            catch (Exception)
            {
                strEndDate = "알 수 없음.";
            }
            Views = "B";
        }

        /// <summary>
        /// 수정하기
        /// </summary>
        private void ByEdit(ProofRepot_Entity proof)
        {
            bnn = proof;
            InsertViews = "B";
        }

        /// <summary>
        /// 삭제하기
        /// </summary>
        private async Task ByRemove(int Aid)
        {
            bool isDelete = await JSRuntime.InvokeAsync<bool>("confirm", $"{Aid}번을 정말로 삭제하시겠습니까?");

            if (isDelete)
            {
                await JSRuntime.InvokeAsync<bool>("confirm", $"{Aid}번을 정말로 삭제할 수 없습니까?");
                await proofReport_Lib.Remove(Aid.ToString());
            }

            await DisplayData();

            StateHasChanged();
        }

        /// <summary>
        /// 제 증명 입력 열기
        /// </summary>
        private void btnOpen()
        {
            strTitle = "증명서 요청 입력";
            bnn.UseDate = DateTime.Now;
            InsertViews = "B";

        }
           
        /// <summary>
        /// 제 증명 입력 닫기
        /// </summary>
        private void btnClose()
        {
            InsertViews = "A";
        }

        private void btnCloseA()
        {
            Views = "A";
        }

        private void btnPrint(int Aid)
        {
            string strUrl = "http://net.wedew.co.kr/ProofReports";
            MyNav.NavigateTo("http://pt.swtmc.co.kr/Prints/Proof_sw.aspx?Aid=" + Aid + "&Ur=" + strUrl); //http://pt.swtmc.co.kr/Prints/Proof_sw.aspx
        }
    }
}
