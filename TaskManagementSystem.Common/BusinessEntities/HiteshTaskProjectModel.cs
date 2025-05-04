using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace TaskManagementSystem.Common.BusinessEntities
{
    public class HiteshTaskProjectModel:ResponseModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public DateTime? CreatedOn { get; set; }

        public string EncProjectId { get; set; }
        public int? CreatedBy { get; set; }

        public string CreatedName { get; set; }

        public string CreatedDate { get; set; }
        public bool IsEdit { get; set; }
        public string? Ip { get; set; }

        public int? UpdatedBy { get; set; }

        public DateTime? UpdatedOn { get; set; }

        public bool? IsActive { get; set; }

        public List<int> UserIds { get; set; } = new List<int>();

        public List<SelectListItem> UserMasterList { get; set; }

        public List<HiteshTaskUserMasterModel> Users { get; set; } = new List<HiteshTaskUserMasterModel>();
    }
}
