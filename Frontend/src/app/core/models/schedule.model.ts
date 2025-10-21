export interface TaskSchedule {
  frequency: ScheduleFrequency;
  interval: number;
  timeOfDay?: string; // Format HH:mm
  dayOfWeek?: number; // 0-6 (dimanche-samedi)
  dayOfMonth?: number; // 1-31
}

export enum ScheduleFrequency {
  EveryMinute = 0,
  Hourly = 1,
  Daily = 2,
  Weekly = 3,
  Monthly = 4,
  Custom = 99
}

export const ScheduleFrequencyLabels: Record<ScheduleFrequency, string> = {
  [ScheduleFrequency.EveryMinute]: 'Toutes les X minutes',
  [ScheduleFrequency.Hourly]: 'Toutes les X heures',
  [ScheduleFrequency.Daily]: 'Quotidien',
  [ScheduleFrequency.Weekly]: 'Hebdomadaire',
  [ScheduleFrequency.Monthly]: 'Mensuel',
  [ScheduleFrequency.Custom]: 'Personnalisé'
};

export const DaysOfWeek = [
  { value: 1, label: 'Lundi' },
  { value: 2, label: 'Mardi' },
  { value: 3, label: 'Mercredi' },
  { value: 4, label: 'Jeudi' },
  { value: 5, label: 'Vendredi' },
  { value: 6, label: 'Samedi' },
  { value: 0, label: 'Dimanche' }
];

