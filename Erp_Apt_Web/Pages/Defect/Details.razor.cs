using Erp_Apt_Lib;
using Erp_Apt_Lib.Appeal;
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

namespace Erp_Apt_Web.Pages.Defect
{
    public partial class Details
    {
        [CascadingParameter]
        public Task<AuthenticationState> AuthenticationStateRef { get; set; }

        [Inject]
        public IApt_Lib apt_Lib { get; set; }  // 공동주택 정보

        [Inject]
        public IReferral_career_Lib referral_Career { get; set; }

        [Inject]
        public IPost_Lib post_Lib { get; set; }

        [Inject]
        public IDefect_Lib defect_lib { get; set; } // 하자 관리

        [Inject]
        public NavigationManager MyNav { get; set; } // Url 

        [Inject]
        public ISw_Files_Lib files_Lib { get; set; } // 파일 첨부

        [Inject]
        public IAppeal_Bloom_Lib bloom_Lib { get; set; } //민원분류


        [Inject] IJSRuntime JSRuntime { get; set; }

        Defect_Entity bnn { get; set; } = new Defect_Entity();
        List<Referral_career_Entity> wnn { get; set; } = new List<Referral_career_Entity>();
        Sw_Files_Entity dnn { get; set; } = new Sw_Files_Entity();
        List<Post_Entity> pnn { get; set; } = new List<Post_Entity>();

        List<Sw_Files_Entity> Files_Entity { get; set; } = new List<Sw_Files_Entity>();
        List<Defect_Entity> ann = new List<Defect_Entity>();

        public string User_Code { get; set; }
        public string User_Name { get; set; }
        public string Apt_Code { get; set; }
        public string Apt_Name { get; set; }
        public string UserName { get; set; }
        public string Views { get; set; } = "A";
        public string lblContent { get; set; } = "";
        public int Files_Count { get; set; } = 0;
        public int intAid { get; set; } = 0;
        public string PostName { get; set; } = "a";
        public string PostCode { get; set; } = "a";
        public string FileViews { get; set; } = "A";

        /// <summary>
        /// 첨부 파일 리스트 보관
        /// </summary>
        //private IFileListEntry[] selectedFiles;

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

                ann = await defect_lib.GetList(Apt_Code);

                //Files_Count = await files_Lib.Sw_Files_Data_Index_Count("Defect", ann.a.ToString(), Apt_Code);

                //if (Files_Count > 0)
                //{
                //    Files_Entity = await files_Lib.Sw_Files_Data_Index("Appeal", ann.Num.ToString(), Apt_Code);
                //}
                //snn = await subAppeal.GetList(ann.Num.ToString());
                //bnn.subDate = DateTime.Now;
            }
            else
            {
                MyNav.NavigateTo("/");
            }
        }

        /// <summary>
        /// 모달 이동 
        /// </summary>
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await JSRuntime.InvokeVoidAsync("setModalDraggableAndResizable");
            await base.OnAfterRenderAsync(firstRender);
        }
    }
}
