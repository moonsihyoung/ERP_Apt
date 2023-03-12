using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Linq;

namespace sw_Lib.Labors
{
    public class wege_Lib : Iwage_Lib
    {
        private readonly IConfiguration _db;
        public wege_Lib(IConfiguration configuration)
        {
            this._db = configuration;
        }


        /// <summary>
        /// 최저임금 입력
        /// </summary>
        /// <param name="dm"></param>
        /// <returns></returns>
        public async Task<wage_Entity> Add(wage_Entity dm)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                await ctx.ExecuteAsync("Insert Into wage (Year, wage, Details, User_Code) Values (@Year, @wage, @Details, @User_Code)", dm, commandType: CommandType.Text);
                return dm;
            }
            
        }

        /// <summary>
        /// 최저임금 수정
        /// </summary>
        /// <param name="dm"></param>
        /// <returns></returns>
        public async Task<wage_Entity> Edit(wage_Entity dm)
        {
            using var ctx = new SqlConnection(_db.GetConnectionString("sw_togather"));
            await ctx.ExecuteAsync("Update wage Set Year = @Year, wage = @wage, Details = @Details, User_Code = @User_Code Where Aid = @Aid", dm, commandType: CommandType.Text);
            return dm;
        }

        /// <summary>
        /// 최저임금 목록 수
        /// </summary>
        /// <returns></returns>
        public async Task<int> GetList_Count()
        {
            using var ctx = new SqlConnection(_db.GetConnectionString("sw_togather"));
            return await ctx.QuerySingleOrDefaultAsync<int>("Select Count(*) From wage");
        }

        /// <summary>
        /// 최저임금 목록
        /// </summary>
        /// <returns></returns>
        public async Task<List<wage_Entity>> GetList(int Page)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                var Lsit = await ctx.QueryAsync<wage_Entity>("Select Top 15 Aid, Year, wage, PostDate, Details, User_Code From wage Where Aid Not In (Select Top (15 * @Page) Aid From wage Order By Aid Desc) Order By Aid Desc", new { Page }, commandType: CommandType.Text);
                return Lsit.ToList();
            }            
        }

        /// <summary>
        /// 최저임금 불러오기
        /// </summary>
        /// <param name="Year"></param>
        /// <returns></returns>
        public async Task<int> wage(int Year)
        {
            using var ctx = new SqlConnection(_db.GetConnectionString("sw_togather"));
            int re = 0;
            try
            {
                re = await ctx.QuerySingleOrDefaultAsync<int>("Select Top 1 wage From wage Where Year = @Year Order By Aid Desc", new { Year }, commandType: CommandType.Text);
            }
            catch (Exception)
            {
                re = 0;
            }
            return re;
        }

        /// <summary>
        /// 최저임금 정보 불러오기
        /// </summary>
        /// <param name="Year"></param>
        /// <returns></returns>
        public async Task<wage_Entity> Details(int Aid)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                return await ctx.QuerySingleOrDefaultAsync<wage_Entity>("Select Aid, Year, wage, PostDate, Details, User_Code From wage Where Aid = @Aid Order By Aid Desc", new { Aid }, commandType: CommandType.Text);
            }
            
        }
    }
}
