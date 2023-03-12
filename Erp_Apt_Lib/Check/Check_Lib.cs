using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Erp_Apt_Lib.Check
{
    /// <summary>
    /// 점검일지 클레스 (완료)
    /// </summary>
    public class Check_List_Lib : ICheck_List_Lib 
    {
        private readonly IConfiguration _db;
        public Check_List_Lib(IConfiguration configuration)
        {
            this._db = configuration;
        }

        /// <summary>
        /// 점검일지 입력된 마지막 날 구하기
        /// </summary>
        /// <returns></returns>
        public async Task<string> CheckLIst_Date_PostDate()
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                return await dba.QuerySingleOrDefaultAsync<string>("Select Top 1 PostDate From Check_List");
            }
            
        }

        /// <summary>
        /// 점검일지 마지막 데이터 불러오기(단지별, 주기별)
        /// </summary>
        public async Task<Check_List_Entity> CheckList_Date_View_A(string Check_Cycle_Code, string AptCode)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                return await dba.QuerySingleOrDefaultAsync<Check_List_Entity>("Select Top 1  CheckID, AptCode, UserID, UserName, UserPost, UserDuty, Check_Object_Code, Check_Cycle_Code, Check_Year, Check_Month, Check_Day, Check_Hour, Check_Items_Code, Check_Effect_Code, Check_Details, FileName, FileSize, Del, PostDate, PostIP, Category, Sort, Complete From Check_List Where Check_Cycle_Code = @Check_Cycle_Code And AptCode = @AptCode Order By CheckID Desc", new { Check_Cycle_Code, AptCode });
            }
            
        }

        /// <summary>
        /// 점검일지 마지막 데이터 불러오기(단지별, 시설물별, 주기별)
        /// </summary>
        /// <param name="Check_Cycle_Code"></param>
        /// <param name="AptCode"></param>
        /// <returns></returns>
        public async Task<Check_List_Entity> CheckList_Date_View_B(string Check_Object_Code, string Check_Cycle_Code, string AptCode)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                return await dba.QuerySingleOrDefaultAsync<Check_List_Entity>("Select Top 1 CheckID, AptCode, UserID, UserName, UserPost, UserDuty, Check_Object_Code, Check_Cycle_Code, Check_Year, Check_Month, Check_Day, Check_Hour, Check_Items_Code, Check_Effect_Code, Check_Details, FileName, FileSize, Del, PostDate, PostIP, Category, Sort, Complete From Check_List Where Check_Object_Code = @Check_Object_Code And Check_Cycle_Code = @Check_Cycle_Code And AptCode = @AptCode Order By CheckID Desc", new { Check_Object_Code, Check_Cycle_Code, AptCode });
            }
            
        }

        /// <summary>
        /// 점검일지 점검결과(일) 여부 구하기
        /// </summary>
        public Check_List_Entity CheckList_Data_Effect_A(string Check_Items_Code, string Check_Cycle_Code, string Check_Year, string Check_Month, string Check_Day, string AptCode)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                return dba.QuerySingleOrDefault<Check_List_Entity>("Select Top 1 CheckID, AptCode, UserID, UserName, UserPost, UserDuty, Check_Object_Code, Check_Cycle_Code, Check_Year, Check_Month, Check_Day, Check_Hour, Check_Items_Code, Check_Effect_Code, Check_Details, FileName, FileSize, Del, PostDate, PostIP, Category, Sort, Complete From Check_List Where Check_Items_Code = @Check_Items_Code and Check_Cycle_Code = @Check_Cycle_Code And Check_Year = @Check_Year And Check_Month = @Check_Month And Check_Day = @Check_Day And AptCode = @AptCode", new { Check_Items_Code, Check_Cycle_Code, Check_Year, Check_Month, Check_Day, AptCode });
            }
            
        }

        /// <summary>
        /// 점검일지 점검결과(일) 존재 여부
        /// </summary>
        public int CheckList_Data_Effect_A_Count(string Check_Items_Code, string Check_Cycle_Code, string Check_Year, string Check_Month, string Check_Day, string AptCode)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                return dba.QuerySingleOrDefault<int>("Select Count(*) From Check_List Where Check_Items_Code = @Check_Items_Code and Check_Cycle_Code = @Check_Cycle_Code And Check_Year = @Check_Year And Check_Month = @Check_Month And Check_Day = @Check_Day And AptCode = @AptCode And Del = 'A'", new { Check_Items_Code, Check_Cycle_Code, Check_Year, Check_Month, Check_Day, AptCode });
            }
            
        }


        /// <summary>
        /// 각 점검일지 점검사항 입력 여부(주간) 구하기
        /// </summary>
        /// <returns></returns>        
        public async Task<Check_List_Entity> CheckList_Data_Items_Effect_Week(string Check_Items_Code, string Check_Year, int Check_Month, int Check_Day, string Total_Day, string AptCode)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                return await dba.QuerySingleOrDefaultAsync<Check_List_Entity>("Check_Effect_Week_View", new { Check_Items_Code, Check_Year, Check_Month, Check_Day, Total_Day, AptCode }, commandType: CommandType.StoredProcedure);
            }
            
        }

        /// <summary>
        /// 주간 입력 여부 확인
        /// </summary>
        /// <returns></returns>
        public async Task<int> CheckList_Data_Items_Effect_Week_Count(string Check_Items_Code, string Check_Year, int Check_Month, int Check_Day, string Total_Day, string AptCode)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                return await dba.QuerySingleOrDefaultAsync<int>("Select Count(*) From Check_List	Where Check_Items_Code = @Check_Items_Code And Check_Year = @Check_Year And ((Check_Month = @Check_Month) or ((@Check_Month - Check_Month) = 1)) And ((Select DateDiff(dd, '2013-01-06', @Total_Day)) - (Select DateDiff(dd, '2013-01-06', PostDate)) < @Check_Day) And AptCode = @AptCode and Del = 'A'", new { Check_Items_Code, Check_Year, Check_Month, Check_Day, Total_Day, AptCode });
            }
            
        }

        /// <summary>
        /// 각 점검일지 점검사항 입력 여부(월간) 구하기
        /// </summary>
        /// <returns></returns>        
        public async Task<Check_List_Entity> CheckList_Data_Items_Effect_Month(string Check_Items_Code, string Check_Year, int Check_Month, string AptCode)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                return await dba.QuerySingleOrDefaultAsync<Check_List_Entity>("Check_Effect_Month_View", new { Check_Items_Code, Check_Year, Check_Month, AptCode }, commandType: CommandType.StoredProcedure);
            }
            
        }

        /// <summary>
        /// 각 점검일지 점검사항 입력 여부(월간) 구하기
        /// </summary>
        /// <returns></returns>        
        public async Task<int> CheckList_Data_Items_Effect_Month_be(string Check_Items_Code, string Check_Year, int Check_Month, string AptCode)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                return await dba.QuerySingleOrDefaultAsync<int>("Select Count(*) From Check_List	Where Check_Items_Code = @Check_Items_Code And Check_Year = @Check_Year And Check_Month = @Check_Month And AptCode = @AptCode and Del = 'A'	Order By CheckID Desc", new { Check_Items_Code, Check_Year, Check_Month, AptCode });
            }
            
        }

        /// <summary>
        /// 월간 입력 여부 확인
        /// </summary>
        /// <returns></returns>
        public async Task<int> CheckList_Data_Items_Effect_Month_Count(string Check_Items_Code, string Check_Year, int Check_Month, string AptCode)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                return await dba.QuerySingleOrDefaultAsync<int>("Select Count(*) From Check_List	Where Check_Items_Code = @Check_Items_Code And Check_Year = @Check_Year And Check_Month = @Check_Month And AptCode = @AptCode and Del = 'A'", new { Check_Items_Code, Check_Year, Check_Month, AptCode });
            }
            
        }

        /// <summary>
        /// 각 점검일지 점검사항 입력 여부(분기) 구하기
        /// </summary>
        /// <returns></returns>        
        public async Task<Check_List_Entity> CheckList_Data_Items_Effect_Quarter(string Check_Items_Code, string Check_Year, int Check_Month, string AptCode)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                return await dba.QuerySingleOrDefaultAsync<Check_List_Entity>("Check_Effect_Quarter_View", new { Check_Items_Code, Check_Year, Check_Month, AptCode }, commandType: CommandType.StoredProcedure);
            }
            
        }

        /// <summary>
        /// 분기 입력 여부 확인
        /// </summary>
        /// <returns></returns>
        public async Task<int> CheckList_Data_Items_Effect_Quarter_Count(string Check_Items_Code, string Check_Year, int Check_Month, string AptCode)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                return await dba.QuerySingleOrDefaultAsync<int>("Select Count(*) From Check_List	Where Check_Items_Code = @Check_Items_Code And Check_Year = @Check_Year And Check_Month <= @Check_Month And (Check_Month % 3) <= 3 and (@Check_Month - Check_Month) < 3 And AptCode = @AptCode and Del = 'A'", new { Check_Items_Code, Check_Year, Check_Month, AptCode });
            }
            
        }

        /// <summary>
        /// 각 점검일지 점검사항 입력 여부(반기) 구하기
        /// </summary>
        /// <returns></returns>        
        public async Task<Check_List_Entity> CheckList_Data_Items_Effect_Half(string Check_Items_Code, string Check_Year, int Check_Month, string AptCode)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                return await dba.QuerySingleOrDefaultAsync<Check_List_Entity>("Check_Effect_Half_View", new { Check_Items_Code, Check_Year, Check_Month, AptCode }, commandType: CommandType.StoredProcedure);
            }
            
        }

        /// <summary>
        /// 분기 입력 여부 확인
        /// </summary>
        /// <returns></returns>
        public async Task<int> CheckList_Data_Items_Effect_Half_Count(string Check_Items_Code, string Check_Year, int Check_Month, string AptCode)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                return await dba.QuerySingleOrDefaultAsync<int>("Select Count(*) From Check_List	Where Check_Items_Code = @Check_Items_Code And Check_Year = @Check_Year And Check_Month <= @Check_Month And (Check_Month - 6) <= 6 and (@Check_Month - Check_Month) < 6 And AptCode = @AptCode and Del = 'A'", new { Check_Items_Code, Check_Year, Check_Month, AptCode });
            }
            
        }

        /// <summary>
        /// 각 점검일지 점검사항 입력 여부(법정) 구하기
        /// </summary>
        /// <returns></returns>        
        public async Task<Check_List_Entity> CheckList_Data_Items_Effect_Law(string Check_Items_Code, string Check_Year, string AptCode)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                return await dba.QuerySingleOrDefaultAsync<Check_List_Entity>("Check_Effect_Law_View", new { Check_Items_Code, Check_Year, AptCode }, commandType: CommandType.StoredProcedure);
            }
            
        }

        /// <summary>
        /// 법정 입력 여부 확인
        /// </summary>
        /// <returns></returns>
        public async Task<int> CheckList_Data_Items_Effect_Law_Count(string Check_Items_Code, string Check_Year, string AptCode)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                return await dba.QuerySingleOrDefaultAsync<int>("Select Count(*) From Check_List	Where Check_Items_Code = @Check_Items_Code And Check_Year = @Check_Year And AptCode = @AptCode and Del = 'A'", new { Check_Items_Code, Check_Year, AptCode });
            }
            
        }

        /// <summary>
        /// 각 점검일지 점검사항 입력 여부 구하기(메서드)
        /// </summary>
        /// <returns></returns>        
        public Check_List_Entity CheckList_Data_Input_A(string Check_Object_Code, string Check_Cycle_Code, string Check_Year, string Check_Month, string Check_Day, string AptCode)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                return dba.QuerySingleOrDefault<Check_List_Entity>("Select Top 1 CheckID, AptCode, UserID, UserName, UserPost, UserDuty, Check_Object_Code, Check_Cycle_Code, Check_Year, Check_Month, Check_Day, Check_Hour, Check_Items_Code, Check_Effect_Code, Check_Details, FileName, FileSize, Del, PostDate, PostIP, Category, Sort, Complete From Check_List Where Check_Object_Code = @Check_Object_Code and Check_Cycle_Code = @Check_Cycle_Code And Check_Year = @Check_Year And Check_Month = @Check_Month And Check_Day = @Check_Day And AptCode = @AptCode Order by CheckID Desc", new { Check_Object_Code, Check_Cycle_Code, Check_Year, Check_Month, Check_Day, AptCode });
            }
            
        }

        /// <summary>
        /// 각 점검일지 점검대상 전체 입력된 수 구하기(년+월+일 단위로 검색)
        /// </summary>
        /// <returns></returns>
        public int CheckList_Data_CardView_Year_Month_Day(string Check_Object_Code, string Check_Cycle_Code, string Check_Year, string Check_Month, string Check_Day, string AptCode)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                return dba.QuerySingleOrDefault<int>("Select count(*) From Check_List Where Check_Object_Code = @Check_Object_Code and Check_Cycle_Code = @Check_Cycle_Code And Check_Year = @Check_Year And Check_Month = @Check_Month And Check_Day = @Check_Day And AptCode = @AptCode", new { Check_Object_Code, Check_Cycle_Code, Check_Year, Check_Month, Check_Day, AptCode });
            }
            
        }

        /// <summary>
        /// 각 점검일지 점검대상 전체 입력된 수 구하기(년+월 단위로 검색) 주별 점검 입력된 수 구하기
        /// </summary>
        /// <returns></returns>
        public int CheckList_Data_CardView_Week(string Check_Object_Code, string Check_Cycle_Code, string Check_Year, int Check_Month, int Check_Day, string Total_Day, string AptCode)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                return dba.QuerySingleOrDefault<int>("Check_Count_Week_Insert", new { Check_Object_Code, Check_Cycle_Code, Check_Year, Check_Month, Check_Day, Total_Day, AptCode }, commandType: CommandType.StoredProcedure);
            }
            
        }

        /// <summary>
        /// 각 점검일지 점검대상 전체 입력된 수 구하기(년+월 단위로 검색) 월별 점검 입력된 수 구하기
        /// </summary>
        /// <returns></returns>
        public int CheckList_Data_CardView_Year_Month(string Check_Object_Code, string Check_Cycle_Code, string Check_Year, string Check_Month, string AptCode)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                return dba.QuerySingleOrDefault<int>("Select count(*) From Check_List Where Check_Object_Code = @Check_Object_Code and Check_Cycle_Code = @Check_Cycle_Code And Check_Year = @Check_Year And Check_Month = @Check_Month And AptCode = @AptCode", new { Check_Object_Code, Check_Cycle_Code, Check_Year, Check_Month, AptCode });
            }
            
        }

        /// <summary>
        /// 각 점검일지 점검대상 전체 입력된 수 구하기(년+월 단위로 검색) 분기별 점검 입력된 수 구하기
        /// </summary>
        /// <returns></returns>
        public int CheckList_Data_CardView_Quarter(string Check_Object_Code, string Check_Cycle_Code, string Check_Year, int Check_Month, string AptCode)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                return dba.QuerySingleOrDefault<int>("Check_Count_Quarter_Insert", new { Check_Object_Code, Check_Cycle_Code, Check_Year, Check_Month, AptCode }, commandType: CommandType.StoredProcedure);
            }
            
        }

        /// <summary>
        /// 각 점검일지 점검대상 전체 입력된 수 구하기(년+월 단위로 검색) 반기별 점검 입력된 수 구하기
        /// </summary>
        /// <returns></returns>
        public int CheckList_Data_CardView_Half(string Check_Object_Code, string Check_Cycle_Code, string Check_Year, int Check_Month, string AptCode)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                return dba.QuerySingleOrDefault<int>("Check_Count_Half_Insert", new { Check_Object_Code, Check_Cycle_Code, Check_Year, Check_Month, AptCode }, commandType: CommandType.StoredProcedure);
            }
            
        }

        /// <summary>
        /// 각 점검일지 점검대상 전체 입력된 수 구하기(년 단위로 검색) 법정점검 수 구하기
        /// </summary>
        /// <returns></returns>
        public int CheckList_Data_CardView_Year(string Check_Object_Code, string Check_Cycle_Code, string Check_Year, string AptCode)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                return dba.QuerySingleOrDefault<int>("Select count(*) From Check_List Where Check_Object_Code = @Check_Object_Code and Check_Cycle_Code = @Check_Cycle_Code  And Check_Year = @Check_Year And AptCode = @AptCode", new { Check_Object_Code, Check_Cycle_Code, Check_Year, AptCode });
            }
            
        }

        /// <summary>
        /// 점검일지 마지막 번호 구하기
        /// </summary>
        /// <returns></returns>
        public async Task<int> CheckList_Data_Count()
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                return await dba.QuerySingleOrDefaultAsync<int>("Select Top 1 CheckID From Check_List Order By CheckID Desc");
            }
            
        }

        /// <summary>
        /// 점검일지 입력
        /// </summary>
        public async Task<Check_List_Entity> CheckList_Date_Insert(Check_List_Entity Ct)
        {
            var Sql = "Insert Check_List (CheckAid, AptCode, UserID, UserName, UserPost, UserDuty, Check_Input_Code, Check_Object_Code, Check_Cycle_Code, Check_Year, Check_Month, Check_Day, Check_Date, Check_Hour, Check_Items_Code, Check_Effect_Code, Check_Details, FileName, FileSize, PostIP, Category, Sort) Values (@CheckAid, @AptCode, @UserID, @UserName, @UserPost, @UserDuty, @Check_Input_Code, @Check_Object_Code, @Check_Cycle_Code, @Check_Year, @Check_Month, @Check_Day, @Check_Date, @Check_Hour, @Check_Items_Code, @Check_Effect_Code, @Check_Details, @FileName, @FileSize, @PostIP, @Category, @Sort)";
            
            using (var dba = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                await dba.ExecuteAsync(Sql, Ct);
                return Ct;
            }
            
        }


        /// <summary>
        /// 점검일지(현재 같은 아이템 입력 여부 확인) 정보
        /// </summary>        
        public async Task<Check_List_Entity> CheckList_Data_Items_Effect_A(string Check_Object_Code, string Check_Items_Code, string Check_Cycle_Code, string Check_Year, string Check_Month, string Check_Day, string AptCode)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                return await dba.QuerySingleOrDefaultAsync<Check_List_Entity>(@"Select Top 1 CheckID, CheckAid, AptCode, UserID, UserName, UserPost, UserDuty, Check_Object_Code, Check_Cycle_Code, Check_Year, Check_Month, Check_Day, Check_Hour, Check_Items_Code, Check_Effect_Code, Check_Details, FileName, FileSize, Del, PostDate, PostIP, Category, Sort, Complete From Check_List Where Check_Object_Code = @Check_Object_Code And Check_Items_Code = @Check_Items_Code And Check_Cycle_Code = @Check_Cycle_Code And Check_Year = @Check_Year And Check_Month = @Check_Month And Check_Day = @Check_Day And AptCode = @AptCode Order By CheckID Desc", new { Check_Object_Code, Check_Items_Code, Check_Cycle_Code, Check_Year, Check_Month, Check_Day, AptCode });
            }
            
        }
        /// <summary>
        /// 해당 공동주택에서 검검사항을 해당 날짜에 존재여부
        /// </summary>
        public async Task<int> CheckList_Items_Efect_Being(string Check_Object_Code, string Check_Items_Code, string Check_Cycle_Code, string Check_Year, string Check_Month, string Check_Day, string AptCode)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                return await dba.QuerySingleOrDefaultAsync<int>("Select Count(*) From Check_List Where Check_Object_Code = @Check_Object_Code And Check_Items_Code = @Check_Items_Code And Check_Cycle_Code = @Check_Cycle_Code And Check_Year = @Check_Year And Check_Month = @Check_Month And Check_Day = @Check_Day And AptCode = @AptCode And Del = 'A'", new { Check_Object_Code, Check_Items_Code, Check_Cycle_Code, Check_Year, Check_Month, Check_Day, AptCode });
            }
            
        }

        /// <summary>
        /// 점검일지 마지막 데이터 목록
        /// </summary>
        public async Task<List<Check_List_Entity>> GetCheckList_List_Index(string Check_Object_Code, string Check_Cycle_Code, string Check_Year, string Check_Month, string Check_Day, string AptCode)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                var lst = await dba.QueryAsync<Check_List_Entity>("Check_List_Index", new { Check_Object_Code, Check_Cycle_Code, Check_Year, Check_Month, Check_Day, AptCode }, commandType: CommandType.StoredProcedure);
                return lst.ToList();
            }
            
        }

        /// <summary>
        /// 점검일지 마지막 데이터 목록(2021)
        /// </summary>
        public async Task<List<Check_List_Entity>> GetCheckList_List_Index_new(string Check_Object_Code, string Check_Cycle_Code, string Check_Year, string Check_Month, string Check_Day, string AptCode)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                var lst = await dba.QueryAsync<Check_List_Entity>("Select a.CheckID, a.AptCode, a.Category, a.Check_Cycle_Code, a.Check_Date, a.Check_Day, a.Check_Details, a.Check_Effect_Code, a.Check_Hour, a.Check_Input_Code, a.Check_Items_Code, a.Check_Month, a.Check_Object_Code, a.Check_Year, a.CheckAid, a.CheckID, a.Complete, a.Del, a.PostDate, a.PostIP, a.UserDuty, a.Sort, a.UserID, a.UserName, a.UserPost, a.FileSize, a.Files_Count, b.Check_Cycle_Name, c.Check_Object_Name, d.Check_Items, e.Check_Effect_Name From Check_List as a Join Check_Cycle as b on a.Check_Cycle_Code = b.Check_Cycle_Code Join Check_Object as c on a.Check_Object_Code = c.Check_Object_Code Join Check_Items as d on a.Check_Items_Code = d.Check_Items_Code Join Check_Effect as e on a.Check_Effect_Code = e.Check_Effect_Code Where a.Check_Cycle_Code = @Check_Cycle_Code and a.Check_Object_Code = @Check_Object_Code And a.Check_Year = @Check_Year And a.Check_Month = @Check_Month And a.Check_Day = @Check_Day And a.AptCode = @AptCode and a.Del = 'A' Order By a.CheckID Desc", new { Check_Object_Code, Check_Cycle_Code, Check_Year, Check_Month, Check_Day, AptCode });
                return lst.ToList();
            }

        }

        /// <summary>
        /// 점검일지 마지막 데이터 목록(2021) Page
        /// </summary>
        public async Task<List<Check_List_Entity>> GetCheckList_List_Index_Page(int Page, string Check_Object_Code, string Check_Cycle_Code, string Check_Year, string Check_Month, string Check_Day, string AptCode)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                var lst = await dba.QueryAsync<Check_List_Entity>("Select Top 10 a.CheckID, a.AptCode, a.Category, a.Check_Cycle_Code, a.Check_Date, a.Check_Day, a.Check_Details, a.Check_Effect_Code, a.Check_Hour, a.Check_Input_Code, a.Check_Items_Code, a.Check_Month, a.Check_Object_Code, a.Check_Year, a.CheckAid, a.CheckID, a.Complete, a.Del, a.PostDate, a.PostIP, a.UserDuty, a.Sort, a.UserID, a.UserName, a.UserPost, a.FileSize, a.Files_Count, b.Check_Cycle_Name, c.Check_Object_Name, d.Check_Items, e.Check_Effect_Name From Check_List as a Join Check_Cycle as b on a.Check_Cycle_Code = b.Check_Cycle_Code Join Check_Object as c on a.Check_Object_Code = c.Check_Object_Code Join Check_Items as d on a.Check_Items_Code = d.Check_Items_Code Join Check_Effect as e on a.Check_Effect_Code = e.Check_Effect_Code Where a.CheckID Not In (Select Top (10 * @Page) a.CheckID From Check_List as a Join Check_Cycle as b on a.Check_Cycle_Code = b.Check_Cycle_Code Join Check_Object as c on a.Check_Object_Code = c.Check_Object_Code Join Check_Items as d on a.Check_Items_Code = d.Check_Items_Code Join Check_Effect as e on a.Check_Effect_Code = e.Check_Effect_Code Where a.Check_Cycle_Code = @Check_Cycle_Code and a.Check_Object_Code = @Check_Object_Code And a.Check_Year = @Check_Year And a.Check_Month = @Check_Month And a.Check_Day = @Check_Day And a.AptCode = @AptCode and a.Del = 'A'  Order By d.CheckItemsID Desc) and a.Check_Cycle_Code = @Check_Cycle_Code and a.Check_Object_Code = @Check_Object_Code And a.Check_Year = @Check_Year And a.Check_Month = @Check_Month And a.Check_Day = @Check_Day And a.AptCode = @AptCode and a.Del = 'A'  Order By d.CheckItemsID Desc", new { Page, Check_Object_Code, Check_Cycle_Code, Check_Year, Check_Month, Check_Day, AptCode });
                return lst.ToList();
            }

        }

        /// <summary>
        /// 점검일지 마지막 데이터 목록 수
        /// </summary>
        public async Task<int> GetCheckList_List_Index_Count_Page(string Check_Object_Code, string Check_Cycle_Code, string Check_Year, string Check_Month, string Check_Day, string AptCode)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                return await dba.QuerySingleOrDefaultAsync<int>("Select Count(*) From Check_List as a Join Check_Cycle as b on a.Check_Cycle_Code = b.Check_Cycle_Code Join Check_Object as c on a.Check_Object_Code = c.Check_Object_Code Join Check_Items as d on a.Check_Items_Code = d.Check_Items_Code Join Check_Effect as e on a.Check_Effect_Code = e.Check_Effect_Code Where a.Check_Cycle_Code = @Check_Cycle_Code and a.Check_Object_Code = @Check_Object_Code And a.Check_Year = @Check_Year And a.Check_Month = @Check_Month And a.Check_Day = @Check_Day And a.AptCode = @AptCode and a.Del = 'A'", new { Check_Object_Code, Check_Cycle_Code, Check_Year, Check_Month, Check_Day, AptCode });
            }

        }

        /// <summary>
        /// 점검일지 마지막 데이터 목록 수
        /// </summary>
        public async Task<int> GetCheckList_List_Index_Count(string Check_Object_Code, string Check_Cycle_Code, string Check_Year, string Check_Month, string Check_Day, string AptCode)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                return await dba.QuerySingleOrDefaultAsync<int>("Select Count(*) From Check_List Where Check_Cycle_Code = @Check_Cycle_Code and Check_Object_Code = @Check_Object_Code And Check_Year = @Check_Year And Check_Month = @Check_Month And Check_Day = @Check_Day And AptCode = @AptCode and Del = 'A'", new { Check_Object_Code, Check_Cycle_Code, Check_Year, Check_Month, Check_Day, AptCode });
            }
            
        }

        /// <summary>
        /// 파일 카운트 수정(2021)
        /// </summary>
        /// <param name="CheckID"></param>
        /// <param name="FileSize"></param>
        /// <param name="Division"></param>
        /// <returns></returns>
        public async Task FileCountAdd(int CheckID, int FileSize, string Division)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                if (Division == "A")
                {
                    await dba.ExecuteAsync("Update Check_List Set FileSize = FileSize + 1 Where CheckID = @CheckID", new { CheckID, FileSize });
                }
                else if (Division == "B")
                {
                    await dba.ExecuteAsync("Update Check_List Set FileSize = FileSize - 1 Where CheckID = @CheckID", new { CheckID, FileSize });
                }
            }
        }

        /// <summary>
        /// 점검일지(주간 점검) 리스트
        /// </summary>        
        public async Task<List<Check_List_Entity>> CheckList_Data_Week_Index(string Check_Object_Code, string Check_Cycle_Code, string Check_Year, int Check_Month, int Check_Day, string Total_Day, string AptCode)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                var lst = await dba.QueryAsync<Check_List_Entity>("Check_List_Week_Index", new { Check_Object_Code, Check_Cycle_Code, Check_Year, Check_Month, Check_Day, Total_Day, AptCode }, commandType: CommandType.StoredProcedure);
                return lst.ToList();
            }
            
        }

        /// <summary>
        /// 점검일지(주간 점검) 리스트
        /// </summary>        
        public async Task<List<Check_List_Entity>> CheckList_Data_Week_Index_New(string Check_Object_Code, string Check_Cycle_Code, string Check_Year, string Check_Month, string Check_Day, string AptCode)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                var lst = await dba.QueryAsync<Check_List_Entity>("Select CheckID, CheckAid, AptCode, UserID, UserName, UserPost, UserDuty, Check_Input_Code, Check_Object_Code, Check_Cycle_Code, Check_Year, Check_Month, Check_Day, Check_Hour, Check_Date, Check_Items_Code, Check_Effect_Code, Check_Details, FileName, FileSize, Del, PostDate, PostIP, ModifyDate, ModifyIP, Category, Sort, Complete From Check_List Where Check_Object_Code = @Check_Object_Code And Check_Cycle_Code = @Check_Cycle_Code And Check_Year = @Check_Year And Check_Month = @Check_Month And Check_Day = @Check_Day And AptCode = @AptCode", new { Check_Object_Code, Check_Cycle_Code, Check_Year, Check_Month, Check_Day, AptCode });
                return lst.ToList();
            }
            
        }

        ////////////////////////////////////////////

        /// <summary>
        /// 점검일지(월간 점검) 리스트
        /// </summary>        
        public async Task<List<Check_List_Entity>> CheckList_Data_Month_Index(string Check_Object_Code, string Check_Cycle_Code, string Check_Year, int Check_Month, string AptCode)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                var lst = await dba.QueryAsync<Check_List_Entity>("Check_List_Month_Index", new { Check_Object_Code, Check_Cycle_Code, Check_Year, Check_Month, AptCode }, commandType: CommandType.StoredProcedure);
                return lst.ToList();
            }
            
        }

        /// <summary>
        /// 점검일지(분기 점검) 리스트
        /// </summary>        
        public async Task<List<Check_List_Entity>> CheckList_Data_Quarter_Index(string Check_Object_Code, string Check_Cycle_Code, string Check_Year, int Check_Month, string AptCode)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                var lst = await dba.QueryAsync<Check_List_Entity>("Check_List_Quarter_Index", new { Check_Object_Code, Check_Cycle_Code, Check_Year, Check_Month, AptCode }, commandType: CommandType.StoredProcedure);
                return lst.ToList();
            }
            
        }

        /// <summary>
        /// 점검일지(반기 점검) 리스트
        /// </summary>        
        public async Task<List<Check_List_Entity>> CheckList_Data_Half_Index(string Check_Object_Code, string Check_Cycle_Code, string Check_Year, int Check_Month, string AptCode)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                var lst = await dba.QueryAsync<Check_List_Entity>("Check_List_Half_Index", new { Check_Object_Code, Check_Cycle_Code, Check_Year, Check_Month, AptCode }, commandType: CommandType.StoredProcedure);
                return lst.ToList();
            }
            
        }

        /// <summary>
        /// 점검일지(법정 점검) 리스트
        /// </summary>        
        public async Task<List<Check_List_Entity>> CheckList_Data_Law_Index(string Check_Object_Code, string Check_Cycle_Code, string Check_Year, string AptCode)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                var lst = await dba.QueryAsync<Check_List_Entity>("Check_List_Law_Index", new { Check_Object_Code, Check_Cycle_Code, Check_Year, AptCode }, commandType: CommandType.StoredProcedure);
                return lst.ToList();
            }
            
        }

        /// <summary>
        /// 점검일지 상세보기
        /// </summary>
        public async Task<Check_List_Entity> CheckList_Data_View(int Num)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                return await dba.QuerySingleOrDefaultAsync<Check_List_Entity>("Check_List_View", new { Num }, commandType: CommandType.StoredProcedure);
            }
            
        }

        /// <summary>
        /// 점검일지 수정하기
        /// </summary>
        public async Task CheckList_Data_Modify(Check_List_Entity mm)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                await dba.ExecuteAsync("Check_List_Modify", new { mm.UserID, mm.UserName, mm.UserPost, mm.UserDuty, mm.Check_Object_Code, mm.Check_Cycle_Code, mm.Check_Year, mm.Check_Month, mm.Check_Day, mm.Check_Hour, mm.Check_Effect_Code, mm.Check_Details, mm.FileName, mm.FileSize, mm.Del, mm.ModifyIP, mm.Category, mm.Sort, mm.CheckID }, commandType: CommandType.StoredProcedure);
            }
            
        }

        /// <summary>
        /// 점검일지 삭제
        /// </summary>
        public async Task CheckList_Date_Delete(int CheckID)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                await dba.ExecuteAsync("Check_List_Delete", new { CheckID }, commandType: CommandType.StoredProcedure);
            }
        }

        /// <summary>
        /// 점검일지 삭제
        /// </summary>
        public async Task CheckList_Date_Remove(int CheckID)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                await dba.ExecuteAsync("Delete Check_List Where CheckID = @CheckID", new { CheckID });
            }
            
        }

        /// <summary>
        /// 점검내용 전체 삭제
        /// </summary>
        public async Task CheckList_Date_Remove_All(int Check_Input_ID)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                await dba.ExecuteAsync("Delete Check_List Where Check_Input_Code = @CheckID", new { Check_Input_ID });
            }

        }


        public async Task ChdeckList_Complete(int CheckID)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                await dba.ExecuteAsync("Updete Check_List Set Complete = 'B' Where CheckID = @CheckID", new { CheckID });
            }
            
        }

        /// <summary>
        /// 점검자 명 가져오기
        /// </summary>
        public async Task<string> CheckList_UserName(string Check_Object_Code, string Check_Cycle_Code, string Check_Year, string Check_Month, string Check_Day, string AptCode)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                return await dba.QuerySingleOrDefaultAsync<string>("Select UserName From Check_List Where Check_Object_Code =@Check_Object_Code and Check_Cycle_Code = @Check_Cycle_Code And Check_Year = @Check_Year And Check_Month = @Check_Month And Check_Day = @Check_Day And AptCode = @AptCode", new { Check_Object_Code, Check_Cycle_Code, Check_Year, Check_Month, Check_Day, AptCode });
            }
           
        }

        /// <summary>
        /// 입력 마지막 식별코드
        /// </summary>
        public async Task<string> CheckList_Being_New(string Check_Object_Code, string Check_Cycle_Code, string Check_Year, string Check_Month, string Check_Day, string AptCode)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                return await dba.QuerySingleOrDefaultAsync<string>("Select Top 1 CheckID From Check_List Where Check_Object_Code =@Check_Object_Code and Check_Cycle_Code = @Check_Cycle_Code And Check_Year = @Check_Year And Check_Month = @Check_Month And Check_Day = @Check_Day And AptCode = @AptCode", new { Check_Object_Code, Check_Cycle_Code, Check_Year, Check_Month, Check_Day, AptCode });
            }
            
        }

        /// <summary>
        /// 해당일 점검 존재 여부
        /// </summary>
        public async Task<int> CheckList_Being_Count(string Check_Object_Code, string Check_Cycle_Code, string Check_Year, string Check_Month, string Check_Day, string AptCode)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                return await dba.QuerySingleOrDefaultAsync<int>("Select Count(*) From Check_List Where Check_Object_Code =@Check_Object_Code and Check_Cycle_Code = @Check_Cycle_Code And Check_Year = @Check_Year And Check_Month = @Check_Month And Check_Day = @Check_Day And AptCode = @AptCode", new { Check_Object_Code, Check_Cycle_Code, Check_Year, Check_Month, Check_Day, AptCode });
            }
        }

        /// <summary>
        /// 해당 시설물에 등록된 파일 수 더하기
        /// </summary>
        public async Task Files_Count_Add(int Aid, string Division)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                if (Division == "A")
                {
                    await dba.ExecuteAsync("Update Check_List Set Files_Count = Files_Count + 1 Where CheckID = @Aid", new { Aid }); //추가
                }
                else if(Division == "B")
                {
                    await dba.ExecuteAsync("Update Check_List Set Files_Count = Files_Count - 1 Where CheckID = @Aid", new { Aid }); //삭제
                }
            }
        }
    }

    /// <summary>
    /// 점검표 정보 클래스(완료)
    /// </summary>
    public class Check_Card_Lib : ICheck_Card_Lib
    {
        private readonly IConfiguration _db;
        public Check_Card_Lib(IConfiguration configuration)
        {
            this._db = configuration;
        }

        /// <summary>
        /// 점검표 마지막 번호 구하기
        /// </summary>
        /// <returns></returns>
        public async Task<int> CheckCard_Data_Count()
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                return await dba.QuerySingleOrDefaultAsync<int>("Select Top 1 CheckCardID From Check_Card Order By CheckCardID Desc");
            }
            
        }

        /// <summary>
        /// 점검표 입력
        /// </summary>
        public async Task<Check_Card_Entity> CheckCard_Date_Insert(Check_Card_Entity Ct)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                await dba.ExecuteAsync("Insert Into Check_Card (AptCode, Check_Card_Code, Check_Object_Code, Check_Cycle_Code, Check_Items_Code, Check_Card_Name, Check_Card_Details, PostIP, Del, Category, Sort) Values (@AptCode, @Check_Card_Code, @Check_Object_Code, @Check_Cycle_Code, @Check_Items_Code, @Check_Card_Name, @Check_Card_Details, @PostIP, @Del, @Category, @Sort)", Ct);
                return Ct;
            }
            
        }

        /// <summary>
        /// 점검표 리스트
        /// </summary>        
        public async Task<List<Check_Card_Entity>> CheckCard_Data_Index(string Check_Object_Code, string Check_Cycle_Code, string AptCode)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                var lst = await dba.QueryAsync<Check_Card_Entity>("Check_Card_Index", new { Check_Object_Code, Check_Cycle_Code, AptCode }, commandType: CommandType.StoredProcedure);
                return lst.ToList();
            }
            
        }

        /// <summary>
        /// 아직 점검하지 않은 점검대상 리스트
        /// </summary>        
        public async Task<List<Check_Card_Entity>> CheckCard_InsertView_Data_Index(string AptCode)
        {

            string del = "A";
            using (var dba = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                var lst = await dba.QueryAsync<Check_Card_Entity>("Select * From Check_Card Where AptCode = @AptCode and Del = '" + del + "' Order By Check_Cycle_Code Asc", new { AptCode });
                return lst.ToList();
            }
            
        }

        /// <summary>
        /// 점검대상 리스트(2022년)
        /// </summary>        
        public async Task<List<Check_Card_Entity>> CheckCard_Index(int Page)
        {
            using var dba = new SqlConnection(_db.GetConnectionString("Ayoung"));
            var lst = await dba.QueryAsync<Check_Card_Entity>("Select Top 15 * From Check_Card Where CheckCardID Not In(Select Top (15 * @Page) CheckCardID From Check_Card Where Del = 'A' Order by CheckCardID Desc) And Del = 'A' Order by CheckCardID Desc", new { Page });
            return lst.ToList();
        }

        /// <summary>
        /// 점검표 리스트(공동주택)(2022년)
        /// </summary>        
        public async Task<List<Check_Card_Entity>> CheckCard_Apt(string AptCode)
        {
            using var dba = new SqlConnection(_db.GetConnectionString("Ayoung"));
            var lst = await dba.QueryAsync<Check_Card_Entity>("Select * From Check_Card Where AptCode = @AptCode And Del = 'A' Order by Check_Cycle_Code  Asc,  CheckCardID Asc", new { AptCode });
            return lst.ToList();
        }

        /// <summary>
        /// 점검표 리스트(공동주택)(2022년)
        /// </summary>        
        public async Task<List<Check_Card_Entity>> CheckCard_Index_Apt(int Page, string AptCode)
        {
            using var dba = new SqlConnection(_db.GetConnectionString("Ayoung"));
            var lst = await dba.QueryAsync<Check_Card_Entity>("Select Top 15 * From Check_Card Where CheckCardID Not In(Select Top (15 * @Page) CheckCardID From Check_Card Where AptCode = @AptCode And Del = 'A' Order by CheckCardID Desc) And AptCode = @AptCode And Del = 'A' Order by CheckCardID Desc", new { Page, AptCode });
            return lst.ToList();
        }

        /// <summary>
        /// 점검표 리스트 수 (공동주택) 2022년
        /// </summary>
        public async Task<int> CheckCard_Index_Apt_Count(string AptCode)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                return await dba.QuerySingleOrDefaultAsync<int>("Select Count(*) From Check_Card Where AptCode = @AptCode", new { AptCode });
            }
        }

        /// <summary>
        /// 점검표 상세보기
        /// </summary>        
        public async Task<Check_Card_Entity> CheckCard_Data_View(int Num)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                return await dba.QuerySingleOrDefaultAsync<Check_Card_Entity>("Select * From Check_Card Where CheckCardID = @Num", new { Num });
                //return lst.ToList();
            }
            
        }

        /// <summary>
        /// 점검표 수정하기
        /// </summary>
        public async Task CheckCard_Data_Modify(Check_Card_Entity mm)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                await dba.ExecuteAsync("Check_Card_Modify", mm, commandType: CommandType.StoredProcedure);
            }
            
        }

        /// <summary>
        /// 점검표 삭제
        /// </summary>
        public async Task CheckCard_Date_Delete(int CheckCardID)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                await dba.ExecuteAsync("Check_Card_Delete", new { CheckCardID }, commandType: CommandType.StoredProcedure);
            }
            
        }

        // <summary>
        /// 점검표 삭제
        /// </summary>
        public async Task Remove(int CheckCardID)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                string re = await dba.QuerySingleOrDefaultAsync<string>("Select Del From Check_Card Where CheckCardID = @CheckCardID");
                if (re != null)
                {
                    if (re == "A")
                    {
                        await dba.ExecuteAsync("Update Check_Card Set Del = 'B' Where CheckCardID = @CheckCardID", new { CheckCardID });
                    }
                    else
                    {
                        await dba.ExecuteAsync("Update Check_Card Set Del = 'A' Where CheckCardID = @CheckCardID", new { CheckCardID });
                    }
                }                
            }
        }

    }

    /// <summary>
    /// 점검 일지 종류
    /// </summary>
    public class Check_Object_Lib : ICheck_Object_Lib
    {
        private readonly IConfiguration _db;
        public Check_Object_Lib(IConfiguration configuration)
        {
            this._db = configuration;
        }

        /// <summary>
        /// 점검대상 이름 구하기
        /// </summary>
        /// <returns></returns>
        public string CheckObject_Data_Name(string Check_Object_Code_da)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                return dba.QuerySingleOrDefault<string>("Select Check_Object_Name From Check_Object Where Check_Object_Code = @Check_Object_Code_da",  new { Check_Object_Code_da });
            }
            
        }

        /// <summary>
        /// 점검대상 이름 구하기
        /// </summary>
        /// <returns></returns>
        public async Task<string> CheckObject_Data_Name_Async(string Check_Object_Code_da)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                return await dba.QuerySingleOrDefaultAsync<string>("Select Check_Object_Name From Check_Object Where Check_Object_Code = @Check_Object_Code_da", new { Check_Object_Code_da });
            }
        }

        /// <summary>
        /// 점검대상 마지막 번호 구하기
        /// </summary>
        /// <returns></returns>
        public async Task<int> CheckObject_Data_Count()
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                return await dba.QuerySingleOrDefaultAsync<int>("Select Top 1 CheckObjectID From Check_Object Order By CheckObjectID Desc");
            }
           
        }

        /// <summary>
        /// 점검대상 입력
        /// </summary>
        public async Task CheckObject_Date_Insert(Check_Object_Entity Ct)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                 await dba.ExecuteAsync("Check_Object_Insert", new { Ct.AptCode, Ct.Category, Ct.Check_Object_Code, Ct.Check_Object_Details, Ct.Check_Object_Name, Ct.PostIP, Ct.Sort } , commandType: CommandType.StoredProcedure);
            }            
        }

        /// <summary>
        /// 점검대상 리스트
        /// </summary>        
        public async Task<List<Check_Object_Entity>> CheckObject_Data_Index()
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                var lst = await dba.QueryAsync<Check_Object_Entity>("Select * From Check_Object Order By CheckObjectID Asc");
                return lst.ToList();
            }
           
        }

        /// <summary>
        /// 점검대상 상세보기
        /// </summary>        
        public async Task<Check_Object_Entity> CheckObject_Data_View(int Num_da)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                return await dba.QuerySingleOrDefaultAsync<Check_Object_Entity>("Check_Object_View", new { Num_da }, commandType: CommandType.StoredProcedure);
            }
           
        }



        /// <summary>
        /// 점검대상 수정하기
        /// </summary>
        public async Task<Check_Object_Entity> CheckObject_Data_Modify(Check_Object_Entity mm)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                await dba.ExecuteAsync("Check_Object_Modify", new { mm.AptCode, mm.Category, mm.CheckObjectID, mm.Check_Object_Code, mm.Check_Object_Details, mm.Check_Object_Name, mm.Del, mm.Sort }, commandType: CommandType.StoredProcedure);
                return mm;
            }
            
        }

        /// <summary>
        /// 점검대상 삭제
        /// </summary>
        public async Task CheckObject_Date_Delete(int CheckObjectID)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                string strA = await dba.QuerySingleOrDefaultAsync<string>("Select Del From Check_Object Where CheckObjectID = @CheckObjectID", new { CheckObjectID });

                if (strA == "A")
                {
                    await dba.ExecuteAsync("Check_Object_Delete", new { CheckObjectID }, commandType: CommandType.StoredProcedure);
                }
                else
                {
                    await dba.ExecuteAsync("Update Check_Object Set Del = 'A' Where CheckObjectID = @CheckObjectID", new { CheckObjectID });
                }
            }
            
            
        }

        /// <summary>
        /// 마지막 번호
        /// </summary>
        /// <returns></returns>
        public async Task<int> CheckObject_Last()
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                return await dba.QuerySingleOrDefaultAsync<int>("Select Top 1 CheckObjectID From Check_Object Order By CheckObjectID Desc");
            }
        }
    }

    /// <summary>
    /// 점검 주기 정보
    /// </summary>
    public class Check_Cycle_Lib : ICheck_Cycle_Lib
    {
        private readonly IConfiguration _db;
        public Check_Cycle_Lib(IConfiguration configuration)
        {
            this._db = configuration;
        }

        /// <summary>
        /// 점검주기 마지막 번호 구하기
        /// </summary>
        /// <returns></returns>
        public async Task<int> CheckCycle_Data_Count()
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                return await dba.QuerySingleOrDefaultAsync<int>("Select Top 1 CheckCycleID From Check_Cycle Order By CheckCycleID Desc");
            }
            
        }

        /// <summary>
        /// 점검주기 이름 구하기
        /// </summary>
        /// <returns></returns>
        public async Task<string> CheckCycle_Data_Name(string Check_Cycle_Code)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                return await dba.QuerySingleOrDefaultAsync<string>("Select Check_Cycle_Name From Check_Cycle Where Check_Cycle_Code =@Check_Cycle_Code", new { Check_Cycle_Code });
            }
            
        }

        /// <summary>
        /// 점검주기 입력
        /// </summary>
        public async Task CheckCycle_Date_Insert(Check_Cycle_Entity Ct)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                await dba.ExecuteAsync("Check_Cycle_Insert", new { Ct.Check_Object_Code, Ct.Check_Cycle_Code, Ct.Check_Cycle_Name, Ct.Check_Cycle_Details, Ct.PostIP, Ct.Category, Ct.Sort }, commandType: CommandType.StoredProcedure);
            }
            
        }

        /// <summary>
        /// 점검주기 리스트
        /// </summary>        
        public async Task<List<Check_Cycle_Entity>> CheckCycle_Data_Index()
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                var lst = await dba.QueryAsync<Check_Cycle_Entity>("Check_Cycle_Index", commandType: CommandType.StoredProcedure);
                return lst.ToList();
            }
            
        }

        /// <summary>
        /// 점검주기 상세보기
        /// </summary>        
        public async Task<Check_Cycle_Entity> CheckCycle_Data_View(int Num)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                return await dba.QuerySingleOrDefaultAsync<Check_Cycle_Entity>("Check_Cycle_View", new { Num }, commandType: CommandType.StoredProcedure);
            }
            
        }

        /// <summary>
        /// 점검주기 수정하기
        /// </summary>
        public async Task CheckCycle_Data_Modify(Check_Cycle_Entity mm)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                await dba.ExecuteAsync("Check_Cycle_Modify", new { mm.Check_Object_Code, mm.Check_Cycle_Code, mm.Check_Cycle_Name, mm.Check_Cycle_Details, mm.Category, mm.Sort, mm.CheckCycleID }, commandType: CommandType.StoredProcedure);
            }
            
        }

        /// <summary>
        /// 점검주기 삭제하기
        /// </summary>
        public async Task CheckCycle_Data_Delete(int intCycleId)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                string strA = await dba.QuerySingleOrDefaultAsync<string>("Select Del From Check_Cycle Where CheckCycleID = @intCycleId", new { intCycleId });

                if (strA == "A")
                {
                    await dba.ExecuteAsync("Update Check_Cycle Set Del = 'B' Where CheckCycleID = @intCycleId", new { intCycleId });
                }
                else
                {
                    await dba.ExecuteAsync("Update Check_Cycle Set Del = 'A' Where CheckCycleID = @intCycleId", new { intCycleId });
                }
            }
            
        }

        /// <summary>
        /// 점검주기 목록 만들기
        /// </summary>
        /// <returns></returns>
        public async Task<List<Check_Cycle_Entity>> GetCheckCycle_List()
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                var lst = await dba.QueryAsync<Check_Cycle_Entity>("Check_Cycle_Index", commandType: CommandType.StoredProcedure);
                return lst.ToList();
            }
            
        }

        /// <summary>
        /// 점검주기 명 불러오기
        /// </summary>
        /// <param name="Check_Cycle_Code"></param>
        /// <returns></returns>
        public string CheckCycle_Name(string Check_Cycle_Code)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                return dba.QuerySingleOrDefault<string>("Select Check_Cycle_Name From Check_Cycle Where Check_Cycle_Code =  @Check_Cycle_Code", new { Check_Cycle_Code });
            }
            
        }

        /// <summary>
        /// 점검주기 마지막 번호
        /// </summary>        
        public async Task<int> CheckCycle_Last()
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                return await dba.QuerySingleOrDefaultAsync<int>("Select Top 1 CheckCycleID From Check_Cycle Order by CheckCycleID Desc");
            }

        }
    }

    /// <summary>
    /// 점검 완료 정보(완료)
    /// </summary>
    public class Check_Input_Lib : ICheck_Input_Lib
    {
        private readonly IConfiguration _db;
        public Check_Input_Lib(IConfiguration configuration)
        {
            this._db = configuration;
        }

        /// <summary>
        /// 사업장 홈페이지 게시판 글 수  가져오기
        /// </summary>
        public async Task<int> Check_GetCount_Data(string Sort, string Code, string AptCode)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                return await dba.QuerySingleOrDefaultAsync<int>("GetCount_sw_Board", new { Sort, Code, AptCode }, commandType: CommandType.StoredProcedure);
            }
            
        }

        /// <summary>
        /// 점검입력여부 마지막 번호 구하기
        /// </summary>
        /// <returns></returns>
        public async Task<int> CheckInput_Data_Count()
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                return await dba.QuerySingleOrDefaultAsync<int>("Select Top 1 CheckInputID From Check_Input Order By CheckInputID Desc");
            }
            
        }

        /// <summary>
        /// 각 점검입력 여부 구하기(년+월+일 단위로 검색)
        /// </summary>
        /// <returns></returns>
        public async Task<int> CheckInput_Data_View_Year_Month_Day(string Check_Object_Code, string Check_Cycle_Code, string Check_Year, string Check_Month, string Check_Day, string AptCode)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                return await dba.QuerySingleOrDefaultAsync<int>("Select count(*) From Check_Input Where Check_Object_Code = @Check_Object_Code and Check_Cycle_Code = @Check_Cycle_Code And Check_Year = @Check_Year  And Check_Month = @Check_Month And Check_Day = @Check_Day And AptCode = @AptCode  And Del = 'A'", new { Check_Object_Code, Check_Cycle_Code, Check_Year, Check_Month, Check_Day, AptCode });
            }
            
        }

        /// <summary>
        /// 각 점검입력 여부 식별코드 구하기(년+월+일 단위로 검색)
        /// </summary>
        /// <returns></returns>
        public async Task<int> CheckInput_Data_View_Year_Month_Day_Aid(string Check_Object_Code, string Check_Cycle_Code, string Check_Year, string Check_Month, string Check_Day, string AptCode)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                return await dba.QuerySingleOrDefaultAsync<int>("Select CheckInputID From Check_Input Where Check_Object_Code = @Check_Object_Code and Check_Cycle_Code = @Check_Cycle_Code And Check_Year = @Check_Year  And Check_Month = @Check_Month And Check_Day = @Check_Day And AptCode = @AptCode  And Del = 'A'", new { Check_Object_Code, Check_Cycle_Code, Check_Year, Check_Month, Check_Day, AptCode });
            }
            
        }


        /// <summary>
        /// 각 점검입력 여부 구하기(년+월+일 단위로 검색)
        /// </summary>
        /// <returns></returns>
        public async Task<int> CheckInput_Data_View_Year_Month_Day_Last(string Check_Object_Code, string Check_Cycle_Code, string Check_Year, string Check_Month, string Check_Day, string AptCode)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                return await dba.QuerySingleOrDefaultAsync<int>(@"Select Top 1 isNull(CheckInputID, 0) From Check_Input Where Check_Object_Code = @Check_Object_Code and Check_Cycle_Code = @Check_Cycle_Code And Check_Year = @Check_Year  And Check_Month = @Check_Month And Check_Day = @Check_Day And AptCode = @AptCode Order By CheckInputID Desc", new { Check_Object_Code, Check_Cycle_Code, Check_Year, Check_Month, Check_Day, AptCode });
            }
            
        }

        /// <summary>
        /// 점검입력여부 입력
        /// </summary>
        public async Task<int> CheckInput_Date_Insert(Check_Input_Entity Ct)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
               return  await dba.QuerySingleOrDefaultAsync<int>("Insert Check_Input (AptCode, Check_Year, Check_Month, Check_Day, Check_Object_Code, Check_Cycle_Code, Check_Items_Code, Check_Effect_Code, PostIP, User_Post, User_Duty, User_Name) Values (@AptCode, @Check_Year, @Check_Month, @Check_Day, @Check_Object_Code, @Check_Cycle_Code, @Check_Items_Code, @Check_Effect_Code, @PostIP, @User_Post, @User_Duty, @User_Name); Select Cast(SCOPE_IDENTITY() As Int);", Ct);
            }
            
        }

        /// <summary>
        /// 점검입력여부 리스트
        /// </summary>        
        public async Task<List<Check_Input_Entity>> CheckInput_Data_Index(string Check_Object_Code, string Check_Cycle_Code, string AptCode)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                var lst = await dba.QueryAsync<Check_Input_Entity>("Check_Input_Index", new { Check_Object_Code, Check_Cycle_Code, AptCode }, commandType: CommandType.StoredProcedure);
                return lst.ToList();
            }
            
        }

        /// <summary>
        /// 점검입력 여부에 입력된 수 가져오기
        /// </summary>
        public async Task<int> Check_Input_Index_GetCount_Data(string Check_Object_Code, string Check_Cycle_Code, string AptCode)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                return await dba.QuerySingleOrDefaultAsync<int>("Select Count(*) From Check_Input Where Check_Object_Code = @Check_Object_Code And Check_Cycle_Code = @Check_Cycle_Code And AptCode = @AptCode ", new { Check_Object_Code, Check_Cycle_Code, AptCode });
            }
            
        }



        /// <summary>
        /// 점검입력여부 모두 리스트(2019)
        /// </summary>        
        public async Task<List<Check_Input_Entity>> CheckInput_Data_Index_All(string AptCode)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                var lst = await dba.QueryAsync<Check_Input_Entity>("SELECT CheckInputID, AptCode, Check_Year, Check_Month, Check_Day, Check_Object_Code, Check_Cycle_Code, Check_Items_Code, Check_Effect_Code, PostDate, PostIP, User_Post, User_Duty, User_Name, ModifyDate, ModifyIP, Del, Complete FROM Check_Input Where AptCode = @AptCode and Del = 'A'  Order By Cast(Check_Year as int) Desc, Cast(Check_Month as int) Desc, Cast(Check_Day as int) Desc, CheckInputID Desc", new { AptCode });
                return lst.ToList();
            }
            
        }

        /// <summary>
        /// 입력된 점검 리스트(페이징) 2021년
        /// </summary>
        /// <param name="Page"></param>
        /// <param name="AptCode"></param>
        /// <returns></returns>
        public async Task<List<Check_Input_Entity>> CheckInput_Data_Index_Page_All(int Page, string AptCode)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                var lst = await dba.QueryAsync<Check_Input_Entity>("Check_Input_Index_All", new { Page, AptCode }, commandType: CommandType.StoredProcedure);
                return lst.ToList();
            }

        }

        /// <summary>
        /// 입력된 점검 리스트(페이징) 2021년 (조인)
        /// </summary>
        /// <param name="Page"></param>
        /// <param name="AptCode"></param>
        /// <returns></returns>
        public async Task<List<Check_Input_Entity>> CheckInput_Data_Index_Page_All_new(int Page, string AptCode)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                var lst = await dba.QueryAsync<Check_Input_Entity>("Select Top 15 a.CheckInputID, a.AptCode, a.Check_Year, a.Check_Month, a.Check_Day, a.Complete, b.Check_Cycle_Name, c.Check_Object_Name, a.Check_Object_Code, a.Check_Cycle_Code, a.Check_Items_Code, a.Check_Effect_Code, a.PostDate, a.PostIP, a.Del, a.User_Post, a.User_Duty, a.User_Name, Check_Count From Check_Input as a Join Check_Cycle as b on a.Check_Cycle_Code = b.Check_Cycle_Code Join Check_Object as c on a.Check_Object_Code = c.Check_Object_Code Where CheckInputID Not In (Select Top (15 * @Page) CheckInputID From Check_Input Where AptCode = @AptCode Order By Cast(Check_Year as int) Desc, Cast(Check_Month as int) Desc, Cast(Check_Day as int) Desc, CheckInputID Desc) and a.AptCode = @AptCode and a.Del = 'A'  Order By Cast(a.Check_Year as int) Desc, Cast(a.Check_Month as int) Desc, Cast(a.Check_Day as int) Desc, a.CheckInputID Desc", new { Page, AptCode });
                return lst.ToList();
            }

        }

        /// <summary>
        /// 점검입력여부 모두 리스트 수(2019)
        /// </summary>
        /// <param name="AptCode"></param>
        /// <returns></returns>
        public async Task<int> CheckInput_Data_Index_All_Count(string AptCode)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                return await dba.QuerySingleOrDefaultAsync<int>("SELECT Count(*) FROM Check_Input Where AptCode = @AptCode and Del = 'A'", new { AptCode });
            }
            
        }

        /// <summary>
        /// 점검입력 여부에 입력된 수 가져오기
        /// </summary>
        public async Task<int> Check_Input_Index_GetCount_All_Data(string AptCode)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                return await dba.QuerySingleOrDefaultAsync<int>("Select Count(*) From Check_Input Where AptCode = @AptCode", new { AptCode });
            }
            
        }

        /// <summary>
        /// 점검입력여부 시설물별로 불러 온 리스트
        /// </summary>        
        public async Task<List<Check_Input_Entity>> CheckInput_Data_Index_Object(string Check_Object_Code, string AptCode)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                var lst = await dba.QueryAsync<Check_Input_Entity>("Select CheckInputID, AptCode, Check_Year, Check_Month, Check_Day, Check_Object_Code, Check_Cycle_Code, Check_Items_Code, Check_Effect_Code, PostDate, PostIP, ModifyDate, ModifyIP, Del, Complete, User_Post, User_Duty, User_Name From Check_Input Where Check_Object_Code = @Check_Object_Code and AptCode = @AptCode Order By Cast(Check_Year as int) Desc, Cast(Check_Month as int) Desc, Cast(Check_Day as int) Desc, CheckInputID Desc", new { Check_Object_Code, AptCode });
                return lst.ToList();
            }
            
        }

        /// <summary>
        /// 점검입력여부 시설물별로 불러 온 리스트(페이징) 2021년
        /// </summary>        
        public async Task<List<Check_Input_Entity>> CheckInput_Data_Index_Page_Object(int Page, string Check_Object_Code, string AptCode)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                var lst = await dba.QueryAsync<Check_Input_Entity>("Check_Input_Index_Object", new { Page, Check_Object_Code, AptCode }, commandType: CommandType.StoredProcedure);
                return lst.ToList();
            }

        }

        /// <summary>
        /// 점검입력여부 시설물별로 불러 온 데이타 수
        /// </summary>
        /// <param name="Check_Object_Code"></param>
        /// <param name="AptCode"></param>
        /// <returns></returns>
        public async Task<int> CheckInput_Data_Index_Object_Count(string Check_Object_Code, string AptCode)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                return await dba.QuerySingleOrDefaultAsync<int>("Select Count(*) From Check_Input Where Check_Object_Code = @Check_Object_Code and AptCode = @AptCode ", new { Check_Object_Code, AptCode });
            }
            
        }

        /// <summary>
        /// 점검입력 여부에 입력된 수 가져오기
        /// </summary>
        public async Task<int> Check_Input_Index_GetCount_Object_Data(string Check_Object_Code, string AptCode)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                return await dba.QuerySingleOrDefaultAsync<int>("Select Count(*) From Check_Input Where Check_Object_Code = @Check_Object_Code And AptCode = @AptCode", new { Check_Object_Code, AptCode });
            }
            
        }

        /// <summary>
        /// 점검입력여부 주기별로 불러 온 리스트
        /// </summary>        
        public async Task<List<Check_Input_Entity>> CheckInput_Data_Index_Cycle(string Check_Cycle_Code, string AptCode)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                var lst = await dba.QueryAsync<Check_Input_Entity>("Select CheckInputID, AptCode, Check_Year, Check_Month, Check_Day, Check_Object_Code, Check_Cycle_Code, Check_Items_Code, Check_Effect_Code, PostDate, PostIP, ModifyDate, ModifyIP, Del, Complete, User_Post, User_Duty, User_Name, Check_Count From Check_Input Where Check_Cycle_Code = @Check_Cycle_Code and AptCode = @AptCode Order By  Cast(Check_Year as int) Desc, Cast(Check_Month as int) Desc, Cast(Check_Day as int) Desc, CheckInputID Desc", new { Check_Cycle_Code, AptCode });
                return lst.ToList();
            }
            
        }

        /// <summary>
        /// 점검입력여부 주기별로 불러 온 리스트(Paging) 2021년
        /// </summary>        
        public async Task<List<Check_Input_Entity>> CheckInput_Data_Index_Page_Cycle(int Page, string Check_Cycle_Code, string AptCode)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                var lst = await dba.QueryAsync<Check_Input_Entity>("Check_Input_Index_Cycle", new { Page, Check_Cycle_Code, AptCode });
                return lst.ToList();
            }

        }

        /// <summary>
        /// 점검입력여부 주기별로 불러 온 리스트 수
        /// </summary>        
        public async Task<int> CheckInput_Data_Index_Cycle_Count(string Check_Cycle_Code, string AptCode)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                return await dba.QuerySingleOrDefaultAsync<int>("Select Count(*) From Check_Input Where Check_Cycle_Code = @Check_Cycle_Code and AptCode = @AptCode", new { Check_Cycle_Code, AptCode });
            }
            
        }
        /// <summary>
        /// 점검입력 여부에 입력된 수 가져오기
        /// </summary>
        public async Task<int> Check_Input_Index_GetCount_Cycle_Data(string Check_Cycle_Code, string AptCode)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                return await dba.QuerySingleOrDefaultAsync<int>("Select Count(*) From Check_Input Where Check_Cycle_Code = @Check_Cycle_Code And AptCode = @AptCode", new { Check_Cycle_Code, AptCode });
            }
            
        }

        /// <summary>
        /// 점검입력여부 이전글 보기
        /// </summary>        
        public async Task<List<Check_Input_Entity>> CheckInput_Data_Ago(string AptCode, int Num)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                var lst = await dba.QueryAsync<Check_Input_Entity>("Select * From Check_Input Where AptCode = '" + AptCode + "' And CheckInputID = (Select Max(CheckInputID) From Check_Input Where AptCode = @AptCode And CheckInputID < " + Num + ") Order By Cast(Check_Year as int) Desc, Cast(Check_Month as int) Desc, Cast(Check_Day as int) Desc, CheckInputID Desc", new { AptCode, Num });
                return lst.ToList();
            }
            
        }

        /// <summary>
        /// 점검입력여부 이전글 보기(a)
        /// </summary>        
        public async Task<int> CheckInput_AgoA(string AptCode, int Num)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                return await dba.QuerySingleOrDefaultAsync<int>("Select top 1 CheckInputID From Check_Input Where AptCode = '" + AptCode + "' And CheckInputID = (Select Max(CheckInputID) From Check_Input Where AptCode = @AptCode And CheckInputID < " + Num + ")", new { AptCode, Num });
            }
            
        }

        /// <summary>
        /// 점검입력여부 이전글 존재 여부 가져오기
        /// </summary>        
        public async Task<int> Check_Input_Count_Ago_Data(string AptCode, int Num)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                return await dba.QuerySingleOrDefaultAsync<int>("Select Count(*) From Check_Input Where AptCode = @AptCode And CheckInputID = (Select Max(CheckInputID) From Check_Input Where AptCode = @AptCode And CheckInputID < " + Num + ")", new { AptCode, Num });
            }
            
        }

        /// <summary>
        /// 점검입력여부 다음글 보기
        /// </summary>        
        public async Task<List<Check_Input_Entity>> CheckInput_Data_Next(string AptCode, int Num)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                var lst = await dba.QueryAsync<Check_Input_Entity>("Select * From Check_Input Where AptCode = @AptCode And CheckInputID = (Select Min(CheckInputID) From Check_Input Where AptCode = @AptCode And CheckInputID > " + Num + ") Order By Cast(Check_Year as int) Desc, Cast(Check_Month as int) Desc, Cast(Check_Day as int) Desc, CheckInputID Desc", new { AptCode, Num });
                return lst.ToList();
            }
            
        }

        /// <summary>
        /// 점검입력여부 다음글 보기(a)
        /// </summary>        
        public async Task<int> CheckInput_NextA(string AptCode, int Num)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                return await dba.QuerySingleOrDefaultAsync<int>("Select top 1 CheckInputID From Check_Input Where AptCode = @AptCode And CheckInputID = (Select Min(CheckInputID) From Check_Input Where AptCode = @AptCode And CheckInputID > " + Num + ")", new { AptCode, Num });
            }
            
        }

        /// <summary>
        /// 점검입력여부 이전글 존재 여부 가져오기
        /// </summary>        
        public async Task<int> Check_Input_Count_Next_Data(string AptCode, int Num)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                return await dba.QuerySingleOrDefaultAsync<int>("Select Count(*) From Check_Input Where AptCode = '" + AptCode + "' And CheckInputID = (Select Min(CheckInputID) From Check_Input Where AptCode = @AptCode And CheckInputID > " + Num + ")", new { AptCode, Num });
            }
            
        }

        /// <summary>
        /// 점검입력여부 상세보기
        /// </summary>        
        public async Task<Check_Input_Entity> CheckInput_Data_View(int CheckInputID, string AptCode)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                return await dba.QuerySingleOrDefaultAsync<Check_Input_Entity>("Check_Input_View", new { CheckInputID, AptCode }, commandType: CommandType.StoredProcedure);
            }
            
        }

        /// <summary>
        /// 점검입력여부 수정하기
        /// </summary>
        public async Task CheckInput_Data_Modify(Check_Input_Entity mm)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                await dba.ExecuteAsync("Check_Input_Modify", new { mm.Check_Year, mm.Check_Month, mm.Check_Day, mm.Check_Object_Code, mm.Check_Cycle_Code, mm.Check_Items_Code, mm.Check_Effect_Code, mm.Del, mm.CheckInputID }, commandType: CommandType.StoredProcedure);
            }
            
        }

        /// <summary>
        /// 점검입력여부 삭제
        /// </summary>
        public async Task CheckInput_Date_Delete(int CheckInputID)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                await dba.ExecuteAsync("Check_Input_Delete", new { CheckInputID }, commandType: CommandType.StoredProcedure);
            }
            
        }

        public async Task<List<Check_Input_Entity>> GetCheckInput_List(string AptCode, int Page)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                var lst = await dba.QueryAsync<Check_Input_Entity>("Check_Input_Index_A", new { AptCode, Page }, commandType: CommandType.StoredProcedure);
                return lst.ToList();
            }
            
        }

        /// <summary>
        /// 결재 여부
        /// </summary>
        public async Task<string> Input_Complate(int Num)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                return await dba.QuerySingleOrDefaultAsync<string>("Select Complete From Check_Input Where CheckInputID = @Num", new { Num });
            }
            
        }

        // 이하 이전 및 다음 페이지 구현 메서드
        /// <summary>
        /// 이전글 존재 여부 가져오기
        /// </summary>
        /// <param name="Apt_Code"></param>
        /// <param name="Num"></param>
        /// <returns></returns>
        public async Task<int> CheckInput_CountNext(string Apt_Code, int Num)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                return await dba.QuerySingleOrDefaultAsync<int>("Select Count(*) From Check_Input Where AptCode = @Apt_Code And CheckInputID = (Select Min(CheckInputID) From Check_Input Where AptCode = @Apt_Code And CheckInputID > @Num)", new { Apt_Code, Num });
            }
            
        }

        /// <summary>
        /// 점검일지 다음글 보기
        /// </summary>
        /// <param name="AptCode"></param>
        /// <param name="Num"></param>
        /// <returns></returns>
        public async Task<List<Check_Input_Entity>> CheckInput_Next(string AptCode, int Num)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                var lst = await dba.QueryAsync<Check_Input_Entity>("Select * From Check_Input Where AptCode = @AptCode And CheckInputID = (Select Min(CheckInputID) From Check_Input Where AptCode =@AptCode And CheckInputID > @Num)", new { AptCode, Num });
                return lst.ToList();
            }
            
        }

        /// <summary>
        /// 점검일지 이전글 존재 여부 가져오기
        /// </summary>
        /// <param name="AptCode"></param>
        /// <param name="Num"></param>
        /// <returns></returns>
        public async Task<int> CheckInput_CountAgo(string AptCode, int Num)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                return await dba.QuerySingleOrDefaultAsync<int>("Select Count(*) From Check_Input Where AptCode = @AptCode And CheckInputID = (Select Max(CheckInputID) From Check_Input Where AptCode = @AptCode And CheckInputID < @Num)", new { AptCode, Num });
            }
            
        }

        /// <summary>
        /// 점검일지 이전글 보기
        /// </summary>
        /// <param name="AptCode"></param>
        /// <param name="Num"></param>
        /// <returns></returns>
        public async Task<List<Check_Input_Entity>> CheckInput_Ago(string AptCode, int Num)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                var lst = await dba.QueryAsync<Check_Input_Entity>("Select * From Check_Input Where AptCode = @AptCode And CheckInputID = (Select Max(CheckInputID) From Check_Input Where AptCode = @AptCode And CheckInputID < @Num) ", new { AptCode, Num });
                return lst.ToList();
            }
            
        }

        /// <summary>
        /// 해당 시설물에 입력된 수
        /// </summary>
        /// <param name="Aid"></param>
        /// <param name="Division"></param>
        /// <returns></returns>
        public async Task Check_Count_Add(int Aid, string Division)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                if (Division == "A")
                {
                    await dba.ExecuteAsync("Update Check_Input Set Check_Count = Check_Count + 1 Where CheckInputID = @Aid", new { Aid, Division });
                }
                else if (Division == "B")
                {
                    await dba.ExecuteAsync("Update Check_Input Set Check_Count = Check_Count - 1 Where CheckInputID = @Aid", new { Aid, Division });
                }
            }
        }

        /// <summary>
        /// 해당 시설물에 등록된 파일 수 더하기
        /// </summary>
        public async Task Files_Count_Add(int Aid, string Division)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                if (Division == "A")
                {
                    await dba.ExecuteAsync("Update Check_Input Set Files_Count = Files_Count + 1 Where CheckInputID = @Aid", new { Aid });
                }
                else if (Division == "B")
                {
                    await dba.ExecuteAsync("Update Check_Input Set Files_Count = Files_Count - 1 Where CheckInputID = @Aid", new { Aid });
                }                
            }
        }
    }

    /// <summary>
    /// 점점사항 클래스(완료)
    /// </summary>
    public class Check_Items_Lib : ICheck_Items_Lib
    {
        private readonly IConfiguration _db;
        public Check_Items_Lib(IConfiguration configuration)
        {
            this._db = configuration;
        }

        /// <summary>
        /// 점검사항 이름 구하기
        /// </summary>
        /// <returns></returns>
        public async Task<string> CheckItems_Data_Name(string Check_Items_Code)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                return await dba.QuerySingleOrDefaultAsync<string>("Select Check_Items From Check_Items Where Check_Items_Code =@Check_Items_Code", new { Check_Items_Code });
            }
            
        }

        /// <summary>
        /// 점검사항 이름
        /// </summary>
        public string CheckItems_Name(string Check_Items_Code)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                return dba.QuerySingleOrDefault<string>("Select Check_Items From Check_Items Where Check_Items_Code =@Check_Items_Code", new { Check_Items_Code });
            }

        }

        /// <summary>
        /// 점검사항 마지막 번호 구하기
        /// </summary>
        /// <returns></returns>
        public async Task<int> CheckItems_Data_Count()
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                return await dba.QuerySingleOrDefaultAsync<int>("Select Top 1 CheckItemsID From Check_Items Order By CheckItemsID Desc");
            }
            
        }

        /// <summary>
        /// 점검사항별 입력된 수 구하기
        /// </summary>
        /// <returns></returns>
        public async Task<int> CheckItems_View_Data_Count(string strObject_Code, string strCycle_Code)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                return await dba.QuerySingleOrDefaultAsync<int>(@"Select Count(*) from Check_Items Where Check_Object_Code = @strObject_Code and Check_Cycle_Code = @strCycle_Code", new { strObject_Code, strCycle_Code });
            }
            
        }
        /// <summary>
        /// 점검사항 입력
        /// </summary>
        public async Task CheckItems_Date_Insert(Check_Items_Entity Ct)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                await dba.ExecuteAsync("Check_Items_Insert", new { Ct.Check_Object_Code, Ct.Check_Cycle_Code, Ct.Check_Items_Code, Ct.Check_Items, Ct.PostIP, Ct.Category, Ct.Sort }, commandType: CommandType.StoredProcedure);
            }
            
        }

        /// <summary>
        /// 점검사항 리스트
        /// </summary>        
        public async Task<List<Check_Items_Entity>> CheckItems_Data_Index(string Check_Object_Code, string Check_Cycle_Code)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                var lst = await dba.QueryAsync<Check_Items_Entity>("Check_Items_Index", new { Check_Object_Code, Check_Cycle_Code }, commandType: CommandType.StoredProcedure);
                return lst.ToList();
            }
            
        }

        /// <summary>
        /// 점검사항 리스트
        /// </summary>        
        public async Task<List<Check_Items_Entity>> CheckItems_Data_Join_Index(int Page, string Check_Object_Code, string Check_Cycle_Code)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                var lst = await dba.QueryAsync<Check_Items_Entity>("Select Top 10 a.Category, a.Check_Cycle_Code, a.Check_Items, a.Check_Items_Code, a.Check_Object_Code, a.CheckItemsID, b.Check_Object_Name, c.Check_Cycle_Name From Check_Items as a Join Check_Object as b on a.Check_Object_Code = b.Check_Object_Code Join Check_Cycle as c on a.Check_Cycle_Code = c.Check_Cycle_Code Where a.CheckItemsID Not In (Select Top (10 * @Page) a.CheckItemsID From Check_Items as a Join Check_Object as b on a.Check_Object_Code = b.Check_Object_Code Join Check_Cycle as c on a.Check_Cycle_Code = c.Check_Cycle_Code Where a.Check_Cycle_Code = @Check_Cycle_Code And a.Check_Object_Code = @Check_Object_Code Order By a.CheckItemsID Asc) and a.Check_Cycle_Code = @Check_Cycle_Code And a.Check_Object_Code = @Check_Object_Code Order By a.CheckItemsID Asc", new { Page, Check_Object_Code, Check_Cycle_Code });
                return lst.ToList();
            }
        }

        /// <summary>
        /// 점검사항 리스트(페이징)
        /// </summary>        
        public async Task<List<Check_Items_Entity>> CheckItems_Page_Index(int Page, string Check_Object_Code, string Check_Cycle_Code)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                var lst = await dba.QueryAsync<Check_Items_Entity>("Select Top 15 * From Check_Items Where CheckItemsID Not In(Select Top(15 * @Page) CheckItemsID From Check_Items Where Check_Object_Code = @Check_Object_Code And Check_Cycle_Code = @Check_Cycle_Code Order By CheckItemsID Desc) And Check_Object_Code = @Check_Object_Code And Check_Cycle_Code = @Check_Cycle_Code Order By CheckItemsID Desc", new { Check_Object_Code, Check_Cycle_Code });
                return lst.ToList();
            }

        }


        /// <summary>
        /// 점검사항 입력된 수
        /// </summary>        
        public async Task<int> CheckItems_Data_Index_Count(string Check_Object_Code, string Check_Cycle_Code)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                return await dba.QuerySingleOrDefaultAsync<int>("Select Count(*) From Check_Items Where Check_Cycle_Code = @Check_Cycle_Code and Check_Object_Code = @Check_Object_Code and Del = 'A' ", new { Check_Object_Code, Check_Cycle_Code });
            }
            
        }

        /// <summary>
        /// 점검사항 상세보기
        /// </summary>        
        public async Task<Check_Items_Entity> CheckItems_Data_View(int CheckItemsID)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                return await dba.QuerySingleOrDefaultAsync<Check_Items_Entity>("Check_Items_View", new { CheckItemsID }, commandType: CommandType.StoredProcedure);
            }
            
        }

        /// <summary>
        /// 점검사항 조인 상세보기(2021)
        /// </summary>
        /// <param name="CheckItemsID"></param>
        /// <returns></returns>
        public async Task<Check_Items_Entity> CheckItems_Data_Details(int CheckItemsID)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                return await dba.QuerySingleOrDefaultAsync<Check_Items_Entity>("Select a.CheckItemsID, a.Check_Object_Code, a.Check_Cycle_Code, a.Check_Items_Code, a.Check_Items, a.Del, a.PostDate, a.PostIP, a.Category, a.Sort, b.Check_Object_Name, c.Check_Cycle_Name From Check_Items as a Join Check_Object as b On a.Check_Object_Code = b.Check_Object_Code Join Check_Cycle as c On a.Check_Cycle_Code = c.Check_Cycle_Code Where CheckItemsID = @CheckItemsID", new { CheckItemsID });
            }
            
        }

        /// <summary>
        /// 점검사항 상세보기_Be
        /// </summary>        
        public async Task<int> CheckItems_Data_View_Be(int CheckItemsID)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                return await dba.QuerySingleOrDefaultAsync<int>("Select Count(*) From Check_Items Where CheckItemsID = @CheckItemsID", new { CheckItemsID });
            }
            
        }

        /// <summary>
        /// 점검사항 수정하기
        /// </summary>
        public async Task CheckItems_Data_Modify(Check_Items_Entity mm)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                await dba.ExecuteAsync("Check_Items_Modify", new { mm.Check_Object_Code, mm.Check_Cycle_Code, mm.Check_Items_Code, mm.Check_Items, mm.Del, mm.Category, mm.Sort, mm.CheckItemsID }, commandType: CommandType.StoredProcedure);
            }
            
        }

        /// <summary>
        /// 점검사항 삭제
        /// </summary>
        public async Task CheckItems_Date_Delete(int CheckItemsID)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                string strA = await dba.QuerySingleOrDefaultAsync<string>("Select Del From Check_Items Where CheckItemsID = @CheckItemsID", new { CheckItemsID });
                if (strA == "A")
                {
                    await dba.ExecuteAsync("Check_Items_Delete", new { CheckItemsID }, commandType: CommandType.StoredProcedure);
                }
                else
                {
                    await dba.ExecuteAsync("Update Check_Items Set Del = 'A' Where CheckItemsID = @CheckItemsID", new { CheckItemsID });
                }
            }
           
        }
        /// <summary>
        /// 점검사항 리스트
        /// </summary>        
        public async Task<List<Check_Items_Entity>> CheckItems_Index(int Page)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                var lst = await dba.QueryAsync<Check_Items_Entity>("Select Top 15 * From Check_Items Where CheckItemsID Not In(Select Top(15 * @Page) CheckItemsID From Check_Items Order By CheckItemsID Desc) Order By CheckItemsID Desc", new { Page });
                return lst.ToList();
            }

        }



        /// <summary>
        /// 점검사항 리스트
        /// </summary>        
        public async Task<int> CheckItems_Index_Count()
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                return await dba.QuerySingleOrDefaultAsync<int>("Select Count(*) From Check_Items");
            }

        }

        /// <summary>
        /// 점검사항 마지막 번호
        /// </summary>        
        public async Task<int> CheckItems_Last()
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                return await dba.QuerySingleOrDefaultAsync<int>("Select Top 1 CheckItemsID From Check_Items Order by CheckItemsID Desc");
            }

        }

        /// <summary>
        /// 점검 내용 수 증가 및 감소
        /// </summary>
        /// <param name="Aid"></param>
        /// <param name="Division"></param>
        /// <returns></returns>
        public async Task Check_Count_Add(int Aid, string Division)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                if (Division == "A")
                {
                    await dba.ExecuteAsync("Update Check_Input Set Check_Count = Check_Count + 1 Where CheckInputID = @Aid", new { Aid });
                }
                else if(Division == "B")
                {
                    await dba.ExecuteAsync("Update Check_Input Set Check_Count = Check_Count - 1 Where CheckInputID = @Aid", new { Aid });
                }
            }
        }
    }

    /// <summary>
    /// 점검 결과 클래스
    /// </summary>
    public class Check_Effect_Lib : ICheck_Effect_Lib
    {
        private readonly IConfiguration _db;
        public Check_Effect_Lib(IConfiguration configuration)
        {
            this._db = configuration;
        }

        /// <summary>
        /// 점검결과 이름 구하기
        /// </summary>
        /// <returns></returns>
        public string CheckEffect_Data_Name(string Check_Effect_Code)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                return dba.QuerySingleOrDefault<string>("Select Check_Effect_Name From Check_Effect Where Check_Effect_Code =@Check_Effect_Code", new { Check_Effect_Code });
            }
            
        }

        /// <summary>
        /// 점검결과 마지막 번호 구하기
        /// </summary>
        /// <returns></returns>
        public async Task<int> CheckEffect_Data_Count()
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                return await dba.QuerySingleOrDefaultAsync<int>("Select Top 1 CheckEffectID From Check_Effect Order By CheckEffectID Desc");
            }
            
        }

        /// <summary>
        /// 점검결과 입력
        /// </summary>
        public async Task CheckEffect_Date_Insert(Check_Effect_Entity Ct)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                await dba.ExecuteAsync("Check_Effect_Insert", new { Ct.AptCode, Ct.Check_Effect_Code, Ct.Check_Effect_Name, Ct.Check_Effect_Details, Ct.PostIP }, commandType: CommandType.StoredProcedure);
            }
            
        }

        /// <summary>
        /// 점검결과 리스트
        /// </summary>        
        public async Task<List<Check_Effect_Entity>> CheckEffect_Data_Index(string AptCode)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                var lst = await dba.QueryAsync<Check_Effect_Entity>("Check_Effect_Index", new { AptCode }, commandType: CommandType.StoredProcedure);
                return lst.ToList();
            }
            
        }



        /// <summary>
        /// 점검결과 상세보기
        /// </summary>        
        public async Task<Check_Effect_Entity> CheckEffect_Data_View(int Num)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                return await dba.QuerySingleOrDefaultAsync<Check_Effect_Entity>("Select * From Check_Effect Where CheckEffectID = @Num", new { Num });
            }
            
        }

        /// <summary>
        /// 점검결과 수정하기
        /// </summary>
        public async Task CheckEffect_Data_Modify(Check_Effect_Entity mm)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                await dba.QueryAsync("Check_Effect_Modify", new { mm.Check_Effect_Code, mm.Check_Effect_Name, mm.Check_Effect_Details, mm.Del, mm.CheckEffectID }, commandType: CommandType.StoredProcedure);
            }
            
        }

        /// <summary>
        /// 점검결과 삭제
        /// </summary>
        public async Task CheckEffect_Date_Delete(int CheckEffectID)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                await dba.ExecuteAsync("Check_Effect_Delete", new { CheckEffectID }, commandType: CommandType.StoredProcedure);
            }
            
        }
    }
}
