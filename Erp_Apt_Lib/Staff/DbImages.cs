using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace DbImage
{
    public class DbImageEntity
    {
        public int Aid { get; set; }

        /// <summary>
        /// 파일명
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// 이미지 타입
        /// </summary>
        public string FileType { get; set; }

        /// <summary>
        /// 이미지 정보
        /// </summary>
        public byte[] Photo { get; set; }

        /// <summary>
        /// 공동주택 식별코드
        /// </summary>
        public string AptCode { get; set; }

        /// <summary>
        /// 유저 식별코드
        /// </summary>
        public string User_Code { get; set; }

        /// <summary>
        /// 입력일
        /// </summary>
        public DateTime PostDate { get; set; }

        /// <summary>
        /// 입력자 아이피
        /// </summary>
        public string PostIP { get; set; }

        /// <summary>
        /// 이름
        /// </summary>
        public string SubName { get; set; }
    }

    public interface IDbImageLib
    {
        /// <summary>
        /// 도장 등록
        /// </summary>
        Task<int> Add(DbImageEntity db);

        /// <summary>
        /// 도장 등록 정보 목록
        /// </summary>
        Task<List<DbImageEntity>> GetList(int Page);

        /// <summary>
        /// 도장 등록 정보 목록 수
        /// </summary>
        Task<int> GetListCount();

        /// <summary>
        /// 도장 등록 정보 목록
        /// </summary>
        Task<List<DbImageEntity>> GetListApt(int Page, string AptCode);

        /// <summary>
        /// 도장 등록 정보 목록 수
        /// </summary>
        Task<int> GetListCountApt(string AptCode);
        
        /// <summary>
        /// 이미지 불러오기
        /// </summary>
        Task<byte[]> Photo_image(string UserCode);

        /// <summary>
        /// 도장 존재 여부
        /// </summary>
        Task<int> Photo_Count(string UserCode);

        /// <summary>
        /// 도장 정보 수정
        /// </summary>
        Task Edit(DbImageEntity Photo);

        /// <summary>
        /// 도장 정보 삭제
        /// </summary>
        Task Remove(int Aid);
    }

    public class DbImageLib : IDbImageLib
    {
        private readonly IConfiguration _db;
        public DbImageLib(IConfiguration configuration)
        {
            this._db = configuration;
        }

        /// <summary>
        /// 도장 등록
        /// </summary>
        /// <param name="db"></param>
        /// <returns></returns>
        public async Task<int> Add(DbImageEntity db)
        {
            var sql = "Insert Into Photo (FileName, FileType, Photo, AptCode, User_Code, PostIP, SubName) Values (@FileName, @FileType, @Photo, @AptCode, @User_Code, @PostIP, @SubName); Select Cast(SCOPE_IDENTITY() As Int);";
            using var df = new SqlConnection(_db.GetConnectionString("sw_togather"));
            int a = await df.QuerySingleOrDefaultAsync<int>(sql, db);
            return a;
        }

        /// <summary>
        /// 도장 정보 수정
        /// </summary>
        public async Task Edit(DbImageEntity Photo)
        {
            using var df = new SqlConnection(_db.GetConnectionString("sw_togather"));
            await df.ExecuteAsync("Update Photo Set FileName = @FileName, FileType = @FileType, Photo = @Photo, PostDate = @PostDate, PostIP = @PostIP", Photo);
        }

        /// <summary>
        /// 도장 등록 정보 목록
        /// </summary>
        public async Task<List<DbImageEntity>> GetList(int Page)
        {
            using var df = new SqlConnection(_db.GetConnectionString("sw_togather"));
            var lst = await df.QueryAsync<DbImageEntity>("Select Top 15 * From Photo Where Aid Not In (Select Top (15 * @Page) Aid From Photo Order By Aid Desc) Order By Aid Desc", new { Page });
            return lst.ToList();
        }

        /// <summary>
        /// 도장 등록 정보 목록 수
        /// </summary>
        public async Task<int> GetListCount()
        {
            using var df = new SqlConnection(_db.GetConnectionString("sw_togather"));
            return await df.QuerySingleOrDefaultAsync<int>("Select Count(*) From Photo");
        }


        /// <summary>
        /// 도장 등록 정보 목록
        /// </summary>
        public async Task<List<DbImageEntity>> GetListApt(int Page, string AptCode)
        {
            using var df = new SqlConnection(_db.GetConnectionString("sw_togather"));
            var lst = await df.QueryAsync<DbImageEntity>("Select Top 15 * From Photo Where Aid Not In (Select Top (15 * @Page) Aid From Photo Where AptCode = @AptCode Order By Aid Desc) And AptCode = @AptCode Order By Aid Desc", new { Page, AptCode });
            return lst.ToList();
        }

        /// <summary>
        /// 도장 등록 정보 목록 수
        /// </summary>
        public async Task<int> GetListCountApt(string AptCode)
        {
            using var df = new SqlConnection(_db.GetConnectionString("sw_togather"));
            return await df.QuerySingleOrDefaultAsync<int>("Select Count(*) From Photo Where AptCode = @AptCode", new { AptCode });
        }

        /// <summary>
        /// 이미지 불러오기
        /// </summary>
        public async Task<byte[]> Photo_image(string UserCode)
        {
            using var df = new SqlConnection(_db.GetConnectionString("sw_togather"));
            return await df.QuerySingleOrDefaultAsync<byte[]>("Select Top 1 Photo From Photo Where User_Code = @UserCode Order by Aid Desc", new { UserCode });
        }

        /// <summary>
        /// 도장 존재 여부
        /// </summary>
        public async Task<int> Photo_Count(string UserCode)
        {
            using var df = new SqlConnection(_db.GetConnectionString("sw_togather"));
            return await df.QuerySingleOrDefaultAsync<int>("Select Count(*) From Photo Where User_Code = @UserCode", new { UserCode });
        }

        /// <summary>
        /// 도장 정보 삭제
        /// </summary>
        public async Task Remove(int Aid)
        {
            using var df = new SqlConnection(_db.GetConnectionString("sw_togather"));
            await df.ExecuteAsync("Delete Photo Where Aid = @Aid", new { Aid });
        }

    }
}
