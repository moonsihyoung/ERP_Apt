using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Erp_Apt_Lib.apt_Erp_Com
{
    /// <summary>
    /// 자전거 속성
    /// </summary>
    public class Bike_Entity
    {
        /// <summary>
        /// 일련번호
        /// </summary>
        public int Aid { get; set; }

        /// <summary>
        /// 공동주택 식별코드
        /// </summary>
        public string Apt_Code { get; set; }

        /// <summary>
        /// 공동주택 명
        /// </summary>
        public string  Apt_Name { get; set; }

        /// <summary>
        /// 동
        /// </summary>
        public string Dong { get; set; }

        /// <summary>
        /// 호
        /// </summary>
        public string Ho { get; set; }

        /// <summary>
        /// 휴대폰
        /// </summary>
        public string Mobile { get; set; }

        /// <summary>
        /// 소유자
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 자전거명
        /// </summary>
        public string Bike_Name { get; set; }

        /// <summary>
        /// 시설설명
        /// </summary>
        public string Etc { get; set; }

        /// <summary>
        /// 삭제 여부
        /// </summary>
        public string del { get; set; }

        /// <summary>
        /// 이사일
        /// </summary>
        public DateTime MoveDate { get; set; }

        /// <summary>
        /// 입력일
        /// </summary>
        public DateTime PostDate { get; set; }

        /// <summary>
        /// 입력자 아이피
        /// </summary>
        public string PostIp { get; set; }
    }

    /// <summary>
    /// 자전거 라이브러리
    /// </summary>
    public class Bike_Lib : IBike_Lib
    {
        private readonly IConfiguration _db;
        public Bike_Lib(IConfiguration configuration)
        {
            this._db = configuration;
        }

        /// <summary>
        /// 입력
        /// </summary>
        public async Task Add(Bike_Entity bike)
        {
            var sql = "Insert Bike (Apt_Code, Apt_Name, Dong, Ho, Mobile, Name, Bike_Name, Etc, PostIp) values (@Apt_Code, @Apt_Name, @Dong, @Ho, @Mobile, @Name, @Bike_Name, @Etc, @PostIp);";
            using var df = new SqlConnection(_db.GetConnectionString("sw_togather"));
            await df.ExecuteAsync(sql, bike);
        }

        /// <summary>
        /// 수정
        /// </summary>
        public async Task Edit(Bike_Entity bike)
        {
            using var df = new SqlConnection(_db.GetConnectionString("sw_togather"));
            await df.ExecuteAsync("Update Bike Set Dong = @Dong, Ho = @Ho, Mobile = @Mobile, Name = @Name, Bike_Name = @Bike_Name, Etc = @Etc, PostIp = @PostIp", bike);
        }

        /// <summary>
        /// 자전거 등록 목록
        /// </summary>
        public async Task<List<Bike_Entity>> GetList(int page)
        {
            using var df = new SqlConnection(_db.GetConnectionString("sw_togather"));
            var lst = await df.QueryAsync<Bike_Entity>("Select Top 15 * From Bike Where Aid Not In (Select Top(15 * @Page) Aid From Bike Order by Aid Desc) Order by Aid Desc", new { page });
            return lst.ToList();
        }

        /// <summary>
        /// 자전기 목록 수
        /// </summary>
        public async Task<int> GetList_Count()
        {
            using var df = new SqlConnection(_db.GetConnectionString("sw_togather"));
            return await df.QuerySingleOrDefaultAsync<int>("Select Count(*) From Bike");
        }

        /// <summary>
        /// 해당 공동주택 자전거 등록 목록
        /// </summary>
        public async Task<List<Bike_Entity>> GetList_Apt(int Page, string Apt_Code)
        {
            using var df = new SqlConnection(_db.GetConnectionString("sw_togather"));
            var lst = await df.QueryAsync<Bike_Entity>("Select Top 15 * From Bike Where Aid Not In (Select Top(15 * @Page) Aid From Bike Where Apt_Code = @Apt_Code Order by Aid Desc) And Apt_Code = @Apt_Code Order by Aid Desc", new { Page, Apt_Code });
            return lst.ToList();
        }

        /// <summary>
        /// 해당 공동주택 자전거 등록 목록 수
        /// </summary>
        public async Task<int> GetList_Apt_Count(string Apt_Code)
        {
            using var df = new SqlConnection(_db.GetConnectionString("sw_togather"));
            return await df.QuerySingleOrDefaultAsync<int>("Select Count(*) From Bike Where Apt_Code = @Apt_Code", new { Apt_Code });
        }

        /// <summary>
        /// 상세보기
        /// </summary>
        public async Task<Bike_Entity> Details(int Aid)
        {
            using var df = new SqlConnection(_db.GetConnectionString("sw_togather"));
            return await df.QuerySingleOrDefaultAsync<Bike_Entity>("Select * From Bike Where Aid = @Aid", new { Aid });
        }

        /// <summary>
        /// 이사
        /// </summary>
        public async Task Remove(int Aid, DateTime MDate)
        {
            using var df = new SqlConnection(_db.GetConnectionString("sw_togather"));
            string strRe = await df.QuerySingleOrDefaultAsync<string>("Select del From Bike Where Aid = @Aid", new { Aid });
            if (strRe == "A")
            {
                await df.ExecuteAsync("Update Bike Set del = 'B', MoveDate = @MDate Where Aid = @Aid", new { Aid, MDate });
            }
            else
            {
                await df.ExecuteAsync("Update Bike Set del = 'A', MoveDate = @MDate Where Aid = @Aid", new { Aid, MDate });
            }
        }

        /// <summary>
        /// 찾기
        /// </summary>
        public async Task<List<Bike_Entity>> SearchList(string Apt_Code, string Dong, string Ho)
        {
            using var df = new SqlConnection(_db.GetConnectionString("sw_togather"));
            var lst = await df.QueryAsync<Bike_Entity>("Select * From Bike Where Apt_Code = @Apt_Code And Dong = @Dong And Ho = @Ho", new {Apt_Code, Dong, Ho});
            return lst.ToList();
        }
    }

    /// <summary>
    /// 자전거 인터페이스
    /// </summary>
    public interface IBike_Lib
    {
        /// <summary>
        /// 입력
        /// </summary>
        Task Add(Bike_Entity bike);

        /// <summary>
        /// 수정
        /// </summary>
        Task Edit(Bike_Entity bike);

        /// <summary>
        /// 자전거 등록 목록
        /// </summary>
        Task<List<Bike_Entity>> GetList(int page);

        /// <summary>
        /// 자전기 목록 수
        /// </summary>
        Task<int> GetList_Count();

        /// <summary>
        /// 해당 공동주택 자전거 등록 목록
        /// </summary>
        Task<List<Bike_Entity>> GetList_Apt(int Page, string Apt_Code);

        /// <summary>
        /// 해당 공동주택 자전거 등록 목록 수
        /// </summary>
        Task<int> GetList_Apt_Count(string Apt_Code);

        /// <summary>
        /// 상세보기
        /// </summary>
        Task<Bike_Entity> Details(int Aid);

        /// <summary>
        /// 이사
        /// </summary>
        Task Remove(int Aid, DateTime MDate);

        /// <summary>
        /// 찾기
        /// </summary>
        Task<List<Bike_Entity>> SearchList(string Apt_Code, string Dong, string Ho);
    }
}
