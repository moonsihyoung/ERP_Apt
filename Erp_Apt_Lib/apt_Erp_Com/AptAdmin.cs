using Dapper;
using Erp_Lib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace Erp_Apt_Lib
{
    
    public class Dong_Lib
    {
        private readonly IConfiguration _db;
        public Dong_Lib(IConfiguration configuration)
        {
            this._db = configuration;
        }

        /// <summary>
        /// 동 정보 입력
        /// </summary>
        /// <param name="dtt"></param>
        public async Task Add(Dong_Entity dtt)
        {
            using (var df = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                await df.ExecuteAsync("Insert Into Dong (Dong_Code, Apt_Code, Dong_Name, Family_Num, Length, Width, Floor_Num, Exit_Num, Elevator_Num, Line_Num, Hall_Form, Roof_Form, Dong_Area, Dong_Etc, PostIP) Values (@Dong_Code, @Apt_Code, @Dong_Name, @Family_Num, @Length, @Width, @Floor_Num, @Exit_Num, @Elevator_Num, @Line_Num, @Hall_Form, @Roof_Form, @Dong_Area, @Dong_Etc, @PostIP)", dtt);
            }            
        }

        /// <summary>
        /// 동 정보 수정
        /// </summary>
        /// <param name="dtt"></param>
        public async Task<Dong_Entity> Edit_Dong(Dong_Entity Dong)
        {
            var sql = "Update Dong Set Dong_Name = @Dong_Name, Family_Num = @Family_Num, Length = @Length, Width = @Width, Floor_Num = @Floor_Num, Exit_Num = @Exit_Num, Elevator_Num = @Elevator_Num, Line_Num = @Line_Num, Hall_Form = @Hall_Form, Roof_Form = @Roof_Form, Dong_Area = @Dong_Area, Dong_Etc = @Dong_Etc, PostIP = @PostIP Where AId = @AId";
            using (var df = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                await df.ExecuteAsync(sql, Dong);
                return Dong;
            }            
        }

        /// <summary>
        /// 마지막 일련번호
        /// </summary>
        public async Task<int> Last_Number()
        {
            using (var df = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                return await df.QuerySingleOrDefaultAsync<int>("Select Top 1 Aid From Dong Order by Aid Desc", new { });
            }            
        }

        /// <summary>
        /// 삭제
        /// </summary>
        /// <param name="AId"></param>
        public async Task Remeove_Dong(int AId)
        {
            using (var df = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                await df.ExecuteAsync("Delete From Dong Where AId = @AId", new { AId });
            }            
        }

        /// <summary>
        ///  동 정보 리스트 구현
        /// </summary>
        public async Task<List<Dong_Entity>> GetList_Dong(string Apt_Code)
        {
            using (var df = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                var lst = await df.QueryAsync<Dong_Entity>("Select Aid, Dong_Code, Apt_Code, Dong_Name, Family_Num, Length, Width, Floor_Num, Exit_Num, Elevator_Num, Line_Num, Hall_Form, Roof_Form, Dong_Area, Dong_Etc, PostDate, PostIP From Dong Where Apt_Code = @Apt_Code Order By Dong_Name Asc", new { Apt_Code });
                return lst.ToList();
            }            
        }

        /// <summary>
        /// 동 정보 리스트 검색된 수 구현
        /// </summary>
        /// <param name="Apt_Code"></param>
        /// <returns></returns>
        public async Task<int> GetList_Dong_Count(string Apt_Code)
        {
            using (var df = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                return await df.QuerySingleOrDefaultAsync<int>("Select Count(*) From Dong Where Apt_Code = @Apt_Code", new { Apt_Code });
            }            
        }

        /// <summary>
        /// 동 상세 정보
        /// </summary>
        /// <param name="Dong_Code"></param>
        /// <returns></returns>
        public async Task<Dong_Entity> Detail_Dong(string Dong_Code)
        {
            using (var df = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                return await df.QuerySingleOrDefaultAsync<Dong_Entity>("Select Aid, Dong_Code, Apt_Code, Dong_Name, Family_Num, Length, Width, Floor_Num, Exit_Num, Elevator_Num, Line_Num, Hall_Form, Roof_Form, Dong_Area, Dong_Etc, PostDate, PostIP From Dong Where Dong_Code = @Dong_Code", new { Dong_Code });
            }
            

        }

        /// <summary>
        /// 동이름 중복 여부
        /// </summary>
        /// <param name="Apt_Code"></param>
        /// <param name="DongName"></param>
        /// <returns></returns>
        public async Task<int> Being(string Apt_Code, string DongName)
        {
            using (var df = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                return await df.QuerySingleOrDefaultAsync<int>("Select Count(*) From Dong Where Apt_Code = @Apt_Code And Dong_Name = @DongName", new { Apt_Code, DongName });
            }
            
        }
    }
    public class Dong_Composition_Lib
    {
        private readonly IConfiguration _db;
        public Dong_Composition_Lib(IConfiguration configuration)
        {
            this._db = configuration;
        }

        // 동 구성 정보 입력
        public async Task<Dong_Composition_Entity> Add_Dong_Composition(Dong_Composition_Entity Dong)
        {
            var sql = "Insert Dong_Composition (Dong_Composition_Code, Apt_Code, Dong_Code, Supply_Area, Area_Family_Num, Only_Area, Total_Area, Dong_Etc, PostIP) Values (@Dong_Composition_Code, @Apt_Code, @Dong_Code, @Supply_Area, @Area_Family_Num, @Only_Area, @Total_Area, @Dong_Etc, @PostIP);";
            using (var df = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                await df.ExecuteAsync(sql, Dong);
                return Dong;
            }
            
        }

        // 동 구성 정보 수정
        public async Task<Dong_Composition_Entity> Edit_Dong_Composition(Dong_Composition_Entity Dong)
        {
            var sql = "Update Dong_Composition Set Dong_Code = @Dong_Code, Supply_Area = @Supply_Area, Area_Family_Num = @Area_Family_Num, Only_Area = @Only_Area, Total_Area = @Total_Area, Dong_Etc = @Dong_Etc Where Aid = @Aid";
            using (var df = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                await df.ExecuteAsync(sql, Dong);
                return Dong;
            }
            
        }

        // 동 구성 정보 삭제
        public async Task Remeove_Dong_Composition(int AId)
        {
            using (var df = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                await df.ExecuteAsync("Delete From Dong_Composition Where AId = @AId", new { AId });
            }
            
        }

        // 마지막 일련번호 얻기
        public async Task<int> Last_Number()
        {
            using (var df = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                return await df.QuerySingleOrDefaultAsync<int>("Select Top 1 Aid From Dong_Composition Order by Aid Desc", new { });
            }
            
        }

        // 중복 체크
        public async Task<int> Overlap_Check(int AId)
        {
            using (var df = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                return await df.QuerySingleOrDefaultAsync<int>("Select Count(*) From Dong_Composition Where AId = @AId", new { AId });
            }
            
        }

        // 공급면적 합계
        public async Task<double> Total_Supply_Account(string Apt_Code)
        {
            using (var df = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                return await df.QuerySingleOrDefaultAsync<float>("Select isnull(Sum(Supply_Area), 0) From Dong_Composition Where Apt_Code = @Apt_Code", new { Apt_Code });
            }
            
        }

        // 세대수  합계
        public async Task<int> Total_Family_Account(string Apt_Code)
        {
            using (var df = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                return await df.QuerySingleOrDefaultAsync<int>("Select isnull(Sum(Area_Family_Num), 0) From Dong_Composition Where Apt_Code = @Apt_Code", new { Apt_Code });
            }
            
        }

        // 관리면적 합계
        public async Task<double> Total_Area_Account(string Apt_Code)
        {
            using (var df = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                return await df.QuerySingleOrDefaultAsync<double>("Select isnull(Sum(Total_Area), 0) From Dong_Composition Where Apt_Code = @Apt_Code", new { Apt_Code });
            }
            
        }


        // 동구성 정보 공동주택코드로 리스트 불러오기
        public async Task<List<Dong_Composition_Entity>> GetList_Dong_Composition(string Apt_Code)
        {
            using (var df = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                var lst = await df.QueryAsync<Dong_Composition_Entity>("Select * From Dong_Composition Where Apt_Code = @Apt_Code Order By Supply_Area Asc", new { Apt_Code });
                return lst.ToList();
            }
            
        }

        // 동 구성 정보 코드명으로 해당 상세정보 불러오기
        public async Task<Dong_Composition_Entity> Detail_Sort(string Dong_Composition_Code)
        {
            using (var df = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                return await df.QuerySingleOrDefaultAsync<Dong_Composition_Entity>("Select * From Dong_Composition Where Dong_Composition_Code = @Dong_Composition_Code", new { Dong_Composition_Code });
            }
            
        }
    }
}
