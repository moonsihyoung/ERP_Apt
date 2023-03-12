using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Erp_Lib;

namespace Mobile.Pages.CarInfor
{
    public partial class Index
    {
        [Parameter] public string Code { get; set; }
        [CascadingParameter] public Task<AuthenticationState> AuthenticationStateRef { get; set; }
        [Inject] public NavigationManager MyNav { get; set; }
        [Inject] IJSRuntime JSRuntime { get; set; }
        [Inject] IHttpContextAccessor contextAccessor { get; set; }
        [Inject] ICar_Infor_Lib car { get; set; }

        List<Car_Infor_entity> ann { get; set; } = new List<Car_Infor_entity>();

        public string User_Code { get; set; }
        public string User_Name { get; set; }
        public string Dong { get; private set; }
        public string Ho { get; private set; }
        public string Apt_Code { get; set; }
        public string Apt_Name { get; set; }

        // <summary>
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
                Dong = authState.User.Claims.FirstOrDefault(c => c.Type == "Dong")?.Value;
                Ho = authState.User.Claims.FirstOrDefault(c => c.Type == "Ho")?.Value;


                await DisplayData();

            }
            else
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "로그인되지 않았습니다..");
                MyNav.NavigateTo("/");
            }
        }

        /// <summary>
        /// 입력된 데이터 로드
        /// </summary>
        private async Task DisplayData()
        {
            ann = await car.GetList_Car_People(Apt_Code, Dong, Ho);
        }
    }
}
