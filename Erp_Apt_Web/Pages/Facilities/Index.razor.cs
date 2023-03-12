using Erp_Apt_Staff;
using Erp_Lib;
using Facilities;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Erp_Apt_Web.Pages.Facilities
{
    public partial class Index
    {
        [CascadingParameter]
        public Task<AuthenticationState> AuthenticationStateRef { get; set; }

        [Inject]
        public IApt_Lib apt_Lib { get; set; }  // 공동주택 정보

        [Inject]
        public IReferral_career_Lib referral_Career { get; set; }

        [Inject] public IBloom_Lib bloom { get; set; }

        [Inject] public NavigationManager MyNav { get; set; } // Url 

        [Inject] IJSRuntime JSRuntime { get; set; }

        List<Bloom_Entity> fbe = new List<Bloom_Entity>();
        List<Bloom_Entity> fbe_b = new List<Bloom_Entity>();
        List<Bloom_Entity> fbe_C = new List<Bloom_Entity>();

        List<Bloom_Entity> fbe_Z = new List<Bloom_Entity>();

        Bloom_Entity ann = new Bloom_Entity();
        Bloom_Entity bnn = new Bloom_Entity();
        Bloom_Entity cnn = new Bloom_Entity();
        Bloom_Entity dnn = new Bloom_Entity();

        public string Select { get; set; } = "A";

        public string User_Code { get; set; }
        public string User_Name { get; set; }
        public int LevelCount { get; private set; }
        public string Apt_Code { get; set; }
        public string Apt_Name { get; set; }
        public string UserName { get; set; }
        public string Views { get; set; } = "Z";
        public string BloomA { get; set; }
        public string BloomB { get; set; }
        public string BloomC { get; set; }

        public string Sort_A { get; set; } = null;
        public int inta { get; set; } = 0;
        public int intb { get; set; } = 0;


        #region MyRegion
        //private int totalPageCount = 0;
        //private int? Page = 0;
        //private int CurrentPage = 0;
        //private int TotalCount = 0;
        //private int PageSize = 20;
        //private double dbTotal = 0;
        //private int PageListCount = 5;
        //private int firstPage = 0;
        //private int lastPage = 0; 
        #endregion

        /// <summary>
        /// 로딩 시 실행
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
                //ann = await defect_lib.GetList(Apt_Code);

                if (LevelCount >= 5)
                {
                    await DisplayData();
                }
                else
                {
                    await JSRuntime.InvokeAsync<object>("alert", "권한이 없습니다.");
                    MyNav.NavigateTo("/");
                }
            }
            else
            {
                await JSRuntime.InvokeAsync<object>("alert", "로그인되지 않았습니다.");
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
        private async Task DisplayData()
        {
            pager.RecordCount = await bloom.GetListCount();
            inta = pager.RecordCount;
            intb = (pager.PageIndex * 15);
            inta = inta - intb;
            fbe = await bloom.GetList(pager.PageIndex); //시설물 정보 목록
        }

        protected DulPager.DulPagerBase pager = new DulPager.DulPagerBase()
        {
            PageNumber = 1,
            PageIndex = 0,
            PageSize = 15,
            PagerButtonCount = 5
        };

        protected async void PageIndexChanged(int pageIndex)
        {
            pager.PageIndex = pageIndex;
            pager.PageNumber = pageIndex + 1;

            await DisplayData();

            StateHasChanged();
        }

        /// <summary>
        /// 대분류 선택 시 중분류 목록 만들기
        /// </summary>
        protected async Task onBloomAA(ChangeEventArgs args)
        {
            if (args.Value.ToString() != "Z")
            {
                Sort_A = args.Value.ToString();
                fbe = await bloom.GetList_bb(args.Value.ToString());
                fbe_Z = fbe;
            }
        }

        /// <summary>
        /// 목록에서 중분류 선택 시 세분류 목록 만들기
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        protected async Task onBloomBB(ChangeEventArgs args)
        {
            fbe = await bloom.GetList_Apt_bc("sw1", args.Value.ToString());
        }

        /// <summary>
        /// 대분류 입력
        /// </summary>
        protected void btnBloomA()
        {
            Select = "A";
        }

        /// <summary>
        /// 중분류 입력
        /// </summary>
        protected void btnBloomB()
        {
            Select = "B";
        }

        /// <summary>
        /// 중분류 입력
        /// </summary>
        protected void btnBloomC()
        {
            Select = "C";
        }

        /// <summary>
        /// 작업 장소 입력
        /// </summary>
        protected void btnBloomD()
        {
            Select = "D";
        }

        /// <summary>
        /// 시설물 분류 입력 혹은 수정
        /// </summary>
        /// <returns></returns>
        protected async Task btnSave()
        {
            if (Select == "A")
            {
                ann.Bloom_Code = Select;

                if (ann.Num > 0)
                {
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
                    ann.ModifyIP = myIPAddress;
                    ann.ModifyDate = DateTime.Now;
                    #endregion
                    await bloom.Edit(ann);
                }
                else
                {
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
                    ann.PostIP = myIPAddress;
                    #endregion
                    await bloom.Add(ann);
                }
                
                fbe = await bloom.GetList_bb(ann.B_N_A_Name);
                ann = new Bloom_Entity();
            }
            else if (Select == "B")
            {
                bnn.Bloom_Code = Select;

                if (bnn.Num > 0)
                {
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
                    bnn.ModifyIP = myIPAddress;
                    bnn.ModifyDate = DateTime.Now;
                    #endregion
                    await bloom.Edit(bnn);
                }
                else
                {
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
                    await bloom.Add(bnn);
                }
                
                fbe = await bloom.GetList_bb(bnn.B_N_A_Name);
                bnn = new Bloom_Entity();
            }
            else if (Select == "C")
            {
                cnn.Bloom_Code = Select;

                if (cnn.Num > 0)
                {
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
                    cnn.ModifyIP = myIPAddress;
                    cnn.ModifyDate = DateTime.Now;
                    #endregion
                    await bloom.Edit(cnn);
                }
                else
                {
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
                    cnn.PostIP = myIPAddress;
                    #endregion
                    await bloom.Add(cnn);

                    //fbe = await bloom.GetList_cc(cnn.B_N_C_Name);
                }
                
                fbe = await bloom.GetList_cc(cnn.B_N_B_Name);
                cnn = new Bloom_Entity();
            }
            else if (Select == "D")
            {
                dnn.Bloom_Code = Select;

                if (dnn.Num > 0)
                {
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
                    dnn.ModifyIP = myIPAddress;
                    dnn.ModifyDate = DateTime.Now;
                    #endregion
                    await bloom.Edit(dnn);
                    //fbe = await bloom.GetList_dd(dnn.B_N_A_Name);
                }
                else
                {
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
                    await bloom.Add(dnn);
                    
                }
                
                fbe = await bloom.GetList_dd(Apt_Code, dnn.B_N_A_Name);
                dnn = new Bloom_Entity();
            }

            
        }

        /// <summary>
        /// 중분류 입력에서 대부분 선택 시 실행
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        protected async Task onBloomA(ChangeEventArgs args)
        {
            if (args.Value.ToString() != "Z")
            {
                if (Select == "B")
                {
                    bnn.B_N_A_Name = args.Value.ToString();
                }
                else if (Select == "C")
                {
                    cnn.B_N_A_Name = args.Value.ToString();
                }
                else if (Select == "D")
                {
                    dnn.B_N_A_Name = args.Value.ToString();
                }
                fbe_C = new List<Bloom_Entity>();
                fbe_b = new List<Bloom_Entity>();
                fbe_b = await bloom.GetList_Apt_bb(Apt_Code, args.Value.ToString()); 
            }
        }

        /// <summary>
        /// 세분류 입력에서 대부분 선택 시 실행
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        protected async Task onBloomB(ChangeEventArgs args)
        {
            if (args.Value.ToString() != "Z")
            {
                if (Select == "C")
                {
                    cnn.B_N_B_Name = args.Value.ToString();
                }
                else if (Select == "D")
                {
                    dnn.B_N_B_Name = args.Value.ToString();
                }

                fbe_C = new List<Bloom_Entity>();

                fbe_C = await bloom.GetList_Apt_bc(Apt_Code, args.Value.ToString()); 
            }
        }

        /// <summary>
        /// 선택 시 실행
        /// </summary>
        /// <param name="entity"></param>
        protected async Task btnSelect(Bloom_Entity entity)
        {
            if (entity.Bloom_Code == "A")
            {
                Select = entity.Bloom_Code;
                ann = entity;
            }
            else if (entity.Bloom_Code == "B")
            {
                BloomA = entity.B_N_A_Name;
                Select = entity.Bloom_Code;
                bnn = entity;
                fbe = await bloom.GetList_bb(entity.B_N_A_Name);
            }
            else if (entity.Bloom_Code == "C")
            {
                BloomA = entity.B_N_A_Name;
                BloomB = entity.B_N_B_Name;

                Select = entity.Bloom_Code;
                cnn = entity;
                fbe = await bloom.GetList_cc(entity.B_N_B_Name);
            }
            else if (entity.Bloom_Code == "D")
            {
                BloomA = entity.B_N_A_Name;
                BloomB = entity.B_N_B_Name;
                BloomC = entity.B_N_C_Name;
                Select = entity.Bloom_Code;
                dnn = entity;
                fbe = await bloom.GetList_dd(Apt_Code, entity.B_N_A_Name);
            }
        }

        /// <summary>
        /// 선택 시 실행
        /// </summary>
        /// <param name="entity"></param>
        protected async Task btnRemove(int aid)
        {
            bool isDelete = await JSRuntime.InvokeAsync<bool>("confirm", $"{aid}번 글을 정말로 삭제하시겠습니까?");
            if (isDelete)
            {
                await bloom.Remove(aid.ToString());
                await DisplayData(); //시설물 분류 정보 목록 
            }
        }

        /// <summary>
        /// 장소정보 버튼
        /// </summary>
        /// <returns></returns>
        protected async Task onPosition()
        {
            if (Sort_A != null)
            {
                fbe = await bloom.GetList_dd(Apt_Code, Sort_A);
            }
            
        }
    }
}
