using Erp_Entity;
using Erp_Lib;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Mobile.Pages.Admin
{
    public partial class Details
    {
        [CascadingParameter] public Task<AuthenticationState> AuthenticationStateRef { get; set; }
        [Inject] public ISido_Lib sido_Lib { get; set; }
        [Inject] public IApt_Lib apt_Lib { get; set; }
        [Inject] public IErp_AptPeople_Lib aptPeople_Lib { get; set; } // 입주자 카드 정보 클래스
        [Inject] public IIn_AptPeople_Lib in_AptPople_Lib { get; set; } // 홈페이지 가입회원 정보 클래스
        [Inject] IJSRuntime JSRuntime { get; set; }
        [Inject] public NavigationManager MyNav { get; set; } // Url 


        //List<Sido_Entity> sidos { get; set; } = new List<Sido_Entity>(); // 시도 정보
        List<Apt_Entity> apts { get; set; } = new List<Apt_Entity>(); //공동주택 정보
        List<Apt_People_Entity> apt_PoplesA { get; set; } = new List<Apt_People_Entity>(); //입주민 목록 정보
        List<Apt_People_Entity> apt_PoplesB { get; set; } = new List<Apt_People_Entity>(); //입주민 목록 정보
        Apt_People_Entity dnn { get; set; } = new Apt_People_Entity(); //입주민 정보

        public In_AptPeople_Entity ann { get; set; } = new In_AptPeople_Entity();// 홈페이지 입주민 가입자

        public string User_Code { get; set; }
        public string User_Name { get; set; }
        public string Apt_Code { get; set; }
        public string Apt_Name { get; set; }
        public string Sido { get; set; }
        public string Checkd { get; set; } = "A";
        public string Views { get; set; } = "B";
        public bool chkInfor { get; set; } = false;
        public bool chkAppoval { get; set; } = false;
        public string Confirm { get; set; }
        public string Result { get; set; } = "A";
        public string UserId_confirm { get; set; } = "A";
        public string pass_confirm { get; set; } = "A";
        public string Mobile { get; set; }


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
                Mobile = authState.User.Claims.FirstOrDefault(c => c.Type == "Mobile")?.Value;

                ann = await in_AptPople_Lib.Detail_M(Mobile);
            }
            else
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "로그인되지 않았습니다. 먼저 로그인하세요.");
                MyNav.NavigateTo("/");
            }
        }

        public int intBeing { get; set; }
        public string Pass { get; set; }
        public string strBePass { get; set; }
        public string PassComform { get; set; }
        public string EditPass { get; set; } = "A";
        private void OnClose()
        {
            Views = "B";
            EditPass = "A";
        }

        private void OnPassEdit()
        {
            Views = "A";
        }
        private async Task OnBePass(ChangeEventArgs a)
        {
            intBeing = await in_AptPople_Lib.Log_views(ann.User_Code, a.Value.ToString());
            if (intBeing > 0)
            {
                EditPass = "B";
            }
            else
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "암호가 맞지 않습니다.");
            }
        }

        private async Task OnPassConform(ChangeEventArgs a)
        {
            PassComform = a.Value.ToString();
            if (string.IsNullOrWhiteSpace(PassComform))
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "암호확인을 입력하지 않았습니다.");
            }
        }

        private async Task OnEditSave()
        {
            //PassComform = a.Value.ToString();
            if (string.IsNullOrWhiteSpace(Pass))
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "새 암호를 입력하지 않았습니다.");
            }
            else if (string.IsNullOrWhiteSpace(PassComform))
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "암호확인을 입력하지 않았습니다.");
            }
            else
            {
                if (Pass == PassComform)
                {
                    await in_AptPople_Lib.Eidt_pass(Pass, ann.User_Code);
                    ann = await in_AptPople_Lib.Detail_M(Mobile);
                    Views = "B";
                    EditPass = "A";
                }
                else
                {
                    await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "새암호와 암호확인이 같지 않았습니다.");
                }
            }
        }
    }
}
