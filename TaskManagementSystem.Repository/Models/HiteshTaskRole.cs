using System;
using System.Collections.Generic;

namespace TaskManagementSystem.Repository.Models;

public partial class HiteshTaskRole
{
    public int Id { get; set; }

    public string? RoleName { get; set; }

    public virtual ICollection<HiteshTaskUserMaster> HiteshTaskUserMasters { get; set; } = new List<HiteshTaskUserMaster>();
}
