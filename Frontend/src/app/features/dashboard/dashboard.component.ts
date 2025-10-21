import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { LogService } from '../../core/services/log.service';
import { TaskService } from '../../core/services/task.service';
import { TaskStats, ScheduledTask, TaskStatus } from '../../core/models/task.model';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [CommonModule],
  template: `
    <div class="space-y-6">
      <div class="flex justify-between items-center">
        <h1 class="text-3xl font-bold text-gray-900">Dashboard</h1>
        <div class="text-sm text-gray-500">
          Dernière mise à jour: {{ currentTime | date:'short' }}
        </div>
      </div>

      <!-- Statistics Cards -->
      @if (stats) {
        <div class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-6">
          <div class="card">
            <div class="flex items-center">
              <div class="flex-shrink-0 bg-green-100 rounded-md p-3">
                <svg class="h-6 w-6 text-green-600" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                  <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M9 12l2 2 4-4m6 2a9 9 0 11-18 0 9 9 0 0118 0z"></path>
                </svg>
              </div>
              <div class="ml-5">
                <p class="text-sm font-medium text-gray-500">Succès</p>
                <p class="text-2xl font-semibold text-gray-900">{{ stats.successCount }}</p>
              </div>
            </div>
          </div>

          <div class="card">
            <div class="flex items-center">
              <div class="flex-shrink-0 bg-red-100 rounded-md p-3">
                <svg class="h-6 w-6 text-red-600" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                  <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M6 18L18 6M6 6l12 12"></path>
                </svg>
              </div>
              <div class="ml-5">
                <p class="text-sm font-medium text-gray-500">Échecs</p>
                <p class="text-2xl font-semibold text-gray-900">{{ stats.failedCount }}</p>
              </div>
            </div>
          </div>

          <div class="card">
            <div class="flex items-center">
              <div class="flex-shrink-0 bg-yellow-100 rounded-md p-3">
                <svg class="h-6 w-6 text-yellow-600" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                  <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 9v2m0 4h.01m-6.938 4h13.856c1.54 0 2.502-1.667 1.732-3L13.732 4c-.77-1.333-2.694-1.333-3.464 0L3.34 16c-.77 1.333.192 3 1.732 3z"></path>
                </svg>
              </div>
              <div class="ml-5">
                <p class="text-sm font-medium text-gray-500">Avertissements</p>
                <p class="text-2xl font-semibold text-gray-900">{{ stats.warningCount }}</p>
              </div>
            </div>
          </div>

          <div class="card">
            <div class="flex items-center">
              <div class="flex-shrink-0 bg-blue-100 rounded-md p-3">
                <svg class="h-6 w-6 text-blue-600" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                  <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 8v4l3 3m6-3a9 9 0 11-18 0 9 9 0 0118 0z"></path>
                </svg>
              </div>
              <div class="ml-5">
                <p class="text-sm font-medium text-gray-500">Durée moyenne</p>
                <p class="text-2xl font-semibold text-gray-900">{{ stats.averageDurationMs / 1000 | number:'1.2-2' }}s</p>
              </div>
            </div>
          </div>
        </div>
      }

      <div class="grid grid-cols-1 lg:grid-cols-2 gap-6">
        <!-- Scheduled Tasks -->
        <div class="card">
          <h2 class="text-lg font-semibold text-gray-900 mb-4">Tâches planifiées</h2>
          <div class="space-y-3">
            @for (task of tasks; track task.id) {
              <div class="flex items-center justify-between p-3 bg-gray-50 rounded-lg">
                <div class="flex-1">
                  <p class="font-medium text-gray-900">{{ task.name }}</p>
                  <p class="text-sm text-gray-500">{{ task.description }}</p>
                  @if (task.nextExecutionTime) {
                    <p class="text-xs text-gray-400 mt-1">
                      Prochaine exécution: {{ task.nextExecutionTime | date:'short' }}
                    </p>
                  }
                </div>
                <div class="ml-4">
                  @if (task.isEnabled) {
                    <span class="badge badge-success">Actif</span>
                  } @else {
                    <span class="badge">Inactif</span>
                  }
                </div>
              </div>
            } @empty {
              <p class="text-gray-500 text-center py-4">Aucune tâche planifiée</p>
            }
          </div>
        </div>

        <!-- Recent Executions -->
        <div class="card">
          <h2 class="text-lg font-semibold text-gray-900 mb-4">Exécutions récentes</h2>
          <div class="space-y-3">
            @if (stats) {
              @if (stats.recentExecutions && stats.recentExecutions.length > 0) {
                @for (log of stats.recentExecutions; track log.id) {
                  <div class="flex items-center justify-between p-3 bg-gray-50 rounded-lg">
                    <div class="flex-1">
                      <p class="font-medium text-gray-900">{{ log.taskName }}</p>
                      <p class="text-xs text-gray-400">{{ log.executionTime | date:'short' }}</p>
                    </div>
                    <div class="ml-4">
                      @switch (log.status) {
                        @case (0) {
                          <span class="badge badge-success">Succès</span>
                        }
                        @case (1) {
                          <span class="badge badge-error">Échec</span>
                        }
                        @case (2) {
                          <span class="badge badge-warning">Avertissement</span>
                        }
                        @case (3) {
                          <span class="badge badge-info">En cours</span>
                        }
                      }
                    </div>
                  </div>
                }
              } @else {
                <p class="text-gray-500 text-center py-4">Aucune exécution récente</p>
              }
            } @else {
              <p class="text-gray-500 text-center py-4">Aucune exécution récente</p>
            }
          </div>
        </div>
      </div>

      <!-- Task Performance -->
      @if (stats) {
        @if (stats.byTask && stats.byTask.length > 0) {
          <div class="card">
            <h2 class="text-lg font-semibold text-gray-900 mb-4">Performance par tâche</h2>
            <div class="overflow-x-auto">
              <table class="min-w-full divide-y divide-gray-200">
                <thead>
                  <tr>
                    <th class="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Tâche</th>
                    <th class="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Exécutions</th>
                    <th class="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Taux de succès</th>
                    <th class="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Statut</th>
                  </tr>
                </thead>
                <tbody class="bg-white divide-y divide-gray-200">
                  @for (taskStat of stats.byTask; track taskStat.taskName) {
                    <tr>
                      <td class="px-6 py-4 whitespace-nowrap text-sm font-medium text-gray-900">{{ taskStat.taskName }}</td>
                      <td class="px-6 py-4 whitespace-nowrap text-sm text-gray-500">{{ taskStat.count }}</td>
                      <td class="px-6 py-4 whitespace-nowrap text-sm text-gray-500">{{ taskStat.successRate | number:'1.2-2' }}%</td>
                      <td class="px-6 py-4 whitespace-nowrap">
                        <div class="w-full bg-gray-200 rounded-full h-2">
                          <div 
                            class="h-2 rounded-full transition-all"
                            [class.bg-green-600]="taskStat.successRate >= 90"
                            [class.bg-yellow-600]="taskStat.successRate >= 70 && taskStat.successRate < 90"
                            [class.bg-red-600]="taskStat.successRate < 70"
                            [style.width.%]="taskStat.successRate">
                          </div>
                        </div>
                      </td>
                    </tr>
                  }
                </tbody>
              </table>
            </div>
          </div>
        }
      }
    </div>
  `
})
export class DashboardComponent implements OnInit {
  stats: TaskStats | null = null;
  tasks: ScheduledTask[] = [];
  currentTime = new Date();

  constructor(
    private logService: LogService,
    private taskService: TaskService
  ) {}

  ngOnInit(): void {
    this.loadStats();
    this.loadTasks();
    
    // Actualiser toutes les 30 secondes
    setInterval(() => {
      this.currentTime = new Date();
      this.loadStats();
    }, 30000);
  }

  loadStats(): void {
    this.logService.getStats(7).subscribe({
      next: (stats) => this.stats = stats,
      error: (err) => console.error('Error loading stats', err)
    });
  }

  loadTasks(): void {
    this.taskService.getTasks().subscribe({
      next: (tasks) => this.tasks = tasks,
      error: (err) => console.error('Error loading tasks', err)
    });
  }
}

