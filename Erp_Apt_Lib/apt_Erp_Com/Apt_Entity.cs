using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Erp_Entity
{

    #region 공동주택 정보
    /// <summary>
    /// 공동주택(아파트) 엔터티
    /// </summary>
    public class Apt_Entity
    {
        /// <summary>
        /// 식별번호
        /// </summary>
        public int Aid { get; set; }
        /// <summary>
        /// 공동주택 식별코드
        /// </summary>
        public string Apt_Code { get; set; }
        /// <summary>
        /// 공동주택 명
        /// </summary>
        public string Apt_Name { get; set; }
        /// <summary>
        /// 공동주택 형태
        /// </summary>
        public string Apt_Form { get; set; }
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
        /// 입력일
        /// </summary>
        public DateTime PostDate { get; set; }
        /// <summary>
        /// 입력자 아이피
        /// </summary>
        public string PostIP { get; set; }
        /// <summary>
        /// 단지 등급
        /// </summary>
        public int LevelCount { get; set; }
        /// <summary>
        /// 구분(결합)
        /// </summary>
        public string Combine { get; set; }
        /// <summary>
        /// 기타
        /// </summary>
        public string Intro { get; set; }
    }

    /// <summary>
    /// 아파트 상세 엔터티
    /// </summary>
    public class Apt_Sub_Entity
    {
        /// <summary>
        /// 식별번호
        /// </summary>
        public int Aid { get; set; }
        /// <summary>
        /// 공동주택 상세 식별코드
        /// </summary>
        public string Apt_Sub_Code { get; set; }
        /// <summary>
        /// 공동주택 실별코드
        /// </summary>
        public string Apt_Code { get; set; }
        /// <summary>
        /// 시행사
        /// </summary>
        public string Developer { get; set; }
        /// <summary>
        /// 시공사
        /// </summary>
        public string Builder { get; set; }
        /// <summary>
        /// 법정 지구 구분
        /// </summary>
        public string District { get; set; }
        /// <summary>
        /// 대지면적
        /// </summary>
        public double Site_Area { get; set; }
        /// <summary>
        /// 건축면적
        /// </summary>
        public double Build_Area { get; set; }
        /// <summary>
        /// 연면적
        /// </summary>
        public double FloorTotal_Area { get; set; }
        /// <summary>
        /// 공급면적
        /// </summary>
        public double Supply_Area { get; set; }
        /// <summary>
        /// 용적율
        /// </summary>
        public double FloorArea_Ratio { get; set; }
        /// <summary>
        /// 건폐율
        /// </summary>
        public double BuildingCoverage_Ratio { get; set; }
        /// <summary>
        /// 최고 높이
        /// </summary>
        public double Heighest { get; set; }
        /// <summary>
        /// 난방방식
        /// </summary>
        public string Heating_Way { get; set; }
        /// <summary>
        /// 급수방식
        /// </summary>
        public string WaterSupply_Way { get; set; }
        /// <summary>
        /// 대표전화번호
        /// </summary>
        public string Telephone { get; set; }
        /// <summary>
        /// 팩스번호
        /// </summary>
        public string Fax { get; set; }
        /// <summary>
        /// 대표 이메일
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// 수전용량
        /// </summary>
        public int Electric_Supply_Capacity { get; set; }
        /// <summary>
        /// 저수량
        /// </summary>
        public double Water_Quantity { get; set; }
        /// <summary>
        /// 주차대수
        /// </summary>
        public int Park_Car_Count { get; set; }
        /// <summary>
        /// 관리방식
        /// </summary>
        public string Management_Way { get; set; }
        /// <summary>
        /// 승강기수
        /// </summary>
        public int Elevator { get; set; }
        /// <summary>
        /// 공동관리여부
        /// </summary>
        public string Joint_Management { get; set; }
        /// <summary>
        /// 입력일
        /// </summary>
        public DateTime PostDate { get; set; }
        /// <summary>
        /// 입력자 아이피
        /// </summary>
        public string PostIP { get; set; }
    } 
    #endregion

    /// <summary>
    /// 시도 정보
    /// </summary>
    public class Sido_Entity
    {
        /// <summary>
        /// 식별번호
        /// </summary>
        public int Aid { get; set; }
        /// <summary>
        /// 식별코드
        /// </summary>
        public string Sido_Code { get; set; }
        /// <summary>
        /// 시도명
        /// </summary>
        public string Sido { get; set; }
        /// <summary>
        /// 시군 명
        /// </summary>
        public string Region { get; set; }
        /// <summary>
        /// 단계
        /// </summary>
        public string Step { get; set; }
        /// <summary>
        /// 입력일
        /// </summary>
        public DateTime PostDate { get; set; }
    }

    /// <summary>
    /// 직원 배치정보 엔터티
    /// </summary>
    public class Referral_career_Entity_A
    {
        public int Aid { get; set; }
        public string Staff_Cd { get; set; }
        public string User_ID { get; set; }
        public string User_Name { get; set; }
        public string Apt_Name { get; set; }
        public string Apt_Code { get; set; }
        public string Post { get; set; }
        public string Duty { get; set; }
        public DateTime Career_Start_Date { get; set; }
        public DateTime Career_End_Date { get; set; }
        public string ContractSort_Code { get; set; }
        public string CC_Code { get; set; }
        public string Cor_Code { get; set; }
        /// <summary>
        /// 퇴사여부(A : 근무중, B : 퇴사)
        /// </summary>
        public string Division { get; set; }

        /// <summary>
        /// 직원 구분(A : 직원, B : 소장, C : 입대의, D : 선관위, E : 자생단체)
        /// </summary>
        public string Post_Code { get; set; }
        public DateTime PostDatge { get; set; }
    }
        
    

    /// <summary>
    /// 첨부 파일 엔터티
    /// </summary>
    public class Erp_Files_Entity
	{
		 public int Num { get; set; }
        public string AptCode { get; set; }
        public string Parents_Name { get; set; }
        public string Parents_Num { get; set; }
        public string Sw_FileName { get; set; }
        public int Sw_FileSize { get; set; }
        public DateTime PostDate { get; set; }
        public string PostID { get; set; }
        public DateTime ModifyDate { get; set; }
        public string ModifyIP { get; set; }
        public string Del { get; set; }
    }

    /// <summary>
    /// 입주자 카드 엔터티
    /// </summary>
    public class Apt_People_Entity
    {
        public int Num { get; set; }
        public string Dong { get; set; }
        public string Ho { get; set; }
        public string Apt_Code { get; set; }
        public string Apt_Name { get; set; }
        public double Area { get; set; }
        public string InDateTime { get; set; }
        public string InnerOwner { get; set; }
        public string Bunyong { get; set; }
        public string Inter { get; set; }
        public string Relation { get; set; }
        public string InnerName { get; set; }
        public string Owner { get; set; }

        public string UserID { get; set; }
        public string Password { get; set; }
        public string Help { get; set; }
        public string Email { get; set; }
        public string Tel { get; set; }
        public string Hp { get; set; }
        public string InnSoceity { get; set; }
        public string InnerScn1 { get; set; }
        public string InnerScn2 { get; set; }
        public int LevelCount { get; set; }
        public int CarCount { get; set; }
        public string MoveA { get; set; }
        public DateTime MoveDate { get; set; }

        public string Intro { get; set; }
        public int VisitCount { get; set; }
        public string PostIP { get; set; }
        public DateTime PostDate { get; set; }
        public string ModifyIP { get; set; }
        public DateTime ModifyDate { get; set; }
        public int AgoCost { get; set; }
        public int Files_Count { get; set; }
    }

    /// <summary>
    /// 부서 및 직책 엔터티
    /// </summary>
    public class PostDuty_Entity
    {
        /// <summary>
        /// 일련번호
        /// </summary>
        public int Num { get; set; }
        /// <summary>
        /// 부서 및 직책 식별코드
        /// </summary>
        public string PD_Code { get; set; }
        /// <summary>
        /// 부서 및 직책 구분명
        /// </summary>
        public string PD_C_Name { get; set; }
        /// <summary>
        /// 부서 및 직책 분류코드
        /// </summary>
        public string Post_Code { get; set; }

        /// <summary>
        /// 부서 및 직책 분류 명
        /// </summary>
        public string Post_Name { get; set; }
        /// <summary>
        /// 부서 직책 식별코드
        /// </summary>
        public string Post_Duty_Code { get; set; }
        /// <summary>
        /// 부서 및 직책 명
        /// </summary>
        public string Post_Duty_Name { get; set; }

        /// <summary>
        /// 부서 직책 구분코드
        /// </summary>
        public string Post_Duty_D_Code { get; set; }
        /// <summary>
        /// 부서 직책 구분 코드명
        /// </summary>
        public string Post_Duty_Direct { get; set; }
        /// <summary>
        /// 부서 직책 설명
        /// </summary>
        public string Post_Duty_Intro { get; set; }
        /// <summary>
        /// 입력자 아이디
        /// </summary>
        public string UserID { get; set; }
        /// <summary>
        /// 입력자 소속 코드
        /// </summary>
        public string AptCode { get; set; }
        /// <summary>
        /// 입력일
        /// </summary>
        public DateTime PostDate { get; set; }
        /// <summary>
        /// 입력자 아이디
        /// </summary>
        public string PostIP { get; set; }
    }

    /// <summary>
    /// 부서 엔터티
    /// </summary>
    public class Post_Entity
    {
        /// <summary>
        /// 식별코드
        /// </summary>
        public int PostCode { get; set; }
        /// <summary>
        /// 부서명
        /// </summary>
        public string PostName { get; set; }
        /// <summary>
        /// 부서 구분
        /// </summary>
        public string Division { get; set; }
        /// <summary>
        /// 기타
        /// </summary>
        public string Etc { get; set; }
        /// <summary>
        /// 입력일
        /// </summary>
        public DateTime PostDate { get; set; }
        /// <summary>
        /// 입력 아이피
        /// </summary>
        public string PostIP { get; set; }
    }

    /// <summary>
    /// 직책 엔터티
    /// </summary>
    public class Duty_Entity
    {
        /// <summary>
        /// 직책 식별코드
        /// </summary>
        public int DutyCode { get; set; }
        /// <summary>
        /// 직책명
        /// </summary>
        public string DutyName { get; set; }
        /// <summary>
        /// 부서 식별코드
        /// </summary>
        public string PostCode { get; set; }
        /// <summary>
        /// 직책구분
        /// </summary>
        public string Division { get; set; }
        /// <summary>
        /// 설명
        /// </summary>
        public string Etc { get; set; }
        /// <summary>
        /// 입력일
        /// </summary>
        public DateTime PostDate { get; set; }
        /// <summary>
        /// 입력자 아이피
        /// </summary>
        public string PostIP { get; set; }

    }

    /// <summary>
    /// 결제 부서 직책 라인 코드 속성17
    /// </summary>
    public class Decision_PostDuty_Entity
    {
        /// <summary>
        /// 일련번호
        /// </summary>
        public int Num { get; set; }

        /// <summary>
        /// 공동주택  코드
        /// </summary>
        public string AptCode { get; set; }

        /// <summary>
        /// 공동주택 명
        /// </summary>
        public string AptName { get; set; }

        /// <summary>
        /// 결제 분류 명
        /// </summary>
        public string Bloom { get; set; }

        /// <summary>
        /// 결재 분류 코드
        /// </summary>
        public string Bloom_Code { get; set; }

        /// <summary>
        ///결재 분류 이름
        /// </summary>
        public string PostDuty { get; set; }

        /// <summary>
        /// 결제 부서 이름
        /// </summary>
        public string Post { get; set; }

        /// <summary>
        /// 결제 직책 이름
        /// </summary>
        public string Duty { get; set; }

        /// <summary>
        /// 결재 분류 설명
        /// </summary>
        public string Intro { get; set; }

        /// <summary>
        /// 수정일
        /// </summary>
        public DateTime ModifyDate { get; set; }

        /// <summary>
        /// 수정자 아이피
        /// </summary>
        public string ModifyIP { get; set; }


        /// <summary>
        /// 작성일
        /// </summary>
        public DateTime PostDate { get; set; }

        /// <summary>
        /// 작성자 아이피
        /// </summary>
        public string PostIP { get; set; }

        /// <summary>
        /// 순서
        /// </summary>
        public int Step { get; set; }
    }    

    /// <summary>
    /// 홈페이지 가입 주민
    /// </summary>
    public class bs_apt_career_Entity
    {
        public int Aid { get; set; }
        public string Dong { get; set; }
        public string Ho { get; set; }
        public string bs_code { get; set; } // 회원 코드
        public string bs_name { get; set; } // 회원 이름

        /// <summary>
        /// 공동주택 식별코드
        /// </summary>
        public string Apt_Code { get; set; }

        /// <summary>
        /// 공동주택 명
        /// </summary>
        public string Apt_Name { get; set; }

        /// <summary>
        /// 가입일
        /// </summary>
        public DateTime bs_start { get; set; }

        /// <summary>
        /// 탈퇴일
        /// </summary>
        public DateTime bs_end { get; set; }
        
        /// <summary>
        /// 구분(A : 승인, B : 불승인)
        /// </summary>
        public string bs_division { get; set; }

        /// <summary>
        /// 자기 소개
        /// </summary>
        public string bs_intro { get; set; }

        /// <summary>
        /// 입력자 아이피
        /// </summary>
        public string postip { get; set; }

        /// <summary>
        /// 작성일
        /// </summary>
        public DateTime Postdate { get; set; }

        /// <summary>
        /// 단지회원 식별코드
        /// </summary>
        public string apt_career_Code { get; set; }

        /// <summary>
        /// 입주자카드 식별코드
        /// </summary>
        public string Sw_People_Code { get; set; }

        /// <summary>
        /// 가입신청 식별코드
        /// </summary>
        public string Apt_Pople_Code { get; set; }
    }

    /// <summary>
    /// 가입 입주민(홈페이지 및 민원, 하자 등에서 사용)
    /// </summary>
    public class In_AptPeople_Entity
    {
        /// <summary>
        /// 일련번호
        /// </summary>
        public int Aid { get; set; }
        /// <summary>
        /// 공동주택 코드
        /// </summary>
        public string Apt_Code { get; set; }
        /// <summary>
        /// 공동주택명
        /// </summary>
        public string Apt_Name { get; set; }
        /// <summary>
        /// 아이디
        /// </summary>
        public string User_Code { get; set; }
        /// <summary>
        /// 이름
        /// </summary>
        public string User_Name { get; set; }
        /// <summary>
        /// 암호
        /// </summary>
        public string Pass_Word { get; set; }
        /// <summary>
        /// 등급
        /// </summary>
        public int LevelCount { get; set; }
        /// <summary>
        /// 동 이름
        /// </summary>
        public string Dong { get; set; }
        /// <summary>
        /// 호 이름
        /// </summary>
        public string Ho { get; set; }
        /// <summary>
        /// 생년월일
        /// </summary>
        public DateTime Scn { get; set; }
        /// <summary>
        /// 휴대폰 번호
        /// </summary>
        public string Mobile { get; set; }
        /// <summary>
        /// 전화번호
        /// </summary>
        public string Telephone { get; set; }
        /// <summary>
        /// 이메일
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// 자기 소개
        /// </summary>
        public string Intro { get; set; }
        /// <summary>
        /// 방문수
        /// </summary>
        public int VisitCount { get; set; }
        /// <summary>
        /// 글쓴 수
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
        /// 파일 업로드 수
        /// </summary>
        public int FileUpCount { get; set; }

        /// <summary>
        /// 이사일
        /// </summary>
        public DateTime MoveDate { get; set; }
        /// <summary>
        /// 입력일
        /// </summary>
        public DateTime PostDate { get; set; }
        /// <summary>
        /// 입력자 아이피
        /// </summary>
        public string PostIP { get; set; }

        /// <summary>
        /// 이사여부
        /// </summary>
        public string Withdrawal { get; set; }

        public string Approval { get; set; }

    }

    /// <summary>
    /// 아파트와 입주민 조인
    /// </summary>
    public class Join_apt_People_a_Career_Entity
    {
        /// <summary>
        /// 일련번호
        /// </summary>
        public int Aid { get; set; }
        /// <summary>
        /// 공동주택 코드
        /// </summary>
        public string Apt_Code { get; set; }
        /// <summary>
        /// 공동주택명
        /// </summary>
        public string Apt_Name { get; set; }
        /// <summary>
        /// 아이디
        /// </summary>
        public string User_Code { get; set; }
        /// <summary>
        /// 이름
        /// </summary>
        public string User_Name { get; set; }
        /// <summary>
        /// 암호
        /// </summary>
        public string Pass_Word { get; set; }
        /// <summary>
        /// 등급
        /// </summary>
        public int LevelCount { get; set; }
        /// <summary>
        /// 동 이름
        /// </summary>
        public string Dong { get; set; }
        /// <summary>
        /// 호 이름
        /// </summary>
        public string Ho { get; set; }
        /// <summary>
        /// 생년월일
        /// </summary>
        public DateTime Scn { get; set; }
        /// <summary>
        /// 휴대폰 번호
        /// </summary>
        public string Mobile { get; set; }
        /// <summary>
        /// 전화번호
        /// </summary>
        public string Telephone { get; set; }
        /// <summary>
        /// 이메일
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// 자기 소개
        /// </summary>
        public string Intro { get; set; }
        /// <summary>
        /// 방문수
        /// </summary>
        public int VisitCount { get; set; }
        /// <summary>
        /// 읽은 수
        /// </summary>
        public int ReadCount { get; set; }
        /// <summary>
        /// 글쓴 수
        /// </summary>
        public int WriteCount { get; set; }
        /// <summary>
        /// 댓글 수
        /// </summary>
        public int CommentsCount { get; set; }
        /// <summary>
        /// 파일 업로드 수
        /// </summary>
        public int FileUpCount { get; set; }
        /// <summary>
        /// 이사일
        /// </summary>
        public DateTime MoveDate { get; set; }
        /// <summary>
        /// 입력일
        /// </summary>
        public DateTime PostDate { get; set; }
        /// <summary>
        /// 입력자 아이피
        /// </summary>
        public string PostIP { get; set; }
        /// <summary>
        /// 이사여부
        /// </summary>
        public string Withdrawal { get; set; }

        public DateTime bs_start { get; set; }
        public DateTime bs_end { get; set; }
        public string bs_division { get; set; }
        public string bs_intro { get; set; }
        public string bs_code { get; set; }
        public string bs_name { get; set; }
        /// <summary>
        /// 단지회원 식별코드
        /// </summary>
        public string apt_career_Code { get; set; }

        /// <summary>
        /// 입주자카드 식별코드
        /// </summary>
        public string Sw_People_Code { get; set; }

        /// <summary>
        /// 가입신청 식별코드
        /// </summary>
        public string Apt_Pople_Code { get; set; }
    }
}
