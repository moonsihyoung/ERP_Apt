using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using System.Linq;
using Microsoft.Data.SqlClient;

namespace Erp_Apt_Lib.ProofReport
{
    /// <summary>
    /// 제 증명서 속성
    /// </summary>
    public class ProofRepot_Entity
    {
        /// <summary>
        /// 식별코드
        /// </summary>
        [Key]
        public int ProofReportId { get; set; }
        /// <summary>
        /// 아파트 코드
        /// </summary>
        public string AptCode { get; set; }
        /// <summary>
        /// 아파트 명
        /// </summary>
        public string AptName { get; set; }
        /// <summary>
        /// 요청자 아이디
        /// </summary>
        public string UserCode { get; set; }
        /// <summary>
        /// 요청자 명
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 모바일
        /// </summary>
        public string Mobile { get; set; }
        /// <summary>
        /// 생년월일
        /// </summary>
        public string Scn { get; set; }

        /// <summary>
        /// 제 증명서 이름
        /// </summary>
        public string ProofName { get; set; }
        /// <summary>
        /// 시작 일
        /// </summary>
        public DateTime StartDate { get; set; }
        /// <summary>
        /// 종료일
        /// </summary>
        public DateTime EndDate { get; set; }
        /// <summary>
        /// 부서 직책
        /// </summary>
        public string PostDuty { get; set; }
        /// <summary>
        /// 사업장명
        /// </summary>
        public string UserPlace { get; set; }

        /// <summary>
        /// 요청일
        /// </summary>
        public DateTime UseDate { get; set; }

        /// <summary>
        /// 발급처 식별코드
        /// </summary>
        public string CompanyCode { get; set; }
        /// <summary>
        /// 발급처
        /// </summary>
        public string CompanyName { get; set; }
        /// <summary>
        /// 발급처 주소
        /// </summary>
        public string Adress { get; set; }
        /// <summary>
        /// 연락처
        /// </summary>
        public string Telephone { get; set; }
        /// <summary>
        /// 대표자
        /// </summary>
        public string CeoName { get; set; }
        /// <summary>
        /// 승인자 식별코드
        /// </summary>
        public string ResultUserCode { get; set; }
        /// <summary>
        /// 기타사항
        /// </summary>
        public string Etc { get; set; }
        /// <summary>
        /// 결재 여부
        /// </summary>
        public string Approval { get; set; }
        /// <summary>
        /// 입력일
        /// </summary>
        public DateTime PostDate { get; set; }
        /// <summary>
        /// 입력자 아이피
        /// </summary>
        public string PostIp { get; set; }

        /// <summary>
        /// 퇴사여부(A : 근무중, B : 퇴사)
        /// </summary>
        public string Resignation { get; set; }
    }

    /// <summary>
    /// 제 증명서 관리 정보 인터페이스
    /// </summary>
    public interface IProofReport_Lib
    {
        Task Add(ProofRepot_Entity pr);
        Task<List<ProofRepot_Entity>> GetProofs_Code(string CompanyCode);
        Task<List<ProofRepot_Entity>> GetProofs();
        Task<List<ProofRepot_Entity>> GetProofs_All(int Page);
        Task<int> GetProofs_All_Count();
        Task<List<ProofRepot_Entity>> GetProofs_Apt(int Page, string AptCode);
        Task<int> GetProofs_Apt_Count(string AptCode);
        /// <summary>
        /// 해당 공동주택에서 이름으로 검색
        /// </summary>
        Task<List<ProofRepot_Entity>> GetProofs_Apt_Name(int Page, string AptCode, string Name);
        Task<int> GetProofs_Apt_Name_Count(string AptCode, string Name);

        Task<List<ProofRepot_Entity>> GetProofs_User(string UserCode);
        Task Edit(ProofRepot_Entity pr);
        Task<ProofRepot_Entity> Details(string ProofReportId);
        Task Approval(string ProofReportId, string ResultUserCode);
        Task Etc_Add(string ProofReportId, string Etc);
        Task Remove(string Aid);        
    }

    /// <summary>
    /// 제 증명서 정보
    /// </summary>
    public class ProofReport_Lib :IProofReport_Lib
    {
        private readonly IConfiguration _db;
        public ProofReport_Lib(IConfiguration configuration)
        {
            this._db = configuration;
        }

        /// <summary>
        /// 제 증명서 입력
        /// </summary>
        /// <param name="pr"></param>
        public async Task Add(ProofRepot_Entity pr)
        {
            var sql = "Insert Into ProofReport ( AptCode, AptName, UserCode, UserName, ProofName, Scn, Mobile, StartDate, EndDate, PostDuty, UserPlace, UseDate, CompanyCode, CompanyName, Adress, Telephone, CeoName, Etc, PostIp, Resignation) Values (@AptCode, @AptName, @UserCode, @UserName, @ProofName, @Scn, @Mobile, @StartDate, @EndDate, @PostDuty, @UserPlace, @UseDate, @CompanyCode, @CompanyName, @Adress, @Telephone, @CeoName, @Etc, @PostIp, @Resignation);";
            using (var db = new SqlConnection(_db.GetConnectionString("sw_Lib")))
            {
               await db.ExecuteAsync(sql, pr);
            }
            
        }

        /// <summary>
        /// 제증명서 발급처별 목록
        /// </summary>
        /// <param name="CompanyCode"></param>
        /// <returns></returns>
        public async Task<List<ProofRepot_Entity>> GetProofs_Code(string CompanyCode)
        {
            using (var db = new SqlConnection(_db.GetConnectionString("sw_Lib")))
            {
                var lst = await db.QueryAsync<ProofRepot_Entity>("Select ProofReportId, AptCode, AptName, UserCode, UserName, Scn, Mobile, ProofName, StartDate, EndDate, PostDuty, UserPlace, UseDate, CompanyCode, CompanyName, Adress, Telephone, CeoName, ResultUserCode, Etc, Approval, PostDate, PostIp, Resignation From ProofReport Where CompanyCode = @Company_Code Order By ProofReportId Desc", new { CompanyCode });
                return lst.ToList();
            }
           
        }

        /// <summary>
        /// 제증명서 발급처별 목록
        /// </summary>
        /// <param name="CompanyCode"></param>
        /// <returns></returns>
        public async Task<List<ProofRepot_Entity>> GetProofs()
        {
            using (var db = new SqlConnection(_db.GetConnectionString("sw_Lib")))
            {
                var lst = await db.QueryAsync<ProofRepot_Entity>("Select ProofReportId, AptCode, AptName, UserCode, UserName, Scn, Mobile, ProofName, StartDate, EndDate, PostDuty, UserPlace, UseDate, CompanyCode, CompanyName, Adress, Telephone, CeoName, ResultUserCode, Approval, Etc, PostDate, PostIp, Resignation From ProofReport Order By ProofReportId Desc");
                return lst.ToList();
            }
            
        }

        /// <summary>
        /// 제증명서 발급처별 목록
        /// </summary>
        public async Task<List<ProofRepot_Entity>> GetProofs_All(int Page)
        {
            using (var db = new SqlConnection(_db.GetConnectionString("sw_Lib")))
            {
                var lst = await db.QueryAsync<ProofRepot_Entity>("Select Top 15 ProofReportId, AptCode, AptName, UserCode, UserName, Scn, Mobile, ProofName, StartDate, EndDate, PostDuty, UserPlace, UseDate, CompanyCode, CompanyName, Adress, Telephone, CeoName, ResultUserCode, Etc, Approval, PostDate, PostIp, Resignation From ProofReport Where ProofReportId Not In (Select Top (15 * @Page) ProofReportId From ProofReport Order By ProofReportId Desc) Order By ProofReportId Desc", new { Page });
                return lst.ToList();
            }            
        }

        public async Task<int> GetProofs_All_Count()
        {
            using (var db = new SqlConnection(_db.GetConnectionString("sw_Lib")))
            {
                return await db.QuerySingleOrDefaultAsync<int>("Select Count(*) From ProofReport");
               
            }
        }

        /// <summary>
        /// 제증명서 발급처별 목록
        /// </summary>
        /// <param name="CompanyCode"></param>
        /// <returns></returns>
        public async Task<List<ProofRepot_Entity>> GetProofs_Apt(int Page, string AptCode)
        {
            using (var db = new SqlConnection(_db.GetConnectionString("sw_Lib")))
            {
                var lst = await db.QueryAsync<ProofRepot_Entity>("Select Top 15 ProofReportId, AptCode, AptName, UserCode, UserName, Scn, Mobile, ProofName, StartDate, EndDate, PostDuty, UserPlace, UseDate, CompanyCode, CompanyName, Adress, Telephone, CeoName, ResultUserCode, Etc, Approval, PostDate, PostIp, Resignation From ProofReport Where ProofReportId Not In (Select Top (15 * @Page) ProofReportId From ProofReport Where AptCode = @AptCode Order By ProofReportId Desc) And AptCode = @AptCode Order By ProofReportId Desc", new { Page, AptCode });
                return lst.ToList();
            }
            
        }

        public async Task<int> GetProofs_Apt_Count(string AptCode)
        {
            using (var db = new SqlConnection(_db.GetConnectionString("sw_Lib")))
            {
                return await db.QuerySingleOrDefaultAsync<int>("Select Count(*) From ProofReport Where AptCode = @AptCode", new { AptCode });
            }
        }

        /// <summary>
        /// 제증명서 발급처별 목록
        /// </summary>
        public async Task<List<ProofRepot_Entity>> GetProofs_Apt_Name(int Page, string AptCode, string Name)
        {
            using (var db = new SqlConnection(_db.GetConnectionString("sw_Lib")))
            {
                var lst = await db.QueryAsync<ProofRepot_Entity>("Select Top 15 ProofReportId, AptCode, AptName, UserCode, UserName, Scn, Mobile, ProofName, StartDate, EndDate, PostDuty, UserPlace, UseDate, CompanyCode, CompanyName, Adress, Telephone, CeoName, ResultUserCode, Etc, Approval, PostDate, PostIp, Resignation From ProofReport Where ProofReportId Not In (Select Top (15 * @Page) ProofReportId From ProofReport Where UserName = @Name Order By ProofReportId Desc) And UserName = @Name Order By ProofReportId Desc", new { Page, Name });
                return lst.ToList();
            }
        }

        public async Task<int> GetProofs_Apt_Name_Count(string AptCode, string Name)
        {
            using (var db = new SqlConnection(_db.GetConnectionString("sw_Lib")))
            {
                return await db.QuerySingleOrDefaultAsync<int>("Select Count(*) From ProofReport Where AptCode = @AptCode And UserName = @Name", new { AptCode, Name });
            }
        }

        /// <summary>
        /// 제증명서 발급처별 목록
        /// </summary>
        /// <param name="CompanyCode"></param>
        /// <returns></returns>
        public async Task<List<ProofRepot_Entity>> GetProofs_User(string UserCode)
        {
            using (var db = new SqlConnection(_db.GetConnectionString("sw_Lib")))
            {
                var lst = await db.QueryAsync<ProofRepot_Entity>("Select ProofReportId, AptCode, AptName, UserCode, UserName, Scn, Mobile, ProofName, StartDate, EndDate, PostDuty, UserPlace, UseDate, CompanyCode, CompanyName, Adress, Telephone, CeoName, ResultUserCode, Etc, Approval, PostDate, PostIp, Resignation From ProofReport Where UserCode = @UserCode Order By ProofReportId Desc", new { UserCode });
                return lst.ToList();
            }
            
        }

        /// <summary>
        /// 수정
        /// </summary>
        /// <param name="pr"></param>
        public async Task Edit(ProofRepot_Entity pr)
        {
            using (var db = new SqlConnection(_db.GetConnectionString("sw_Lib")))
            {
               await db.ExecuteAsync("Update ProofReport Set Adress = @Adress, CeoName = @CeoName, CompanyCode = @CompanyCode, CompanyName = @CompanyName, EndDate = @EnDate, PostDuty = @PostDuty, ProofReportId = @ProofReportId, StartDate = @StartDate, UserPlace = @UserPlace, Etc = @Etc. PostIp = @PostIp, , Resignation = @Resignation", new { pr.Adress, pr.CeoName, pr.CompanyCode, pr.CompanyName, pr.EndDate, pr.PostDuty, pr.ProofReportId, pr.StartDate, pr.UserPlace, pr.PostIp });
            }
            
        }

        /// <summary>
        /// 식별코드로 상세보기
        /// </summary>
        /// <param name="ProofReportId"></param>
        /// <returns></returns>
        public async Task<ProofRepot_Entity> Details(string ProofReportId)
        {
            using (var db = new SqlConnection(_db.GetConnectionString("sw_Lib")))
            {
                return await db.QuerySingleOrDefaultAsync<ProofRepot_Entity>("Select ProofReportId, AptCode, AptName, UserCode, UserName, Scn, Mobile, ProofName, StartDate, EndDate, PostDuty, UserPlace, UseDate, CompanyCode, CompanyName, Adress, Telephone, CeoName, ResultUserCode, Etc, Approval, PostDate, PostIp, Resignation From ProofReport Where ProofReportId = @ProofReportId", new { ProofReportId });
            }
            
        }

        /// <summary>
        /// 승인 여부
        /// </summary>
        /// <param name="ProofReportId"></param>
        public async Task Approval(string ProofReportId, string ResultUserCode)
        {
            using (var db = new SqlConnection(_db.GetConnectionString("sw_Lib")))
            {
                string strR = await db.QuerySingleOrDefaultAsync<string>("Select Approval From ProofReport Where ProofReportId = @ProofReportId", new { ProofReportId });

                if (strR == "B")
                {
                    await db.ExecuteAsync("Update ProofReport Set Approval = 'A' Where ProofReportId = @ProofReportId", new { ProofReportId, ResultUserCode });
                }
                else
                {
                    await db.ExecuteAsync("Update ProofReport Set Approval = 'B'  Where ProofReportId = @ProofReportId", new { ProofReportId, ResultUserCode });
                }
            }
            
        }

        /// <summary>
        /// 기타 사항 입력
        /// </summary>
        /// <param name="ProofReportId"></param>
        /// <param name="Etc"></param>
        public async Task Etc_Add(string ProofReportId, string Etc)
        {
            using (var db = new SqlConnection(_db.GetConnectionString("sw_Lib")))
            {
                await db.ExecuteAsync("Update ProofReport Set Etc = @Etc Where ProofReportId = @ProofReportId", new { ProofReportId, Etc });
            }
            
        }

        /// <summary>
        /// 삭제
        /// </summary>
        /// <param name="Aid"></param>
        public async Task Remove(string Aid)
        {
            using (var db = new SqlConnection(_db.GetConnectionString("sw_Lib")))
            {
                await db.ExecuteAsync("Delete ProofReport Where ProofReportId = @Aid", new { Aid });
            }
            
        }
    }
}
