import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { TaskService } from '../../core/services/task.service';
import { ScheduledTask } from '../../core/models/task.model';
import { TaskSchedule, ScheduleFrequency } from '../../core/models/schedule.model';
import { SchedulePickerComponent } from '../../shared/components/schedule-picker/schedule-picker.component';

@Component({
  selector: 'app-tasks',
  standalone: true,
  imports: [CommonModule, FormsModule, SchedulePickerComponent],
  template: `
    <div class="space-y-6">
      <div class="flex justify-between items-center">
        <h1 class="text-3xl font-bold text-gray-900">Tâches planifiées</h1>
        <button class="btn btn-primary" (click)="showAddForm = true">
          <svg class="w-5 h-5 mr-2 inline-block" fill="none" stroke="currentColor" viewBox="0 0 24 24">
            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 4v16m8-8H4"></path>
          </svg>
          Nouvelle tâche
        </button>
      </div>

      <!-- Add Task Form -->
      @if (showAddForm) {
        <div class="card">
          <h2 class="text-xl font-semibold mb-4">Nouvelle tâche</h2>
          <form (ngSubmit)="createTask()" class="space-y-4">
            <div class="grid grid-cols-2 gap-4">
              <div>
                <label class="block text-sm font-medium text-gray-700 mb-2">Nom</label>
                <input type="text" [(ngModel)]="newTask.name" name="name" required class="input">
              </div>
              <div>
                <label class="block text-sm font-medium text-gray-700 mb-2">Type</label>
                <select [(ngModel)]="newTask.taskType" name="taskType" required class="input">
                  <option value="Routenverfuegbarkeit">Routenverfügbarkeit</option>
                  <option value="StagingPlan">Staging Plan</option>
                  <option value="DNRUnits">DNR Units</option>
                </select>
              </div>
            </div>
            <div>
              <label class="block text-sm font-medium text-gray-700 mb-2">Description</label>
              <textarea [(ngModel)]="newTask.description" name="description" rows="2" class="input"></textarea>
            </div>
            <div>
              <label class="block text-sm font-medium text-gray-700 mb-2">Planification</label>
              <app-schedule-picker 
                [schedule]="newTaskSchedule" 
                (scheduleChange)="onScheduleChange($event)">
              </app-schedule-picker>
            </div>
            <div class="flex items-center">
              <input type="checkbox" [(ngModel)]="newTask.isEnabled" name="isEnabled" 
                     id="isEnabled" class="rounded text-primary-600">
              <label for="isEnabled" class="ml-2 text-sm text-gray-700">Activée</label>
            </div>
            <div class="flex justify-end space-x-3">
              <button type="button" (click)="cancelAdd()" class="btn btn-secondary">Annuler</button>
              <button type="submit" class="btn btn-primary">Créer</button>
            </div>
          </form>
        </div>
      }

      <!-- Tasks List -->
      <div class="card">
        <div class="overflow-x-auto">
          <table class="min-w-full divide-y divide-gray-200">
            <thead>
              <tr>
                <th class="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase">Nom</th>
                <th class="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase">Type</th>
                <th class="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase">Cron</th>
                <th class="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase">Prochaine exécution</th>
                <th class="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase">Statut</th>
                <th class="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase">Actions</th>
              </tr>
            </thead>
            <tbody class="bg-white divide-y divide-gray-200">
              @for (task of tasks; track task.id) {
                <tr>
                  <td class="px-6 py-4 whitespace-nowrap">
                    <div class="text-sm font-medium text-gray-900">{{ task.name }}</div>
                    <div class="text-sm text-gray-500">{{ task.description }}</div>
                  </td>
                  <td class="px-6 py-4 whitespace-nowrap text-sm text-gray-500">
                    {{ task.taskType }}
                  </td>
                  <td class="px-6 py-4 whitespace-nowrap text-sm text-gray-500 font-mono">
                    {{ task.cronExpression }}
                  </td>
                  <td class="px-6 py-4 whitespace-nowrap text-sm text-gray-500">
                    {{ task.nextExecutionTime ? (task.nextExecutionTime | date:'short') : '-' }}
                  </td>
                  <td class="px-6 py-4 whitespace-nowrap">
                    @if (task.isEnabled) {
                      <span class="badge badge-success">Activée</span>
                    } @else {
                      <span class="badge">Désactivée</span>
                    }
                  </td>
                  <td class="px-6 py-4 whitespace-nowrap text-sm font-medium space-x-2">
                    <button (click)="executeTask(task.id)" class="text-primary-600 hover:text-primary-900">
                      Exécuter
                    </button>
                    <button (click)="deleteTask(task.id)" class="text-red-600 hover:text-red-900">
                      Supprimer
                    </button>
                  </td>
                </tr>
              } @empty {
                <tr>
                  <td colspan="6" class="px-6 py-4 text-center text-gray-500">
                    Aucune tâche configurée
                  </td>
                </tr>
              }
            </tbody>
          </table>
        </div>
      </div>
    </div>
  `
})
export class TasksComponent implements OnInit {
  tasks: ScheduledTask[] = [];
  showAddForm = false;
  newTask: Partial<ScheduledTask> = {
    name: '',
    description: '',
    cronExpression: '',
    taskType: 'Routenverfuegbarkeit',
    isEnabled: true
  };
  
  newTaskSchedule: TaskSchedule = {
    frequency: ScheduleFrequency.Daily,
    interval: 1,
    timeOfDay: '09:00'
  };

  constructor(private taskService: TaskService) {}

  ngOnInit(): void {
    this.loadTasks();
  }

  loadTasks(): void {
    this.taskService.getTasks().subscribe({
      next: (tasks) => this.tasks = tasks,
      error: (err) => console.error('Error loading tasks', err)
    });
  }

  onScheduleChange(schedule: TaskSchedule): void {
    this.newTaskSchedule = schedule;
    // Le backend générera automatiquement l'expression CRON
    // On stocke juste la planification simplifiée en JSON
    this.newTask.cronExpression = 'AUTO'; // Marqueur pour le backend
  }

  createTask(): void {
    // Ajouter la planification simplifiée au payload
    const taskWithSchedule = {
      ...this.newTask,
      scheduleJson: JSON.stringify(this.newTaskSchedule)
    };
    
    this.taskService.createTask(taskWithSchedule).subscribe({
      next: () => {
        this.loadTasks();
        this.cancelAdd();
      },
      error: (err) => console.error('Error creating task', err)
    });
  }

  executeTask(id: number): void {
    if (confirm('Êtes-vous sûr de vouloir exécuter cette tâche maintenant ?')) {
      this.taskService.executeTask(id).subscribe({
        next: (result) => alert(`Tâche lancée. Job ID: ${result.jobId}`),
        error: (err) => console.error('Error executing task', err)
      });
    }
  }

  deleteTask(id: number): void {
    if (confirm('Êtes-vous sûr de vouloir supprimer cette tâche ?')) {
      this.taskService.deleteTask(id).subscribe({
        next: () => this.loadTasks(),
        error: (err) => console.error('Error deleting task', err)
      });
    }
  }

  cancelAdd(): void {
    this.showAddForm = false;
    this.newTask = {
      name: '',
      description: '',
      cronExpression: '',
      taskType: 'Routenverfuegbarkeit',
      isEnabled: true
    };
    this.newTaskSchedule = {
      frequency: ScheduleFrequency.Daily,
      interval: 1,
      timeOfDay: '09:00'
    };
  }
}

