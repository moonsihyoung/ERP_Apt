using Company;
using DbImage;
using Erp_Apt_Lib.Accounting;
using Erp_Apt_Lib.Appeal;
using Erp_Apt_Lib.apt_Erp_Com;
using Erp_Apt_Lib.Apt_Reports;
using Erp_Apt_Lib.Board;
using Erp_Apt_Lib.Check;
using Erp_Apt_Lib.Community;
using Erp_Apt_Lib.Decision;
using Erp_Apt_Lib.Documents;
using Erp_Apt_Lib.Draft;
using Erp_Apt_Lib.Logs;
using Erp_Apt_Lib.MaintenanceCost;
using Erp_Apt_Lib.MonthlyUsage;
using Erp_Apt_Lib.Plans;
using Erp_Apt_Lib.Print_Images;
using Erp_Apt_Lib.ProofReport;
using Erp_Apt_Lib.Stocks;
using Erp_Apt_Lib.Up_Files;
using Erp_Apt_Lib;
using Erp_Apt_Staff;
using Erp_Lib;
using Facilities;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.WebEncoders;
using Mobile.Areas.Identity;
using Mobile.Data;
using sw_Lib.Labors;
using Sw_Lib;
using System.ComponentModel;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using Wedew_Lib;
using Works;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("sw_togather");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddScoped<AuthenticationStateProvider, RevalidatingIdentityAuthenticationStateProvider<IdentityUser>>();
builder.Services.AddSingleton<WeatherForecastService>();

builder.Services.AddHttpClient();
builder.Services.AddScoped<Container>();
builder.Services.AddSignalR(e =>
{
    e.MaximumReceiveMessageSize = 10240000;
});
builder.Services.AddServerSideBlazor().AddHubOptions(o =>
{
    o.MaximumReceiveMessageSize = 30 * 1024 * 1024;
});

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromSeconds(50);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddRazorPages().AddSessionStateTempDataProvider();
//2022-09-06 추가
builder.Services.AddServerSideBlazor().AddCircuitOptions(options => { options.DetailedErrors = true; });

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(); //쿠기 인증

//한글처리
builder.Services.Configure<WebEncoderOptions>(options =>
{
    options.TextEncoderSettings = new TextEncoderSettings(UnicodeRanges.All); // 한글이 인코딩되는 문제 해결
});

Class_lib(builder.Services);

void Class_lib(object services)
{
    builder.Services.AddTransient<IApt_Lib, Apt_Lib>(); //아파트 정보
    builder.Services.AddTransient<IApt_Sub_Lib, Apt_Sub_Lib>(); // 아파트 정보 상세
    builder.Services.AddTransient<IAppeal_Bloom_Lib, Appeal_Bloom_Lib>();//민원 분류
    builder.Services.AddTransient<IAppeal_Lib, Appeal_Lib>(); //민원
    builder.Services.AddTransient<IsubAppeal_Lib, subAppeal_Lib>(); //민원 처리 내용
    builder.Services.AddTransient<IsubWorker_Lib, subWorker_Lib>(); //민원 처리자System.InvalidOperationException: 'Cannot modify ServiceCollection after application is built.'

    builder.Services.AddTransient<IErp_AptPeople_Lib, Erp_AptPeople_Lib>(); //입주민 정보
    builder.Services.AddTransient<IPost_Lib, Post_Lib>(); //직원 부서 정보
    builder.Services.AddTransient<IDuty_Lib, Duty_Lib>(); //직원 직책 정보
    builder.Services.AddTransient<ICar_Infor_Lib, Car_Infor_Lib>(); //차량관리 
    builder.Services.AddTransient<ISido_Lib, Sido_Lib>(); // 주소
    builder.Services.AddTransient<ILogs_Lib, Logs_Lib>(); // 로그 파일 만들기
    builder.Services.AddTransient<IStaff_Lib, Staff_Lib>(); // 관리소 직원
    builder.Services.AddTransient<IStaff_Sub_Lib, Staff_Sub_Lib>(); //관리소 직원 상세
    builder.Services.AddTransient<IReferral_career_Lib, Referral_career_Lib>(); //관리소 직원 배치정보
    builder.Services.AddTransient<IStaff_staffSub_Lib, Staff_staffSub_Lib>(); // 직원 상세 조인 정보
    builder.Services.AddTransient<IStaff_Career_Lib, Staff_Career_Lib>(); //관리소 직원 배치정보 조인 정보
    builder.Services.AddTransient<IPresent_Lib, Present_Lib>(); // 홈페이지 방문 정보
    builder.Services.AddTransient<IService_Worker_Lib, Service_Worker_Lib>(); // 작업자 정보
    builder.Services.AddTransient<IErp_Files_Lib, Erp_Files_Lib>(); //첨부 파일 관리
    builder.Services.AddTransient<ISw_Files_Lib, Sw_Files_Lib>(); //첨부 파일 관리 
    builder.Services.AddTransient<IUpFile_Lib, UpFile_Lib>(); //첨부파일 별도 관리
    builder.Services.AddTransient<IDefect_Lib, Defect_Lib>(); // 하자관리
    builder.Services.AddTransient<IBloom_Lib, Bloom_Lib>(); // 시설물 분류 관리
    builder.Services.AddTransient<IFacility_Lib, Facility_Lib>(); // 시설물 정보 관리
    builder.Services.AddTransient<ICompany_Lib, Company_Lib>(); // 업체 기본 정보 관리
    builder.Services.AddTransient<ICompany_Sub_Lib, Company_Sub_Lib>(); // 업체 상세 정보 관리
    builder.Services.AddTransient<ICompany_Apt_Career_Lib, Company_Apt_Career_Lib>(); // 위탁관리 정보
    builder.Services.AddTransient<ICompany_Join_Lib, Company_Join_Lib>(); // 공동주택 기본 및 상세 정보 통합
    builder.Services.AddTransient<IContract_Sort_Lib, Contract_Sort_Lib>(); // 업체 분류
    builder.Services.AddTransient<ICompany_Apt_Career_Lib, Company_Apt_Career_Lib>(); // 단지 계약 정보 
    builder.Services.AddTransient<IApt_Reports_Lib, Apt_Reports_Lib>();//단지별 보고서
    builder.Services.AddTransient<IReport_Bloom_Lib, Report_Bloom_Lib>();//보고서 분류 
    builder.Services.AddTransient<IReport_Division_Lib, Report_Division_Lib>();//보고서 구분 
    builder.Services.AddTransient<Ibs_apt_career, bs_apt_career>();//홈페이지 가입승인
    builder.Services.AddTransient<IIn_AptPeople_Lib, In_AptPeople_Lib>(); // 홈페이지 가입 입주민 정보
    builder.Services.AddTransient<ILabor_contract_Lib, Labor_contract_Lib>(); //계약서 관리
    builder.Services.AddTransient<Iwage_Lib, wege_Lib>(); //최저 임금
    builder.Services.AddTransient<IWorks_Lib, Works_Lib>(); //작업일지 정보
    builder.Services.AddTransient<IWorksSub_Lib, WorksSub_Lib>(); //작업내용 정보
    builder.Services.AddTransient<IDecision_Lib, Decision_Lib>(); //결재정보
    builder.Services.AddTransient<IDbImageLib, DbImageLib>(); //결재도장 정보
    builder.Services.AddTransient<IDbImagesLib, DbImagesLib>();  // 결재 도장 정보
    builder.Services.AddTransient<ICheck_List_Lib, Check_List_Lib>(); // 시설물 점검 목록 정보
    builder.Services.AddTransient<ICheck_Card_Lib, Check_Card_Lib>(); //시설물 점검표
    builder.Services.AddTransient<ICheck_Object_Lib, Check_Object_Lib>(); //시설물 점검 대상
    builder.Services.AddTransient<ICheck_Cycle_Lib, Check_Cycle_Lib>(); // 시설물 점검 주기
    builder.Services.AddTransient<ICheck_Input_Lib, Check_Input_Lib>();// 시설물 점검 완료
    builder.Services.AddTransient<ICheck_Items_Lib, Check_Items_Lib>(); //시설물 점검 사항
    builder.Services.AddTransient<ICheck_Effect_Lib, Check_Effect_Lib>(); // 시설물 점검 결과
    builder.Services.AddTransient<IApproval_Lib, Approval_Lib>(); //결재란 정보 클래스
    builder.Services.AddTransient<IDocument_Lib, Document_Lib>(); //문서관리 정보 클래스
    builder.Services.AddTransient<IDocument_Sort_Lib, Document_Sort_Lib>(); //문서분류관리 정보 클래스
    builder.Services.AddTransient<IDraft_Lib, Draft_Lib>(); //기안문서 관리 클래스
    builder.Services.AddTransient<IDraftDetail_Lib, DraftDetail_Lib>(); //기안문서 상세 관리 클래스
    builder.Services.AddTransient<IDraftAttach_Lib, DraftAttach_Lib>(); //기안문서 상세 항목 관리 클래스
    builder.Services.AddTransient<IProofReport_Lib, ProofReport_Lib>(); //제 증명 관리
    builder.Services.AddTransient<INotice_Lib, Notice_Lib>();//방송/공고 관리
    builder.Services.AddTransient<IStocks_Lib, Stocks_Lib>(); //자재관리 정보
    builder.Services.AddTransient<IWhSock_Lib, WhSock_Lib>(); //자재입출 관리 정보
    builder.Services.AddTransient<IDisbursementSort_Lib, DisbursementSort_Lib>(); //지출결의서 종류
    builder.Services.AddTransient<IDisbursement_Lib, Disbursement_Lib>();
    builder.Services.AddTransient<IAccount_Lib, Account_Lib>();//계정과목 정보
    builder.Services.AddTransient<IAccountSort_Lib, AccountSort_Lib>();
    builder.Services.AddTransient<IAccountDeals_Lib, AccountDeals_Lib>();
    builder.Services.AddTransient<IBankAccount_Lib, BankAccount_Lib>();
    builder.Services.AddTransient<IBankAccountDeals_Lib, BankAccountDeals_Lib>();
    builder.Services.AddTransient<IPrint_Images_Lib, Print_Images_Lib>();
    builder.Services.AddTransient<ICommunity_Lib, Community_Lib>(); //커뮤니티 관리
    builder.Services.AddTransient<ICommunityUsingKind_Lib, CommunityUsingKind_Lib>();//커뮤니티 종류
    builder.Services.AddTransient<ICommunityUsingTicket_Lib, CommunityUsingTicket_Lib>();//이용료 종류
    builder.Services.AddTransient<IPlan_Lib, Plans_Lib>(); //관리계획 관련 정보 관리
    builder.Services.AddTransient<IPlan_Sort_Lib, Plan_Sort_Lib>(); //관리계획 관련 분류 정보 관리
    builder.Services.AddTransient<IPlan_Man_Lib, Plan_Man_Lib>(); //
    builder.Services.AddTransient<Isw_Note_Lib, sw_Note_Lib>();
    builder.Services.AddTransient<ISurisan_Comments, Surisan_Comments>();
    builder.Services.AddTransient<Isw_Note_Sort_Lib, sw_Note_Sort_Lib>();
    builder.Services.AddTransient<IReadView_Lib, ReadView_Lib>();
    builder.Services.AddTransient<Iwedew_Lib, wedew_Lib>(); //게시판 
    builder.Services.AddTransient<IFiless_Lib, Filess_Lib>(); // 게시판 첨부 파일
    builder.Services.AddTransient<ISort_Lib, Sort_Lib>(); //게시판 분류
    builder.Services.AddTransient<IWedew_Comments, Wedew_Comments>(); //댓글
    builder.Services.AddTransient<ICostDebit_Lib, CostDebit_Lib>();//관리비 정보
    builder.Services.AddTransient<IMonthlyUsage_Lib, MonthlyUsage_Lib>(); //사용량 정보
    builder.Services.AddTransient<IUsageDetails_Lib, UsageDetails_Lib>(); //사용량 상세정보
    builder.Services.AddTransient<IBike_Lib, Bike_Lib>(); //자전거 정보
}

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseSession();
app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

//app.MapControllers();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}"
                    );
    endpoints.MapRazorPages();
    endpoints.MapBlazorHub();
    endpoints.MapFallbackToPage("/_Host");
});
//app.MapBlazorHub();
//app.MapFallbackToPage("/_Host");
//app.AddComponent<App>("app");
app.Run();
