using System.Threading.Tasks;
using ContosoDashboard.Data;
using ContosoDashboard.Models;
using ContosoDashboard.Services;
using Microsoft.AspNetCore.Mvc;

namespace ContosoDashboard.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DocumentsController : ControllerBase
{
    private readonly DocumentService _documentService;
    private readonly ApplicationDbContext _db;

    public DocumentsController(DocumentService documentService, ApplicationDbContext db)
    {
        _documentService = documentService;
        _db = db;
    }

    [HttpPost]
    public async Task<IActionResult> Upload([FromForm] Microsoft.AspNetCore.Http.IFormFile file, [FromForm] string title, [FromForm] string category, [FromForm] string? description, [FromForm] int? projectId, [FromForm] int uploaderId)
    {
        if (file == null) return BadRequest("File is required");
        var doc = await _documentService.UploadDocumentAsync(file, uploaderId, title, category, description, projectId);
        return CreatedAtAction(nameof(Get), new { id = doc.DocumentId }, new { id = doc.DocumentId, title = doc.Title, uploadDateUtc = doc.UploadDateUtc, status = doc.ScanStatus });
    }

    [HttpGet]
    public IActionResult List()
    {
        // Try to get current user id from claims
        var userIdClaim = HttpContext.User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (!int.TryParse(userIdClaim, out var userId))
        {
            return Unauthorized();
        }

        var docs = _db.Documents.AsQueryable().Where(d => d.UploaderId == userId && !d.IsDeleted).ToList();
        return Ok(new { items = docs, total = docs.Count });
    }

    [HttpGet("{id}")]
    public IActionResult Get(int id)
    {
        var doc = _db.Documents.Find(id);
        if (doc == null) return NotFound();
        return Ok(doc);
    }
}
