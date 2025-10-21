using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using AutomationSystem.Infrastructure.Data;
using AutomationSystem.Core.Models;

namespace AutomationSystem.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class FilesController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly IConfiguration _configuration;
    private readonly ILogger<FilesController> _logger;

    public FilesController(
        ApplicationDbContext context,
        IConfiguration configuration,
        ILogger<FilesController> logger)
    {
        _context = context;
        _configuration = configuration;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<FileUpload>>> GetFiles(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20)
    {
        var files = await _context.FileUploads
            .OrderByDescending(f => f.UploadedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return Ok(files);
    }

    [HttpPost("upload")]
    public async Task<ActionResult<FileUpload>> UploadFile(IFormFile file, [FromForm] string? description = null)
    {
        if (file == null || file.Length == 0)
        {
            return BadRequest("Aucun fichier fourni");
        }

        var maxFileSizeMB = _configuration.GetValue<int>("FileStorage:MaxFileSizeMB", 50);
        if (file.Length > maxFileSizeMB * 1024 * 1024)
        {
            return BadRequest($"Le fichier est trop volumineux. Maximum: {maxFileSizeMB}MB");
        }

        var basePath = _configuration["FileStorage:BasePath"] ?? "uploads";
        if (!Directory.Exists(basePath))
        {
            Directory.CreateDirectory(basePath);
        }

        var fileName = $"{Guid.NewGuid()}_{file.FileName}";
        var filePath = Path.Combine(basePath, fileName);

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        var fileUpload = new FileUpload
        {
            FileName = fileName,
            OriginalFileName = file.FileName,
            FilePath = filePath,
            FileType = Path.GetExtension(file.FileName),
            FileSize = file.Length,
            UploadedBy = User.Identity?.Name ?? "Unknown",
            UploadedAt = DateTime.UtcNow,
            ProcessingStatus = FileProcessingStatus.Pending
        };

        _context.FileUploads.Add(fileUpload);
        await _context.SaveChangesAsync();

        _logger.LogInformation("File uploaded: {FileName} by {User}", file.FileName, fileUpload.UploadedBy);

        return CreatedAtAction(nameof(GetFile), new { id = fileUpload.Id }, fileUpload);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<FileUpload>> GetFile(int id)
    {
        var file = await _context.FileUploads.FindAsync(id);

        if (file == null)
        {
            return NotFound();
        }

        return file;
    }

    [HttpGet("{id}/download")]
    public async Task<IActionResult> DownloadFile(int id)
    {
        var file = await _context.FileUploads.FindAsync(id);

        if (file == null)
        {
            return NotFound();
        }

        if (!System.IO.File.Exists(file.FilePath))
        {
            return NotFound("Le fichier physique n'existe pas");
        }

        var memory = new MemoryStream();
        using (var stream = new FileStream(file.FilePath, FileMode.Open))
        {
            await stream.CopyToAsync(memory);
        }
        memory.Position = 0;

        return File(memory, "application/octet-stream", file.OriginalFileName);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteFile(int id)
    {
        var file = await _context.FileUploads.FindAsync(id);
        if (file == null)
        {
            return NotFound();
        }

        // Supprimer le fichier physique
        if (System.IO.File.Exists(file.FilePath))
        {
            System.IO.File.Delete(file.FilePath);
        }

        _context.FileUploads.Remove(file);
        await _context.SaveChangesAsync();

        _logger.LogInformation("File deleted: {FileName}", file.FileName);

        return NoContent();
    }
}

