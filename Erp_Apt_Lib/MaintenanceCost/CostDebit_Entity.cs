using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace Erp_Apt_Lib.MaintenanceCost
{
    /// <summary>
    /// 관리비 정보 엔터티
    /// </summary>
    public class CostDebit_Entity
    {
        /// <summary>
        /// 일련번호
        /// </summary>
        public int Aid { get; set; }

        /// <summary>
        /// 부과월
        /// </summary>
        public string Month { get; set; }

        /// <summary>
        /// 동
        /// </summary>
        public string dong { get; set; }

        /// <summary>
        /// 호
        /// </summary>
        public string ho { get; set; }

        /// <summary>
        /// 공동 식별코드
        /// </summary>
        public string Apt_Code { get; set; }

        /// <summary>
        /// 공동주택 명칭
        /// </summary>
        public string Apt_Name { get; set; }

        /// <summary>
        /// 세대주
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 입주일자
        /// </summary>
        public string innerDate { get; set; }

        /// <summary>
        /// 공급면적
        /// </summary>
        public double supplyArea { get; set; }

        /// <summary>
        /// 장기수선충당금
        /// </summary>
        public int longRepairDue { get; set; }

        /// <summary>
        /// 보험료
        /// </summary>
        public int premiumDue { get; set; }

        /// <summary>
        /// 대표회의 운영비
        /// </summary>
        public int delegateCdue { get; set; }

        /// <summary>
        /// 선거관리 운영비
        /// </summary>
        public int electionMdue { get; set; }

        /// <summary>
        /// 주차분당금
        /// </summary>
        public int parkingDue { get; set; }

        /// <summary>
        /// 세대전기료
        /// </summary>
        public int electricPcharges { get; set; }

        /// <summary>
        /// 공동전기료
        /// </summary>
        public int electricCcharges { get; set; }

        /// <summary>
        /// 승강기유지비
        /// </summary>
        public int elevatorExpenseDue { get; set; }

        /// <summary>
        /// 승강기 전기료
        /// </summary>
        public int elevatorEcharges { get; set; }

        /// <summary>
        /// TV수신료
        /// </summary>
        public int tvFee { get; set; }

        /// <summary>
        /// 월별 일련번호
        /// </summary>
        public int id_number { get; set; }

        /// <summary>
        /// 전기차 충전료
        /// </summary>
        public int electricChargeDue { get; set; }

        /// <summary>
        /// 세대 수도료
        /// </summary>
        public int waterPrates { get; set; }

        /// <summary>
        /// 공용 수도료
        /// </summary>
        public int waterCrates { get; set; }

        /// <summary>
        /// 전기사용량
        /// </summary>
        public int electricUse { get; set; }

        /// <summary>
        /// 온수 사용량
        /// </summary>
        public int hotWaterUse { get; set; }

        /// <summary>
        /// 난방 사용량
        /// </summary>
        public int heatingUse { get; set; }

        /// <summary>
        /// 가스 사용량
        /// </summary>
        public int gasUse { get; set; }

        /// <summary>
        /// 주민공동시설 이용료
        /// </summary>
        public int facilityCdue { get; set; }

        /// <summary>
        /// 가수금
        /// </summary>
        public int suspenseReciept { get; set; }

        /// <summary>
        /// 일자리 안전 차감
        /// </summary>
        public int workSdeduction { get; set; }

        /// <summary>
        /// 관리비 차감
        /// </summary>               
        public int costMdeduction { get; set; }

        /// <summary>
        /// 할인액
        /// </summary>
        public int discountSum { get; set; }

        /// <summary>
        /// 수도 사용량
        /// </summary>
        public int waterUse { get; set; }

        /// <summary>
        /// 일반관리비
        /// </summary>
        public int generalDue { get; set; }

        /// <summary>
        /// 청소비
        /// </summary>
        public int cleaningDue { get; set; }

        /// <summary>
        /// 경비비
        /// </summary>
        public int securityDue { get; set; }

        /// <summary>
        /// 자동이체
        /// </summary>
        public string directDebit { get; set; }
        
        /// <summary>
        /// 소독비
        /// </summary>
        public int disinfectionDue { get; set; }

        /// <summary>
        /// 수선유지비
        /// </summary>
        public int repairDue { get; set; }

        /// <summary>
        /// 위탁수수료
        /// </summary>
        public int trustDue { get; set; }

        /// <summary>
        /// 관리비 합계
        /// </summary>
        public int maintenanceFeeSum { get; set; }

        /// <summary>
        /// 사용료 합계
        /// </summary>
        public int useFeeSum { get; set; }

        /// <summary>
        /// 미납액
        /// </summary>
        public int unpaidSum { get; set; }

        /// <summary>
        /// 월합계
        /// </summary>
        public int monthTotalSum { get; set; }

        /// <summary>
        /// 납기내 합계
        /// </summary>
        public int extendDue { get; set; }

        /// <summary>
        /// 미납 연체료
        /// </summary>
        public int unpaidOverdue { get; set; }

        /// <summary>
        /// 납기 후 연체료
        /// </summary>
        public int afterOverdue { get; set; }

        /// <summary>
        /// 납기 후 금액
        /// </summary>
        public int afterExtendDue { get; set; }
    }
}
