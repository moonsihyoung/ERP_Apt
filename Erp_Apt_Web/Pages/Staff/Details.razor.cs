using Company;
using Erp_Apt_Lib;
using Erp_Apt_Staff;
using Erp_Entity;
using Erp_Lib;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Erp_Apt_Web.Pages.Staff
{
    public partial class Details
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
        [Inject] public ICompany_Apt_Career_Lib company_Career_Lib { get; set; }
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
        public int intIdBe { get; set; } = 0;
        public string D_Division { get; set; }
        public string Password_sw { get; set; } = "A";
        public string SiGunGu { get; set; }
        public string StaffName { get; set; }
        public DateTime Scn { get; set; }
        public DateTime Career_End_Date { get; set; } = DateTime.Now;
        public int intAid { get; set; } = 0;
        public string Sido { get; set; }
        #endregion

        #region 속성
        List<Staff_Career_Entity> ann { get; set; } = new List<Staff_Career_Entity>();
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
        public List<Sido_Entity> sigungu_Career { get; set; } = new List<Sido_Entity>();
        public Sido_Entity sido_Entity { get; set; } = new Sido_Entity();
        public List<Post_Entity> Post { get; set; } = new List<Post_Entity>();
        public List<Duty_Entity> Duty { get; set; } = new List<Duty_Entity>();

        Sw_Files_Entity hnn { get; set; } = new Sw_Files_Entity(); // 첨부 파일 정보
        #endregion

        /// <summary>
        /// 방문 시 실행
        /// </summary>
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
                LevelCount = Convert.ToInt32(authState.User.Claims.FirstOrDefault(c => c.Type == "LevelCount")?.Value);
                await DispalyData();
            }
            else
            {
                await JSRuntime.InvokeAsync<object>("alert", "로그인되지 않았습니다.");
                MyNav.NavigateTo("/");
            }
        }

        private async Task DispalyData()
        {
            staff = await staff_Lib.View(User_Code);
            staffSub = await staff_Sub_Lib.View(User_Code); 
            ann = await staff_Career_Lib.StaffCareer_Join_Users(User_Code);

            Files_Count = await files_Lib.Sw_Files_Data_Index_Count("Staff", staff.Aid.ToString(), Apt_Code);
            if (Files_Count > 0)
            {
                Files_Entity = await files_Lib.Sw_Files_Data_Index("Staff", staff.Aid.ToString(), Apt_Code);
            }
        }

        private async Task OnEditViews()
        {
            bnn = staff;
            cnn = staffSub;

            try
            {
                Scn = Convert.ToDateTime(staff.Scn);
            }
            catch (Exception)
            {
                //
            }
            sigungu = await sido_Lib.GetList(staffSub.st_Sido);
            cnn = staffSub;
            Sido = cnn.st_Sido;
            SiGunGu = await sido_Lib.Region_Code(staffSub.st_GunGu);

            InsertViews = "B";
        }   
        
        private void OnEditPaw()
        {
            PassViews = "B";
        }


        /// <summary>
        /// 첨부파일 모달 열기
        /// </summary>
        private void btnFileInsert()
        {
            FileViews = "B";
        }

        /// <summary>
        /// 파일 첨부 닫기
        /// </summary>
        private void FilesClose()
        {
            FileViews = "A";
        }

        /// <summary>
        /// 첨부파일 삭제
        /// </summary>
        /// <param name="sw_Files"></param>
        private async Task ByFileRemove(Sw_Files_Entity _files)
        {
            bool isDelete = await JSRuntime.InvokeAsync<bool>("confirm", $"{_files.Sw_FileName} 첨부파일을 정말로 삭제하시겠습니까?");

            if (isDelete)
            {
                if (!string.IsNullOrEmpty(_files?.Sw_FileName))
                {
                    // 첨부 파일 삭제 
                    //await FileStorageManagerInjector.DeleteAsync(_files.Sw_FileName, "");
                    string rootFolder = $"{env.WebRootPath}\\UpFiles\\Staff";
                    File.Delete(rootFolder);
                }
                await files_Lib.FileRemove(_files.Num.ToString(), "Staff", Apt_Code);

                //await draft_Lib.FilesCount(bnn.Aid, "B"); //파일 수 줄이기
                //await UploadRepositoryAsyncReference.DeleteAsync(Id); // 삭제
                //NavigationManagerReference.NavigateTo("/Uploads"); // 리스트 페이지로 이동
                Files_Count = await files_Lib.Sw_Files_Data_Index_Count("Staff", staff.Aid.ToString(), Apt_Code);
                if (Files_Count > 0)
                {
                    Files_Entity = await files_Lib.Sw_Files_Data_Index("Staff", staff.Aid.ToString(), Apt_Code);
                }
            }
            else
            {
                await JSRuntime.InvokeAsync<object>("alert", "취소되었습니다.");
            }
        }





        #region Event Handlers
        private long maxFileSize = 1024 * 1024 * 4;
        [Inject] Microsoft.AspNetCore.Hosting.IWebHostEnvironment env { get; set; }
        public string CompleteViews { get; private set; }

        /// <summary>
        /// 저장하기 버튼 클릭 이벤트 처리기
        /// </summary>
        protected async void btnFileSave()
        {
            hnn.Parents_Num = staff.Aid.ToString(); // 선택한 ParentId 값 가져오기 
            hnn.Sub_Num = hnn.Parents_Num;
            var fileName = "";

            var format = "image/png";

            #region 파일 업로드 관련 추가 코드 영역

            foreach (var file in selectedImage)
            {
                var resizedImageFile = await file.RequestImageFileAsync(format, 1025, 760);
                var buffer = new byte[resizedImageFile.Size];
                await resizedImageFile.OpenReadStream(maxFileSize).ReadAsync(buffer);
                var imageDataUrl = $"data:{format};base64,{Convert.ToBase64String(buffer)}";

                Stream stream = resizedImageFile.OpenReadStream(maxFileSize);

                var path = $"{env.WebRootPath}\\UpFiles\\Staff";

                string _FileName = file.Name;
                string _FileNameA = Path.GetExtension(_FileName);
                string _FileNameB = "Staff" + Apt_Code;
                _FileName = _FileNameB + _FileNameA;

                fileName = Dul.FileUtility.GetFileNameWithNumbering(path, _FileName);


                path = path + $"\\{fileName}";

                FileStream fs = File.Create(path);
                await stream.CopyToAsync(fs);
                fs.Close();

                hnn.Sw_FileName = fileName;
                hnn.Sw_FileSize = Convert.ToInt32(file.Size);
                hnn.Parents_Name = "Staff";
                hnn.AptCode = Apt_Code;
                hnn.Del = "A";

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
                hnn.PostIP = myIPAddress;
                #endregion
                await files_Lib.Sw_Files_Date_Insert(hnn); //첨부파일 데이터 db 저장

                //await defect_lib.Edit_ImagesCount(bnn.Aid); // 첨부파일 추가를 db 저장(하자, defect)


                //dnn = new Sw_Files_Entity();
            }


            FileViews = "A";

            Files_Count = await files_Lib.Sw_Files_Data_Index_Count("Staff", staff.Aid.ToString(), Apt_Code);
            if (Files_Count > 0)
            {
                Files_Entity = await files_Lib.Sw_Files_Data_Index("Staff", staff.Aid.ToString(), Apt_Code);
            }

            StateHasChanged();
            #endregion           
        }

        #endregion

        /// <summary>
        /// 파일 업로드
        /// </summary>
        private IList<string> imageDataUrls = new List<string>();
        IReadOnlyList<IBrowserFile> selectedImage;
        private async Task OnFileChage(InputFileChangeEventArgs e)
        {
            var maxAllowedFiles = 5;
            var format = "image/png";
            selectedImage = e.GetMultipleFiles(maxAllowedFiles);
            foreach (var imageFile in selectedImage)
            {
                var resizedImageFile = await imageFile.RequestImageFileAsync(format, 300, 300);
                var buffer = new byte[resizedImageFile.Size];
                await resizedImageFile.OpenReadStream().ReadAsync(buffer);
                var imageDataUrl = $"data:{format};base64,{Convert.ToBase64String(buffer)}";
                imageDataUrls.Add(imageDataUrl);
            }
            StateHasChanged();
        }

        

        /// <summary>
        /// 확정자 찾기오기
        /// </summary>
        /// <param name="filesName"></param>
        /// <returns></returns>
        private string OnExtension(string filesName)
        {
            return Path.GetExtension(filesName);
        }


        /// <summary>
        /// 직원 정보 수정
        /// </summary>
        public async Task btnStaffSave()
        {
            bnn.M_Apt_Code = Apt_Code;
            bnn.Scn = Scn.ToShortDateString();
            bnn.Scn_Code = bnn.Scn.Replace("-", "");
            bnn.Staff_Cd = bnn.User_ID;
            bnn.Intro = cnn.Etc;
            bnn.JoinDate = DateTime.Now;
            bnn.LevelCount = 3;
            bnn.Password_sw = bnn.User_ID + "pw";
            bnn.Sido = cnn.st_Sido;
            bnn.SiGunGu = cnn.st_GunGu;
            bnn.RestAdress = cnn.st_Adress_Rest;
            bnn.LevelCount = 3;

            cnn.d_division = D_Division;
            cnn.M_Apt_Code = bnn.M_Apt_Code;
            cnn.Staff_Cd = bnn.User_ID;
            cnn.User_ID = bnn.User_ID;
            cnn.Staff_Name = bnn.User_Name;
            cnn.Staff_Sub_Cd = bnn.User_ID;
            cnn.levelcount = 3;
            cnn.End_Date = DateTime.Now;

            if (bnn.M_Apt_Code == "" || bnn.M_Apt_Code == null)
            {
                await JSRuntime.InvokeAsync<object>("alert", "로그인되지 않았습니다.");
                MyNav.NavigateTo("/");
            }
            else if (bnn.Scn == "" || bnn.Scn == null)
            {
                await JSRuntime.InvokeAsync<object>("alert", "출생일은 입력하지 않았습니다.");
            }
            else if (bnn.User_ID == "" || bnn.User_ID == null)
            {
                await JSRuntime.InvokeAsync<object>("alert", "출생일은 입력하지 않았습니다.");
            }
            else if (bnn.User_Name == "" || bnn.User_Name == null)
            {
                await JSRuntime.InvokeAsync<object>("alert", "출생일은 입력하지 않았습니다.");
            }
            else if (cnn.Staff_Name == "" || cnn.Staff_Name == null)
            {
                await JSRuntime.InvokeAsync<object>("alert", "출생일은 입력하지 않았습니다.");
            }
            else if (cnn.st_Sido == "Z" || cnn.st_Sido == null)
            {
                await JSRuntime.InvokeAsync<object>("alert", "출생일은 입력하지 않았습니다.");
            }
            else if (cnn.st_GunGu == "Z" || cnn.st_GunGu == null)
            {
                await JSRuntime.InvokeAsync<object>("alert", "출생일은 입력하지 않았습니다.");
            }
            else if (cnn.Mobile_Number == "" || cnn.Mobile_Number == null)
            {
                await JSRuntime.InvokeAsync<object>("alert", "출생일은 입력하지 않았습니다.");
            }
            else
            {
                if (bnn.Aid < 1)
                {
                    bnn.Scn = Scn.ToShortDateString();
                    await staff_Lib.Add(bnn);
                    await staff_Sub_Lib.Add(cnn);
                    #region 로그 파일 만들기
                    //logs.Note = bnn.User_Name + " " + bnn.User_ID + " " + bnn.Scn + " " + bnn.JoinDate.ToShortDateString() + "입력했습니다."; logs.Logger = User_Code; logs.Application = "직원관리"; logs.ipAddress = ""; logs.Message = Apt_Code + " " + Apt_Name;
                    //await logs_Lib.add(logs);
                    #endregion
                }
                else
                {
                    await staff_Lib.Edit(bnn);
                    await staff_Sub_Lib.Edit(cnn);
                }
            }
            await DispalyData();
            InsertViews = "A";            
            bnn = new Staff_Entity();
            cnn = new Staff_Sub_Entity();
        }
        
        /// <summary>
        /// 아이디 중복 확인
        /// </summary>
        public async Task OnSearchID()
        {

            if (bnn.Password_sw == Password_sw)
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "암호가 일치합니다..");
                intIdBe = 0;
            }
            else
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "암호가 일치하지 않습니다..");
                intIdBe = 1;
            }
            //StateHasChanged();
        }

        /// <summary>
        /// 자격증 등록 
        /// </summary>
        /// <param name="args"></param>
        public void OnD_Division(ChangeEventArgs args)
        {
            if (D_Division != null)
            {
                D_Division = D_Division + ", " + args.Value.ToString();
            }
            else
            {
                D_Division = args.Value.ToString();
            }

            cnn.d_division = D_Division;
            //StateHasChanged();
        }

        /// <summary>
        /// 시군구 선택 실행
        /// </summary>
        public async Task onGunGu(ChangeEventArgs args)
        {
            sido_Entity = await sido_Lib.Details(args.Value.ToString());

            cnn.st_Sido = sido_Entity.Sido;
            cnn.st_GunGu = sido_Entity.Region;
            bnn.User_ID = Apt_Code + await referral_Career_Lib.Count_apt_staff(Apt_Code);

            //StateHasChanged();
        }

        /// <summary>
        /// 시도 선택 시 시군구 만들기
        /// </summary>
        public async Task OnSido(ChangeEventArgs args)
        {
            //bnnA.JoinDate = DateTime.Now;
            //bnnB.st_Sido = args.Value.ToString();
            sigungu = await sido_Lib.GetList(args.Value.ToString());
            //StateHasChanged();
        }

        /// <summary>
        /// 직원정보 입력 닫기
        /// </summary>
        private void btnStaffClose()
        {
            InsertViews = "A";
        }

        /// <summary>
        /// 암호 수정 닫기
        /// </summary>
        private void PassClose()
        {
            PassViews = "A";
            Password_sw = "A";
        }

        /// <summary>
        /// 암호 수정
        /// </summary>
        /// <returns></returns>
        private async Task btnPassSave()
        {
            await staff_Lib.Edit_Pass(User_Code, Old_Password, New_Password);
            PassViews = "A";
            Password_sw = "A";
        }

        /// <summary>
        /// 기존 암호 확인
        /// </summary>
        private async Task OnPass(ChangeEventArgs a)
        {
            int reA = await staff_Lib.Login(User_Code, a.Value.ToString());

            if (reA > 0)
            {
                Old_Password = a.Value.ToString();
                Password_sw = "B"; 
            }
        }
    }
}
