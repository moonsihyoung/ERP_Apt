using Erp_Apt_Lib.Up_Files;
using Erp_Apt_Staff;
using Erp_Entity;
using Erp_Lib;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using sw_Lib.Labors;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Erp_Apt_Web.Pages.Admin.Labor_contract
{
    public partial class Index
    {
        [Parameter] public string Code { get; set; }
        [CascadingParameter] public Task<AuthenticationState> AuthenticationStateRef { get; set; }
        [Inject] public NavigationManager MyNav { get; set; }
        [Inject] IJSRuntime JSRuntime { get; set; }
        [Inject] ILabor_contract_Lib labor_Contract_Lib { get; set; }
        [Inject] ISido_Lib sido_Lib { get; set; }
        [Inject] IApt_Lib apt_Lib { get; set; }

        List<Sido_Entity> sido { get; set; } = new List<Sido_Entity>();
        List<Apt_Entity> apt { get; set; } = new List<Apt_Entity>();

        public string User_Code { get; set; }
        public string User_Name { get; set; }
        public string Apt_Code { get; set; }
        public string Apt_Name { get; set; }
        public string UserName { get; set; }
        public int LevelCount { get; set; }

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
                LevelCount = Convert.ToInt32(authState.User.Claims.FirstOrDefault(c => c.Type == "LevelCount")?.Value);

                if (LevelCount >= 10)
                {
                    await DisplayData();                    
                }
                else
                {
                    await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "접근 권한이 없습니다..");
                    MyNav.NavigateTo("/");
                }
            }
            else
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "로그인되지 않았습니다..");
                MyNav.NavigateTo("/");
            }
        }

        List<Labor_contract_Entity> ann { get; set; } = new List<Labor_contract_Entity>();

        public string Sort { get; set; }
        //public string strApt_Code { get; set; }
        private async Task DisplayData()
        {            
            if (Sort == "A")
            {
                pager.RecordCount = await labor_Contract_Lib.Contract_count_Apt(strAptCode);
                ann = await labor_Contract_Lib.Contract_list_Apt(pager.PageIndex, strAptCode);                
            }
            else if (Sort == "B")
            {
                pager.RecordCount = await labor_Contract_Lib.Contract_List_Name_count(strSearchName);
                ann = await labor_Contract_Lib.Contract_list_Name(pager.PageIndex, strSearchName);
            }            
            else
            {
                pager.RecordCount = await labor_Contract_Lib.Contract_count_All();
                ann = await labor_Contract_Lib.GetList_All(pager.PageIndex);
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
        /// 근로계약 검색
        /// </summary>
        public string strAptCode { get; set; }
        public string SearchViews { get; set; } = "A";
        private void btnSearch_Apt()
        {
            SearchViews = "B";
            strAptCode = "";
            strTitle = "단지별 근로 계약 목록";
        }


        /// <summary>
        ///  근무일 수 계산
        /// </summary>
        [Inject] public IReferral_career_Lib career_Lib { get; set; }
        protected string Func_span(object objstart, object objend)
        {
            int ddate = 0;
            DateTime date = Convert.ToDateTime(objend);
            if (date.Year == 0001)
            {
                DateTime start = Convert.ToDateTime(objstart);
                //DateTime end = Convert.ToDateTime(objend);
                string start_a = start.ToShortDateString();
                string end_a = DateTime.Now.ToShortDateString();   //end.ToShortDateString();
                ddate = career_Lib.Date_scomp(start_a, end_a);
            }
            else
            {
                DateTime start = Convert.ToDateTime(objstart);
                DateTime end = Convert.ToDateTime(objend);
                string start_a = start.ToShortDateString();
                string end_a = end.ToShortDateString(); //DateTime.Now.ToShortDateString();
                ddate = career_Lib.Date_scomp(start_a, end_a);
            }

            return string.Format("{0: ###,###}", ddate);
        }

        /// <summary>
        ///  승인여부
        /// </summary>
        public string FuncResult(object CodeA)
        {
            string code = CodeA.ToString();
            if (code == "B")
            {
                code = "승인";
            }
            else
            {
                code = "미승인";
            }
            return code;
        }

        /// <summary>
        /// 확정자 찾기오기
        /// </summary>
        private string OnExtension(string filesName)
        {
            return Path.GetExtension(filesName);
        }

        /// <summary>
        /// 삭제
        /// </summary>
        private async Task btnRemove(int Aid)
        {
            bool isDelete = await JSRuntime.InvokeAsync<bool>("confirm", $"{Aid}번 글을 정말로 삭제하시겠습니까?");
            if (isDelete)
            {
                await labor_Contract_Lib.Remove(Aid.ToString());
                await DisplayData();
            }
        }

        /// <summary>
        /// 첨부파일 보기
        /// </summary>
        [Inject] public IUpFile_Lib files_Lib { get; set; }
        public string FileInputViews { get; set; } = "A";
        public string FileViews { get; set; } = "A";
        public int Files_Count { get; set; } = 0;
        List<UpFile_Entity> Files_Entity { get; set; } = new List<UpFile_Entity>();
        private async Task OnFileViews(int a)
        {
            FileViews = "B";
            Files_Count = await files_Lib.UpFile_Count("ContractOfEmployment", a.ToString(), bnn.Apt_Code);
            if (Files_Count > 0)
            {
                Files_Entity = await files_Lib.UpFile_List("ContractOfEmployment", a.ToString(), bnn.Apt_Code);
            }
        }

        /// <summary>
        /// 승인하기
        /// </summary>
        //private async Task OnApprovalA(Labor_contract_Entity _labor)
        //{
        //    if (LevelCount >= 10)
        //    {
        //        if (_labor.Division == "A")
        //        {
        //            await labor_Contract_Lib.Approval(_labor.Aid.ToString());
        //        }
        //        else
        //        {
        //            await labor_Contract_Lib.Approval(_labor.Aid.ToString());
        //        }
        //    }
        //    else
        //    {
        //        await JSRuntime.InvokeAsync<object>("alert", "권한이 없습니다.");
        //    }
        //}

        /// <summary>
        /// 승인
        /// </summary>
        private async Task OnApproval(int Aid)
        {
            if (LevelCount >= 10)
            {
                await labor_Contract_Lib.Approval(Aid.ToString());
                await DisplayData();
            }
            else
            {
                await JSRuntime.InvokeAsync<object>("alert", "권한이 없습니다.");
            }
        }

        /// <summary>
        /// 승인
        /// </summary>
        private async Task OnApprovalB(int Aid)
        {
            if (LevelCount >= 10)
            {
                await labor_Contract_Lib.Approval(Aid.ToString());
                await DisplayData();
            }
            else
            {
                await JSRuntime.InvokeAsync<object>("alert", "권한이 없습니다.");
            }
        }

        /// <summary>
        /// 상세보기
        /// </summary>
        public string Views { get; set; } = "A";
        public string Sum_Pay { get; set; } = "0";
        Labor_contract_Entity bnn { get; set; } = new Labor_contract_Entity();
        public string strTitle { get; set; }
        public void SelectView(Labor_contract_Entity _labor)
        {
            Views = "B";
            strTitle = "근로계약서 상세정보";
            bnn = _labor;
            int a = bnn.BasicsPay + bnn.Pay_A + bnn.Pay_B + bnn.Pay_C + bnn.Pay_D + bnn.Pay_E + bnn.Pay_F;
            Sum_Pay = string.Format("{0: ###,###.##}", a);
            bnn.WorkMonthTime = Math.Round(bnn.WorkMonthTime, 2);
        }

        private void btnClose()
        {
            Views = "A";
        }

        private async Task OnFileViews()
        {
            FileViews = "B";
            Files_Count = await files_Lib.UpFile_Count("ContractOfEmployment", bnn.Aid.ToString(), Apt_Code);
            if (Files_Count > 0)
            {
                Files_Entity = await files_Lib.UpFile_List("ContractOfEmployment", bnn.Aid.ToString(), Apt_Code);
            }
        }

        private void ViewsClose()
        {
            FileViews = "A";
        }

        /// <summary>
        /// 첨부파일 삭제
        /// </summary>
        private async Task FilesRemove(UpFile_Entity _files)
        {
            bool isDelete = await JSRuntime.InvokeAsync<bool>("confirm", $"{_files.FileName} 첨부파일을 정말로 삭제하시겠습니까?");

            if (isDelete)
            {
                if (!string.IsNullOrEmpty(_files?.FileName))
                {
                    // 첨부 파일 삭제 
                    //await FileStorageManagerInjector.DeleteAsync(_files.Sw_FileName, "");
                    try
                    {
                        string rootFolder = $"D:\\Msh\\Msh\\sw_Web\\Files_Up\\A_Files\\{_files.FileName}";
                        File.Delete(rootFolder);
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
                await files_Lib.Remove_UpFile(_files.Aid.ToString());

                await labor_Contract_Lib.Files_Count_Add(bnn.Aid, "B"); //파일 수 줄이기
                //await UploadRepositoryAsyncReference.DeleteAsync(Id); // 삭제
                //NavigationManagerReference.NavigateTo("/Uploads"); // 리스트 페이지로 이동
                Files_Count = await files_Lib.UpFile_Count("ContractOfEmployment", bnn.Aid.ToString(), Apt_Code);
                if (Files_Count > 0)
                {
                    Files_Entity = await files_Lib.UpFile_List("ContractOfEmployment", bnn.Aid.ToString(), Apt_Code);
                }
                else
                {
                    Files_Entity = new List<UpFile_Entity>();
                }
            }
            else
            {
                await JSRuntime.InvokeAsync<object>("alert", "취소되었습니다.");
            }
        }

        /// <summary>
        /// 시도 선택
        /// </summary>
        public string strSido { get; set; }
        public string strSiGunGu { get; set; }
        private async Task OnSido(ChangeEventArgs a)
        {
            strSido = a.Value.ToString();
            sido = await sido_Lib.GetList(strSido);
        }

        private async Task OnSiGunGu(ChangeEventArgs a)
        {
            strSiGunGu = a.Value.ToString();
            apt = await apt_Lib.GetList_All_Sido_Gun(strSido, strSiGunGu);
        }

        private async Task OnApt(ChangeEventArgs a)
        {
            Sort = "A";
            strAptCode = a.Value.ToString();
            await DisplayData();
            SearchViews = "A";
            strSido = "";
            strSiGunGu = "";
        }

        private async Task btnCloseS()
        {
            SearchViews = "A";
            strAptCode = "";
            await DisplayData();
            strSido = "";
            strSiGunGu = "";
            strSido = "";
            strAptCode = "";
        }

        public string strSearchName { get; set; }
        private async Task OnSearchName(ChangeEventArgs a)
        {
            Sort = "B";
            strSearchName = a.Value.ToString();
            await DisplayData();
        }

        public string strSearchApt { get; set; }

        private async Task OnSearchApt(ChangeEventArgs a)
        {
            strSearchApt = a.Value.ToString();
            apt = await apt_Lib.SearchList(strSearchApt);
        }
    }
}
