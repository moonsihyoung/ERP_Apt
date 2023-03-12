using Company;
using Erp_Apt_Lib;
using Erp_Apt_Lib.Appeal;
using Erp_Apt_Staff;
using Erp_Entity;
using Erp_Lib;
using Facilities;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Erp_Apt_Web.Pages.Defect
{
    public partial class Index_M
    {
        //[Parameter]
        //public string Dong { get; set; }

        //[Parameter]
        //public string Ho { get; set; }

        [CascadingParameter]
        public Task<AuthenticationState> AuthenticationStateRef { get; set; }

        [Inject] public IApt_Lib apt_Lib { get; set; }  // 공동주택 정보
        [Inject] public IApt_Sub_Lib apt_Sub_Lib { get; set; }  // 공동주택 상세 정보
        [Inject] public IReferral_career_Lib referral_Career { get; set; }

        [Inject] public IPost_Lib post_Lib { get; set; }

        [Inject] public IDefect_Lib defect_lib { get; set; } // 하자 관리

        [Inject] public NavigationManager MyNav { get; set; } // Url 

        [Inject] public ISw_Files_Lib files_Lib { get; set; } // 파일 첨부

        //[Inject] public IAppeal_Bloom_Lib bloom_Lib { get; set; } //민원분류

        [Inject] public IAppeal_Bloom_Lib appeal_bloom_Lib { get; set; } //민원분류

        [Inject] IErp_AptPeople_Lib aptPeople_Lib { get; set; } // 입주민 정보 클래스

        [Inject] IBloom_Lib Bloom_Lib { get; set; }

        [Inject] IJSRuntime JSRuntime { get; set; }

        Defect_Entity bnn { get; set; } = new Defect_Entity();
        List<Referral_career_Entity> wnn { get; set; } = new List<Referral_career_Entity>();
        Sw_Files_Entity dnn { get; set; } = new Sw_Files_Entity();
        List<Post_Entity> pnn { get; set; } = new List<Post_Entity>();
        
        List<Sw_Files_Entity> Files_Entity { get; set; } = new List<Sw_Files_Entity>();
        List<Defect_Entity> ann = new List<Defect_Entity>();

        List<Appeal_Bloom_Entity> abe { get; set; } = new List<Appeal_Bloom_Entity>(); // 민원 분류
        List<Bloom_Entity> boo_b { get; set; } = new List<Bloom_Entity>(); //시설물 분류
        List<Bloom_Entity> boo_c { get; set; } = new List<Bloom_Entity>(); //시설물 분류
        List<Bloom_Entity> boo_d { get; set; } = new List<Bloom_Entity>(); //시설물 장소
        Apt_Sub_Entity apt_Sub_Entity { get; set; } = new Apt_Sub_Entity(); //공동주택 상세 정보
        List<Apt_People_Entity> apt_PoplesA { get; set; } = new List<Apt_People_Entity>(); //입주민 정보
        List<Apt_People_Entity> apt_PoplesH { get; set; } = new List<Apt_People_Entity>(); // 입주민 정보

        Apt_People_Entity apt_PoplesB { get; set; } = new Apt_People_Entity(); // 입주민 정보

        public string User_Code { get; set; }
        public string User_Name { get; set; }
        public string Apt_Code { get; set; }
        public string Apt_Name { get; set; }
        public string UserName { get; set; }
        public string Views { get; set; } = "A"; //상세 열기
        public string EditOpen { get; set; } = "A";//수정 열기
        public string InsertViews { get; set; } = "A"; // 하자 입력 열기
        public string Modals { get; set; } = "A"; // 업체정보 입력 열기
        public string lblContent { get; set; } = "";
        public int Files_Count { get; set; } = 0;
        public int intAid { get; set; } = 0;
        public string PostName { get; set; } = "a";
        public string PostCode { get; set; } = "a";
        public string FileViews { get; set; } = "A";
        public string strDong { get; set; }
        public string strHo { get; set; }

        public string Sort_Name { get; set; }
        public string Asort_Name { get; set; }
        public int Period { get; set; }
        public string Private { get; set; } = "B";
        public string strSido { get; set; } = "";
        public string strGunGu { get; set; } = "";
        
        public string FileInputViews { get; set; } = "A";
        public string CompleteViews { get; set; } = "A";

        

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

                //ann = await defect_lib.Private_List(Apt_Code);

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
        /// 데이터 메서드
        /// </summary>
        /// <returns></returns>
        private async Task DisplayData()
        {            
            ann = await defect_lib.GetList_DongHo(Apt_Code, strDong, strHo); //하자입력 정보 목록
        }

        /// <summary>
        /// 하자 입력(주요 사항)
        /// </summary>
        /// <returns></returns>
        public async Task onbtnSave()
        {
            var authState = await AuthenticationStateRef;
            if (authState.User.Identity.IsAuthenticated)
            {
                if (bnn.Aid > 0)
                {
                    if (bnn.Private == "B") //공용부분 하자
                    {
                        await defect_lib.Edit_Official(bnn);
                    }
                    else if (bnn.Private == "A") // 전용부분 하자
                    {
                        await defect_lib.Edit_Private(bnn);
                    }
                }
                else
                {
                    apt_Sub_Entity = await apt_Sub_Lib.Detail(Apt_Code);
                    bnn.Company_Name = apt_Sub_Entity.Builder;
                    bnn.Company_Code = Apt_Code;
                    if (bnn.Private == "공용") //공용부분 하자
                    {
                        if (bnn.dfPost == "" || bnn.dfPost == null)
                        {
                            await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "하자 접수 부서를 선택하지 않았습니다..");
                        }
                        else if (bnn.User_Code == "" || bnn.User_Code == null)
                        {
                            await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "입력자를 입력하지 않았습니다..");
                        }
                        else if (bnn.dfContent == "" || bnn.dfContent == null)
                        {
                            await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "하자 내요을 입력하지 않았습니다..");
                        }
                        else if (bnn.Bloom_Name_A == "Z")
                        {
                            await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "대분류를 선택하지 않았습니다..");
                        }
                        else if (bnn.Bloom_Name_B == "Z")
                        {
                            await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "중분류를 선택하지 않았습니다..");
                        }
                        else if (bnn.Bloom_Name_C == "Z")
                        {
                            await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "세분류를 선택하지 않았습니다..");
                        }
                        else if (bnn.Position == "Z")
                        {
                            await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "하자 발생 장소를 선택하지 않았습니다..");
                        }
                        else
                        {
                            bnn.dfYear = bnn.DefectDate.Year;
                            bnn.dfMonth = bnn.DefectDate.Month;
                            bnn.dfDay = bnn.DefectDate.Day;
                            bnn.User_Code = User_Name;
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
                            bnn.PostIp = myIPAddress;
                            #endregion

                            //bnn.Position_Code = 

                            bnn.dfTitle = bnn.Bloom_Name_A + "_" + bnn.Bloom_Name_B + "_" + bnn.Bloom_Name_C;

                            await defect_lib.Add_Official(bnn);

                            bnn = new Defect_Entity();
                            InsertViews = "A";
                            await DisplayData();
                        }
                    }
                    else if (bnn.Private == "전용") // 전용부분 하자
                    {
                        if (bnn.dfPost == "" || bnn.dfPost == null)
                        {
                            await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "하자 발생 장소를 입력하지 않았습니다..");
                        }
                        else if (bnn.User_Code == "" || bnn.User_Code == null)
                        {
                            await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "입력자를 입력하지 않았습니다..");
                        }
                        else if (bnn.dfContent == "" || bnn.dfContent == null)
                        {
                            await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "하자 내용을 입력하지 않았습니다..");
                        }
                        else if (bnn.Dong == "Z")
                        {
                            await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "동을 선택하지 않았습니다..");
                        }
                        else if (bnn.Ho == "Z")
                        {
                            await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "호를 선택하지 않았습니다..");
                        }
                        else if (bnn.InnerName == "Z")
                        {
                            await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "민원인을 입력하지 않았습니다..");
                        }
                        else if (bnn.Bloom_Name_B == "Z")
                        {
                            await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "중분류를 선택하지 않았습니다..");
                        }
                        else if (bnn.Bloom_Name_C == "Z")
                        {
                            await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "세분류를 선택하지 않았습니다..");
                        }
                        else if (bnn.Position == "Z")
                        {
                            await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "하자 발생 장소를 선택하지 않았습니다..");
                        }
                        else if (bnn.Dong == "" || bnn.Dong == null)
                        {
                            await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "동을 선택하지 않았습니다..");
                        }
                        else if (bnn.Ho == "" || bnn.Ho == null)
                        {
                            await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "호를 선택하지 않았습니다..");
                        }
                        else if (bnn.Relation == "" || bnn.Relation == null)
                        {
                            await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "관계를 입력하지 않았습니다..");
                        }
                        else
                        {
                            bnn.Bloom_Name_A = "전용 하자";
                            bnn.Bloom_Code_A = "1";

                            bnn.Mobile = apt_PoplesB.Hp;
                            bnn.InnerName = apt_PoplesB.InnerName;
                            bnn.Email = apt_PoplesB.Email;
                            bnn.Dong = strDong;
                            bnn.Ho = strHo;
                            bnn.User_Code = User_Name;

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
                            bnn.PostIp = myIPAddress;
                            bnn.dfYear = bnn.DefectDate.Year;
                            bnn.dfMonth = bnn.DefectDate.Month;
                            bnn.dfDay = bnn.DefectDate.Day;
                            #endregion

                            bnn.dfTitle = bnn.Bloom_Name_A + "_" + bnn.Bloom_Name_B + "_" + bnn.Bloom_Name_C;
                            bnn.Position_Code = "1";
                            await defect_lib.Add_Private(bnn);

                            bnn = new Defect_Entity();
                            InsertViews = "A";
                            await DisplayData();
                        }
                    }
                }


            }
            else
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "로그아웃 되었습니다..");
                MyNav.NavigateTo("/");
            }
        }

        /// <summary>
        /// 대분류 선택 실행
        /// </summary>
        protected async Task onSort(ChangeEventArgs args)
        {
            Sort_Name = args.Value.ToString();
            abe = new List<Appeal_Bloom_Entity>();
            bnn.Bloom_Code_B = await appeal_bloom_Lib.Sort_Code(Sort_Name);
            bnn.Bloom_Name_B = Sort_Name;
            Period = 0;
            abe = await appeal_bloom_Lib.Asort_List(args.Value.ToString());
        }

        /// <summary>
        /// 소분류 선택 실행
        /// </summary>
        protected async Task onAsort(ChangeEventArgs args)
        {
            Period = 0;
            Asort_Name = args.Value.ToString();
            bnn.Bloom_Code_C = await appeal_bloom_Lib.Asort_Code(Asort_Name);
            bnn.Bloom_Name_C = Asort_Name;
            Period = await appeal_bloom_Lib.Period(Asort_Name);
            bnn.Period = Period;
        }

        /// <summary>
        /// 전용 및 공용 선택 시 실행
        /// </summary>
        /// <param name="args"></param>
        protected void onPrivate(ChangeEventArgs args)
        {
            if (args.Value.ToString() == "공용")
            {
                Private = "B";
            }
            else
            {
                Private = "A";
            }

            bnn.Private = args.Value.ToString();
        }

        /// <summary>
        /// 시설물 대분류 선택 시 실행
        /// </summary>
        protected async Task onBloomA(ChangeEventArgs args)
        {
            bnn.Bloom_Name_A = args.Value.ToString();
            bnn.Bloom_Code_A = await Bloom_Lib.B_N_A_Code(bnn.Bloom_Name_A);
            boo_b = new List<Bloom_Entity>();
            boo_c = new List<Bloom_Entity>();
            boo_d = new List<Bloom_Entity>();
            boo_b = await Bloom_Lib.GetList_Apt_bb(Apt_Code, args.Value.ToString());
        }

        /// <summary>
        /// 시설물 중분류 선택 시 실행
        /// </summary>
        protected async Task onBloomB(ChangeEventArgs args)
        {
            bnn.Bloom_Name_B = args.Value.ToString();
            bnn.Bloom_Code_B = await Bloom_Lib.B_N_B_Code(bnn.Bloom_Name_A, bnn.Bloom_Name_B);
            boo_c = new List<Bloom_Entity>();
            boo_d = new List<Bloom_Entity>();
            boo_c = await Bloom_Lib.GetList_Apt_bc(Apt_Code, args.Value.ToString());
        }

        /// <summary>
        /// 시설물 소분류 선택 시 실행
        /// </summary>
        protected async Task onBloomC(ChangeEventArgs args)
        {
            bnn.Bloom_Name_C = args.Value.ToString();
            bnn.Bloom_Code_C = await Bloom_Lib.B_N_C_Code(bnn.Bloom_Name_A, bnn.Bloom_Name_B, bnn.Bloom_Name_C);
            bnn.Period = await Bloom_Lib.Period(bnn.Bloom_Code_C);

            boo_d = new List<Bloom_Entity>();
            boo_d = await Bloom_Lib.GetList_dd(Apt_Code, bnn.Bloom_Name_A);
        }

        /// <summary>
        /// 시설물 소분류 선택 시 실행
        /// </summary>
        protected async Task onBloomD(ChangeEventArgs args)
        {
            bnn.Position = args.Value.ToString();
            bnn.Position_Code = await Bloom_Lib.B_N_D_Code(bnn.Bloom_Name_A, bnn.Position, Apt_Code);
        }

        /// <summary>
        /// 동 정보
        /// </summary>
        /// <returns></returns>
        protected async Task onDong(ChangeEventArgs args)
        {
            strDong = args.Value.ToString();
            bnn.Dong = strDong;
            apt_PoplesA = await aptPeople_Lib.Dong_HoList(Apt_Code, strDong);
        }

        /// <summary>
        /// 호 정보
        /// </summary>
        /// <returns></returns>
        protected async Task onHo(ChangeEventArgs args)
        {
            strHo = args.Value.ToString();
            bnn.Ho = strHo;
            apt_PoplesB = await aptPeople_Lib.DongHo_Name(Apt_Code, strDong, strHo);
        }
    }
}
