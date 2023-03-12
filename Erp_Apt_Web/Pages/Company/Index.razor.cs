using Company;
using Erp_Entity;
using Erp_Lib;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Erp_Apt_Web.Pages.Company
{
    public partial class Index
    {
        [CascadingParameter] public Task<AuthenticationState> AuthenticationStateRef { get; set; }
        [Inject] public ICompany_Lib company_Lib { get; set; }
        [Inject] public ICompany_Sub_Lib company_sub_Lib { get; set; }
        [Inject] public ICompany_Apt_Career_Lib company_Apt_Career_Lib { get; set; }
        [Inject] public ICompany_Join_Lib company_Join_Lib { get; set; }
        [Inject] public IContract_Sort_Lib contract_Sort_Lib { get; set; }
        [Inject] IJSRuntime JSRuntime { get; set; }
        [Inject] public NavigationManager MyNav { get; set; } // Url 
        [Inject] public ISido_Lib sido_Lib { get; set; }

        List<Company_Join_Entity> lstA { get; set; } = new List<Company_Join_Entity>();
        public int intNum { get; private set; }
        Company_Entity ann { get; set; } = new Company_Entity();
        Company_Sub_Entity bnn { get; set; } = new Company_Sub_Entity();
        Company_Join_Entity Join_Entity { get; set; } = new Company_Join_Entity();

        List<Contract_Sort_Entity> lstB { get; set; } = new List<Contract_Sort_Entity>();
        List<Contract_Sort_Entity> lstC { get; set; } = new List<Contract_Sort_Entity>();

        List<Sido_Entity> sidos { get; set; } = new List<Sido_Entity>();


        #region 변수
        public string User_Code { get; set; }
        public string User_Name { get; set; }
        public string Apt_Code { get; set; }
        public string Apt_Name { get; set; }
        public int LevelCount { get; set; } = 0;
        public string Views { get; set; } = "A"; //상세 열기
        public string RemoveViews { get; set; } = "A";//삭제 열기
        public string InsertViews { get; set; } = "A"; // 입력 열기

        public string FileViews { get; set; }
        public int Files_Count { get; set; } = 0;
        public string Sido { get; set; }

        public string Confirm { get; set; } = "A";
        public string Sido_Code { get; set; }
        public string Sort_NameA { get; set; }
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
                    lstB = await contract_Sort_Lib.List("Aa"); //업체 대분류 목록
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
            pager.RecordCount = await company_Join_Lib.GetList_Count();
            lstA = await company_Join_Lib.List_Page(pager.PageIndex);
            //StateHasChanged();
            intNum = pager.RecordCount - (pager.PageIndex * pager.PageSize);
        }

        /// <summary>
        /// 페이징
        /// </summary>
        protected async void PageIndexChanged(int pageIndex)
        {
            pager.PageIndex = pageIndex;
            pager.PageNumber = pageIndex + 1;

            await DisplayData();

            //StateHasChanged();
        }

        /// <summary>
        /// 업체 정보 입력 열기
        /// </summary>
        /// <returns></returns>
        protected void OnCompnayInput()
        {            
            InsertViews = "B";
            //lstC = await contract_Sort_Lib.List("Aa"); //업체 대분류 목록
            //await company_Lib.Add(ann);
            //StateHasChanged();
        }

        /// <summary>
        /// 대분류 선택 시 실행(소분류 만들기)
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        protected async Task onSortA(ChangeEventArgs args)
        {
            //Sort_NameA = args.Value.ToString();
            //bnn.TypeOfBusiness = await contract_Sort_Lib.Name(args.Value.ToString());
            lstC = await contract_Sort_Lib.List(args.Value.ToString());
            //StateHasChanged();
        }

        /// <summary>
        /// 소분류 선택 시 실행
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        protected void onSortB(ChangeEventArgs args)
        {
            //bnn.BusinessConditions = await contract_Sort_Lib.Name(args.Value.ToString());
            bnn.Company_Sort = args.Value.ToString();
            //lstC = await contract_Sort_Lib.List(args.Value.ToString());
            //StateHasChanged();
        }

        /// <summary>
        /// 사업자 번호 중복 체크
        /// </summary>
        protected async Task OnRepeatCheck()
        {
            ann.CorporateResistration_Num = ann.CorporateResistration_Num.Replace("-", "").Replace(" ", "");
            int intR = await company_Lib.CorNum_Being(ann.CorporateResistration_Num.ToString());

            //string cp_Num = ann.CorporateResistration_Num;
            //cp_Num = cp_Num.Insert(3, "-");
            //cp_Num = cp_Num.Insert(6, "-");

            //ann.CorporateResistration_Num = cp_Num;
            //string strResult = "";
            //List<SelectListItem> licities = new List<SelectListItem>();

            bool tr = checkCpIdenty(ann.CorporateResistration_Num);

            //licities.Add(new SelectListItem { Text = "::분류선택::", Value = "0" });
            if (tr == true)
            {
                if (intR > 0)
                {
                    await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", ann.CorporateResistration_Num + "는 이미 입력된 사업자 등록 번호 입니다. 다시 입력해 주세요...");
                    ann.CorporateResistration_Num = "";
                }
                else
                {
                    await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", ann.CorporateResistration_Num + "는 입력 가능한 사업자 번호 입니다...."); 
                }                
            }
            else
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", ann.CorporateResistration_Num + "는 잘못된 사업자 등록 번호 입니다. 다시 입력해 주세요...");
                ann.CorporateResistration_Num = "";
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
            ann.Adress_Sido = await sido_Lib.SidoName(args.Value.ToString());
            bnn.Sido = ann.Adress_Sido;
            sidos = await sido_Lib.GetList_Code(args.Value.ToString());
            //StateHasChanged();
        }

        /// <summary>
        /// 시군구 선택 실행
        /// </summary>
        /// <param name="args"></param>
        public void onGunGu(ChangeEventArgs args)
        {
            ann.Adress_GunGu = args.Value.ToString();
            bnn.GunGu = ann.Adress_GunGu;
            //StateHasChanged();
        }

        /// <summary>
        /// 업체 정보 등록
        /// </summary>
        /// <returns></returns>
        public async Task btnCompanySave()
        {
            bnn.Telephone = ann.Telephone;
            if (bnn.Company_Sort == "" || bnn.Company_Sort == null)
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "업체분류를 선택해 주세요...");
            }
            else if (bnn.TypeOfBusiness == "" || bnn.TypeOfBusiness == null)
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "업종을 입력해 주세요...");
            }
            else if (bnn.BusinessConditions == "" || bnn.BusinessConditions == null)
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "업태를 입력해 주세요...");
            }
            else if (ann.CorporateResistration_Num == "" || ann.CorporateResistration_Num == null)
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "사업자번호를 입력해 주세요...");
            }
            else if (ann.Cor_Name == "" || ann.Cor_Name == null)
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "업체명을 입력해 주세요...");
            }
            else if (bnn.GunGu == "" || bnn.GunGu == null)
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "시군구를 입력해 주세요...");
            }
            else if (bnn.Adress == "" || bnn.Adress == null)
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "상세 주소를 입력해 주세요...");
            }
            else if (bnn.Telephone == "" || bnn.Telephone == null)
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "대표번호를 입력해 주세요...");
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
                ann.User_Code = User_Code;
                bnn.PostIP = myIPAddress;
                #endregion
                ann.Adress_Rest = bnn.Adress;
                ann.LevelCount = 3;
                ann.Intro = bnn.Etc;
                ann.Cor_Code = await sido_Lib.Region_Code(bnn.GunGu) + await company_Lib.Num_Count();

                int intA = 0;
                //try
                //{
                    if (ann.Aid < 1)
                    {
                        intA = await company_Lib.Add(ann);
                    }
                    else
                    {
                        await company_Lib.edit(ann);
                    }
                //}
                //catch (Exception e)
                //{
                //    await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", e);
                //}

                
                    bnn.Adress = ann.Adress_Rest;
                    bnn.Ceo_Mobile = ann.Mobile;
                    bnn.Ceo_Name = ann.Ceo_Name;
                    bnn.Telephone = ann.Telephone;
                    bnn.PostIP = ann.PostIP;

                    bnn.Company_Code = ann.Cor_Code;
                    bnn.Sido = ann.Adress_Sido;
                    bnn.GunGu = ann.Adress_GunGu;

                    //try
                    //{
                        if (bnn.Aid < 1)
                        {
                            if (intA > 0)
                            {
                                await company_sub_Lib.Add(bnn); 
                            }
                        }
                        else
                        {
                            await company_sub_Lib.Edit(bnn);
                        }
                //}
                //catch (Exception e)
                //{
                //    await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", e);
                //}

                ann = new Company_Entity();
                bnn = new Company_Sub_Entity();               
                
                InsertViews = "A";

                await DisplayData();
            }
        }

        /// <summary>
        /// 업체 정보 입력
        /// </summary>
        public void btnClose()
        {
            InsertViews = "A";
            //StateHasChanged();
        }

        /// <summary>
        /// 업체 상세보기
        /// </summary>
        /// <param name="entity"></param>
        public void ByDetails(Company_Join_Entity entity)
        {
            Views = "B";
            Join_Entity = entity;
            //Confirm = "B";
            //StateHasChanged();
        }

        /// <summary>
        /// 업체 정보 수정
        /// </summary>
        /// <param name="entity"></param>
        public async Task ByEdit(Company_Join_Entity cj)
        {
            InsertViews = "B";

            #region 시도 
            if (cj.Sido == "서울특별시")
            {
                Sido_Code = "A";
            }
            else if (cj.Sido == "경기도")
            {
                Sido_Code = "B";
            }
            else if (cj.Sido == "부산광역시")
            {
                Sido_Code = "C";
            }
            else if (cj.Sido == "대구광역시")
            {
                Sido_Code = "D";
            }
            else if (cj.Sido == "인천광역시")
            {
                Sido_Code = "E";
            }
            else if (cj.Sido == "광주광역시")
            {
                Sido_Code = "F";
            }
            else if (cj.Sido == "대전광역시")
            {
                Sido_Code = "G";
            }
            else if (cj.Sido == "울산광역시")
            {
                Sido_Code = "H";
            }
            else if (cj.Sido == "세종특별자치시")
            {
                Sido_Code = "I";
            }
            else if (cj.Sido == "충청남도")
            {
                Sido_Code = "J";
            }
            else if (cj.Sido == "충청북도")
            {
                Sido_Code = "K";
            }
            else if (cj.Sido == "경상남도")
            {
                Sido_Code = "L";
            }
            else if (cj.Sido == "경상북도")
            {
                Sido_Code = "M";
            }
            else if (cj.Sido == "전라남도")
            {
                Sido_Code = "N";
            }
            else if (cj.Sido == "전라북도")
            {
                Sido_Code = "O";
            }
            else if (cj.Sido == "강원도")
            {
                Sido_Code = "P";
            }
            else if (cj.Sido == "제주특별자치도")
            {
                Sido_Code = "Q";
            }
            #endregion

            sidos = await sido_Lib.GetList(cj.Adress_Sido);
            var strA = await contract_Sort_Lib.Detail(cj.Company_Sort);
            Sort_NameA = strA.Up_Code;
            lstC = await contract_Sort_Lib.List(strA.Up_Code);

            //Sido_Code = await sido_Lib.Region_Code(ann.Adress_GunGu);

            ann.Adress_GunGu = cj.Adress_GunGu;
            ann.Adress_Rest = cj.Adress_Rest;
            ann.Adress_Sido = cj.Adress_Sido;
            ann.Aid = cj.Aid;
            ann.Ceo_Name = cj.Ceo_Name;
            ann.CorporateResistration_Num = cj.CorporateResistration_Num;
            ann.Cor_Code = cj.Cor_Code;
            ann.Cor_Name = cj.Cor_Name;
            ann.Intro = cj.Intro;
            ann.LevelCount = cj.LevelCount;
            ann.Mobile = cj.Mobile;
            ann.Telephone = cj.Telephone;

            bnn.Adress = cj.Adress;            
            bnn.Aid = cj.SubAid;
            bnn.BusinessConditions = cj.BusinessConditions;
            bnn.CapitalStock = cj.CapitalStock;
            bnn.Ceo_Mobile = cj.Ceo_Mobile;
            bnn.Ceo_Name = cj.Ceo_Name;
            bnn.ChargeMan = cj.ChargeMan;
            bnn.ChargeMan_Mobile = cj.ChargeMan_Mobile;
            bnn.Company_Code = cj.Company_Code;
            bnn.Company_Sort = cj.Company_Sort;
            bnn.CraditRating = cj.CraditRating;
            bnn.Email = cj.Email;
            bnn.Etc = cj.Etc;
            bnn.Fax = cj.Fax;
            bnn.GunGu = cj.GunGu;
            bnn.Sido = cj.Sido;
            bnn.PostIP = cj.PostIP;
            bnn.Telephone = cj.Telephone;
            bnn.TypeOfBusiness = cj.TypeOfBusiness;
          
            Confirm = "B";
        }

        public void ByRemove(string Code)
        {
            //await company_Lib.Remove(Code);
            //await company_sub_Lib.
        }

        /// <summary>
        /// 업체 검색
        /// </summary>
        public string strSearch { get; set; } = "A";
        private void OnCompnaySearch()
        {
            strSearch = "B";
        }
    }
}
