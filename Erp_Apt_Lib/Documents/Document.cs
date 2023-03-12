using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using System.Data;

namespace Erp_Apt_Lib.Documents
{
    /// <summary>
    /// 공문서 관리 분류 엔터티
    /// </summary>
    public class Document_Sort_Entity
    {
        public int Aid { get; set; }
        public string Sort_Name { get; set; }
        public string Sort_Step { get; set; }
        public string Up_Sort_Step { get; set; }
        public string Explanation { get; set; }
        public DateTime PostDate { get; set; }
        public string Apt_Code { get; set; }
    }

    /// <summary>
    /// 공문서 관리 분류 클래스
    /// </summary>
    public class Document_Sort_Lib : IDocument_Sort_Lib
    {
        private readonly IConfiguration _db;
        public Document_Sort_Lib(IConfiguration configuration)
        {
            this._db = configuration;
        }

        /// <summary>
        /// 공문서 관리 분류 저장
        /// </summary>
        public async Task<Document_Sort_Entity> Add(Document_Sort_Entity ds)
        {
            var sql = "Insert Document_Sort (Sort_Name, Sort_Step, Up_Sort_Step, Explanation, Apt_Code) Values (@Sort_Name, @Sort_Step, @Up_Sort_Step, @Explanation, @Apt_Code)";
            using (var dba = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
               await dba.ExecuteAsync(sql, ds);
                return ds;
            }
            
        }

        /// <summary>
        /// 공문서 관리 분류 수정
        /// </summary>
        public async Task<Document_Sort_Entity> Edit(Document_Sort_Entity ds)
        {
            //var sql = "Update Document_Sort Set Sort_Name = @Sort_Name, Sort_Step = @Sort_Step, Up_Sort_Step = @Up_Sort_Step, Explanation = @Explanation Where Aid = @Aid";
            var sql = "Update Document_Sort Set Sort_Name = @Sort_Name, Sort_Step = @Sort_Step, Up_Sort_Step = @Up_Sort_Step Where Aid = @Aid";
            using (var dba = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                await dba.ExecuteAsync(sql, ds);
                return ds;
            }            
        }

        /// <summary>
        /// 문서 관리 분류 목록
        /// </summary>
        public async Task<List<Document_Sort_Entity>> GetList(string Apt_Code)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                var lst = await dba.QueryAsync<Document_Sort_Entity>("Select Aid, Sort_Name, Sort_Step, Up_Sort_Step, Explanation, PostDate, Apt_Code From Document_Sort Where Apt_Code = 'swA'", new { Apt_Code });
                return lst.ToList();
            }            
        }

        /// <summary>
        /// 문서 관리 분류 목록
        /// </summary>
        public async Task<List<Document_Sort_Entity>> GetList_Page(int Page)
        {
            using var dba = new SqlConnection(_db.GetConnectionString("sw_togather"));
            var lst = await dba.QueryAsync<Document_Sort_Entity>("Select Top 15 * From Document_Sort Where Aid Not In (Select Top (15 * @Page) Aid From Document_Sort Order by Aid Desc) Order By Aid Desc", new { Page });
            return lst.ToList();
        }

        public async Task<int> GetList_Page_Count()
        {
            using var db = new SqlConnection(_db.GetConnectionString("sw_togather"));
            return await db.QuerySingleOrDefaultAsync<int>("Select Count(*) From Document_Sort");        }

        /// <summary>
        /// 문서 관리 분류 상세
        /// </summary>
        public async Task<Document_Sort_Entity> Details(int Aid)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                return await dba.QuerySingleOrDefaultAsync<Document_Sort_Entity>("Select Aid, Sort_Name, Sort_Step, Up_Sort_Step, Explanation, PostDate, Apt_Code From Documnet_Sort Where Aid = @Aid", new { Aid });
            }
            
        }

        /// <summary>
        /// 문서관리 분류 삭제
        /// </summary>
        public async Task Remove(int Aid)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                await dba.ExecuteAsync("Delete Document_Sort Where Aid = @Aid", new { Aid });
            }            
        }

        /// <summary>
        /// 분류명 불러오기
        /// </summary>
        public async Task<string> Sort_Name(string Aid)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                return await dba.QuerySingleOrDefaultAsync<string>("Select Sort_Name From Document_Sort Where Aid = @Aid", new { Aid });
            }
        }

        /// <summary>
        /// 분류명 불러오기
        /// </summary>
        public string SortName(string Aid)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                return dba.QuerySingleOrDefault<string>("Select Sort_Name From Document_Sort Where Aid = @Aid", new { Aid });
            }
        }

        /// <summary>
        /// 분류명 불러오기
        /// </summary>
        public async Task<string> Last_Code()
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                return await dba.QuerySingleOrDefaultAsync<string>("Select Top 1 Aid From Document_Sort Order by Aid Desc");
            }
        }
    }

    /// <summary>
    /// 공문서 관리 엔터티
    /// </summary>
    public class Document_Entity
    {
        public int Aid { get; set; }
        public string Sort_Code { get; set; }
        public string Doc_Code { get; set; }
        public string Organization { get; set; }

        /// <summary>
        /// 공개여부
        /// </summary>
        public string OpenPublic { get; set; }
        public string Title { get; set; }
        public string Details { get; set; }
        public string Division { get; set; }
        public string Etc { get; set; }
        public DateTime AcceptDate { get; set; }
        public string WorkMan { get; set; }
        public string Apt_Code { get; set; }
        public string Apt_Name { get; set; }
        public DateTime PostDate { get; set; }
        public string Approval { get; set; }
        /// <summary>
        /// 첨부파일 수
        /// </summary>
        public int Files_Count { get; set; }
    }

    public class Document_Lib : IDocument_Lib
    {
        private readonly IConfiguration _db;
        public Document_Lib(IConfiguration configuration)
        {
            this._db = configuration;
        }

        /// <summary>
        /// 공문서 입력
        /// </summary>
        /// <param name="dc"></param>
        /// <returns></returns>
        public async Task<int> Add(Document_Entity dc)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                var sql = "Insert Document (Sort_Code, Doc_Code, OpenPublic, Title, Organization, Details, Division, Etc, AcceptDate, WorkMan, Apt_Code, Apt_Name) Values (@Sort_Code, @Doc_Code, @OpenPublic, @Title, @Organization, @Details, @Division, @Etc, @AcceptDate, @WorkMan, @Apt_Code, @Apt_Name);Select Cast(SCOPE_IDENTITY() As Int);";
                return await dba.QuerySingleOrDefaultAsync<int>(sql, dc);
            }
            
        }

        /// <summary>
        /// 공문서 수정
        /// </summary>
        /// <param name="dc"></param>
        /// <returns></returns>
        public async Task<Document_Entity> Edit(Document_Entity dc)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                var sql = "Update Document Set Sort_Code = @Sort_Code, Title = @Title, Organization = @Organization, Details = @Details, Division = @Division, Etc = @Etc, AcceptDate = @AcceptDate Where Aid = @Aid";
                await dba.ExecuteAsync(sql, dc);
                return dc;
            }
            
        }

        /// <summary>
        /// 공개여부
        /// </summary>
        /// <param name="Aid"></param>
        public async Task getOpen(int Aid)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                string a = await dba.QuerySingleOrDefaultAsync<string>("Select OpenPublic From Documnet Where Aid = @Aid", new { Aid });
                if (a == "A")
                {
                    await dba.ExecuteAsync("Update Document Set OpenPublic = 'B' Where Aid = @Aid", new { Aid });
                }
                else
                {
                    await dba.ExecuteAsync("Update Document Set OpenPublic = 'A' Where Aid = @Aid", new { Aid });
                }
            }
            
        }

        /// <summary>
        /// 공문서 목록
        /// </summary>
        /// <param name="Apt_Code"></param>
        /// <returns></returns>
        public async Task<List<Document_Entity>> GetList(string Apt_Code)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                var lst = await dba.QueryAsync<Document_Entity>("Select Aid, Sort_Code, Doc_Code, OpenPublic, Title, Organization, Details, Division, Etc, AcceptDate, WorkMan, Apt_Code, Apt_Name, PostDate, Approval, Files_Count From Document Where Apt_Code = @Apt_Code Order By AcceptDate Desc, Aid Desc", new { Apt_Code });
                return lst.ToList();
            }
            
        }

        /// <summary>
        /// 공문서 목록
        /// </summary>
        /// <param name="Apt_Code"></param>
        /// <returns></returns>
        public async Task<List<Document_Entity>> GetList_Page(int Page, string Apt_Code)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                var lst = await dba.QueryAsync<Document_Entity>("Select Top 15 Aid, Sort_Code, Doc_Code, OpenPublic, Title, Organization, Details, Division, Etc, AcceptDate, WorkMan, Apt_Code, Apt_Name, PostDate, Approval, Files_Count From Document Where Aid Not In (Select Top (15 * @Page) Aid From Document Where Apt_Code = @Apt_Code Order By AcceptDate Desc, Aid Desc) And Apt_Code = @Apt_Code Order By AcceptDate Desc, Aid Desc", new {Page, Apt_Code });
                return lst.ToList();
            }

        }

        /// <summary>
        /// 공문서 목록 수
        /// </summary>
        /// <param name="Apt_Code"></param>
        /// <returns></returns>
        public async Task<int> GetList_Count(string Apt_Code)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                return await dba.QuerySingleOrDefaultAsync<int>("Select Count(*) From Document Where Apt_Code = @Apt_Code", new { Apt_Code });
            }
            
        }

        /// <summary>
        /// 구분 및 단지별 입력된 수
        /// </summary>
        /// <param name="Apt_Code"></param>
        /// <returns></returns>
        public async Task<int> Division_Count(string Apt_Code, string Division, string Start, string End)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                return await dba.QuerySingleOrDefaultAsync<int>("Select Count(*) From Document Where Apt_Code = @Apt_Code And Division = @Division And AcceptDate between @Start And @End", new { Apt_Code, Division, Start, End });
            }
            
        }

        /// <summary>
        /// 공문서 상세
        /// </summary>
        /// <param name="Aid"></param>
        /// <returns></returns>
        public async Task<Document_Entity> Details(int Aid)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                return await dba.QuerySingleOrDefaultAsync<Document_Entity>("Select Aid, Sort_Code, Doc_Code, OpenPublic, Title, Organization, Details, Division, Etc, AcceptDate, WorkMan, Apt_Code, Apt_Name, PostDate, Approval, Files_Count From Document Where Aid = @Aid", new { Aid });
            }
            
        }

        /// <summary>
        /// 공문서 삭제
        /// </summary>
        /// <param name="Aid"></param>
        public async Task Remove(int Aid)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                await dba.ExecuteAsync("Delete Document Where Aid = @Aid", new { Aid });
            }
            
        }

        /// <summary>
        /// 찾기
        /// </summary>
        /// <param name="Feild"></param>
        /// <param name="Query"></param>
        /// <returns></returns>
        public async Task<List<Document_Entity>> SearchList(string Feild, string Query, string Apt_Code)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                var lst = await dba.QueryAsync<Document_Entity>("Select Aid, Sort_Code, Doc_Code, OpenPublic, Title, Organization, Details, Division, Etc, AcceptDate, WorkMan, Apt_Code, Apt_Name, PostDate, Approval, Files_Count From Document Where Apt_Code = @Apt_Code And " + Feild + " Like '%" + Query + "%' Order By Aid Desc", new { Feild, Query, Apt_Code });
                return lst.ToList();
            }
            
        }

        /// <summary>
        /// 찾기
        /// </summary>
        /// <param name="Feild"></param>
        /// <param name="Query"></param>
        /// <returns></returns>
        public async Task<List<Document_Entity>> SearchList_Page(int Page, string Feild, string Query, string Apt_Code)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                var lst = await dba.QueryAsync<Document_Entity>("Select Top 15 Aid, Sort_Code, Doc_Code, OpenPublic, Title, Organization, Details, Division, Etc, AcceptDate, WorkMan, Apt_Code, Apt_Name, PostDate, Approval, Files_Count From Document Where Aid Not In(Select Top (15 * @Page) Aid From Document Where Apt_Code = @Apt_Code And " + Feild + " Like '%" + Query + "%' Order By Aid Desc) And Apt_Code = @Apt_Code And " + Feild + " Like '%" + Query + "%' Order By Aid Desc", new { Page, Feild, Query, Apt_Code });
                return lst.ToList();
            }
        }


        /// <summary>
        /// 찾기
        /// </summary>
        /// <param name="Feild"></param>
        /// <param name="Query"></param>
        /// <returns></returns>
        public async Task<int> SearchList_Count(string Feild, string Query, string Apt_Code)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                return await dba.QuerySingleOrDefaultAsync<int>("Select Count(*) From Document Where " + Feild + " Like '%" + Query + "%' And Apt_Code = @Apt_Code", new { Feild, Query, Apt_Code });
            }
            
        }

        /// <summary>
        /// 결재완려 여부(승인 or 미승인)
        /// </summary>
        /// <param name="Aid"></param>
        /// <param name="View"></param>
        public async Task Document_Comform(int Aid, string View)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                await dba.ExecuteAsync("Update Document Set Approval = @View Where Aid = @Aid", new { Aid, View });
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
            using (var dba = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                return await dba.QuerySingleOrDefaultAsync<string>("Select ISNULL(Aid, 0) From Document Where Apt_Code = @AptCode and Aid = (Select max(Aid) From Document Where Apt_Code = @AptCode and Aid < @Aid)", new { AptCode, Aid });
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
            using (var dba = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                return await dba.QuerySingleOrDefaultAsync<int>("Select Count(*) From Document Where Apt_Code = @AptCode and Aid = (Select max(Aid) From Document Where Apt_Code = @AptCode and Aid < @Aid)", new { AptCode, Aid });
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
            using (var dba = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                return await dba.QuerySingleOrDefaultAsync<string>("Select ISNULL(Aid, 0) From Document Where Apt_Code = @AptCode and Aid = (Select Min(Aid) From Document Where Apt_Code =@AptCode and Aid > @Aid)", new { AptCode, Aid });
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
            using (var dba = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                return await dba.QuerySingleOrDefaultAsync<int>("Select Count(*) From Document Where Apt_Code = @AptCode and Aid = (Select Min(Aid) From Document Where Apt_Code =@AptCode and Aid > @Aid)", new { AptCode, Aid });
            }
            
        }

        /// <summary>
        /// 첨부파일 추가 또는 삭제
        /// </summary>
        /// <param name="Aid"></param>
        /// <param name="Division"></param>
        /// <returns></returns>
        public async Task FilesCount(int Aid, string Division)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                if (Division == "A")
                {
                    await dba.ExecuteAsync("Update Document Set Files_Count = Files_Count + 1 Where Aid = @Aid", new { Aid });
                }
                else if (Division == "B")
                {
                    await dba.ExecuteAsync("Update Document Set Files_Count = Files_Count - 1 Where Aid = @Aid", new { Aid });
                }

            }
        }
    }
}
