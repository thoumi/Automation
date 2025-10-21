namespace AutomationSystem.Core.Models;

public class TaskSchedule
{
    public ScheduleFrequency Frequency { get; set; }
    public int Interval { get; set; } = 1; // Tous les X heures/jours/semaines
    public TimeOnly? TimeOfDay { get; set; } // Heure spécifique (pour Daily, Weekly, Monthly)
    public DayOfWeek? DayOfWeek { get; set; } // Jour de la semaine (pour Weekly)
    public int? DayOfMonth { get; set; } // Jour du mois (pour Monthly)
}

public enum ScheduleFrequency
{
    EveryMinute = 0,    // Toutes les X minutes
    Hourly = 1,         // Toutes les X heures
    Daily = 2,          // Tous les jours à une heure précise
    Weekly = 3,         // Chaque semaine un jour spécifique
    Monthly = 4,        // Chaque mois un jour spécifique
    Custom = 99         // Expression CRON personnalisée
}

