using Microsoft.AspNetCore.Mvc.Rendering;
using TaskManagementSystem.Common.BusinessEntities;

namespace TaskManagementSystem.Web.Models
{
    public class TaskViewModel
    {
        public HiteshTaskAssignTaskModel HiteshTaskAssignTaskModel { get;  set; }=new HiteshTaskAssignTaskModel();
        public List<SelectListItem> ProjectList { get;  set; }=new List<SelectListItem>();
        public List<SelectListItem> UserMasterList { get; set; } = new List<SelectListItem>();
        public HiteshTaskProjectModel HiteshTaskProjectModel { get;  set; }= new HiteshTaskProjectModel();
        public HiteshTaskUserMasterModel HiteshTaskUserMasterModel { get; set; } = new HiteshTaskUserMasterModel();

        public HiteshTaskProjectAccessModel HiteshTaskProjectAccessModel { get; set; } = new HiteshTaskProjectAccessModel();

        public HiteshTaskRoleModel HiteshTaskRoleModel { get; set; } = new HiteshTaskRoleModel();

        public HiteshTaskDocumentSaveModel HiteshTaskDocumentSaveModel { get; set; }=new HiteshTaskDocumentSaveModel();

        public List<HiteshTaskDocumentSaveModel> hiteshTaskDocumentSaveModelList { get; set; } = new List<HiteshTaskDocumentSaveModel>();

        public HIteshTaskMessageModel HIteshTaskMessageModel { get; set; } = new HIteshTaskMessageModel();

        public List<HIteshTaskMessageModel> hIteshTaskMessageModelsList { get; set; } = new List<HIteshTaskMessageModel>();

        public List<HiteshTaskAssignTaskModel> HiteshAssignTaskModelList { get; set; }=new List<HiteshTaskAssignTaskModel> ();
      public int RoleId { get; set; }

        public int UserId { get; set; }
        public int total { get; set; }

        public int success { get; set; }

        public bool isSuccess { get; set; }
        public int errorcount { get; set; }

        public string[] StatusChartLabels { get; set; }
        public int[] StatusChartData { get; set; }

        public int totalcount { get; set; }
        public string[] PriorityChartLabels { get; set; }
        public int[] PriorityChartData { get; set; }


    }
}
