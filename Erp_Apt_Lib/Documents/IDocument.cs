using System.Collections.Generic;
using System.Threading.Tasks;

namespace Erp_Apt_Lib.Documents
{
    /// <summary>
    /// 문서관리 분류 클래스
    /// </summary>
    public interface IDocument_Sort_Lib
    {
        Task<Document_Sort_Entity> Add(Document_Sort_Entity ds);
        Task<Document_Sort_Entity> Edit(Document_Sort_Entity ds);
        Task<List<Document_Sort_Entity>> GetList(string Apt_Code);

        /// <summary>
        /// 문서 관리 분류 목록
        /// </summary>
        Task<List<Document_Sort_Entity>> GetList_Page(int Page);

        Task<int> GetList_Page_Count();

        Task<Document_Sort_Entity> Details(int Aid);
        Task Remove(int Aid);
        Task<string> Sort_Name(string Aid);
        string SortName(string Aid);
        Task<string> Last_Code();
    }

    /// <summary>
    /// 문서관리 클래스
    /// </summary>
    public interface IDocument_Lib
    {
        Task<int> Add(Document_Entity dc);
        Task<Document_Entity> Edit(Document_Entity dc);
        Task getOpen(int Aid);
        Task<List<Document_Entity>> GetList(string Apt_Code);
        Task<List<Document_Entity>> GetList_Page(int Page, string Apt_Code);
        Task<int> GetList_Count(string Apt_Code);
        Task<int> Division_Count(string Apt_Code, string Division, string Start, string End);
        Task<Document_Entity> Details(int Aid);
        Task Remove(int Aid);
        Task<List<Document_Entity>> SearchList(string Feild, string Query, string Apt_Code);
        Task<int> SearchList_Count(string Feild, string Query, string Apt_Code);
        Task<List<Document_Entity>> SearchList_Page(int Page, string Feild, string Query, string Apt_Code);
        Task Document_Comform(int Aid, string View);
        Task<string> Ago(string AptCode, string Aid);
        Task<int> AgoBe(string AptCode, string Aid);
        Task<string> Next(string AptCode, string Aid);
        Task<int> NextBe(string AptCode, string Aid);
        Task FilesCount(int Aid, string Division);
    }
}
