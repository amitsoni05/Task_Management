using System;
using System.Collections.Generic;

namespace TaskManagementSystem.Repository.Models;

public partial class HiteshTaskMessage
{
    public int Id { get; set; }

    public string? Message { get; set; }

    public int? TaskId { get; set; }

    public TimeOnly Time { get; set; }

    public DateOnly Date { get; set; }

    public int SendId { get; set; }

    public int ReceiveId { get; set; }

    public virtual HiteshTaskAssignTask Task { get; set; } = null!;
}
