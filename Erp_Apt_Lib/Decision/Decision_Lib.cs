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

    /// <summary>
    /// 결재 클리스
    /// </summary>
    public class Decision_Lib : IDecision_Lib
    {
        private readonly IConfiguration _db;
        public Decision_Lib(IConfiguration configuration)
        {
            this._db = configuration;
        }

        /// <summary>
        /// 결재 상세(async)
        /// </summary>
        public async Task<Decision_Entity> Detail(string AptCode, string BloomCode, string Parent, string PostDuty)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                return await dba.QuerySingleOrDefaultAsync<Decision_Entity>("Decision_Detail", new { AptCode, BloomCode, Parent, PostDuty }, commandType: CommandType.StoredProcedure);                
            }
        }

        /// <summary>
        /// 결재 상세 
        /// </summary>
        public Decision_Entity Details(string AptCode, string BloomCode, string Parent, string PostDuty)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                return dba.QuerySingleOrDefault<Decision_Entity>("Decision_Detail", new { AptCode, BloomCode, Parent, PostDuty }, commandType: CommandType.StoredProcedure);
            }
        }

        /// <summary>
        /// 해당 결재 존재여부확인
        /// </summary>
        /// <param name="AptCode"></param>
        /// <param name="BloomCode"></param>
        /// <param name="Parent"></param>
        /// <param name="PostDuty"></param>
        /// <returns></returns>
        public async Task<int> Detail_Count(string AptCode, string BloomCode, string Parent, string PostDuty)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                return await dba.QuerySingleOrDefaultAsync<int>("Select Count(*) From Decision Where AptCode = @AptCode and BloomCode = @BloomCode and Parent = @Parent and PostDuty = @PostDuty", new { AptCode, BloomCode, Parent, PostDuty });
            }
            
        }

        /// <summary>
        /// 해당 결재 존재여부확인
        /// </summary>
        /// <param name="AptCode"></param>
        /// <param name="BloomCode"></param>
        /// <param name="Parent"></param>
        /// <param name="PostDuty"></param>
        /// <returns></returns>
        public int Details_Count(string AptCode, string BloomCode, string Parent, string PostDuty)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                return dba.QuerySingleOrDefault<int>("Select Count(*) From Decision Where AptCode = @AptCode and BloomCode = @BloomCode and Parent = @Parent and PostDuty = @PostDuty", new { AptCode, BloomCode, Parent, PostDuty });
            }

        }


        /// <summary>
        /// 결재 여부 확인
        /// </summary>
        /// <param name="AptCode"></param>
        /// <param name="BloomCode"></param>
        /// <param name="Parent"></param>
        /// <param name="PostDuty"></param>
        /// <returns></returns>
        public async Task<int> Decision_Being_Count(string AptCode, string Parent, string BloomCode, string PostDuty, string User_Code)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                return await dba.QuerySingleOrDefaultAsync<int>("Select Count(*) From Decision Where AptCode = @AptCode and BloomCode = @BloomCode and Parent = @Parent and PostDuty = @PostDuty", new { AptCode, BloomCode, Parent, PostDuty, User_Code });
            }            
        }

        /// <summary>
        /// 결재 여부 확인
        /// </summary>
        public int Decisions_Being_Count(string AptCode, string Parent, string BloomCode, string PostDuty, string User_Code)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                return dba.QuerySingleOrDefault<int>("Select Count(*) From Decision Where AptCode = @AptCode and BloomCode = @BloomCode and Parent = @Parent and PostDuty = @PostDuty", new { AptCode, BloomCode, Parent, PostDuty, User_Code });
            }
        }

        /// <summary>
        /// 결재라인에서 해당 부서직책 존재 여부 확인
        /// </summary>
        public int PostDutyBeCount(string AptCode, string Bloom, string PostDuty)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                return dba.QuerySingleOrDefault<int>("Select Count(*) From PostDuty  Where AptCode = @AptCode And Bloom_Code = @Bloom And PostDuty = @PostDuty", new { AptCode, Bloom, PostDuty });
            }
            
        }

        /// <summary>
        /// 결재 입력
        /// </summary>
        /// <param name="_Entity"></param>
        /// <returns></returns>
        public async Task<int> Add(Decision_Entity _Entity)
        {
            var sql = "Insert Decision (AptCode, Parent, BloomCode, UserName, PostDuty, Comform, User_Code, PostIP ) Values (@AptCode, @Parent, @BloomCode, @UserName, @PostDuty, @Comform, @User_Code, @PostIP); Select Cast(SCOPE_IDENTITY() As Int);";
            using (var dba = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                int Aid = await dba.QuerySingleOrDefaultAsync<int>(sql, _Entity);
                return Aid;
            }
        }

        /// <summary>
        /// 해당 업무에 관리소장 결재 완료 입력
        /// </summary>
        /// <param name="Num">식별코드 번호</param>
        /// <param name="TableName">테이블명</param>
        /// <param name="Conform">결재완료 필드명</param>
        /// <param name="Feild">해당 업무의 식별코드 필드명</param>
        public async Task Decision_Comform(string Num, string TableName, string Conform, string Feild)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
               await dba.ExecuteAsync("Update " + TableName + " Set " + Conform + " = 'B' Where " + Feild + " = @Num", new { Num, TableName, Conform, Feild });
            }            
        }        
    }


    /// <summary>
    /// 결재 도장 클래스
    /// </summary>
    public class DbImagesLib : IDbImagesLib
    {
        private readonly IConfiguration _db;
        public DbImagesLib(IConfiguration configuration)
        {
            this._db = configuration;
        }

        public async Task<int> Add(DbImagesEntity db)
        {
            var sql = "Select Into Photo (FileName, FileType, Photo, AptCode, User_Code, PostIP) Values (@FileName, @FileType, @Photo, @AptCode, @User_Code, @PostIP); Select Cast(SCOPE_IDENTITY() As Int);";
            using (var dba = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                int a = await dba.QueryFirstOrDefaultAsync<int>(sql, db);
                return a;
            }
            
        }

        public async Task<List<DbImagesEntity>> GetList(string AptCode)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                var lst = await dba.QueryAsync<DbImagesEntity>("Select * From Photo Where AptCode = @AptCode Order By Aid Desc", new { AptCode });
                return lst.ToList();
            }
            
        }

        /// <summary>
        /// 이미지 불러오기
        /// </summary>
        /// <param name="UserCode"></param>
        /// <returns></returns>
        public async Task<byte[]> Photo_image(string UserCode)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                return await dba.QuerySingleOrDefaultAsync<byte[]>("Select Top 1 Photo From Photo Where User_Code = @UserCode Order by Aid Desc", new { UserCode });
            }
            
        }

        /// <summary>
        /// 도장 존재 여부
        /// </summary>
        /// <param name="UserCode"></param>
        /// <returns></returns>
        public async Task<int> Photo_Count(string UserCode)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                return await dba.QuerySingleOrDefaultAsync<int>("Select Count(*) From Photo Where User_Code = @UserCode", new { UserCode });
            }
            
        }

        /// <summary>
        /// 이미지 불러오기
        /// </summary>
        /// <param name="UserCode"></param>
        /// <returns></returns>
        public byte[] Photos_image(string UserCode)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                return dba.QuerySingleOrDefault<byte[]>("Select top 1 Photo From Photo Where User_Code = @UserCode Order by Aid Desc", new { UserCode });
            }
            
        }

        /// <summary>
        /// 도장 존재 여부
        /// </summary>
        /// <param name="UserCode"></param>
        /// <returns></returns>
        public int Photos_Count(string UserCode)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                return dba.QuerySingleOrDefault<int>("Select Count(*) From Photo Where User_Code = @UserCode", new { UserCode });
            }
            
        }
    }
}
