using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Erp_Apt_Lib.Decision
{
    public interface IDecision_Lib
    {
        Task<Decision_Entity> Detail(string AptCode, string BloomCode, string Parent, string PostDuty);
        Task<int> Detail_Count(string AptCode, string BloomCode, string Parent, string PostDuty);
        int Details_Count(string AptCode, string BloomCode, string Parent, string PostDuty);
        Decision_Entity Details(string AptCode, string BloomCode, string Parent, string PostDuty);
        Task<int> Decision_Being_Count(string AptCode, string Parent, string BloomCode, string PostDuty, string User_Code);
        int Decisions_Being_Count(string AptCode, string Parent, string BloomCode, string PostDuty, string User_Code);
        int PostDutyBeCount(string AptCode, string Bloom, string PostDuty);
        Task<int> Add(Decision_Entity _Entity);
        Task Decision_Comform(string Num, string TableName, string Conform, string Feild);
    }

    public interface IDbImagesLib
    {
        Task<int> Add(DbImagesEntity db);
        Task<List<DbImagesEntity>> GetList(string AptCode);
        /// <summary>
        /// 이미지 불러오기
        /// </summary>
        Task<byte[]> Photo_image(string UserCode);

        /// <summary>
        /// 도장 존재 여부
        /// </summary>
        Task<int> Photo_Count(string UserCode);


        /// <summary>
        /// 이미지 불러오기
        /// </summary>
        byte[] Photos_image(string UserCode);

        /// <summary>
        /// 도장 존재 여부
        /// </summary>
        int Photos_Count(string UserCode);
    }
}
