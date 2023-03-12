using Erp_Lib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using System.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;
using Facilities;

namespace Works
{
    /// <summary>
    /// 작업일지 클래스
    /// </summary>
    public class Works_Lib : IWorks_Lib
    {
        private readonly IConfiguration _db;
        public Works_Lib(IConfiguration configuration)
        {
            this._db = configuration;
        }

        // <summary>
        /// 외부작업 여부 입력
        /// </summary>
        public async Task Service_WriteOutViw(Works_Entity dnn)
        {
            using SqlConnection dba = new SqlConnection(_db.GetConnectionString("Ayoung"));
            await dba.ExecuteAsync("WriteServiceOutViw", dnn, commandType: CommandType.StoredProcedure);
            //_Conn.ctx_a.Execute("WriteServiceOutViw", dnn, commandType: CommandType.StoredProcedure);
        }

        /// <summary>
        /// 단지 시설물 대분류 드롭다운리스트
        /// </summary>
        public async Task<List<Bloom_Entity>> List_Bloom_Drop()
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                var lst = await dba.QueryAsync<Bloom_Entity>("ListBloomDrop", commandType: CommandType.StoredProcedure);
                return lst.ToList();
            }
        }

        /// <summary>
        /// 단지 시설물 중분류 드롭다운리스트
        /// </summary>
        public async Task<List<Bloom_Entity>> List_Bloom_DropB(string B_N_B_Name)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                var lst = await dba.QueryAsync<Bloom_Entity>("ListBloomDropB", new { B_N_B_Name }, commandType: CommandType.StoredProcedure);
                return lst.ToList();
            }
        }

        /// <summary>
        /// 단지 시설물 세분류 드롭다운리스트
        /// </summary>
        public async Task<List<Bloom_Entity>> List_Bloom_DropC(string B_N_A_Name, string B_N_B_Name)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                var lst = await dba.QueryAsync<Bloom_Entity>("ListBloomDropC", new { B_N_A_Name, B_N_B_Name }, commandType: CommandType.StoredProcedure);
                    return lst.ToList();
            }            
        }

        /// <summary>
        /// 단지 시설물 폼목 입력하기 위한 마지막 번호 찾기(각 사업장별 특성이나 시설물 입력)
        /// </summary>
        public async Task<int> List_Bloom_D()
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                return await dba.QuerySingleOrDefaultAsync<int>("ListBloomD", commandType: CommandType.StoredProcedure);
            }
            
        }

        /// <summary>
        /// 단지 시설물 품목분류 드롭다운리스트
        /// </summary>
        public async Task<List<Bloom_Entity>> List_Bloom_DropD(string B_N_A_Name, string AptCode)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                var lst = await dba.QueryAsync<Bloom_Entity>("ListBloomDrop_D", new { B_N_A_Name, AptCode }, commandType: CommandType.StoredProcedure);
                return lst.ToList();
            }            
        }

        /// <summary>
        /// 단지 시설물 품목분류 드롭다운리스트
        /// </summary>
        public async Task<List<Bloom_Entity>> List_Bloom_Drop_Place(string B_N_A_Name, string AptCode)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                var lst = await dba.QueryAsync<Bloom_Entity>("Select Bloom, B_N_Code From Bloom Where Bloom_Code = 'D' And B_N_A_Name And AptCode = @AptCode", new { B_N_A_Name, AptCode });
                return lst.ToList();
            }
            
        }

        /// <summary>
        /// 단지 시설물 소분류 리스트로 검색(업무일지)
        /// </summary>
        public async Task<List<Works_Entity>> ListA(int Page, string AptCode, string B_N_A_Name, string B_N_B_Name, string B_N_C_Name)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                var lst = await dba.QueryAsync<Works_Entity>("ListServiceB", new { Page, AptCode, B_N_A_Name, B_N_B_Name, B_N_C_Name }, commandType: CommandType.StoredProcedure);
                return lst.ToList();
            }
            
        }

        /// <summary>
        /// 단지 시설물 리스트로 작업일지 대분류 검색
        /// </summary>
        public async Task<List<Works_Entity>> ListWorkA(int Page, string AptCode, string B_N_A_Name)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                var lst = await dba.QueryAsync<Works_Entity>("Select Top 15 * From ServiceA Where Num Not In (Select Top (15 * @Page) Num From ServiceA Where AptCode = @AptCode And svBloomA = @B_N_A_Name Order By Num Desc) And AptCode = @AptCode And svBloomA = @B_N_A_Name Order By Num Desc", new { Page, AptCode, B_N_A_Name });
                return lst.ToList();
            }
        }

        /// <summary>
        /// 단지 시설물 리스트로 작업일지 대분류 검색 수
        /// </summary>
        public async Task<int> ListWorkA_Count(string AptCode, string B_N_A_Name)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                return await dba.QuerySingleOrDefaultAsync<int>("Select Count(*) From ServiceA Where AptCode = @AptCode And svBloomA = @B_N_A_Name", new { AptCode, B_N_A_Name });
            }
        }

        /// <summary>
        /// 단지 시설물 리스트로 작업일지 중분류 검색
        /// </summary>
        public async Task<List<Works_Entity>> ListWorkB(int Page, string AptCode, string B_N_A_Name, string B_N_B_Name)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                var lst = await dba.QueryAsync<Works_Entity>("Select Top 15 * From ServiceA Where Num Not In (Select Top (15 * @Page) Num From ServiceA Where AptCode = @AptCode And svBloomA = @B_N_A_Name And svBloomB = @B_N_B_Name Order By Num Desc) And AptCode = @AptCode And svBloomA = @B_N_A_Name And svBloomB = @B_N_B_Name Order By Num Desc", new { Page, AptCode, B_N_A_Name, B_N_B_Name });
                return lst.ToList();
            }
        }

        /// <summary>
        /// 단지 시설물 리스트로 작업일지 중분류 검색 수
        /// </summary>
        public async Task<int> ListWorkB_Count(string AptCode, string B_N_A_Name, string B_N_B_Name)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                return await dba.QuerySingleOrDefaultAsync<int>("Select Count(*) From ServiceA Where AptCode = @AptCode And svBloomA = @B_N_A_Name And svBloomB = @B_N_B_Name", new { AptCode, B_N_A_Name, B_N_B_Name });
            }
        }

        /// <summary>
        /// 단지 시설물 리스트로 작업일지 소분류 검색
        /// </summary>
        public async Task<List<Works_Entity>> ListWorkC(int Page, string AptCode, string B_N_A_Name, string B_N_B_Name, string B_N_C_Name)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                var lst = await dba.QueryAsync<Works_Entity>("Select Top 15 * From ServiceA Where Num Not In (Select Top (15 * @Page) Num From ServiceA Where AptCode = @AptCode And svBloomA = @B_N_A_Name And svBloomB = @B_N_B_Name And svBloomC = @B_N_C_Name Order By Num Desc) And AptCode = @AptCode And svBloomA = @B_N_A_Name And svBloomB = @B_N_B_Name And svBloomC = @B_N_C_Name Order By Num Desc", new { Page, AptCode, B_N_A_Name, B_N_B_Name, B_N_C_Name });
                return lst.ToList();
            }
        }

        /// <summary>
        /// 단지 시설물 리스트로 작업일지 소분류 검색 수
        /// </summary>
        public async Task<int> ListWorkC_Count(string AptCode, string B_N_A_Name, string B_N_B_Name, string B_N_C_Name)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                return await dba.QuerySingleOrDefaultAsync<int>("Select Count(*) From ServiceA Where AptCode = @AptCode And svBloomA = @B_N_A_Name And svBloomB = @B_N_B_Name And svBloomC = @B_N_C_Name", new { AptCode, B_N_A_Name, B_N_B_Name, B_N_C_Name });
            }
        }

        /// <summary>
        /// 작업일지 소분류 리스트로 검색(업무일지) 2019년 9월
        /// </summary>
        public async Task<List<Works_Entity>> BoomSearchListC(int Page, string AptCode, string svBloomA, string svBloomB, string svBloomC)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                var lst = await dba.QueryAsync<Works_Entity>("Select Top 15 * From ServiceA Where Num Not In (Select Top (15 * @Page) Num From ServiceA Where AptCode = @AptCode And svBloomA = @svBloomA And svBloomB = @svBloomB And svBloomC = @svBloomC Order By Cast(svYear As Int) Desc, Cast(svMonth As Int) Desc, Cast(svDay As Int) Desc, Num Desc) And AptCode = @AptCode And svBloomA = @svBloomA And svBloomB = @svBloomB And svBloomC = @svBloomC Order By Cast(svYear As Int) Desc, Cast(svMonth As Int) Desc, Cast(svDay As Int) Desc, Num Desc", new { Page, AptCode, svBloomA, svBloomB, svBloomC });
                return lst.ToList();
            }
            
        }

        /// <summary>
        /// 작업일지 소분류 리스트로 검색(업무일지) 2019년 9월
        /// </summary>
        public async Task<List<Works_Entity>> FacilityC(string AptCode, string svBloomA, string svBloomB, string svBloomC)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                var lst = await dba.QueryAsync<Works_Entity>("Select Top 30 * From ServiceA Where AptCode = @AptCode And svBloomA = @svBloomA And svBloomB = @svBloomB And svBloomC = @svBloomC Order By Num Desc", new { AptCode, svBloomA, svBloomB, svBloomC });
                return lst.ToList();
            }
            
        }

        /// <summary>
        ///작업일지 소분류 리스트로 검색(업무일지) 2019년 9월
        /// </summary>
        public async Task<int> BoomSearchList_CountC(string AptCode, string svBloomA, string svBloomB, string svBloomC)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                return await dba.QuerySingleOrDefaultAsync<int>("Select Count(*) From ServiceA Where AptCode = @AptCode And svBloomA = @svBloomA And svBloomB = @svBloomB And svBloomC = @svBloomC", new { AptCode, svBloomA, svBloomB, svBloomC });
            }
            
        }


        /// <summary>
        /// 작업일지 중분류 리스트로 검색(업무일지) 2019년 9월
        /// </summary>
        public async Task<List<Works_Entity>> BoomSearchListB(int Page, string AptCode, string svBloomA, string svBloomB)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                var lst = await dba.QueryAsync<Works_Entity>("Select Top 15 * From ServiceA Where Num Not In (Select Top (15 * @Page) Num From ServiceA Where AptCode = @AptCode And svBloomA = @svBloomA And svBloomB = @svBloomB Order By Cast(svYear As Int) Desc, Cast(svMonth As Int) Desc, Cast(svDay As Int) Desc, Num Desc) And AptCode = @AptCode And svBloomA = @svBloomA And svBloomB = @svBloomB Order By Cast(svYear As Int) Desc, Cast(svMonth As Int) Desc, Cast(svDay As Int) Desc, Num Desc", new { Page, AptCode, svBloomA, svBloomB });
                return lst.ToList();
            }
            
        }

        /// <summary>
        /// 작업일지 중분류 리스트로 검색(업무일지) 2019년 9월
        /// </summary>
        public async Task<int> BoomSearchList_CountB(string AptCode, string svBloomA, string svBloomB)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                return await dba.QuerySingleOrDefaultAsync<int>("Select Count(*) From ServiceA Where AptCode = @AptCode And svBloomA = @svBloomA And svBloomB = @svBloomB", new { AptCode, svBloomA, svBloomB });
            }            
        }

        /// <summary>
        /// 작업일지 대분류 리스트로 검색(업무일지) 2019년 9월
        /// </summary>
        public async Task<List<Works_Entity>> BoomSearchListA(int Page, string AptCode, string svBloomA)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                var lst = await dba.QueryAsync<Works_Entity>("Select Top 15 * From ServiceA Where Num Not In (Select Top (15 * @Page) Num From ServiceA Where AptCode = @AptCode And svBloomA = @svBloomA Order By Cast(svYear As Int) Desc, Cast(svMonth As Int) Desc, Cast(svDay As Int) Desc, Num Desc) And AptCode = @AptCode And svBloomA = @svBloomA Order By Cast(svYear As Int) Desc, Cast(svMonth As Int) Desc, Cast(svDay As Int) Desc, Num Desc", new { Page, AptCode, svBloomA });
                return lst.ToList();
            }
            
        }

        /// <summary>
        /// 작업일지 대분류 리스트로 검색(업무일지) 2019년 9월
        /// </summary>
        public async Task<int> BoomSearchList_CountA(string AptCode, string svBloomA)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                return await dba.QuerySingleOrDefaultAsync<int>("Select Count(*) From ServiceA Where AptCode = @AptCode And svBloomA = @svBloomA", new { AptCode, svBloomA });
            }
            
        }


        /// <summary>
        /// 작업일지 날짜 간격 리스트로 검색(업무일지) 2019년 9월
        /// </summary>
        public async Task<List<Works_Entity>> DateSearchListA(int Page, string AptCode, string StartDate, string EndDate)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                var lst = await dba.QueryAsync<Works_Entity>("Select Top 15 * From ServiceA Where Num Not In (Select Top (15 * @Page) Num From  ServiceA Where AptCode = @AptCode And PostDate Between @StartDate And @EndDate Order By Cast(svYear As Int) Desc, Cast(svMonth As Int) Desc, Cast(svDay As Int) Desc, Num Desc) And AptCode = @AptCode And PostDate Between @StartDate And @EndDate Order By Cast(svYear As Int) Desc, Cast(svMonth As Int) Desc, Cast(svDay As Int) Desc, Num Desc", new { Page, AptCode, StartDate, EndDate });
                return lst.ToList();
            }
            
        }

        /// <summary>
        /// 작업일지 날짜 간력 리스트로 검색(업무일지) 2019년 9월
        /// </summary>
        public async Task<int> DateSearchList_CountA(string AptCode, string StartDate, string EndDate)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                return await dba.QuerySingleOrDefaultAsync<int>("Select Count(*) From ServiceA Where AptCode = @AptCode And PostDate Between @StartDate And @EndDate", new { AptCode, StartDate, EndDate });
            }
            
        }


        /// <summary>
        /// 작업일지 키워드 리스트로 검색(업무일지) 2019년 9월
        /// </summary>
        public async Task<List<Works_Entity>> WordSearchListA(int Page, string AptCode, string Word)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                var lst = await dba.QueryAsync<Works_Entity>("Select top 15 * From ServiceA Where Num Not In (Select Top (15 * @Page) Num From ServiceA Where AptCode = @AptCode And ((svContent Like '%" + @Word + "%') or (svReceiver Like '%" + @Word + "%') or (svBloom Like '%" + @Word + "%')) Order By Cast(svYear As Int) Desc, Cast(svMonth As Int) Desc, Cast(svDay As Int) Desc, Num Desc) And AptCode = @AptCode And ((svContent Like '%" + @Word + "%') or (svReceiver Like '%" + @Word + "%') or (svBloom Like '%" + @Word + "%')) Order By Cast(svYear As Int) Desc, Cast(svMonth As Int) Desc, Cast(svDay As Int) Desc, Num Desc", new { Page, AptCode, Word });
                return lst.ToList();
            }
           
        }

        /// <summary>
        /// 작업일지 키워드 리스트로 검색(업무일지) 2019년 9월
        /// </summary>
        public async Task<int> WordSearchList_CountA(string AptCode, string Word)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                return await dba.QuerySingleOrDefaultAsync<int>("Select Count(*) From ServiceA Where AptCode = @AptCode And ((svContent Like '%" + Word + "%') or (svReceiver Like '%" + Word + "%') or (svBloom Like '%" + Word + "%'))", new { AptCode, Word });
            }
            
        }


        /// <summary>
        /// 단지 시설물 보관장소 드롭 리스트
        /// </summary>
        public async Task<List<Bloom_Entity>> Wh_BloomD_List(string AptCode)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                string BloomD = "자재입고";
                string Bloom_Code = "D";
                var lst = await dba.QueryAsync<Bloom_Entity>("Select distinct Bloom From Bloom Where Bloom_Code = '" + Bloom_Code + "' and AptCode = @AptCode And B_N_C_Name = '" + BloomD + "'", new { AptCode });
                return lst.ToList();
            }
            
        }

        /// <summary>
        /// 단지 업무일지 소분류 리스트로 검색(업무일지) 수 가져오기
        /// </summary>
        //public int GetCountServiceListA(string AptCode, string B_N_A_Name, string B_N_B_Name, string B_N_C_Name)
        //{
        //    return _Conn.ctx_a.Query<int>("Select Count(*) From ServiceA Where ComAlias =@AptCode And svBloomA = @B_N_A_Name And svBloomB = @B_N_B_Name And svBloomC = @B_N_C_Name", new{);
        //}

        /// <summary>
        /// 단지 업무일지 날짜로 검색 리스트   
        /// </summary>
        public async Task<List<Works_Entity>> ListB(int Page, string AptCode, string svYear, string svMonth, string svDay)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                var lst = await dba.QueryAsync<Works_Entity>("ListServiceDate", new { Page, AptCode, svYear, svMonth, svDay }, commandType: CommandType.StoredProcedure);
                return lst.ToList();
            }
            
        }

        /// <summary>
        /// 단지 시설물관리 날짜로 리스트로 검색(업무일지) 수 가져오기
        /// </summary>
        public async Task<int> GetCountServiceListB(string AptCode, string Year, string Month, string Day)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                return await dba.QuerySingleOrDefaultAsync<int>("Select Count(*) From ServiceA Where ComAlias = @AptCode And svYear = @Year And svMonth = @Month And svDay = @Day", new { AptCode, Year, Month, Day });
            }
            
        }

        /// <summary>
        /// 단지 업무일지 월로 검색 리스트   
        /// </summary>
        public async Task<List<Works_Entity>> ListB_A(int Page, string AptCode, string svYear, string svMonth)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                var lst = await dba.QueryAsync<Works_Entity>("ListServiceDate_A", new { Page, AptCode, svYear, svMonth }, commandType: CommandType.StoredProcedure);
                return lst.ToList();
            }
            
        }

        /// <summary>
        /// 외부 작업 리스트에 디스플레이된 외부작업 코드로 검색된 업무일지 리스트   
        /// </summary>
        public async Task<List<Works_Entity>> Service_ScwCode_Data_List(string AptCode, string Scw_Code_da)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                var lst = await dba.QueryAsync<Works_Entity>("Select * From ServiceA Where ComAlias =@AptCode And Scw_Code = @Scw_Code_da", new { AptCode, Scw_Code_da });
                return lst.ToList();
            }
            
        }

        /// <summary>
        /// 단지 시설물관리 날짜로 리스트로 검색(업무일지) 수 가져오기
        /// </summary>
        public async Task<int> GetCountServiceListB_A(string AptCode, string Year, string Month)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                return await dba.QuerySingleOrDefaultAsync<int>("Select Count(*) From ServiceA Where ComAlias =@AptCode And svYear = @Year And svMonth = @Month", new { AptCode, Year, Month });
            }
            
        }

        /// <summary>
        /// 단지별 업무일지 리스트
        /// </summary>
        public async Task<List<Works_Entity>> Service_List(int Page, string AptCode)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                //var lst = await dba.QueryAsync<Works_Entity>("Select Num, AptCode, svYear, svMonth, svDay, svClock, svMinute, svDirect, svBloomCode, svBloomA, svBloomB, svBloomC, svBloom, svPost, svReceiver, svContent, PostDate, PostIP, ComFileSize5, UserIDW, svMoney, svCost, svAmount, svWorkDaily, subYear, subMonth, subDay, subClock, subMinute, svOutViw, Scw_Code, svOutName, svOutTelCom, svOutNameCom, innViw, svWorkerName, svWorkerCount, svWorkPost, WorkContent, apSatisfaction, ModifyDate, ModifyIP, ComFileName6, ComFileSize6, ComFileName7, ComFileSize7, ComFileName8, ComFileSize8, ComFileName9, ComFileSize9, ComFileName10, ComFileSize10, ComCommentCount, ComAlias, Apt_Name, UserIDM, Complete, Conform, Privater, Snp_Number From ServiceA Where AptCode = @AptCode Order By Cast(svYear As Int) Desc, Cast(svMonth As Int) Desc, Cast(svDay As Int) Desc, Num Desc", new { AptCode });
                var lst = await dba.QueryAsync<Works_Entity>("ListServiceR", new { Page, AptCode }, commandType: CommandType.StoredProcedure);
                return lst.ToList();
            }
            //return _Conn.ctx_a.Query<Works_Report_Entity>("ListServiceR", new { Page, AptCode }, commandType: CommandType.StoredProcedure).ToList();
            
        }

        /// <summary>
        /// 단지별 업무일지 미완료 리스트
        /// </summary>
        public async Task<List<Works_Entity>> ServiceListComplete(string AptCode)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                var lst = await dba.QueryAsync<Works_Entity>("Select Num, AptCode, svYear, svMonth, svDay, svClock, svMinute, svDirect, svBloomCode, svBloomA, svBloomB, svBloomC, svBloom, svPost, svReceiver, svContent, PostDate, PostIP, ComFileSize5, UserIDW, svMoney, svCost, svAmount, svWorkDaily, subYear, subMonth, subDay, subClock, subMinute, svOutViw, Scw_Code, svOutName, svOutTelCom, svOutNameCom, innViw, svWorkerName, svWorkerCount, svWorkPost, WorkContent, apSatisfaction, ModifyDate, ModifyIP, ComFileName6, ComFileSize6, ComFileName7, ComFileSize7, ComFileName8, ComFileSize8, ComFileName9, ComFileSize9, ComFileName10, ComFileSize10, ComCommentCount, ComAlias, Apt_Name, UserIDM, Complete, Conform, Privater, Snp_Number From ServiceA Where AptCode = @AptCode And Complete != 'B' Order By Cast(svYear As Int) Desc, Cast(svMonth As Int) Desc, Cast(svDay As Int) Desc, Num Desc", new { AptCode });
                return lst.ToList();
            }
            //return _Conn.ctx_a.Query<Works_Report_Entity>("ListServiceR", new { Page, AptCode }, commandType: CommandType.StoredProcedure).ToList();
            
        }

        /// <summary>
        /// 단지별 업무일지 리스트 목록 수
        /// </summary>
        /// <param name="AptCode"></param>
        /// <returns></returns>
        public async Task<int> Service_List_Count(string AptCode)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                return await dba.QuerySingleOrDefaultAsync<int>("Select Count(*) From ServiceA Where AptCode = @AptCode", new { AptCode });
            }
            //return _Conn.ctx_a.Query<Works_Report_Entity>("ListServiceR", new { Page, AptCode }, commandType: CommandType.StoredProcedure).ToList();
            
        }

        /// <summary>
        /// 단지별 업무일지 미완료 리스트 목록 수
        /// </summary>
        /// <param name="AptCode"></param>
        /// <returns></returns>
        public async Task<int> ServiceListCompleteCount(string AptCode)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                return await dba.QuerySingleOrDefaultAsync<int>("Select Count(*) From ServiceA Where AptCode = @AptCode And Complete != 'B'", new { AptCode });
            }
            //return _Conn.ctx_a.Query<Works_Report_Entity>("ListServiceR", new { Page, AptCode }, commandType: CommandType.StoredProcedure).ToList();
            
        }

        /// <summary>
        /// 업무일지 리스트 수 가져오기
        /// </summary>
        public async Task<int> GetCountServiceT(string AptCode)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                return await dba.QuerySingleOrDefaultAsync<int>("GetCountServiceT", new { AptCode }, commandType: CommandType.StoredProcedure);
            }
            
        }

        /// <summary>
        /// 업무일지 리스트 수 가져오기
        /// </summary>
        public async Task<int> GetCountService(string AptCode)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                return await dba.QuerySingleOrDefaultAsync<int>("ServiceGetCount", new { AptCode }, commandType: CommandType.StoredProcedure);
            }
            
        }

        /// <summary>
        /// 단지 업무일지 중분류로 검색 수 가져오기
        /// </summary>
        public async Task<int> GetCountServiceListC(string AptCode, string B_N_A_Name, string B_N_B_Name)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                return await dba.QuerySingleOrDefaultAsync<int>("Select Count(*) From ServiceA Where ComAlias = @AptCode And svBloomA = @B_N_A_Name And svBloomB = @B_N_B_Name", new { AptCode, B_N_A_Name, B_N_B_Name });
            }
            
        }

        /// <summary>
        /// 단지 업무일지 중분류 검색 리스트
        /// </summary>
        public async Task<List<Works_Entity>> ListC(int Page, string AptCode, string B_N_A_Name, string B_N_B_Name)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                var lst = await dba.QueryAsync<Works_Entity>("ListServiceC", new { Page, AptCode, B_N_A_Name, B_N_B_Name }, commandType: CommandType.StoredProcedure);
                return lst.ToList();
            }
            
        }

        /// <summary>
        /// 단지 업무일지 대분류로 검색 수 가져오기
        /// </summary>
        public async Task<int> GetCountServiceListC_A(string AptCode, string B_N_A_Name)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                return await dba.QuerySingleOrDefaultAsync<int>("Select Count(*) From ServiceA Where ComAlias =@AptCode And svBloomA = @B_N_A_Name", new { AptCode, B_N_A_Name });
            }
            
        }

        /// <summary>
        /// 단지 업무일지 대분류 검색 리스트
        /// </summary>
        public async Task<List<Works_Entity>> ListC_A(int Page, string AptCode, string B_N_A_Name)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                var lst = await dba.QueryAsync<Works_Entity>("ListServiceC_A", new { Page, AptCode, B_N_A_Name }, commandType: CommandType.StoredProcedure);
                return lst.ToList();
            }
            
        }

        /// <summary>
        /// 업무일지 최종 번호 가져오기
        /// <param name="AptCode">사업장 코드</param>
        /// </summary>
        public async Task<int> Service_Last_Num_Data(string AptCode)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                return await dba.QuerySingleOrDefaultAsync<int>("Select Top 1 Num  From ServiceA Where ComAlias = @AptCode Order By Num Desc", new { AptCode });
            }
            
        }

        /// <summary>
        /// 업무일지  입력
        /// </summary>
        public async Task Service_Write(Works_Entity Ct)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                await dba.ExecuteAsync("Insert ServiceA (AptCode, svYear, svMonth, svDay, svClock, svMinute, svDirect, svBloomCode, svBloomA, svBloomB, svBloomC, svBloom, svPost, svReceiver, svContent, PostIP, ComAlias, Apt_Name, UserIDW) Values (@AptCode, @svYear, @svMonth, @svDay, @svClock, @svMinute, @svDirect, @svBloomCode, @svBloomA, @svBloomB, @svBloomC, @svBloom, @svPost, @svReceiver, @svContent, @PostIP, @ComAlias, @Apt_Name, @UserIDW)", Ct);
            }
            
        }

        /// <summary>
        /// 입무일지 지시내용 수정
        /// </summary>
        /// <param name="ct"></param>
        public async Task UpdateWorksEdit(Works_Entity ct)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                await dba.ExecuteAsync("Update ServiceA Set SvBloomA = @svBloomA, svBloomB = @svBloomB, svBloomC = @svBloomC, svBloom = @svBloom, svPost = @svPost, svReceiver = @svReceiver, svContent = @svContent, ModifyDate = @ModifyDate, ModifyIP = @ModifyIP Where Num = @Num", ct);
            }            
        }


        /// <summary>
        /// 품목  입력
        /// </summary>
        public async Task Bloom_D_Write(Bloom_Entity As)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                await dba.ExecuteAsync("WriteBloom", As, commandType: CommandType.StoredProcedure);
            }
            
        }

        /// <summary>
        /// 업무일지(번호) 상세보기
        /// </summary>
        public async Task<Works_Entity> Service_View_Num(int Num)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                return await dba.QuerySingleOrDefaultAsync<Works_Entity>("Select * From ServiceA Where Num = @Num", new { Num });
            }
            
        }

        /// <summary>
        /// 업무일지 완료
        /// </summary>
        /// <param name="Num"></param>
        public async Task ServiceComplete(string Num)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                string A = await dba.QuerySingleOrDefaultAsync<string>("Select Complete From ServiceA Where Num = @Num", new { Num });
                if (A == "A")
                {
                    await dba.ExecuteAsync("Update ServiceA Set Complete = 'C' Where Num = @Num", new { Num });
                }
                else if (A == "C")
                {
                    await dba.ExecuteAsync("Update ServiceA Set Complete = 'B' Where Num = @Num", new { Num });
                }
                else
                {
                    await dba.ExecuteAsync("Update ServiceA Set Complete = 'A' Where Num = @Num", new { Num });
                }
            }
            
        }

        /// <summary>
        /// 결재 완료
        /// </summary>
        /// <param name="Num"></param>
        public async Task ServiceConform(string Num)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                string A = await dba.QuerySingleOrDefaultAsync<string>("Select Conform From ServiceA Where Num = @Num", new { Num });
                if (A == "A")
                {
                    await dba.ExecuteAsync("Update ServiceA Set Conform = 'B' Where Num = @Num", new { Num });
                }
                else
                {
                    await dba.ExecuteAsync("Update ServiceA Set Conform = 'A' Where Num = @Num", new { Num });
                }
            }
            
        }

        /// <summary>
        /// 업무일지 완료 여부
        /// </summary>
        /// <param name="Num"></param>
        /// <returns></returns>
        public async Task<string> CompleteBeing(string Num)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                return await dba.QuerySingleOrDefaultAsync<string>("Select Complete From ServiceA Where Num = @Num", new { Num });
            }
            
        }

        /// <summary>
        /// 결재 완료 여부
        /// </summary>
        /// <param name="Num"></param>
        /// <returns></returns>
        public async Task<string> ConformBeing(string Num)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                return await dba.QuerySingleOrDefaultAsync<string>("Select Conform From ServiceA Where Num = @Num", new { Num });
            }
            
        }

        /// <summary>
        // 앞 업무 정보
        /// </summary>
        /// <param name="AptCode"></param>
        /// <param name="Num"></param>
        /// <returns></returns>
        public async Task<string> svAgo(string AptCode, string Num)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                return await dba.QuerySingleOrDefaultAsync<string>("Select ISNULL(Num, 0) From ServiceA Where AptCode = @AptCode and Num = (Select max(Num) From ServiceA Where AptCode = @AptCode and Num < @Num)", new { AptCode, Num });
            }
            
        }

        /// <summary>
        // 앞 업무  존재여부
        /// </summary>
        /// <param name="AptCode"></param>
        /// <param name="Num"></param>
        /// <returns></returns>
        public async Task<int> svAgoBe(string AptCode, string Num)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                return await dba.QuerySingleOrDefaultAsync<int>("Select Count(*) From ServiceA Where AptCode = @AptCode and Num = (Select max(Num) From ServiceA Where AptCode = @AptCode and Num < @Num)", new { AptCode, Num });
            }
            
        }

        /// <summary>
        // 뒤 업무
        /// </summary>
        /// <param name="AptCode"></param>
        /// <param name="Num"></param>
        /// <returns></returns>
        public async Task<string> svNext(string AptCode, string Num)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                return await dba.QuerySingleOrDefaultAsync<string>("Select ISNULL(Num, 0) From ServiceA Where AptCode = @AptCode and Num = (Select Min(Num) From ServiceA Where AptCode =@AptCode and Num > @Num)", new { AptCode, Num });
            }
            
        }

        /// <summary>
        // 뒤 업무 존재 여부
        /// </summary>
        /// <param name="AptCode"></param>
        /// <param name="Num"></param>
        /// <returns></returns>
        public async Task<int> svNextBe(string AptCode, string Num)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                return await dba.QuerySingleOrDefaultAsync<int>("Select Count(*) From ServiceA Where AptCode = @AptCode and Num = (Select Min(Num) From ServiceA Where AptCode =@AptCode and Num > @Num)", new { AptCode, Num });
            }
            
        }

        /// <summary>
        /// 업무일지 존재여부
        /// </summary>
        /// <param name="Num"></param>
        /// <returns></returns>
        public async Task<string> WorkBeing(string Num)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                return await dba.QuerySingleOrDefaultAsync<string>("Select isNull(subYear, 'A') From ServiceA Where Num = @Num", new { Num });
            }
            
        }        

        /// <summary>
        /// 업무일지 삭제
        /// </summary>
        /// <param name="id"></param>
        public async Task WorksRemove(string id)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("Ayoung")))
            {
                await dba.QueryAsync("Delete ServiceA Where Num = @id", new { id });
            }            
        }
    }

    /// <summary>
    /// 작업내용 클래스
    /// </summary>
    public class WorksSub_Lib : IWorksSub_Lib
    {
        private readonly IConfiguration _db;
        public WorksSub_Lib(IConfiguration configuration)
        {
            this._db = configuration;
        }

        /// <summary>
        /// 작업 내용 입력
        /// </summary>
        /// <param name="at"></param>
        public async Task Add(WorksSub_Entity at)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                await dba.ExecuteAsync("Insert into WorkSub (Service_Code, Net_Group, AptCode, Cost, Amount, subYear, subMonth, subDay, WorkDate, Out_In_Viw, Scw_Code, OutCorCharger, OutCorMobile, OutCorName, WorkerName, WorkerCount, WorkPost, WorkContent, PostIP, UserID) Values (@Service_Code, @Net_Group, @AptCode, @Cost, @Amount, @subYear, @subMonth, @subDay, @WorkDate, @Out_In_Viw, @Scw_Code, @OutCorCharger, @OutCorMobile, @OutCorName, @WorkerName, @WorkerCount, @WorkPost, @WorkContent, @PostIP, @UserID)", at);
            }
            
        }

        /// <summary>
        /// 작업 내용 수정
        /// </summary>
        /// <param name="at"></param>
        /// <returns></returns>
        public async Task<WorksSub_Entity> Edit(WorksSub_Entity at)
        {
            //var sql = "Update WorkSub Set Cost = @Cost, Amount = @Amount, subYear = @subYear, subMonth = @subMonth, subDay = @subDay, WorkDate = @WorkDate, Out_In_Viw = @Out_In_Viw, Scw_Code = @Scw_Code, OutCorCharger = @OutCorCharger, OutCorMobile = @OutCorMobile, OutCorName = @OutCorName, WorkerName = @WorkerName, WorkerCount = @WorkerCount, WorkPost = @WorkPost, WorkContent = @WorkContent, ModifyIP = @ModifyIP, ModifyDate = @ModifyDate, UserID = @UserID Where Aid = @Aid";
            using var dba = new SqlConnection(_db.GetConnectionString("sw_togather"));
            await dba.ExecuteAsync("Update WorkSub Set Cost = @Cost, Amount = @Amount, subYear = @subYear, subMonth = @subMonth, subDay = @subDay, WorkDate = @WorkDate, Out_In_Viw = @Out_In_Viw, Scw_Code = @Scw_Code, OutCorCharger = @OutCorCharger, OutCorMobile = @OutCorMobile, OutCorName = @OutCorName, WorkerName = @WorkerName, WorkerCount = @WorkerCount, WorkPost = @WorkPost, WorkContent = @WorkContent, ModifyIP = @ModifyIP, ModifyDate = @ModifyDate, UserID = @UserID Where Aid = @Aid", at);
            return at;
        }

        /// <summary>
        /// 해당 외래 코드로 목록 만들기
        /// </summary>
        /// <param name="Service_Code"></param>
        /// <returns></returns>
        public async Task<List<WorksSub_Entity>> Getlist(string Service_Code)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                var lst = await dba.QueryAsync<WorksSub_Entity>("Select Aid, Service_Code, Net_Group, AptCode, Cost, Amount, subYear, subMonth, subDay, WorkDate, Out_In_Viw, Scw_Code, OutCorCharger, OutCorMobile, OutCorName, WorkerName, WorkerCount, WorkPost, WorkContent, Work_Complete, Satisfaction, PostDate, PostIP, UserID, Del FROM WorkSub Where Service_Code = @Service_Code", new { Service_Code });
                return lst.ToList();
            }            
        }

        /// <summary>
        /// 상세보기
        /// </summary>
        /// <param name="Num"></param>
        /// <returns></returns>
        public async Task<WorksSub_Entity> Details(string Num)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                return await dba.QuerySingleOrDefaultAsync<WorksSub_Entity>("Select Aid, Service_Code, Net_Group, AptCode, Cost, Amount, subYear, subMonth, subDay, WorkDate, Out_In_Viw, Scw_Code, OutCorCharger, OutCorMobile, OutCorName, WorkerName, WorkerCount, WorkPost, WorkContent, Work_Complete, Satisfaction, PostDate, PostIP, UserID, Del FROM WorkSub Where Num = @Num", new { Num });
            }            
        }

        /// <summary>
        /// 마지막 번호
        /// </summary>
        /// <returns></returns>
        public async Task<string> LastNumber()
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                return await dba.QuerySingleOrDefaultAsync<string>("Select Top 1 Num From WorkSub Order by Num Desc");
            }            
        }

        /// <summary>
        /// 입력된 수
        /// </summary>
        /// <returns></returns>
        public async Task<int> BeingCount(string AptCode, string Service_Code)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                return await dba.QuerySingleOrDefaultAsync<int>("Select Count(*) Num From WorkSub Where AptCode = @AptCode And Service_Code = @Service_Code", new { AptCode, Service_Code });
            }            
        }        

        /// <summary>
        /// 작업내용 삭제
        /// </summary>
        /// <param name="Aid"></param>
        public async Task Remove(int Aid)
        {
            using (var dba = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                await dba.ExecuteAsync("Delete WorkSub Where Aid = @Aid", new { Aid });
            }            
        }
    }
}