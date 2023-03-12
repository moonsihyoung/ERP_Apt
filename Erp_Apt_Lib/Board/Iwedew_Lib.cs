using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wedew_Lib;

namespace Erp_Apt_Lib.Board
{
    public interface Iwedew_Lib
    {
        /// <summary>
        /// 자료실 내용 입력
        /// </summary>
        Task<wedew_board> Add(wedew_board _add);

        /// <summary>
        /// 자료실 수정
        /// </summary>
        Task<wedew_board> Edit(wedew_board _Edit);

        /// <summary>
        /// 분류별 리스트 합계 수
        /// </summary>
        Task<int> Sort_List_Count(string Sort_Field, string Sort_Query);

        /// <summary>
        /// 분류별 리스트
        /// </summary>
        Task<List<wedew_board>> GetList_Sort(int Page, string Sort_Field, string Sort_Query);

        /// <summary>
        /// 자료실 상세보기
        /// </summary>
        Task<wedew_board> Detail(string Content_Code);

        /// <summary>
        /// 가장 최근 입력된 5개 목록
        /// </summary>
        Task<List<wedew_board>> GetList_New(string Type_Code);

        /// <summary>
        /// 읽을 수 올리기
        /// </summary>
        Task Read_Count_Add(string Num);
    }

    public interface IFiless_Lib
    {
        /// <summary>
        /// 첨부파일 리스트 
        /// </summary>
        Task<List<Files>> List_Files(string Content_Code);
    }

    public interface ISort_Lib 
    {
        /// <summary>
        /// 분류정보 불러오기
        /// </summary>
        Task<Sort> Detail_Sort_Search(string SortFeild, string SortQuery);

        /// <summary>
        /// 식별코드로 전체 정보 불러오기
        /// </summary>
        Task<Sort> Detail(string Sort_Code);

        /// <summary>
        /// 분류별 목록
        /// </summary>
        Task<List<Sort>> GetList_Sort(string Up_Code);

        /// <summary>
        /// 단계별 목록
        /// </summary>
        Task<List<Sort>> GetList_Sort_Step(string Sort_Step);

        /// <summary>
        /// 하위분류 목록
        /// </summary>
        Task<List<Sort>> GetList_Sort_Code(string Sort_Code);

        /// <summary>
        /// 상위 분류코드명 불러오기
        /// </summary>
        Task<string> Sort_Up_Code(string Code);

        /// <summary>
        /// 분류명 불러오기
        /// </summary>
        Task<string> Sort_Name(string Code);

        /// <summary>
        /// 각 분류명 불러오기
        /// </summary>
        Task<string> Code_Name(string Code_Field, string Code_Query, string Code_Name);
    }

    public interface IWedew_Comments
    {
        /// <summary>
        /// 댓글 입력
        /// </summary>
        Task<Comments> Add_Comments(Comments _Co);

        /// <summary>
        /// 댓글 수정
        /// </summary>
        Task<Comments> Edit_Comments(Comments co);

        /// <summary>
        /// 대글 목록
        /// </summary>
        Task<List<Comments>> GetList_Comment(string Content_Code);

        /// <summary>
        /// 삭제
        /// </summary>
        Task Remove(int Num);
    }
}
