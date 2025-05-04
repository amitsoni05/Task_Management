using System;
using System.Collections.Generic;

namespace TaskManagementSystem.Repository.Models;

public partial class HiteshTaskDocumentSave
{
    public int Id { get; set; }

    public string? DocumentName { get; set; }

    public int? ProjectId { get; set; }

    public int? TaskId { get; set; }

    public string? DocumentType { get; set; }

    public virtual HiteshTaskProject? Project { get; set; }

    public virtual HiteshTaskAssignTask? Task { get; set; }
}
