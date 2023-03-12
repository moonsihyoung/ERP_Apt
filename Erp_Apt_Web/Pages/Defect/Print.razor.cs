using Erp_Apt_Lib;
using Erp_Apt_Staff;
using Erp_Lib;
using Microsoft.AspNetCore.Components;

namespace Erp_Apt_Web.Pages.Defect
{
    public partial class Print
    {
        [Parameter] public int Aid { get; set; }
        [Parameter] public string Name { get; set; }
        [Parameter] public string AptCode { get; set; }
        [Inject] public IApt_Lib apt_Lib { get; set; }  // 공동주택 정보
        [Inject] public IDefect_Lib defect_lib { get; set; } // 하자 관리
        [Inject] public IStaff_Lib staff_lib { get; set; } // 하자 관리
        [Inject] public NavigationManager MyNav { get; set; } // Url 
        [Inject] public ISw_Files_Lib files_Lib { get; set; } // 파일 첨부

        Defect_Entity ann { get; set; } = new Defect_Entity();
        List<Sw_Files_Entity> bnn { get; set; } = new List<Sw_Files_Entity>();
        public string AptName { get; set; } = "";
        public string UserName { get; set; }

        /// <summary>
        /// 방문 시 실행
        /// </summary>
        protected override async Task OnInitializedAsync()
        {
            if (Aid > 0)
            {
                AptName = await apt_Lib.Apt_Name(AptCode);
                ann = await defect_lib.Details(Aid);
                bnn = await files_Lib.Sw_Files_List(Name, Aid.ToString());
                //string code = ann.User_Code.ToString();
                UserName = staff_lib.UsersName(ann.User_Code);
            }
            else
            {
                MyNav.NavigateTo("/");
            }
        }

        private void ByAid(int Aid)
        {
            MyNav.NavigateTo("/");
        }
    }
}
