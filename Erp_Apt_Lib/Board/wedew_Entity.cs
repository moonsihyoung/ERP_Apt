using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// 자료실 속성(엔터티)
/// </summary>
namespace Wedew_Lib
{
    /// <summary>
    /// 게시판 엔터티
    /// </summary>
    public class wedew_board
    {
        public int Num { get; set; }
        public string Content_Code { get; set; }
        public string User_Code { get; set; }
        public string Type_Code { get; set; }
        public string Sort_Code { get; set; }
        public string Cate_Code { get; set; }
        public string Group_Code { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public int CommentCount { get; set; }
        public DateTime PostDate { get; set; }
        public string PostIP { get; set; }
        public int ReadCount { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Personal { get; set; }
        public string Viw { get; set; }
        public string Encoding { get; set; }
        public string Homegage { get; set; }
        public DateTime ModifyDate { get; set; }
        public string ModifyIP { get; set; }
        public string FileName { get; set; }
        public int FileSize { get; set; }
        public string FileName2 { get; set; }
        public int FileSize2 { get; set; }
        public string FileName3 { get; set; }
        public int FileSize3 { get; set; }
        public string FileName4 { get; set; }
        public int FileSize4 { get; set; }
        public string FileName5 { get; set; }
        public int FileSize5 { get; set; }
        public int DownCount { get; set; }
        public int Ref { get; set; }
        public int Step { get; set; }
        public int RefOrder { get; set; }
        public int AnswerNum { get; set; }
        public string Noad { get; set; }
        public string del { get; set; }
    }

    /// <summary>
    /// 댓글 엔터티
    /// </summary>
    public class Comments
    {
        /// <summary>
        /// 일련번호
        /// </summary>
        public int Num { get; set; }
        /// <summary>
        /// 게시판 코드
        /// </summary>
        public string Board_Code { get; set; }
        /// <summary>
        /// 식별코드
        /// </summary>
        public string Content_Code { get; set; }
        /// <summary>
        /// 작성자 코드
        /// </summary>
        public string User_Code { get; set; }
        /// <summary>
        /// 작성자 아이디
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 코멘트 내용
        /// </summary>
        public string Opinion { get; set; }
        /// <summary>
        /// 입력일
        /// </summary>
        public DateTime PostDate { get; set; }
        /// <summary>
        /// 입력자 아이피
        /// </summary>
        public string PostIP { get; set; }
        /// <summary>
        /// 수정일
        /// </summary>
        public DateTime ModifyDate { get; set; }
        /// <summary>
        /// 수정자 아이피
        /// </summary>
        public string ModifyIP { get; set; }
        /// <summary>
        /// 삭제 여부
        /// </summary>
        public string del { get; set; }
    }

    /// <summary>
    /// 첨부파일 엔터티
    /// </summary>
    public class Files
    {
        /// <summary>
        /// 식별코드
        /// </summary>
        public int Aid { get; set; }
        /// <summary>
        /// 게시판 분류 코드(부모 분류 코드)
        /// </summary>
        public string Board_Code { get; set; }
        /// <summary>
        /// 게시판 일련번호(식별코드)
        /// </summary>
        public string Content_Code { get; set; }
        /// <summary>
        /// 입력자명
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 파일크기
        /// </summary>
        public int Size { get; set; }
        /// <summary>
        /// 다운 수
        /// </summary>
        public int DownCount { get; set; }
        /// <summary>
        /// 입력일
        /// </summary>
        public DateTime PostDate { get; set; }
        /// <summary>
        /// 입력자 아이피
        /// </summary>
        public string PostIP { get; set; }
        /// <summary>
        /// 상세 설명
        /// </summary>
        public string Detail { get; set; }
        /// <summary>
        /// 삭제 여부
        /// </summary>
        public string del { get; set; }
    }

    /// <summary>
    /// 분류 엔터티
    /// </summary>
    public class Sort
    {
        /// <summary>
        /// 식별코드
        /// </summary>
        public int Aid { get; set; }
        /// <summary>
        /// 분류코드
        /// </summary>
        public string Sort_Code { get; set; }
        /// <summary>
        /// 상위분류코드
        /// </summary>
        public string Up_Code { get; set; }
        /// <summary>
        /// 분류명
        /// </summary>
        public string Sort_Name { get; set; }
        /// <summary>
        /// 단계수
        /// </summary>
        public string Sort_Step { get; set; }
        /// <summary>
        /// 관리코드
        /// </summary>
        public string Admin_Code { get; set; }
        /// <summary>
        /// 입력자 레벨
        /// </summary>
        public int User_Count { get; set; }

        /// <summary>
        /// 설명
        /// </summary>
        public string Detail { get; set; }

        /// <summary>
        /// 입력일
        /// </summary>
        public DateTime WriteDate { get; set; }

        /// <summary>
        /// 입력자 아이피
        /// </summary>
        public string PostIP { get; set; }
    }
}
