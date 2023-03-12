using Erp_Lib;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace Car_Infor_Web.Pages.Car_Infor { 
    public partial class CarInfor
    {

        List<Car_Infor_entity> car = new List<Car_Infor_entity>();
        Car_Infor_entity ann = new Car_Infor_entity();
        [Inject] public ICar_Infor_Lib lstCar { get; set; }
        [Inject] public NavigationManager MyNav { get; set; }
        [CascadingParameter] public Task<AuthenticationState> AuthenticationStateRef { get; set; }
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




        private async Task btnSave()
        {
            if (ann.Car_Num != null)
            {
                car = await lstCar.Search_Car("B1011645", ann.Car_Num);

                if (car != null)
                {
                    ann.Car_Num = "";
                }
            }
        }



    }
}
