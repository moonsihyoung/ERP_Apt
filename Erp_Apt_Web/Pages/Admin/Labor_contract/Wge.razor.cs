using Company;
using Erp_Apt_Lib.Logs;
using Erp_Apt_Lib;
using Erp_Apt_Staff;
using Erp_Entity;
using Erp_Lib;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.Linq;
using sw_Lib.Labors;

namespace Erp_Apt_Web.Pages.Admin.Labor_contract
{
    public partial class Wge
    {
        #region 인스턴스
        [CascadingParameter] public Task<AuthenticationState> AuthenticationStateRef { get; set; }
        [Inject] public NavigationManager MyNav { get; set; } // Url 
        [Inject] IJSRuntime JSRuntime { get; set; }
        [Inject] ILogs_Lib logs_Lib { get; set; }
        [Inject] public Iwage_Lib wage_Lib { get; set; }        

        #endregion

        #region 변수
        public string User_Code { get; set; }
        public string User_Name { get; set; }
        public string Apt_Code { get; set; }
        public string Apt_Name { get; set; }
        public int LevelCount { get; set; } = 0;
        public string InsertViews { get; set; } = "A"; // 입력 열기
        #endregion

        #region 속성
        List<wage_Entity> ann { get; set; } = new List<wage_Entity>();
        wage_Entity bnn { get; set; } = new wage_Entity();
        
        #endregion


        #region 페이징 로그
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
        /// 로그입력
        /// </summary>
        Logs_Entites logs { get; set; } = new Logs_Entites();
        protected async Task Loks(string Note, string Application, string Logger, string Message)
        {
            logs.Apt_Code = Apt_Code;
            logs.Note = Note;
            logs.Application = Application;
            logs.Logger = Logger;
            logs.Message = Message;

            await logs_Lib.add(logs); //로그입력
        } 
        #endregion

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
        /// 배치정보 목록 불러오기
        /// </summary>
        private async Task DisplayData()
        {
            pager.RecordCount = await wage_Lib.GetList_Count();

            ann = await wage_Lib.GetList(pager.PageIndex);
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
        /// 최저임금 입력 열기
        /// </summary>
        private void btnOpen()
        {
            InsertViews = "B";
        }

        /// <summary>
        /// 최저임금 입력
        /// </summary>
        /// <returns></returns>
        private async Task btnSave()
        {
            bnn.User_Code = User_Code;            
            await wage_Lib.Add(bnn);

            await Loks(bnn.Details, "wages", bnn.User_Code, "최저임금 입력");

            await DisplayData();
            bnn = new();
            InsertViews = "A";
        }
    }
}
