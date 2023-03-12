using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sw_Entity
{
    /// <summary>
    /// 게시판 엔터티
    /// </summary>
    public class Note_Entity
    {
        public int Num { get; set; }
        public string UserID { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string P_Adress { get; set; }
        public string Title { get; set; }
        public DateTime PostDate { get; set; }
        public string PostIP { get; set; }
        public string Content { get; set; }
        public string Password { get; set; }
        public int ReadCount { get; set; }
        public string Encoding { get; set; }
        public string Homepage { get; set; }
        public DateTime ModityDate { get; set; }
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
        public int Notice { get; set; }
        public int DownCount { get; set; }
        public int Ref { get; set; }
        public int Step { get; set; }
        public int RefOrder { get; set; }
        public int AnswerNum { get; set; }
        public int ParentNum { get; set; }
        public int CommentCount { get; set; }
        public string Category { get; set; }
        public string Sort { get; set; }
        public string Noad { get; set; }
        public string Apt_Code { get; set; }
        public string Code { get; set; }
        public string Board_Code { get; set; }
        public string Viw { get; set; }
        public string Pb { get; set; }
    }

    /// <summary>
    /// 게시판 댓글 엔터티
    /// </summary>
    public class Note_Comment_Entity
    {
        public int Num { get; set; }
        /// <summary>
        /// 유저 아이디
        /// </summary>
        public string UserID { get; set; }

        /// <summary>
        /// 유저 이름
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 주소
        /// </summary>
        public string P_Adress { get; set; }

        /// <summary>
        /// 입력일
        /// </summary>
        public DateTime PostDate { get; set; }

        /// <summary>
        /// 입력자 아이피
        /// </summary>
        public string PostIP { get; set; }

        /// <summary>
        /// 댓글
        /// </summary>
        public string Opinion { get; set; }

        /// <summary>
        /// 암호
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 게시판 번호
        /// </summary>
        public string BoardNum { get; set; }

        /// <summary>
        /// 게시판 분류
        /// </summary>
        public string Sort { get; set; }

        /// <summary>
        /// 지역
        /// </summary>
        public string Noad { get; set; }

        /// <summary>
        /// 구분
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 수정일
        /// </summary>
        public DateTime ModifyDate { get; set; }

        /// <summary>
        /// 수정자 아이피
        /// </summary>
        public string ModifyIP { get; set; }
    }

    /// <summary>
    /// 게시판 분류 엔터티
    /// </summary>
    public class Note_Sort_Entity
    {
        public int Nnm { get; set; }
        public string Board_Name { get; set; }
        public string Board_Doc { get; set; }
        public string Board_Title { get; set; }
        public string Board_Code { get; set; }
        public string Board_Master_Name { get; set; }
        public string Apt_Code { get; set; }
        public string Board_Cafe_Name { get; set; }
        public string Board_Cafe_Doc { get; set; }
        public DateTime PostDate { get; set; }
        public string PostIP { get; set; }
        public string LevelCount { get; set; }
        public string WriteCount { get; set; }
        public string DownCount { get; set; }
        public string AptCode { get; set; }
    }

    /// <summary>
    /// 첨부 파일 엔터티
    /// </summary>
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


    


    /// <summary>
    /// 주소 시도
    /// </summary>
    public class Sido_Entity
    {
        public int Aid { get; set; }
        public string Sido_Code { get; set; }
        public string Sido { get; set; }
        public string Region { get; set; }
        public string Step { get; set; }
        public DateTime PostDate { get; set; }
    }

    

    /// <summary>
    /// 로그체크
    /// </summary>
    public class Log_count_Entity
    {
        public int Aid { get; set; }

        /// <summary>
        /// 유저 아이디
        /// </summary>
        public string UserID { get; set; }

        /// <summary>
        /// 유저 이름
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 공동주택 코드
        /// </summary>
        public string Apt_Code { get; set; }

        /// <summary>
        /// 로그 카운트
        /// </summary>
        public int Log_count { get; set; }

        /// <summary>
        /// 입력일
        /// </summary>
        public DateTime PostDate { get; set; }

        /// <summary>
        /// 입력자 아이디
        /// </summary>
        public string PostIP { get; set; }
    }

    /// <summary>
    /// 각 홈피 방문여부 확인
    /// </summary>
    public class Present_Entity
    {
        /// <summary>
        /// 식별코드
        /// </summary>
        public int PresentID { get; set; }

        /// <summary>
        /// 방문자 명
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 방문자 아이디
        /// </summary>
        public string UserID { get; set; }
        /// <summary>
        /// 직책
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 등급
        /// </summary>
        public int LevelCount { get; set; }

        /// <summary>
        /// 방문일
        /// </summary>
        public int DayDate { get; set; }
        /// <summary>
        /// 방문월
        /// </summary>
        public int MonthDate { get; set; }
        /// <summary>
        /// 방문년도
        /// </summary>
        public int YearDate { get; set; }
        /// <summary>
        /// 입력일
        /// </summary>
        public DateTime PostDate { get; set; }
        /// <summary>
        /// 입력자 아이피
        /// </summary>
        public string PostIP { get; set; }
        /// <summary>
        /// 방문자 공동주택 명
        /// </summary>
        public string AptName { get; set; }
        /// <summary>
        /// 방문자 공동주택 식별코드
        /// </summary>
        public string AptCode { get; set; }
        /// <summary>
        /// 배치정보 존재 여부
        /// </summary>
        public string Viws { get; set; }
    }

    /// <summary>
    /// 공지사항 확인 여부 확인
    /// </summary>
    public class ReadView
    {
        public int ReadViewID { get; set; }
        public string AptCode { get; set; }
        public string AptName { get; set; }
        public string BoardCode { get; set; }
        public string BoardID { get; set; }
        public string UserCode { get; set; }
        public string UserName { get; set; }
        public DateTime PostDate { get; set; }
        public string PostIP { get; set; }
        public string PostDuty { get; set; }
    }    
}
