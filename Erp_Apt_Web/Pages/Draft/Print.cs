using Erp_Apt_Lib;
using Erp_Lib;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Erp_Apt_Web.Pages.Draft
{
    public partial class Print
    {
        [Parameter] public int Aid { get; set; }
        [Parameter] public string Name { get; set; }
        [Inject] public IApt_Lib apt_Lib { get; set; }  // 공동주택 정보
        [Inject] public IDefect_Lib defect_lib { get; set; } // 하자 관리
        [Inject] public NavigationManager MyNav { get; set; } // Url 
        [Inject] public ISw_Files_Lib files_Lib { get; set; } // 파일 첨부

        Defect_Entity ann { get; set; } = new Defect_Entity();
        List<Sw_Files_Entity> bnn { get; set; } = new List<Sw_Files_Entity>();

        /// <summary>
        /// 방문 시 실행
        /// </summary>
        protected override async Task OnInitializedAsync()
        {
            if (Aid > 0)
            {
                
                ann = await defect_lib.Details(Aid);
                bnn = await files_Lib.Sw_Files_List(Name, Aid.ToString());
            }
            else
            {
                MyNav.NavigateTo("/");
            }
        }
    }
}
