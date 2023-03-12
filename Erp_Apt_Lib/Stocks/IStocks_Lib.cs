using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Erp_Apt_Lib.Stocks
{
    public interface IStocks_Lib
    {
        Task Stock_Code_Write(Stock_Code_Entity Ct);
        Task<int> Stock_Code_GetCount();
        Task<int> Stock_Code_GetCount_Apt(string AptCode);
        Task<List<Stock_Code_Entity>> St_List(int intPage);
        Task<List<Stock_Code_Entity>> St_List_Apt_A(string AptCode);
        Task<List<Stock_Code_Entity>> St_List_Apt(string AptCode, string Wh_Section);
        Task<int> St_List_Apt_Count(string AptCode, string Wh_Section);
        Task<List<Stock_Code_Entity>> St_List_Apt_New(int Page, string AptCode);
        Task<int> St_List_Apt_New_Count(string AptCode);
        Task<List<Stock_Code_Entity>> St_List_Apt_Query(string AptCode, string Field, string Query, string Section);
        Task<List<Stock_Code_Entity>> St_List_Apt_Query_new(string AptCode, string Field, string Query);
        Task<int> St_List_Apt_Query_Count(string AptCode, string Faild, string Query);
        Task<int> St_List_Apt_Query_New_Count(string AptCode, string Faild, string Query);
        Task<Stock_Code_Entity> St_View(int Num);
        Task<Stock_Code_Entity> St_View_Code(string St_Code);
        Task St_Modify(Stock_Code_Entity mm);
        Task<string> St_Name_Wh(string St_Code);
        Task<List<Stock_Code_Entity>> St_Search(string SearchField, string SearchQuery, int intPage);
        Task<List<Stock_Code_Entity>> St_SearchStGroup(string St_Group, string Apt_Code);
        Task<int> St_SearchStGroupCount(string St_Group, string Apt_Code);
        Task<int> St_Count_Data();
        Task<string> St_Down_File(int Num);
        Task<string> St_Down_Photo(int Num);
        Task Remove(int Num);

        /// <summary>
        /// 자재명 찾기
        /// </summary>
        /// <param name="Apt_Code"></param>
        /// <param name="Query"></param>
        /// <returns></returns>
        Task<List<Stock_Code_Entity>> stName_Query(string Apt_Code, string Query);

    }

    public interface IWhSock_Lib
    {
        /// <summary>
        /// 자재관리 입력
        /// </summary>
        /// <param name="Ct"></param>
        /// <returns></returns>
        Task Warehouse_Write(WareHouse_Entity Ct);

        /// <summary>
        /// 자재관리 수정
        /// </summary>
        Task Warehouse_Update(WareHouse_Entity Ct); 
        Task<List<Stock_Code_Entity>> Wh_St_Group();
        Task<List<Stock_Code_Entity>> Wh_St_Group_A(string AptCode);
        Task<List<Stock_Code_Entity>> Wh_St_Section(string St_Group);
        Task<List<Stock_Code_Entity>> Wh_St_Section_A(string St_Group, string AptCode);
        Task<List<Stock_Code_Entity>> Wh_St_Asort(string St_Group, string St_Section);
        Task<List<Stock_Code_Entity>> Wh_St_Asort_A(string St_Group, string St_Section, string AptCode);
        Task<List<Stock_Code_Entity>> Wh_St_Bloom(string St_Group, string St_Section, string St_Asort);
        Task<List<Stock_Code_Entity>> Wh_St_Bloom_A(string St_Group, string St_Section, string St_Asort, string Apt_Code);
        Task<List<Stock_Code_Entity>> Wh_St_Section_Code(string St_Group, string St_Section);
        Task<List<Stock_Code_Entity>> Wh_St_Code(string St_Group, string St_Section, string St_Asort);
        Task<List<Stock_Code_Entity>> Wh_St_Code_A(string St_Group, string St_Section, string St_Asort, string AptCode);
        Task<List<Stock_Code_Entity>> Wh_St_Code_Bloom(string St_Group, string St_Section, string St_Asort, string St_Bloom);
        Task<List<Stock_Code_Entity>> Wh_St_Code_Bloom_A(string St_Group, string St_Section, string St_Asort, string St_Bloom, string AptCode);
        Task<int> Wh_Balance(string St_Code, string AptCode);
        Task<string> Wh_Place(string St_Code, string AptCode);
        Task<int> Wh_Balance_Obj(string St_Code);
        Task<int> Wh_Count_Data();
        Task<WareHouse_Entity> Wh_View_Parents(string Parents);
        Task<WareHouse_Entity> Wh_View_StCode(string Apt_Code, string St_Code);
        Task<int> Wh_Parents_Obj(string Parents);
        Task<int> Wh_InOut_GetCount(string AptCode, string Wh_Section);
        Task<int> Wh_Gruop_GetCount(string AptCode, string St_Group);
        Task<int> Wh_Gruop_St_Section_GetCount(string AptCode, string St_Group, string St_Section);
        Task<int> Wh_InOut_A_GetCount(string AptCode, string St_Code);
        Task<int> Wh_Search_GetCount(string SearchField, string SearchQuery, string AptCode);
        Task<int> Wh_Search_A_GetCount(string StartDate, string EndDate, string St_Code, string AptCode);
        Task<List<WareHouse_Entity>> Wh_List_In_Out(int intPage, string AptCode, string Wh_Section);
        Task<List<WareHouse_Entity>> Wh_List_All(string AptCode);
        Task<int> Wh_List_All_Count(string AptCode);
        Task<List<WareHouse_Entity>> Wh_List_Wh_Section(string AptCode, string Wh_Section);
        Task<int> Wh_List_Wh_Section_Count(string AptCode, string Wh_Section);
        Task<List<WareHouse_Entity>> Wh_List_St_Grpup(string AptCode, string St_Group);
        Task<int> Wh_List_St_Grpup_Count(string AptCode, string St_Group);
        Task<List<WareHouse_Entity>> Wh_List_StName(string AptCode, string St_Name);
        Task<int> Wh_List_StName_Count(string AptCode, string St_Name);
        Task<List<WareHouse_Entity>> Wh_List_In_Out_A(int intPage, string AptCode, string Wh_Section, string St_Code);
        Task<List<WareHouse_Entity>> Search_Wh_List(string SearchField, string SearchQuery, int intPage, string AptCode);
        Task<List<WareHouse_Entity>> Search_A_Wh_List(string StartDate, string EndDate, string St_Code, string AptCode, int intPage);
        Task<List<WareHouse_Entity>> Wh_List_Parents(string AptCode, string Parents);
        Task<List<WareHouse_Entity>> Wh_View_Num(int Num);
        Task Wh_Modify_In(WareHouse_Entity mm);
        Task<int> Wh_Count_Last(string AptCode, string St_Code, string Wh_Section);
        Task<int> Wh_LastNumber();
        Task Wh_Delete(int Num);
        Task<int> SearchSt_Model(string St_Name);
        Task<int> SearchSt_Model_Apt(string St_Name, string AptCode);
        Task<List<Stock_Code_Entity>> St_Model_List(string St_Group, string St_Section, string St_Asort, string St_Bloom);
        Task<List<Stock_Code_Entity>> St_Name_Search_List(string St_Name);
        Task<List<Stock_Code_Entity>> St_Name_Search_List_Apt(string St_Name, string AptCode);
        Task<int> St_Name_Search_Count(string St_Name, string AptCode);
        Task<List<Stock_Code_Entity>> St_Wh_T_List_Data(int Page, string AptCode, string St_Group);
        Task<int> St_Wh_T_GetCount_Data(string AptCode_Ds, string St_Code_Ds);
        Task<List<SC_WH_Join_Entity>> St_Wh_Section_List_Data(int Page, string AptCode, string St_Group, string St_Section);
        Task<List<SC_WH_Join_Entity>> St_Wh_Asort_List_Data(int Page, string AptCode, string St_Group, string St_Section, string St_Asort);
        Task<int> St_Wh_Section_GetCount_Data(string AptCode_Ds, string St_Code_Ds, string St_Section_Ds);
        Task<string> St_Wh_Section_List_Detail_Data(string St_Code);
        Task<List<WareHouse_Entity>> St_Wh_Code_List_Total_Data(string AptCode, string St_Code);
        Task<List<Stock_Code_Entity>> St_Code_List_Detail_Data(string St_Code);
        Task<List<Stock_Code_Entity>> St_Group_List(int intPage, string St_Group);
        Task<List<Stock_Code_Entity>> St_Group_List_Apt(string St_Group, string AptCode);
        Task<int> St_List_AptCount(string AptCode);
        Task<List<Stock_Code_Entity>> St_Group_Section_List(int intPage, string St_Group, string St_Section);
        Task<List<Stock_Code_Entity>> St_Group_Section_Asort_List(int intPage, string St_Group, string St_Section, string St_Asort);
        Task<int> Stock_Code_GetCount_Group(string St_Group);
        Task<int> Stock_Code_GetCount_Group_Section(string St_Group, string St_Section);
        Task<int> Stock_Code_GetCount_Group_Section_Asort(string St_Group, string St_Section, string St_Asort);
        Task<int> Wh_Cost_Sum_Year(string AptCode, string St_Code, string PostDate, string PostDate_i);
        Task<int> Wh_Quantity_Sum_Year(string AptCode, string St_Code, string PostDate, string PostDate_i, string Wh_Quantity, string Wh_Section);
        Task<int> Wh_Balance_Ago_Year(string AptCode, string St_Code, string strPostDate, string strPostDate_A);
        int Wh_BalanceNew(string AptCode, string St_Code);
        int Wh_BalanceYearLast(string AptCode, string St_Code, string Wh_Year);
        Task<string> St_Aid_Last();
        int InOutSum(string Year, string St_Code, string AptCode, string Wh_Section);
        Task<List<WareHouse_Entity>> Group_List(string AptCode, string P_Group, string Parents, string Wh_Section);
        Task<List<WareHouse_Entity>> GetList_StCode(int Page, string AptCode, string St_Code);
        Task<int> GetList_StCodeCount(string AptCode, string St_Code);
        Task<DateTime> WareHouseDate(string AptCode, string St_Code);
        Task<List<WareHouse_Entity>> Group_List_AB(string AptCode, string P_Group, string Parents);
        Task<List<SC_WH_Join_Entity>>Group_List_ABC(string AptCode, string P_Group, string Parents);
        Task<int> Wh_InputCount(string AptCode, string P_Group, string Parents, string Wh_Section);
        int wh_Cost_Sum(string AptCode, string St_Code, string Wh_Year);

        /// <summary>
        /// 재고관리 (입출고) 잔고 수 가져오기
        /// </summary>
        int Wh_Balance_(string St_Code, string AptCode);

        /// <summary>
        /// 재고관리 (입출고) 잔고 수 존재여부 가져오기
        /// </summary>
        int Wh_Balance_Obj_(string St_Code);
    }
}
