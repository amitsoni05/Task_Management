using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace TaskManagementSystem.Common.BusinessEntities
{
    public class HiteshTaskAssignTaskModel:ResponseModel
    {
        public int Id { get; set; }

        public string EncTaskId { get; set; }
        public string Title { get; set; }

        public string Description { get; set; }
    
        public byte Priority { get; set; }
        public string PriorityString { get; set; }

        public int roleid { get; set; }
        public byte Status { get; set; }
        public string StatusString { get; set; }

        public DateOnly DeadLine { get; set; }
        public string CreatedDate { get; set; }
        public string CompleteBy { get; set; }
        public string ProjectName { get; set; }
        public int? CreatedBy { get; set; }

        public DateTime? CreatedOn { get; set; }

        public bool Active { get; set; }

        public bool IsEdit { get; set; }

        public string AssignTo { get; set; }

       public string DocumentType { get; set; }

        public List<string> DocumentTypeList { get; set; }
        public List<IFormFile> Files { get; set; }

        public List<string> DocName { get; set; }

        public int ProjectId { get; set; }
        public List<SelectListItem> UserMasterList { get; set; }
        public List<int> UserIds { get; set; } = new List<int>();

        public int? UpdatedBy { get; set; }

        public DateTime? UpdatedOn { get; set; }

        public bool AllDay { get; set; }
        public string Start { get; set; }
        public string End { get; set; }
        public int TotalCount { get; set; }
        public string SlotDate { get; set; }
        public string? Ip { get; set; }

        public int complete { get; set; }

        public int pending { get; set; }
      public int UserId { get; set; }
        public List<string> ExistingFiles { get; set; }
        public List<string> ExistingDocumentTypes { get; set; }

        public List<HiteshTaskDocumentSaveModel> AllDocumentsList { get; set; }
        public List<HiteshTaskUserMasterModel> Users { get; set; }

    }
}
