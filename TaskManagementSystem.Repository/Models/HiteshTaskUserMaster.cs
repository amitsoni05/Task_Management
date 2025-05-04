using System;
using System.Collections.Generic;

namespace TaskManagementSystem.Repository.Models;

public partial class HiteshTaskUserMaster
{
    public int Id { get; set; }

    public string FullName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public int Role { get; set; }

    public string? Ip { get; set; }

    public DateTime CreatedOn { get; set; }

    public int CreatedBy { get; set; }

    public bool Active { get; set; }

    public int? UpdateBy { get; set; }

    public DateTime? UpdatedOn { get; set; }

    public string? Image { get; set; }

    public virtual ICollection<HiteshTaskAssignTask> HiteshTaskAssignTaskCreatedByNavigations { get; set; } = new List<HiteshTaskAssignTask>();

    public virtual ICollection<HiteshTaskAssignTask> HiteshTaskAssignTaskUpdatedByNavigations { get; set; } = new List<HiteshTaskAssignTask>();

    public virtual ICollection<HiteshTaskProjectAccess> HiteshTaskProjectAccesses { get; set; } = new List<HiteshTaskProjectAccess>();

    public virtual ICollection<HiteshTaskProject> HiteshTaskProjectCreatedByNavigations { get; set; } = new List<HiteshTaskProject>();

    public virtual ICollection<HiteshTaskProject> HiteshTaskProjectUpdatedByNavigations { get; set; } = new List<HiteshTaskProject>();

    public virtual HiteshTaskRole RoleNavigation { get; set; } = null!;
}
