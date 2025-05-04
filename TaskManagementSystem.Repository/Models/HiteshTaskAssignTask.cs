using System;
using System.Collections.Generic;

namespace TaskManagementSystem.Repository.Models;

public partial class HiteshTaskAssignTask
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public string Description { get; set; } = null!;

    public byte Priority { get; set; }

    public byte Status { get; set; }

    public DateOnly DeadLine { get; set; }

    public int CreatedBy { get; set; }

    public DateTime CreatedOn { get; set; }

    public bool Active { get; set; }

    public string AssignTo { get; set; } = null!;

    public int ProjectId { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedOn { get; set; }

    public string Ip { get; set; } = null!;

    public int? CompleteBy { get; set; }

    public virtual HiteshTaskUserMaster CreatedByNavigation { get; set; } = null!;

    public virtual ICollection<HiteshTaskDocumentSave> HiteshTaskDocumentSaves { get; set; } = new List<HiteshTaskDocumentSave>();

    public virtual ICollection<HiteshTaskMessage> HiteshTaskMessages { get; set; } = new List<HiteshTaskMessage>();

    public virtual HiteshTaskProject Project { get; set; } = null!;

    public virtual HiteshTaskUserMaster? UpdatedByNavigation { get; set; }
}
