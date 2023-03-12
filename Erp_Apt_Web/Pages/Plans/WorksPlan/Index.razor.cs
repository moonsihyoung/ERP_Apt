using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Threading.Tasks;
using Erp_Apt_Lib.Plans;
using Microsoft.AspNetCore.Http;
using Erp_Lib;
using Facilities;
using Erp_Apt_Staff;
using System.Collections.Generic;
using Erp_Entity;
using Erp_Apt_Lib.Community;
using System.Linq;
using System;
using System.Net;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using Works;
using Microsoft.Extensions.Hosting;

namespace Erp_Apt_Web.Pages.Plans.WorksPlan
{
    public partial class Index
    {
        [Parameter] public string Code { get; set; }
        [CascadingParameter] public Task<AuthenticationState> AuthenticationStateRef { get; set; }
        [Inject] public NavigationManager MyNav { get; set; }
        [Inject] IJSRuntime JSRuntime { get; set; }
        [Inject] IPlan_Lib plan_Lib { get; set; }
        [Inject] IPlan_Sort_Lib plan_Sort { get; set; }
        [Inject] public IPost_Lib Post_Lib { get; set; }
        [Inject] public IDuty_Lib duty_Lib { get; set; }
        [Inject] public IBloom_Lib bloom_Lib { get; set; }
        [Inject] public IReferral_career_Lib referral_Career_Lib { get; set; }
        [Inject] IHttpContextAccessor contextAccessor { get; set; }

        List<Plan_Entity> ann { get; set; } = new List<Plan_Entity>();
        Plan_Entity bnn { get; set; } = new Plan_Entity();
        List<Plan_Sort_Entity> cnn { get; set;} = new List<Plan_Sort_Entity>(); //계획 분류
        Plan_Sort_Entity dnn { get; set; } = new Plan_Sort_Entity();
        List<Post_Entity> pnn { get; set; } = new List<Post_Entity>();
        List<Duty_Entity> dnnA { get; set; } = new List<Duty_Entity>();
        List<Duty_Entity> dnnB { get; set; } = new List<Duty_Entity>();
        List<Referral_career_Entity> rnnA = new List<Referral_career_Entity>();
        List<Referral_career_Entity> rnnB = new List<Referral_career_Entity>();
        List<Bloom_Entity> bloom_A { get; set; } = new List<Bloom_Entity>();
        List<Bloom_Entity> bloom_B { get; set; } = new List<Bloom_Entity>();
        List<Bloom_Entity> bloom_C { get; set; } = new List<Bloom_Entity>();
        List<Plan_Sort_Entity> sort { get; set; } = new List<Plan_Sort_Entity>();
        List<Plan_Sort_Entity> aort { get; set; } = new List<Plan_Sort_Entity>();

        public string User_Code { get; set; }
        public string User_Name { get; set; }
        public int LevelCount { get; private set; }
        public string Apt_Code { get; set; }
        public string Apt_Name { get; set; }

        public string strPost { get; set; }
        public string strPostA { get; set; }
        public string strDuty { get; set; }
        public string strSort { get; set; }
        public string strAsort { get; set; }
        public string InsertViews { get; set; } = "A";
        public string SortViews { get; set; } = "A";
        public string SortInsViews { get; set; } = "A";
        public string DetailsViews { get; set; } = "A";

        public string strWSortA { get; set; }
        public string strWSortB { get; set; }
        public string strWSortC { get; set; }

        public string strTitle { get; set; }


        /// <summary>
        /// 페이징
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
        /// 페이징
        /// </summary>
        protected DulPager.DulPagerBase pagerA = new DulPager.DulPagerBase()
        {
            PageNumber = 1,
            PageIndex = 0,
            PageSize = 15,
            PagerButtonCount = 5
        };

        /// <summary>
        /// 페이징
        /// </summary>
        protected async void PageIndexChangedA(int pageIndex)
        {
            pagerA.PageIndex = pageIndex;
            pagerA.PageNumber = pageIndex + 1;

            await DisplayData();

            StateHasChanged();
        }

        /// <summary>
        /// 로드시 실행
        /// </summary>
        protected override async Task OnInitializedAsync()
        {
            var authState = await AuthenticationStateRef;
            if (authState.User.Identity.IsAuthenticated)
            {
                Apt_Code = authState.User.Claims.FirstOrDefault(c => c.Type == "Apt_Code")?.Value;
                Apt_Name = authState.User.Claims.FirstOrDefault(c => c.Type == "Apt_Name")?.Value;
                User_Code = authState.User.Claims.FirstOrDefault(c => c.Type == "User_Code")?.Value;
                User_Name = authState.User.Claims.FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name")?.Value;
                LevelCount = Convert.ToInt32(authState.User.Claims.FirstOrDefault(c => c.Type == "LevelCount")?.Value);

                pnn = await Post_Lib.GetList("A");//부서 목록
                bloom_A = await bloom_Lib.GetList_Apt_ba(Apt_Code); // 작업 대분류
                sort = await plan_Sort.SortList();
                await DisplayData();
            }
            else
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "로그인되지 않았습니다..");
                MyNav.NavigateTo("/");
            }
        }

        private async Task DisplayData()
        {
            pager.RecordCount = await plan_Lib.Apt_List_Count(Apt_Code);
            ann = await plan_Lib.Apt_List(pager.PageIndex, Apt_Code);
        }




        /// <summary>
        /// 아이피 추출 
        /// </summary>
        public string MyIpAdress { get; set; }
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await JSRuntime.InvokeVoidAsync("setModalDraggableAndResizable");
            await base.OnAfterRenderAsync(firstRender);

            if (firstRender)
            {
                MyIpAdress = contextAccessor.HttpContext.Connection?.RemoteIpAddress.ToString();
                StateHasChanged();
            }
        }

        /// <summary>
        /// 계획명 선택
        /// </summary>
        private async Task OnPlan_Name(ChangeEventArgs a)
        {
            if (a.Value != null)
            {
                bnn.Plan_Code = a.Value.ToString();
                strSort = bnn.Plan_Code;
                bnn.Plan_Name = await plan_Sort.SortNaneAsync(bnn.Plan_Code);
                bnn.Sort = bnn.Plan_Name;
                bnn.Sort_Code = bnn.Plan_Code;
                aort = await plan_Sort.AsortList(strSort); 
            }
        }

        /// <summary>
        /// 분류명 선택
        /// </summary>
        private async void OnAsort(ChangeEventArgs a)
        {
            if (a.Value != null)
            {
                bnn.Asort_Code = a.Value.ToString();
                strAsort = bnn.Asort_Code;
                bnn.Asort = await plan_Sort.SortNaneAsync(bnn.Plan_Code);
            }
        }

        #region 작업분류 관련
        /// <summary>
        /// 대분류 선택
        /// </summary>
        private async Task onWSortA(ChangeEventArgs a)
        {
            bnn.BloomA_Code = a.Value.ToString();
            strWSortA = a.Value.ToString();
            bnn.BloomA = await bloom_Lib.Sort_Name(strWSortA, "A");
            bloom_B = await bloom_Lib.GetList_bb(bnn.BloomA);
            bnn.BloomA = strWSortA;
            bloom_C = new List<Bloom_Entity>();
            strWSortB = "";
            strWSortC = "";
        }

        /// <summary>
        /// 중분류 선택
        /// </summary>
        private async Task onWSortB(ChangeEventArgs a)
        {
            string strA = a.Value.ToString();
            strWSortB = strA;
            strWSortC = "";
            bnn.BloomB_Code = strA;
            bnn.BloomB = await bloom_Lib.Sort_Name(strA, "B");
            bloom_C = await bloom_Lib.GetList_cc(bnn.BloomB);
            
        }

        /// <summary>
        /// 세분류 선택
        /// </summary>
        private async void onWSortC(ChangeEventArgs a)
        {
            string strA = a.Value.ToString();
            bnn.BloomC_Code = strA;
            bnn.BloomC = await bloom_Lib.Sort_Name(strA, "C");
        }        
        #endregion

        /// <summary>
        /// 부선 선택
        /// </summary>
        private async Task OnPost(ChangeEventArgs a)
        {
            strPost = a.Value.ToString();
            bnn.Post = await Post_Lib.PostName(strPost);

            dnnA = await duty_Lib.GetList(strPost, "A");
            rnnA = await referral_Career_Lib.GetList_Post_Staff_be(bnn.Post, Apt_Code);
        }

        /// <summary>
        /// 부선 선택
        /// </summary>
        private async Task OnPostA(ChangeEventArgs a)
        {
            strPostA = a.Value.ToString();
            bnn.W_Post = await Post_Lib.PostName(strPost);

            dnnB = await duty_Lib.GetList(strPostA, "A");
            rnnB = await referral_Career_Lib.GetList_Post_Staff_be(bnn.W_Post, Apt_Code);
        }

        private void btnOpen()
        {
            strTitle = "각종 계획 내용 입력";
            bnn = new Plan_Entity();
            bnn.Month = 0;
            InsertViews = "B";
        }

        /// <summary>
        /// 수정하기
        /// </summary>
        /// <param name="plan"></param>
        private void ByEdit(Plan_Entity plan)
        {
            bnn = plan;
            InsertViews = "B";
        }

        /// <summary>
        /// 삭제하기
        /// </summary>
        private async Task ByRemove(Plan_Entity plan)
        {
            bool isDelete = await JSRuntime.InvokeAsync<bool>("confirm", $"{plan.Aid}번 계획을 정말로 삭제하시겠습니까?");
            if (isDelete)
            {
                await plan_Lib.Delete(plan.Aid);
                await DisplayData();
            }
        }

        /// <summary>
        /// 분류 수정하기
        /// </summary>
        private void ByEditA(Plan_Sort_Entity plan)
        {
            dnn = plan;
            SortInsViews = "B";
        }

        /// <summary>
        /// 분류 삭제하기
        /// </summary>
        private async Task ByRemoveA(Plan_Sort_Entity plan)
        {
            bool isDelete = await JSRuntime.InvokeAsync<bool>("confirm", $"{plan.Aid}번 계획을 정말로 삭제하시겠습니까?");
            if (isDelete)
            {
                await plan_Sort.Delete(plan.Aid);
                await DisplayData();
            }
        }

        /// <summary>
        /// 계획 상세보기
        /// </summary>
        private void ByDetails(Plan_Entity plan)
        {
            bnn = plan;
            DetailsViews = "B";
        }

        /// <summary>
        /// 계획 등록
        /// </summary>
        /// <returns></returns>
        private async Task btnSave()
        {
            var ar = await plan_Sort.Sort_Details(bnn.Plan_Code);
            bnn.Apt_Code = Apt_Code;
            bnn.User_Code = User_Code;
            bnn.User_Name = User_Name;
            bnn.Law_Division = ar.Law_Division;
            
           if (string.IsNullOrEmpty(bnn.Apt_Code))
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "로그인되지 않았습니다..");
            }
            else if (string.IsNullOrEmpty(bnn.Post))
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "관리자 부서를 선택하지 않았습니다..");
            }
            else if (bnn.Month < 1)
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "집행월을 선택하지 않았습니다..");
            }
            else if (string.IsNullOrWhiteSpace(bnn.Plan_Name))
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "계획명을 선택하지 않았습니다..");
            }
            else if (string.IsNullOrWhiteSpace(bnn.Asort))
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "업무명을 선택하지 않았습니다..");
            }
            else if (string.IsNullOrWhiteSpace(bnn.Worker))
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "주관리자를 선택하지 않았습니다..");
            }
            else if (string.IsNullOrWhiteSpace(bnn.Plan_Details))
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "계획 상세를 입력하지 않았습니다..");
            }
            else if (string.IsNullOrWhiteSpace(bnn.Menager))
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "부관리자를 선택하지 않았습니다..");
            }
            else if (string.IsNullOrWhiteSpace(bnn.BloomC))
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "분류를 선택하지 않았습니다..");
            }
            else
            {
                bnn.PostIP = MyIpAdress;

                if (bnn.Aid < 1)
                {
                    bnn.StartDate = DateTime.Now;
                    bnn.EndDate = DateTime.Now;
                    bnn.Year = DateTime.Now.Year;

                    await plan_Lib.Add(bnn);
                    await DisplayData();
                }
                else
                {
                    await plan_Lib.Update(bnn);
                    await DisplayData();
                    DetailsViews = "A";
                }
            }            
        }

        /// <summary>
        /// 계획 입력닫기
        /// </summary>
        private void btnClose()
        {
            InsertViews = "A";
        }

        /// <summary>
        /// 계획 분류 목록 닫기
        /// </summary>
        private void btnCloseA()
        {
            SortViews = "A";
        }

        /// <summary>
        /// 분류 입력 닫기
        /// </summary>
        private void btnCloseB()
        {
            SortInsViews = "A";
        }

        /// <summary>
        /// 계획분류 목록 열기
        /// </summary>
        /// <returns></returns>
        private async Task btnSortOpen()
        {
            SortViews = "B";
            strTitle = "계획 분류 관리 정보";
            cnn = await plan_Sort.SortList_All();
        }

        /// <summary>
        /// 분류 입력 열기
        /// </summary>
        private void btnSaveOpen()
        {
            dnn = new Plan_Sort_Entity();
            SortInsViews = "B";
        }

        /// <summary>
        /// 계획 분류 새로 등록
        /// </summary>
        private async Task btnSaveB()
        {
            dnn.Apt_Code = Apt_Code;
            dnn.PostIP = MyIpAdress;
            dnn.User_Code = User_Code;

            if (string.IsNullOrEmpty(dnn.Apt_Code))
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "로그인되지 않았습니다..");
            }
            else if (dnn.Sort_Name == null || dnn.Sort_Name == "")
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "분류명을 입력하지 않았습니다..");
            }
            else if (dnn.Division == null || dnn.Division == "")
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "구분을 선택하지 않았습니다..");
            }
            else if (dnn.Law_Division == null || dnn.Law_Division == "")
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "법정여부를 선택하지 않았습니다..");
            }
            else
            {
                if (dnn.Aid < 1)
                {
                    if (dnn.Division == "A")
                    {
                        dnn.Sort_Code = (await plan_Sort.Last_Aid() +1).ToString();
                        dnn.Asort_Code = "A";
                    }
                    else if (dnn.Division == "B")
                    {
                        dnn.Asort_Code = ((await plan_Sort.Last_Aid()) +1).ToString();
                    }

                    await plan_Sort.Add(dnn);
                }
                else
                {
                    await plan_Sort.Update(dnn);
                }

                cnn = await plan_Sort.SortList();
                SortViews = "B";
            }          
        }

        /// <summary>
        /// 계획 구분 선택 실행
        /// </summary>
        public string strDivision { get; set; } = "A";
        private async Task OnDivision(ChangeEventArgs a)
        {
            dnn.Division = a.Value.ToString();
            if (dnn.Division == "B")
            {
                sort = await plan_Sort.SortList(); 
            }
        }

        /// <summary>
        /// 대분류 선택
        /// </summary>
        /// <param name="a"></param>
        private void OnSort(ChangeEventArgs a)
        {
            dnn.Sort_Code = a.Value.ToString();

        }

        /// <summary>
        /// 상세보기 닫기
        /// </summary>
        private void btnCloseV()
        {
            DetailsViews = "A";
        }
    }
}
