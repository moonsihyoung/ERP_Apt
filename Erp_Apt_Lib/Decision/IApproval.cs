using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Erp_Apt_Lib.Decision
{
    public interface IApproval_Lib
    {
        /// <summary>
        /// 결재란 만들기
        /// </summary>
        Task<List<Approval_Entity>> GetList(string AptCode, string Bloom);

        // <summary>
        /// 결제자 라인 입력
        /// </summary>
        Task Add(Approval_Entity dnn);

        /// <summary>
        /// 결재자 라인 정보 수정
        /// </summary>
        Task Edit(Approval_Entity dp);

        /// <summary>
        /// 결재 정보 목록
        /// </summary>
        Task<int> ListCount_Bloom(string AptCode, string Bloom);

        /// <summary>
        /// 결재라인에서 해당 부서직책 존재 여부 확인
        /// </summary>
        Task<int> PostDutyBeCount(string AptCode, string Bloom, string PostDuty);

        /// <summary>
        /// 결재 정보 목록 (공동주택 전체)
        /// </summary>
        Task<List<Approval_Entity>> GetBloomList(int Page, string AptCode);

        /// <summary>
        /// 결재 정보 목록 (공동주택 전체) 수
        /// </summary>
        Task<int> GetListCount(string AptCode);

        /// <summary>
        /// 삭제
        /// </summary>
        Task Remove(int Num);

    }
}
