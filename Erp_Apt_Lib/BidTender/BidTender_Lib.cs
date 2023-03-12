using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Erp_Apt_Lib.BidTender
{
    /// <summary>
    /// 1. 입찰 클래스
    /// </summary>
    public class BidTender_Lib
    {
        private readonly IConfiguration _db;
        public BidTender_Lib(IConfiguration configuration)
        {
            this._db = configuration;
        }

        /// <summary>
        /// 입찰 기본 정보 입력
        /// </summary>
        /// <param name="ad"></param>
        /// <returns></returns>
        public async Task<int> Add(BidTender ad)
        {
            var sql = "Insert Into BidTender (LawCode, BidTenderName, BidSort, BidCategory, AptCode, Bid, Tender, BidDate, BidingDate, QualificationDate, Details, PostIp, UserCode) Values ();Select Cast(SCOPE_IDENTITY() As Int);";
            using (var db = new SqlConnection(_db.GetConnectionString("BidTender")))
            {
                int a = await db.QuerySingleOrDefaultAsync<int>(sql, ad);
                return a;
            }
            
        }

        /// <summary>
        /// 입찰 기본 정보 수정
        /// </summary>
        /// <param name="ad"></param>
        public async Task Edit(BidTender ad)
        {
            using (var db = new SqlConnection(_db.GetConnectionString("BidTender")))
            {
                await db.ExecuteAsync("Update BidTender Set LawCode = @LawCode, BidTenderName = @BidTenderName, BidSort = @BidSort, Bid = @Bid, Tender = @Tender, BidDate = @BidDate, BidingDate = @BidingDate, QualificationDate = @QualificationDate, Details = @Details Where BidTenderAid = @BidTenderAid", new { ad });
            }
            
        }

        /// <summary>
        /// 입찰 기본 정보 목록
        /// </summary>
        /// <param name="AptCode"></param>
        /// <returns></returns>
        public async Task<List<BidTender>> GetList(string AptCode)
        {
            using (var db = new SqlConnection(_db.GetConnectionString("BidTender")))
            {
                var lst = await db.QueryAsync<BidTender>("Select BidTenderAid, LawCode, BidTenderName, BidSort, BidCategory, AptCode, Bid, Tender, BidDate, BidingDate, QualificationDate, Details, PosDate, PostIp, UserCode, del From BidTender Where AptCode = @AptCode And del = 'A'", new { AptCode });
                return lst.ToList();
            }
            
        }

        /// <summary>
        /// 해당 공동주택에 입력 총 수
        /// </summary>
        /// <param name="AptCode"></param>
        /// <returns></returns>
        public async Task<int> TotalCount(string AptCode)
        {
            using (var db = new SqlConnection(_db.GetConnectionString("BidTender")))
            {
                return await db.QuerySingleOrDefaultAsync<int>("Select Count(*) From BidTender Where AptCode = @AptCode And del = 'A'", new { AptCode });
            }           

        }

        /// <summary>
        /// 입찰 기본정보 상세
        /// </summary>
        /// <param name="BidTenderAid"></param>
        /// <returns></returns>
        public async Task<BidTender> Details(string BidTenderAid)
        {
            using (var db = new SqlConnection(_db.GetConnectionString("BidTender")))
            {
                return await db.QuerySingleOrDefaultAsync<BidTender>("Select BidTenderAid, LawCode, BidTenderName, BidSort, BidCategory, AptCode, Bid, Tender, BidDate, BidingDate, QualificationDate, Details, PosDate, PostIp, UserCode, del From BidTender Where BidTenderAid= @BidTenderAid", new { BidTenderAid });
            }
            
        }

        /// <summary>
        /// 입찰 기본 정보 삭제
        /// </summary>
        /// <param name="BidTenderAid"></param>
        public async Task Remove(string BidTenderAid)
        {
            using (var db = new SqlConnection(_db.GetConnectionString("BidTender")))
            {
                await db.ExecuteAsync("Updat;e BidTender Set del = 'B' Where BidTenderAid = @BidTenderAid");
            }
            
        }
    }

    /// <summary>
    /// 2. 입찰 분류 클래스(주택관리업자, 공사, 용역 등)
    /// </summary>
    public class BidSort_Lib
    {
        private readonly IConfiguration _db;
        public BidSort_Lib(IConfiguration configuration)
        {
            this._db = configuration;
        }

        /// <summary>
        /// 입찰분류 입력
        /// </summary>
        /// <param name="ad"></param>
        /// <returns></returns>
        public async Task<int> Add(BidSort ad)
        {
            var sql = "Insert into BidSort (SortName, UpCode, Step, LawCode, Details, UserCode) Values (@SortName, @UpCode, @Step, @LawCode, @Details, @UserCode); Select Cast(SCOPE_IDENTITY() As Int);";
            using (var db = new SqlConnection(_db.GetConnectionString("BidTender")))
            {
                int a = await db.QuerySingleOrDefaultAsync<int>(sql, ad);
                return a;
            }
            
        }

        /// <summary>
        /// 입찰분류 입력
        /// </summary>
        /// <param name="ad"></param>
        /// <returns></returns>
        public async Task<int> Edit(BidSort ad)
        {
            var sql = "Update BidSort Set SortName = @SortName, UpCode = @UpCode, Step = @Step, LawCode = @LawCode, Details = @Details Where BidsortAid = @BidsortAid;";
            using (var db = new SqlConnection(_db.GetConnectionString("BidTender")))
            {
                int a = await db.QuerySingleOrDefaultAsync<int>(sql, ad);
                return a;
            }

        }

        /// <summary>
        /// 입찰 분류 선택
        /// </summary>
        /// <returns></returns>
        public async Task<List<BidSort>> GetList()
        {
            using (var db = new SqlConnection(_db.GetConnectionString("BidTender")))
            {
                var lst = await db.QueryAsync<BidSort>("Select BidsortAid, SortName, UpCode, Step, LawCode, Details, PostDate, UserCode From BidSort Where del = 'A'");
                return lst.ToList();
            }
            
        }

        /// <summary>
        /// 입찰 분류 삭제
        /// </summary>
        /// <param name="Aid"></param>
        public async Task Remove(string Aid)
        {
            using (var db = new SqlConnection(_db.GetConnectionString("BidTender")))
            {
                await db.ExecuteAsync("Update Sort Set del = 'B' Where BidsortAid = @Aid", new { Aid });
            }
            
        }
    }

    /// <summary>
    /// 3. 입찰선택
    /// 일반, 제한, 부대, 수의 선택
    /// </summary>
    public class BidSelect_Lib
    {
        private readonly IConfiguration _db;
        public BidSelect_Lib(IConfiguration configuration)
        {
            this._db = configuration;
        }

        /// <summary>
        /// 입찰선택 입력
        /// </summary>
        /// <param name="ad"></param>
        /// <returns></returns>
        public async Task<int> Add(BidSelect ad)
        {
            var sql = "Insert into BidSelect (BidTenderCode, BidSortCode, BidSortName, Details, PostIp, UserCode) Values (@BidTenderCode, @BidSortCode, @BidSortName, @Details, @PostIp, @UserCode); Select Cast(SCOPE_IDENTITY() As Int);";
            using (var db = new SqlConnection(_db.GetConnectionString("BidTender")))
            {
                int a = await db.QuerySingleOrDefaultAsync<int>(sql, ad);
                return a;
            }

        }

        /// <summary>
        /// 입찰선택 수정
        /// </summary>
        /// <param name="ad"></param>
        /// <returns></returns>
        public async Task<int> Edit(BidSelect ad)
        {
            var sql = "Update BidSelect Set BidTenderCode = @BidTenderCode, BidSortCode = @BidSortCode, BidSortName = @BidSortName, Details = @Details, PostIp = @PostIp, UserCode = @UserCode Where BidSelectAid = @BidSelectAid;";
            using (var db = new SqlConnection(_db.GetConnectionString("BidTender")))
            {
                int a = await db.QuerySingleOrDefaultAsync<int>(sql, ad);
                return a;
            }

        }

        /// <summary>
        /// 입찰선택 선택
        /// </summary>
        /// <returns></returns>
        public async Task<List<BidSelect>> GetList()
        {
            using (var db = new SqlConnection(_db.GetConnectionString("BidTender")))
            {
                var lst = await db.QueryAsync<BidSelect>("Select BidSelectAid, BidTenderCode, BidSortCode, BidSortName, Details, PostDate, PostIp, UserCode From BidSelect");
                return lst.ToList();
            }

        }

        /// <summary>
        /// 입찰선택 삭제
        /// </summary>
        /// <param name="Aid"></param>
        public async Task Remove(string Aid)
        {
            using (var db = new SqlConnection(_db.GetConnectionString("BidTender")))
            {
                await db.ExecuteAsync("Delete BidSelect Where BidSelectAid = @Aid", new { Aid });
            }

        }
    }

    /// <summary>
    /// 4. 제한 경쟁 선택 시 
    /// 상세 내용 입력
    /// </summary>
    public class LimitCompetition_Lib
    {
        private readonly IConfiguration _db;
        public LimitCompetition_Lib(IConfiguration configuration)
        {
            this._db = configuration;
        }

        /// <summary>
        /// 제한 경쟁 입력
        /// </summary>
        /// <param name="ad"></param>
        /// <returns></returns>
        public async Task<int> Add(LimitCompetition ad)
        {
            var sql = "Insert into LimitCompetition (BidTenderCode, AptCode, BidSelectCode, Capital, Performance, Technical, Details, UserCode) Values (@BidTenderCode, @AptCode, @BidSelectCode, @Capital, @Performance, @Technical, @Details, @UserCode); Select Cast(SCOPE_IDENTITY() As Int);";
            using (var db = new SqlConnection(_db.GetConnectionString("BidTender")))
            {
                int a = await db.QuerySingleOrDefaultAsync<int>(sql, ad);
                return a;
            }

        }

        /// <summary>
        /// 제한 경쟁 수정
        /// </summary>
        /// <param name="ad"></param>
        /// <returns></returns>
        public async Task<int> Edit(LimitCompetition ad)
        {
            var sql = "Update LimitCompetition Set BidTenderCode = @BidTenderCode, AptCode = @AptCode, BidSelectCode = @BidSelectCode, Capital = @Capital, Performance = @Performance, Technical = @Technical, Details = @Details, UserCode = @UserCode Where LimitCompetitionAid = @LimitCompetitionAid;";
            using (var db = new SqlConnection(_db.GetConnectionString("BidTender")))
            {
                int a = await db.QuerySingleOrDefaultAsync<int>(sql, ad);
                return a;
            }

        }

        /// <summary>
        /// 제한 경쟁 선택
        /// </summary>
        /// <returns></returns>
        public async Task<List<LimitCompetition>> GetList()
        {
            using (var db = new SqlConnection(_db.GetConnectionString("BidTender")))
            {
                var lst = await db.QueryAsync<LimitCompetition>("Select LimitCompetitionAid, BidTenderCode, AptCode, BidSelectCode, Capital, Performance, Technical, Details, PostDate, UserCode, del From LimitCompetition");
                return lst.ToList();
            }

        }

        /// <summary>
        /// 제한 경쟁 삭제
        /// </summary>
        /// <param name="Aid"></param>
        public async Task Remove(string Aid)
        {
            using (var db = new SqlConnection(_db.GetConnectionString("BidTender")))
            {
                await db.ExecuteAsync("Update LimitCompetition Set del = 'B' Where LimitCompetitionAid = @Aid", new { Aid });
            }

        }
    }

    /// <summary>
    /// 9. 입찰 방법 클래스
    /// </summary>
    public class BidingMethod_Lib
    {
        private readonly IConfiguration _db;
        public BidingMethod_Lib(IConfiguration configuration)
        {
            this._db = configuration;
        }

        /// <summary>
        /// 낙찰 방법 입력
        /// </summary>
        /// <param name="ad"></param>
        /// <returns></returns>
        public async Task<int> Add(BidingMethod ad)
        {
            var sql = "Insert Into BidingMethod (BidingMethodName, LawCode, Details, PostIp, UserCode) Values (@BidingMethodName, @LawCode, @Details, @PostIp, @UserCode);Select Cast(SCOPE_IDENTITY() As Int);";
            using (var db = new SqlConnection(_db.GetConnectionString("BidTender")))
            {
                int a = await db.QuerySingleOrDefaultAsync<int>(sql, ad);
                return a;
            }
        }

        /// <summary>
        /// 낙찰방법 목록
        /// </summary>
        /// <returns></returns>
        public async Task<List<BidingMethod>> GetList()
        {
            using (var db = new SqlConnection(_db.GetConnectionString("BidTender")))
            {
                var lst = await db.QueryAsync<BidingMethod>("Select BidingMethodAid, BidingMethodName, LawCode, Details, PostDate, PostIp, UserCode From BidingMethod Where del = 'A'");
                return lst.ToList();
            }
            
        }

        /// <summary>
        /// 삭제
        /// </summary>
        /// <param name="Aid"></param>
        public async Task Remove(string Aid)
        {
            using (var db = new SqlConnection(_db.GetConnectionString("BidTender")))
            {
                await db.ExecuteAsync("Updage BidingMethod Set del = 'B' Where BidingMethodAid = @Aid");
            }            
        }
    }


}
