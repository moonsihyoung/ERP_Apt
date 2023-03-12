using Dapper;
using Erp_Apt_Lib.Board;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.AccessControl;
using System.Threading.Tasks;

/// <summary>
/// 자료실 메서드
/// </summary>
namespace Wedew_Lib
{
    /// <summary>
    /// 자료실 클래스
    /// </summary>
    public class wedew_Lib : Iwedew_Lib
    {
        private readonly IConfiguration _db;
        public wedew_Lib(IConfiguration configuration)
        {
            this._db = configuration;
        }

        /// <summary>
        /// 자료실 내용 입력
        /// </summary>
        public async Task<wedew_board> Add(wedew_board _add)
        {
            using var db = new SqlConnection(_db.GetConnectionString("wedew"));
            var sql = "Insert Wedew(Content_Code, User_Code, Password, Email, Type_Code, Sort_Code, Cate_Code, Group_Code, Title, Content, PostIP) Values(@Content_Code, @User_Code, @Password, @Email, @Type_Code, @Sort_Code, @Cate_Code, @Group_Code, @Title, @Content, @PostIP); Select Cast(SCOPE_IDENTITY() As Int);";

            var Num = await db.QuerySingleOrDefaultAsync<int>(sql, _add);
            _add.Num = Num;
            return _add;
        }

        /// <summary>
        /// 자료실 수정
        /// </summary>
        public async Task<wedew_board> Edit(wedew_board _Edit)
        {
            using var db = new SqlConnection(_db.GetConnectionString("wedew"));
            var sql = "Update Wedew Set Type_Code = @Type_Code,  Sort_Code = @Sort_Code, Cate_Code = @Cate_Code, Group_Code = @Group_Code, Title = @Title, Content = @Content Where Content_Code = @Content_Code;";
            await db.ExecuteAsync(sql, _Edit);
            return _Edit;
        }

        /// <summary>
        /// 분류별 리스트 합계 수
        /// </summary>
        public async Task<int> Sort_List_Count(string Sort_Field, string Sort_Query)
        {
            using var db = new SqlConnection(_db.GetConnectionString("wedew"));
            return await db.QuerySingleOrDefaultAsync<int>("Select Count(*) From Wedew Where " + Sort_Field + " = @Sort_Query", new { Sort_Field, Sort_Query } );
        }

        /// <summary>
        /// 분류별 리스트
        /// </summary>
        public async Task<List<wedew_board>> GetList_Sort(int Page, string Sort_Field, string Sort_Query)
        {
            using var db = new SqlConnection(_db.GetConnectionString("wedew"));
            var lst = await db.QueryAsync<wedew_board>("Wedew_List_Sort", new { Page, Sort_Field, Sort_Query }, commandType: CommandType.StoredProcedure);
            return lst.ToList();
        }

        /// <summary>
        /// 자료실 상세보기
        /// </summary>
        public async Task<wedew_board> Detail(string Content_Code)
        {
            using var db = new SqlConnection(_db.GetConnectionString("wedew"));
            return await db.QuerySingleOrDefaultAsync<wedew_board>("Select Num, Content_Code, User_Code, Type_Code, Sort_Code, Cate_Code, Group_Code, Title, Content, CommentCount, PostDate, PostIP, ReadCount, Name, Email, Password, Personal, Viw, Encoding, Homepage, ModifyDate, ModifyIP, DownCount, Noad, del From Wedew  Where Num = @Content_Code", new { Content_Code });
        }

        /// <summary>
        /// 가장 최근 입력된 5개 목록
        /// </summary>
        public async Task<List<wedew_board>> GetList_New(string Type_Code)
        {
            using var db = new SqlConnection(_db.GetConnectionString("wedew"));
            var lst = await db.QueryAsync<wedew_board>("Select Top 7 Num, Content_Code, User_Code, Type_Code, Sort_Code, Cate_Code, Group_Code, Title, Content, CommentCount, PostDate, PostIP, ReadCount, Name, Email, Password, Personal, Viw, Encoding, Homepage, ModifyDate, ModifyIP, DownCount, Ref, Step, RefOrder, AnswerNum, ParentNum, Noad, del From Wedew Where Type_Code = @Type_Code Order By Num Desc", new { Type_Code });
            return lst.ToList();
        }

        /// <summary>
        /// 읽을 수 올리기
        /// </summary>
        public async Task Read_Count_Add(string Num)
        {
            using var db = new SqlConnection(_db.GetConnectionString("wedew"));
            await db.ExecuteAsync("Update Wedew Set ReadCount = ReadCount + 1 Where Content_Code = @Num", new { Num });
        }
    }

    /// <summary>
    /// 파일관련 자료 클래스
    /// </summary>
    public class Filess_Lib : IFiless_Lib
    {
        private readonly IConfiguration _db;
        public Filess_Lib(IConfiguration configuration)
        {
            this._db = configuration;
        }

        /// <summary>
        /// 첨부파일 리스트 
        /// </summary>
        public async Task<List<Files>> List_Files(string Content_Code)
        {
            using var db = new SqlConnection(_db.GetConnectionString("wedew"));
            var lst = await db.QueryAsync<Files>("Select * From Files Where Content_Code = @Content_Code And del = 'A' Order By Aid Desc", new { Content_Code });
            return lst.ToList();
        }

    }

    /// <summary>
    /// 자료실 분류 클래스
    /// </summary>
    public class Sort_Lib : ISort_Lib
    {
        private readonly IConfiguration _db;
        public Sort_Lib(IConfiguration configuration)
        {
            this._db = configuration;
        }

        /// <summary>
        /// 분류정보 불러오기
        /// </summary>
        public async Task<Sort> Detail_Sort_Search(string SortFeild, string SortQuery)
        {
            using var db = new SqlConnection(_db.GetConnectionString("wedew"));
            return await db.QuerySingleOrDefaultAsync<Sort>("", new { SortFeild, SortQuery });
        }

        /// <summary>
        /// 식별코드로 전체 정보 불러오기
        /// </summary>
        public async Task<Sort> Detail(string Sort_Code)
        {
            using var db = new SqlConnection(_db.GetConnectionString("wedew"));
            return await db.QuerySingleOrDefaultAsync<Sort>("Select Aid, Sort_Code, Up_Code, Sort_Name, Sort_Step, Admin_Code, User_Count, Detail, WriteDate, PostIP From Sort Where del = 'A' And Sort_Code = @Sort_Code And we = 'A'", new { Sort_Code });
        }

        /// <summary>
        /// 분류별 목록
        /// </summary>
        public async Task<List<Sort>> GetList_Sort(string Up_Code)
        {
            using var db = new SqlConnection(_db.GetConnectionString("wedew"));
            var lst = await db.QueryAsync<Sort>("Select Aid, Sort_Code, Up_Code, Sort_Name, Sort_Step, Admin_Code, User_Count, Detail, WriteDate, PostIP From Sort Where Up_Code = @Up_Code And del = 'A' And we = 'A' Order By Aid Desc", new { Up_Code });
            return lst.ToList();
        }

        /// <summary>
        /// 단계별 목록
        /// </summary>
        public async Task<List<Sort>> GetList_Sort_Step(string Sort_Step)
        {
            using var db = new SqlConnection(_db.GetConnectionString("wedew"));
            var lst = await db.QueryAsync<Sort>("Select Aid, Sort_Code, Up_Code, Sort_Name, Sort_Step, Admin_Code, User_Count, Detail, WriteDate, PostIP From Sort Where Sort_Step = @Sort_Step And we = 'A' Order By Aid Asc", new { Sort_Step });
            return lst.ToList();
        }

        /// <summary>
        /// 하위분류 목록
        /// </summary>
        public async Task<List<Sort>> GetList_Sort_Code(string Sort_Code)
        {
            using var db = new SqlConnection(_db.GetConnectionString("wedew"));
            var lst = await db.QueryAsync<Sort>("Select Aid, Sort_Code, Up_Code, Sort_Name, Sort_Step, Admin_Code, User_Count, Detail, WriteDate, PostIP From Sort Where Up_Code = @Sort_Code And del = 'A' And we = 'A' Order By Aid Desc", new { Sort_Code });
            return lst.ToList();
        }

        /// <summary>
        /// 상위 분류코드명 불러오기
        /// </summary>
        public async Task<string> Sort_Up_Code(string Code)
        {
            using var db = new SqlConnection(_db.GetConnectionString("wedew"));
            return await db.QuerySingleOrDefaultAsync<string>("Select Top 1 Sort_Up_Code From Sort Where del = 'A' And Sort_Code = @Code And we = 'A' Order By Aid Desc", new { Code });
        }

        /// <summary>
        /// 분류명 불러오기
        /// </summary>
        public async Task<string> Sort_Name(string Code)
        {
            using var db = new SqlConnection(_db.GetConnectionString("wedew"));
            return await db.QuerySingleOrDefaultAsync<string> ("Select Top 1 Sort_Name From Sort Where del = 'A' And Sort_Code = @Code And we = 'A' Order By Aid Desc", new { Code });
        }

        /// <summary>
        /// 각 분류명 불러오기
        /// </summary>
        public async Task<string> Code_Name(string Code_Field, string Code_Query, string Code_Name)
        {
            using var db = new SqlConnection(_db.GetConnectionString("wedew"));
            return await db.QuerySingleOrDefaultAsync<string>("Sort_CodeName", new { Code_Field, Code_Query, Code_Name }, commandType: CommandType.StoredProcedure);
        }
    }

    /// <summary>
    /// 댓글 클래스
    /// </summary>
    public class Wedew_Comments : IWedew_Comments
    {
        private readonly IConfiguration _db;
        public Wedew_Comments(IConfiguration configuration)
        {
            this._db = configuration;
        }

        /// <summary>
        /// 댓글 입력
        /// </summary>
        public async Task<Comments> Add_Comments(Comments _Co)
        {
            using var db = new SqlConnection(_db.GetConnectionString("wedew"));
            var sql = "Insert Comments (Board_Code, Content_Code, User_Code, Password, Opinion, PostIP) Values (@Board_Code, @Content_Code, @User_Code, @Password, @Opinion, @PostIP);";
            await db.ExecuteAsync(sql, _Co);
            return _Co;
        }        

        /// <summary>
        /// 댓글 수정
        /// </summary>
        public async Task<Comments> Edit_Comments(Comments co)
        {
            using var db = new SqlConnection(_db.GetConnectionString("wedew"));
            await db.ExecuteAsync("Update Comments Set Opinion = @Opinion, ModifyDate = getdate() ModifyIP = @ModifyIP Where Num = @Num And Password = @Password;", co);
            return co;
        }

        /// <summary>
        /// 대글 목록
        /// </summary>
        public async Task<List<Comments>> GetList_Comment(string Content_Code)
        {
            using var db = new SqlConnection(_db.GetConnectionString("wedew"));
            var lst = await db.QueryAsync<Comments>("Select * From Comments Where Content_Code = @Content_Code And del = 'A' Order By Aid Desc", new { Content_Code });
            return lst.ToList();
        }

        /// <summary>
        /// 삭제
        /// </summary>
        public async Task Remove(int Num)
        {
            using var db = new SqlConnection(_db.GetConnectionString("wedew"));
            await db.ExecuteAsync("Update Comments Set del = 'B' Where Num = @Num", new { Num });
        }
    }    
}
