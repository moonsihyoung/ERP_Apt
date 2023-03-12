using Erp_Apt_Lib.Appeal;
using Erp_Apt_Lib.Decision;
using Erp_Apt_Lib;
using Erp_Apt_Staff;
using Erp_Entity;
using Erp_Lib;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Erp_Apt_Web.Pages.Complain
{
    public partial class Details
    {
        #region 인스턴스
        [Parameter] public int Code { get; set; }
        [CascadingParameter] public Task<AuthenticationState> AuthenticationStateRef { get; set; }
        [Inject] public IApt_Lib apt_Lib { get; set; }  // 공동주택 정보
        [Inject] public IReferral_career_Lib referral_Career { get; set; }
        [Inject] public IPost_Lib post_Lib { get; set; }
        [Inject] public IAppeal_Lib appeal { get; set; } // 민원 접수
        [Inject] public NavigationManager MyNav { get; set; } // Url
        [Inject] public ISw_Files_Lib files_Lib { get; set; } // 파일 첨부
        [Inject] public IsubAppeal_Lib subAppeal { get; set; } //민원처리
        [Inject] public IsubWorker_Lib subWorker { get; set; } // 민원처리자
        [Inject] public IDecision_Lib decision_Lib { get; set; } // 결재정보
        [Inject] IJSRuntime JSRuntime { get; set; }
        [Inject] public IReferral_career_Lib referral_Career_Lib { get; set; }
        [Inject] public IDbImagesLib dbImageLib { get; set; }

        #endregion

        #region 제네릭
        Appeal_Entity ann { get; set; } = new Appeal_Entity();
        subAppeal_Entity bnn { get; set; } = new subAppeal_Entity();
        List<Referral_career_Entity> wnn { get; set; } = new List<Referral_career_Entity>();
        Sw_Files_Entity dnn { get; set; } = new Sw_Files_Entity();
        List<Post_Entity> pnn { get; set; } = new List<Post_Entity>();

        List<Sw_Files_Entity> Files_Entity { get; set; } = new List<Sw_Files_Entity>();
        List<subAppeal_Entity> snn { get; set; } = new List<subAppeal_Entity>();
        List<Approval_Entity> app { get; set; } = new List<Approval_Entity>();
        Referral_career_Entity referral { get; set; } = new Referral_career_Entity();
        public string PostDuty { get; private set; }
        #endregion

        #region 변수
        public string User_Code { get; set; }
        public string User_Name { get; set; }
        public string Apt_Code { get; set; }
        public string Apt_Name { get; set; }
        public string UserName { get; set; }
        public string Views { get; set; } = "A";
        public string lblContent { get; set; } = "";
        public int apAgoBe { get; private set; }
        public int apNextBe { get; private set; }
        public int Files_Count { get; set; } = 0;
        public string FileViews { get; set; } = "A";
        public string FileInsertViews { get; set; } = "A";
        public int intAid { get; set; } = 0;
        public string PostName { get; set; } = "a";
        public string PostCode { get; set; } = "a";
        public string PostCodeA { get; set; }
        public string Worker { get; set; } = "";

        public string Satisfaction { get; set; }
        public string decisionA { get; set; }
        public string strNum { get; set; }

        #endregion

        /// <summary>
        /// 페이지 로딩
        /// </summary>
        /// <returns></returns>
        protected override async Task OnInitializedAsync()
        {
            // = "sw134";
            if (Code != 0)
            {
                var authState = await AuthenticationStateRef;
                if (authState.User.Identity.IsAuthenticated)
                {
                    //로그인 정보
                    Apt_Code = authState.User.Claims.FirstOrDefault(c => c.Type == "Apt_Code")?.Value;
                    User_Code = authState.User.Claims.FirstOrDefault(c => c.Type == "User_Code")?.Value;
                    Apt_Name = authState.User.Claims.FirstOrDefault(c => c.Type == "Apt_Name")?.Value;
                    User_Name = authState.User.Claims.FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name")?.Value;

                    await DisplayViews(Code.ToString());
                    bnn.subDate = DateTime.Now;
                    app = await approval_Lib.GetList(Apt_Code, "민원일지");

                    referral = await referral_Career_Lib.Details(User_Code);
                    PostDuty = referral.Post + referral.Duty;
                }
                else
                {
                    MyNav.NavigateTo("/");
                }

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

        #region 결제 여부(민원)
        Decision_Entity decision { get; set; } = new Decision_Entity();
        [Inject] public IApproval_Lib approval_Lib { get; set; }
        [Inject] public IDbImagesLib dbImagesLib { get; set; }
        public string strUserName { get; set; }

        /// <summary>
        ///결재여부
        /// </summary>
        /// <param name="objPostDate"></param>
        /// <returns></returns>
        public string FuncShowOK(string strPostDuty, string Apt_Code, int apNum)
        {
            string strBloomCode = "Appeal";
            string strUserName = "";
            string nu = apNum.ToString();
            strNum = nu;
            int intA = decision_Lib.Details_Count(Apt_Code, strBloomCode, nu, strPostDuty);
            if (intA > 0)
            {
                decision = decision_Lib.Details(Apt_Code, strBloomCode, strNum, strPostDuty);
                //PostDuty = decision.PostDuty;
                strUserName = decision.UserName;
                strUserCode = decision.User_Code;
            }
            else
            {
                strPostDuty = "";
                strUserName = "";
            }


            if (intA < 1)
            {
                decisionA = "결재하기";
                return "결재하기";
            }
            else
            {
                decisionA = strUserName;
                return strUserName;
            }
        }

        /// <summary>
        /// 결재도장 불러오기
        /// </summary>
        public byte[] sealImage(string aptcode, string post, string duty, object apNum, string BloomCode, string UserCode)
        {
            string strNum = apNum.ToString();
            string strPostDuty = "";
            if (post == "회계(경리)")
            {
                post = "회계";
            }
            if (duty == "담당자")
            {
                strPostDuty = duty;
            }
            else
            {
                strPostDuty = post + duty;
            }

            byte[] strResult;
            int Being = decision_Lib.Details_Count(aptcode, BloomCode, strNum, strPostDuty);

            if (Being > 0)
            {
                Decision_Entity ann = decision_Lib.Details(aptcode, BloomCode, apNum.ToString(), strPostDuty);

                if (ann.User_Code != null && ann.User_Code != "A")
                {
                    if (strPostDuty != "담당자")
                    {
                        if (post == "회계")
                        {
                            post = "회계(경리)";
                        }
                        //string sstPostDuty = post + duty;

                        //decision = decision_Lib.Details(Apt_Code, BloomCode, strNum, strPostDuty);
                        //string User_Code = referral_Career_Lib.User_Code_Bes(aptcode, post, duty);
                        int CodeBeing = dbImagesLib.Photos_Count(UserCode);

                        if (CodeBeing > 0)
                        {
                            strResult = dbImagesLib.Photos_image(UserCode);
                        }
                        else
                        {
                            strResult = dbImagesLib.Photos_image("confirmation");
                        }
                    }
                    else
                    {
                        strResult = dbImagesLib.Photos_image("confirmation");
                    }
                }
                else if (ann.User_Code == "A")
                {
                    strResult = dbImagesLib.Photos_image("confirmation");
                }
                else
                {
                    strResult = dbImagesLib.Photos_image("inn");
                }
            }
            else
            {
                strResult = dbImagesLib.Photos_image("inn");
            }

            return strResult;
        }

        /// <summary>
        /// 결재 입력
        /// </summary>
        public async Task btnDecision()
        {
            decision.AptCode = Apt_Code;
            decision.BloomCode = "Appeal";
            decision.Parent = strNum;
            if (referral.Duty == "직원" || referral.Duty == "반원" || referral.Duty == "반장" || referral.Duty == "주임" || referral.Duty == "기사" || referral.Duty == "서무")
            {
                decision.PostDuty = "담당자";
            }
            else
            {
                decision.PostDuty = referral.Post + referral.Duty;
            }
            decision.User_Code = User_Code;
            decision.UserName = User_Name;

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
            decision.PostIP = myIPAddress;
            #endregion

            if (decision.PostDuty == "관리소장")
            {
                if (ann.innViw == "B")
                {
                    await decision_Lib.Add(decision);
                    await appeal.Edit_Complete(strNum);

                    app = await approval_Lib.GetList(Apt_Code, "민원일지");
                    await DisplayViews(strNum);
                }
                else
                {
                    await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "민원이 완료되지 않았습니다..");
                }
            }
            else
            {
                await decision_Lib.Add(decision);
                //await appeal.Edit_Complete(Code.ToString());

                app = await approval_Lib.GetList(Apt_Code, "민원일지");
                await DisplayViews(strNum);
            }
            StateHasChanged();
        }

        #endregion

        /// <summary>
        ///  데이타 보기
        /// </summary>
        /// <returns></returns>
        private async Task DisplayViews(string Code)
        {

            ann = await appeal.Detail(Code);
            lblContent = Dul.HtmlUtility.EncodeWithTabAndSpace(ann.apContent);
            apAgoBe = await appeal.apAgoBe(Apt_Code, Code);
            apNextBe = await appeal.apNextBe(Apt_Code, Code);
            Files_Count = await files_Lib.Sw_Files_Data_Index_Count("Appeal", ann.Num.ToString(), Apt_Code);

            if (Files_Count > 0)
            {
                Files_Entity = await files_Lib.Sw_Files_Data_Index("Appeal", ann.Num.ToString(), Apt_Code);
            }
            snn = await subAppeal.GetList(ann.Num.ToString());
        }

        /// <summary>
        /// 민원처리 입력 열기
        /// </summary>
        protected async Task Worked(int Num)
        {
            Views = "B";
            intAid = Num;
            bnn.subDate = DateTime.Now;
            pnn = await post_Lib.GetList("A");
            StateHasChanged();
        }

        /// <summary>
        /// 파일 첨부 열기
        /// </summary>
        protected void FiledBy(int Num)
        {
            FileInsertViews = "B";
            //StateHasChanged();
        }

        /// <summary>
        /// 첨부된 사진 보기
        /// </summary>
        protected async Task FileViewsBy(int Num)
        {
            FileViews = "B";
            Files_Count = await files_Lib.Sw_Files_Data_Index_Count("Appeal", ann.Num.ToString(), Apt_Code);
            if (Files_Count > 0)
            {
                Files_Entity = await files_Lib.Sw_Files_Data_Index("Appeal", ann.Num.ToString(), Apt_Code);
            }

            //StateHasChanged();
        }

        /// <summary>
        /// 파일 입력 닫기
        /// </summary>
        protected void FilesClose()
        {
            FileInsertViews = "A";
            //StateHasChanged();
        }

        /// <summary>
        /// 민원처리 입력 닫기
        /// </summary>
        protected void WorkClose()
        {
            Views = "A";
            bnn = new subAppeal_Entity();
            //StateHasChanged();
        }



        /// <summary>
        /// 민원처리 내용 입력
        /// </summary>
        /// <returns></returns>
        public async Task btnSave()
        {
            bnn.subWorker = Worker;
            bnn.subPost = PostName;
            bnn.apNum = ann.Num.ToString();
            bnn.AppealViw = "A";
            bnn.AptCode = Apt_Code;
            bnn.AptName = Apt_Name;
            bnn.Complete = "A";
            bnn.innView = "A";
            bnn.outMobile = "A";
            bnn.outName = "A";
            bnn.outViw = "A";

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
            bnn.subYear = bnn.subDate.Year;
            bnn.subMonth = bnn.subDate.Month;
            bnn.subDay = bnn.subDate.Day;
            bnn.subDuty = "A";

            if (bnn.subPost == "" || bnn.subPost == null)
            {
                await JSRuntime.InvokeAsync<object>("alert", "작업부서가 선택되지 않았습니다.");
            }
            else if (bnn.subWorker == "" || bnn.subWorker == null)
            {
                await JSRuntime.InvokeAsync<object>("alert", "작업자가 입력되지 않았습니다.");
            }
            else if (bnn.subContent == "" || bnn.subContent == null)
            {
                await JSRuntime.InvokeAsync<object>("alert", "작업내용이 입력되지 않았습니다.");
            }
            else
            {
                if (bnn.subAid < 1)
                {
                    await subAppeal.Add(bnn);
                }
                else
                {
                    await subAppeal.Edit(bnn);
                }
            }

            bnn = new subAppeal_Entity();

            //Files_Count = await files_Lib.Sw_Files_Data_Index_Count("Appeal", ann.Num.ToString(), Apt_Code);
            //if (Files_Count > 0)
            //{
            //    Files_Entity = await files_Lib.Sw_Files_Data_Index("Appeal", ann.Num.ToString(), Apt_Code);
            //}
            snn = await subAppeal.GetList(ann.Num.ToString());

            PostName = "";
            PostCode = "a";
            Worker = "";

            Views = "A";
            StateHasChanged();
        }

        /// <summary>
        /// 부서 선택 실행
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public async Task onPost(ChangeEventArgs args)
        {
            wnn = new List<Referral_career_Entity>();
            PostCode = args.Value.ToString();

            PostName = await post_Lib.PostName(PostCode);

            wnn = await referral_Career.GetList_Post_Staff_be(PostName, Apt_Code);
            StateHasChanged();
        }

        /// <summary>
        /// 작업자 만들기
        /// </summary>
        /// <param name="args"></param>
        public void onCareer(ChangeEventArgs args)
        {
            if (Worker == "")
            {
                Worker = PostName + "▶" + args.Value.ToString();
            }
            else
            {
                Worker = Worker + ", " + PostName + "▶" + args.Value.ToString();
            }

            StateHasChanged();
        }



        /// <summary>
        /// 파일 보기 닫기
        /// </summary>
        private void FilesViewsClose()
        {
            FileViews = "A";
            // StateHasChanged();
        }

        /// <summary>
        /// 작업 처리 내용 수정 열기
        /// </summary>
        private async Task btnSubEdit(subAppeal_Entity sann)
        {
            Views = "B";
            bnn = sann;
            pnn = await post_Lib.GetList("A");
            PostName = bnn.subPost;
            PostCode = await post_Lib.PostCode(bnn.subPost);
            Worker = bnn.subWorker;
            wnn = await referral_Career.GetList_Post_Staff_be(bnn.subPost, Apt_Code);
            //StateHasChanged();
        }

        /// <summary>
        /// 작업처리 내용 삭제
        /// </summary>
        private async Task btnSubRemove(int Aid)
        {
            bool isDelete = await JSRuntime.InvokeAsync<bool>("confirm", $"{Aid}번 글을 정말로 삭제하시겠습니까?");
            if (isDelete)
            {
                await subAppeal.Remove(Aid.ToString()); // 처리내용 삭제
                snn = await subAppeal.GetList(ann.Num.ToString());
                StateHasChanged();
            }
            else
            {
                await JSRuntime.InvokeAsync<object>("alert", "삭제되지 않았습니다.");
            }
        }

        #region Event Handlers
        private long maxFileSize = 1024 * 1024 * 30;
        [Inject] Microsoft.AspNetCore.Hosting.IWebHostEnvironment env { get; set; }
        public string CompleteViews { get; private set; }
        public string strUserCode { get; private set; }

        /// <summary>
        /// 저장하기 버튼 클릭 이벤트 처리기
        /// </summary>
        protected async void btnFileSave()
        {
            dnn.Parents_Num = ann.Num.ToString(); // 선택한 ParentId 값 가져오기 
            dnn.Sub_Num = dnn.Parents_Num;
            var fileName = "";

            var format = "image/png";

            #region 파일 업로드 관련 추가 코드 영역

            foreach (var file in selectedImage)
            {
                var resizedImageFile = await file.RequestImageFileAsync(format, 1025, 1024);
                var buffer = new byte[resizedImageFile.Size];
                await resizedImageFile.OpenReadStream(maxFileSize).ReadAsync(buffer);
                var imageDataUrl = $"data:{format};base64,{Convert.ToBase64String(buffer)}";

                Stream stream = resizedImageFile.OpenReadStream(maxFileSize);

                var path = $"E:\\Img_Files\\Complains";

                fileName = Dul.FileUtility.GetFileNameWithNumbering(path, file.Name);
                path = path + $"\\{fileName}";

                FileStream fs = File.Create(path);
                await stream.CopyToAsync(fs);
                fs.Close();

                dnn.Sw_FileName = fileName;
                dnn.Sw_FileSize = Convert.ToInt32(file.Size);
                dnn.Parents_Name = "Appeal";
                dnn.AptCode = Apt_Code;
                dnn.Del = "A";

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
                dnn.PostIP = myIPAddress;
                #endregion
                await files_Lib.Sw_Files_Date_Insert(dnn); //첨부파일 데이터 db 저장

                //await defect_lib.Edit_ImagesCount(bnn.Aid); // 첨부파일 추가를 db 저장(하자, defect)


                //dnn = new Sw_Files_Entity();
            }


            FileInsertViews = "A";

            Files_Count = await files_Lib.Sw_Files_Data_Index_Count("Appeal", ann.Num.ToString(), Apt_Code);
            if (Files_Count > 0)
            {
                Files_Entity = await files_Lib.Sw_Files_Data_Index("Appeal", ann.Num.ToString(), Apt_Code);
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
        /// 첨부 파일 삭제
        /// </summary>
        protected async Task ByFileRemove(Sw_Files_Entity _files)
        {

            bool isDelete = await JSRuntime.InvokeAsync<bool>("confirm", $"{_files.Sw_FileName} 첨부파일을 정말로 삭제하시겠습니까?");

            if (isDelete)
            {
                if (!string.IsNullOrEmpty(_files?.Sw_FileName))
                {
                    // 첨부 파일 삭제 
                    //await FileStorageManagerInjector.DeleteAsync(_files.Sw_FileName, "");
                    string rootFolder = $"E:\\Img_Files\\Complains\\{_files.Sw_FileName}";
                    File.Delete(rootFolder);
                }
                await files_Lib.FileRemove(_files.Num.ToString(), "Appeal", Apt_Code);
                //await UploadRepositoryAsyncReference.DeleteAsync(Id); // 삭제
                //NavigationManagerReference.NavigateTo("/Uploads"); // 리스트 페이지로 이동
                Files_Count = await files_Lib.Sw_Files_Data_Index_Count("Appeal", ann.Num.ToString(), Apt_Code);
                if (Files_Count > 0)
                {
                    Files_Entity = await files_Lib.Sw_Files_Data_Index("Appeal", ann.Num.ToString(), Apt_Code);
                }
            }
            else
            {
                await JSRuntime.InvokeAsync<object>("alert", "취소되었습니다.");
            }
        }

        /// <summary>
        /// 만족도 선택 
        /// </summary>
        private void onSortAAA(ChangeEventArgs args)
        {
            Satisfaction = args.Value.ToString();
            //StateHasChanged();
        }

        /// <summary>
        /// 민원완료 입력
        /// </summary>
        /// <returns></returns>
        public async Task btnAppealCompleteSave()
        {
            if (ann.innViw == "C" || ann.innViw == "A")
            {
                await appeal.apSatisfaction(ann.Num.ToString(), Satisfaction);
                await appeal.Edit_WorkComplete(ann.Num.ToString());
            }
            else
            {
                await appeal.apSatisfaction(ann.Num.ToString(), null);
                await appeal.Edit_WorkComplete(ann.Num.ToString());
            }
            await DisplayViews(ann.Num.ToString());
            CompleteViews = "A";
            //StateHasChanged();
        }

        /// <summary>
        /// 민원완료 닫기
        /// </summary>
        private void btnAppealCompleteClose()
        {
            CompleteViews = "A";
            //StateHasChanged();
        }

        /// <summary>
        /// 민원완료 열기
        /// </summary>
        private void CompleteBy(int Num)
        {
            ann.Num = Num;
            CompleteViews = "B";
            //StateHasChanged();
        }

        private async Task btnAgo(int Num)
        {
            apAgoBe = await appeal.apAgoBe(Apt_Code, Num.ToString());
            apNextBe = await appeal.apNextBe(Apt_Code, Num.ToString());
            if (apAgoBe > 0)
            {
                strNum = await appeal.apAgo(Apt_Code, Num.ToString());
                ann = await appeal.Detail(strNum);
                lblContent = Dul.HtmlUtility.EncodeWithTabAndSpace(ann.apContent);
                apAgoBe = await appeal.apAgoBe(Apt_Code, Code.ToString());
                Files_Count = await files_Lib.Sw_Files_Data_Index_Count("Appeal", ann.Num.ToString(), Apt_Code);

                if (Files_Count > 0)
                {
                    Files_Entity = await files_Lib.Sw_Files_Data_Index("Appeal", ann.Num.ToString(), Apt_Code);
                }
                snn = await subAppeal.GetList(ann.Num.ToString());
            }


        }

        private async Task btnNext(int Num)
        {
            apNextBe = await appeal.apNextBe(Apt_Code, Num.ToString());
            apAgoBe = await appeal.apAgoBe(Apt_Code, Num.ToString());
            if (apNextBe > 0)
            {
                strNum = await appeal.apNext(Apt_Code, Num.ToString());
                ann = await appeal.Detail(strNum);
                lblContent = Dul.HtmlUtility.EncodeWithTabAndSpace(ann.apContent);
                apNextBe = await appeal.apNextBe(Apt_Code, Code.ToString());
                Files_Count = await files_Lib.Sw_Files_Data_Index_Count("Appeal", ann.Num.ToString(), Apt_Code);

                if (Files_Count > 0)
                {
                    Files_Entity = await files_Lib.Sw_Files_Data_Index("Appeal", ann.Num.ToString(), Apt_Code);
                }
                snn = await subAppeal.GetList(ann.Num.ToString());
            }

        }

        /// <summary>
        /// 돌아가기
        /// </summary>
        /// <returns></returns>
        private void btnList()
        {
            MyNav.NavigateTo("/Complain");
        }
    }
}
