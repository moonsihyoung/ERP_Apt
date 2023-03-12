using System;
using System.Collections.Generic;
using System.Text;

namespace Erp_Apt_Lib
{
    /// <summary>
    /// 동 정보
    /// </summary>
    public class Dong_Entity
    {
        public int Aid { get; set; }
        /// <summary>
        /// 동 식별코드
        /// </summary>
        public string Dong_Code { get; set; }

        /// <summary>
        /// 공동주택 식별코드
        /// </summary>
        public string Apt_Code { get; set; }

        /// <summary>
        /// 동이름
        /// </summary>
        public string Dong_Name { get; set; }

        /// <summary>
        /// 세대수
        /// </summary>
        public int Family_Num { get; set; }

        /// <summary>
        /// 동 길이
        /// </summary>
        public int Length { get; set; }

        /// <summary>
        /// 동 폭
        /// </summary>
        public int Width { get; set; }

        /// <summary>
        /// 층수
        /// </summary>
        public int Floor_Num { get; set; }

        /// <summary>
        /// 통로 수
        /// </summary>
        public int Exit_Num { get; set; }

        /// <summary>
        /// 승강기 수
        /// </summary>
        public int Elevator_Num { get; set; }

        /// <summary>
        /// 라인 수
        /// </summary>
        public int Line_Num { get; set; }

        /// <summary>
        /// 복도 형태
        /// </summary>
        public string Hall_Form { get; set; }

        /// <summary>
        /// 지붕 형태
        /// </summary>
        public string Roof_Form { get; set; }

        /// <summary>
        /// 동면적
        /// </summary>
        public int Dong_Area { get; set; }

        /// <summary>
        /// 설명
        /// </summary>
        public string Dong_Etc { get; set; }

        /// <summary>
        /// 입력일
        /// </summary>
        public DateTime PostDate { get; set; }

        /// <summary>
        /// 입력자 아이피
        /// </summary>
        public string PostIP { get; set; }
    }

    /// <summary>
    /// 동 구성정보
    /// </summary>
    public class Dong_Composition_Entity
    {
        public int Aid { get; set; }

        /// <summary>
        /// 동 구성정보 코드
        /// </summary>
        public string Dong_Composition_Code { get; set; }

        /// <summary>
        /// 공동주택 코드
        /// </summary>
        public string Apt_Code { get; set; }

        /// <summary>
        /// 동 코드
        /// </summary>
        public string Dong_Code { get; set; }

        /// <summary>
        /// 공급면적
        /// </summary>
        public double Supply_Area { get; set; }

        /// <summary>
        /// 세대수
        /// </summary>
        public int Area_Family_Num { get; set; }

        /// <summary>
        /// 전용면적
        /// </summary>
        public double Only_Area { get; set; }

        /// <summary>
        /// 합계 면적
        /// </summary>
        public double Total_Area { get; set; }

        /// <summary>
        /// 비고
        /// </summary>
        public string Dong_Etc { get; set; }
        public DateTime PostDate { get; set; }
        public string PostIP { get; set; }
    }
}
