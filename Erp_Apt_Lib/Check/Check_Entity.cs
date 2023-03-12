using System;
using System.Collections.Generic;
using System.Text;

namespace Erp_Apt_Lib.Check
{
    /// <summary>
    /// 점검입력여부 엔터티
    /// </summary>
    public class Check_Input_Entity
    {
        /// <summary>
        /// 번호
        /// </summary>
        public int CheckInputID { get; set; }

        /// <summary>
        /// 사업장 코드
        /// </summary>
        public string AptCode { get; set; }

        /// <summary>
        /// 점검년도
        /// </summary>
        public string Check_Year { get; set; }

        /// <summary>
        /// 점검 월
        /// </summary>
        public string Check_Month { get; set; }

        /// <summary>
        /// 점검 일
        /// </summary>
        public string Check_Day { get; set; }

        /// <summary>
        /// 점검대상 시설물 코드
        /// </summary>
        public string Check_Object_Code { get; set; }

        public string Check_Object_Name { get; set; }

        /// <summary>
        /// 점검주기 코드
        /// </summary>
        public string Check_Cycle_Code { get; set; }
        public string Check_Cycle_Name { get; set; }

        /// <summary>
        /// 점검사항 코드
        /// </summary>
        public string Check_Items_Code { get; set; }

        public string Check_Items { get; set; }

        /// <summary>
        /// 점검결과 이름
        /// </summary>
        public string Check_Effect_Code { get; set; }
        public string Check_Effect_Name { get; set; }

        /// <summary>
        /// 입력일
        /// </summary>
        public DateTime PostDate { get; set; }

        /// <summary>
        /// 입력아이피
        /// </summary>
        public string PostIP { get; set; }

        public string User_Post { get; set; }
        public string User_Duty { get; set; }
        public string User_Name { get; set; }

        /// <summary>
        /// 해당 시설에 등록된 점검 수
        /// </summary>
        public int Check_Count { get; set; }

        /// <summary>
        /// 입력된 파일 수
        /// </summary>
        public int Files_Count { get; set; }

        /// <summary>
        /// 삭제여부
        /// </summary>
        public string Del { get; set; }

        /// <summary>
        /// 결제완료 여부
        /// </summary>
        public string Complete { get; set; }
    }

    /// <summary>
    /// 점검표 엔터티
    /// </summary>
    public class Check_Card_Entity
    {
        /// <summary>
        /// 번호
        /// </summary>
        public int CheckCardID { get; set; }

        /// <summary>
        /// 사업장 코드
        /// </summary>
        public string AptCode { get; set; }

        /// <summary>
        /// 점검표 카드 코드
        /// </summary>
        public string Check_Card_Code { get; set; }

        /// <summary>
        /// 점검대상 시설물 코드
        /// </summary>
        public string Check_Object_Code { get; set; }

        /// <summary>
        /// 점검주기 코드
        /// </summary>
        public string Check_Cycle_Code { get; set; }

        /// <summary>
        /// 점검사항 코드
        /// </summary>
        public string Check_Items_Code { get; set; }

        /// <summary>
        /// 점검표 이름
        /// </summary>
        public string Check_Card_Name { get; set; }

        /// <summary>
        /// 점검표 상세
        /// </summary>
        public string Check_Card_Details { get; set; }

        /// <summary>
        /// 입력일
        /// </summary>
        public DateTime PostDate { get; set; }

        /// <summary>
        /// 입력아이피
        /// </summary>
        public string PostIP { get; set; }

        /// <summary>
        /// 수정일
        /// </summary>
        public DateTime? ModifyDate { get; set; }

        /// <summary>
        /// 수정자 아이피
        /// </summary>
        public string ModifyIP { get; set; }

        /// <summary>
        /// 삭제여부
        /// </summary>
        public string Del { get; set; }

        /// <summary>
        /// 기타
        /// </summary>
        public string Category { get; set; }

        public string Sort { get; set; }
    }

    /// <summary>
    /// 점검일지 엔터티
    /// </summary>
    public class Check_List_Entity
    {
        /// <summary>
        /// 번호
        /// </summary>
        public int CheckID { get; set; }

        public string CheckAid { get; set; }

        /// <summary>
        /// 사업장코드
        /// </summary>
        public string AptCode { get; set; }

        /// <summary>
        /// 점검자 아이디
        /// </summary>
        public string UserID { get; set; }

        /// <summary>
        /// 검점자
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 점검부서
        /// </summary>
        public string UserPost { get; set; }

        /// <summary>
        /// 점검자 직책
        /// </summary>
        public string UserDuty { get; set; }

        /// <summary>
        /// 점검제목 코드
        /// </summary>
        public string Check_Input_Code { get; set; }

        /// <summary>
        /// 점검대상 시설물 코드
        /// </summary>
        public string Check_Object_Code { get; set; }

        public string Check_Object_Name { get; set; }

        /// <summary>
        /// 점검주기코드
        /// </summary>
        public string Check_Cycle_Code { get; set; }

        public string Check_Cycle_Name { get; set; }

        /// <summary>
        /// 점검사항 코드
        /// </summary>
        public string Check_Items_Code { get; set; }

        public string Check_Items { get; set; }

        /// <summary>
        /// 점검내용 코드
        /// </summary>
        public string Check_Effect_Code { get; set; }

        public string Check_Effect_Name { get; set; }

        /// <summary>
        /// 점점 조치 내용
        /// </summary>
        public string Check_Details { get; set; }

        /// <summary>
        /// 점검년도
        /// </summary>
        public string Check_Year { get; set; }

        /// <summary>
        /// 점검 월
        /// </summary>
        public string Check_Month { get; set; }

        /// <summary>
        /// 점검 일
        /// </summary>
        public string Check_Day { get; set; }

        public DateTime Check_Date { get; set; }

        /// <summary>
        /// 점검 시간
        /// </summary>
        public string Check_Hour { get; set; }

        /// <summary>
        /// 첨부파일
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// 첨부파일 크기
        /// </summary>
        public int FileSize { get; set; }

        /// <summary>
        /// 입력된 파일 수
        /// </summary>
        public int Files_Count { get; set; }

        /// <summary>
        /// 입력일
        /// </summary>
        public DateTime PostDate { get; set; }

        /// <summary>
        /// 입력자 아이피
        /// </summary>
        public string PostIP { get; set; }

        /// <summary>
        /// 수정일
        /// </summary>
        public DateTime? ModifyDate { get; set; }

        /// <summary>
        /// 수정자 아이피
        /// </summary>
        public string ModifyIP { get; set; }


        /// <summary>
        /// 삭제여부
        /// </summary>
        public string Del { get; set; }

        public string Category { get; set; }

        public string Sort { get; set; }

        /// <summary>
        /// 결재여부
        /// </summary>
        public string Complete { get; set; }
    }

    /// <summary>
    /// 점검사항 엔터티
    /// </summary>
    public class Check_Items_Entity
    {
        public int CheckItemsID { get; set; }

        /// <summary>
        /// 점검대상 시설물 코드
        /// </summary>
        public string Check_Object_Code { get; set; }

        /// <summary>
        /// 점검주기 코드
        /// </summary>
        public string Check_Cycle_Code { get; set; }

        /// <summary>
        /// 점검사항 코드
        /// </summary>
        public string Check_Items_Code { get; set; }

        /// <summary>
        /// 점검사항 명
        /// </summary>
        public string Check_Items { get; set; }

        /// <summary>
        /// 삭제여부
        /// </summary>
        public string Del { get; set; }

        /// <summary>
        /// 입력일
        /// </summary>
        public DateTime PostDate { get; set; }

        /// <summary>
        /// 입력아이피
        /// </summary>
        public string PostIP { get; set; }

        public string Category { get; set; }

        public string Sort { get; set; }

        public string Check_Cycle_Name { get; set; }
        public string Check_Object_Name { get; set; }
    }



    /// <summary>
    /// 점검주기 엔터티
    /// </summary>
    public class Check_Cycle_Entity
    {
        public int CheckCycleID { get; set; }

        /// <summary>
        /// 점검대상 시설물 코드
        /// </summary>
        public string Check_Object_Code { get; set; }

        /// <summary>
        /// 점검주기 코드
        /// </summary>
        public string Check_Cycle_Code { get; set; }

        /// <summary>
        /// 점검주기 명
        /// </summary>
        public string Check_Cycle_Name { get; set; }

        /// <summary>
        /// 점검주기 설명
        /// </summary>
        public string Check_Cycle_Details { get; set; }

        /// <summary>
        /// 입력일
        /// </summary>
        public DateTime PostDate { get; set; }

        /// <summary>
        /// 사용여부
        /// </summary>
        public string Del { get; set; }

        /// <summary>
        /// 입력아이피
        /// </summary>
        public string PostIP { get; set; }

        public string Category { get; set; }

        public string Sort { get; set; }
    }

    /// <summary>
    /// 점검결과 엔터티
    /// </summary>
    public class Check_Effect_Entity
    {
        public int CheckEffectID { get; set; }

        /// <summary>
        /// 사업장 코드
        /// </summary>
        public string AptCode { get; set; }

        /// <summary>
        /// 점검주기 코드
        /// </summary>
        public string Check_Effect_Code { get; set; }

        /// <summary>
        /// 점검주기 명
        /// </summary>
        public string Check_Effect_Name { get; set; }

        /// <summary>
        /// 점검주기 설명
        /// </summary>
        public string Check_Effect_Details { get; set; }

        /// <summary>
        /// 삭제여부
        /// </summary>
        public string Del { get; set; }

        /// <summary>
        /// 입력일
        /// </summary>
        public DateTime PostDate { get; set; }

        /// <summary>
        /// 입력아이피
        /// </summary>
        public string PostIP { get; set; }
    }

    /// <summary>
    /// 점검대상 엔터티
    /// </summary>
    public class Check_Object_Entity
    {
        public int CheckObjectID { get; set; }

        /// <summary>
        /// 사업장코드
        /// </summary>
        public string AptCode { get; set; }

        /// <summary>
        /// 점검대상 코드
        /// </summary>
        public string Check_Object_Code { get; set; }

        /// <summary>
        /// 점검대상 명
        /// </summary>
        public string Check_Object_Name { get; set; }

        /// <summary>
        /// 점검대상 설명
        /// </summary>
        public string Check_Object_Details { get; set; }

        /// <summary>
        /// 삭제여부
        /// </summary>
        public string Del { get; set; }

        /// <summary>
        /// 입력일
        /// </summary>
        public DateTime PostDate { get; set; }

        /// <summary>
        /// 입력아이피
        /// </summary>
        public string PostIP { get; set; }

        public string Category { get; set; }

        public string Sort { get; set; }
    }
}
