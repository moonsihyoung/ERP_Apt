using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using System.Data;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
//using CarBlazor_Lib;
//using Microsoft.EntityFrameworkCore;

namespace Erp_Lib
{ 
    /// <summary>
    /// 자동차 등록 엔터티
    /// </summary>
    [Table("Car_Infor")]
    public class Car_Infor_entity
    {
        [Key]
        public int Aid { get; set; }
        public string Dong { get; set; }
        public string Ho { get; set; }
        public string Car_Num { get; set; }
        public string OwnerName { get; set; }
        public string Car_Name { get; set; }
        public string Mobile { get; set; }
        public DateTime ResisterDate { get; set; }
        public string Sort_Aid { get; set; }
        public string Relation { get; set; }

        public string Apt_Code { get; set; }
        public DateTime PostDate { get; set; }
        public string Move { get; set; }
        public DateTime MoveDate { get; set; }
        public int Files_Count { get; set; }
    }

    public interface ICar_Infor_Lib
    {
        Task<Car_Infor_entity> Add(Car_Infor_entity car);
        Task<Car_Infor_entity> Edit(Car_Infor_entity car);

        /// <summary>
        /// 해당 세대 차량 등록 취소
        /// </summary
        Task Remove_Ho(string Apt_Code, string Dong, string Ho);

        Task<int> being(string Car_Num, string Apt_Code);
        Task<List<Car_Infor_entity>> GetList_Ho(string Apt_Code, string Dong, string Ho);
        Task<Car_Infor_entity> Detail(string Aid);
        Task<List<Car_Infor_entity>> GetList_Car(int Page, string Apt_Code);
        Task<List<Car_Infor_entity>> GetList_Car_All(string Apt_Code);
        Task<List<Car_Infor_entity>> GetList_Car_People(string Apt_Code, string Dong, string Ho);
        Task<int> GetList_Car_Count(string Apt_Code);
        Task<List<Car_Infor_entity>> Search_Car(string Apt_Code, string Car_Num);
        Task<List<Car_Infor_entity>> Search_Car_Infor(string Apt_Code, string Field, string Query);
        Task<int> GetList_Dong_Car_Count(string Apt_Code, string Dong);

        Task<List<Car_Infor_entity>> GetList_Car_All_A(int Page, string Apt_Code);
        Task<string> Last_SortAid(string Apt_Code);
        Task FilesCountAdd(string Aid, string Division);
    }

    public class Car_Infor_Lib : ICar_Infor_Lib
    {
        private readonly IConfiguration _db;
        public Car_Infor_Lib(IConfiguration configuration)
        {
            this._db = configuration;
        }        

        /// <summary>
        /// 자동자 정보 입력
        /// </summary>
        /// <param name="car"></param>
        /// <returns></returns>
        public async Task<Car_Infor_entity> Add(Car_Infor_entity car)
        {
            var sql = "Insert Into Car_Infor (Dong, Ho, Car_Num, OwnerName, Car_Name, Mobile, ResisterDate, Move, Sort_Aid, Relation, Apt_Code) Values (@Dong, @Ho, @Car_Num, @OwnerName, @Car_Name, @Mobile, @ResisterDate, @Move, @Sort_Aid, @Relation, @Apt_Code); Select Cast(SCOPE_IDENTITY() As Int)";

            using(var _Conn = new SqlConnection(_db.GetConnectionString("bscity")))
            { 
                int Aid = await _Conn.QueryFirstOrDefaultAsync<int>(sql, car);
                car.Aid = Aid;
                return car;
            }
        }

        /// <summary>
        /// 차량 정보 수정
        /// </summary>
        /// <param name="car"></param>
        /// <returns></returns>
        public async Task<Car_Infor_entity> Edit(Car_Infor_entity car)
        {
            var sql = "Update Car_Infor Set Dong = @Dong, Ho = @Ho, Car_Num = @Car_Num, OwnerName = @OwnerName, Car_Name = @Car_Name, Mobile = @Mobile, ResisterDate = @ResisterDate, Sort_Aid = @Sort_Aid, Relation = @Relation, Move = @Move Where Aid = @Aid";
            using (var ctx = new SqlConnection(_db.GetConnectionString("bscity")))
            {
                int Aid = await ctx.QueryFirstOrDefaultAsync<int>(sql, car);
                car.Aid = Aid;
                return car;
            }
        }

        /// <summary>
        /// 차량 등록 취소
        /// </summary>
        /// <param name="car"></param>
        /// <returns></returns>
        public async Task<Car_Infor_entity> Edit_Move(Car_Infor_entity car)
        {

            using (var ctx = new SqlConnection(_db.GetConnectionString("bscity")))
            {
                await ctx.ExecuteScalarAsync("Update Car_Infor Set Move = @Move, MoveDate = @MoveDate Where Aid = @Aid", car);               
                return car;
            }
            
        }

        /// <summary>
        /// 해당 세대 차량 등록 취소
        /// </summary
        public async Task Remove_Ho(string Apt_Code, string Dong, string Ho)
        {

            using (var ctx = new SqlConnection(_db.GetConnectionString("bscity")))
            {
                string date = DateTime.Now.ToShortDateString();
                await ctx.ExecuteScalarAsync("Update Car_Infor Set Move = 'B', MoveDate = '" + date + "' Where Apt_Code = @Apt_Code And Dong = @Dong And Ho = @Ho", new {Apt_Code, Dong, Ho});     //2023-02-22            
            }

        }

        /// <summary>
        /// 해당 차량 번호 존재 확인
        /// </summary>
        /// <param name="Car_Num"></param>
        /// <param name="Apt_Code"></param>
        /// <returns></returns>
        public async Task<int> being(string Car_Num, string Apt_Code)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("bscity")))
            {
                return await ctx.QueryFirstOrDefaultAsync<int>("Select Count(*) From Car_Infor Where Apt_Code = @Apt_Code And Car_Num = @Car_Num", new { Apt_Code, Car_Num }, commandType: CommandType.Text );                
            }
            //return _Conn.ctx_e.Query<int>("Select Count(*) From Car_Infor Where Apt_Code = @Apt_Code And Car_Num = @Car_Num", new { Apt_Code, Car_Num });
        }

        /// <summary>
        /// 동호로 찾은 등록된 차량 정보 목록
        /// </summary>
        /// <param name="Apt_Code"></param>
        /// <param name="Dong"></param>
        /// <param name="Ho"></param>
        /// <returns></returns>
        public async Task<List<Car_Infor_entity>> GetList_Ho(string Apt_Code, string Dong, string Ho)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("bscity")))
            {
                var Car1 =  await ctx.QueryAsync<Car_Infor_entity>("Select Aid, Dong, Ho, Car_Num, OwnerName, Car_Name, Mobile, ResisterDate, Sort_Aid, Relation, Apt_Code, PostDate, Move, MoveDate, Move, MoveDate, Files_Count From Car_Infor Where Apt_Code = @Apt_Code And Dong = @Dong And Ho = @Ho Order By Aid Desc", new { Apt_Code, Dong, Ho }, commandType: CommandType.Text);
                return Car1.ToList();
            }
        }

        /// <summary>
        /// 차량 정보 상세보기
        /// </summary>
        /// <param name="Aid"></param>
        /// <returns></returns>
        public async Task<Car_Infor_entity> Detail(string Aid)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("bscity")))
            {
                return await ctx.QueryFirstOrDefaultAsync<Car_Infor_entity>("Select Aid, Dong, Ho, Car_Num, OwnerName, Car_Name, Mobile, ResisterDate, Sort_Aid, Relation, Apt_Code, PostDate, Move, MoveDate, Files_Count From Car_Infor Where Aid = @Aid", new { Aid }, commandType: CommandType.Text);
            }
            //return _Conn.ctx_e.Query<Car_Infor_entity>("", new { Aid });
        }

        /// <summary>
        /// 해당 공동주택의 자동차 등록 정보 목록
        /// </summary>
        /// <param name="Apt_Code"></param>
        /// <returns></returns>
        public async Task<List<Car_Infor_entity>> GetList_Car(int Page, string Apt_Code)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("bscity")))
            {
                var Car1 = await ctx.QueryAsync<Car_Infor_entity>("Select Top 15 Aid, Dong, Ho, Car_Num, OwnerName, Car_Name, Mobile, ResisterDate, Sort_Aid, Relation, Apt_Code, PostDate, Move, MoveDate, Files_Count From Car_Infor Where Aid Not In(Select Top(15 * " + @Page + ") Aid From Car_Infor Where Apt_Code = @Apt_Code Order By Aid Desc) and Apt_Code = @Apt_Code Order By Aid Desc", new { Apt_Code }, commandType: CommandType.Text);
                return Car1.ToList();
            }
            //return _Conn.ctx_e.Query<Car_Infor_entity>("Select Top 15 Aid, Dong, Ho, Car_Num, OwnerName, Car_Name, Mobile, ResisterDate, Sort_Aid, Apt_Code, PostDate, Move, MoveDate From Car_Infor Where Aid Not In(Select Top(15 * " + @Page + ") Aid From Car_Infor Where Apt_Code = @Apt_Code Order By Aid Desc) and Apt_Code = @Apt_Code Order By Aid Desc", new { Apt_Code }).ToList();
        }

        /// <summary>
        /// 해당 공동주택의 자동차 등록 정보 목록
        /// </summary>
        /// <param name="Apt_Code"></param>
        /// <returns></returns>
        public async Task<List<Car_Infor_entity>> GetList_Car_All(string Apt_Code)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("bscity")))
            {
                var Car1 = await ctx.QueryAsync<Car_Infor_entity>("Select Aid, Dong, Ho, Car_Num, OwnerName, Car_Name, Mobile, ResisterDate, Sort_Aid, Relation, Apt_Code, PostDate, Move, MoveDate, Files_Count From Car_Infor Where Apt_Code = @Apt_Code Order By Aid Desc", new { Apt_Code }, commandType: CommandType.Text);
                return Car1.ToList();
            }
            //return _Conn.ctx_e.Query<Car_Infor_entity>("Select Aid, Dong, Ho, Car_Num, OwnerName, Car_Name, Mobile, ResisterDate, Sort_Aid, Apt_Code, PostDate, Move, MoveDate From Car_Infor Where Apt_Code = @Apt_Code Order By Aid Desc", new { Apt_Code }).ToList();
        }

        /// <summary>
        /// 해당 공동주택의 자동차 등록 정보 목록
        /// </summary>
        /// <param name="Apt_Code"></param>
        /// <returns></returns>
        public async Task<List<Car_Infor_entity>> GetList_Car_All_A(int Page, string Apt_Code)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("bscity")))
            {
                var Car1 = await ctx.QueryAsync<Car_Infor_entity>("Select Top 15 Aid, Dong, Ho, Car_Num, OwnerName, Car_Name, Mobile, ResisterDate, Sort_Aid, Relation, Apt_Code, PostDate, Move, MoveDate, Files_Count From Car_Infor Where Aid Not In (Select Top (15 * @Page) Aid From Car_Infor Where Apt_Code = @Apt_Code Order By Aid Desc) And Apt_Code = @Apt_Code Order By Aid Desc", new { Page, Apt_Code });
                return Car1.ToList();
            }
            //return _Conn.ctx_e.Query<Car_Infor_entity>("Select Aid, Dong, Ho, Car_Num, OwnerName, Car_Name, Mobile, ResisterDate, Sort_Aid, Apt_Code, PostDate, Move, MoveDate From Car_Infor Where Apt_Code = @Apt_Code Order By Aid Desc", new { Apt_Code }).ToList();
        }

        /// <summary>
        /// 해당 공동주택의 자동차 등록 정보 목록
        /// </summary>
        /// <param name="Apt_Code"></param>
        /// <returns></returns>
        public async Task<List<Car_Infor_entity>> GetList_Car_People(string Apt_Code, string Dong, string Ho)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("bscity")))
            {
                var Car1 = await ctx.QueryAsync<Car_Infor_entity>("Select Aid, Dong, Ho, Car_Num, OwnerName, Car_Name, Mobile, ResisterDate, Sort_Aid, Relation, Apt_Code, PostDate, Move, MoveDate, Files_Count From Car_Infor Where Apt_Code = @Apt_Code And Dong = @Dong And Ho = @Ho And Move = 'A' Order By Aid Desc", new { Apt_Code, Dong, Ho }, commandType: CommandType.Text);
                return Car1.ToList();
            }
            //return _Conn.ctx_e.Query<Car_Infor_entity>("Select Aid, Dong, Ho, Car_Num, OwnerName, Car_Name, Mobile, ResisterDate, Sort_Aid, Apt_Code, PostDate, Move, MoveDate From Car_Infor Where Apt_Code = @Apt_Code And Dong = @Dong And Ho = @Ho And Move = 'A' Order By Aid Desc", new { Apt_Code, Dong, Ho }).ToList();
        }

        /// <summary>
        /// 등록된 차량 정보 수
        /// </summary>
        /// <param name="Apt_Code"></param>
        /// <returns></returns>
        public async Task<int> GetList_Car_Count(string Apt_Code)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("bscity")))
            {
                return await ctx.QueryFirstOrDefaultAsync<int>("Select Count(*) From Car_Infor Where Apt_Code = @Apt_Code And Move = 'A'", new { Apt_Code }, commandType: CommandType.Text);
            }
            //return _Conn.ctx_e.Query<int>("Select Count(*) From Car_Infor Where Apt_Code = @Apt_Code", new { Apt_Code });
        }


        /// <summary>
        /// 등록된 동별 차량 정보 수
        /// </summary>
        /// <param name="Apt_Code"></param>
        /// <returns></returns>
        public async Task<int> GetList_Dong_Car_Count(string Apt_Code, string Dong)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("bscity")))
            {
                return await ctx.QueryFirstOrDefaultAsync<int>("Select Count(*) From Car_Infor Where Apt_Code = @Apt_Code And Dong = @Dong", new { Apt_Code, Dong }, commandType: CommandType.Text);
            }
            //return _Conn.ctx_e.Query<int>("Select Count(*) From Car_Infor Where Apt_Code = @Apt_Code", new { Apt_Code });
        }

        /// <summary>
        /// 차량번호 뒤자로 차량 찾기
        /// </summary>
        /// <param name="Apt_Code"></param>
        /// <param name="Car_Num"></param>
        /// <returns></returns>
        public async Task<List<Car_Infor_entity>> Search_Car(string Apt_Code, string Car_Num)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("bscity")))
            {
                string sql = "Select top 10 Aid, Dong, Ho, Car_Num, OwnerName, Car_Name, Mobile, ResisterDate, Sort_Aid, Relation, Apt_Code, PostDate, Move, MoveDate, Files_Count From Car_Infor Where Apt_Code = @Apt_Code And Move = 'A' And Car_Num Like '%" + Car_Num + "%'";
                var Car1 = await ctx.QueryAsync<Car_Infor_entity>(sql, new { Apt_Code, Car_Num }, commandType: CommandType.Text);
                return Car1.ToList();
            }

           // return _Conn.ctx_e.Query<Car_Infor_entity>("Select top 10 Aid, Dong, Ho, Car_Num, OwnerName, Car_Name, Mobile From Car_Infor Where Apt_Code = @Apt_Code And Move = 'A' And Car_Num Like '%" + Car_Num + "%'", new { Apt_Code, Car_Num }).ToList();
        }

        /// <summary>
        /// 선택으로 차량 찾기
        /// </summary>
        /// <param name="Apt_Code"></param>
        /// <param name="Car_Num"></param>
        /// <returns></returns>
        public async Task<List<Car_Infor_entity>> Search_Car_Infor(string Apt_Code, string Field, string Query)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("bscity")))
            {
                var Car1 = await ctx.QueryAsync<Car_Infor_entity>("Select top 10 Aid, Dong, Ho, Car_Num, OwnerName, Car_Name, Mobile, ResisterDate, Sort_Aid, Relation, Apt_Code, PostDate, Move, MoveDate, Files_Count From Car_Infor Where Apt_Code = @Apt_Code And Move = 'A' And " + Field + " Like '%" + Query + "%' Order By Aid Desc", new { Apt_Code, Field, Query }, commandType: CommandType.Text);
                return Car1.ToList();
            }
            //return _Conn.ctx_e.Query<Car_Infor_entity>("Select top 10 Aid, Dong, Ho, Car_Num, OwnerName, Car_Name, Mobile From Car_Infor Where Apt_Code = @Apt_Code And Move = 'A' And " + Field + " Like '%" + Query + "%'", new { Apt_Code, Field, Query }).ToList();
        }

        /// <summary>
        /// 마지막 등록 번호
        /// </summary>
        /// <param name="Apt_Code"></param>
        /// <returns></returns>
        public async Task<string> Last_SortAid(string Apt_Code)
        {
            using (var df = new SqlConnection(_db.GetConnectionString("bscity")))
            {
                return await df.QuerySingleOrDefaultAsync<string>("SELECT TOP 1 Sort_Aid FROM Car_Infor Where Apt_Code = @Apt_Code Order By Aid Desc", new { Apt_Code });
            }
        }

        /// <summary>
        /// 첨부 파일 더하기
        /// </summary>
        /// <param name="Apt_Code"></param>
        /// <returns></returns>
        public async Task FilesCountAdd(string Aid, string Division)
        {
            using (var df = new SqlConnection(_db.GetConnectionString("bscity")))
            {
                if(Division == "A")
                {
                    await df.ExecuteAsync("Update Car_Infor Set Files_Count = Files_Count + 1 Where Aid = @Aid", new { Aid });
                }
                else
                {
                    await df.ExecuteAsync("Update Car_Infor Set Files_Count = Files_Count - 1 Where Aid = @Aid", new { Aid });
                }
            }
        }
    }
}
