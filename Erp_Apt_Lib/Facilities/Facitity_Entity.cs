using System;
using System.Collections.Generic;
using System.Text;

namespace Facilities
{
    /// <summary>
    /// 시설물 정보
    /// </summary>
    public class Facility_Entity
    {
        /// <summary>
        /// 식별코드
        /// </summary>
        public int Aid { get; set; }
        /// <summary>
        /// 공동주택 식별코드
        /// </summary>
        public string Apt_Code { get; set; }
        /// <summary>
        /// 공동주택명
        /// </summary>
        public string Apt_Name { get; set; }
        /// <summary>
        /// 시설물명
        /// </summary>
        public string Facility_Name { get; set; }
        /// <summary>
        /// 대분류
        /// </summary>
        public string Sort_A_Name { get; set; }
        /// <summary>
        /// 대분류코드
        /// </summary>
        public string Sort_A_Code { get; set; }
        /// <summary>
        /// 중분류명
        /// </summary>
        public string Sort_B_Name { get; set; }
        /// <summary>
        /// 중분류코드
        /// </summary>
        public string Sort_B_Code { get; set; }
        /// <summary>
        /// 소분류명
        /// </summary>
        public string Sort_C_Name { get; set; }

        /// <summary>
        /// 제조사
        /// </summary>
        public string Manufacture { get; set; }

        /// <summary>
        /// 제조사 연락처
        /// </summary>
        public string Manufacture_Telephone { get; set; }

        /// <summary>
        /// 제조사 담당자
        /// </summary>
        public string Manufacture_Menager { get; set; }

        /// <summary>
        /// 소분류코드
        /// </summary>
        public string Sort_C_Code { get; set; }
        /// <summary>
        /// 위치(장소)
        /// </summary>
        public string Position { get; set; }
        /// <summary>
        /// 수량
        /// </summary>
        public int Quantity { get; set; }
        /// <summary>
        /// 구입가
        /// </summary>
        public double Price { get; set; }
        /// <summary>
        /// 규격
        /// </summary>
        public string Standard { get; set; }
        /// <summary>
        /// 용량
        /// </summary>
        public string Capacity { get; set; }
        /// <summary>
        /// 모델명
        /// </summary>
        public string Model { get; set; }
        /// <summary>
        /// 설명
        /// </summary>
        public string Explanation { get; set; }
        /// <summary>
        /// 메뉴얼
        /// </summary>
        public string Menual { get; set; }
        /// <summary>
        /// 설치일
        /// </summary>
        public DateTime Installation_Date { get; set; }
        /// <summary>
        /// 폐쇄일
        /// </summary>
        public DateTime Remove_Date { get; set; }
        /// <summary>
        /// 입력일
        /// </summary>
        public DateTime PostDate { get; set; }
        /// <summary>
        /// 입력자아이피
        /// </summary>
        public string PostIP { get; set; }
        /// <summary>
        /// 기타
        /// </summary>
        public string Etc { get; set; }
    }

    

    /// <summary>
    /// 장기 시설물 분류
    /// </summary>
    public class FacilitySort_Entity
    {
        /// <summary>
        /// 식별코드
        /// </summary>
        public int Aid { get; set; }
        /// <summary>
        /// 분류 식별코드
        /// </summary>
        public string Facility_Sort_Code { get; set; }
        /// <summary>
        /// 분류명
        /// </summary>
        public string Sort_Name { get; set; }
        /// <summary>
        /// 상위 식별코드
        /// </summary>
        public string Up_Code { get; set; }
        /// <summary>
        /// 분류단계
        /// </summary>
        public string Sort_Step { get; set; }
        /// <summary>
        /// 분류순서
        /// </summary>
        public string Sort_Order { get; set; }
        /// <summary>
        /// 전체수선주기
        /// </summary>
        public int Repair_Cycle { get; set; }
        /// <summary>
        /// 부분수선주기
        /// </summary>
        public int Repair_Cycle_Part { get; set; }
        /// <summary>
        /// 부분수선율
        /// </summary>
        public int Repair_Rate { get; set; }
        /// <summary>
        /// 법정여부
        /// </summary>
        public string Sort_Division { get; set; }
        /// <summary>
        /// 설명
        /// </summary>
        public string Sort_Detail { get; set; }
        /// <summary>
        /// 법 개정일
        /// </summary>
        public string Enforce_Date { get; set; }
        /// <summary>
        /// 입력일
        /// </summary>
        public DateTime PostDate { get; set; }
        /// <summary>
        /// 미지정
        /// </summary>
        public int OrderBy { get; set; }
    }

    /// <summary>
    /// 시설물 관리자 정보
    /// </summary>
    public class FacilityManager_Entity
    {
        /// <summary>
        /// 시설물 관리자 식별코드
        /// </summary>
        public int Aid { get; set; }
        /// <summary>
        /// 공동주택 식별코드
        /// </summary>
        public string Apt_Code { get; set; }
        /// <summary>
        /// 시설물 식별코드
        /// </summary>
        public string Facility_Code { get; set; }
        /// <summary>
        /// 관리자 식별코드
        /// </summary>
        public string Staff_Code { get; set; }
        /// <summary>
        /// 관리자명
        /// </summary>
        public string Staff_Name { get; set; }
        /// <summary>
        /// 관리자 지정일
        /// </summary>
        public string StartDate { get; set; }
        /// <summary>
        /// 관리자 지정 해제일
        /// </summary>
        public string EndDate { get; set; }
        /// <summary>
        /// 관리자 직책
        /// </summary>
        public string Duty { get; set; }
        /// <summary>
        /// 기타
        /// </summary>
        public string Etc { get; set; }

    }

    /// <summary>
    /// 시설물 제조사 정보 및 담당자 정보
    /// </summary>
    public class MakingCompany_Entity
    {
        /// <summary>
        /// 시설물 제조사 식별코드
        /// </summary>
        public int Aid { get; set; }

        /// <summary>
        /// 공동주택 식별코드
        /// </summary>
        public string Apt_Code { get; set; }

        /// <summary>
        /// 시설물 정보 식별코드
        /// </summary>
        public string Facility_Code { get; set; }

        /// <summary>
        /// 시설물 제조사 정보 식별코드
        /// </summary>
        public string Company_Code { get; set; }

        /// <summary>
        /// 제조자 담당자 명
        /// </summary>
        public string CompanyManagerName { get; set; }

        /// <summary>
        /// 제조사 담당자 휴대폰 번호
        /// </summary>
        public string CompanyManagerMobile { get; set; }

        /// <summary>
        /// 제조사 담당자 이메일
        /// </summary>
        public string CompanyManagerEmail { get; set; }

        /// <summary>
        /// 입력일
        /// </summary>
        public DateTime PostDate { get; set; }

        /// <summary>
        /// 입력자 아이피
        /// </summary>
        public string PostIP { get; set; }

        /// <summary>
        /// 기타
        /// </summary>
        public string Etc { get; set; }
    }
}