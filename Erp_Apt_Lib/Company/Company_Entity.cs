using System;

namespace Company
{
    /// <summary>
    /// 업체정보
    /// </summary>
    public class Company_Entity
    {
        /// <summary>
        /// 식별코드
        /// </summary>
        public int Aid { get; set; }
        /// <summary>
        /// 업체식별코드
        /// </summary>
        public string Cor_Code { get; set; }
        /// <summary>
        /// 업체명
        /// </summary>
        public string Cor_Name { get; set; }
        /// <summary>
        /// 사업자등록번호
        /// </summary>
        public string CorporateResistration_Num { get; set; }
        /// <summary>
        /// 시도
        /// </summary>
        public string Adress_Sido { get; set; }
        /// <summary>
        /// 시군구
        /// </summary>
        public string Adress_GunGu { get; set; }
        /// <summary>
        /// 나머지 주소
        /// </summary>
        public string Adress_Rest { get; set; }

        /// <summary>
        /// 대표자 휴대폰 번호
        /// </summary>
        public string Mobile { get; set; }
        /// <summary>
        /// 전화번호
        /// </summary>
        public string Telephone { get; set; }

        /// <summary>
        /// 사업자 대표자
        /// </summary>
        public string Ceo_Name { get; set; }


        /// <summary>
        /// 등급
        /// </summary>
        public int LevelCount { get; set; }
        /// <summary>
        /// 입력일
        /// </summary>
        public DateTime PostDate { get; set; }

        /// <summary>
        /// 입력자
        /// </summary>
        public string User_Code { get; set; }

        /// <summary>
        /// 입력자 아이피
        /// </summary>
        public string PostIP { get; set; }
        /// <summary>
        /// 비고
        /// </summary>
        public string Intro { get; set; }
    }

    /// <summary>
    /// 업체 상세 정보
    /// </summary>
    public class Company_Sub_Entity
    {
        /// <summary>
        /// 일련번호
        /// </summary>
        public int Aid { get; set; }
        /// <summary>
        /// 업체 식별코드
        /// </summary>
        public string Company_Code { get; set; }
        /// <summary>
        /// 시도
        /// </summary>
        public string Sido { get; set; }
        /// <summary>
        /// 시군구
        /// </summary>
        public String GunGu { get; set; }
        /// <summary>
        /// 주소
        /// </summary>
        public string Adress { get; set; }
        /// <summary>
        /// 대표전화
        /// </summary>
        public string Telephone { get; set; }
        /// <summary>
        /// 팩스
        /// </summary>
        public string Fax { get; set; }
        /// <summary>
        /// 이메일
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// 대표자 이름
        /// </summary>
        public string Ceo_Name { get; set; }
        /// <summary>
        /// 대표자 연락처
        /// </summary>
        public string Ceo_Mobile { get; set; }
        /// <summary>
        /// 담당자 이름
        /// </summary>
        public string ChargeMan { get; set; }
        /// <summary>
        /// 담당자 연락처
        /// </summary>
        public string ChargeMan_Mobile { get; set; }
        /// <summary>
        /// 자본금
        /// </summary>
        public double CapitalStock { get; set; }
        /// <summary>
        /// 신용등급
        /// </summary>
        public string CraditRating { get; set; }
        /// <summary>
        /// 업종
        /// </summary>
        public string TypeOfBusiness { get; set; }
        /// <summary>
        /// 업태
        /// </summary>
        public string BusinessConditions { get; set; }
        /// <summary>
        /// 업체 분류
        /// </summary>
        public string Company_Sort { get; set; }
        /// <summary>
        /// 업체 상세정보
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
    /// 업체정보 메인 상세 조인
    /// </summary>
    public class Company_Join_Entity
    {
        /// <summary>
        /// 식별코드
        /// </summary>
        public int Aid { get; set; }
        /// <summary>
        /// 업체식별코드
        /// </summary>
        public string Cor_Code { get; set; }
        /// <summary>
        /// 업체명
        /// </summary>
        public string Cor_Name { get; set; }
        /// <summary>
        /// 사업자등록번호
        /// </summary>
        public string CorporateResistration_Num { get; set; }
        /// <summary>
        /// 시도
        /// </summary>
        public string Adress_Sido { get; set; }
        /// <summary>
        /// 시군구
        /// </summary>
        public string Adress_GunGu { get; set; }
        /// <summary>
        /// 나머지 주소
        /// </summary>
        public string Adress_Rest { get; set; }

        /// <summary>
        /// 대표자 휴대폰 번호
        /// </summary>
        public string Mobile { get; set; }
        /// <summary>
        /// 전화번호
        /// </summary>
        public string Telephone { get; set; }


        /// <summary>
        /// 등급
        /// </summary>
        public int LevelCount { get; set; }

        /// <summary>
        /// 비고
        /// </summary>
        public string Intro { get; set; }

        /// <summary>
        /// 삭제여부
        /// </summary>
        public string Del { get; set; }

        /// <summary>
        /// 업체 상세 식별코드
        /// </summary>
        public int SubAid { get; set; }

        /// <summary>
        /// 업체 식별코드
        /// </summary>
        public string Company_Code { get; set; }
        /// <summary>
        /// 시도
        /// </summary>
        public string Sido { get; set; }
        /// <summary>
        /// 시군구
        /// </summary>
        public String GunGu { get; set; }
        /// <summary>
        /// 주소
        /// </summary>
        public string Adress { get; set; }

        /// <summary>
        /// 팩스
        /// </summary>
        public string Fax { get; set; }
        /// <summary>
        /// 이메일
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// 대표자 이름
        /// </summary>
        public string Ceo_Name { get; set; }
        /// <summary>
        /// 대표자 연락처
        /// </summary>
        public string Ceo_Mobile { get; set; }
        /// <summary>
        /// 담당자 이름
        /// </summary>
        public string ChargeMan { get; set; }
        /// <summary>
        /// 담당자 연락처
        /// </summary>
        public string ChargeMan_Mobile { get; set; }
        /// <summary>
        /// 자본금
        /// </summary>
        public double CapitalStock { get; set; }
        /// <summary>
        /// 신용등급
        /// </summary>
        public string CraditRating { get; set; }
        /// <summary>
        /// 업종
        /// </summary>
        public string TypeOfBusiness { get; set; }
        /// <summary>
        /// 업태
        /// </summary>
        public string BusinessConditions { get; set; }
        /// <summary>
        /// 업체 분류
        /// </summary>
        public string Company_Sort { get; set; }
        /// <summary>
        /// 업체 상세정보
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
    /// 업체 계약 정보
    /// </summary>
    public class Company_Career_Entity
    {
        /// <summary>
        /// 일련번호
        /// </summary>
        public int Aid { get; set; }
        /// <summary>
        /// 식별코드
        /// </summary>
        public string CC_Code { get; set; }
        /// <summary>
        /// 업체 식별코드
        /// </summary>
        public string Cor_Code { get; set; }
        /// <summary>
        /// 업체명
        /// </summary>
        public string Company_Name { get; set; }
        /// <summary>
        /// 공동주택 식별코드
        /// </summary>
        public string Apt_Code { get; set; }
        /// <summary>
        /// 공동주택명
        /// </summary>
        public string Apt_Name { get; set; }
        /// <summary>
        /// 계약 분류
        /// </summary>
        public string ContractSort { get; set; }
        /// <summary>
        /// 입찰방법
        /// </summary>
        public string Tender { get; set; }
        /// <summary>
        /// 낙찰방법
        /// </summary>
        public string Bid { get; set; }
        /// <summary>
        /// 계약 주체
        /// </summary>
        public string ContractMainAgent { get; set; }

        /// <summary>
        /// 계약일
        /// </summary>
        public DateTime Contract_date { get; set; }

        /// <summary>
        /// 계약 시작일
        /// </summary>
        public DateTime Contract_start_date { get; set; }
        /// <summary>
        /// 계약 종료일
        /// </summary>
        public DateTime Contract_end_date { get; set; }
        /// <summary>
        /// 계약 구분 (A : 계약 중, B : 계약 종료)
        /// </summary>
        public string Division { get; set; }
        /// <summary>
        /// 입력일
        /// </summary>
        public DateTime PostDate { get; set; }
        /// <summary>
        /// 계약 금액
        /// </summary>
        public double Contract_Sum { get; set; }
        /// <summary>
        /// 계약 기간
        /// </summary>
        public int Contract_Period { get; set; }
        /// <summary>
        /// 설명
        /// </summary>
        public string Intro { get; set; }

        public string Sum { get; set; }

        public int FileCount { get; set; }

    }

    /// <summary>
    /// 업체 분류
    /// </summary>
    public class Contract_Sort_Entity
    {
        public int Aid { get; set; }
        /// <summary>
        /// 공동주택코드
        /// </summary>
        public string Apt_Code { get; set; }
        /// <summary>
        /// 업체 분류 코드
        /// </summary>
        public string ContractSort_Code { get; set; }
        /// <summary>
        /// 업체 분류명
        /// </summary>
        public string ContractSort_Name { get; set; }
        /// <summary>
        /// 입력자 아이디
        /// </summary>
        public string Staff_Code { get; set; }
        /// <summary>
        /// 업체분류 상위 코드
        /// </summary>
        public string Up_Code { get; set; }
        /// <summary>
        /// 업체분류 단계
        /// </summary>
        public string ContractSort_Step { get; set; }
        /// <summary>
        /// 업체분류 구분
        /// </summary>
        public string ContractSort_Division { get; set; }
        /// <summary>
        /// 설명
        /// </summary>
        public string ContractSort_Etc { get; set; }
        /// <summary>
        /// 입력일
        /// </summary>
        public DateTime PostDate { get; set; }
    }
}
