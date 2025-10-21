using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using AutomationSystem.Infrastructure.Data;
using AutomationSystem.Core.Models;
using TaskStatus = AutomationSystem.Core.Models.TaskStatus;

namespace AutomationSystem.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class LogsController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public LogsController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<object>> GetLogs(
        [FromQuery] string? taskName = null,
        [FromQuery] DateTime? startDate = null,
        [FromQuery] DateTime? endDate = null,
        [FromQuery] TaskStatus? status = null,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 50)
    {
        var query = _context.TaskExecutionLogs.AsQueryable();

        if (!string.IsNullOrEmpty(taskName))
        {
            query = query.Where(l => l.TaskName.Contains(taskName));
        }

        if (startDate.HasValue)
        {
            query = query.Where(l => l.ExecutionTime >= startDate.Value);
        }

        if (endDate.HasValue)
        {
            query = query.Where(l => l.ExecutionTime <= endDate.Value);
        }

        if (status.HasValue)
        {
            query = query.Where(l => l.Status == status.Value);
        }

        var total = await query.CountAsync();

        var logs = await query
            .OrderByDescending(l => l.ExecutionTime)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return Ok(new
        {
            data = logs,
            pagination = new
            {
                page,
                pageSize,
                total,
                totalPages = (int)Math.Ceiling(total / (double)pageSize)
            }
        });
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TaskExecutionLog>> GetLog(int id)
    {
        var log = await _context.TaskExecutionLogs.FindAsync(id);

        if (log == null)
        {
            return NotFound();
        }

        return log;
    }

    [HttpGet("stats")]
    public async Task<ActionResult<object>> GetStats([FromQuery] int days = 7)
    {
        var startDate = DateTime.UtcNow.AddDays(-days);

        var logs = await _context.TaskExecutionLogs
            .Where(l => l.ExecutionTime >= startDate)
            .ToListAsync();

        var stats = new
        {
            totalExecutions = logs.Count,
            successCount = logs.Count(l => l.Status == TaskStatus.Success),
            failedCount = logs.Count(l => l.Status == TaskStatus.Failed),
            warningCount = logs.Count(l => l.Status == TaskStatus.Warning),
            averageDurationMs = logs.Any() ? logs.Average(l => l.DurationMs) : 0,
            byTask = logs.GroupBy(l => l.TaskName)
                .Select(g => new
                {
                    taskName = g.Key,
                    count = g.Count(),
                    successRate = g.Count(l => l.Status == TaskStatus.Success) * 100.0 / g.Count()
                })
                .ToList(),
            recentExecutions = logs.OrderByDescending(l => l.ExecutionTime).Take(10).ToList()
        };

        return Ok(stats);
    }

    [HttpDelete("cleanup")]
    public async Task<IActionResult> CleanupOldLogs([FromQuery] int daysToKeep = 30)
    {
        var cutoffDate = DateTime.UtcNow.AddDays(-daysToKeep);

        var oldLogs = await _context.TaskExecutionLogs
            .Where(l => l.ExecutionTime < cutoffDate)
            .ToListAsync();

        _context.TaskExecutionLogs.RemoveRange(oldLogs);
        await _context.SaveChangesAsync();

        return Ok(new { deletedCount = oldLogs.Count });
    }
}

