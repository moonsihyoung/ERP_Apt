using Erp_Lib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;

namespace Erp_Apt_Lib
{
    public class Notice_Entity
    {
        /// <summary>
        /// 식별코드
        /// </summary>
        public int Aid { get; set; }

        /// <summary>
        /// 방송안내 번호
        /// </summary>
        public string NoticeNum { get; set; }

        /// <summary>
        /// 공동주택 식별코드
        /// </summary>
        public string AptCode { get; set; }

        /// <summary>
        /// 공동주택명
        /// </summary>
        public string AptName { get; set; }

        /// <summary>
        /// 제목
        /// </summary>
        public string NoticeTitle { get; set; }

        /// <summary>
        /// 내용
        /// </summary>
        public string NoticeContent { get; set; }

        /// <summary>
        /// 구분(방송 혹은 공고)
        /// </summary>
        public string Division { get; set; }

        /// <summary>
        /// 부서
        /// </summary>
        public string Post { get; set; }

        /// <summary>
        /// 안내 대상
        /// </summary>
        public string NoticeTarget { get; set; }

        /// <summary>
        /// 작성자
        /// </summary>
        public string WorkMan { get; set; }

        /// <summary>
        /// 분류
        /// </summary>
        public string NoticeSort { get; set; }

        /// <summary>
        /// 작정자 아이디
        /// </summary>
        public string UserCode { get; set; }

        /// <summary>
        /// 기간
        /// </summary>
        public string Period { get; set; }

        /// <summary>
        /// 시행일자
        /// </summary>
        public DateTime AcceptDate { get; set; }

        /// <summary>
        /// 시행년도
        /// </summary>
        public int AcceptYear { get; set; }

        /// <summary>
        /// 공개 여부
        /// </summary>
        public string OpenPublic { get; set; }

        /// <summary>
        /// 결재 여부
        /// </summary>
        public string Approval { get; set; }

        /// <summary>
        /// 작성일
        /// </summary>
        public string PostDate { get; set; }

        /// <summary>
        /// 작정자 아이피
        /// </summary>
        public string PostIP { get; set; }

        /// <summary>
        /// 기타
        /// </summary>
        public string Etc { get; set; }

        public int Files_Count { get; set; }

    }

    public interface INotice_Lib
    {
        Task<int> Add(Notice_Entity nt);
        Task Edit(Notice_Entity nt);
        Task<List<Notice_Entity>> GetListApt(string AptCode);
        Task<int> GetListAptCount(string AptCode);
        Task<int> GetListAptYearCount(string AptCode, string Division, int Year);
        Task<List<Notice_Entity>> GetList(int Page, string AptCode);
        Task<int> GetListCount(string AptCode);
        Task<List<Notice_Entity>> SearchList(int Page, string AptCode, string Field, string Query);
        Task<int> SearchListCount(string AptCode, string Field, string Query);
        Task NoticeApproval(int Aid);
        Task<Notice_Entity> Details(int Aid);
        Task Remove(int Aid);
        Task<string> Ago(string AptCode, string Aid);
        Task<int> AgoBe(string AptCode, string Aid);
        Task<string> Next(string AptCode, string Aid);
        Task<int> NextBe(string AptCode, string Aid);
        Task<List<Notice_Entity>> SearchList_Two(int Page, string AptCode, string Field_a, string Query_a, string Field_b, string Query_b);
        Task<int> SearchListCount_Two(string AptCode, string Field_a, string Query_a, string Field_b, string Query_b);
        Task FilesCount(int Aid, string division);
        Task<List<Notice_Entity>> SearchYear(string AptCode);
    }

    public class Notice_Lib : INotice_Lib
    {
        private readonly IConfiguration _db;
        public Notice_Lib(IConfiguration configuration)
        {
            this._db = configuration;
        }

        /// <summary>
        /// 방송 및 공고 입력
        /// </summary>
        /// <param name="nt"></param>
        /// <returns></returns>
        public async Task<int> Add(Notice_Entity nt)
        {
            int a = 0;
            var sql = "Insert Notice (NoticeNum, AptCode, AptName, NoticeTitle, NoticeContent, Division, NoticeSort, Post, NoticeTarget, UserCode, WorkMan, AcceptDate, AcceptYear, Period, OpenPublic, PostIP, Etc) Values (@NoticeNum, @AptCode, @AptName, @NoticeTitle, @NoticeContent, @Division, @NoticeSort, @Post, @NoticeTarget, @UserCode, @WorkMan, @AcceptDate, @AcceptYear, @Period, @OpenPublic, @PostIP, @Etc); Select Cast(SCOPE_IDENTITY() As Int);";
            using (var df = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                a = await df.QuerySingleOrDefaultAsync<int>(sql, nt);
                return a;
            }
            
        }

        /// <summary>
        /// 방송 및 공고 수정
        /// </summary>
        /// <param name="nt"></param>
        public async Task Edit(Notice_Entity nt)
        {
            using (var df = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                await df.ExecuteAsync("Update Notice Set NoticeTitle = @NoticeTitle, NoticeContent = @NoticeContent, NoticeTarget = @NoticeTarget, UserCode = @UserCode, WorkMan = @WorkMan, AcceptDate = @AcceptDate, AcceptYear = @AcceptYear, Period = @Period, PostIP = @PostIP, Etc = @Etc Where Aid = @Aid", nt);
            }            
        }

        /// <summary>
        /// 방송 및 안내 단지 별 목록
        /// </summary>
        /// <param name="AptCode"></param>
        /// <returns></returns>
        public async Task<List<Notice_Entity>> GetListApt(string AptCode)
        {
            using (var df = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                var lst = await df.QueryAsync<Notice_Entity>("Select Aid, NoticeNum, AptCode, AptName, NoticeTitle, NoticeContent, Division, NoticeSort, Post, NoticeTarget, UserCode, WorkMan, AcceptDate, AcceptYear, Period, OpenPublic, Approval, PostDate, PostIP, Etc, Files_Count From From Notice Where AptCode = @AptCode And Del = 'A' Order By Aid Desc", new { AptCode });
                return lst.ToList();
            }            
        }

        /// <summary>
        /// 방송 및 안내 단지별 목록 수
        /// </summary>
        /// <param name="AptCode"></param>
        /// <returns></returns>
        public async Task<int> GetListAptCount(string AptCode)
        {
            using (var df = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                return await df.QuerySingleOrDefaultAsync<int>("Select Count(*) From Notice Where AptCode = @AptCode And Del = 'A'", new { AptCode });
            }            
        }

        /// <summary>
        /// 방송 및 안내 단지별 해당 년도 목록 수
        /// </summary>
        public async Task<int> GetListAptYearCount(string AptCode, string Division, int Year)
        {
            using (var df = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                return await df.QuerySingleOrDefaultAsync<int>("Select Count(*) From Notice Where AptCode = @AptCode And Division = @Division And AcceptYear = @Year", new { AptCode, Division, Year });
            }
            
        }

        // <summary>
        /// 방송 및 안내 전체 목록
        /// </summary>
        public async Task<List<Notice_Entity>> GetList(int Page, string AptCode)
        {
            using (var df = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                var lst = await df.QueryAsync<Notice_Entity>("Select Top 15 Aid, NoticeNum, AptCode, AptName, NoticeTitle, NoticeContent, Division, NoticeSort, Post, NoticeTarget, UserCode, WorkMan, AcceptDate, AcceptYear, Period, OpenPublic, Approval, PostDate, PostIP, Etc, Files_Count From Notice Where Aid Not In (Select Top (15 * @Page) Aid From Notice Where AptCode = @AptCode and Del = 'A' Order By Aid Desc) and AptCode = @AptCode And Del = 'A' Order By Aid Desc", new { Page, AptCode });
                return lst.ToList();
            }
            
        }

        /// <summary>
        /// 방송 및 안내 목록 수
        /// </summary>
        /// <param name="AptCode"></param>
        /// <returns></returns>
        public async Task<int> GetListCount(string AptCode)
        {
            using (var df = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                return await df.QuerySingleOrDefaultAsync<int>("Select Count(*) From Notice Where AptCode = @AptCode And Del = 'A'", new { AptCode });
            }            
        }

        /// <summary>
        /// 방송 및 안내 검색된 목록
        /// </summary>
        /// <param name="Field"></param>
        /// <param name="Query"></param>
        /// <returns></returns>
        public async Task<List<Notice_Entity>> SearchList(int Page, string AptCode, string Field, string Query)
        {
            using (var df = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                var lst = await df.QueryAsync<Notice_Entity>("Select Top 15 Aid, NoticeNum, AptCode, AptName, NoticeTitle, NoticeContent, Division, NoticeSort, Post, NoticeTarget, UserCode, WorkMan, AcceptDate, AcceptYear, Period, OpenPublic, Approval, PostDate, PostIP, Etc, Files_Count From Notice Where Aid Not In (Select Top (15 * @Page) Aid From Notice Where AptCode = @AptCode and Del = 'A' And " + Field + " Like '%" + @Query + "%' Order By Aid Desc) and AptCode = @AptCode And Del = 'A' And " + Field + " Like '%" + @Query + "%' Order By Aid Desc", new { Page, AptCode, Field, Query });
                return lst.ToList();
            }
            
        }

        /// <summary>
        /// 방송 및 안내 검색된 목록 수
        /// </summary>
        public async Task<int> SearchListCount(string AptCode, string Field, string Query)
        {
            using (var df = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                return await df.QuerySingleOrDefaultAsync<int>("Select Count(*) From Notice Where AptCode = @AptCode And Del = 'A' And " + Field + " Like '%" + @Query + "%'", new { AptCode, Field, Query });
            }
            
        }

        /// <summary>
        /// 방송 및 안내 결재 완료
        /// </summary>
        /// <param name="Aid"></param>
        public async Task NoticeApproval(int Aid)
        {
            using (var df = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
               await df.ExecuteAsync("Update Notice Set Approval = 'B' Where Aid = @Aid", new { Aid });
            }
            
        }

        /// <summary>
        /// 방송 및 안내 상세
        /// </summary>
        /// <param name="Aid"></param>
        /// <returns></returns>
        public async Task<Notice_Entity> Details(int Aid)
        {
            using (var df = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                return await df.QuerySingleOrDefaultAsync<Notice_Entity>("Select Aid, NoticeNum, AptCode, AptName, NoticeTitle, NoticeContent, Division, NoticeSort, Post, NoticeTarget, UserCode, WorkMan, AcceptDate, Period, OpenPublic, Approval, PostDate, PostIP, Etc, Files_Count From Notice Where Aid = @Aid", new { Aid });
            }
            
        }

        /// <summary>
        /// 방송 및 공고 삭제
        /// </summary>
        /// <param name="Aid"></param>
        public async Task Remove(int Aid)
        {
            using (var df = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                await df.ExecuteAsync("Update Notice Set Del = 'B' Where Aid = @Aid", new { Aid });
            }
            
        }

        /// <summary>
        // 앞 업무 정보
        /// </summary>
        /// <param name="AptCode"></param>
        /// <param name="Num"></param>
        /// <returns></returns>
        public async Task<string> Ago(string AptCode, string Aid)
        {
            using (var df = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                return await df.QuerySingleOrDefaultAsync<string>("Select ISNULL(Aid, 0) From Notice Where AptCode = @AptCode and Aid = (Select max(Aid) From Notice Where AptCode = @AptCode and Aid < @Aid)", new { AptCode, Aid });
            }
            
        }

        /// <summary>
        // 앞 업무  존재여부
        /// </summary>
        /// <param name="AptCode"></param>
        /// <param name="Num"></param>
        /// <returns></returns>
        public async Task<int> AgoBe(string AptCode, string Aid)
        {
            using (var df = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                return await df.QuerySingleOrDefaultAsync<int>("Select Count(*) From Notice Where AptCode = @AptCode and Aid = (Select max(Aid) From Notice Where AptCode = @AptCode and Aid < @Aid)", new { AptCode, Aid });
            }
            
        }

        /// <summary>
        // 뒤 업무
        /// </summary>
        /// <param name="AptCode"></param>
        /// <param name="Num"></param>
        /// <returns></returns>
        public async Task<string> Next(string AptCode, string Aid)
        {
            using (var df = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                return await df.QuerySingleOrDefaultAsync<string>("Select ISNULL(Aid, 0) From Notice Where AptCode = @AptCode and Aid = (Select Min(Aid) From Notice Where AptCode =@AptCode and Aid > @Aid)", new { AptCode, Aid });
            }
            
        }

        /// <summary>
        // 뒤 업무 존재 여부
        /// </summary>
        /// <param name="AptCode"></param>
        /// <param name="Num"></param>
        /// <returns></returns>
        public async Task<int> NextBe(string AptCode, string Aid)
        {
            using (var df = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                return await df.QuerySingleOrDefaultAsync<int>("Select Count(*) From Notice Where AptCode = @AptCode and Aid = (Select Min(Aid) From Notice Where AptCode =@AptCode and Aid > @Aid)", new { AptCode, Aid });
            }
            
        }

        /// <summary>
        /// 이중 조건 검색
        /// </summary>
        public async Task<List<Notice_Entity>> SearchList_Two(int Page, string AptCode, string Field_a, string Query_a, string Field_b, string Query_b)
        {
            using (var df = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                var sql = "Select Top 15 Aid, NoticeNum, AptCode, AptName, NoticeTitle, NoticeContent, Division, NoticeSort, Post, NoticeTarget, UserCode, WorkMan, AcceptDate, AcceptYear, Period, OpenPublic, Approval, PostDate, PostIP, Etc, Files_Count From Notice Where Aid Not In (Select Top (15 * @Page) Aid From Notice Where AptCode = @AptCode and Del = 'A' And " + Field_a + " Like '%" + @Query_a + "%' And " + Field_b + " Like '%" + @Query_b + "%' Order By Aid Desc) and AptCode = @AptCode And Del = 'A' And " + Field_a + " Like '%" + @Query_a + "%' And " + Field_b + " Like '%" + @Query_b + "%' Order By Aid Desc";
                var lst = await df.QueryAsync<Notice_Entity>(sql, new { Page, AptCode, Field_a, Query_a, Field_b, Query_b });
                return lst.ToList();
            }
        }

        /// <summary>
        /// 입력년도
        /// </summary>
        public async Task<List<Notice_Entity>> SearchYear(string AptCode)
        {
            using (var df = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                var lst = await df.QueryAsync<Notice_Entity>("Select distinct AcceptYear From Notice Where AptCode = @AptCode Order By AcceptYear Desc", new { AptCode });
                return lst.ToList();
            }
        }

        /// <summary>
        /// 이중 조건으로 검색된 수
        /// </summary>
        public async Task<int> SearchListCount_Two(string AptCode, string Field_a, string Query_a, string Field_b, string Query_b)
        {
            using (var df = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                return await df.QuerySingleOrDefaultAsync<int>("Select Count(*) From Notice Where AptCode = @AptCode And Del = 'A' And " + Field_a + " Like '%" + @Query_a + "%' And " + Field_b + " Like '%" + @Query_b + "%'", new { AptCode, Field_a, Query_a, Field_b, Query_b });
            }
        }

        /// <summary>
        /// 파일 증가 혹은 감소
        /// </summary>
        public async Task FilesCount(int Aid, string division)
        {
            using (var df = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                if (division == "A")
                {
                    await df.ExecuteAsync("Update Notice Set Files_Count = Files_Count + 1 Where Aid = @Aid", new { Aid, division });
                }
                else
                {
                    await df.ExecuteAsync("Update Notice Set Files_Count = Files_Count - 1 Where Aid = @Aid", new { Aid, division });
                }
            }
        }
    }
}
