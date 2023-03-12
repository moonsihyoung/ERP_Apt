using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Erp_Apt_Lib.MonthlyUsage
{
    public class MonthlyUsage_Lib : IMonthlyUsage_Lib
    {
        private readonly IConfiguration _db;
        public MonthlyUsage_Lib(IConfiguration configuration)
        {
            this._db = configuration;
        }

        /// <summary>
        /// 사용량 입력
        /// </summary>
        public async Task Add(MonthlyUsage_Entity ar)
        {
            using var db = new SqlConnection(_db.GetConnectionString("sw_togather"));
            var sql = "Insert into MonthlyUsage  (Apt_Code, Apt_Name, UseDate, IntYear, intMonth, ElectricContractMethod, ElectricInpose, ElectricIndustry, ElectricIndustryFee, ElectricStreetLamp, ElectricStreetLampFee, ElectricAllUsage, ElectricAllFee, ElectricComUsage, ElectricComFee, ElectricPerUsage, ElectricPerFee, WaterAllUsage, WaterAllFee, WaterComUsage, WaterComFee, WaterPerUsage, WaterPerFee, HotWaterAllUsage, HotWaterComUsage, HotWaterPerUsage, codeHeatNm, HeatingAllUsage, HeatingComUsage, HeatingPerUsage, GasUsage, HotWaterAllFee, HotWaterComFee, HotWaterPerFee, HeatingAllFee, HeatingComFee, HeatingPerFee, GasFee, PostIp, UserCode) Values (@Apt_Code, @Apt_Name, @UseDate, @IntYear, @intMonth, @ElectricContractMethod, @ElectricInpose, @ElectricIndustry, @ElectricIndustryFee, @ElectricStreetLamp, @ElectricStreetLampFee, @ElectricAllUsage, @ElectricAllFee, @ElectricComUsage, @ElectricComFee, @ElectricPerUsage, @ElectricPerFee, @WaterAllUsage, @WaterAllFee, @WaterComUsage, @WaterComFee, @WaterPerUsage, @WaterPerFee, @HotWaterAllUsage, @HotWaterComUsage, @HotWaterPerUsage, @codeHeatNm, @HeatingAllUsage, @HeatingComUsage, @HeatingPerUsage, @GasUsage, @HotWaterAllFee, @HotWaterComFee, @HotWaterPerFee, @HeatingAllFee, @HeatingComFee, @HeatingPerFee, @GasFee, @PostIp, @UserCode)";
            await db.ExecuteAsync(sql, ar);
        }

        /// <summary>
        /// 사용량 수정
        /// </summary>
        public async Task Edit(MonthlyUsage_Entity ar)
        {
            using var db = new SqlConnection(_db.GetConnectionString("sw_togather"));
            var sql = "Update MonthlyUsage Set UseDate = @UseDate, intYear = @intYear, intMonth = @intMonth, ElectricContractMethod = @ElectricContractMethod, ElectricInpose = @ElectricInpose, ElectricIndustry = @ElectricIndustry, ElectricIndustryFee = @ElectricIndustryFee, ElectricStreetLamp = @ElectricStreetLamp, ElectricStreetLampFee = @ElectricStreetLampFee, ElectricAllUsage = @ElectricAllUsage, ElectricAllFee = @ElectricAllFee, ElectricComUsage = @ElectricComUsage, ElectricComFee = @ElectricComFee, ElectricPerUsage = @ElectricPerUsage, ElectricPerFee = @ElectricPerFee, WaterAllUsage = @WaterAllUsage, WaterAllFee = @WaterAllFee, WaterComUsage = @WaterComUsage, WaterComFee = @WaterComFee, WaterPerUsage = @WaterPerUsage, WaterPerFee = @WaterPerFee, HotWaterAllUsage = @HotWaterAllUsage, HotWaterAllFee = @HotWaterAllFee, HotWaterComUsage = @HotWaterComUsage, HotWaterComFee = @HotWaterComFee, HotWaterPerUsage = @HotWaterPerUsage,HotWaterPerFee = @HotWaterPerFee,  codeHeatNm = @codeHeatNm, HeatingAllUsage = @HeatingAllUsage, HeatingAllFee = @HeatingAllFee, HeatingComUsage = @HeatingComUsage, HeatingComFee = @HeatingComFee, HeatingPerUsage = @HeatingPerUsage, HeatingPerFee = @HeatingPerFee, GasUsage = @GasUsage, GasFee = @GasFee, PostIp = @PostIp, UserCode = @UserCode Where Aid = @Aid;";

            await db.ExecuteAsync(sql, ar);
        }

        /// <summary>
        /// 사용량 상세보기
        /// </summary>
        public async Task<MonthlyUsage_Entity> GetById(string Apt_Code, int intYear, int intMonth)
        {
            using var db = new SqlConnection(_db.GetConnectionString("sw_togather"));
            return await db.QuerySingleOrDefaultAsync<MonthlyUsage_Entity>("Select * From MonthlyUsage Where Apt_Code = @Apt_Code And intYear = @intYear And intMonth = @intMonth", new { Apt_Code, intYear, intMonth });
        }

        /// <summary>
        /// 해당월 사용량 존재 여부
        /// </summary>
        public async Task<int> GetById_Count(string Apt_Code, int intYear, int intMonth)
        {
            using var db = new SqlConnection(_db.GetConnectionString("sw_togather"));
            return await db.QuerySingleOrDefaultAsync<int>("Select Count(*) From MonthlyUsage Where Apt_Code = @Apt_Code And intYear = @intYear And intMonth = @intMonth", new { Apt_Code, intYear, intMonth });
        }

        /// <summary>
        /// 해당월 사용량 존재 여부
        /// </summary>
        public async Task<int> GetById_Being(string Apt_Code)
        {
            using var db = new SqlConnection(_db.GetConnectionString("sw_togather"));
            return await db.QuerySingleOrDefaultAsync<int>("Select Top 1 Aid From MonthlyUsage Where Apt_Code = @Apt_Code Order by Aid Desc", new { Apt_Code });
        }

        /// <summary>
        /// 사용량 상세보기
        /// </summary>
        public async Task<MonthlyUsage_Entity> GetDetails(int Aid)
        {
            using var db = new SqlConnection(_db.GetConnectionString("sw_togather"));
            return await db.QuerySingleOrDefaultAsync<MonthlyUsage_Entity>("Select * From MonthlyUsage Where Aid = @Aid", new { Aid });
        }

        /// <summary>
        /// 모든 사용량 정보
        /// </summary>
        public async Task<List<MonthlyUsage_Entity>> GetListAll(int Page)
        {
            using var db = new SqlConnection(_db.GetConnectionString("sw_togather"));
            var lst = await db.QueryAsync<MonthlyUsage_Entity>("Select Top 15 * From MonthlyUsage Where Aid Not In (Select Top(15 * @Page) Aid From MonthlyUsage Order by Aid Desc) Order by Aid Desc", new { Page });
            return lst.ToList();
        }

        /// <summary>
        /// 모든 사용량 정보 수
        /// </summary>
        public async Task<int> GetListAll_Count()
        {
            using var db = new SqlConnection(_db.GetConnectionString("sw_togather"));
            return await db.QuerySingleOrDefaultAsync<int>("Select Count(*) From MonthlyUsage");
        }

        /// <summary>
        /// 해당 공동주택 모든 사용량 정보
        /// </summary>
        public async Task<List<MonthlyUsage_Entity>> GetList(int Page, string Apt_Code)
        {
            using var db = new SqlConnection(_db.GetConnectionString("sw_togather"));
            var lst = await db.QueryAsync<MonthlyUsage_Entity>("Select Top 15 * From MonthlyUsage Where Aid Not In (Select Top (15 * @Page) Aid From MonthlyUsage Where Apt_Code = @Apt_Code And Del = 'A' Order by Aid Desc) And Apt_Code = @Apt_Code And Del = 'A' Order by Aid Desc", new { Page, Apt_Code });
            return lst.ToList();
        }

        /// <summary>
        /// 해당 공동주택 모든 사용량 정보 수
        /// </summary>
        public async Task<int> GetList_Count(string Apt_Code)
        {
            using var db = new SqlConnection(_db.GetConnectionString("sw_togather"));
            return await db.QuerySingleOrDefaultAsync<int>("Select Count(*) From MonthlyUsage Where Apt_Code = @Apt_Code And Del = 'A'", new {Apt_Code});
        }

        /// <summary>
        /// 삭제
        /// </summary>
        public async Task Remove(int Aid)
        {
            using var db = new SqlConnection(_db.GetConnectionString("sw_togather"));
            await db.ExecuteAsync("Update MonthlyUsage Set Del = 'B' Where Aid = @Aid", new { Aid });
        }
    }

    public class UsageDetails_Lib : IUsageDetails_Lib
    {
        private readonly IConfiguration _db;
        public UsageDetails_Lib(IConfiguration configuration)
        {
            this._db = configuration;
        }

        /// <summary>
        /// 상세 사용량 정보 입력
        /// </summary>
        public async Task Add(UsageDetails_Entity ar)
        {
            using var db = new SqlConnection(_db.GetConnectionString("sw_togather"));
            var sql = "Insert into UsageDetails (Apt_Code, Apt_Name, UsageDate, intYear, intMonth, Division, UseName, Usage, CostSum, Etc, Details, PostIp, UserCode) Values (@Apt_Code, @Apt_Name, @UsageDate, @intYear, @intMonth, @Division, @UseName, @Usage, @CostSum, @Etc, @Details, @PostIp, @UserCode)";
            await db.ExecuteAsync(sql, ar);
        }

        /// <summary>
        /// 상세 사용량 정보 수정
        /// </summary>
        public async Task Edit(UsageDetails_Entity ar)
        {
            using var db = new SqlConnection(_db.GetConnectionString("sw_togather"));
            var sql = "Update UsageDetails Set UsageDate = @UsageDate, intYear = @intYear, intMonth = @intMonth, Division = @Division, UseName = @UseName, Usage = @Usage, CostSum = @CostSum, Etc = @Etc, Details = @Details, PostIp = @PostIp, UserCode = @UserCode Where Aid = @Aid;";

            await db.ExecuteAsync(sql, ar);
        }

        /// <summary>
        /// 해당 상세 정보
        /// </summary>
        public async Task<UsageDetails_Entity> GetById(string Apt_Code, int intYear, int intMonth)
        {
            using var db = new SqlConnection(_db.GetConnectionString("sw_togather"));
            return await db.QuerySingleOrDefaultAsync<UsageDetails_Entity>("Select * From UsageDetails Where Apt_Code = @Apt_Code And intYear = @intYear And intMonth = @intMonth", new { Apt_Code, intYear, intMonth });
        }


        /// <summary>
        /// 해당 공동주택 사용량 상세 정보 존재 여부 
        /// </summary>
        public async Task<int> GetById_Count(string Apt_Code, int intYear, int intMonth)
        {
            using var db = new SqlConnection(_db.GetConnectionString("sw_togather"));
            return await db.QuerySingleOrDefaultAsync<int>("Select Count(*) From UsageDetails Where Apt_Code = @Apt_Code And intYear = @intYear And intMonth = @intMonth", new { Apt_Code, intYear, intMonth });
        }

        /// <summary>
        /// 상세정보 모두 목록
        /// </summary>
        public async Task<List<UsageDetails_Entity>> GetListAll(int Page)
        {
            using var db = new SqlConnection(_db.GetConnectionString("sw_togather"));
            var lst = await db.QueryAsync<UsageDetails_Entity>("Select Top 15 * From UsageDetails Where Aid Not In (Select Top(15 * @Page) Aid From MonthlyUsage Order by Aid Desc) Order by Aid Desc", new { Page });
            return lst.ToList();
        }

        /// <summary>
        /// 상세정보 모두 목록 수
        /// </summary>
        public async Task<int> GetListAll_Count()
        {
            using var db = new SqlConnection(_db.GetConnectionString("sw_togather"));
            return await db.QuerySingleOrDefaultAsync<int>("Select Count(*) From UsageDetails");
        }

        /// <summary>
        /// 해당 공동주택 월 사용량 상세정보 목록
        /// </summary>
        public async Task<List<UsageDetails_Entity>> GetList(string Apt_Code, int Year, int Month)
        {
            using var db = new SqlConnection(_db.GetConnectionString("sw_togather"));
            var lst = await db.QueryAsync<UsageDetails_Entity>("Select * From UsageDetails Where Apt_Code = @Apt_Code And intYear = @Year And intMonth = @Month Order by Aid Desc", new { Apt_Code, Year, Month });
            return lst.ToList();
        }

        /// <summary>
        /// 해당 공동주택 월 사용량 상세정보 목록
        /// </summary>
        public async Task<List<UsageDetails_Entity>> GetList_sort(string Apt_Code, int Year, int Month, string Division)
        {
            using var db = new SqlConnection(_db.GetConnectionString("sw_togather"));
            var lst = await db.QueryAsync<UsageDetails_Entity>("Select * From UsageDetails Where Apt_Code = @Apt_Code And intYear = @Year And intMonth = @Month And Division = @Division Order by Aid Desc", new { Apt_Code, Year, Month, Division });
            return lst.ToList();
        }

        /// <summary>
        /// 해당 공동주택 월 사용량 상세정보 목록 존재 여부
        /// </summary>
        public async Task<int> GetList_Count(string Apt_Code, int Year, int Month)
        {
            using var db = new SqlConnection(_db.GetConnectionString("sw_togather"));
            return await db.QuerySingleOrDefaultAsync<int>("Select Count(*) From UsageDetails Apt_Code = @Apt_Code And intYear = @Year And intMonth = @intMonth", new { Apt_Code, Year, Month });
        }
    }
}
