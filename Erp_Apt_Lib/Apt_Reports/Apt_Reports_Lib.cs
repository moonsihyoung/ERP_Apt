using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using System.Linq;

namespace Erp_Apt_Lib.Apt_Reports
{
    public class Apt_Reports_Lib : IApt_Reports_Lib
    {
        private readonly IConfiguration _db;
        public Apt_Reports_Lib(IConfiguration configuration)
        {
            this._db = configuration;
        }

        /// <summary>
        /// 보고서 입력
        /// </summary>
        /// <param name="_Entity"></param>
        /// <returns></returns>
        public async Task<Apt_Reports_Entity> Add(Apt_Reports_Entity _Entity)
        {
            var sql = "Insert into Apt_Reports (Staff_Code, Report_Bloom_Code, Apt_Code, Report_Title, Report_Content, Report_Year, Report_Month, Report_Division_Code, PostIP) Values (@Staff_Code, @Report_Bloom_Code, @Apt_Code, @Report_Title, @Report_Content, @Report_Year, @Report_Month, @Report_Division_Code, @PostIP); Select Cast(SCOPE_IDENTITY() As Int);";
            using var dba = new SqlConnection(_db.GetConnectionString("sw_togather"));
            var Aid = await dba.QuerySingleOrDefaultAsync<int>(sql, _Entity);
            _Entity.Aid = Aid;
            return _Entity;
        }

        /// <summary>
        /// 첨부 파일 추가 시 실행
        /// </summary>
        public async Task Files_Add(int Aid, string Sort)
        {
            using var dba = new SqlConnection(_db.GetConnectionString("sw_togather"));
            if (Sort == "A")
            {
                await dba.ExecuteAsync("Update Apt_Reports Set FilesCount = FilesCount + 1 Where Aid = @Aid", new { Aid });
            }
            else
            {
                int re = await dba.QuerySingleOrDefaultAsync<int>("Select FilesCount From Apt_Reports Where Aid = @Aid", new {Aid});
                if (re > 0)
                {
                    await dba.ExecuteAsync("Update Apt_Reports Set FilesCount = FilesCount - 1 Where Aid = @Aid", new { Aid });
                }
            }            
        }

        /// <summary>
        /// 보고서 수정
        /// </summary>
        /// <param name="_Entity"></param>
        /// <returns></returns>
        public async Task<Apt_Reports_Entity> Edit(Apt_Reports_Entity _Entity)
        {
            var sql = "Update Apt_Reports Set Staff_Code = @Staff_Code, Report_Bloom_Code = @Report_Bloom_Code, Report_Title = @Report_Title, Report_Content =@Report_Content, Report_Year = @Report_Year, Report_Month = @Report_Month, Report_Division_Code = @Report_Division_Code, Complete = 'A' Where Aid = @Aid";
            using var dba = new SqlConnection(_db.GetConnectionString("sw_togather"));
            await dba.ExecuteAsync(sql, _Entity);
            return _Entity;
        }

        /// <summary>
        /// 각종 보고서 상세보기
        /// </summary>
        /// <param name="Aid"></param>
        /// <returns></returns>
        public async Task<Apt_Reports_Entity> Detail(string Aid)
        {
            using var dba = new SqlConnection(_db.GetConnectionString("sw_togather"));
            return await dba.QuerySingleOrDefaultAsync<Apt_Reports_Entity>("Select a.Aid, a.Staff_Code, a.Report_Bloom_Code, a.Apt_Code, a.Report_Title, a.Report_Content, a.Report_Year, a.Report_Month, a.Report_Division_Code, a.PostDate, a.PostIP, a.Result, b.Report_Bloom_Name, c.Report_Division_Name From Apt_Reports as a Join Report_Bloom as b on a.Report_Bloom_Code = b.Report_Bloom_Code Join Report_Division as c on a.Report_Division_Code = c.Report_Division_Code Where Aid = @Aid", new { Aid });
        }

        /// <summary>
        /// 해당 공동주택 분류별 보고서 리스트
        /// </summary>
        /// <param name="Apt_Code"></param>
        /// <param name="Report_Bloom_Code"></param>
        /// <returns></returns>
        public async Task<List<Apt_Reports_Entity>> GetList(int Page, string Apt_Code, string Report_Bloom_Code)
        {
            using var dba = new SqlConnection(_db.GetConnectionString("sw_togather"));
            var lst = await dba.QueryAsync<Apt_Reports_Entity>("Select Top 15 a.Aid, a.Staff_Code, a.Report_Bloom_Code, a.Apt_Code, a.Report_Title, a.Report_Content, a.Report_Year, a.Report_Month, a.Report_Division_Code, a.PostDate, a.PostIP, a.Result, b.Report_Bloom_Name, c.Report_Division_Name From Apt_Reports as a Join Report_Bloom as b on a.Report_Bloom_Code = b.Report_Bloom_Code Join Report_Division as c on a.Report_Division_Code = c.Report_Division_Code Where a.Aid Not In(Select Top(15 * @Page) a.Aid From Apt_Reports as a Join Report_Bloom as b on a.Report_Bloom_Code = b.Report_Bloom_Code Join Report_Division as c on a.Report_Division_Code = c.Report_Division_Code Where a.Apt_Code = @Apt_Code And a.Report_Bloom_Code = @Report_Bloom_Code And a.Complete = 'A' Order By Aid Desc) And a.Apt_Code = @Apt_Code And a.Report_Bloom_Code = @Report_Bloom_Code And a.Complete = 'A' Order By a.Aid Desc", new { Page, Apt_Code, Report_Bloom_Code });
            return lst.ToList();
        }

        /// <summary>
        /// 해당 공동주택 분류별 보고서 수
        /// </summary>
        /// <param name="Apt_Code"></param>
        /// <param name="Report_Bloom_Code"></param>
        /// <returns></returns>
        public async Task<int> GetList_Count(string Apt_Code, string Report_Bloom_Code)
        {
            using var dba = new SqlConnection(_db.GetConnectionString("sw_togather"));
            return await dba.QuerySingleOrDefaultAsync<int>("Select Count(*) From Apt_Reports Where Apt_Code = @Apt_Code And Report_Bloom_Code = @Report_Bloom_Code", new { Apt_Code, Report_Bloom_Code });
        }

        /// <summary>
        /// 해당 공동주택 분류별 보고서 리스트 (all)
        /// </summary>
        /// <returns></returns>
        public async Task<List<Apt_Reports_Entity>> GetList_All(int Page)
        {
            using var dba = new SqlConnection(_db.GetConnectionString("sw_togather"));
            var lst = await dba.QueryAsync<Apt_Reports_Entity>("Select Top 15 a.Aid, a.Staff_Code, a.Report_Bloom_Code, a.Apt_Code, a.Report_Title, a.Report_Content, a.Report_Year, a.Report_Month, a.Report_Division_Code, a.PostDate, a.PostIP, a.Result, b.Report_Bloom_Name, c.Report_Division_Name, d.User_Name, e.Apt_Name From Apt_Reports as a Join Report_Bloom as b on a.Report_Bloom_Code = b.Report_Bloom_Code Join Report_Division as c on a.Report_Division_Code = c.Report_Division_Code Join Staff as d On a.Staff_Code = d.User_ID Join Apt as e On a.Apt_Code = e.Apt_Code Where a.Aid Not In(Select Top(15 * @Page) a.Aid From Apt_Reports as a Join Report_Bloom as b on a.Report_Bloom_Code = b.Report_Bloom_Code Join Report_Division as c on a.Report_Division_Code = c.Report_Division_Code Join Staff as d On a.Staff_Code = d.User_ID Join Apt as e On a.Apt_Code = e.Apt_Code Order By a.Aid Desc) Order By a.Aid Desc", new { Page });
            return lst.ToList();
        }


        /// <summary>
        /// 해당 공동주택 분류별 보고서 수
        /// </summary>
        /// <param name="Apt_Code"></param>
        /// <param name="Report_Bloom_Code"></param>
        /// <returns></returns>
        public async Task<int> GetList_Count_All()
        {
            using var dba = new SqlConnection(_db.GetConnectionString("sw_togather"));
            return await dba.QuerySingleOrDefaultAsync<int>("Select Count(*) From Apt_Reports as a Join Report_Bloom as b on a.Report_Bloom_Code = b.Report_Bloom_Code Join Report_Division as c on a.Report_Division_Code = c.Report_Division_Code Join Staff as d On a.Staff_Code = d.User_ID Join Apt as e On a.Apt_Code = e.Apt_Code");
        }


        /// <summary>
        /// 해당 공동주택 분류별 보고서 리스트 (all)
        /// </summary>
        /// <returns></returns>
        public async Task<List<Apt_Reports_Entity>> GetList_Apt_Code(int Page, string Apt_Code)
        {
            using var dba = new SqlConnection(_db.GetConnectionString("sw_togather"));
            
            var lst = await dba.QueryAsync<Apt_Reports_Entity>("Select Top 15 a.Aid, a.Staff_Code, a.Report_Bloom_Code, a.Apt_Code, a.Report_Title, a.Report_Content, a.Report_Year, a.Report_Month, a.Report_Division_Code, a.PostDate, a.PostIP, a.Result, b.Report_Bloom_Name, c.Report_Division_Name, d.User_Name, e.Apt_Name From Apt_Reports as a Join Report_Bloom as b on a.Report_Bloom_Code = b.Report_Bloom_Code Join Report_Division as c on a.Report_Division_Code = c.Report_Division_Code Join Staff as d On a.Staff_Code = d.User_ID Join Apt as e On a.Apt_Code = e.Apt_Code Where a.Aid Not In(Select Top(15 * @Page) a.Aid From Apt_Reports as a Join Report_Bloom as b on a.Report_Bloom_Code = b.Report_Bloom_Code Join Report_Division as c on a.Report_Division_Code = c.Report_Division_Code Join Staff as d On a.Staff_Code = d.User_ID Join Apt as e On a.Apt_Code = e.Apt_Code Where a.Apt_Code = @Apt_Code Order By a.Aid Desc) And a.Apt_Code = @Apt_Code Order By a.Aid Desc", new
            {
               Page,  Apt_Code
            });
            return lst.ToList();
        }


        /// <summary>
        /// 해당 공동주택 분류별 보고서 수
        /// </summary>
        /// <param name="Apt_Code"></param>
        /// <param name="Report_Bloom_Code"></param>
        /// <returns></returns>
        public async Task<int> GetList_Count_Apt(string Apt_Code)
        {
            using var dba = new SqlConnection(_db.GetConnectionString("sw_togather"));
            return await dba.QuerySingleOrDefaultAsync<int>("Select Count(*) From Apt_Reports Where Apt_Code = @Apt_Code And Complete = 'A'", new { Apt_Code });
        }

        /// <summary>
        /// 분사 분류별 보고서 리스트
        /// </summary>
        /// <param name="Apt_Code"></param>
        /// <param name="Report_Bloom_Code"></param>
        /// <returns></returns>
        public async Task<List<Apt_Reports_Entity>> GeList_Center(int Page, string Report_Bloom_Code)
        {
            using var dba = new SqlConnection(_db.GetConnectionString("sw_togather"));
            var lst = await dba.QueryAsync<Apt_Reports_Entity>("Select Top 15 a.Aid, a.Staff_Code, a.Report_Bloom_Code, a.Apt_Code, a.Report_Title, a.Report_Content, a.Report_Year, a.Report_Month, a.Report_Division_Code, a.PostDate, a.PostIP, a.Result, b.Report_Bloom_Name, c.Report_Division_Name From Apt_Reports as a Join Report_Bloom as b on a.Report_Bloom_Code = b.Report_Bloom_Code Join Report_Division as c on a.Report_Division_Code = c.Report_Division_Code Where a.Aid Not In(Select Top(15 * @Page) a.Aid From Apt_Reports as a Join Report_Bloom as b on a.Report_Bloom_Code = b.Report_Bloom_Code Join Report_Division as c on a.Report_Division_Code = c.Report_Division_Code Where a.Report_Bloom_Code = @Report_Bloom_Code And a.Complete = 'A' Order By a.Aid Desc) And a.Report_Bloom_Code = @Report_Bloom_Code And a.Complete = 'A' Order By a.Aid Desc", new { Page, Report_Bloom_Code });
            return lst.ToList();
        }

        /// <summary>
        /// 분사 분류별 보고서 리스트 수
        /// </summary>
        public async Task<int> GeList_Center_Count(string Report_Bloom_Code)
        {
            using var dba = new SqlConnection(_db.GetConnectionString("sw_togather"));
            return await dba.QuerySingleOrDefaultAsync<int>("Select Count(*) From Apt_Reports Where Report_Bloom_Code = @Report_Bloom_Code And Complete = 'A'", new { Report_Bloom_Code });
        }


        /// <summary>
        /// 보고서별 입력된 수 
        /// </summary>
        public async Task<int> Report_Bloom_Count(string Apt_Code, string Report_Bloom_Code)
        {
            using var dba = new SqlConnection(_db.GetConnectionString("sw_togather"));
            return await dba.QuerySingleOrDefaultAsync<int>("Select Count(*) From Apt_Reports Where Apt_Code = @Apt_Code And Report_Bloom_Code = @Report_Bloom_Code", new { Apt_Code, Report_Bloom_Code });
        }

        /// <summary>
        /// 년간 보고서 입력된 수 
        /// </summary>
        public async Task<int> Report_Count(string Apt_Code, string Report_Bloom_Code, string Report_Year)
        {
            using var dba = new SqlConnection(_db.GetConnectionString("sw_togather"));
            return await dba.QuerySingleOrDefaultAsync<int>("Select Count(*) From Apt_Reports Where Apt_Code = @Apt_Code And Report_Bloom_Code = @Report_Bloom_Code And Report_Year = @Report_Year", new { Apt_Code, Report_Bloom_Code, Report_Year });
        }

        /// <summary>
        /// 월간 보고서 입력된 수 
        /// </summary>
        public async Task<int> Report_Month_Count(string Apt_Code, string Report_Bloom_Code, string Report_Year, string Report_Month)
        {
            using var dba = new SqlConnection(_db.GetConnectionString("sw_togather"));
            return await dba.QuerySingleOrDefaultAsync<int>("Select Count(*) From Apt_Reports Where Apt_Code = @Apt_Code And Report_Bloom_Code = @Report_Bloom_Code And Report_Year = @Report_Year And Report_Month = @Report_Month", new { Apt_Code, Report_Bloom_Code, Report_Year, Report_Month });
        }

        /// <summary>
        /// 보고서 승인 여부
        /// </summary>
        /// <param name="result"></param>
        /// <param name="Aid"></param>
        public async Task Result(string result, string Aid)
        {
            using var dba = new SqlConnection(_db.GetConnectionString("sw_togather"));
            await dba.ExecuteAsync("Update Apt_Reports Set Result = @result Where Aid = @Aid ", new { result, Aid });
        }

        /// <summary>
        /// 보고서 삭제
        /// </summary>
        /// <param name="result"></param>
        /// <param name="Aid"></param>
        public async Task Delete(string complete, string Aid)
        {
            using var dba = new SqlConnection(_db.GetConnectionString("sw_togather"));
            await dba.ExecuteAsync("Update Apt_Reports Set Complete = @complete Where Aid = @Aid ", new { complete, Aid });
        }

        /// <summary>
        /// 보고서 삭제
        /// </summary>
        public async Task Remove(string Aid)
        {
            using var dba = new SqlConnection(_db.GetConnectionString("sw_togather"));
            await dba.ExecuteAsync("Delete Apt_Reports Where Aid = @Aid ", new { Aid });
        }
    }

    
    /// <summary>
    /// 보고서 분류
    /// </summary>
    public class Report_Bloom_Lib : IReport_Bloom_Lib
    {
        private readonly IConfiguration _db;
        public Report_Bloom_Lib(IConfiguration configuration)
        {
            this._db = configuration;
        }

        public async Task<Report_Bloom_Entity> add(Report_Bloom_Entity _Entity)
        {
            using var dba = new SqlConnection(_db.GetConnectionString("sw_togather"));
            var sql = "Insert into Report_Bloom (Report_Bloom_Name, Report_Bloom_Detail) Values (@Report_Bloom_Name, @Report_Bloom_Detail); Select Cast(SCOPE_IDENTITY() As Int);";
            var Aid = await dba.QuerySingleOrDefaultAsync<int>(sql, _Entity);
            _Entity.Report_Bloom_Code = Aid;
            return _Entity;
        }

        /// <summary>
        /// 보고서 수정
        /// </summary>
        /// <param name="_Entity"></param>
        /// <returns></returns>
        public async Task<Report_Bloom_Entity> Edit(Report_Bloom_Entity _Entity)
        {
            using var dba = new SqlConnection(_db.GetConnectionString("sw_togather"));
            var sql = "Update Report_Bloom Set Report_Bloom_Name = @Report_Bloom_Name, Report_Bloom_Detail = @Report_Bloom_Detail Where Report_Bloom_Code = @Report_Bloom_Code";
            await dba.ExecuteAsync(sql, _Entity);
            return _Entity;
        }

        /// <summary>
        /// 분류 보고서 리스트
        /// </summary>
        /// <param name="Apt_Code"></param>
        /// <param name="Report_Bloom_Code"></param>
        /// <returns></returns>
        public async Task<List<Report_Bloom_Entity>> GeList(string Views)
        {
            using var dba = new SqlConnection(_db.GetConnectionString("sw_togather"));
            var lst = await dba.QueryAsync<Report_Bloom_Entity>("SELECT Report_Bloom_Code, Report_Bloom_Name, Report_Bloom_Detail, PostDate, Views FROM Report_Bloom Where Views = @Views", new { Views });
            return lst.ToList();
        }

        /// <summary>
        /// 보고서 분류명 불러오기
        /// </summary>
        /// <param name="Report_Bloom_Code"></param>
        /// <returns></returns>
        public async Task<string> ReportBloom_Name(string Report_Bloom_Code)
        {
            using var dba = new SqlConnection(_db.GetConnectionString("sw_togather"));
            return await dba.QuerySingleOrDefaultAsync<string>("Select Report_Bloom_Name From Report_Bloom Where Report_Bloom_Code = @Report_Bloom_Code", new { Report_Bloom_Code });
        }

        /// <summary>
        /// 보고서 분류명 불러오기
        /// </summary>
        /// <param name="Report_Bloom_Code"></param>
        /// <returns></returns>
        public async Task<string> Report_Division_Name(string Report_Division_Code)
        {
            using var dba = new SqlConnection(_db.GetConnectionString("sw_togather"));
            return await dba.QuerySingleOrDefaultAsync<string>("Select Report_Division_Name From Report_Bloom Where Report_Division_Code = @Report_Division_Code", new { Report_Division_Code });
        }

        /// <summary>
        /// 삭제
        /// </summary>
        public async Task Views(string Aid, string Views)
        {
            using var dba = new SqlConnection(_db.GetConnectionString("sw_togather"));
            await dba.ExecuteAsync("Update Report_Bloom Set Views = @Views Where Report_Bloom_Code = @Aid", new { Aid, Views });
        }
    }



    /// <summary>
    /// 보고서 구분
    /// </summary>
    public class Report_Division_Lib : IReport_Division_Lib
    {
        private readonly IConfiguration _db;
        public Report_Division_Lib(IConfiguration configuration)
        {
            this._db = configuration;
        }

        public async Task<Report_Division_Entity> add(Report_Division_Entity _Entity)
        {
            using var dba = new SqlConnection(_db.GetConnectionString("sw_togather"));
            var sql = "Insert into Report_Division (Report_Division_Name, Report_Division_Detail) Values (@Report_Division_Name, @Report_Division_Detail); Select Cast(SCOPE_IDENTITY() As Int);";
            var Aid = await dba.QuerySingleOrDefaultAsync<int>(sql, _Entity);
            _Entity.Report_Division_Code = Aid;
            return _Entity;
        }

        /// <summary>
        /// 보고서 수정
        /// </summary>
        /// <param name="_Entity"></param>
        /// <returns></returns>
        public async Task<Report_Division_Entity> Edit(Report_Division_Entity _Entity)
        {
            using var dba = new SqlConnection(_db.GetConnectionString("sw_togather"));
            var sql = "Update Report_Division Set Report_Division_Name = @Report_Division_Name, Report_Division_Detail = @Report_Division_Detail Where Report_Division_Code = @Report_Division_Code";
            await dba.ExecuteAsync(sql, _Entity);
            return _Entity;
        }

        /// <summary>
        /// 분류 보고서 리스트
        /// </summary>
        /// <param name="Apt_Code"></param>
        /// <param name="Report_Bloom_Code"></param>
        /// <returns></returns>
        public async Task<List<Report_Division_Entity>> GeList(string Views)
        {
            using var dba = new SqlConnection(_db.GetConnectionString("sw_togather"));
            var lst = await dba.QueryAsync<Report_Division_Entity>("SELECT Report_Division_Code, Report_Division_Name, Report_Division_Detail, PostDate, Views FROM Report_Division Where Views = @Views", new { Views });
            return lst.ToList();
        }

        /// <summary>
        /// 보고서 구분명 불러오기
        /// </summary>
        /// <param name="Report_Division_Code"></param>
        /// <returns></returns>
        public async Task<string> ReportDivisin_Name(string Report_Division_Code)
        {
            using var dba = new SqlConnection(_db.GetConnectionString("sw_togather"));
            return await dba.QuerySingleOrDefaultAsync<string>("Select Report_Division_Name From Report_Division Where Report_Division_Code = @Report_Division_Code", new { Report_Division_Code });
        }

        /// <summary>
        /// 삭제
        /// </summary>
        public async Task Views(string Aid, string Views)
        {
            using var dba = new SqlConnection(_db.GetConnectionString("sw_togather"));
            await dba.ExecuteAsync("Update Report_Division Set Views = @Views Where Report_Division_Code = @Aid", new { Aid, Views });
        }
    }
}
