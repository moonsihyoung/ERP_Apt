using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Erp_Apt_Lib.MaintenanceCost
{
    public class CostDebit_Lib : ICostDebit_Lib
    {
        private readonly IConfiguration _db;
        public CostDebit_Lib(IConfiguration configuration)
        {
            this._db = configuration;
        }

        /// <summary>
        /// 신규 입력
        /// </summary>
        public async Task Add(CostDebit_Entity ann)
        {
            using var db = new SqlConnection(_db.GetConnectionString("sw_togather"));
            var sql = "Insert Into CostDebit (Apt_Code, Apt_Name, Month, dong, ho, Name, innerDate, supplyArea, longRepairDue, premiumDue, delegateCdue, electionMdue, parkingDue, electricPcharges, electricCcharges, elevatorExpenseDue, elevatorEcharges, tvFee, id_number, electricChargeDue, waterPrates, waterCrates, electricUse, hotWaterUse, heatingUse, gasUse, facilityCdue, suspenseReciept, workSdeduction, costMdeduction, discountSum, waterUse, generalDue, cleaningDue, securityDue, directDebit, disinfectionDue, repairDue, trustDue, maintenanceFeeSum, useFeeSum, unpaidSum, monthTotalSum, extendDue, unpaidOverdue, afterOverdue, afterExtendDue) values (@Apt_Code, @Apt_Name, @Month, @dong, @ho, @Name, @innerDate, @supplyArea, @longRepairDue, @premiumDue, @delegateCdue, @electionMdue, @parkingDue, @electricPcharges, @electricCcharges, @elevatorExpenseDue, @elevatorEcharges, @tvFee, @id_number, @electricChargeDue, @waterPrates, @waterCrates, @electricUse, @hotWaterUse, @heatingUse, @gasUse, @facilityCdue, @suspenseReciept, @workSdeduction, @costMdeduction, @discountSum, @waterUse, @generalDue, @cleaningDue, @securityDue, @directDebit, @disinfectionDue, @repairDue, @trustDue, @maintenanceFeeSum, @useFeeSum, @unpaidSum, @monthTotalSum, @extendDue, @unpaidOverdue, @afterOverdue, @afterExtendDue)";
            await db.ExecuteAsync(sql, ann);
        }

        /// <summary>
        /// 전체 목록
        /// </summary>
        public async Task<List<CostDebit_Entity>> GetList(int Page)
        {
            using var db = new SqlConnection(_db.GetConnectionString("sw_togather"));
            var lst = await db.QueryAsync<CostDebit_Entity>("Select Top 15 * From CostDebit Where Aid Not In (Select Top(15 * @Page) Aid From CostDebit Order By Aid Asc) Order by Aid Asc", new { Page });
            return lst.ToList();
        }

        /// <summary>
        /// 전체 목록 수
        /// </summary>
        public async Task<int> GetList_Count()
        {
            using var db = new SqlConnection(_db.GetConnectionString("sw_togather"));
            return await db.QuerySingleOrDefaultAsync<int>("Select Count(*) From CostDebit");
        }

        /// <summary>
        /// 전체 공동주택 목록
        /// </summary>
        public async Task<List<CostDebit_Entity>> GetList_Apt(int Page, string Apt_Code)
        {
            using var db = new SqlConnection(_db.GetConnectionString("sw_togather"));
            var lst = await db.QueryAsync<CostDebit_Entity>("Select Top 15 * From CostDebit Where Aid Not In (Select Top(15 * @Page) Aid From CostDebit Where Apt_Code = @Apt_Code Order By Aid desc) And Apt_Code = @Apt_Code Order by Aid Desc", new { Page, Apt_Code });
            return lst.ToList();
        }

        /// <summary>
        /// 전체 공동주택 목록 수
        /// </summary>
        public async Task<int> GetList_Apt_Count(string Apt_Code)
        {
            using var db = new SqlConnection(_db.GetConnectionString("sw_togather"));
            return await db.QuerySingleOrDefaultAsync<int>("Select Count(*) From CostDebit Where Apt_Code = @Apt_Code", new {Apt_Code});
        }

        /// <summary>
        /// 전체 공동주택 목록
        /// </summary>
        public async Task<List<CostDebit_Entity>> GetList_Apt_Month(int Page, string Apt_Code, string Month)
        {
            using var db = new SqlConnection(_db.GetConnectionString("sw_togather"));
            var lst = await db.QueryAsync<CostDebit_Entity>("Select Top 15 * From CostDebit Where Aid Not In (Select Top(15 * @Page) Aid From CostDebit Where Apt_Code = @Apt_Code And Month = @Month Order By Aid Asc) And Apt_Code = @Apt_Code And Month = @Month Order by Aid Asc", new { Page, Apt_Code, Month });
            return lst.ToList();
        }

        /// <summary>
        /// 전체 공동주택 목록 수
        /// </summary>
        public async Task<int> GetList_Apt_Month_Count(string Apt_Code, string Month)
        {
            using var db = new SqlConnection(_db.GetConnectionString("sw_togather"));
            return await db.QuerySingleOrDefaultAsync<int>("Select Count(*) From CostDebit Where Apt_Code = @Apt_Code And Month", new { Apt_Code, Month });
        }

        /// <summary>
        /// 전체 공동주택 목록
        /// </summary>
        public async Task<List<CostDebit_Entity>> GetList_Apt_Month_dongho(int Page, string Apt_Code, string Month, string dong, string ho)
        {
            using var db = new SqlConnection(_db.GetConnectionString("sw_togather"));
            var lst = await db.QueryAsync<CostDebit_Entity>("Select Top 15 * From CostDebit Where Aid Not In (Select Top(15 * @Page) Aid From CostDebit Where Apt_Code = @Apt_Code and Month = @Month And dong = @dong And ho = @ho Order By Aid Asc) And Apt_Code = @Apt_Code And Month = @Month And dong = @dong And ho = @ho Order by Aid Asc", new { Page, Apt_Code, Month, dong, ho });
            return lst.ToList();
        }

        /// <summary>
        /// 전체 공동주택 목록 수
        /// </summary>
        public async Task<int> GetList_Apt_Month_dongho_Count(string Apt_Code, string Month, string dong, string ho)
        {
            using var db = new SqlConnection(_db.GetConnectionString("sw_togather"));
            return await db.QuerySingleOrDefaultAsync<int>("Select Count(*) From CostDebit Where Apt_Code = @Apt_Code And Month = @Month And dong = @dong And ho = @ho", new { Apt_Code, Month, dong, ho });
        }

        /// <summary>
        /// 세대 해당월 상세정보
        /// </summary>
        public async Task<CostDebit_Entity> GetBy(string Apt_Code, string dong, string ho, string Month)
        {
            using var db = new SqlConnection(_db.GetConnectionString("sw_togather"));
            return await db.QuerySingleOrDefaultAsync<CostDebit_Entity>("Select Top 1 * From CostDebit Where Apt_Code = @Apt_Code And dong = @dong And ho = @ho And Month = @Month", new { Apt_Code, dong, ho, Month });
        }

        /// <summary>
        /// 세대 해당월 상세정보 존재 여부
        /// </summary>
        public async Task<int> GetBy_be(string Apt_Code, string dong, string ho, string Month)
        {
            using var db = new SqlConnection(_db.GetConnectionString("sw_togather"));
            return await db.QuerySingleOrDefaultAsync<int>("Select Count(*) From CostDebit Where Apt_Code = @Apt_Code And dong = @dong And ho = @ho And Month = @Month", new { Apt_Code, dong, ho, Month });
        }


        /// <summary>
        /// 전체 공동주택 동호 목록 목록
        /// </summary>
        public async Task<List<CostDebit_Entity>> GetList_Apt_dongho(int Page, string Apt_Code, string dong, string ho)
        {
            using var db = new SqlConnection(_db.GetConnectionString("sw_togather"));
            var lst = await db.QueryAsync<CostDebit_Entity>("Select Top 15 * From CostDebit Where Aid Not In (Select Top(15 * @Page) Aid From CostDebit Where Apt_Code = @Apt_Code And dong = @dong And ho = @ho Order By Aid Asc) And Apt_Code = @Apt_Code And dong = @dong And ho = @ho Order by Aid Asc", new { Page, Apt_Code, dong, ho });
            return lst.ToList();
        }

        /// <summary>
        /// 전체 공동주택 동호 목록 목록 수
        /// </summary>
        public async Task<int> GetList_Apt_dongho_Count(string Apt_Code, string dong, string ho)
        {
            using var db = new SqlConnection(_db.GetConnectionString("sw_togather"));
            return await db.QuerySingleOrDefaultAsync<int>("Select Count(*) From CostDebit Where Apt_Code = @Apt_Code And dong = @dong And ho = @ho", new { Apt_Code, dong, ho });
        }
    }
}
