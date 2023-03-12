using System;

namespace Works
{
    /// <summary>
    /// 작업 보고서 
    /// </summary>
    public class Works_Entity
    {        
        /// <summary>
        /// 업무 일련번호
        /// </summary>
        public int Num { get; set; }

        /// <summary>
        /// 사업장 코드
        /// </summary>
        public string AptCode { get; set; }

        /// <summary>
        /// 접수 년도
        /// </summary>
        public string svYear { get; set; }

        /// <summary>
        /// 접수 월
        /// </summary>
        public string svMonth { get; set; }

        /// <summary>
        /// 접수 일
        /// </summary>
        public string svDay { get; set; }

        /// <summary>
        /// 접수 시간
        /// </summary>
        public string svClock { get; set; }


        /// <summary>
        /// 접수 분
        /// </summary>
        public string svMinute { get; set; }

        /// <summary>
        /// 작업지시자
        /// </summary>
        public string svDirect { get; set; }

        /// <summary>
        /// 분류코드
        /// </summary>
        public string svBloomCode { get; set; }

        /// <summary>
        /// 대분류
        /// </summary>
        public string svBloomA { get; set; }

        /// <summary>
        /// 중분류
        /// </summary>
        public string svBloomB { get; set; }

        /// <summary>
        /// 소분류
        /// </summary>
        public string svBloomC { get; set; }

        /// <summary>
        /// 품목 분류
        /// </summary>
        public string svBloom { get; set; }

        /// <summary>
        /// 접수 부서
        /// </summary>
        public string svPost { get; set; }

        /// <summary>
        /// 접수자 
        /// </summary>
        public string svReceiver { get; set; }

        /// <summary>
        /// 작업 지시내용
        /// </summary>
        public string svContent { get; set; }

        /// <summary>
        /// 접수 입력 일
        /// </summary>
        public DateTime PostDate { get; set; }

        /// <summary>
        /// 작업접수자 아이피
        /// </summary>
        public string PostIP { get; set; }

        /// <summary>
        /// 첨부파일
        /// </summary>
        public string ComFileName { get; set; }
        public int ComFileSize { get; set; }
        public string ComFileName2 { get; set; }
        public int ComFileSize2 { get; set; }
        public string ComFileName3 { get; set; }
        public int ComFileSize3 { get; set; }
        public string ComFileName4 { get; set; }
        public int ComFileSize4 { get; set; }
        public string ComFileName5 { get; set; }
        public int ComFileSize5 { get; set; }

        /// <summary>
        /// 작업일지 입력자
        /// </summary>
        public string UserIDW { get; set; }

        /// <summary>
        /// 비용 발생 여부
        /// </summary>
        public string svMoney { get; set; }

        /// <summary>
        /// 작업비용
        /// </summary>
        public int svCost { get; set; }

        /// <summary>
        /// 작업 비용 총액
        /// </summary>
        public int svAmount { get; set; }

        /// <summary>
        /// 작업 일 수
        /// </summary>
        public string svWorkDaily { get; set; }

        /// <summary>
        /// 작업 년
        /// </summary>
        public string subYear { get; set; }

        /// <summary>
        /// 작업 월
        /// </summary>
        public string subMonth { get; set; }

        /// <summary>
        /// 작업일
        /// </summary>
        public string subDay { get; set; }

        /// <summary>
        /// 작업 시간
        /// </summary>
        public string subClock { get; set; }

        /// <summary>
        /// 작업 분
        /// </summary>
        public string subMinute { get; set; }

        /// <summary>
        /// 외부작업 코드
        /// </summary>
        public string Scw_Code { get; set; }

        /// <summary>
        /// 외부작업 여부
        /// </summary>
        public string svOutViw { get; set; }

        /// <summary>
        /// 외부작업자
        /// </summary>
        public string svOutName { get; set; }

        /// <summary>
        /// 외부업체 전화
        /// </summary>
        public string svOutTelCom { get; set; }

        /// <summary>
        /// 외부업체 명
        /// </summary>
        public string svOutNameCom { get; set; }

        /// <summary>
        /// 빈셀
        /// </summary>
        public string innViw { get; set; }

        /// <summary>
        /// 작업자
        /// </summary>
        public string svWorkerName { get; set; }

        /// <summary>
        /// 작업자 수
        /// </summary>
        public int svWorkerCount { get; set; }

        /// <summary>
        /// 작업 부서
        /// </summary>
        public string svWorkPost { get; set; }

        /// <summary>
        /// 작업 내용
        /// </summary>
        public string WorkContent { get; set; }

        /// <summary>
        /// 만족도
        /// </summary>
        public string apSatisfaction { get; set; }

        /// <summary>
        /// 수정일
        /// </summary>
        public DateTime ModifyDate { get; set; }

        /// <summary>
        /// 수정아이피
        /// </summary>
        public string ModifyIP { get; set; }

        /// <summary>
        /// 작업 사진(첨부파일)
        /// </summary>
        public string ComFileName6 { get; set; }
        public int ComFileSize6 { get; set; }
        public string ComFileName7 { get; set; }
        public int ComFileSize7 { get; set; }
        public string ComFileName8 { get; set; }
        public int ComFileSize8 { get; set; }
        public string ComFileName9 { get; set; }
        public int ComFileSize9 { get; set; }
        public string ComFileName10 { get; set; }
        public int ComFileSize10 { get; set; }

        /// <summary>
        /// 코멘트 수
        /// </summary>
        public int ComCommentCount { get; set; }

        /// <summary>
        /// 공동주택 코드
        /// </summary>
        public string ComAlias { get; set; }

        /// <summary>
        /// 공동주택 명
        /// </summary>
        public string Apt_Name { get; set; }

        /// <summary>
        /// 입력자 아이디
        /// </summary>
        public string UserIDM { get; set; }

        /// <summary>
        /// 작업완료 여부
        /// </summary>
        public string Complete { get; set; }

        /// <summary>
        /// 결제 여부
        /// </summary>
        public string Conform { get; set; }

        public string Privater { get; set; }

    }

    /// <summary>
    /// 작업내용 속성
    /// </summary>
    public class WorksSub_Entity
    {
        /// <summary>
        /// 일련번호
        /// </summary>
        public int Aid { get; set; }

        /// <summary>
        /// 식별코드(외래)
        /// </summary>
        public string Service_Code { get; set; }

        /// <summary>
        /// 식별 이름(외래)
        /// </summary>
        public string Net_Group { get; set; }

        /// <summary>
        /// 공동주택 식별코드
        /// </summary>
        public string AptCode { get; set; }

        /// <summary>
        /// 소요비용
        /// </summary>
        public int Cost { get; set; }

        /// <summary>
        /// 작업횟수
        /// </summary>
        public int Amount { get; set; }

        /// <summary>
        /// 작업년도
        /// </summary>
        public string subYear { get; set; }

        /// <summary>
        /// 작업월
        /// </summary>
        public string subMonth { get; set; }

        /// <summary>
        /// 작업일
        /// </summary>
        public string subDay { get; set; }

        /// <summary>
        /// 작업일
        /// </summary>
        public DateTime WorkDate { get; set; }

        /// <summary>
        /// 외주여부
        /// </summary>
        public string Out_In_Viw { get; set; }

        /// <summary>
        /// 외부업체 코드
        /// </summary>
        public string Scw_Code { get; set; }

        /// <summary>
        /// 담당자
        /// </summary>
        public string OutCorCharger { get; set; }

        /// <summary>
        /// 외부 업체 연락처
        /// </summary>
        public string OutCorMobile { get; set; }

        /// <summary>
        /// 외주업체 명
        /// </summary>
        public string OutCorName { get; set; }

        /// <summary>
        /// 작업자
        /// </summary>
        public string WorkerName { get; set; }

        /// <summary>
        /// 작업자 수
        /// </summary>
        public int WorkerCount { get; set; }

        /// <summary>
        /// 작업부서
        /// </summary>
        public string WorkPost { get; set; }

        /// <summary>
        /// 작업내용
        /// </summary>
        public string WorkContent { get; set; }

        /// <summary>
        /// 완료여부
        /// </summary>
        public string Work_Complete { get; set; }

        /// <summary>
        /// 만족도
        /// </summary>
        public string Satisfaction { get; set; }

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
        /// 입력자 아이디
        /// </summary>
        public string UserID { get; set; }

        /// <summary>
        /// 삭제여부
        /// </summary>
        public string Del { get; set; }
    }

}
