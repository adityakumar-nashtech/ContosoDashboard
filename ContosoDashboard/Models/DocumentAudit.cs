using System;
using System.ComponentModel.DataAnnotations;

namespace ContosoDashboard.Models;

public class DocumentAudit
{
    [Key]
    public int DocumentAuditId { get; set; }

    public int DocumentId { get; set; }

    public string Action { get; set; } = string.Empty;

    public int PerformedByUserId { get; set; }

    public DateTime PerformedAtUtc { get; set; }

    public string? Details { get; set; }
}
