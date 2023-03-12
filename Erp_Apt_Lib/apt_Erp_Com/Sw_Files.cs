using Erp_Lib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using System.Data;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;

namespace Erp_Apt_Lib
{
    public class Sw_Files_Entity
    {
        public int Num { get; set; }
        public string AptCode { get; set; }
        public string Parents_Name { get; set; }
        public string Parents_Num { get; set; }
        public string Sub_Num { get; set; }
        public string Sw_FileName { get; set; }
        public int Sw_FileSize { get; set; }
        public string PostIP { get; set; }
        public DateTime PostDate { get; set; }
        public DateTime ModifyDate { get; set; }
        public string ModifyIP { get; set; }
        public string Del { get; set; }
    }

    public interface ISw_Files_Lib
    {
        Task<int> Sw_Files_Data_Count();
        Task<int> Sw_View_Data_Count(string Parents_Name, string Parents_Num);
        Task Sw_Files_Date_Insert(Sw_Files_Entity Ct);
        Task<List<Sw_Files_Entity>> Sw_Files_Data_Index(string Parents_Name, string Parents_Num, string AptCode);
        Task<int> Sw_Files_Data_Index_Count(string Parents_Name, string Parents_Num, string AptCode);
        Task<List<Sw_Files_Entity>> Sw_Files_Data_Index_m(string Parents_Name, string Sub_Num, string AptCode);
        Task<int> Sw_Files_Data_Index_m_Count(string Parents_Name, string Sub_Num, string AptCode);
        Task<Sw_Files_Entity> Sw_Files_InsertView_Data_Index();
        Task<int> BeCount(string subAid, string Parents_Name);
        Task<int> BeCountSub(string subAid, string Parents_Num, string Parents_Name);
        Task FileRemove(string subAid, string Parents_Name, string AptCode);
        Task<Sw_Files_Entity> Sw_Files_Data_View(int Num);
        Task Sw_Files_Data_Modify(Sw_Files_Entity mm);
        Task Sw_Files_Date_Delete(int Num);
        //Task<List<Sw_Files_Entity>> Sw_Files_Data_Index();

        /// <summary>
        /// 파일업로드 aa 리스트
        /// </summary>        
        Task<List<Sw_Files_Entity>> Sw_Files_List(string Parents_Name, string Parents_Num);
    }

    public class Sw_Files_Lib : ISw_Files_Lib
    {
        private readonly IConfiguration _db;
        public Sw_Files_Lib(IConfiguration configuration)
        {
            this._db = configuration;
        }

        #region 중복된 파일명 뒤에 번호 붙이는 메서드 : GetFilePath
        /// <summary>
        /// GetFilePath : 파일명 뒤에 번호 붙이는 메서드
        /// </summary>
        /// <param name="strBaseDirTemp">경로(c:\MyFiles)</param>
        /// <param name="strFileNameTemp">Test.txt</param>
        /// <returns>Test(1).txt</returns>
        public string GetFilePath(
          string strBaseDirTemp, string strFileNameTemp)
        {
            string strName = //순수파일명 : Test
                Path.GetFileNameWithoutExtension(strFileNameTemp);
            string strExt =     //확장자 : .txt
                Path.GetExtension(strFileNameTemp);
            bool blnExists = true;
            int i = 0;
            while (blnExists)
            {
                if (File.Exists(
                  Path.Combine(strBaseDirTemp, strFileNameTemp)))
                {//Path.Combine(경로, 파일명) = 경로+파일명
                    i++;
                    strFileNameTemp =
                      strName + "(" + i + ")" + strExt;//Test(3).txt
                }
                else
                {
                    blnExists = false;
                }
            }
            return strFileNameTemp;
        }
        #endregion
        /// <summary>
        /// 파일업로드 마지막 번호 구하기
        /// </summary>
        /// <returns></returns>
        public async Task<int> Sw_Files_Data_Count()
        {
            using (var df = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                return await df.QuerySingleOrDefaultAsync<int>("Select Top 1 Num From Sw_Files Order By Num Desc");
            }
            
        }

        /// <summary>
        /// 파일업로드 파일 유무 구하기
        /// </summary>
        /// <returns></returns>
        public async Task<int> Sw_View_Data_Count(string Parents_Name, string Parents_Num)
        {
            using (var df = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                return await df.QuerySingleOrDefaultAsync<int>("Select Count(*) From Sw_Files Where Parents_Name = @Parents_Name And Parents_Num = @Parents_Num", new { Parents_Name, Parents_Num });
            }
            
        }

        /// <summary>
        /// 파일업로드 입력
        /// </summary>
        public async Task Sw_Files_Date_Insert(Sw_Files_Entity Ct)
        {
            using (var df = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                await df.ExecuteAsync("Insert into Sw_Files (AptCode, Parents_Name, Sub_Num, Parents_Num, Sw_FileName, Sw_FileSize, PostIP) Values (@AptCode, @Parents_Name, @Sub_Num, @Parents_Num, @Sw_FileName, @Sw_FileSize, @PostIP)", Ct);
            }
            
        }

        /// <summary>
        /// 파일업로드 리스트
        /// </summary>        
        public async Task<List<Sw_Files_Entity>> Sw_Files_Data_Index(string Parents_Name, string Parents_Num, string AptCode)
        {
            using (var df = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                var lst = await df.QueryAsync<Sw_Files_Entity>("Sw_Files_Index", new { Parents_Name, Parents_Num, AptCode }, commandType: CommandType.StoredProcedure);
                return lst.ToList();
            }            
        }

        /// <summary>
        /// 파일업로드 aa 리스트
        /// </summary>        
        public async Task<List<Sw_Files_Entity>> Sw_Files_List(string Parents_Name, string Parents_Num)
        {
            using (var df = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                var lst = await df.QueryAsync<Sw_Files_Entity>("Select * From Sw_Files Where Parents_Num = @Parents_Num And Parents_Name = @Parents_Name", new { Parents_Name, Parents_Num });
                return lst.ToList();
            }
        }

        /// <summary>
        /// 파일업로드 수
        /// </summary>
        public async Task<int> Sw_Files_Data_Index_Count(string Parents_Name, string Parents_Num, string AptCode)
        {
            using (var df = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                return await df.QuerySingleOrDefaultAsync<int>("Select Count(*) From Sw_Files Where Parents_Num = @Parents_Num And AptCode = @AptCode And Parents_Name = @Parents_Name", new { Parents_Name, Parents_Num, AptCode });
            }
            
        }

        /// <summary>
        /// 파일업로드 리스트
        /// </summary>        
        public async Task<List<Sw_Files_Entity>> Sw_Files_Data_Index_m(string Parents_Name, string Sub_Num, string AptCode)
        {
            using (var df = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                var lst = await df.QueryAsync<Sw_Files_Entity>("Select Num, AptCode, Parents_Name, Parents_Num, Sub_Num, Sw_FileName, Sw_FileSize, PostDate, PostIP, ModifyDate, ModifyIP, Del From Sw_Files Where Sub_Num = @Sub_Num And AptCode = @AptCode And Parents_Name = @Parents_Name", new { Parents_Name, Sub_Num, AptCode });
                return lst.ToList();
            }
            
        }

        /// <summary>
        /// 파일업로드 리스트 수
        /// </summary>        
        public async Task<int> Sw_Files_Data_Index_m_Count(string Parents_Name, string Sub_Num, string AptCode)
        {
            using (var df = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                return await df.QuerySingleOrDefaultAsync<int>("Select Count(*) From Sw_Files Where Sub_Num = @Sub_Num And AptCode = @AptCode And Parents_Name = @Parents_Name", new { Parents_Name, Sub_Num, AptCode });
            }
            
        }

        /// <summary>
        /// 파일 업로드된 모든 파일 리스트
        /// </summary>        
        public async Task<Sw_Files_Entity> Sw_Files_InsertView_Data_Index()
        {
            using (var df = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                string del = "A";
                return await df.QuerySingleOrDefaultAsync<Sw_Files_Entity>("Select Num, AptCode, Parents_Name, Parents_Num, Sub_Num, Sw_FileName, Sw_FileSize, PostDate, PostIP, ModifyDate, ModifyIP, Del From Sw_Files Where Del = " + del + " Order By Num Desc");
            }
            
        }

        /// <summary>
        /// 해당 식별코드에 입력된 수
        /// </summary>
        /// <param name="subAid"></param>
        /// <returns></returns>
        public async Task<int> BeCount(string subAid, string Parents_Name)
        {
            using (var df = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                return await df.QuerySingleOrDefaultAsync<int>("Select Count(*) From Sw_Files Where Parents_Name = @Parents_Name And Parents_Num = @subAid", new { subAid, Parents_Name });
            }
            
        }        

        /// <summary>
        /// 해당 식별코드에 입력된 수
        /// </summary>
        /// <param name="subAid"></param>
        /// <returns></returns>
        public async Task<int> BeCountSub(string subAid, string Parents_Num, string Parents_Name)
        {
            using (var df = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                return await df.QuerySingleOrDefaultAsync<int>("Select Count(*) From Sw_Files Where Parents_Name = @Parents_Name And Parents_Num = @Parents_Num And Sub_Num = @subAid", new { subAid, Parents_Num, Parents_Name });
            }
            
        }

        /// <summary>
        /// 식별코드로 모두 삭제
        /// </summary>
        /// <param name="subAid"></param>
        /// <param name="Parents_Name"></param>
        /// <param name="AptCode"></param>
        public async Task FileRemove(string Aid, string Parents_Name, string AptCode)
        {
            using (var df = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                await df.ExecuteAsync("Delete Sw_Files Where Num = @Aid And Parents_Name = @Parents_Name And AptCode = @AptCode", new { Aid, Parents_Name, AptCode });
            }
            
        }

        /// <summary>
        /// 파일업로드 상세보기
        /// </summary>        
        public async Task<Sw_Files_Entity> Sw_Files_Data_View(int Num)
        {
            using (var df = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                return await df.QuerySingleOrDefaultAsync<Sw_Files_Entity>("Sw_Files_View", new { Num }, commandType: CommandType.StoredProcedure);
            }
            
        }

        /// <summary>
        /// 파일업로드 수정하기
        /// </summary>
        public async Task Sw_Files_Data_Modify(Sw_Files_Entity mm)
        {
            using (var df = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                await df.ExecuteAsync("Sw_Files_Modify", new { mm.Sw_FileName, mm.Sw_FileSize, mm.Del, mm.ModifyDate, mm.ModifyIP, mm.Num }, commandType: CommandType.StoredProcedure);
            }
            
        }

        /// <summary>
        /// 파일업로드 삭제
        /// </summary>
        public async Task Sw_Files_Date_Delete(int Num)
        {
            using (var df = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                await df.ExecuteAsync("Delete Sw_Files Where Num = @Num", new { Num });
            }
            
        }        
    }
}
