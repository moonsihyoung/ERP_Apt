using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Erp_Apt_Lib.Up_Files
{

    public class UpFile_Entity
    {
        /// <summary>
        /// 일련번호
        /// </summary>
        public int Aid { get; set; }
        /// <summary>
        /// 공동주택코드
        /// </summary>
        public string Apt_Code { get; set; }
        /// <summary>
        /// 구분
        /// </summary>
        public string Sort { get; set; }
        /// <summary>
        /// 분류
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 파일명
        /// </summary>
        public string FileName { get; set; }
        /// <summary>
        /// 파일사이즈
        /// </summary>
        public int FileSize { get; set; }
        /// <summary>
        /// 부모번호
        /// </summary>
        public string Cnn_Code { get; set; }
        /// <summary>
        /// 부모명
        /// </summary>
        public string Cnn_Name { get; set; }
        /// <summary>
        /// 파일설명
        /// </summary>
        public string File_Etc { get; set; }
        /// <summary>
        /// 다운 카운트
        /// </summary>
        public int Down_Count { get; set; }
        /// <summary>
        /// 입력일
        /// </summary>
        public DateTime PostDate { get; set; }
        /// <summary>
        /// 입력자 아이피
        /// </summary>
        public string PostIP { get; set; }
    }

    public interface IUpFile_Lib
    {
        Task<UpFile_Entity> Add_UpFile(UpFile_Entity UpFile);
        Task<UpFile_Entity> Edit_UpFile(UpFile_Entity UpFile);
        Task<List<UpFile_Entity>> GetList_UpFile(string Cnn_Code, string Sort, string Code);
        Task<List<UpFile_Entity>> GetList_CoeList(string Cnn_Code, string Sort, string Code);
        Task<int> GetList_CoeListCount(string Cnn_Code, string Sort, string Code);
        Task<List<UpFile_Entity>> GetList(string Apt_Code, string Sort, string Code);
        Task<int> UpFile_Count(string Cnn_Name, string Cnn_Code, string Apt_Code);
        Task<string> FileName(string Aid);
        Task down_count(string Aid);
        Task down_count_File(string FileName);
        Task Remove_UpFile_Code(string Cnn_Code);
        Task Remove_UpFile(string Aid);
        Task<List<UpFile_Entity>> UpFile_List(string Cnn_Name, string Cnn_Code, string Apt_Code);

        /// <summary>
        /// 첨부파일 상세보기
        /// </summary>
        Task<UpFile_Entity> GetDetails(int Aid);

        /// <summary>
        /// 파일 갯수(보고서 별)
        /// </summary>
        int FilesCount(string Cnn_Name, string Cnn_Code, string Apt_Code);
    }

    public class UpFile_Lib : IUpFile_Lib
    {
        private readonly IConfiguration _db;
        public UpFile_Lib(IConfiguration configuration)
        {
            this._db = configuration;
        }

        public async Task<UpFile_Entity> Add_UpFile(UpFile_Entity UpFile)
        {
            var sql = "Insert UpFile (Apt_Code, Sort, Code, FileName, FileSize, Cnn_Code, Cnn_Name, File_Etc, PostIP) Values (@Apt_Code, @Sort, @Code, @FileName, @FileSize, @Cnn_Code, @Cnn_Name, @File_Etc, @PostIP)";
            using (var db = new SqlConnection(_db.GetConnectionString("sw_Lib")))
            {
                await db.ExecuteAsync(sql, UpFile);
                return UpFile;
            }
            
        }

        // 첨부 파일 수정 메서드
        public async Task<UpFile_Entity> Edit_UpFile(UpFile_Entity UpFile)
        {
            var sql = "Update UpFile Set FileName = @FileName, FileSize = @FileSize, PostIP = @PostIP Where Aid = @Aid;";
            using (var db = new SqlConnection(_db.GetConnectionString("sw_Lib")))
            {
                await db.ExecuteAsync(sql, UpFile);
                return UpFile;
            }
            
        }

        /// <summary>
        /// 해당 게시판 글에 첨부파일 관련 코드로 리스트 메서드
        /// </summary>
        public async Task<List<UpFile_Entity>> GetList_UpFile(string Cnn_Code, string Sort, string Code)
        {
            using (var db = new SqlConnection(_db.GetConnectionString("sw_Lib")))
            {
                var lst = await db.QueryAsync<UpFile_Entity>("Select * From UpFile Where Cnn_Code = @Cnn_Code And Sort = @Sort And Code = @Code Order By Aid Desc", new { Cnn_Code, Sort, Code });
                return lst.ToList();
            }
            
        }        

        /// <summary>
        /// 근로계약서 첨부 파일 
        /// </summary>
        /// <param name="Cnn_Code"></param>
        /// <param name="Sort"></param>
        /// <param name="Code"></param>
        /// <returns></returns>
        public async Task<List<UpFile_Entity>> GetList_CoeList(string Cnn_Code, string Sort, string Code)
        {
            using (var db = new SqlConnection(_db.GetConnectionString("sw_Lib")))
            {
                var lst = await db.QueryAsync<UpFile_Entity>("Select * From UpFile Where Cnn_Code = @Cnn_Code And Sort = @Sort And Code = @Code Order By Aid Desc", new { Cnn_Code, Sort, Code });
                return lst.ToList();
            }
            
        }

        /// <summary>
        /// 파일 존재 여부 확인
        /// </summary>
        /// <param name="Cnn_Code"></param>
        /// <param name="Sort"></param>
        /// <param name="Code"></param>
        /// <returns></returns>
        public async Task<int> GetList_CoeListCount(string Cnn_Code, string Sort, string Code)
        {
            using (var db = new SqlConnection(_db.GetConnectionString("sw_Lib")))
            {
                return await db.QuerySingleOrDefaultAsync<int>("Select Count(*) From UpFile Where Cnn_Code = @Cnn_Code And Sort = @Sort And Code = @Code", new { Cnn_Code, Sort, Code });
            }
        }

        /// <summary>
        /// 해당공동주택에 업로드된 그림 파일 리스트 목록
        /// </summary>
        /// <param name="Apt_Code"></param>
        /// <returns></returns>
        public async Task<List<UpFile_Entity>> GetList(string Apt_Code, string Sort, string Code)
        {
            using (var db = new SqlConnection(_db.GetConnectionString("sw_Lib")))
            {
                var lst = await db.QueryAsync<UpFile_Entity>("Select top 5 a.Aid, a.Apt_Code, a.FileName, a.FileSize, a.Sort, a.Code, a.Cnn_Name, a.Cnn_Code, a.File_Etc, a.Down_Count, a.PostDate, a.PostIP From UpFile as a Join Surisan as b on a.Cnn_Code = b.Num and a.Code = b.Code Where a.Apt_Code = @Apt_Code And a.Sort = @Sort And a.Code = @Code Order By a.Aid Desc", new { Apt_Code, Sort, Code });
                return lst.ToList();
            }
            
        }

        /// <summary>
        /// 해당 식별코드로 입력된 첨부파일 수
        /// </summary>
        /// <param name="Cnn_Code"></param>
        /// <returns></returns>
        
        public async Task<int> UpFile_Count(string Cnn_Name, string Cnn_Code, string Apt_Code)
        {
            using (var db = new SqlConnection(_db.GetConnectionString("sw_Lib")))
            {
                return await db.QuerySingleOrDefaultAsync<int>("Select Count(*) From UpFile Where Cnn_Name = @Cnn_Name And Cnn_Code = @Cnn_Code And Apt_Code = @Apt_Code", new { Cnn_Name, Cnn_Code, Apt_Code});
            }           
        }

        /// <summary>
        /// 해당공동주택에 업로드된 그림 파일 리스트 목록
        /// </summary>
        /// <param name="Apt_Code"></param>
        /// <returns></returns>
        public async Task<List<UpFile_Entity>> UpFile_List(string Cnn_Name, string Cnn_Code, string Apt_Code)
        {
            using (var db = new SqlConnection(_db.GetConnectionString("sw_Lib")))
            {
                var lst = await db.QueryAsync<UpFile_Entity>("Select * From UpFile Where Cnn_Name = @Cnn_Name And Cnn_Code = @Cnn_Code And Apt_Code = @Apt_Code Order By Aid Desc", new { Cnn_Name, Cnn_Code, Apt_Code });
                return lst.ToList();
            }

        }

        /// <summary>
        /// 파일명 불러오기
        /// </summary>
        /// <param name="Aid"></param>
        /// <returns></returns>
        public async Task<string> FileName(string Aid)
        {
            using (var db = new SqlConnection(_db.GetConnectionString("sw_Lib")))
            {
                return await db.QuerySingleOrDefaultAsync<string>("Select FileName From UpFile Where Aid = @Aid", new { Aid });
            }
            
        }

        /// <summary>
        /// 다운 수 추가 메서드
        /// </summary>
        /// <param name="Aid">첨부파일 식별코드</param>
        public async Task down_count(string Aid)
        {
            using (var db = new SqlConnection(_db.GetConnectionString("sw_Lib")))
            {
                await db.ExecuteAsync("Update UpFile Set Down_Count = Down_Count + 1 Where Aid = @Aid", new { Aid });
            }
           
        }

        /// <summary>
        /// 다운 수 추가 메서드
        /// </summary>
        /// <param name="Aid">첨부파일 식별코드</param>
        public async Task down_count_File(string FileName)
        {
            using (var db = new SqlConnection(_db.GetConnectionString("sw_Lib")))
            {
                await db.ExecuteAsync("Update UpFile Set Down_Count = Down_Count + 1 Where FileName = @FileName", new { FileName });
            }
           
        }

        /// <summary>
        /// 첨부파일 모두 삭제 메서드
        /// </summary>
        public async Task Remove_UpFile_Code(string Cnn_Code)
        {
            using (var db = new SqlConnection(_db.GetConnectionString("sw_Lib")))
            {
                await db.ExecuteAsync("Delete UpFile Where Cnn_Code = @Cnn_Code", new { Cnn_Code });
            }
            
        }

        /// <summary>
        /// 첨부파일 삭제 메서드
        /// </summary>
        public async Task Remove_UpFile(string Aid)
        {
            using var db = new SqlConnection(_db.GetConnectionString("sw_Lib"));
            await db.ExecuteAsync("Delete UpFile Where Aid = @Aid", new { Aid });
        }

        /// <summary>
        /// 첨부파일 상세보기
        /// </summary>
        /// <param name="Aid"></param>
        /// <returns></returns>
        public async Task<UpFile_Entity> GetDetails(int Aid)
        {
            using var db = new SqlConnection(_db.GetConnectionString("sw_Lib"));
            return await db.QuerySingleOrDefaultAsync<UpFile_Entity>("Select * From UpFile Where Aid = @Aid", new { Aid });
        }

        public int FilesCount(string Cnn_Name, string Cnn_Code, string Apt_Code)
        {
            using var db = new SqlConnection(_db.GetConnectionString("sw_Lib"));
            return db.QuerySingleOrDefault<int>("Select Count(*) From UpFile Where Apt_Code = @Apt_Code And Cnn_Name = @Cnn_Name And Cnn_Code = @Cnn_Code", new { Cnn_Name, Cnn_Code, Apt_Code });
        }
    }
}
