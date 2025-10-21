import { Component, Input, Output, EventEmitter, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { TaskSchedule, ScheduleFrequency, ScheduleFrequencyLabels, DaysOfWeek } from '../../../core/models/schedule.model';

@Component({
  selector: 'app-schedule-picker',
  standalone: true,
  imports: [CommonModule, FormsModule],
  template: `
    <div class="space-y-4">
      <!-- Sélection de la fréquence -->
      <div>
        <label class="block text-sm font-medium text-gray-700 mb-2">
          Fréquence
        </label>
        <select 
          [(ngModel)]="schedule.frequency"
          (ngModelChange)="onFrequencyChange()"
          class="input">
          @for (freq of frequencies; track freq.value) {
            <option [value]="freq.value">{{ freq.label }}</option>
          }
        </select>
      </div>

      <!-- Toutes les X minutes/heures -->
      @if (schedule.frequency === ScheduleFrequency.EveryMinute || schedule.frequency === ScheduleFrequency.Hourly) {
        <div>
          <label class="block text-sm font-medium text-gray-700 mb-2">
            {{ schedule.frequency === ScheduleFrequency.EveryMinute ? 'Toutes les combien de minutes ?' : 'Toutes les combien d\'heures ?' }}
          </label>
          <input 
            type="number" 
            [(ngModel)]="schedule.interval"
            (ngModelChange)="onScheduleChange()"
            min="1"
            [max]="schedule.frequency === ScheduleFrequency.EveryMinute ? 60 : 24"
            class="input">
        </div>
      }

      <!-- Quotidien - Heure -->
      @if (schedule.frequency === ScheduleFrequency.Daily) {
        <div>
          <label class="block text-sm font-medium text-gray-700 mb-2">
            À quelle heure ?
          </label>
          <input 
            type="time" 
            [(ngModel)]="schedule.timeOfDay"
            (ngModelChange)="onScheduleChange()"
            class="input">
        </div>
      }

      <!-- Hebdomadaire - Jour + Heure -->
      @if (schedule.frequency === ScheduleFrequency.Weekly) {
        <div class="grid grid-cols-2 gap-4">
          <div>
            <label class="block text-sm font-medium text-gray-700 mb-2">
              Quel jour ?
            </label>
            <select 
              [(ngModel)]="schedule.dayOfWeek"
              (ngModelChange)="onScheduleChange()"
              class="input">
              @for (day of daysOfWeek; track day.value) {
                <option [value]="day.value">{{ day.label }}</option>
              }
            </select>
          </div>
          <div>
            <label class="block text-sm font-medium text-gray-700 mb-2">
              À quelle heure ?
            </label>
            <input 
              type="time" 
              [(ngModel)]="schedule.timeOfDay"
              (ngModelChange)="onScheduleChange()"
              class="input">
          </div>
        </div>
      }

      <!-- Mensuel - Jour du mois + Heure -->
      @if (schedule.frequency === ScheduleFrequency.Monthly) {
        <div class="grid grid-cols-2 gap-4">
          <div>
            <label class="block text-sm font-medium text-gray-700 mb-2">
              Quel jour du mois ?
            </label>
            <input 
              type="number" 
              [(ngModel)]="schedule.dayOfMonth"
              (ngModelChange)="onScheduleChange()"
              min="1"
              max="31"
              class="input">
          </div>
          <div>
            <label class="block text-sm font-medium text-gray-700 mb-2">
              À quelle heure ?
            </label>
            <input 
              type="time" 
              [(ngModel)]="schedule.timeOfDay"
              (ngModelChange)="onScheduleChange()"
              class="input">
          </div>
        </div>
      }

      <!-- Aperçu de la planification -->
      <div class="mt-4 p-4 bg-blue-50 rounded-lg">
        <p class="text-sm text-blue-900">
          <strong>Résumé :</strong> {{ getDescription() }}
        </p>
      </div>
    </div>
  `
})
export class SchedulePickerComponent implements OnInit {
  @Input() schedule: TaskSchedule = {
    frequency: ScheduleFrequency.Daily,
    interval: 1,
    timeOfDay: '09:00'
  };
  
  @Output() scheduleChange = new EventEmitter<TaskSchedule>();

  ScheduleFrequency = ScheduleFrequency;
  frequencies = Object.entries(ScheduleFrequencyLabels)
    .filter(([key]) => key !== ScheduleFrequency.Custom.toString())
    .map(([value, label]) => ({ value: parseInt(value), label }));
  daysOfWeek = DaysOfWeek;

  ngOnInit(): void {
    this.initializeDefaults();
  }

  onFrequencyChange(): void {
    this.initializeDefaults();
    this.onScheduleChange();
  }

  onScheduleChange(): void {
    this.scheduleChange.emit(this.schedule);
  }

  private initializeDefaults(): void {
    switch (this.schedule.frequency) {
      case ScheduleFrequency.EveryMinute:
        this.schedule.interval = this.schedule.interval || 5;
        break;
      case ScheduleFrequency.Hourly:
        this.schedule.interval = this.schedule.interval || 1;
        break;
      case ScheduleFrequency.Daily:
        this.schedule.timeOfDay = this.schedule.timeOfDay || '09:00';
        break;
      case ScheduleFrequency.Weekly:
        this.schedule.dayOfWeek = this.schedule.dayOfWeek ?? 1; // Lundi
        this.schedule.timeOfDay = this.schedule.timeOfDay || '09:00';
        break;
      case ScheduleFrequency.Monthly:
        this.schedule.dayOfMonth = this.schedule.dayOfMonth || 1;
        this.schedule.timeOfDay = this.schedule.timeOfDay || '09:00';
        break;
    }
  }

  getDescription(): string {
    switch (this.schedule.frequency) {
      case ScheduleFrequency.EveryMinute:
        return this.schedule.interval === 1
          ? 'Toutes les minutes'
          : `Toutes les ${this.schedule.interval} minutes`;
      
      case ScheduleFrequency.Hourly:
        return this.schedule.interval === 1
          ? 'Toutes les heures'
          : `Toutes les ${this.schedule.interval} heures`;
      
      case ScheduleFrequency.Daily:
        return `Tous les jours à ${this.schedule.timeOfDay}`;
      
      case ScheduleFrequency.Weekly:
        const dayName = this.daysOfWeek.find(d => d.value === this.schedule.dayOfWeek)?.label || 'Lundi';
        return `Chaque ${dayName} à ${this.schedule.timeOfDay}`;
      
      case ScheduleFrequency.Monthly:
        return `Le ${this.schedule.dayOfMonth} de chaque mois à ${this.schedule.timeOfDay}`;
      
      default:
        return 'Non planifié';
    }
  }
}

