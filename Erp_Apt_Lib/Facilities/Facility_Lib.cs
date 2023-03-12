using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using System.Linq;

namespace Facilities
{
    public class Facility_Lib : IFacility_Lib
    {
        private readonly IConfiguration _db;
        public Facility_Lib(IConfiguration configuration)
        {
            this._db = configuration;
        }

        /// <summary>
        /// 시설물 정보 입력
        /// </summary>
        /// <param name="fac"></param>
        /// <returns></returns>
        public async Task<int> Add(Facility_Entity fac)
        {
            var sql = "Insert Facility (Apt_Code, Apt_Name, Facility_Name, Sort_A_Name, Sort_A_Code, Sort_B_Name, Sort_B_Code, Sort_C_Name, Sort_C_Code, Position, Manufacture, Manufacture_Telephone, Manufacture_Menager, Quantity, Price, Standard, Capacity, Model, Explanation, Menual, Installation_Date, PostIP) Values (@Apt_Code, @Apt_Name, @Facility_Name, @Sort_A_Name, @Sort_A_Code, @Sort_B_Name, @Sort_B_Code, @Sort_C_Name, @Sort_C_Code, @Position, @Manufacture, @Manufacture_Telephone, @Manufacture_Menager, @Quantity, @Price, @Standard, @Capacity, @Model, @Explanation, @Menual, @Installation_Date, @PostIP); Select Cast(SCOPE_IDENTITY() As Int);";
            using var db = new SqlConnection(_db.GetConnectionString("sw_togather"));
            var id = await db.QuerySingleOrDefaultAsync<int>(sql, fac);
            //fac.Aid = id;
            return id;
        }

        /// <summary>
        /// 시설물 정보 수정
        /// </summary>
        /// <param name="fac"></param>
        public async Task Edit(Facility_Entity fac)
        {
            var sql = "Update Facility Set Facility_Name = @Facility_Name, Sort_A_Name = @Sort_A_Name, Sort_A_Code = @Sort_A_Code, Sort_B_Name = @Sort_B_Name, Sort_B_Code = @Sort_B_Code, Sort_C_Name = @Sort_C_Name, Sort_C_Code = @Sort_C_Code, Position= @Position, Manufacture = @Manufacture, Manufacture_Telephone = @Manufacture_Telephone, Manufacture_Menager = @Manufacture_Menager, Quantity= @Quantity, Price= @Price, Standard = @Standard, Capacity = @Capacity, Model = @Model, Explanation = @Explanation, Menual = @Menual, Installation_Date = @Installation_Date, PostIP = @PostIP Where Aid = @Aid";
            using var db = new SqlConnection(_db.GetConnectionString("sw_togather"));
            await db.ExecuteAsync(sql, fac);
        }

        /// <summary>
        /// 공동주택코드로 목록만들기
        /// </summary>
        /// <param name="Apt_Code"></param>
        /// <returns></returns>
        public async Task<List<Facility_Entity>> GetList_Apt(int Page, string Apt_Code)
        {
            using var db = new SqlConnection(_db.GetConnectionString("sw_togather"));
            var lst = await db.QueryAsync<Facility_Entity>("Select Top 15 Aid, Apt_Code, Apt_Name, Facility_Name, Sort_A_Name, Sort_A_Code, Sort_B_Name, Sort_B_Code, Sort_C_Name, Sort_C_Code, Position, Manufacture, Manufacture_Telephone, Manufacture_Menager, Quantity, Price, Standard, Capacity, Model, Explanation, Menual, Installation_Date, Remove_Date, PostDate, PostIP From Facility Where Aid Not In(Select Top (15 * @Page) Aid From Facility Where Apt_Code = @Apt_Code) And Apt_Code = @Apt_Code", new { Page, Apt_Code });
            return lst.ToList();
        }

        /// <summary>
        /// 공동주택식별코드로 목록 수 구하기
        /// </summary>
        /// <param name="Apt_Code"></param>
        /// <returns></returns>
        public async Task<int> GetList_Apt_Count(string Apt_Code)
        {
            using var db = new SqlConnection(_db.GetConnectionString("sw_togather"));
            return await db.QuerySingleOrDefaultAsync<int>("Select Count(*) From Facility Where Apt_Code = @Apt_Code", new { Apt_Code });
        }

        /// <summary>
        /// 해당 공동주택에 대분류로 검색된 목록
        /// </summary>
        /// <param name="Apt_Code"></param>
        /// <returns></returns>
        public async Task<List<Facility_Entity>> GetList_Apt_SortA(int Page, string Apt_Code, string Sort_A_Code)
        {
            using var db = new SqlConnection(_db.GetConnectionString("sw_togather"));
            var lst = await db.QueryAsync<Facility_Entity>("Select Top 15 Aid, Apt_Code, Apt_Name, Facility_Name, Sort_A_Name, Sort_A_Code, Sort_B_Name, Sort_B_Code, Sort_C_Name, Sort_C_Code, Position, Manufacture, Manufacture_Telephone, Manufacture_Menager, Quantity, Price, Standard, Capacity, Model, Explanation, Menual, Installation_Date, Remove_Date, PostDate, PostIP From Facility Where Aid Not In(Select Top (15 * @Page) Aid From Facility Where Apt_Code = @Apt_Code And Sort_A_Name = @Sort_A_Code) And Apt_Code = @Apt_Code And Sort_A_Name = @Sort_A_Code", new { Page, Apt_Code, Sort_A_Code });
            return lst.ToList();
        }

        /// <summary>
        /// 공동주택식별코드로 목록 수 구하기
        /// </summary>
        /// <param name="Apt_Code"></param>
        /// <returns></returns>
        public async Task<int> GetList_Apt_SortA_Count(string Apt_Code, string Sort_A_Code)
        {
            using var db = new SqlConnection(_db.GetConnectionString("sw_togather"));
            return await db.QuerySingleOrDefaultAsync<int>("Select Count(*) From Facility Where Apt_Code = @Apt_Code And Sort_A_Name = @Sort_A_Code", new { Apt_Code, Sort_A_Code });
        }

        /// <summary>
        /// 해당 공동주택에 중분류로 검색된 목록
        /// </summary>
        /// <param name="Apt_Code"></param>
        /// <returns></returns>
        public async Task<List<Facility_Entity>> GetList_Apt_SortB(int Page, string Apt_Code, string Sort_A_Code, string Sort_B_Code)
        {
            using var db = new SqlConnection(_db.GetConnectionString("sw_togather"));
            var lst = await db.QueryAsync<Facility_Entity>("Select Top 15 Aid, Apt_Code, Apt_Name, Facility_Name, Sort_A_Name, Sort_A_Code, Sort_B_Name, Sort_B_Code, Sort_C_Name, Sort_C_Code, Position, Manufacture, Manufacture_Telephone, Manufacture_Menager, Quantity, Price, Standard, Capacity, Model, Explanation, Menual, Installation_Date, Remove_Date, PostDate, PostIP From Facility Where Aid Not In(Select Top (15 * @Page) Aid From Facility Where Apt_Code = @Apt_Code And Sort_A_Name = @Sort_A_Code And Sort_B_Name = @Sort_B_Code) And Apt_Code = @Apt_Code And Sort_A_Name = @Sort_A_Code And Sort_B_Name = @Sort_B_Code", new { Page, Apt_Code, Sort_A_Code, Sort_B_Code });
            return lst.ToList();
        }

        /// <summary>
        /// 공동주택식별코드로 목록 수 구하기
        /// </summary>
        /// <param name="Apt_Code"></param>
        /// <returns></returns>
        public async Task<int> GetList_Apt_SortB_Count(string Apt_Code, string Sort_A_Code, string Sort_B_Code)
        {
            using var db = new SqlConnection(_db.GetConnectionString("sw_togather"));
            return await db.QuerySingleOrDefaultAsync<int>("Select Count(*) From Facility Where Apt_Code = @Apt_Code And Sort_A_Name = @Sort_A_Code And Sort_B_Name = @Sort_B_Code", new { Apt_Code, Sort_A_Code, Sort_B_Code });
        }

        /// <summary>
        /// 해당 공동주택에 소분류로 검색된 목록
        /// </summary>
        /// <param name="Apt_Code"></param>
        /// <returns></returns>
        public async Task<List<Facility_Entity>> GetList_Apt_SortC(int Page, string Apt_Code, string Sort_A_Code, string Sort_B_Code, string Sort_C_Code)
        {
            using var db = new SqlConnection(_db.GetConnectionString("sw_togather"));
            var lst = await db.QueryAsync<Facility_Entity>("Select Top 15 Aid, Apt_Code, Apt_Name, Facility_Name, Sort_A_Name, Sort_A_Code, Sort_B_Name, Sort_B_Code, Sort_C_Name, Sort_C_Code, Position, Manufacture, Manufacture_Telephone, Manufacture_Menager, Quantity, Price, Standard, Capacity, Model, Explanation, Menual, Installation_Date, Remove_Date, PostDate, PostIP From Facility Where Aid Not In(Select Top (15 * @Page) Aid From Facility Where Apt_Code = @Apt_Code And Sort_A_Name = @Sort_A_Code And Sort_B_Name = @Sort_B_Code And Sort_C_Name = @Sort_C_Code) And Apt_Code = @Apt_Code And Sort_A_Name = @Sort_A_Code And Sort_B_Name = @Sort_B_Code And Sort_C_Name = @Sort_C_Code", new { Page, Apt_Code, Sort_A_Code, Sort_B_Code, Sort_C_Code });
            return lst.ToList();
        }

        /// <summary>
        /// 공동주택식별코드로 목록 수 구하기
        /// </summary>
        /// <param name="Apt_Code"></param>
        /// <returns></returns>
        public async Task<int> GetList_Apt_SortC_Count(string Apt_Code, string Sort_A_Code, string Sort_B_Code, string Sort_C_Code)
        {
            using var db = new SqlConnection(_db.GetConnectionString("sw_togather"));
            return await db.QuerySingleOrDefaultAsync<int>("Select Count(*) From Facility Where Apt_Code = @Apt_Code And Sort_A_Name = @Sort_A_Code And Sort_B_Name = @Sort_B_Code And Sort_C_Name = @Sort_C_Code", new { Apt_Code, Sort_A_Code, Sort_B_Code, Sort_C_Code });
        }

        /// <summary>
        /// 시설물 정보 상세
        /// </summary>
        public async Task<Facility_Entity> Detail(string Apt_Code, string Aid)
        {
            using var db = new SqlConnection(_db.GetConnectionString("sw_togather"));
            return await db.QuerySingleOrDefaultAsync<Facility_Entity>("Select Aid, Apt_Code, Apt_Name, Facility_Name, Sort_A_Name, Sort_A_Code, Sort_B_Name, Sort_B_Code, Sort_C_Name, Sort_C_Code, Position, Manufacture, Manufacture_Telephone, Manufacture_Menager, Quantity, Price, Standard, Capacity, Model, Explanation, Menual, Installation_Date, Remove_Date, PostDate, PostIP From Facility Where Apt_Code = @Apt_Code And Aid = @Aid", new { Apt_Code, Aid });
        }

        /// <summary>
        /// 해당 시설물 정보 삭제
        /// </summary>
        /// <param name="Aid"></param>
        public async Task Remove(string Aid)
        {
            using var db = new SqlConnection(_db.GetConnectionString("sw_togather"));
            await db.ExecuteAsync("Delete Facility Where Aid = Aid", new { Aid });
        }
    }
}
