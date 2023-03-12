using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace sw_Lib.Labors
{
    /// <summary>
    /// 최저임금 인터페이스
    /// </summary>
    public interface Iwage_Lib
    {
        Task<wage_Entity> Add(wage_Entity dm);
        Task<wage_Entity> Edit(wage_Entity dm);
        Task<List<wage_Entity>> GetList(int Page);

        /// <summary>
        /// 최저임금 목록 수
        /// </summary>
        /// <returns></returns>
        Task<int> GetList_Count();

        Task<int> wage(int Year);
        Task<wage_Entity> Details(int Aid);

    }
}
