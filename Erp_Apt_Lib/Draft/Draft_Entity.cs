using System;
using System.Collections.Generic;
using System.Text;

namespace Erp_Apt_Lib.Draft
{
    public class DraftEntity
    {
        /// <summary>
        /// 번호
        /// </summary>
        public int Aid { get; set; }

        /// <summary>
        /// 기안자명
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 사업장면
        /// </summary>
        public string AptName { get; set; }

        /// <summary>
        /// 사업장 코드
        /// </summary>
        public string AptCode { get; set; }

        /// <summary>
        /// 기안자 아이디
        /// </summary>
        public string UserCode { get; set; }

        /// <summary>
        /// 기안부서
        /// </summary>
        public string Post { get; set; }

        /// <summary>
        /// 대분류
        /// </summary>
        public string BranchA { get; set; }

        /// <summary>
        /// 중분류
        /// </summary>
        public string BranchB { get; set; }

        /// <summary>
        /// 세분류
        /// </summary>
        public string BranchC { get; set; }

        /// <summary>
        /// 품분류
        /// </summary>
        public string BranchD { get; set; }

        /// <summary>
        /// 기안 제목
        /// </summary>
        public string DraftTitle { get; set; }

        /// <summary>
        /// 기안입력일자
        /// </summary>
        public DateTime PostDate { get; set; }

        /// <summary>
        /// 기안 입력자 아이피
        /// </summary>
        public string PostIP { get; set; }

        /// <summary>
        /// 기안내용
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 결재여부
        /// </summary>
        public string Decision { get; set; }

        /// <summary>
        /// 기안번호
        /// </summary>
        public string DraftNum { get; set; }

        /// <summary>
        /// 보존년한
        /// </summary>
        public int KeepYear { get; set; }

        /// <summary>
        /// 기안년도
        /// </summary>
        public int DraftYear { get; set; }

        /// <summary>
        /// 기안월
        /// </summary>
        public int DraftMonth { get; set; }

        /// <summary>
        /// 기안일
        /// </summary>
        public int DraftDay { get; set; }

        /// <summary>
        /// 기안일자
        /// </summary>
        public DateTime DraftDate { get; set; }

        /// <summary>
        /// 시행일자
        /// </summary>
        public DateTime ExecutionDate { get; set; }


        /// <summary>
        /// 부가세 여부 
        /// </summary>
        public string Vat { get; set; }

        /// <summary>
        /// 부가세
        /// </summary>
        public double VatAcount { get; set; }

        /// <summary>
        /// 총합계 A
        /// </summary>
        public int TotalAcount { get; set; }

        /// <summary>
        /// 가상
        /// </summary>
        public string TA { get; set; }

        /// <summary>
        /// 수정일
        /// </summary>
        public DateTime ModifyDate { get; set; }

        /// <summary>
        /// 수정자 아이피
        /// </summary>
        public string ModifyIP { get; set; }

        /// <summary>
        /// 외부결재 여부
        /// </summary>
        public string OutDraft { get; set; }

        /// <summary>
        /// 기안기관
        /// </summary>
        public string Organization { get; set; }

        /// <summary>
        /// 결재 여부
        /// </summary>
        public string Approval { get; set; }

        /// <summary>
        /// 첨부파일 수
        /// </summary>
        public int Files_Count { get; set; }
    }

    public class DraftDetailEntity
    {
        /// <summary>
        /// 번호
        /// </summary>
        public int Aid { get; set; }

        /// <summary>
        /// 사업장 코드
        /// </summary>
        public string AptCode { get; set; }

        /// <summary>
        /// 기안자 아이디
        /// </summary>
        public string UserCode { get; set; }

        /// <summary>
        /// 품명 A
        /// </summary>
        public string Article { get; set; }

        /// <summary>
        /// 규격 및 재질 A
        /// </summary>
        public string Goods { get; set; }

        /// <summary>
        /// 수량 A
        /// </summary>
        public double Quantity { get; set; }

        /// <summary>
        /// 단가 A
        /// </summary>
        public double UnitCost { get; set; }

        /// <summary>
        /// 합계 A
        /// </summary>
        public double SupplyPrice { get; set; }

        /// <summary>
        /// 부가세 여부 
        /// </summary>
        public string Vat { get; set; }

        /// <summary>
        /// 부가세 여부 
        /// </summary>
        public double VatAcount { get; set; }

        /// <summary>
        /// 총합계 A
        /// </summary>
        public double TotalAcount { get; set; }

        /// <summary>
        /// 기안 번호
        /// </summary>
        public int ParentAid { get; set; }

        /// <summary>
        /// 입력자 아이피
        /// </summary>
        public string PostIP { get; set; }
    }

    public class DraftAttachEntity
    {
        /// <summary>
        /// 번호
        /// </summary>
        public int Aid { get; set; }

        /// <summary>
        /// 사업장 코드
        /// </summary>
        public string AptCode { get; set; }

        /// <summary>
        /// 기안자 아이디
        /// </summary>
        public string UserCode { get; set; }

        /// <summary>
        /// 문서명
        /// </summary>
        public string Attachment { get; set; }

        /// <summary>
        /// 포함된 문서 수
        /// </summary>
        public int DNo { get; set; }

        /// <summary>
        /// 번호
        /// </summary>
        public int ParentAid { get; set; }
    }
}
