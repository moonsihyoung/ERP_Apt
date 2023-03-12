using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using System.Linq;

namespace Erp_Apt_Lib.Draft
{
    public class Draft_Lib : IDraft_Lib
    {
        private readonly IConfiguration _db;
        public Draft_Lib(IConfiguration configuration)
        {
            this._db = configuration;
        }

        /// <summary>
        /// 기안 문서 입력
        /// </summary>
        /// <param name="df"></param>
        /// <returns></returns>
        public async Task<int> Add(DraftEntity df)
        {
            var sql = "Insert Into Draft (UserName, AptName, AptCode, UserCode, Post, BranchA, BranchB, BranchC, BranchD, DraftTitle, Content, PostIP, DraftNum, KeepYear, DraftYear, DraftMonth, DraftDay, DraftDate, ExecutionDate, Vat, VatAcount, TotalAcount, OutDraft, Organization) Values (@UserName, @AptName, @AptCode, @UserCode, @Post, @BranchA, @BranchB, @BranchC, @BranchD, @DraftTitle, @Content, @PostIP, @DraftNum, @KeepYear, @DraftYear, @DraftMonth, @DraftDay, @DraftDate, @ExecutionDate, @Vat, @VatAcount, @TotalAcount, @OutDraft, @Organization);Select Cast(SCOPE_IDENTITY() As Int);";
            using var dba = new SqlConnection(_db.GetConnectionString("sw_togather"));
            return await dba.QuerySingleOrDefaultAsync<int>(sql, df);
        }

        /// <summary>
        /// 기안 내용 수정
        /// </summary>
        /// <param name="df"></param>
        public async Task Edit(DraftEntity df)
        {
            var sql = "Update Draft Set DraftTitle = @DraftTitle, Content = @Content, DraftYear = @DraftYear, DraftMonth = @DraftMonth, DraftDay = @DraftDay, DraftDate = @DraftDate, ExecutionDate = @ExecutionDate Where Aid = @Aid";
            using var dba = new SqlConnection(_db.GetConnectionString("sw_togather"));
            await dba.ExecuteAsync(sql, df);
        }

        /// <summary>
        /// 기안문서 목록(공동주택별)
        /// </summary>
        /// <param name="AptCode"></param>
        /// <returns></returns>
        public async Task<List<DraftEntity>> GetList(int Page, string AptCode)
        {
            using var dba = new SqlConnection(_db.GetConnectionString("sw_togather"));
            var lst = await dba.QueryAsync<DraftEntity>("Select Top 15 Aid, UserName, AptName, AptCode, UserCode, Post, BranchA, BranchB, BranchC, BranchD, DraftTitle, [Content], PostDate, PostIP, Dicision, DraftNum, KeepYear, DraftYear, DraftMonth, DraftDay, DraftDate, ExecutionDate, Vat, VatAcount, TotalAcount, OutDraft, Organization, ModifyDate, MdifyIP, Approval, Files_Count From Draft Where Aid Not In(Select Top (15 * @Page) Aid From Draft Where AptCode = @AptCode Order By Aid Desc) And AptCode = @AptCode Order By Aid Desc", new { Page, AptCode });
            return lst.ToList();
        }
        

        /// <summary>
        /// 기안문서 목록 수(공동주택별)
        /// </summary>
        /// <param name="AptCode"></param>
        /// <returns></returns>
        public async Task<int> GetListCount(string AptCode)
        {
            using var dba = new SqlConnection(_db.GetConnectionString("sw_togather"));
            return await dba.QuerySingleOrDefaultAsync<int>("Select Count(*) From Draft Where AptCode = @AptCode", new { AptCode });
        }

        /// <summary>
        /// 기안문서 찾기
        /// </summary>
        /// <param name="Feild"></param>
        /// <param name="Query"></param>
        /// <returns></returns>
        public async Task<List<DraftEntity>> SearchList(int Page, string Feild, string Query, string AptCode)
        {
            using var dba = new SqlConnection(_db.GetConnectionString("sw_togather"));
            var lst = await dba.QueryAsync<DraftEntity>("Select Top 15 Aid, UserName, AptName, AptCode, UserCode, Post, BranchA, BranchB, BranchC, BranchD, DraftTitle, [Content], PostDate, PostIP, Dicision, DraftNum, KeepYear, DraftYear, DraftMonth, DraftDay, DraftDate, ExecutionDate, Vat, VatAcount, TotalAcount, OutDraft, Organization, ModifyDate, MdifyIP, Approval, Files_Count From Draft Where Aid Not In(Select Top (15 * @Page) Aid From Document Where AptCode = @AptCode And " + Feild + " Like '%" + Query + "%' Order By Aid Desc) And AptCode = @AptCode And " + Feild + " Like '%" + Query + "%' Order By Aid Desc", new { Page, Feild, Query, AptCode });
            return lst.ToList();
        }

        /// <summary>
        /// 기안문서 찾은 수
        /// </summary>
        /// <param name="Feild"></param>
        /// <param name="Query"></param>
        /// <returns></returns>
        public async Task<int> SearchListCount(string Feild, string Query, string AptCode)
        {
            using var dba = new SqlConnection(_db.GetConnectionString("sw_togather"));
            return await dba.QuerySingleOrDefaultAsync<int>("Select Count(*) From Draft Where AptCode = @AptCode And " + Feild + " Like '%" + Query + "%'", new { Feild, Query, AptCode });
        }

        /// <summary>
        /// 기안 문서 상세보기
        /// </summary>
        /// <param name="Aid"></param>
        /// <returns></returns>
        public async Task<DraftEntity> Details(int Aid)
        {
            using var dba = new SqlConnection(_db.GetConnectionString("sw_togather"));
            return await dba.QuerySingleOrDefaultAsync<DraftEntity>("Select Aid, UserName, AptName, AptCode, UserCode, Post, BranchA, BranchB, BranchC, BranchD, DraftTitle, [Content], PostDate, PostIP, Dicision, DraftNum, KeepYear, DraftYear, DraftMonth, DraftDay, DraftDate, ExecutionDate, Vat, VatAcount, TotalAcount, OutDraft, Organization, ModifyDate, MdifyIP, Approval, Files_Count From Draft Where Aid = @Aid", new { Aid });

        }

        /// <summary>
        /// 기안 문서 삭제
        /// </summary>
        /// <param name="Aid"></param>
        public async Task Remove(int Aid)
        {
            using var dba = new SqlConnection(_db.GetConnectionString("sw_togather"));
            await dba.ExecuteAsync("Delete Draft Where Aid = @Aid", new { Aid });

        }

        /// <summary>
        /// 결재 완료
        /// </summary>
        /// <param name="Aid"></param>
        public async Task Decision(int Aid)
        {
            using var dba = new SqlConnection(_db.GetConnectionString("sw_togather"));
            await dba.ExecuteAsync("Update Draft Set Dicision = 'B' Where Aid = @Aid", new { Aid });

        }

        /// <summary>
        /// 마지막 번호
        /// </summary>
        /// <param name="AptCode"></param>
        /// <returns></returns>
        public async Task<int> LastAid(string AptCode, int Year)
        {
            using var dba = new SqlConnection(_db.GetConnectionString("sw_togather"));
            return await dba.QuerySingleOrDefaultAsync<int>("Select Count(*) From Draft Where AptCode = @AptCode And DraftYear = @Year", new { AptCode, Year });

        }

        /// <summary>
        /// 결재완료(승인 or 미승인)
        /// </summary>
        /// <param name="Aid"></param>
        /// <param name="View"></param>
        public async Task Draft_Comform(int Aid, string View)
        {
            using var dba = new SqlConnection(_db.GetConnectionString("sw_togather"));
            await dba.ExecuteAsync("Update Draft Set Approval = @View Where Aid = @Aid", new { Aid, View });

        }

        /// <summary>
        /// 결재완료 여부
        /// </summary>
        /// <param name="Aid"></param>
        public async Task<string> Comform(int Aid)
        {
            using var dba = new SqlConnection(_db.GetConnectionString("sw_togather"));
            return await dba.QuerySingleOrDefaultAsync<string>("Select Approval From Draft Where Aid = @Aid", new { Aid });

        }

        /// <summary>
        /// 앞 업무 정보
        /// </summary>
        public async Task<string> Ago(string AptCode, string Aid)
        {
            using var dba = new SqlConnection(_db.GetConnectionString("sw_togather"));
            return await dba.QuerySingleOrDefaultAsync<string>("Select ISNULL(Aid, 0) From Draft Where AptCode = @AptCode and Aid = (Select max(Aid) From Draft Where AptCode = @AptCode and Aid < @Aid)", new { AptCode, Aid });

        }

        /// <summary>
        /// 앞 업무  존재여부
        /// </summary>
        /// <param name="AptCode"></param>
        /// <param name="Num"></param>
        /// <returns></returns>
        public async Task<int> AgoBe(string AptCode, string Aid)
        {
            using var dba = new SqlConnection(_db.GetConnectionString("sw_togather"));
            return await dba.QuerySingleOrDefaultAsync<int>("Select Count(*) From Draft Where AptCode = @AptCode and Aid = (Select max(Aid) From Draft Where AptCode = @AptCode and Aid < @Aid)", new { AptCode, Aid });

        }

        /// <summary>
        /// 뒤 업무
        /// </summary>
        /// <param name="AptCode"></param>
        /// <param name="Num"></param>
        /// <returns></returns>
        public async Task<string> Next(string AptCode, string Aid)
        {
            using var dba = new SqlConnection(_db.GetConnectionString("sw_togather"));
            return await dba.QuerySingleOrDefaultAsync<string>("Select ISNULL(Aid, 0) From Draft Where AptCode = @AptCode and Aid = (Select Min(Aid) From Draft Where AptCode =@AptCode and Aid > @Aid)", new { AptCode, Aid });

        }

        /// <summary>
        // 뒤 업무 존재 여부
        /// </summary>
        /// <param name="AptCode"></param>
        /// <param name="Num"></param>
        /// <returns></returns>
        public async Task<int> NextBe(string AptCode, string Aid)
        {
            using var dba = new SqlConnection(_db.GetConnectionString("sw_togather"));
            return await dba.QuerySingleOrDefaultAsync<int>("Select Count(*) From Draft Where AptCode = @AptCode and Aid = (Select Min(Aid) From Draft Where AptCode =@AptCode and Aid > @Aid)", new { AptCode, Aid });

        }

        /// <summary>
        /// 첨부파일 추가 또는 삭제
        /// </summary>
        /// <param name="Aid"></param>
        /// <param name="Division"></param>
        /// <returns></returns>
        public async Task FilesCount(int Aid, string Division)
        {
            using var dba = new SqlConnection(_db.GetConnectionString("sw_togather"));
            if (Division == "A")
            {
                await dba.ExecuteAsync("Update Draft Set Files_Count = Files_Count + 1 Where Aid = @Aid", new { Aid });
            }
            else if (Division == "B")
            {
                await dba.ExecuteAsync("Update Draft Set Files_Count = Files_Count - 1 Where Aid = @Aid", new { Aid });
            }
        }
    }

    public class DraftDetail_Lib : IDraftDetail_Lib
    {
        private readonly IConfiguration _db;
        public DraftDetail_Lib(IConfiguration configuration)
        {
            this._db = configuration;
        }

        /// <summary>
        /// 기안상세 입력
        /// </summary>
        /// <param name="dd"></param>
        /// <returns></returns>
        public async Task Add(DraftDetailEntity dd)
        {
            var sql = "Insert DraftDetail (AptCode, UserCode, Article, Goods, Quantity, UnitCost, SupplyPrice, Vat, VatAcount, TotalAcount, ParentAid, PostIP) Values (@AptCode, @UserCode, @Article, @Goods, @Quantity, @UnitCost, @SupplyPrice, @Vat, @VatAcount, @TotalAcount, @ParentAid, @PostIP)";
            using var dba = new SqlConnection(_db.GetConnectionString("sw_togather"));
            await dba.ExecuteAsync(sql, dd);

        }

        /// <summary>
        /// 기안상세 수정
        /// </summary>
        /// <param name="dd"></param>
        public async Task Edit(DraftDetailEntity dd)
        {
            using var dba = new SqlConnection(_db.GetConnectionString("sw_togather"));
            await dba.ExecuteAsync("Update DraftDetail Set Article = @Article, Goods = @Goods, Quantity = @Quantity, UnitCost = @UnitCost, SupplyPrice = @SupplyPrice, Vat = @Vat, VatAcount = @VatAcount, TotalAcount = @TotalAcount, PostIP = @PostIP Where Aid = @Aid", dd);
        }

        /// <summary>
        /// 기안상세 목록
        /// </summary>
        /// <param name="ParentAid"></param>
        /// <returns></returns>
        public async Task<List<DraftDetailEntity>> GetList(int ParentAid)
        {
            using var dba = new SqlConnection(_db.GetConnectionString("sw_togather"));
            var lst = await dba.QueryAsync<DraftDetailEntity>("Select Aid, AptCode, UserCode, Article, Goods, Quantity, UnitCost, SupplyPrice, Vat, VatAcount, TotalAcount, ParentAid, PostDate, PostIP From DraftDetail Where ParentAid = @ParentAid", new { ParentAid });
            return lst.ToList();

        }

        /// <summary>
        /// 기안상세 목록 수
        /// </summary>
        /// <param name="ParentAid"></param>
        /// <returns></returns>
        public async Task<int> GetListCount(int ParentAid)
        {
            using var dba = new SqlConnection(_db.GetConnectionString("sw_togather"));
            return await dba.QuerySingleOrDefaultAsync<int>("Select Count(*) From DraftDetail Where ParentAid = @ParentAid", new { ParentAid });

        }

        /// <summary>
        /// 기안상세 삭제
        /// </summary>
        /// <param name="Aid"></param>
        public async Task Remove(int Aid)
        {
            using var dba = new SqlConnection(_db.GetConnectionString("sw_togather"));
            await dba.ExecuteAsync("Delete DraftDetail Where Aid = @Aid", new { Aid });

        }

        /// <summary>
        /// 계산 합계 구하기
        /// </summary>
        /// <param name="ParentAid"></param>
        /// <returns></returns>
        public async Task<double> ToralCount(int ParentAid)
        {
            using var dba = new SqlConnection(_db.GetConnectionString("sw_togather"));
            return await dba.QuerySingleOrDefaultAsync<double>("Select ISNULL(Sum(TotalAcount), 0) From DraftDetail Where ParentAid = @ParentAid", new { ParentAid });

        }

        /// <summary>
        /// 계산 합계 구하기
        /// </summary>
        /// <param name="ParentAid"></param>
        /// <returns></returns>
        public double ToralSum(int ParentAid)
        {
            using var dba = new SqlConnection(_db.GetConnectionString("sw_togather"));
            return dba.QuerySingleOrDefault<double>("Select ISNULL(Sum(TotalAcount), 0) From DraftDetail Where ParentAid = @ParentAid", new { ParentAid });

        }

        /// <summary>
        /// 부가세 계산 합계 구하기
        /// </summary>
        /// <param name="ParentAid"></param>
        /// <returns></returns>
        public async Task<double> VatToralCount(int ParentAid)
        {
            using var dba = new SqlConnection(_db.GetConnectionString("sw_togather"));
            return await dba.QuerySingleOrDefaultAsync<double>("Select ISNULL(Sum(VatAcount), 0) From DraftDetail Where ParentAid = @ParentAid", new { ParentAid });

        }
    }

    public class DraftAttach_Lib : IDraftAttach_Lib
    {
        private readonly IConfiguration _db;
        public DraftAttach_Lib(IConfiguration configuration)
        {
            this._db = configuration;
        }

        /// <summary>
        /// 첨부서류 입력
        /// </summary>
        /// <param name="da"></param>
        /// <returns></returns>
        public async Task<int> Add(DraftAttachEntity da)
        {
            var sql = "Insert DraftAttach (AptCode, UserCode, Attachment, DNo, ParentAid, PostIP) Values (@AptCode, @UserCode, @Attachment, @DNo, @ParentAid, @PostIP)";
            using var dba = new SqlConnection(_db.GetConnectionString("sw_togather"));
            int Aid = await dba.QuerySingleOrDefaultAsync<int>(sql, da);
            return Aid;

        }

        /// <summary>
        /// 첨부서류 수정
        /// </summary>
        /// <param name="da"></param>
        public async Task Edit(DraftAttachEntity da)
        {
            using var dba = new SqlConnection(_db.GetConnectionString("sw_togather"));
            await dba.ExecuteAsync("Update DraftAttach Set Attachment = @Attachment, DNo = @DNo, PostDate = @PostDate, Post = @PostIP Where Aid = @Aid", da);

        }

        /// <summary>
        /// 첨부서류 목록
        /// </summary>
        /// <param name="ParentAid"></param>
        /// <returns></returns>
        public async Task<List<DraftAttachEntity>> GetList(int ParentAid)
        {
            using var dba = new SqlConnection(_db.GetConnectionString("sw_togather"));
            var lst = await dba.QueryAsync<DraftAttachEntity>("Select Aid, AptCode, UserCode, Attachment, DNo, ParentAid, PostDate, PostIP From DraftAttach Where ParentAid = @ParentAid", new { ParentAid });
            return lst.ToList();

        }

        /// <summary>
        /// 첨부서류 목록 수
        /// </summary>
        /// <param name="ParentAid"></param>
        /// <returns></returns>
        public async Task<int> GetListCount(int ParentAid)
        {
            using var dba = new SqlConnection(_db.GetConnectionString("sw_togather"));
            return await dba.QuerySingleOrDefaultAsync<int>("Select Count(*) From DraftAttach Where ParentAid = @ParentAid", new { ParentAid });

        }

        /// <summary>
        /// 삭제
        /// </summary>
        /// <param name="Aid"></param>
        public async Task Delete(int Aid)
        {
            using var dba = new SqlConnection(_db.GetConnectionString("sw_togather"));
            await dba.ExecuteAsync("Delete DraftAttach Where Aid = @Aid", new { Aid });

        }
    }
}
