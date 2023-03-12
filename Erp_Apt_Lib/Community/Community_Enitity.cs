using System;
using System.Collections.Generic;
using System.Text;

namespace Erp_Apt_Lib.Community
{
    public class Community_Entity
    {
        public int Aid { get; set; }
        public string AptCode { get; set; }
        public string AptName { get; set; }
        public string UserCode { get; set; }
        public string UserName { get; set; }
        public string Dong { get; set; }
        public string Ho { get; set; }
        public string Mobile { get; set; }
        public string Relation { get; set; }
        public string Division { get; set; }
        public string UsingKindName { get; set; }
        public string UsingKindCode { get; set; }
        public string Ticket { get; set; }
        public string Ticket_Code { get; set; }
        public DateTime UserStartDate { get; set; }
        public DateTime UserEndDate { get; set; }

        /// <summary>
        /// 시작 시간
        /// </summary>
        public int UserStartHour { get; set; }

        /// <summary>
        /// 종료 시간
        /// </summary>
        public int UserEndHour { get; set; }

        public int ScamDays { get; set; }
        public int UseCost { get; set; }
        public string Etc { get; set; }

        /// <summary>
        /// 이용자 아이피
        /// </summary>
        public string PostIP { get; set; }

        public DateTime PostDate { get; set; }
        public string User_Code { get; set; }
        public int FilesCount { get; set; }

        public string Mobile_Use { get; set; }

        /// <summary>
        /// 승인여부
        /// </summary>
        public string Approval { get; set; }

        /// <summary>
        /// 각 세대별 월 합계
        /// </summary>
        public int TotalSum { get; set; }

        /// <summary>
        /// 신청순서
        /// </summary>
        public int OrderBy { get; set; }
    }

    public class CommunityUsingKind_Entity
    {
        public int Aid { get; set; }
        public string Kind_Code { get; set; }
        public string Kind_Name { get; set; }
        public string AptCode { get; set; }
        public string AptName { get; set; }
        public string Using { get; set; }
        public DateTime PostDate { get; set; }
        public string User_Code { get; set; }
    }

    public class CommunityUsingTicket_Entity
    {
        public int Aid { get; set; }
        public string Kind_Code { get; set; }
        public string Kind_Name { get; set; }
        public string Ticket_Code { get; set; }
        public string Ticket_Name { get; set; }
        public int Ticket_Cost { get; set; }
        public string AptCode { get; set; }
        public string AptName { get; set; }
        public string Using { get; set; }
        public DateTime PostDate { get; set; }
        public string User_Code { get; set; }
    }

    public class MonthTotalSum_Entity
    {
        public string Dong { get; set; }
        public string Ho { get; set; }
        public string TotalSum { get; set; }
    }



}
