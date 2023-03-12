using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Erp_Apt_Lib.BidTender
{
    /// <summary>
    /// 1. 입찰 기본 정보
    /// </summary>
    public class BidTender
    {
        public int BidTenderAid { get; set; }
        /// <summary>
        /// 선정지침 코드
        /// </summary>
        public int LawCode { get; set; }
        /// <summary>
        /// 입찰명
        /// </summary>
        public string BidTenderName { get; set; }
        /// <summary>
        /// 입찰 분류
        /// </summary>
        public string BidSort { get; set; }
        /// <summary>
        /// 입찰상세분류
        /// </summary>
        public string BidCategory { get; set; }
        /// <summary>
        /// 공동주택코드
        /// </summary>
        public string AptCode { get; set; }
        /// <summary>
        /// 입찰방법
        /// </summary>
        public string Bid { get; set; }
        /// <summary>
        /// 낙찰방법
        /// </summary>
        public string Tender { get; set; }
        /// <summary>
        /// 입찰일
        /// </summary>
        public DateTime BidDate { get; set; }
        /// <summary>
        /// 낙찰일
        /// </summary>
        public DateTime BidingDate { get; set; }
        /// <summary>
        /// 심사일
        /// </summary>
        public DateTime QualificationDate { get; set; }
        /// <summary>
        /// 상세
        /// </summary>
        public string Details { get; set; }
        /// <summary>
        /// 입력일
        /// </summary>
        public DateTime PostDate { get; set; }
        /// <summary>
        /// 입력자 아이피
        /// </summary>
        public string PostIP { get; set; }
        /// <summary>
        /// 입력자
        /// </summary>
        public string User_Code { get; set; }
        /// <summary>
        /// 삭제여부
        /// </summary>
        public string del { get; set; }
    }

    /// <summary>
    /// 2. 입찰분류 정보
    /// </summary>
    public class BidSort
    {
        public int BidSortAid { get; set; }
        /// <summary>
        /// 입찰종류 명
        /// </summary>
        public string SortName { get; set; }
        /// <summary>
        /// 상위 식별코드
        /// </summary>
        public string UpCode { get; set; }
        /// <summary>
        /// 단계
        /// </summary>
        public int Spep { get; set; }
        /// <summary>
        /// 입찰종료 설명
        /// </summary>
        public string Details { get; set; }
        /// <summary>
        /// 입력일
        /// </summary>
        public DateTime PostDate { get; set; }
        /// <summary>
        /// 입력자 아이디
        /// </summary>
        public string UserCode { get; set; }
        /// <summary>
        /// 삭제여부
        /// </summary>
        public string del { get; set; }
    }

    /// <summary>
    /// 3. 입찰선택
    /// 일반, 제한, 수의 계약 선택
    /// </summary>
    public class BidSelect
    {
        /// <summary>
        /// 식별코드
        /// </summary>
        public int BidSelectAid { get; set; }
        /// <summary>
        /// 입찰 식별코드
        /// </summary>
        public int BidTenderCode { get; set; }
        /// <summary>
        /// 입찰종류 식별코드
        /// </summary>
        public int BidSortCode { get; set; }
        /// <summary>
        /// 입찰종류 명
        /// </summary>
        public string BidSortName { get; set; }
        /// <summary>
        /// 상세
        /// </summary>
        public string Details { get; set; }
        /// <summary>
        /// 입력일
        /// </summary>
        public DateTime PostDate { get; set; }
        /// <summary>
        /// 입력자 아이피
        /// </summary>
        public string PostIP { get; set; }
        /// <summary>
        /// 입력자
        /// </summary>
        public string User_Code { get; set; }
    }

    /// <summary>
    /// 4. 제한경쟁 선택인 경우
    /// </summary>
    public class LimitCompetition
    {
        /// <summary>
        /// 식별코드
        /// </summary>
        public int LimitCompetitionAid { get; set; }
        /// <summary>
        /// 공동주택 식별코드
        /// </summary>
        public string AptCode { get; set; }
        /// <summary>
        /// 입찰 식별코드
        /// </summary>
        public string BidTenderCode { get; set; }
        /// <summary>
        /// 입찰방법 식별코드
        /// </summary>
        public string BidSelectCode { get; set; }
        /// <summary>
        /// 자본금
        /// </summary>
        public double Capital { get; set; }
        /// <summary>
        /// 실적
        /// </summary>
        public string Performance { get; set; }
        /// <summary>
        /// 기술능력
        /// </summary>
        public string Technical { get; set; }
        /// <summary>
        /// 상세
        /// </summary>
        public string Details { get; set; }
        /// <summary>
        /// 입력일
        /// </summary>
        public DateTime PostDate { get; set; }
        /// <summary>
        /// 입력자 아이피
        /// </summary>
        public string PostIP { get; set; }
        /// <summary>
        /// 입력자
        /// </summary>
        public string User_Code { get; set; }
        /// <summary>
        /// 삭제여부
        /// </summary>
        public string del { get; set; }
    }

    /// <summary>
    /// 5. 수의계약 선택시 내용
    /// </summary>
    public class PrivateContract
    {
        public int PrivateContractAid { get; set; }
        /// <summary>
        /// 공동주택 식별코드
        /// </summary>
        public string AptCode { get; set; }
        /// <summary>
        /// 입찰 식별코드
        /// </summary>
        public string BidTenderCode { get; set; }
        /// <summary>
        /// 입찰방법 식별코드
        /// </summary>
        public string BidSelectCode { get; set; }
        /// <summary>
        /// 구분(재계약, 수의계약)선택
        /// </summary>
        public string PrivateDivision { get; set; }
        /// <summary>
        /// 업체 코드
        /// </summary>
        public string Company_Code { get; set; }
        /// <summary>
        /// 계약금액
        /// </summary>
        public string ContractSum { get; set; }
        /// <summary>
        /// 계약 시작일
        /// </summary>
        public DateTime ContractStartDate { get; set; }
        /// <summary>
        /// 계약 종료일
        /// </summary>
        public DateTime ContractEndDate { get; set; }

        /// 상세
        /// </summary>
        public string Details { get; set; }

        /// <summary>
        /// 입력일
        /// </summary>
        public DateTime PostDate { get; set; }
        /// <summary>
        /// 입력자 아이피
        /// </summary>
        public string PostIP { get; set; }
        /// <summary>
        /// 입력자
        /// </summary>
        public string UserCode { get; set; }
        /// <summary>
        /// 삭제여부
        /// </summary>
        public string del { get; set; }

    }

    /// <summary>
    /// 6. 업무 평가(수의계약 업무 평가)
    /// 수의계약 표준평가표에 따라 관리규약으로 정한 평가표에 의하여 평가한 표
    /// </summary>
    public class BusinessEvaluation
    {
        public int BusinessEvaluationAid { get; set; }
        /// <summary>
        /// 공동주택 식별코드
        /// </summary>
        public string AptCode { get; set; }
        /// <summary>
        /// 입찰 식별코드
        /// </summary>
        public string BidTenderCode { get; set; }
        /// <summary>
        /// 입찰방법 식별코드
        /// </summary>
        public string BidSelectCode { get; set; }
        /// <summary>
        /// 수의계약 코드
        /// </summary>
        public string PrivateContractCode { get; set; }
        /// <summary>
        /// 분류(업무평가 분류에서 선택)
        /// </summary>
        public string Sort { get; set; }
        /// <summary>
        /// 세분류(업무평가 분류에서 선택)
        /// </summary>
        public string Category { get; set; }
        /// <summary>
        /// 항목(업무평가 항목)
        /// </summary>
        public string Article { get; set; }

        /// <summary>
        /// 평가기준
        /// </summary>
        public string Criteria { get; set; }
        /// <summary>
        /// 배점
        /// </summary>
        public string Distribution { get; set; }
        /// <summary>
        /// 점수
        /// </summary>        
        public int Score { get; set; }
        /// <summary>
        /// 평가 이유
        /// </summary>
        public string Reson { get; set; }
        /// 상세
        /// </summary>
        public string Details { get; set; }
        /// <summary>
        /// 입력일
        /// </summary>
        public DateTime PostDate { get; set; }
        /// <summary>
        /// 입력자 아이피
        /// </summary>
        public string PostIP { get; set; }
        /// <summary>
        /// 입력자
        /// </summary>
        public string UserCode { get; set; }
        /// <summary>
        /// 삭제여부
        /// </summary>
        public string del { get; set; }
    }

    /// <summary>
    /// 7. 수의계약 표준 평가표
    /// 관리규약 및 선정지침, 법령으로 정해지는 수의계약 표준 평가표
    /// 현재는 관리규약준칙으로 정하고 있음. 
    /// </summary>
    public class BusinesEvaluationSort
    {
        public int BusinesEvaluationSortAid { get; set; }

        /// <summary>
        /// 입찰분류 식별코드
        /// 위탁, 공사, 용역, 기타 구분 선택
        /// </summary>
        public int BidSortCode { get; set; }
        /// <summary>
        /// 위탁, 공사, 용역, 기타 구분 선택
        /// </summary>
        public string BidSortName { get; set; }
        /// <summary>
        /// 평가 항목
        /// </summary>
        public string Article { get; set; }
        /// <summary>
        /// 평가 기준
        /// </summary>
        public string Criteria { get; set; }
        /// <summary>
        /// 해당 지역
        /// </summary>
        public string Region { get; set; }
        /// <summary>
        /// 공포일
        /// </summary>
        public DateTime PromulgationDate { get; set; }
        /// <summary>
        /// 입력일
        /// </summary>
        public DateTime PostDate { get; set; }
        /// <summary>
        /// 입력자 아이피
        /// </summary>
        public string PostIP { get; set; }
        /// <summary>
        /// 입력자
        /// </summary>
        public string UserCode { get; set; }
    }

    /// <summary>
    /// 8. 수의계약 견적
    /// </summary>
    public class Estimate
    {
        public int EstimateAid { get; set; }
        /// <summary>
        /// 공동주택 식별코드
        /// </summary>
        public string AptCode { get; set; }
        /// <summary>
        /// 입찰 식별코드
        /// 외래키
        /// </summary>
        public string BidTenderCode { get; set; }
        /// <summary>
        /// 입찰방법 식별코드
        /// 외래키
        /// </summary>
        public string BidMethodCode { get; set; }
        /// <summary>
        /// 수의계약 코드
        /// 외래키
        /// </summary>
        public string PrivateContractCode { get; set; }
        /// <summary>
        /// 구분(위탁, 용역, 공사, 기타)
        /// 외래키
        /// </summary>
        public int BidSortCode { get; set; }
        /// <summary>
        /// 견적명
        /// </summary>
        public string EstimateName { get; set; }
        /// <summary>
        /// 견적업체
        /// </summary>
        public string Company_Code { get; set; }
        /// <summary>
        /// 견적금액
        /// </summary>
        public double EstiMateSum { get; set; }
        /// 상세
        /// </summary>
        public string Details { get; set; }
        /// <summary>
        /// 입력일
        /// </summary>
        public DateTime PostDate { get; set; }
        /// <summary>
        /// 입력자 아이피
        /// </summary>
        public string PostIP { get; set; }
        /// <summary>
        /// 입력자
        /// </summary>
        public string UserCode { get; set; }
    }

    /// <summary>
    /// 9. 낙찰방법 기본 정보
    /// 최저가, 최고가, 적격심사 등
    /// </summary>
    public class BidingMethod
    {
        public int BidingMethodAid { get; set; }
        /// <summary>
        /// 낙찰방법 명
        /// </summary>
        public string BidingMethodName { get; set; }
        /// <summary>
        /// 선정지침 식별코드
        /// </summary>
        [Required]
        public int LawCode { get; set; }
        /// <summary>
        /// 상세
        /// </summary>
        public string Details { get; set; }
        /// <summary>
        /// 입력일
        /// </summary>
        public DateTime PostDate { get; set; }
        /// <summary>
        /// 입력자 아이피
        /// </summary>
        public string PostIP { get; set; }
        /// <summary>
        /// 입력자
        /// </summary>
        public string UserCode { get; set; }
        /// <summary>
        /// 삭제여부
        /// </summary>
        public string del { get; set; }
    }

    /// <summary>
    /// 9_1. 낙찰방법 선택
    /// </summary>
    public class BidingSelect
    {
        public int BidingSelectAid { get; set; }
        /// <summary>
        /// 입찰방법 식별코드
        /// </summary>
        public int BidTenerCode { get; set; }
        /// <summary>
        /// 낙찰방법 식별코드
        /// </summary>
        public int BidingMethodCode { get; set; }
        /// <summary>
        /// 공동주택 식별코드
        /// </summary>
        public string AptCode { get; set; }
        /// <summary>
        /// 설명
        /// </summary>
        public string Details { get; set; }
        /// <summary>
        /// 입력일
        /// </summary>
        public DateTime PostDate { get; set; }
        /// <summary>
        /// 입력자 아이디
        /// </summary>
        public string UserCode { get; set; }
    }

    /// <summary>
    /// 10. 적격심사 기본 정보
    /// 적격심사 기본 저장 
    /// </summary>
    public class QualificationStandard
    {
        public int QualificationStandardAid { get; set; }
        /// <summary>
        /// 공동주택 식별코드
        /// </summary>
        public string AptCode { get; set; }
        /// <summary>
        /// 적격심사 명
        /// </summary>
        public string QualificationStandardName { get; set; }
        /// <summary>
        /// 표준평가표 선택
        /// </summary>
        public int StandardDistributionCode { get; set; }
        /// <summary>
        /// 상세
        /// </summary>
        public string Details { get; set; }
        /// <summary>
        /// 입력일
        /// </summary>
        public DateTime PostDate { get; set; }
        /// <summary>
        /// 입력자 아이피
        /// </summary>
        public string PostIP { get; set; }
        /// <summary>
        /// 입력자
        /// </summary>
        public string UserCode { get; set; }
    }

    /// <summary>
    /// 11. 적격심사 평가자
    /// 적격심사를 위하여 선택된 평가자
    /// </summary>
    public class EvaluationMan
    {
        /// <summary>
        /// 평가자 식별코드
        /// </summary>
        public int EvaluationManAid { get; set; }
        /// <summary>
        /// 공동주택 식별코드
        /// </summary>
        public string AptCode { get; set; }
        /// <summary>
        /// 입찰 식별코드
        /// </summary>
        public int BidTenderCode { get; set; }
        /// <summary>
        /// 적격심사 식별코드
        /// </summary>
        public int QualificationStandardCode { get; set; }
        /// <summary>
        /// 계약 주체
        /// </summary>
        public string ContractSort { get; set; }
        /// <summary>
        /// 평가자 아아디
        /// </summary>
        public string UserId { get; set; }
        /// <summary>
        /// 평가자 명
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 동명
        /// </summary>
        public string Dong { get; set; }
        /// <summary>
        /// 호명
        /// </summary>
        public string Ho { get; set; }
        /// <summary>
        /// 부서
        /// </summary>
        public string Post { get; set; }
        /// <summary>
        /// 직책
        /// </summary>
        public string Duty { get; set; }
        /// <summary>
        /// 상세
        /// </summary>
        public string Details { get; set; }
        /// <summary>
        /// 입력일자
        /// </summary>
        public DateTime PostDate { get; set; }
        /// <summary>
        /// 입력자 아이피
        /// </summary>
        public string PostIp { get; set; }
        /// <summary>
        /// 입력자 아이디
        /// </summary>
        public string UserCode { get; set; }
    }

    /// <summary>
    /// 12. 적격심사 배점표
    /// 관리규약 등에 규정 혹은 관리규약에 규정이 없어 입찰 시에 정한 배점표
    /// </summary>
    public class DistributionMark
    {
        public int DistributionMarkAid { get; set; }
        /// <summary>
        /// 공동주택 식별코드
        /// </summary>
        public string AptCode { get; set; }
        /// <summary>
        /// 적격심사 기본 정보 코드
        /// </summary>
        public int QualificationStandardCode { get; set; }
        /// <summary>
        /// 낙찰방법 코드
        /// </summary>
        public int BidingCode { get; set; }
        /// <summary>
        /// 표준평가표 코드
        /// </summary>
        public int StandarddistributionMarkCode { get; set; }
        /// <summary>
        /// 입찰종류 코드
        /// </summary>
        public int BidSortCode { get; set; }
        /// <summary>
        /// 항목
        /// </summary>
        public string Article { get; set; }
        /// <summary>
        /// 배점
        /// </summary>
        public double Distribution { get; set; }
        /// <summary>
        /// 설명
        /// </summary>
        public string Details { get; set; }
        /// <summary>
        /// 기타
        /// </summary>
        public string Etc { get; set; }
        /// <summary>
        /// 입력일
        /// </summary>
        public DateTime PostDate { get; set; }
        /// <summary>
        /// 입력자 코드
        /// </summary>
        public string UserCode { get; set; }
    }

    /// <summary>
    /// 13. 적격심사 평가표
    /// 선택된 평가자 준 평가표
    /// </summary>
    public class EvaluationMark
    {
        public int EvaluationMarkSAid { get; set; }
        /// <summary>
        /// 공동주택 식별코드
        /// </summary>
        public string AptCode { get; set; }
        /// <summary>
        /// 입찰 식별코드
        /// </summary>
        public int BidTenderCode { get; set; }
        /// <summary>
        /// 적격심사 기본정보 코드
        /// </summary>
        public int QualificationStandardCode { get; set; }
        /// <summary>
        /// 낙찰방법 기본정보
        /// </summary>
        public int BidingCode { get; set; }
        /// <summary>
        /// 평가자 코드
        /// </summary>
        public int EvaluationManCode { get; set; }
        /// <summary>
        /// 배점표 코드
        /// </summary>
        public int DistributionMarkCode { get; set; }
        /// <summary>
        /// 항목
        /// </summary>
        public string Article { get; set; }
        /// <summary>
        /// 점수
        /// </summary>
        public double Point { get; set; }
        /// <summary>
        /// 설명
        /// </summary>
        public string Details { get; set; }
        /// <summary>
        /// 입력일
        /// </summary>
        public DateTime PostDate { get; set; }
        /// <summary>
        /// 입력자 코드
        /// </summary>
        public string UserCode { get; set; }

    }

    /// <summary>
    /// 14.적격심사 표준평가표
    /// 선정지침에 규정된 평가표
    /// </summary>
    public class StandarddistributionMark
    {
        /// <summary>
        /// 적격심사 표준평가표 식별코드        
        /// </summary>
        public int StandarddistributionMarkAid { get; set; }
        /// <summary>
        /// 선정지침 식별코드
        /// </summary>
        public int LawCode { get; set; }
        /// <summary>
        /// 입찰종류 코드
        /// </summary>
        public int BidSortCode { get; set; }
        /// <summary>
        /// 항목
        /// </summary>
        public string Article { get; set; }
        /// <summary>
        /// 분류
        /// </summary>
        public string Sort { get; set; }
        /// <summary>
        /// 배점
        /// </summary>
        public int Distribution { get; set; }
        /// <summary>
        /// 세부배점
        /// </summary>
        public int DetailsDistribution { get; set; }
        /// <summary>
        /// 평가 내용
        /// </summary>
        public string DistributionDetails { get; set; }
        /// <summary>
        /// 제출서류
        /// </summary>
        public string InputPaper { get; set; }
        /// <summary>
        /// 참고 설명
        /// </summary>
        public string Details { get; set; }
        /// <summary>
        /// 입력일
        /// </summary>
        public DateTime PostDate { get; set; }
        /// <summary>
        /// 입력자 코드
        /// </summary>
        public string UserCode { get; set; }
    }

    /// <summary>
    /// 15. 제출서류
    /// </summary>
    public class InputReport
    {
        public int InputReportAid { get; set; }
        /// <summary>
        /// 선정지침 코드
        /// </summary>
        public int LawCode { get; set; }
        /// <summary>
        /// 법정 및 임의
        /// </summary>
        public string Sort { get; set; }
        /// <summary>
        /// 서류명
        /// </summary>
        public string PapersName { get; set; }
        /// <summary>
        /// 무효 또는 영점 처리
        /// </summary>
        public string Nullity { get; set; }
        /// <summary>
        /// 설명
        /// </summary>
        public string Details { get; set; }
        /// <summary>
        /// 입력일
        /// </summary>
        public DateTime PostDate { get; set; }
        /// <summary>
        /// 입력자 코드
        /// </summary>
        public string UserCode { get; set; }
    }

    /// <summary>
    /// 16. 입찰제출서류(입찰에 제출할 서류 지정)
    /// </summary>
    public class InputPaper
    {
        /// <summary>
        /// 응찰자 제출서류 식별코드
        /// </summary>
        public int InputPaperAid { get; set; }
        /// <summary>
        /// 공동주택 식별코드
        /// </summary>
        public string AptCode { get; set; }
        /// <summary>
        /// 입찰 식별코드
        /// </summary>
        public string BidTenderCode { get; set; }
        /// <summary>
        /// 제출서류 코드
        /// </summary>
        public string InputReportCode { get; set; }
        /// <summary>
        /// 서류명
        /// </summary>
        public string PaperName { get; set; }
        /// <summary>
        /// 필수여부
        /// </summary>
        public string Necesary { get; set; }
        /// <summary>
        /// 응찰업체명
        /// </summary>
        public string Company_Name { get; set; }
        /// <summary>
        /// 응찰업체 식별코드
        /// </summary>
        public string Company_Code { get; set; }
        /// <summary>
        /// 상세정보
        /// </summary>
        public string Details { get; set; }
        /// <summary>
        /// 입력일
        /// </summary>
        public DateTime PostDate { get; set; }
        /// <summary>
        /// 입력자 아이피
        /// </summary>
        public string PostIp { get; set; }
        /// <summary>
        /// 입력자 코드
        /// </summary>
        public string UserCode { get; set; }
    }

    /// <summary>
    /// 17. 사업자 선정지침 선택
    /// </summary>
    public class Law
    {
        /// <summary>
        /// 식별코드
        /// </summary>
        public int LawAid { get; set; }
        /// <summary>
        /// 법령명
        /// </summary>
        public string LawName { get; set; }
        /// <summary>
        /// 법률 식별번호
        /// </summary>
        public string LawCode { get; set; }
        /// <summary>
        /// 공포일
        /// </summary>
        public DateTime ProclamationDate { get; set; }
        /// <summary>
        /// 기관
        /// </summary>
        public string Organization { get; set; }
        /// <summary>
        /// 설명
        /// </summary>
        public string Details { get; set; }
        /// <summary>
        /// 종류
        /// </summary>
        public string Sort { get; set; }
        /// <summary>
        /// 상위법규
        /// </summary>
        public string UpLaw { get; set; }
        /// <summary>
        /// 입력일
        /// </summary>
        public DateTime PostDate { get; set; }
        /// <summary>
        /// 입력자 아이피
        /// </summary>
        public string PostIp { get; set; }
        /// <summary>
        /// 입력자 식별코드
        /// </summary>
        public string UserCode { get; set; }
    }

    /// <summary>
    /// 18. 유의사항
    /// </summary>
    public class CrucialNote
    {
        /// <summary>
        /// 유의사항 식별코드
        /// </summary>
        public int CrucialNoteAid { get; set; }
        /// <summary>
        /// 입찰 식별코드
        /// </summary>
        public string BidTenderCode { get; set; }
        /// <summary>
        /// 유의사항 명
        /// </summary>
        public string CucialNateName { get; set; }
        /// <summary>
        /// 유의사상
        /// </summary>
        public string Note { get; set; }
        /// <summary>
        /// 설명
        /// </summary>
        public string Details { get; set; }
        /// <summary>
        /// 입력일
        /// </summary>
        public DateTime PostDate { get; set; }
        /// <summary>
        /// 입력자 식별코드
        /// </summary>
        public string UserCode { get; set; }

    }

    /// <summary>
    /// 19. 현장설명회 기본정보
    /// </summary>
    public class PlaceExplanation
    {
        public int PlaceExplanationAid { get; set; }
        /// <summary>
        /// 설명회 미참석 무효여부
        /// </summary>
        public string Nullity { get; set; }
        /// <summary>
        /// 설명회 일자
        /// </summary>
        public DateTime ExplanationDate { get; set; }
        /// <summary>
        /// 설명회 장소
        /// </summary>
        public string ExplanationPlace { get; set; }
        /// <summary>
        /// 설명회 방법
        /// </summary>
        public string ExplanationMethod { get; set; }
        /// <summary>
        /// 설명내용
        /// </summary>
        public string Details { get; set; }
        /// <summary>
        /// 기타
        /// </summary>
        public string Etc { get; set; }
        /// <summary>
        /// 입력일
        /// </summary>
        public DateTime PostDate { get; set; }
        /// <summary>
        /// 입력자 아이피
        /// </summary>
        public string PostIp { get; set; }
        /// <summary>
        /// 입력자 아이디
        /// </summary>
        public string UserCode { get; set; }
    }

    /// <summary>
    /// T1.응찰정보
    /// </summary>
    public class Tender
    {
        public int TenderAid { get; set; }
        /// <summary>
        /// 입찰기본정보 식별코드
        /// </summary>
        public string BidTenderCode { get; set; }
        /// <summary>
        /// 현장설명회 참석코드
        /// </summary>
        public string PlacePresenceCode { get; set; }
        /// <summary>
        /// 업체 코드
        /// </summary>
        public string CompanyCode { get; set; }
        /// <summary>
        /// 설명회 참석여부
        /// </summary>
        public string ExplanationBeing { get; set; }
        /// <summary>
        /// 제출방법
        /// </summary>
        public string InputMethod { get; set; }
        /// <summary>
        /// 응찰금액
        /// </summary>
        public double TenderSum { get; set; }
        /// <summary>
        /// 담당자
        /// </summary>
        public string ChargeMan { get; set; }
        /// <summary>
        /// 연락처
        /// </summary>
        public string Mobile { get; set; }
        /// <summary>
        /// 설명
        /// </summary>
        public string Details { get; set; }
        /// <summary>
        /// 입력일
        /// </summary>
        public DateTime PostDate { get; set; }
        /// <summary>
        /// 입력자 아이피
        /// </summary>
        public string PostIp { get; set; }
        /// <summary>
        /// 입력자
        /// </summary>
        public string UserCode { get; set; }
    }

    /// <summary>
    /// T2. 응찰서류
    /// </summary>
    public class TenderPaper
    {
        public int TenderPaperAid { get; set; }
        /// <summary>
        /// 제출서류 식별코드
        /// </summary>
        public string InputReportCode { get; set; }
        /// <summary>
        /// 응찰 식별코드
        /// </summary>
        public string TenderCode { get; set; }
        /// <summary>
        /// 서류명
        /// </summary>
        public string PaperName { get; set; }
        /// <summary>
        /// 제출여부
        /// </summary>
        public string Input { get; set; }
        /// <summary>
        /// 제출일
        /// </summary>
        public DateTime InputDate { get; set; }
        /// <summary>
        /// 설명
        /// </summary>
        public string Details { get; set; }
        /// <summary>
        /// 입력일
        /// </summary>
        public DateTime PostDate { get; set; }
        /// <summary>
        /// 입력자 아이디
        /// </summary>
        public string UserCode { get; set; }
    }

    /// <summary>
    /// 20.현장설명회 참석 정보
    /// </summary>
    public class PlacePresence
    {
        public int PlacePresenceAid { get; set; }
        /// <summary>
        /// 현장설명회 기본 정보 코드
        /// </summary>
        public string PlaceExplanationCode { get; set; }
        /// <summary>
        /// 입찰기본정보 코드
        /// </summary>
        public string BidTenderCode { get; set; }
        /// <summary>
        /// 업체코드
        /// </summary>
        public string CompanyCode { get; set; }
        /// <summary>
        /// 참석일
        /// </summary>
        public DateTime ExplanationDate { get; set; }
        /// <summary>
        /// 담당자
        /// </summary>
        public string ChargeMan { get; set; }
        /// <summary>
        /// 연락처
        /// </summary>
        public string Mobile { get; set; }
        /// <summary>
        /// 무효여부
        /// </summary>
        public string Nullity { get; set; }
        /// <summary>
        /// 설명
        /// </summary>
        public string Details { get; set; }
        /// <summary>
        /// 입력일
        /// </summary>
        public DateTime PostDate { get; set; }
        /// <summary>
        /// 입력자
        /// </summary>
        public string UserCode { get; set; }

    }
}
