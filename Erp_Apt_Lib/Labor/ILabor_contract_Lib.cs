using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace sw_Lib.Labors
{
    public interface ILabor_contract_Lib
    {
        Task<Labor_contract_Entity> add(Labor_contract_Entity labor_Contract);
        Task<List<Labor_contract_Entity>> GetList(string Apt_Code);

        /// <summary>
        /// 근로계약서 목록(단지별)
        /// </summary>
        Task<List<Labor_contract_Entity>> GetList_All(int Page);

        Task<List<Labor_contract_Entity>> GetList_User(string Apt_Code, string UserID);
        Task<Labor_contract_Entity> Edit(Labor_contract_Entity _Contract_Entity);
        Task<List<Labor_contract_Entity>> Contract_list_Apt(int Page, string Apt_Code);

        /// <summary>
        /// 단지별 근로계약서 목록
        /// </summary>
        Task<List<Labor_contract_Entity>> Contract_list_Name(int Page, string UserName);

        /// <summary>
        /// 단지별 근로계약서 수
        /// </summary>
        Task<int> Contract_List_Name_count(string UserName);

        Task<List<Labor_contract_Entity>> Contract_list_Apt_A(string Apt_Code);
        Task<List<Labor_contract_Entity>> Contract_list_Apt_All();
        Task<List<Labor_contract_Entity>> Contract_list_A();
        Task<List<Labor_contract_Entity>> Contract_list_C();
        Task<int> Contract_count_Apt(string Apt_Code);
        Task<int> Contract_count_All();
        Task<List<Labor_contract_Entity>> labor_C_list_Apt(int Page, string Apt_Code, string Division);
        Task<int> labor_C_count_Apt(string Apt_Code, string Division);
        Task<List<Labor_contract_Entity>> labor_C_list_Cor(int Page, string Cor_Code, string Division);
        Task<int> labor_C_count_Cor(string Cor_Code, string Division);
        Task<List<Labor_contract_Entity>> labor_C_list_Cor_Apt(int Page, string Cor_Code, string Apt_Code, string Division);
        Task<int> labor_C_count_Cor_Apt(string Cor_Code, string Apt_Code, string Division);
        Task<Labor_contract_Entity> labor_Contract_detail(string Aid);
        Task<Labor_contract_Entity> labor_Contract_detail_UserID(string UserID);
        Task Approval(string Aid);
        Task<int> labor_Contract_detail_userID_Being(string UserID);
        Task Remove(string Aid);
        Task Files_Count_Add(int Aid, string Division);
    }
}
