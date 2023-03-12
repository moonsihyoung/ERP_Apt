using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Erp_Apt_Lib.Community
{
    public class Community_Lib :ICommunity_Lib
    {
        private readonly IConfiguration _db;
        public Community_Lib(IConfiguration configuration)
        {
            this._db = configuration;
        }

        public async Task<int> Add(Community_Entity cn)
        {
            var sql = "Insert into Community (AptCode, AptName, UserCode, UserName, Dong, Ho, Mobile, Relation, Division, UsingKindName, UsingKindCode, Ticket, Ticket_Code, UserStartDate, UserEndDate, UserStartHour, UserEndHour, ScamDays, UseCost, Etc, PostIP, User_Code, Mobile_Use, Approval, OrderBy) Values (@AptCode, @AptName, @UserCode, @UserName, @Dong, @Ho, @Mobile, @Relation, @Division, @UsingKindName, @UsingKindCode, @Ticket, @Ticket_Code, @UserStartDate, @UserEndDate, @UserStartHour, @UserEndHour, @ScamDays, @UseCost, @Etc, @PostIP, @User_Code, @Mobile_Use, @Approval, @OrderBy); Select Cast(SCOPE_IDENTITY() As Int);";
            using (var db = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                int Being = await db.QuerySingleOrDefaultAsync<int>(sql, cn);
                return Being;
            }
        }

        public async Task Edit(Community_Entity cn)
        {
            var sql = "Update Community Set Division = @Division, UserCode = @UserCode, UsingKindName = @UsingKindName, UsingKindCode = @UsingKindCode, Ticket = @Ticket, Ticket_Code = @Ticket_Code, UserStartDate = @UserStartDate, UserEndDate = @UserEndDate, UserStartHour = @UserStartHour, UserEndHour = @UserEndHour, ScamDays = @ScamDays, UseCost = @UseCost, Etc = @Etc, PostIP = @PostIP, User_Code = @User_Code, Approval = @Approval, OrderBy = @OrderBy Where Aid = @Aid";
            using var db = new SqlConnection(_db.GetConnectionString("sw_togather"));
            await db.ExecuteAsync(sql, cn);
        }

        public async Task<List<Community_Entity>> GetList(int Page)
        {
            var sql = "Select Top 15 * From Community Where Aid Not In(Select Top(15 * @Page) Aid From Community Where Approval = 'B' Order By Aid Desc) And Approval = 'B' And Del = 'A' Order By Aid Desc";
            using (var db = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                var lst = await db.QueryAsync<Community_Entity>(sql, new { Page });
                return lst.ToList();
            }
        }

        public async Task<int> GetListCount()
        {
            using (var db = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                return await db.QuerySingleOrDefaultAsync<int>("Select Count(*) From Community And Approval = 'B' And Del = 'A'");
            }
        }

        public async Task<List<Community_Entity>> GetListApt(int Page, string AptCode)
        {
            var sql = "Select Top 15 * From Community Where Aid Not In(Select Top(15 * @Page) Aid From Community Where AptCode = @AptCode And Approval = 'B' And Del = 'A' Order By Aid Desc) And AptCode = @AptCode And Approval = 'B' And Del = 'A' Order By Aid Desc";
            using (var db = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                var lst = await db.QueryAsync<Community_Entity>(sql, new { Page, AptCode });
                return lst.ToList();
            }
        }

        public async Task<int> GetListCountApt(string AptCode)
        {
            using (var db = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                return await db.QuerySingleOrDefaultAsync<int>("Select Count(*) From Community Where AptCode = @AptCode And Approval = 'B' And Del = 'A'", new { AptCode });
            }
        }

        public async Task<List<Community_Entity>> GetListApt_NewList(int Page, string AptCode) //GetListApt_NewList
        {
            var sql = "Select Top 15 * From Community Where Aid Not In(Select Top(15 * @Page) Aid From Community Where AptCode = @AptCode And Approval = 'A' And Del = 'A' Order By Aid Desc) And AptCode = @AptCode And Approval = 'A' And Del = 'A' Order By Aid Desc";
            using (var db = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                var lst = await db.QueryAsync<Community_Entity>(sql, new { Page, AptCode });
                return lst.ToList();
            }
        }

        public async Task<int> GetListCountApt_NewList(string AptCode)
        {
            using (var db = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                return await db.QuerySingleOrDefaultAsync<int>("Select Count(*) From Community Where AptCode = @AptCode And Approval = 'A' And Del = 'A'", new { AptCode });
            }
        }

        /// <summary>
        /// 신청자 정보 검색 목록(호)
        /// </summary>
        public async Task<List<Community_Entity>> GetListApt_NewList_Sa_Ho(int Page, string AptCode, string Dong, string Ho) 
        {
            var sql = "Select Top 15 * From Community Where Aid Not In(Select Top(15 * @Page) Aid From Community Where AptCode = @AptCode And Dong = @Dong And Ho = @Ho And Approval = 'A' And Del = 'A' Order By Aid Desc) And AptCode = @AptCode And Dong = @Dong And Ho = @Ho And Approval = 'A' And Del = 'A' Order By Aid Desc";
            using (var db = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                var lst = await db.QueryAsync<Community_Entity>(sql, new { Page, AptCode, Dong, Ho });
                return lst.ToList();
            }
        }

        /// <summary>
        /// 신청자 정보 검색 목록 수(호)
        /// </summary>
        public async Task<int> GetListCountApt_NewList_Sa_Ho(string AptCode, string Dong, string Ho)
        {
            using (var db = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                return await db.QuerySingleOrDefaultAsync<int>("Select Count(*) From Community Where AptCode = @AptCode And Dong = @Dong And Ho = @Ho And Approval = 'A' And Del = 'A'", new { AptCode, Dong, Ho });
            }
        }

        /// <summary>
        /// 신청자 정보 검색 목록(이용장소)
        /// </summary>
        public async Task<List<Community_Entity>> GetListApt_NewList_Sa(int Page, string AptCode, string UsingKindCode)
        {
            var sql = "Select Top 15 * From Community Where Aid Not In(Select Top(15 * @Page) Aid From Community Where AptCode = @AptCode And UsingKindCode = @UsingKindCode And Approval = 'A' And Del = 'A' Order By Aid Desc) And AptCode = @AptCode And UsingKindCode = @UsingKindCode And Approval = 'A' And Del = 'A' Order By Aid Desc";
            using (var db = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                var lst = await db.QueryAsync<Community_Entity>(sql, new { Page, AptCode, UsingKindCode });
                return lst.ToList();
            }
        }

        /// <summary>
        /// 신청자 정보 검색 목록 수(이용장소)
        /// </summary>
        public async Task<int> GetListCountApt_NewList_Sa(string AptCode, string UsingKindCode)
        {
            using (var db = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                return await db.QuerySingleOrDefaultAsync<int>("Select Count(*) From Community Where AptCode = @AptCode And UsingKindCode = @UsingKindCode And Approval = 'A' And Del = 'A'", new { AptCode, UsingKindCode });
            }
        }

        /// <summary>
        /// 신청자 정보 검색 목록(이용방법)
        /// </summary>
        public async Task<List<Community_Entity>> GetListApt_NewList_Sb(int Page, string AptCode, string UsingKindCode, string Ticket_Code)
        {
            var sql = "Select Top 15 * From Community Where Aid Not In (Select Top(15 * @Page) Aid From Community Where AptCode = @AptCode And UsingKindCode = @UsingKindCode And Ticket_Code = @Ticket_Code And Approval = 'A' And Del = 'A' Order By Aid Desc) And AptCode = @AptCode And UsingKindCode = @UsingKindCode And Ticket_Code = @Ticket_Code And Approval = 'A' And Del = 'A' Order By Aid Desc";
            using (var db = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                var lst = await db.QueryAsync<Community_Entity>(sql, new { Page, AptCode, UsingKindCode, Ticket_Code });
                return lst.ToList();
            }
        }

        /// <summary>
        /// 신청자 정보 검색 목록 수(이용방법)
        /// </summary>
        public async Task<int> GetListCountApt_NewList_Sb(string AptCode, string UsingKindCode, string Ticket_Code)
        {
            using (var db = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                return await db.QuerySingleOrDefaultAsync<int>("Select Count(*) From Community Where AptCode = @AptCode And UsingKindCode = @UsingKindCode And Ticket_Code = @Ticket_Code And Approval = 'A' And Del = 'A'", new { AptCode, UsingKindCode, Ticket_Code });
            }
        }

        /// <summary>
        /// 스크린골프 신청자 명단
        /// </summary>
        public async Task<List<Community_Entity>> GetListApt_Golf_NewList(int Page, string AptCode) //GetListApt_NewList
        {
            var sql = "Select Top 15 * From Community Where Aid Not In(Select Top(15 * @Page) Aid From Community Where AptCode = @AptCode And UsingKindCode = 'Kd4' And Ticket_Code = 'Tc18' And Approval = 'A' And Del = 'A' Order By Aid Desc) And AptCode = @AptCode And UsingKindCode = 'Kd4' And Ticket_Code = 'Tc18' And Approval = 'A' And Del = 'A' Order By Aid Desc";
            using (var db = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                var lst = await db.QueryAsync<Community_Entity>(sql, new { Page, AptCode });
                return lst.ToList();
            }
        }

        /// <summary>
        /// 스크린 골프 신청자 명단 수
        /// </summary>
        public async Task<int> GetListCountApt_Golf_NewList(string AptCode)
        {
            using (var db = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                return await db.QuerySingleOrDefaultAsync<int>("Select Count(*) From Community Where AptCode = @AptCode And UsingKindCode = 'Kd4' And Ticket_Code = 'Tc18' And Approval = 'A' And Del = 'A'", new { AptCode });
            }
        }

        public async Task<List<Community_Entity>> GetListSearch(int Page, string AptCode, string Field, string Query)
        {
            var sql = "Select Top 15 * From Community Where Aid Not In(Select Top(15 * @Page) Aid From Community Where AptCode = @AptCode And " + Field + " Like '%" + Query + "%' And Approval = 'B' And Del = 'A' Order By Aid Desc) And AptCode = @AptCode And " + Field + " Like '%" + Query + "%' And Approval = 'B' And Del = 'A' Order By Aid Desc";
            using (var db = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                var lst = await db.QueryAsync<Community_Entity>(sql, new { Page, AptCode, Field, Query });
                return lst.ToList();
            }
        }

        public async Task<int> GetListCountSearch(string AptCode, string Field, string Query)
        {
            using (var db = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                return await db.QuerySingleOrDefaultAsync<int>("Select Count(*) From Community Where AptCode = @AptCode And " + Field + " Like '%" + Query + "%' And Approval = 'B' And Del = 'A'", new { AptCode, Field, Query });
            }
        }

        public async Task<List<Community_Entity>> GetListSearchDongHo(int Page, string AptCode, string Dong, string Ho, string StartDate, string EndDate)
        {
            var sql = "Select Top 15 * From Community Where AptCode = @AptCode And Dong = @Dong And Ho = @Ho And UserStartDate > @StartDate And UserEndDate <= @EndDate And Approval = 'B' And Del = 'A' Order By Aid Desc";
            using (var db = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                var lst = await db.QueryAsync<Community_Entity>(sql, new { Page, AptCode, Dong, Ho, StartDate, EndDate });
                return lst.ToList();
            }
        }

        /// <summary>
        /// 동호 해당월 사용내역
        /// </summary>
        public async Task<List<Community_Entity>> GetListDongHoDate(string AptCode, string Dong, string Ho, string StartDate, string EndDate)
        {
            var sql = "Select Top 30 * From Community Where AptCode = @AptCode And Dong = @Dong And Ho = @Ho And UserStartDate >= @StartDate And UserEndDate <= @EndDate And Approval = 'B' And Del = 'A' Order By Aid Asc";
            using (var db = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                var lst = await db.QueryAsync<Community_Entity>(sql, new { AptCode, Dong, Ho, StartDate, EndDate });
                return lst.ToList();
            }
        }


        /// <summary>
        /// 세대 정보 불러오기 2022
        /// </summary>
        public async Task<List<Community_Entity>> GetListDongHo(string AptCode, string Dong, string Ho)
        {
            var sql = "Select Top 20 * From Community Where AptCode = @AptCode And Dong = @Dong And Ho = @Ho And Del = 'A' Order By Aid Desc";
            using (var db = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                var lst = await db.QueryAsync<Community_Entity>(sql, new { AptCode, Dong, Ho });
                return lst.ToList();
            }
        }

        /// <summary>
        /// 이용자 이용정보 불러오기 2022
        /// </summary>
        public async Task<List<Community_Entity>> GetListDongHo_Personal(string AptCode, string Dong, string Ho, string Name)
        {
            var sql = "Select Top 10 * From Community Where AptCode = @AptCode And Dong = @Dong And Ho = @Ho And UserName = @Name And Del = 'A' Order By Aid Desc";
            using (var db = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                var lst = await db.QueryAsync<Community_Entity>(sql, new { AptCode, Dong, Ho, Name });
                return lst.ToList();
            }
        }

        public async Task<int> GetListCountSearchDongHo(string AptCode, string Dong, string Ho, string StartDate, string EndDate)
        {
            using (var db = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                return await db.QuerySingleOrDefaultAsync<int>("Select Count(*) From Community Where AptCode = @AptCode And Dong = @Dong And Ho = @Ho And UserStartDate > @StartDate And UserEndDate <= @EndDate And Approval = 'B' And Del = 'A'", new { AptCode, Dong, Ho, StartDate, EndDate });
            }
        }

        public async Task<Community_Entity> Details(int Aid)
        {
            using (var db = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                return await db.QuerySingleOrDefaultAsync<Community_Entity>("Select * From Community Where Aid = @Aid And Approval = 'B' And Del = 'A'", new { Aid });
            }
        }

        public async Task Remove(int Aid)
        {
            using (var db = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                string re = await db.QuerySingleOrDefaultAsync<string>("Select Del From Community Where Aid = @Aid", new { Aid });
                if (re == "A")
                {
                    await db.ExecuteAsync("Update Community Set Del = 'B' Where Aid = @Aid", new {Aid});
                }
                else
                {
                    await db.ExecuteAsync("Update Community Set Del = 'A' Where Aid = @Aid", new { Aid });
                }
                //await db.ExecuteAsync("Delete Community Where Aid = @Aid", new { Aid }); 2022-10-31
            }
        }

        /// <summary>
        /// 시설별 이용자 수
        /// </summary>
        public async Task<int> KindCount(string AptCode, string Query, string StartDate, string EndDate)
        {
            using var db = new SqlConnection(_db.GetConnectionString("sw_togather"));

            var sql = "Select Count(*) From Community Where AptCode = '" + AptCode + "' And UsingKindName = '" + Query + "' And UserStartDate >= '" + StartDate + "' And UserEndDate <= '" + EndDate + "' And Approval = 'B' And Del = 'A'";
            return await db.QuerySingleOrDefaultAsync<int>(sql);
        }

        /// <summary>
        /// 시설별 합계 금액
        /// </summary>
        public async Task<int> KindSum(string AptCode, string Query, string StartDate, string EndDate)
        {
            using (var db = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                //int Month = DateTime.Now.Month;
                //int Year = DateTime.Now.Year;
                //string dt = Year + "-" + Month + "-01";
                var sql = "Select ISNULL(Sum(UseCost), 0) From Community Where AptCode = '" + AptCode + "' And UsingKindName = '" + Query + "' And UserStartDate >= '" + StartDate + "' And UserEndDate <= '" + EndDate + "' And Approval = 'B' And Del = 'A'";

                return await db.QuerySingleOrDefaultAsync<int>(sql);
            }
        }

        /// <summary>
        /// 월 총액
        /// </summary>
        public async Task<int> MonthSum(string AptCode, string StartDate, string EndDate)
        {
            using (var db = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                //int Month = DateTime.Now.Month;
                //int Year = DateTime.Now.Year;
                //string dt = Year + "-" + Month + "-01";
                var sql = "Select ISNULL(Sum(UseCost), 0) From Community Where AptCode = '" + AptCode + "' And UserStartDate >= '" + StartDate + "' And UserEndDate <= '" + EndDate + "' And Approval = 'B' And Del = 'A'";

                return await db.QuerySingleOrDefaultAsync<int>(sql);
            }
        }

        /// <summary>
        /// 파일 추가 삭제
        /// </summary>
        public async Task FilesCount(int Aid, string Sort)
        {
            using (var db = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                if (Sort == "A")
                {
                    await db.ExecuteAsync("Update Community Set FilesCount = Files + 1 Where Aid = @Aid And Approval = 'B' And Del = 'A'", new { Aid, Sort });
                }
                else
                {
                    await db.ExecuteAsync("Update Community Set FilesCount = Files + 1 Where Aid = @Aid And Approval = 'B' And Del = 'A'", new { Aid, Sort });
                }                
            }
        }

        /// <summary>
        /// 동호로 해당월 검색 목록
        /// </summary>
        public async Task<List<Community_Entity>> Search_Month_List(int Page, string AptCode, string StartDate, string EndDate)
        {
            var sql = "Select Top 15 * From Community Where Aid Not In(Select Top(15 * @Page) Aid From Community Where AptCode = @AptCode And UserEndDate <= '" + @EndDate + "' And UserStartDate >= '" + @StartDate + "' And Del = 'A' Order By Aid Desc) And AptCode = @AptCode And UserEndDate <= '" + @EndDate + "' And UserStartDate >= '" + @StartDate + "' And Approval = 'B' And Del = 'A' Order By Aid Desc";
            using (var db = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                var lst = await db.QueryAsync<Community_Entity>(sql, new { Page, AptCode, StartDate, EndDate });
                return lst.ToList();
            }
        }

        /// <summary>
        /// 동호로 해당월 검색 목록
        /// </summary>
        public async Task<List<Community_Entity>> Month_Apt_List(string AptCode, string StartDate, string EndDate, string Place_Code)
        {
            var sql = "Select * From Community Where AptCode = @AptCode And UsingKindCode = @Place_Code And UserEndDate <= '" + @EndDate + "' And UserStartDate >= '" + @StartDate + "' And Approval = 'B' And Del = 'A' Order By Aid Desc";
            using (var db = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                var lst = await db.QueryAsync<Community_Entity>(sql, new { AptCode, StartDate, EndDate, Place_Code });
                return lst.ToList();
            }
        }

        /// <summary>
        /// 해당 월로 검색된 수
        /// </summary>
        public async Task<int> Search_Month_ListCount(string AptCode, string StartDate, string EndDate)
        {
            using (var db = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                return await db.QuerySingleOrDefaultAsync<int>("Select Count(*) From Community Where AptCode = @AptCode And UserEndDate <= '" + @EndDate + "' And UserStartDate >= '" + @StartDate + "' And Approval = 'B' And Del = 'A'", new { AptCode, StartDate, EndDate });
            }
        }

        /// <summary>
        /// 해당 월로 검색된 수
        /// </summary>
        public async Task<int> Search_Month_Place_ListCount(string AptCode, string StartDate, string EndDate, string Place_Code)
        {
            using (var db = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                return await db.QuerySingleOrDefaultAsync<int>("Select Count(*) From Community Where AptCode = @AptCode And UsingKindCode = @Place_Code And UserEndDate <= '" + @EndDate + "' And UserStartDate >= '" + @StartDate + "' And Approval = 'B' And Del = 'A'", new { AptCode, StartDate, EndDate, Place_Code });
            }
        }

        /// <summary>
        /// 동호로 해당 월에 정보 검색 목록
        /// </summary>
        public async Task<List<Community_Entity>> Search_List(int Page, string AptCode, string StartDate, string EndDate, string Dong, string Ho)
        {
            var sql = "Select Top 15 * From Community Where Aid Not In(Select Top(15 * @Page) Aid From Community Where AptCode = @AptCode And UserStartDate > @StartDate And UserEndDate <= @EndDate And Dong = @Dong And Ho = @Ho And Approval = 'B' And Del = 'A' Order By Aid Desc) And AptCode = @AptCode And UserEndDate <= '" + @EndDate + "' And UserStartDate >= '" + @StartDate + "' And Dong = @Dong And Ho = @Ho And Approval = 'B' And Del = 'A' Order By Aid Desc";
            using (var db = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                var lst = await db.QueryAsync<Community_Entity>(sql, new { Page, AptCode, StartDate, EndDate, Dong, Ho });
                return lst.ToList();
            }
        }

        /// <summary>
        /// 해당동호로 해당월에 검색된 정보 수
        /// </summary>
        public async Task<int> Search_ListCount(string AptCode, string StartDate, string EndDate, string Dong, string Ho)
        {
            using (var db = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                return await db.QuerySingleOrDefaultAsync<int>("Select Count(*) From Community Where AptCode = @AptCode And UserEndDate <= '" + @EndDate + "' And UserStartDate >= '" + @StartDate + "' And Dong = @Dong And Ho = @Ho And Approval = 'B' And Del = 'A'", new { AptCode, EndDate, Dong, Ho });
            }
        }


        /// <summary>
        /// 동호로 검색 목록
        /// </summary>
        public async Task<List<Community_Entity>> Search_List_All(int Page, string AptCode, string Dong, string Ho)
        {
            var sql = "Select Top 15 * From Community Where Aid Not In(Select Top(15 * @Page) Aid From Community Where AptCode = @AptCode And Dong = @Dong And Ho = @Ho And Approval = 'B' And Del = 'A'  Order By Aid Desc) And AptCode = @AptCode And Dong = @Dong And Ho = @Ho And Approval = 'B' And Del = 'A' Order By Aid Desc";
            using (var db = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                var lst = await db.QueryAsync<Community_Entity>(sql, new { Page, AptCode, Dong, Ho });
                return lst.ToList();
            }
        }

        /// <summary>
        /// 해당 월에 동호로 검색된 수
        /// </summary>
        public async Task<int> Search_List_All_Count(string AptCode, string Dong, string Ho)
        {
            using var db = new SqlConnection(_db.GetConnectionString("sw_togather"));
            return await db.QuerySingleOrDefaultAsync<int>("Select Count(*) From Community Where AptCode = @AptCode And Dong = @Dong And Ho = @Ho And Approval = 'B' And Del = 'A'", new { AptCode, Dong, Ho });
        }

        /// <summary>
        /// 같은 세대 중복 신청 정보 목록
        /// </summary>
        public async Task<List<Community_Entity>> RepeatList(string AptCode, string StartDate, string EndDate, string UsingKindName, int Count)
        {
            using var db = new SqlConnection(_db.GetConnectionString("sw_togather"));
            var lst = await db.QueryAsync<Community_Entity>("SELECT Dong, Ho, Count(*) cnt FROM Community Where AptCode = @AptCode And UsingKindName = @UsingKindName And UserEndDate <= '" + EndDate + "' And UserStartDate >= '" + StartDate + "' And Approval = 'B' And Del = 'A' Group by Ho, Dong Having count(*) = @Count Order By Cast(Dong As Int) Asc, Cast(Ho As Int) Asc", new { AptCode, UsingKindName, StartDate, EndDate, Count });
            return lst.ToList();
        }

        /// <summary>
        /// 같은 세대 중복 신청 정보 목록 수
        /// </summary>
        public async Task<int> RepeatList_Count(string AptCode, string StartDate, string EndDate, string UsingKindName, int Count)
        {
            using var db = new SqlConnection(_db.GetConnectionString("sw_togather"));
            return await db.QuerySingleOrDefaultAsync<int>("SELECT Count(*) FROM Community Where AptCode = @AptCode And UsingKindName = @UsingKindName And UserEndDate >= '" + EndDate + "' And UserStartDate <= '" + StartDate + "' And Approval = 'B' And Del = 'A'", new { AptCode, UsingKindName, StartDate, EndDate, Count });
            
        }

        /// <summary>
        /// 같은 세대 중복 신청 정보 목록
        /// </summary>
        public async Task<List<Community_Entity>> PlaceList(int Page, string AptCode, string StartDate, string EndDate, string UsingKindName)
        {
            using var db = new SqlConnection(_db.GetConnectionString("sw_togather"));
            var lst = await db.QueryAsync<Community_Entity>("SELECT Top 15 * FROM Community Where Aid Not In(Select Top(15 * @Page) Aid From Community Where AptCode = @AptCode And UsingKindName = @UsingKindName And UserEndDate <= '" + EndDate + "' And UserStartDate >= '" + StartDate + "' And Approval = 'B' And Del = 'A' Order By Cast(Dong As Int) Asc, Cast(Ho As Int) Asc) And AptCode = @AptCode And UsingKindName = @UsingKindName And UserEndDate <= '" + EndDate + "' And UserStartDate >= '" + StartDate + "' And Approval = 'B' And Del = 'A' Order By Cast(Dong As Int) Asc, Cast(Ho As Int) Asc", new { Page, AptCode, UsingKindName, StartDate, EndDate });
            return lst.ToList();
        }

        /// <summary>
        /// 시설물 장소별 정보 목록 수
        /// </summary>
        public async Task<int> PlaceList_Count(string AptCode, string StartDate, string EndDate, string UsingKindName)
        {
            using var db = new SqlConnection(_db.GetConnectionString("sw_togather"));
            return await db.QuerySingleOrDefaultAsync<int>("SELECT Count(*) FROM Community Where AptCode = @AptCode And UsingKindName = @UsingKindName And UserEndDate <= '" + EndDate + "' And UserStartDate >= '" + StartDate + "' And Approval = 'B' And Del = 'A'", new { AptCode, UsingKindName, StartDate, EndDate });

        }

        /// <summary>
        /// 시설물 장소별 정보 목록(식별코드)
        /// </summary>
        public async Task<List<Community_Entity>> PlaceList_Code(int Page, string AptCode, string StartDate, string EndDate, string UsingKindCode)
        {
            using var db = new SqlConnection(_db.GetConnectionString("sw_togather"));
            var lst = await db.QueryAsync<Community_Entity>("SELECT Top 15 * FROM Community Where Aid Not In(Select Top(15 * @Page) Aid From Community Where AptCode = @AptCode And UsingKindCode = @UsingKindCode And UserEndDate >= '" + EndDate + "' And UserStartDate >= '" + StartDate + "' And Approval = 'B' And Del = 'A' Order By Cast(Dong As Int) Asc, Cast(Ho As Int) Asc) And AptCode = @AptCode And UsingKindCode = @UsingKindCode And UserEndDate <= '" + EndDate + "' And UserStartDate >= '" + StartDate + "' And Approval = 'B' And Del = 'A' Order By Cast(Dong As Int) Asc, Cast(Ho As Int) Asc", new { Page, AptCode, UsingKindCode, StartDate, EndDate });
            return lst.ToList();
        }


       

        /// <summary>
        /// 스크린 골프 신청자 정보 목록 수
        /// </summary>
        public int GolfSceenInfor(string AptCode, string StartDate, string EndDate, string UsingKindCode, int Hour)
        {
            using var db = new SqlConnection(_db.GetConnectionString("sw_togather"));
            return db.QuerySingleOrDefault<int>("SELECT Count(*) FROM Community Where AptCode = @AptCode And UsingKindCode = @UsingKindCode And UserEndDate <= '" + EndDate + "' And UserStartDate >= '" + StartDate + "' And UserStartHour = @Hour And Approval = 'B' And Del = 'A'", new { AptCode, UsingKindCode, StartDate, EndDate, Hour });
        }

        /// <summary>
        /// 이름으로 검색된 정보 목록 수
        /// </summary>
        public async Task<int> NameList_Count(string AptCode, string StartDate, string EndDate, string UserName)
        {
            using var db = new SqlConnection(_db.GetConnectionString("sw_togather"));
            return await db.QuerySingleOrDefaultAsync<int>("SELECT Count(*) FROM Community Where AptCode = @AptCode And UserName = @UserName And UserEndDate <= '" + EndDate + "' And UserStartDate >= '" + StartDate + "' And Approval = 'B' And Del = 'A'", new { AptCode, UserName, StartDate, EndDate });

        }



        /// <summary>
        /// 이름으로 검색된 정보 목록
        /// </summary>
        public async Task<List<Community_Entity>> NameList(int Page, string AptCode, string StartDate, string EndDate, string UserName)
        {
            using var db = new SqlConnection(_db.GetConnectionString("sw_togather"));
            var lst = await db.QueryAsync<Community_Entity>("SELECT Top 15 * FROM Community Where Aid Not In(Select Top(15 * @Page) Aid From Community Where AptCode = @AptCode And UserName = @UserName And UserEndDate >= '" + EndDate + "' And UserStartDate >= '" + StartDate + "' And Approval = 'B' And Del = 'A' Order By Cast(Dong As Int) Asc, Cast(Ho As Int) Asc) And AptCode = @AptCode And UserName = @UserName And UserEndDate <= '" + EndDate + "' And UserStartDate >= '" + StartDate + "' And Approval = 'B' And Del = 'A' Order By Cast(Dong As Int) Asc, Cast(Ho As Int) Asc", new { Page, AptCode, UserName, StartDate, EndDate });
            return lst.ToList();
        }

        /// <summary>
        /// 이용방법 목록(공동주택식별코드, 시작일, 종료일, 시설 코드, 이용방법 코드) 20221226
        /// </summary>
        public async Task<List<Community_Entity>> TicketList(int Page, string AptCode, string StartDate, string EndDate, string UsingKindCode, string Ticket_Code)
        {
            using var db = new SqlConnection(_db.GetConnectionString("sw_togather"));
            var lst = await db.QueryAsync<Community_Entity>("SELECT Top 15 * FROM Community Where Aid Not In(Select Top(15 * @Page) Aid From Community Where AptCode = @AptCode And UsingKindCode = @UsingKindCode And Ticket_Code = @Ticket_Code And UserEndDate >= '" + EndDate + "' And UserStartDate >= '" + StartDate + "' And Approval = 'B' And Del = 'A' Order By Aid Desc) And AptCode = @AptCode And UsingKindCode = @UsingKindCode And Ticket_Code = @Ticket_Code And UserEndDate <= '" + EndDate + "' And UserStartDate >= '" + StartDate + "' And Approval = 'B' And Del = 'A' Order By Aid Desc", new { Page, AptCode, StartDate, EndDate, UsingKindCode, Ticket_Code });
            return lst.ToList();

        }

        /// <summary>
        /// 이용방법 목록 수(공동주택식별코드, 시작일, 종료일, 시설 코드, 이용방법 코드) 20221226
        /// </summary>
        public async Task<int> TicketList_Count(string AptCode, string StartDate, string EndDate, string UsingKindCode, string Ticket_Code)
        {
            using var db = new SqlConnection(_db.GetConnectionString("sw_togather"));
            return await db.QuerySingleOrDefaultAsync<int>("SELECT Count(*) FROM Community Where AptCode = @AptCode And UsingKindCode = @UsingKindCode And Ticket_Code = @Ticket_Code And UserEndDate <= '" + EndDate + "' And UserStartDate >= '" + StartDate + "' And Approval = 'B' And Del = 'A'", new { AptCode, UsingKindCode, StartDate, EndDate, Ticket_Code });

        }

        /// <summary>
        /// 같은 세대 중복 신청 정보 목록 수(식별코드)
        /// </summary>
        public async Task<int> PlaceList_Code_Count(string AptCode, string StartDate, string EndDate, string UsingKindCode)
        {
            using var db = new SqlConnection(_db.GetConnectionString("sw_togather"));
            return await db.QuerySingleOrDefaultAsync<int>("SELECT Count(*) FROM Community Where AptCode = @AptCode And UsingKindCode = @UsingKindCode And UserEndDate <= '" + EndDate + "' And UserStartDate >= '" + StartDate + "' And Approval = 'B' And Del = 'A'", new { AptCode, UsingKindCode, StartDate, EndDate });

        }

        /// <summary>
        /// 커뮤니티 시설명 목록
        /// </summary>
        public async Task<List<Community_Entity>> UsingKindName(string Apt_Code)
        {
            using var db = new SqlConnection(_db.GetConnectionString("sw_togather"));
            var lst = await db.QueryAsync<Community_Entity>("SELECT UsingKindName, UsingKindCode FROM Community Where AptCode = @Apt_Code And Approval = 'B' And Del = 'A' Group by UsingKindName, UsingKindCode", new { Apt_Code });
            return lst.ToList();
        }

        /// <summary>
        /// 해당 월에 동호로 검색된 합계
        /// </summary>
        public async Task<int> Search_List_All_Sum(string AptCode, string StartDate, string EndDate, string Dong, string Ho)
        {
            using (var db = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                return await db.QuerySingleOrDefaultAsync<int>("Select ISNULL(Sum(UseCost), 0) From Community Where AptCode = @AptCode And UserEndDate <= '" + EndDate + "' And UserStartDate >= '" + StartDate + "' And Dong = @Dong And Ho = @Ho And Approval = 'B' And Del = 'A'", new { AptCode, Dong, Ho });
            }
        }

        /// <summary>
        /// 월 결산 목록
        /// </summary>
        public async Task<List<MonthTotalSum_Entity>> Month_Sum(string Apt_Code, string StartDate, string EndDate)
        {
            using var db = new SqlConnection(_db.GetConnectionString("sw_togather"));
            var lst = await db.QueryAsync<MonthTotalSum_Entity>("SELECT Dong, Ho, SUM(UseCost) as TotalSum FROM Community Where AptCode = @Apt_Code And UserEndDate <= '" + EndDate + "' And UserStartDate >= '" + StartDate + "' And Approval = 'B' And Del = 'A' Group by Ho, Dong Order By Cast(Dong As Int) Asc, Cast(Ho As Int) Asc", new { Apt_Code, StartDate, EndDate });
            return lst.ToList();
        }

        /// <summary>
        /// 월 신청자 정보 목록
        /// </summary>
        public async Task<List<Community_Entity>> Month_Input_List (string Apt_Code, string StartDate, string EndDate)
        {
            using var db = new SqlConnection(_db.GetConnectionString("sw_togather"));
            var lst = await db.QueryAsync<Community_Entity>("Select UserCode, UserName, Dong, Ho, Mobile, UsingKindName, Ticket, UseCost From Community Where AptCode = @Apt_Code And UserEndDate <= '" + EndDate + "' And UserStartDate >= '" + StartDate + "' And Approval = 'B' And Del = 'A' Order By Cast(Dong As Int) Asc, Cast(Ho As Int) Asc", new { Apt_Code, StartDate, EndDate });
            return lst.ToList();
        }


        /// <summary>
        /// 승인하기
        /// </summary>
        public async Task Approval(int Aid)
        {
            using var db = new SqlConnection(_db.GetConnectionString("sw_togather"));
            string re = await db.QuerySingleOrDefaultAsync<string>("Select Approval From Community Where Aid = @Aid And Del = 'A'", new { Aid });
            if (!string.IsNullOrWhiteSpace(re))
            {
                if (re == "A")
                {
                    await db.ExecuteAsync("Update Community Set Approval = 'B' Where Aid = @Aid And Del = 'A'", new { Aid });
                }
                else
                {
                    await db.ExecuteAsync("Update Community Set Approval = 'A' Where Aid = @Aid And Del = 'A'", new { Aid });
                }
            }
        }    
        
        /// <summary>
        /// 지문 식별코드 불러오기
        /// </summary>
        public async Task<string> ByUserCode(string AptCode, string Dong, string Ho, string UserName)
        {
            using var db = new SqlConnection(_db.GetConnectionString("sw_togather"));
            return await db.QuerySingleOrDefaultAsync<string>("Select Top 1 UserCode From Community Where AptCode = @AptCode And Dong = @Dong And Ho = @Ho And UserName = @UserName Order by Aid desc", new { AptCode, Dong, Ho, UserName });
        }

        /// <summary>
        /// 해당월 
        /// </summary>
        public async Task<int> BeingCount(string AptCode, string KindCode, string Ticket_Code, string Dong, string Ho, string UserName, string Mobile, DateTime StartDate, DateTime EndDate)
        {
            using var db = new SqlConnection(_db.GetConnectionString("sw_togather"));
            return await db.QuerySingleOrDefaultAsync<int>("Select Count(*) From Community Where AptCode = @AptCode And UsingKindCode = @KindCode And Ticket_Code = @Ticket_Code And Dong = @Dong And Ho = @Ho And UserName = @UserName And Mobile = @Mobile And UserStartDate >= @StartDate And UserEndDate <= @EndDate And Del = 'A'", new { AptCode, KindCode, Ticket_Code, Dong, Ho, UserName, Mobile, StartDate, EndDate });
        }

        /// <summary>
        /// 해당월 이용 여부 확인
        /// </summary>
        public async Task<int> BeingCount_DongHo(string AptCode, string KindCode, string Ticket_Code, string Dong, string Ho, string Mobile, DateTime StartDate, DateTime EndDate)
        {
            using var db = new SqlConnection(_db.GetConnectionString("sw_togather"));
            return await db.QuerySingleOrDefaultAsync<int>("Select Count(*) From Community Where AptCode = @AptCode And UsingKindCode = @KindCode And Ticket_Code = @Ticket_Code And Dong = @Dong And Ho = @Ho And Mobile = @Mobile And UserStartDate >= @StartDate And UserEndDate <= @EndDate And Del = 'A'", new { AptCode, KindCode, Ticket_Code, Dong, Ho, Mobile, StartDate, EndDate });
        }


        /// <summary>
        /// 해당 시간 존재 여부
        /// </summary>
        public async Task<int> HourBeingCount(string AptCode, string KindCode, DateTime StartDate, DateTime EndDate, int StartHour)
        {
            using var db = new SqlConnection(_db.GetConnectionString("sw_togather"));
            return await db.QuerySingleOrDefaultAsync<int>("Select Count(*) From Community Where AptCode = @AptCode And UsingKindCode = @KindCode And UserStartDate = @StartDate And UserStartHour = @StartHour And Del = 'A'", new { AptCode, KindCode, StartDate, EndDate, StartHour });
        }

        /// <summary>
        /// 해당 세대 시설이용 월로 등록여부 확인
        /// </summary>
        public async Task<int> DongHoSameCount(string AptCode, string Dong, string Ho, string UsingKindCode, string StartDate, string EndDate)
        {
            using var db = new SqlConnection(_db.GetConnectionString("sw_togather"));
            return await db.QuerySingleOrDefaultAsync<int>("Select Count(*) From Community Where AptCode = @AptCode And Dong = @Dong And Ho = @Ho And UsingKindCode = @UsingKindCode And UserEndDate <= @EndDate And UserStartDate >= @StartDate And Del = 'A'", new { AptCode, Dong, Ho, UsingKindCode, StartDate, EndDate });
        }

        /// <summary>
        /// 관리자 모두 보기
        /// </summary>
        public async Task<List<Community_Entity>> All_List(int Page)
        {
            using var db = new SqlConnection(_db.GetConnectionString("sw_togather"));
            var lst = await db.QueryAsync<Community_Entity>("SELECT Top 15 * FROM Community Where Aid Not In (Select Top(15 * @Page) Aid From Community Order By Aid Desc) Order By Aid desc", new { Page });
            return lst.ToList();
        }

        public async Task<int> All_List_Count()
        {
            using var db = new SqlConnection(_db.GetConnectionString("sw_togather"));
            return await db.QuerySingleOrDefaultAsync<int>("Select Count(*) From Community");
        }


        /// <summary>
        /// 신청순서 값 불러오기
        /// </summary>
        public async Task<int> OrderBy(string Apt_Code, string UsingKindCode, string Ticket_Code, DateTime StartDate, DateTime EndDate)
        {
            using var db = new SqlConnection(_db.GetConnectionString("sw_togather"));
            return await db.QuerySingleOrDefaultAsync<int>("Select Count(*) From Community Where AptCode = @Apt_Code And UsingKindCode = @UsingKindCode And Ticket_Code = @Ticket_Code And UserStartDate >= @StartDate And UserEndDate <= @EndDate And Del = 'A'", new { Apt_Code, UsingKindCode, Ticket_Code, StartDate, EndDate });
            //return Count;
        }

        /// <summary>
        /// 주민공동시설 이용 신청 순서 수정
        /// </summary>
        public async Task OrderBy_Edit(int Aid, int OrderBy)
        {
            using var db = new SqlConnection(_db.GetConnectionString("sw_togather"));
            await db.ExecuteAsync("Update Community Set OrderBy = @OrderBy Where Aid = @Aid", new { Aid, OrderBy });
        }

    }

    public class CommunityUsingKind_Lib : ICommunityUsingKind_Lib
    {
        private readonly IConfiguration _db;
        public CommunityUsingKind_Lib(IConfiguration configuration)
        {
            this._db = configuration;
        }

        public async Task<int> Add(CommunityUsingKind_Entity cu)
        {
            var sql = "Insert into CommunityUsingKind (AptCode, AptName, Kind_Code, Kind_Name, User_Code) Values (@AptCode, @AptName, @Kind_Code, @Kind_Name, @User_Code); Select Cast(SCOPE_IDENTITY() As Int);";
            using (var db = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                int Being = await db.QuerySingleOrDefaultAsync<int>(sql, cu);
                return Being;
            }
        }

        public async Task Edit(CommunityUsingKind_Entity cu)
        {
            var sql = "Update CommunityUsingKind Set Kind_Name = @Kind_Name, Kind_Code = @Kind_Code, User_Code = @User_Code Where Aid = @Aid";
            using (var db = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                await db.ExecuteAsync(sql, cu);
            }
        }

        public async Task<List<CommunityUsingKind_Entity>> GetList(int Page)
        {
            var sql = "Select Top 15 * From CommunityUsingKind Where Aid Not In(Select Top(15 * @Page) Aid From CommunityUsingKind Where Using = 'A' Order By Aid Desc) And Using = 'A' Order By Aid Desc";
            using (var db = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                var lst = await db.QueryAsync<CommunityUsingKind_Entity>(sql, new { Page });
                return lst.ToList();
            }
        }

        public async Task<int> GetListCount()
        {
            using (var db = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                return await db.QuerySingleOrDefaultAsync<int>("Select Count(*) From CommunityUsingKind Where Using = 'A'");
            }
        }

        public async Task<List<CommunityUsingKind_Entity>> GetList_Apt(string AptCode)
        {
            var sql = "Select * From CommunityUsingKind Where AptCode = @AptCode And Using = 'A'";
            using (var db = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                var lst = await db.QueryAsync<CommunityUsingKind_Entity>(sql, new { AptCode });
                return lst.ToList();
            }
        }

        public async Task<string> KindName(string Kind_Code)
        {
            using (var db = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                return await db.QuerySingleOrDefaultAsync<string>("Select Kind_Name From CommunityUsingKind Where Kind_Code = @Kind_Code", new { Kind_Code });
            }
        }

        public async Task Remove(int Aid, string Using)
        {
            using (var db = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                if (Using == "A")
                {
                    await db.ExecuteAsync("Update CommunityUsingKind Set Using = 'B' Where Aid = @Aid", new { Aid });
                }
                else
                {
                    await db.ExecuteAsync("Update CommunityUsingKind Set Using = 'A' Where Aid = @Aid", new { Aid });
                }                
            }
        }

        public async Task<string> ListAid()
        {
            using (var db = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                return await db.QuerySingleOrDefaultAsync<string>("Select Top 1 Aid From CommunityUsingKind Order by Aid Desc");
            }
        }
    }

    public class CommunityUsingTicket_Lib : ICommunityUsingTicket_Lib
    {
        private readonly IConfiguration _db;
        public CommunityUsingTicket_Lib(IConfiguration configuration)
        {
            this._db = configuration;
        }

        public async Task<int> Add(CommunityUsingTicket_Entity cu)
        {
            var sql = "Insert into CommunityUsingTicket (AptCode, AptName, Kind_Code, Kind_Name, Ticket_Code, Ticket_Name, Ticket_Cost, User_Code) Values (@AptCode, @AptName, @Kind_Code, @Kind_Name, @Ticket_Code, @Ticket_Name, @Ticket_Cost, @User_Code); Select Cast(SCOPE_IDENTITY() As Int);";
            using (var db = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                int Being = await db.QuerySingleOrDefaultAsync<int>(sql, cu);
                return Being;
            }
        }

        public async Task Edit(CommunityUsingTicket_Entity cu)
        {
            var sql = "Update CommunityUsingTicket Set Kind_Code = @Kind_Code, Kind_Name = @Kind_Name, Ticket_Name = @Ticket_Name, Ticket_Code = @Ticket_Code, Ticket_Cost = @Ticket_Cost, User_Code = @User_Code Where Aid = @Aid";
            using (var db = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                await db.ExecuteAsync(sql, cu);
            }
        }

        public async Task<List<CommunityUsingTicket_Entity>> GetList(int Page)
        {
            var sql = "Select Top 15 * From CommunityUsingTicket Where Aid Not In(Select Top(15 * @Page) Aid From CommunityUsingTicket Where Using = 'A' Order By Aid Desc) And Using = 'A' Order By Aid Desc";
            using (var db = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                var lst = await db.QueryAsync<CommunityUsingTicket_Entity>(sql, new { Page });
                return lst.ToList();
            }
        }

        public async Task<int> GetListCount()
        {
            using (var db = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                return await db.QuerySingleOrDefaultAsync<int>("Select Count(*) From CommunityUsingTicket Where Using = 'A'");
            }
        }

        public async Task<List<CommunityUsingTicket_Entity>> GetList_Apt(string AptCode)
        {
            var sql = "Select * From CommunityUsingTicket Where AptCode = @AptCode And Using = 'A'";
            using var db = new SqlConnection(_db.GetConnectionString("sw_togather"));
            var lst = await db.QueryAsync<CommunityUsingTicket_Entity>(sql, new { AptCode });
            return lst.ToList();
        }

        /// <summary>
        /// 이용장소의 이용방법 목록
        /// </summary>
        public async Task<List<CommunityUsingTicket_Entity>> GetList_Apt_Kind(string AptCode, string Kind_Code)
        {
            var sql = "Select * From CommunityUsingTicket Where AptCode = @AptCode And Kind_Code = @Kind_Code And Using = 'A'";
            using var db = new SqlConnection(_db.GetConnectionString("sw_togather"));
            var lst = await db.QueryAsync<CommunityUsingTicket_Entity>(sql, new { AptCode, Kind_Code });
            return lst.ToList();
        }


        /// <summary>
        /// 이용방법 상세보기
        /// </summary>
        public async Task<CommunityUsingTicket_Entity> Details(string Aid)
        {
            using var db = new SqlConnection(_db.GetConnectionString("sw_togather"));
            return await db.QuerySingleOrDefaultAsync<CommunityUsingTicket_Entity>("Select * From CommunityUsingTicket Where Aid = @Aid", new { Aid });
        }

        /// <summary>
        /// 이용방법 코드로 상세보기
        /// </summary>
        public async Task<CommunityUsingTicket_Entity> Details_Code(string Ticket_Code)
        {
            using var db = new SqlConnection(_db.GetConnectionString("sw_togather"));
            return await db.QuerySingleOrDefaultAsync<CommunityUsingTicket_Entity>("Select * From CommunityUsingTicket Where Ticket_Code = @Ticket_Code", new { Ticket_Code });
        }

        public async Task<string> TicketName(string Ticket_Code)
        {
            using (var db = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                return await db.QuerySingleOrDefaultAsync<string>("Select Ticket_Name From CommunityUsingTicket Where Ticket_Code = @Ticket_Code", new { Ticket_Code });
            }
        }

        public async Task Remove(int Aid, string Using)
        {
            using (var db = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                if (Using == "A")
                {
                    await db.ExecuteAsync("Update CommunityUsingTicket Set Using = 'B' Where Aid = @Aid", new { Aid });
                }
                else
                {
                    await db.ExecuteAsync("Update CommunityUsingTicket Set Using = 'A' Where Aid = @Aid", new { Aid });
                }
            }
        }

        public async Task<string> ListAid()
        {
            using (var db = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                return await db.QuerySingleOrDefaultAsync<string>("Select Top 1 Aid From CommunityUsingTicket Order by Aid Desc");
            }
        }
    }
}
