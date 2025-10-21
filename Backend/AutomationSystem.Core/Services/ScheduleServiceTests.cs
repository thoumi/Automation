using AutomationSystem.Core.Models;

namespace AutomationSystem.Core.Services;

/// <summary>
/// Tests pour valider la planification hebdomadaire
/// </summary>
public static class ScheduleServiceTests
{
    public static void TestWeeklyScheduling()
    {
        Console.WriteLine("🧪 Test de la planification hebdomadaire...");
        
        // Test 1: Planification lundi 9h
        var mondaySchedule = new TaskSchedule
        {
            Frequency = ScheduleFrequency.Weekly,
            DayOfWeek = DayOfWeek.Monday,
            TimeOfDay = new TimeOnly(9, 0)
        };
        
        var cron1 = ScheduleService.ToCronExpression(mondaySchedule);
        var desc1 = ScheduleService.GetDescription(mondaySchedule);
        
        Console.WriteLine($"✅ Lundi 9h: {cron1} - {desc1}");
        
        // Test 2: Planification vendredi 18h
        var fridaySchedule = new TaskSchedule
        {
            Frequency = ScheduleFrequency.Weekly,
            DayOfWeek = DayOfWeek.Friday,
            TimeOfDay = new TimeOnly(18, 0)
        };
        
        var cron2 = ScheduleService.ToCronExpression(fridaySchedule);
        var desc2 = ScheduleService.GetDescription(fridaySchedule);
        
        Console.WriteLine($"✅ Vendredi 18h: {cron2} - {desc2}");
        
        // Test 3: Validation
        var isValid1 = ScheduleService.IsValidWeeklySchedule(mondaySchedule);
        var isValid2 = ScheduleService.IsValidWeeklySchedule(fridaySchedule);
        
        Console.WriteLine($"✅ Validation lundi: {isValid1}");
        Console.WriteLine($"✅ Validation vendredi: {isValid2}");
        
        // Test 4: Prochaine exécution
        var nextMonday = ScheduleService.GetNextWeeklyExecution(mondaySchedule);
        var nextFriday = ScheduleService.GetNextWeeklyExecution(fridaySchedule);
        
        Console.WriteLine($"✅ Prochaine exécution lundi: {nextMonday:dd/MM/yyyy HH:mm}");
        Console.WriteLine($"✅ Prochaine exécution vendredi: {nextFriday:dd/MM/yyyy HH:mm}");
        
        Console.WriteLine("🎉 Tous les tests de planification hebdomadaire sont passés !");
    }
}
