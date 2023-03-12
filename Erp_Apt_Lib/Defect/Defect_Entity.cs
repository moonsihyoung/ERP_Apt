using System;
using System.Collections.Generic;
using System.Text;

namespace Erp_Apt_Lib
{
    public class Defect_Entity
    {
        public int Aid { get; set; }

        public string AptCode { get; set; }
        public string AptName { get; set; }

        public string Company_Code { get; set; }
        public string Company_Name { get; set; }        

        public string subCompany_Code { get; set; }
        public string subCompany_Name { get; set; }

        /// <summary>
        /// 전용 또는 공용 여부
        /// </summary>
        public string Private { get; set; }

        /// <summary>
        /// 전용
        /// </summary>
        public string Dong { get; set; }
        public string Ho { get; set; }
        public string InnerName { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public string Relation { get; set; }
        public string Etc { get; set; }




        /// <summary>
        /// 하자 발생일
        /// </summary>
        public DateTime DefectDate { get; set; }

        /// <summary>
        /// 하자 입력 년도
        /// </summary>
        public int dfYear { get; set; }

        /// <summary>
        /// 하자 입력 월
        /// </summary>
        public int dfMonth { get; set; }

        /// <summary>
        /// 하자 입력 일
        /// </summary>
        public int dfDay { get; set; }

        /// <summary>
        /// 하자 분류
        /// </summary>
        public string Bloom_Code_A { get; set; }
        public string Bloom_Name_A { get; set; }

        public string Bloom_Code_B { get; set; }
        public string Bloom_Name_B { get; set; }

        public string Bloom_Code_C { get; set; }
        public string Bloom_Name_C { get; set; }

        /// <summary>
        /// 하자기간
        /// </summary>
        public int Period { get; set; }

        /// <summary>
        /// 장소
        /// </summary>        
        public string Position { get; set; }
        public string Position_Code { get; set; }

        public string dfPost { get; set; }

        /// <summary>
        /// 접수자
        /// </summary>
        public string dfApplicant { get; set; }

        public string dfApplicant_Code { get; set; }

        /// <summary>
        /// 제목
        /// </summary>
        public string dfTitle { get; set; }

        /// <summary>
        /// 하자 내용
        /// </summary>
        public string dfContent { get; set; }

        /// <summary>
        /// 처리완료 여부
        /// </summary>
        public string Complete { get; set; }

        /// <summary>
        /// 만족도
        /// </summary>
        public string dfSatisfaction { get; set; }

        /// <summary>
        /// 하자 통보 여부
        /// </summary>
        public string Inform { get; set; }

        /// <summary>
        /// 첨부파일 수
        /// </summary>
        public int ImagesCount { get; set; }

        public string Approval { get; set; }

        public DateTime PostDate { get; set; }
        public string PostIp { get; set; }

        public string User_Code { get; set; }


    }
}
