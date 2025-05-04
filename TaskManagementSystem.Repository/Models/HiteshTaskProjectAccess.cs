using System;
using System.Collections.Generic;

namespace TaskManagementSystem.Repository.Models;

public partial class HiteshTaskProjectAccess
{
    public int Id { get; set; }

    public int ProjectId { get; set; }

    public int UserId { get; set; }

    public virtual HiteshTaskProject Project { get; set; } = null!;

    public virtual HiteshTaskUserMaster User { get; set; } = null!;
}
