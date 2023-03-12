using System;
using System.ComponentModel.DataAnnotations;

namespace Erp_Apt_Staff
{
    /// <summary>
    /// 직원 기본 정보 엔터티
    /// </summary>
    public class Staff_Entity
    {
        /// <summary>
        /// 식별코드
        /// </summary>
        public int Aid { get; set; }

        /// <summary>
        /// 아이디
        /// </summary>
        public string User_ID { get; set; }
        /// <summary>
        /// 직원코드
        /// </summary>
        public string Staff_Cd { get; set; }
        /// <summary>
        /// 구 아이디
        /// </summary>
        public string Old_UserID { get; set; }
        /// <summary>
        /// 이름
        /// </summary>
        public string User_Name { get; set; }

        /// <summary>
        /// 시도
        /// </summary>
        public string Sido { get; set; }
        /// <summary>
        /// 시군구
        /// </summary>
        public string SiGunGu { get; set; }
        /// <summary>
        /// 나머지 주소
        /// </summary>
        public string RestAdress { get; set; }
        /// <summary>
        /// 출생일
        /// </summary>
        public string Scn { get; set; }
        /// <summary>
        /// 암호
        /// </summary>
        public string Password_sw { get; set; }
        /// <summary>
        /// 출생코드
        /// </summary>
        public string Scn_Code { get; set; }

        /// <summary>
        /// 직원 소개
        /// </summary>
        public string Intro { get; set; }

        /// <summary>
        /// 등급
        /// </summary>
        public int LevelCount { get; set; }
        /// <summary>
        /// 방문 수
        /// </summary>
        public int VisitCount { get; set; }
        /// <summary>
        /// 글 수
        /// </summary>
        public int WriteCount { get; set; }
        /// <summary>
        /// 읽은 수
        /// </summary>
        public int ReadCount { get; set; }
        /// <summary>
        /// 댓글 수
        /// </summary>
        public int CommentsCount { get; set; }
        /// <summary>
        /// 파일 첨부 수
        /// </summary>
        public int FileUpCount { get; set; }
        /// <summary>
        /// 가입일
        /// </summary>
        public DateTime JoinDate { get; set; }
        /// <summary>
        /// 공동주택 식별코드
        /// </summary>
        public string M_Apt_Code { get; set; }
        /// <summary>
        /// 삭제 여부
        /// </summary>
        public string Del { get; set; }
    }

    

    /// <summary>
    /// 직원 상세정보 엔터티
    /// </summary>
    public class Staff_Sub_Entity
    {
        /// <summary>
        /// 식별번호
        /// </summary>
        public int Aid { get; set; }
        /// <summary>
        /// 휴대폰번호
        /// </summary>
        public string Mobile_Number { get; set; }
        /// <summary>
        /// 전화번호
        /// </summary>
        public string Telephone { get; set; }
        /// <summary>
        /// 이메일
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// 식별코드
        /// </summary>
        public string Staff_Cd { get; set; }
        /// <summary>
        /// 아이디
        /// </summary>
        public string User_ID { get; set; }

        

        /// <summary>
        /// 등급
        /// </summary>
        public int levelcount { get; set; }
        /// <summary>
        /// 직원 명
        /// </summary>
        public string Staff_Name { get; set; }
        /// <summary>
        /// 직원 상세 식별코드
        /// </summary>
        public string Staff_Sub_Cd { get; set; }
        /// <summary>
        /// 시도
        /// </summary>
        public string st_Sido { get; set; }
        /// <summary>
        /// 시군
        /// </summary>
        public string st_GunGu { get; set; }
        /// <summary>
        /// 나머지 주소
        /// </summary>
        public string st_Adress_Rest { get; set; }
        /// <summary>
        /// 기타 설명
        /// </summary>
        public string Etc { get; set; }
        /// <summary>
        /// 시작일
        /// </summary>
        public DateTime Start_Date { get; set; }
        /// <summary>
        /// 종료일
        /// </summary>
        public DateTime End_Date { get; set; }
        /// <summary>
        /// 구분
        /// </summary>
        public string d_division { get; set; }
        /// <summary>
        /// 공동주택 식별코드
        /// </summary>
        public string M_Apt_Code { get; set; }
        /// <summary>
        /// 작성일
        /// </summary>
        public DateTime PostDate { get; set; }

    }

    /// <summary>
    /// 직원 기본 및 상세 정보 결합 엔터티
    /// </summary>
    public class Staff_StaffSub_Entity
    {
        /// <summary>
        /// 식별코드
        /// </summary>
        public int Aid { get; set; }
        /// <summary>
        /// 아이디
        /// </summary>
        public string User_ID { get; set; }
        /// <summary>
        /// 직원코드
        /// </summary>
        public string Staff_Cd { get; set; }
        /// <summary>
        /// 구 아이디
        /// </summary>
        public string Old_UserID { get; set; }
        /// <summary>
        /// 이름
        /// </summary>
        public string User_Name { get; set; }
        /// <summary>
        /// 시도
        /// </summary>
        public string Sido { get; set; }
        /// <summary>
        /// 시군구
        /// </summary>
        public string SiGunGu { get; set; }
        /// <summary>
        /// 나머지 주소
        /// </summary>
        public string RestAdress { get; set; }
        /// <summary>
        /// 출생일
        /// </summary>
        public string Scn { get; set; }
        /// <summary>
        /// 암호
        /// </summary>
        public string Password_sw { get; set; }
        /// <summary>
        /// 출생코드
        /// </summary>
        public string Scn_Code { get; set; }
        /// <summary>
        /// 등급
        /// </summary>
        public int LevelCount { get; set; }
        /// <summary>
        /// 방문 수
        /// </summary>
        public int VisitCount { get; set; }
        /// <summary>
        /// 글 수
        /// </summary>
        public int WriteCount { get; set; }
        /// <summary>
        /// 읽은 수
        /// </summary>
        public int ReadCount { get; set; }
        /// <summary>
        /// 댓글 수
        /// </summary>
        public int CommentsCount { get; set; }
        /// <summary>
        /// 파일 첨부 수
        /// </summary>
        public int FileUpCount { get; set; }
        /// <summary>
        /// 가입일
        /// </summary>
        public DateTime JoinDate { get; set; }
        /// <summary>
        /// 삭제 여부
        /// </summary>
        public string Del { get; set; }

        /// <summary>
        /// 휴대폰번호
        /// </summary>
        public string Mobile_Number { get; set; }
        /// <summary>
        /// 전화번호
        /// </summary>
        public string Telephone { get; set; }
        /// <summary>
        /// 이메일
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 서버 식별코드
        /// </summary>
        public int SAid { get; set; }

        /// <summary>
        /// 등급
        /// </summary>
        public int LeveCount { get; set; }
        /// <summary>
        /// 직원 명
        /// </summary>
        public string Staff_Name { get; set; }
        /// <summary>
        /// 직원 상세 식별코드
        /// </summary>
        public string Staff_Sub_Cd { get; set; }
        /// <summary>
        /// 시도
        /// </summary>
        public string st_Sido { get; set; }
        /// <summary>
        /// 시군
        /// </summary>
        public string st_GunGu { get; set; }
        /// <summary>
        /// 나머지 주소
        /// </summary>
        public string st_Adress_Rest { get; set; }
        /// <summary>
        /// 기타 설명
        /// </summary>
        public string Etc { get; set; }
        /// <summary>
        /// 시작일
        /// </summary>
        public DateTime Start_Date { get; set; }
        /// <summary>
        /// 종료일
        /// </summary>
        public DateTime End_Date { get; set; }
        /// <summary>
        /// 구분
        /// </summary>
        public string d_division { get; set; }
        /// <summary>
        /// 공동주택 식별코드
        /// </summary>
        public string M_Apt_Code { get; set; }
        /// <summary>
        /// 작성일
        /// </summary>
        public DateTime PostDate { get; set; }

    }

    /// <summary>
    /// 배치정보 엔터티
    /// </summary>
    public class Referral_career_Entity
    {
        /// <summary>
        /// 식별번호
        /// </summary>
        public int Aid { get; set; }
        /// <summary>
        /// 식별코드
        /// </summary>
        public string Staff_Cd { get; set; }
        /// <summary>
        /// 유저명
        /// </summary>
        public string User_Name { get; set; }
        /// <summary>
        /// 구분
        /// </summary>
        public string Division { get; set; }
        /// <summary>
        /// 소속회사 코드
        /// </summary>
        public string Cor_Code { get; set; }
        /// <summary>
        /// 계약서 코드
        /// </summary>
        public string CC_Code { get; set; }
        /// <summary>
        /// 계약 종류 코드
        /// </summary>
        public string ContractSort_Code { get; set; }
        /// <summary>
        /// 계약 종료일
        /// </summary>
        public DateTime Career_End_Date { get; set; }
        /// <summary>
        /// 계약 시작일
        /// </summary>
        public DateTime Career_Start_Date { get; set; }
        /// <summary>
        /// 부서 구분
        /// </summary>
        public string Post_Code { get; set; }
        /// <summary>
        /// 직책
        /// </summary>
        public string Duty { get; set; }
        /// <summary>
        /// 공동주택 식별코드
        /// </summary>
        public string Apt_Code { get; set; }
        /// <summary>
        /// 공동주택 명
        /// </summary>
        public string Apt_Name { get; set; }
        /// <summary>
        /// 유저 아이디
        /// </summary>
        public string User_ID { get; set; }
        /// <summary>
        /// 부서
        /// </summary>
        public string Post { get; set; }
        /// <summary>
        /// 입력일
        /// </summary>
        public DateTime PostDatge { get; set; }

        /// <summary>
        /// 기타
        /// </summary>
        public string Etc { get; set; }
    }

    /// <summary>
    /// 직원정보 및 배치 정보 결합 엔터티
    /// </summary>
    public class Staff_Career_Entity
    {
        /// <summary>
        /// 식별코드
        /// </summary>
        public int Aid { get; set; }
        /// <summary>
        /// 아이디
        /// </summary>
        public string User_ID { get; set; }
        /// <summary>
        /// 직원코드
        /// </summary>
        public string Staff_Cd { get; set; }
        /// <summary>
        /// 구 아이디
        /// </summary>
        public string Old_UserID { get; set; }
        /// <summary>
        /// 이름
        /// </summary>
        public string User_Name { get; set; }
        /// <summary>
        /// 시도
        /// </summary>
        public string Sido { get; set; }
        /// <summary>
        /// 시군구
        /// </summary>
        public string SiGunGu { get; set; }
        /// <summary>
        /// 나머지 주소
        /// </summary>
        public string RestAdress { get; set; }
        /// <summary>
        /// 출생일
        /// </summary>
        public string Scn { get; set; }
        
        /// <summary>
        /// 출생코드
        /// </summary>
        public string Scn_Code { get; set; }
        /// <summary>
        /// 등급
        /// </summary>
        public int LevelCount { get; set; }

        
        /// <summary>
        /// 방문 수
        /// </summary>
        public int VisitCount { get; set; }
        /// <summary>
        /// 글 수
        /// </summary>
        public int WriteCount { get; set; }
        /// <summary>
        /// 읽은 수
        /// </summary>
        public int ReadCount { get; set; }
        /// <summary>
        /// 댓글 수
        /// </summary>
        public int CommentsCount { get; set; }
        /// <summary>
        /// 파일 첨부 수
        /// </summary>
        public int FileUpCount { get; set; }
        /// <summary>
        /// 가입일
        /// </summary>
        public DateTime JoinDate { get; set; }
        /// <summary>
        /// 삭제 여부
        /// </summary>
        public string Del { get; set; }






        /// <summary>
        /// 휴대폰번호
        /// </summary>
        public string Mobile_Number { get; set; }
        /// <summary>
        /// 전화번호
        /// </summary>
        public string Telephone { get; set; }       

        /// <summary>
        /// 이메일
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// 직원 명
        /// </summary>
        public string Staff_Name { get; set; }

        /// <summary>
        /// 직원 상세 식별코드
        /// </summary>
        public string Staff_Sub_Cd { get; set; }

        /// <summary>
        /// 시도
        /// </summary>
        public string st_Sido { get; set; }
        /// <summary>
        /// 시군
        /// </summary>
        public string st_GunGu { get; set; }
        /// <summary>
        /// 나머지 주소
        /// </summary>
        public string st_Adress_Rest { get; set; }
        /// <summary>
        /// 기타 설명
        /// </summary>
        public string Etc { get; set; }
        /// <summary>
        /// 시작일
        /// </summary>
        public DateTime Start_Date { get; set; }
        /// <summary>
        /// 종료일
        /// </summary>
        public DateTime End_Date { get; set; }

        /// <summary>
        /// 구분
        /// </summary>
        public string d_division { get; set; }

        public string M_Apt_Code { get; set; }
        /// <summary>
        /// 작성일
        /// </summary>
        public DateTime PostDate { get; set; }



        /// <summary>
        /// 배치정보
        /// </summary>
        
        /// <summary>
        /// 구분
        /// </summary>
        public string Division { get; set; }

        /// <summary>
        /// 소속회사 코드
        /// </summary>
        public string Cor_Code { get; set; }

        /// <summary>
        /// 계약서 코드
        /// </summary>
        public string CC_Code { get; set; }


        /// <summary>
        /// 계약 종류 코드
        /// </summary>
        public string ContractSort_Code { get; set; }


        /// <summary>
        /// 계약 종료일
        /// </summary>
        public DateTime Career_End_Date { get; set; }


        /// <summary>
        /// 계약 시작일
        /// </summary>
        public DateTime Career_Start_Date { get; set; }


        /// <summary>
        /// 부서 구분
        /// </summary>
        public string Post_Code { get; set; }
        /// <summary>
        /// 직책
        /// </summary>
        public string Duty { get; set; }
        /// <summary>
        /// 공동주택 식별코드
        /// </summary>
        public string Apt_Code { get; set; }
        /// <summary>
        /// 공동주택 명
        /// </summary>
        public string Apt_Name { get; set; }

        /// <summary>
        /// 동수
        /// </summary>
        public int Dong_Num { get; set; }

        /// <summary>
        /// 주소 시도
        /// </summary>
        public string Apt_Adress_Sido { get; set; }

        /// <summary>
        /// 주소 시군
        /// </summary>
        public string Apt_Adress_Gun { get; set; }

        /// <summary>
        /// 주소 나머지 상세
        /// </summary>
        public string Apt_Adress_Rest { get; set; }

        /// <summary>
        /// 사업자 번호
        /// </summary>
        public string CorporateResistration_Num { get; set; }

        /// <summary>
        /// 사용검사일
        /// </summary>
        public DateTime AcceptancedOfWork_Date { get; set; }

        /// <summary>
        /// 세대수
        /// </summary>
        public int HouseHold_Num { get; set; }

        /// <summary>
        /// 부서
        /// </summary>
        public string Post { get; set; }
        /// <summary>
        /// 입력일
        /// </summary>
        public DateTime PostDatge { get; set; }
    }

    /// <summary>
    /// 작업자 엔터티
    /// </summary>
    public class Service_Worker_Entity
    {
        public int Num { get; set; }
        public string Worker_Code { get; set; }
        public string AptCode { get; set; }
        public string Service_Code { get; set; }
        public string Sub_Code { get; set; }
        public string Group { get; set; }
        public string Post { get; set; }
        public string Duty { get; set; }
        public string Staff_Code { get; set; }
        public string Staff_Name { get; set; }
        public string WorkDate { get; set; }
        public string Del { get; set; }
        public DateTime PostDate { get; set; }
        public string PostIP { get; set; }
        public DateTime ModifyDate { get; set; }
        public string ModifyIP { get; set; }
    }

    /// <summary>
    /// 각 홈피 방문여부 확인
    /// </summary>
    public class Present_Entity
    {
        /// <summary>
        /// 식별코드
        /// </summary>
        public int PresentID { get; set; }

        /// <summary>
        /// 방문자 명
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 방문자 아이디
        /// </summary>
        public string UserID { get; set; }
        /// <summary>
        /// 직책
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 등급
        /// </summary>
        public int LevelCount { get; set; }

        /// <summary>
        /// 방문일
        /// </summary>
        public int DayDate { get; set; }
        /// <summary>
        /// 방문월
        /// </summary>
        public int MonthDate { get; set; }
        /// <summary>
        /// 방문년도
        /// </summary>
        public int YearDate { get; set; }
        /// <summary>
        /// 입력일
        /// </summary>
        public DateTime PostDate { get; set; }
        /// <summary>
        /// 입력자 아이피
        /// </summary>
        public string PostIP { get; set; }
        /// <summary>
        /// 방문자 공동주택 명
        /// </summary>
        public string AptName { get; set; }
        /// <summary>
        /// 방문자 공동주택 식별코드
        /// </summary>
        public string AptCode { get; set; }

        /// <summary>
        /// 위탁코드
        /// </summary>
        public string MenagementCode { get; set; }
    }
}
