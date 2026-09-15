using System;
using System.IO;
using System.Threading.Tasks;
using ContosoDashboard.Data;
using ContosoDashboard.Models;
using Microsoft.AspNetCore.Http;

namespace ContosoDashboard.Services;

public class DocumentService
{
    private readonly ApplicationDbContext _db;
    private readonly IFileStorageService _storage;
    private readonly ILogger<DocumentService> _logger;

    public DocumentService(ApplicationDbContext db, IFileStorageService storage, ILogger<DocumentService> logger)
    {
        _db = db;
        _storage = storage;
        _logger = logger;
    }

    public async Task<Document> UploadDocumentAsync(IFormFile file, int uploaderId, string title, string category, string? description = null, int? projectId = null)
    {
        if (file == null) throw new ArgumentNullException(nameof(file));
        if (file.Length == 0) throw new ArgumentException("File is empty", nameof(file));

        using var ms = new MemoryStream();
        await file.CopyToAsync(ms);
        ms.Position = 0;

        var storagePath = await _storage.UploadAsync(ms, file.FileName, file.ContentType);

        var doc = new Document
        {
            Title = title,
            Description = description,
            Category = category,
            ProjectId = projectId,
            UploaderId = uploaderId,
            UploadDateUtc = DateTime.UtcNow,
            FileSizeBytes = file.Length,
            ContentType = file.ContentType ?? string.Empty,
            StoragePath = storagePath,
            ScanStatus = "Queued"
        };

        _db.Documents.Add(doc);
        await _db.SaveChangesAsync();

        // TODO: enqueue scan message and send notifications

        return doc;
    }

    public async Task<Document> UploadDocumentAsync(Stream fileStream, string fileName, string contentType, int uploaderId, string title, string category, string? description = null, int? projectId = null, IProgress<long>? progress = null)
    {
        if (fileStream == null) throw new ArgumentNullException(nameof(fileStream));

        // Do not attempt to set Position on non-seekable streams (e.g. Blazor IBrowserFile stream)
        var storagePath = await _storage.UploadAsync(fileStream, fileName, contentType, progress);

        var doc = new Document
        {
            Title = title,
            Description = description,
            Category = category,
            ProjectId = projectId,
            UploaderId = uploaderId,
            UploadDateUtc = DateTime.UtcNow,
            FileSizeBytes = fileStream.CanSeek ? fileStream.Length : 0,
            ContentType = contentType ?? string.Empty,
            StoragePath = storagePath,
            ScanStatus = "Queued"
        };

        _db.Documents.Add(doc);
        await _db.SaveChangesAsync();

        try
        {
            _logger.LogInformation("Document created: Id={DocumentId}, Title={Title}, UploaderId={UploaderId}", doc.DocumentId, doc.Title, doc.UploaderId);
        }
        catch { }

        return doc;
    }

    public List<Document> GetDocumentsForUser(int userId)
    {
        return _db.Documents.Where(d => d.UploaderId == userId && !d.IsDeleted).ToList();
    }
}
