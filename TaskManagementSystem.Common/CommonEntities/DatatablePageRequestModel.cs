using System;
using System.Collections.Generic;
using System.Text;

namespace TaskManagementSystem.Common.CommonEntities
{
    public class DatatablePageRequestModel
    {
        public int StartIndex { get; set; } = 0;
        public int PageSize { get; set; } = 10;
        public string SearchText { get; set; } = "";
        public string SortColumnName { get; set; } = "";
        public string SortDirection { get; set; } = "";
        public object Draw { get; set; } = "";
        public string ExtraSearch { get; set; } = "";
        public int UserId { get; set; }
        public int WorkOrderPriority { get; set; }
        public int WorkOrderStatus { get; set; }
        public short Priority { get; set; }
        public int PatientId { get; set; }
        public int RoleId { get; set; }
        public int RoomStatusId { get; set; }
        public int BoroomMasterId { get; set; }
        public int PrimaryDiscipline { get; set; }
        public int OrientationStatus { get; set; }
        public int CandidateStatus { get; set; }
        public string CreatedOn { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public int DoctordId { get; set; }
        public int RoomId { get; set; }
        public int AppointmentTypeId { get; set; }
        public int Status { get; set; }
        public int CourtId { get; set; }
        public string Category {  get; set; }

        public string Date { get; set; }
        public int DdlStatus {  get; set; }

    }
}
