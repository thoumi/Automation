using AutomationSystem.Core.Models;
using System.Text;

namespace AutomationSystem.Core.Services;

public class ScheduleService
{
    /// <summary>
    /// Convertit un TaskSchedule en expression CRON
    /// </summary>
    public static string ToCronExpression(TaskSchedule schedule)
    {
        return schedule.Frequency switch
        {
            ScheduleFrequency.EveryMinute => $"*/{schedule.Interval} * * * *",
            
            ScheduleFrequency.Hourly => $"0 */{schedule.Interval} * * *",
            
            ScheduleFrequency.Daily => schedule.TimeOfDay.HasValue
                ? $"{schedule.TimeOfDay.Value.Minute} {schedule.TimeOfDay.Value.Hour} * * *"
                : "0 9 * * *", // Par défaut 9h du matin
            
            ScheduleFrequency.Weekly => schedule.DayOfWeek.HasValue && schedule.TimeOfDay.HasValue
                ? $"{schedule.TimeOfDay.Value.Minute} {schedule.TimeOfDay.Value.Hour} * * {(int)schedule.DayOfWeek.Value}"
                : "0 9 * * 1", // Par défaut lundi 9h
            
            ScheduleFrequency.Monthly => schedule.DayOfMonth.HasValue && schedule.TimeOfDay.HasValue
                ? $"{schedule.TimeOfDay.Value.Minute} {schedule.TimeOfDay.Value.Hour} {schedule.DayOfMonth.Value} * *"
                : "0 9 1 * *", // Par défaut 1er du mois à 9h
            
            _ => "0 9 * * *" // Par défaut quotidien à 9h
        };
    }

    /// <summary>
    /// Génère une description lisible de la planification
    /// </summary>
    public static string GetDescription(TaskSchedule schedule)
    {
        return schedule.Frequency switch
        {
            ScheduleFrequency.EveryMinute => schedule.Interval == 1
                ? "Toutes les minutes"
                : $"Toutes les {schedule.Interval} minutes",
            
            ScheduleFrequency.Hourly => schedule.Interval == 1
                ? "Toutes les heures"
                : $"Toutes les {schedule.Interval} heures",
            
            ScheduleFrequency.Daily => schedule.TimeOfDay.HasValue
                ? $"Tous les jours à {schedule.TimeOfDay.Value:HH\\:mm}"
                : "Tous les jours à 09:00",
            
            ScheduleFrequency.Weekly => GetWeeklyDescription(schedule),
            
            ScheduleFrequency.Monthly => GetMonthlyDescription(schedule),
            
            ScheduleFrequency.Custom => "Planification personnalisée",
            
            _ => "Non planifié"
        };
    }

    private static string GetWeeklyDescription(TaskSchedule schedule)
    {
        var time = schedule.TimeOfDay?.ToString("HH\\:mm") ?? "09:00";
        
        var dayNameFr = schedule.DayOfWeek switch
        {
            DayOfWeek.Monday => "lundi",
            DayOfWeek.Tuesday => "mardi",
            DayOfWeek.Wednesday => "mercredi",
            DayOfWeek.Thursday => "jeudi",
            DayOfWeek.Friday => "vendredi",
            DayOfWeek.Saturday => "samedi",
            DayOfWeek.Sunday => "dimanche",
            _ => "lundi"
        };
        
        return $"Chaque {dayNameFr} à {time}";
    }

    /// <summary>
    /// Valide une planification hebdomadaire
    /// </summary>
    public static bool IsValidWeeklySchedule(TaskSchedule schedule)
    {
        return schedule.Frequency == ScheduleFrequency.Weekly 
               && schedule.DayOfWeek.HasValue 
               && schedule.TimeOfDay.HasValue;
    }

    /// <summary>
    /// Obtient le prochain jour d'exécution pour une planification hebdomadaire
    /// </summary>
    public static DateTime GetNextWeeklyExecution(TaskSchedule schedule)
    {
        if (!IsValidWeeklySchedule(schedule))
            return DateTime.Now.AddDays(1);

        var now = DateTime.Now;
        var targetDay = (int)schedule.DayOfWeek!.Value;
        var targetTime = schedule.TimeOfDay!.Value;
        
        // Calculer le prochain jour de la semaine
        var daysUntilTarget = (targetDay - (int)now.DayOfWeek + 7) % 7;
        if (daysUntilTarget == 0 && now.TimeOfDay >= targetTime.ToTimeSpan())
        {
            daysUntilTarget = 7; // Prochaine semaine
        }
        
        var nextExecution = now.Date.AddDays(daysUntilTarget).Add(targetTime.ToTimeSpan());
        return nextExecution;
    }

    private static string GetMonthlyDescription(TaskSchedule schedule)
    {
        var day = schedule.DayOfMonth ?? 1;
        var time = schedule.TimeOfDay?.ToString("HH\\:mm") ?? "09:00";
        return $"Le {day} de chaque mois à {time}";
    }
}

