using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Erp_Apt_Lib.Draft
{
    public interface IDraft_Lib
    {
        Task<int> Add(DraftEntity df);
        Task Edit(DraftEntity df);
        Task<List<DraftEntity>> GetList(int Page, string AptCode);
        Task<int> GetListCount(string AptCode);
        Task<List<DraftEntity>> SearchList(int Page, string Feild, string Query, string AptCode);
        Task<int> SearchListCount(string Feild, string Query, string AptCode);
        Task<DraftEntity> Details(int Aid);
        Task Remove(int Aid);
        Task Decision(int Aid);
        Task<int> LastAid(string AptCode, int Year);
        Task Draft_Comform(int Aid, string View);
        Task<string> Comform(int Aid);
        Task<string> Ago(string AptCode, string Aid);
        Task<int> AgoBe(string AptCode, string Aid);
        Task<string> Next(string AptCode, string Aid);
        Task<int> NextBe(string AptCode, string Aid);
        Task FilesCount(int Aid, string Division);
    }

    public interface IDraftDetail_Lib
    {
        Task Add(DraftDetailEntity dd);
        Task Edit(DraftDetailEntity dd);
        Task<List<DraftDetailEntity>> GetList(int ParentAid);
        Task<int> GetListCount(int ParentAid);
        Task Remove(int Aid);
        Task<double> ToralCount(int ParentAid);
        double ToralSum(int ParentAid);
        Task<double> VatToralCount(int ParentAid);

    }

    public interface IDraftAttach_Lib
    {
        Task<int> Add(DraftAttachEntity da);
        Task Edit(DraftAttachEntity da);
        Task<List<DraftAttachEntity>> GetList(int ParentAid);
        Task<int> GetListCount(int ParentAid);
        Task Delete(int Aid);
    }
}
