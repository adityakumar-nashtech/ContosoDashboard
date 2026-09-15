using System;
using System.ComponentModel.DataAnnotations;

namespace ContosoDashboard.Models;

public class DocumentShare
{
    [Key]
    public int DocumentShareId { get; set; }

    public int DocumentId { get; set; }

    public int? SharedWithUserId { get; set; }

    public int? SharedWithTeamId { get; set; }

    public int SharedByUserId { get; set; }

    public DateTime SharedAtUtc { get; set; }

    public string Permissions { get; set; } = "View";
}
