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
//2022-09-06 �߰�
builder.Services.AddServerSideBlazor().AddCircuitOptions(options => { options.DetailedErrors = true; });

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(); //��� ����

//�ѱ�ó��
builder.Services.Configure<WebEncoderOptions>(options =>
{
    options.TextEncoderSettings = new TextEncoderSettings(UnicodeRanges.All); // �ѱ��� ���ڵ��Ǵ� ���� �ذ�
});

Class_lib(builder.Services);

void Class_lib(object services)
{
    builder.Services.AddTransient<IApt_Lib, Apt_Lib>(); //����Ʈ ����
    builder.Services.AddTransient<IApt_Sub_Lib, Apt_Sub_Lib>(); // ����Ʈ ���� ��
    builder.Services.AddTransient<IAppeal_Bloom_Lib, Appeal_Bloom_Lib>();//�ο� �з�
    builder.Services.AddTransient<IAppeal_Lib, Appeal_Lib>(); //�ο�
    builder.Services.AddTransient<IsubAppeal_Lib, subAppeal_Lib>(); //�ο� ó�� ����
    builder.Services.AddTransient<IsubWorker_Lib, subWorker_Lib>(); //�ο� ó����System.InvalidOperationException: 'Cannot modify ServiceCollection after application is built.'

    builder.Services.AddTransient<IErp_AptPeople_Lib, Erp_AptPeople_Lib>(); //���ֹ� ����
    builder.Services.AddTransient<IPost_Lib, Post_Lib>(); //���� �μ� ����
    builder.Services.AddTransient<IDuty_Lib, Duty_Lib>(); //���� ��å ����
    builder.Services.AddTransient<ICar_Infor_Lib, Car_Infor_Lib>(); //�������� 
    builder.Services.AddTransient<ISido_Lib, Sido_Lib>(); // �ּ�
    builder.Services.AddTransient<ILogs_Lib, Logs_Lib>(); // �α� ���� �����
    builder.Services.AddTransient<IStaff_Lib, Staff_Lib>(); // ������ ����
    builder.Services.AddTransient<IStaff_Sub_Lib, Staff_Sub_Lib>(); //������ ���� ��
    builder.Services.AddTransient<IReferral_career_Lib, Referral_career_Lib>(); //������ ���� ��ġ����
    builder.Services.AddTransient<IStaff_staffSub_Lib, Staff_staffSub_Lib>(); // ���� �� ���� ����
    builder.Services.AddTransient<IStaff_Career_Lib, Staff_Career_Lib>(); //������ ���� ��ġ���� ���� ����
    builder.Services.AddTransient<IPresent_Lib, Present_Lib>(); // Ȩ������ �湮 ����
    builder.Services.AddTransient<IService_Worker_Lib, Service_Worker_Lib>(); // �۾��� ����
    builder.Services.AddTransient<IErp_Files_Lib, Erp_Files_Lib>(); //÷�� ���� ����
    builder.Services.AddTransient<ISw_Files_Lib, Sw_Files_Lib>(); //÷�� ���� ���� 
    builder.Services.AddTransient<IUpFile_Lib, UpFile_Lib>(); //÷������ ���� ����
    builder.Services.AddTransient<IDefect_Lib, Defect_Lib>(); // ���ڰ���
    builder.Services.AddTransient<IBloom_Lib, Bloom_Lib>(); // �ü��� �з� ����
    builder.Services.AddTransient<IFacility_Lib, Facility_Lib>(); // �ü��� ���� ����
    builder.Services.AddTransient<ICompany_Lib, Company_Lib>(); // ��ü �⺻ ���� ����
    builder.Services.AddTransient<ICompany_Sub_Lib, Company_Sub_Lib>(); // ��ü �� ���� ����
    builder.Services.AddTransient<ICompany_Apt_Career_Lib, Company_Apt_Career_Lib>(); // ��Ź���� ����
    builder.Services.AddTransient<ICompany_Join_Lib, Company_Join_Lib>(); // �������� �⺻ �� �� ���� ����
    builder.Services.AddTransient<IContract_Sort_Lib, Contract_Sort_Lib>(); // ��ü �з�
    builder.Services.AddTransient<ICompany_Apt_Career_Lib, Company_Apt_Career_Lib>(); // ���� ��� ���� 
    builder.Services.AddTransient<IApt_Reports_Lib, Apt_Reports_Lib>();//������ ����
    builder.Services.AddTransient<IReport_Bloom_Lib, Report_Bloom_Lib>();//���� �з� 
    builder.Services.AddTransient<IReport_Division_Lib, Report_Division_Lib>();//���� ���� 
    builder.Services.AddTransient<Ibs_apt_career, bs_apt_career>();//Ȩ������ ���Խ���
    builder.Services.AddTransient<IIn_AptPeople_Lib, In_AptPeople_Lib>(); // Ȩ������ ���� ���ֹ� ����
    builder.Services.AddTransient<ILabor_contract_Lib, Labor_contract_Lib>(); //��༭ ����
    builder.Services.AddTransient<Iwage_Lib, wege_Lib>(); //���� �ӱ�
    builder.Services.AddTransient<IWorks_Lib, Works_Lib>(); //�۾����� ����
    builder.Services.AddTransient<IWorksSub_Lib, WorksSub_Lib>(); //�۾����� ����
    builder.Services.AddTransient<IDecision_Lib, Decision_Lib>(); //��������
    builder.Services.AddTransient<IDbImageLib, DbImageLib>(); //���絵�� ����
    builder.Services.AddTransient<IDbImagesLib, DbImagesLib>();  // ���� ���� ����
    builder.Services.AddTransient<ICheck_List_Lib, Check_List_Lib>(); // �ü��� ���� ��� ����
    builder.Services.AddTransient<ICheck_Card_Lib, Check_Card_Lib>(); //�ü��� ����ǥ
    builder.Services.AddTransient<ICheck_Object_Lib, Check_Object_Lib>(); //�ü��� ���� ���
    builder.Services.AddTransient<ICheck_Cycle_Lib, Check_Cycle_Lib>(); // �ü��� ���� �ֱ�
    builder.Services.AddTransient<ICheck_Input_Lib, Check_Input_Lib>();// �ü��� ���� �Ϸ�
    builder.Services.AddTransient<ICheck_Items_Lib, Check_Items_Lib>(); //�ü��� ���� ����
    builder.Services.AddTransient<ICheck_Effect_Lib, Check_Effect_Lib>(); // �ü��� ���� ���
    builder.Services.AddTransient<IApproval_Lib, Approval_Lib>(); //����� ���� Ŭ����
    builder.Services.AddTransient<IDocument_Lib, Document_Lib>(); //�������� ���� Ŭ����
    builder.Services.AddTransient<IDocument_Sort_Lib, Document_Sort_Lib>(); //�����з����� ���� Ŭ����
    builder.Services.AddTransient<IDraft_Lib, Draft_Lib>(); //��ȹ��� ���� Ŭ����
    builder.Services.AddTransient<IDraftDetail_Lib, DraftDetail_Lib>(); //��ȹ��� �� ���� Ŭ����
    builder.Services.AddTransient<IDraftAttach_Lib, DraftAttach_Lib>(); //��ȹ��� �� �׸� ���� Ŭ����
    builder.Services.AddTransient<IProofReport_Lib, ProofReport_Lib>(); //�� ���� ����
    builder.Services.AddTransient<INotice_Lib, Notice_Lib>();//���/���� ����
    builder.Services.AddTransient<IStocks_Lib, Stocks_Lib>(); //������� ����
    builder.Services.AddTransient<IWhSock_Lib, WhSock_Lib>(); //�������� ���� ����
    builder.Services.AddTransient<IDisbursementSort_Lib, DisbursementSort_Lib>(); //������Ǽ� ����
    builder.Services.AddTransient<IDisbursement_Lib, Disbursement_Lib>();
    builder.Services.AddTransient<IAccount_Lib, Account_Lib>();//�������� ����
    builder.Services.AddTransient<IAccountSort_Lib, AccountSort_Lib>();
    builder.Services.AddTransient<IAccountDeals_Lib, AccountDeals_Lib>();
    builder.Services.AddTransient<IBankAccount_Lib, BankAccount_Lib>();
    builder.Services.AddTransient<IBankAccountDeals_Lib, BankAccountDeals_Lib>();
    builder.Services.AddTransient<IPrint_Images_Lib, Print_Images_Lib>();
    builder.Services.AddTransient<ICommunity_Lib, Community_Lib>(); //Ŀ�´�Ƽ ����
    builder.Services.AddTransient<ICommunityUsingKind_Lib, CommunityUsingKind_Lib>();//Ŀ�´�Ƽ ����
    builder.Services.AddTransient<ICommunityUsingTicket_Lib, CommunityUsingTicket_Lib>();//�̿�� ����
    builder.Services.AddTransient<IPlan_Lib, Plans_Lib>(); //������ȹ ���� ���� ����
    builder.Services.AddTransient<IPlan_Sort_Lib, Plan_Sort_Lib>(); //������ȹ ���� �з� ���� ����
    builder.Services.AddTransient<IPlan_Man_Lib, Plan_Man_Lib>(); //
    builder.Services.AddTransient<Isw_Note_Lib, sw_Note_Lib>();
    builder.Services.AddTransient<ISurisan_Comments, Surisan_Comments>();
    builder.Services.AddTransient<Isw_Note_Sort_Lib, sw_Note_Sort_Lib>();
    builder.Services.AddTransient<IReadView_Lib, ReadView_Lib>();
    builder.Services.AddTransient<Iwedew_Lib, wedew_Lib>(); //�Խ��� 
    builder.Services.AddTransient<IFiless_Lib, Filess_Lib>(); // �Խ��� ÷�� ����
    builder.Services.AddTransient<ISort_Lib, Sort_Lib>(); //�Խ��� �з�
    builder.Services.AddTransient<IWedew_Comments, Wedew_Comments>(); //���
    builder.Services.AddTransient<ICostDebit_Lib, CostDebit_Lib>();//������ ����
    builder.Services.AddTransient<IMonthlyUsage_Lib, MonthlyUsage_Lib>(); //��뷮 ����
    builder.Services.AddTransient<IUsageDetails_Lib, UsageDetails_Lib>(); //��뷮 ������
    builder.Services.AddTransient<IBike_Lib, Bike_Lib>(); //������ ����
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
