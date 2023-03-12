using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using System.Linq;
using System.Reflection.Emit;

namespace Facilities
{
    /// <summary>
    /// 시설물 분류 클래스
    /// </summary>
    public class Bloom_Lib : IBloom_Lib
    {
        private readonly IConfiguration _db;
        public Bloom_Lib(IConfiguration configuration)
        {
            this._db = configuration;
        }

        /// <summary>
        /// 시설물 분류 저장
        /// </summary>
        public async Task Add(Bloom_Entity bn)
        {
            var sql = "Insert Bloom (AptCode, Apt_Name, B_N_A_Name, B_N_B_Name, B_N_C_Name, BloomA, BloomB, B_N_Code, Bloom, Bloom_Code, Period, intro, PostIP, UserCode) Values (@AptCode, @Apt_Name, @B_N_A_Name, @B_N_B_Name, @B_N_C_Name, @BloomA, @BloomB, @B_N_Code, @Bloom, @Bloom_Code, @Period, @intro, @PostIP, @UserCode)";
            using var dba = new SqlConnection(_db.GetConnectionString("sw_togather"));
            await dba.ExecuteAsync(sql, bn);
        }

        /// <summary>
        /// 시설물 분류 수정
        /// </summary>
        /// <param name="bn"></param>
        public async Task Edit(Bloom_Entity bn)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                await dba.ExecuteAsync("Update Bloom Set B_N_A_Name = @B_N_A_Name, B_N_B_Name = @B_N_B_Name, B_N_C_Name = @B_N_C_Name, BloomA = @BloomA, BloomB = @BloomB, B_N_Code = @B_N_Code, Bloom = @Bloom, Bloom_Code = @Bloom_Code, Period = @Period, intro = @intro, ModifyDate = @ModifyDate, ModifyIP = @ModifyIP Where Num = @Num", bn);
            }
        }


        /// <summary>
        /// 시설물 분류 수정(Ajax)
        /// </summary>
        /// <param name="bn"></param>
        public async Task Update(Bloom_Entity bn)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                await dba.ExecuteAsync("Update Bloom Set Bloom = @Bloom, intro = @intro, ModifyDate = @ModifyDate, ModifyIP = @ModifyIP Where Num = " +
                    "@Num", bn);
            }
        }

        /// <summary>
        /// 작업장소 불러오기
        /// </summary>
        public async Task<string> Position_Name(string Code)
        {
            using var dba = new SqlConnection(_db.GetConnectionString("sw_togather"));
            return await dba.QuerySingleOrDefaultAsync<string>("Select Bloom From Bloom Where Views = 'A' And Num = @Code", new { Code });
        }

        /// <summary>
        /// 시설물 분류 전체 목록
        /// </summary>
        /// <returns></returns>
        public async Task<List<Bloom_Entity>> GetList(int Page)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                var lst = await dba.QueryAsync<Bloom_Entity>("Select top 15 * From Bloom Where Num Not In(Select Top(15 * @Page) Num From Bloom Order By Num Desc) Order By Num Desc", new { Page });
                return lst.ToList();
            }
        }

        //Select Top 15 * FROM Appeal_Sort Where Num Not In(Select Top(15 * @Page) Num From Appeal_Sort Order By Sort Asc) Order by Sort Asc

        /// <summary>
        /// 시설물 분류 전체 목록
        /// </summary>
        /// <returns></returns>
        public async Task<int> GetListCount()
        {
            using var dba = new SqlConnection(_db.GetConnectionString("sw_togather"));
            return await dba.QuerySingleOrDefaultAsync<int>("Select Count(*) From Bloom");
        }

        /// <summary>
        /// 시설물 분류 해당 공동주택 전체 목록
        /// </summary>
        /// <returns></returns>
        public async Task<List<Bloom_Entity>> GetList_Apt(int Page, string AptCode)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                var lst = await dba.QueryAsync<Bloom_Entity>("Select Top 15 Num, AptCode, Apt_Name, B_N_A_Name, B_N_B_Name, B_N_C_Name, BloomA, BloomB, B_N_Code, Bloom, Bloom_Code, Period, intro, ModifyDate, ModifyIP, PostDate, PostIP From Bloom Where Num Not In (Select Top (15 * @Page) Num From Bloom Where Views = 'A' And AptCode = @AptCode Order By Num Desc) And Views = 'A' And AptCode = @AptCode Order by Num desc", new { Page, AptCode });
                return lst.ToList();
            }
        }

        /// <summary>
        /// 해당 공동주택의 장소가 입력된 수
        /// </summary>
        public async Task<int> GetList_Apt_Count(string AptCode)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                return await dba.QuerySingleOrDefaultAsync<int>("Select Count(*) From Bloom Where AptCode = @AptCode and Views = 'A'", new { AptCode });
                
            }
        }

        /// <summary>
        /// 시설물 분류 해당 공동주택 전체 목록(기존)
        /// </summary>
        /// <returns></returns>
        public async Task<List<Bloom_Entity>> GetList_Apt_a(string AptCode)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                var lst = await dba.QueryAsync<Bloom_Entity>("Select Num, AptCode, Apt_Name, B_N_A_Name, B_N_B_Name, B_N_C_Name, BloomA, BloomB, B_N_Code, Bloom, Bloom_Code, Period, intro, ModifyDate, ModifyIP, PostDate, PostIP From Bloom Where Views = 'A' Order By Num Desc", new { AptCode });
                return lst.ToList();
            }
        }

        /// <summary>
        /// 시설물 분류 해당 공동주택 전체 목록
        /// </summary>
        /// <returns></returns>
        public async Task<List<Bloom_Entity>> GetList_Apt_b(string AptCode)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                var lst = await dba.QueryAsync<Bloom_Entity>("Select Num, AptCode, Apt_Name, B_N_A_Name, B_N_B_Name, B_N_C_Name, BloomA, BloomB, B_N_Code, Bloom, Bloom_Code, Period, intro, ModifyDate, ModifyIP, PostDate, PostIP From Bloom Where Views = 'A'", new { AptCode });
                return lst.ToList();
            }
        }

        /// <summary>
        /// 시설물 분류 해당 공동주택 전체 목록(대분류)
        /// </summary>
        /// <returns></returns>
        public async Task<List<Bloom_Entity>> GetList_Apt_ba(string AptCode)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                var lst = await dba.QueryAsync<Bloom_Entity>("Select Num, AptCode, Apt_Name, B_N_A_Name, B_N_B_Name, B_N_C_Name, BloomA, BloomB, B_N_Code, Bloom, Bloom_Code, Period, intro, ModifyDate, ModifyIP, PostDate, PostIP From Bloom Where Views = 'A' And Bloom_Code = 'A'", new { AptCode });
                return lst.ToList();
            }
        }

        /// <summary>
        /// 시설물 분류 해당 공동주택 전체 목록(대분류) 수
        /// </summary>
        /// <returns></returns>
        public async Task<int> GetList_Apt_ba_Count(string AptCode)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                return await dba.QuerySingleOrDefaultAsync<int>("Select Count(*) From Bloom Where Views = 'A' And Bloom_Code = 'A'", new { AptCode });
            }
        }

        /// <summary>
        /// 시설물 분류 해당 공동주택 전체 목록(중분류)
        /// </summary>
        /// <returns></returns>
        public async Task<List<Bloom_Entity>> GetList_Apt_bb(string AptCode, string B_N_A_Name)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                var lst = await dba.QueryAsync<Bloom_Entity>("Select Num, AptCode, Apt_Name, B_N_A_Name, B_N_B_Name, B_N_C_Name, BloomA, BloomB, B_N_Code, Bloom, Bloom_Code, Period, intro, ModifyDate, ModifyIP, PostDate, PostIP From Bloom Where Views = 'A' And Bloom_Code = 'B' And B_N_A_Name = @B_N_A_Name", new { AptCode, B_N_A_Name });
                return lst.ToList();
            }
        }

        /// <summary>
        /// 시설물 분류 해당 공동주택 전체 목록(장소)
        /// </summary>
        public async Task<List<Bloom_Entity>> GetList_Apt_Bloom(int Page, string AptCode, string B_N_A_Name)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                var lst = await dba.QueryAsync<Bloom_Entity>("Select top 15 Num, AptCode, Apt_Name, B_N_A_Name, B_N_B_Name, B_N_C_Name, BloomA, BloomB, B_N_Code, Bloom, Bloom_Code, Period, intro, ModifyDate, ModifyIP, PostDate, PostIP From Bloom Where Num Not In (Select Top (15 * @Page) Num From Bloom Where Views = 'A' And AptCode = @AptCode And Bloom_Code = 'D' And B_N_A_Name = @B_N_A_Name Order By Num Desc) And Views = 'A' And AptCode = @AptCode And Bloom_Code = 'D' And B_N_A_Name = @B_N_A_Name Order By Num Desc", new { Page, AptCode, B_N_A_Name });
                return lst.ToList();
            }
        }

        /// <summary>
        /// 시설물 분류 해당 공동주택 전체 목록(장소) 수
        /// </summary>
        /// <returns></returns>
        public async Task<int> GetList_Apt_Bloom_Count(string AptCode, string B_N_A_Name)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                return await dba.QuerySingleOrDefaultAsync<int>("Select Count(*) From Bloom Where Views = 'A' And AptCode = @AptCode And Bloom_Code = 'D' And B_N_A_Name = @B_N_A_Name", new { AptCode, B_N_A_Name });
            }
        }

        /// <summary>
        /// 시설물 분류 해당 공동주택 전체 목록(중분류) 수
        /// </summary>
        /// <returns></returns>
        public async Task<int> GetList_Apt_bb_Count(string AptCode, string B_N_A_Name)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                return await dba.QuerySingleOrDefaultAsync<int>("Select Count(*) From Bloom Where Views = 'A' And Bloom_Code = 'B' And B_N_A_Name = @B_N_A_Name", new { AptCode, B_N_A_Name });
            }
        }

        /// <summary>
        /// 시설물 분류 해당 공동주택 전체 목록(소분류)
        /// </summary>
        /// <returns></returns>
        public async Task<List<Bloom_Entity>> GetList_Apt_bc(string AptCode, string B_N_B_Name)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                var lst = await dba.QueryAsync<Bloom_Entity>("Select Num, AptCode, Apt_Name, B_N_A_Name, B_N_B_Name, B_N_C_Name, BloomA, BloomB, B_N_Code, Bloom, Bloom_Code, Period, intro, ModifyDate, ModifyIP, PostDate, PostIP From Bloom Where Views = 'A' And Bloom_Code = 'C' And B_N_B_Name = @B_N_B_Name", new { AptCode, B_N_B_Name });
                return lst.ToList();
            }
        }

        /// <summary>
        /// 시설물 분류 해당 공동주택 전체 목록(소분류)
        /// </summary>
        /// <returns></returns>
        public async Task<int> GetList_Apt_bc_Count(string AptCode, string B_N_B_Name)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                return await dba.QuerySingleOrDefaultAsync<int>("Select Count(*) From Bloom Where Views = 'A' And Bloom_Code = 'C' And B_N_B_Name = @B_N_B_Name", new { AptCode, B_N_B_Name });
            }
        }

        /// <summary>
        /// 시설물 분류 해당 공동주택 전체 목록(기본 분류)?
        /// </summary>
        /// <returns></returns>
        public async Task<List<Bloom_Entity>> GetList_Apt_c(string AptCode, string B_N_A_Name)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                var lst = await dba.QueryAsync<Bloom_Entity>("Select Num, AptCode, Apt_Name, B_N_A_Name, B_N_B_Name, B_N_C_Name, BloomA, BloomB, B_N_Code, Bloom, Bloom_Code, Period, intro, ModifyDate, ModifyIP, PostDate, PostIP From Bloom Where Views = 'A' And Bloom_Code = 'D' And B_N_A_Name = @B_N_A_Name And AptCode = @AptCode Order By Num Desc", new { AptCode, B_N_A_Name });
                return lst.ToList();
            }
        }

        /// <summary>
        /// 작업 분류 해당 공동주택 전체 목록(장소)?
        /// </summary>
        /// <returns></returns>
        public async Task<List<Bloom_Entity>> GetListBloomPlece(string AptCode)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                var lst = await dba.QueryAsync<Bloom_Entity>("Select Num, AptCode, Apt_Name, B_N_A_Name, B_N_B_Name, B_N_C_Name, BloomA, BloomB, B_N_Code, Bloom, Bloom_Code,Period,  intro, ModifyDate, ModifyIP, PostDate, PostIP From Bloom Where Views = 'A' And Bloom_Code = 'D' And AptCode = @AptCode Order By Num Desc", new { AptCode });
                return lst.ToList();
            }
        }

        /// <summary>
        /// 시설물 분류 해당 공동주택 전체 목록(기본 분류) 수
        /// </summary>
        /// <returns></returns>
        public async Task<int> GetList_Apt_c_Count(string AptCode, string B_N_A_Name)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                return await dba.QuerySingleOrDefaultAsync<int>("Select Count(*) From Bloom Where Views = 'A' And Bloom_Code = 'D' And B_N_A_Name = @B_N_A_Name And AptCode = @AptCode", new { AptCode, B_N_A_Name });
            }
        }

        /// <summary>
        /// 입력된 마지막 일련 번호 가져오기
        /// </summary>
        /// <returns></returns>
        public async Task<string> LastAid()
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                return await dba.QuerySingleOrDefaultAsync<string>("Select Top 1 Num From Bloom Order by Num Desc");
            }
        }

        /// <summary>
        /// 대분류명으로 시설물 코드 불러오기
        /// </summary>
        /// <param name="B_N_A_Name"></param>
        /// <returns></returns>
        public async Task<string> BloomNameA(string B_N_A_Name)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                return await dba.QuerySingleOrDefaultAsync<string>("Select B_N_Code From Bloom Where Views = 'A' And B_N_A_Name = @B_N_A_Name And Bloom_Code = 'A'", new { B_N_A_Name });
            }
        }

        /// <summary>
        /// 중분류명으로 시설물 코드 불러오기
        /// </summary>
        /// <param name="B_N_A_Name"></param>
        /// <returns></returns>
        public async Task<string> BloomNameB(string B_N_A_Name, string B_N_B_Name)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                return await dba.QuerySingleOrDefaultAsync<string>("Select B_N_Code From Bloom Where Views = 'A' And B_N_A_Name = @B_N_A_Name And B_N_B_Name = @B_N_B_Name And Bloom_Code = 'B'", new { B_N_A_Name, B_N_B_Name });
            }
        }

        /// <summary>
        /// 소분류명으로 시설물 코드 불러오기
        /// </summary>
        /// <param name="B_N_A_Name"></param>
        /// <returns></returns>
        public async Task<string> BloomNameC(string B_N_A_Name, string B_N_B_Name, string B_N_C_Name)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                return await dba.QuerySingleOrDefaultAsync<string>("Select B_N_Code From Bloom Where Views = 'A' And B_N_A_Name = @B_N_A_Name And B_N_B_Name = @B_N_B_Name And B_N_C_Name = @B_N_C_Name And Bloom_Code = 'C'", new { B_N_A_Name, B_N_B_Name, B_N_C_Name });
            }
        }

        /// <summary>
        /// 같은 작업 장소 존재여부 확인(공동주택 별)
        /// </summary>
        /// <param name="AptCode"></param>
        /// <param name="B_N_A_Name"></param>
        /// <param name="Bloom"></param>
        /// <returns></returns>
        public async Task<int> Be(string AptCode, string B_N_A_Name, string Bloom)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                return await dba.QuerySingleOrDefaultAsync<int>("Select Count(*) From Bloom Where Views = 'A' And AptCode = @AptCode And B_N_A_Name = @B_N_A_Name And Bloom = @Bloom", new { AptCode, B_N_A_Name, Bloom });
            }
        }

        /// <summary>
        /// 해당 시설물 분류 삭제
        /// </summary>
        /// <param name="Num"></param>
        public async Task Remove(string Num)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                await dba.ExecuteAsync("Update Bloom Set Views = 'B' Where Num = @Num", new { Num });
            }
        }

        public async Task<List<Bloom_Entity>> GetList_bb(string B_N_A_Name)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                var lst = await dba.QueryAsync<Bloom_Entity>("Select Num, AptCode, Apt_Name, B_N_A_Name, B_N_B_Name, B_N_C_Name, BloomA, BloomB, B_N_Code, Bloom, Bloom_Code, Period, intro, ModifyDate, ModifyIP, PostDate, PostIP From Bloom Where Views = 'A' And B_N_A_Name = @B_N_A_Name And Bloom_Code = 'B'", new { B_N_A_Name });
                return lst.ToList();
            }
        }

        public async Task<List<Bloom_Entity>> GetList_cc(string B_N_B_Name)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                var lst = await dba.QueryAsync<Bloom_Entity>("Select Num, AptCode, Apt_Name, B_N_A_Name, B_N_B_Name, B_N_C_Name, BloomA, BloomB, B_N_Code, Bloom, Bloom_Code, Period, intro, ModifyDate, ModifyIP, PostDate, PostIP From Bloom Where Views = 'A' And B_N_B_Name = @B_N_B_Name And Bloom_Code = 'C'", new { B_N_B_Name });
                return lst.ToList();
            }
        }

        public async Task<List<Bloom_Entity>> GetList_dd(string AptCode, string B_N_A_Name)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                var lst = await dba.QueryAsync<Bloom_Entity>("Select Num, AptCode, Apt_Name, B_N_A_Name, B_N_B_Name, B_N_C_Name, BloomA, BloomB, B_N_Code, Bloom, Bloom_Code, Period, intro, ModifyDate, ModifyIP, PostDate, PostIP From Bloom Where Views = 'A' And AptCode = @AptCode And B_N_A_Name = @B_N_A_Name And Bloom_Code = 'D'", new { AptCode, B_N_A_Name });
                return lst.ToList();
            }
        }

        /// <summary>
        /// 시설물 대분류 코드
        /// </summary>
        /// <param name="B_N_A_Name"></param>
        /// <returns></returns>
        public async Task<string> B_N_A_Code(string B_N_A_Name)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                return await dba.QuerySingleOrDefaultAsync<string>("Select top 1 Num From Bloom Where B_N_A_Name = @B_N_A_Name and Views = 'A'", new { B_N_A_Name });
            }
        }

        /// <summary>
        /// 시설물 중분류 코드
        /// </summary>
        /// <param name="B_N_A_Name"></param>
        /// <param name="B_N_B_Name"></param>
        /// <returns></returns>
        public async Task<string>  B_N_B_Code(string B_N_A_Name, string B_N_B_Name)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                return await dba.QuerySingleOrDefaultAsync<string>("Select top 1 Num From Bloom Where B_N_A_Name = @B_N_A_Name And B_N_B_Name = @B_N_B_Name and Views = 'A'", new { B_N_A_Name, B_N_B_Name });
            }
        }

        /// <summary>
        /// 시설물 소분류 코드
        /// </summary>
        /// <param name="B_N_A_Name"></param>
        /// <param name="B_N_B_Name"></param>
        /// <param name="B_N_C_Name"></param>
        /// <returns></returns>
        public async Task<string>  B_N_C_Code(string B_N_A_Name, string B_N_B_Name, string B_N_C_Name)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                return await dba.QuerySingleOrDefaultAsync<string>("Select top 1 Num From Bloom Where Views = 'A' And B_N_A_Name = @B_N_A_Name And B_N_B_Name = @B_N_B_Name And B_N_C_Name = @B_N_C_Name", new { B_N_A_Name, B_N_B_Name, B_N_C_Name });
            }
        }

        /// <summary>
        /// 장소 코드
        /// </summary>
        /// <param name="B_N_A_Name"></param>
        /// <param name="B_N_B_Name"></param>
        /// <param name="B_N_C_Name"></param>
        /// <param name="Bloom"></param>
        /// <param name="AptCode"></param>
        /// <returns></returns>
        public async Task<string> B_N_D_Code(string B_N_A_Name, string Bloom, string AptCode)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                return await dba.QuerySingleOrDefaultAsync<string>("Select top 1 Num From Bloom Where Views = 'A' And B_N_A_Name = @B_N_A_Name And Bloom = @Bloom And AptCode = @AptCode", new { B_N_A_Name, Bloom, AptCode });
            }
        }

        /// <summary>
        /// 분류명 불러오기
        /// </summary>
        public async Task<string> Sort_Name(string Num, string Bloom_Code)
        {
            string Re = "";
            using (var dba = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                if (Bloom_Code == "A")
                {
                    Re = await dba.QuerySingleOrDefaultAsync<string>("Select B_N_A_Name From Bloom Where Num = @Num", new { Num, Bloom_Code });
                }
                else if (Bloom_Code == "B")
                {
                    Re = await dba.QuerySingleOrDefaultAsync<string>("Select B_N_B_Name From Bloom Where Num = @Num", new { Num, Bloom_Code });
                }
                else if (Bloom_Code == "C")
                {
                    Re = await dba.QuerySingleOrDefaultAsync<string>("Select B_N_C_Name From Bloom Where Num = @Num", new { Num, Bloom_Code });
                }
                return Re;
            }
        }

        /// <summary>
        /// 하자 기간
        /// </summary>
        /// <param name="Num"></param>
        /// <returns></returns>
        public async Task<int> Period(string Num)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                return await dba.QuerySingleOrDefaultAsync<int>("Select top 1 Period From Bloom Where Num = @Num", new { Num });
            }
        }  
        
        /// <summary>
        /// 찾기
        /// </summary>
        /// <param name="Field"></param>
        /// <param name="Query"></param>
        /// <returns></returns>
        public async Task<List<Bloom_Entity>> SearchList(string Field, string Query)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                var lst = await dba.QueryAsync<Bloom_Entity>("Select Top 20 * From Bloom Where " + Field + " Like '%" + Query +"%'", new { Field, Query });
                return lst.ToList();
            }
        }
    }
}
