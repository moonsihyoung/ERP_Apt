using System;
using System.Collections.Generic;
using System.Text;

namespace Erp_Apt_Lib.Stocks
{
    /// <summary>
    /// Stock_Code_Entity 재고관리 코드 DB 속성 입니다.
    /// </summary>
    public class Stock_Code_Entity
    {
        /// <summary>
        /// 번호
        /// </summary>
        public int Num { get; set; }

        /// <summary>
        /// 제품 코드명
        /// </summary>
        public string St_Code { get; set; }

        /// <summary>
        /// 제품 구분명
        /// </summary>
        public string St_Group { get; set; }

        /// <summary>
        /// 제품 명
        /// </summary>
        public string St_Name { get; set; }

        /// <summary>
        /// 제품 모델명
        /// </summary>
        public string St_Model { get; set; }

        /// <summary>
        /// 제품 모델 번호
        /// </summary>
        public string St_Model_No { get; set; }

        /// <summary>
        /// 제품 단위
        /// </summary>
        public string St_Unit { get; set; }

        /// <summary>
        /// 보관장소
        /// </summary>
        public string St_Place { get; set; }

        /// <summary>
        /// 용량 또는 규격
        /// </summary>
        public string St_Dosage { get; set; }

        /// <summary>
        /// 용도
        /// </summary>
        public string St_Using { get; set; }

        /// <summary>
        /// 제품 구분(대분류)
        /// </summary>
        public string St_Section { get; set; }

        /// <summary>
        /// 제품 분류(중분류)
        /// </summary>
        public string St_Asort { get; set; }

        /// <summary>
        /// 제품 분류(세분류)
        /// </summary>
        public string St_Bloom { get; set; }

        /// <summary>
        /// 제품 사진
        /// </summary>
        public string St_Photo { get; set; }

        /// <summary>
        /// 제품사진 사이즈
        /// </summary>
        public int St_PhotoSize { get; set; }

        /// <summary>
        /// 사용 설명서
        /// </summary>
        public string St_Manual { get; set; }

        /// <summary>
        /// 입력자 아이디
        /// </summary>
        public string St_UserID { get; set; }

        /// <summary>
        /// 첨부파일
        /// </summary>
        public string St_FileName { get; set; }

        /// <summary>
        /// 첨부파일 크기
        /// </summary>
        public int St_FileSize { get; set; }

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
        public DateTime ModifyDate { get; set; }

        /// <summary>
        /// 수정자 아이피
        /// </summary>
        public string ModifyIP { get; set; }

        /// <summary>
        /// 수정자 아이디
        /// </summary>
        public string Modify_UserID { get; set; }

        /// <summary>
        /// 공동주택(아파트) 코드명
        /// </summary>
        public string Apt_Code { get; set; }

        public int Wh_Quantity { get; set; }
        public int Wh_Balance { get; set; }
        public DateTime b_date { get; set; }
    }

    /// <summary>
    /// WareHouse_Entity 재고관리 입출력 DB 속성 입니다.
    /// </summary>
    public class WareHouse_Entity
    {
        /// <summary>
        /// 번호
        /// </summary>
        public int Num { get; set; }

        /// <summary>
        /// 입출고 구분
        /// </summary>
        public string Wh_Section { get; set; }

        /// <summary>
        /// 입출고 코드
        /// </summary>
        public string Wh_Code { get; set; }

        /// <summary>
        /// 제품 코드명
        /// </summary>
        public string St_Code { get; set; }

        /// <summary>
        /// 제품 구분 코드
        /// </summary>
        public string St_Group { get; set; }

        /// <summary>
        /// 입출고 수량
        /// </summary>
        public int Wh_Quantity { get; set; }

        /// <summary>
        /// 잔고 수량
        /// </summary>
        public int Wh_Balance { get; set; }

        /// <summary>
        /// 구입비용
        /// </summary>
        public int Wh_Cost { get; set; }

        /// <summary>
        /// 단위
        /// </summary>
        public string Wh_Unit { get; set; }

        /// <summary>
        /// 보관장소
        /// </summary>
        public string Wh_Place { get; set; }

        /// <summary>
        /// 사용 내역
        /// </summary>
        public string Wh_Use { get; set; }

        /// <summary>
        /// 입력자 아이디
        /// </summary>
        public string Wh_UserID { get; set; }

        /// <summary>
        /// 입출고 년도
        /// </summary>
        public string Wh_Year { get; set; }

        /// <summary>
        /// 입출고 월
        /// </summary>
        public string Wh_Month { get; set; }

        /// <summary>
        /// 입출고 날짜
        /// </summary>
        public string Wh_Day { get; set; }

        /// <summary>
        /// 입출고 사진
        /// </summary>
        public string Wh_FileName { get; set; }

        /// <summary>
        /// 입출고 사진파일 크기
        /// </summary>
        public int Wh_FileSize { get; set; }

        /// <summary>
        /// 입출고 사진2
        /// </summary>
        public string Wh_FileName2 { get; set; }

        /// <summary>
        /// 입출고 사진파일 크기2
        /// </summary>
        public int Wh_FileSize2 { get; set; }

        /// <summary>
        /// 입출고 사진3
        /// </summary>
        public string Wh_FileName3 { get; set; }

        /// <summary>
        /// 입출고 사진파일 크기3
        /// </summary>
        public int Wh_FileSize3 { get; set; }

        /// <summary>
        /// 입출고 사진4
        /// </summary>
        public string Wh_FileName4 { get; set; }

        /// <summary>
        /// 입출고 사진파일 크기4
        /// </summary>
        public int Wh_FileSize4 { get; set; }

        /// <summary>
        /// 입출고 사진
        /// </summary>
        public string Wh_FileName5 { get; set; }

        /// <summary>
        /// 입출고 사진파일 크기
        /// </summary>
        public int Wh_FileSize5 { get; set; }

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
        public DateTime ModifyDate { get; set; }

        /// <summary>
        /// 수정자 아이피
        /// </summary>
        public string ModifyIP { get; set; }

        /// <summary>
        /// 수정자 아이디
        /// </summary>
        public string Modify_UserID { get; set; }

        /// <summary>
        /// 제품 코드명
        /// </summary>
        public string Etc { get; set; }

        /// <summary>
        /// 공동주택(아파트) 코드명
        /// </summary>
        public string AptCode { get; set; }

        /// <summary>
        /// 일출고 사용 작업일지 번호
        /// </summary>
        public string Parents { get; set; }

        /// <summary>
        /// 연결 작업 명
        /// </summary>
        public string P_Group { get; set; }
    }

    /// <summary>
    /// 자재 및 입출고 조인 엔터티
    /// </summary>
    public class SC_WH_Join_Entity
    {
        /// <summary>
        /// 제품 코드명
        /// </summary>
        public string St_Code { get; set; }

        /// <summary>
        /// 제품 구분명
        /// </summary>
        public string St_Group { get; set; }

        /// <summary>
        /// 제품 명
        /// </summary>
        public string St_Name { get; set; }

        /// <summary>
        /// 제품 모델명
        /// </summary>
        public string St_Model { get; set; }

        /// <summary>
        /// 제품 모델 번호
        /// </summary>
        public string St_Model_No { get; set; }

        /// <summary>
        /// 제품 단위
        /// </summary>
        public string St_Unit { get; set; }

        /// <summary>
        /// 보관장소
        /// </summary>
        public string St_Place { get; set; }

        /// <summary>
        /// 용량 또는 규격
        /// </summary>
        public string St_Dosage { get; set; }

        /// <summary>
        /// 용도
        /// </summary>
        public string St_Using { get; set; }

        /// <summary>
        /// 제품 구분(대분류)
        /// </summary>
        public string St_Section { get; set; }

        /// <summary>
        /// 제품 분류(중분류)
        /// </summary>
        public string St_Asort { get; set; }

        /// <summary>
        /// 제품 분류(세분류)
        /// </summary>
        public string St_Bloom { get; set; }

        /// <summary>
        /// 제품 사진
        /// </summary>
        public string St_Photo { get; set; }

        /// <summary>
        /// 제품사진 사이즈
        /// </summary>
        public int St_PhotoSize { get; set; }

        /// <summary>
        /// 사용 설명서
        /// </summary>
        public string St_Manual { get; set; }

        /// <summary>
        /// 입력자 아이디
        /// </summary>
        public string St_UserID { get; set; }

        /// <summary>
        /// 첨부파일
        /// </summary>
        public string St_FileName { get; set; }

        /// <summary>
        /// 첨부파일 크기
        /// </summary>
        public int St_FileSize { get; set; }




        /// <summary>
        /// 번호
        /// </summary>
        public int Num { get; set; }

        /// <summary>
        /// 입출고 구분
        /// </summary>
        public string Wh_Section { get; set; }

        /// <summary>
        /// 입출고 코드
        /// </summary>
        public string Wh_Code { get; set; }



        /// <summary>
        /// 입출고 수량
        /// </summary>
        public int Wh_Quantity { get; set; }

        /// <summary>
        /// 잔고 수량
        /// </summary>
        public int Wh_Balance { get; set; }

        /// <summary>
        /// 구입비용
        /// </summary>
        public int Wh_Cost { get; set; }

        /// <summary>
        /// 단위
        /// </summary>
        public string Wh_Unit { get; set; }

        /// <summary>
        /// 보관장소
        /// </summary>
        public string Wh_Place { get; set; }

        /// <summary>
        /// 사용 내역
        /// </summary>
        public string Wh_Use { get; set; }

        /// <summary>
        /// 입력자 아이디
        /// </summary>
        public string Wh_UserID { get; set; }

        /// <summary>
        /// 입출고 년도
        /// </summary>
        public string Wh_Year { get; set; }

        /// <summary>
        /// 입출고 월
        /// </summary>
        public string Wh_Month { get; set; }

        /// <summary>
        /// 입출고 날짜
        /// </summary>
        public string Wh_Day { get; set; }

        /// <summary>
        /// 입출고 사진
        /// </summary>
        public string Wh_FileName { get; set; }

        /// <summary>
        /// 입출고 사진파일 크기
        /// </summary>
        public int Wh_FileSize { get; set; }

        /// <summary>
        /// 입출고 사진2
        /// </summary>
        public string Wh_FileName2 { get; set; }

        /// <summary>
        /// 입출고 사진파일 크기2
        /// </summary>
        public int Wh_FileSize2 { get; set; }

        /// <summary>
        /// 입출고 사진3
        /// </summary>
        public string Wh_FileName3 { get; set; }

        /// <summary>
        /// 입출고 사진파일 크기3
        /// </summary>
        public int Wh_FileSize3 { get; set; }

        /// <summary>
        /// 입출고 사진4
        /// </summary>
        public string Wh_FileName4 { get; set; }

        /// <summary>
        /// 입출고 사진파일 크기4
        /// </summary>
        public int Wh_FileSize4 { get; set; }

        /// <summary>
        /// 입출고 사진
        /// </summary>
        public string Wh_FileName5 { get; set; }

        /// <summary>
        /// 입출고 사진파일 크기
        /// </summary>
        public int Wh_FileSize5 { get; set; }

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
        public DateTime ModifyDate { get; set; }

        /// <summary>
        /// 수정자 아이피
        /// </summary>
        public string ModifyIP { get; set; }

        /// <summary>
        /// 수정자 아이디
        /// </summary>
        public string Modify_UserID { get; set; }

        /// <summary>
        /// 제품 코드명
        /// </summary>
        public string Etc { get; set; }

        /// <summary>
        /// 공동주택(아파트) 코드명
        /// </summary>
        public string AptCode { get; set; }

        /// <summary>
        /// 일출고 사용 작업일지 번호
        /// </summary>
        public string Parents { get; set; }

        /// <summary>
        /// 연결 작업 명
        /// </summary>
        public string P_Group { get; set; }
    }
}
