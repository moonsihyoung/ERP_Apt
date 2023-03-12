using Sw_Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sw_Lib
{
	public interface Isw_Note_Lib
	{
        /// <summary>
        /// 게시판 입력
        /// </summary>
        Task<int> Add(Note_Entity _note);

        /// <summary>
        /// 게시판 수정
        /// </summary>
        Task<Note_Entity> Edit(Note_Entity _note);

        /// <summary>
        /// 임시 게시물 삭제
        /// </summary>
        Task Remove_Temp();

        /// <summary>
        /// 메인화면 최신 게시판 정보 보여주기
        /// </summary>
        Task<List<Note_Entity>> MainListA(string _sort, string _code);

        /// <summary>
        /// 게시판 리스트
        /// </summary>
        Task<List<Note_Entity>> GetList(int Page, string Sort, string Code, string Apt_Code);

        /// <summary>
        /// 입력된 수 
        /// </summary>
        Task<int> GetList_Count(string Sort, string Code, string Apt_Code);

        /// <summary>
        /// 게시판 모바일 리스트
        /// </summary>
        Task<List<Note_Entity>> GetList_M(int Page, string Sort, string Code, string Apt_Code);

        /// <summary>
        /// 분류별 찾기
        /// </summary>
        Task<List<Note_Entity>> Search_List(int Page, string SearchField, string SearchQuery, string _sort, string _code);

        /// <summary>
        /// 검색된 수 
        /// </summary>
        Task<int> Search_List_Count(string SearchField, string SearchQuery, string _sort, string _code);

        /// <summary>
        /// 게시판 상세보기
        /// </summary>
        Task<Note_Entity> Detail_Note(string Num, string _sort, string _code);

        /// <summary>
        /// 게시판 제목 불러오기
        /// </summary>
        Task<string> Detail_Title(string Num);

        /// <summary>
        /// 분류별 찾기(전체)
        /// </summary>
        Task<List<Note_Entity>> All_Search_List(int Page, string Apt_Code, string SearchQuery);

        /// <summary>
        /// 메인 전체 검색 tn
        /// </summary>
        Task<int> All_Search_Count(string Apt_Code, string SearchQuery);

        /// <summary>
        /// 게시글 삭제
        /// </summary>
        Task Remove(int Num);

        /// <summary>
        /// 읽은 수 입력
        /// </summary>
        Task Update_ReadCount(string Num);

        /// <summary>
        /// 댓글 수 입력
        /// </summary>
        Task Update_CommentCount(string Num);

        /// <summary>
        /// 댓글 수 줄이기 입력
        /// </summary>
        /// <param name="Num"></param>
        Task Update_CommentCount_Remove(string Num);

        /// <summary>
        /// 마지막 입력된 공지사항의 일련번호
        /// </summary>
        Task<int> top_count();
    }

    public interface ISurisan_Comments
    {
        /// <summary>
        /// 코멘트 입력
        /// </summary>
        Task<Note_Comment_Entity> Add(Note_Comment_Entity cont);

        /// <summary>
        /// 코멘트 수정
        /// </summary>        
        Task Edit(string Opinion, string Num);

        /// <summary>
        /// 댓글 목록만들기
        /// </summary>
        Task<List<Note_Comment_Entity>> GetList(string BoardNum);

        /// <summary>
        /// 댓글 목록만들기(Code)
        /// </summary>
        Task<List<Note_Comment_Entity>> GetList_Sort(string BoardNum, string Code);

        /// <summary>
        /// 댓글 삭제
        /// </summary>
        /// <param name="Num"></param>
        Task Remove(int Num);

        /// <summary>
        /// 게시판에 입력된 댓글 합계
        /// </summary>
        Task<int> TotalCount(string BoardNum);

        /// <summary>
        /// 수정을 위하여 해당글 있는지 확인
        /// </summary>
        Task<int> Being(string Num);
    }

    public interface Isw_Note_Sort_Lib
    {
        /// <summary>
        /// 게시판 분류 입력하기
        /// </summary>
        Task<Note_Sort_Entity> Add(Note_Sort_Entity _Sort_Entity);

        /// <summary>
        /// 게시판 분류 수정하기
        /// </summary>
        Task<Note_Sort_Entity> edit(Note_Sort_Entity _Sort_Entity);

        /// <summary>
        /// 게시판명 불러오기
        /// </summary>
        Task<string> Sort_Name(string Board_Title, string Board_Cafe_Name);

        Task<string> Sort_NameA(string Sort, string Code);
    }

    public interface IUpFiless_Lib
    {
        /// <summary>
        /// 첨부파일 올리기 메서드
        /// </summary>
        Task<UpFile_Entity> Add_UpFile(UpFile_Entity UpFile);

        /// <summary>
        /// 첨부 파일 수정 메서드
        /// </summary>
        Task<UpFile_Entity> Edit_UpFile(UpFile_Entity UpFile);

        /// <summary>
        /// 해당 게시판 글에 첨부파일 관련 코드로 리스트 메서드
        /// </summary>
        Task<List<UpFile_Entity>> GetList_UpFile(string Cnn_Code, string Sort, string Code);

        /// <summary>
        /// 근로계약서 첨부 파일 
        /// </summary>
        Task<List<UpFile_Entity>> GetList_CoeList(string Cnn_Code, string Sort, string Code);

        /// <summary>
        /// 파일 존재 여부 확인
        /// </summary>
        Task<int> GetList_CoeListCount(string Cnn_Code, string Sort, string Code);

        /// <summary>
        /// 해당공동주택에 업로드된 그림 파일 리스트 목록
        /// </summary>
        Task<List<UpFile_Entity>> GetList(string Apt_Code, string Sort, string Code);

        /// <summary>
        /// 해당 식별코드로 입력된 첨부파일 수
        /// </summary>
        Task<int> UpFile_Count(string Cnn_Code);

        /// <summary>
        /// 파일명 불러오기
        /// </summary>
        Task<string> FileName(string Aid);

        /// <summary>
        /// 다운 수 추가 메서드
        /// </summary>
        Task down_count(string Aid);

        /// <summary>
        /// 다운 수 추가 메서드
        /// </summary>
        Task down_count_File(string FileName);

        /// <summary>
        /// 첨부파일 모두 삭제 메서드
        /// </summary>
        Task Remove_UpFile_Code(string Cnn_Code);

        /// <summary>
        /// 첨부파일 삭제 메서드
        /// </summary>
        Task Remove_UpFile(string Aid);
    }

    public interface IReadView_Lib
    {
        Task Add(ReadView read);

        /// <summary>
        /// 공지사항 식별코드로 목록 만들기
        /// </summary>
        Task<List<ReadView>> GetViewsID(string BoardID);


        /// <summary>
        /// 공지사항 식별코드로 목록 만들기
        /// </summary>
        Task<List<ReadView>> GetViewsIDUser(string BoardID, string UserID);

        /// <summary>
        /// 입력여부 확인
        /// </summary>
        Task<int> Being(string BoardID, string UserID);
    }
}
