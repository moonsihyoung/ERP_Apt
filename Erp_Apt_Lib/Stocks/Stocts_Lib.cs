using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Erp_Apt_Lib.Stocks
{

    public class Stocks_Lib : IStocks_Lib
    {
        private readonly IConfiguration _db;
        public Stocks_Lib(IConfiguration configuration)
        {
            this._db = configuration;
        }

        /// <summary>
        /// 재고관리 제품 코드 입력
        /// </summary>
        public async Task Stock_Code_Write(Stock_Code_Entity Ct)
        {
            using SqlConnection db = new SqlConnection(_db.GetConnectionString("Ayoung"));
            await db.ExecuteAsync("Insert Stock_Code (St_Code, St_Group, St_Name, St_Model, St_Unit, St_Place, St_Dosage, St_Using, St_Section, St_Manual, St_UserID, PostIP, Apt_Code) Values (@St_Code, @St_Group, @St_Name, @St_Model, @St_Unit, @St_Place, @St_Dosage, @St_Using, @St_Section, @St_Manual, @St_UserID, @PostIP, @Apt_Code)", Ct);
        }

        /// <summary>
        /// 재고관리 재품 코드 수  가져오기
        /// </summary>
        public async Task<int> Stock_Code_GetCount()
        {
            using SqlConnection db = new SqlConnection(_db.GetConnectionString("Ayoung"));
            return await db.QuerySingleOrDefaultAsync<int>("St_GetCount", commandType: CommandType.StoredProcedure);
        }

        /// <summary>
        /// 재고관리 재품 코드 수  가져오기 (공동주택) 2019년
        /// </summary>
        public async Task<int> Stock_Code_GetCount_Apt(string AptCode)
        {
            using SqlConnection db = new SqlConnection(_db.GetConnectionString("Ayoung"));
            return await db.QuerySingleOrDefaultAsync<int>("St_GetCount_Apt", new { AptCode }, commandType: CommandType.StoredProcedure);
        }

        

        /// <summary>
        /// 재고 관리 제품 코드 리스트
        /// </summary>
        public async Task<List<Stock_Code_Entity>> St_List(int intPage)
        {
            using SqlConnection db = new SqlConnection(_db.GetConnectionString("Ayoung"));
            var lst = await db.QueryAsync<Stock_Code_Entity>("St_List", new { intPage }, commandType: CommandType.StoredProcedure);
            return lst.ToList();
        }

        /// <summary>
        /// 일반
        /// </summary>
        /// <param name="AptCode"></param>
        /// <returns></returns>
        public async Task<List<Stock_Code_Entity>> St_List_Apt_A(string AptCode)
        {
            using SqlConnection db = new SqlConnection(_db.GetConnectionString("Ayoung"));
            var lst = await db.QueryAsync<Stock_Code_Entity>("Select  Num, St_Code, St_Group, St_Name, St_Model, St_Model_No, St_Unit, St_Place, St_Dosage, St_Using, St_Section, St_Asort, St_Bloom, St_Photo, St_PhotoSize, St_Manual, St_UserID, St_FileName, St_FileSize, PostDate, PostIP, ModifyDate, ModifyIP, Modify_UserID, Apt_Code From Stock_Code Where Apt_Code = @AptCode Order By  Num Desc", new { AptCode });
            return lst.ToList();
        }

        /// <summary>
        /// 재고 관리 제품 코드 리스트 (공동주택별) - 2019년
        /// </summary>
        public async Task<List<Stock_Code_Entity>> St_List_Apt(string AptCode, string Wh_Section)
        {
            if (Wh_Section == "A")
            {
                using SqlConnection db = new SqlConnection(_db.GetConnectionString("Ayoung"));
                var lst = await db.QueryAsync<Stock_Code_Entity>("Select a.Num, a.St_Code, a.St_Group, a.St_Name, a.St_Model, a.St_Model_No, a.St_Unit, a.St_Place, a.St_Dosage, a.St_Using, a.St_Section, a.St_Asort, a.St_Bloom, a.St_Photo, a.St_PhotoSize, a.St_Manual, a.St_UserID, a.St_FileName, a.St_FileSize, a.PostDate, a.PostIP, a.ModifyDate, a.ModifyIP, a.Modify_UserID, a.Apt_Code, b.Wh_Quantity, b.Wh_Balance, b.Wh_Section, b.PostDate as b_date From Stock_Code as a Join Warehouse as b on a.St_Code = b.St_Code Where b.AptCode = @AptCode And b.Wh_Section = 'A' Order By b.Num Desc", new { AptCode });
                return lst.ToList();
                
            }
            else
            {
                using SqlConnection db = new SqlConnection(_db.GetConnectionString("Ayoung"));
                var lst = await db.QueryAsync<Stock_Code_Entity>("Select a.Num, a.St_Code, a.St_Group, a.St_Name, a.St_Model, a.St_Model_No, a.St_Unit, a.St_Place, a.St_Dosage, a.St_Using, a.St_Section, a.St_Asort, a.St_Bloom, a.St_Photo, a.St_PhotoSize, a.St_Manual, a.St_UserID, a.St_FileName, a.St_FileSize, a.PostDate, a.PostIP, a.ModifyDate, a.ModifyIP, a.Modify_UserID, a.Apt_Code, b.Wh_Quantity, b.Wh_Balance, b.Wh_Section, b.PostDate as b_date From Stock_Code as a Join Warehouse as b on a.St_Code = b.St_Code Where b.AptCode = @AptCode And b.Wh_Section = 'A' Order By b.Num Desc", new { AptCode });
                return lst.ToList();
            }
        }


        /// <summary>
        /// 재고 관리 제품 코드 리스트 수 (공동주택별) - 2019년
        /// </summary>
        public async Task<int> St_List_Apt_Count(string AptCode, string Wh_Section)
        {
            using SqlConnection db = new SqlConnection(_db.GetConnectionString("Ayoung"));
            if (Wh_Section == "A")
            {
                return await db.QuerySingleOrDefaultAsync<int>("Select Count(*) From Stock_Code as a Join Warehouse as b on a.St_Code = b.St_Code Where b.AptCode = @AptCode And b.Wh_Section = 'A'", new { AptCode });
            }
            else
            {
                return await db.QuerySingleOrDefaultAsync<int>("Select Count(*) From Stock_Code as a Join Warehouse as b on a.St_Code = b.St_Code Where b.AptCode = @AptCode And b.Wh_Section = 'B'", new { AptCode });
            }
        }


        /// <summary>
        /// 재고 관리 제품 코드 리스트 (공동주택별) - 2020년
        /// </summary>
        public async Task<List<Stock_Code_Entity>> St_List_Apt_New(int Page, string AptCode)
        {
            using SqlConnection db = new SqlConnection(_db.GetConnectionString("Ayoung"));
            var lst = await db.QueryAsync<Stock_Code_Entity>("Select Top 15 Num, St_Code, St_Group, St_Name, St_Model, St_Model_No, St_Unit, St_Place, St_Dosage, St_Using, St_Section, St_Asort, St_Bloom, St_Photo, St_PhotoSize, St_Manual, St_UserID, St_FileName, St_FileSize, PostDate, PostIP, ModifyDate, ModifyIP, Modify_UserID, Apt_Code From Stock_Code Where Num Not In(Select Top(15 * @Page) Num From Stock_Code Where Apt_Code = @AptCode Order By Num Desc) And Apt_Code = @AptCode Order By Num Desc", new { Page, AptCode });
            return lst.ToList();
        }

        /// <summary>
        /// 재고 관리 제품 코드 리스트 (공동주택별) - 2020년
        /// </summary>
        public async Task<int> St_List_Apt_New_Count(string AptCode)
        {
            using SqlConnection db = new SqlConnection(_db.GetConnectionString("Ayoung"));
            return await db.QuerySingleOrDefaultAsync<int>("Select Count(*) From Stock_Code Where Apt_Code = @AptCode", new { AptCode });
        }


        /// <summary>
        /// 재고 관리 제품 코드 리스트 (공동주택별) - 2019년
        /// </summary>
        public async Task<List<Stock_Code_Entity>> St_List_Apt_Query(string AptCode, string Field, string Query, string Section)
        {
            using SqlConnection db = new SqlConnection(_db.GetConnectionString("Ayoung"));
            var lst = await db.QueryAsync<Stock_Code_Entity>("Select a.Num, a.St_Code, a.St_Group, a.St_Name, a.St_Model, a.St_Model_No, a.St_Unit, a.St_Place, a.St_Dosage, a.St_Using, a.St_Section, a.St_Asort, a.St_Bloom, a.St_Photo, a.St_PhotoSize, a.St_Manual, a.St_UserID, a.St_FileName, a.St_FileSize, a.PostDate, a.PostIP, a.ModifyDate, a.ModifyIP, a.Modify_UserID, a.Apt_Code, b.Wh_Quantity, b.Wh_Balance, b.Wh_Section, b.PostDate as b_date From Stock_Code as a Join Warehouse as b on a.St_Code = b.St_Code Where b.AptCode = @AptCode And a." + Field + " Like '%" + Query + "%' And b.Wh_Section = @Section Order By b.Num Desc", new { AptCode, Section, Field, Query });
            return lst.ToList();
        }

        public async Task<List<Stock_Code_Entity>> St_List_Apt_Query_new(string AptCode, string Field, string Query)
        {
            using SqlConnection db = new SqlConnection(_db.GetConnectionString("Ayoung"));
            var lst = await db.QueryAsync<Stock_Code_Entity>("Select Num, St_Code, St_Group, St_Name, St_Model, St_Model_No, St_Unit, St_Place, St_Dosage, St_Using, St_Section, St_Asort, St_Bloom, St_Photo, St_PhotoSize, St_Manual, St_UserID, St_FileName, St_FileSize, PostDate, PostIP, ModifyDate, ModifyIP, Modify_UserID, Apt_Code From Stock_Code Where Apt_Code = @AptCode And " + Field + " Like '%" + Query + "%' Order By Num Desc", new { AptCode, Field, Query });
            return lst.ToList();
        }

        /// <summary>
        /// 재고 관리 제품 코드 리스트 수 (공동주택별) - 2019년
        /// </summary>
        public async Task<int> St_List_Apt_Query_Count(string AptCode, string Faild, string Query)
        {
            using SqlConnection db = new SqlConnection(_db.GetConnectionString("Ayoung"));
            return await db.QuerySingleOrDefaultAsync<int>("Select Count(*) From Stock_Code as a Join Warehouse as b on a.St_Code = b.St_Code Where Apt_Code = @AptCode And a." + Faild + " Like '%" + Query + "%'", new { AptCode, Faild, Query });
        }

        /// <summary>
        /// 재고 관리 제품 코드 리스트 수 (공동주택별) - 2019년
        /// </summary>
        public async Task<int> St_List_Apt_Query_New_Count(string AptCode, string Faild, string Query)
        {
            using SqlConnection db = new SqlConnection(_db.GetConnectionString("Ayoung"));
            return await db.QuerySingleOrDefaultAsync<int>("Select Count(*) From Stock_Code Where Apt_Code = @AptCode And " + Faild + " Like '%" + Query + "%'", new { AptCode, Faild, Query });
        }

        /// <summary>
        /// 재고관리 제품 코드(번호) 상세보기
        /// </summary>
        public async Task<Stock_Code_Entity> St_View(int Num)
        {
            using SqlConnection db = new SqlConnection(_db.GetConnectionString("Ayoung"));
            return await db.QuerySingleOrDefaultAsync<Stock_Code_Entity>("St_View_Num", new { Num }, commandType: CommandType.StoredProcedure);
        }

        /// <summary>
        /// 재고관리 제품 코드(코드) 상세보기
        /// </summary>
        public async Task<Stock_Code_Entity> St_View_Code(string St_Code)
        {
            using SqlConnection db = new SqlConnection(_db.GetConnectionString("Ayoung"));
            return await db.QuerySingleOrDefaultAsync<Stock_Code_Entity>("St_View_Code", new { St_Code }, commandType: CommandType.StoredProcedure);
        }

        /// <summary>
        /// 재고관리 재품 코드 내용 수정하기
        /// </summary>
        public async Task St_Modify(Stock_Code_Entity mm)
        {
            using SqlConnection db = new SqlConnection(_db.GetConnectionString("Ayoung"));
            await db.ExecuteAsync("Update Stock_Code Set St_Group = @St_Group, St_Name = @St_Name, St_Model = @St_Model, St_Unit = @St_Unit, St_Place = @St_Place, St_Dosage = @St_Dosage, St_Using = @St_Using, St_Section = @St_Section, St_Manual = @St_Manual, ModifyDate = GetDate(), ModifyIP = @ModifyIP, Modify_UserID = @Modify_UserID, Apt_Code = @Apt_Code Where Num = @Num", mm);
        }

        /// <summary>
        /// 제품명 찾아오기
        /// </summary>
        public async Task<string> St_Name_Wh(string St_Code)
        {
            using SqlConnection db = new SqlConnection(_db.GetConnectionString("Ayoung"));
            return await db.QuerySingleOrDefaultAsync<string>("Select St_Name From Stock_Code Where St_Code = @St_Code", new { St_Code });
        }

        /// <summary>
        /// 재고관리 제품 코드 찾기
        /// </summary>
        public async Task<List<Stock_Code_Entity>> St_Search(string SearchField, string SearchQuery, int intPage)
        {
            using SqlConnection db = new SqlConnection(_db.GetConnectionString("Ayoung"));
            var lst = await db.QueryAsync<Stock_Code_Entity>("St_Search", new { SearchField, SearchQuery, intPage }, commandType: CommandType.StoredProcedure);
            return lst.ToList();
        }

        /// <summary>
        /// 재고관리 구분코드 제품 정보 찾기
        /// </summary>
        public async Task<List<Stock_Code_Entity>> St_SearchStGroup(string St_Group, string Apt_Code)
        {
            using SqlConnection db = new SqlConnection(_db.GetConnectionString("Ayoung"));
            var lst = await db.QueryAsync<Stock_Code_Entity>("Select Num, St_Code, St_Group, St_Name, St_Model, St_Model_No, St_Unit, St_Place, St_Dosage, St_Using, St_Section, St_Asort, St_Bloom, St_Photo, St_PhotoSize, St_Manual,  St_UserID, St_FileName, St_FileSize, PostDate, PostIP, ModifyDate, ModifyIP, Modify_UserID, Apt_Code From Stock_Code Where St_Group = @St_Group And Apt_Code = @Apt_Code  Order By Num Desc", new { St_Group, Apt_Code });
            return lst.ToList();
        }

        /// <summary>
        /// 재고관리 구분코드 제품 정보 찾은 수
        /// </summary>
        public async Task<int> St_SearchStGroupCount(string St_Group, string Apt_Code)
        {
            using SqlConnection db = new SqlConnection(_db.GetConnectionString("Ayoung"));
            return await db.QuerySingleOrDefaultAsync<int>("Select Num, St_Code, St_Group, St_Name, St_Model, St_Model_No, St_Unit, St_Place, St_Dosage, St_Using, St_Section, St_Asort, St_Bloom, St_Photo, St_PhotoSize, St_Manual,  St_UserID, St_FileName, St_FileSize, PostDate, PostIP, ModifyDate, ModifyIP, Modify_UserID, Apt_Code From Stock_Code Where St_Group = @St_Group And Apt_Code = @Apt_Code", new { St_Group, Apt_Code });
        }

        /// <summary>
        /// 재고관리 제품 코드 마지막 입력 번호
        /// </summary>
        public async Task<int> St_Count_Data()
        {
            using SqlConnection db = new SqlConnection(_db.GetConnectionString("Ayoung"));
            return await db.QuerySingleOrDefaultAsync<int>("Select Top 1 Num From Stock_Code Order By Num Desc");
        }

        /// <summary>
        /// 첨부 파일 다운로드
        /// </summary>
        public async Task<string> St_Down_File(int Num)
        {
            using SqlConnection db = new SqlConnection(_db.GetConnectionString("Ayoung"));
            return await db.QuerySingleOrDefaultAsync<string>("Select St_FileName From Stock_Code Where Num = @Num", new { Num });
        }

        /// <summary>
        /// 첨부 사진 다운로드
        /// </summary>
        public async Task<string> St_Down_Photo(int Num)
        {
            using SqlConnection db = new SqlConnection(_db.GetConnectionString("Ayoung"));
            return await db.QuerySingleOrDefaultAsync<string>("Select St_Photo From Stock_Code Where Num = @Num", new { Num });
        }

        /// <summary>
        /// 자재 기본 정보 삭제
        /// </summary>
        /// <param name="Num"></param>
        public async Task Remove(int Num)
        {
            using SqlConnection db = new SqlConnection(_db.GetConnectionString("Ayoung"));
            await db.ExecuteAsync("Delete Stock_Code Where Num = @Num", new { Num });
        }

        /// <summary>
        /// 자재명 찾기
        /// </summary>
        public async Task<List<Stock_Code_Entity>> stName_Query(string Apt_Code, string Query)
        {
            using SqlConnection db = new SqlConnection(_db.GetConnectionString("Ayoung"));
            var lst = await db.QueryAsync<Stock_Code_Entity>("Select Top 30 * From Stock_Code Where Apt_Code = @Apt_Code And St_Name Like '%" + Query + "%' Order by Num Desc", new { Apt_Code, Query });
            return lst.ToList();
        }
    }



    /// <summary>
    /// 재고관리 함수
    /// </summary>
    public class WhSock_Lib : IWhSock_Lib
    {
        private readonly IConfiguration _db;
        public WhSock_Lib(IConfiguration configuration)
        {
            this._db = configuration;
        }

        /// <summary>
        /// 재고관리 입출고  입력
        /// </summary>
        public async Task Warehouse_Write(WareHouse_Entity Ct)
        {
            using SqlConnection db = new SqlConnection(_db.GetConnectionString("Ayoung"));
            await db.ExecuteAsync("Insert Warehouse (Wh_Section, Wh_Code, St_Code, St_Group, Wh_Quantity, Wh_Balance, Wh_Cost, Wh_Unit, Wh_Place, Wh_Use, Wh_UserID, Wh_Year, Wh_Month, Wh_Day, PostIP, Etc, AptCode, P_Group, Parents) Values(@Wh_Section, @Wh_Code, @St_Code, @St_Group, @Wh_Quantity, @Wh_Balance, @Wh_Cost, @Wh_Unit, @Wh_Place, @Wh_Use, @Wh_UserID, @Wh_Year, @Wh_Month, @Wh_Day, @PostIP, @Etc, @AptCode, @P_Group, @Parents)", Ct);

        }

        /// <summary>
        /// 재고관리 입출고  수정
        /// </summary>
        public async Task Warehouse_Update(WareHouse_Entity Ct)
        {
            using SqlConnection db = new SqlConnection(_db.GetConnectionString("Ayoung"));
            await db.ExecuteAsync("Update Warehouse Set Wh_Quantity = @Wh_Quantity, Wh_Balance = @Wh_Balance, Wh_Cost = @Wh_Cost, Wh_Unit = @Wh_Unit, Wh_Place = @Wh_Place, Wh_Year = @Wh_Year, Wh_Month = @Wh_Month, Wh_Day = @Wh_Day, ModifyDate = @ModifyDate, Modify_IP =@Modify_IP, Modify_UserID = @Modify_UserID, Etc = @Etc Where Num = @Num", Ct);

        }

        /// <summary>
        /// 재고 관리(입출고) 구분명 (드롭다운) 리스트
        /// </summary>
        public async Task<List<Stock_Code_Entity>> Wh_St_Group()
        {
            using SqlConnection db = new SqlConnection(_db.GetConnectionString("Ayoung"));
            var lst = await db.QueryAsync<Stock_Code_Entity>("Select distinct St_Group From Stock_Code");
            return lst.ToList();
        }

        /// <summary>
        /// 재고 관리(입출고) 구분명 (드롭다운) 리스트 (2019년 A)
        /// </summary>
        public async Task<List<Stock_Code_Entity>> Wh_St_Group_A(string AptCode)
        {
            using SqlConnection db = new SqlConnection(_db.GetConnectionString("Ayoung"));
            var lst = await db.QueryAsync<Stock_Code_Entity>("Select distinct St_Group From Stock_Code Where Apt_Code = @AptCode", new { AptCode });
            return lst.ToList();
        }

        /// <summary>
        /// 재고 관리(입출고) 대분류명 (드롭다운) 리스트
        /// </summary>
        public async Task<List<Stock_Code_Entity>> Wh_St_Section(string St_Group)
        {
            using SqlConnection db = new SqlConnection(_db.GetConnectionString("Ayoung"));
            var lst = await db.QueryAsync<Stock_Code_Entity>("Select distinct St_Section From Stock_Code Where St_Group = @St_Group", new { St_Group });
            return lst.ToList();
        }

        /// <summary>
        /// 재고 관리(입출고) 대분류명 (드롭다운) 리스트 2019년 수정
        /// </summary>
        public async Task<List<Stock_Code_Entity>> Wh_St_Section_A(string St_Group, string AptCode)
        {
            using SqlConnection db = new SqlConnection(_db.GetConnectionString("Ayoung"));
            var lst = await db.QueryAsync<Stock_Code_Entity>("Select distinct St_Section From Stock_Code Where St_Group = @St_Group And Apt_Code = @AptCode", new { St_Group, AptCode });
            return lst.ToList();
        }

        /// <summary>
        /// 재고관리 (입출고) 중분류명 (드롭다운) 리스트
        /// </summary>
        public async Task<List<Stock_Code_Entity>> Wh_St_Asort(string St_Group, string St_Section)
        {
            using SqlConnection db = new SqlConnection(_db.GetConnectionString("Ayoung"));
            var lst = await db.QueryAsync<Stock_Code_Entity>("Select distinct St_Asort From Stock_Code Where St_Group = @St_Group And St_Section = @St_Section", new { St_Group, St_Section });
            return lst.ToList();
        }


        /// <summary>
        /// 재고관리 (입출고) 중분류명 (드롭다운) 리스트 2019년 수정
        /// </summary>
        public async Task<List<Stock_Code_Entity>> Wh_St_Asort_A(string St_Group, string St_Section, string AptCode)
        {
            using SqlConnection db = new SqlConnection(_db.GetConnectionString("Ayoung"));
            var lst = await db.QueryAsync<Stock_Code_Entity>("Select distinct St_Asort From Stock_Code Where St_Group = @St_Group And St_Section = @St_Section And Apt_Code = @AptCode", new { St_Group, St_Section, AptCode });
            return lst.ToList();
        }

        /// <summary>
        /// 재고관리 (입출고) 세분류명 (드롭다운) 리스트
        /// <param name="St_Group">구분</param>
        /// <param name="St_Section">대분류</param>
        /// <param name="St_Asort">중분류</param>
        /// </summary>
        public async Task<List<Stock_Code_Entity>> Wh_St_Bloom(string St_Group, string St_Section, string St_Asort)
        {
            using SqlConnection db = new SqlConnection(_db.GetConnectionString("Ayoung"));
            var lst = await db.QueryAsync<Stock_Code_Entity>("Select distinct St_Bloom From Stock_Code Where St_Group = @St_Group And St_Section = @St_Section And St_Asort = @St_Asort", new { St_Group, St_Section, St_Asort });
            return lst.ToList();
        }

        /// <summary>
        /// 재고관리 (입출고) 세분류명 (드롭다운) 리스트 2019년 수정
        /// <param name="St_Group">구분</param>
        /// <param name="St_Section">대분류</param>
        /// <param name="St_Asort">중분류</param>
        /// </summary>
        public async Task<List<Stock_Code_Entity>> Wh_St_Bloom_A(string St_Group, string St_Section, string St_Asort, string Apt_Code)
        {
            using SqlConnection db = new SqlConnection(_db.GetConnectionString("Ayoung"));
            var lst = await db.QueryAsync<Stock_Code_Entity>("Select distinct St_Bloom From Stock_Code Where St_Group = @St_Group And St_Section = @St_Section And St_Asort = @St_Asort Apt_Code = @Apt_Code", new { St_Group, St_Section, St_Asort, Apt_Code });
            return lst.ToList();
        }

        /// <summary>
        /// 재고관리 (입출고) 제품명 (드롭다운) 리스트
        /// <param name="St_Group">구분</param>
        /// <param name="St_Section">대분류</param>
        /// </summary>
        public async Task<List<Stock_Code_Entity>> Wh_St_Section_Code(string St_Group, string St_Section)
        {
            using SqlConnection db = new SqlConnection(_db.GetConnectionString("Ayoung"));
            var lst = await db.QueryAsync<Stock_Code_Entity>("Select distinct St_Code, St_Name From Stock_Code Where St_Group = @St_Group And St_Section = @St_Section", new { St_Group, St_Section });
            return lst.ToList();
        }

        /// <summary>
        /// 재고관리 (입출고) 제품명 (드롭다운) 리스트
        /// <param name="St_Group">구분</param>
        /// <param name="St_Section">대분류</param>
        /// <param name="St_Asort">중분류</param>
        /// </summary>
        public async Task<List<Stock_Code_Entity>> Wh_St_Code(string St_Group, string St_Section, string St_Asort)
        {
            using SqlConnection db = new SqlConnection(_db.GetConnectionString("Ayoung"));
            var lst = await db.QueryAsync<Stock_Code_Entity>("Select distinct St_Code, St_Name From Stock_Code Where St_Group = @St_Group And St_Section = @St_Section and St_Asort = @St_Asort", new { St_Group, St_Section, St_Asort });
            return lst.ToList();
        }

        /// <summary>
        /// 재고관리 (입출고) 제품명 (드롭다운) 리스트 2019년 수정
        /// <param name="St_Group">구분</param>
        /// <param name="St_Section">대분류</param>
        /// <param name="St_Asort">중분류</param>
        /// </summary>
        public async Task<List<Stock_Code_Entity>> Wh_St_Code_A(string St_Group, string St_Section, string St_Asort, string AptCode)
        {
            using SqlConnection db = new SqlConnection(_db.GetConnectionString("Ayoung"));
            var lst = await db.QueryAsync<Stock_Code_Entity>("Select distinct St_Code, St_Name From Stock_Code Where St_Group = @St_Group And St_Section = @St_Section and St_Asort = @St_Asort And Apt_Code = @AptCode", new { St_Group, St_Section, St_Asort, AptCode });
            return lst.ToList();
        }

        /// <summary>
        /// 재고관리 (입출고) 제품명 (드롭다운) 리스트
        /// <param name="St_Group">제품구분</param>
        /// <param name="St_Section">대분류</param>
        /// <param name="St_Asort">중분류</param>
        /// <param name="St_Bloom">세분류</param>
        /// </summary>
        public async Task<List<Stock_Code_Entity>> Wh_St_Code_Bloom(string St_Group, string St_Section, string St_Asort, string St_Bloom)
        {
            using SqlConnection db = new SqlConnection(_db.GetConnectionString("Ayoung"));
            var lst = await db.QueryAsync<Stock_Code_Entity>("Select distinct St_Code, St_Name From Stock_Code Where St_Group = @St_Group And St_Section = @St_Section and St_Asort = @St_Asort and St_Bloom = @St_Bloom", new { St_Group, St_Section, St_Asort, St_Bloom });
            return lst.ToList();
        }

        /// <summary>
        /// 재고관리 (입출고) 제품명 (드롭다운) 리스트 2019년 수정
        /// <param name="St_Group">제품구분</param>
        /// <param name="St_Section">대분류</param>
        /// <param name="St_Asort">중분류</param>
        /// <param name="St_Bloom">세분류</param>
        /// </summary>
        public async Task<List<Stock_Code_Entity>> Wh_St_Code_Bloom_A(string St_Group, string St_Section, string St_Asort, string St_Bloom, string AptCode)
        {
            using SqlConnection db = new SqlConnection(_db.GetConnectionString("Ayoung"));
            var lst = await db.QueryAsync<Stock_Code_Entity>("Select distinct St_Code, St_Name From Stock_Code Where St_Group = @St_Group And St_Section = @St_Section and St_Asort = @St_Asort and St_Bloom = @St_Bloom And Apt_Code = @AptCode", new { St_Group, St_Section, St_Asort, St_Bloom, AptCode });
            return lst.ToList();
        }

        /// <summary>
        /// 재고관리 (입출고) 잔고 수 가져오기
        /// </summary>
        public async Task<int> Wh_Balance(string St_Code, string AptCode)
        {
            using SqlConnection db = new SqlConnection(_db.GetConnectionString("Ayoung"));
            return await db.QuerySingleOrDefaultAsync<int>("Select Top 1 Wh_Balance  From Warehouse Where St_Code =@St_Code And AptCode = @AptCode Order By Num Desc", new { St_Code, AptCode });
        }

        /// <summary>
        /// 재고관리 (입출고) 잔고 수 가져오기
        /// </summary>
        public int Wh_Balance_(string St_Code, string AptCode)
        {
            using SqlConnection db = new SqlConnection(_db.GetConnectionString("Ayoung"));
            return db.QuerySingleOrDefault<int>("Select Top 1 Wh_Balance  From Warehouse Where St_Code =@St_Code And AptCode = @AptCode Order By Num Desc", new { St_Code, AptCode });
        }

        /// <summary>
        /// 재고관리 (입출고) 보관장소
        /// </summary>
        public async Task<string> Wh_Place(string St_Code, string AptCode)
        {
            using SqlConnection db = new SqlConnection(_db.GetConnectionString("Ayoung"));
            return await db.QuerySingleOrDefaultAsync<string>("Select Top 1 Wh_Place  From Warehouse Where St_Code =@St_Code And AptCode = @AptCode Order By Num Desc", new { St_Code, AptCode });
        }

        /// <summary>
        /// 재고관리 (입출고) 잔고 수 존재여부 가져오기
        /// </summary>
        public async Task<int> Wh_Balance_Obj(string St_Code)
        {
            using SqlConnection db = new SqlConnection(_db.GetConnectionString("Ayoung"));
            return await db.QuerySingleOrDefaultAsync<int>("Select Count(*) From Warehouse Where St_Code =@St_Code", new { St_Code });
        }

        /// <summary>
        /// 재고관리 (입출고) 잔고 수 존재여부 가져오기
        /// </summary>
        public int Wh_Balance_Obj_(string St_Code)
        {
            using SqlConnection db = new SqlConnection(_db.GetConnectionString("Ayoung"));
            return db.QuerySingleOrDefault<int>("Select Count(*) From Warehouse Where St_Code =@St_Code", new { St_Code });
        }

        /// <summary>
        /// 재고관리 입출고 마지막 입력 번호
        /// </summary>
        public async Task<int> Wh_Count_Data()
        {
            using SqlConnection db = new SqlConnection(_db.GetConnectionString("Ayoung"));
            return await db.QuerySingleOrDefaultAsync<int>("Select Top 1 Num From Warehouse Order By Num Desc");
        }

        /// <summary>
        /// 재고관리 입출고(번호) 상세보기
        /// </summary>
        public async Task<WareHouse_Entity> Wh_View_Parents(string Parents)
        {
            using SqlConnection db = new SqlConnection(_db.GetConnectionString("Ayoung"));
            return await db.QuerySingleOrDefaultAsync<WareHouse_Entity>("Select Top 1 * From Warehouse Where Parents = @Parents Order by Num Desc", new { Parents });
        }

        /// <summary>
        /// 재고관리 입출고(자재코드로) 상세보기
        /// </summary>
        public async Task<WareHouse_Entity> Wh_View_StCode(string Apt_Code, string St_Code)
        {
            using SqlConnection db = new SqlConnection(_db.GetConnectionString("Ayoung"));
            return await db.QuerySingleOrDefaultAsync<WareHouse_Entity>("Select Top 1 * From Warehouse Where AptCode = @Apt_Code And St_Code = @St_Code Order by Num Desc", new { Apt_Code, St_Code });
        }

        /// <summary>
        /// 재고관리 (입출고) 입출력 존재여부 가져오기
        /// </summary>
        public async Task<int> Wh_Parents_Obj(string Parents)
        {
            using SqlConnection db = new SqlConnection(_db.GetConnectionString("Ayoung"));
            return await db.QuerySingleOrDefaultAsync<int>("Select Count(*) From Warehouse Where Parents =@Parents", new { Parents });
        }

        /// <summary>
        /// 재고관리 입출력 구분에 따라 찾은 수 가져오기
        /// </summary>
        public async Task<int> Wh_InOut_GetCount(string AptCode, string Wh_Section)
        {
            using SqlConnection db = new SqlConnection(_db.GetConnectionString("Ayoung"));
            return await db.QuerySingleOrDefaultAsync<int>("Select Count(*) From Warehouse Where AptCode =@AptCode and Wh_Section = @Wh_Section", new { AptCode, Wh_Section });
        }

        /// <summary>
        /// 재고관리 구분 분류 따라 찾은 수 가져오기
        /// </summary>
        public async Task<int> Wh_Gruop_GetCount(string AptCode, string St_Group)
        {
            using SqlConnection db = new SqlConnection(_db.GetConnectionString("Ayoung"));
            return await db.QuerySingleOrDefaultAsync<int>("Select Count(*) From Warehouse Where AptCode =@AptCode and St_Group = @St_Group", new { AptCode, St_Group });
        }

        /// <summary>
        /// 재고관리 구분 및 대분류 따라 찾은 수 가져오기
        /// </summary>
        public async Task<int> Wh_Gruop_St_Section_GetCount(string AptCode, string St_Group, string St_Section)
        {
            using SqlConnection db = new SqlConnection(_db.GetConnectionString("Ayoung"));
            return await db.QuerySingleOrDefaultAsync<int>("Select Count(*) From Warehouse Where AptCode =@AptCode and St_Group = @St_Group and St_Section = @St_Section", new { AptCode, St_Group, St_Section });
        }

        /// <summary>
        /// 재고관리 제품코드로 검색된 수 가져오기
        /// </summary>
        public async Task<int> Wh_InOut_A_GetCount(string AptCode, string St_Code)
        {
            using SqlConnection db = new SqlConnection(_db.GetConnectionString("Ayoung"));
            return await db.QuerySingleOrDefaultAsync<int>("Select Count(*) From Warehouse Where AptCode =@AptCode and St_Code = @St_Code", new { AptCode, St_Code });
        }

        /// <summary>
        /// 재고관리 입출력 찾기 수 가져오기
        /// </summary>
        public async Task<int> Wh_Search_GetCount(string SearchField, string SearchQuery, string AptCode)
        {
            using SqlConnection db = new SqlConnection(_db.GetConnectionString("Ayoung"));
            return await db.QuerySingleOrDefaultAsync<int>("Select Count(*) From Warehouse Where AptCode =@AptCode and @SearchField = @SearchQuery", new
            {
                SearchField,
                SearchQuery,
                AptCode
            });
        }

        /// <summary>
        /// 재고관리 입출력 기간으로 찾기 수 가져오기
        /// </summary>
        public async Task<int> Wh_Search_A_GetCount(string StartDate, string EndDate, string St_Code, string AptCode)
        {
            using SqlConnection db = new SqlConnection(_db.GetConnectionString("Ayoung"));
            return await db.QuerySingleOrDefaultAsync<int>("Select Count(*) From Warehouse Where PostDate between @StartDate and @EndDate and St_Code = @St_Code and AptCode = @AptCode", new { StartDate, EndDate, St_Code, AptCode });
        }

        /// <summary>
        /// 재고 관리 출입고 리스트
        /// </summary>
        public async Task<List<WareHouse_Entity>> Wh_List_In_Out(int intPage, string AptCode, string Wh_Section)
        {
            using SqlConnection db = new SqlConnection(_db.GetConnectionString("Ayoung"));
            var lst = await db.QueryAsync<WareHouse_Entity>("Wh_List", new { intPage, AptCode, Wh_Section }, commandType: CommandType.StoredProcedure);
            return lst.ToList();
        }

        /// <summary>
        /// 재고 관리 출입고 리스트 (2019년 9월)
        /// </summary>
        public async Task<List<WareHouse_Entity>> Wh_List_All(string AptCode)
        {
            using SqlConnection db = new SqlConnection(_db.GetConnectionString("Ayoung"));
            var lst = await db.QueryAsync<WareHouse_Entity>("Select Num, Wh_Section, Wh_Code, St_Code, St_Group, P_Group, Wh_Quantity, Wh_Balance, Wh_Cost, Wh_Unit, Wh_Place, Wh_Use, Wh_UserID, Wh_Year, Wh_Month, Wh_Day, Wh_FileName, Wh_FileSize, Wh_FileName2, Wh_FileSize2, Wh_FileName3, Wh_FileSize3, Wh_FileName4, Wh_FileSize4, Wh_FileName5, Wh_FileSize5, PostDate, PostIP, ModifyDate, ModifyIP, Modify_UserID, Etc, AptCode, Parents, Sub_Parents From WareHouse Where AptCode = @AptCode Order By Num Desc", new { AptCode });
            return lst.ToList();
        }


        /// <summary>
        /// 재고 관리 출입고 리스트 (2019년 9월) 수
        /// </summary>
        public async Task<int> Wh_List_All_Count(string AptCode)
        {
            using SqlConnection db = new SqlConnection(_db.GetConnectionString("Ayoung"));
            return await db.QuerySingleOrDefaultAsync<int>("Select Count(*) From WareHouse Where AptCode = @AptCode", new { AptCode });
        }

        /// <summary>
        /// 재고 관리 출입고 리스트 (2019년 9월) (입출고 구분으로 선택 하여)
        /// </summary>
        public async Task<List<WareHouse_Entity>> Wh_List_Wh_Section(string AptCode, string Wh_Section)
        {
            using SqlConnection db = new SqlConnection(_db.GetConnectionString("Ayoung"));
            var lst = await db.QueryAsync<WareHouse_Entity>("Select Num, Wh_Section, Wh_Code, St_Code, St_Group, P_Group, Wh_Quantity, Wh_Balance, Wh_Cost, Wh_Unit, Wh_Place, Wh_Use, Wh_UserID, Wh_Year, Wh_Month, Wh_Day, Wh_FileName, Wh_FileSize, Wh_FileName2, Wh_FileSize2, Wh_FileName3, Wh_FileSize3, Wh_FileName4, Wh_FileSize4, Wh_FileName5, Wh_FileSize5, PostDate, PostIP, ModifyDate, ModifyIP, Modify_UserID, Etc, AptCode, Parents, Sub_Parents From WareHouse Where AptCode = @AptCode And Wh_Section = @Wh_Section Order By Num Desc", new { AptCode, Wh_Section });
            return lst.ToList();
        }

        /// <summary>
        /// 재고 관리 출입고 리스트 (2019년 9월) (입출구 구분으로 선택 하여)  수
        /// </summary>
        public async Task<int> Wh_List_Wh_Section_Count(string AptCode, string Wh_Section)
        {
            using SqlConnection db = new SqlConnection(_db.GetConnectionString("Ayoung"));
            return await db.QuerySingleOrDefaultAsync<int>("Select Count(*) From WareHouse Where AptCode = @AptCode And Wh_Section = @Wh_Section", new { AptCode, Wh_Section });
        }

        /// <summary>
        /// 재고 관리 출입고 리스트 (2019년 9월) (자재 구분으로 선택 하여)
        /// </summary>
        public async Task<List<WareHouse_Entity>> Wh_List_St_Grpup(string AptCode, string St_Group)
        {
            using SqlConnection db = new SqlConnection(_db.GetConnectionString("Ayoung"));
            var lst = await db.QueryAsync<WareHouse_Entity>("Select Num, Wh_Section, Wh_Code, St_Code, St_Group, P_Group, Wh_Quantity, Wh_Balance, Wh_Cost, Wh_Unit, Wh_Place, Wh_Use, Wh_UserID, Wh_Year, Wh_Month, Wh_Day, Wh_FileName, Wh_FileSize, Wh_FileName2, Wh_FileSize2, Wh_FileName3, Wh_FileSize3, Wh_FileName4, Wh_FileSize4, Wh_FileName5, Wh_FileSize5, PostDate, PostIP, ModifyDate, ModifyIP, Modify_UserID, Etc, AptCode, Parents, Sub_Parents From WareHouse Where AptCode = @AptCode And St_Group = @St_Group Order By Num Desc", new { AptCode, St_Group });
            return lst.ToList();
        }

        /// <summary>
        /// 재고 관리 출입고 리스트 (2019년 9월) (자재 구분으로 선택 하여) 수
        /// </summary>
        public async Task<int> Wh_List_St_Grpup_Count(string AptCode, string St_Group)
        {
            using SqlConnection db = new SqlConnection(_db.GetConnectionString("Ayoung"));
            return await db.QuerySingleOrDefaultAsync<int>("Select Count(*) From WareHouse Where AptCode = @AptCode And St_Group = @St_Group", new { AptCode, St_Group });
        }

        /// <summary>
        /// 재고 관리 출입고 리스트 (2019년 9월) (자재 구분으로 선택 하여)
        /// </summary>
        public async Task<List<WareHouse_Entity>> Wh_List_StName(string AptCode, string St_Name)
        {
            using SqlConnection db = new SqlConnection(_db.GetConnectionString("Ayoung"));
            var lst = await db.QueryAsync<WareHouse_Entity>("Select a.Num, a.Wh_Section, a.Wh_Code, a.St_Code, a.St_Group, a.P_Group, a.Wh_Quantity, a.Wh_Balance, a.Wh_Cost, a.Wh_Unit, a.Wh_Place, a.Wh_Use, a.Wh_UserID, a.Wh_Year, a.Wh_Month, a.Wh_Day, a.Wh_FileName, a.Wh_FileSize, a.Wh_FileName2, a.Wh_FileSize2, a.Wh_FileName3, a.Wh_FileSize3, a.Wh_FileName4, a.Wh_FileSize4, a.Wh_FileName5, a.Wh_FileSize5, a.PostDate, a.PostIP, a.ModifyDate, a.ModifyIP, a.Modify_UserID, a.Etc, a.AptCode, a.Parents, a.Sub_Parents From WareHouse as a Join Stock_Code as b on a.St_Code = b.St_Code Where a.AptCode = @AptCode And b.St_Name Like '%" + St_Name + "%' Order By Num Desc", new { AptCode, St_Name });
            return lst.ToList();
        }

        /// <summary>
        /// 재고 관리 출입고 리스트 (2019년 9월) (자재 구분으로 선택 하여) 수
        /// </summary>
        public async Task<int> Wh_List_StName_Count(string AptCode, string St_Name)
        {
            using SqlConnection db = new SqlConnection(_db.GetConnectionString("Ayoung"));
            return await db.QuerySingleOrDefaultAsync<int>("Select Count(*) From WareHouse as a Join Stock_Code as b on a.St_Code = b.St_Code Where AptCode = @AptCode And b.St_Name Like '%" + St_Name + "%'", new { AptCode, St_Name });
        }

        /// <summary>
        /// 재고 관리 출입고 검색 리스트
        /// </summary>
        public async Task<List<WareHouse_Entity>> Wh_List_In_Out_A(int intPage, string AptCode, string Wh_Section, string St_Code)
        {
            using SqlConnection db = new SqlConnection(_db.GetConnectionString("Ayoung"));
            var lst = await db.QueryAsync<WareHouse_Entity>("Wh_List_A", new { intPage, AptCode, Wh_Section, St_Code }, commandType: CommandType.StoredProcedure);
            return lst.ToList();
        }

        /// <summary>
        /// 재고 관리 출입고 찾기 리스트
        /// </summary>
        public async Task<List<WareHouse_Entity>> Search_Wh_List(string SearchField, string SearchQuery, int intPage, string AptCode)
        {
            using SqlConnection db = new SqlConnection(_db.GetConnectionString("Ayoung"));
            var lst = await db.QueryAsync<WareHouse_Entity>("Search_Warehouse", new { SearchField, SearchQuery, intPage, AptCode }, commandType: CommandType.StoredProcedure);
            return lst.ToList();
        }

        /// <summary>
        /// 재고 관리 출입고 기간으로 찾기 리스트
        /// </summary>
        public async Task<List<WareHouse_Entity>> Search_A_Wh_List(string StartDate, string EndDate, string St_Code, string AptCode, int intPage)
        {
            using SqlConnection db = new SqlConnection(_db.GetConnectionString("Ayoung"));
            var lst = await db.QueryAsync<WareHouse_Entity>("List_Warehouse_Search", new { StartDate, EndDate, St_Code, AptCode, intPage }, commandType: CommandType.StoredProcedure);
            return lst.ToList();
        }

        /// <summary>
        /// 재고관리 출고 리스트 (
        /// </summary>
        public async Task<List<WareHouse_Entity>> Wh_List_Parents(string AptCode, string Parents)
        {
            using SqlConnection db = new SqlConnection(_db.GetConnectionString("Ayoung"));
            var lst = await db.QueryAsync<WareHouse_Entity>("Select * From Warehouse Where AptCode =@AptCode and Parents = @Parents", new { AptCode, Parents });
            return lst.ToList();
        }

        /// <summary>
        /// 재고관리 입출고(번호) 상세보기
        /// </summary>
        public async Task<List<WareHouse_Entity>> Wh_View_Num(int Num)
        {
            using SqlConnection db = new SqlConnection(_db.GetConnectionString("Ayoung"));
            var lst = await db.QueryAsync<WareHouse_Entity>("Select * From Warehouse Where Num = @Num", new { Num });
            return lst.ToList();
        }

        /// <summary>
        /// 재고관리 입고내용 수정하기
        /// </summary>
        public async Task Wh_Modify_In(WareHouse_Entity mm)
        {
            using SqlConnection db = new SqlConnection(_db.GetConnectionString("Ayoung"));
            await db.ExecuteAsync("Wh_Modify", mm, commandType: CommandType.StoredProcedure);
        }

        /// <summary>
        /// 재고관리 입고 마지막 입력 번호
        /// </summary>
        public async Task<int> Wh_Count_Last(string AptCode, string St_Code, string Wh_Section)
        {
            using SqlConnection db = new SqlConnection(_db.GetConnectionString("Ayoung"));
            return await db.QuerySingleOrDefaultAsync<int>("Select Top 1 Num From Warehouse Where AptCode = @AptCode And St_Code = @St_Code And Wh_Section = @Wh_Section Order By Num Desc", new { AptCode, St_Code, Wh_Section });
        }

        /// <summary>
        /// 재고관리 마지막 일련번호
        /// </summary>
        /// <returns></returns>
        public async Task<int> Wh_LastNumber()
        {
            using SqlConnection db = new SqlConnection(_db.GetConnectionString("Ayoung"));
            return await db.QuerySingleOrDefaultAsync<int>("Select Top 1 Num From Warehouse Order By Num Desc");
        }

        /// <summary>
        /// 재고관리 입고내용 삭제하기
        /// </summary>
        public async Task Wh_Delete(int Num)
        {
            using SqlConnection db = new SqlConnection(_db.GetConnectionString("Ayoung"));
            await db.ExecuteAsync("Delete Warehouse Where Num=@Num", new { Num });
        }

        /// <summary>
        /// 자재명 여부 검색
        /// </summary>
        public async Task<int> SearchSt_Model(string St_Name)
        {
            using SqlConnection db = new SqlConnection(_db.GetConnectionString("Ayoung"));
            return await db.QuerySingleOrDefaultAsync<int>("Select Count(*) From Stock_Code Where St_Name =@St_Name", new { St_Name });
        }

        /// <summary>
        /// 자재명 여부 검색 공동주택 2019년
        /// </summary>
        public async Task<int> SearchSt_Model_Apt(string St_Name, string AptCode)
        {
            using SqlConnection db = new SqlConnection(_db.GetConnectionString("Ayoung"));
            return await db.QuerySingleOrDefaultAsync<int>("Select Count(*) From Stock_Code Where Apt_Code = @AptCode And St_Name =@St_Name", new { St_Name, AptCode });
        }

        /// <summary>
        /// 재고 관리 중복 여부 리스트 만들기
        /// </summary>
        public async Task<List<Stock_Code_Entity>> St_Model_List(string St_Group, string St_Section, string St_Asort, string St_Bloom)
        {
            using SqlConnection db = new SqlConnection(_db.GetConnectionString("Ayoung"));
            var lst = await db.QueryAsync<Stock_Code_Entity>("Select * From Stock_Code Where St_Group = @St_Group And St_Section = @St_Section And St_Asort = @St_Asort And St_Bloom = @St_Bloom", new { St_Group, St_Section, St_Asort, St_Bloom });
            return lst.ToList();
        }

        /// <summary>
        /// 재고 관리 제품명으로 찾기
        /// </summary>
        public async Task<List<Stock_Code_Entity>> St_Name_Search_List(string St_Name)
        {
            using SqlConnection db = new SqlConnection(_db.GetConnectionString("Ayoung"));
            var lst = await db.QueryAsync<Stock_Code_Entity>("Select Top 15 * From Stock_Code Where St_Name Like '%" + St_Name + "%' Order By Num Desc", new { St_Name });
            return lst.ToList();
        }

        /// <summary>
        /// 재고 관리 제품명으로 찾기(공동주택) 2019년
        /// </summary>
        public async Task<List<Stock_Code_Entity>> St_Name_Search_List_Apt(string St_Name, string AptCode)
        {
            using SqlConnection db = new SqlConnection(_db.GetConnectionString("Ayoung"));
            var lst = await db.QueryAsync<Stock_Code_Entity>("Select * From Stock_Code Where Apt_Code = @AptCode And St_Name Like '%" + St_Name + "%' And AptCode = @Apt_Code Order By Num Desc", new { St_Name, AptCode });
            return lst.ToList();
        }

        /// <summary>
        /// 재고 관리 제품명으로 중복검사
        /// </summary>
        public async Task<int> St_Name_Search_Count(string St_Name, string AptCode)
        {
            using SqlConnection db = new SqlConnection(_db.GetConnectionString("Ayoung"));
            return await db.QuerySingleOrDefaultAsync<int>("Select Count(*) From Stock_Code Where St_Name = @St_Name And AptCode = @AptCode", new { St_Name, AptCode });
        }

        /// <summary>
        /// 1. 재고 관리에서 제품 구분별로 불러 온 제품 코드 및 제품명 중복없는 리스트 (재고관리 전체 보고서)
        /// </summary>
        public async Task<List<Stock_Code_Entity>> St_Wh_T_List_Data(int Page, string AptCode, string St_Group)
        {
            using SqlConnection db = new SqlConnection(_db.GetConnectionString("Ayoung"));
            var lst = await db.QueryAsync<Stock_Code_Entity>("Wh_Group_T_List", new { Page, AptCode, St_Group }, commandType: CommandType.StoredProcedure);
            return lst.ToList();
        }

        /// <summary>
        /// 1_1. 재고 관리에서 제품 구분별로 불러 온 제품 코드 및 제품명 중복없는 수 (재고관리 전체 보고서)
        /// </summary>
        public async Task<int> St_Wh_T_GetCount_Data(string AptCode_Ds, string St_Code_Ds)
        {
            using SqlConnection db = new SqlConnection(_db.GetConnectionString("Ayoung"));
            return await db.QuerySingleOrDefaultAsync<int>("Select Count(*) From Warehouse Where AptCode = @AptCode_Ds And St_Code =@St_Code_Ds", new { AptCode_Ds, St_Code_Ds });
        }

        /// <summary>
        /// 2. 재고 관리에서 제품구분 및 대분류 별로 볼러 온 제품코드 및 제품명 중복 없는 리스트 (재고관리 전체 보고서)
        /// </summary>
        public async Task<List<SC_WH_Join_Entity>> St_Wh_Section_List_Data(int Page, string AptCode, string St_Group, string St_Section)
        {
            using SqlConnection db = new SqlConnection(_db.GetConnectionString("Ayoung"));
            var lst = await db.QueryAsync<SC_WH_Join_Entity>("Select distinct Warehouse.St_Code From Warehouse inner join Stock_Code on Warehouse.St_Code = Stock_Code.St_Code and Warehouse.AptCode = @AptCode and Warehouse.St_Group = @St_Group and Stock_Code.St_Section = @St_Section", new { Page, AptCode, St_Group, St_Section });
            return lst.ToList();
        }

        /// <summary>
        /// 2_2. 재고 관리에서 제품구분 및 중분류 별로 볼러 온 제품코드 및 제품명 중복 없는 리스트 (재고관리 전체 보고서)
        /// </summary>
        public async Task<List<SC_WH_Join_Entity>> St_Wh_Asort_List_Data(int Page, string AptCode, string St_Group, string St_Section, string St_Asort)
        {
            using SqlConnection db = new SqlConnection(_db.GetConnectionString("Ayoung"));
            var lst = await db.QueryAsync<SC_WH_Join_Entity>("Select distinct Warehouse.St_Code From Warehouse inner join Stock_Code on Warehouse.St_Code = Stock_Code.St_Code and Warehouse.AptCode = @AptCode and Warehouse.St_Group = @St_Group and Stock_Code.St_Section = @St_Section And Stock_Code.St_Asort = @St_Asort", new { Page, AptCode, St_Group, St_Section, St_Asort });
            return lst.ToList();
        }

        /// <summary>
        /// 2_1. 재고 관리에서 제품 구분별로 불러 온 제품 코드 및 제품명 중복없는 수 (재고관리 전체 보고서)
        /// </summary>
        public async Task<int> St_Wh_Section_GetCount_Data(string AptCode_Ds, string St_Code_Ds, string St_Section_Ds)
        {
            using SqlConnection db = new SqlConnection(_db.GetConnectionString("Ayoung"));
            return await db.QuerySingleOrDefaultAsync<int>("Select Count(*) From Warehouse Where AptCode = @AptCode_Ds And St_Code =@St_Code_Ds And St_Section = @St_Section_Ds", new { AptCode_Ds, St_Code_Ds, St_Section_Ds });
        }

        /// <summary>
        /// 3. 재고 관리에서 제품 구분별로 불러 온 제품 코드 및 제품명으로 각 리스트에서 보관장소 불러오기 
        /// </summary>
        public async Task<string> St_Wh_Section_List_Detail_Data(string St_Code)
        {
            using SqlConnection db = new SqlConnection(_db.GetConnectionString("Ayoung"));
            return await db.QuerySingleOrDefaultAsync<string>("Select Wh_Place From Warehouse Where St_Code = @St_Code", new { St_Code });
        }

        /// <summary>
        /// 4. 재고 관리에서  제품코드명으로 불러 온 각 제품코드로 입출력 내용 리스트 
        /// </summary>
        public async Task<List<WareHouse_Entity>> St_Wh_Code_List_Total_Data(string AptCode, string St_Code)
        {
            using SqlConnection db = new SqlConnection(_db.GetConnectionString("Ayoung"));
            string stryear = DateTime.Now.Year.ToString();

            var lst = await db.QueryAsync<WareHouse_Entity>("Select *  From Warehouse Where AptCode = @AptCode And St_Code = @St_Code and Wh_Year = " + stryear + "Order By Num Desc", new { AptCode, St_Code });
            return lst.ToList();
        }




        /// <summary>
        /// 5. 제품코드에서 제품 구분별로 불러 온 제품 코드 및 제품명으로 각 리스트에서 상세내용 불러오기 
        /// </summary>
        public async Task<List<Stock_Code_Entity>> St_Code_List_Detail_Data(string St_Code)
        {
            using SqlConnection db = new SqlConnection(_db.GetConnectionString("Ayoung"));
            var lst = await db.QueryAsync<Stock_Code_Entity>("Select * From Stock_Code Where St_Code = @St_Code", new { St_Code });
            return lst.ToList();
        }

        /// <summary>
        /// 제품 코드 테이블에서 구분으로 검색한 리스트 1
        /// </summary>
        public async Task<List<Stock_Code_Entity>> St_Group_List(int intPage, string St_Group)
        {
            using SqlConnection db = new SqlConnection(_db.GetConnectionString("Ayoung"));
            var lst = await db.QueryAsync<Stock_Code_Entity>("St_List_Gruop", new { intPage, St_Group }, commandType: CommandType.StoredProcedure);
            return lst.ToList();
        }

        /// <summary>
        /// 제품 코드 테이블에서 구분으로 검색한 리스트 1 공동주택 2019년
        /// </summary>
        public async Task<List<Stock_Code_Entity>> St_Group_List_Apt(string St_Group, string AptCode)
        {
            using SqlConnection db = new SqlConnection(_db.GetConnectionString("Ayoung"));
            var lst = await db.QueryAsync<Stock_Code_Entity>("Select * From Stock_Code  Where Apt_Code = @AptCode And St_Group = @St_Group Order By Num Desc", new { St_Group, AptCode });
            return lst.ToList();
        }

        /// <summary>
        /// 제품 코드 테이블에서 공동주택 검색된 수 2019년
        /// </summary>
        public async Task<int> St_List_AptCount(string AptCode)
        {
            using SqlConnection db = new SqlConnection(_db.GetConnectionString("Ayoung"));
            return await db.QuerySingleOrDefaultAsync<int>("Select Count(*) From Stock_Code Where Apt_Code = @AptCode", new { AptCode });
        }

        /// <summary>
        /// 제품 코드 테이블에서 구분 및 대분류로 검색한 리스트 2
        /// </summary>
        public async Task<List<Stock_Code_Entity>> St_Group_Section_List(int intPage, string St_Group, string St_Section)
        {
            using SqlConnection db = new SqlConnection(_db.GetConnectionString("Ayoung"));
            var lst = await db.QueryAsync<Stock_Code_Entity>("St_List_Gruop_Section", new { intPage, St_Group, St_Section }, commandType: CommandType.StoredProcedure);
            return lst.ToList();
        }

        /// <summary>
        /// 제품 코드 테이블에서 구분 및 대분류, 중분류로 검색한 리스트 2
        /// </summary>
        public async Task<List<Stock_Code_Entity>> St_Group_Section_Asort_List(int intPage, string St_Group, string St_Section, string St_Asort)
        {
            using SqlConnection db = new SqlConnection(_db.GetConnectionString("Ayoung"));
            var lst = await db.QueryAsync<Stock_Code_Entity>("St_List_Gruop_Section_Asort", new { intPage, St_Group, St_Section, St_Asort }, commandType: CommandType.StoredProcedure);
            return lst.ToList();
        }

        /// <summary>
        /// 재고관리 구분으로 검색된 재품 코드 수  가져오기
        /// </summary>
        public async Task<int> Stock_Code_GetCount_Group(string St_Group)
        {
            using SqlConnection db = new SqlConnection(_db.GetConnectionString("Ayoung"));
            return await db.QuerySingleOrDefaultAsync<int>("St_GetCount_Group", new { St_Group }, commandType: CommandType.StoredProcedure);
        }

        /// <summary>
        /// 재고관리 구분 및 대분류로 검색된 재품 코드 수  가져오기
        /// </summary>
        public async Task<int> Stock_Code_GetCount_Group_Section(string St_Group, string St_Section)
        {
            using SqlConnection db = new SqlConnection(_db.GetConnectionString("Ayoung"));
            return await db.QuerySingleOrDefaultAsync<int>("St_GetCount_Group_Section", new { St_Group, St_Section }, commandType: CommandType.StoredProcedure);
        }

        /// <summary>
        /// 재고관리 구분 및 대분류로 검색된 재품 코드 수  가져오기
        /// </summary>
        public async Task<int> Stock_Code_GetCount_Group_Section_Asort(string St_Group, string St_Section, string St_Asort)
        {
            using SqlConnection db = new SqlConnection(_db.GetConnectionString("Ayoung"));
            return  await db.QuerySingleOrDefaultAsync<int>("St_GetCount_Group_Section_Asort", new { St_Group, St_Section, St_Asort }, commandType: CommandType.StoredProcedure);
        }

        /// <summary>
        /// 해당년도 제품코드별 총 구입비용 구하기
        /// </summary>
        public async Task<int> Wh_Cost_Sum_Year(string AptCode, string St_Code, string PostDate, string PostDate_i)
        {
            using SqlConnection db = new SqlConnection(_db.GetConnectionString("Ayoung"));
            return await db.QuerySingleOrDefaultAsync<int>("Select Sum(Wh_Cost) from Warehouse where AptCode = @AptCode And St_Code = @St_Code and (PostDate >= @PostDate) and (PostDate < @PostDate_i )", new { AptCode, St_Code, PostDate, PostDate_i });
        }

        /// <summary>
        /// 해당년도 제품코드별 입출고 합계 구하기
        /// </summary>
        public async Task<int> Wh_Quantity_Sum_Year(string AptCode, string St_Code, string PostDate, string PostDate_i, string Wh_Quantity, string Wh_Section)
        {
            using SqlConnection db = new SqlConnection(_db.GetConnectionString("Ayoung"));
            return await db.QuerySingleOrDefaultAsync<int>("Select Sum(" + Wh_Quantity + ") from Warehouse where AptCode = @AptCode And St_Code = @St_Code And Wh_Section = @Wh_Section and (PostDate >= @PostDate) and (PostDate < @PostDate_i)", new { AptCode, St_Code, PostDate, PostDate_i, Wh_Quantity, Wh_Section });
        }

        /// <summary>
        /// 전년도 제품코드별 이월 잔고 구하기
        /// </summary>
        public async Task<int> Wh_Balance_Ago_Year(string AptCode, string St_Code, string strPostDate, string strPostDate_A)
        {
            using SqlConnection db = new SqlConnection(_db.GetConnectionString("Ayoung"));
            return await db.QuerySingleOrDefaultAsync<int>("Select Top 1 Wh_Balance from Warehouse where AptCode = @AptCode And St_Code = @St_Code And PostDate >= @strPostDate and PostDate <= @strPostDate_A Order By Num Desc", new { AptCode, St_Code, strPostDate, strPostDate_A });
        }

        /// <summary>
        /// 마지막 잔고 구하기 2019년 7월(Wh_Balance)
        /// </summary>
        /// <param name="AptCode"></param>
        /// <param name="St_Code"></param>
        /// <returns></returns>
        public int Wh_BalanceNew(string AptCode, string St_Code)
        {
            using SqlConnection db = new SqlConnection(_db.GetConnectionString("Ayoung"));
            return db.QuerySingleOrDefault<int>("Select Top 1 Wh_Balance from Warehouse where AptCode = @AptCode And St_Code = @St_Code Order By Num Desc", new { AptCode, St_Code });
        }

        /// <summary>
        /// 해당 년도 미자막 잔고 값 가져오기
        /// </summary>
        public int Wh_BalanceYearLast(string AptCode, string St_Code, string Wh_Year)
        {
            using SqlConnection db = new SqlConnection(_db.GetConnectionString("Ayoung"));
            return db.QuerySingleOrDefault<int>("Select Top 1 Wh_Balance from Warehouse where AptCode = @AptCode And St_Code = @St_Code And Wh_Year = @Wh_Year Order By Num Desc", new { AptCode, St_Code, Wh_Year });
        }


        /// <summary>
        /// 자재명 관리에 입력된 마지막 일련번호
        /// </summary>
        /// <returns></returns>
        public async Task<string> St_Aid_Last()
        {
            using SqlConnection db = new SqlConnection(_db.GetConnectionString("Ayoung"));
            return await db.QuerySingleOrDefaultAsync<string>("Select Num From Stock_Code Order By Num Desc");
        }

        /// <summary>
        /// 해당 년도의 입출고 합계 구하기
        /// </summary>
        /// <param name="Year">해당년</param>
        /// <param name="St_Code">자재코드</param>
        /// <param name="AptCode">공동주택코드</param>
        /// <param name="Wh_Section">입출고 여부 A:B</param>
        /// <returns></returns>
        public int InOutSum(string Year, string St_Code, string AptCode, string Wh_Section)
        {
            using SqlConnection db = new SqlConnection(_db.GetConnectionString("Ayoung"));
            return db.QuerySingleOrDefault<int>("Select ISNULL(Sum(Wh_Quantity), 0) From Warehouse Where Wh_Year = @Year and St_Code = @St_Code and AptCode = @AptCode And Wh_Section = @Wh_Section", new { Year, St_Code, AptCode, Wh_Section });
        }

        /// <summary>
        /// 자재관리 출고 리스트 만들기
        /// </summary>
        /// <param name="AptCode"></param>
        /// <param name="P_Group"></param>
        /// <param name="Parents"></param>
        /// <returns></returns>
        public async Task<List<WareHouse_Entity>> Group_List(string AptCode, string P_Group, string Parents, string Wh_Section)
        {
            using SqlConnection db = new SqlConnection(_db.GetConnectionString("Ayoung"));
            var lst = await db.QueryAsync<WareHouse_Entity>("Select * From Warehouse Where AptCode = @AptCode And P_Group = @P_Group And Parents = @Parents And Wh_Section = @Wh_Section", new { AptCode, P_Group, Parents, Wh_Section });
            return lst.ToList();
        }

        /// <summary>
        /// 자재관리 입출고 리스트(2020)
        /// </summary>
        public async Task<List<WareHouse_Entity>> GetList_StCode(int Page, string AptCode, string St_Code)
        {
            using SqlConnection db = new SqlConnection(_db.GetConnectionString("Ayoung"));
            var lst = await db.QueryAsync<WareHouse_Entity>("Select Top 15 Num, Wh_Section, Wh_Code, St_Code, St_Group, P_Group, Wh_Quantity, Wh_Balance, Wh_Cost, Wh_Unit, Wh_Place, Wh_Use, Wh_UserID, Wh_Year, Wh_Month, Wh_Day, Wh_FileName, Wh_FileSize, Wh_FileName2, Wh_FileSize2, Wh_FileName3, Wh_FileSize3, Wh_FileName4, Wh_FileSize4, Wh_FileName5, Wh_FileSize5, PostDate, PostIP, ModifyDate, ModifyIP, Modify_UserID, Etc, AptCode, Parents, Sub_Parents From Warehouse Where Num Not In (Select top (@Page * 15) Num From Warehouse Where AptCode = @AptCode And St_Code = @St_Code Order By Num Desc) And AptCode = @AptCode And St_Code = @St_Code Order By Num Desc", new { Page, AptCode, St_Code });
            return lst.ToList();
        }

        /// <summary>
        /// 자재관리 입출고 리스트 수(2020)
        /// </summary>
        /// <param name="AptCode"></param>
        /// <param name="St_Code"></param>
        /// <returns></returns>
        public async Task<int> GetList_StCodeCount(string AptCode, string St_Code)
        {
            using SqlConnection db = new SqlConnection(_db.GetConnectionString("Ayoung"));
            return await db.QuerySingleOrDefaultAsync<int>("Select Count(*) From Warehouse Where AptCode = @AptCode And St_Code = @St_Code", new { AptCode, St_Code });
        }

        public async Task<DateTime> WareHouseDate(string AptCode, string St_Code)
        {
            using SqlConnection db = new SqlConnection(_db.GetConnectionString("Ayoung"));
            return await db.QuerySingleOrDefaultAsync<DateTime>("Select Top 1 PostDate From Warehouse Where AptCode = @AptCode And St_Code = @St_Code Order By Num Desc", new { AptCode, St_Code });
        }

        /// <summary>
        /// 자재관리 입출고 리스트 만들기
        /// </summary>
        public async Task<List<WareHouse_Entity>> Group_List_AB(string AptCode, string P_Group, string Parents)
        {
            using SqlConnection db = new SqlConnection(_db.GetConnectionString("Ayoung"));
            var lst = await db.QueryAsync<WareHouse_Entity>("Select * From Warehouse Where AptCode = @AptCode And P_Group = @P_Group And Parents = @Parents", new { AptCode, P_Group, Parents });
            return lst.ToList();
        }

        public async Task<List<SC_WH_Join_Entity>> Group_List_ABC(string AptCode, string P_Group, string Parents)
        {
            using SqlConnection db = new SqlConnection(_db.GetConnectionString("Ayoung"));
            var lst = await db.QueryAsync<SC_WH_Join_Entity>("Select a.Num, a.Wh_Section, a.Wh_Code, a.St_Code, a.St_Group, a.P_Group, a.Wh_Quantity, a.Wh_Balance, a.Wh_Cost, a.Wh_Unit, a.Wh_Place, a.Wh_Use, a.Wh_UserID, a.Wh_Year, a.Wh_Month, a.Wh_Day, a.Wh_FileName, a.Wh_FileSize, a.Wh_FileName2, a.Wh_FileSize2, a.Wh_FileName3, a.Wh_FileSize3, a.Wh_FileName4, a.Wh_FileSize4, a.Wh_FileName5, a.Wh_FileSize5, a.Etc, a.AptCode, a.Parents, a.Sub_Parents, b.St_Name From Warehouse as a Join Stock_Code as b on a.St_Code = b.St_Code Where a.AptCode = @AptCode And a.P_Group = @P_Group And a.Parents = @Parents", new { AptCode, P_Group, Parents });
            return lst.ToList();
        }
        /// <summary>
        /// 해당 작업에 입력된 자재관리 수
        /// </summary>
        public async Task<int> Wh_InputCount(string AptCode, string P_Group, string Parents, string Wh_Section)
        {
            using SqlConnection db = new SqlConnection(_db.GetConnectionString("Ayoung"));
            return await db.QuerySingleOrDefaultAsync<int>("Select Count(*) From Warehouse Where AptCode = @AptCode And P_Group = @P_Group and Parents = @Parents", new { AptCode, P_Group, Parents, Wh_Section });
        }

        /// <summary>
        /// 년간 비용합계
        /// </summary>
        /// <param name="AptCode"></param>
        /// <param name="St_Code"></param>
        /// <param name="Wh_Year"></param>
        /// <returns></returns>
        public int wh_Cost_Sum(string AptCode, string St_Code, string Wh_Year)
        {
            using SqlConnection db = new SqlConnection(_db.GetConnectionString("Ayoung"));
            return db.QuerySingleOrDefault<int>("Select ISNULL(Sum(Wh_Cost), 0) From Warehouse Where AptCode = @AptCode And St_Code = @St_Code And Wh_Year = @Wh_Year", new { AptCode, St_Code, Wh_Year });
        }
    }

}