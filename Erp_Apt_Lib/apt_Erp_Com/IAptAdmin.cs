using System.Collections.Generic;
using System.Threading.Tasks;

namespace Erp_Apt_Lib
{
    public interface IDong_Lib
    {
        Task Add(Dong_Entity dtt);
        Task<Dong_Entity> Edit_Dong(Dong_Entity Dong);
        Task<int> Last_Number();
        Task Remeove_Dong(int AId);
        Task<List<Dong_Entity>> GetList_Dong(string Apt_Code);
        Task<int> GetList_Dong_Count(string Apt_Code);
        Task<Dong_Entity> Detail_Dong(string Dong_Code);
        Task<int> Being(string Apt_Code, string DongName);
    }

    public interface IDong_Composition_Lib
    {
        Task<Dong_Composition_Entity> Add_Dong_Composition(Dong_Composition_Entity Dong);
        Task<Dong_Composition_Entity> Edit_Dong_Composition(Dong_Composition_Entity Dong);
        Task Remeove_Dong_Composition(int AId);
        Task<int> Last_Number();
        Task<int> Overlap_Check(int AId);
        Task<double> Total_Supply_Account(string Apt_Code);
        Task<int> Total_Family_Account(string Apt_Code);
        Task<double> Total_Area_Account(string Apt_Code);
        Task<List<Dong_Composition_Entity>> GetList_Dong_Composition(string Apt_Code);
        Task<Dong_Composition_Entity> Detail_Sort(string Dong_Composition_Code);
    }
}