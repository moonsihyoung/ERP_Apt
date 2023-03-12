using System;
using System.Collections.Generic;
using System.Text;

namespace Erp_Apt_Lib.Appeal
{
    /// <summary>
    /// 민원일지 분류  속성 목록
    /// </summary>
    public class Appeal_Bloom_Entity
    {
        /// <summary>
        /// 일련번호(민원)
        /// </summary>
        public int Num { get; set; }

        /// <summary>
        /// 사업장 코드
        /// </summary>
        public string Apt_Code { get; set; }

        /// <summary>
        /// 민원 분류 코드
        /// </summary>
        public string Bloom_Code { get; set; }

        /// <summary>
        /// 대 분류
        /// </summary>
        public string Sort { get; set; }

        /// <summary>
        /// 소 분류
        /// </summary>
        public string Asort { get; set; }

        /// <summary>
        /// 하자기간
        /// </summary>
        public int Period { get; set; }

        /// <summary>
        /// 자세한 설명
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 입력일
        /// </summary>
        public DateTime PostDate { get; set; }

        /// <summary>
        /// 입력자 아이피
        /// </summary>
        public string PostIP { get; set; }

        /// <summary>
        /// 수정일자
        /// </summary>
        public DateTime ModifyDate { get; set; }

        /// <summary>
        /// 수정자 아이피
        /// </summary>
        public string ModifyIP { get; set; }

        /// <summary>
        /// 사용여부
        /// </summary>
        public string Useing { get; set; }

        /// <summary>
        /// 첨부파일 이름
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// 파일 사이즈
        /// </summary>
        public int FileSize { get; set; }

        /// <summary>
        /// 첨부파일 이름2
        /// </summary>
        public string FileName2 { get; set; }

        /// <summary>
        /// 파일 사이즈2
        /// </summary>
        public int FileSize2 { get; set; }

        /// <summary>
        /// 첨부파일 이름3
        /// </summary>
        public string FileName3 { get; set; }

        /// <summary>
        /// 파일 사이즈3
        /// </summary>
        public int FileSize3 { get; set; }

        /// <summary>
        /// 첨부파일 이름4
        /// </summary>
        public string FileName4 { get; set; }

        /// <summary>
        /// 파일 사이즈4
        /// </summary>
        public int FileSize4 { get; set; }

        /// <summary>
        /// 첨부파일 이름5
        /// </summary>
        public string FileName5 { get; set; }

        /// <summary>
        /// 파일 사이즈5
        /// </summary>
        public int FileSize5 { get; set; }

        /// <summary>
        /// 입력자 아이디
        /// </summary>
        public string UserID { get; set; }

        /// <summary>
        /// 입력자 명
        /// </summary>
        public string UserName { get; set; }
    }

    /// <summary>
    /// 민원일지 속성 목록
    /// </summary>
    public class Appeal_Entity
    {
        /// <summary>
        /// 일련번호(민원)
        /// </summary>
        public int Num { get; set; }

        /// <summary>
        /// 사업장 코드
        /// </summary>
        public string AptCode { get; set; }

        /// <summary>
        /// 사업장 명
        /// </summary>
        public string AptName { get; set; }

        /// <summary>
        /// 공용 및 전용 여부 선택
        /// </summary>
        public string Private { get; set; }

        /// <summary>
        /// 민원 입력 년도
        /// </summary>
        public string apYear { get; set; }

        /// <summary>
        /// 민원 입력 월
        /// </summary>
        public string apMonth { get; set; }

        /// <summary>
        /// 민원 입력 일
        /// </summary>
        public string apDay { get; set; }

        /// <summary>
        /// 민원 입력 시
        /// </summary>
        public string apClock { get; set; }

        /// <summary>
        /// 민원 입력 분
        /// </summary>
        public string apMinute { get; set; }

        /// <summary>
        /// 민원 동
        /// </summary>
        public string apDongNo { get; set; }

        /// <summary>
        /// 민원 호
        /// </summary>
        public string apHoNo { get; set; }

        /// <summary>
        /// 민원인
        /// </summary>
        public string apName { get; set; }

        /// <summary>
        /// 민원인 손전화
        /// </summary>
        public string apHp { get; set; }

        /// <summary>
        /// 민원 접수 부서
        /// </summary>
        public string apPost { get; set; }

        /// <summary>
        /// 민원 분류 코드명
        /// </summary>
        public string Bloom_Code { get; set; }

        /// <summary>
        /// 민원 분류
        /// </summary>
        public string apTitle { get; set; }

        /// <summary>
        /// 민원 접수자
        /// </summary>
        public string apReciever { get; set; }

        /// <summary>
        /// 민원 내용
        /// </summary>
        public string apContent { get; set; }

        /// <summary>
        /// 민원 처리 여부
        /// </summary>
        public string apOk { get; set; }

        /// <summary>
        /// 민원 처리 년도
        /// </summary>
        public string subYear { get; set; }

        /// <summary>
        /// 민원 처리 월
        /// </summary>
        public string subMonth { get; set; }

        /// <summary>
        /// 민원처리 일
        /// </summary>
        public string subDay { get; set; }

        /// <summary>
        /// 민원처리 시간
        /// </summary>
        public string subClock { get; set; }

        /// <summary>
        /// 민원처리 분
        /// </summary>
        public string subMinute { get; set; }

        /// <summary>
        /// 미지정
        /// </summary>
        public string outViw { get; set; }

        /// <summary>
        /// 민원 외부처리자
        /// </summary>
        public string outName { get; set; }

        /// <summary>
        /// 민원처리자 연락처
        /// </summary>
        public string outTelCom1 { get; set; }

        /// <summary>
        /// 민원처리자 연락처 2
        /// </summary>
        public string outTelCom2 { get; set; }

        /// <summary>
        /// 민원처리자 연락처 3
        /// </summary>
        public string outTelCom3 { get; set; }

        /// <summary>
        /// 민원처리자 업체명
        /// </summary>
        public string outNameCom { get; set; }

        /// <summary>
        /// 민원처리 완료 여부
        /// </summary>
        public string innViw { get; set; }

        /// <summary>
        /// 결제 완료 여부
        /// </summary>
        public string Complete { get; set; }

        /// <summary>
        /// 민원처리자(내부)
        /// </summary>
        public string innName { get; set; }

        /// <summary>
        /// 민원처리 부서
        /// </summary>
        public string subPost { get; set; }

        /// <summary>
        /// 민원처리 내용
        /// </summary>
        public string innContent { get; set; }

        /// <summary>
        /// 민원처리 만족도
        /// </summary>
        public string apSatisfaction { get; set; }

        /// <summary>
        /// 민원 입력일
        /// </summary>
        public DateTime PostDate { get; set; }

        /// <summary>
        /// 민원입력자 아이피
        /// </summary>
        public string PostIP { get; set; }

        /// <summary>
        /// 민원처리일자
        /// </summary>
        public DateTime ModifyDate { get; set; }

        /// <summary>
        /// 민원처리 입력자 아이피
        /// </summary>
        public string ModifyIP { get; set; }

        /// <summary>
        /// 첨부파일 이름
        /// </summary>
        public string ComFileName { get; set; }

        /// <summary>
        /// 파일 사이즈
        /// </summary>
        public int ComFileSize { get; set; }

        /// <summary>
        /// 첨부파일 이름2
        /// </summary>
        public string ComFileName2 { get; set; }

        /// <summary>
        /// 파일 사이즈2
        /// </summary>
        public int ComFileSize2 { get; set; }

        /// <summary>
        /// 첨부파일 이름3
        /// </summary>
        public string ComFileName3 { get; set; }

        /// <summary>
        /// 파일 사이즈3
        /// </summary>
        public int ComFileSize3 { get; set; }

        /// <summary>
        /// 첨부파일 이름4
        /// </summary>
        public string ComFileName4 { get; set; }

        /// <summary>
        /// 파일 사이즈4
        /// </summary>
        public int ComFileSize4 { get; set; }

        /// <summary>
        /// 첨부파일 이름5
        /// </summary>
        public string ComFileName5 { get; set; }

        /// <summary>
        /// 파일 사이즈5
        /// </summary>
        public int ComFileSize5 { get; set; }

        /// <summary>
        /// 첨부파일 이름6
        /// </summary>
        public string ComFileName6 { get; set; }

        /// <summary>
        /// 파일 사이즈6
        /// </summary>
        public int ComFileSize6 { get; set; }

        /// <summary>
        /// 첨부파일 이름7
        /// </summary>
        public string ComFileName7 { get; set; }

        /// <summary>
        /// 파일 사이즈7
        /// </summary>
        public int ComFileSize7 { get; set; }

        /// <summary>
        /// 첨부파일 이름8
        /// </summary>
        public string ComFileName8 { get; set; }

        /// <summary>
        /// 파일 사이즈8
        /// </summary>
        public int ComFileSize8 { get; set; }

        /// <summary>
        /// 첨부파일 이름9
        /// </summary>
        public string ComFileName9 { get; set; }

        /// <summary>
        /// 파일 사이즈9
        /// </summary>
        public int ComFileSize9 { get; set; }

        /// <summary>
        /// 첨부파일 이름10
        /// </summary>
        public string ComFileName10 { get; set; }

        /// <summary>
        /// 파일 사이즈10
        /// </summary>
        public int ComFileSize10 { get; set; }

        /// <summary>
        /// 사업장 코드
        /// </summary>
        public string ComAlias { get; set; }

        /// <summary>
        /// 사업장 명
        /// </summary>
        public string ComTitle { get; set; }

        /// <summary>
        /// 미지정
        /// </summary>
        public string AppealViw { get; set; }

        /// <summary>
        /// 입력자 아이디
        /// </summary>
        public string apUserName { get; set; }


    }

    /// <summary>
    /// 민원 응대 상세 엔터티
    /// </summary>
    public class subAppeal_Entity
    {
        /// <summary>
        /// 식별코드
        /// </summary>
        public int subAid { get; set; }

        /// <summary>
        /// 접수번호
        /// </summary>
        public string apNum { get; set; }

        /// <summary>
        /// 공동주택 식별코드
        /// </summary>
        public string AptCode { get; set; }

        /// <summary>
        /// 공동주택 명
        /// </summary>
        public string AptName { get; set; }

        /// <summary>
        /// 민원작업일
        /// </summary>
        public DateTime subDate { get; set; }

        /// <summary>
        /// 민원작업년도
        /// </summary>
        public int subYear { get; set; }

        /// <summary>
        /// 민원 작업월
        /// </summary>
        public int subMonth { get; set; }

        /// <summary>
        /// 민원 작업일
        /// </summary>
        public int subDay { get; set; }

        /// <summary>
        /// 외부작업자
        /// </summary>
        public string outName { get; set; }

        /// <summary>
        /// 민원 외부작업 번호
        /// </summary>
        public string outMobile { get; set; }

        /// <summary>
        /// 외부작업 여부
        /// </summary>
        public string outViw { get; set; }

        /// <summary>
        /// 결재여부
        /// </summary>
        public string innView { get; set; }

        /// <summary>
        /// 완료여부
        /// </summary>
        public string Complete { get; set; }

        /// <summary>
        /// 작업자
        /// </summary>
        public string subWorker { get; set; }

        /// <summary>
        /// 작업내용
        /// </summary>
        public string subContent { get; set; }

        /// <summary>
        /// 작업부서
        /// </summary>
        public string subPost { get; set; }

        /// <summary>
        /// 작업자 직책
        /// </summary>
        public string subDuty { get; set; }

        /// <summary>
        /// 입력일
        /// </summary>
        public DateTime PostDate { get; set; }

        /// <summary>
        /// 입력자 아이피
        /// </summary>
        public string PostIP { get; set; }

        /// <summary>
        /// 모름
        /// </summary>
        public string AppealViw { get; set; }
    }

    /// <summary>
    /// 민원 작업자
    /// </summary>
    public class subWorker_Entity
    {
        /// <summary>
        /// 식별코드
        /// </summary>
        public int workerAid { get; set; }

        /// <summary>
        /// 민원작업 식별코드
        /// </summary>
        public string subAid { get; set; }

        /// <summary>
        /// 작업자 식별코드
        /// </summary>
        public string StaffCode { get; set; }

        /// <summary>
        /// 민원 식별코드
        /// </summary>
        public string apNum { get; set; }

        /// <summary>
        /// 작업자명
        /// </summary>
        public string StaffName { get; set; }

        /// <summary>
        /// 입력일
        /// </summary>
        public DateTime PostDate { get; set; }

        /// <summary>
        /// 입력자 아이피
        /// </summary>
        public string PostIP { get; set; }
    }

    
}
