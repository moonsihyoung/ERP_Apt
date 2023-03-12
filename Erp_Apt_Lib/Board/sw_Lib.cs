using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Sw_Entity;
using System.Linq;
using System.Data;
using System.Drawing;

namespace Sw_Lib
{
    /// <summary>
    /// 게시판 관련 클레스
    /// </summary>
    public class sw_Note_Lib : Isw_Note_Lib
    {
        private readonly IConfiguration _db;
        public sw_Note_Lib(IConfiguration configuration)
        {
            this._db = configuration;
        }

        /// <summary>
        /// 게시판 입력
        /// </summary>
        public async Task<int> Add(Note_Entity _note)
        {
            var sql = "Insert into Surisan (UserID, UserName, Email, P_Adress, Title, PostIP, Content, Encoding, Homepage, Notice, Category, Sort, Noad, Apt_Code, Code, Board_Code, Viw, Pb) Values (@UserID, @UserName, @Email, @P_Adress, @Title, @PostIP, @Content, @Encoding, @Homepage, @Notice, @Category, @Sort, @Noad, @Apt_Code, @Code, @Board_Code, @Viw, @Pb); Select Cast(SCOPE_IDENTITY() As Int);";
            using var db = new SqlConnection(_db.GetConnectionString("sw_togather"));
            var Num = await db.QuerySingleOrDefaultAsync<int>(sql, _note);
            _note.Num = Num;
            return Num;
        }

        /// <summary>
        /// 게시판 수정
        /// </summary>
        public async Task<Note_Entity> Edit(Note_Entity _note)
        {
            var sql = "Update Surisan Set Email = @Email, P_Adress = P_Adress, Title = @Title, Content = @Content, Encoding = @Encoding, HomePage = Homepage, Notice = @Notice, Sort = @Sort, Code = @Code, Viw = @Viw, Pb = @Pb Where Num = @Num";
            using var db = new SqlConnection(_db.GetConnectionString("sw_Lib"));
            await db.ExecuteAsync(sql, _note);
            return _note;
        }

        /// <summary>
        /// 임시 게시물 삭제
        /// </summary>
        public async Task Remove_Temp()
        {
            using var db = new SqlConnection(_db.GetConnectionString("sw_Lib"));
            await db.ExecuteAsync("Delete Surisan Where Title = '임시저장'");
        }

        /// <summary>
        /// 메인화면 최신 게시판 정보 보여주기
        /// </summary>
        public async Task<List<Note_Entity>> MainListA(string _sort, string _code)
        {
            using var db = new SqlConnection(_db.GetConnectionString("sw_Lib"));
            var lst = await db.QueryAsync<Note_Entity>("Select Top 7 * From Surisan Where Sort = @_sort And Code = @_code Order By PostDate Desc", new { _sort, _code });
             return lst.ToList();
        }

        /// <summary>
        /// 게시판 리스트
        /// </summary>
        public async Task<List<Note_Entity>> GetList(int Page, string Sort, string Code, string Apt_Code)
        {
            using var db = new SqlConnection(_db.GetConnectionString("sw_Lib"));
            var lst = await db.QueryAsync<Note_Entity>("ListSurisan", new { Page, Sort, Code, Apt_Code }, commandType: CommandType.StoredProcedure);
            return lst.ToList();
        }

        /// <summary>
        /// 입력된 수 
        /// </summary>
        public async Task<int> GetList_Count(string Sort, string Code, string Apt_Code)
        {
            using var db = new SqlConnection(_db.GetConnectionString("sw_Lib"));
            return await db.QuerySingleOrDefaultAsync<int>("GetCountSurisan", new { Sort, Code, Apt_Code }, commandType: CommandType.StoredProcedure);
        }

        /// <summary>
        /// 게시판 모바일 리스트
        /// </summary>
        public async Task<List<Note_Entity>> GetList_M(int Page, string Sort, string Code, string Apt_Code)
        {
            using var db = new SqlConnection(_db.GetConnectionString("sw_Lib"));
            var lst = await db.QueryAsync<Note_Entity>("ListSurisan_M", new { Page, Sort, Code, Apt_Code }, commandType: CommandType.StoredProcedure);
                return lst.ToList();
        }

        /// <summary>
        /// 분류별 찾기
        /// </summary>
        public async Task<List<Note_Entity>> Search_List(int Page, string SearchField, string SearchQuery, string _sort, string _code)
        {
            using var db = new SqlConnection(_db.GetConnectionString("sw_Lib"));
            var lst = await db.QueryAsync<Note_Entity>("SearchSurisan", new { SearchField, SearchQuery, Page, _sort, _code }, commandType: CommandType.StoredProcedure);
            return lst.ToList();
        }

        /// <summary>
        /// 검색된 수 
        /// </summary>
        public async Task<int> Search_List_Count(string SearchField, string SearchQuery, string _sort, string _code)
        {
            using var db = new SqlConnection(_db.GetConnectionString("sw_Lib"));
            return await db.QuerySingleOrDefaultAsync<int>("GetCountSurisanSearch", new { SearchField, SearchQuery, _sort, _code }, commandType: CommandType.StoredProcedure);
        }

        /// <summary>
        /// 게시판 상세보기
        /// </summary>
        public async Task<Note_Entity> Detail_Note(string Num, string _sort, string _code)
        {
            using var db = new SqlConnection(_db.GetConnectionString("sw_Lib"));
            return await db.QuerySingleOrDefaultAsync<Note_Entity>("Select * From Surisan Where Sort = @_sort And Code = @_code And Num = @Num", new { Num, _sort, _code });
        }

        /// <summary>
        /// 게시판 제목 불러오기
        /// </summary>
        public async Task<string> Detail_Title(string Num)
        {
            using var db = new SqlConnection(_db.GetConnectionString("sw_Lib"));
            return await db.QuerySingleOrDefaultAsync<string>("Select Title From Surisan Where Num = @Num", new { Num });
        }

        
        /// <summary>
        /// 분류별 찾기(전체)
        /// </summary>
        public async Task<List<Note_Entity>> All_Search_List(int Page, string Apt_Code, string SearchQuery)
        {
            using var db = new SqlConnection(_db.GetConnectionString("sw_Lib"));
            var lst = await db.QueryAsync<Note_Entity>("All_SearchSurisan", new { Page, Apt_Code, SearchQuery }, commandType: CommandType.StoredProcedure);
            return lst.ToList();
        }

        /// <summary>
        /// 메인 전체 검색 tn
        /// </summary>
        public async Task<int> All_Search_Count(string Apt_Code, string SearchQuery)
        {
            using var db = new SqlConnection(_db.GetConnectionString("sw_Lib"));
            return await db.QuerySingleOrDefaultAsync<int>("Select Count(*) From Surisan Where Title like '%" + SearchQuery + "%' And Apt_Code = @Apt_Code", new { SearchQuery, Apt_Code });
        }

           

        /// <summary>
        /// 게시글 삭제
        /// </summary>
        public async Task Remove(int Num)
        {
            using var db = new SqlConnection(_db.GetConnectionString("sw_Lib"));
            await db.QuerySingleOrDefaultAsync("Delete Surisan Where Num = @Num", new { Num });
        }

        /// <summary>
        /// 읽은 수 입력
        /// </summary>
        public async Task Update_ReadCount(string Num)
        {
            using var db = new SqlConnection(_db.GetConnectionString("sw_Lib"));
            await db.ExecuteAsync("Update Surisan Set ReadCount = ReadCount +1 Where Num = @Num", new { Num });
        }

        /// <summary>
        /// 댓글 수 입력
        /// </summary>
        public async Task Update_CommentCount(string Num)
        {
            using var db = new SqlConnection(_db.GetConnectionString("sw_Lib"));
            await db.ExecuteAsync("Update Surisan Set CommentCount = CommentCount +1 Where Num = @Num", new { Num });
        }

        /// <summary>
        /// 댓글 수 줄이기 입력
        /// </summary>
        /// <param name="Num"></param>
        public async Task Update_CommentCount_Remove(string Num)
        {
            using var db = new SqlConnection(_db.GetConnectionString("sw_Lib"));
            await db.ExecuteAsync("Update Surisan Set CommentCount = CommentCount - 1 Where Num = @Num", new { Num });
        }

        /// <summary>
        /// 마지막 입력된 공지사항의 일련번호
        /// </summary>
        public async Task<int> top_count()
        {
            using var db = new SqlConnection(_db.GetConnectionString("sw_Lib"));
            return await db.QuerySingleOrDefaultAsync<int>("Select Top 1 Num From Surisan Where Sort = 'Ifm' And Code = 'Notice' Order By PostDate Desc");
        }

    }

    /// <summary>
    /// 댓글관련 클래스
    /// </summary>
    public class Surisan_Comments : ISurisan_Comments
    {
        private readonly IConfiguration _db;
        public Surisan_Comments(IConfiguration configuration)
        {
            this._db = configuration;
        }

        /// <summary>
        /// 코멘트 입력
        /// </summary>
        public async Task<Note_Comment_Entity> Add(Note_Comment_Entity cont)
        {
            var sql = "Insert into SurisanComments (UserID, Name, P_Adress, PostIP, Opinion, Password, BoardNum, Sort, Noad, Code) Values (@UserID, @Name, @P_Adress, @PostIP, @Opinion, @Password, @BoardNum, @Sort, @Noad, @Code)";
            using var db = new SqlConnection(_db.GetConnectionString("sw_Lib"));
            await db.ExecuteAsync(sql, cont);
            return cont;
        }

        /// <summary>
        /// 코멘트 수정
        /// </summary>
        public async Task Edit(string Opinion, string Num)
        {
            using var db = new SqlConnection(_db.GetConnectionString("sw_Lib"));
            await db.ExecuteAsync("Update SurisanComments Set Opinion = @Opinion Where Num = @Num", new { Opinion, Num });            
        }

        /// <summary>
        /// 댓글 목록만들기
        /// </summary>
        public async Task<List<Note_Comment_Entity>> GetList(string BoardNum)
        {
            using var db = new SqlConnection(_db.GetConnectionString("sw_Lib"));
            var lst = await db.QueryAsync<Note_Comment_Entity>("procListComment_Surisan", new { BoardNum }, commandType: CommandType.StoredProcedure);
            return lst.ToList();
        }

        /// <summary>
        /// 댓글 목록만들기(Code)
        /// </summary>
        public async Task<List<Note_Comment_Entity>> GetList_Sort(string BoardNum, string Code)
        {
            using var db = new SqlConnection(_db.GetConnectionString("sw_Lib"));
            var lst = await db.QueryAsync<Note_Comment_Entity>("Select Num, UserID, Name, P_Adress, PostIP, Opinion, Password, BoardNum, Sort, Noad, Code, PostDate, ModifyDate, ModifyIP From SurisanComments Where BoardNum = @BoardNum And Code = @Code Order by Num Desc", new { BoardNum, Code });
            return lst.ToList();
        }

        /// <summary>
        /// 댓글 삭제
        /// </summary>
        /// <param name="Num"></param>
        public async Task Remove(int Num)
        {
            using var db = new SqlConnection(_db.GetConnectionString("sw_Lib"));
            await db.ExecuteAsync("Delete SurisanComments Where Num = @Num", new { Num });
        }

        /// <summary>
        /// 게시판에 입력된 댓글 합계
        /// </summary>
        public async Task<int> TotalCount(string BoardNum)
        {
            using var db = new SqlConnection(_db.GetConnectionString("sw_Lib"));
            return await db.QuerySingleOrDefaultAsync<int>("Select Count(*) From SurisanComments Where BoardNum = @BoardNum", new { BoardNum });
        }

        /// <summary>
        /// 수정을 위하여 해당글 있는지 확인
        /// </summary>
        public async Task<int> Being(string Num)
        {
            using var db = new SqlConnection(_db.GetConnectionString("sw_Lib"));
            return await db.QuerySingleOrDefaultAsync<int>("Select Count(*) From SurisanComments Where Num = @Num", new { Num });
        }
    }

    /// <summary>
    /// 게시판 분류 클래스
    /// </summary>
    public class sw_Note_Sort_Lib : Isw_Note_Sort_Lib
    {
        private readonly IConfiguration _db;
        public sw_Note_Sort_Lib(IConfiguration configuration)
        {
            this._db = configuration;
        }

        /// <summary>
        /// 게시판 분류 입력하기
        /// </summary>
        public async Task<Note_Sort_Entity> Add(Note_Sort_Entity _Sort_Entity)
        {
            using var db = new SqlConnection(_db.GetConnectionString("sw_Lib"));
            var sql = "Insert Home_Title (Board_Name, Board_Title, Board_Code, Board_Master_Name, Apt_Code, Board_Cafe_Name, Board_Cafe_Doc, PostIP, LevelCount, Write_Count, Down_Count, AptCode) Values (@Board_Name, @Board_Title, @Board_Code, @Board_Master_Name, @Apt_Code, @Board_Cafe_Name, @Board_Cafe_Doc, @PostIP, @LevelCount, @Write_Count, @Down_Count, @AptCode)";
            await db.ExecuteAsync(sql, _Sort_Entity);
            return _Sort_Entity;
        }

        /// <summary>
        /// 게시판 분류 수정하기
        /// </summary>
        public async Task<Note_Sort_Entity> edit(Note_Sort_Entity _Sort_Entity)
        {
            using var db = new SqlConnection(_db.GetConnectionString("sw_Lib"));
            var sql = "Update Home_Title Set Board_Name = @Board_Name Where Num = @Num";
            await db.ExecuteAsync(sql, _Sort_Entity);
            return _Sort_Entity;
        }

        /// <summary>
        /// 게시판명 불러오기
        /// </summary>
        public async Task<string> Sort_Name(string Board_Title, string Board_Cafe_Name)
        {
            using var db = new SqlConnection(_db.GetConnectionString("sw_Lib"));
            return await db.QuerySingleOrDefaultAsync<string>("Select Board_Doc From Home_Title Where Board_Title = @Board_Title And Board_Cafe_Name = @Board_Cafe_Name", new { Board_Title, Board_Cafe_Name });
        }

        public async Task<string> Sort_NameA(string Sort, string Code)
        {
            using var db = new SqlConnection(_db.GetConnectionString("sw_Lib"));
            return await db.QuerySingleOrDefaultAsync<string>("Select Board_Doc From Home_Title Where Sort = @Sort And Code = @Code", new { Sort, Code });
        }
    }

    /// <summary>
    /// 첨부파일 업로드 함수
    /// </summary>
    public class UpFiless_Lib : IUpFiless_Lib
    {
        private readonly IConfiguration _db;
        public UpFiless_Lib(IConfiguration configuration)
        {
            this._db = configuration;
        }

        /// <summary>
        /// 첨부파일 올리기 메서드
        /// </summary>
        public async Task<UpFile_Entity> Add_UpFile(UpFile_Entity UpFile)
        {
            using var db = new SqlConnection(_db.GetConnectionString("sw_Lib"));
            var sql = "Insert UpFile (Apt_Code, Sort, Code, FileName, FileSize, Cnn_Code, Cnn_Name, File_Etc, PostIP) Values (@Apt_Code, @Sort, @Code, @FileName, @FileSize, @Cnn_Code, @Cnn_Name, @File_Etc, @PostIP)";
            await db.ExecuteAsync(sql, UpFile);
            return UpFile;
        }

        /// <summary>
        /// 첨부 파일 수정 메서드
        /// </summary>
        public async Task<UpFile_Entity> Edit_UpFile(UpFile_Entity UpFile)
        {
            using var db = new SqlConnection(_db.GetConnectionString("sw_Lib"));
            var sql = "Update UpFile Set FileName = @FileName, FileSize = @FileSize, PostIP = @PostIP Where Aid = @Aid;";
            await db.ExecuteAsync(sql, UpFile);
            return UpFile;
        }

        /// <summary>
        /// 해당 게시판 글에 첨부파일 관련 코드로 리스트 메서드
        /// </summary>
        public async Task<List<UpFile_Entity>> GetList_UpFile(string Cnn_Code, string Sort, string Code)
        {
            using var db = new SqlConnection(_db.GetConnectionString("sw_Lib"));
            var lst = await db.QueryAsync<UpFile_Entity>("Select * From UpFile Where Cnn_Code = @Cnn_Code And Sort = @Sort And Code = @Code Order By Aid Desc", new { Cnn_Code, Sort, Code });
            return lst.ToList();
        }


        /// <summary>
        /// 근로계약서 첨부 파일 
        /// </summary>
        public async Task<List<UpFile_Entity>> GetList_CoeList(string Cnn_Code, string Sort, string Code)
        {
            using var db = new SqlConnection(_db.GetConnectionString("sw_Lib"));
            var lst = await db.QueryAsync<UpFile_Entity>("Select * From UpFile Where Cnn_Code = @Cnn_Code And Sort = @Sort And Code = @Code Order By Aid Desc", new { Cnn_Code, Sort, Code });
            return lst.ToList();
        }

        /// <summary>
        /// 파일 존재 여부 확인
        /// </summary>
        public async Task<int> GetList_CoeListCount(string Cnn_Code, string Sort, string Code)
        {
            using var db = new SqlConnection(_db.GetConnectionString("sw_Lib"));
            return await db.QuerySingleOrDefaultAsync<int>("Select Count(*) From UpFile Where Cnn_Code = @Cnn_Code And Sort = @Sort And Code = @Code", new { Cnn_Code, Sort, Code });
        }


        /// <summary>
        /// 해당공동주택에 업로드된 그림 파일 리스트 목록
        /// </summary>
        public async Task<List<UpFile_Entity>> GetList(string Apt_Code, string Sort, string Code)
        {
            using var db = new SqlConnection(_db.GetConnectionString("sw_Lib"));
            var lst = await db.QueryAsync<UpFile_Entity>("Select top 5 a.Aid, a.Apt_Code, a.FileName, a.FileSize, a.Sort, a.Code, a.Cnn_Name, a.Cnn_Code, a.File_Etc, a.Down_Count, a.PostDate, a.PostIP From UpFile as a Join Surisan as b on a.Cnn_Code = b.Num and a.Code = b.Code Where a.Apt_Code = @Apt_Code And a.Sort = @Sort And a.Code = @Code Order By a.Aid Desc", new { Apt_Code, Sort, Code });
            return lst.ToList();
        }

        /// <summary>
        /// 해당 식별코드로 입력된 첨부파일 수
        /// </summary>
        public async Task<int> UpFile_Count(string Cnn_Code)
        {
            using var db = new SqlConnection(_db.GetConnectionString("sw_Lib"));
            return await db.QuerySingleOrDefaultAsync<int>("Select Count(*) From UpFile Where Cnn_Code = @Cnn_Code", new { Cnn_Code });
        }

        /// <summary>
        /// 파일명 불러오기
        /// </summary>
        public async Task<string> FileName(string Aid)
        {
            using var db = new SqlConnection(_db.GetConnectionString("sw_Lib"));
            return await db.QuerySingleOrDefaultAsync<string>("Select FileName From UpFile Where Aid = @Aid", new { Aid });
        }

        /// <summary>
        /// 다운 수 추가 메서드
        /// </summary>
        public async Task down_count(string Aid)
        {
            using var db = new SqlConnection(_db.GetConnectionString("sw_Lib"));
            await db.QuerySingleOrDefaultAsync("Update UpFile Set Down_Count = Down_Count + 1 Where Aid = @Aid", new { Aid });
        }

        /// <summary>
        /// 다운 수 추가 메서드
        /// </summary>
        public async Task down_count_File(string FileName)
        {
            using var db = new SqlConnection(_db.GetConnectionString("sw_Lib"));
            await db.ExecuteAsync("Update UpFile Set Down_Count = Down_Count + 1 Where FileName = @FileName", new { FileName });
        }

        /// <summary>
        /// 첨부파일 모두 삭제 메서드
        /// </summary>
        public async Task Remove_UpFile_Code(string Cnn_Code)
        {
            using var db = new SqlConnection(_db.GetConnectionString("sw_Lib"));
            await db.ExecuteAsync("Delete UpFile Where Cnn_Code = @Cnn_Code", new { Cnn_Code });
        }

        /// <summary>
        /// 첨부파일 삭제 메서드
        /// </summary>
        public async Task Remove_UpFile(string Aid)
        {
            using var db = new SqlConnection(_db.GetConnectionString("sw_Lib"));
            await db.ExecuteAsync("Delete UpFile Where Aid = @Aid", new { Aid });
        }        
    }
       

    /// <summary>
    /// 공지사항 확인정보
    /// </summary>
    public class ReadView_Lib : IReadView_Lib
    {
        private readonly IConfiguration _db;
        public ReadView_Lib(IConfiguration configuration)
        {
            this._db = configuration;
        }

        public async Task Add(ReadView read)
        {
            using var db = new SqlConnection(_db.GetConnectionString("sw_Lib"));
            var sql = "Insert Into ReadView (AptCode, AptName, BoardCode, BoardID, UserCode, UserName, PostIP, PostDuty) Values (@AptCode, @AptName, @BoardCode, @BoardID, @UserCode, @UserName, @PostIP, @PostDuty)";

            await db.ExecuteAsync(sql, read);
        }

        /// <summary>
        /// 공지사항 식별코드로 목록 만들기
        /// </summary>
        public async Task<List<ReadView>> GetViewsID(string BoardID)
        {
            using var db = new SqlConnection(_db.GetConnectionString("sw_Lib"));
            var lst = await db.QueryAsync<ReadView> ("Select ReadViewId, AptCode, AptName, BoardCode, BoardID, UserCode, UserName, PostDate, PostIP, PostDuty From ReadView Where BoardID = @BoardID Order By AptName Asc, ReadViewId Asc", new { BoardID });
            return lst.ToList();
        }

        /// <summary>
        /// 공지사항 식별코드로 목록 만들기
        /// </summary>
        public async Task<List<ReadView>> GetViewsIDUser(string BoardID, string UserID)
        {
            using var db = new SqlConnection(_db.GetConnectionString("sw_Lib"));
            var lst = await db.QueryAsync<ReadView>("Select ReadViewId, AptCode, AptName, BoardCode, BoardID, UserCode, UserName, PostDate, PostIP, PostDuty From ReadView Where BoardID = @BoardID And UserCode = @UserID", new { BoardID, UserID });
            return lst.ToList();
        }

        /// <summary>
        /// 입력여부 확인
        /// </summary>
        public async Task<int> Being(string BoardID, string UserID)
        {
            using var db = new SqlConnection(_db.GetConnectionString("sw_Lib"));
            return await db.QuerySingleOrDefaultAsync<int>("Select Count(*) From ReadView Where BoardID = @BoardID And UserCode = @UserID", new { BoardID, UserID });
        }
    }    
}
