using System.Collections.Generic;
using System.Threading.Tasks;

namespace Erp_Apt_Lib
{
    /// <summary>
    /// 하자정보 인터페이스
    /// </summary>
    public interface IDefect_Lib
    {
        Task<Defect_Entity> Add_Private(Defect_Entity df);
        Task<Defect_Entity> Add_Official(Defect_Entity df);       
        
        Task Edit_Complete(Defect_Entity df);
        Task Edit_Approval(string Approval, int Aid);
        Task Edit_ImagesCount(int Aid);
        //Task Edit_dfSatisfaction(string subCompany_Code, string subCompany_Name, int Aid);
        Task Edit_dfSatisfaction(Defect_Entity df);
        Task<string> CompleteView(int Aid);
        Task Edit_Private(Defect_Entity df);
        Task Edit_Official(Defect_Entity df);
        Task Edit_Private_People(Defect_Entity df);
        //Task GetById(int Aid);
        Task<List<Defect_Entity>> GetList(string AptCode);
        Task<List<Defect_Entity>> GetList_DongHo(string AptCode, string Dong, string Ho);
        Task<int> GetList_DongHoCount(string AptCode, string Dong, string Ho);
        Task<int> GetListCount(string AptCode);
        Task<List<Defect_Entity>> GetList_Page(int Page, string AptCode);

        Task<int> GetList_Sort_Count(string AptCode, string Sort);
        Task<List<Defect_Entity>> GetList_Sort_Page(int Page, string AptCode, string Sort);

        Task<int> GetList_Details_Count(string AptCode, string Details);
        Task<List<Defect_Entity>> GetList_Details_Page(int Page, string AptCode, string Details);

        Task<List<Defect_Entity>> SearchList(string AptCode, string Feild, string Query);
        Task<Defect_Entity> Details(int Aid);
        Task Remove(int Aid);
        //Task<List<Defect_Entity>> Private_List(string Dong, string Ho);
    }
}
