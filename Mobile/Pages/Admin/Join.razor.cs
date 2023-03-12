using Erp_Entity;
using Erp_Lib;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Mobile.Pages.Admin
{
    public partial class Join
    {
        [Inject] public ISido_Lib sido_Lib { get; set; }
        [Inject] public IApt_Lib apt_Lib { get; set; }
        [Inject] public IErp_AptPeople_Lib aptPeople_Lib { get; set; } // 입주자 카드 정보 클래스
        [Inject] public IIn_AptPeople_Lib in_AptPople_Lib { get; set; } // 홈페이지 가입회원 정보 클래스
        [Inject] IJSRuntime JSRuntime { get; set; }
        [Inject] public NavigationManager MyNav { get; set; } // Url 
        [Inject] IHttpContextAccessor contextAccessor { get; set; }

        List<Sido_Entity> sidos { get; set; } = new List<Sido_Entity>(); // 시도 정보
        List<Apt_Entity> apts { get; set; } = new List<Apt_Entity>(); //공동주택 정보
        List<Apt_People_Entity> apt_PoplesA { get; set; } = new List<Apt_People_Entity>(); //입주민 목록 정보
        List<Apt_People_Entity> apt_PoplesB { get; set; } = new List<Apt_People_Entity>(); //입주민 목록 정보
        Apt_People_Entity dnn { get; set; } = new Apt_People_Entity(); //입주민 정보

        public In_AptPeople_Entity ann { get; set; } = new In_AptPeople_Entity();// 홈페이지 입주민 가입자

        public string Sido { get; set; }
        public string SiGuGo { get; set; }
        public string Checkd { get; set; } = "A";
        public string Views { get; set; } = "A";
        public bool chkInfor { get; set; } = false;
        public bool chkAppoval { get; set; } = false;
        public string Confirm { get; set; }
        public string Result { get; set; } = "A";
        public string UserId_confirm { get; set; } = "A";
        public string pass_confirm { get; set; } = "A";
        public string AptSelect { get; set; } = "A";
        public string Apt_Code { get; set; } = "";
        public string Apt_Name { get; set; }
        public string strDong { get; set; } = "A";
        public string strHo { get; set; } = "A";


        protected override void OnInitialized()
        {
            ann = new In_AptPeople_Entity();
            dnn = new Apt_People_Entity();
            ann.Scn = Convert.ToDateTime("1980-01-01");
            ann.Mobile = "";
        }

        /// <summary>
        /// 모달 이동 
        /// </summary>
        public string MyIpAdress { get; set; }
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            //await JSRuntime.InvokeVoidAsync("setModalDraggableAndResizable");
            await base.OnAfterRenderAsync(firstRender);

            if (firstRender)
            {
                MyIpAdress = contextAccessor.HttpContext.Connection?.RemoteIpAddress.ToString();
                StateHasChanged();
            }
        }

        /// <summary>
        /// 시도 선택 실행
        /// </summary>
        protected async Task OnSido(ChangeEventArgs args)
        {
            sidos = await sido_Lib.GetList_Code(args.Value.ToString());
            Sido = args.Value.ToString();
        }

        /// <summary>
        /// 시군구 선택 실행
        /// </summary>
        public async Task onGunGu(ChangeEventArgs args)
        {
            SiGuGo = await sido_Lib.SidoName(Sido);
            apts = await apt_Lib.GetList_All_Sido_Gun(SiGuGo, args.Value.ToString());
            //StateHasChanged();
        }

        /// <summary>
        /// 아파트 선택 시 실행
        /// </summary>
        protected async Task onApt(ChangeEventArgs args)
        {
            ann.Apt_Code = args.Value.ToString();
            Apt_Code = args.Value.ToString();

            ann.Apt_Name = await apt_Lib.Apt_Name(ann.Apt_Code);
            Apt_Name = ann.Apt_Name;
            apt_PoplesA = await aptPeople_Lib.DongList(ann.Apt_Code); //해당 공동주택 동정보 목록

        }

        /// <summary>
        /// 동 정보
        /// </summary>
        protected async Task onDong(ChangeEventArgs args)
        {
            strDong = args.Value.ToString();
            ann.Dong = strDong;
            apt_PoplesB = await aptPeople_Lib.Dong_HoList_Ds(ann.Apt_Code, strDong);
        }

        /// <summary>
        /// 호 정보
        /// </summary>
        List<Apt_People_Entity> HoList = new List<Apt_People_Entity>();
        protected async Task onHo(ChangeEventArgs args)
        {
            ann.Ho = args.Value.ToString();
            strHo = ann.Ho;
            //HoList = await aptPeople_Lib.Dong_Ho_Name_List(Apt_Code, strDong, strHo);
            ann.Mobile = "";
            if (string.IsNullOrWhiteSpace(Apt_Code))
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "공동주택을 선택하지 않았습니다. 먼저 공동주택을 선택하세요.");
            }
            else if (string.IsNullOrWhiteSpace(strDong))
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "동이름을 선택하지 않았습니다. 먼저 동이름을 선택하세요.");
            }
            else if (string.IsNullOrWhiteSpace(strHo))
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "호이름을 선택하지 않았습니다. 먼저 호이름을 선택하세요.");
            }
            else
            {
                Views = "B";
                AptSelect = "A";
            }
        }

        /// <summary>
        /// 개인 정보 동의
        /// </summary>
        protected void onchkAppoval()
        {
            if (chkAppoval)
            {
                chkAppoval = false;
            }
            else
            {
                chkAppoval = true;
            }

            if (chkInfor && chkAppoval)
            {
                Checkd = "B";
                //iews = "B";
                AptSelect = "B";
            }
            else
            {
                Checkd = "A";
                //Views = "A";
                AptSelect = "A";
            }
            StateHasChanged();
        }

        /// <summary>
        /// 홈페이지 가입 동의
        /// </summary>
        protected void onchkInfor()
        {
            if (chkInfor)
            {
                chkInfor = false;
            }
            else
            {
                chkInfor = true;
            }

            if (chkInfor && chkAppoval)
            {
                Checkd = "B";
                //Views = "B";
                AptSelect = "B";
            }
            else
            {
                Checkd = "A";
                //Views = "A";
                AptSelect = "A";
            }

            StateHasChanged();
        }

        /// <summary>
        /// 아이디 중복확인
        /// </summary>
        protected async Task OnResult()
        {
            int Count = await in_AptPople_Lib.Be_UserCode(ann.User_Code);

            int length = ann.User_Code.Length;

            if (Count > 0)
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "이미 등록 아이디 입니다..");
                ann.User_Code = "";
                UserId_confirm = "A";
            }
            else if (length < 6)
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "아이디는 6자 이상이어야 합니다..");
                UserId_confirm = "A";
            }
            else
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "사용할 수 있는 아이디입니다..");
                UserId_confirm = "B";
            }
            StateHasChanged();
        }

        /// <summary>
        /// 암호 확인
        /// </summary>
        protected async Task OnConfirm()
        {
            if (ann.Pass_Word == Confirm)
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "확인되었습니다..");
                pass_confirm = "B";
            }
            else
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "일치하지 않습니다.. 다시 입력해 주세요.");
                Confirm = "";
                pass_confirm = "A";
            }
            StateHasChanged();
        }

        /// <summary>
        /// 회원 가입
        /// </summary>
        protected async Task OnSave()
        {
            if (pass_confirm != "B" && ann.Pass_Word == null)
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "암호를 확인하지 않았습니다..");
            }
            else if (string.IsNullOrWhiteSpace(ann.User_Name))
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "이름을 입력하지 않았습니다..");
            }
            else if (ann.Mobile == null)
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "핸드폰 번호를 입력하지 않았습니다..");
            }
            else
            {
                ann.Mobile = ann.Mobile.Replace("-", "");

                ann.Mobile = ann.Mobile.Substring(0, 3) + "-" + ann.Mobile.Substring(3, 4) + "-" + ann.Mobile.Substring(7, 4);

                int be = await in_AptPople_Lib.Mobile_Being(ann.Mobile);

                if (be < 1)
                {
                    ann.PostIP = MyIpAdress;
                    //ann.Apt_Code = dnn.Apt_Code;
                    //ann.Apt_Name = dnn.Apt_Name;
                    ann.User_Code = ann.Apt_Code + ann.Dong + ann.Ho;
                    ann.Telephone = ann.Mobile;
                    ann.Withdrawal = "A";
                    ann.LevelCount = 1;
                    ann.Approval = "A";
                    if (string.IsNullOrWhiteSpace(ann.Intro))
                    {
                        ann.Intro = "기재하지 않음.";
                    }
                    await in_AptPople_Lib.add(ann);

                    await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "등록에 성공했습니다..\n 관리사무소에서 승인해야만 로그인이 가능합니다. \n 보다 자세한 사항은 관리사무소에 문의바랍니다.(연락처 : 02-2611-0967)");

                    MyNav.NavigateTo("Logs/Index", true);
                }
                else
                {
                    #region MyRegion
                    //bool isDelete = await JSRuntime.InvokeAsync<bool>("confirm", $"{ann.Mobile} 은 이미 등록된 정보가 있습니다.., \n 이미 입력된 공동주택은 더 이상 사용할 수 없습니다.... \n  그래도 등록하시겠습니까?");
                    //if (isDelete)
                    //{
                    //    try
                    //    {
                    //        await in_AptPople_Lib.Approval_Remove(ann.Mobile);

                    //        //dnn = await aptPeople_Lib.Dedeils_Mobile(ann.Mobile, ann.Dong, ann.Ho);

                    //        #region 아이피 입력
                    //        string myIPAddress = "";
                    //        var ipentry = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName());
                    //        foreach (var ip in ipentry.AddressList)
                    //        {
                    //            if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                    //            {
                    //                myIPAddress = ip.ToString();
                    //                break;
                    //            }
                    //        }
                    //        ann.PostIP = myIPAddress;
                    //        #endregion
                    //        ann.Apt_Code = dnn.Apt_Code;
                    //        ann.Apt_Name = dnn.Apt_Name;
                    //        ann.User_Code = dnn.Apt_Code + dnn.Dong + dnn.Ho;
                    //        ann.Telephone = ann.Mobile;
                    //        ann.Withdrawal = "A";

                    //        await in_AptPople_Lib.add(ann);

                    //        await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "등록에 성공했습니다..");


                    //        MyNav.NavigateTo("Logs/Index", true);
                    //    }
                    //    catch (Exception e)
                    //    {
                    //        await in_AptPople_Lib.Approval_Remove_ReSet(ann.Mobile);
                    //        await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", e + "등록에 실패했습니다..");
                    //    }
                    //}
                    //else
                    //{
                    //    ann.Mobile = "";

                    //    await Focus("txtMobile");
                    //}
                    #endregion

                    await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "이미 등록된 입주민 입니다. \n 보다 자세한 사항을 알고 싶은 입주민께서는 관리사무소로 문의하세요.");
                }
            }
        }

        private async Task OnHoViews(ChangeEventArgs a)
        {
            if (a != null)
            {
                string re = a.Value.ToString();

                dnn = await aptPeople_Lib.Dedeils_Name(re);
                //ann.User_Name = dnn.InnerName;
                if (!string.IsNullOrWhiteSpace(dnn.Hp))
                {
                    ann.Mobile = dnn.Hp;
                    ann.Pass_Word = null;

                    try
                    {
                        if (string.IsNullOrWhiteSpace(dnn.InnerScn1))
                        {
                            ann.Scn = DateTime.Now;
                        }
                        else
                        {
                            ann.Scn = Convert.ToDateTime(dnn.InnerScn1);
                        }
                    }
                    catch (Exception)
                    {
                        ann.Scn = DateTime.Now;
                    }
                }
                else
                {
                    ann.Mobile = null;
                    //ann.Scn = null;
                    await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "입주자 카드에 모바일 전화 번호를 등록하지 않았습니다. \n 핸드폰 번호가 없는 경우, 주민운동시설 및 민원신청 등을 이용하기 위한 회원 가입이 불가능합니다.  \n 보다 자세한 사항은 관리사무소로 문의하세요.");
                }
            }
        }

        public async Task Focus(string elementId)
        {
            await JSRuntime.InvokeVoidAsync("jsfunction.focusElement", elementId);
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
                    ann.Scn = Convert.ToDateTime(strYear + "-" + strMonth + "-" + strDay);
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
    }
}
