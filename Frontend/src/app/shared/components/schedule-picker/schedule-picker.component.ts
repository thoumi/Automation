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
        <div class="space-y-4">
          <!-- Sélection du jour -->
          <div>
            <label class="block text-sm font-medium text-gray-700 mb-3">
              📅 Quel jour de la semaine ?
            </label>
            <div class="grid grid-cols-2 gap-2">
              @for (day of daysOfWeek; track day.value) {
                <button
                  type="button"
                  [class]="getDayButtonClass(day.value)"
                  (click)="selectDay(day.value)">
                  {{ day.label }}
                </button>
              }
            </div>
          </div>
          
          <!-- Sélection de l'heure -->
          <div>
            <label class="block text-sm font-medium text-gray-700 mb-2">
              🕐 À quelle heure ?
            </label>
            <div class="flex items-center space-x-2">
              <input 
                type="time" 
                [(ngModel)]="schedule.timeOfDay"
                (ngModelChange)="onScheduleChange()"
                class="input flex-1">
              <div class="flex space-x-1">
                <button 
                  type="button"
                  (click)="setQuickTime('09:00')"
                  class="px-2 py-1 text-xs bg-gray-100 hover:bg-gray-200 rounded">
                  9h
                </button>
                <button 
                  type="button"
                  (click)="setQuickTime('12:00')"
                  class="px-2 py-1 text-xs bg-gray-100 hover:bg-gray-200 rounded">
                  12h
                </button>
                <button 
                  type="button"
                  (click)="setQuickTime('18:00')"
                  class="px-2 py-1 text-xs bg-gray-100 hover:bg-gray-200 rounded">
                  18h
                </button>
              </div>
            </div>
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
      <div class="mt-4 p-4 bg-gradient-to-r from-blue-50 to-indigo-50 rounded-lg border border-blue-200">
        <div class="flex items-center space-x-2 mb-2">
          <span class="text-blue-600">📋</span>
          <span class="text-sm font-medium text-blue-900">Résumé de la planification</span>
        </div>
        <p class="text-sm text-blue-800">
          {{ getDescription() }}
        </p>
        @if (schedule.frequency === ScheduleFrequency.Weekly) {
          <div class="mt-2 text-xs text-blue-600">
            💡 La tâche s'exécutera automatiquement chaque semaine
          </div>
        }
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

  // Méthodes pour la sélection des jours
  selectDay(dayValue: number): void {
    this.schedule.dayOfWeek = dayValue;
    this.onScheduleChange();
  }

  getDayButtonClass(dayValue: number): string {
    const isSelected = this.schedule.dayOfWeek === dayValue;
    return `px-3 py-2 text-sm font-medium rounded-lg border transition-colors ${
      isSelected 
        ? 'bg-blue-500 text-white border-blue-500' 
        : 'bg-white text-gray-700 border-gray-300 hover:bg-gray-50'
    }`;
  }

  // Méthodes pour les heures rapides
  setQuickTime(time: string): void {
    this.schedule.timeOfDay = time;
    this.onScheduleChange();
  }
}

