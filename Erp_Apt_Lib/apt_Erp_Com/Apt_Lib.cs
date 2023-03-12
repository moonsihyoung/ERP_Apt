using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Erp_Entity;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Data;
//using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Erp_Lib
{
    /// <summary>
    /// 공동주택 정보 클래스
    /// </summary>
    public class Apt_Lib : IApt_Lib
    {
        private readonly IConfiguration _db;
        public Apt_Lib(IConfiguration configuration)
        {
            this._db = configuration;
        }

        /// <summary>
        /// 공동주택 정보 새로 입력
        /// </summary>
        /// <param name="apt"></param>
        /// <returns></returns>
        public async Task<int> Add(Apt_Entity apt)
        {
            var sql = "Insert Apt (Apt_Code, Apt_Name, Apt_Form, Dong_Num, Apt_Adress_Sido, Apt_Adress_Gun, Apt_Adress_Rest, CorporateResistration_Num, AcceptancedOfWork_Date, HouseHold_Num, PostIP, LevelCount, Combine, Intro) values (@Apt_Code, @Apt_Name, @Apt_Form, @Dong_Num, @Apt_Adress_Sido, @Apt_Adress_Gun, @Apt_Adress_Rest, @CorporateResistration_Num, @AcceptancedOfWork_Date, @HouseHold_Num, @PostIP, @LevelCount, @Combine, @Intro); Select Cast(SCOPE_IDENTITY() As Int);";
            using var df = new SqlConnection(_db.GetConnectionString("sw_togather"));
            apt.Aid = await df.ExecuteAsync(sql, apt);
            return apt.Aid;

        }

        /// <summary>
        /// 입력된 아파트 정보 수정
        /// </summary>
        /// <param name="apt"></param>
        public async Task Edit(Apt_Entity apt)
        {
            var sql = "Update Apt Set Apt_Name = @Apt_Name, Apt_Form = @Apt_Form, Dong_Num = @Dong_Num, Apt_Adress_Sido = @Apt_Adress_Sido, Apt_Adress_Gun =  @Apt_Adress_Gun, Apt_Adress_Rest = @Apt_Adress_Rest, CorporateResistration_Num = @CorporateResistration_Num, AcceptancedOfWork_Date = @AcceptancedOfWork_Date, HouseHold_Num = @HouseHold_Num, PostIP = @PostIP, LevelCount = @LevelCount, Combine = @Combine, Intro = @Intro Where Aid = @Aid";
            using (var df = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                await df.ExecuteAsync(sql, apt);
            }

        }

        /// <summary>
        /// 해당 공동주택 정보 상세
        /// </summary>
        public async Task<DateTime> Apt_BuildDate(string Apt_Code)
        {
            using (var df = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                return await df.QuerySingleOrDefaultAsync<DateTime>("Select AcceptancedOfWork_Date From Apt Where Apt_Code = @Apt_Code", new { Apt_Code });
            }

        }

        /// <summary>
        /// 해당 공동주택 정보 상세
        /// </summary>
        /// <param name="Aid"></param>
        /// <returns></returns>
        public async Task<Apt_Entity> Detail(string Aid)
        {
            using (var df = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                return await df.QuerySingleOrDefaultAsync<Apt_Entity>("Select AId, Apt_Code, Apt_Name, Apt_Form, Dong_Num, Apt_Adress_Sido, Apt_Adress_Gun, Apt_Adress_Rest, CorporateResistration_Num, AcceptancedOfWork_Date, HouseHold_Num, PostDate, PostIP, LevelCount, Combine, Intro From Apt Where AId = @Aid", new { Aid });
            }

        }

        /// <summary>
        /// 해당 공동주택 정보 상세
        /// </summary>
        /// <param name="Aid"></param>
        /// <returns></returns>
        public async Task<Apt_Entity> Details(string AptCode)
        {
            using (var df = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                return await df.QuerySingleOrDefaultAsync<Apt_Entity>("Select AId, Apt_Code, Apt_Name, Apt_Form, Dong_Num, Apt_Adress_Sido, Apt_Adress_Gun, Apt_Adress_Rest, CorporateResistration_Num, AcceptancedOfWork_Date, HouseHold_Num, PostDate, PostIP, LevelCount, Combine, Intro From Apt Where Apt_Code = @AptCode", new { AptCode });
            }

        }


        /// <summary>
        /// 해당 코드로 존재여부 확인
        /// </summary>
        /// <returns></returns>
        public async Task<int> Be_Count(string Apt_Code)
        {
            using (var df = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                return await df.QuerySingleOrDefaultAsync<int>("Select Count(*) From Apt Where Apt_Code = @Apt_Code", new { Apt_Code });
            }

        }

        /// <summary>
        /// 입력된 공동주택 정보 목록
        /// </summary>
        /// <param name="Page"></param>
        /// <returns></returns>
        public async Task<List<Apt_Entity>> GetList(int Page)
        {
            using (var df = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                var lst = await df.QueryAsync<Apt_Entity>("Select Top 15 AId, Apt_Code, Apt_Name, Apt_Form, Dong_Num, Apt_Adress_Sido, Apt_Adress_Gun, Apt_Adress_Rest, CorporateResistration_Num, AcceptancedOfWork_Date, HouseHold_Num, PostDate, PostIP, LevelCount, Combine, Intro From Apt Where AId Not In (Select Top (15 * @Page) AId From Apt Order By AId Desc) Order By AId Desc", new { Page });
                return lst.ToList();
            }

        }

        /// <summary>
        /// 최근 전체 목록 
        /// </summary>
        /// <returns></returns>
        public async Task<List<Apt_Entity>> GetList_All()
        {
            using (var df = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                var lst = await df.QueryAsync<Apt_Entity>("Select AId, Apt_Code, Apt_Name, Apt_Form, Dong_Num, Apt_Adress_Sido, Apt_Adress_Gun, Apt_Adress_Rest, CorporateResistration_Num, AcceptancedOfWork_Date, HouseHold_Num, PostDate, PostIP, LevelCount, Combine, Intro From Apt Order By AId Desc");
                return lst.ToList();
            }

        }

        /// <summary>
        /// 최근 전체 목록 수
        /// </summary>
        /// <returns></returns>
        public async Task<int> GetList_All_Count()
        {
            using (var df = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                return await df.QuerySingleOrDefaultAsync<int>("Select Count(*) From Apt");
            }

        }

        /// <summary>
        /// 최근 전체 시도 목록 
        /// </summary>
        /// <returns></returns>
        public async Task<List<Apt_Entity>> GetList_All_Sido(string Apt_Adress_Sido)
        {
            using (var df = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                var lst = await df.QueryAsync<Apt_Entity>("Select AId, Apt_Code, Apt_Name, Apt_Form, Dong_Num, Apt_Adress_Sido, Apt_Adress_Gun, Apt_Adress_Rest, CorporateResistration_Num, AcceptancedOfWork_Date, HouseHold_Num, PostDate, PostIP, LevelCount, Combine, Intro From Apt Where Apt_Adress_Sido = @Apt_Adress_Sido Order By AId Desc", new { Apt_Adress_Sido });
                return lst.ToList();
            }

        }

        /// <summary>
        /// 최근 전체 시도 목록 수
        /// </summary>
        /// <returns></returns>
        public async Task<int> GetList_All_Sido_Count(string Apt_Adress_Sido)
        {
            using (var df = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                return await df.QuerySingleOrDefaultAsync<int>("Select Count(*) From Apt Where Apt_Adress_Sido = @Apt_Adress_Sido", new { Apt_Adress_Sido });
            }

        }

        /// <summary>
        /// 최근 전체 시군구 목록 
        /// </summary>
        /// <returns></returns>
        public async Task<List<Apt_Entity>> GetList_All_Sido_Gun(string Apt_Adress_Sido, string Apt_Adress_Gun)
        {
            using (var df = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                var lst = await df.QueryAsync<Apt_Entity>("Select AId, Apt_Code, Apt_Name, Apt_Form, Dong_Num, Apt_Adress_Sido, Apt_Adress_Gun, Apt_Adress_Rest, CorporateResistration_Num, AcceptancedOfWork_Date, HouseHold_Num, PostDate, PostIP, LevelCount, Combine, Intro From Apt Where Apt_Adress_Sido = @Apt_Adress_Sido And Apt_Adress_Gun = @Apt_Adress_Gun Order By AId Desc", new { Apt_Adress_Sido, Apt_Adress_Gun });
                return lst.ToList();
            }

        }

        /// <summary>
        /// 최근 전체 시군구 목록 
        /// </summary>
        /// <returns></returns>
        public async Task<int> GetList_All_Sido_Gun_Count(string Apt_Adress_Sido, string Apt_Adress_Gun)
        {
            using (var df = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                return await df.QuerySingleOrDefaultAsync<int>("Select Count(*) From Apt Where Apt_Adress_Sido = @Apt_Adress_Sido And Apt_Adress_Gun = @Apt_Adress_Gun", new { Apt_Adress_Sido, Apt_Adress_Gun });
            }

        }

        /// <summary>
        /// 최근 목록 20개
        /// </summary>
        /// <returns></returns>
        public async Task<List<Apt_Entity>> GetList_new()
        {
            using var df = new SqlConnection(_db.GetConnectionString("sw_togather"));
            var lst = await df.QueryAsync<Apt_Entity>("Select Top 20 AId, Apt_Code, Apt_Name, Apt_Form, Dong_Num, Apt_Adress_Sido, Apt_Adress_Gun, Apt_Adress_Rest, CorporateResistration_Num, AcceptancedOfWork_Date, HouseHold_Num, PostDate, PostIP, LevelCount, Combine, Intro From Apt Order By AId Desc");
            return lst.ToList();

        }

        /// <summary>
        /// 입력된 공동주택 정보 목록
        /// </summary>
        /// <param name="Page">페이지</param>
        /// <param name="Apt_Adress_Sido">시도</param>
        /// <returns></returns>
        public async Task<List<Apt_Entity>> GetList_Sido(int Page, string Apt_Adress_Sido)
        {
            using (var df = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                var lst = await df.QueryAsync<Apt_Entity>("Select Top 20 AId, Apt_Code, Apt_Name, Apt_Form, Dong_Num, Apt_Adress_Sido, Apt_Adress_Gun, Apt_Adress_Rest, CorporateResistration_Num, AcceptancedOfWork_Date, HouseHold_Num, PostDate, PostIP, LevelCount, Combine, Intro From Apt Where AId Not In (Select Top (20 * " + Page + ") AId From Apt Where Apt_Adress_Sido = @Apt_Adress_Sido) And Apt_Adress_Sido = @Apt_Adress_Sido Order By AId Desc", new { Page, Apt_Adress_Sido });
                return lst.ToList();
            }

        }

        /// <summary>
        /// 입력된 공동주택 정보 목록
        /// </summary>
        /// <param name="Page">페이지</param>
        /// <param name="Apt_Adress_Sido">시도</param>
        /// <param name="Apt_Adress_Gun">시도</param>
        /// <returns></returns>
        public async Task<List<Apt_Entity>> GetList_Sido_Gun(int Page, string Apt_Adress_Sido, string Apt_Adress_Gun)
        {
            using (var df = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                var lst = await df.QueryAsync<Apt_Entity>("Select Top 20 AId, Apt_Code, Apt_Name, Apt_Form, Dong_Num, Apt_Adress_Sido, Apt_Adress_Gun, Apt_Adress_Rest, CorporateResistration_Num, AcceptancedOfWork_Date, HouseHold_Num, PostDate, PostIP, LevelCount, Combine, Intro From Apt Where AId Not In (Select Top (20 * " + Page + ") AId From Apt Where Apt_Adress_Sido = @Apt_Adress_Sido And Apt_Adress_Gun = @Apt_Adress_Gun) And Apt_Adress_Sido = @Apt_Adress_Sido and Apt_Adress_Gun = @Apt_Adress_Gun Order By AId Desc", new { Page, Apt_Adress_Sido, Apt_Adress_Gun });
                return lst.ToList();
            }

        }

        /// <summary>
        /// 마지막 일련 번호
        /// </summary>
        /// <returns></returns>
        public async Task<int> Aid_Count(string Apt_Adress_Gun)
        {
            using (var df = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                return await df.QuerySingleOrDefaultAsync<int>("Select Count(*) From Apt Where Apt_Adress_Gun = Apt_Adress_Gun", new { Apt_Adress_Gun });
            }

        }

        /// <summary>
        /// 사업자 등록 번호 존재 여부
        /// </summary>
        /// <param name="CorporateResistration_Num"></param>
        /// <returns></returns>
        public async Task<int> Cn_Check(string CorporateResistration_Num)
        {
            using (var df = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                return await df.QuerySingleOrDefaultAsync<int>("Select Count(*) From Apt Where CorporateResistration_Num = @CorporateResistration_Num", new { CorporateResistration_Num });
            }

        }

        /// <summary>
        /// 해당 공동주택 정보를 삭제
        /// </summary>
        /// <param name="AId"></param>
        public async Task Delete(string AId)
        {
            using (var df = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                await df.ExecuteAsync("Delete Apt Where AId = @Aid", new { AId });
            }

        }

        /// <summary>
        /// 마지막 일련 번호
        /// </summary>
        /// <returns></returns>
        public async Task<string> List_Number()
        {
            using (var df = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                return await df.QuerySingleOrDefaultAsync<string>("Select Top 1 Aid From Apt Order By Aid Desc");
            }

        }

        /// <summary>
        /// 사업자번호 체크
        /// </summary>
        public static bool checkCpIdenty(string cpNum)
        {
            cpNum = cpNum.Replace("-", "");
            if (cpNum.Length != 10)
            {
                return false;
            }
            int sum = 0;
            string checkNo = "137137135";

            // 1
            for (int i = 0; i < checkNo.Length; i++)
            {
                sum += (int)Char.GetNumericValue(cpNum[i]) * (int)Char.GetNumericValue(checkNo[i]);
            }

            // 2
            sum += (int)Char.GetNumericValue(cpNum[8]) * 5 / 10;

            // 3
            sum %= 10;

            // 4
            if (sum != 0)
            {
                sum = 10 - sum;
            }

            // 5
            if (sum != (int)Char.GetNumericValue(cpNum[9]))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// 공동주택 식별코드로 공동주택명 불러오기
        /// </summary>
        public async Task<string> Apt_Name(string Apt_Code)
        {
            using (var df = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                return await df.QuerySingleOrDefaultAsync<string>("Select Top 1 Apt_Name From Apt Where Apt_Code = @Apt_Code Order By Aid Desc", new {Apt_Code });
            }
        }

        /// <summary>
        /// 단지 찾기
        /// </summary>
        /// <param name="AptName"></param>
        /// <returns></returns>
        public async Task<List<Apt_Entity>> SearchList(string AptName)
        {
            using var df = new SqlConnection(_db.GetConnectionString("sw_togather"));
            var lst = await df.QueryAsync<Apt_Entity>("Select top 12 * From Apt Where Apt_Name Like '%" + AptName + "%' Order By Apt_Name Asc", new { AptName });
            return lst.ToList();
        }

        /// <summary>
        /// 해당 공동주택에 상세정보 존재 여부
        /// </summary>
        /// <param name="Apt_Code"></param>
        /// <returns></returns>
        //public int Being(string Apt_Code)
        //{
        //    return _Conn.ctx_c.Query<int>("Select Count(*) From Apt Where Apt_Code = @Apt_Code", new { Apt_Code });
        //}
    }

    /// <summary>
    /// 공동주택 상세정보 클래스
    /// </summary>
    public class Apt_Sub_Lib : IApt_Sub_Lib
    {
        private readonly IConfiguration _db;
        public Apt_Sub_Lib(IConfiguration configuration)
        {
            this._db = configuration;
        }

        /// <summary>
        /// 공동주택 상세정보 입력
        /// </summary>
        /// <param name="ast"></param>
        /// <returns></returns>
        public async Task<Apt_Sub_Entity> Add(Apt_Sub_Entity ast)
        {
            var sql = "Insert Apt_Sub ( Apt_Sub_Code, Apt_Code, Developer, Builder, District, Site_Area, Build_Area, FloorTotal_Area, Supply_Area, FloorArea_Ratio, BuildingCoverage_Ratio, Heighest, Heating_Way, WaterSupply_Way, Telephone, Fax, Email, Electric_Supply_Capacity, Water_Quantity, Park_Car_Count, Management_Way, Elevator, Joint_Management, PostIP) values (@Apt_Sub_Code, @Apt_Code, @Developer, @Builder, @District, @Site_Area, @Build_Area, @FloorTotal_Area, @Supply_Area, @FloorArea_Ratio, @BuildingCoverage_Ratio, @Heighest, @Heating_Way, @WaterSupply_Way, @Telephone, @Fax, @Email, @Electric_Supply_Capacity, @Water_Quantity, @Park_Car_Count, @Management_Way, @Elevator, @Joint_Management, @PostIP); Select Cast(SCOPE_IDENTITY() As Int);";
            using (var df = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                ast.Aid = await df.QuerySingleOrDefaultAsync<int>(sql, ast);
                return ast;
            }
        }

        /// <summary>
        /// 공동주택 상세정보 수정
        /// </summary>
        /// <param name="ast"></param>
        public async Task Edit(Apt_Sub_Entity ast)
        {
            var sql = "Update Apt_Sub Set Developer = @Developer, Builder = @Builder, District = @District, Site_Area = @Site_Area, Build_Area= @Build_Area, FloorTotal_Area = @FloorTotal_Area, Supply_Area = @Supply_Area, FloorArea_Ratio = @FloorArea_Ratio, BuildingCoverage_Ratio = @BuildingCoverage_Ratio, Heighest = @Heighest, Heating_Way = @Heating_Way, WaterSupply_Way = @WaterSupply_Way, Telephone = @Telephone, Fax = @Fax, Email = @Email, Electric_Supply_Capacity = @Electric_Supply_Capacity, Water_Quantity = @Water_Quantity, Park_Car_Count = @Park_Car_Count, Management_Way = @Management_Way, Elevator = @Elevator, Joint_Management = @Joint_Management, PostDate = @PostDate, PostIP = @PostIP Where Aid = @Aid";
            using (var df = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                await df.ExecuteAsync(sql, ast);
            }

        }
        /// <summary>
        /// 공동주택 상세정보 보기
        /// </summary>
        /// <param name="Apt_Code"></param>
        /// <returns></returns>
        public async Task<Apt_Sub_Entity> Detail(string Apt_Code)
        {
            using (var df = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                return await df.QuerySingleOrDefaultAsync<Apt_Sub_Entity>("Select Top 1 AId, Apt_Sub_Code, Apt_Code, Developer, Builder, District, Site_Area, Build_Area, FloorTotal_Area, Supply_Area, FloorArea_Ratio, BuildingCoverage_Ratio, Heighest, Heating_Way, WaterSupply_Way, Telephone, Fax, Email, Electric_Supply_Capacity, Water_Quantity, Park_Car_Count, Management_Way, Elevator, Joint_Management, PostDate, PostIP From Apt_Sub Where Apt_Code = @Apt_Code Order By Aid Desc", new { Apt_Code });
            }

        }

        /// <summary>
        /// 해당 공동주택 존재여부 
        /// </summary>
        /// <param name="Apt_Code"></param>
        /// <returns></returns>
        public async Task<int> being(string Apt_Code)
        {
            using (var df = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                return await df.QuerySingleOrDefaultAsync<int>("Select Count(*) From Apt_Sub Where Apt_Code = @Apt_Code", new { Apt_Code });
            }
        }

        /// <summary>
        /// 삭제
        /// </summary>
        /// <param name="Aid"></param>
        /// <returns></returns>
        public async Task Remove(int Aid)
        {
            using (var df = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                 await df.ExecuteAsync("Delete Apt_Sub Where Aid = @Aid", new { Aid });
            }
        }
    }

    /// <summary>
    /// 시도 주소 정보 클래스
    /// </summary>
    public class Sido_Lib : ISido_Lib
    {
        private readonly IConfiguration _db;
        public Sido_Lib(IConfiguration configuration)
        {
            this._db = configuration;
        }

        /// <summary>
        /// 시도 입력
        /// </summary>
        /// <param name="sido"></param>
        /// <returns></returns>
        public async Task<Sido_Entity> Add(Sido_Entity sido)
        {
            using (var df = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                sido.Aid = await df.QuerySingleOrDefaultAsync<int>("Insert Sido (Sido_Code, Sido, Region, Step) values (@Sido_Code, @Sido, @Region, @Step); Select Cast(SCOPE_IDENTITY() As Int);", sido);
                return sido;
            }

        }

        /// <summary>
        /// 시도 수정
        /// </summary>
        /// <param name="sido"></param>
        public async Task Edit(Sido_Entity sido)
        {
            using (var df = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                await df.ExecuteAsync("Update Sido Set Sido_Code = @Sido_Code, Sido = @Sido, Region = @Region, Step = @Step Where Aid = @Aid", sido);
            }

        }

        /// <summary>
        /// 시도로 검색된 군 목록
        /// </summary>
        /// <param name="Sido"></param>
        /// <returns></returns>
        public async Task<List<Sido_Entity>> GetList(string Sido)
        {
            using (var df = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                var lst = await df.QueryAsync<Sido_Entity>("Select Aid, Sido_Code, Sido, Region, Step, PostDate From Sido Where Sido = @Sido", new { Sido });
                return lst.ToList();
            }

        }

        /// <summary>
        /// 시도로 검색된 군 목록
        /// </summary>
        /// <param name="Sido"></param>
        /// <returns></returns>
        public async Task<List<Sido_Entity>> GetList_Code(string Sido)
        {
            using (var df = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                var lst = await df.QueryAsync<Sido_Entity>("Select Aid, Sido_Code, Sido, Region, Step, PostDate From Sido Where Sido_Code Like '" + @Sido + "%'", new { Sido });
                return lst.ToList();
            }

        }

        /// <summary>
        /// 시군구 이름 불러오기
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public async Task<string> GunGoName(string code)
        {
            using (var df = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                return await df.QuerySingleOrDefaultAsync<string>("Select Top 1 Region From Sido Where Sido_Code Like '" + @code + "%'", new { code });
            }

        }

        /// <summary>
        /// 시도 이름 불러오기
        /// </summary>
        public async Task<string> SidoName(string code)
        {
            using (var df = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                return await df.QuerySingleOrDefaultAsync<string>("Select Top 1 Sido From Sido Where Sido_Code Like '" + @code + "%'", new { code });
            }

        }

        /// <summary>
        /// 시군구 코드 불러오기
        /// </summary>
        public async Task<string> Region(string Region)
        {
            using (var df = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                return await df.QuerySingleOrDefaultAsync<string>("Select Top 1 Sido From Sido Where Region = @Region", new { Region });
            }

        }

        /// <summary>
        /// 시군구 코드 불러오기
        /// </summary>
        public async Task<string> Region_Code(string Region)
        {
            using (var df = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                return await df.QuerySingleOrDefaultAsync<string>("Select Top 1 Sido_Code From Sido Where Region = @Region", new { Region });
            }

        }

        /// <summary>
        /// 식별코드 상세정보 불러오기
        /// </summary>
        /// <param name="Code"></param>
        /// <returns></returns>
        public async Task<Sido_Entity> Details(string Code)
        {
            using (var df = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                return await df.QuerySingleOrDefaultAsync<Sido_Entity>("Select Top 1 * From Sido Where Sido_Code = @Code", new { Code });
            }
        }

        /// <summary>
        /// 식별코드 상세정보 불러오기
        /// </summary>
        public async Task<Sido_Entity> Details_Name(string Name)
        {
            using (var df = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                return await df.QuerySingleOrDefaultAsync<Sido_Entity>("Select Top 1 * From Sido Where Region = @Name", new { Name });
            }
        }

        /// <summary>
        /// 해당단지에 등록 직원 수 구하기
        /// </summary>
        /// <param name="Apt_Code"></param>
        /// <returns></returns>
        public async Task<int> Apt_Count(string Apt_Code)
        {
            using (var df = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                return await df.QuerySingleOrDefaultAsync<int>("Select Count(*) From Staff Where Apt_Code = @Apt_Code", new { Apt_Code });
            }
        }
    }

    /// <summary>
    /// 파일올리기 클래스
    /// </summary>
    public class Erp_Files_Lib :IErp_Files_Lib
    {
        private readonly IConfiguration _db;
        public Erp_Files_Lib(IConfiguration configuration)
        {
            this._db = configuration;
        }

        /// <summary>
        /// 첨부 파일 입력
        /// </summary>
        /// <param name="_Entity"></param>
        /// <returns></returns>
        public async Task<Erp_Files_Entity> Add(Erp_Files_Entity _Entity)
        {
            using (var df = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                await df.ExecuteAsync("Sw_Files_Insert", _Entity, commandType: CommandType.StoredProcedure);
                return _Entity;
            }

        }

        /// <summary>
        /// 해당 부모에 첨부된 파일 리스트
        /// </summary>
        /// <param name="Parents_Num"></param>
        /// <param name="Parents_Name"></param>
        /// <returns></returns>
        public async Task<List<Erp_Files_Entity>> GeList(string Parents_Num, string Parents_Name)
        {
            using (var df = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                var lst = await df.QueryAsync<Erp_Files_Entity>("Select Num, AptCode, Parents_Name, Parents_Num, Sw_FileName, Sw_FileSize, PostDate, PostIP, ModifyDate, ModifyIP, Del From Sw_Files Where Parents_Num = @Parents_Num And Parents_Name = @Parents_Name Order By Num Desc", new { Parents_Num, Parents_Name });
                return lst.ToList();
            }

        }

        /// <summary>
        /// 파일 첨부 존재 여부 확인
        /// </summary>
        /// <param name="Parents_Num"></param>
        /// <param name="Parents_Name"></param>
        /// <returns></returns>
        public async Task<int> Be_Count(string Parents_Num, string Parents_Name)
        {
            using (var df = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                return await df.QuerySingleOrDefaultAsync<int>("Select Count(*) From Sw_Files Where Parents_Num = @Parents_Num And Parents_Name = @Parents_Name", new { Parents_Num, Parents_Name });
            }

        }

        /// <summary>
        /// 첨부파일 삭제
        /// </summary>
        /// <param name="Parents_Num"></param>
        /// <param name="Parents_Name"></param>
        /// <param name="Sw_FileName"></param>
        public void Delete(string Parents_Num, string Parents_Name)
        {
            using (var df = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                var Files = df.Query<object>("Select Sw_FileNameWhere Parents_Num = @Parents_Num And Parents_Name = @Parents_Name", new { Parents_Num, Parents_Name }).ToList();

                foreach (string f in Files)
                {
                    if (File.Exists(@"E:\Home\Ayoung\sw_Files_View\Sw_files\" + f))
                    {
                        // Use a try block to catch IOExceptions, to
                        // handle the case of the file already being
                        // opened by another process.
                        try
                        {
                            System.IO.File.Delete(@"E:\Home\Ayoung\sw_Files_View\Sw_files\" + f);
                        }
                        catch (System.IO.IOException e)
                        {
                            Console.WriteLine(e.Message);
                            return;
                        }
                    }

                }
                try
                {
                    df.Execute("Delete Sw_Files Where Parents_Num = @Parents_Num And Parents_Name = @Parents_Name", new { Parents_Num, Parents_Name });
                }
                catch (Exception)
                {

                }
            }
        }
    }

    /// <summary>
    /// 입주민 클래스
    /// </summary>
    public class Erp_AptPeople_Lib : IErp_AptPeople_Lib
    {
        private readonly IConfiguration _db;
        public Erp_AptPeople_Lib(IConfiguration configuration)
        {
            this._db = configuration;
        }

        /// <summary>
        /// 입주민 입력
        /// </summary>
        /// <param name="at"></param>
        /// <returns></returns>
        public async Task<Apt_People_Entity> Add(Apt_People_Entity at)
        {
            var sql = "Insert Into Sw_People (Dong, Ho, Apt_Code, Apt_Name, Area, InDateTime, InnerOwner, Bunyong, Inter, Relation, InnerName, Owner, UserID, Password, Help, Email, Tel, Hp, InnSoceity, InnerScn1, LevelCount, Intro, CarCount, PostIP, AgoCost) Values (@Dong, @Ho, @Apt_Code, @Apt_Name, @Area, @InDateTime, @InnerOwner, @Bunyong, @Inter, @Relation, @InnerName, @Owner, @UserID, @Password, @Help, @Email, @Tel, @Hp, @InnSoceity, @InnerScn1, @LevelCount, @Intro, @CarCount, @PostIP, @AgoCost)";

            using (var df = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                await df.ExecuteAsync(sql, at);
                return at;
            }
        }

        /// <summary>
        /// 입주민 정보 수정
        /// </summary>
        /// <param name="at"></param>
        /// <returns></returns>
        public async Task<Apt_People_Entity> Edit(Apt_People_Entity at)
        {
            var sql = "Update Sw_People Set Area = @Area, InDateTime = @InDateTime, InnerOwner = @InnerOwner, Bunyong = @Bunyong, Inter = @Inter, InnerName = @InnerName, Owner = @Owner, Help = @Help, Email = @Email, Tel = @Tel, Hp = @Hp, InnSoceity = @InnSoceity, InnerScn1 = @InnerScn1, MoveA = @MoveA, Intro = @Intro Where Num = @Num";
            using (var df = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                await df.ExecuteAsync(sql, at);
                return at;
            }

        }

        /// <summary>
        /// 이사 정보 입력
        /// </summary>
        public async Task Move(string MoveA, string MoveDate, int Num)
        {
            using (var df = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                await df.ExecuteAsync("Update Sw_People Set MoveA = @MoveA, MoveDate = @MoveDate Where Num = @Num", new { MoveA, MoveDate, Num });               
            }
        }

        /// <summary>
        /// 동호로 이사 정보 입력
        /// </summary>
        public async Task Remove_Ho(string Apt_Code, string Dong, string Ho)
        {
            using (var df = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                string date = DateTime.Now.ToShortDateString();
                await df.ExecuteAsync("Update Sw_People Set MoveA = 'B', MoveDate = '" + date + "' Where Apt_Code = @Apt_Code And Dong = @Dong And Ho = @Ho", new { Apt_Code, Dong, Ho }); //2023 - 02 - 22
            }
        }

        /// <summary>
        /// 동 목록 만들기
        /// </summary>
        public async Task<List<Apt_People_Entity>> DongList(string Apt_Code)
        {
            using (var df = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                var lst = await df.QueryAsync<Apt_People_Entity>("Select DISTINCT Dong From Sw_People Where Apt_Code = @Apt_Code order by dong asc", new { Apt_Code });
                return lst.ToList();
            }
        }

        /// <summary>
        /// 호 목록 만들기
        /// </summary>
        public async Task<List<Apt_People_Entity>> HoList(string Apt_Code, string Dong)
        {
            using (var df = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                var lst = await df.QueryAsync<Apt_People_Entity>("Select Ho From Sw_People Where Apt_Code = @Apt_Code And Dong = @Dong Order By CONVERT(INT, Ho) Asc", new { Apt_Code, Dong });
                return lst.ToList();
            }
        }

        /// <summary>
        /// 동호 만들기 검색된 갯수
        /// </summary>
        /// <param name="Apt_Code"></param>
        /// <param name="Dong"></param>
        /// <returns></returns>
        public async Task<int> Dong_HoList_Count(string Apt_Code, string Dong)
        {
            using (var df = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                return await df.QuerySingleOrDefaultAsync<int>("Select Count(*) From Sw_People Where Apt_Code = @Apt_Code And Dong = @Dong And MoveA = 'A'", new { Apt_Code, Dong });
            }

        }

        /// <summary>
        /// 동호 만들기 검색된 갯수
        /// </summary>
        public async Task<int> Dong_Ho_Count(string Apt_Code, string Dong, string Ho)
        {
            using (var df = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                return await df.QuerySingleOrDefaultAsync<int>("Select Count(*) From Sw_People Where Apt_Code = @Apt_Code And Dong = @Dong And Ho = @Ho And MoveA = 'A'", new { Apt_Code, Dong, Ho });
            }

        }

        /// <summary>
        /// 동호 만들기 검색된 갯수
        /// </summary>
        public async Task<int> Dong_Ho_Count_New(string Apt_Code, string Dong, string Ho)
        {
            using (var df = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                return await df.QuerySingleOrDefaultAsync<int>("Select Count(*) From Sw_People Where Apt_Code = @Apt_Code And Dong = @Dong And Ho = @Ho", new { Apt_Code, Dong, Ho });
            }

        }

        /// <summary>
        /// 동호 만들기 검색된 정보
        /// </summary>
        public async Task<List<Apt_People_Entity>> Apt_List(string Apt_Code)
        {
            using (var df = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                var lst = await df.QueryAsync<Apt_People_Entity>("Select Num, Dong, Ho, Apt_Code, Apt_Name, Area, InDateTime, InnerOwner, Bunyong, Inter, Relation, InnerName, Owner, UserID, Password, Help, Email, Tel, Hp, InnSoceity, InnerScn1, InnerScn2, LevelCount, CarCount, MoveA, MoveDate, Intro, VisitCount, PostDate, PostIP, AgoCost, ModifyIP, ModifyDate From Sw_People Where Apt_Code = @Apt_Code Order By CONVERT(INT, Ho) Asc", new { Apt_Code });
                return lst.ToList();
            }
        }

        /// <summary>
        /// 모바일로 검색된 정보
        /// </summary>
        public async Task<List<Apt_People_Entity>> Apt_Mobile_List(string Apt_Code, string Mobile)
        {
            using (var df = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                var lst = await df.QueryAsync<Apt_People_Entity>("Select Num, Dong, Ho, Apt_Code, Apt_Name, Area, InDateTime, InnerOwner, Bunyong, Inter, Relation, InnerName, Owner, UserID, Password, Help, Email, Tel, Hp, InnSoceity, InnerScn1, InnerScn2, LevelCount, CarCount, MoveA, MoveDate, Intro, VisitCount, PostDate, PostIP, AgoCost, ModifyIP, ModifyDate From Sw_People Where Apt_Code = @Apt_Code And Hp Like '%" + Mobile + "%' Order By CONVERT(INT, Ho) Asc", new { Apt_Code });
                return lst.ToList();
            }
        }

        /// <summary>
        /// 이름으로 검색된 정보
        /// </summary>
        public async Task<List<Apt_People_Entity>> Apt_Name_List(string Apt_Code, string Name)
        {
            using (var df = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                var lst = await df.QueryAsync<Apt_People_Entity>("Select Num, Dong, Ho, Apt_Code, Apt_Name, Area, InDateTime, InnerOwner, Bunyong, Inter, Relation, InnerName, Owner, UserID, Password, Help, Email, Tel, Hp, InnSoceity, InnerScn1, InnerScn2, LevelCount, CarCount, MoveA, MoveDate, Intro, VisitCount, PostDate, PostIP, AgoCost, ModifyIP, ModifyDate From Sw_People Where Apt_Code = @Apt_Code And InnerName Like '%" + Name + "%' Order By CONVERT(INT, Ho) Asc", new { Apt_Code });
                return lst.ToList();
            }
        }

        /// <summary>
        /// 동호 만들기 검색된 정보
        /// </summary>
        /// <param name="Apt_Code"></param>
        /// <param name="Dong"></param>
        /// <returns></returns>
        public async Task<List<Apt_People_Entity>> Apt_List_Page(int Page, string Apt_Code)
        {
            using (var df = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                var lst = await df.QueryAsync<Apt_People_Entity>("Select Top 15 * From Sw_People Where Num Not In(Select Top(15 * @Page) Num From Sw_People Where Apt_Code = @Apt_Code And MoveA = 'A' Order By Num Desc) and Apt_Code = @Apt_Code Order By Num Desc", new { Page, Apt_Code });
                return lst.ToList();
            }

        }

        /// <summary>
        /// 공동주택별 입주민 검색된 갯 수
        /// </summary>
        /// <param name="Apt_Code"></param>
        /// <param name="Dong"></param>
        /// <returns></returns>
        public async Task<int> Apt_List_Count(string Apt_Code)
        {
            using (var df = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                return await df.QuerySingleOrDefaultAsync<int>("Select Count(*) From Sw_People Where Apt_Code = @Apt_Code And MoveA = 'A'", new { Apt_Code });
            }

        }

        /// <summary>
        /// 공동주택별 입주민 검색된 정보
        /// </summary>
        public async Task<List<Apt_People_Entity>> Dong_HoList(string Apt_Code, string Dong)
        {
            using (var df = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                var lst = await df.QueryAsync<Apt_People_Entity>("Select Num, Dong, Ho, Apt_Code, Apt_Name, Area, InDateTime, InnerOwner, Bunyong, Inter, Relation, InnerName, Owner, UserID, Password, Help, Email, Tel, Hp, InnSoceity, InnerScn1, InnerScn2, LevelCount, CarCount, MoveA, MoveDate, Intro, VisitCount, PostDate, PostIP, AgoCost, ModifyIP, ModifyDate From Sw_People Where Num in (Select max(Num) From Sw_People group by Dong, Ho) And Apt_Code = @Apt_Code And Dong = @Dong Order By CONVERT(INT, Ho) Asc", new { Apt_Code, Dong });
                return lst.ToList();
            }
        }


        /// <summary>
        /// 공동주택별 입주민 중복 제거 정보 목록
        /// </summary>
        public async Task<List<Apt_People_Entity>> Dong_HoList_Ds(string Apt_Code, string Dong)
        {
            using (var df = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                var lst = await df.QueryAsync<Apt_People_Entity>("Select Ho From Sw_People Where Apt_Code = @Apt_Code And Dong = @Dong  Group By Dong, Ho Order By CONVERT(INT, Ho) Asc", new { Apt_Code, Dong });
                return lst.ToList();
            }
        }

        /// <summary>
        /// 동으로 검색된 호 목록
        /// </summary>
        public async Task<List<Apt_People_Entity>> DongHoList_new(string Apt_Code, string Dong)
        {
            using var df = new SqlConnection(_db.GetConnectionString("Ayoung"));
            var lst = await df.QueryAsync<Apt_People_Entity>("SELECT Ho, Count(*) cnt FROM Sw_People Where Apt_Code = @Apt_Code And Dong = @Dong Group by [Ho] Order By CONVERT(INT, Ho) Asc", new { Apt_Code, Dong });
            return lst.ToList();
        }

        /// <summary>
        /// 공동주택별 입주민 검색된 갯 수
        /// </summary>
        public async Task<List<Apt_People_Entity>> Dong_Ho_Name_List(string Apt_Code, string Dong, string Ho)
        {
            using (var df = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                var lst = await df.QueryAsync<Apt_People_Entity>("Select Num, Dong, Ho, Apt_Code, Apt_Name, Area, InDateTime, InnerOwner, Bunyong, Inter, Relation, InnerName, Owner, UserID, Password, Help, Email, Tel, Hp, InnSoceity, InnerScn1, InnerScn2, LevelCount, CarCount, MoveA, MoveDate, Intro, VisitCount, PostDate, PostIP, AgoCost, ModifyIP, ModifyDate From Sw_People Where Apt_Code = @Apt_Code And Dong = @Dong And Ho = @Ho And MoveA = 'A' Order By CONVERT(INT, Ho) Asc", new { Apt_Code, Dong, Ho });
                return lst.ToList();
            }
        }

        /// <summary>
        /// 공동주택별 입주민 검색된 갯 수(페이징)
        /// </summary>
        public async Task<List<Apt_People_Entity>> Dong_HoList_Page(int Page, string Apt_Code, string Dong)
        {
            using (var df = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                var lst = await df.QueryAsync<Apt_People_Entity>("Select * From Sw_People Where Num Not In(Select Top(15 * @Page) Num From Sw_People Where Apt_Code = @Apt_Code And Dong = @Dong Order By Dong Asc, CONVERT(INT, Ho) Asc) And Apt_Code = @Apt_Code And Dong = @Dong Order By Dong Asc, CONVERT(INT, Ho) Asc", new { Page, Apt_Code, Dong });
                return lst.ToList();
            }
        }

        /// <summary>
        /// 해당 세대 상세 정보
        /// </summary>
        public async Task<Apt_People_Entity> DongHo_Name(string Apt_Code, string Dong, string Ho)
        {
            using (var df = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                return await df.QuerySingleOrDefaultAsync<Apt_People_Entity>("SELECT TOP 1 Num, Dong, Ho, Apt_Code, Apt_Name, Area, InDateTime, InnerOwner, Bunyong, Inter, Relation, InnerName, Owner, UserID, Password, Help, Email, Tel, Hp, InnSoceity, InnerScn1, InnerScn2, LevelCount, CarCount, MoveA, MoveDate, Intro, VisitCount, PostDate, PostIP, AgoCost, ModifyIP, ModifyDate FROM Sw_People Where Apt_Code = @Apt_Code And Dong = @Dong And Ho = @Ho And MoveA = 'A' Order By Num Desc", new { Apt_Code, Dong, Ho });
            }
        }

        /// <summary>
        /// 해당 세대 상세 정보
        /// </summary>
        public async Task<Apt_People_Entity> DongHo_Repeat(string Apt_Code, string Dong)
        {
            using (var df = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                return await df.QuerySingleOrDefaultAsync<Apt_People_Entity>("SELECT distinct Ho FROM Sw_People Where Apt_Code = @Apt_Code And Dong = @Dong Order By Num Desc", new { Apt_Code, Dong });
            }
        }

        /// <summary>
        /// 해당 세대 상세 정보
        /// </summary>
        public async Task<int> Dong_Ho_Name_Being(string Apt_Code, string Dong, string Ho, string Name)
        {
            using (var df = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                return await df.QuerySingleOrDefaultAsync<int>("SELECT Count(*) FROM Sw_People Where Apt_Code = @Apt_Code And Dong = @Dong And Ho = @Ho And MoveA = 'A' And InnerName = @Name", new { Apt_Code, Dong, Ho, Name });
            }
        }

        /// <summary>
        /// 해당 세대 상세 정보
        /// </summary>
        public async Task<Apt_People_Entity> Dedeils_Mobile(string Mobile, string Dong, string Ho)
        {
            using (var df = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                return await df.QuerySingleOrDefaultAsync<Apt_People_Entity>("SELECT TOP 1 Num, Dong, Ho, Apt_Code, Apt_Name, Area, InDateTime, InnerOwner, Bunyong, Inter, Relation, InnerName, Owner, UserID, Password, Help, Email, Tel, Hp, InnSoceity, InnerScn1, InnerScn2, LevelCount, CarCount, MoveA, MoveDate, Intro, VisitCount, PostDate, PostIP, AgoCost, ModifyIP, ModifyDate FROM Sw_People Where Hp = @Mobile And Dong = @Dong And Ho = @Ho And MoveA = 'A' Order By Num Desc", new { Mobile, Dong, Ho });
            }
        }


        /// <summary>
        /// 해당 세대 이름으로 검색된 상세 정보
        /// </summary>
        public async Task<Apt_People_Entity> Dedeils_Name(string Apt_Code, string Name, string Dong, string Ho)
        {
            using (var df = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                return await df.QuerySingleOrDefaultAsync<Apt_People_Entity>("SELECT TOP 1 Num, Dong, Ho, Apt_Code, Apt_Name, Area, InDateTime, InnerOwner, Bunyong, Inter, Relation, InnerName, Owner, UserID, Password, Help, Email, Tel, Hp, InnSoceity, InnerScn1, InnerScn2, LevelCount, CarCount, MoveA, MoveDate, Intro, VisitCount, PostDate, PostIP, AgoCost, ModifyIP, ModifyDate FROM Sw_People Where Apt_Code = @Apt_Code And InnerName = @Name And Dong = @Dong And Ho = @Ho And MoveA = 'A' Order By Num Desc", new { Apt_Code, Name, Dong, Ho });
            }
        }

        /// <summary>
        /// 해당 세대 상세 정보 수
        /// </summary>
        public async Task<int> Dedeils_Mobile_Count(string Mobile, string Dong, string Ho, string InnerName)
        {
            using (var df = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                return await df.QuerySingleOrDefaultAsync<int>("SELECT Count(*) FROM Sw_People Where Hp = @Mobile And Dong = @Dong And Ho = @Ho And InnerName = @InnerName And MoveA = 'A'", new { Mobile, Dong, Ho, InnerName });
            }
        }

        /// <summary>
        /// 해당 세대 상세 정보 수
        /// </summary>
        public async Task<Apt_People_Entity> Dedeils_Name(string Num)
        {
            using (var df = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                return await df.QuerySingleOrDefaultAsync<Apt_People_Entity>("SELECT * FROM Sw_People Where Num = @Num And MoveA = 'A'", new { Num });
            }
        }

        /// <summary>
        /// 해당 세대 면적 정보
        /// </summary>
        public async Task<double> DongHo_Area(string Apt_Code, string Dong, string Ho)
        {
            using (var df = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                return await df.QuerySingleOrDefaultAsync<double>("SELECT TOP 1 Area FROM Sw_People Where Apt_Code = @Apt_Code And Dong = @Dong And Ho = @Ho Order By Num Desc", new { Apt_Code, Dong, Ho });
            }
        }

        /// <summary>
        /// 동호로 입주민 검색된 목록
        /// </summary>
        public async Task<List<Apt_People_Entity>> DongHoList(string Apt_Code, string Dong, string Ho)
        {
            using (var df = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                var lst = await df.QueryAsync<Apt_People_Entity>("Select * From Sw_People Where Apt_Code = @Apt_Code And Dong = @Dong And Ho = @Ho Order By Num Desc", new { Apt_Code, Dong, Ho });
                return lst.ToList();
            }
        }

        /// <summary>
        /// 첨부파일 수 더하기
        /// </summary>
        public async Task FilesCountAdd(string Aid, string Division)
        {
            using var df = new SqlConnection(_db.GetConnectionString("Ayoung"));
            if (Division == "A")
            {
                await df.ExecuteAsync("Update Sw_People Set Files_Count = Files_Count + 1 Where Num = @Aid", new { Aid });
            }
            else
            {
                await df.ExecuteAsync("Update Sw_People Set Files_Count = Files_Count - 1 Where Num = @Aid", new { Aid });
            }

        }        
    }

    /// <summary>
    /// 부서 및 직책 클래스
    /// </summary>
    public class PostDuty_Lib : IPostDuty_Lib
    {
        private readonly IConfiguration _db;
        public PostDuty_Lib(IConfiguration configuration)
        {
            this._db = configuration;
        }

        /// <summary>
        /// 부서 및 직책 입력
        /// </summary>
        /// <param name="pd"></param>
        /// <returns></returns>
        public async Task<PostDuty_Entity> Add(PostDuty_Entity pd)
        {

            var sql = "Insert Into Post_Duty (PD_Code, PD_C_Name, Post_Code, Post_Name, Post_Duty_Code, Post_Duty_Name, Post_Duty_D_Code, Post_Duty_Direct, Post_Duty_Intro, UserID, AptCode, PostIP) Values (@PD_Code, @PD_C_Name, @Post_Code, @Post_Name, @Post_Duty_Code, @Post_Duty_Name, @Post_Duty_D_Code, @Post_Duty_Direct, @Post_Duty_Intro, @UserID, @AptCode, @PostIP)";
            using (var df = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                await df.ExecuteAsync(sql, pd);
                return pd;
            }

        }

        /// <summary>
        /// 부서 혹은 직책 수정
        /// </summary>
        /// <param name="pd"></param>
        public async Task Edit(PostDuty_Entity pd)
        {
            var sql = "Update Post_Duty Set Post_Duty_Name = @Post_Duty_Name, Post_Duty_Intro = @Post_Duty_Intro, UserID = @UserID, AptCode = @AptCode, PostIP = @PostIP Where Num = @Num";
            using (var df = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                await df.QuerySingleOrDefaultAsync(sql, pd);
            }

        }

        /// <summary>
        /// 부서 직책  삭제
        /// </summary>
        /// <param name="Num"></param>
        public async Task Remove(string Num)
        {
            using (var df = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                await df.ExecuteAsync("Delete Post_Duty Where Num = @Num", new { Num });
            }
        }

        /// <summary>
        /// 부서 드롭다운 리스트 만들기
        /// </summary>
        /// <param name="PD_Code"></param>
        /// <param name="Post_Duty_D_Code"></param>
        /// <returns></returns>
        public async Task<List<PostDuty_Entity>> GetList_Post_P(string PD_Code, string Post_Duty_D_Code)
        {
            using (var df = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                var lst = await df.QueryAsync<PostDuty_Entity>("Select Num, PD_Code, Post_Code, Post_Name, PD_C_Name, Post_Duty_Code, Post_Duty_Name, Post_Duty_D_Code, Post_Duty_Direct, Post_Duty_Intro, UserID, AptCode, PostDate, PostIP From Post_Duty_Table Where PD_Code = @PD_Code And Post_Duty_D_Code = @Post_Duty_D_Code Order By Num Asc", new { PD_Code, Post_Duty_D_Code });
                return lst.ToList();
            }

        }

        //public List<PostDuty_Entity> PostList(string PD_Code, string )

        /// <summary>
        /// 부서 혹은 직책 식별코드 불러오기
        /// </summary>
        /// <param name="PD_Code"></param>
        /// <param name="Post_Duty_Name"></param>
        /// <returns></returns>
        public async Task<string> Post_Duty_Code(string PD_Code, string Post_Duty_Name)
        {
            using (var df = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                return await df.QuerySingleOrDefaultAsync<string>("Select top 1 Post_Duty_Code From Post_Duty_Table Where PD_Code = @PD_Code And Post_Duty_Name = @Post_Duty_Name", new { PD_Code, Post_Duty_Name });
            }

        }

        /// <summary>
        /// 부서 및 직책 분류코드로 불러오기
        /// </summary>
        /// <param name="Post_Code"></param>
        /// <param name="PD_Code"></param>
        /// <returns></returns>
        public async Task<List<PostDuty_Entity>> Duty_List(string Post_Code, string PD_Code)
        {
            using (var df = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                var lst = await df.QueryAsync<PostDuty_Entity>("Select Num, PD_Code, Post_Code, Post_Name, PD_C_Name, Post_Duty_Code, Post_Duty_Name, Post_Duty_D_Code, Post_Duty_Direct, Post_Duty_Intro, UserID, AptCode, PostDate, PostIP From Post_Duty Where PD_Code = @PD_Code And Post_Code = @Post_Code Order By Num Asc", new { Post_Code, PD_Code });
                return lst.ToList();
            }

        }

        /// <summary>
        /// 식별코드로 상세 정보 불러오기
        /// </summary>
        /// <param name="Post_Code"></param>
        /// <param name="PD_Code"></param>
        /// <returns></returns>
        public async Task<PostDuty_Entity> Detail(string Num)
        {
            using (var df = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                return await df.QuerySingleOrDefaultAsync<PostDuty_Entity>("Select Num, PD_Code, Post_Code, Post_Name, PD_C_Name, Post_Duty_Code, Post_Duty_Name, Post_Duty_D_Code, Post_Duty_Direct, Post_Duty_Intro, UserID, AptCode, PostDate, PostIP From Post_Duty Where Num = @Num", new { Num });
            }

        }

        /// <summary>
        /// 부서 및 직책 모든 목록
        /// </summary>
        /// <returns></returns>
        public async Task<List<PostDuty_Entity>> PostDuty_AllList()
        {
            using (var df = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                var lst = await df.QueryAsync<PostDuty_Entity>("Select Num, PD_Code, Post_Code, Post_Name, PD_C_Name, Post_Duty_Code, Post_Duty_Name, Post_Duty_D_Code, Post_Duty_Direct, Post_Duty_Intro, UserID, AptCode, PostDate, PostIP From Post_Duty Order By Num Asc");
                return lst.ToList();
            }

        }

        /// <summary>
        /// 마지막 일련번호 불러오기
        /// </summary>
        /// <returns></returns>
        public async Task<int> Be_Last()
        {
            using (var df = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                return await df.QuerySingleOrDefaultAsync<int>("Select Top 1 Num From Post_Duty Order By Num Desc");
            }

        }

        /// <summary>
        /// 부서 정보 목록
        /// </summary>
        public async Task<List<PostDuty_Entity>> PostList(string PD_Code, string Post_Code)
        {
            using (var df = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                var lst = await df.QueryAsync<PostDuty_Entity>("SELECT Num, PD_Code, PD_C_Name, Post_Code, Post_Name, Post_Duty_Code, Post_Duty_Name, Post_Duty_D_Code, Post_Duty_Direct, Post_Duty_Intro, UserID, AptCode, PostDate, PostIP FROM Post_Duty WHERE(PD_Code = @PD_Code) AND(Post_Code = @Post_Code)", new { PD_Code, Post_Code });
                return lst.ToList();
            }
        }
    }

    /// <summary>
    /// 부서 관련 클래스
    /// </summary>
    public class Post_Lib : IPost_Lib
    {
        private readonly IConfiguration _db;
        public Post_Lib(IConfiguration configuration)
        {
            this._db = configuration;
        }

        /// <summary>
        /// 부서 입력
        /// </summary>
        /// <param name="post"></param>
        public async Task Add(Post_Entity post)
        {
            using (var df = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                await df.ExecuteAsync("Insert into Post (PostName, Division, Etc, PostIP) Values (@PostName, @Division, @Etc, @PostIP)", post);
            }
        }

        /// <summary>
        /// 부서 수정
        /// </summary>
        /// <param name="post"></param>
        public async Task Edit(Post_Entity post)
        {
            using (var df = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                await df.ExecuteAsync("Update Post Set PostName = @PostName, PostDate = @PostDate, PostIP = @PostIP", post);
            }
        }

        /// <summary>
        /// 입력된 수
        /// </summary>
        /// <returns></returns>
        public async Task<int> Count()
        {
            using (var df = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                return await df.QuerySingleOrDefaultAsync<int>("Select Count(*) From Post");
            }

        }

        /// <summary>
        /// 부서 목록 구현(구분)
        /// </summary>
        /// <param name="Division"></param>
        /// <returns></returns>
        public async Task<List<Post_Entity>> GetList(string Division)
        {
            using (var df = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                var lst = await df.QueryAsync<Post_Entity>("Select PostCode, PostName, Division, Etc, PostDate, PostIP From Post Where Division = @Division", new { Division });
                return lst.ToList();
            }
        }

        /// <summary>
        /// 부서 천제 목록 구현
        /// </summary>
        /// <returns></returns>
        public async Task<List<Post_Entity>> GetListAll()
        {
            using (var df = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                var lst = await df.QueryAsync<Post_Entity>("Select PostCode, PostName, Division, Etc, PostDate, PostIP From Post Order By PostCode Asc");
                return lst.ToList();
            }
        }

        /// <summary>
        /// 부서명 불러오기
        /// </summary>
        /// <param name="PostCode"></param>
        /// <returns></returns>
        public async Task<string> PostName(string PostCode)
        {
            using (var df = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                return await df.QuerySingleOrDefaultAsync<string>("Select PostName From Post Where PostCode = @PostCode", new { PostCode });
            }

        }

        /// <summary>
        /// 부서 삭제
        /// </summary>
        /// <param name="PostCode"></param>
        public async Task Remove(string PostCode)
        {
            using (var df = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                await df.ExecuteAsync("Delete Post Where PostCode = @PostCode", new { PostCode });
            }

        }

        /// <summary>
        /// 부서 이름 불러오기
        /// </summary>
        /// <param name="PostName"></param>
        /// <returns></returns>
        public async Task<string> PostCode(string PostName)
        {
            using (var df = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                return await df.QuerySingleOrDefaultAsync<string>("Select PostCode From Post Where PostName = @PostName", new { PostName });
            }
        }
    }

    /// <summary>
    /// 직책 관련 클래스
    /// </summary>
    public class Duty_Lib : IDuty_Lib
    {
        private readonly IConfiguration _db;
        public Duty_Lib(IConfiguration configuration)
        {
            this._db = configuration;
        }

        /// <summary>
        /// 직책 입력
        /// </summary>
        /// <param name="ann"></param>
        public async Task Add(Duty_Entity ann)
        {
            using (var df = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                await df.ExecuteAsync("Insert Into Duty (DutyName, PostCode, Division, Etc, PostIP) Values (@DutyName, @PostCode, @Division, @Etc, @PostIP)", ann);
            }
        }

        /// <summary>
        /// 직책 수정
        /// </summary>
        public async Task Edit(Duty_Entity ann)
        {
            using (var df = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                await df.ExecuteAsync("Update Duty Set DutyName = @DutyName, PostCode = @PostCode, Division = @Division, Etc = @Etc, PostIP = @PostIP)", ann);
            }

        }

        /// <summary>
        /// 직책 정목 목록 만들기
        /// </summary>
        /// <param name="PostCode"></param>
        /// <param name="Division"></param>
        /// <returns></returns>
        public async Task<List<Duty_Entity>> GetList(string PostCode, string Division)
        {
            using (var df = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                var lst = await df.QueryAsync<Duty_Entity>("Select DutyCode, DutyName, PostCode, Division, Etc, PostDate, PostIP From Duty Where PostCode = @PostCode Order by DutyCode Asc", new { PostCode, Division });
                return lst.ToList();
            }

        }

        /// <summary>
        /// 직책 목록(중복제거) 만들기
        /// </summary>
        /// <param name="PostCode"></param>
        /// <param name="Division"></param>
        /// <returns></returns>
        public async Task<List<Duty_Entity>> GetList_B(string PostCode)
        {
            using (var df = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                var lst = await df.QueryAsync<Duty_Entity>("Select DutyName, DutyCode From Duty Where PostCode = @PostCode Group By DutyName, DutyCode Order by DutyCode Asc", new { PostCode });
                return lst.ToList();
            }

        }

        /// <summary>
        /// 직책 정목 목록 만들기
        /// </summary>
        /// <param name="PostCode"></param>
        /// <param name="Division"></param>
        /// <returns></returns>
        public async Task<List<Duty_Entity>> GetListAll(int Page)
        {
            using (var df = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                var lst = await df.QueryAsync<Duty_Entity>("Select Top 15 DutyCode, DutyName, PostCode, Division, Etc, PostDate, PostIP From Duty Where DutyCode Not In (Select Top (15 * @Page) DutyCode From Duty Order By DutyCode Desc) Order By DutyCode Desc", new {Page});
                return lst.ToList();
            }
        }

        /// <summary>
        /// 직책명 불러오기
        /// </summary>
        public async Task<int> GetListAll_Count()
        {
            using (var df = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                return await df.QuerySingleOrDefaultAsync<int>("Select Count(*) From Duty");
            }
        }

        /// <summary>
        /// 직책명 불러오기
        /// </summary>
        /// <param name="DutyCode"></param>
        /// <returns></returns>
        public async Task<string> DutyName(string DutyCode)
        {
            using (var df = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                return await df.QuerySingleOrDefaultAsync<string>("Select DutyName From Duty Where DutyCode = @DutyCode", new { DutyCode });
            }

        }

        /// <summary>
        /// 직책코드 불러오기
        /// </summary>
        public async Task<int> DutyCode(string Post_Code, string Duty)
        {
            using var df = new SqlConnection(_db.GetConnectionString("sw_togather"));
            return await df.QuerySingleOrDefaultAsync<int>("Select DutyCode From Duty Where PostCode = @PostCode And DutyName = @Duty", new { Post_Code, Duty });

        }

        /// <summary>
        /// 직책 정보 삭제
        /// </summary>
        public async Task Remove(int Aid)
        {
            using var df = new SqlConnection(_db.GetConnectionString("sw_togather"));
            await df.ExecuteAsync("Delete Duty Where DutyCode = @Aid", new { Aid });
        }
    }

    /// <summary>
    /// 결재관리
    /// </summary>
    public class Decusion_PostDuty_Lib : IDecusion_PostDuty_Lib
    {
        private readonly IConfiguration _db;
        public Decusion_PostDuty_Lib(IConfiguration configuration)
        {
            this._db = configuration;
        }

        /// <summary>
        /// 결제자 라인 입력
        /// </summary>
        public async Task Add(Decision_PostDuty_Entity dnn)
        {
            using (var df = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                await df.ExecuteAsync("Insert PostDuty (AptCode, AptName, Bloom, PostDuty, Post, Duty, Intro, PostIP, Step) Values (@AptCode, @AptName, @Bloom, @PostDuty, @Post, @Duty, @Intro, @PostIP, @Step)", dnn);
            }

        }

        /// <summary>
        /// 결재자 라인 정보 수정
        /// </summary>
        /// <param name="dp"></param>
        public async Task Edit(Decision_PostDuty_Entity dp)
        {
            using (var df = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                await df.ExecuteAsync("Update PostDuty Bloom = @Bloom, PostDuty = @PostDuty, Post = @Post, Duty = @Duty, Intro = @Intro, PostIP = @PostIP, @ Step = @Step Where Num = @Num", dp);
            }

        }

        /// <summary>
        /// 결재자 정보 상세보기
        /// </summary>
        public async Task<Decision_PostDuty_Entity> Details(int Num)
        {
            using (var df = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                return await df.QuerySingleOrDefaultAsync<Decision_PostDuty_Entity>("Select Num, AptCode, AptName, Bloom, PostDuty, Post, Duty, Intro, ModifyDate, ModifyIP, PostDate, PostIP, Step From PostDuty Where Num = @Num", new { Num });
            }

        }

        /// <summary>
        /// 결재 정보 목록
        /// </summary>
        public async Task<List<Decision_PostDuty_Entity>> List(string AptCode, string Bloom)
        {
            using (var df = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                var lst = await df.QueryAsync<Decision_PostDuty_Entity>("Select Num, AptCode, AptName, Bloom, PostDuty, Post, Duty, Intro, ModifyDate, ModifyIP, PostDate, PostIP From PostDuty Where AptCode = @AptCode And Bloom = @Bloom Order By Step Asc, Num Asc", new { AptCode, Bloom });
                return lst.ToList();
            }

        }

        /// <summary>
        /// 결재 정보 목록
        /// </summary>
        public async Task<int> ListCount(string AptCode, string Bloom)
        {
            using (var df = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                return await df.QuerySingleOrDefaultAsync<int>("Select Count(*) From PostDuty  Where AptCode = @AptCode And Bloom = @Bloom", new { AptCode, Bloom });
            }

        }

        /// <summary>
        /// 결재라인에서 해당 부서직책 존재 여부 확인
        /// </summary>
        public async Task<int> PostDutyBeCount(string AptCode, string Bloom, string PostDuty)
        {
            using (var df = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                return await df.QuerySingleOrDefaultAsync<int>("Select Count(*) From PostDuty Where AptCode = @AptCode And Bloom_Code = @Bloom And PostDuty = @PostDuty", new { AptCode, Bloom, PostDuty });

            }

        }

        /// <summary>
        /// 결재 정보 목록 (공동주택 전체)
        /// </summary>
        public async Task<List<Decision_PostDuty_Entity>> GetList(int Page, string AptCode)
        {
            using (var df = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                var lst = await df.QueryAsync<Decision_PostDuty_Entity>("Select Top 15 Num, AptCode, AptName, Bloom, Bloom_Code, PostDuty, Post, Duty, Intro, ModifyDate, ModifyIP, PostDate, PostIP, Step From PostDuty Where Num Not In(Select Top(15 * @Page) Num From PostDuty Where AptCode = @AptCode Order By Step Asc, Num Asc) And AptCode = @AptCode Order By Step Asc, Num Asc", new { Page, AptCode });
                return lst.ToList();
            }

        }

        /// <summary>
        /// 결재 정보 목록 (공동주택 전체) 수
        /// </summary>
        /// <param name="AptCode"></param>
        /// <returns></returns>
        public async Task<int> GetListCount(string AptCode)
        {
            using var df = new SqlConnection(_db.GetConnectionString("Ayoung"));
            return await df.QuerySingleOrDefaultAsync<int>("Select Count(*) From PostDuty Where AptCode = @AptCode", new { AptCode });

        }

        /// <summary>
        /// 삭제
        /// </summary>
        public async Task Remove(int Num)
        {
            using var df = new SqlConnection(_db.GetConnectionString("Ayoung"));
            await df.ExecuteAsync("Delete PostDuty Where Num = @Num", new { Num });
        }
    }

    /// <summary>
    /// 입주민 가입 승인 정보
    /// </summary>
    public class bs_apt_career : Ibs_apt_career
    {
        private readonly IConfiguration _db;
        public bs_apt_career(IConfiguration configuration)
        {
            this._db = configuration;
        }

        /// <summary>
        /// 홈피 입력
        /// </summary>
        /// <param name="_Career"></param>
        /// <returns></returns>
        public async Task<bs_apt_career_Entity> add(bs_apt_career_Entity _Career)
        {
            using (var df = new SqlConnection(_db.GetConnectionString("bscity")))
            {
                int Aid = await df.QuerySingleOrDefaultAsync<int>("insert into bs_apt_career (Dong, Ho, bs_code, bs_name, Apt_Code, bs_start, bs_intro, bs_division, postip, apt_career_Code, Sw_People_Code, Apt_Pople_Code) Values (@Dong, @Ho, @bs_code, @bs_name, @Apt_Code, @bs_start, @bs_intro, @bs_division, @postip, @apt_career_Code, @Sw_People_Code, @Apt_Pople_Code); Select Cast(SCOPE_IDENTITY() As Int);", _Career);
                _Career.Aid = Aid;
                return _Career;
            }
            
        }

        

        /// <summary>
        /// 사용 정지 및 탈퇴, 이사 등 처리
        /// </summary>
        /// <param name="bs_end"></param>
        /// <param name="Aid"></param>
        public async Task edit_end(DateTime bs_end, int Aid, string bs_division)
        {
            using (var df = new SqlConnection(_db.GetConnectionString("bscity")))
            {
                await df.ExecuteAsync("update bs_apt_career set bs_end = @bs_end, bs_division = @bs_division Where Aid = @Aid", new { bs_end, Aid, bs_division });
            }
            
        }

        /// <summary>
        /// 사용 정지 및 탈퇴, 이사 등 처리
        /// </summary>
        /// <param name="bs_end"></param>
        /// <param name="Aid"></param>
        public async Task edit_end_A(int Aid, string bs_division)
        {
            using (var df = new SqlConnection(_db.GetConnectionString("bscity")))
            {
                await df.ExecuteAsync("update bs_apt_career set bs_division = @bs_division Where Aid = @Aid", new { Aid, bs_division });
            }
            
        }

        /// <summary>
        /// 해당 공동주택의 모든 회원 정보 목록
        /// </summary>
        /// <param name="Page"></param>
        /// <param name="Apt_Code"></param>
        /// <returns></returns>
        public async Task<List<bs_apt_career_Entity>> GetList_all(int Page, string Apt_Code)
        {
            using (var df = new SqlConnection(_db.GetConnectionString("bscity")))
            {
                var lst = await df.QueryAsync<bs_apt_career_Entity>("Select Top 15 * From bs_apt_career Where Aid Not In(Select Top(15 * @Page) Aid From bs_apt_career Where Apt_Code = @Apt_Code Order By Aid Desc) and Apt_Code = @Apt_Code Order By Aid Desc", new { Page, Apt_Code });
                return lst.ToList();
            }
            
        }

        /// <summary>
        /// 해당 공동주택의 모든 회원 정보 수
        /// </summary>
        /// <param name="Apt_Code"></param>
        /// <returns></returns>
        public async Task<int> GetList_all_Count(string Apt_Code)
        {
            using (var df = new SqlConnection(_db.GetConnectionString("bscity")))
            {
                return await df.QuerySingleOrDefaultAsync<int>("Select Count(*) From bs_apt_career Where Apt_Code = @Apt_Code", new { Apt_Code });
            }
            
        }

        
        /// <summary>
        /// 해당 공동주택의 회원 정보 목록
        /// </summary>
        /// <param name="Page"></param>
        /// <param name="Apt_Code"></param>
        /// <returns></returns>
        public async Task<List<bs_apt_career_Entity>> GetList_ok(int Page, string Apt_Code, string bs_division)
        {
            using (var df = new SqlConnection(_db.GetConnectionString("bscity")))
            {
                var lst = await df.QueryAsync<bs_apt_career_Entity>("Select Top 15 * From bs_apt_career Where Aid Not In(Select Top(15 * " + @Page + ") Aid From bs_apt_career Where Apt_Code = @Apt_Code And bs_division = @bs_division Order By Aid Desc) and Apt_Code = @Apt_Code And bs_division = @bs_division Order By Aid Desc", new { Page, Apt_Code, bs_division });
                return lst.ToList();
            }
            
        }

        /// <summary>
        /// 가입 승인된 단지 회원 수
        /// </summary>
        /// <param name="Apt_Code"></param>
        /// <param name="bs_division"></param>
        /// <returns></returns>
        public async Task<int> GetList_ok_Count(string Apt_Code, string bs_division)
        {
            using (var df = new SqlConnection(_db.GetConnectionString("bscity")))
            {
                return await df.QuerySingleOrDefaultAsync<int>("Select Count(*) From bs_apt_career Where Apt_Code = @Apt_Code And bs_division = @bs_division", new { Apt_Code, bs_division });
            }
            
        }

        /// <summary>
        /// 해당 공동주택의 회원 정보 목록
        /// </summary>
        /// <param name="Page"></param>
        /// <param name="Apt_Code"></param>
        /// <returns></returns>
        public async Task<List<Join_apt_People_a_Career_Entity>> Join_GetList_People_a_Career(int Page, string Apt_Code, string bs_division)
        {
            using (var df = new SqlConnection(_db.GetConnectionString("bscity")))
            {
                var lst = await df.QueryAsync<Join_apt_People_a_Career_Entity>("Select Top 15 b.Aid, b.Apt_Code, a.Apt_Name, a.User_Code, a.User_Name, a.LevelCount, b.Dong, b.Ho, a.Scn, a.Mobile, a.Telephone, a.Email, a.Intro, a.VisitCount, a.WriteCount, a.CommentsCount, a.MoveDate, a.PostDate, a.PostIP, a.Withdrawal,  b.bs_code, b.bs_name, b.bs_start, b.bs_end, b.bs_division, bs_intro, apt_career_Code, Sw_People_Code From Apt_Pople as a Join bs_apt_career as b on a.Aid = b.Apt_Pople_Code Where b.Aid Not In(Select Top(15 * " + @Page + ") Aid From bs_apt_career Where a.Apt_Code = @Apt_Code And b.bs_division = @bs_division Order By a.Aid Desc) and a.Apt_Code = @Apt_Code And b.bs_division = @bs_division Order By a.Aid Desc", new { Page, Apt_Code, bs_division });
                    return lst.ToList();
            }
            
        }

        /// <summary>
        /// 가입 승인된 단지 회원 수
        /// </summary>
        /// <param name="Apt_Code"></param>
        /// <param name="bs_division"></param>
        /// <returns></returns>
        public async Task<int> Join_GetList_People_a_Career_Count(string Apt_Code, string bs_division)
        {
            using (var df = new SqlConnection(_db.GetConnectionString("bscity")))
            {
                return await df.QuerySingleOrDefaultAsync<int>("Select Count(*) From Apt_Pople as a Join bs_apt_career as b on a.Aid = b.Apt_Pople_Code Where b.Apt_Code = @Apt_Code And b.bs_division = @bs_division", new { Apt_Code, bs_division });
            }
            
        }

        /// <summary>
        /// 승인된 회원 아이디로 상세보기
        /// </summary>
        /// <param name="Apt_Code"></param>
        /// <param name="User_Code"></param>
        /// <returns></returns>
        public async Task<Join_apt_People_a_Career_Entity> Join_detail_UserCode(string Apt_Code, string User_Code)
        {
            using (var df = new SqlConnection(_db.GetConnectionString("bscity")))
            {
                return await df.QuerySingleOrDefaultAsync<Join_apt_People_a_Career_Entity>("Select Top 1 b.Aid, b.Apt_Code, a.Apt_Name, a.User_Code, a.User_Name, a.LevelCount, b.Dong, b.Ho, a.Scn, a.Mobile, a.Telephone, a.Email, a.Intro, a.VisitCount, a.WriteCount, a.CommentsCount, a.MoveDate, a.PostDate, a.PostIP, a.Withdrawal, b.bs_code, b.bs_name, b.bs_start, b.bs_end, b.bs_division, bs_intro, apt_career_Code, Sw_People_Code From Apt_Pople as a Join bs_apt_career as b on a.Aid = b.Apt_Pople_Code Where b.Apt_Code = @Apt_Code And a.User_Code = @User_Code And b.bs_division = 'A' Order By b.Aid Desc", new { Apt_Code, User_Code });
            }
            
        }

        /// <summary> 
        /// 아이디 존재 여부 확인
        /// </summary>
        /// <param name="bs_code"></param>
        /// <returns></returns>
        public async Task<int> be(string bs_code)
        {
            using (var df = new SqlConnection(_db.GetConnectionString("bscity")))
            {
                return await df.QuerySingleOrDefaultAsync<int>("Select Count(*) From bs_apt_career Where bs_code = @bs_code And bs_division = 'A'", new { bs_code });
            }
            
        }

        /// <summary>
        /// 해당 아파트에 승인된 회원수
        /// </summary>
        /// <param name="Apt_Code"></param>
        /// <returns></returns>
        public async Task<int> be_apt(string Apt_Code)
        {
            using (var df = new SqlConnection(_db.GetConnectionString("bscity")))
            {
                return await df.QuerySingleOrDefaultAsync<int>("Select Count(*) From bs_apt_career Where Apt_Code = @Apt_Code", new { Apt_Code });
            }
            
        }

        /// <summary>
        /// 같은 아이디로 동호 있는 지 확인
        /// </summary>
        /// <param name="bs_code"></param>
        /// <param name="Dong"></param>
        /// <param name="Ho"></param>
        /// <returns></returns>
        public async Task<int> be_dong(string Apt_Code, string bs_code, string Dong, string Ho)
        {
            using (var df = new SqlConnection(_db.GetConnectionString("bscity")))
            {
                return await df.QuerySingleOrDefaultAsync<int>("Select Count(*) From bs_apt_career Where Apt_Code = @Apt_Code And bs_code = @bs_code and Dong = @Dong And Ho = @Ho And bs_division = 'A'", new { Apt_Code, bs_code, Dong, Ho });
            }
            
        }

        /// <summary>
        /// 동호에 정보 있는 지 확인
        /// </summary>
        /// <param name="Dong"></param>
        /// <param name="Ho"></param>
        /// <returns></returns>
        public async Task<int> be_dongHo(string Apt_Code, string Dong, string Ho)
        {
            using (var df = new SqlConnection(_db.GetConnectionString("bscity")))
            {
                return await df.QuerySingleOrDefaultAsync<int>("Select Count(*) From bs_apt_career Where Apt_Code = @Apt_Code And Dong = @Dong And Ho = @Ho And bs_division = 'A'", new { Apt_Code, Dong, Ho });
            }
            
        }

        /// <summary>
        /// 로그인 확인
        /// </summary>
        /// <param name="User_ID"></param>
        /// <param name="Password_sw"></param>
        public async Task<int> Log_views(string User_ID, string Password_sw)
        {
            using (var df = new SqlConnection(_db.GetConnectionString("bscity")))
            {
                return await df.QuerySingleOrDefaultAsync<int>("bsLogin", new { User_ID, Password_sw }, commandType: CommandType.StoredProcedure);
            }
            
        }

        /// <summary>
        /// 입주민 이름 가져오기
        /// </summary>
        /// <param name="bs_Code"></param>
        /// <returns></returns>
        public async Task<string> Log_Name(string bs_Code)
        {
            using (var df = new SqlConnection(_db.GetConnectionString("bscity")))
            {
                return await df.QuerySingleOrDefaultAsync<string>("Select top 1 bs_name From bs_apt_career Where bs_Code = @bs_Code And bs_division = 'A' Order By Aid Desc", new { bs_Code });
            }
        }

        /// <summary>
        /// 입주민 상세정보
        /// </summary>
        /// <param name="bs_Code"></param>
        /// <returns></returns>
        public async Task<bs_apt_career_Entity> detail(string bs_Code, string bs_division)
        {
            using (var df = new SqlConnection(_db.GetConnectionString("bscity")))
            {
                return await df.QuerySingleOrDefaultAsync<bs_apt_career_Entity>("Select Top 1 * From bs_apt_career Where bs_Code = @bs_Code And bs_division = @bs_division Order By Aid Desc", new { bs_Code, bs_division });
            }
        }

        /// <summary>
        /// 해당 입주민의 등록 여부 확인
        /// </summary>
        /// <param name="bs_Code"></param>
        /// <param name="bs_division"></param>
        /// <returns></returns>
        public async Task<int> Be_Count(string bs_Code, string bs_division)
        {
            using (var df = new SqlConnection(_db.GetConnectionString("bscity")))
            {
                return await df.QuerySingleOrDefaultAsync<int>("Select Count(*) From bs_apt_career Where bs_Code = @bs_Code And bs_division = @bs_division", new { bs_Code, bs_division });
            }
        }

    }

    /// <summary>
    /// 가입 입주민 정보
    /// </summary>
    public class In_AptPeople_Lib : IIn_AptPeople_Lib
    {
        private readonly IConfiguration _db;
        public In_AptPeople_Lib(IConfiguration configuration)
        {
            this._db = configuration;
        }

        /// <summary>
        /// 입주민 가입 정보 입력
        /// </summary>
        /// <param name="ap"></param>
        /// <returns></returns>
        public async Task<In_AptPeople_Entity> add(In_AptPeople_Entity ap)
        {
            using (var df = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                var sql = "Insert Into Apt_People (User_Code, Apt_Code, Apt_Name, User_Name, Dong, Ho, Mobile, Scn, Telephone, Email, Intro, PostIP, Pass_Word, Withdrawal) Values (@User_Code, @Apt_Code, @Apt_Name, @User_Name, @Dong, @Ho, @Mobile, @Scn, @Telephone, @Email, @Intro, @PostIP, PwdEncrypt('' + @Pass_Word + ''), @Withdrawal); Select Cast(SCOPE_IDENTITY() As Int);";
                int Aid = await df.QuerySingleOrDefaultAsync<int>(sql, ap);
                ap.Aid = Aid;
                return ap;
            }
            
        }

        /// <summary>
        /// 입주민 가입 정보 수정
        /// </summary>
        /// <param name="ap"></param>
        /// <returns></returns>
        public async Task<In_AptPeople_Entity> edit(In_AptPeople_Entity ap)
        {
            using (var df = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                var sql = "Update Apt_People Set User_Code = @User_Code, User_Name = @User_Name, Dong = @Dong, Ho = @Ho, Scn = @Scn, Mobile = @Mobile, Telephone = @Telephone, Email = @Email, Intro = @Intro, PostIP = @PostIP Where Aid = @Aid";
                await df.ExecuteAsync(sql, ap);
                return ap;
            }
            
        }

        /// <summary>
        /// 암호 변경
        /// </summary>
        /// <param name="Pass_Word"></param>
        /// <param name="Aid"></param>
        /// <returns></returns>
        public async Task Eidt_pass(string Pass_Word, string User_Code)
        {
            using (var df = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                await df.ExecuteAsync("Update Apt_People Set Pass_Word = PwdEncrypt('' + @Pass_Word + '') Where User_Code = @User_Code", new { Pass_Word, User_Code });
            }
            
        }


        /// <summary>
        /// 암호 초기화 변경
        /// </summary>
        public async Task PassResert(int Aid)
        {
            using (var df = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                var sq = "12345";
                //var sql = "Update Apt_People Set Pass_Word = PwdEncrypt('' + @sq + '') Where Aid = @Aid";
                await df.ExecuteAsync("Update Apt_People Set Pass_Word = PwdEncrypt('' + @sq + '') Where Aid = @Aid", new { Aid, sq });
                //await df.ExecuteAsync("Update Apt_People Set Pass_Word = PwdEncrypt('' + @Pass_Word + '') Where User_Code = @User_Code", new { Pass_Word, User_Code });
            }

        }

        /// <summary>
        /// 등급 변경
        /// </summary>
        /// <param name="Pass_Word"></param>
        /// <param name="Aid"></param>
        /// <returns></returns>
        public async Task Edit_Level(int LevelCount, string User_Code)
        {
            using (var df = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                await df.ExecuteAsync("Update Apt_People Set LevelCount = @LevelCount Where User_Code = @User_Code", new { LevelCount, User_Code });
            }
            
        }

        /// <summary>
        /// 방문수 증가
        /// </summary>
        /// <param name="User_Code"></param>
        public async Task VisitCount_Add(string User_Code)
        {
            using (var df = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                await df.ExecuteAsync("Update Apt_People Set VisitCount = VisitCount +1 Where User_Code = @User_Code", new { User_Code });
            }
            
        }

        /// <summary>
        /// 글수 증가
        /// </summary>
        /// <param name="User_Code"></param>
        public async Task WriteCount_Add(string User_Code)
        {
            using (var df = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                await df.ExecuteAsync("Update Apt_People Set WriteCount = WriteCount +1 Where User_Code = @User_Code", new { User_Code });
            }
            
        }

        /// <summary>
        /// 읽은 수 증가
        /// </summary>
        /// <param name="User_Code"></param>
        public async Task ReadCount_Add(string User_Code)
        {
            using (var df = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                await df.ExecuteAsync("Update Apt_People Set ReadCount = ReadCount +1 Where User_Code = @User_Code", new { User_Code });
            }
            
        }

        /// <summary>
        /// 댓글수 증가
        /// </summary>
        /// <param name="User_Code"></param>
        public async Task CommentsCount_Add(string User_Code)
        {
            using (var df = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                await df.ExecuteAsync("Update Apt_People Set CommentsCount = CommentsCount +1 Where User_Code = @User_Code", new { User_Code });
            }
            
        }

        /// <summary>
        /// 파일업로드 수 증가
        /// </summary>
        /// <param name="User_Code"></param>
        public async Task FileUp_Add(string User_Code)
        {
            using (var df = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
               await df.ExecuteAsync("Update Apt_People Set FileUpCount = FileUpCount +1 Where User_Code = @User_Code", new { User_Code });
            }
           
        }

        /// <summary>
        /// 해당 공동주택 가입 입주민 목록
        /// </summary>
        /// <param name="Apt_Code"></param>
        /// <returns></returns>
        public async Task<List<In_AptPeople_Entity>> GetList(string Apt_Code)
        {
            using (var df = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                var lst = await df.QueryAsync<In_AptPeople_Entity>("Select Aid, Apt_Code, Apt_Name, User_Code, User_Name, Dong, Ho, Mobile, Telephone, Intro, LevelCount, VisitCount, WriteCount, CommentsCount, FileUpCount, MoveDate, PostIP, Withdrawal, Approval From Apt_People Where Apt_Code = @Apt_Code", new { Apt_Code });
                return lst.ToList();
            }
            
        }

        /// <summary>
        /// 가입 입주민 상세정보
        /// </summary>
        public async Task<In_AptPeople_Entity> Detail(string User_Code)
        {
            using (var df = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                return await df.QuerySingleOrDefaultAsync<In_AptPeople_Entity>("Select Aid, Apt_Code, Apt_Name, User_Code, User_Name, Dong, Ho, Mobile, Scn, Email, Telephone, Intro, LevelCount, VisitCount, WriteCount, CommentsCount, FileUpCount, MoveDate, PostIP, Withdrawal, Approval From Apt_People Where User_Code = @User_Code", new { User_Code });
            }
            
        }

        /// <summary>
        /// 가입 입주민 상세정보
        /// </summary>
        public async Task<In_AptPeople_Entity> Detail_M(string Mobile)
        {
            string a = Mobile.Replace("-", "");
            string b = a.Substring(0, 3) + "-" + a.Substring(3, 4) + "-" + a.Substring(7, 4);
            using (var df = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                return await df.QuerySingleOrDefaultAsync<In_AptPeople_Entity>("Select Top 1 Aid, Apt_Code, Apt_Name, User_Code, User_Name, Dong, Ho, Mobile, Scn, Email, Telephone, Intro, LevelCount, VisitCount, WriteCount, CommentsCount, FileUpCount, MoveDate, PostIP, Withdrawal, Approval From Apt_People Where (Mobile = @a or Mobile = @b) Order By Aid Desc", new { a, b });
            }

        }


        /// <summary>
        /// 가입한 회원 목록(단지별)
        /// </summary>
        public async Task<List<In_AptPeople_Entity>> aptHumanList(int Page, string Apt_Code)
        {
            using (var df = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                var lst = await df.QueryAsync<In_AptPeople_Entity>("Select Top 15 Aid, Apt_Code, Apt_Name, User_Code, User_Name, Dong, Ho, Mobile, Scn, Email, Telephone, Intro, LevelCount, VisitCount, ReadCount, WriteCount, CommentsCount, FileUpCount, MoveDate, PostDate, PostIP, Withdrawal, Approval From Apt_People Where Aid Not In(Select Top(15 * " + @Page + ") Aid From Apt_People Where Apt_Code = @Apt_Code Order By Aid Desc) and Apt_Code = @Apt_Code Order By Aid Desc", new { Page, Apt_Code });
                return lst.ToList();
            }
            
        }

        /// <summary>
        /// 가입한 회원 수(단지별)
        /// </summary>
        public async Task<int> aptHumanList_Count(string Apt_Code)
        {
            using (var df = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                return await df.QuerySingleOrDefaultAsync<int>("Select Count(*) From Apt_People Where Apt_Code = @Apt_Code", new { Apt_Code });
            }            
        }

        /// <summary>
        /// 가입한 회원 이름으로 검색된 목록(단지별)
        /// </summary>
        public async Task<List<In_AptPeople_Entity>> aptHuman_Name_List(int Page, string Apt_Code, string Name)
        {
            using (var df = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                var lst = await df.QueryAsync<In_AptPeople_Entity>("Select Top 15 Aid, Apt_Code, Apt_Name, User_Code, User_Name, Dong, Ho, Mobile, Scn, Email, Telephone, Intro, LevelCount, VisitCount, ReadCount, WriteCount, CommentsCount, FileUpCount, MoveDate, PostDate, PostIP, Withdrawal, Approval From Apt_People Where Aid Not In(Select Top(15 * " + @Page + ") Aid From Apt_People Where Apt_Code = @Apt_Code And User_Name = @Name Order By Aid Desc) and Apt_Code = @Apt_Code And User_Name = @Name Order By Aid Desc", new { Page, Apt_Code, Name });
                return lst.ToList();
            }

        }

        /// <summary>
        /// 가입한 회원 이름 검색된 수(단지별)
        /// </summary>
        public async Task<int> aptHuman_Name_List_Count(string Apt_Code, string Name)
        {
            using (var df = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                return await df.QuerySingleOrDefaultAsync<int>("Select Count(*) From Apt_People Where Apt_Code = @Apt_Code And User_Name = @Name", new { Apt_Code, Name });
            }
        }

        /// <summary>
        /// 동호로 검색된 목록
        /// </summary>
        /// <returns></returns>
        public async Task<List<In_AptPeople_Entity>> DongHoList(int Page, string Apt_Code, string Dong, string Ho)
        {
            using (var df = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                var lst = await df.QueryAsync<In_AptPeople_Entity>("Select Top 15 Aid, Apt_Code, Apt_Name, User_Code, User_Name, Dong, Ho, Mobile, Scn, Email, Telephone, Intro, LevelCount, VisitCount, WriteCount, CommentsCount, FileUpCount, MoveDate, PostIP, Withdrawal, PostDate, Approval From Apt_People Where Aid Not In(Select Top(15 * " + @Page + ") Aid From Apt_People Where Apt_Code = @Apt_Code And Dong = @Dong And Ho = @Ho Order By Aid Desc) and Apt_Code = @Apt_Code And Dong = @Dong And Ho = @Ho Order By Aid Desc", new { Page, Apt_Code, Dong, Ho });
                return lst.ToList();
            }
            
        }

        /// <summary>
        /// 동호로 검색한 수
        /// </summary>
        /// <param name="Apt_Code"></param>
        /// <param name="Dong"></param>
        /// <param name="Ho"></param>
        /// <returns></returns>
        public async Task<int> DongHoList_Count(string Apt_Code, string Dong, string Ho)
        {
            using (var df = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                return await df.QuerySingleOrDefaultAsync<int>("Select Count(*) From Apt_People Where Apt_Code = @Apt_Code And Dong = @Dong And Ho = @Ho", new { Apt_Code, Dong, Ho });
            }
            
        }

        /// <summary>
        /// 아이디 중복 여부 확인
        /// </summary>
        /// <param name="User_Code"></param>
        /// <returns></returns>
        public async Task<int> Be_UserCode(string User_Code)
        {
            using (var df = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                return await df.QuerySingleOrDefaultAsync<int>("Select Count(*) From Apt_People Where User_Code = @User_Code", new { User_Code });
            }
        }

        /// <summary>
        /// 해당 동호 가입 여부
        /// </summary>
        /// <param name="User_Code"></param>
        /// <param name="Dong"></param>
        /// <param name="Ho"></param>
        /// <returns></returns>
        public async Task<int> Be_DongHo(string User_Code, string Dong, string Ho)
        {
            using (var df = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                return await df.QuerySingleOrDefaultAsync<int>("Select Count(*) From Apt_People Where User_Code = @User_Code And Dong = @Dong And Ho = @Ho", new { User_Code, Dong, Ho });
            }
        }

        /// <summary>
        /// 로그인 확인
        /// </summary>
        /// <param name="User_ID"></param>
        /// <param name="Password"></param>
        public async Task<int> Log_views_M(string MobileA, string MobileB, string Password)
        {
            using (var df = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                //return await df.QuerySingleOrDefaultAsync<int>("bs_Login", new { User_ID, Password }, commandType: CommandType.StoredProcedure);

                return await df.QuerySingleOrDefaultAsync<int>("Select PwdCompare(@Password, Pass_Word) from Apt_People Where (Mobile = @MobileA or Mobile = @MobileB)", new { MobileA, MobileB, Password });
                //return await df.QuerySingleOrDefaultAsync<int>("Select PwdCompare(@Password, Pass_Word) from Apt_People Where (Mobile = @MobileA or Mobile = @MobileB) And Approval = 'B' And LevelCount >= 3", new { MobileA, MobileB, Password });
            }
            
        }

        public async Task<int> Log_views(string User_ID, string Password)
        {
            using (var df = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                return await df.QuerySingleOrDefaultAsync<int>("bs_Login", new { User_ID, Password }, commandType: CommandType.StoredProcedure);
            }                
        }
        /// <summary>
        /// 기입승인
        /// </summary>
        public async Task Approval_being(int Aid)
        {
            using (var df = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                string a = await df.QuerySingleOrDefaultAsync<string>("Select Approval From Apt_People Where Aid = @Aid", new { Aid });
                if (a == "A")
                {
                    await df.ExecuteAsync("Update Apt_People Set Approval = 'B', LevelCount = 3 Where Aid = @Aid", new { Aid });
                }
                else
                {
                    await df.ExecuteAsync("Update Apt_People Set Approval = 'A', LevelCount = 1 Where Aid = @Aid", new { Aid });
                }
            }

        }

        /// <summary>
        /// 모바일 번호 존내 여부 확인
        /// </summary>
        /// <param name="Mobile"></param>
        /// <returns></returns>
        public async Task<int> Mobile_Being(string Mobile)
        {
            string a = Mobile.Replace("-", "");
            string b = a.Substring(0, 3) + "-" + a.Substring(3, 4) + "-" + a.Substring(7, 4);
            using var df = new SqlConnection(_db.GetConnectionString("sw_togather"));
            return await df.QuerySingleOrDefaultAsync<int>("Select Count(*) From Apt_People Where Mobile = @b or Mobile = @a", new { Mobile, a, b });
        }

        /// <summary>
        /// 모바일 번호로 입력된 내용 모두 제거
        /// </summary>
        /// <param name="Mobile"></param>
        /// <returns></returns>
        public async Task Approval_Remove(string Mobile)
        {
            string a = Mobile.Replace("-", "");
            string b = a.Substring(0, 3) + "-" + a.Substring(3, 4) + "-" + a.Substring(7, 4);
            using var df = new SqlConnection(_db.GetConnectionString("sw_togather"));
            await df.ExecuteAsync("Update Apt_People Set Approval = 'A' Where Mobile = @a or Mobile = @b", new { a, b });          

        }
        /// <summary>
        /// 모바일 번호로 입력된 내용 모두 제거
        /// </summary>
        /// <param name="Mobile"></param>
        /// <returns></returns>
        public async Task Approval_Remove_ReSet(string Mobile)
        {
            string a = Mobile.Replace("-", "");
            string b = a.Substring(0, 3) + "-" + a.Substring(3, 4) + "-" + a.Substring(7, 4);
            using var df = new SqlConnection(_db.GetConnectionString("sw_togather"));
            await df.ExecuteAsync("Update Apt_People Set Approval = 'B' Where Mobile = @a or Mobile = @b", new { a, b });

        }

        /// <summary>
        /// 호 중복 제거한 호 리스트
        /// </summary>
        /// <param name="Apt_Code"></param>
        /// <param name="Dong"></param>
        /// <returns></returns>
        public async Task<List<Apt_People_Entity>> Dong_HoList(string Apt_Code, string Dong)
        {
            using var df = new SqlConnection(_db.GetConnectionString("sw_togather"));
            var lst = await df.QueryAsync<Apt_People_Entity>("Select Ho From Apt_People Where Apt_Code = 'B812245' And Dong = '104' Group by Ho Order By CONVERT(INT, Ho) Asc;", new { Apt_Code, Dong });
            return lst.ToList();
        }
    }
}
