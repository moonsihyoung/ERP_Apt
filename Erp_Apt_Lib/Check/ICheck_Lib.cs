using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Erp_Apt_Lib.Check
{
    public interface ICheck_List_Lib
    {
        Task<string> CheckLIst_Date_PostDate();
        Task<Check_List_Entity> CheckList_Date_View_A(string Check_Cycle_Code, string AptCode);
        Task<Check_List_Entity> CheckList_Date_View_B(string Check_Object_Code, string Check_Cycle_Code, string AptCode);
        Check_List_Entity CheckList_Data_Effect_A(string Check_Items_Code, string Check_Cycle_Code, string Check_Year, string Check_Month, string Check_Day, string AptCode);
        int CheckList_Data_Effect_A_Count(string Check_Items_Code, string Check_Cycle_Code, string Check_Year, string Check_Month, string Check_Day, string AptCode);
        Task<Check_List_Entity> CheckList_Data_Items_Effect_Week(string Check_Items_Code, string Check_Year, int Check_Month, int Check_Day, string Total_Day, string AptCode);
        Task<int> CheckList_Data_Items_Effect_Week_Count(string Check_Items_Code, string Check_Year, int Check_Month, int Check_Day, string Total_Day, string AptCode);
        Task<Check_List_Entity> CheckList_Data_Items_Effect_Month(string Check_Items_Code, string Check_Year, int Check_Month, string AptCode);
        Task<int> CheckList_Data_Items_Effect_Month_be(string Check_Items_Code, string Check_Year, int Check_Month, string AptCode);
        Task<int> CheckList_Data_Items_Effect_Month_Count(string Check_Items_Code, string Check_Year, int Check_Month, string AptCode);
        Task<Check_List_Entity> CheckList_Data_Items_Effect_Quarter(string Check_Items_Code, string Check_Year, int Check_Month, string AptCode);
        Task<int> CheckList_Data_Items_Effect_Quarter_Count(string Check_Items_Code, string Check_Year, int Check_Month, string AptCode);
        Task<Check_List_Entity> CheckList_Data_Items_Effect_Half(string Check_Items_Code, string Check_Year, int Check_Month, string AptCode);
        Task<int> CheckList_Data_Items_Effect_Half_Count(string Check_Items_Code, string Check_Year, int Check_Month, string AptCode);
        Task<Check_List_Entity> CheckList_Data_Items_Effect_Law(string Check_Items_Code, string Check_Year, string AptCode);
        Task<int> CheckList_Data_Items_Effect_Law_Count(string Check_Items_Code, string Check_Year, string AptCode);
        Check_List_Entity CheckList_Data_Input_A(string Check_Object_Code, string Check_Cycle_Code, string Check_Year, string Check_Month, string Check_Day, string AptCode);
        int CheckList_Data_CardView_Year_Month_Day(string Check_Object_Code, string Check_Cycle_Code, string Check_Year, string Check_Month, string Check_Day, string AptCode);
        int CheckList_Data_CardView_Week(string Check_Object_Code, string Check_Cycle_Code, string Check_Year, int Check_Month, int Check_Day, string Total_Day, string AptCode);
        int CheckList_Data_CardView_Year_Month(string Check_Object_Code, string Check_Cycle_Code, string Check_Year, string Check_Month, string AptCode);
        int CheckList_Data_CardView_Quarter(string Check_Object_Code, string Check_Cycle_Code, string Check_Year, int Check_Month, string AptCode);
        int CheckList_Data_CardView_Half(string Check_Object_Code, string Check_Cycle_Code, string Check_Year, int Check_Month, string AptCode);
        int CheckList_Data_CardView_Year(string Check_Object_Code, string Check_Cycle_Code, string Check_Year, string AptCode);
        Task<int> CheckList_Data_Count();
        Task<Check_List_Entity> CheckList_Date_Insert(Check_List_Entity Ct);
        Task<Check_List_Entity> CheckList_Data_Items_Effect_A(string Check_Object_Code, string Check_Items_Code, string Check_Cycle_Code, string Check_Year, string Check_Month, string Check_Day, string AptCode);
        Task<int> CheckList_Items_Efect_Being(string Check_Object_Code, string Check_Items_Code, string Check_Cycle_Code, string Check_Year, string Check_Month, string Check_Day, string AptCode);
        Task<List<Check_List_Entity>> GetCheckList_List_Index(string Check_Object_Code, string Check_Cycle_Code, string Check_Year, string Check_Month, string Check_Day, string AptCode);
        Task<int> GetCheckList_List_Index_Count(string Check_Object_Code, string Check_Cycle_Code, string Check_Year, string Check_Month, string Check_Day, string AptCode);
        Task<List<Check_List_Entity>> CheckList_Data_Week_Index(string Check_Object_Code, string Check_Cycle_Code, string Check_Year, int Check_Month, int Check_Day, string Total_Day, string AptCode);
        Task<List<Check_List_Entity>> CheckList_Data_Week_Index_New(string Check_Object_Code, string Check_Cycle_Code, string Check_Year, string Check_Month, string Check_Day, string AptCode);
        Task<List<Check_List_Entity>> CheckList_Data_Month_Index(string Check_Object_Code, string Check_Cycle_Code, string Check_Year, int Check_Month, string AptCode);
        Task<List<Check_List_Entity>> CheckList_Data_Quarter_Index(string Check_Object_Code, string Check_Cycle_Code, string Check_Year, int Check_Month, string AptCode);
        Task<List<Check_List_Entity>> CheckList_Data_Half_Index(string Check_Object_Code, string Check_Cycle_Code, string Check_Year, int Check_Month, string AptCode);
        Task<List<Check_List_Entity>> CheckList_Data_Law_Index(string Check_Object_Code, string Check_Cycle_Code, string Check_Year, string AptCode);
        Task<Check_List_Entity> CheckList_Data_View(int Num);
        Task CheckList_Data_Modify(Check_List_Entity mm);
        Task CheckList_Date_Delete(int CheckID);
        Task CheckList_Date_Remove(int CheckID);
        Task ChdeckList_Complete(int CheckID);
        Task<string> CheckList_UserName(string Check_Object_Code, string Check_Cycle_Code, string Check_Year, string Check_Month, string Check_Day, string AptCode);
        Task<string> CheckList_Being_New(string Check_Object_Code, string Check_Cycle_Code, string Check_Year, string Check_Month, string Check_Day, string AptCode);
        Task<int> CheckList_Being_Count(string Check_Object_Code, string Check_Cycle_Code, string Check_Year, string Check_Month, string Check_Day, string AptCode);
        Task<List<Check_List_Entity>> GetCheckList_List_Index_new(string Check_Object_Code, string Check_Cycle_Code, string Check_Year, string Check_Month, string Check_Day, string AptCode);
        Task FileCountAdd(int CheckID, int FileSize, string Division);//파일 카운트 수정
        Task<List<Check_List_Entity>> GetCheckList_List_Index_Page(int Page, string Check_Object_Code, string Check_Cycle_Code, string Check_Year, string Check_Month, string Check_Day, string AptCode);
        Task<int> GetCheckList_List_Index_Count_Page(string Check_Object_Code, string Check_Cycle_Code, string Check_Year, string Check_Month, string Check_Day, string AptCode);
        Task Files_Count_Add(int Aid, string Division);

        /// <summary>
        /// 점검내용 전체 삭제
        /// </summary>
        Task CheckList_Date_Remove_All(int Check_Input_ID);
    }

    public interface ICheck_Card_Lib
    {
        Task<int> CheckCard_Data_Count();
        Task<Check_Card_Entity> CheckCard_Date_Insert(Check_Card_Entity Ct);
        Task<List<Check_Card_Entity>> CheckCard_Data_Index(string Check_Object_Code, string Check_Cycle_Code, string AptCode);
        Task<List<Check_Card_Entity>> CheckCard_InsertView_Data_Index(string AptCode);
        Task<Check_Card_Entity> CheckCard_Data_View(int Num);
        Task CheckCard_Data_Modify(Check_Card_Entity mm);
        Task CheckCard_Date_Delete(int CheckCardID);

        /// <summary>
        /// 점검대상 리스트(2022년)
        /// </summary>        
        Task<List<Check_Card_Entity>> CheckCard_Index(int Page);

        // <summary>
        /// 점검표 삭제
        /// </summary>
        Task Remove(int CheckCardID);

        /// <summary>
        /// 점검표 리스트(공동주택)(2022년)
        /// </summary>        
        Task<List<Check_Card_Entity>> CheckCard_Apt(string AptCode);

        /// <summary>
        /// 점검표 리스트(공동주택)(2022년)
        /// </summary>        
        Task<List<Check_Card_Entity>> CheckCard_Index_Apt(int Page, string AptCode);

        /// <summary>
        /// 점검표 리스트 수 (공동주택) 2022년
        /// </summary>
        Task<int> CheckCard_Index_Apt_Count(string AptCode);
    }

    public interface ICheck_Object_Lib
    {
        string CheckObject_Data_Name(string Check_Object_Code_da);
        Task<int> CheckObject_Data_Count();
        Task CheckObject_Date_Insert(Check_Object_Entity Ct);
        Task<List<Check_Object_Entity>> CheckObject_Data_Index();
        Task<Check_Object_Entity> CheckObject_Data_View(int Num_da);
        Task<Check_Object_Entity> CheckObject_Data_Modify(Check_Object_Entity mm);
        Task CheckObject_Date_Delete(int CheckObjectID);
        Task<int> CheckObject_Last();
        Task<string> CheckObject_Data_Name_Async(string Check_Object_Code_da);
    }

    public interface ICheck_Cycle_Lib
    {
        Task<int> CheckCycle_Data_Count();
        Task<string> CheckCycle_Data_Name(string Check_Cycle_Code);
        Task CheckCycle_Date_Insert(Check_Cycle_Entity Ct);
        Task<List<Check_Cycle_Entity>> CheckCycle_Data_Index();
        Task<Check_Cycle_Entity> CheckCycle_Data_View(int Num);
        Task CheckCycle_Data_Modify(Check_Cycle_Entity mm);
        Task CheckCycle_Data_Delete(int intCycleId);
        Task<List<Check_Cycle_Entity>> GetCheckCycle_List();
        string CheckCycle_Name(string Check_Cycle_Code);
        Task<int> CheckCycle_Last();
    }

    public interface ICheck_Input_Lib
    {
        Task<int> Check_GetCount_Data(string Sort, string Code, string AptCode);
        Task<int> CheckInput_Data_Count();
        Task<int> CheckInput_Data_View_Year_Month_Day(string Check_Object_Code, string Check_Cycle_Code, string Check_Year, string Check_Month, string Check_Day, string AptCode);
        Task<int> CheckInput_Data_View_Year_Month_Day_Aid(string Check_Object_Code, string Check_Cycle_Code, string Check_Year, string Check_Month, string Check_Day, string AptCode);
        Task<int> CheckInput_Data_View_Year_Month_Day_Last(string Check_Object_Code, string Check_Cycle_Code, string Check_Year, string Check_Month, string Check_Day, string AptCode);
        Task<int> CheckInput_Date_Insert(Check_Input_Entity Ct);
        Task<List<Check_Input_Entity>> CheckInput_Data_Index(string Check_Object_Code, string Check_Cycle_Code, string AptCode);
        Task<int> Check_Input_Index_GetCount_Data(string Check_Object_Code, string Check_Cycle_Code, string AptCode);
        Task<List<Check_Input_Entity>> CheckInput_Data_Index_All(string AptCode);
        Task<int> CheckInput_Data_Index_All_Count(string AptCode);
        Task<int> Check_Input_Index_GetCount_All_Data(string AptCode);
        Task<List<Check_Input_Entity>> CheckInput_Data_Index_Object(string Check_Object_Code, string AptCode);
        Task<int> CheckInput_Data_Index_Object_Count(string Check_Object_Code, string AptCode);
        Task<int> Check_Input_Index_GetCount_Object_Data(string Check_Object_Code, string AptCode);
        Task<List<Check_Input_Entity>> CheckInput_Data_Index_Cycle(string Check_Cycle_Code, string AptCode);
        Task<int> CheckInput_Data_Index_Cycle_Count(string Check_Cycle_Code, string AptCode);
        Task<int> Check_Input_Index_GetCount_Cycle_Data(string Check_Cycle_Code, string AptCode);
        Task<List<Check_Input_Entity>> CheckInput_Data_Ago(string AptCode, int Num);
        Task<int> CheckInput_AgoA(string AptCode, int Num);
        Task<int> Check_Input_Count_Ago_Data(string AptCode, int Num);
        Task<List<Check_Input_Entity>> CheckInput_Data_Next(string AptCode, int Num);
        Task<int> CheckInput_NextA(string AptCode, int Num);
        Task<int> Check_Input_Count_Next_Data(string AptCode, int Num);
        Task<Check_Input_Entity> CheckInput_Data_View(int CheckInputID, string AptCode);
        Task CheckInput_Data_Modify(Check_Input_Entity mm);
        Task CheckInput_Date_Delete(int CheckInputID);
        Task<List<Check_Input_Entity>> GetCheckInput_List(string AptCode, int Page);
        Task<string> Input_Complate(int Num);
        Task<int> CheckInput_CountNext(string Apt_Code, int Num);
        Task<List<Check_Input_Entity>> CheckInput_Next(string AptCode, int Num);
        Task<int> CheckInput_CountAgo(string AptCode, int Num);
        Task<List<Check_Input_Entity>> CheckInput_Ago(string AptCode, int Num);
        Task<List<Check_Input_Entity>> CheckInput_Data_Index_Page_All(int Page, string AptCode);
        Task<List<Check_Input_Entity>> CheckInput_Data_Index_Page_Object(int Page, string Check_Object_Code, string AptCode);
        Task<List<Check_Input_Entity>> CheckInput_Data_Index_Page_Cycle(int Page, string Check_Cycle_Code, string AptCode);
        Task<List<Check_Input_Entity>> CheckInput_Data_Index_Page_All_new(int Page, string AptCode);
        Task Check_Count_Add(int Aid, string Division);
        Task Files_Count_Add(int Aid, string Division);
    }

    public interface ICheck_Items_Lib
    {
        Task<string> CheckItems_Data_Name(string Check_Items_Code);

        /// <summary>
        /// 점검사항 이름
        /// </summary>
        string CheckItems_Name(string Check_Items_Code);

        Task<int> CheckItems_Data_Count();
        Task<int> CheckItems_View_Data_Count(string strObject_Code, string strCycle_Code);
        Task CheckItems_Date_Insert(Check_Items_Entity Ct);
        Task<List<Check_Items_Entity>> CheckItems_Data_Index(string Check_Object_Code, string Check_Cycle_Code);
        Task<int> CheckItems_Data_Index_Count(string Check_Object_Code, string Check_Cycle_Code);
        Task<Check_Items_Entity> CheckItems_Data_View(int CheckItemsID);
        Task<Check_Items_Entity> CheckItems_Data_Details(int CheckItemsID);
        Task<int> CheckItems_Data_View_Be(int CheckItemsID);
        Task CheckItems_Data_Modify(Check_Items_Entity mm);
        Task CheckItems_Date_Delete(int CheckItemsID);
        Task<List<Check_Items_Entity>> CheckItems_Index(int Page);
        Task<int> CheckItems_Index_Count();
        Task<List<Check_Items_Entity>> CheckItems_Page_Index(int Page, string Check_Object_Code, string Check_Cycle_Code);
        Task<int> CheckItems_Last();
        Task<List<Check_Items_Entity>> CheckItems_Data_Join_Index( int Page, string Check_Object_Code, string Check_Cycle_Code);
    }

    public interface ICheck_Effect_Lib
    {
        string CheckEffect_Data_Name(string Check_Effect_Code);
        Task<int> CheckEffect_Data_Count();
        Task CheckEffect_Date_Insert(Check_Effect_Entity Ct);
        Task<List<Check_Effect_Entity>> CheckEffect_Data_Index(string AptCode);
        Task<Check_Effect_Entity> CheckEffect_Data_View(int Num);
        Task CheckEffect_Data_Modify(Check_Effect_Entity mm);
        Task CheckEffect_Date_Delete(int CheckEffectID);

    }
}
