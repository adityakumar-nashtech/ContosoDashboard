using System;
using System.ComponentModel.DataAnnotations;

namespace ContosoDashboard.Models;

public class Document
{
    [Key]
    public int DocumentId { get; set; }

    [Required]
    [MaxLength(250)]
    public string Title { get; set; } = string.Empty;

    public string? Description { get; set; }

    [Required]
    public string Category { get; set; } = string.Empty;

    public int? ProjectId { get; set; }

    public int UploaderId { get; set; }

    public DateTime UploadDateUtc { get; set; }

    public long FileSizeBytes { get; set; }

    [Required]
    [MaxLength(255)]
    public string ContentType { get; set; } = string.Empty;

    [Required]
    public string StoragePath { get; set; } = string.Empty;

    public string? Checksum { get; set; }

    public int Version { get; set; } = 1;

    public bool IsDeleted { get; set; } = false;

    public string ScanStatus { get; set; } = "Queued";
}
