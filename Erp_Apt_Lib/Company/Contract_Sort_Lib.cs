using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Company
{
    /// <summary>
    /// 계약 정보 분류
    /// </summary>
    public class Contract_Sort_Lib : IContract_Sort_Lib
    {
        private readonly IConfiguration _db;
        public Contract_Sort_Lib(IConfiguration configuration)
        {
            this._db = configuration;
        }

        /// <summary>
        /// 업체 분류 입력
        /// </summary>
        /// <param name="cs"></param>
        /// <returns></returns>
        public async Task<int> Add(Contract_Sort_Entity cs)
        {
            var sql = "Insert into Contract_Sort (Apt_Code, ContractSort_Code, ContractSort_Name, Staff_Code, Up_Code, ContractSort_Step, ContractSort_Division, ContractSort_Etc) Values (@Apt_Code, @ContractSort_Code, @ContractSort_Name, @Staff_Code, @Up_Code, @ContractSort_Step, @ContractSort_Division, @ContractSort_Etc); Select Cast(SCOPE_IDENTITY() As Int);";
            using (var dba = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                var Num = await dba.QuerySingleOrDefaultAsync<int>(sql, cs);
                cs.Aid = Num;
                return Num; 
            }
        }

        /// <summary>
        /// 전체 목록
        /// </summary>
        /// <param name="Company_Sort_Step"></param>
        /// <returns></returns>
        public async Task<List<Contract_Sort_Entity>> List_All(string ContractSort_Step)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                var lst = await dba.QueryAsync<Contract_Sort_Entity>("Select * From Contract_Sort Where ContractSort_Step = @ContractSort_Step Order By Aid Asc", new { ContractSort_Step });
                return lst.ToList(); 
            }
        }

        public async Task<Contract_Sort_Entity> Detail(string ContractSort_Code)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                return await dba.QuerySingleOrDefaultAsync<Contract_Sort_Entity>("Select Top 1 * From Contract_Sort Where ContractSort_Code = @ContractSort_Code Order by Aid Desc", new { ContractSort_Code }); 
            }
        }

        /// <summary>
        /// 대분류 목록
        /// </summary>
        /// <param name="Contract_Sort_Step"></param>
        /// <returns></returns>
        public async Task<List<Contract_Sort_Entity>> List(string Up_Code)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                var lst = await dba.QueryAsync<Contract_Sort_Entity>("Select ContractSort_Code, ContractSort_Name From Contract_Sort Where Up_Code = @Up_Code Order By Aid Desc", new { Up_Code });
                return lst.ToList(); 
            }
        }

        /// <summary>
        /// 계약 및 업체 분류 전체 목록
        /// </summary>
        /// <returns></returns>
        public async Task<List<Contract_Sort_Entity>> GetLists()
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                var lst = await dba.QueryAsync<Contract_Sort_Entity>("Select Aid, Apt_Code, ContractSort_Code, ContractSort_Name, Staff_Code, Up_Code, ContractSort_Step, ContractSort_Division, ContractSort_Etc, PostDate From Contract_Sort Order By Aid Desc");
                return lst.ToList(); 
            }
        }

        /// <summary>
        /// 계약 및 업체 분류 전체 목록
        /// </summary>
        /// <returns></returns>
        public async Task<List<Contract_Sort_Entity>> GetLists_Page(int Page)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                var lst = await dba.QueryAsync<Contract_Sort_Entity>("Select Top 15 * From Contract_Sort Where Aid Not In(Select Top(15 * @Page) Aid From Contract_Sort Order By Aid Desc) Order By Aid Desc", new { Page });
                return lst.ToList();
            }
        }


        /// <summary>
        /// 계약 및 업체 분류 전체 목록
        /// </summary>
        /// <returns></returns>
        public async Task<int> GetListsCount()
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                return await dba.QuerySingleOrDefaultAsync<int>("Select Count(*) From Contract_Sort"); 
            }
        }

        /// <summary>
        /// 하위분류별 목록
        /// </summary>
        /// <param name="ContractSort_Step"></param>
        /// <param name="Up_Code"></param>
        /// <returns></returns>
        public async Task<List<Contract_Sort_Entity>> List_Code(string ContractSort_Step, string Up_Code)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                var lst = await dba.QueryAsync<Contract_Sort_Entity>("Select ContractSort_Code, ContractSort_Name From Contract_Sort Where ContractSort_Step = @ContractSort_Step And Up_Code = @Up_Code Order By Aid Desc", new { ContractSort_Step, Up_Code });
                return lst.ToList(); 
            }
        }

        /// <summary>
        /// 하위분류별 목록 수
        /// </summary>
        /// <param name="ContractSort_Step"></param>
        /// <param name="Up_Code"></param>
        /// <returns></returns>
        public async Task<int> List_Code_Count(string ContractSort_Step, string Up_Code)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                return await dba.QuerySingleOrDefaultAsync<int>("Select Count(*) From Contract_Sort Where ContractSort_Step = @ContractSort_Step And Up_Code = @Up_Code", new { ContractSort_Step, Up_Code }); 
            }
        }

        /// <summary>
        /// 분류명 불러오기
        /// </summary>
        /// <param name="Company_Sort_Code"></param>
        /// <returns></returns>
        public async Task<string> Name(string ContractSort_Code)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                return await dba.QuerySingleOrDefaultAsync<string>("Select Top 1 ContractSort_Name From Contract_Sort Where ContractSort_Code = @ContractSort_Code Order By Aid Desc", new { ContractSort_Code }); 
            }
        }

        /// <summary>
        /// 분류명 불러오기
        /// </summary>
        /// <param name="Company_Sort_Code"></param>
        /// <returns></returns>
        public string _Name(string ContractSort_Code)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                return dba.QuerySingleOrDefault<string>("Select Top 1 ContractSort_Name From Contract_Sort Where ContractSort_Code = @ContractSort_Code Order By Aid Desc", new { ContractSort_Code });
            }
        }

        /// <summary>
        /// 마지막 번호
        /// </summary>
        /// <returns></returns>
        public async Task<int> LastNumber()
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                return await dba.QuerySingleOrDefaultAsync<int>("Select Top 1 Aid From Contract_Sort Order By Aid Desc"); 
            }
        }

        /// <summary>
        /// 삭제
        /// </summary>
        /// <returns></returns>
        public async Task Remove(int Aid)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                await dba.ExecuteAsync("Delete Contract_Sort Where Aid = @Aid", new { Aid });
            }
        }
    }
}

