using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using AutomationSystem.Infrastructure.Data;
using AutomationSystem.Core.Models;
using Hangfire;
using AutomationSystem.Core.Jobs;
using AutomationSystem.Core.Services;
using System.Text.Json;

namespace AutomationSystem.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class TasksController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<TasksController> _logger;

    public TasksController(ApplicationDbContext context, ILogger<TasksController> logger)
    {
        _context = context;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ScheduledTask>>> GetTasks()
    {
        return await _context.ScheduledTasks.ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ScheduledTask>> GetTask(int id)
    {
        var task = await _context.ScheduledTasks.FindAsync(id);

        if (task == null)
        {
            return NotFound();
        }

        return task;
    }

    [HttpPost]
    public async Task<ActionResult<ScheduledTask>> CreateTask(ScheduledTask task)
    {
        // Convertir le ScheduleJson en CronExpression si présent
        if (!string.IsNullOrEmpty(task.ScheduleJson))
        {
            try
            {
                var schedule = JsonSerializer.Deserialize<TaskSchedule>(task.ScheduleJson);
                if (schedule != null)
                {
                    task.CronExpression = ScheduleService.ToCronExpression(schedule);
                }
            }
            catch (JsonException ex)
            {
                _logger.LogError(ex, "Error parsing schedule JSON");
                return BadRequest("Invalid schedule format");
            }
        }
        
        task.CreatedAt = DateTime.UtcNow;
        _context.ScheduledTasks.Add(task);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetTask), new { id = task.Id }, task);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTask(int id, ScheduledTask task)
    {
        if (id != task.Id)
        {
            return BadRequest();
        }

        // Convertir le ScheduleJson en CronExpression si présent
        if (!string.IsNullOrEmpty(task.ScheduleJson))
        {
            try
            {
                var schedule = JsonSerializer.Deserialize<TaskSchedule>(task.ScheduleJson);
                if (schedule != null)
                {
                    task.CronExpression = ScheduleService.ToCronExpression(schedule);
                }
            }
            catch (JsonException ex)
            {
                _logger.LogError(ex, "Error parsing schedule JSON");
                return BadRequest("Invalid schedule format");
            }
        }

        task.UpdatedAt = DateTime.UtcNow;
        _context.Entry(task).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await _context.ScheduledTasks.AnyAsync(e => e.Id == id))
            {
                return NotFound();
            }
            throw;
        }

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTask(int id)
    {
        var task = await _context.ScheduledTasks.FindAsync(id);
        if (task == null)
        {
            return NotFound();
        }

        _context.ScheduledTasks.Remove(task);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpPost("{id}/execute")]
    public async Task<IActionResult> ExecuteTask(int id)
    {
        var task = await _context.ScheduledTasks.FindAsync(id);
        if (task == null)
        {
            return NotFound();
        }

        // Déclencher manuellement l'exécution de la tâche
        var jobId = task.TaskType switch
        {
            "Routenverfuegbarkeit" => BackgroundJob.Enqueue<RoutenverfuegbarkeitJob>(job => job.ExecuteAsync()),
            "StagingPlan" => BackgroundJob.Enqueue<StagingPlanJob>(job => job.ExecuteAsync()),
            "DNRUnits" => BackgroundJob.Enqueue<DNRUnitsJob>(job => job.ExecuteAsync()),
            _ => null
        };

        if (jobId == null)
        {
            return BadRequest("Type de tâche non reconnu");
        }

        _logger.LogInformation("Manual execution triggered for task {TaskId}, Job ID: {JobId}", id, jobId);

        return Ok(new { jobId });
    }
}

