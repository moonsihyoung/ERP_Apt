using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Erp_Apt_Lib.Plans
{
    /// <summary>
    /// 관리 계획 관련 정보(사업계획, 안전관리계획 등)
    /// </summary>
    public class Plans_Lib : IPlan_Lib
    {
        private readonly IConfiguration _db;
        public Plans_Lib(IConfiguration configuration)
        {
            this._db = configuration;
        }

        public async Task Add(Plan_Entity plan)
        {
            using var db = new SqlConnection(_db.GetConnectionString("sw_togather"));
            await db.ExecuteAsync("Insert Into Plans (Apt_Code, User_Name, User_Code, Plan_Name, Plan_Code, Year, Month, StartDate, EndDate, Sort, Sort_Code, Asort, Asort_Code, Plan_Details, Law_Division, BloomA, BloomB, BloomC, BloomA_Code, BloomB_Code, BloomC_Code, Post, Duty, Menager, W_Post, W_Duty, Worker, Etc, PostIP) values (@Apt_Code, @User_Name, @User_Code, @Plan_Name, @Plan_Code, @Year, @Month, @StartDate, @EndDate, @Sort, @Sort_Code, @Asort, @Asort_Code, @Plan_Details, @Law_Division, @BloomA, @BloomB, @BloomC, @BloomA_Code, @BloomB_Code, @BloomC_Code, @Post, @Duty, @Menager, @W_Post, @W_Duty, @Worker, @Etc, @PostIP)", plan);
        }

        public async Task<List<Plan_Entity>> All_List(int Page)
        {
            using var db = new SqlConnection(_db.GetConnectionString("sw_togather"));
            var lst = await db.QueryAsync<Plan_Entity>("Select Top 15 * From Plans Where Aid Not In (Select Top (15  @Page) Aid From Plans Where Using = 'A' Order By Aid Desc) And Using = 'A' Order by Aid Desc", new { Page } );
            return lst.ToList();
        }

        public async Task<int> All_List_Count()
        {
            using var db = new SqlConnection(_db.GetConnectionString("sw_togather"));
            return await db.QuerySingleOrDefaultAsync<int>("Select Count(*) From Plans Where Using = 'A'");
        }

        public async Task<List<Plan_Entity>> Apt_List(int Page, string Apt_Code)
        {
            using var db = new SqlConnection(_db.GetConnectionString("sw_togather"));
            var lst = await db.QueryAsync<Plan_Entity>("Select Top 15 * From Plans Where Aid Not In (Select Top (15 * @Page) Aid From Plans Where Apt_Code = @Apt_Code And Using = 'A' Order By Aid Desc) And Apt_Code = @Apt_Code And Using = 'A' Order by Aid Desc", new { Page, Apt_Code} );
            return lst.ToList();
        }

        public async Task<int> Apt_List_Count(string Apt_Code)
        {
            using var db = new SqlConnection(_db.GetConnectionString("sw_togather"));
            return await db.QuerySingleOrDefaultAsync<int>("Select Count(*) From Plans Where Apt_Code = @Apt_Code And Using = 'A'", new {Apt_Code});
        }

        public async Task<List<Plan_Entity>> ASort_List(int Page, string Apt_Code, string Sort_Code, string Asort_Code)
        {
            using var db = new SqlConnection(_db.GetConnectionString("sw_togather"));
            var lst = await db.QueryAsync<Plan_Entity>("Select Top 15 * From Plans Where Aid Not In (Select Top (15  @Page) Aid From Plans Where Apt_Code = @Apt_Code And Sort_Code = @Sort_Code And Asort_Code = @Asort_Code And Using = 'A' Order By Aid Desc) and Apt_Code = @Apt_Code And Sort_Code = @Sort_Code And Asort_Code = @Asort_Code And Using = 'A' Order by Aid Desc", new { Page, Apt_Code, Sort_Code, Asort_Code} );
            return lst.ToList();
        }

        public async Task<int> Asort_List_Count(string Apt_Code, string Sort_Code, string Asort_Code)
        {
            using var db = new SqlConnection(_db.GetConnectionString("sw_togather"));
            return await db.QuerySingleOrDefaultAsync<int>("Select Count(*) From Plans Whehe Apt_Code = @Apt_Code And Sort_Code = @Sort_Code And Asort = @Asort And Using = 'A'", new {Apt_Code, Sort_Code, Asort_Code});
        }

        public async Task Delete(int Aid)
        {
            using var db = new SqlConnection(_db.GetConnectionString("sw_togather"));
            await db.ExecuteAsync("Update Plans Set Using = 'B' Where Aid = @Aid", new { Aid });
        }

        public async Task<Plan_Entity> Details(int Aid)
        {
            using var db = new SqlConnection(_db.GetConnectionString("sw_togather"));
            return await db.QuerySingleOrDefaultAsync<Plan_Entity>("Select * From Plans Where Aid = @Aid", new { Aid });
        }

        public async Task<List<Plan_Entity>> Sort_List(int Page, string Apt_Code, string Sort_Code)
        {
            using var db = new SqlConnection(_db.GetConnectionString("sw_togather"));
            var lst = await db.QueryAsync<Plan_Entity>("Select Top 15 * From Plans Where Aid Not In (Select Top (15  @Page) Aid From Plans Where Apt_Code = @Apt_Code And Sort_Code = @Sort_Code And Using = 'A' Order By Aid Desc) and Apt_Code = @Apt_Code And Sort_Code = @Sort_Code And Using = 'A' Order by Aid Desc", new { Page, Apt_Code, Sort_Code });
            return lst.ToList();
        }

        public async Task<int> Sort_List_Count(string Apt_Code, string Sort_Code)
        {
            using var db = new SqlConnection(_db.GetConnectionString("sw_togather"));
            return await db.QuerySingleOrDefaultAsync<int>("Select Count(*) From Plans Where Apt_Code = @Apt_Code And Sort_Code = @Sort_Code And Using = 'A'");
        }

        public async Task Update(Plan_Entity plan)
        {
            using var db = new SqlConnection(_db.GetConnectionString("sw_togather"));
            await db.ExecuteAsync("Update Plans Set User_Name = @User_Name, User_Code = @User_Code, Plan_Name = @Plan_Name, Plan_Code = @Plan_Code, Year = @Year, Month = @Month, StartDate = @StartDate, EndDate = @EndDate, Sort = @Sort, Sort_Code = @Sort_Code, Asort = @Asort, Asort_Code = @Asort_Code, Plan_Details = @Plan_Details, Etc = @Etc, PostDate = '" + DateTime.Now.ToShortDateString() + "', PostIP = @PostIP, BloomA = @BloomA, BloomB = @BloomB, BloomC = @BloomC, BloomA_Code = @BloomA_Code, BloomB_Code = @BloomB_Code, BloomC_Code = @BloomC_Code, Post = @Post, Duty = @Duty, Menager = @Menager, W_Post = @W_Post, W_Duty = @W_Duty, Worker = @Worker Where Aid = @Aid", plan);
        }
    }

    /// <summary>
    /// 계획 관련 분류
    /// </summary>
    public class Plan_Sort_Lib : IPlan_Sort_Lib
    {
        private readonly IConfiguration _db;
        public Plan_Sort_Lib(IConfiguration configuration)
        {
            this._db = configuration;
        }
        public async Task Add(Plan_Sort_Entity plan)
        {
            using var db = new SqlConnection(_db.GetConnectionString("sw_togather"));
            await db.ExecuteAsync("Insert Into Plans_Sort (Apt_Code, Sort_Name, Sort_Code, Asort_Code, Law_Division, Details, User_Code, PostIP, Division) values (@Apt_Code, @Sort_Name, @Sort_Code, @Asort_Code, @Law_Division, @Details, @User_Code, @PostIP, @Division)", plan);
        }

        public async Task<List<Plan_Sort_Entity>> Apt_AsortList(string Apt_Code, string Sort_Code)
        {
            using var db = new SqlConnection(_db.GetConnectionString("sw_togather"));
            var lst = await db.QueryAsync<Plan_Sort_Entity>("Select * From Plans_Sort Where Apt_Code = @Apt_Code And Sort_Code = @Sort_Code And Division = 'B' And Using = 'A' Order By Aid Desc", new { Apt_Code, Sort_Code });
            return lst.ToList();
        }

        public async Task<List<Plan_Sort_Entity>> Apt_SortList(string Apt_Code)
        {
            using var db = new SqlConnection(_db.GetConnectionString("sw_togather"));
            var lst = await db.QueryAsync<Plan_Sort_Entity>("Select * From Plans_Sort Where Apt_Code = @Apt_Code And Division = 'A' And Using = 'A' Order By Aid Desc", new { Apt_Code });
            return lst.ToList();
        }

        public async Task<List<Plan_Sort_Entity>> AsortList(string Sort_Code)
        {
            using var db = new SqlConnection(_db.GetConnectionString("sw_togather"));
            var lst = await db.QueryAsync<Plan_Sort_Entity>("Select * From Plans_Sort Where Sort_Code = @Sort_Code And Division = 'B' And Using = 'A' Order By Aid Desc", new { Sort_Code });
            return lst.ToList();
        }

        public async Task Delete(int Aid)
        {
            using var db = new SqlConnection(_db.GetConnectionString("sw_togather"));
            await db.ExecuteAsync("Update Plans_Sort Set Using = 'A' Where Aid = @Aid", new { Aid });
        }

        public async Task<Plan_Sort_Entity> Details(int Aid)
        {
            using var db = new SqlConnection(_db.GetConnectionString("sw_togather"));
            return await db.QuerySingleOrDefaultAsync<Plan_Sort_Entity>("Select * From Plans_Sort Where Aid = @Aid", new { Aid });
        }

        public async Task<Plan_Sort_Entity> Sort_Details(string Sort_Code)
        {
            using var db = new SqlConnection(_db.GetConnectionString("sw_togather"));
            return await db.QuerySingleOrDefaultAsync<Plan_Sort_Entity>("Select Top 1 * From Plans_Sort Where Sort_Code = @Sort_Code And Division = 'A' Order by Aid Asc", new { Sort_Code });
        }

        /// <summary>
        /// 계획분류 대분류 목록
        /// </summary>
        public async Task<List<Plan_Sort_Entity>> SortList()
        {
            using var db = new SqlConnection(_db.GetConnectionString("sw_togather"));
            var lst = await db.QueryAsync<Plan_Sort_Entity>("Select * From Plans_Sort Where Division = 'A' And Using = 'A' Order By Aid Desc");
            return lst.ToList();
        }

        /// <summary>
        /// 계획분류 전체 목록
        /// </summary>
        public async Task<List<Plan_Sort_Entity>> SortList_All()
        {
            using var db = new SqlConnection(_db.GetConnectionString("sw_togather"));
            var lst = await db.QueryAsync<Plan_Sort_Entity>("Select * From Plans_Sort Where Using = 'A' Order By Aid Desc");
            return lst.ToList();
        }

        /// <summary>
        /// 계획 분류 수정
        /// </summary>
        public async Task Update(Plan_Sort_Entity plan)
        {
            using var db = new SqlConnection(_db.GetConnectionString("sw_togather"));
            await db.ExecuteAsync("Update Plans_Sort Set Apt_Code = @Apt_Code, Sort_Name = @Sort_Name, Sort_Code = @Sort_Code, Asort_Code = @Asort_Code, Law_Division = @Law_Division, Details = @Details, User_Code = @User_Code, PostDate = " + DateTime.Now + ", PostIP = @PostIP, Division = @Division Where Aid = @Aid", plan);
        }

        public async Task<string> SortNaneAsync(string Sort_Code)
        {
            using var db = new SqlConnection(_db.GetConnectionString("sw_togather"));
            return await db.QuerySingleOrDefaultAsync<string>("Select Top 1 Sort_Name From Plans_Sort Where Sort_Code = @Sort_Code And Division = 'A' Order By Aid Asc", new { Sort_Code });
        }

        public string SortNane(string Sort_Code)
        {
            using var db = new SqlConnection(_db.GetConnectionString("sw_togather"));
            return db.QuerySingleOrDefault<string>("Select Sort_Name From Plans_Sort Where Sort_Code = @Sort_Code", new { Sort_Code });
        }

        public async Task<string> AsortNaneAsync(string Sort_Code)
        {
            using var db = new SqlConnection(_db.GetConnectionString("sw_togather"));
            return await db.QuerySingleOrDefaultAsync<string>("Select Sort_Name From Plans_Sort Where Asort_Code = @Sort_Code", new { Sort_Code });
        }

        public string AsortNane(string Sort_Code)
        {
            using var db = new SqlConnection(_db.GetConnectionString("sw_togather"));
            return db.QuerySingleOrDefault<string>("Select Sort_Name From Plans_Sort Where Asort_Code = @Sort_Code", new { Sort_Code });
        }

        public async Task<int> Last_Aid()
        {
            using var db = new SqlConnection(_db.GetConnectionString("sw_togather"));
            return await db.QuerySingleOrDefaultAsync<int>("Select Top 1 Aid From Plans_Sort Order by Aid Desc");
        }
    }

    /// <summary>
    /// 계획관리자 정보
    /// </summary>
    public class Plan_Man_Lib : IPlan_Man_Lib
    {
        private readonly IConfiguration _db;
        public Plan_Man_Lib(IConfiguration configuration)
        {
            this._db = configuration;
        }
        /// <summary>
        /// 계획 관리자 입력
        /// </summary>
        /// <param name="plan"></param>
        /// <returns></returns>
        public async Task Add(Plan_Man_Entity plan)
        {
            using var db = new SqlConnection(_db.GetConnectionString("sw_togather"));
            await db.ExecuteAsync("Insert Into Plans_Man (Apt_Code, Plan_Code, Post, Duty, User_Code, Divsion, PostIP) values (@Apt_Code, @Plan_Code, @Post, @Duty, @Division, @PostIP)", plan);
        }

        /// <summary>
        /// 삭제
        /// </summary>
        /// <param name="Aid"></param>
        /// <returns></returns>
        public async Task Delete(int Aid)
        {
            using var db = new SqlConnection(_db.GetConnectionString("sw_togather"));
            await db.ExecuteAsync("Update Plans_Man Set Using = 'B' Where Aid = @Aid", new {Aid });
        }

        /// <summary>
        /// 상세보기
        /// </summary>
        /// <param name="Aid"></param>
        /// <returns></returns>
        public async Task<Plan_Man_Entity> Details(int Aid)
        {
            using var db = new SqlConnection(_db.GetConnectionString("sw_togather"));
            return await db.QuerySingleOrDefaultAsync<Plan_Man_Entity>("Select * From Plans_Man Where Aid = @Aid", new { Aid });
        }

        /// <summary>
        /// 관리자 정보 목록
        /// </summary>
        public async Task<List<Plan_Man_Entity>> GetList(string Plan_Code, string Division)
        {
            using var db = new SqlConnection(_db.GetConnectionString("sw_togather"));
            var lst = await db.QueryAsync<Plan_Man_Entity>("Select * From Plans_Man Where Plan_Code = @Plan_Code And Division = @Division And Using = 'A' Order By Aid Desc", new { Plan_Code, Division });
            return lst.ToList();
        }

        /// <summary>
        /// 관리자 수정
        /// </summary>
        /// <param name="plan"></param>
        /// <returns></returns>
        public async Task Update(Plan_Man_Entity plan)
        {
            using var db = new SqlConnection(_db.GetConnectionString("sw_togather"));
            await db.ExecuteAsync("Update Plans_Man Set Apt_Code = @Apt_Code, Plan_Code = @Plan_Code, Post = @Post, Duty = @Duty, User_Code = @User_Code, User_Name = @User_Name, PostDate = " + DateTime.Now + ", PostIP = @PostIP, Division = @Division Where Aid = @Aid", plan);
        }
    }
}
