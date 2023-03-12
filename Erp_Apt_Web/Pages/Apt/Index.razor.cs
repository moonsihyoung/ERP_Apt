using Company;
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

namespace Erp_Apt_Web.Pages.Apt
{
    public partial class Index
    {
        #region 인스턴스
        [CascadingParameter] public Task<AuthenticationState> AuthenticationStateRef { get; set; }
        [Inject] public NavigationManager MyNav { get; set; } // Url 
        [Inject] IJSRuntime JSRuntime { get; set; }
        [Inject] public IApt_Lib apt_Lib { get; set; }
        [Inject] public IApt_Sub_Lib apt_Sub_Lib { get; set; }
        [Inject] public ISido_Lib sido_Lib { get; set; }
        [Inject] public IStaff_Lib staff_Lib { get; set; }
        [Inject] public IStaff_Sub_Lib staff_Sub_Lib { get; set; }
        [Inject] public IReferral_career_Lib referral_Career_Lib { get; set; }
        [Inject] public ICompany_Lib company_Lib { get; set; }
        #endregion

        #region 속성
        Apt_Entity ann { get; set; } = new Apt_Entity();
        Apt_Sub_Entity bnn { get; set; } = new Apt_Sub_Entity();
        List<Apt_Entity> annA { get; set; } = new List<Apt_Entity>();
        List<Sido_Entity> sidos { get; set; } = new List<Sido_Entity>();
        Staff_Entity snn { get; set; } = new Staff_Entity();
        Staff_Sub_Entity ssnn { get; set; } = new Staff_Sub_Entity();
        Referral_career_Entity rcnn { get; set; } = new Referral_career_Entity();
        List<Referral_career_Entity> Listrc { get; set; } = new List<Referral_career_Entity>();
        List<Apt_Entity> apt { get; set; } = new List<Apt_Entity>();
        #endregion

        #region 변수
        public string Views { get; set; } = "A";
        public string InsertViews { get; set; } = "A";
        public string Apt_Code { get; set; }
        public string User_Code { get; set; }
        public string Apt_Name { get; set; }
        public string User_Name { get; set; }
        public int LevelCount { get; set; }
        public string strTitle { get; set; }
        public string strSido { get; set; }
        public string strSort { get; set; } = "A";
        public string SearchViews { get; set; } = "A";
        private ElementReference myref;
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
        /// 데이터 정보 
        /// </summary>
        private async Task DisplayData()
        {
            if (strSort == "A")
            {
                pager.RecordCount = await apt_Lib.GetList_All_Count();
                annA = await apt_Lib.GetList(pager.PageIndex);
            }
            else if (strSort == "B")
            {
                pager.RecordCount = 15;
                annA = await apt_Lib.SearchList(strSearchApt);
            }
            else
            {
                pager.RecordCount = await apt_Lib.GetList_All_Count();
                annA = await apt_Lib.GetList(pager.PageIndex);
            }
        }

        /// <summary>
        /// 상세
        /// </summary>
        public async Task ByAid(Apt_Entity apt)
        {
            ann = apt;
            bnn = await apt_Sub_Lib.Detail(apt.Apt_Code);
            Listrc = await referral_Career_Lib.GetList_Apt_Sojang(apt.Apt_Code);
            strTitle = ann.Apt_Name + " 아파트 상세보기";
            Views = "B";
        }

       

        /// <summary>
        /// 수정
        /// </summary>
        public async Task ByEdit(Apt_Entity apt)
        {
            strTitle = "공동주택 수정";
            ann = apt;
            if (ann.Apt_Adress_Sido == "서울특별시")
            {
                strSido = "A";
            }
            else if (ann.Apt_Adress_Sido == "경기도")
            {
                strSido = "B";
            }
            else if (ann.Apt_Adress_Sido == "부산광역시")
            {
                strSido = "C";
            }
            else if (ann.Apt_Adress_Sido == "대구광역시")
            {
                strSido = "D";
            }
            else if (ann.Apt_Adress_Sido == "인천광역시")
            {
                strSido = "E";
            }
            else if (ann.Apt_Adress_Sido == "광주광역시")

            #region 시도
            {
                strSido = "F";
            }
            else if (ann.Apt_Adress_Sido == "대전광역시")
            {
                strSido = "G";
            }
            else if (ann.Apt_Adress_Sido == "울산광역시")
            {
                strSido = "H";
            }
            else if (ann.Apt_Adress_Sido == "세종특별자치도")
            {
                strSido = "I";
            }
            else if (ann.Apt_Adress_Sido == "충청남도")
            {
                strSido = "J";
            }
            else if (ann.Apt_Adress_Sido == "충청북도")
            {
                strSido = "K";
            }
            else if (ann.Apt_Adress_Sido == "경상남도")
            {
                strSido = "L";
            }
            else if (ann.Apt_Adress_Sido == "경상북도")
            {
                strSido = "M";
            }
            else if (ann.Apt_Adress_Sido == "전라남도")
            {
                strSido = "N";
            }
            else if (ann.Apt_Adress_Sido == "전라복도")
            {
                strSido = "O";
            }
            else if (ann.Apt_Adress_Sido == "강원도")
            {
                strSido = "P";
            }
            else if (ann.Apt_Adress_Sido == "제주특별자치도")
            {
                strSido = "Q";
            }
            #endregion

            sidos = await sido_Lib.GetList_Code(strSido);

            bnn = await apt_Sub_Lib.Detail(apt.Apt_Code);
            InsertViews = "B";
        }

        /// <summary>
        /// 삭제
        /// </summary>
        public async Task ByRemove(int Aid)
        {
            bool isDelete = await JSRuntime.InvokeAsync<bool>("confirm", $"{Aid} 첨부파일을 정말로 삭제하시겠습니까?");
            if (isDelete)
            {
                await apt_Lib.Delete(Aid.ToString());
                await apt_Sub_Lib.Remove(Aid);
                await DisplayData();
            }
        }

        /// <summary>
        /// 새로 등록 열기
        /// </summary>
        private void onOpen()
        {
            strTitle = "공동주택 등록";
            ann.AcceptancedOfWork_Date = Convert.ToDateTime("2000-01-01");
            InsertViews = "B";
        }

        /// <summary>
        /// 등록 닫기
        /// </summary>
        private void btnClose()
        {
            InsertViews = "A";
        }

        /// <summary>
        /// 사업자 번호 체크 
        /// </summary>
        private async Task onCR_Num(ChangeEventArgs a)
        {
            string cp_Num = a.Value.ToString();
            cp_Num = cp_Num.Replace("_", "").Replace(",", "").Replace("-", "").Replace(".", "");
            cp_Num = cp_Num.Insert(3, "-");
            cp_Num = cp_Num.Insert(6, "-");

            bool tr = checkCpIdenty(cp_Num);
            int intCr = await apt_Lib.Cn_Check(cp_Num);

            if (tr == false)
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", cp_Num + "는 잘못된 사업자 등록 번호 입니다. 다시 입력해 주세요...");
                await JSRuntime.InvokeVoidAsync("exampleJsFunctionsA.focusElement",  myref); //set Focus;
            }
            else
            {
                if (intCr > 0)
                {
                    await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", cp_Num + "는 이미 입력된 사업자 등록 번호 입니다.");
                    await JSRuntime.InvokeVoidAsync("exampleJsFunctionsA.focusElement", myref); //set Focus;
                }
                else
                {
                    ann.CorporateResistration_Num = cp_Num;
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
            ann.Apt_Adress_Sido = await sido_Lib.SidoName(args.Value.ToString());
            sidos = await sido_Lib.GetList_Code(args.Value.ToString());
            strSido = args.Value.ToString();
            string re = await apt_Lib.List_Number();
            ann.Apt_Code = strSido + re;
        }

        /// <summary>
        /// 시군구 선택 실행
        /// </summary>
        /// <param name="args"></param>
        public void onGunGu(ChangeEventArgs args)
        {
            ann.Apt_Adress_Gun = args.Value.ToString();
            //StateHasChanged();
        }

        /// <summary>
        /// 공동주택 정보 수정 및 저장
        /// </summary>
        /// <returns></returns>
        private async Task btnSave()
        {
            if (ann.Apt_Name == null || ann.Apt_Name == "")
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "공동주택명을 입력하지 않았습니다.");
            }
            else if (ann.Apt_Form == null || ann.Apt_Form == "" || ann.Apt_Form == "Z")
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "공동주택 형태를 입력하지 않았습니다.");
            }
            else if (ann.Apt_Adress_Sido == null || ann.Apt_Adress_Sido == "" || ann.Apt_Adress_Sido == "Z")
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "시도를 입력하지 않았습니다.");
            }
            else if (ann.Apt_Adress_Gun == null || ann.Apt_Adress_Gun == "" || ann.Apt_Adress_Gun == "Z")
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "시군구를 입력하지 않았습니다.");
            }
            else if (ann.Apt_Adress_Rest == null || ann.Apt_Adress_Rest == "")
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "나머지 상세 주소를 입력하지 않았습니다.");
            }
            else if (ann.CorporateResistration_Num == null || ann.CorporateResistration_Num == "")
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "사업자 등록번로를 입력하지 않았습니다.");
            }
            else if (ann.HouseHold_Num == 0)
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "세대수를 입력하지 않았습니다.");
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
                ann.PostIP = myIPAddress;
                //ann.User_Code = User_Code;
                //bnn.PostIP = myIPAddress;
                #endregion
                if (ann.Aid < 1)
                {
                    int intAdd = await apt_Lib.Add(ann);
                    await DisplayData();
                    InsertViews = "A";
                    ann = new Apt_Entity();
                }
                else
                {
                    await apt_Lib.Edit(ann);
                    InsertViews = "A";
                    ann = new Apt_Entity();
                }
            }            
        }

        /// <summary>
        /// 상세보기 모달 닫기
        /// </summary>
        private void btnCloseA()
        {
            Views = "A";
        }

        

        /// <summary>
        /// 상세보기 모달 닫기
        /// </summary>
        private void btnCloseS()
        {
            SearchViews = "A";
        }

        public string strAptCode { get; set; }
        public string strSearchApt { get; set; }
        private async Task OnApt(ChangeEventArgs a)
        {
            strAptCode = a.Value.ToString();
            ann = await apt_Lib.Details(strAptCode);
            bnn = await apt_Sub_Lib.Detail(strAptCode);
            Listrc = await referral_Career_Lib.GetList_Apt_Sojang(strAptCode);
            strTitle = ann.Apt_Name + " 아파트 상세보기";
            Views = "B";
        }

        /// <summary>
        /// 공동주택 리스트 만들기
        /// </summary>
        private async Task OnSearchApt(ChangeEventArgs a)
        {
            strSearchApt = a.Value.ToString();
            apt = await apt_Lib.SearchList(strSearchApt);
            strSort = "B";
            await DisplayData();
        }

        private void onSearch()
        {
            SearchViews = "B";
        }

    }
}
