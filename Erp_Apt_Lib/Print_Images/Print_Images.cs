using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using System.Linq;

namespace Erp_Apt_Lib.Print_Images
{
    public class Print_Images
    {
        public int Aid { get; set; }
        public string SortName { get; set; }
        public int SortCode { get; set; }
        public byte[] _Images { get; set; }
        public DateTime PostDate { get; set; }
    }

    public interface IPrint_Images_Lib
    {
        /// <summary>
        /// 이미지 파일 저장
        /// </summary>
        Task<int> Add(Print_Images im);

        /// <summary>
        /// 이미지 파일 수정
        /// </summary>
        Task Edit(Print_Images im);

        /// <summary>
        /// 이미지 파일 목록
        /// </summary>
        Task<List<Print_Images>> List(string Sort_Code);

        /// <summary>
        /// 이미지 파일 상세
        /// </summary>
        Task<Print_Images> Views(int Aid);

        /// <summary>
        /// 이미지 파일 삭제
        /// </summary>
        Task Remove(int Aid);


        /// <summary>
        /// 이미지 파일 모두 삭제
        /// </summary>
        Task Remove_All(string SortCode);
    }

    public class Print_Images_Lib : IPrint_Images_Lib
    {
        private readonly IConfiguration _db;
        public Print_Images_Lib(IConfiguration configuration)
        {
            this._db = configuration;
        }

        /// <summary>
        /// 이미지 파일 저장
        /// </summary>
        public async Task<int> Add(Print_Images im)
        {
            var sql = "Select Into Print_Images (SortName, SortCode, _Images) Values (@SortName, @SortCode, @_Images); Select Cast(SCOPE_IDENTITY() As Int);";
            using var db = new SqlConnection(_db.GetConnectionString("sw_togather"));
            return await db.QuerySingleOrDefaultAsync<int>(sql, im);
        }

        /// <summary>
        /// 이미지 파일 수정
        /// </summary>
        public async Task Edit(Print_Images im)
        {
            var sql = "Update Print_Images set SortName = @SortName, SortCode = @SortCode, _Images = @_Images";
            using var db = new SqlConnection(_db.GetConnectionString("sw_togather"));
            await db.ExecuteAsync(sql, im);
        }

        /// <summary>
        /// 이미지 파일 목록
        /// </summary>
        public async Task<List<Print_Images>> List(string Sort_Code)
        {
            using var db = new SqlConnection(_db.GetConnectionString("sw_togather"));
            var lst = await db.QueryAsync<Print_Images>("Select Aid, SortName, SortCode, _Images From Print_Images Where Sort_Code = @Sort_Code", new { Sort_Code });
            return lst.ToList();
        }

        /// <summary>
        /// 이미지 파일 상세
        /// </summary>
        public async Task<Print_Images> Views(int Aid)
        {
            using var db = new SqlConnection(_db.GetConnectionString("sw_togather"));
            return await db.QuerySingleOrDefaultAsync<Print_Images>("Select Aid, SortName, SortCode, _Images From Print_Images Where Aid = @Aid", new { Aid });
        }

        /// <summary>
        /// 이미지 파일 삭제
        /// </summary>
        public async Task Remove(int Aid)
        {
            using var db = new SqlConnection(_db.GetConnectionString("sw_togather"));
            await db.ExecuteAsync("Delete Print_Images Where Aid = @Aid", new { Aid });
        }

        /// <summary>
        /// 이미지 파일 모두 삭제
        /// </summary>
        public async Task Remove_All(string SortCode)
        {
            using var db = new SqlConnection(_db.GetConnectionString("sw_togather"));
            await db.ExecuteAsync("Delete Print_Images Where SortCode = @SortCode", new { SortCode });
        }
    }
}
