using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using System.Linq;

namespace Company
{
    public class Company_Career_Lib : ICompany_Career_Lib
    {
        private readonly IConfiguration _db;
        public Company_Career_Lib(IConfiguration configuration)
        {
            this._db = configuration;
        }

        /// <summary>
        /// 계약 정보 입력
        /// </summary>
        /// <param name="cc"></param>
        /// <returns></returns>
        public async Task<Company_Career_Entity> add(Company_Career_Entity cc)
        {
            var sql = "Insert into Company_Career (CC_Code, Cor_Code, Company_Name, Apt_Code, Apt_Name, ContractSort, Tender, Bid, ContractMainAgent, Contract_date, Contract_start_date, Contract_end_date, Contract_Sum, Contract_Period, Intro) Values (@CC_Code, @Cor_Code, @Company_Name, @Apt_Code, @Apt_Name, @ContractSort, @Tender, @Bid, @ContractMainAgent, @Contract_date, @Contract_start_date, @Contract_end_date, @Contract_Sum, @Contract_Period, @Intro);";
            using (var db = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                await db.ExecuteAsync(sql, cc);
                return cc;
            }
            
        }

        /// <summary>
        /// 계약 정보 수정
        /// </summary>
        /// <param name="cc"></param>
        /// <returns></returns>
        public async Task<Company_Career_Entity> edit(Company_Career_Entity cc)
        {
            var sql = "Update Company_Career Set Cor_Code = @Cor_Code, Company_Name = @Company_Name, Apt_Code = @Apt_Code, Apt_Name = @Apt_Name, ContractSort = @ContractSort, Tender = @Tender, Bid = @Bid, ContractMainAgent = @ContractMainAgent, Contract_date = @Contract_date, Contract_start_date = @Contract_start_date, Contract_end_date = @Contract_end_date, Contract_Sum = @Contract_Sum, Contract_Period = @Contract_Period, Intro = @Intro Where Aid = @Aid";
            using (var db = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                await db.ExecuteAsync(sql, cc);
                return cc;
            }

            
        }

        /// <summary>
        /// 계약 정보 상세보기
        /// </summary>
        /// <param name="CC_Code"></param>
        /// <returns></returns>
        public async Task<Company_Career_Entity> detail(string CC_Code)
        {
            using (var db = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                return await db.QuerySingleOrDefaultAsync<Company_Career_Entity>("Select * From Company_Career Where CC_Code = @CC_Code", new { CC_Code });
            }
            
        }

        /// <summary>
        /// 가장 최근 계약 정보 상세보기(위탁관리만)
        /// </summary>
        /// <param name="CC_Code"></param>
        /// <returns></returns>
        public async Task<Company_Career_Entity> detail_Apt(string Apt_Code, string ContractSort)
        {
            using (var db = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                return await db.QuerySingleOrDefaultAsync<Company_Career_Entity>("Select Top 1 Aid, CC_Code, Cor_Code, Company_Name, Apt_Code, Apt_Name, ContractSort, Bid, Tender, ContractMainAgent, Contract_date, Contract_start_date, Contract_end_date, Division, PostDate, Contract_Sum, Contract_Period, Intro, FileCount From Company_Career Where Apt_Code = @Apt_Code And ContractSort = @ContractSort Order By Aid Desc", new { Apt_Code, ContractSort });
            }
            
        }

        /// <summary>
        /// 가장 최근 계약 정보 상세보기(위탁관리만)
        /// </summary>
        /// <param name="CC_Code"></param>
        /// <returns></returns>
        public async Task<int> detail_Apt_Count(string Apt_Code, string ContractSort, string Contract_start_date, string Contract_end_date)
        {
            using (var db = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                return await db.QuerySingleOrDefaultAsync<int>("Select Count(*) From Company_Career Where Apt_Code = @Apt_Code And ContractSort = @ContractSort And Contract_start_date <= @Contract_start_date And Contract_end_date >= @Contract_end_date And Division = 'A'", new { Apt_Code, ContractSort, Contract_start_date, Contract_end_date });
            }
        }

        /// <summary>
        /// 계약 정보 전체 목록
        /// </summary>
        /// <returns></returns>
        public async Task<List<Company_Career_Entity>> getlist_all(int Page, string Division)
        {
            using (var db = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                var lst = await db.QueryAsync<Company_Career_Entity>("Select Top 15 * From Company_Career Where Aid Not In(Select Top(15 * @Page) Aid From Company_Career Where Division = @Division Order By Aid Desc) and Division = @Division Order By Aid Desc", new { Page, Division });
                return lst.ToList();
            }
            
        }

        /// <summary>
        /// 계약 정보 전체 수
        /// </summary>
        /// <param name="Division"></param>
        /// <returns></returns>
        public async Task<int> Getcount_all(string Division)
        {
            using (var db = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                return await db.QuerySingleOrDefaultAsync<int>("Select Count(*) From Company_Career Where Division = @Division", new { Division });
            }
            
        }

        /// <summary>
        /// 계약 정보 옵션 목록
        /// </summary>
        /// <returns></returns>
        public async Task<List<Company_Career_Entity>> getlist_option(int Page, string Feild, string Query)
        {
            using (var db = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                var lst = await db.QueryAsync<Company_Career_Entity>("Select Top 15 * From Company_Career Where Aid Not In(Select Top(15 * @Page) Aid From Company_Career Where " + Feild + " = @Query And Division = 'A' Order By Aid Desc) and " + Feild + " = @Query And Division = 'A' Order By Aid Desc", new { Page, Feild, Query });
                return lst.ToList();
            }
            
        }



        /// <summary>
        /// 계약 정보 옵션 목록
        /// </summary>
        /// <returns></returns>
        public async Task<List<Company_Career_Entity>> getlist(string Feild, string Query, string Division)
        {
            using (var db = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                var lst = await db.QueryAsync<Company_Career_Entity>("Select * From Company_Career Where " + Feild + " = @Query And Division = @Division Order By Aid Desc", new { Feild, Query, Division });
                return lst.ToList();
            }
            
        }
        /// <summary>
        /// 계약 정보 옵션 수
        /// </summary>
        /// <param name="Division"></param>
        /// <returns></returns>
        public async Task<int> Getcount_option(string Feild, string Query)
        {
            var sql = "Select Count(*) From Company_Career Where " + Feild + " = @Query And Division = 'A'";
            using (var db = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                return await db.QuerySingleOrDefaultAsync<int>(sql, new { Feild, Query });
            }
            
        }

        /// <summary>
        /// 계약 정보 검색된 목록
        /// </summary>
        /// <returns></returns>
        public async Task<List<Company_Career_Entity>> getlist_search(int Page, string Feild, string Query, string Division)
        {
            using (var db = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                var lst = await db.QueryAsync<Company_Career_Entity>("Select Top 15 * From Company_Career Where Aid Not In(Select Top(15 * @Page) Aid From Company_Career Where " + Feild + " Like '%" + @Query + "%' And Division = @Division Order By Aid Desc) and " + Feild + " Like '%" + @Query + "%' And Division = @Division Order By Aid Desc", new { Page, Feild, Query, Division });
                return lst.ToList();
            }
            
        }

        /// <summary>
        /// 계약 정보 검색된 수
        /// </summary>
        /// <param name="Division"></param>
        /// <returns></returns>
        public async Task<int> Getcount_search(string Feild, string Query, string Division)
        {
            using (var db = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                return await db.QuerySingleOrDefaultAsync<int>("Select Count(*) From Company_Career Where " + Feild + " Like '%" + @Query + "%' And Division = @Division", new { Feild, Query, Division });
            }
            
        }

        /// <summary>
        /// 공동주택명으로 계약정보 목록 만들기
        /// </summary>
        /// <param name="Apt_Name"></param>
        /// <returns></returns>
        public async Task<List<Company_Career_Entity>> getlist_name_search(string Apt_Name)
        {
            using (var db = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                var lst = await db.QueryAsync<Company_Career_Entity>("Select DISTINCT Apt_Name From Company_Career Where Apt_Name Like '%" + Apt_Name + "%'", new { Apt_Name });
                return lst.ToList();
            }
            
        }

        /// <summary>
        /// 식별코드로 계약정보 목록 만들기
        /// </summary>
        /// <param name="Apt_Name"></param>
        /// <returns></returns>
        public async Task<List<Company_Career_Entity>> getlist_name(string Apt_Name)
        {
            using (var db = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                var lst = await db.QueryAsync<Company_Career_Entity>("Select * From Company_Career Where Apt_Name = @Apt_Name Order By Aid Desc", new { Apt_Name });
                return lst.ToList();
            }
            
        }

        /// <summary>
        /// 공동주택명으로 찾은 수
        /// </summary>
        /// <param name="Apt_Name"></param>
        /// <returns></returns>
        public async Task<int> apt_name_count(string Apt_Name)
        {
            using (var db = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                return await db.QuerySingleOrDefaultAsync<int>("Select Count(*) From Company_Career Where Apt_Name = @Apt_Name", new { Apt_Name });
            }
            
        }

        /// <summary>
        /// 해당 년도와 월로 계약 정보 존재 여부
        /// </summary>
        /// <param name="Apt_Code"></param>
        /// <param name="Cor_Code"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        public async Task<int> be_date(string Apt_Code, string Cor_Code, string Contract_start_date, string Contract_end_date)
        {
            using (var db = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                return await db.QuerySingleOrDefaultAsync<int>("Select Count(*) From Company_Career Where Apt_Code = @Apt_Code And Cor_Code = @Cor_Code And Contract_start_date >= @Contract_start_date And Contract_end_date <= @Contract_end_date And Division = 'A'", new { Apt_Code, Cor_Code, Contract_start_date, Contract_end_date });
            }
            
        }

        /// <summary>
        /// 해당 공동주택 신원주택 위탁계약 정보 존재 여부 확인
        /// </summary>
        /// <param name="Apt_Code"></param>
        /// <param name="Cor_Code"></param>
        /// <returns></returns>
        public async Task<int> BeApt(string Apt_Code, string Cor_Code)
        {
            using (var db = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                return await db.QuerySingleOrDefaultAsync<int>("Select Count(*) From Company_Career Where Apt_Code = @Apt_Code And Cor_Code = @Cor_Code And DATEDIFF (DD, GETDATE(), Contract_End_date) >= 1", new { Apt_Code, Cor_Code });
            }
            
        }

        /// <summary>
        /// 위탁계약 코드 찾아 오기
        /// </summary>
        /// <param name="Apt_Code"></param>
        /// <param name="Cor_Code"></param>
        /// <returns></returns>
        public async Task<Company_Career_Entity> BeAptCompany_Code(string Apt_Code, string Cor_Code)
        {
            using (var db = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                return await db.QuerySingleOrDefaultAsync<Company_Career_Entity>("Select Top 1 Aid, CC_Code, Cor_Code, Company_Name, Apt_Code, Apt_Name, ContractSort, Bid, Tender, ContractMainAgent, Contract_date, Contract_start_date, Contract_end_date, Division, PostDate, Contract_Sum, Contract_Period, Intro, FileCount From Company_Career Where Apt_Code = @Apt_Code And Cor_Code = @Cor_Code And DATEDIFF (DD, GETDATE(), Contract_End_date) >= 1 Order By Aid Desc", new { Apt_Code, Cor_Code });
            }
            
        }

        /// <summary>
        /// 해당 식별코드로 계약 정보 존재 여부
        /// </summary>
        /// <param name="Apt_Code"></param>
        /// <param name="Cor_Code"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        public async Task<int> be_Code(int Aid)
        {
            using (var db = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                return await db.QuerySingleOrDefaultAsync<int>("Select Count(*) From Company_Career Where Aid = @Aid", new { Aid });
            }
            
        }

        /// <summary>
        /// 삭제
        /// </summary>
        /// <param name="Aid"></param>
        public async Task delete(string Aid, string Division)
        {
            using (var db = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                await db.ExecuteAsync("Update Company_Career Set Division = @Division Where Aid = @Aid", new { Aid, Division });
            }
            
        }

        /// <summary>
        /// 계약일자 목록
        /// </summary>
        /// <param name="Apt_Code"></param>
        /// <returns></returns>
        public async Task<List<Company_Career_Entity>> ListDrop(string Apt_Code, string ContractSort_Code)
        {
            using (var db = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                var lst = await db.QueryAsync<Company_Career_Entity>("Select a.CC_Code, CONVERT(Varchar, a.Contract_end_date, 23) as End_Date  from Company_Career as a Join Contract_Sort as b on a.ContractSort = b.ContractSort_Code where -60 < (Select DATEDIFF (DD, GETDATE(), a.Contract_end_date)) And a.Apt_Code = @Apt_Code And a.ContractSort = @ContractSort_Code Order By a.Aid Desc", new { Apt_Code, ContractSort_Code });
                return lst.ToList();
            }
            
        }

        /// <summary>
        /// 마지막 일련번호
        /// </summary>
        /// <returns></returns>
        public async Task<string> LastCount()
        {
            using (var db = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                return await db.QuerySingleOrDefaultAsync<string>("Select Top 1 Aid From Company_Career Order By Aid Desc");
            }
            
        }

        /// <summary>
        /// 지나온 날짜 계산
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public int Date_scomp(string start, string end)
        {
            using var df = new SqlConnection(_db.GetConnectionString("sw_togather"));
            return df.QuerySingleOrDefault<int>("Select DATEDIFF(DD, @start, @end)", new { start, end });

        }

        /// <summary>
        /// 첨부파일 추가
        /// </summary>
        public async Task File_Plus(int Aid)
        {
            using var df = new SqlConnection(_db.GetConnectionString("sw_togather"));
            await df.ExecuteAsync("Update Company_Career Set FileCount = FileCount + 1 Where Aid = @Aid", new { Aid });
        }

        /// <summary>
        /// 첨부파일 추가
        /// </summary>
        public async Task File_Minus(int Aid)
        {
            using var df = new SqlConnection(_db.GetConnectionString("sw_togather"));
            await df.ExecuteAsync("Update Company_Career Set FileCount = FileCount - 1 Where Aid = @Aid", new { Aid });
        }
    }
}
