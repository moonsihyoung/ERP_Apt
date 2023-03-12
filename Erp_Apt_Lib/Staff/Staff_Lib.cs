using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Erp_Apt_Staff
{

    /// <summary>
    /// 직원클래스
    /// </summary>
    public class Staff_Lib : IStaff_Lib
    {
        private readonly IConfiguration _db;
        public Staff_Lib(IConfiguration configuration)
        {
            this._db = configuration;
        }

        /// <summary>
        /// 직원 기본 정보 입력
        /// </summary>
        /// <param name="st"></param>
        /// <returns></returns>
        public async Task<int> Add(Staff_Entity st)
        {
            var sql = "Insert into Staff (User_ID, Staff_Cd, Old_UserID, User_Name, Sido, SiGunGu, RestAdress, Scn, Password_sw, Scn_Code, Intro, LevelCount, M_Apt_Code) Values (@User_ID, @Staff_Cd, @Old_UserID, @User_Name, @Sido, @SiGunGu, @RestAdress, @Scn, PwdEncrypt('' + @Password_sw + ''), @Scn_Code, @Intro, @LevelCount, @M_Apt_Code); Select Cast(SCOPE_IDENTITY() As Int);";

            using var df = new SqlConnection(_db.GetConnectionString("sw_togather"));
            st.Aid = await df.QuerySingleOrDefaultAsync<int>(sql, st);
            return st.Aid;
        }

        /// <summary>
        /// 직원 기본 정보 입력
        /// </summary>
        /// <param name="st"></param>
        /// <returns></returns>
        public async Task Add1(int Aid, string st)
        {
            var sql = "Update Staff Set ScnA ='" + st + "' Where Aid = " + Aid;
            using var df = new SqlConnection(_db.GetConnectionString("sw_togather"));
            await df.ExecuteAsync(sql, new { Aid, st });            
        }

        /// <summary>
        /// 직원 기본 정보 수정
        /// </summary>
        /// <param name="st"></param>
        public async Task Edit(Staff_Entity st)
        {
            using (var df = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                await df.ExecuteAsync("Update Staff Set User_Name = @User_Name, Sido = @Sido, SiGunGu = @SiGunGu, RestAdress = @RestAdress, Scn = @Scn, Scn_Code = @Scn_Code, Intro = @Intro Where User_ID = @User_ID", st);
            }
        }

        /// <summary>
        /// 직원 목록
        /// </summary>
        /// <param name="Apt_Code"></param>
        /// <returns></returns>
        public async Task<List<Staff_Entity>> Getlist()
        {
            using (var df = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                var lst = await df.QueryAsync<Staff_Entity>("Select Aid, User_ID, Staff_Cd, Old_UserID, User_Name, Sido, SiGunGu, RestAdress, Scn, Scn, Scn_Code, Intro, LevelCount, VisitCount, WriteCount, ReadCount, CommentsCount, FileUpCount, JoinDate, M_Apt_Code, Del From Staff Order By Aid Desc");
                return lst.ToList();
            }

        }

        /// <summary>
        /// 직원 목록
        /// </summary>
        /// <param name="Apt_Code"></param>
        /// <returns></returns>
        public async Task<List<Staff_Entity>> GetlistApt(string AptCode)
        {
            using (var df = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                var lst = await df.QueryAsync<Staff_Entity>("Select Aid, User_ID, Staff_Cd, Old_User_ID, User_Name, Sido, SiGunGu, RestAdress, Scn, Scn, Scn_Code, Intro, LevelCount, VisitCount, WriteCount, ReadCount, CommentsCount, FileUpCount, JoinDate, M_Apt_Code, Del From Staff Where AptCode = @AptCode Order By Desc", new { AptCode });
                return lst.ToList();
            }

        }

        /// <summary>
        /// 직원 기본정보 상세
        /// </summary>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public async Task<Staff_Entity> View(string UserID)
        {
            using (var df = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                return await df.QuerySingleOrDefaultAsync<Staff_Entity>("Select Aid, User_ID, Staff_Cd, Old_UserID, User_Name, Sido, SiGunGu, RestAdress, Scn, Scn, Scn_Code, Intro, LevelCount, VisitCount, WriteCount, ReadCount, CommentsCount, FileUpCount, JoinDate, Del, M_Apt_Code From Staff Where User_ID = @UserID", new { UserID });
            }

        }

        /// <summary>
        /// 아이디 존재 여부 확인
        /// </summary>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public async Task<int> Be(string UserID)
        {
            using (var df = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                return await df.QuerySingleOrDefaultAsync<int>("Select Count(*) From Staff Where User_ID = @UserID", new { UserID });
            }

        }

        /// <summary>
        /// 아이디와 암호로 일치 여부 확인
        /// </summary>
        /// <param name="UserID"></param>
        /// <param name="Password_sw"></param>
        /// <returns></returns>
        public async Task<int> Login(string User_ID, string Password_sw)
        {
            using (var df = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                return await df.QuerySingleOrDefaultAsync<int>("Staff_Login", new { User_ID, Password_sw }, commandType: CommandType.StoredProcedure);
            }

        }

        /// <summary>
        /// 암호 변경
        /// </summary>
        /// <param name="UserID"></param>
        /// <param name="Password_sw"></param>        
        public async Task<string> Edit_Pass(string User_ID, string Password_sw_Old, string Password_sw_New)
        {
            using (var df = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                int aa = await Login(User_ID, Password_sw_Old);
                string result = "";
                if (aa == 1)
                {
                    var sql = "Update Staff Set Password_sw = PwdEncrypt('' + @Password_sw_New + '') Where User_ID = @User_ID";
                    await df.ExecuteAsync(sql, new { User_ID, Password_sw_Old, Password_sw_New });
                    result = "t";
                }
                else
                {
                    result = "f";
                }

                return result;
            }
        }

        /// <summary>
        /// 삭제(등급을 0으로)
        /// </summary>
        /// <param name="UserID"></param>
        public async Task Remove(string User_ID)
        {
            using (var df = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                await df.ExecuteAsync("Update Staff Set LevelCount = 0 Where User_ID = @User_ID", new { User_ID });
            }

        }

        /// <summary>
        /// 유저 이름 불러오기
        /// </summary>
        /// <param name="User_ID"></param>
        /// <returns></returns>
        public async Task<string> Name(string User_ID)
        {
            using (var df = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                return await df.QuerySingleOrDefaultAsync<string>("Select User_Name From Staff Where User_ID = @User_ID", new { User_ID });
            }

        }

        /// <summary>
        /// 유저 이름 불러오기
        /// </summary>
        /// <param name="User_ID"></param>
        /// <returns></returns>
        public string UsersName(string User_ID)
        {
            using (var df = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                return df.QuerySingleOrDefault<string>("Select User_Name From Staff Where User_ID = @User_ID", new { User_ID });
            }
        }

        /// <summary>
        /// 유저 이름 불러오기
        /// </summary>
        public string Staff_Name(string Apt_Code)
        {
            using (var df = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                return df.QuerySingleOrDefault<string>("SELECT  Top 1 User_Name FROM Referral_Career WHERE (Apt_Code = @Apt_Code) AND (Post = '관리') AND (Duty = '소장' OR Duty = '센터장' OR Duty = '실장' OR Duty = '센타장') AND (Division = 'A') ORDER BY Aid DESC", new { Apt_Code });
            }
        }

        /// <summary>
        /// 유저 생년월일 불러오기
        /// </summary>
        /// <param name="User_ID"></param>
        /// <returns></returns>
        public string Users_bath(string User_ID)
        {
            using (var df = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                return df.QuerySingleOrDefault<string>("Select Scn From Staff Where User_ID = @User_ID", new { User_ID });
            }

        }

        /// <summary>
        /// 직원 마지막 일련번호
        /// </summary>
        /// <returns></returns>
        public async Task<int> List_Number()
        {
            using (var df = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                return await df.QuerySingleOrDefaultAsync<int>("Select Top 1 Aid from Staff Order By Aid Desc");
            }
        }

        /// <summary>
        /// 직원 마지막 일련번호
        /// </summary>
        public async Task<int> Apt_Number_Count(string AptCode)
        {
            using (var df = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                return await df.QuerySingleOrDefaultAsync<int>("Select Count(*) from Referral_Career Where Apt_Code = @AptCode", new {AptCode});
            }

        }

        /// <summary>
        /// 직원 정보 존재 여부
        /// </summary>
        public async Task<int> Being_Staff(string UserName, string Scn, string Sido)
        {
            using (var df = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                return await df.QuerySingleOrDefaultAsync<int>("Select Count(*) From Staff as a Join Staff_Sub as b on a.User_ID = b.User_ID Where a.User_Name = @UserName And a.Scn = @Scn And b.st_Sido = @Sido And b.st_GunGu = @GunGo", new { UserName, Scn, Sido });
            }

        }

        // <summary>
        /// 모바일과 이름으로 직원 정보 불러오기
        /// </summary>
        /// <param name="UserName">이름</param>
        /// <param name="Mobile">모바일</param>
        /// <returns></returns>
        public async Task<Staff_StaffSub_Entity> Be_Staff(string UserName, string Mobile, string Mobile_A)
        {
            using var df = new SqlConnection(_db.GetConnectionString("sw_togather"));
            int result = await df.QuerySingleOrDefaultAsync<int>("Select Count(*) From Staff as a Join Staff_Sub as b on a.User_ID = b.User_ID Where a.User_Name = @UserName And (b.Mobile_Number = @Mobile or b.Mobile_Number = @Mobile_A) And a.Del = 'A'", new { UserName, Mobile, Mobile_A });
            if (result > 0)
            {
                return await df.QuerySingleOrDefaultAsync<Staff_StaffSub_Entity>("Select Top 1 a.User_ID, a.User_Name, a.Scn, a.Intro, b.Mobile_Number, b.Email From Staff as a Join Staff_Sub as b on a.User_ID = b.User_ID Where a.User_Name = @UserName And (b.Mobile_Number = @Mobile or b.Mobile_Number = @Mobile_A) And a.Del = 'A' Order By b.Aid Desc", new { UserName, Mobile, Mobile_A });
            }
            else
            {
                return await df.QuerySingleOrDefaultAsync<Staff_StaffSub_Entity>("Select a.User_ID, a.User_Name, a.Scn, a.Intro, b.Mobile_Number, b.Email From Staff as a Join Staff_Sub as b on a.User_ID = b.User_ID Where a.User_ID = 'dodam'", new { UserName, Mobile, Mobile_A });
            }

        }

        /// <summary>
        /// 등급 수정
        /// </summary>
        public async Task LevelUpdate(string User_ID, int LevelCount)
        {
            using var df = new SqlConnection(_db.GetConnectionString("sw_togather"));
            await df.ExecuteAsync("Update Staff Set LevelCount = @LevelCount Where User_ID = @User_ID", new { User_ID, LevelCount });
            await df.ExecuteAsync("Update Staff_Sub Set levelCount = @LevelCount Where User_ID = @User_ID", new { User_ID, LevelCount });
        }


        /// <summary>
        /// 존재 여부 확인
        /// </summary>
        public async Task<int> Be_Staff_Count(string UserName, string Mobile, string Mobile_A)
        {
            using (var df = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                return await df.QuerySingleOrDefaultAsync<int>("Select Count(*) From Staff as a Join Staff_Sub as b on a.User_ID = b.User_ID Where a.User_Name = @UserName And (b.Mobile_Number = @Mobile or b.Mobile_Number = @Mobile_A) And a.Del = 'A'", new { UserName, Mobile, Mobile_A });
            }
        }

        /// <summary>
        /// 직원 등급 정보 불러오기
        /// </summary>
        public async Task<int> Staff_LevelCount(string UserId)
        {
            using (var df = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                return await df.QuerySingleOrDefaultAsync<int>("Select LevelCount From Staff Where User_ID = @UserId And Del = 'A'", new { UserId });
            }
        }

        /// <summary>
        /// 지역과 이름 직원 검색
        /// </summary>
        public async Task<List<Staff_Entity>> Staff_Name_Search(string User_Name, string Sido, string SiGunGu)
        {
            using (var df = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                var lst = await df.QueryAsync<Staff_Entity>("Select Aid, User_ID, Staff_Cd, Old_UserID, User_Name, Sido, SiGunGu, RestAdress, Scn, Scn_Code, Intro, LevelCount, VisitCount, WriteCount, ReadCount, CommentsCount, FileUpCount, JoinDate, M_Apt_Code, Del From Staff Where Sido = @Sido And SiGunGu = @SiGunGu And User_Name = @User_Name", new { User_Name, Sido, SiGunGu });
                return lst.ToList();
            }
        }

        /// <summary>
        /// 지역과 이름 직원 검색
        /// </summary>
        public async Task<List<Staff_Entity>> Staff_Name_SearchA(string User_Name)
        {
            using var df = new SqlConnection(_db.GetConnectionString("sw_togather"));
            var lst = await df.QueryAsync<Staff_Entity>("Select Top 50 Aid, User_ID, Staff_Cd, Old_UserID, User_Name, Sido, SiGunGu, RestAdress, Scn, Scn_Code, Intro, LevelCount, VisitCount, WriteCount, ReadCount, CommentsCount, FileUpCount, JoinDate, M_Apt_Code, Del From Staff Where User_Name Like '%" + User_Name + "%' Order By User_Name Asc", new { User_Name });
            return lst.ToList();
        }


        /// <summary>
        /// 방문수 증가
        /// </summary>
        /// <param name="User_Code"></param>
        public async Task VisitCount_Add(string User_Code)
        {
            using var df = new SqlConnection(_db.GetConnectionString("sw_togather"));
            await df.ExecuteAsync("Update Staff Set VisitCount = VisitCount +1 Where User_ID = @User_Code", new { User_Code });
        }

        /// <summary>
        /// 글수 증가
        /// </summary>
        public async Task WriteCount_Add(string User_Code)
        {
            using var df = new SqlConnection(_db.GetConnectionString("sw_togather"));
            await df.ExecuteAsync("Update Staff Set WriteCount = WriteCount +1 Where User_ID = @User_Code", new { User_Code });
        }

        /// <summary>
        /// 읽은 수 증가
        /// </summary>
        public async Task ReadCount_Add(string User_Code)
        {
            using var df = new SqlConnection(_db.GetConnectionString("sw_togather"));
            await df.ExecuteAsync("Update Staff Set ReadCount = ReadCount +1 Where User_ID = @User_Code", new { User_Code });
        }

        /// <summary>
        /// 댓글수 증가
        /// </summary>
        public async Task CommentsCount_Add(string User_Code)
        {
            using var df = new SqlConnection(_db.GetConnectionString("sw_togather"));
            await df.ExecuteAsync("Update Staff Set CommentsCount = CommentsCount +1 Where User_ID = @User_Code", new { User_Code });
        }

        /// <summary>
        /// 파일 업로드 수 증가
        /// </summary>
        public async Task FileUpCount_Add(string User_Code)
        {
            using var df = new SqlConnection(_db.GetConnectionString("sw_togather"));
            await df.ExecuteAsync("Update Staff Set FileUpCount = FileUpCount +1 Where User_ID = @User_Code", new { User_Code });
        }
    }


    /// <summary>
    /// 직원 상세정보 클래스
    /// </summary>
    public class Staff_Sub_Lib : IStaff_Sub_Lib
    {
        private readonly IConfiguration _db;
        public Staff_Sub_Lib(IConfiguration configuration)
        {
            this._db = configuration;
        }

        /// <summary>
        /// 직원 상세정보 입력
        /// </summary>
        public async Task<int> Add(Staff_Sub_Entity sse)
        {
            var sql = "Insert Staff_Sub (Mobile_Number, TelePhone, Email, Staff_Cd, User_ID, levelcount, Staff_Name, Staff_Sub_Cd, st_Sido, st_GunGu, st_Adress_Rest, Etc, Start_Date, d_division, M_Apt_Code) Values (@Mobile_Number, @TelePhone, @Email, @Staff_Cd, @User_ID, @levelcount, @Staff_Name, @Staff_Sub_Cd, @st_Sido, @st_GunGu, @st_Adress_Rest, @Etc, @Start_Date, @d_division, @M_Apt_Code); Select Cast(SCOPE_IDENTITY() As Int);";
            using (var df = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                sse.Aid = await df.QuerySingleOrDefaultAsync<int>(sql, sse);
                //int a = await df.QuerySingleOrDefaultAsync<int>("Select Count(*) From Staff_Sub Where User_ID = @User_ID", sse.User_ID);
                //if (a > 0)
                //{
                //    int b = await df.QuerySingleOrDefaultAsync<int>("Select Top 1 Aid From Staff_Sub Where User_ID = @User_ID Order by Aid Desc", sse.User_ID);
                //    DateTime db = DateTime.Now;
                //    await df.ExecuteAsync("Update Staff_Sub Set End_Date = " + db + ", Levelcount = 0 Where User_ID = @User_ID And Aid = @Aid", sse.User_ID);
                //}
                return sse.Aid;
            }

        }

        /// <summary>
        /// 직원 상세 정보 수정
        /// </summary>
        public async Task Edit(Staff_Sub_Entity sse)
        {
            using (var df = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                await df.ExecuteAsync("Update Staff_Sub Set Mobile_Number = @Mobile_Number, TelePhone = @TelePhone, Email = @Email, levelcount = @levelcount, st_Sido = @st_Sido, st_GunGu = @st_GunGu, st_Adress_Rest = @st_Adress_Rest, Etc = @Etc, Start_Date = @Start_Date, d_division = @d_division Where User_ID = @User_ID", sse);
            }

        }

        /// <summary>
        /// 직원 상세 정보 보기
        /// </summary>
        public async Task<Staff_Sub_Entity> View(string User_ID)
        {
            using (var df = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                return await df.QuerySingleOrDefaultAsync<Staff_Sub_Entity>("Select Top 1 Aid, Mobile_Number, TelePhone, Email, Staff_Cd, User_ID, levelcount, Staff_Name, Staff_Sub_Cd, st_Sido, st_GunGu, st_Adress_Rest, Etc, Start_Date, End_Date, d_division, PostDate From Staff_Sub Where User_ID = @User_ID Order By Aid Desc", new { User_ID });
            }

        }

        /// <summary>
        /// 직원 상세정보 존재 여부
        /// </summary>
        public async Task<int> be(string User_ID)
        {
            using (var df = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                return await df.QuerySingleOrDefaultAsync<int>("Select Count(*) From Staff_Sub Where User_ID = @User_ID", new { User_ID });
            }

        }

        /// <summary>
        /// 직원 상세 정보 삭제
        /// </summary>
        public async Task Remove(string User_ID)
        {
            using (var df = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                await df.ExecuteAsync("Delete Staff_Sub Where User_ID = @User_ID", new { User_ID });
            }

        }

        /// <summary>
        /// 도장 이미지 불러오기
        /// </summary>
        public async Task<string> SealView(string User_ID)
        {
            using (var df = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                return await df.QuerySingleOrDefaultAsync<string>("Select isNull(Seal, 'A') From Staff_Sub Where User_ID = @User_ID", new { User_ID });
            }

        }

        /// <summary>
        /// 회원정보 상세보기
        /// </summary>
        public async Task<Staff_Sub_Entity> Detail(string User_ID)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                return await ctx.QuerySingleOrDefaultAsync<Staff_Sub_Entity>("Select Top 1 Aid, Mobile_Number, TelePhone, Email, Staff_Cd, User_ID, LevelCount, Staff_Name, st_Sido, st_GunGu, st_Adress_Rest, Etc, Start_Date, End_Date, d_division From Staff_Sub Where User_ID = @User_ID Order By Aid Desc", new { User_ID });
            }
        }

        /// <summary>
        /// 시군구 이름 해당 직원 목록 만들기
        /// </summary>
        public async Task<List<Staff_Sub_Entity>> GetList_Name(string Sido, string Gungo, string Name)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                var lst = await ctx.QueryAsync<Staff_Sub_Entity>("Select Aid, Mobile_Number, TelePhone, Email, Staff_Cd, User_ID, LevelCount, Staff_Name, st_Sido, st_GunGu, st_Adress_Rest, Etc, Start_Date, End_Date, d_division From Staff_Sub Where st_Sido = @Sido And st_GunGu = @Gungo And Staff_Name = @Name Order By Aid Desc", new { Sido, Gungo, Name });
                return lst.ToList();
            }
        }
    }


    /// <summary>
    /// 직원 정보 조인
    /// </summary>
    public class Staff_staffSub_Lib : IStaff_staffSub_Lib
    {
        private readonly IConfiguration _db;
        public Staff_staffSub_Lib(IConfiguration configuration)
        {
            this._db = configuration;
        }

        /// <summary>
        /// 직원정보 상세
        /// </summary>
        public async Task<Staff_StaffSub_Entity> Views(string User_ID)
        {
            using (var df = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                return await df.QuerySingleOrDefaultAsync<Staff_StaffSub_Entity>("Select Top 1 a.Aid, a.CommentsCount, a.Del, a.FileUpCount, a.JoinDate, a.LevelCount, a.ReadCount, a.Scn, a.User_ID, a.User_Name, a.VisitCount, a.WriteCount, b.d_division, b.Email, b.End_Date, b.Etc, a.Intro, b.Mobile_Number, b.PostDate, b.st_Adress_Rest, b.st_GunGu, b.st_Sido, b.Staff_Sub_Cd, b.Start_Date, b.TelePhone From Staff as a Join Staff_Sub as b On a.User_ID = b.User_ID Where a.LevelCount > 1 And a.User_ID = @User_ID Order by a.Aid Desc", new { User_ID });
            }

        }

        /// <summary>
        /// 배치정보 상세
        /// </summary>
        public async Task<Staff_Career_Entity> CareerViews(string User_ID)
        {
            using (var df = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                return await df.QuerySingleOrDefaultAsync<Staff_Career_Entity>("Select Top 1 a.Aid, a.CommentsCount, a.Del, a.FileUpCount, a.JoinDate, a.LevelCount, a.ReadCount, a.Scn, a.User_ID, a.User_Name, a.VisitCount, a.WriteCount, b.d_division, b.Email, b.End_Date, b.Etc, a.Intro, b.Mobile_Number, b.PostDate, b.st_Adress_Rest, b.st_GunGu, b.st_Sido, b.Staff_Sub_Cd, b.Start_Date, b.TelePhone From Staff as a Join Staff_Sub as b On a.User_ID = b.User_ID Where a.LevelCount > 1 And a.User_ID = @User_ID Order by a.Aid Desc", new { User_ID });
            }

        }

        /// <summary>
        /// 단지에서 입력한 회원 정보
        /// </summary>
        public async Task<List<Staff_StaffSub_Entity>> GetlistApt(string M_Apt_Code)
        {
            using (var df = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                var lst = await df.QueryAsync<Staff_StaffSub_Entity>("Select b.Aid, b.CommentsCount, b.FileUpCount, b.JoinDate, b.ReadCount, b.Scn, b.User_ID, b.User_Name, b.VisitCount, b.WriteCount, b.Del, c.d_division, c.Email, c.Etc, c.levelcount, c.Mobile_Number, c.PostDate, c.st_Adress_Rest, c.st_GunGu, c.st_Sido, c.Start_Date, c.TelePhone, c.End_Date From Staff as b Join Staff_Sub as c on b.User_ID = c.User_ID where b.Apt_Code = @M_Apt_Code Order by b.Aid Desc", new { M_Apt_Code });
                return lst.ToList();
            }
        }


        /// <summary>
        /// 회원 정보 목록
        /// </summary>
        public async Task<List<Staff_StaffSub_Entity>> GetList(int Page)
        {
            using (var df = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                var lst = await df.QueryAsync<Staff_StaffSub_Entity>("Select Top 15 a.Aid, a.User_ID, a.User_Name, a.FileUpCount, a.JoinDate, a.ReadCount, a.Scn, b.User_ID, a.User_Name, a.VisitCount, a.WriteCount, a.Del, b.d_division, b.Email, b.Etc, a.levelcount, b.Mobile_Number, b.PostDate, b.st_Adress_Rest, b.st_GunGu, b.st_Sido, b.Start_Date, b.TelePhone, b.End_Date From Staff as a Join Staff_Sub as b on a.User_ID = b.User_ID where a.Aid Not In (Select Top (15 * @Page) a.Aid From Staff as a Join Staff_Sub as b on b.User_ID = a.User_ID Order By a.Aid Desc) Order by a.Aid Desc", new { Page });
                return lst.ToList();
            }
        }

        /// <summary>
        /// 회원 정보 목록 수
        /// </summary>
        public async Task<int> GetList_Count()
        {
            using (var df = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                return await df.QuerySingleOrDefaultAsync<int>("Select Count(*) From Staff as b Join Staff_Sub as c on b.User_ID = c.User_ID");         
            }
        }

        /// <summary>
        /// 회원 배치 정보 목록
        /// </summary>
        public async Task<List<Staff_Career_Entity>> GetCareerList(int Page)
        {
            using (var df = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                var lst = await df.QueryAsync<Staff_Career_Entity>("Select Top 15 a.Aid, a.User_ID, a.User_Name, a.FileUpCount, a.JoinDate, a.ReadCount, a.Scn, b.User_ID, a.User_Name, a.VisitCount, a.WriteCount, a.Del, b.d_division, b.Email, b.Etc, a.levelcount, b.Mobile_Number, b.PostDate, b.st_Adress_Rest, b.st_GunGu, b.st_Sido, b.Start_Date, b.TelePhone, b.End_Date, c.Career_End_Date, c.Career_Start_Date, c.CC_Code, c.ContractSort_Code, c.Division, c.Duty, c.Etc, c.Post, c.Post_Code, c.Apt_Code, c.Apt_Name From Staff as a Join Staff_Sub as b on a.User_ID = b.User_ID Join Referral_Career as c on a.User_ID = c.User_ID where a.Aid Not In (Select Top (15 * @Page) a.Aid From Staff as a Join Staff_Sub as b on b.User_ID = a.User_ID Join Referral_Career as c on a.User_ID = c.User_ID Order By a.Aid Desc) Order by a.Aid Desc", new { Page });
                return lst.ToList();
            }
        }

        /// <summary>
        /// 회원 배치 정보 목록 수
        /// </summary>
        public async Task<int> GetCareerList_Count()
        {
            using (var df = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                return await df.QuerySingleOrDefaultAsync<int>("Select Count(*) From Staff as a Join Staff_Sub as b on a.User_ID = b.User_ID Join Referral_Career as c on a.User_ID = c.User_ID");
            }
        }

        /// <summary>
        /// 회원 정보 존재여부 확인
        /// </summary>
        public async Task<int> GetListInsertCount(string Name, string Mobile)
        {
            using var df = new SqlConnection(_db.GetConnectionString("sw_togather"));
            return await df.QuerySingleOrDefaultAsync<int>("Select Count(*) From Staff as a Join Staff_Sub as b on a.User_ID = b.User_ID Where a.User_Name = @Name And b.Mobile_Number = @Mobile", new { Name, Mobile });
        }

        public async Task<List<Staff_StaffSub_Entity>> GetList_Search(string Feild, string Query)
        {
            using var df = new SqlConnection(_db.GetConnectionString("sw_togather"));
            if (Feild == "User_Name")
            {
                var sql = @"Select a.Aid, a.User_ID, a.User_Name, a.FileUpCount, a.JoinDate, a.ReadCount, a.Scn, b.User_ID, a.User_Name, a.VisitCount, a.WriteCount, a.Del, b.d_division, b.Email, b.Etc, a.levelcount, b.Mobile_Number, b.PostDate, b.st_Adress_Rest, b.st_GunGu, b.st_Sido, b.Start_Date, b.TelePhone, b.End_Date From Staff as a Join Staff_Sub as b on A.User_ID = B.User_ID Where a.User_Name = @Query";
                var lst = await df.QueryAsync<Staff_StaffSub_Entity>(sql, new { Feild, Query });
                return lst.ToList();
            }
            else if (Feild == "Mobile_Number")
            {
                var sql = @"Select a.Aid, a.User_ID, a.User_Name, a.FileUpCount, a.JoinDate, a.ReadCount, a.Scn, b.User_ID, a.User_Name, a.VisitCount, a.WriteCount, a.Del, b.d_division, b.Email, b.Etc, a.levelcount, b.Mobile_Number, b.PostDate, b.st_Adress_Rest, b.st_GunGu, b.st_Sido, b.Start_Date, b.TelePhone, b.End_Date From Staff as a Join Staff_Sub as b on a.User_ID = b.User_ID Where b.Mobile_Number Like '%" + Query + "%'";
                var lst = await df.QueryAsync<Staff_StaffSub_Entity>(sql, new { Feild, Query });
                return lst.ToList();
            }
            else
            {
                var lst = await df.QueryAsync<Staff_StaffSub_Entity>("Select Top 15 a.Aid, a.User_ID, a.User_Name, a.FileUpCount, a.JoinDate, a.ReadCount, a.Scn, b.User_ID, a.User_Name, a.VisitCount, a.WriteCount, a.Del, b.d_division, b.Email, b.Etc, a.levelcount, b.Mobile_Number, b.PostDate, b.st_Adress_Rest, b.st_GunGu, b.st_Sido, b.Start_Date, b.TelePhone, b.End_Date From Staff as a Join Staff_Sub as b on a.User_ID = b.User_ID Order by a.Aid Desc");
                return lst.ToList();
            }
        }

        public async Task<List<Staff_Career_Entity>> GetCareerList_Search(int Page, string Feild, string Query, string AptName)
        {
            using var df = new SqlConnection(_db.GetConnectionString("sw_togather"));
            if (Feild == "User_Name")
            {
                var sql = @"Select Top 15 a.Aid, a.User_ID, a.User_Name, a.FileUpCount, a.JoinDate, a.ReadCount, a.Scn, b.User_ID, a.User_Name, a.VisitCount, a.WriteCount, a.Del, b.d_division, b.Email, b.Etc, a.levelcount, b.Mobile_Number, b.PostDate, b.st_Adress_Rest, b.st_GunGu, b.st_Sido, b.Start_Date, b.TelePhone, b.End_Date, c.Career_End_Date, c.Career_Start_Date, c.CC_Code, c.ContractSort_Code, c.Division, c.Duty, c.Etc, c.Post, c.Post_Code, c.Apt_Code, c.Apt_Name From Staff as a Join Staff_Sub as b on a.User_ID = b.User_ID Join Referral_Career as c on a.User_ID = c.User_ID Where a .Aid Not In (Select Top (15 * @Page) a.Aid From Staff as a Join Staff_Sub as b on b.User_ID = a.User_ID Join Referral_Career as c on a.User_ID = c.User_ID Where a.User_Name = @Query And c.Apt_Name = @AptName Order by a.Aid Desc) And a.User_Name = @Query And c.Apt_Name = @AptName Order by a.Aid Desc";
                var lst = await df.QueryAsync<Staff_Career_Entity>(sql, new { Page, Feild, Query, AptName });
                return lst.ToList();
            }
            else if (Feild == "Apt_Name")
            {
                var sql = @"Select Top 15 a.Aid, a.User_ID, a.User_Name, a.FileUpCount, a.JoinDate, a.ReadCount, a.Scn, b.User_ID, a.User_Name, a.VisitCount, a.WriteCount, a.Del, b.d_division, b.Email, b.Etc, a.levelcount, b.Mobile_Number, b.PostDate, b.st_Adress_Rest, b.st_GunGu, b.st_Sido, b.Start_Date, b.TelePhone, b.End_Date, c.Career_End_Date, c.Career_Start_Date, c.CC_Code, c.ContractSort_Code, c.Division, c.Duty, c.Etc, c.Post, c.Post_Code, c.Apt_Code, c.Apt_Name From Staff as a Join Staff_Sub as b on a.User_ID = b.User_ID Join Referral_Career as c on a.User_ID = c.User_ID Where a .Aid Not In (Select Top (15 * @Page) a.Aid From Staff as a Join Staff_Sub as b on b.User_ID = a.User_ID Join Referral_Career as c on a.User_ID = c.User_ID Where c.Apt_Name Like '%" + Query + "%' Order by a.Aid Desc) And c.Apt_Name Like '%" + Query + "%' Order by a.Aid Desc";
                var lst = await df.QueryAsync<Staff_Career_Entity>(sql, new { Page, Feild, Query, AptName });
                return lst.ToList();
            }
            else if (Feild == "Mobile_Number")
            {
                var sql = @"Select top 15 a.Aid, a.User_ID, a.User_Name, a.FileUpCount, a.JoinDate, a.ReadCount, a.Scn, b.User_ID, a.User_Name, a.VisitCount, a.WriteCount, a.Del, b.d_division, b.Email, b.Etc, a.levelcount, b.Mobile_Number, b.PostDate, b.st_Adress_Rest, b.st_GunGu, b.st_Sido, b.Start_Date, b.TelePhone, b.End_Date, c.Career_End_Date, c.Career_Start_Date, c.CC_Code, c.ContractSort_Code, c.Division, c.Duty, c.Etc, c.Post, c.Post_Code, c.Apt_Code, c.Apt_Name From Staff as a Join Staff_Sub as b on a.User_ID = b.User_ID Join Referral_Career as c on a.User_ID = c.User_ID Where a .Aid Not In (Select Top (15 * @Page) a.Aid From Staff as a Join Staff_Sub as b on b.User_ID = a.User_ID Join Referral_Career as c on a.User_ID = c.User_ID Where b.Mobile_Number Like '%" + Query + "%' And c.Apt_Name = @AptName Order by a.Aid Desc) And b.Mobile_Number Like '%" + Query + "%' And c.Apt_Name = @AptName Order by a.Aid Desc";
                var lst = await df.QueryAsync<Staff_Career_Entity>(sql, new { Page, Feild, Query, AptName });
                return lst.ToList();
            }
            else if (Feild == "Post")
            {
                var sql = @"Select a.Aid, a.User_ID, a.User_Name, a.FileUpCount, a.JoinDate, a.ReadCount, a.Scn, b.User_ID, a.User_Name, a.VisitCount, a.WriteCount, a.Del, b.d_division, b.Email, b.Etc, a.levelcount, b.Mobile_Number, b.PostDate, b.st_Adress_Rest, b.st_GunGu, b.st_Sido, b.Start_Date, b.TelePhone, b.End_Date, c.Career_End_Date, c.Career_Start_Date, c.CC_Code, c.ContractSort_Code, c.Division, c.Duty, c.Etc, c.Post, c.Post_Code, c.Apt_Code, c.Apt_Name From Staff as a Join Staff_Sub as b on a.User_ID = b.User_ID Join Referral_Career as c on a.User_ID = c.User_ID Wherea .Aid Not In (Select Top (15 * @Page) a.Aid From Staff as a Join Staff_Sub as b on b.User_ID = a.User_ID Join Referral_Career as c on a.User_ID = c.User_ID And c.Post = @Query And c.Apt_Name = @AptName Order by a.Aid Desc) And c.Post = @Query And c.Apt_Name = @AptName Order by a.Aid Desc";
                var lst = await df.QueryAsync<Staff_Career_Entity>(sql, new { Page, Feild, Query, AptName });
                return lst.ToList();
            }
            else if (Feild == "Duty")
            {
                var sql = @"Select a.Aid, a.User_ID, a.User_Name, a.FileUpCount, a.JoinDate, a.ReadCount, a.Scn, b.User_ID, a.User_Name, a.VisitCount, a.WriteCount, a.Del, b.d_division, b.Email, b.Etc, a.levelcount, b.Mobile_Number, b.PostDate, b.st_Adress_Rest, b.st_GunGu, b.st_Sido, b.Start_Date, b.TelePhone, b.End_Date, c.Career_End_Date, c.Career_Start_Date, c.CC_Code, c.ContractSort_Code, c.Division, c.Duty, c.Etc, c.Post, c.Post_Code, c.Apt_Code, c.Apt_Name From Staff as a Join Staff_Sub as b on a.User_ID = b.User_ID Join Referral_Career as c on a.User_ID = c.User_ID Wherea .Aid Not In (Select Top (15 * @Page) a.Aid From Staff as a Join Staff_Sub as b on b.User_ID = a.User_ID Join Referral_Career as c on a.User_ID = c.User_ID And c.Duty = @Query And c.Apt_Name = @AptName Order by a.Aid Desc) And c.Duty = @Query And c.Apt_Name = @AptName Order by a.Aid Desc";
                var lst = await df.QueryAsync<Staff_Career_Entity>(sql, new { Page, Feild, Query, AptName });
                return lst.ToList();
            }
            else
            {
                var lst = await df.QueryAsync<Staff_Career_Entity>("Select Top 15 a.Aid, a.User_ID, a.User_Name, a.FileUpCount, a.JoinDate, a.ReadCount, a.Scn, b.User_ID, a.User_Name, a.VisitCount, a.WriteCount, a.Del, b.d_division, b.Email, b.Etc, a.levelcount, b.Mobile_Number, b.PostDate, b.st_Adress_Rest, b.st_GunGu, b.st_Sido, b.Start_Date, b.TelePhone, b.End_Date, c.Career_End_Date, c.Career_Start_Date, c.CC_Code, c.ContractSort_Code, c.Division, c.Duty, c.Etc, c.Post, c.Post_Code, c.Apt_Code, c.Apt_Name From Staff as a Join Staff_Sub as b on a.User_ID = b.User_ID Join Referral_Career as c on a.User_ID = c.User_ID where a.Aid Not In (Select Top (15 * @Page) a.Aid From Staff as a Join Staff_Sub as b on b.User_ID = a.User_ID Join Referral_Career as c on a.User_ID = c.User_ID Order By a.Aid Desc) Order by a.Aid Desc", new { Page });
                return lst.ToList();
            }
        }

        public async Task<int> GetCareer_Search_Count(string Feild, string Query, string AptName)
        {
            using var df = new SqlConnection(_db.GetConnectionString("sw_togather"));
            if (Feild == "User_Name")
            {
               return  await df.QuerySingleOrDefaultAsync<int>("Select Count(*) From Staff as a Join Staff_Sub as b on a.User_ID = b.User_ID Join Referral_Career as c on a.User_ID = c.User_ID Where a.User_Name = @Query And c.Apt_Name = @AptName", new { Feild, Query, AptName });
            }
            else if (Feild == "Apt_Name")
            {
                return await df.QuerySingleOrDefaultAsync<int>("Select Count(*) From Staff as a Join Staff_Sub as b on a.User_ID = b.User_ID Join Referral_Career as c on a.User_ID = c.User_ID Where c.Apt_Name Like '%" + Query + "%'", new { Feild, Query, AptName });
            }
            else if (Feild == "Mobile_Number")
            {
                return await df.QuerySingleOrDefaultAsync<int>("Select Count(*) From Staff as a Join Staff_Sub as b on a.User_ID = b.User_ID Join Referral_Career as c on a.User_ID = c.User_ID Where b.Mobile_Number Like '%" + Query + "%' And c.Apt_Name = @AptName", new { Feild, Query, AptName });
            }
            else if (Feild == "Post")
            {
                return await df.QuerySingleOrDefaultAsync<int>("Select Count(*) From Staff as a Join Staff_Sub as b on a.User_ID = b.User_ID Join Referral_Career as c on a.User_ID = c.User_ID Where c.Post = @Query And c.Apt_Name = @AptName", new { Feild, Query, AptName });
            }
            else if (Feild == "Duty")
            {
                return await df.QuerySingleOrDefaultAsync<int>("Select Count(*) From Staff as a Join Staff_Sub as b on a.User_ID = b.User_ID Join Referral_Career as c on a.User_ID = c.User_ID Where c.Duty = @Query And c.Apt_Name = @AptName", new { Feild, Query, AptName });
            }
            else
            {                
                return await df.QuerySingleOrDefaultAsync<int>("Select Count(*) From Staff as a Join Staff_Sub as b on a.User_ID = b.User_ID Join Referral_Career as c on a.User_ID = c.User_ID");
            }
        }
    }

    /// <summary>
    /// 직원 클래스
    /// </summary>
    public class Referral_career_Lib : IReferral_career_Lib
    {
        private readonly IConfiguration _db;
        public Referral_career_Lib(IConfiguration configuration)
        {
            this._db = configuration;
        }

        /// <summary>
        /// 부서별 직원 목록
        /// </summary>
        /// <param name="Post"></param>
        /// <param name="Apt_Code"></param>
        /// <returns></returns>
        public async Task<List<Referral_career_Entity>> GetList_Post_Staff(string Post, string Apt_Code)
        {
            using (var df = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                var lst = await df.QueryAsync<Referral_career_Entity>("SELECT Aid, Staff_Cd, User_ID, User_Name, Apt_Code, Apt_Name, Cor_Code, Post, Duty, Career_Start_Date, Career_End_Date, Division, PostDatge, Post_Code, CC_Code, ContractSort_Code, Etc FROM Referral_Career WHERE Post = @Post And Apt_Code = @Apt_Code", new { Post, Apt_Code });
                return lst.ToList();
            }

        }

        /// <summary>
        /// 부서별 직원 목록(현직)
        /// </summary>
        /// <param name="Post"></param>
        /// <param name="Apt_Code"></param>
        /// <returns></returns>
        public async Task<List<Referral_career_Entity>> GetList_Post_Staff_be(string Post, string Apt_Code)
        {
            using (var df = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                var lst = await df.QueryAsync<Referral_career_Entity>("SELECT Aid, Staff_Cd, User_ID, User_Name, Apt_Code, Apt_Name, Cor_Code, Post, Duty, Career_Start_Date, Career_End_Date, Division, PostDatge, Post_Code, CC_Code, ContractSort_Code, Etc FROM Referral_Career WHERE Post = @Post And Apt_Code = @Apt_Code And Division = 'A'", new { Post, Apt_Code });
                return lst.ToList();
            }

        }

        /// <summary>
        /// 부서별 직원 목록(현직) 수
        /// </summary>
        /// <param name="Post"></param>
        /// <param name="Apt_Code"></param>
        /// <returns></returns>
        public async Task<int> GetList_Post_Staff_beCount(string Post, string Apt_Code)
        {
            using (var df = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                return await df.QuerySingleOrDefaultAsync<int>("SELECT Count(*) FROM Referral_Career WHERE Post = @Post And Apt_Code = @Apt_Code And Division = 'A'", new { Post, Apt_Code });
            }

        }

        /// <summary>
        /// 관리소장 목록(현직)
        /// </summary>
        /// <param name="Post"></param>
        /// <returns></returns>
        public async Task<List<Referral_career_Entity>> GetList_Sojang()
        {
            using (var df = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                var lst = await df.QueryAsync<Referral_career_Entity>("SELECT Aid, Staff_Cd, User_ID, User_Name, Apt_Code, Apt_Name, Cor_Code, Post, Duty, Career_Start_Date, Career_End_Date, Division, PostDatge, Post_Code, CC_Code, ContractSort_Code, Etc FROM Referral_Career WHERE Post = '관리' And Duty = '소장' And Division = 'A' Order By Aid Desc");
                return lst.ToList();
            }
        }

        /// <summary>
        /// 단지별 관리소장 목록
        /// </summary>
        /// <param name="Post"></param>
        /// <returns></returns>
        public async Task<List<Referral_career_Entity>> GetList_Apt_Sojang(string Apt_Code)
        {
            using (var df = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                var lst = await df.QueryAsync<Referral_career_Entity>("SELECT Aid, Staff_Cd, User_ID, User_Name, Apt_Code, Apt_Name, Cor_Code, Post, Duty, Career_Start_Date, Career_End_Date, Division, PostDatge, Post_Code, CC_Code, ContractSort_Code, Etc FROM Referral_Career WHERE (Duty = '소장' or Duty = '실장' or Duty = '센터장') And Apt_Code = @Apt_Code Order By Aid Desc", new { Apt_Code });
                return lst.ToList();
            }
        }

        /// <summary>
        /// 관리소장 목록(현직) 수
        /// </summary>
        /// <param name="Post"></param>
        /// <param name="Apt_Code"></param>
        /// <returns></returns>
        public async Task<int> GetList_Sojang_Count()
        {
            using (var df = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                return await df.QuerySingleOrDefaultAsync<int>("SELECT Count(*) FROM Referral_Career WHERE Post = '관리' And Duty = '소장' And Division = 'A'");
            }

        }

        /// <summary>
        /// 관리소장 목록
        /// </summary>
        /// <param name="Post"></param>
        /// <returns></returns>
        public async Task<List<Referral_career_Entity>> GetList_SojangA(int Page)
        {
            using (var df = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                var lst = await df.QueryAsync<Referral_career_Entity>("SELECT Top 15 Aid, Staff_Cd, User_ID, User_Name, Apt_Code, Apt_Name, Cor_Code, Post, Duty, Career_Start_Date, Career_End_Date, Division, PostDatge, Post_Code, CC_Code, ContractSort_Code, Etc FROM Referral_Career WHERE Aid Not In (Select Top (15 * @Page) Aid From Referral_Career Where Post = '관리' And Duty = '소장' Order By Aid Desc) And Post = '관리' And Duty = '소장' Order By Aid Desc", new { Page });
                return lst.ToList();
            }
        }

        /// <summary>
        /// 관리소장 목록 수
        /// </summary>
        /// <param name="Post"></param>
        /// <param name="Apt_Code"></param>
        /// <returns></returns>
        public async Task<int> GetList_Sojang_CountA()
        {
            using (var df = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                return await df.QuerySingleOrDefaultAsync<int>("SELECT Count(*) FROM Referral_Career WHERE Post = '관리' And Duty = '소장'");
            }

        }

        /// <summary>
        /// 관리소장 목록(현직)
        /// </summary>
        /// <param name="Post"></param>
        /// <returns></returns>
        public async Task<List<Referral_career_Entity>> GetList_Sojang_Seach(string Query)
        {
            using (var df = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                var lst = await df.QueryAsync<Referral_career_Entity>("SELECT Aid, Staff_Cd, User_ID, User_Name, Apt_Code, Apt_Name, Cor_Code, Post, Duty, Career_Start_Date, Career_End_Date, Division, PostDatge, Post_Code, CC_Code, ContractSort_Code, Etc FROM Referral_Career WHERE Post = '관리' And Duty = '소장' And Division = 'A' Order By Aid Desc", new { Query });
                return lst.ToList();
            }

        }

        /// <summary>
        /// 관리소장 목록(현직) 수
        /// </summary>
        /// <param name="Post"></param>
        /// <param name="Apt_Code"></param>
        /// <returns></returns>
        public async Task<int> GetList_Sojang_Search_Count(string Query)
        {
            using (var df = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                return await df.QuerySingleOrDefaultAsync<int>("SELECT Count(*) FROM Referral_Career WHERE Post = '관리' And Duty = '소장' And Division = 'A' And (User_ID Like '%" + Query + "%' or User_Name Like '%" + Query + "%' or Apt_Name Like '%" + Query + "%' or Apt_Code Like '%" + Query + "%'");
            }

        }


        


        /// <summary>
        /// 아이디로 배치정보 불러오기
        /// </summary>
        /// <param name="User_ID"></param>
        /// <returns></returns>
        public async Task<Referral_career_Entity> Details(string User_ID)
        {
            using (var df = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                return await df.QuerySingleOrDefaultAsync<Referral_career_Entity>("SELECT Aid, Staff_Cd, User_ID, User_Name, Apt_Code, Apt_Name, Cor_Code, Post, Duty, Career_Start_Date, Career_End_Date, Division, PostDatge, Post_Code, CC_Code, ContractSort_Code, Etc FROM Referral_Career WHERE User_ID = @User_ID And Division = 'A'", new { User_ID });
            }

        }

        /// <summary>
        /// 아이디로 배치정보 불러오기
        /// </summary>
        /// <param name="User_ID"></param>
        /// <returns></returns>
        public Referral_career_Entity Detailss(string User_ID)
        {
            using (var df = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                return df.QuerySingleOrDefault<Referral_career_Entity>("SELECT Aid, Staff_Cd, User_ID, User_Name, Apt_Code, Apt_Name, Cor_Code, Post, Duty, Career_Start_Date, Career_End_Date, Division, PostDatge, Post_Code, CC_Code, ContractSort_Code, Etc FROM Referral_Career WHERE User_ID = @User_ID And Division = 'A'", new { User_ID });
            }

        }

        /// <summary>
        /// 공동주택별 직원 목록
        /// </summary>
        /// <param name="Post"></param>
        /// <param name="Apt_Code"></param>
        /// <returns></returns>
        public async Task<List<Referral_career_Entity>> GetList_Apt_Staff(string Apt_Code)
        {
            using (var df = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                var lst = await df.QueryAsync<Referral_career_Entity>("SELECT Aid, Staff_Cd, User_ID, User_Name, Apt_Code, Apt_Name, Cor_Code, Post, Duty, Career_Start_Date, Career_End_Date, Division, PostDatge, Post_Code, CC_Code, ContractSort_Code, Etc FROM Referral_Career WHERE Apt_Code = @Apt_Code", new { Apt_Code });
                return lst.ToList();
            }

        }

        /// <summary>
        /// 공동주택별 직원 목록(페이징) 2021년
        /// </summary>
        public async Task<List<Referral_career_Entity>> GetList_Apt_Staff_Page(int Page, string Apt_Code)
        {
            using (var df = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                var lst = await df.QueryAsync<Referral_career_Entity>("Select Top 15 a.Aid, Staff_Cd, User_ID, User_Name, Apt_Code, Apt_Name, a.Cor_Code, Post, Duty, Career_Start_Date, Career_End_Date, Division, PostDatge, Post_Code, b.Cor_Name as Cor_Code, ContractSort_Code, Etc From Referral_Career as a Join Company as b on a.Cor_Code = b.Cor_Code Where a.Aid Not In (Select Top (15 * @Page) a.Aid From Referral_Career as a Join Company as b on a.Cor_Code = b.Cor_Code Where Apt_Code = @Apt_Code Order By a.Aid Desc) and Apt_Code = @Apt_Code  Order By a.Aid Desc", new { Page, Apt_Code });
                return lst.ToList();
            }
        }


        /// <summary>
        /// 공동주택별 직원 목록(현직)
        /// </summary>
        /// <param name="Post"></param>
        /// <param name="Apt_Code"></param>
        /// <returns></returns>
        public async Task<List<Referral_career_Entity>> GetList_Apt_Staff_be(string Apt_Code)
        {
            using (var df = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                var lst = await df.QueryAsync<Referral_career_Entity>("SELECT Aid, Staff_Cd, User_ID, User_Name, Apt_Code, Apt_Name, Cor_Code, Post, Duty, Career_Start_Date, Career_End_Date, Division, PostDatge, Post_Code, CC_Code, ContractSort_Code, Etc FROM Referral_Career WHERE Apt_Code = @Apt_Code And Division = 'A'", new { Apt_Code });
                return lst.ToList();
            }
        }

        /// <summary>
        /// 공동주택별 직원 목록(현직) 수
        /// </summary>
        /// <param name="Post"></param>
        /// <param name="Apt_Code"></param>
        /// <returns></returns>
        public async Task<int> GetList_Apt_Staff_beCount(string Apt_Code)
        {
            using (var df = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                return await df.QuerySingleOrDefaultAsync<int>("SELECT Count(*) FROM Referral_Career  WHERE Apt_Code = @Apt_Code And Division = 'A'", new { Apt_Code });
            }
        }

        /// <summary>
        /// 해당 공동주택에 부서 중복 제거하여 불러오기
        /// </summary>
        /// <param name="Apt_Code"></param>
        /// <returns></returns>
        public async Task<List<Referral_career_Entity>> GetList_Post(string Apt_Code)
        {
            using (var df = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                var lst = await df.QueryAsync<Referral_career_Entity>("Select Distinct Post From Referral_Career Where Apt_Code = @Apt_Code", new { Apt_Code });
                return lst.ToList();
            }
        }

        /// <summary>
        /// 해당 공동주택에 직책 중복 제거하여 불러오기
        /// </summary>
        /// <param name="Apt_Code"></param>
        /// <returns></returns>
        public async Task<List<Referral_career_Entity>> GetList_Duty(string Apt_Code)
        {
            using (var df = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                var lst = await df.QueryAsync<Referral_career_Entity>("Select Distinct Duty From Referral_Career Where Apt_Code = @Apt_Code", new { Apt_Code });
                return lst.ToList();
            }

        }

        /// <summary>
        /// 배치 및 퇴직 정보 입력
        /// </summary>
        /// <param name="rc"></param>
        /// <returns></returns>
        public async Task<Referral_career_Entity> Add_rc(Referral_career_Entity rc)
        {
            var sql = "Insert into Referral_Career (Staff_Cd, User_ID, User_Name, Apt_Code, Apt_Name, Cor_Code, CC_Code, ContractSort_Code, Post, Duty, Career_Start_Date, Division, Post_Code, Etc) Values (@Staff_Cd, @User_ID, @User_Name, @Apt_Code, @Apt_Name, @Cor_Code, @CC_Code, @ContractSort_Code, @Post, @Duty, @Career_Start_Date, 'A', @Post_Code, @Etc)";
            using (var df = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                await df.ExecuteAsync(sql, rc);
                return rc;
            }

        }

        /// <summary>
        /// 배치 정보 수정
        /// </summary>
        /// <param name="rc"></param>
        /// <returns></returns>
        public async Task<Referral_career_Entity> Edit_rc(Referral_career_Entity rc)
        {
            var sql = "Update Referral_Career Set Apt_Code = @Apt_Code, Apt_Name = @Apt_Name, Cor_Code = @Cor_Code, CC_Code = @CC_Code, ContractSort_Code = @ContractSort_Code, Post = @Post, Duty = @Duty, Career_Start_Date = @Career_Start_Date, Career_End_Date = @Career_End_Date, Post_Code = @Post_Code, Division = @Division, Etc = @Etc Where Aid = @Aid";
            using (var df = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                await df.ExecuteAsync(sql, rc);
                return rc;
            }

        }

        /// <summary>
        /// 배치 정보 수정(퇴직 제외)
        /// </summary>
        /// <param name="rc"></param>
        /// <returns></returns>
        public async Task<Referral_career_Entity> Edit_rc_A(Referral_career_Entity rc)
        {
            var sql = "Update Referral_Career Set Apt_Code = @Apt_Code, Apt_Name = @Apt_Name, Cor_Code = @Cor_Code, CC_Code = @CC_Code, ContractSort_Code = @ContractSort_Code, Post = @Post, Duty = @Duty, Career_Start_Date = @Career_Start_Date, Post_Code = @Post_Code, Division = @Division Where Aid = @Aid";
            using (var df = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                await df.ExecuteAsync(sql, rc);
                return rc;
            }

        }

        /// <summary>
        /// 퇴직 처리
        /// </summary>
        /// <param name="rc"></param>
        /// <returns></returns>
        public async Task Edit_Resign(DateTime Career_End_Date, string Division, int Aid)
        {
            var sql = "Update Referral_Career Set Career_End_Date = @Career_End_Date, Division = @Division Where Aid = @Aid";
            using (var df = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                await df.ExecuteAsync(sql, new { Career_End_Date, Division,Aid });

            }

        }

        /// <summary>
        /// 퇴직 처리(a)
        /// </summary>
        /// <param name="rc"></param>
        /// <returns></returns>
        public async Task Resign(DateTime Career_End_Date, string Division, string Etc, int Aid)
        {
            var sql = "Update Referral_Career Set Career_End_Date = @Career_End_Date, Division = @Division, Etc = @Etc Where Aid = @Aid";
            using (var df = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                await df.ExecuteAsync(sql, new { Career_End_Date, Division, Etc, Aid });

            }

        }

        /// <summary>
        /// 배치 및 퇴직 정보(소장) 본사
        /// </summary>
        /// <param name="Feild"></param>
        /// <param name="Query"></param>
        /// <returns></returns>
        public async Task<List<Referral_career_Entity>> GetList(int Page, string Feild, string Query)
        {
            using (var df = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                var lst = await df.QueryAsync<Referral_career_Entity>("Select * From Referral_Career Where " + Feild + " = @Query And Post_Code = 'B' Order By Aid Desc", new { Page, Feild, Query });
                return lst.ToList();
            }

        }

        /// <summary>
        /// 배치 및 퇴직 정보 수
        /// </summary>
        /// <param name="Feild"></param>
        /// <param name="Query"></param>
        /// <returns></returns>
        public async Task<int> GetCount(string Feild, string Query)
        {
            using (var df = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                return await df.QuerySingleOrDefaultAsync<int>("", new { Feild, Query });
            }

        }

        /// <summary>
        /// 부서 이름 찾기 
        /// </summary>
        /// <param name="User_ID"></param>
        /// <returns></returns>
        public async Task<string> PostName(string User_ID)
        {
            using (var df = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                return await df.QuerySingleOrDefaultAsync<string>("Select top 1 isnull (Post, '미배치') From Referral_Career Where User_ID = @User_ID And Career_End_Date IS NULL Order By Aid Desc", new { User_ID });
            }

        }

        /// <summary>
        /// 직책 이름 찾기 
        /// </summary>
        /// <param name="User_ID"></param>
        /// <returns></returns>
        public async Task<string> DutyName(string User_ID)
        {
            using (var df = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                return await df.QuerySingleOrDefaultAsync<string>("Select top 1 isnull (Duty, '미배치') From Referral_Career Where User_ID = @User_ID And Career_End_Date IS NULL Order By Aid Desc", new { User_ID });
            }

        }

        /// <summary>
        /// 이름 찾기 
        /// </summary>
        public async Task<string> UserName(string User_ID)
        {
            using (var df = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                return await df.QuerySingleOrDefaultAsync<string>("Select top 1 isnull (User_Name, '미배치') From Referral_Career Where User_ID = @User_ID And Career_End_Date IS NULL Order By Aid Desc", new { User_ID });
            }
        }

        /// <summary>
        /// 단지명 이름 찾기 
        /// </summary>
        /// <param name="User_ID"></param>
        /// <returns></returns>
        public async Task<string> AptName(string User_ID)
        {
            using (var df = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                return await df.QuerySingleOrDefaultAsync<string>("Select top 1 isnull (Apt_Name, '미배치') From Referral_Career Where User_ID = @User_ID And Career_End_Date IS NULL Order By Aid Desc", new { User_ID });
            }

        }

        /// <summary>
        /// 직원 배치 정보 목록
        /// </summary>
        /// <param name="User_ID"></param>
        /// <returns></returns>
        public async Task<List<Referral_career_Entity>> GetList_Code(string User_ID)
        {
            using (var df = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                var lst = await df.QueryAsync<Referral_career_Entity>("Select * From Referral_Career Where User_ID = @User_ID Order By Aid Desc", new { User_ID });
                return lst.ToList();
            }

        }
        //Select Top 20 * From Referral_Career Where " + Feild + " = @Query

        /// <summary>
        /// 배치 및 퇴직 정보 수 구하기
        /// </summary>
        public async Task<int> Getcount(string Feild, string Query)
        {
            using (var df = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                return await df.QuerySingleOrDefaultAsync<int>("Select Count(*) From Referral_Career Where " + Feild + " = @Query And Post_Code = 'B'", new { Feild, Query });
            }

        }

        /// <summary>
        /// 배치정보 찾기
        /// </summary>
        public async Task<List<Referral_career_Entity>> _Career_Search(int Page, string Feild, string Query)
        {
            using (var df = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                var lst = await df.QueryAsync<Referral_career_Entity>("Select Top 15 * From Referral_Career Where Aid Not In(Select Top(15 * @Page) Aid From Referral_Career Where " + Feild + " Like '%" + @Query + "%' And Post_Code = 'B' Order By Aid Desc) and " + Feild + " Like '%" + @Query + "%' And Post_Code = 'B' Order By Aid Desc", new { Page, Feild, Query });
                return lst.ToList();
            }
        }

        /// <summary>
        /// 배치 및 퇴직 정보 수 구하기(new)
        /// </summary>
        public async Task<int> GetListStaffCarrerSearchCount(string Apt_Code, string Post_Code, string Division)
        {
            using (var df = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                return await df.QuerySingleOrDefaultAsync<int>("Select Count(*) From Referral_Career as a Join Staff as b on a.User_ID = b.User_ID Join Staff_Sub as c On a.User_ID = c.User_ID Where a.Apt_Code = @Apt_Code And a.Post_Code = @Post_Code And a.Division = @Division", new { Apt_Code, Post_Code, Division });
            }

        }

        /// <summary>
        /// 배치정보 찾기(new)
        /// </summary>
        public async Task<List<Staff_Career_Entity>> GetListStaffCarrerSearch(int Page, string Apt_Code, string Post_Code, string Division)
        {
            using (var df = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                var lst = await df.QueryAsync<Staff_Career_Entity>("Select Top 15 a.Aid, a.Apt_Code, a.Apt_Name, a.Career_End_Date, a.Career_Start_Date, a.CC_Code, a.ContractSort_Code, a.Cor_Code, a.Division, a.Duty, a.Etc, a.Post, a.Post_Code, a.PostDatge, a.Staff_Cd, a.User_ID, a.User_Name, b.CommentsCount, b.Del, b.FileUpCount, b.Intro, b.JoinDate, b.LevelCount, b.M_Apt_Code, b.M_Apt_Code, b.Old_UserID, b.ReadCount, b.RestAdress, b.Scn, b.Scn_Code, b.Sido, b.SiGunGu, b.User_ID, b.User_Name, b.VisitCount, b.WriteCount, c.d_division, c.Email, c.End_Date, c.Etc, c.Mobile_Number, c.Seal, c.st_Adress_Rest, c.TelePhone From Referral_Career as a Join Staff as b on a.User_ID = b.User_ID Join Staff_Sub as c On a.User_ID = c.User_ID Where a.Aid Not In(Select Top(15 * @Page) a.Aid From Referral_Career as a Join Staff as b on a.User_ID = b.User_ID Join Staff_Sub as c On a.User_ID = c.User_ID Where a.Apt_Code = @Apt_Code And a.Post_Code = @Post_Code And a.Division = @Division Order By a.Aid Desc) and a.Apt_Code = @Apt_Code And a.Post_Code = @Post_Code And a.Division = @Division Order By a.Aid Desc", new { Page, Apt_Code, Post_Code, Division });
                return lst.ToList();
            }
        }

        /// <summary>
        /// 지나온 날짜 계산
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public int Date_scom(string start, string end)
        {
            using (var df = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                return df.QuerySingleOrDefault<int>("Select DATEDIFF(DD, @start, @end)", new { start, end });
            }

        }

        /// <summary>
        /// 지나온 날짜 계산
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public int Date_scomp(string start, string end)
        {
            using (var df = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                return df.QuerySingleOrDefault<int>("Select DATEDIFF(DD, @start, @end)", new { start, end });
            }

        }

        /// <summary>
        /// 근무중인 배치정보
        /// </summary>
        /// <param name="User_ID"></param>
        /// <returns></returns>
        public async Task<int> be(string User_ID)
        {
            using (var df = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                return await df.QuerySingleOrDefaultAsync<int>("Select Count(*) From Referral_Career Where User_ID = @User_ID And Division = 'A'", new { User_ID });
            }

        }

        /// <summary>
        /// 배치정보 존재 여부
        /// </summary>
        /// <param name="User_ID"></param>
        /// <returns></returns>
        public async Task<int> being(string User_ID)
        {
            using (var df = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                return await df.QuerySingleOrDefaultAsync<int>("Select Count(*) From Referral_Career Where User_ID = @User_ID", new { User_ID });
            }

        }

        /// <summary>
        /// 배치정보 존재 여부
        /// </summary>
        /// <param name="User_ID"></param>
        /// <returns></returns>
        public async Task<int> be_apt(string User_ID, string Apt_Code)
        {
            using (var df = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                return await df.QuerySingleOrDefaultAsync<int>("Select Count(*) From Referral_Career Where User_ID = @User_ID And Apt_Code = @Apt_Code And Division = 'A'", new { User_ID, Apt_Code });
            }

        }

        /// <summary>
        /// 재직기간 중복 체크
        /// </summary>
        /// <param name="User_ID"></param>
        /// <param name="Career_Start_Date">배치일</param>
        /// <returns></returns>
        public async Task<int> be_not(string User_ID, string Career_Start_Date)
        {
            using (var df = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                return await df.QuerySingleOrDefaultAsync<int>("Select Count(*) From Referral_Career Where User_ID = @User_ID And Career_End_Date is not null And Career_End_Date <= @Career_Start_Date And Division = 'A'", new { User_ID, Career_Start_Date });
            }

        }

        /// <summary>
        /// 배치정보에서 상세정보 불러오기(현재 배치정보)
        /// </summary>
        /// <param name="User_ID"></param>
        /// <returns></returns>
        public async Task<Referral_career_Entity> Detail(string User_ID)
        {
            using (var df = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                return await df.QuerySingleOrDefaultAsync<Referral_career_Entity>("Select Top 1 * From Referral_Career Where User_ID = @User_ID And Division = 'A'", new { User_ID });
            }


            //_Conn.ctx_c.Query<Referral_career_Entity>("Select Top 1 * From Referral_Career Where User_ID = @User_ID And (Career_End_Date is null or Career_End_Date = '')", new { User_ID });
        }

        /// <summary>
        /// 배치정보에서 상세정보 불러오기
        /// </summary>
        /// <param name="User_ID"></param>
        /// <returns></returns>
        public async Task<Referral_career_Entity> Detail_A(string User_ID, string Aid)
        {
            using (var df = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                return await df.QuerySingleOrDefaultAsync<Referral_career_Entity>("Select * From Top 1 Referral_Career Where User_ID = @User_ID Aid = @Aid And Division = 'A'", new { User_ID, Aid });
            }

        }



        /// <summary>
        /// 입력 직원 수
        /// </summary>
        /// <param name="Apt_Code"></param>
        /// <returns></returns>
        public async Task<int> Count_apt_staff(string Apt_Code)
        {
            using (var df = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                return await df.QuerySingleOrDefaultAsync<int>("Select Count(*) From Referral_Career Where Apt_Code = @Apt_Code", new { Apt_Code });
            }

        }

        /// <summary>
        /// 삭제
        /// </summary>
        /// <param name="Aid"></param>
        public async Task delete(string Aid)
        {
            using (var df = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                await df.ExecuteAsync("Delete Referral_Career Where Aid = @Aid", new { Aid });
            }

        }

        /// <summary>
        /// 해당 공동주택 직원 목록
        /// </summary>
        /// <param name="Apt_Code"></param>
        /// <param name="Division"></param>
        /// <returns></returns>
        public async Task<List<Referral_career_Entity>> Getlist_apt(string Apt_Code, string Division, string Post_Code)
        {
            using (var df = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                var lst = await df.QueryAsync<Referral_career_Entity>("Select * From Referral_Career Where Apt_Code = @Apt_Code And Division = @Division And Post_Code = @Post_Code", new { Apt_Code, Division, Post_Code });
                return lst.ToList();
            }

        }

        /// <summary>
        /// 이름으로 배치정보 찾기
        /// </summary>
        /// <param name="Feild"></param>
        /// <param name="Query"></param>
        /// <returns></returns>
        public async Task<List<Referral_career_Entity>> _Career_Name_Search(string Feild, string Query)
        {
            using (var df = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                var lst = await df.QueryAsync<Referral_career_Entity>("Select top 15 * From Referral_Career Where " + Feild + " Like '%" + @Query + "%' Order By Aid Desc", new { Feild, Query });
                return lst.ToList();
            }
        }

        /// <summary>
        /// 이름으로 배치정보 찾기(관리자)
        /// </summary>
        /// <param name="Feild"></param>
        /// <param name="Query"></param>
        /// <returns></returns>
        public async Task<List<Referral_career_Entity>> _Career_Feild_Search(string Feild, string Query)
        {
            using (var df = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                var lst = await df.QueryAsync<Referral_career_Entity>("Select top 15 * From Referral_Career Where Post = '관리' And Duty = '소장' And " + Feild + " Like '%" + @Query + "%' Order By Aid Desc", new { Feild, Query });
                return lst.ToList();
            }
        }

        /// <summary>
        /// 이름r과 아파트로 배치정보 찾기
        /// </summary>
        /// <param name="Feild"></param>
        /// <param name="Query"></param>
        /// <returns></returns>
        public async Task<List<Referral_career_Entity>> _Career_Name_Apt_Search(string Feild, string Query, string Apt_Code)
        {
            using (var df = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                var lst = await df.QueryAsync<Referral_career_Entity>("Select * From Referral_Career Where " + Feild + " Like '%" + @Query + "%' And Apt_Code = @Apt_Code Order By Aid Desc", new { Feild, Query, Apt_Code });
                return lst.ToList();
            }

        }

        /// <summary>
        /// 직원구분과 아파트로 배치정보 찾기
        /// </summary>
        /// <param name="Post_Code"></param>
        /// <param name="Apt_Code"></param>
        /// <returns></returns>
        public async Task<List<Referral_career_Entity>> _Career_PostCode_Apt_Search(string Post_Code, string Post_CodeA, string Apt_Code)
        {
            using (var df = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                var lst = await df.QueryAsync<Referral_career_Entity>("Select * From Referral_Career Where  (Post_Code = @Post_Code or Post_Code = @Post_CodeA ) And Apt_Code = @Apt_Code And Division = 'A' Order By Aid Desc", new { Post_Code, Post_CodeA, Apt_Code });
                return lst.ToList();
            }

        }

        /// <summary>
        /// 해당 공동주택의 부서와 직책으로 찾기(가장 최근)
        /// </summary>
        /// <param name="AptCode"></param>
        /// <param name="Post"></param>
        /// <param name="Duty"></param>
        /// <returns></returns>
        public async Task<string> User_Code_Be(string AptCode, string Post, string Duty)
        {
            using (var df = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                return await df.QuerySingleOrDefaultAsync<string>("Select Top 1 User_ID From Referral_Career Where Apt_Code = @AptCode And Post = @Post And Duty = @Duty Order by Aid Desc", new { AptCode, Post, Duty });
            }

        }

        public string User_Code_Bes(string AptCode, string Post, string Duty, string User_Code)
        {
            using (var df = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                return df.QuerySingleOrDefault<string>("Select Top 1 User_ID From Referral_Career Where Apt_Code = @AptCode And Post = @Post And Duty = @Duty And User_ID = @User_Code Order by Aid Desc", new { AptCode, Post, Duty, User_Code });
            }

        }

        /// <summary>
        /// 삭제
        /// </summary>
        /// <param name="User_ID"></param>
        public async Task Remove(string User_ID)
        {
            using (var df = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                await df.ExecuteAsync("Update Referral_Career Set Division = 'B' Where User_ID = @User_ID", new { User_ID });
            }
        }

        /// <summary>
        /// 해당 공동주택 근무자 목록(Division = A면 근무자, B면 비근무자) 
        /// </summary>
        public async Task<List<Staff_Career_Entity>> StaffCareer_Join(string Apt_Code, string Division)
        {
            using (var df = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                var lst = await df.QueryAsync<Staff_Career_Entity>("Select * From Referral_Career as a Join Staff as b on a.User_ID = b.User_ID Join Staff_Sub as c on b.User_ID = c.User_ID where a.Apt_Code = @Apt_Code and a.Division = @Divsion", new { Apt_Code, Division });
                return lst.ToList();
            }
        }

        /// <summary>
        /// 해당 공동주택 근무자 목록 조인
        /// </summary>
        public async Task<List<Staff_Career_Entity>> Staff_Career_Join(int Page, string Apt_Code)
        {
            using (var df = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                var lst = await df.QueryAsync<Staff_Career_Entity>("Select Top 15 a.Aid, a.Staff_Cd, a.User_ID, a.User_Name, a.Apt_Code, a.Apt_Name, a.Cor_Code, a.Post, a.Duty, a.Career_Start_Date, a.Career_End_Date, a.Division, a.PostDatge, a.Post_Code, a.CC_Code, a.ContractSort_Code, b.CommentsCount, b.Del, b.FileUpCount, b.Intro, b.JoinDate, b.LevelCount, b.ReadCount, b.Scn, b.Scn_Code, b.VisitCount, b.WriteCount, c.d_division, c.Email, c.End_Date, c.Etc, c.Mobile_Number, c.Seal, c.st_Adress_Rest, c.st_GunGu, c.st_Sido, c.Start_Date, c.TelePhone from Referral_Career as a Join Staff as b on a.User_ID = b.User_ID Join Staff_Sub as c on a.User_ID = c.User_ID  Where a.Aid Not In(Select Top(15 * " + Page + ") a.Aid From Referral_Career as a Join Staff as b on a.User_ID = b.User_ID Join Staff_Sub as c on a.User_ID = c.User_ID Where a.Apt_Code = @Apt_Code Order By a.Division Asc, a.Aid Desc) And a.Apt_Code = @Apt_Code Order By a.Division Asc, a.Aid Desc", new { Page, Apt_Code });
                return lst.ToList();
            }
        }

        /// <summary>
        /// 해당 공동주택 근무자 목록 조인(수)
        /// </summary>
        public async Task<int> Staff_Career_Join_Count(string Apt_Code)
        {
            using (var df = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                return await df.QuerySingleOrDefaultAsync<int>("Select Count(*) from Referral_Career as a Join Staff as b on a.User_ID = b.User_ID Join Staff_Sub as c on a.User_ID = c.User_ID Where a.Apt_Code = @Apt_Code", new { Apt_Code });
            }
        }

        /// <summary>
        /// 해당 공동주택 근무자 상세정보
        /// </summary>
        public async Task<Staff_Career_Entity> Details_Staff_Career_Join(string User_Code)
        {
            using (var df = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                return await df.QuerySingleOrDefaultAsync<Staff_Career_Entity>("Select a.Aid, a.Staff_Cd, a.User_ID, a.User_Name, a.Apt_Code, a.Apt_Name, a.Cor_Code, a.Post, a.Duty, a.Career_Start_Date, a.Career_End_Date, a.Division, a.PostDatge, a.Post_Code, a.CC_Code, a.ContractSort_Code, b.CommentsCount, b.Del, b.FileUpCount, b.Intro, b.JoinDate, b.LevelCount, b.ReadCount, b.Scn, b.Scn_Code, b.VisitCount, b.WriteCount, c.d_division, c.Email, c.End_Date, c.Etc, c.Mobile_Number, c.Seal, c.st_Adress_Rest, c.st_GunGu, c.st_Sido, c.Start_Date, c.TelePhone from Referral_Career as a Join Staff as b on a.User_ID = b.User_ID Join Staff_Sub as c on a.User_ID = c.User_ID  Where a.User_ID = @User_Code", new { User_Code });
            }
        }
    }


    /// <summary>
    /// 직원정보 및 배치정보 결합 클래스
    /// </summary>
    public class Staff_Career_Lib : IStaff_Career_Lib
    {
        private readonly IConfiguration _db;
        public Staff_Career_Lib(IConfiguration configuration)
        {
            this._db = configuration;
        }

        /// <summary>
        /// 해당 공동주택 근무자 목록(Division = A면 근무자, B면 비근무자) 
        /// </summary>
        /// <param name="Apt_Code"></param>
        /// <param name="Division"></param>
        /// <returns></returns>
        public async Task<List<Staff_Career_Entity>> StaffCareer_Join(string Apt_Code, string Division)
        {
            using var df = new SqlConnection(_db.GetConnectionString("sw_togather"));
            var lst = await df.QueryAsync<Staff_Career_Entity>("Select * From Referral_Career as a Join Staff as b on a.User_ID = b.User_ID Join Staff_Sub as c on b.User_ID = c.User_ID where a.Apt_Code = @Apt_Code and a.Division = @Divsion", new { Apt_Code, Division });
            return lst.ToList();
        }

        /// <summary>
        /// 직원아이디로 배치 및 회원 정보 불러오기
        /// </summary>
        public async Task<Staff_Career_Entity> Details_Staff_Career(string UserID)
        {
            using var df = new SqlConnection(_db.GetConnectionString("sw_togather"));
            return await df.QuerySingleOrDefaultAsync<Staff_Career_Entity>("Select Top 1 a.Aid, a.Apt_Code, a.Apt_Name, a.Career_End_Date, a.Career_Start_Date, a.Cor_Code, a.Division, a.Duty, a.Post, a.Post_Code, a.User_ID, a.User_Name, b.Apt_Adress_Sido, b.Apt_Adress_Gun, b.Apt_Adress_Rest, b.CorporateResistration_Num, b.Apt_Form, b.Dong_Num, b.HouseHold_Num, c.LevelCount, c.Scn, c.JoinDate, d.d_division, d.Email, d.Mobile_Number, d.st_Adress_Rest, d.st_GunGu, d.st_Sido From Referral_Career as a Join Apt as b on a.Apt_Code = b.Apt_Code Join Staff as c on a.User_ID = c.User_ID Join Staff_Sub as d on a.User_ID = d.User_ID Where a.User_ID = @UserID Order By Aid Desc", new { UserID });
        }

        /// <summary>
        /// 해당 공동주택 근무자 목록(Division = A면 근무자, B면 비근무자) 
        /// </summary>
        public async Task<List<Staff_Career_Entity>> StaffCareer_Join_Users(string User_Code)
        {
            using var df = new SqlConnection(_db.GetConnectionString("sw_togather"));
            var lst = await df.QueryAsync<Staff_Career_Entity>("Select a.Aid, a.Apt_Code, a.Apt_Name, a.Career_End_Date, a.Career_Start_Date, a.Cor_Code, a.Division, a.Duty, a.Post, a.Post_Code, a.User_ID, a.User_Name, b.Apt_Adress_Sido, b.Apt_Adress_Gun, b.Apt_Adress_Rest, b.CorporateResistration_Num, b.Apt_Form, b.Dong_Num, b.HouseHold_Num, c.LevelCount, c.Scn, c.JoinDate, d.d_division, d.Email, d.Mobile_Number, d.st_Adress_Rest, d.st_GunGu, d.st_Sido, a.PostDatge, d.st_Adress_Rest, d.st_GunGu, d.st_Sido, a.PostDatge, b.Apt_Adress_Gun, b.Apt_Adress_Rest, b.Apt_Adress_Sido, b.CorporateResistration_Num, b.AcceptancedOfWork_Date, b.Dong_Num, b.HouseHold_Num From Referral_Career as a Join Apt as b on a.Apt_Code = b.Apt_Code Join Staff as c on a.User_ID = c.User_ID Join Staff_Sub as d on a.User_ID = d.User_ID where a.User_ID = @User_Code Order by Aid Desc", new { User_Code });
            return lst.ToList();
        }
    }


    /// <summary>
    /// 작업자 메세드
    /// </summary>
    public class Service_Worker_Lib : IService_Worker_Lib
    {
        private readonly IConfiguration _db;
        public Service_Worker_Lib(IConfiguration configuration)
        {
            this._db = configuration;
        }

        /// <summary>
        /// 작업자 입력
        /// </summary>
        /// <param name="_Entity"></param>
        /// <returns></returns>
        public async Task<Service_Worker_Entity> Add(Service_Worker_Entity _Entity)
        {
            var sql = "Insert Into Service_Worker (Worker_Code, AptCode, Service_Code, Sub_Code, [Group], Post, Duty, Staff_Code, Staff_Name, WorkDate, Del, PostIP) Values (@Worker_Code, @AptCode, @Service_Code, @Sub_Code, @Group, @Post, @Duty, @Staff_Code, @Staff_Name, @WorkDate, 'A', @PostIP)";
            using (var df = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                await df.ExecuteAsync(sql, _Entity);
                return _Entity;
            }
        }

        /// <summary>
        /// 해당 업무에 작업자 목록
        /// </summary>
        /// <param name="AptCode"></param>
        /// <param name="Service_Code"></param>
        /// <returns></returns>
        public async Task<List<Service_Worker_Entity>> GetList(string AptCode, string Service_Code)
        {
            using (var df = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                var lst = await df.QueryAsync<Service_Worker_Entity>("Select  Num, Worker_Code, AptCode, Service_Code, Sub_Code, [Group], Post, Duty, Staff_Code, Staff_Name, WorkDate, Del, PostDate From Service_Worker Where AptCode = @AptCode And Service_Code = @Service_Code And Del = 'A'", new { AptCode, Service_Code });
                return lst.ToList();
            }
        }

        /// <summary>
        /// 작업자 삭제
        /// </summary>
        /// <param name="Num"></param>
        public async Task Delete(string Num)
        {
            using (var df = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                await df.ExecuteAsync("Update Service_Worker Set Del = 'B' Where Num = @Num", new { Num });
            }

        }

        /// <summary>
        /// 마지막 일련번호
        /// </summary>
        /// <returns></returns>
        public async Task<int> Worker_Count()
        {
            using (var df = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                return await df.QuerySingleOrDefaultAsync<int>("Select Top 1 Num From Service_Worker Order By Num Desc");
            }

        }
    }


    /// <summary>
    /// 홈페이지 방문 정보
    /// </summary>
    public class Present_Lib : IPresent_Lib
    {
        private readonly IConfiguration _db;
        public Present_Lib(IConfiguration configuration)
        {
            this._db = configuration;
        }

        /// <summary>
        /// 방문자 정보 입력
        /// </summary>
        /// <param name="pe"></param>
        public async Task Add(Present_Entity pe)
        {
            var sql = "Insert into Present (UserName, UserID, Title, DayDate, MonthDate, YearDate, PostIP, AptName, AptCode) Values (@UserName, @UserID, @Title, @DayDate, @MonthDate, @YearDate, @PostIP, @AptName, @AptCode)";
            using (var df = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                await df.ExecuteAsync(sql, pe);
            }
        }

        /// <summary>
        /// 현 재일 접속 정보 목록
        /// </summary>
        /// <param name="YearDate"></param>
        /// <param name="MonthDate"></param>
        /// <param name="DayDate"></param>
        /// <param name="User_Code"></param>
        /// <returns></returns>
        public async Task<List<Present_Entity>> GetLists(string YearDate, string MonthDate, string DayDate, string User_Code)
        {
            using (var df = new SqlConnection(_db.GetConnectionString("sw_togather")))
            {
                var lst = await df.QueryAsync<Present_Entity>("Select PresentID, UserName, UserID, Title, LevelCount, DayDate, MonthDate, YearDate, PostDate, PostIP, AptName, AptCode From Present Where YearDate = @YearDAte And MonthDate = @MonthDate And DayDate = @DayDate And UserID = @User_Code Order by PresentID Desc", new { YearDate, MonthDate, DayDate, User_Code });
                return lst.ToList();
            }

        }
    }
}
