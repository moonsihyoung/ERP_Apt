using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Threading.Tasks;
using Erp_Lib;
using System.Collections.Generic;
using Erp_Entity;
using Erp_Apt_Lib.Logs;
using Erp_Apt_Staff;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Hosting;
using System.Linq;
using System;
using System.Security.Cryptography;
using Works;
using System.Net;

namespace Erp_Apt_Web.Pages.Admin.PostDuty
{
    public partial class Index
    {
        [CascadingParameter] public Task<AuthenticationState> AuthenticationStateRef { get; set; }
        [Inject] public NavigationManager MyNav { get; set; } // Url 
        [Inject] IJSRuntime JSRuntime { get; set; }
        [Inject] public ILogs_Lib logs_Lib { get; set; }
        [Inject] public IPost_Lib post_Lib { get; set; }
        [Inject] public IDuty_Lib duty_Lib { get; set; }

        public Logs_Entites logs { get; set; } = new Logs_Entites();
        List<Post_Entity> annP { get; set; } = new List<Post_Entity>();
        List<Duty_Entity> annD { get; set; } = new List<Duty_Entity>();
        public List<Post_Entity> Post { get; set; } = new List<Post_Entity>();
        public List<Duty_Entity> Duty { get; set; } = new List<Duty_Entity>();

        Post_Entity bnnP { get; set; } = new Post_Entity();
        Duty_Entity bnnD { get; set; } = new Duty_Entity();

        public string User_Code { get; set; }
        public string User_Name { get; set; }
        public string Apt_Code { get; set; }
        public string Apt_Name { get; set; }
        public int LevelCount { get; set; } = 0;
        public string Views { get; set; } = "A"; //상세 열기
        public string InsertViewsP { get; set; } = "A";//삭제 열기
        public string InsertViewsD { get; set; } = "A"; // 입력 열기

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

                #region 로그 파일 만들기
                logs.Note = "직원관리에 들어왔습니다."; logs.Logger = User_Code; logs.Application = "직원관리"; logs.ipAddress = ""; logs.Message = Apt_Code + " " + Apt_Name;
                await logs_Lib.add(logs);
                #endregion

                if (LevelCount >= 5)
                {
                    await DisplayData();
                    Post = await post_Lib.GetListAll();
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

        private void OnPost(ChangeEventArgs a)
        {
            if (!string.IsNullOrWhiteSpace(a.Value.ToString()))
            {
                bnnD.PostCode = a.Value.ToString();
            }
        }

        /// <summary>
        /// 배치정보 목록 불러오기
        /// </summary>
        private async Task DisplayData()
        {  
            annP = await post_Lib.GetListAll();
            pager.PageCount = await duty_Lib.GetListAll_Count();
            annD = await duty_Lib.GetListAll(pager.PageIndex);
        }

        /// <summary>
        /// 모달 이동 
        /// </summary>
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await JSRuntime.InvokeVoidAsync("setModalDraggableAndResizable");
            await base.OnAfterRenderAsync(firstRender);
        }

        private void btnPostInput()
        {
            InsertViewsP = "B";
        }

        private void btnDutyInput()
        {
            InsertViewsD = "B";
        }

        private void ByUpdateP(Post_Entity ar)
        {
            bnnP = ar;
        }

        private void ByRemoveP(Post_Entity ar)
        {
            bnnP = ar;
        }

        private void ByUpdateD(Duty_Entity ar)
        {
            bnnD = ar;
        }

        private async Task ByRemoveD(Duty_Entity ar)
        {
            bool isDelete = await JSRuntime.InvokeAsync<bool>("confirm", $"{ar.DutyName}번 직책을 정말로 삭제하시겠습니까?");
            if (isDelete)
            {
                //svDate = DateTime.Now.Date;
                await duty_Lib.Remove(ar.DutyCode);
                await DisplayData();
            }
        }

        /// <summary>
        /// 직책 입력
        /// </summary>
        private async Task btnSaveD()
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
            bnnD.PostIP = myIPAddress;
            #endregion
            if (string.IsNullOrWhiteSpace(bnnD.Division))
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "구분을 입력하지 않았습니다..");
            }
            else if (string.IsNullOrWhiteSpace(bnnD.DutyName))
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "직책명을 입력하지 않았습니다..");
            }
            else if (string.IsNullOrWhiteSpace(bnnD.PostCode))
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "부서 코드를 입력하지 않았습니다..");
            }
            else if (string.IsNullOrWhiteSpace(bnnD.Etc))
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "설명을 입력하지 않았습니다..");
            }
            else
            {
                if (bnnD.DutyCode < 1)
                {
                    await duty_Lib.Add(bnnD);
                }
                else
                {
                    await duty_Lib.Edit(bnnD);
                }
                await DisplayData();
            }
        }

        /// <summary>
        /// 직책 열기 닫기
        /// </summary>
        private void btnCloseD()
        {
            InsertViewsD = "A";
        }
    }
}
