using Erp_Apt_Lib.Appeal;
using Erp_Apt_Lib.Logs;
using Erp_Lib;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Works;

namespace Erp_Apt_Web.Pages
{
    public partial class Index
    {
        [CascadingParameter] public Task<AuthenticationState> AuthenticationStateRef { get; set; }
        [Inject] public NavigationManager MyNav { get; set; } // Url 
        [Inject] Microsoft.AspNetCore.Http.IHttpContextAccessor HttpContextAccessor { get; set; }
        [Inject] public IApt_Lib apt_Lib { get; set; }
        [Inject] public IAppeal_Lib appeal { get; set; }
        [Inject] public IWorks_Lib works_Lib { get; set; }
        [Inject] IJSRuntime iJSRuntime { get; set; }


        public string Apt_Code { get; private set; }
        public string User_Code { get; private set; }
        public string Apt_Name { get; private set; }
        public string User_Name { get; private set; }

        List<Appeal_Entity> ann = new List<Appeal_Entity>();
        List<Works_Entity> bnn = new List<Works_Entity>();

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
                    ann = await appeal.AppealListComplete(Apt_Code);
                    bnn = await works_Lib.ServiceListComplete(Apt_Code);
                    //await Logs();
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
            dnn.Message = "관리전산을 방문" + Apt_Name;
            dnn.MessageTemplate = "";
            dnn.Properties = "";
            dnn.TimeStamp = DateTime.Now.ToShortDateString();
            await logs_Lib.add(dnn);
        }

        /// <summary>
        /// 민원 상세보기로 이동
        /// </summary>
        private void ByAidAppeal(int Num)
        {
            MyNav.NavigateTo("/Complain/Details/" + Num);
        }

        /// <summary>
        /// 민원 상세보기로 이동
        /// </summary>
        private void ByAidWorks(int Num)
        {
            MyNav.NavigateTo("/Works/Details/" + Num);
        }

        private void GenerateExcel()
        {
            MyNav.NavigateTo("http://net.wedew.co.kr/Excel/GetExcelFiles?AptCode=B812245&StartDate=2022-06-01&EndDate=2022-06-30", true);
        }
    }
}
