using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Threading.Tasks;
//using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Company
{
    /// <summary>
    /// 업체 정보
    /// </summary>
    public class Company_Lib : ICompany_Lib
    {
        private readonly IConfiguration _db;
        public Company_Lib(IConfiguration configuration)
        {
            this._db = configuration;
        }

        /// <summary>
        /// 업체등록
        /// </summary>
        public async Task<int> Add(Company_Entity ci)
        {
            var sql = "Insert into Company (Cor_Code, Cor_Name, CorporateResistration_Num, Adress_Sido, Adress_GunGu, Adress_Rest, Mobile, Telephone, Ceo_Name, LevelCount, Intro, PostIP, User_Code) Values (@Cor_Code, @Cor_Name, @CorporateResistration_Num, @Adress_Sido, @Adress_GunGu, @Adress_Rest, @Mobile, @Telephone, @Ceo_Name, @LevelCount, @Intro, @PostIP, @User_Code); Select Cast(SCOPE_IDENTITY() As Int);";
            using (var dba = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                int Being = await dba.QuerySingleOrDefaultAsync<int>(sql, ci);
                return Being; 
            }
        }

        /// <summary>
        /// 업체 정보 수정
        /// </summary>
        /// <param name="ci"></param>
        /// <returns></returns>
        public async Task<Company_Entity> edit(Company_Entity ci)
        {
            var sql = "Update Company Set Cor_Name = @Cor_Name, Adress_Sido = @Adress_Sido, Adress_GunGu = @Adress_GunGu, Adress_Rest = @Adress_Rest, Mobile = @Mobile, Telephone = @Telephone, CorporateResistration_Num = @CorporateResistration_Num, Ceo_Name = @Ceo_Name, LevelCount = @LevelCount, Intro = @Intro Where Aid = @Aid";
            using (var dba = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                await dba.ExecuteAsync(sql, ci);
                return ci; 
            }
        }

        /// <summary>
        /// 사업자등록번호 중복확인
        /// </summary>
        public async Task<int> CorNum_Being(string CorporateResistration_Num)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                return await dba.QuerySingleOrDefaultAsync<int>("Select Count(*) From Company Where CorporateResistration_Num = @CorporateResistration_Num", new { CorporateResistration_Num }); 
            }
        }

        /// <summary>
        /// 사업자등록번호로 상세 정보
        /// </summary>
        public async Task<Company_Entity> CorNum_Detail(string CorporateResistration_Num)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                return await dba.QuerySingleOrDefaultAsync<Company_Entity>("Select * From Company Where CorporateResistration_Num = @CorporateResistration_Num", new { CorporateResistration_Num }); 
            }
        }

        /// <summary>
        /// 업체명 불러오기(사업자 번호)
        /// </summary>
        /// <param name="CorporateResistration_Num"></param>
        /// <returns></returns>
        public async Task<string> Cor_name(string CorporateResistration_Num)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                return await dba.QuerySingleOrDefaultAsync<string>("Select Cor_Name From Company Where CorporateResistration_Num = @CorporateResistration_Num", new { CorporateResistration_Num }); 
            }
        }

        /// <summary>
        /// 등록된 업체 목록(시도)
        /// </summary>
        public async Task<List<Company_Entity>> GetList_Sido(string Sido)
        {

            using (var dba = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                var lst = await dba.QueryAsync<Company_Entity>("Select Aid, Cor_Code, Cor_Name, CorporateResistration_Num, Adress_Sido, Adress_GunGu, Adress_Rest, Mobile, Telephone, Ceo_Name, LevelCount, PostDate, PostIP, Intro From Company Where Adress_Sido = @Sido Order By Aid Desc", new { Sido });
                return lst.ToList(); 
            }
        }

        /// <summary>
        /// 등록된 업체 수
        /// </summary>
        public async Task<int> GetList_Sido_Count(string Sido)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                return await dba.QuerySingleOrDefaultAsync<int>("Select Count(*) From Company Where Adress_Sido = @Sido", new { Sido }); 
            }
        }

        /// <summary>
        /// 등록된 업체 목록
        /// </summary>
        public async Task<List<Company_Entity>> GetList()
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                var lst = await dba.QueryAsync<Company_Entity>("Select Aid, Cor_Code, Cor_Name, CorporateResistration_Num, Adress_Sido, Adress_GunGu, Adress_Rest, Mobile, Telephone, Ceo_Name, LevelCount, PostDate, PostIP, Intro From Company Order By Aid Desc");
                return lst.ToList(); 
            }
        }

        /// <summary>
        /// 업체 목록(Page)
        /// </summary>
        /// <param name="Page"></param>
        /// <returns></returns>
        public async Task<List<Company_Entity>> List_Page(int Page)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                var lst = await dba.QueryAsync<Company_Entity>("Select Top 15 * From Company Where Aid Not In(Select Top(15 * @Page) Aid From Company Order By Aid Desc) Order By Aid Desc", new { Page });
                return lst.ToList();
            }
        }

        /// <summary>
        /// 등록된 업체 수
        /// </summary>
        public async Task<int> GetList_Count()
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                return await dba.QuerySingleOrDefaultAsync<int>("Select Count(*) From Company"); 
            }
        }

        /// <summary>
        /// 업체 찾기
        /// </summary>
        /// <param name="Feild"></param>
        /// <param name="Query"></param>
        /// <returns></returns>
        public async Task<List<Company_Entity>> Search_List(int Page, string Feild, string Query)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                var lst = await dba.QueryAsync<Company_Entity>("Select Top 15 * From Company Where Aid Not In(Select Top(15 * @Page) Aid From Company Where " + Feild + " Like '%" + Query + "%' Order By Aid Desc) and " + Feild + " Like '%" + Query + "%' Order By Aid Desc", new { Page, Feild, Query });
                return lst.ToList(); 
            }
        }

        /// <summary>
        /// 등록된 업체 수
        /// </summary>
        public async Task<int> Search_List_Count(string Feild, string Query)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                return await dba.QuerySingleOrDefaultAsync<int>("Select Count(*) From Company Where " + Feild + " Like '%" + Query + "%'");
            }
        }

        /// <summary>
        /// 업체 찾기(사업자번호)
        /// </summary>
        /// <param name="Feild"></param>
        /// <param name="Query"></param>
        /// <returns></returns>
        public async Task<Company_Entity> Search_Cpr_Details(string Query)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                return await dba.QuerySingleOrDefaultAsync<Company_Entity>("Select Aid, Cor_Code, Cor_Name, CorporateResistration_Num, Adress_Sido, Adress_GunGu, Adress_Rest, Mobile, Telephone, Ceo_Name, LevelCount, PostDate, PostIP, Intro From Company Where CorporateResistration_Num = @Query Order By Aid Desc", new { Query }); 
            }
        }

        /// <summary>
        /// 업체 찾은 목록(사업자번호)
        /// </summary>
        /// <param name="Query"></param>
        /// <returns></returns>
        public async Task<List<Company_Entity>> List_Cpr_Details(string Query)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                var lst = await dba.QueryAsync<Company_Entity>("Select Aid, Cor_Code, Cor_Name, CorporateResistration_Num, Adress_Sido, Adress_GunGu, Adress_Rest, Mobile, Telephone, Ceo_Name, LevelCount, PostDate, PostIP, Intro From Company Where CorporateResistration_Num = @Query Order By Aid Desc", new { Query });
                return lst.ToList(); 
            }
        }

        /// <summary>
        /// 입력 마지막 번호
        /// </summary>
        public async Task<int> Num_Count()
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                return await dba.QuerySingleOrDefaultAsync<int>("Select Top 1 Aid From Company Order by Aid Desc"); 
            }
        }

        /// <summary>
        /// 업체 기본 정보 상세
        /// </summary>
        /// <param name="Cor_Code"></param>
        /// <returns></returns>
        public async Task<Company_Entity> detail(string Cor_Code)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                return await dba.QuerySingleOrDefaultAsync<Company_Entity>("Select * From Company Where Cor_Code = @Cor_Code", new { Cor_Code }); 
            }
        }

        /// <summary>
        /// 이름으로 찾은 목록
        /// </summary>
        /// <param name="Cor_Name"></param>
        /// <returns></returns>
        public async Task<List<Company_Entity>> Company_Name_List(string Cor_Name)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                var lst = await dba.QueryAsync<Company_Entity>("Select * From Company Where Cor_Name Like '%" + Cor_Name + "%' Order By Aid Desc", new { Cor_Name });
                return lst.ToList(); 
            }
        }

        /// <summary>
        /// 식별코드로 찾은 목록
        /// </summary>
        public async Task<string> Company_Name_Code(string Cor_Code)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                return await dba.QuerySingleOrDefaultAsync<string>("Select Cor_Name From Company Where Cor_Code = @Cor_Code", new { Cor_Code }); 
            }
        }

        

        /// <summary>
        /// 식별코드로 찾은 목록(동기)
        /// </summary>
        public string Company_Name_Code_A(string Cor_Code)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                return dba.QuerySingleOrDefault<string>("Select Cor_Name From Company Where Cor_Code = @Cor_Code", new { Cor_Code });
            }
        }


        /// <summary>
        /// 사업자 번로로 업체 식별코드 찾기
        /// </summary>
        /// <param name="Cor_Code"></param>
        /// <returns></returns>
        public async Task<string> Company_Code(string CorporateResistration_Num)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                return await dba.QuerySingleOrDefaultAsync<string>("Select Cor_Code From Company Where CorporateResistration_Num = @CorporateResistration_Num", new { CorporateResistration_Num }); 
            }
        }

        /// <summary>
        /// 업체 정보 삭제
        /// </summary>
        /// <param name="Cor_Code"></param>
        public async Task Remove(string Cor_Code)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                await dba.ExecuteAsync("Update Company Set Del = 'B' Where Cor_Code = @Cor_Code", new { Cor_Code }); 
            }
        }

        /// <summary>
        /// 업체 정보 삭제
        /// </summary>
        /// <param name="Cor_Code"></param>
        public async Task Delete(string Cor_Code)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                await dba.ExecuteAsync("Delete Company Where Cor_Code = @Cor_Code", new { Cor_Code });
            }
        }
    }

    /// <summary>
    /// 업체 상세 정보
    /// </summary>
    public class Company_Sub_Lib : ICompany_Sub_Lib
    {
        private readonly IConfiguration _db;
        public Company_Sub_Lib(IConfiguration configuration)
        {
            this._db = configuration;
        }

        public async Task<Company_Sub_Entity> Add(Company_Sub_Entity _Entity)
        {
            var sql = "Insert into Company_Sub (Company_Code, Sido, GunGu, Adress, Telephone, Fax, Email, Ceo_Name, Ceo_Mobile, ChargeMan, ChargeMan_Mobile, CapitalStock, CraditRating, TypeOfBusiness, BusinessConditions, Company_Sort, Etc, PostIP) Values (@Company_Code, @Sido, @GunGu, @Adress, @Telephone, @Fax, @Email, @Ceo_Name, @Ceo_Mobile, @ChargeMan, @ChargeMan_Mobile, @CapitalStock, @CraditRating, @TypeOfBusiness, @BusinessConditions, @Company_Sort, @Etc, @PostIP); Select Cast(SCOPE_IDENTITY() As Int)";
            using (var dba = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                var Num = await dba.QuerySingleOrDefaultAsync<int>(sql, _Entity);
                _Entity.Aid = Num;
                return _Entity; 
            }
        }

        /// <summary>
        /// 업체 상세정보 수정
        /// </summary>
        /// <param name="_Entity"></param>
        /// <returns></returns>
        public async Task<Company_Sub_Entity> Edit(Company_Sub_Entity _Entity)
        {
            var sql = "Update Company_Sub Set Sido = @Sido, GunGu = @GunGu, Adress = @Adress, Telephone = @Telephone, Fax = @Fax, Email = @Email, Ceo_Name = @Ceo_Name, Ceo_Mobile = @Ceo_Mobile, ChargeMan = @ChargeMan, ChargeMan_Mobile = @ChargeMan_Mobile, CapitalStock= @CapitalStock, CraditRating = @CraditRating, TypeOfBusiness = @TypeOfBusiness, BusinessConditions = @BusinessConditions, Company_Sort = @Company_Sort, Etc = @Etc, PostIP = @PostIP Where Aid = @Aid";
            using (var dba = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                await dba.QueryAsync(sql, _Entity);
                return _Entity; 
            }
        }

        /// <summary>
        /// 상세보기
        /// </summary>
        /// <param name="Company_Code"></param>
        /// <returns></returns>
        public async Task<Company_Sub_Entity> Detail(string Company_Code)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                return await dba.QuerySingleOrDefaultAsync<Company_Sub_Entity>("Select Top 1 Aid, Company_Code, Sido, GunGu, Adress, Telephone, Fax, Email, Ceo_Name, Ceo_Mobile, ChargeMan, ChargeMan_Mobile, CapitalStock, CraditRating, TypeOfBusiness, BusinessConditions, Company_Sort, Etc, PostDate, PostIP From Company_Sub Where Company_Code = @Company_Code Order By Aid Desc", new { Company_Code }); 
            }
        }
        

        /// <summary>
        /// 업체별 상세정보 이력 정보
        /// </summary>
        /// <param name="Company_Code"></param>
        /// <returns></returns>
        public async Task<List<Company_Sub_Entity>> List(string Company_Code)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                var lst = await dba.QueryAsync<Company_Sub_Entity>("Select Aid Company_Code, Sido, GunGu, Adress, Telephone, Fax, Email, Ceo_Name, Ceo_Mobile, ChargeMan, ChargeMan_Mobile, CapitalStock, CraditRating, TypeOfBusiness, BusinessConditions, Company_Sort, Etc, PostDate, PostIP From Company_Sub Where Company_Code = @Company_Code", new { Company_Code });
                return lst.ToList(); 
            }
        }

        /// <summary>
        /// 해당 업체 정보 존재 여부
        /// </summary>
        /// <param name="Cor_Code"></param>
        /// <returns></returns>
        public async Task<int> being(string Cor_Code)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                return await dba.QuerySingleOrDefaultAsync<int>("Select Count(*) From Company_Sub Where Company_Code = @Cor_Code", new { Cor_Code }); 
            }
        }
    }

    /// <summary>
    /// 업체 및 직원 상세 정보
    /// </summary>
    public class Company_Join_Lib : ICompany_Join_Lib
    {
        private readonly IConfiguration _db;
        public Company_Join_Lib(IConfiguration configuration)
        {
            this._db = configuration;
        }

        /// <summary>
        /// 업체 정보 조인리스트
        /// </summary>
        /// <param name="Sido"></param>
        /// <param name="GunGu"></param>
        /// <returns></returns>
        public async Task<List<Company_Join_Entity>> List_Join(string Sido, string GunGu)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                var lst = await dba.QueryAsync<Company_Join_Entity>("Select * From Company as a Join Company_Sub as b on a.Cor_Code = b.Company_Code Where a.Adress_Sido = @Sido And a.Adress_GunGu Like '" + @GunGu + "%' And a.Del = 'A' Order By b.Aid Desc", new { Sido, GunGu });
                return lst.ToList(); 
            }
        }

        /// <summary>
        /// 업체 목록(Page)
        /// </summary>
        /// <param name="Page"></param>
        /// <returns></returns>
        public async Task<List<Company_Join_Entity>> List_Page(int Page)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                var lst = await dba.QueryAsync<Company_Join_Entity>("Select Top 15 a.Aid, a.Cor_Code, a.Cor_Name, a.CorporateResistration_Num, a.Adress_Sido, a.Adress_GunGu, a.Adress_Rest, a.Mobile, a.Telephone, a.Ceo_Name, a.LevelCount, a.PostDate, a.User_Code, a.PostIP, a.Intro, a.Del, b.Aid as SubAid, b.Company_Code, b.Sido, b.GunGu, b.Adress, b.Telephone, b.Fax, b.Email, b.Ceo_Name, b.Ceo_Mobile, b.ChargeMan, b.ChargeMan_Mobile, b.CapitalStock, b.CraditRating, b.TypeOfBusiness, b.BusinessConditions, b.Company_Sort, b.Etc From Company as a Join Company_Sub as b on a.Cor_Code = b.Company_Code Where a.Aid Not In(Select Top(15 * @Page) a.Aid From Company as a Join Company_Sub as b on a.Cor_Code = b.Company_Code And a.Del = 'A' Order By a.Aid Desc) And a.Del = 'A' Order By a.Aid Desc", new { Page });
                return lst.ToList();
            }
        }

        /// <summary>
        /// 등록된 업체 수
        /// </summary>
        public async Task<int> GetList_Count()
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                return await dba.QuerySingleOrDefaultAsync<int>("Select Count(*) From Company as a Join Company_Sub as b on a.Cor_Code = b.Company_Code Where a.Del = 'A'");
            }
        }

        /// <summary>
        /// 업체 조인 상세정보
        /// </summary>
        /// <param name="Company_Code"></param>
        /// <returns></returns>
        public async Task<Company_Join_Entity> Detail(string Company_Code)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                return await dba.QuerySingleOrDefaultAsync<Company_Join_Entity>("Select top 1 * From Company as a Join Company_Sub as b on a.Cor_Code = b.Company_Code Where a.Cor_Code = @Company_Code And a.Del = 'A' order by b.Aid Desc", new { Company_Code }); 
            }
        }

        public async Task<List<Company_Join_Entity>> List_Join_A(string Company_Sort)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                var lst = await dba.QueryAsync<Company_Join_Entity>("Select * From Company as a Join Company_Sub as b on a.Cor_Code = b.Company_Code Where b.Company_Sort = @Company_Sort And a.Del = 'A' Order By b.Aid Desc", new { Company_Sort });
                return lst.ToList();
            }
        }

        public async Task<List<Company_Join_Entity>> List_Join_B(string Type, string Condition, string Sido)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                var lst = await dba.QueryAsync<Company_Join_Entity>("Select * From Company as a Join Company_Sub as b on a.Cor_Code = b.Company_Code Where b.TypeOfBusiness = @Type And b.BusinessConditions = @Condition And a.Adress_Sido = @Sido Order By b.Aid Desc", new { Type, Condition, Sido });
                return lst.ToList();
            }
        }

        public async Task<List<Company_Join_Entity>> List_Join_C(string Type, string Sido, string GunGu)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                var lst = await dba.QueryAsync<Company_Join_Entity>("Select * From Company as a Join Company_Sub as b on a.Cor_Code = b.Company_Code Where b.TypeOfBusiness = @Type And a.Adress_Sido = @Sido And a.Adress_GunGu Like '" + @GunGu + "%' And a.Del = 'A' Order By b.Aid Desc", new { Type, Sido, GunGu });
                return lst.ToList();
            }
        }

        public async Task<List<Company_Join_Entity>> List_Join_D(string Type, string Condition, string Sido, string GunGu)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                var lst = await dba.QueryAsync<Company_Join_Entity>("Select * From Company as a Join Company_Sub as b on a.Cor_Code = b.Company_Code Where b.TypeOfBusiness = @Type And b.BusinessConditions = @Condition And a.Adress_Sido = @Sido And a.Adress_GunGu Like '" + @GunGu + "%' And a.Del = 'A' Order By b.Aid Desc", new { Type, Condition, Sido, GunGu });
                return lst.ToList();
            }
        }

        public async Task<List<Company_Join_Entity>> List_Join_E(string Company_Sort, string Sido)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                var lst = await dba.QueryAsync<Company_Join_Entity>("Select * From Company as a Join Company_Sub as b on a.Cor_Code = b.Company_Code Where b.Company_Sort = @Company_Sort And a.Adress_Sido = @Sido And a.Del = 'A' Order By b.Aid Desc", new { Company_Sort, Sido });
                return lst.ToList();
            }
        }
        public async Task<List<Company_Join_Entity>> List_Join_F(string Company_Sort, string Sido, string GunGu)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                var lst = await dba.QueryAsync<Company_Join_Entity>("Select * From Company as a Join Company_Sub as b on a.Cor_Code = b.Company_Code Where b.Company_Sort = @Company_Sort And a.Adress_Sido = @Sido And a.Adress_GunGu Like '" + @GunGu + "%' And a.Del = 'A' Order By b.Aid Desc", new { Company_Sort, Sido, GunGu });
                return lst.ToList();
            }
        }      

    }

    /// <summary>
    /// 아파트 계약 정보
    /// </summary>
    public class Company_Apt_Career_Lib : ICompany_Apt_Career_Lib
    {
        private readonly IConfiguration _db;
        public Company_Apt_Career_Lib(IConfiguration configuration)
        {
            this._db = configuration;
        }

        /// <summary>
        /// 계약 정보 입력
        /// </summary>
        /// <param name="cc"></param>
        /// <returns></returns>
        public async Task<Company_Career_Entity> add(Company_Career_Entity cc)
        {
            var sql = "Insert into Company_Career (CC_Code, Cor_Code, Company_Name, Apt_Code, Apt_Name, ContractSort, Tender, Bid, ContractMainAgent, Contract_start_date, Contract_end_date, Contract_Sum, Contract_Period, Intro) Values (@CC_Code, @Cor_Code, @Company_Name, @Apt_Code, @Apt_Name, @ContractSort, @Tender, @Bid, @ContractMainAgent, @Contract_start_date, @Contract_end_date, @Contract_Sum, @Contract_Period, @Intro);";
            using (var dba = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                await dba.ExecuteAsync(sql, cc);
                return cc; 
            }
        }

        /// <summary>
        /// 계약 정보 수정
        /// </summary>
        /// <param name="cc"></param>
        /// <returns></returns>
        public async Task<Company_Career_Entity> edit(Company_Career_Entity cc)
        {
            var sql = "Update Company_Career Set Cor_Code = @Cor_Code, Company_Name = @Company_Name, Apt_Code = @Apt_Code, Apt_Name = @Apt_Name, ContractSort = @ContractSort, Tender = @Tender, Bid = @Bid, ContractMainAgent = @ContractMainAgent, Contract_start_date = @Contract_start_date, Contract_end_date = @Contract_end_date, Division = @Division, Contract_Sum = @Contract_Sum, Contract_Period = @Contract_Period, Intro = @Intro Where Aid = @Aid";
            using (var dba = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                await dba.ExecuteAsync(sql, cc);
                return cc; 
            }
        }

        /// <summary>
        /// 계약 정보 상세보기
        /// </summary>
        /// <param name="CC_Code"></param>
        /// <returns></returns>
        public async Task<Company_Career_Entity> detail(string CC_Code)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                return await dba.QuerySingleOrDefaultAsync<Company_Career_Entity>("Select * From Company_Career Where CC_Code = @CC_Code", new { CC_Code }); 
            }
        }

        /// <summary>
        /// 해당 공동주택 신원주택 위탁계약 정보 존재 여부 확인
        /// </summary>
        public async Task<int> BeApt(string Apt_Code, string Cor_Code)
        {
            using (var db = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                return await db.QuerySingleOrDefaultAsync<int>("Select Count(*) From Company_Career Where Apt_Code = @Apt_Code And Cor_Code = @Cor_Code And DATEDIFF (DD, GETDATE(), Contract_End_date) >= 1", new { Apt_Code, Cor_Code });
            }

        }

        /// <summary>
        /// 위탁계약 코드 찾아 오기
        /// </summary>
        public async Task<Company_Career_Entity> BeAptCompany_Code(string Apt_Code, string Cor_Code)
        {
            using (var db = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                return await db.QuerySingleOrDefaultAsync<Company_Career_Entity>("Select Top 1 Aid, CC_Code, Cor_Code, Company_Name, Apt_Code, Apt_Name, ContractSort, Bid, Tender, ContractMainAgent, Contract_date, Contract_start_date, Contract_end_date, Division, PostDate, Contract_Sum, Contract_Period, Intro, FileCount From Company_Career Where Apt_Code = @Apt_Code And Cor_Code = @Cor_Code And DATEDIFF (DD, GETDATE(), Contract_End_date) >= 1 Order By Aid Desc", new { Apt_Code, Cor_Code });
            }

        }

        /// <summary>
        /// 가장 최근 계약 정보 상세보기(위탁관리만)
        /// </summary>
        /// <param name="CC_Code"></param>
        /// <returns></returns>
        public async Task<Company_Career_Entity> detail_Apt(string Apt_Code, string ContractSort)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                return await dba.QuerySingleOrDefaultAsync<Company_Career_Entity>("Select Top 1 Aid, CC_Code, Cor_Code, Company_Name, Apt_Code, Apt_Name, ContractSort, Bid, Tender, ContractMainAgent, Contract_start_date, Contract_end_date, Division, PostDate, Contract_Sum, Contract_Period, Intro From Company_Career Where Apt_Code = @Apt_Code And ContractSort = @ContractSort Order By Aid Desc", new { Apt_Code, ContractSort }); 
            }
        }

        /// <summary>
        /// 계약 정보 상세보기(일련번호)
        /// </summary>
        /// <param name="Aid"></param>
        /// <returns></returns>
        public async Task<Company_Career_Entity> Details(int Aid)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                return await dba.QuerySingleOrDefaultAsync<Company_Career_Entity>("Select Aid, CC_Code, Cor_Code, Company_Name, Apt_Code, Apt_Name, ContractSort, Bid, Tender, ContractMainAgent, Contract_start_date, Contract_end_date, Division, PostDate, Contract_Sum, Contract_Period, Intro From Company_Career Where Aid = @Aid", new { Aid }); 
            }
        }


        /// <summary>
        /// 계약 정보 전체 목록
        /// </summary>
        /// <returns></returns>
        public async Task<List<Company_Career_Entity>> getlist_all(int Page, string Division)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {

                var lst = await dba.QueryAsync<Company_Career_Entity>("Select Top 15 * From Company_Career Where Aid Not In(Select Top(15 * @Page) Aid From Company_Career Where Division = @Division And ContractSort = 'Cb45' And Cor_Code = @Cor_Code Order By Aid Desc) and Division = @Division And ContractSort = 'Cb45' Order By Aid Desc", new { Page, Division });
                return lst.ToList(); 
            }
        }

        /// <summary>
        /// 계약 정보 전체 수 
        /// </summary>
        /// <param name="Division"></param>
        /// <returns></returns>
        public async Task<int> Getcount_all(string Division)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                return await dba.QuerySingleOrDefaultAsync<int>("Select Count(*) From Company_Career Where Division = @Division And ContractSort = 'Cb45' And Cor_Code = @Cor_Code", new { Division }); 
            }
        }

        /// <summary>
        /// 계약 정보 전체 목록 2022-10-13
        /// </summary>
        public async Task<List<Company_Career_Entity>> getlist_apt_all(int Page, string Cor_Code)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                //string db = DateTime.Now.ToShortDateString();
                //db = db + " 23:59:59.993";
                var lst = await dba.QueryAsync<Company_Career_Entity>("Select Top 15 * From Company_Career Where Aid Not In(Select Top(15 * @Page) Aid From Company_Career Where ContractSort = 'Cb45' And Cor_Code = @Cor_Code And Contract_end_Date >= '" + DateTime.Now.ToShortDateString() + "' Order By Aid Desc) and ContractSort = 'Cb45' And Division = 'A' And Cor_Code = @Cor_Code And Division = 'A' And Contract_end_Date >= '" + DateTime.Now.ToShortDateString() + "' Order By Aid Desc", new { Page, Cor_Code });
                return lst.ToList();
            }
        }


        /// <summary>
        /// 가장 최근 계약 정보 상세보기(위탁관리만)
        /// </summary>
        /// <param name="CC_Code"></param>
        /// <returns></returns>
        public async Task<int> detail_Apt_Count(string Apt_Code, string ContractSort, string Contract_start_date, string Contract_end_date)
        {
            using (var db = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                return await db.QuerySingleOrDefaultAsync<int>("Select Count(*) From Company_Career Where Apt_Code = @Apt_Code And ContractSort = @ContractSort And Contract_start_date <= @Contract_start_date And Contract_end_date >= @Contract_end_date And Division = 'A'", new { Apt_Code, ContractSort, Contract_start_date, Contract_end_date });
            }
        }

        /// <summary>
        /// 계약 정보 전체 수 2022-10-13
        /// </summary>
        public async Task<int> Getcount_apt_all(string Cor_Code)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                return await dba.QuerySingleOrDefaultAsync<int>("Select Count(*) From Company_Career Where ContractSort = 'Cb45' And Cor_Code = @Cor_Code And Division = 'A' And Contract_end_Date >= '" + DateTime.Now.ToShortDateString() + "'", new { Cor_Code });
            }
        }

        /// <summary>
        /// 계약 정보 옵션 목록
        /// </summary>
        /// <returns></returns>
        public async Task<List<Company_Career_Entity>> getlist_option(int Page, string Feild, string Query, string Division)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                var lst = await dba.QueryAsync<Company_Career_Entity>("Select Top 15 * From Company_Career Where Aid Not In(Select Top(15 * @Page) Aid From Company_Career Where " + Feild + " = @Query And Division = @Division Order By Aid Desc) and " + Feild + " = @Query And Division = @Division Order By Aid Desc", new { Page, Feild, Query, Division });
                return lst.ToList(); 
            }
        }

        /// <summary>
        /// 계약 정보 옵션 수
        /// </summary>
        /// <param name="Division"></param>
        /// <returns></returns>
        public async Task<int> Getcount_option(string Feild, string Query, string Division)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                return await dba.QuerySingleOrDefaultAsync<int>("Select Count(*) From Company_Career Where " + Feild + " = @Query And Division = @Division", new { Feild, Query, Division }); 
            }
        }

        /// <summary>
        /// 계약 정보 옵션 목록(2020)
        /// </summary>
        /// <returns></returns>
        public async Task<List<Company_Career_Entity>> GetList_option_new(string Apt_Code, string Division)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                var lst = await dba.QueryAsync<Company_Career_Entity>("Select Aid, CC_Code, Cor_Code, Company_Name, Apt_Code, Apt_Name, ContractSort, Bid, Tender, ContractMainAgent, Contract_start_date, Contract_end_date, PostDate, Contract_Sum, Contract_Period, Intro From Company_Career Where Apt_Code = @Apt_Code And Division = @Division Order By Aid Desc", new { Apt_Code, Division });
                return lst.ToList(); 
            }
        }

        /// <summary>
        /// 계약 정보 옵션 수(2020)
        /// </summary>
        /// <param name="Division"></param>
        /// <returns></returns>
        public async Task<int> Getcount_option_new(string Apt_Code, string Division)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                return await dba.QuerySingleOrDefaultAsync<int>("Select Count(*) From Company_Career Where Apt_Code = @Apt_Code And Division = @Division", new { Apt_Code, Division }); 
            }
        }

        /// <summary>
        /// 계약 정보 분류 옵션 목록(2020)
        /// </summary>
        /// <returns></returns>
        public async Task<List<Company_Career_Entity>> GetList_option_Sort_new(string Apt_Code, string Feild, string Query, string Division)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                var lst = await dba.QueryAsync<Company_Career_Entity>("Select Aid, CC_Code, Cor_Code, Company_Name, Apt_Code, Apt_Name, ContractSort, Bid, Tender, ContractMainAgent, Contract_start_date, Contract_end_date, PostDate, Contract_Sum, Contract_Period, Intro From Company_Career Where " + Feild + " = @Query And Division = @Division Order By Aid Desc", new { Apt_Code, Feild, Query, Division });
                return lst.ToList(); 
            }
        }

        /// <summary>
        /// 계약 정보 분류 옵션 수(2020)
        /// </summary>
        /// <param name="Division"></param>
        /// <returns></returns>
        public async Task<int> Getcount_option_Sort_new(string Apt_Code, string Feild, string Query, string Division)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                return await dba.QuerySingleOrDefaultAsync<int>("Select Count(*) From Company_Career Where Apt_Code = @Apt_Code And " + Feild + " = @Query And Division = @Division", new { Feild, Query, Division }); 
            }
        }

        /// <summary>
        /// 계약 정보 검색된 목록
        /// </summary>
        /// <returns></returns>
        public async Task<List<Company_Career_Entity>> getlist_search(int Page, string Feild, string Query, string Division)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                var lst = await dba.QueryAsync<Company_Career_Entity>("Select Top 15 * From Company_Career Where Aid Not In(Select Top(15 * @Page) Aid From Company_Career Where " + Feild + " Like '%" + @Query + "%' And Division = @Division Order By Aid Desc) and " + Feild + " Like '%" + @Query + "%' And Division = @Division Order By Aid Desc", new { Page, Feild, Query, Division });
                return lst.ToList(); 
            }
        }

        /// <summary>
        /// 계약 정보 검색된 수
        /// </summary>
        /// <param name="Division"></param>
        /// <returns></returns>
        public async Task<int> Getcount_search(string Feild, string Query, string Division)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                return await dba.QuerySingleOrDefaultAsync<int>("Select Count(*) From Company_Career Where " + Feild + " Like '%" + @Query + "%' And Division = @Division", new { Feild, Query, Division }); 
            }
        }

        /// <summary>
        /// 공동주택명으로 계약정보 목록 만들기
        /// </summary>
        /// <param name="Apt_Name"></param>
        /// <returns></returns>
        public async Task<List<Company_Career_Entity>> getlist_name_search(string Apt_Name)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                var lst = await dba.QueryAsync<Company_Career_Entity>("Select DISTINCT Apt_Name From Company_Career Where Apt_Name Like '%" + Apt_Name + "%'", new { Apt_Name });
                return lst.ToList(); 
            }
        }

        /// <summary>
        /// 식별코드로 계약정보 목록 만들기
        /// </summary>
        /// <param name="Apt_Name"></param>
        /// <returns></returns>
        public async Task<List<Company_Career_Entity>> getlist_name(string Apt_Name)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                var lst = await dba.QueryAsync<Company_Career_Entity>("Select * From Company_Career Where Apt_Name = @Apt_Name Order By Aid Desc", new { Apt_Name }); 
                return lst.ToList(); 
            }
        }

        /// <summary>
        /// 공동주택명으로 찾은 수
        /// </summary>
        /// <param name="Apt_Name"></param>
        /// <returns></returns>
        public async Task<int> apt_name_count(string Apt_Name)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                return await dba.QuerySingleOrDefaultAsync<int>("Select Count(*) From Company_Career Where Apt_Name = @Apt_Name", new { Apt_Name }); 
            }
        }

        /// <summary>
        /// 해당 년도와 월로 계약 정보 존재 여부
        /// </summary>
        /// <param name="Apt_Code"></param>
        /// <param name="Cor_Code"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        public async Task<int> be_date(string Apt_Code, string Cor_Code, string ContractSort, string Contract_start_date, string Contract_end_date)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                return await dba.QuerySingleOrDefaultAsync<int>("Select Count(*) From Company_Career Where Apt_Code = @Apt_Code And Cor_Code = @Cor_Code And ContractSort = @ContractSort And Contract_start_date >= @Contract_start_date And Contract_end_date <= @Contract_end_date And Division = 'A'", new { Apt_Code, Cor_Code, ContractSort, Contract_start_date, Contract_end_date }); 
            }
        }

        /// <summary>
        /// 해당 식별코드로 계약 정보 존재 여부
        /// </summary>
        /// <param name="Apt_Code"></param>
        /// <param name="Cor_Code"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        public async Task<int> be_Code(int Aid)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                return await dba.QuerySingleOrDefaultAsync<int>("Select Count(*) From Company_Career Where Aid = @Aid", new { Aid }); 
            }
        }

        /// <summary>
        /// 삭제
        /// </summary>
        /// <param name="Aid"></param>
        public async Task delete(string Aid, string Division)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                await dba.ExecuteAsync("Update Company_Career Set Division = @Division Where Aid = @Aid", new { Aid, Division }); 
            }
        }

        /// <summary>
        /// 계약일자 목록
        /// </summary>
        /// <param name="Apt_Code"></param>
        /// <returns></returns>
        public async Task<List<Company_Career_Entity>> ListDrop(string Apt_Code, string ContractSort_Code)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                var lst = await dba.QueryAsync<Company_Career_Entity>("Select a.CC_Code, CONVERT(Varchar, a.Contract_end_date, 23) as End_Date  from Company_Career as a Join Contract_Sort as b on a.ContractSort = b.ContractSort_Code where -60 < (Select DATEDIFF (DD, GETDATE(), a.Contract_end_date)) And a.Apt_Code = @Apt_Code And a.ContractSort = @ContractSort_Code Order By a.Aid Desc", new { Apt_Code, ContractSort_Code });
                return lst.ToList(); 
            }
        }

        /// <summary>
        /// 마지막 일련번호
        /// </summary>
        /// <returns></returns>
        public async Task<string> LastCount()
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                return await dba.QuerySingleOrDefaultAsync<string>("Select Top 1 Aid From Company_Career Order By Aid Desc"); 
            }
        }


        /// <summary>
        // 앞 업무 정보
        /// </summary>
        /// <param name="AptCode"></param>
        /// <param name="Num"></param>
        /// <returns></returns>
        public async Task<string> svAgo(string Apt_Code, string Aid)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                return await dba.QuerySingleOrDefaultAsync<string>("Select ISNULL(Aid, 0) From Company_Career Where Apt_Code = @Apt_Code and Aid = (Select max(Aid) From Company_Career Where Apt_Code = @Apt_Code and Aid < @Aid)", new { Apt_Code, Aid }); 
            }
        }

        /// <summary>
        // 앞 업무  존재여부
        /// </summary>
        /// <param name="AptCode"></param>
        /// <param name="Num"></param>
        /// <returns></returns>
        public async Task<int> svAgoBe(string Apt_Code, string Aid)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                return await dba.QuerySingleOrDefaultAsync<int>("Select Count(*) From Company_Career Where Apt_Code = @Apt_Code and Aid = (Select max(Aid) From Company_Career Where Apt_Code = @Apt_Code and Aid < @Aid)", new { Apt_Code, Aid }); 
            }
        }

        /// <summary>
        // 뒤 업무
        /// </summary>
        /// <param name="AptCode"></param>
        /// <param name="Num"></param>
        /// <returns></returns>
        public async Task<string> svNext(string Apt_Code, string Aid)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                return await dba.QuerySingleOrDefaultAsync<string>("Select ISNULL(Aid, 0) From Company_Career Where Apt_Code = @Apt_Code and Aid = (Select Min(Aid) From Company_Career Where Apt_Code =@Apt_Code and Aid > @Aid)", new { Apt_Code, Aid }); 
            }
        }

        /// <summary>
        // 뒤 업무 존재 여부
        /// </summary>
        /// <param name="AptCode"></param>
        /// <param name="Num"></param>
        /// <returns></returns>
        public async Task<int> svNextBe(string Apt_Code, string Aid)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                return await dba.QuerySingleOrDefaultAsync<int>("Select Count(*) From Company_Career Where Apt_Code = @Apt_Code and Aid = (Select Min(Aid) From Company_Career Where Apt_Code =@Apt_Code and Aid > @Aid)", new { Apt_Code, Aid }); 
            }
        }

        /// <summary>
        /// 계약 분류 식별코드로 계약정보 목록 만들기 2022-10-13
        /// </summary>
        public async Task<List<Company_Career_Entity>> getlist(int Page, string ContractSort, string Cor_Code)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                var lst = await dba.QueryAsync<Company_Career_Entity>("Select Top 15 * From Company_Career Where Aid Not In(Select Top(15 * @Page) Aid From Company_Career Where ContractSort = @ContractSort And Cor_Code = @Cor_Code Order By Aid Desc) and ContractSort = @ContractSort And Cor_Code = @Cor_Code Order By Aid Desc", new { Page, ContractSort, Cor_Code });
                return lst.ToList();
            }
        }

        /// <summary>
        /// 계약 분류 식별코드로 계약정보 목록으로 찾은 수 2022-10-13
        /// </summary>
        public async Task<int> getlist_count(string ContractSort, string Cor_Code)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                return await dba.QuerySingleOrDefaultAsync<int>("Select Count(*) From Company_Career Where ContractSort = @ContractSort And Cor_Code = @Cor_Code", new { ContractSort, Cor_Code });
            }
        }

        /// <summary>
        /// 계약 정보 옵션 목록
        /// </summary>
        /// <returns></returns>
        public async Task<List<Company_Career_Entity>> getlist_option(int Page, string Feild, string Query)
        {
            using (var db = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                var lst = await db.QueryAsync<Company_Career_Entity>("Select Top 15 * From Company_Career Where Aid Not In(Select Top(15 * @Page) Aid From Company_Career Where " + Feild + " = @Query And Division = 'A' Order By Aid Desc) and " + Feild + " = @Query And Division = 'A' Order By Aid Desc", new { Page, Feild, Query });
                return lst.ToList();
            }

        }

        /// <summary>
        /// 계약 정보 옵션 수
        /// </summary>
        /// <param name="Division"></param>
        /// <returns></returns>
        public async Task<int> Getcount_option(string Feild, string Query)
        {
            var sql = "Select Count(*) From Company_Career Where " + Feild + " = @Query And Division = 'A'";
            using (var db = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                return await db.QuerySingleOrDefaultAsync<int>(sql, new { Feild, Query });
            }

        }

        /// <summary>
        /// 지나온 날짜 계산
        /// </summary>
        public int Date_scomp(string start, string end)
        {
            using var df = new SqlConnection(_db.GetConnectionString("sw_togather"));
            return df.QuerySingleOrDefault<int>("Select DATEDIFF(DD, @start, @end)", new { start, end });

        }

        /// <summary>
        /// 첨부파일 추가
        /// </summary>
        public async Task File_Plus(int Aid)
        {
            using var df = new SqlConnection(_db.GetConnectionString("sw_togather"));
            await df.ExecuteAsync("Update Company_Career Set FileCount = FileCount + 1 Where Aid = @Aid", new { Aid });
        }

        /// <summary>
        /// 첨부파일 추가
        /// </summary>
        public async Task File_Minus(int Aid)
        {
            using var df = new SqlConnection(_db.GetConnectionString("sw_togather"));
            await df.ExecuteAsync("Update Company_Career Set FileCount = FileCount - 1 Where Aid = @Aid", new { Aid });
        }

    }
}

