using DbImage;
using Erp_Apt_Staff;
using Erp_Entity;
using Erp_Lib;
using Facilities;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Erp_Apt_Web.Pages.Admin.Staff
{
    public partial class Seal_Photo
    {
        #region Inject
        [Parameter] public string Code { get; set; }

        [CascadingParameter] public Task<AuthenticationState> AuthenticationStateRef { get; set; }
        [Inject] public IApt_Lib apt_Lib { get; set; }
        [Inject] public NavigationManager MyNav { get; set; }
        [Inject] IJSRuntime JSRuntime { get; set; }
        [Inject] public IStaff_Lib staff { get; set; }
        [Inject] public IReferral_career_Lib referral_Career_Lib { get; set; }
        [Inject] public IPost_Lib Post_Lib { get; set; }
        [Inject] public IDbImageLib dbImageLib { get; set; }

        #endregion

        #region 목록
        Referral_career_Entity cnn { get; set; } = new Referral_career_Entity();

        List<Post_Entity> pnn { get; set; } = new List<Post_Entity>();
        List<Referral_career_Entity> dnn = new List<Referral_career_Entity>();
        List<DbImageEntity> ann { get; set; } = new List<DbImageEntity>();
        DbImageEntity bnn { get; set; } = new DbImageEntity();
        public string strDong_No { get; private set; }
        #endregion

        #region 변수
        public string User_Code { get; set; }
        public string User_Name { get; set; }
        public string Apt_Code { get; set; }
        public string Apt_Name { get; set; }
        public string UserName { get; set; }
        public string ListViews { get; set; } = "B";
        public string PostDuty { get; set; }
        public string DetailsViews { get; set; } = "A";
        public string InsertViews { get; set; } = "A";
        public string RemoveViews { get; set; } = "A";
        public string strUserName { get; set; }
        public int intNum { get; set; }
        public string strTitle { get; set; }

        public string strKeyword { get; set; }
        #endregion

        /// <summary>
        /// 로드시 실행
        /// </summary>
        /// <returns></returns>
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
                                
                await DisplayData();
            }
            else
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "로그인되지 않았습니다..");
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

        /// <summary>
        /// 페이징
        /// </summary>
        protected DulPager.DulPagerBase pager = new DulPager.DulPagerBase()
        {
            PageNumber = 1,
            PageIndex = 0,
            PageSize = 15,
            PagerButtonCount = 5
        };

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
        /// 데이터 뷰
        /// </summary>
        private async Task DisplayData()
        {
            // 해당 공동주택의 민원 목록
            pager.RecordCount = await dbImageLib.GetListCountApt(Apt_Code);
            ann = await dbImageLib.GetListApt(pager.PageIndex, Apt_Code);
        }

        /// <summary>
        /// 도장 새로 등록
        /// </summary>
        private void btnOpen()
        {
            strTitle = "결재 도장 등록";
            bnn = new DbImageEntity();
            InsertViews = "B";
        }

        private void ByEdit(DbImageEntity db)
        {
            bnn = db;
            bnn.PostDate = DateTime.Now;
            strTitle = "결재 도장 수정";
            //await dbImageLib.Edit(db);
            InsertViews = "B";
        }

        private async Task ByRemove(int db)
        {
            bool isDelete = await JSRuntime.InvokeAsync<bool>("confirm", $"{db}번 도장 정보를 정말로 삭제하시겠습니까?");
            if (isDelete)
            {
                //svDate = DateTime.Now.Date;
                await dbImageLib.Remove(db);
                await DisplayData();
            }
        }

        private void btnClose()
        {
            InsertViews = "A";
        }

        public byte[] ImgUpload { get; set; }
        public async Task fileUp(InputFileChangeEventArgs e)
        {
            var file = e.File;
            bnn.FileType = "image/png";
            if (file != null)
            {
                var resizedimagesFile = await file.RequestImageFileAsync(bnn.FileType, 100, 100);
                var ImgUpload = new byte[resizedimagesFile.Size];
                bnn.Photo = ImgUpload;
                await resizedimagesFile.OpenReadStream().ReadAsync(ImgUpload);
                bnn.FileName = resizedimagesFile.Name;
            }
            
        }

        private async Task btnSave()
        {
            bnn.AptCode = Apt_Code;
            #region 아이피 입력
            string myIPAddress = "";
            var ipentry = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName());
            foreach (var ip in ipentry.AddressList)
            {
                if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    myIPAddress = ip.ToString();
                    break;
                }
            }
            bnn.PostIP = myIPAddress;
            #endregion
            if (bnn.Photo == null)
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "이미지 파일이 선택되지 않았습니다..");
            }
            else if (string.IsNullOrWhiteSpace(bnn.FileType))
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "파일 타입이 입력되지 않았습니다..");
            }
            else if (string.IsNullOrWhiteSpace(bnn.User_Code))
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "로그인되지 않았습니다..");
            }
            else if (string.IsNullOrWhiteSpace(bnn.AptCode))
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "로그인되지 않았습니다..");
            }
            else
            {
                bnn.SubName = "Service";
                
                if (bnn.Aid < 1)
                {
                    await dbImageLib.Add(bnn);
                }
                else
                {
                    await dbImageLib.Edit(bnn);
                }

                await DisplayData();
            }
        }

        private async void OnUserCode(ChangeEventArgs a)
        {
            bnn.User_Code = a.Value.ToString();
            var ve = await staff.Be(bnn.User_Code);
            if (ve > 0)
            {
                strUserName = await staff.Name(bnn.User_Code);
            }
            else
            {
                bnn.User_Code = null;
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "로그인되지 않았습니다..");
            }
        }
    }
}
