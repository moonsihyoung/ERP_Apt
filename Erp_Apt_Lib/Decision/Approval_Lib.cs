using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Erp_Apt_Lib.Decision
{
    public class Approval_Lib : IApproval_Lib
    {
        private readonly IConfiguration _db;
        public Approval_Lib(IConfiguration configuration)
        {
            this._db = configuration;
        }

        /// <summary>
        /// 결재란 만들기
        /// </summary>
        /// <param name="AptCode"></param>
        /// <param name="Bloom"></param>
        /// <returns>PostDuty</returns>
        public async Task<List<Approval_Entity>> GetList(string AptCode, string Bloom)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                var lst = await dba.QueryAsync<Approval_Entity>("ListPostDuty", new { AptCode, Bloom }, commandType: CommandType.StoredProcedure);
                return lst.ToList();
            }
        }

        /// <summary>
        /// 결제자 라인 입력
        /// </summary>
        public async Task Add(Approval_Entity dnn)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                await dba.ExecuteAsync("Insert PostDuty (AptCode, AptName, Bloom, Bloom_Code, PostDuty, Post, Duty, Intro, PostIP, Step) Values (@AptCode, @AptName, @Bloom, @Bloom_Code, @PostDuty, @Post, @Duty, @Intro, @PostIP, @Step)", dnn);
            }

        }

        /// <summary>
        /// 결재자 라인 정보 수정
        /// </summary>
        /// <param name="dp"></param>
        public async Task Edit(Approval_Entity dp)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                await dba.ExecuteAsync("Update PostDuty Bloom = @Bloom, Bloom_Code = @Bloom_Code, PostDuty = @PostDuty, Post = @Post, Duty = @Duty, Intro = @Intro, PostIP = @PostIP, @ Step = @Step Where Num = @Num", dp);
            }

        }

        /// <summary>
        /// 결재자 정보 상세보기
        /// </summary>
        public async Task<Approval_Entity> Details(int Num)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                return await dba.QuerySingleOrDefaultAsync<Approval_Entity>("Select Num, AptCode, AptName, Bloom, Bloom_Code, PostDuty, Post, Duty, Intro, ModifyDate, ModifyIP, PostDate, PostIP, Step From PostDuty Where Num = @Num", new { Num });
            }

        }


        /// <summary>
        /// 결재 정보 목록
        /// </summary>
        public async Task<int> ListCount_Bloom(string AptCode, string Bloom_Code)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                return await dba.QuerySingleOrDefaultAsync<int>("Select Count(*) From PostDuty  Where AptCode = @AptCode And Bloom_Code = @Bloom_Code", new { AptCode, Bloom_Code });
            }

        }

        /// <summary>
        /// 결재라인에서 해당 부서직책 존재 여부 확인
        /// </summary>
        public async Task<int> PostDutyBeCount(string AptCode, string Bloom_Code, string PostDuty)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                return await dba.QuerySingleOrDefaultAsync<int>("Select Count(*) From PostDuty  Where AptCode = @AptCode And Bloom_Code = @Bloom_Code And PostDuty = @PostDuty", new { AptCode, Bloom_Code, PostDuty });
            }

        }

        /// <summary>
        /// 결재 정보 목록 (공동주택 전체)
        /// </summary>
        public async Task<List<Approval_Entity>> GetBloomList(int Page, string AptCode)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                var lst = await dba.QueryAsync<Approval_Entity>("Select Top 15 * From PostDuty Where Num Not In(Select Top(15 * @Page) Num From PostDuty Where AptCode = @AptCode Order By Num Desc) And AptCode = @AptCode Order By Num Desc", new { Page, AptCode });
                return lst.ToList();
            }

        }

        /// <summary>
        /// 결재 정보 목록 (공동주택 전체) 수
        /// </summary>
        /// <param name="AptCode"></param>
        /// <returns></returns>
        public async Task<int> GetListCount(string AptCode)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                return await dba.QuerySingleOrDefaultAsync<int>("Select Count(*) From PostDuty Where AptCode = @AptCode", new { AptCode });
            }

        }

        /// <summary>
        /// 삭제 (2022)
        /// </summary>
        public async Task Remove(int Num)
        {
            using var df = new SqlConnection(_db.GetConnectionString("Ayoung"));
            await df.ExecuteAsync("Delete PostDuty Where Num = @Num", new { Num });
        }
        
    }
}
