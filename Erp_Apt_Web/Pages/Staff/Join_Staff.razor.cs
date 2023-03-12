using Company;
using Erp_Apt_Lib;
using Erp_Apt_Staff;
using Erp_Entity;
using Erp_Lib;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Erp_Apt_Web.Pages.Staff
{
    public partial class Join_Staff
    {
        #region 인스턴스
        [CascadingParameter] public Task<AuthenticationState> AuthenticationStateRef { get; set; }
        [Inject] public NavigationManager MyNav { get; set; } // Url 
        [Inject] IJSRuntime JSRuntime { get; set; }
        [Inject] public IReferral_career_Lib referral_Career_Lib { get; set; }
        [Inject] public IStaff_Lib staff_Lib { get; set; }
        [Inject] public IStaff_staffSub_Lib staff_StaffSub_Lib { get; set; }
        [Inject] public IStaff_Sub_Lib staff_Sub_Lib { get; set; }
        [Inject] public IStaff_Career_Lib staff_Career_Lib { get; set; }
        [Inject] public ISido_Lib sido_Lib { get; set; }
        [Inject] public IPost_Lib post_Lib { get; set; }
        [Inject] public IDuty_Lib duty_Lib { get; set; }
        [Inject] public ICompany_Career_Lib company_Career_Lib { get; set; }
        [Inject] public ISw_Files_Lib files_Lib { get; set; } // 파일 첨부
        #endregion

        #region 변수
        public string User_Code { get; set; }
        public string User_Name { get; set; }
        public string Apt_Code { get; set; }
        public string Apt_Name { get; set; }
        public int LevelCount { get; set; } = 0;
        public string Views { get; set; } = "A"; //상세 열기
        public string RemoveViews { get; set; } = "A";//삭제 열기
        public string InsertViews { get; set; } = "A"; // 입력 열기
        public string PassViews { get; set; } = "A";
        public string Old_Password { get; set; }
        public string New_Password { get; set; }
        public string FileViews { get; set; }
        public int Files_Count { get; set; } = 0;
        public string InsertFiles { get; set; } = "A";
        public int intIDBe { get; set; } = 0;
        public string Division { get; set; }
        public string Password_sw { get; set; } = "A";
        public string strSido { get; set; }
        public string strSiGunGu { get; set; }
        public string StaffName { get; set; }
        public DateTime Scn { get; set; }
        public DateTime Career_End_Date { get; set; } = DateTime.Now;
        #endregion

        #region 속성
        //List<Staff_Career_Entity> ann { get; set; } = new List<Staff_Career_Entity>();
        public Staff_Entity bnn { get; set; } = new Staff_Entity();
        public Staff_Sub_Entity cnn { get; set; } = new Staff_Sub_Entity();
        Staff_Career_Entity rnn { get; set; } = new Staff_Career_Entity();
        Referral_career_Entity rce { get; set; } = new Referral_career_Entity();
        public Staff_Entity staff { get; set; } = new Staff_Entity();
        public Staff_StaffSub_Entity staff_Sub { get; set; } = new Staff_StaffSub_Entity();
        public List<Staff_Entity> staffA { get; set; } = new List<Staff_Entity>();
        public Staff_Sub_Entity staffSub { get; set; } = new Staff_Sub_Entity();
        public Sw_Files_Entity dnn { get; set; } = new Sw_Files_Entity();
        List<Sw_Files_Entity> Files_Entity { get; set; } = new List<Sw_Files_Entity>();
        public List<Sido_Entity> sigungu { get; set; } = new List<Sido_Entity>();
        public Sido_Entity sido { get; set; } = new Sido_Entity();
        public List<Post_Entity> Post { get; set; } = new List<Post_Entity>();
        public List<Duty_Entity> Duty { get; set; } = new List<Duty_Entity>();

        Sw_Files_Entity hnn { get; set; } = new Sw_Files_Entity(); // 첨부 파일 정보
        #endregion

        /// <summary>
        /// 방문 시 실행
        /// </summary>
        protected override void OnInitialized()
        {
            InsertViews = "B";
            //bnn.Scn = Convert.ToDateTime()
            //await JSRuntime.InvokeAsync<object>("alert", "로그인되지 않았습니다.");
            ////MyNav.NavigateTo("/");
            cnn.Start_Date = DateTime.Now;
        }

        private async Task DispalyData()
        {
            staff = await staff_Lib.View(bnn.User_ID);
            staffSub = await staff_Sub_Lib.View(cnn.User_ID);
            //ann = await staff_Career_Lib.StaffCareer_Join_Users(User_Code);

            Files_Count = await files_Lib.Sw_Files_Data_Index_Count("Staff", staff.Aid.ToString(), Apt_Code);
            if (Files_Count > 0)
            {
                Files_Entity = await files_Lib.Sw_Files_Data_Index("Staff", staff.Aid.ToString(), Apt_Code);
            }
        }

        /// <summary>
        /// 직원 정보 입력
        /// </summary>
        /// <returns></returns>
        public async Task btnStaffSave()
        {
            bnn.M_Apt_Code = "A";
            bnn.Scn = Scn.ToShortDateString();
            bnn.Scn_Code = bnn.Scn.Replace("-", "");
            //bnn.User_ID = Apt_Code + await staff_Lib.List_Number();
            bnn.Staff_Cd = bnn.User_ID;
            cnn.User_ID = bnn.User_ID;
            cnn.Staff_Cd = bnn.User_ID;
            cnn.Staff_Sub_Cd = bnn.User_ID;
            if (cnn.Etc == null || cnn.Etc == "")
            {
                cnn.Etc = "관리자가 입력했으므로 입력하지 않았습니다.";
                bnn.Intro = cnn.Etc;
            }
            else
            {
                bnn.Intro = cnn.Etc;
            }
            bnn.JoinDate = DateTime.Now;
            bnn.LevelCount = 3;
            //bnn.Password_sw = bnn.User_ID + "pw";
            bnn.Sido = cnn.st_Sido;
            bnn.SiGunGu = cnn.st_GunGu;
            bnn.RestAdress = cnn.st_Adress_Rest;
            bnn.LevelCount = 3;

            if (Division == null || Division == "")
            {
                cnn.d_division = "기타";
            }
            else
            {
                cnn.d_division = Division;
            }
            cnn.M_Apt_Code = bnn.M_Apt_Code;
            cnn.Staff_Cd = bnn.User_ID;
            cnn.User_ID = bnn.User_ID;
            cnn.Staff_Name = bnn.User_Name;
            cnn.Staff_Sub_Cd = bnn.User_ID;
            cnn.levelcount = 3;
            cnn.End_Date = DateTime.Now;

            if (string.IsNullOrWhiteSpace(bnn.M_Apt_Code))
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "단지 코드를 입력하지 않았습니다.");
                MyNav.NavigateTo("/");
            }
            else if (string.IsNullOrWhiteSpace(bnn.Scn))
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "출생일을 입력하지 않았습니다.");
            }
            else if (string.IsNullOrWhiteSpace(bnn.User_ID))
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "아이디를 입력하지 않았습니다.");
            }
            else if (string.IsNullOrWhiteSpace(bnn.User_Name))
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "직원명을 입력하지 않았습니다.");
            }
            else if (string.IsNullOrWhiteSpace(cnn.Staff_Name))
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "직원명을 입력하지 않았습니다.");
            }
            else if (string.IsNullOrWhiteSpace(cnn.st_Sido))
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "시도를 선택하지 않았습니다.");
            }
            else if (string.IsNullOrWhiteSpace(cnn.st_GunGu))
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "시군구를 입력하지 않았습니다.");
            }
            else if (string.IsNullOrWhiteSpace(cnn.Mobile_Number))
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "핸드폰 전화번호를 입력하지 않았습니다.");
            }
            else if (string.IsNullOrWhiteSpace(cnn.d_division))
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "구분을 선택하지 않았습니다.");
            }
            else if (string.IsNullOrWhiteSpace(cnn.Etc))
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "소개를 설명하지 않았습니다.");
            }
            else
            {
                try
                {
                    cnn.Mobile_Number = cnn.Mobile_Number.Replace("-", "").Replace(" ", "");
                    string cp_Num = cnn.Mobile_Number;
                    cp_Num = cp_Num.Insert(3, "-");
                    cp_Num = cp_Num.Insert(8, "-");
                    cnn.Mobile_Number = cp_Num;
                }
                catch (Exception)
                {
                    await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "핸드폰 전화번호를 입력하지 않았습니다.");
                }

                int su = await staff_StaffSub_Lib.GetListInsertCount(bnn.User_Name, cnn.Mobile_Number);
                if (su < 1)
                {
                    if (bnn.Aid < 1)
                    {
                        bnn.Scn = Scn.ToShortDateString();
                        try
                        {
                            await staff_Lib.Add(bnn);
                            try
                            {
                                await staff_Sub_Lib.Add(cnn);
                                await DispalyData();
                            }
                            catch (Exception)
                            {
                                await staff_Lib.Remove(cnn.User_ID);
                                await JSRuntime.InvokeAsync<object>("alert", "직원 기본 정보 입력에 실패했습니다.");
                            }
                        }
                        catch (Exception)
                        {
                            await JSRuntime.InvokeAsync<object>("alert", "작원 상세 정보 입력에 실패했습니다.");
                        }

                        #region 로그 파일 만들기
                        //logs.Note = bnnA.User_Name + " " + bnnA.User_ID + " " + bnnA.Scn + " " + bnnA.JoinDate.ToShortDateString() + "입력했습니다."; logs.Logger = User_Code; logs.Application = "직원관리"; logs.ipAddress = ""; logs.Message = Apt_Code + " " + Apt_Name;
                        //await logs_Lib.add(logs);
                        #endregion
                    }
                    else
                    {
                        await staff_Lib.Edit(bnn);
                        await staff_Sub_Lib.Edit(cnn);
                        await DispalyData();
                    }

                    InsertViews = "A";
                }
                else
                {
                    await JSRuntime.InvokeAsync<object>("alert", "해당 성명과 핸드폰 번호로 입력된 정보가 있어서 입력되지 않았습니다.");
                }
            }
        }

        /// <summary>
        /// 시도 선택 시 시군구 만들기
        /// </summary>
        public async Task OnSido(ChangeEventArgs args)
        {
            //bnnA.JoinDate = DateTime.Now;
            //bnnB.st_Sido = args.Value.ToString();
            sigungu = await sido_Lib.GetList_Code(args.Value.ToString());
            //StateHasChanged();
        }

        /// <summary>
        /// 아이디 중복 확인
        /// </summary>
        public async Task OnSearchID()
        {
            if (!string.IsNullOrWhiteSpace(bnn.User_ID))
            {
                bnn.User_ID = bnn.User_ID.Trim();
                string re = bnn.User_ID;
                int txtlength = re.Length;
                if (txtlength < 5)
                {
                    await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "아이디는 5자 이상이어야 합니다..");
                }
                else if (txtlength > 12)
                {
                    await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "아이디는 12자 이하이어야 합니다..");
                }
                else
                {
                    int be = await staff_Lib.Be(bnn.User_ID);
                    if (be > 0)
                    {
                        bnn.User_ID = "";
                        await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "이미 사용하고 있는 아이디 입니다..");
                    }
                }                
            }
            else
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "암호가 일치하지 않습니다..");
                
            }
        }

        /// <summary>
        /// 암호 일치 여부 확인
        /// </summary>
        public string strComform { get; set; } = "";
        private async Task OnPassComform()
        {
            if (!string.IsNullOrWhiteSpace(bnn.Password_sw) && !string.IsNullOrWhiteSpace(Password_sw))
            {                
                if (bnn.Password_sw == Password_sw)
                {
                    int txtlength = bnn.Password_sw.Length;
                    if (txtlength < 6)
                    {
                        await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "암호는 6자 이상 이여야 합니다..");
                    }
                    else if (txtlength > 12 )
                    {
                        await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "암호는 12자 이하 일치합니다..");
                    }
                    else
                    {
                        strComform = "B";
                        await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "암호가 일치합니다..");
                    }
                }
                else
                {
                    strComform = "";
                    await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "암호가 일치하지 않습니다..");
                }
            }
            else
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "암호와 암호 확인을 입력하지 않습니다..");
            }
        }

        public string strYear { get; set; }
        public string strMonth { get; set; }
        public string strDay { get; set; }
        private async Task OnDays(ChangeEventArgs a)
        {
            strDay = a.Value.ToString();

            if (!string.IsNullOrWhiteSpace(strDay))
            {
                if (!string.IsNullOrWhiteSpace(strYear) && !string.IsNullOrWhiteSpace(strMonth))
                {
                    bnn.Scn = strYear + "-" + strMonth + "-" + strDay;
                }
                else
                {
                    await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "먼저 년도와 월 선택해 주세요..");
                }
            }
            else
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "일자를 선택해 주세요..");
            }
        }

        /// <summary>
        /// 자격증 등록 
        /// </summary>
        /// <param name="args"></param>
        public void OnD_Division(ChangeEventArgs args)
        {
            if (Division != null)
            {
                Division = Division + ", " + args.Value.ToString();
            }
            else
            {
                Division = args.Value.ToString();
            }

            cnn.d_division = Division;
        }

        /// <summary>
        /// 시군구 선택 실행
        /// </summary>
        public async Task onGunGu(ChangeEventArgs args)
        {
            var soe = await sido_Lib.Details(args.Value.ToString());

            cnn.st_Sido = soe.Sido;
            cnn.st_GunGu = soe.Region;
        }

        /// <summary>
        /// 직원정보 입력 닫기
        /// </summary>
        private void btnStaffClose()
        {
            InsertViews = "A";
        }


        private void btnUpdate()
        {
            
        }
    }
}
