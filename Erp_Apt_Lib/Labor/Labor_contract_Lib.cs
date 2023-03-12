using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace sw_Lib.Labors
{
    public class Labor_contract_Lib : ILabor_contract_Lib
    {
        private readonly IConfiguration _db;
        public Labor_contract_Lib(IConfiguration configuration)
        {
            this._db = configuration;
        }

        /// <summary>
        /// 근로계약서 입력
        /// </summary>
        /// <param name="labor_Contract"></param>
        /// <returns></returns>
        public async Task<Labor_contract_Entity> add(Labor_contract_Entity labor_Contract)
        {
            var sql = "INSERT INTO Labor_contract (Apt_Code, Apt_Name, UserID, UserName, Adress, Mobile, Telephone, PartTime, LaborStartDate, LaborEndDate, WorkDetail, BasicsPay, Pay_A, Pay_B, Pay_C, Pay_D, Pay_E, Pay_F, TotalSum, Pay_Etc, WorktimeSort, WorkPlace, WorkTime, WorkTimeWeekend, WorkMonthTime, WorkTimeEtc, StartWorkTime, EndWorkTime, BreakTimeSort, BreaktimeDaytime, BreaktimeNight, BreakTimeEtc, Holiday, RetirementAge, Read_Approval, Read_Approval1, Copy_Approval, Contract_Etc, WorkContract_Date, Cor_Code, Ceo_Adress, Ceo_Telephone, Ceo_Company, Ceo_Name, ContractNotice, ContractApprovalDivision, PostIP) VALUES (@Apt_Code, @Apt_Name, @UserID, @UserName, @Adress, @Mobile, @Telephone, @PartTime, @LaborStartDate, @LaborEndDate, @WorkDetail, @BasicsPay, @Pay_A, @Pay_B, @Pay_C, @Pay_D, @Pay_E, @Pay_F, @TotalSum, @Pay_Etc, @WorktimeSort, @WorkPlace, @WorkTime, @WorkTimeWeekend, @WorkMonthTime, @WorkTimeEtc, @StartWorkTime, @EndWorkTime, @BreakTimeSort, @BreaktimeDaytime, @BreaktimeNight, @BreakTimeEtc, @Holiday, @RetirementAge, @Read_Approval, @Read_Approval1, @Copy_Approval, @Contract_Etc, @WorkContract_Date, @Cor_Code, @Ceo_Adress, @Ceo_Telephone, @Ceo_Company, @Ceo_Name, @ContractNotice, @ContractApprovalDivision, @PostIP)";
            using var ctx = new SqlConnection(_db.GetConnectionString("sw_togather"));
            await ctx.ExecuteAsync(sql, labor_Contract, commandType: CommandType.Text);
            //labor_Contract.Aid = Num;            
            return labor_Contract;

        }

        /// <summary>
        /// 근로계약서 목록(단지별)
        /// </summary>
        /// <param name="Apt_Code"></param>
        /// <returns></returns>
        public async Task<List<Labor_contract_Entity>> GetList(string Apt_Code)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                var Lsit = await ctx.QueryAsync<Labor_contract_Entity>("Select Aid, Apt_Code, Apt_Name, UserID, UserName, Adress, Mobile, Telephone, PartTime, LaborStartDate, LaborEndDate, WorkDetail, BasicsPay, Pay_A, Pay_B, Pay_C, Pay_D, Pay_E, Pay_F, TotalSum, Pay_Etc, WorkPlace, WorkTime, WorkTimeWeekend, WorkMonthTime, WorkTimeEtc, StartWorkTime, EndWorkTime, BreakTimeSort, BreaktimeDaytime, BreaktimeNight, BreakTimeEtc, Holiday, RetirementAge, Read_Approval, Read_Approval1, Copy_Approval, Contract_Etc, WorkContract_Date, Cor_Code, Ceo_Adress, Ceo_Telephone, Ceo_Company, Ceo_Name, ContractNotice, ContractApprovalDivision, PostDate, PostIP, Division From Labor_contract Where Apt_Code = @Apt_Code Order By Aid Desc", new { Apt_Code }, commandType: CommandType.Text);
                return Lsit.ToList();
            }
            
        }

        /// <summary>
        /// 근로계약서 목록(단지별)
        /// </summary>
        public async Task<List<Labor_contract_Entity>> GetList_All(int Page)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                var Lsit = await ctx.QueryAsync<Labor_contract_Entity>("Select Top 15 * From Labor_contract Where Aid Not In(Select Top(15 * @Page) Aid From Labor_contract Order By Aid Desc) Order By Aid Desc", new { Page }, commandType: CommandType.Text);
                return Lsit.ToList();
            }

        }

        /// <summary>
        /// 근로계약서 목록(근로자 별)
        /// </summary>
        /// <param name="User_Code"></param>
        /// <returns></returns>
        public async Task<List<Labor_contract_Entity>> GetList_User(string Apt_Code, string UserID)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                var Lsit = await ctx.QueryAsync<Labor_contract_Entity>("Select Aid, Apt_Code, Apt_Name, UserID, UserName, Adress, Mobile, Telephone, PartTime, LaborStartDate, LaborEndDate, WorkDetail, BasicsPay, Pay_A, Pay_B, Pay_C, Pay_D, Pay_E, Pay_F, TotalSum, Pay_Etc, WorkPlace, WorkTime, WorkTimeWeekend, WorkMonthTime, WorkTimeEtc, StartWorkTime, EndWorkTime, BreakTimeSort, BreaktimeDaytime, BreaktimeNight, BreakTimeEtc, RetirementAge, Holiday, Read_Approval, Read_Approval1, Copy_Approval, Contract_Etc, WorkContract_Date, Cor_Code, Ceo_Adress, Ceo_Telephone, Ceo_Company, Ceo_Name, ContractNotice, ContractApprovalDivision, PostDate, PostIP, Division From Labor_contract Where Apt_Code = @Apt_Code And UserID = @UserID Order By Aid Desc", new { Apt_Code, UserID }, commandType: CommandType.Text);
                return Lsit.ToList();
            }
            
        }

        /// <summary>
        /// 근로계약서 수정
        /// </summary>
        /// <param name="_Contract_Entity"></param>
        /// <returns></returns>
        public async Task<Labor_contract_Entity> Edit(Labor_contract_Entity _Contract_Entity)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                var v = "Update Labor_contract Set Adress = @Adress, Mobile = @Mobile, Telephone = @Telephone, LaborStartDate = @LaborStartDate, LaborEndDate = @LaborEndDate, WorkDetail = @WorkDetail, BasicsPay = @BasicsPay, Pay_A = @Pay_A, Pay_B = @Pay_B, Pay_C = @Pay_C, Pay_D = @Pay_D, Pay_E = @Pay_E, Pay_F = @Pay_F, TotalSum = @TotalSum, Pay_Etc= @Pay_Etc, WorktimeSort = @WorktimeSort, WorkPlace = @WorkPlace, WorkTime =@WorkTime, WorkTimeWeekend = @WorkTimeWeekend, WorkMonthTime = @WorkMonthTime, StartWorkTime = @StartWorkTime, EndWorkTime = @EndWorkTime, WorkTimeEtc = @WorkTimeEtc, BreakTimeSort = @BreakTimeSort, BreaktimeDaytime = @BreaktimeDaytime, BreaktimeNight = @BreaktimeNight, BreakTimeEtc = @BreakTimeEtc, Holiday = @Holiday, RetirementAge = @RetirementAge, Read_Approval = @Read_Approval, Read_Approval1 = @Read_Approval1, Copy_Approval = @Copy_Approval, Contract_Etc = @Contract_Etc, WorkContract_Date = @WorkContract_Date, Cor_Code = @Cor_Code, Ceo_Adress = @Ceo_Adress, Ceo_Telephone = @Ceo_Telephone, Ceo_Company = @Ceo_Company, Ceo_Name = @Ceo_Name, ContractNotice = @ContractNotice, ContractApprovalDivision = @ContractApprovalDivision, PostDate = @PostDate, PostIP = @PostIP Where Aid= @Aid";
                //var sql = V;
                await ctx.ExecuteAsync(v, _Contract_Entity, commandType: CommandType.Text);
                return _Contract_Entity;
            }
            
        }


        /// <summary>
        /// 단지별 근로계약서 목록
        /// </summary>
        /// <param name="Apt_Code"></param>
        /// <returns></returns>
        public async Task<List<Labor_contract_Entity>> Contract_list_Apt(int Page, string Apt_Code)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                var Lsit = await ctx.QueryAsync<Labor_contract_Entity>("Select Top 15 * From Labor_contract Where Aid Not In(Select Top(15 * @Page) Aid From Labor_contract Where Apt_Code = @Apt_Code Order By Aid Desc) and Apt_Code = @Apt_Code Order By Aid Desc, LaborStartDate Desc", new { Page, Apt_Code }, commandType: CommandType.Text);
                return Lsit.ToList();
            }
            
        }

        /// <summary>
        /// 단지별 근로계약서 목록
        /// </summary>
        public async Task<List<Labor_contract_Entity>> Contract_list_Name(int Page, string UserName)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                var Lsit = await ctx.QueryAsync<Labor_contract_Entity>("Select Top 15 * From Labor_contract Where Aid Not In(Select Top(15 * @Page) Aid From Labor_contract Where UserName Like '%" + UserName + "%' Order By Aid Desc) and UserName Like '%" + UserName + "%' Order By Aid Desc, LaborStartDate Desc", new { Page, UserName}, commandType: CommandType.Text);
                return Lsit.ToList();
            }
        }

        /// <summary>
        /// 단지별 근로계약서 수
        /// </summary>
        public async Task<int> Contract_List_Name_count(string UserName)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                return await ctx.QuerySingleOrDefaultAsync<int>("Select Count(*) From Labor_contract Where UserName Like '%" + UserName + "%'", new { UserName }, commandType: CommandType.Text);
            }

        }

        /// <summary>
        /// 단지별 근로계약서 목록
        /// </summary>
        /// <param name="Apt_Code"></param>
        /// <returns></returns>
        public async Task<List<Labor_contract_Entity>> Contract_list_Apt_A(string Apt_Code)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                var Lsit = await ctx.QueryAsync<Labor_contract_Entity>("Select Aid, Apt_Code, Apt_Name, UserID, UserName, Adress, Mobile, Telephone, PartTime, LaborStartDate, LaborEndDate, WorkPlace, WorkDetail, BasicsPay, Pay_A, Pay_B, Pay_C, Pay_D, Pay_E, Pay_F, TotalSum, Pay_Etc, WorktimeSort, Worktime, WorkTimeWeekend, WorkMonthTime, WorkTimeEtc, StartWorkTime, EndWorkTime, BreaktimeSort, BreaktimeDaytime, BreaktimeNight, BreakTimeEtc, Holiday, RetirementAge, Read_approval, Read_Approval1, Copy_approval, Contract_Etc, WorkContract_Date, Cor_Code, Ceo_Adress, Ceo_Telephone, Ceo_Company, Ceo_Name, ContractNotice, ContractApprovalDivision, PostDate, PostIP, Division From Labor_contract Where Apt_Code = @Apt_Code  Order By Aid Desc", new { Apt_Code }, commandType: CommandType.Text);
                return Lsit.ToList();
            }            
        }

        /// <summary>
        /// 단지별 근로계약서 목록
        /// </summary>
        /// <param name="Apt_Code"></param>
        /// <returns></returns>
        public async Task<List<Labor_contract_Entity>> Contract_list_Apt_All()
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                var Lsit = await ctx.QueryAsync<Labor_contract_Entity>("Select Aid, Apt_Code, Apt_Name, UserID, UserName, Adress, Mobile, Telephone, PartTime, LaborStartDate, LaborEndDate, WorkPlace, WorkDetail, BasicsPay, Pay_A, Pay_B, Pay_C, Pay_D, Pay_E, Pay_F, TotalSum, Pay_Etc, WorktimeSort, Worktime, WorkTimeWeekend, WorkMonthTime, WorkTimeEtc, StartWorkTime, EndWorkTime, BreaktimeSort, BreaktimeDaytime, BreaktimeNight, BreakTimeEtc, Holiday, RetirementAge, Read_approval, Read_Approval1, Copy_approval, Contract_Etc, WorkContract_Date, Cor_Code, Ceo_Adress, Ceo_Telephone, Ceo_Company, Ceo_Name, ContractNotice, ContractApprovalDivision, PostDate, PostIP, Division From Labor_contract Order By Aid Desc", commandType: CommandType.Text);
                return Lsit.ToList();
            }
            
        }

        /// <summary>
        /// 단지별 근로계약서 목록
        /// </summary>
        /// <param name="Apt_Code"></param>
        /// <returns></returns>
        public async Task<List<Labor_contract_Entity>> Contract_list_A()
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                var Lsit = await ctx.QueryAsync<Labor_contract_Entity>("Select Aid, Apt_Code, Apt_Name, UserID, UserName, Adress, Mobile, Telephone, PartTime, LaborStartDate, LaborEndDate, WorkPlace, WorkDetail, BasicsPay, Pay_A, Pay_B, Pay_C, Pay_D, Pay_E, Pay_F, TotalSum, Pay_Etc, WorktimeSort, Worktime, WorkTimeWeekend, WorkMonthTime, WorkTimeEtc, StartWorkTime, EndWorkTime, BreaktimeSort, BreaktimeDaytime, BreaktimeNight, BreakTimeEtc, Holiday, RetirementAge, Read_approval, Read_Approval1, Copy_approval, Contract_Etc, WorkContract_Date, Cor_Code, Ceo_Adress, Ceo_Telephone, Ceo_Company, Ceo_Name, ContractNotice, ContractApprovalDivision, PostDate, PostIP, Division From Labor_contract Where ContractApprovalDivision = 'A' Order By Aid Desc", commandType: CommandType.Text);
                return Lsit.ToList();
            }
            
        }

        /// <summary>
        /// 단지별 근로계약서 목록
        /// </summary>
        /// <param name="Apt_Code"></param>
        /// <returns></returns>
        public async Task<List<Labor_contract_Entity>> Contract_list_C()
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                var Lsit = await ctx.QueryAsync<Labor_contract_Entity>("Select Aid, Apt_Code, Apt_Name, UserID, UserName, Adress, Mobile, Telephone, PartTime, LaborStartDate, LaborEndDate, WorkPlace, WorkDetail, BasicsPay, Pay_A, Pay_B, Pay_C, Pay_D, Pay_E, Pay_F, TotalSum, Pay_Etc, WorktimeSort, Worktime, WorkTimeWeekend, WorkMonthTime, WorkTimeEtc, StartWorkTime, EndWorkTime, BreaktimeSort, BreaktimeDaytime, BreaktimeNight, BreakTimeEtc, Holiday, RetirementAge, Read_approval, Read_Approval1, Copy_approval, Contract_Etc, WorkContract_Date, Cor_Code, Ceo_Adress, Ceo_Telephone, Ceo_Company, Ceo_Name, ContractNotice, ContractApprovalDivision, PostDate, PostIP, Division From Labor_contract Where ContractApprovalDivision = 'C' Order By Aid Desc", commandType: CommandType.Text);
                return Lsit.ToList();
            }
            
        }

        /// <summary>
        /// 단지별 근로계약서 수
        /// </summary>
        /// <param name="Apt_Code"></param>
        /// <returns></returns>
        public async Task<int> Contract_count_Apt(string Apt_Code)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                return await ctx.QuerySingleOrDefaultAsync<int>("Select Count(*) From Labor_contract Where Apt_Code = @Apt_Code", new { Apt_Code }, commandType: CommandType.Text);
            }
            
        }

        /// <summary>
        /// 총 근로계약서 수
        /// </summary>
        public async Task<int> Contract_count_All()
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                return await ctx.QuerySingleOrDefaultAsync<int>("Select Count(*) From Labor_contract", commandType: CommandType.Text);
            }
            
        }

        /// <summary>
        /// 단지별 근로계약서 목록
        /// </summary>
        /// <param name="Apt_Code"></param>
        /// <returns></returns>
        public async Task<List<Labor_contract_Entity>> labor_C_list_Apt(int Page, string Apt_Code, string Division)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                var Lsit = await ctx.QueryAsync<Labor_contract_Entity>("Select Top 15 * From Labor_contract Where Aid Not In(Select Top(15 * @Page) Aid From Labor_contract Where Apt_Code = @Apt_Code And Division = @Division Order By Aid Desc) and Apt_Code = @Apt_Code And Division = @Division Order By Aid Desc", new { Page, Apt_Code, Division });
                return Lsit.ToList();
            }
            
        }

        /// <summary>
        /// 단지별 근로계약서 수
        /// </summary>
        /// <param name="Apt_Code"></param>
        /// <returns></returns>
        public async Task<int> labor_C_count_Apt(string Apt_Code, string Division)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                return await ctx.QuerySingleOrDefaultAsync<int>("Select Count(*) From Labor_contract Where Apt_Code = @Apt_Code And Division = @Division", new { Apt_Code, Division }, commandType: CommandType.Text);
            }
           
        }

        /// <summary>
        /// 회사별 근로계약서 목록
        /// </summary>
        /// <param name="Cor_Code"></param>
        /// <returns></returns>
        public async Task<List<Labor_contract_Entity>> labor_C_list_Cor(int Page, string Cor_Code, string Division)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                var Lsit = await ctx.QueryAsync<Labor_contract_Entity>("Select Top 15 * From Labor_contract Where Aid Not In(Select Top(15 * @Page) Aid From Labor_contract Where Cor_Code = @Cor_Code And Division = @Division Order By Aid Desc) and Cor_Code = @Cor_Code And Division = @Division Order By Aid Desc", new { Page, Cor_Code, Division }, commandType: CommandType.Text);
                return Lsit.ToList();
            }
            
        }

        /// <summary>
        /// 회사별 근로계약서 수
        /// </summary>
        /// <param name="Cor_Code"></param>
        /// <returns></returns>
        public async Task<int> labor_C_count_Cor(string Cor_Code, string Division)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                return await ctx.QuerySingleOrDefaultAsync<int>("Select Count(*) From Labor_contract Where Cor_Code = @Cor_Code And Division = @Division", new { Cor_Code, Division }, commandType: CommandType.Text);
            }
           
        }

        /// <summary>
        /// 회사별 단지 근로계약서 목록
        /// </summary>
        /// <param name="Cor_Code"></param>
        /// <returns></returns>
        public async Task<List<Labor_contract_Entity>> labor_C_list_Cor_Apt(int Page, string Cor_Code, string Apt_Code, string Division)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                var Lsit = await ctx.QueryAsync<Labor_contract_Entity>("Select Top 15 * From Labor_contract Where Aid Not In(Select Top(15 * @Page) Aid From Labor_contract Where Cor_Code = @Cor_Code And Apt_Code = @Apt_Code And Division = @Division Order By Aid Desc) and Cor_Code = @Cor_Code And Apt_Code = @Apt_Code And Division = @Division Order By Aid Desc", new { Page, Cor_Code, Apt_Code, Division }, commandType: CommandType.Text);
                return Lsit.ToList();
            }
            
        }

        /// <summary>
        /// 회사별 단지 근로계약서 수
        /// </summary>
        /// <param name="Cor_Code"></param>
        /// <returns></returns>
        public async Task<int> labor_C_count_Cor_Apt(string Cor_Code, string Apt_Code, string Division)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                return await ctx.QuerySingleOrDefaultAsync<int>("Select Count(*) From Labor_contract Where Cor_Code = @Cor_Code And Apt_Code = @Apt_Code And Division = @Division", new { Cor_Code, Apt_Code, Division }, commandType: CommandType.Text);
            }
            
        }

        /// <summary>
        /// 식별코드로 상세보기
        /// </summary>
        /// <param name="Aid"></param>
        /// <returns></returns>
        public async Task<Labor_contract_Entity> labor_Contract_detail(string Aid)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                return await ctx.QuerySingleOrDefaultAsync<Labor_contract_Entity>("Select Aid, Apt_Code, Apt_Name, UserID, UserName, Adress, Mobile, Telephone, PartTime, LaborStartDate, LaborEndDate, WorkDetail, BasicsPay, Pay_A, Pay_B, Pay_C, Pay_D, Pay_E, Pay_F, TotalSum, Pay_Etc, WorkPlace, WorkTime, WorkTimeWeekend, WorkMonthTime, WorkTimeEtc, StartWorkTime, EndWorkTime, BreakTimeSort, BreaktimeDaytime, BreaktimeNight, BreakTimeEtc, Holiday, RetirementAge, Read_Approval, Read_Approval1, Copy_Approval, Contract_Etc, WorkContract_Date, Cor_Code, Ceo_Adress, Ceo_Telephone, Ceo_Company, Ceo_Name, ContractNotice, ContractApprovalDivision, Division, PostDate, PostIP From Labor_contract Where Aid = @Aid", new { Aid }, commandType: CommandType.Text);
            }
            
        }

        /// <summary>
        /// 근로자 아이디로 상세보기(가장 최근 근로 계약서)
        /// </summary>
        /// <param name="Aid"></param>
        /// <returns></returns>
        public async Task<Labor_contract_Entity> labor_Contract_detail_UserID(string UserID)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                return await ctx.QuerySingleOrDefaultAsync<Labor_contract_Entity>("Select Top 1 Aid, Apt_Code, Apt_Name, UserID, UserName, Adress, Mobile, Telephone, PartTime, LaborStartDate, LaborEndDate, WorkDetail, BasicsPay, Pay_A, Pay_B, Pay_C, Pay_D, Pay_E, Pay_F, TotalSum, Pay_Etc, WorkPlace, WorkTime, WorkTimeWeekend, WorkMonthTime, WorkTimeEtc, StartWorkTime, EndWorkTime, BreakTimeSort, BreaktimeDaytime, BreaktimeNight, BreakTimeEtc, RetirementAge, Holiday, Read_Approval, Read_Approval1, Copy_Approval, Contract_Etc, WorkContract_Date, Ceo_Adress, Ceo_Telephone, Ceo_Company, Ceo_Name, ContractNotice, ContractApprovalDivision, Division, PostDate, PostIP From Labor_contract Where UserID = @UserID Order By Aid Desc", new { UserID }, commandType: CommandType.Text);
            }
            
        }

        /// <summary>
        /// 해당 근로 계약서 승인 미승인
        /// </summary>
        /// <param name="Aid"></param>
        /// <param name="Division"></param>
        public async Task Approval(string Aid)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                string re = await ctx.QuerySingleOrDefaultAsync<string>("Select Division From Labor_contract Where Aid = @Aid", new { Aid });
                if (re == "A")
                {
                    await ctx.ExecuteAsync("Update Labor_contract Set Division = 'B', ContractApprovalDivision = 'B' Where Aid = @Aid", new { Aid }, commandType: CommandType.Text);
                }
                else
                {
                    await ctx.ExecuteAsync("Update Labor_contract Set Division = 'A', ContractApprovalDivision = 'A' Where Aid = @Aid", new { Aid }, commandType: CommandType.Text);
                }
            }            
        }

        /// <summary>
        /// 해당 근로자의 근로 계약서 존재 여부 확인
        /// </summary>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public async Task<int> labor_Contract_detail_userID_Being(string UserID)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                return await ctx.QuerySingleOrDefaultAsync<int>("Select Count(*) From Labor_contract Where UserID = @UserID", new { UserID }, commandType: CommandType.Text);
            }
            
        }

        /// <summary>
        /// 삭제
        /// </summary>
        /// <param name="Aid"></param>
        public async Task Remove(string Aid)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                await ctx.ExecuteAsync("Delete Labor_contract Where Aid = @Aid", new { Aid }, commandType: CommandType.Text);
            }
            
        }

        /// <summary>
        /// 첨부 관련 메서드
        /// </summary>
        /// <param name="Aid"></param>
        /// <param name="Division"></param>
        /// <returns></returns>
        public async Task Files_Count_Add(int Aid, string Division)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                if (Division == "A")
                {
                    await ctx.ExecuteAsync("Update Labor_contract Set Files_Count = Files_Count + 1 Where Aid = @Aid", new { Aid });
                }
                else
                {
                    await ctx.ExecuteAsync("Update Labor_contract Set Files_Count = Files_Count - 1 Where Aid = @Aid", new { Aid });
                }
                
            }
        }
    }
}
