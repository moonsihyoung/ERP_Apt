using Erp_Apt_Lib.Appeal;
using Erp_Apt_Lib.Logs;
using Erp_Lib;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Works;

namespace Car_Infor_Web.Pages
{
    public partial class Index
    {
        [CascadingParameter] public Task<AuthenticationState> AuthenticationStateRef { get; set; }
        [Inject] public NavigationManager MyNav { get; set; } // Url 
        [Inject] Microsoft.AspNetCore.Http.IHttpContextAccessor HttpContextAccessor { get; set; }
        [Inject] public IApt_Lib apt_Lib { get; set; }


        public string Apt_Code { get; private set; }
        public string User_Code { get; private set; }
        public string Apt_Name { get; private set; }
        public string User_Name { get; private set; }

        protected override async Task OnInitializedAsync()
        {
            var asa = await apt_Lib.Apt_Name("sw5");
            var authState = await AuthenticationStateRef;
            if (authState.User.Identity.IsAuthenticated)
            {
                //로그인 정보
                Apt_Code = authState.User.Claims.FirstOrDefault(c => c.Type == "Apt_Code")?.Value;
                User_Code = authState.User.Claims.FirstOrDefault(c => c.Type == "User_Code")?.Value;
                Apt_Name = authState.User.Claims.FirstOrDefault(c => c.Type == "Apt_Name")?.Value;
                User_Name = authState.User.Claims.FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name")?.Value;
            }
            else
            {
                MyNav.NavigateTo("/Home/Index", true);
            }
        }

        /// <summary>
        /// 로그인
        /// </summary>
        [Inject] ILogs_Lib logs_Lib { get; set; }
        private async Task Logs()
        {
            Logs_Entites dnn = new Logs_Entites();
            dnn.Apt_Code = Apt_Code;
            dnn.Note = User_Name;
            dnn.Application = "메인";
            dnn.LogEvent = "클릭";
            dnn.Callsite = "";
            dnn.Exception = "";
            dnn.ipAddress = HttpContextAccessor.HttpContext.Connection?.RemoteIpAddress.ToString();
            dnn.Level = "3";
            dnn.Logger = User_Code;
            dnn.Message = "자동차 정보 찾기에 방문" + Apt_Name;
            dnn.MessageTemplate = "";
            dnn.Properties = "";
            dnn.TimeStamp = DateTime.Now.ToShortDateString();
            await logs_Lib.add(dnn);
        }
    }
}
