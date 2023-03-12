using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Logs_Lib_A
{
    /// <summary>
    /// Logs 테이블과 일대일로 매핑되는 모델(뷰 모델, 엔터티) 클래스
    /// </summary>
    public class LogViewModel
    {
        /// <summary>
        /// 일련번호
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 비고
        /// </summary>
        public string Note { get; set; }

        /// <summary>
        /// 응용 프로그램: 게시판 관리, 상품 관리
        /// </summary>
        public string Application { get; set; }

        /// <summary>
        /// 사용자 정보(로그인 사용 아이디)
        /// </summary>
        public string Logger { get; set; }

        /// <summary>
        /// 로그 이벤트(사용자 정의 이벤트 ID)
        /// </summary>
        public string LogEvent { get; set; }

        /// <summary>
        /// 로그 메시지 
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 로그 메시지에 대한 템플릿
        /// </summary>
        public string MessageTemplate { get; set; }

        /// <summary>
        /// 로그 레벨(정보, 에러, 경고)
        /// </summary>
        public string Level { get; set; }

        /// <summary>
        /// 로그 발생 시간(LogCreationDate)
        /// </summary>
        public DateTime TimeStamp { get; set; }

        /// <summary>
        /// 예외 메시지 
        /// </summary>
        public string Exception { get; set; }

        /// <summary>
        /// 기타 여러 속성들을 XML 저장
        /// </summary>
        public string Properties { get; set; }

        /// <summary>
        /// 호출사이트
        /// </summary>
        public string Callsite { get; set; }

        /// <summary>
        /// IP 주소
        /// </summary>
        public string IpAddress { get; set; }
    }

    public interface ILogViewer
    {
        Task<LogViewModel> Add(LogViewModel model);
        Task<List<LogViewModel>> GetAllWithPaging(int pageIndex, int pageSize);
        Task<LogViewModel> GetById(int id);
        Task<int> GetCount();
        Task DeleteByIds(params int[] ids);
        Task<List<LogViewModel>> SearchLogsBySearchQuery(string startDate, string endDate);

    }

    public class LogViewer
    {
        private readonly IConfiguration _db;
        public LogViewer(IConfiguration configuration)
        {
            this._db = configuration;
        }

        /// <summary>
        /// 로그 저장하기
        /// </summary>
        public async Task<LogViewModel> Add(LogViewModel model)
        {
            var sql = @"
                Insert Into Logs (Note, Application, Logger, LogEvent, Message, MessageTemplate, Level, TimeStamp, Exception, Properties, Callsite, IpAddress) Values (@Note, @Application, @Logger, @LogEvent, @Message, @MessageTemplate, @Level, @TimeStamp, @Exception, @Properties, @Callsite, @IpAddress); Select Cast(SCOPE_IDENTITY() As Int);";

            using (var dba = new SqlConnection(_db.GetConnectionString("apt_Erp")))
            {
                var id = await dba.QuerySingleOrDefaultAsync<int>(sql, model);
                model.Id = id;
                return model;
            }
        }

        /// <summary>
        /// 페이징 처리된 로그 리스트
        /// </summary>
        public async Task<List<LogViewModel>> GetAllWithPaging(int pageIndex, int pageSize)
        {
            // 인라인 SQL(Ad Hoc 쿼리) 또는 저장 프로시저 지정
            string sql = "GetLogsWithPaging"; // 페이징 저장 프로시저

            // 파라미터 추가
            var parameters = new DynamicParameters();
            parameters.Add("@PageIndex",
                value: pageIndex,
                dbType: DbType.Int32,
                direction: ParameterDirection.Input);
            parameters.Add("@PageSize",
                value: pageSize,
                dbType: DbType.Int32,
                direction: ParameterDirection.Input);

            // 실행
            using (var dba = new SqlConnection(_db.GetConnectionString("apt_Erp")))
            {
                var lst = await dba.QueryAsync<LogViewModel>(sql, parameters,
                 commandType: CommandType.StoredProcedure);
                return lst.ToList();
            }

        }

        /// <summary>
        /// 특정 번호(Id)에 해당하는 로그
        /// </summary>
        public async Task<LogViewModel> GetById(int id)
        {
            string sql = "Select * From Logs Where Id = @Id";
            using (var dba = new SqlConnection(_db.GetConnectionString("apt_Erp")))
            {
                return await dba.QuerySingleOrDefaultAsync<LogViewModel>(sql, new { id });
            }

        }

        /// <summary>
        /// 로그 테이블의 전체 레코드 수
        /// </summary>
        public async Task<int> GetCount()
        {
            var sql = "Select Count(*) From Logs";
            using (var dba = new SqlConnection(_db.GetConnectionString("apt_Erp")))
            {
                return await dba.QuerySingleOrDefaultAsync<int>(sql);
            }

        }

        /// <summary>
        /// 선택 삭제
        /// </summary>
        public async Task DeleteByIds(params int[] ids)
        {
            string sql = "Delete Logs Where Id In @Ids";
            using (var dba = new SqlConnection(_db.GetConnectionString("apt_Erp")))
            {
                await dba.ExecuteAsync(sql, new { Ids = ids });
            }

        }

        /// <summary>
        /// 시작 날짜와 종료 날짜 사이의 데이터 검색
        /// </summary>
        public async Task<List<LogViewModel>> SearchLogsBySearchQuery(
            string startDate, string endDate)
        {
            string sql = @"
                Select * From Logs 
                Where 
                    TimeStamp 
                        Between @StartDate And @EndDate";
            using (var dba = new SqlConnection(_db.GetConnectionString("apt_Erp")))
            {
                var lst = await dba.QueryAsync<LogViewModel>(sql, new
                {
                    StartDate = startDate,
                    EndDate = endDate
                });
                return lst.ToList();
            }
        }
    }
}

