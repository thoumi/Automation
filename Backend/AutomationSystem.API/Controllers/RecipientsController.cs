using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using AutomationSystem.Infrastructure.Data;
using AutomationSystem.Core.Models;

namespace AutomationSystem.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class RecipientsController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<RecipientsController> _logger;

    public RecipientsController(ApplicationDbContext context, ILogger<RecipientsController> logger)
    {
        _context = context;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<NotificationRecipient>>> GetRecipients(
        [FromQuery] NotificationType? type = null)
    {
        var query = _context.NotificationRecipients.AsQueryable();

        if (type.HasValue)
        {
            query = query.Where(r => r.Type == type.Value);
        }

        return await query.OrderBy(r => r.Name).ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<NotificationRecipient>> GetRecipient(int id)
    {
        var recipient = await _context.NotificationRecipients.FindAsync(id);

        if (recipient == null)
        {
            return NotFound();
        }

        return recipient;
    }

    [HttpPost]
    public async Task<ActionResult<NotificationRecipient>> CreateRecipient(NotificationRecipient recipient)
    {
        recipient.CreatedAt = DateTime.UtcNow;
        _context.NotificationRecipients.Add(recipient);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Recipient created: {Name} ({Type})", recipient.Name, recipient.Type);

        return CreatedAtAction(nameof(GetRecipient), new { id = recipient.Id }, recipient);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateRecipient(int id, NotificationRecipient recipient)
    {
        if (id != recipient.Id)
        {
            return BadRequest();
        }

        _context.Entry(recipient).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await _context.NotificationRecipients.AnyAsync(e => e.Id == id))
            {
                return NotFound();
            }
            throw;
        }

        _logger.LogInformation("Recipient updated: {Name} ({Type})", recipient.Name, recipient.Type);

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteRecipient(int id)
    {
        var recipient = await _context.NotificationRecipients.FindAsync(id);
        if (recipient == null)
        {
            return NotFound();
        }

        _context.NotificationRecipients.Remove(recipient);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Recipient deleted: {Name}", recipient.Name);

        return NoContent();
    }

    [HttpPatch("{id}/toggle")]
    public async Task<IActionResult> ToggleRecipient(int id)
    {
        var recipient = await _context.NotificationRecipients.FindAsync(id);
        if (recipient == null)
        {
            return NotFound();
        }

        recipient.IsActive = !recipient.IsActive;
        await _context.SaveChangesAsync();

        return Ok(new { isActive = recipient.IsActive });
    }
}

