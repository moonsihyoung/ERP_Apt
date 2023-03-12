using Company;
using Erp_Apt_Lib.Logs;
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

namespace Erp_Apt_Web.Pages.Staff
{
    public partial class My_Page
    {
        [CascadingParameter] public Task<AuthenticationState> AuthenticationStateRef { get; set; }
        [Inject] public NavigationManager MyNav { get; set; } // Url 
        [Inject] IJSRuntime JSRuntime { get; set; }
        [Inject] public IReferral_career_Lib referral_Career_Lib { get; set; }
        [Inject] public IStaff_Lib staff_Lib { get; set; }
        [Inject] public IStaff_staffSub_Lib staff_StaffSub_Lib { get; set; }
        [Inject] public IStaff_Sub_Lib staff_Sub_Lib { get; set; }
        [Inject] public ISido_Lib sido_Lib { get; set; }
        [Inject] public IPost_Lib post_Lib { get; set; }
        [Inject] public IDuty_Lib duty_Lib { get; set; }
        [Inject] public ICompany_Career_Lib company_Career_Lib { get; set; }
        [Inject] public ILogs_Lib logs_Lib { get; set; }

        public Staff_Career_Entity ann { get; set; } = new Staff_Career_Entity();
        public Referral_career_Entity rce { get; set; } = new Referral_career_Entity();
        public Staff_Entity bnn { get; set; } = new Staff_Entity();
        public Staff_Sub_Entity cnn { get; set; } = new Staff_Sub_Entity();
        public Staff_Career_Entity dnn { get; set; } = new Staff_Career_Entity();
        public Sido_Entity sido_Entity { get; set; } = new Sido_Entity();
        public List<Post_Entity> Post { get; set; } = new List<Post_Entity>();
        public List<Duty_Entity> Duty { get; set; } = new List<Duty_Entity>();

        public string User_Code { get; set; }
        public string User_Name { get; set; }
        public string Apt_Code { get; set; }
        public string Apt_Name { get; set; }
        public int LevelCount { get; set; } = 0;

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
                    Post = await post_Lib.GetList("A");
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
            ann = await referral_Career_Lib.Details_Staff_Career_Join(User_Code);            
        }
    }
}
