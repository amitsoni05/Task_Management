using System;
using System.Collections.Generic;

namespace TaskManagementSystem.Repository.Models;

public partial class HiteshTaskProject
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public string Description { get; set; } = null!;

    public DateTime CreatedOn { get; set; }

    public int CreatedBy { get; set; }

    public string Ip { get; set; } = null!;

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedOn { get; set; }

    public bool IsActive { get; set; }

    public virtual HiteshTaskUserMaster CreatedByNavigation { get; set; } = null!;

    public virtual ICollection<HiteshTaskAssignTask> HiteshTaskAssignTasks { get; set; } = new List<HiteshTaskAssignTask>();

    public virtual ICollection<HiteshTaskDocumentSave> HiteshTaskDocumentSaves { get; set; } = new List<HiteshTaskDocumentSave>();

    public virtual ICollection<HiteshTaskProjectAccess> HiteshTaskProjectAccesses { get; set; } = new List<HiteshTaskProjectAccess>();

    public virtual HiteshTaskUserMaster? UpdatedByNavigation { get; set; }
}
