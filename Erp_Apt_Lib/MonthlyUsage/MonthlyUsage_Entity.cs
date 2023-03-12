using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace Erp_Apt_Lib.MonthlyUsage
{
    /// <summary>
    /// 사용량 정보
    /// </summary>
    public class MonthlyUsage_Entity
    {
        /// <summary>
        /// 식별코드
        /// </summary>
        public int Aid { get; set; }
        
        /// <summary>
        /// 공동주택 코드
        /// </summary>
        public string Apt_Code { get; set; }

        /// <summary>
        /// 공동주택 명칭
        /// </summary>
        public string Apt_Name { get; set; }
        
        /// <summary>
        /// 해당 일자
        /// </summary>
        public DateTime UseDate { get; set; }

        /// <summary>
        /// 해당 년도
        /// </summary>
        public int intYear { get; set; }

        /// <summary>
        /// 해당월
        /// </summary>
        public int intMonth { get; set; }
                
        /// <summary>
        /// 전기계약 방식
        /// </summary>
        public string ElectricContractMethod { get; set; }

        /// <summary>
        /// 전기부과 방식
        /// </summary>
        public string ElectricInpose { get; set; }

        /// <summary>
        /// 산업용 전기료 사용량
        /// </summary>
        public int ElectricIndustry { get; set; }

        /// <summary>
        /// 산업용 전기료 사용료
        /// </summary>
        public int ElectricIndustryFee { get; set; }

        /// <summary>
        /// 가로등 사용량
        /// </summary>
        public int ElectricStreetLamp { get; set; }

        /// <summary>
        /// 가로등 사용료
        /// </summary>
        public int ElectricStreetLampFee { get; set; }

        /// <summary>
        /// 전기 전체 사용량
        /// </summary>
        public int ElectricAllUsage { get; set; }

        /// <summary>
        /// 전기 주택용 전체 전기료
        /// </summary>
        public int ElectricAllFee { get; set; }

        /// <summary>
        /// 전기 공용사용량
        /// </summary>
        public int ElectricComUsage { get; set; }

        /// <summary>
        /// 전기 공용 사용료
        /// </summary>
        public int ElectricComFee { get; set; }

        /// <summary>
        /// 전기 세대 사용량 합계 
        /// </summary>
        public int ElectricPerUsage { get; set; }

        /// <summary>
        /// 전기 세대 사용료 합계
        /// </summary>
        public int ElectricPerFee { get; set; }

        /// <summary>
        /// 수도 전체 사용량
        /// </summary>
        public int WaterAllUsage { get; set; }

        /// <summary>
        /// 수도 전체 사용료
        /// </summary>
        public int WaterAllFee { get; set; }

        /// <summary>
        /// 수도 공용 사용량
        /// </summary>
        public int WaterComUsage { get; set; }

        /// <summary>
        /// 수도 공용 사용료
        /// </summary>
        public int WaterComFee { get; set; }

        /// <summary>
        /// 수도 세대 사용량 합계
        /// </summary>
        public int WaterPerUsage { get; set; }

        /// <summary>
        /// 수도 세대 사용료 합계
        /// </summary>
        public int WaterPerFee { get; set; }


        /// <summary>
        /// 급탕 사용량
        /// </summary>
        public int HotWaterAllUsage { get; set; }

        /// <summary>
        /// 급탕 사용료
        /// </summary>
        public int HotWaterAllFee { get; set; }

        /// <summary>
        /// 급탕 공용 사용량
        /// </summary>
        public int HotWaterComUsage { get; set; }

        /// <summary>
        /// 급탕 공용 사용료
        /// </summary>
        public int HotWaterComFee { get; set; }

        /// <summary>
        /// 급탕 세대 사용량 합계
        /// </summary>
        public int HotWaterPerUsage { get; set; }

        /// <summary>
        /// 급탕 세대 사용료 합계
        /// </summary>
        public int HotWaterPerFee { get; set; }

        /// <summary>
        /// 난방방식
        /// </summary>
        public string codeHeatNm { get; set; }

        /// <summary>
        /// 난방 전체 사용량
        /// </summary>
        public int HeatingAllUsage { get; set;}

        /// <summary>
        /// 난방 전체 사용료
        /// </summary>
        public int HeatingAllFee { get; set; }

        /// <summary>
        /// 난방 공용사용량
        /// </summary>
        public int HeatingComUsage { get; set; }

        /// <summary>
        /// 난방 공용사용료
        /// </summary>
        public int HeatingComFee { get; set; }

        /// <summary>
        /// 난방 세대 사용량 합계
        /// </summary>
        public int HeatingPerUsage { get; set; }

        /// <summary>
        /// 난방 세대 사용료 합계
        /// </summary>
        public int HeatingPerFee { get; set; }

        /// <summary>
        /// 가스 사용량
        /// </summary>
        public int GasUsage{ get; set; }

        /// <summary>
        /// 가스 사용료
        /// </summary>
        public int GasFee { get; set; }

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

        /// <summary>
        /// 삭제 여부
        /// </summary>
        public string Del { get; set; }
    }

    /// <summary>
    /// 상세 사용량
    /// </summary>
    public class UsageDetails_Entity
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
        /// 공동주택 명칭
        /// </summary>
        public string Apt_Name { get; set; }

        /// <summary>
        /// 해당일자
        /// </summary>
        public DateTime UsageDate { get; set; }

        /// <summary>
        /// 해당년도
        /// </summary>
        public int intYear { get; set; }

        /// <summary>
        /// 해당월
        /// </summary>
        public int intMonth { get; set; }

        /// <summary>
        /// 구분(전기, 수도, 가스, 난방, 급탕 등)
        /// </summary>
        public string Division { get; set; }

        /// <summary>
        /// 세부 구분(TV수신료, 어린이집, 주민공동시설 등)
        /// </summary>
        public string UseName { get; set; }

        /// <summary>
        /// 사용량
        /// </summary>
        public int Usage { get; set; }

        /// <summary>
        /// 금액
        /// </summary>
        public int CostSum { get; set; }

        /// <summary>
        /// 비고
        /// </summary>
        public string Etc { get; set; }

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
        public string PostIp { get; set; }

        /// <summary>
        /// 입력자 아이디
        /// </summary>
        public string UserCode { get; set; }

        /// <summary>
        /// 삭제 여부
        /// </summary>
        public string Del { get; set; }
    }
}
