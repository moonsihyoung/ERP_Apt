using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using System.Linq;

namespace Erp_Apt_Lib.Accounting
{
    /// <summary>
    /// 지출결의서 종류 정보 메서드
    /// </summary>
    public class DisbursementSort_Lib : IDisbursementSort_Lib
    {
        private readonly IConfiguration _db;
        public DisbursementSort_Lib(IConfiguration configuration)
        {
            this._db = configuration;
        }

        /// <summary>
        /// 지출결의서 종류 입력
        /// </summary>
        /// <param name="ds"></param>
        /// <returns></returns>
        public async Task<int> Add(DisbursementSortEnity ds)
        {
            var sql = "Insert Into DisbursementSort (AptCode, DisbursementName, Details, PostIp, User_Code) Values (@AptCode, @DisbursementName, @Details, @PostIp, @User_Code);Select Cast(SCOPE_IDENTITY() As Int);";
            using var db = new SqlConnection(_db.GetConnectionString("sw_togather"));
            var intA = await db.QuerySingleOrDefaultAsync<int>(sql, ds);
            return intA;
        }

        /// <summary>
        /// 지출결의서 종류 수정
        /// </summary>
        /// <param name="ds"></param>
        public async Task Edit(DisbursementSortEnity ds)
        {
            using var db = new SqlConnection(_db.GetConnectionString("sw_togather"));
            await db.ExecuteAsync("Update DisbursementSort Set DisbursementName = @DisbursementName, Details = @Details, PostIp = @PostIp, User_Code = @User_Code Where Aid = @Aid", ds);
        }

        /// <summary>
        /// 공동주택별 지출결의서 종류 목록
        /// </summary>
        /// <param name="AptCode"></param>
        /// <returns></returns>
        public async Task<List<DisbursementSortEnity>> GetList(string AptCode)
        {
            using var db = new SqlConnection(_db.GetConnectionString("sw_togather"));
            var lst = await db.QueryAsync<DisbursementSortEnity>("Select Aid, AptCode, DisbursementName, Details, PostDate, PostIp, User_Code, del From DisbursementSort Where AptCode = @AptCode Order by Aid Desc", new { AptCode });
            return lst.ToList();
        }

        /// <summary>
        /// 공동주택별 지출결의서 종류 목록 수
        /// </summary>
        /// <param name="AptCode"></param>
        /// <returns></returns>
        public async Task<int> GetListCount(string AptCode)
        {
            using var db = new SqlConnection(_db.GetConnectionString("sw_togather"));
            return await db.QuerySingleOrDefaultAsync<int>("Select Count(*) From DisbursementSort Where AptCode = @AptCode", new { AptCode });
        }

        /// <summary>
        /// 공동주택별 지출결의서 종류 목록
        /// </summary>
        /// <param name="AptCode"></param>
        /// <returns></returns>
        public async Task<List<DisbursementSortEnity>> GetListUsing(string AptCode)
        {
            using var db = new SqlConnection(_db.GetConnectionString("sw_togather"));
            var lst = await db.QueryAsync<DisbursementSortEnity>("Select Aid, AptCode, DisbursementName, Details, PostDate, PostIp, User_Code, del From DisbursementSort Where AptCode = @AptCode And del = 'A' Order by Aid Desc", new { AptCode });
            return lst.ToList();
        }

        /// <summary>
        /// 공동주택별 지출결의서 종류 목록 수
        /// </summary>
        /// <param name="AptCode"></param>
        /// <returns></returns>
        public async Task<int> GetListCountUsing(string AptCode)
        {
            using var db = new SqlConnection(_db.GetConnectionString("sw_togather"));
            return await db.QuerySingleOrDefaultAsync<int>("Select Count(*) From DisbursementSort Where AptCode = @AptCode And del = 'A'", new { AptCode });
        }


        /// <summary>
        /// 공동주택별 지출결의서 종류 목록 수
        /// </summary>
        /// <param name="AptCode"></param>
        /// <returns></returns>
        public async Task<string> dbsortName(int Aid)
        {
            using var db = new SqlConnection(_db.GetConnectionString("sw_togather"));
            return await db.QuerySingleOrDefaultAsync<string>("Select DisbursementName From DisbursementSort Where Aid = @Aid", new { Aid });
        }

        /// <summary>
        /// 공동주택별 지출결의서 종류 목록 수
        /// </summary>
        public string dbsort_Name(string Aid)
        {
            using var db = new SqlConnection(_db.GetConnectionString("sw_togather"));
            return db.QuerySingleOrDefault<string>("Select DisbursementName From DisbursementSort Where Aid = @Aid", new { Aid });
        }

        /// <summary>
        /// 공동주택별 지출결의서 종류 목록
        /// </summary>
        /// <param name="AptCode"></param>
        /// <returns></returns>
        public async Task<List<DisbursementSortEnity>> GetList_Name(string AptCode)
        {
            using var db = new SqlConnection(_db.GetConnectionString("sw_togather"));
            var lst = await db.QueryAsync<DisbursementSortEnity>("Select Aid, DisbursementName From DisbursementSort Where AptCode = @AptCode And del = 'A' Order by Aid Asc", new { AptCode });
            return lst.ToList();
        }

        /// <summary>
        /// 지출결의서 종류 상세정보
        /// </summary>
        /// <param name="Aid"></param>
        /// <returns></returns>
        public async Task<DisbursementSortEnity> Details(int Aid)
        {
            using var db = new SqlConnection(_db.GetConnectionString("sw_togather"));
            return await db.QuerySingleOrDefaultAsync<DisbursementSortEnity>("Select Aid, AptCode, DisbursementName, Details, PostDate, PostIp, User_Code, del From DisbursementSort Where Aid = @Aid", new { Aid });
        }

        /// <summary>
        /// 삭제
        /// </summary>
        /// <param name="Aid"></param>
        public async Task Remove(int Aid)
        {
            using var db = new SqlConnection(_db.GetConnectionString("sw_togather"));
            string d = await db.QuerySingleOrDefaultAsync<string>("Select del From DisbursementSort Where Aid = @Aid", new { Aid });

            if (d == "A")
            {
                await db.ExecuteAsync("Update DisbursementSort Set del = 'B' Where Aid = @Aid", new { Aid });
            }
            else
            {
                await db.ExecuteAsync("Update DisbursementSort Set del = 'B' Where Aid = @Aid", new { Aid });
            }
        }
    }

    /// <summary>
    /// 지출결의서 정보 클래스
    /// </summary>
    public class Disbursement_Lib : IDisbursement_Lib
    {
        private readonly IConfiguration _db;
        public Disbursement_Lib(IConfiguration configuration)
        {
            this._db = configuration;
        }

        /// <summary>
        /// 지출결의서 입력하기
        /// </summary>
        /// <param name="de"></param>
        /// <returns></returns>
        public async Task<int> Add(DisbursementEntity de)
        {
            var sql = "Insert Into Disbursement (AptCode, DisbursementName, DraftDate, InputDate, InputYear, InputMonth, Details, PostIp, User_Code) Values (@AptCode, @DisbursementName, @DraftDate, @InputDate, @InputYear, @InputMonth, @Details, @PostIp, @User_Code);Select Cast(SCOPE_IDENTITY() As Int);";
            using var db = new SqlConnection(_db.GetConnectionString("sw_togather"));
            int intA = await db.QuerySingleOrDefaultAsync<int>(sql, de);
            return intA;
        }

        /// <summary>
        /// 지출결의서 수정
        /// </summary>
        /// <param name="de"></param>
        public async Task Edit(DisbursementEntity de)
        {
            using var db = new SqlConnection(_db.GetConnectionString("sw_togather"));
            await db.ExecuteAsync("Update Disbursement Set DisbursementName = @DisbursementName, DraftDate = @DraftDate, InputDate = @InputDate, InputYear = @InputYear, InputMonth = @InputMonth, Details = @Details, PostIp = @PostIp Where Aid = @Aid", de);
        }

        /// <summary>
        /// 결재 완료
        /// </summary>
        /// <param name="Aid"></param>
        public async Task ApprovalEdit(int Aid)
        {
            using var db = new SqlConnection(_db.GetConnectionString("sw_togather"));
            await db.ExecuteAsync("Update Disbursement Set Approval = 'B' Where Aid = @Aid", new { Aid });
        }

        /// <summary>
        /// 결재여부 확인
        /// </summary>
        /// <param name="Aid"></param>
        /// <returns></returns>
        public async Task<string> Approval(int Aid)
        {
            using var db = new SqlConnection(_db.GetConnectionString("sw_togather"));
            return await db.QuerySingleOrDefaultAsync<string>("Select Approval From Disbursement Where Aid = @Aid", new { Aid });
        }

        /// <summary>
        /// 지출결의서 목록(공동주택 단위별)
        /// </summary>
        public async Task<List<DisbursementEntity>> GetList(int Page, string AptCode)
        {
            using var db = new SqlConnection(_db.GetConnectionString("sw_togather"));
            var lst = await db.QueryAsync<DisbursementEntity>("Select Top 15 a.Aid, a.AptCode, b.DisbursementName, DraftDate, InputDate, InputYear, InputMonth, InputSum, a.Details, a.PostDate, a.PostIp, a.User_Code, Approval, c.User_Name From Disbursement as a Join DisbursementSort as b on a.DisbursementName = b.Aid Join Staff as c On a.User_Code = c.User_ID Where a.Aid Not In(Select Top (15 * @Page) a.Aid From Disbursement as a Join DisbursementSort as b on a.DisbursementName = b.Aid Where a.AptCode = @AptCode And b.AptCode = @AptCode And a.del = 'A' Order By a.Aid Desc) And a.AptCode = @AptCode And b.AptCode = @AptCode And a.del = 'A' Order by a.Aid Desc", new { Page, AptCode });
            return lst.ToList();
        }

        /// <summary>
        /// 지출결의서 목록 수
        /// </summary>
        public async Task<int> GetListCount(string AptCode)
        {
            using var db = new SqlConnection(_db.GetConnectionString("sw_togather"));
            return await db.QuerySingleOrDefaultAsync<int>("Select Count(*) From Disbursement Where AptCode = @AptCode And del = 'A'", new { AptCode });
        }

        /// <summary>
        /// 지출결의서 상세
        /// </summary>
        /// <param name="Aid"></param>
        /// <returns></returns>
        public async Task<DisbursementEntity> Details(int Aid)
        {
            using var db = new SqlConnection(_db.GetConnectionString("sw_togather"));
            return await db.QuerySingleOrDefaultAsync<DisbursementEntity>("Select Aid, AptCode, DisbursementName, DraftDate, InputDate, InputYear, InputMonth, InputSum, Details, PostDate, PostIp, User_Code, Approval From Disbursement Where Aid = @Aid", new { Aid });
        }

        /// <summary>
        /// 삭제
        /// </summary>
        /// <param name="Aid"></param>
        public async Task Remove(int Aid)
        {
            using var db = new SqlConnection(_db.GetConnectionString("sw_togather"));
            await db.ExecuteAsync("Update Disbursement Set del = 'B' Where Aid = @Aid", new { Aid });
        }

        /// <summary>
        /// 지출결의서 찾기
        /// </summary>
        /// <param name="Feild"></param>
        /// <param name="Query"></param>
        /// <returns></returns>
        public async Task<List<DisbursementEntity>> Search(string Feild, string Query)
        {
            using var db = new SqlConnection(_db.GetConnectionString("sw_togather"));
            var lst = await db.QueryAsync<DisbursementEntity>("Select Aid, AptCode, DisbursementName, DraftDate, InputDate, InputYear, InputMonth, InputSum, Details, PostDate, PostIp, User_Code, Approval From Disbursement Where " + Feild + " = " + Query + " Order by Aid Desc", new { Feild, Query });
            return lst.ToList();
        }

        /// <summary>
        /// 지출결의서 년월 찾기
        /// </summary>
        /// <param name="InputYear"></param>
        /// <param name="InputMonth"></param>
        /// <returns></returns>
        public async Task<List<DisbursementEntity>> SearchDate(string InputYear, string InputMonth)
        {
            using var db = new SqlConnection(_db.GetConnectionString("sw_togather"));
            var lst = await db.QueryAsync<DisbursementEntity>("Select Aid, AptCode, DisbursementName, DraftDate, InputDate, InputYear, InputMonth, InputSum, Details, PostDate, PostIp, User_Code, Approval From Disbursement Where InputYear = @InputYear And InputMonth = @InputMonth Order by Aid Desc", new { InputYear, InputMonth });
                return lst.ToList();
        }

        /// <summary>
        /// 합계 금액 입력
        /// </summary>
        /// <param name="DisbursementCode"></param>
        /// <param name="InputSum"></param>
        public async Task Sum_InputSum(string DisbursementCode, double InputSum)
        {
            using var db = new SqlConnection(_db.GetConnectionString("sw_togather"));
            await db.ExecuteAsync("Update Disbursement Set InputSum = @InputSum Where Aid = @DisbursementCode", new { DisbursementCode, InputSum });
        }

        /// <summary>
        /// 앞 지출결의서 정보
        /// </summary>
        /// <param name="AptCode"></param>
        /// <param name="Num"></param>
        /// <returns></returns>
        public async Task<string> Ago(string AptCode, string Aid)
        {
            using var db = new SqlConnection(_db.GetConnectionString("sw_togather"));
            return await db.QuerySingleOrDefaultAsync<string>("Select ISNULL(Aid, 0) From Disbursement Where AptCode = @AptCode And del = 'A' and Aid = (Select max(Aid) From Disbursement Where AptCode = @AptCode And del = 'A' and Aid < @Aid)", new { AptCode, Aid });
        }

        /// <summary>
        /// 앞 지출결의서  존재여부
        /// </summary>
        /// <param name="AptCode"></param>
        /// <param name="Num"></param>
        /// <returns></returns>
        public async Task<int> AgoBe(string AptCode, string Aid)
        {
            using var db = new SqlConnection(_db.GetConnectionString("sw_togather"));
            return await db.QuerySingleOrDefaultAsync<int>("Select Count(*) From Disbursement Where AptCode = @AptCode And del = 'A' and Aid = (Select max(Aid) From Disbursement Where AptCode = @AptCode And del = 'A' and Aid < @Aid)", new { AptCode, Aid });
        }

        /// <summary>
        /// 뒤 지출결의서
        /// </summary>
        public async Task<string> Next(string AptCode, string Aid)
        {
            using var db = new SqlConnection(_db.GetConnectionString("sw_togather"));
            return await db.QuerySingleOrDefaultAsync<string>("Select ISNULL(Aid, 0) From Disbursement Where AptCode = @AptCode And del = 'A' and Aid = (Select Min(Aid) From Disbursement Where AptCode =@AptCode And del = 'A' and Aid > @Aid)", new { AptCode, Aid });
        }

        /// <summary>
        /// 뒤 지출결의서 존재 여부
        /// </summary>
        /// <param name="AptCode"></param>
        /// <param name="Num"></param>
        /// <returns></returns>
        public async Task<int> NextBe(string AptCode, string Aid)
        {
            using var db = new SqlConnection(_db.GetConnectionString("sw_togather"));
            return await db.QuerySingleOrDefaultAsync<int>("Select Count(*) From Disbursement Where AptCode = @AptCode And del = 'A' and Aid = (Select Min(Aid) From Disbursement Where AptCode =@AptCode And del = 'A' and Aid > @Aid)", new { AptCode, Aid });
        }

        /// <summary>
        /// 지출결의서 종류 중 최근 정보 번호 구하기
        /// </summary>
        public async Task<int> Top_Code(string DisbursementName, string AptCode)
        {
            using var db = new SqlConnection(_db.GetConnectionString("sw_togather"));
            return await db.QuerySingleOrDefaultAsync<int>("Select Top 1 Aid From Disbursement Where DisbursementName = @DisbursementName And AptCode = @AptCode And del = 'A' Order By Aid Desc", new { DisbursementName, AptCode });
        }
    }

    /// <summary>
    /// 계정과목 정보 클래스
    /// </summary>
    public class Account_Lib : IAccount_Lib
    {
        private readonly IConfiguration _db;
        public Account_Lib(IConfiguration configuration)
        {
            this._db = configuration;
        }

        /// <summary>
        /// 계정과목 입력
        /// </summary>
        /// <param name="at"></param>
        /// <returns></returns>
        public async Task<int> Add(AccountEntity at)
        {
            var sql = "Insert Into Account (AccountType, SortA, SortA_Code, SortB, SortB_Code, AccountName, Details, PostIp, User_Code) Values (@AccountType, @SortA, @SortA_Code, @SortB, @SortB_Code, @AccountName, @Details, @PostIp, @User_Code);Select Cast(SCOPE_IDENTITY() As Int);";
            using var db = new SqlConnection(_db.GetConnectionString("sw_togather"));
            int intA = await db.QuerySingleOrDefaultAsync<int>(sql, at);
            return intA;
        }

        /// <summary>
        /// 수정
        /// </summary>
        /// <param name="at"></param>
        public async Task Edit(AccountEntity at)
        {
            using var db = new SqlConnection(_db.GetConnectionString("sw_togather"));
            await db.ExecuteAsync("Update Account Set SortA = @SortA, SortA_Code = @SortA_Code, SortB = @SortB, SortB_Code = @SortB_Code, Details = @Details, AccountName = @AccountName, PostIp = @PostIp, User_Code = @User_Code Where Aid = @Aid", at);
        }

        /// <summary>
        /// 계정과목 목록
        /// </summary>
        /// <returns></returns>
        public async Task<List<AccountEntity>> GetList(int Page)
        {
            using var db = new SqlConnection(_db.GetConnectionString("sw_togather"));
            var lst = await db.QueryAsync<AccountEntity>("Select Top 15 Aid, AccountType, SortA, SortA_Code, SortB, SortB_Code, AccountName, Details, PostDate, PostIp, User_Code, del From Account Where Aid Not In (Select Top (15 * @Page) Aid From Account Order by Aid Desc) Order by Aid desc", new { Page });
            return lst.ToList();
        }

        /// <summary>
        /// 계정과목 목록 수
        /// </summary>
        /// <returns></returns>
        public async Task<int> GetListCount()
        {
            using var db = new SqlConnection(_db.GetConnectionString("sw_togather"));
            return await db.QuerySingleOrDefaultAsync<int>("Select Count(*) From Account");
        }

        /// <summary>
        /// 계정과목 목록
        /// </summary>
        /// <returns></returns>
        public async Task<List<AccountEntity>> GetListUsing(int Page)
        {
            using var db = new SqlConnection(_db.GetConnectionString("sw_togather"));
            var lst = await db.QueryAsync<AccountEntity>("Select Top 15 Aid, AccountType, SortA, SortA_Code, SortB, SortB_Code, AccountName, Details, PostDate, PostIp, User_Code, del From Account Where Aid Not In (Select Top (15 * @Page) Aid From Account Order by Aid Desc) And del = 'A' Order by Aid desc", new { Page });
            return lst.ToList();
        }

        /// <summary>
        /// 계정과목 목록 수
        /// </summary>
        /// <returns></returns>
        public async Task<int> GetListCountUsing()
        {
            using var db = new SqlConnection(_db.GetConnectionString("sw_togather"));
            return await db.QuerySingleOrDefaultAsync<int>("Select Count(*) From Account And del = 'A'");
        }

        /// <summary>
        /// 대분류로 계정과목 검색하기
        /// </summary>
        /// <param name="SortA_Code"></param>
        /// <returns></returns>
        public async Task<List<AccountEntity>> GetList_SortA(int SortA_Code)
        {
            using var db = new SqlConnection(_db.GetConnectionString("sw_togather"));
            var lst = await db.QueryAsync<AccountEntity>("Select Aid, AccountType, SortA, SortA_Code, SortB, SortB_Code, AccountName, Details, PostDate, PostIp, User_Code, del From Account Where SortA_Code = @SortA_Code And del = 'A' Order by Aid Desc", new { SortA_Code });
            return lst.ToList();
        }

        /// <summary>
        /// 계정과목 대분류 검색된 목록 수
        /// </summary>
        /// <returns></returns>
        public async Task<int> GetList_SortA_Count(int SortA_Code)
        {
            using var db = new SqlConnection(_db.GetConnectionString("sw_togather"));
            return await db.QuerySingleOrDefaultAsync<int>("Select Count(*) From Account Where SortA_Code = @SortA_Code And del = 'A'", new { SortA_Code });
        }

        /// <summary>
        /// 중분류로 검색하기
        /// </summary>
        /// <param name="SortA_Code"></param>
        /// <returns></returns>
        public async Task<List<AccountEntity>> GetList_SortB(int SortB_Code)
        {
            using var db = new SqlConnection(_db.GetConnectionString("sw_togather"));
            var lst = await db.QueryAsync<AccountEntity>("Select Aid, AccountName, Details From Account Where SortB_Code = @SortB_Code And del = 'A' Order by Aid Asc", new { SortB_Code });
            return lst.ToList();
        }

        /// <summary>
        ///  중분류로 검색된 목록 수
        /// </summary>
        /// <returns></returns>
        public async Task<int> GetList_SortB_Count(int SortB_Code)
        {
            using var db = new SqlConnection(_db.GetConnectionString("sw_togather"));
            return await db.QuerySingleOrDefaultAsync<int>("Select Count(*) From Account Where SortB_Code = @SortB_Code And del = 'A'", new { SortB_Code });
        }

        /// <summary>
        /// 대분류 그리고 중분류로 계정과목 목록
        /// </summary>
        /// <param name="SortA_Code"></param>
        /// <param name="SortB_Code"></param>
        /// <returns></returns>
        public async Task<List<AccountEntity>> GetList_Sort_AB(int SortA_Code, int SortB_Code)
        {
            using var db = new SqlConnection(_db.GetConnectionString("sw_togather"));
            var lst = await db.QueryAsync<AccountEntity>("Select Aid, AccountName, Details From Account Where SortA_Code = @SortA_Code And SortB_Code = @SortB_Code And del = 'A' Order by Aid Asc", new { SortA_Code, SortB_Code });
            return lst.ToList();
        }

        /// <summary>
        /// 계정과목 상세보기
        /// </summary>
        /// <param name="Aid"></param>
        /// <returns></returns>
        public async Task<AccountEntity> Details(int Aid)
        {
            using var db = new SqlConnection(_db.GetConnectionString("sw_togather"));
            return await db.QuerySingleOrDefaultAsync<AccountEntity>("Select Aid, AccountType, SortA, SortA_Code, SortB, SortB_Code, AccountName, Details, PostDate, PostIp, User_Code, del From Account Where Aid = @Aid", new { Aid });
        }

        /// <summary>
        /// 계정과목 명 불러오기
        /// </summary>
        /// <param name="Aid"></param>
        /// <returns></returns>
        public async Task<string> AccountName(int Aid)
        {
            using var db = new SqlConnection(_db.GetConnectionString("sw_togather"));
            return await db.QuerySingleOrDefaultAsync<string>("Select AccountName From Account Where Aid = @Aid", new { Aid });
        }

        /// <summary>
        /// 계정과목 명 불러오기
        /// </summary>
        public string Account_Name(string Aid)
        {
            using var db = new SqlConnection(_db.GetConnectionString("sw_togather"));
            return db.QuerySingleOrDefault<string>("Select AccountName From Account Where Aid = @Aid", new { Aid });
        }

        /// <summary>
        /// 계정과목 삭제
        /// </summary>
        /// <param name="Aid"></param>
        public async Task Remove(int Aid)
        {
            using var db = new SqlConnection(_db.GetConnectionString("sw_togather"));
            string strDel = await db.QuerySingleOrDefaultAsync<string>("Select del From Account Where Aid = @Aid", new { Aid });
            if (strDel == "A")
            {
                await db.ExecuteAsync("Update Account Set del = 'B' Where Aid = @Aid", new { Aid });
            }
            else
            {
                await db.ExecuteAsync("Update Account Set del = 'A' Where Aid = @Aid", new { Aid });
            }
        }
    }

    /// <summary>
    /// 계정분류 정보 클래스
    /// </summary>
    public class AccountSort_Lib : IAccountSort_Lib
    {
        private readonly IConfiguration _db;
        public AccountSort_Lib(IConfiguration configuration)
        {
            this._db = configuration;
        }

        /// <summary>
        /// 계정 분류 입력
        /// </summary>
        /// <param name="ase"></param>
        /// <returns></returns>
        public async Task<int> Add(AccountSortEntity ase)
        {
            var sql = "Insert Into AccountSort (Division, SortName, UpSort, Details, PostIp, User_Code) Values (@Division, @SortName, @UpSort, @Details, @PostIp, @User_Code);Select Cast(SCOPE_IDENTITY() As Int);";
            using var db = new SqlConnection(_db.GetConnectionString("sw_togather"));
            int a = await db.QuerySingleOrDefaultAsync<int>(sql, ase);
            return a;
        }

        /// <summary>
        /// 계정 분류 수정
        /// </summary>
        /// <param name="ase"></param>
        public async Task Edit(AccountSortEntity ase)
        {
            using var db = new SqlConnection(_db.GetConnectionString("sw_togather"));
            await db.ExecuteAsync("Update AccountSort Set UpSort = @UpSort, SortName = @SortName, PostIp = @PostIp, Details = @Details, User_Code = @User_Code Where Aid = @Aid", ase);
        }

        /// <summary>
        /// 계정 분류 전체 목록
        /// </summary>
        /// <returns></returns>
        public async Task<List<AccountSortEntity>> GetList()
        {
            using var db = new SqlConnection(_db.GetConnectionString("sw_togather"));
            var lst = await db.QueryAsync<AccountSortEntity>("Select Aid, Division, SortName, UpSort, Details, PostDate, PostIp, User_Code, del From AccountSort Order by Division Asc, SortName Asc, Aid Asc");
            return lst.ToList();
        }

        /// <summary>
        /// 계정분류 구분별 목록
        /// </summary>
        public async Task<List<AccountSortEntity>> GetList_Division(string Division)
        {
            using var db = new SqlConnection(_db.GetConnectionString("sw_togather"));
            var lst = await db.QueryAsync<AccountSortEntity>("Select Aid, Division, SortName, UpSort, Details, PostDate, PostIp, User_Code, del From AccountSort Where Division = @Division Order by Aid Asc", new { Division });
            return lst.ToList();
        }

        /// <summary>
        /// 대분류로 중분류 목록 구하기
        /// </summary>
        /// <param name="Division"></param>
        /// <returns></returns>
        public async Task<List<AccountSortEntity>> GetList_Sort(string SortA)
        {
            using var db = new SqlConnection(_db.GetConnectionString("sw_togather"));
            var lst = await db.QueryAsync<AccountSortEntity>("Select Aid, Division, SortName, UpSort, Details, PostDate, PostIp, User_Code, del From AccountSort Where UpSort = @SortA Order by Aid Asc", new { SortA });
            return lst.ToList();
        }

        /// <summary>
        /// 계정분류 상세
        /// </summary>
        /// <param name="Aid"></param>
        /// <returns></returns>
        public async Task<AccountSortEntity> Details(int Aid)
        {
            using var db = new SqlConnection(_db.GetConnectionString("sw_togather"));
            return await db.QuerySingleOrDefaultAsync<AccountSortEntity>("Select Aid, Division, SortName, UpSort, Details, PostDate, PostIp, User_Code, del From AccountSort Where Aid = @Aid Order by Aid Asc", new { Aid });
        }

        /// <summary>
        /// 분류명 불러오기
        /// </summary>
        /// <param name="Aid"></param>
        /// <returns></returns>
        public async Task<string> Sort_Name(string Aid)
        {
            using var db = new SqlConnection(_db.GetConnectionString("sw_togather"));
            return await db.QuerySingleOrDefaultAsync<string>("Select SortName From AccountSort Where Aid = @Aid", new { Aid });
        }

        /// <summary>
        /// 분류명 불러오기
        /// </summary>
        public string SortName(string Aid)
        {
            using var db = new SqlConnection(_db.GetConnectionString("sw_togather"));
            return db.QuerySingleOrDefault<string>("Select SortName From AccountSort Where Aid = @Aid", new { Aid });
        }

        /// <summary>
        /// 분류 삭제
        /// </summary>
        /// <param name="Aid"></param>
        public async Task Remove(int Aid)
        {
            using var db = new SqlConnection(_db.GetConnectionString("sw_togather"));
            string strDel = await db.QuerySingleOrDefaultAsync<string>("Select Del From AccountSort Where Aid = @Aid", new { Aid });

            if (strDel == "A")
            {
                await db.ExecuteAsync("Update AccountSort Set del = 'B' Where Aid = @Aid", new { Aid });
            }
            else
            {
                await db.ExecuteAsync("Update AccountSort Set del = 'A' Where Aid = @Aid", new { Aid });
            }
            
            
            //var sql1 = "Select Count(*) From Account Where SortA_Code = @SortA_Code And del = 'A'";
            //var sql2 = "Select Count(*) From Account Where SortB_Code = @SortB_Code And del = 'A'";

            //using var db = new SqlConnection(_db.GetConnectionString("sw_togather"));
            //int a = await db.QuerySingleOrDefaultAsync<int>(sql1, Aid);
            //int b = await db.QuerySingleOrDefaultAsync<int>(sql2, Aid); ;

            //if (a < 1 || b < 1)
            //{
            //    await db.ExecuteAsync("Delete AccountSort Where Aid = @Aid", new { Aid });
            //}
        }
    }

    /// <summary>
    /// 지출내역 상세 클래스
    /// </summary>
    public class AccountDeals_Lib : IAccountDeals_Lib
    {
        private readonly IConfiguration _db;
        public AccountDeals_Lib(IConfiguration configuration)
        {
            this._db = configuration;
        }

        /// <summary>
        /// 지출내역 상세 입력
        /// </summary>
        /// <param name="ade"></param>
        /// <returns></returns>
        public async Task<int> Add(AccountDealsEntity ade)
        {
            var sql = "Insert Into AccountDeals (AptCode, AccountSortCodeA, AccountSortCodeB, AccountCode, SubstitutionAccountCode, DisbursementCode, BankAccountCode, CompanyCode, ProvidePlace, InputSum, ProvideWay, ExecutionDate, InOutDivision, InputBankAccountCode, Details, PostIp, User_Code) Values (@AptCode, @AccountSortCodeA, @AccountSortCodeB, @AccountCode, @SubstitutionAccountCode, @DisbursementCode, @BankAccountCode, @CompanyCode, @ProvidePlace, @InputSum, @ProvideWay, @ExecutionDate, @InOutDivision, @InputBankAccountCode, @Details, @PostIp, @User_Code);Select Cast(SCOPE_IDENTITY() As Int);";
            using var db = new SqlConnection(_db.GetConnectionString("sw_togather"));
            int aa = await db.QuerySingleOrDefaultAsync<int>(sql, ade);
            return aa;
        }

        /// <summary>
        /// 지출내역 상세 수정
        ///  </summary>
        /// <param name="ade"></param>
        /// <returns></returns>
        public async Task Edit(AccountDealsEntity ade)
        {
            var sql = "Update AccountDeals Set AccountSortCodeA = @AccountSortCodeA, AccountSortCodeB = @AccountSortCodeB, AccountCode = @AccountCode, SubstitutionAccountCode = @SubstitutionAccountCode, BankAccountCode = @BankAccountCode, CompanyCode = @CompanyCode, ProvidePlace = @ProvidePlace, InputSum = @InputSum, ProvideWay = @ProvideWay, ExecutionDate = @ExecutionDate, InOutDivision = @InOutDivision, InputBankAccountCode = @InputBankAccountCode, Details = @Details, PostIp = @PostIp, PostDate = @PostDate, User_Code = @User_Code Where Aid = @Aid";
            using var db = new SqlConnection(_db.GetConnectionString("sw_togather"));
            await db.ExecuteAsync(sql, ade);
        }

        /// <summary>
        /// 지출결의서별 지출내역 상세정보
        /// </summary>
        /// <param name="Aid"></param>
        /// <param name="AptCode"></param>
        /// <returns></returns>
        public async Task<AccountDealsEntity> DealsDetails(int Aid, string AptCode)
        {
            using var db = new SqlConnection(_db.GetConnectionString("sw_togather"));
            return await db.QuerySingleOrDefaultAsync<AccountDealsEntity>("Select Aid, AptCode, AccountSortCodeA, AccountSortCodeB, AccountCode, SubstitutionAccountCode, DisbursementCode, BankAccountCode, CompanyCode, ProvidePlace, InputSum, ProvideWay, ExecutionDate, InOutDivision, InputBankAccountCode, Details, PostDate, PostIp, User_Code, Files_Count From AccountDeals Where Aid = @Aid And AptCode = @AptCode", new { Aid, AptCode });
        }


        /// <summary>
        /// 지출결의서별 지출내역
        /// </summary>
        /// <param name="DisbursementCode"></param>
        /// <param name="AptCode"></param>
        /// <returns></returns>
        public async Task<List<AccountDealsEntity>> GetList_Apt_DBMC(string DisbursementCode, string AptCode)
        {
            using var db = new SqlConnection(_db.GetConnectionString("sw_togather"));
            var lst = await db.QueryAsync<AccountDealsEntity>("Select Aid, AptCode, AccountSortCodeA, AccountSortCodeB, AccountCode, SubstitutionAccountCode, DisbursementCode, BankAccountCode, CompanyCode, ProvidePlace, InputSum, ProvideWay, ExecutionDate, InOutDivision, InputBankAccountCode, Details, PostDate, PostIp, User_Code, Files_Count From AccountDeals Where DisbursementCode = @DisbursementCode And AptCode = @AptCode", new { DisbursementCode, AptCode });
            return lst.ToList();
        }


        /// <summary>
        /// 입력된 수
        /// </summary>
        public async Task<int> GetList_Apt_DBMC_Count(string DisbursementCode, string AptCode)
        {
            using var db = new SqlConnection(_db.GetConnectionString("sw_togather"));
            return await db.QuerySingleOrDefaultAsync<int>("Select Count(*) From AccountDeals Where DisbursementCode = @DisbursementCode And AptCode = @AptCode", new { DisbursementCode, AptCode });
        }

        /// <summary>
        /// 지출결의서 별 총액
        /// </summary>
        public async Task<double> Sum_Apt_DBMC(string DisbursementCode, string AptCode)
        {
            using var db = new SqlConnection(_db.GetConnectionString("sw_togather"));
            return await db.QuerySingleOrDefaultAsync<double>("Select ISNULL(Sum(InputSum), 0) From AccountDeals Where DisbursementCode = @DisbursementCode And AptCode = @AptCode", new { DisbursementCode, AptCode });
        }

        /// <summary>
        /// 지출결의서별 지급방법별 지출내역(현금 지출)
        /// </summary>
        public async Task<List<AccountDealsEntity>> GetList_Apt_DBMC_Provide_A(string DisbursementCode, string ProvideWay, string AptCode)
        {
            using var db = new SqlConnection(_db.GetConnectionString("sw_togather"));
            var lst = await db.QueryAsync<AccountDealsEntity>("Select Aid, AptCode, AccountSortCodeA, AccountSortCodeB, AccountCode, SubstitutionAccountCode, DisbursementCode, BankAccountCode, CompanyCode, ProvidePlace, InputSum, ProvideWay, ExecutionDate, InOutDivision, InputBankAccountCode, Details, PostDate, PostIp, User_Code, Files_Count From AccountDeals Where DisbursementCode = @DisbursementCode And ProvideWay != @ProvideWay And AptCode = @AptCode", new { DisbursementCode, ProvideWay, AptCode });
            return lst.ToList();
        }

        /// <summary>
        /// 지출결의서별 지급방법별 지출내역(현금 지출) 수
        /// </summary>
        /// <param name="DisbursementCode"></param>
        /// <param name="AptCode"></param>
        /// <returns></returns>
        public async Task<int> GetList_Apt_DBMC_Provide_A_Count(string DisbursementCode, string ProvideWay, string AptCode)
        {
            using var db = new SqlConnection(_db.GetConnectionString("sw_togather"));
            return await db.QuerySingleOrDefaultAsync<int>("Select Count(*) From AccountDeals Where DisbursementCode = @DisbursementCode And ProvideWay != @ProvideWay And AptCode = @AptCode", new { DisbursementCode, ProvideWay, AptCode });
        }

        /// <summary>
        /// 지출결의서별 지급방법별 지출내역(자동이체)
        /// </summary>
        public async Task<List<AccountDealsEntity>> GetList_Apt_DBMC_Provide_B(string DisbursementCode, string ProvideWay, string AptCode)
        {
            using var db = new SqlConnection(_db.GetConnectionString("sw_togather"));
            var lst = await db.QueryAsync<AccountDealsEntity>("Select Aid, AptCode, AccountSortCodeA, AccountSortCodeB, AccountCode, SubstitutionAccountCode, DisbursementCode, BankAccountCode, CompanyCode, ProvidePlace, InputSum, ProvideWay, ExecutionDate, InOutDivision, InputBankAccountCode, Details, PostDate, PostIp, User_Code, Files_Count From AccountDeals Where DisbursementCode = @DisbursementCode And ProvideWay = @ProvideWay And AptCode = @AptCode", new { DisbursementCode, ProvideWay, AptCode });
            return lst.ToList();
        }

        /// <summary>
        /// 지출결의서별 지급방법별 지출내역(현금 지출) 수
        /// </summary>
        public async Task<int> GetList_Apt_DBMC_Provide_B_Count(string DisbursementCode, string ProvideWay, string AptCode)
        {
            using var db = new SqlConnection(_db.GetConnectionString("sw_togather"));
            return await db.QuerySingleOrDefaultAsync<int>("Select Count(*) From AccountDeals Where DisbursementCode = @DisbursementCode And ProvideWay = @ProvideWay And AptCode = @AptCode", new { DisbursementCode, ProvideWay, AptCode });
        }

        /// <summary>
        /// 지출결의서별 지급방법별 지출내역 총액A(자동이체 제외하고 모두)
        /// </summary>
        public async Task<double> Sum_Apt_DBMC_Provide_A(string DisbursementCode, string ProvideWay, string AptCode)
        {
            using var db = new SqlConnection(_db.GetConnectionString("sw_togather"));
            return await db.QuerySingleOrDefaultAsync<long>("Select ISNULL(Sum(InputSum), 0) From AccountDeals Where DisbursementCode = @DisbursementCode And ProvideWay != @ProvideWay And AptCode = @AptCode", new { DisbursementCode, ProvideWay, AptCode });
        }



        /// <summary>
        /// 지출결의서별 지급방법별 지출내역 총액B(자동이체 만)
        /// </summary>
        /// <param name="DisbursementCode"></param>
        /// <param name="AptCode"></param>
        /// <returns></returns>
        public async Task<double> Sum_Apt_DBMC_Provide_B(string DisbursementCode, string ProvideWay, string AptCode)
        {
            using var db = new SqlConnection(_db.GetConnectionString("sw_togather"));
            return await db.QuerySingleOrDefaultAsync<long>("Select ISNULL(Sum(InputSum), 0) From AccountDeals Where DisbursementCode = @DisbursementCode And ProvideWay = @ProvideWay And AptCode = @AptCode", new { DisbursementCode, ProvideWay, AptCode });
        }

        /// <summary>
        /// 지출결의서별 은행통장별 지출내역 총액
        /// </summary>
        public double Sum_Apt_DBMC_BankAccount(string DisbursementCode, string BankAccountCode, string AptCode)
        {
            using var db = new SqlConnection(_db.GetConnectionString("sw_togather"));
            return db.QuerySingleOrDefault<double>("Select ISNULL(Sum(InputSum), 0) From AccountDeals Where DisbursementCode = @DisbursementCode And BankAccountCode = @BankAccountCode And AptCode = @AptCode", new { DisbursementCode, BankAccountCode, AptCode });
        }

        /// <summary>
        /// 지출결의서별 은행통장별 지출내역 총액(내부 계좌 이체 입금)
        /// </summary>
        public double Sum_Apt_DBMC_inPut_BankAccount(string DisbursementCode, string BankAccountCode, string AptCode)
        {
            using var db = new SqlConnection(_db.GetConnectionString("sw_togather"));
            return db.QuerySingleOrDefault<double>("Select ISNULL(Sum(InputSum), 0) From AccountDeals Where DisbursementCode = @DisbursementCode And InputBankAccountCode = @BankAccountCode And AptCode = @AptCode And InOutDivision = 'B'", new { DisbursementCode, BankAccountCode, AptCode });
        }

        /// <summary>
        /// 지출내역 삭제
        /// </summary>
        public async Task Remove(int Aid)
        {
            using var db = new SqlConnection(_db.GetConnectionString("sw_togather"));
            await db.ExecuteAsync("Delete AccountDeals Where Aid = @Aid", new { Aid });
        }

        /// <summary>
        /// 지출내역 삭제
        /// </summary>
        /// <param name="Aid"></param>
        public async Task RemoveAll(string DisbursementCode, string AptCode)
        {
            using var db = new SqlConnection(_db.GetConnectionString("sw_togather"));
            await db.ExecuteAsync("Delete AccountDeals Where DisbursementCode = @DisbursementCode And AptCode = @Aptcode", new { DisbursementCode, AptCode });
        }

        /// <summary>
        /// 내부 이체 계좌 합계 구하기
        /// </summary>
        public async Task<double> InputBankAccountSum(string AptCode, string DisbursementCode, string BankAccountCode)
        {
            using var db = new SqlConnection(_db.GetConnectionString("sw_togather"));
            return await db.QuerySingleOrDefaultAsync<double>("Select ISNULL(Sum(InputSum), 0) From AccountDeals Where AptCode = @AptCode And DisbursementCode = @DisbursementCode And InputBankAccountCode = @BankAccountCode", new { DisbursementCode, BankAccountCode, AptCode });
        }

        /// <summary>
        /// 첨부파일 추가 또는 삭제
        /// </summary>
        public async Task FilesCount(string Aid, string Division)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                if (Division == "A")
                {
                    await dba.ExecuteAsync("Update AccountDeals Set Files_Count = Files_Count + 1 Where Aid = @Aid", new { Aid });
                }
                else if (Division == "B")
                {
                    await dba.ExecuteAsync("Update AccountDeals Set Files_Count = Files_Count - 1 Where Aid = @Aid", new { Aid });
                }

            }
        }

    }

    /// <summary>
    ///  통장 클래스
    /// </summary>
    public class BankAccount_Lib : IBankAccount_Lib
    {
        private readonly IConfiguration _db;
        public BankAccount_Lib(IConfiguration configuration)
        {
            this._db = configuration;
        }

        /// <summary>
        /// 통장정보 입력
        /// </summary>
        public async Task<int> Add(BankAccountEntity ba)
        {
            var sql = "Insert Into BankAccount (AptCode, BankName, BankAccountName, BankNumber, BankAccountSort, BankAccountDivision, InputDivision, Details, PostIp, User_Code) Values (@AptCode, @BankName, @BankAccountName, @BankNumber, @BankAccountSort, @BankAccountDivision, @InputDivision, @Details, @PostIp, @User_Code);Select Cast(SCOPE_IDENTITY() As Int);";
            using var db = new SqlConnection(_db.GetConnectionString("sw_togather"));
            int a = await db.QuerySingleOrDefaultAsync<int>(sql, ba);
            return a;
        }

        /// <summary>
        /// 통장정보 수정
        /// </summary>
        /// <param name="da"></param>
        public async Task Edit(BankAccountEntity da)
        {
            using var db = new SqlConnection(_db.GetConnectionString("sw_togather"));
            await db.ExecuteAsync("Update BankAccount Set BankName = @BankName, BankAccountName = @BankAccountName, BankAccountSort = @BankAccountSort, BankAccountDivision = @BankAccountDivision, InputDivision = @InputDivision, PostIp = @PostIp, User_Code = @User_Code Where del = 'A' And Aid = @Aid", da);
        }

        /// <summary>
        /// 통장 목록만들기
        /// </summary>
        /// <param name="AptCode"></param>
        /// <returns></returns>
        public async Task<List<BankAccountEntity>> GetList(string AptCode)
        {
            using var db = new SqlConnection(_db.GetConnectionString("sw_togather"));
            var lst = await db.QueryAsync<BankAccountEntity>("Select Aid, AptCode, BankName, BankAccountName, BankNumber, BankAccountSort, BankAccountDivision, InputDivision, Details, PostDate, PostIp, User_Code, del From BankAccount Where AptCode = @AptCode And del = 'A' Order By Aid Desc", new { AptCode });
            return lst.ToList();
        }

        /// <summary>
        /// 상세정보
        /// </summary>
        /// <param name="Aid"></param>
        /// <returns></returns>
        public async Task<BankAccountEntity> Details(int Aid)
        {
            using var db = new SqlConnection(_db.GetConnectionString("sw_togather"));
            return await db.QuerySingleOrDefaultAsync<BankAccountEntity>("Select Aid, AptCode, BankName, BankAccountName, BankNumber, BankAccountSort, BankAccountDivision, InputDivision, Details, PostDate, PostIp, User_Code, del From BankAccount Where Aid = @Aid", new { Aid });
        }

        /// <summary>
        /// 통장 삭제
        /// </summary>
        public async Task Remove(int Aid)
        {
            using var db = new SqlConnection(_db.GetConnectionString("sw_togather"));
            string strSort = await db.QuerySingleOrDefaultAsync<string>("Select del From BankAccount Where Aid = @Aid", new { Aid });
            if (strSort == "A")
            {
                await db.ExecuteAsync("Update BankAccount Set del = 'B' Where Aid = @Aid", new { Aid });
            }
            else
            {
                await db.ExecuteAsync("Update BankAccount Set del = 'A' Where Aid = @Aid", new { Aid });
            }
        }

        /// <summary>
        /// 통장 명(비동기실)
        /// </summary>
        public async Task<string> BankAccountName(int Aid)
        {
            using var db = new SqlConnection(_db.GetConnectionString("sw_togather"));
            return await db.QuerySingleOrDefaultAsync<string>("Select BankAccountName From BankAccount Where Aid = @Aid", new { Aid });
        }

        /// <summary>
        /// 통장 명(동기식)
        /// </summary>
        public string BankAccount_Name(int Aid)
        {
            using var db = new SqlConnection(_db.GetConnectionString("sw_togather"));
            return db.QuerySingleOrDefault<string>("Select BankAccountName From BankAccount Where Aid = @Aid", new { Aid });
        }

        /// <summary>
        /// 시재금 여부
        /// </summary>
        /// <param name="Aid"></param>
        /// <returns></returns>
        public async Task<string> InputDivision(string Aid)
        {
            using var db = new SqlConnection(_db.GetConnectionString("sw_togather"));
            return await db.QuerySingleOrDefaultAsync<string>("Select InputDivision From BankAccount Where Aid = @Aid", new { Aid });
        }
    }

    /// <summary>
    /// 통장거래 내역 클래스
    /// </summary>
    public class BankAccountDeals_Lib : IBankAccountDeals_Lib
    {
        private readonly IConfiguration _db;
        public BankAccountDeals_Lib(IConfiguration configuration)
        {
            this._db = configuration;
        }

        /// <summary>
        /// 통장 내역 입력
        /// </summary>
        /// <param name="bade"></param>
        /// <returns></returns>
        public async Task<int> Add(BankAccountDealsEntity bade)
        {
            var sql = "Insert Into BankAccountDeals (AptCode, BankAccountCode, DisbursementCode, Ago_Balance, After_Balance, InputSum, OutputSum, Details, PostIp, User_Code) Values (@AptCode, @BankAccountCode, @DisbursementCode, @Ago_Balance, @After_Balance, @InputSum, @OutputSum, @Details, @PostIp, @User_Code);Select Cast(SCOPE_IDENTITY() As Int);";
            using var db = new SqlConnection(_db.GetConnectionString("sw_togather"));
            int a = await db.QuerySingleOrDefaultAsync<int>(sql, bade);
            return a;
        }

        /// <summary>
        /// 통장 내역 수정(전금액, 후금액, 지출합계, 상세, 아이피, 사용자코드)
        /// </summary>
        /// <param name="bade"></param>
        public async Task Edit(BankAccountDealsEntity bade)
        {
            using var db = new SqlConnection(_db.GetConnectionString("sw_togather"));
            await db.ExecuteAsync("Update BankAccountDeals Set Ago_Balance = Ago_Balance, InputSum = @InputSum, OutputSum = @OutputSum, After_Balance = @After_Balance, Details = @Details, PostIp = @PostIp, User_Code = @User_Code Where BankAccountCode = @BankAccountCode And DisbursementCode = @DisbursementCode And Aid = @Aid", bade);
        }

        /// <summary>
        /// 통장 내역 보고
        /// </summary>
        /// <param name="AptCode"></param>
        /// <returns></returns>
        public async Task<List<BankAccountDealsEntity>> GetList(string AptCode)
        {
            using var db = new SqlConnection(_db.GetConnectionString("sw_togather"));
            var lst = await db.QueryAsync<BankAccountDealsEntity>("Select Aid, AptCode, BankAccountCode, DisbursementCode, Ago_Balance, After_Balance, InputSum, OutputSum, Details, PostDate, PostIp, User_Code, Files_Count From BankAccountDeals Where AptCode = @AptCode And del = 'A' Order by Aid Desc", new { AptCode });
            return lst.ToList();
        }

        /// <summary>
        /// 상세보기(지출결의서 식별코드)
        /// </summary>
        public async Task<BankAccountDealsEntity> Details(string AptCode, string DisbursementCode, string BankAccountCode)
        {
            using var db = new SqlConnection(_db.GetConnectionString("sw_togather"));
            return await db.QuerySingleOrDefaultAsync<BankAccountDealsEntity>("Select Top 1 Aid, AptCode, BankAccountCode, DisbursementCode, Ago_Balance, After_Balance, InputSum, OutputSum, Details, PostDate, PostIp, User_Code, Files_Count From BankAccountDeals Where DisbursementCode = @DisbursementCode And BankAccountCode = @BankAccountCode And del = 'A' Order by Aid Desc", new { AptCode, DisbursementCode, BankAccountCode });
        }

        /// <summary>
        /// 해당 지출결의서 코드로 존재 여부 확인
        /// </summary>
        /// <param name="AptCode"></param>
        /// <param name="DisbursementCode"></param>
        /// <returns></returns>
        public async Task<int> Being(string AptCode, string DisbursementCode)
        {
            using var db = new SqlConnection(_db.GetConnectionString("sw_togather"));
            return await db.QuerySingleOrDefaultAsync<int>("Select Count(*) From BankAccountDeals Where AptCode = @AptCode And DisbursementCode = @DisbursementCode", new { AptCode, DisbursementCode });
        }


        /// <summary>
        /// 해당 지출결의서 코드로 존재 여부 확인
        /// </summary>
        public int Being_BankAccount(string AptCode, string DisbursementCode, string BankAccountCode)
        {
            using var db = new SqlConnection(_db.GetConnectionString("sw_togather"));
            return db.QuerySingleOrDefault<int>("Select Count(*) From BankAccountDeals Where AptCode = @AptCode And DisbursementCode = @DisbursementCode And BankAccountCode = @BankAccountCode", new { AptCode, DisbursementCode, BankAccountCode });
        }

        /// <summary>
        /// 지출결의서 별 통장거래 내역  불러오기
        /// </summary>
        /// <param name="DisbursementCode"></param>
        /// <returns></returns>
        public async Task<BankAccountDealsEntity> DetailsDBS(string DisbursementCode)
        {
            using var db = new SqlConnection(_db.GetConnectionString("sw_togather"));
            return await db.QuerySingleOrDefaultAsync<BankAccountDealsEntity>("Select Aid, AptCode, BankAccountCode, DisbursementCode, Ago_Balance, After_Balance, InputSum, OutputSum, Details, PostDate, PostIp, User_Code, Files_Count From BankAccountDeals Where DisbursementCode = @DisbursementCode And del = 'A' Order by Aid Desc", new { DisbursementCode });
        }

        /// <summary>
        /// 통장별 마지막 지급 후 잔액 구하기
        /// </summary>
        /// <param name="AptCode"></param>
        /// <param name="BankAccountCode"></param>
        /// <returns></returns>
        public double Balance_Last(string AptCode, string DisbursementCode, string BankAccountCode)
        {
            using var db = new SqlConnection(_db.GetConnectionString("sw_togather"));
            return db.QuerySingleOrDefault<double>("Select Top 1 ISNULL(After_Balance, 0) From BankAccountDeals Where AptCode = 'sw134' And BankAccountCode = @BankAccountCode And DisbursementCode < @DisbursementCode Order By Aid Desc", new { AptCode, DisbursementCode, BankAccountCode });
        }

        /// <summary>
        /// 통장별 마지막 지급 후 잔액 구하기(하나 전)
        /// </summary>
        public async Task<double> Balance_Last_Result(string AptCode, string Aid, string BankAccountCode)
        {
            using var db = new SqlConnection(_db.GetConnectionString("sw_togather"));
            return await db.QuerySingleOrDefaultAsync<double>("Select ISNULL(After_Balance, 0) From BankAccountDeals Where AptCode = @AptCode And BankAccountCode = @BankAccountCode And del = 'A' And Aid = (Select max(Aid) From BankAccountDeals Where AptCode = @AptCode And del = 'A' and Aid < @Aid)", new { AptCode, BankAccountCode });
        }

        /// <summary>
        /// 통장 내역 존재 여부
        /// </summary>
        public async Task<int> Being_Last(string AptCode, string DisbursementCode, string BankAccountCode)
        {
            using var db = new SqlConnection(_db.GetConnectionString("sw_togather"));
            return await db.QuerySingleOrDefaultAsync<int>("Select Count(*) From BankAccountDeals Where AptCode = @AptCode And BankAccountCode = @BankAccountCode And DisbursementCode = @DisbursementCode And del = 'A'", new { AptCode, DisbursementCode, BankAccountCode });
        }

        /// <summary>
        /// 통장 내역 존재 여부(일련번호
        /// </summary>
        public async Task<int> Being_Aid(string AptCode, string DisbursementCode, string BankAccountCode)
        {
            using var db = new SqlConnection(_db.GetConnectionString("sw_togather"));
            return await db.QuerySingleOrDefaultAsync<int>("Select Count(*) From BankAccountDeals Where AptCode = @AptCode And BankAccountCode = @BankAccountCode And DisbursementCode = @DisbursementCode And del = 'A'", new { AptCode, DisbursementCode, BankAccountCode });
        }

        /// <summary>
        /// 해당 지출결의서에 통장별 지급 후 잔액 구하기
        /// </summary>
        /// <param name="AptCode"></param>
        /// <param name="BankAccountCode"></param>
        /// <returns></returns>
        public double After_Balance_Last(string AptCode, string BankAccountCode, string DisbursementCode)
        {
            using var db = new SqlConnection(_db.GetConnectionString("sw_togather"));
            return db.QuerySingleOrDefault<double>("Select Top 1 ISNULL(After_Balance, 0) From BankAccountDeals Where AptCode = @AptCode And BankAccountCode = @BankAccountCode And DisbursementCode = @DisbursementCode And del = 'A' Order By Aid Desc", new { AptCode, BankAccountCode, DisbursementCode });
        }

        /// <summary>
        /// 해당 지출결의서에 통장별 지급 전 잔액 구하기
        /// </summary>
        /// <param name="AptCode"></param>
        /// <param name="BankAccountCode"></param>
        /// <returns></returns>
        public double Ago_Balance_Last(string AptCode, string BankAccountCode, string DisbursementCode)
        {
            using var db = new SqlConnection(_db.GetConnectionString("sw_togather"));
            return db.QuerySingleOrDefault<double>("Select Top 1 ISNULL(Ago_Balance, 0) From BankAccountDeals Where AptCode = @AptCode And BankAccountCode = @BankAccountCode And DisbursementCode = @DisbursementCode And del = 'A' Order By Aid Desc", new { AptCode, BankAccountCode, DisbursementCode });
        }

        /// <summary>
        /// 해당 지출결의서에 통장별 지출액 구하기
        /// </summary>
        /// <param name="AptCode"></param>
        /// <param name="BankAccountCode"></param>
        /// <returns></returns>
        public double InputSum_Last(string AptCode, string BankAccountCode, string DisbursementCode)
        {
            using var db = new SqlConnection(_db.GetConnectionString("sw_togather"));
            return db.QuerySingleOrDefault<double>("Select Top 1 ISNULL(InputSum, 0) From BankAccountDeals Where AptCode = @AptCode And BankAccountCode = @BankAccountCode And DisbursementCode = @DisbursementCode And del = 'A' Order By Aid Desc", new { AptCode, BankAccountCode, DisbursementCode });
        }

        /// <summary>
        /// 해당 지출결의서에 통장별 입금액 구하기
        /// </summary>
        /// <param name="AptCode"></param>
        /// <param name="BankAccountCode"></param>
        /// <returns></returns>
        public double OutputSum_Last(string AptCode, string BankAccountCode, string DisbursementCode)
        {
            using var db = new SqlConnection(_db.GetConnectionString("sw_togather"));
            return db.QuerySingleOrDefault<double>("Select Top 1 ISNULL(OutputSum, 0) From BankAccountDeals Where AptCode = @AptCode And BankAccountCode = @BankAccountCode And DisbursementCode = @DisbursementCode And del = 'A' Order By Aid Desc", new { AptCode, BankAccountCode, DisbursementCode });
        }

        /// <summary>
        /// 해당 지출결의서에 통장내역의 입력일 구하기
        /// </summary>
        /// <param name="AptCode"></param>
        /// <param name="BankAccountCode"></param>
        /// <returns></returns>
        public DateTime InputDate(string AptCode, string DisbursementCode, string BankAccountCode)
        {
            using var db = new SqlConnection(_db.GetConnectionString("sw_togather"));
            return db.QuerySingleOrDefault<DateTime>("Select PostDate From BankAccountDeals Where AptCode = @AptCode And BankAccountCode = @BankAccountCode And DisbursementCode = @DisbursementCode And del = 'A'", new { AptCode, BankAccountCode, DisbursementCode });
        }

        /// <summary>
        /// 삭제
        /// </summary>
        /// <param name="Aid"></param>
        public async Task Remove(int Aid)
        {
            using var db = new SqlConnection(_db.GetConnectionString("sw_togather"));
            await db.ExecuteAsync("Update BankAccountDeals Set del = 'B' Where Aid = @Aid", new { Aid });
        }

        /// <summary>
        /// 삭제
        /// </summary>
        /// <param name="Aid"></param>
        public async Task Delete(string DisbursementCode, string AptCode)
        {
            using var db = new SqlConnection(_db.GetConnectionString("sw_togather"));
            await db.ExecuteAsync("Delete BankAccountDeals Where DisbursementCode = @DisbursementCode And AptCode = @AptCode", new { DisbursementCode, AptCode });
        }

        /// <summary>
        /// 결재 입력
        /// </summary>
        /// <param name="Aid"></param>
        public async Task ApprovalEdit(string DisbursementCode, string BankAccountCode)
        {
            using var db = new SqlConnection(_db.GetConnectionString("sw_togather"));
            await db.ExecuteAsync("Update BankAccountDeals Set Approval = 'B' Where DisbursementCode = @DisbursementCode And BankAccountCode = @BankAccountCode", new { DisbursementCode, BankAccountCode });
        }

        /// <summary>
        /// 결재여부 확인
        /// </summary>
        /// <param name="DisbursementCode"></param>
        /// <param name="BankAccountCode"></param>
        /// <returns></returns>
        public string ApprovalView(string DisbursementCode, string BankAccountCode)
        {
            using var db = new SqlConnection(_db.GetConnectionString("sw_togather"));
            return db.QuerySingleOrDefault<string>("Select Approval From BankAccountDeals Where DisbursementCode = @DisbursementCode And BankAccountCode = @BankAccountCode", new { DisbursementCode, BankAccountCode });
        }

        /// <summary>
        /// 첨부파일 추가 또는 삭제
        /// </summary>
        public async Task FilesCount(string Aid, string strAid, string Division)
        {
            using var dba = new SqlConnection(_db.GetConnectionString("sw_togather"));
            if (Division == "A")
            {
                await dba.ExecuteAsync("Update BankAccountDeals Set Files_Count = Files_Count + 1 Where BankAccountCode = @Aid And DisbursementCode = @strAid", new { Aid, strAid });
            }
            else if (Division == "B")
            {
                await dba.ExecuteAsync("Update BankAccountDeals Set Files_Count = Files_Count - 1 Where BankAccountCode = @Aid And DisbursementCode = @strAid", new { Aid, strAid });
            }
        }

        public int FileCount(string Aid, string Dv_Aid)
        {
            using var dba = new SqlConnection(_db.GetConnectionString("sw_togather"));
            return dba.QuerySingleOrDefault<int>("Select Files_Count From BankAccountDeals Where BankAccountCode = @Aid And DisbursementCode = @Dv_Aid", new { Aid, Dv_Aid });
        }

        /// <summary>
        /// 결재 후 잔액 불러오기
        /// </summary>
        public async Task<double> Be_Ago_Disbursement (string AptCode, string BankAccountCode, string DisbursementCode)
        {
            using var dba = new SqlConnection(_db.GetConnectionString("sw_togather"));
            return await dba.QuerySingleOrDefaultAsync<double>("Select Top 2 Aid From Disbursement Order By Aid Desc", new { AptCode, BankAccountCode, DisbursementCode });
        }
    }
}
