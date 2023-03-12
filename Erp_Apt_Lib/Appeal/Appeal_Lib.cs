using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Erp_Apt_Lib.Appeal
{
    /// <summary>
    /// 민원분류 클래스
    /// </summary>
    public class Appeal_Bloom_Lib : IAppeal_Bloom_Lib
    {
        private readonly IConfiguration _db;
        public Appeal_Bloom_Lib(IConfiguration configuration)
        {
            this._db = configuration;
        }

        /// <summary>
        /// 민원분류 입력
        /// </summary>
        /// <param name="ad"></param>
        /// <returns></returns>
        public async Task<Appeal_Bloom_Entity> Add(Appeal_Bloom_Entity ad)
        {
            var sql = "Insert Appeal_Sort (Apt_Code, Bloom_Code, Sort, Asort, Period, Content, PostIP, UserID, UserName) Values (@Apt_Code, @Bloom_Code, @Sort, @Asort, @Period, @Content, @PostIP, @UserID, @UserName)";
            using (var dba = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                await dba.ExecuteAsync(sql, ad);
                return ad;
            }

        }

        /// <summary>
        /// 수정
        /// </summary>
        /// <param name="ad"></param>
        /// <returns></returns>
        public async Task<Appeal_Bloom_Entity> Edit(Appeal_Bloom_Entity ad)
        {
            var sql = "Update Appeal_Sort Set Sort = @Sort, Asort = @Asort, Period = @Period, Content = @Content, ModifyDate = @ModifyDate, ModifyIP =@ModifyIP, UserID = @UserID, UserName = @UserName Where Num = @Num";
            using (var dba = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                await dba.ExecuteAsync(sql, ad);
                return ad;
            }

        }

        /// <summary>
        /// 사용 여부 수정
        /// </summary>
        /// <param name="ad"></param>
        /// <returns></returns>
        public async Task<Appeal_Bloom_Entity> Edit_Use(Appeal_Bloom_Entity ad)
        {
            var sql = "Update Appeal_Sort Set Useing = @Useing, ModifyDate = @ModifyDate, ModifyIP =@ModifyIP, UserID = @UserID, UserName = @UserName Where Num = @Num";
            using (var dba = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                await dba.ExecuteAsync(sql, ad);
                return ad;
            }

        }

        /// <summary>
        /// 민원 분류 전체 리스트
        /// </summary>
        /// <returns></returns>
        public async Task<List<Appeal_Bloom_Entity>> GetList()
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                var lst = await dba.QueryAsync<Appeal_Bloom_Entity>("Select Num, Apt_Code, Bloom_Code, Sort, Asort, [Content], Period, PostDate, PostIP, ModifyDate, ModifyIP, FileName, FileSize, FileName2, FileSize2, FileName3, FileSize3, FileName4, FileSize4, FileName5, FileSize5, Useing, UserID, UserName, Del FROM Appeal_Sort Order by Sort Asc");
                return lst.ToList();
            }
        }

        /// <summary>
        /// 민원 분류 전체 리스트
        /// </summary>
        /// <returns></returns>
        public async Task<List<Appeal_Bloom_Entity>> GetList_Page(int Page)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                var lst = await dba.QueryAsync<Appeal_Bloom_Entity>("Select Top 15 * FROM Appeal_Sort Where Num Not In(Select Top(15 * @Page) Num From Appeal_Sort Order By Sort Asc) Order by Sort Asc", new { Page });
                return lst.ToList();
            }
        }

        //Select Top 15 * From Defect Where Aid Not In(Select Top(15 * @Page) Aid From Defect Where AptCode = @AptCode Order By Aid Desc) And AptCode = @AptCode Order By Aid Desc

        /// <summary>
        /// 민원 분류 전체 리스트 수
        /// </summary>
        /// <returns></returns>
        public async Task<int> GetList_Count()
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                return await dba.QuerySingleOrDefaultAsync<int>("Select Count(*) FROM Appeal_Sort");
            }

        }

        /// <summary>
        /// 민원 분류 상세보기
        /// </summary>
        /// <param name="Num"></param>
        /// <returns></returns>
        public async Task<Appeal_Bloom_Entity> Details(int Num)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                return await dba.QuerySingleOrDefaultAsync<Appeal_Bloom_Entity>("Select * FROM Appeal_Sort Where Aid = @Aid");
            }
        }

        /// <summary>
        /// 마지막 일련 번호
        /// </summary>
        /// <returns></returns>
        public async Task<string> LastNumber()
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                return await dba.QuerySingleOrDefaultAsync<string>("Select top 1 Num From Appeal_Sort Order by Num Desc");
            }

        }

        /// <summary>
        /// 민원 대분류 리스트
        /// </summary>
        /// <returns></returns>
        public async Task<List<Appeal_Bloom_Entity>> Sort_Name_List()
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                var lst = await dba.QueryAsync<Appeal_Bloom_Entity>("Select distinct Sort From Appeal_Sort");
                return lst.ToList();
            }
        }

        /// <summary>
        /// 민원 소분류 리스트
        /// </summary>
        /// <param name="Sort"></param>
        /// <returns></returns>
        public async Task<List<Appeal_Bloom_Entity>> Asort_Name_List(string Sort)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                var lst = await dba.QueryAsync<Appeal_Bloom_Entity>("Select Bloom_Code, Asort From Appeal_Sort Where Sort = @Sort And Useing = 'A' Order By Num Desc", new { Sort });
                return lst.ToList();
            }
        }

        /// <summary>
        /// 민원 리스트
        /// </summary>
        /// <param name="Sort"></param>
        /// <returns></returns>
        public async Task<List<Appeal_Bloom_Entity>> Asort_List(string Sort)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                var lst = await dba.QueryAsync<Appeal_Bloom_Entity>("Select * From Appeal_Sort Where Sort = @Sort And Useing = 'A' Order By Num Desc", new { Sort });
                return lst.ToList();
            }
        }

        /// <summary>
        /// 소분류명 불러오기
        /// </summary>
        /// <param name="Bloom_Code"></param>
        /// <returns></returns>
        public async Task<string> ASortName(string Bloom_Code)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                return await dba.QuerySingleOrDefaultAsync<string>("Select top 1 Asort From Appeal_Sort Where Bloom_Code = @Bloom_Code", new { Bloom_Code });
            }

        }

        /// <summary>
        /// 대분류명 불러오기
        /// </summary>
        /// <param name="Bloom_Code"></param>
        /// <returns></returns>
        public async Task<string> SortName(string Bloom_Code)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                return await dba.QuerySingleOrDefaultAsync<string>("Select top 1 Sort From Appeal_Sort Where Bloom_Code = @Bloom_Code And Useing = 'A'", new { Bloom_Code });
            }
        }

        /// <summary>
        /// 하자 기간 가져오기
        /// </summary>
        public async Task<int> Period(string Asort)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                return await dba.QuerySingleOrDefaultAsync<int>("Select top 1 Period From Appeal_Sort Where Asort = @Asort And Useing = 'A'", new { Asort });
            }
        }

        public async Task<string> Sort_Code(string SortName)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                return await dba.QuerySingleOrDefaultAsync<string>("Select top 1 Num From Appeal_Sort Where Sort = @SortName And Useing = 'A'", new { SortName });
            }
        }

        public async Task<string> Asort_Code(string AsortName)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                return await dba.QuerySingleOrDefaultAsync<string>("Select top 1 Bloom_Code From Appeal_Sort Where Asort = @AsortName And Useing = 'A'", new { AsortName });
            }
        }

        /// <summary>
        /// 민원 세분류 선택 시 실행
        /// </summary>
        /// <param name="Bloom_Code"></param>
        /// <returns></returns>
        public async Task<Appeal_Bloom_Entity> Details_Code(string Bloom_Code)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                return await dba.QuerySingleOrDefaultAsync<Appeal_Bloom_Entity>("Select top 1 * From Appeal_Sort Where Bloom_Code = @Bloom_Code And Useing = 'A'", new { Bloom_Code });
            }
        }
    }

    /// <summary>
    /// 민원일지 분류  속성 목록
    /// </summary>
    public class Appeal_Lib : IAppeal_Lib
    {
        private readonly IConfiguration _db;
        public Appeal_Lib(IConfiguration configuration)
        {
            this._db = configuration;
        }

        /// <summary>
        /// 입주민 민원 입력하기
        /// </summary>
        /// <param name="appeal"></param>
        /// <returns></returns>
        public async Task<Appeal_Entity> add(Appeal_Entity appeal)
        {
            var sql = "Insert into Appeal (AptCode, AptName, apYear, apMonth, apDay, apClock, apMinute, apDongNo, apHoNo, apName, apHp, apPost, Bloom_Code, apTitle, apReciever, apContent, PostIP, ComAlias, ComTitle, apUserName, Private) Values (@AptCode, @AptName, @apYear, @apMonth, @apDay, @apClock, @apMinute, @apDongNo, @apHoNo, @apName, @apHp, @apPost, @Bloom_Code, @apTitle, @apReciever, @apContent, @PostIP, @ComAlias, @ComTitle, @apUserName, @Private); Select Cast(SCOPE_IDENTITY() As Int);";
            using (var dba = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                int Aid = await dba.QuerySingleOrDefaultAsync<int>(sql, appeal);
                appeal.Num = Aid;
                return appeal;
            }
        }

        /// <summary>
        /// 민원 내용 삭제
        /// </summary>
        /// <param name="Num"></param>
        public async Task Remove(string Num)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                await dba.ExecuteAsync("Delete From Appeal Where Num = @Num", new { Num });
            }
        }

        /// <summary>
        /// 입주민 민원 내역
        /// </summary>
        public async Task<List<Appeal_Entity>> getlist(int Page, string AptCode, string apDongNo, string apHoNo)
        {
            var sql = "Select Top 15 Num, AptCode, AptName, apYear, apMonth, apDay, apClock, apMinute, apDongNo, apHoNo, apName, apHp, apPost, Bloom_Code, apTitle, apReciever, apContent, apOk, subYear, subMonth, subDay, subClock, subMinute, outViw, outName, outTelCom1, outTelCom2, outTelCom3, outNameCom, innViw, Complete, innName, subPost, innContent, apSatisfaction, PostDate, PostIP, ModifyDate, ModifyIP, ComFileName, ComFileSize, ComFileName2, ComFileSize2, ComFileName3, ComFileSize3, ComFileName4, ComFileSize4, ComFileName5, ComFileSize5, ComFileName6, ComFileSize6, ComFileName7, ComFileSize7, ComFileName8, ComFileSize8, ComFileName9, ComFileSize9, ComFileName10, ComFileSize10, ComAlias, ComTitle, AppealViw, apUserName, Private From Appeal Where Num Not In(Select Top(15 * " + @Page + ") Num From Appeal Where AptCode = @AptCode And apDongNo = @apDongNo And apHoNo = @apHoNo And apReciever = '인터넷' Order By Num Desc) and AptCode = @AptCode And apDongNo = @apDongNo And apHoNo = @apHoNo And apReciever = '인터넷' Order By Num Desc;";

            using var dba = new SqlConnection(_db.GetConnectionString("Ayoung"));
            var lst = await dba.QueryAsync<Appeal_Entity>(sql, new { Page, AptCode, apDongNo, apHoNo });
            return lst.ToList();
        }

        /// <summary>
        /// 입주민 민원 내역 수
        /// </summary>
        public async Task<int> getlist_count(string AptCode, string apDongNo, string apHoNo)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                return await dba.QuerySingleOrDefaultAsync<int>("Select Count(*) From Appeal Where AptCode = @AptCode And apDongNo = @apDongNo And apHoNo = @apHoNo And apReciever = '인터넷'", new { AptCode, apDongNo, apHoNo });
            }
        }

        /// <summary>
        /// 동호로 검색
        /// </summary>
        public async Task<List<Appeal_Entity>> getlistDongHo(int Page, string AptCode, string apDongNo, string apHoNo)
        {
            var sql = "Select Top 15 Num, AptCode, AptName, apYear, apMonth, apDay, apClock, apMinute, apDongNo, apHoNo, apName, apHp, apPost, Bloom_Code, apTitle, apReciever, apContent, apOk, subYear, subMonth, subDay, subClock, subMinute, outViw, outName, outTelCom1, outTelCom2, outTelCom3, outNameCom, innViw, Complete, innName, subPost, innContent, apSatisfaction, PostDate, PostIP, ModifyDate, ModifyIP, ComFileName, ComFileSize, ComFileName2, ComFileSize2, ComFileName3, ComFileSize3, ComFileName4, ComFileSize4, ComFileName5, ComFileSize5, ComFileName6, ComFileSize6, ComFileName7, ComFileSize7, ComFileName8, ComFileSize8, ComFileName9, ComFileSize9, ComFileName10, ComFileSize10, ComAlias, ComTitle, AppealViw, apUserName, Private From Appeal Where Num Not In(Select Top(15 * " + @Page + ") Num From Appeal Where AptCode = @AptCode And apDongNo = @apDongNo And apHoNo = @apHoNo Order By Num Desc) And AptCode = @AptCode And apDongNo = @apDongNo And apHoNo = @apHoNo Order By Num Desc;";
            using var dba = new SqlConnection(_db.GetConnectionString("Ayoung"));
            var lst = await dba.QueryAsync<Appeal_Entity>(sql, new { Page, AptCode, apDongNo, apHoNo });
            return lst.ToList();
        }

        /// <summary>
        /// 입주민 민원 내역 수
        /// </summary>
        /// <param name="AptCode"></param>
        /// <param name="apDongNo"></param>
        /// <param name="apHoNo"></param>
        /// <returns></returns>
        public async Task<int> getlistDongHo_count(string AptCode, string apDongNo, string apHoNo)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                return await dba.QuerySingleOrDefaultAsync<int>("Select Count(*) From Appeal Where AptCode = @AptCode And apDongNo = @apDongNo And apHoNo = @apHoNo", new { AptCode, apDongNo, apHoNo });
            }

        }

        /// <summary>
        /// 입주민 민원 내역 해당 공동주택
        /// </summary>
        public async Task<List<Appeal_Entity>> getlist_apt(int Page, string AptCode)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                var lst = await dba.QueryAsync<Appeal_Entity>("Select Top 15 Num, AptCode, AptName, apYear, apMonth, apDay, apClock, apMinute, apDongNo, apHoNo, apName, apHp, apPost, Bloom_Code, apTitle, apReciever, apContent, apOk, subYear, subMonth, subDay, subClock, subMinute, outViw, outName, outTelCom1, outTelCom2, outTelCom3, outNameCom, innViw, Complete, innName, subPost, innContent, apSatisfaction, PostDate, PostIP, ModifyDate, ModifyIP, ComFileName, ComFileSize, ComFileName2, ComFileSize2, ComFileName3, ComFileSize3, ComFileName4, ComFileSize4, ComFileName5, ComFileSize5, ComFileName6, ComFileSize6, ComFileName7, ComFileSize7, ComFileName8, ComFileSize8, ComFileName9, ComFileSize9, ComFileName10, ComFileSize10, ComAlias, ComTitle, AppealViw, apUserName, Private From Appeal Where Num Not In(Select Top(15 * " + @Page + ") Num From Appeal Where AptCode = @AptCode Order By Num Desc) and AptCode = @AptCode Order By Num Desc; ", new { Page, AptCode });
                return lst.ToList();
            }
        }

        /// <summary>
        /// 입주민 민원 내역 해당 공동주택
        /// </summary>
        public async Task<List<Appeal_Entity>> getlist_Mobile_apt(string AptCode)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                var lst = await dba.QueryAsync<Appeal_Entity>("Select Top 30 Num, AptCode, AptName, apYear, apMonth, apDay, apClock, apMinute, apDongNo, apHoNo, apName, apHp, apPost, Bloom_Code, apTitle, apReciever, apContent, apOk, subYear, subMonth, subDay, subClock, subMinute, outViw, outName, outTelCom1, outTelCom2, outTelCom3, outNameCom, innViw, Complete, innName, subPost, innContent, apSatisfaction, PostDate, PostIP, ModifyDate, ModifyIP, ComAlias, ComTitle, AppealViw, apUserName, Private From Appeal Where AptCode = @AptCode Order By Num Desc", new { AptCode });
                return lst.ToList();
            }
        }

        /// <summary>
        /// 입주민 민원 내역 해당 공동주택(UP 2019년)
        /// </summary>
        /// <param name="Page"></param>
        /// <param name="Apt_Code"></param>
        /// <returns></returns>
        public async Task<List<Appeal_Entity>> getlistPage(string AptCode)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                var lst = await dba.QueryAsync<Appeal_Entity>("Select Num, AptCode, AptName, apYear, apMonth, apDay, apClock, apMinute, apDongNo, apHoNo, apName, apHp, apPost, Bloom_Code, apTitle, apReciever, apContent, apOk, subYear, subMonth, subDay, subClock, subMinute, outViw, outName, outTelCom1, outTelCom2, outTelCom3, outNameCom, innViw, Complete, innName, subPost, innContent, apSatisfaction, PostDate, PostIP, ModifyDate, ModifyIP, ComFileName, ComFileSize, ComFileName2, ComFileSize2, ComFileName3, ComFileSize3, ComFileName4, ComFileSize4, ComFileName5, ComFileSize5, ComFileName6, ComFileSize6, ComFileName7, ComFileSize7, ComFileName8, ComFileSize8, ComFileName9, ComFileSize9, ComFileName10, ComFileSize10, ComAlias, ComTitle, AppealViw, apUserName, Private From Appeal Where AptCode = @AptCode Order By Num Desc", new { AptCode });
                return lst.ToList();
            }

        }

        /// <summary>
        /// 입주민 미완료 민원 내역 해당 공동주택(UP 2019년)
        /// </summary>
        public async Task<List<Appeal_Entity>> AppealListComplete(string AptCode)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                var lst = await dba.QueryAsync<Appeal_Entity>("Select Num, AptCode, AptName, apYear, apMonth, apDay, apClock, apMinute, apDongNo, apHoNo, apName, apHp, apPost, Bloom_Code, apTitle, apReciever, apContent, apOk, subYear, subMonth, subDay, subClock, subMinute, outViw, outName, outTelCom1, outTelCom2, outTelCom3, outNameCom, innViw, Complete, innName, subPost, innContent, apSatisfaction, PostDate, PostIP, ModifyDate, ModifyIP, ComFileName, ComFileSize, ComFileName2, ComFileSize2, ComFileName3, ComFileSize3, ComFileName4, ComFileSize4, ComFileName5, ComFileSize5, ComFileName6, ComFileSize6, ComFileName7, ComFileSize7, ComFileName8, ComFileSize8, ComFileName9, ComFileSize9, ComFileName10, ComFileSize10, ComAlias, ComTitle, AppealViw, apUserName, Private From Appeal Where AptCode = @AptCode And innViw != 'B' Order By Num Desc", new { AptCode });
                return lst.ToList();
            }

        }

        /// <summary>
        /// 입주민 미완료 민원 내역 해당 공동주택 수
        /// </summary>
        public async Task<int> AppealListCompleteCount(string AptCode)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                return await dba.QuerySingleOrDefaultAsync<int>("Select Count(*) From Appeal Where AptCode = @AptCode And innViw != 'B';", new { AptCode });
            }

        }

        /// <summary>
        /// 키워드로 검색 목록(2019년9월)
        /// </summary>
        public async Task<List<Appeal_Entity>> getlist_Search(int Page, string AptCode, string Word)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                var lst = await dba.QueryAsync<Appeal_Entity>("Select Top 15 Num, AptCode, AptName, apYear, apMonth, apDay, apClock, apMinute, apDongNo, apHoNo, apName, apHp, apPost, Bloom_Code, apTitle, apReciever, apContent, apOk, subYear, subMonth, subDay, subClock, subMinute, outViw, outName, outTelCom1, outTelCom2, outTelCom3, outNameCom, innViw, Complete, innName, subPost, innContent, apSatisfaction, PostDate, PostIP, ModifyDate, ModifyIP, ComFileName, ComFileSize, ComFileName2, ComFileSize2, ComFileName3, ComFileSize3, ComFileName4, ComFileSize4, ComFileName5, ComFileSize5, ComFileName6, ComFileSize6, ComFileName7, ComFileSize7, ComFileName8, ComFileSize8, ComFileName9, ComFileSize9, ComFileName10, ComFileSize10, ComAlias, ComTitle, AppealViw, apUserName, Private From Appeal Where Num Not In(Select Top(15 * " + @Page + ") Num From Appeal Where AptCode = @AptCode And (apContent like '%" + Word + "%' or apHp like '%" + Word + "%') Order By Num Desc) And AptCode = @AptCode And (apContent like '%" + Word + "%' or apHp like '%" + Word + "%') Order By Num Desc", new { Page, AptCode, Word });
                return lst.ToList();
            }
        }

        /// <summary>
        /// 키워드로 검색 목록(UP 2019년)
        /// </summary>
        public async Task<int> getlist_Search_Count(string AptCode, string Word)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                return await dba.QuerySingleOrDefaultAsync<int>("Select Count(*) From Appeal Where AptCode = @AptCode And (apContent like '%" + Word + "%' or apHp like '%" + Word + "%')", new { AptCode, Word });
            }

        }

        /// <summary>
        /// 입주민 민원 내역 해당 공동주택
        /// </summary>
        public async Task<List<Appeal_Entity>> getlist_apt_New(int Page, string AptCode, string StartDate, string EndDate)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                var lst = await dba.QueryAsync<Appeal_Entity>("Select Top 15 Num, AptCode, AptName, apYear, apMonth, apDay, apClock, apMinute, apDongNo, apHoNo, apName, apHp, apPost, Bloom_Code, apTitle, apReciever, apContent, apOk, subYear, subMonth, subDay, subClock, subMinute, outViw, outName, outTelCom1, outTelCom2, outTelCom3, outNameCom, innViw, Complete, innName, subPost, innContent, apSatisfaction, PostDate, PostIP, ModifyDate, ModifyIP, ComFileName, ComFileSize, ComFileName2, ComFileSize2, ComFileName3, ComFileSize3, ComFileName4, ComFileSize4, ComFileName5, ComFileSize5, ComFileName6, ComFileSize6, ComFileName7, ComFileSize7, ComFileName8, ComFileSize8, ComFileName9, ComFileSize9, ComFileName10, ComFileSize10, ComAlias, ComTitle, AppealViw, apUserName From Appeal Where Num Not In(Select Top(15 * " + @Page + ") Num From Appeal Where AptCode = @AptCode And PostDate Between @StartDate And @EndDate Order By Num Desc) And AptCode = @AptCode And PostDate Between @StartDate And @EndDate Order By Num Desc;", new { Page, AptCode, StartDate, EndDate });
                return lst.ToList();
            }

        }

        /// <summary>
        /// 입주민 민원 내역 해당 공동주택 수
        /// </summary>
        /// <param name="Apt_Code">공동주택 식별코드</param>
        /// <param name="StartDate">시작일</param>
        /// <param name="EndDate">종료일</param>
        /// <returns></returns>
        public async Task<int> getlist_apt_New_Count(string AptCode, string StartDate, string EndDate)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                return await dba.QuerySingleOrDefaultAsync<int>("Select Count(*) From Appeal Where AptCode = @AptCode And PostDate Between @StartDate And @EndDate;", new { AptCode, StartDate, EndDate });
            }

        }

        /// <summary>
        /// 민원 처리 내용 보여주기(구) 2019년
        /// </summary>
        public async Task<Appeal_Entity> ViewsSv(string Num, string AptCode)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                return await dba.QuerySingleOrDefaultAsync<Appeal_Entity>("Select subYear, subMonth, subDay, subClock, subMinute, outViw, outName, outTelCom1, outTelCom2, outTelCom3, outNameCom, innViw, Complete, innName, subPost, innContent, apSatisfaction, PostDate, PostIP, ModifyDate, ModifyIP, ComFileName, ComFileSize, ComFileName2, ComFileSize2, ComFileName3, ComFileSize3, ComFileName4, ComFileSize4, ComFileName5, ComFileSize5, ComFileName6, ComFileSize6, ComFileName7, ComFileSize7, ComFileName8, ComFileSize8, ComFileName9, ComFileSize9, ComFileName10, ComFileSize10, ComAlias, ComTitle, AppealViw, apUserName From Appeal Where AptCode = @AptCode And  Num = @Num", new { Num, AptCode });
            }

        }

        /// <summary>
        /// 구 민원처리 내용 존재 여부
        /// </summary>
        /// <param name="Num"></param>
        /// <returns></returns>
        public async Task<string> apOk(string Num)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                return await dba.QuerySingleOrDefaultAsync<string>("Select apOk From Appeal Where Num = @Num", new { Num });
            }

        }

        /// <summary>
        /// 민원 분류로 검색
        /// </summary>
        public async Task<List<Appeal_Entity>> getlistSort(string AptCode, string Bloom_Code)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                var lst = await dba.QueryAsync<Appeal_Entity>("Select Num, AptCode, AptName, apYear, apMonth, apDay, apClock, apMinute, apDongNo, apHoNo, apName, apHp, apPost, Bloom_Code, apTitle, apReciever, apContent, apOk, subYear, subMonth, subDay, subClock, subMinute, outViw, outName, outTelCom1, outTelCom2, outTelCom3, outNameCom, innViw, Complete, innName, subPost, innContent, apSatisfaction, PostDate, PostIP, ModifyDate, ModifyIP, ComFileName, ComFileSize, ComFileName2, ComFileSize2, ComFileName3, ComFileSize3, ComFileName4, ComFileSize4, ComFileName5, ComFileSize5, ComFileName6, ComFileSize6, ComFileName7, ComFileSize7, ComFileName8, ComFileSize8, ComFileName9, ComFileSize9, ComFileName10, ComFileSize10, ComAlias, ComTitle, AppealViw, apUserName, Private From Appeal Where AptCode = @AptCode And Bloom_Code = @Bloom_Code Order By Num Desc;", new { AptCode, Bloom_Code });
                return lst.ToList();
            }

        }

        /// <summary>
        /// 민원 분류로 검색 수
        /// </summary>
        /// <param name="AptCode"></param>
        /// <param name="Bloom_Code"></param>
        /// <returns></returns>
        public async Task<int> getlistSort_Count(string AptCode, string Bloom_Code)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                return await dba.QuerySingleOrDefaultAsync<int>("Select Count(*) From Appeal Where AptCode = @AptCode And Bloom_Code = @Bloom_Code;", new { AptCode, Bloom_Code });
            }

        }

        /// <summary>
        /// 민원 대분류로 검색
        /// </summary>
        /// <param name="AptCode"></param>
        /// <param name="Bloom_Code"></param>
        /// <returns></returns>
        public async Task<List<Appeal_Entity>> getlistSortA(int Page, string AptCode, string apTitle)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                var lst = await dba.QueryAsync<Appeal_Entity>("Select Top 15 Num, AptCode, AptName, apYear, apMonth, apDay, apClock, apMinute, apDongNo, apHoNo, apName, apHp, apPost, Bloom_Code, apTitle, apReciever, apContent, apOk, subYear, subMonth, subDay, subClock, subMinute, outViw, outName, outTelCom1, outTelCom2, outTelCom3, outNameCom, innViw, Complete, innName, subPost, innContent, apSatisfaction, PostDate, PostIP, ModifyDate, ModifyIP, ComFileName, ComFileSize, ComFileName2, ComFileSize2, ComFileName3, ComFileSize3, ComFileName4, ComFileSize4, ComFileName5, ComFileSize5, ComFileName6, ComFileSize6, ComFileName7, ComFileSize7, ComFileName8, ComFileSize8, ComFileName9, ComFileSize9, ComFileName10, ComFileSize10, ComAlias, ComTitle, AppealViw, apUserName, Private From Appeal Where Num Not In(Select Top(15 * " + @Page + ") Num From Appeal Where AptCode = @AptCode And apTitle Like '" + @apTitle + "%' Order By Num Desc) And AptCode = @AptCode And apTitle Like '" + @apTitle + "%' Order By Num Desc;", new { Page, AptCode, apTitle });
                return lst.ToList();
            }

        }

        /// <summary>
        /// 민원 대분류로 검색 수
        /// </summary>
        /// <param name="AptCode"></param>
        /// <param name="apTitle"></param>
        /// <returns></returns>
        public async Task<int> getlistSortA_Count(string AptCode, string apTitle)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                return await dba.QuerySingleOrDefaultAsync<int>("Select Count(*) From Appeal Where AptCode = @AptCode And apTitle Like '" + @apTitle + "%'; ", new { AptCode, apTitle });
            }

        }

        /// <summary>
        /// 입주민 민원 내역 수
        /// </summary>
        /// <param name="AptCode"></param>
        /// <returns></returns>
        public async Task<int> getlist_apt_count(string AptCode)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                return await dba.QuerySingleOrDefaultAsync<int>("Select Count(*) From Appeal Where AptCode = @AptCode", new { AptCode });
            }

        }

        /// <summary>
        /// 민원 파일 올리기
        /// </summary>
        /// <param name="Num"></param>
        public async Task File_Up(string ComFileName, int ComFileSize, string ComFileName2, int ComFileSize2, string ComFileName3, int ComFileSize3, string ComFileName4, int ComFileSize4, string ComFileName5, int ComFileSize5, int Num)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                await dba.ExecuteAsync("Update Appeal Set ComFileName = @ComFileName, ComFileSize = @ComFileSize, ComFileName2 = @ComFileName2, ComFileSize2 = @ComFileSize2, ComFileName3 = @ComFileName3, ComFileSize3 = @ComFileSize3, ComFileName4 = @ComFileName4, ComFileSize4 = @ComFileSize4, ComFileName5 = @ComFileName, ComFileSize5 = @ComFileSize5 Where Num = @Num", new { ComFileName, ComFileSize, ComFileName2, ComFileSize2, ComFileName3, ComFileSize3, ComFileName4, ComFileSize4, ComFileName5, ComFileSize5, Num });
            }

        }

        /// <summary>
        /// 민원 파일 올리기(응용)
        /// </summary>
        /// <param name="Num"></param>
        public async Task File_Up_set(string ComFileName_Field, string ComFileName_Query, string ComFileSize_Field, int ComFileSize_Query, int Num)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                await dba.ExecuteAsync("Update Appeal Set " + ComFileName_Field + " = @ComFileName_Query, " + ComFileSize_Field + " = @ComFileSize_Query  Where Num = @Num", new { ComFileName_Field, ComFileName_Query, ComFileSize_Field, ComFileSize_Query, Num });
            }

        }

        /// <summary>
        /// 민원 상세보기
        /// </summary>
        public async Task<Appeal_Entity> Detail(string Num)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                return await dba.QuerySingleOrDefaultAsync<Appeal_Entity>("Select Num, AptCode, AptName, apYear, apMonth, apDay, apClock, apMinute, apDongNo, apHoNo, apName, apHp, apPost, Bloom_Code, apTitle, apReciever, apContent, apOk, subYear, subMonth, subDay, subClock, subMinute, outViw, outName, outTelCom1, outTelCom2, outTelCom3, outNameCom, innViw, Complete, innName, subPost, innContent, apSatisfaction, PostDate, PostIP, ModifyDate, ModifyIP, ComFileName, ComFileSize, ComFileName2, ComFileSize2, ComFileName3, ComFileSize3, ComFileName4, ComFileSize4, ComFileName5, ComFileSize5, ComFileName6, ComFileSize6, ComFileName7, ComFileSize7, ComFileName8, ComFileSize8, ComFileName9, ComFileSize9, ComFileName10, ComFileSize10, ComAlias, ComTitle, AppealViw, apUserName, Private From Appeal Where Num = @Num", new { Num });
            }

        }

        /// <summary>
        /// 입주민 민원내용 수정
        /// </summary>
        /// <param name="appeal"></param>
        /// <returns></returns>
        public async Task<Appeal_Entity> Edit_Insert(Appeal_Entity appeal)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                await dba.ExecuteAsync("Update Appeal Set apYear = @apYear, apMonth = @apMonth, apDay = @apDay, apDongNo = @apDongNo, apHoNo = @apHoNO, apName = @apName, apPost = @apPost, apHp = @apHp, Bloom_Code = @Bloom_Code, apTitle = @apTitle, apReciever = @apReciever, apContent = @apContent, apSatisfaction = @apSatisfaction Where Num = @Num", appeal);
                return appeal;
            }

        }

        /// <summary>
        /// 입주민 민원내용 수정
        /// </summary>
        /// <param name="appeal"></param>
        /// <returns></returns>
        public async Task<Appeal_Entity> Edit(Appeal_Entity appeal)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                await dba.ExecuteAsync("Update Appeal Set apYear = @apYear, apMonth = @apMonth, apDay = @apDay, apName = @apName, apHp = @apHp, apContent = @apContent Where Num = @Num", appeal);
                return appeal;
            }

        }

        /// <summary>
        /// 민원 작업 입력
        /// </summary>
        /// <param name="appeal"></param>
        /// <returns></returns>
        public async Task<Appeal_Entity> Edit_JobWork(Appeal_Entity appeal)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                await dba.ExecuteAsync("ModifyAppeal", appeal, commandType: CommandType.StoredProcedure);
                return appeal;
            }

        }

        /// <summary>
        /// 민원 결재 완료 입력
        /// </summary>
        /// <param name="Num"></param>
        public async Task Edit_Complete(string Num)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                await dba.ExecuteAsync("ModifyApConform", new { Num }, commandType: CommandType.StoredProcedure);
            }

        }

        /// <summary>
        /// 결재여부 확인
        /// </summary>
        /// <param name="Num"></param>
        /// <returns></returns>
        public async Task<string> Complete(string Num)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                return await dba.QuerySingleOrDefaultAsync<string>("Select Complete From Appeal Where Num = @Num", new { Num });
            }

        }

        /// <summary>
        /// 처리 내용 존재여부
        /// </summary>
        /// <param name="Num"></param>
        public async Task apOkComplete(string Num)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                await dba.ExecuteAsync("Update Appeal Set apOk = 'B' Where Num = @Num", new { Num });
            }

        }

        /// <summary>
        /// 만족도 입력
        /// </summary>
        /// <param name="Num"></param>
        public async Task apSatisfaction(string Num, string Ok)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                await dba.ExecuteAsync("Update Appeal Set apSatisfaction = @Ok Where Num = @Num", new { Num, Ok });
            }

        }

        /// <summary>
        /// 민원 작업 완료
        /// </summary>
        /// <param name="Num"></param>
        public async Task Edit_WorkComplete(string Num)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                string InnViw = await dba.QuerySingleOrDefaultAsync<string>("Select innViw From Appeal Where Num = @Num", new { Num });
                if (InnViw == "A")
                {
                    await dba.ExecuteAsync("Update Appeal Set innViw = 'C' Where Num = @Num", new { Num });
                }
                else if (InnViw == "C")
                {
                    await dba.ExecuteAsync("Update Appeal Set innViw = 'B' Where Num = @Num", new { Num });
                }
                else
                {
                    await dba.ExecuteAsync("Update Appeal Set innViw = 'A' Where Num = @Num", new { Num });
                }
            }

        }

        /// <summary>
        /// 민원 처리 완료
        /// </summary>
        /// <param name="Num"></param>
        public async Task Modify_WorkComplete(string Num, string innViw)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                await dba.ExecuteAsync("Update Appeal Set innViw = @innViw Where Num = @Num", new { Num, innViw });
            }

        }

        /// <summary>
        /// 처리 완료 여부 확인
        /// </summary>
        /// <param name="Num"></param>
        /// <returns></returns>
        public async Task<string> innView(string Num)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                return await dba.QuerySingleOrDefaultAsync<string>("Select innViw From Appeal Where Num = @Num", new { Num });
            }

        }

        /// <summary>
        /// 첨부파일 보기
        /// </summary>
        /// <param name="Num"></param>
        /// <returns></returns>
        public async Task<Appeal_Entity> File_View(string Num)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                return await dba.QuerySingleOrDefaultAsync<Appeal_Entity>("Select ComFileName, ComFileSize, ComFileName2, ComFileSize2, ComFileName3, ComFileSize3, ComFileName4, ComFileSize4, ComFileName5, ComFileSize5 From Appeal Where Num = @Num", new { Num });
            }

        }

        /// <summary>
        /// 관리전산에서 입력 민원처리  첨부파일 관련 코드로 리스트 메서드
        /// </summary>
        public async Task<List<Sw_Files_Entity>> GetList_UpFile_Appeal(string Apt_Code, string Parents_Name, string Parents_Num)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                var lst = await dba.QueryAsync<Sw_Files_Entity>("Select Num, Sw_FileName, Sw_FileSize From Sw_Files Where AptCode = @Apt_Code And Parents_Name = @Parents_Name And Parents_Num = @Parents_Num Order By Num Asc", new { Apt_Code, Parents_Name, Parents_Num });
                return lst.ToList();
            }

        }

        /// <summary>
        /// 해당 민원처리 부분 첨부파일 수
        /// </summary>
        /// <param name="Apt_Code"></param>
        /// <param name="Parents_Name"></param>
        /// <param name="Parents_Num"></param>
        /// <returns></returns>
        public async Task<int> GetList_UpFile_Appeal_Count(string Apt_Code, string Parents_Name, string Parents_Num)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                return await dba.QuerySingleOrDefaultAsync<int>("Select Count(*) From Sw_Files Where AptCode = @Apt_Code And Parents_Name = @Parents_Name And Parents_Num = @Parents_Num", new { Apt_Code, Parents_Name, Parents_Num });
            }

        }

        /// <summary>
        // 앞민원
        /// </summary>
        /// <param name="AptCode"></param>
        /// <param name="Num"></param>
        /// <returns></returns>
        public async Task<string> apAgo(string AptCode, string Num)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                return await dba.QuerySingleOrDefaultAsync<string>("Select ISNULL(Num, 0) From Appeal Where AptCode = @AptCode and Num = (Select max(Num) From Appeal Where AptCode = @AptCode and Num < @Num)", new { AptCode, Num });
            }

        }

        /// <summary>
        // 앞민원  존재여부
        /// </summary>
        /// <param name="AptCode"></param>
        /// <param name="Num"></param>
        /// <returns></returns>
        public async Task<int> apAgoBe(string AptCode, string Num)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                return await dba.QuerySingleOrDefaultAsync<int>("Select Count(*) From Appeal Where AptCode = @AptCode and Num = (Select max(Num) From Appeal Where AptCode = @AptCode and Num < @Num)", new { AptCode, Num });
            }

        }

        /// <summary>
        // 뒤 민원
        /// </summary>
        /// <param name="AptCode"></param>
        /// <param name="Num"></param>
        /// <returns></returns>
        public async Task<string> apNext(string AptCode, string Num)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                return await dba.QuerySingleOrDefaultAsync<string>("Select ISNULL(Num, 0) From Appeal Where AptCode = @AptCode and Num = (Select Min(Num) From Appeal Where AptCode =@AptCode and Num > @Num)", new { AptCode, Num });
            }

        }

        /// <summary>
        // 뒤 민원 존재 여부
        /// </summary>
        /// <param name="AptCode"></param>
        /// <param name="Num"></param>
        /// <returns></returns>
        public async Task<int> apNextBe(string AptCode, string Num)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                return await dba.QuerySingleOrDefaultAsync<int>("Select Count(*) From Appeal Where AptCode = @AptCode and Num = (Select Min(Num) From Appeal Where AptCode =@AptCode and Num > @Num)", new { AptCode, Num });
            }

        }
    }

    /// <summary>
    /// 민원처리 내용 클래스
    /// </summary>
    public class subAppeal_Lib : IsubAppeal_Lib
    {
        private readonly IConfiguration _db;
        public subAppeal_Lib(IConfiguration configuration)
        {
            this._db = configuration;
        }

        /// <summary>
        /// 민원 집행 등록
        /// </summary>
        /// <param name="sub"></param>
        public async Task Add(subAppeal_Entity sub)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                await dba.ExecuteAsync("Insert Into subAppeal (apNum, AptCode, AptName, subDate, subYear, subMonth, subDay, outName, outMobile, outViw, innView, Complete, subWorker, subContent, subPost, subDuty, PostIP, AppealViw) Values (@apNum, @AptCode, @AptName, @subDate, @subYear, @subMonth, @subDay, @outName, @outMobile, @outViw, @innView, @Complete, @subWorker, @subContent, @subPost, @subDuty, @PostIP, @AppealViw)", sub);
            }

        }

        /// <summary>
        /// 수정
        /// </summary>
        /// <param name="sub"></param>
        public async Task Edit(subAppeal_Entity sub)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                await dba.ExecuteAsync("Update subAppeal Set outName = @outName, subDate = @subDate, subYear = @subYear, subMonth = @subMonth, subDay = @subDay, subContent = @subContent, subPost = @subPost, subDuty = @subDuty, subWorker = @subWorker Where subAid = @subAid", sub);
            }

        }

        /// <summary>
        /// 해당 민원 작업 목록
        /// </summary>
        /// <param name="apNum"></param>
        /// <returns></returns>
        public async Task<List<subAppeal_Entity>> GetList(string apNum)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                var lst = await dba.QueryAsync<subAppeal_Entity>("Select subAid, apNum, AptCode, AptName, subDate, subYear, subMonth, subDay, outName, outMobile, outViw, innView, Complete, subWorker, subContent, subPost, subDuty, PostDate, PostIP, AppealViw From subAppeal Where apNum =@apNum Order by subAid Desc", new { apNum });
                return lst.ToList();
            }

        }

        /// <summary>
        /// 해당 민원 작업 상세
        /// </summary>
        /// <param name="subAid"></param>
        /// <returns></returns>
        public async Task<subAppeal_Entity> Detail(string subAid)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                return await dba.QuerySingleOrDefaultAsync<subAppeal_Entity>("Select subAid, apNum, AptCode, AptName, subDate, subYear, subMonth, subDay, outName, outMobile, outViw, innView, Complete, subWorker, subContent, subPost, subDuty, PostDate, PostIP, AppealViw From subAppeal Where subAid =@subAid Order by subAid Desc", new { subAid });
            }

        }

        /// <summary>
        /// 민원에 입력되 수
        /// </summary>
        /// <param name="apNum"></param>
        /// <returns></returns>
        public async Task<int> BeCount(string apNum)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                return await dba.QuerySingleOrDefaultAsync<int>("Select Count(*) From subAppeal Where apNum = @apNum", new { apNum });
            }

        }

        /// <summary>
        /// 민원 메인 코드
        /// </summary>
        /// <param name="apNum"></param>
        /// <returns></returns>
        public async Task<string> apNum(string subAid)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                return await dba.QuerySingleOrDefaultAsync<string>("Select apNum From subAppeal Where subAid = @subAid", new { subAid });
            }

        }

        /// <summary>
        /// 삭제
        /// </summary>
        /// <param name="subAid"></param>
        public async Task Remove(string subAid)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                await dba.ExecuteAsync("Delete From subAppeal Where subAid = @subAid", new { subAid });
            }

        }

        /// <summary>
        /// 완료여부
        /// </summary>
        /// <param name="subAid"></param>
        /// <param name="Complete"></param>
        public async Task EditComplete(string subAid, string Complete)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                await dba.ExecuteAsync("Update subAppeal Set Complete = @Complete Where subAid = @subAid", new { subAid, Complete });
            }

        }
    }

    /// <summary>
    /// 민원처리자 클래
    /// </summary>
    public class subWorker_Lib : IsubWorker_Lib
    {
        private readonly IConfiguration _db;
        public subWorker_Lib(IConfiguration configuration)
        {
            this._db = configuration;
        }

        /// <summary>
        /// 민원 작업자 등록
        /// </summary>
        /// <param name="sub"></param>
        public async Task Add(subWorker_Entity sub)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                await dba.ExecuteAsync("Insert into subWorker (subAid, StaffCode, apNum, StaffName, PostIP) Values (@subAid, @StaffCode, @apNum, @StaffName, @PostIP)", sub);
            }

        }

        /// <summary>
        /// 작업자 목록
        /// </summary>
        /// <param name="subAid"></param>
        /// <returns></returns>
        public async Task<List<subWorker_Entity>> GetList(string subAid)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                var lst = await dba.QueryAsync<subWorker_Entity>("Select workAid, subAid, StaffCode, apNum, StaffName, PostDate, PostIP From subWorker Where subAid = @subAid", new { subAid });
                return lst.ToList();
            }

        }

        /// <summary>
        /// 민원 작업자 삭제
        /// </summary>
        /// <param name="workAid"></param>
        public async Task Remove(string workAid)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                await dba.ExecuteAsync("Delete From subWorker Where workAid = @workAid", new { workAid });
            }

        }
    }
}
