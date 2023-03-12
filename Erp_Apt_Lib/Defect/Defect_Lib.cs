using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Erp_Apt_Lib
{
    /// <summary>
    /// 하자정보 클래스
    /// </summary>
    public class Defect_Lib : IDefect_Lib
    {
        private readonly IConfiguration _db;
        public Defect_Lib(IConfiguration configuration)
        {
            this._db = configuration;
        }

        /// <summary>
        /// 전용 하자 신청
        /// </summary>
        /// <param name="df"></param>
        /// <returns></returns>
        public async Task<Defect_Entity> Add_Private(Defect_Entity df)
        {
            var sql = "Insert Into Defect (AptCode, AptName, Company_Code, Company_Name, Private, Dong, Ho, InnerName, Mobile, Email, Relation, Etc, DefectDate, dfYear, dfMonth, dfDay, Bloom_Code_A, Bloom_Name_A, Bloom_Code_B, Bloom_Name_B, Bloom_Code_C, Bloom_Name_C, Period, Position, Position_Code, dfPost, dfApplicant, dfApplicant_Code, dfTitle, dfContent, PostIp, User_Code) Values (@AptCode, @AptName, @Company_Code, @Company_Name, @Private, @Dong, @Ho, @InnerName, @Mobile, @Email, @Relation, @Etc, @DefectDate, @dfYear, @dfMonth, @dfDay, @Bloom_Code_A, @Bloom_Name_A, @Bloom_Code_B, @Bloom_Name_B, @Bloom_Code_C, @Bloom_Name_C, @Period, @Position, @Position_Code, @dfPost, @dfApplicant, @dfApplicant_Code, @dfTitle, @dfContent, @PostIp, @User_Code)";
            using var dba = new SqlConnection(_db.GetConnectionString("sw_togather"));
            await dba.ExecuteAsync(sql, df);
            return df;

        }

        /// <summary>
        /// 공용 하자 신청
        /// </summary>
        /// <param name="df"></param>
        /// <returns></returns>
        public async Task<Defect_Entity> Add_Official(Defect_Entity df)
        {
            var sql = "Insert Into Defect (AptCode, AptName, Company_Code, Company_Name, Private, DefectDate, dfYear, dfMonth, dfDay, Bloom_Code_A, Bloom_Name_A, Bloom_Code_B, Bloom_Name_B, Bloom_Code_C, Bloom_Name_C, Period, Position, Position_Code, dfPost, dfApplicant, dfApplicant_Code, dfTitle, dfContent, PostIp, Relation, User_Code) Values(@AptCode, @AptName, @Company_Code, @Company_Name, @Private, @DefectDate, @dfYear, @dfMonth, @dfDay, @Bloom_Code_A, @Bloom_Name_A, @Bloom_Code_B, @Bloom_Name_B, @Bloom_Code_C, @Bloom_Name_C, @Period, @Position, @Position_Code, @dfPost, @dfApplicant, @dfApplicant_Code, @dfTitle, @dfContent, @PostIp, @Relation, @User_Code)";
            using (var dba = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                await dba.ExecuteAsync(sql, df);
                return df;
            }
        }

        /// <summary>
        /// 완료여부 입력
        /// </summary>
        public async Task Edit_Complete(Defect_Entity df)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                await dba.ExecuteAsync("Update Defect Set Complete = @Complete, dfSatisfaction = @dfSatisfaction Where Aid = @Aid", df);               
            }
        }

        /// <summary>
        /// 완료여부 입력
        /// </summary>
        public async Task<string> CompleteView(int Aid)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
               return await dba.QuerySingleOrDefaultAsync("Select Complete Where Aid = @Aid", new { Aid });
            }
        }

        /// <summary>
        /// 결재 완료 입력
        /// </summary>
        public async Task Edit_Approval(string Approval, int Aid)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                await dba.ExecuteAsync("Update Defect Set Approval = @Approval Where Aid = @Aid", new { Approval, Aid });
            }
        }

        /// <summary>
        /// 첨부파일 입력
        /// </summary>
        public async Task Edit_ImagesCount(int Aid)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                await dba.ExecuteAsync("Update Defect Set ImagesCount = ImagesCount + 1 Where Aid = @Aid", new { Aid });
            }
        }

        /// <summary>
        /// 처리 입력
        /// </summary>
        public async Task Edit_dfSatisfaction(Defect_Entity df)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                await dba.ExecuteAsync("Update Defect Set subCompany_Code = @subCompany_Code, subCompany_Name = @subCompany_Name, Inform = 'B', Etc = @Etc Where Aid = @Aid", df);
            }
        }

        /// <summary>
        /// 수정(공용)
        /// </summary>
        public async Task Edit_Official(Defect_Entity df)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                await dba.ExecuteAsync("Update Defect Set DefectDate = @DefectDate, dfYear = @dfYear, dfMonth = @dfMonth, dfDay = @dfDay, Bloom_Code_A = @Bloom_Code_A, Bloom_Name_A = @Bloom_Name_A, Bloom_Code_B = @Bloom_Code_B, Bloom_Name_B = @Bloom_Name_B, Bloom_Code_C = @Bloom_Code_C, Bloom_Name_C = @Bloom_Name_C, Period = @Period, Position = @Position, Position_Code = @Position_Code, dfPost = @dfPost, dfApplicant = @dfApplicant, dfApplicant_Code = @dfApplicant, dfTitle = @dfTitle, dfContent = @dfContent, Relation = @Relation, User_Code = @User_Code Where Aid = @Aid", df);
            }
        }

        /// <summary>
        /// 수정(전용)
        /// </summary>
        public async Task Edit_Private(Defect_Entity df)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                await dba.ExecuteAsync("Update Defect Set Dong = @Dong And Ho = @Ho, InnerName = @InnerName, Mobile = @Mobile, Email = @Email, Relation = @Relation, Etc = @Etc, DefectDate = @DefectDate, dfYear = @dfYear, dfMonth = @dfMonth, dfDay = @dfDay, Bloom_Code_A = @Bloom_Code_A, Bloom_Name_A = @Bloom_Name_A, Bloom_Code_B = @Bloom_Code_B, Bloom_Name_B = @Bloom_Name_B, Bloom_Code_C = @Bloom_Code_C, Bloom_Name_C = @Bloom_Name_C, Period = @Period, Position = @Position, Position_Code = @Position_Code, dfPost = @dfPost, dfApplicant = @dfApplicant, dfApplicant_Code = @dfApplicant, dfTitle = @dfTitle, dfContent = @dfContent, User_Code = @User_Code Where Aid = @Aid", df);
            }
        }

        /// <summary>
        /// 수정(전용)
        /// </summary>
        public async Task Edit_Private_People(Defect_Entity df)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                await dba.ExecuteAsync("Update Defect Set Mobile = @Mobile, Email = @Email, Relation = @Relation, Etc = @Etc, DefectDate = @DefectDate, dfYear = @dfYear, dfMonth = @dfMonth, dfDay = @dfDay, Bloom_Code_B = @Bloom_Code_B, Bloom_Name_B = @Bloom_Name_B, Bloom_Code_C = @Bloom_Code_C, Bloom_Name_C = @Bloom_Name_C, Position = @Position, Position_Code = @Position_Code, dfTitle = @dfTitle, dfContent = @dfContent Where Aid = @Aid", df);
            }
        }

        /// <summary>
        /// 하자 목록
        /// </summary>
        /// <param name="AptCode"></param>
        /// <returns></returns>
        public async Task<List<Defect_Entity>> GetList(string AptCode)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                var lst = await dba.QueryAsync<Defect_Entity>("Select * From Defect Where AptCode = @AptCode Order By Aid Desc", new { AptCode });
                return lst.ToList();
            }
        }

        /// <summary>
        /// 하자 목록(각 세대별)
        /// </summary>
        /// <param name="AptCode"></param>
        /// <returns></returns>
        public async Task<List<Defect_Entity>> GetList_DongHo(string AptCode, string Dong, string Ho)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                var lst = await dba.QueryAsync<Defect_Entity>("Select * From Defect Where AptCode = @AptCode And Dong = @Dong And Ho = @Ho Order By Aid Desc", new { AptCode, Dong, Ho });
                return lst.ToList();
            }
        }

        /// <summary>
        /// 각 세대 하자 신청 건수
        /// </summary>
        public async Task<int> GetList_DongHoCount(string AptCode, string Dong, string Ho)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                return await dba.QuerySingleOrDefaultAsync<int>("Select Count(*) From Defect Where AptCode = @AptCode And Dong = @Dong And Ho = @Ho", new { AptCode, Dong, Ho });
            }
        }

        /// <summary>
        /// 입력된 전체 목록 수
        /// </summary>
        public async Task<int> GetListCount(string AptCode)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                return await dba.QuerySingleOrDefaultAsync<int>("Select Count(*) From Defect Where AptCode = @AptCode", new { AptCode });
            }
        }

        /// <summary>
        /// 하자 목록(페이징)
        /// </summary>
        public async Task<List<Defect_Entity>> GetList_Page(int Page, string AptCode)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                var lst = await dba.QueryAsync<Defect_Entity>("Select Top 15 * From Defect Where Aid Not In(Select Top(15 * @Page) Aid From Defect Where AptCode = @AptCode Order By Aid Desc) And AptCode = @AptCode Order By Aid Desc", new { Page, AptCode });
                return lst.ToList();
            }
        }

        /// <summary>
        /// 입력된 전체 목록 수
        /// </summary>
        public async Task<int> GetList_Sort_Count(string AptCode, string Sort)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                return await dba.QuerySingleOrDefaultAsync<int>("Select Count(*) From Defect Where AptCode = @AptCode And Bloom_Name_A = @Sort", new { AptCode, Sort });
            }
        }

        /// <summary>
        /// 하자 목록(페이징)
        /// </summary>
        public async Task<List<Defect_Entity>> GetList_Sort_Page(int Page, string AptCode, string Sort)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                var lst = await dba.QueryAsync<Defect_Entity>("Select Top 15 * From Defect Where Aid Not In(Select Top(15 * @Page) Aid From Defect Where AptCode = @AptCode And Bloom_Name_A = @Sort Order By Aid Desc) And AptCode = @AptCode And Bloom_Name_A = @Sort Order By Aid Desc", new { Page, AptCode, Sort });
                return lst.ToList();
            }
        }

        /// <summary>
        /// 입력된 전체 목록 수
        /// </summary>
        public async Task<int> GetList_Details_Count(string AptCode, string Details)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                return await dba.QuerySingleOrDefaultAsync<int>("Select Count(*) From Defect Where AptCode = @AptCode And dfContent Like '%" + Details + "%'", new { AptCode, Details });
            }
        }

        /// <summary>
        /// 하자 목록(페이징)
        /// </summary>
        public async Task<List<Defect_Entity>> GetList_Details_Page(int Page, string AptCode, string Details)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                var lst = await dba.QueryAsync<Defect_Entity>("Select Top 15 * From Defect Where Aid Not In(Select Top(15 * @Page) Aid From Defect Where AptCode = @AptCode And dfContent Like '%" + Details + "%' Order By Aid Desc) And AptCode = @AptCode And dfContent Like '%" + Details + "%' Order By Aid Desc", new { Page, AptCode, Details });
                return lst.ToList();
            }
        }

        /// <summary>
        /// 하자 찾기
        /// </summary>
        public async Task<List<Defect_Entity>> SearchList(string AptCode, string Feild, string Query)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                var lst = await dba.QueryAsync<Defect_Entity>("Select * From Defect Where Feild = @Feild And Query = '%'" + @Query + "%' And AptCode = @AptCode Order By Aid Desc", new { Feild, Query, AptCode });
                return lst.ToList();
            }
        }

        /// <summary>
        /// 하자 상세보기
        /// </summary>
        /// <param name="Aid"></param>
        /// <returns></returns>
        public async Task<Defect_Entity> Details(int Aid)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                return await dba.QuerySingleOrDefaultAsync<Defect_Entity>("Select * From Defect Where Aid = @Aid", new { Aid});
            }
        }

        /// <summary>
        /// 삭제
        /// </summary>
        /// <param name="Aid"></param>
        /// <returns></returns>
        public async Task Remove(int Aid)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                await dba.ExecuteAsync("Delete Defect Where Aid = @Aid", new { Aid });
            }
        }

        /// <summary>
        /// 세대 하자 목록
        /// </summary>
        public async Task<List<Defect_Entity>> Private_List(string Dong, string Ho)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                var lst = await dba.QueryAsync<Defect_Entity>("Select * From Defect Where Dong = @Dong And Ho = @Ho Order By Aid Desc", new { Dong, Ho });
                return lst.ToList();
            }
        }

        
    }
}
